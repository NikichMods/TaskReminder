# SPDX-License-Identifier: MPL-2.0
param(
    [Parameter(Mandatory=$true)][string]$GameRoot,
    [string]$DotnetRoot = 'C:\Program Files\dotnet',
    [Parameter(Mandatory=$true)][string]$ExpectedSource,
    [switch]$CheckOnly
)
$ErrorActionPreference = 'Stop'
$repo = $PSScriptRoot
$source = (& git -C $repo rev-parse HEAD).Trim()
if ($source -ne $ExpectedSource) { throw 'Unexpected source commit' }
if (!$CheckOnly -and (& git -C $repo status --porcelain --untracked-files=normal)) { throw 'Handoff requires clean committed source' }
$managed = Join-Path $GameRoot 'Graveyard Keeper_Data/Managed'
$core = Join-Path $GameRoot 'BepInEx/core'
$compiler = Join-Path $DotnetRoot 'sdk/6.0.406/Roslyn/bincore/csc.dll'
$dotnet = Join-Path $DotnetRoot 'dotnet.exe'
$match = [regex]::Match((Get-Content -LiteralPath (Join-Path $repo 'src/Plugin.cs') -Raw), 'public const string Version = "([0-9]+\.[0-9]+\.[0-9]+)";')
if (!$match.Success) { throw 'Missing canonical version' }
$version = $match.Groups[1].Value
$out = Join-Path $repo ('artifacts/TaskReminder-' + $version)
New-Item -ItemType Directory -Path $out -Force | Out-Null
$dll = Join-Path $out $(if ($CheckOnly) {'compile-check.dll'} else {'TaskReminder-' + $version + '.dll'})
if (!$CheckOnly -and (Test-Path -LiteralPath $dll)) { throw 'Immutable numbered output already exists' }
$refs = @('mscorlib.dll','System.dll','System.Core.dll','Assembly-CSharp.dll','Assembly-CSharp-firstpass.dll','UnityEngine.dll','UnityEngine.CoreModule.dll','UnityEngine.TextRenderingModule.dll','UnityEngine.IMGUIModule.dll','UnityEngine.InputLegacyModule.dll') | ForEach-Object { Join-Path $managed $_ }
$refs += @('BepInEx.dll','0Harmony.dll') | ForEach-Object { Join-Path $core $_ }
$baseArgs = @('/nologo','/nostdlib+','/noconfig','/langversion:7.3','/optimize+','/deterministic+','/warnaserror+')
$baseArgs += $refs | ForEach-Object { if (!(Test-Path -LiteralPath $_)) { throw "Missing reference $_" }; '/reference:' + $_ }
$files = @(Get-ChildItem -LiteralPath (Join-Path $repo 'src') -Filter '*.cs' | Sort-Object Name | ForEach-Object FullName)
& $dotnet $compiler @baseArgs /target:library ('/out:'+$dll) @files
if ($LASTEXITCODE -ne 0) { throw 'Compile failed' }
if ([Reflection.AssemblyName]::GetAssemblyName($dll).Version.ToString() -ne ($version+'.0')) { throw 'Assembly version mismatch' }
if ([Diagnostics.FileVersionInfo]::GetVersionInfo($dll).FileVersion -ne ($version+'.0')) { throw 'File version mismatch' }
$test = Join-Path $out 'TaskReminder.Tests.exe'
& $dotnet $compiler @baseArgs /target:exe ('/out:'+$test) ('/reference:'+$dll) (Join-Path $repo 'tests/PolicyTests.cs')
if ($LASTEXITCODE -ne 0) { throw 'Test compile failed' }
& $test $managed $core $dll
if ($LASTEXITCODE -ne 0) { throw 'Policy tests failed' }
if ($CheckOnly) { Write-Output 'Development compile/version/actual-assembly policy checks passed; not a handoff artifact'; return }
$manifest = [ordered]@{version=$version;source=$source;compiler='SDK6.0.406 Roslyn';artifact=(Split-Path $dll -Leaf);sha256=(Get-FileHash -LiteralPath $dll -Algorithm SHA256).Hash.ToLowerInvariant();bytes=(Get-Item -LiteralPath $dll).Length;checks='Release compile, assembly/file version, actual-assembly policy tests';references=@($refs | ForEach-Object { [ordered]@{name=(Split-Path $_ -Leaf);sha256=(Get-FileHash -LiteralPath $_ -Algorithm SHA256).Hash.ToLowerInvariant()} })}
$manifest | ConvertTo-Json -Depth 6 | Set-Content -LiteralPath (Join-Path $out 'build-identity.json') -Encoding utf8
Write-Output "Built $dll source=$source SHA256=$($manifest.sha256)"
