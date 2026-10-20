# Development recovery

Current candidate0.1.4 is a prerelease with the final0.1.3 appearance and no F2 editor. Work remains on codex/feature/native-reminder; no main promotion or stable Release. Start PRERELEASE.md for adopted values/gate and TEST_LOG.md for exact source/artifact/install state. Local machine paths and private evidence stay in ignored LOCAL_ENVIRONMENT.md.

## Build

Run `build.ps1 -GameRoot <local-game-root> -ExpectedSource <HEAD>` from a clean committed checkout for an immutable handoff; add `-CheckOnly` for a development check. SDK6.0.406 Roslyn compiles against private installed BepInEx/Harmony/Unity/game references. No package restore, host-asset distribution or hosted CI/reference provisioning is selected. A numbered handed DLL is never overwritten. The build identity records source/version/reference/artifact hashes.

The actual-assembly tests cover native weekday/week wrap, read-only Visible filtering/order/nulls/completion, caption collapse, all final defaults against tests/FinalAppearance.cfg, old-config load without writes, preserved optional Planner profile/coordinate conversion, and absence of editor/input-guard types, OnGUI/editor entries and config transaction helpers. They do not prove Unity rendering or all runtime paths.

## Final appearance

Options now defaults to every saved parameter from the user-adopted0.1.3 view. Existing BepInEx config values remain authoritative overrides; construction disables SaveOnConfigSet and no project save/reset/transaction code remains. Installation preserves the existing cfg. Fresh defaults and the final saved fixture are checked entry by entry. There is no runtime editing/reload UI; later visual changes require another requested build or deliberate external configuration.

Rendering source is unchanged from0.1.3: native text/fonts/icons, narrow measured per-line backing, visibility/layout rules and optional documented Planner style. Final selected view is manual Journal14/Outline/width130/names hidden; Planner matching is off. Exact values are in PRERELEASE.md and the fixture. No full-card rectangle is introduced.

## Editor removal

SettingsPanel and its LazyInput key/direction Harmony guards are deleted. Plugin has no editor OnGUI, F2/Esc polling, input capture, panel close lifecycle or config mutation helpers. It retains only its native KnownNPC.SetQuestState refresh postfix. It does not change timeScale or save data. Recovery source for the old editor is68b0abc55f26e173574100c8b9b396f411bd0430; restore only after a new user request and versioned handoff.

## Installed return and acceptance

Inspect game-process state before replacing the canonical TaskReminder.dll. Back up and hash-verify the previous payload outside plugins, preserve config/log snapshots, install exact numbered bytes and verify the installed digest. Never replace a loaded DLL or launch/terminate the game without authorization. CODEX_WORKFLOW.md owns this shared loop.

First0.1.4 return: accepted view unchanged, F2 opens no Task Reminder UI, ordinary controls work, menu/HUD return and reload retain the view. Read the accessible native log independently. Prior0.1.3 TR_READY/final settings plus user visual acceptance establish bounded evidence, not every DLC/language/long-list/save/day/cutscene path. Collect natural task completion/day change/save-switch observations when encountered; do not manufacture progression. Only explicit stable acceptance promotes exact bytes to main/tag/GitHub Releases.
