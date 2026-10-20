// SPDX-License-Identifier: MPL-2.0
using System;
using System.IO;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using TaskReminder;
internal static class PolicyTests
{
    private static string[] _paths;
    private static int Main(string[] args)
    {
        _paths = args;
        AppDomain.CurrentDomain.AssemblyResolve += Resolve;
        try { Run(); Console.WriteLine("PASS native calendar/week-wrap, read-only visible selection, header collapse, frozen final defaults, read-only config compatibility, Planner profile/frame scale, compiled no-editor surface"); return 0; }
        catch (Exception ex) { Console.Error.WriteLine(ex); return 1; }
    }
    private static Assembly Resolve(object sender, ResolveEventArgs args)
    {
        string name = new AssemblyName(args.Name).Name;
        if (AssemblyName.GetAssemblyName(_paths[2]).Name == name) return Assembly.LoadFrom(_paths[2]);
        for (int i = 0; i < 2; i++) { string path = Path.Combine(_paths[i], name + ".dll"); if (File.Exists(path)) return Assembly.LoadFrom(path); }
        return null;
    }
    [MethodImpl(MethodImplOptions.NoInlining)]
    private static void Run()
    {
        var save = (GameSave)FormatterServices.GetUninitializedObject(typeof(GameSave));
        string[] expected = { "npc_astrologer", "npc_bishop", "npc_actress", "npc_merchant", "npc_cultist", "npc_inquisitor" };
        for (int day = 0; day < 18; day++)
        {
            save.day = day;
            Require(ReminderPolicy.NpcForDay(day, 0) == expected[save.day_of_week], "Native calendar mismatch");
            Require(ReminderPolicy.NpcForDay(day, 1) == expected[(save.day_of_week + 1) % 6], "Tomorrow/week wrap mismatch");
        }
        var npc = new KnownNPC { npc_id = "npc_merchant" };
        var first = new KnownNPC.TaskState { id = "merchant_garden", state = KnownNPC.TaskState.State.Visible };
        var second = new KnownNPC.TaskState { id = "merchant_curse", state = KnownNPC.TaskState.State.Visible };
        npc.tasks.Add(first); npc.tasks.Add(new KnownNPC.TaskState { id = "done", state = KnownNPC.TaskState.State.Complete });
        npc.tasks.Add(new KnownNPC.TaskState { id = "unknown", state = KnownNPC.TaskState.State.Unknown });
        npc.tasks.Add(null); npc.tasks.Add(second);
        var result = ReminderPolicy.VisibleTasks(npc);
        Require(result.Count == 2 && ReferenceEquals(result[0], first) && ReferenceEquals(result[1], second), "Visible/order mismatch");
        Require(npc.tasks.Count == 5 && first.state == KnownNPC.TaskState.State.Visible, "Selection mutated native records");
        Require(ReminderPolicy.VisibleTasks(null).Count == 0, "Missing NPC mismatch");
        first.state = KnownNPC.TaskState.State.Complete;
        Require(ReminderPolicy.VisibleTasks(npc).Count == 1, "Completion not reflected");
        bool rejected = false; try { ReminderPolicy.NpcForDay(-1, 0); } catch (ArgumentOutOfRangeException) { rejected = true; }
        Require(rejected, "Invalid day must be rejected");
        Require(ReminderPolicy.CaptionRowHeight(false, false, 20, 1) == 0, "Hidden header must release its full height");
        Require(ReminderPolicy.CaptionRowHeight(false, true, 20, 1) == 5, "Divider-only header must ignore hidden text height");
        Require(ReminderPolicy.CaptionRowHeight(true, false, 20, 1) == 24, "Text-only header height mismatch");
        Require(ReminderPolicy.CaptionRowHeight(true, true, 20, 1) == 24, "Combined header height mismatch");
        string configPath = Path.Combine(Path.GetDirectoryName(_paths[2]), "final-defaults-check.cfg");
        File.WriteAllText(configPath, "");
        var config = new BepInEx.Configuration.ConfigFile(configPath, false);
        var options = new Options(config);
        var expectedSettings = new Dictionary<string, string>();
        foreach (string line in File.ReadAllLines(_paths[3]))
        {
            int equals = line.IndexOf('=');
            if (equals > 0) expectedSettings.Add(line.Substring(0, equals).Trim(), line.Substring(equals + 1).Trim());
        }
        int count = 0;
        foreach (var entry in config)
        {
            string expectedValue;
            Require(expectedSettings.TryGetValue(entry.Key.Key, out expectedValue), "Unpinned default " + entry.Key.Key);
            Require(string.Equals(Convert.ToString(entry.Value.BoxedValue, CultureInfo.InvariantCulture), expectedValue, StringComparison.OrdinalIgnoreCase), "Final default mismatch " + entry.Key.Key);
            Require(Equals(entry.Value.BoxedValue, entry.Value.DefaultValue), "Fresh value differs from default"); count++;
        }
        Require(count == expectedSettings.Count && count > 30, "Final fixture coverage mismatch");
        Require(File.ReadAllText(configPath) == "", "Loading fresh defaults wrote config");
        File.WriteAllText(configPath, "[Appearance]\nOffsetX = 12\nWidth = 154\nFontSize = 13\nHeadingSize = 14\nLineSpacing = -4\nEffectSize = 1\nFont = Items\nEffect = Shadow\n");
        string oldBytes = File.ReadAllText(configPath);
        var restored = new Options(new BepInEx.Configuration.ConfigFile(configPath, false));
        Require(restored.X.Value == 12 && restored.Width.Value == 154 && restored.FontSize.Value == 13 && restored.Font.Value == NativeFont.Items && restored.Effect.Value == TextEffect.Shadow, "Existing override changed");
        Require(restored.CaptionSize.Value == 8 && restored.EffectSize.Value == 1f, "Missing-key defaults or effect migration mismatch");
        Require(File.ReadAllText(configPath) == oldBytes, "Existing config load wrote disk");
        var style = new Appearance(options);
        Require(!style.Matched && style.Width == 130 && style.FontSize == 14 && style.CaptionSize == 8 && style.Font == NativeFont.Journal && style.Effect == TextEffect.Outline, "Final effective appearance changed");
        Require(!options.ShowNames.Value && options.QuestIconSize.Value == 9 && !options.QuestIconShadow.Value && options.ScalePercent.Value == 100 && options.ShadeOffsetY.Value == -3, "Final visibility/icons/scale changed");
        options.MatchPlanner.Value = true;
        Require(style.Width == 286 && style.FontSize == 24 && style.CaptionSize == 21 && style.Font == NativeFont.Items && style.Effect == TextEffect.Outline && !style.RasterProfile, "Preserved Planner config profile mismatch");
        Require(Math.Abs(style.Scale - .65f) < .0001f && Math.Abs(style.ShadeAlpha - .39f) < .0001f, "Planner scale/backing mismatch");
        options.MatchPlanner.Value = false;
        Require(style.Width == 130 && style.FontSize == 14, "Final profile restoration mismatch");
        foreach (float hudScale in new[] { .75f, 1f, 1.1f, 2f })
        {
            var converted = Appearance.InHudScale(new UnityEngine.Vector3(1f / hudScale, 0, 0), new UnityEngine.Vector3(0, 1f / hudScale, 0), .65f);
            Require(Math.Abs(converted.x * hudScale - .65f) < .0001f && Math.Abs(converted.y * hudScale - .65f) < .0001f, "HUD scale doubled in Planner mode");
        }
        foreach (float invalid in new[] { 0f, -1f, float.NaN, float.PositiveInfinity })
        {
            bool invalidRejected = false;
            try { Appearance.InHudScale(new UnityEngine.Vector3(invalid, 0, 0), new UnityEngine.Vector3(0, 1, 0), .65f); }
            catch (InvalidOperationException) { invalidRejected = true; }
            Require(invalidRejected, "Invalid coordinate scale accepted");
        }
        bool rotatedRejected = false;
        try { Appearance.InHudScale(new UnityEngine.Vector3(1, .1f, 0), new UnityEngine.Vector3(0, 1, 0), .65f); }
        catch (InvalidOperationException) { rotatedRejected = true; }
        Require(rotatedRejected, "Rotated coordinate basis accepted");
        Assembly candidate = typeof(Plugin).Assembly;
        foreach (string removed in new[] { "SettingsPanel", "SettingsKeyGuard", "SettingsDirectionGuard" })
            Require(candidate.GetType("TaskReminder." + removed) == null, "Editor/input guard still compiled: " + removed);
        Require(typeof(Plugin).GetMethod("OnGUI", BindingFlags.NonPublic | BindingFlags.Instance) == null, "Editor draw hook still compiled");
        foreach (string removed in new[] { "CanOpenPanel", "LogSettings", "Changed" })
            Require(typeof(Plugin).GetMethod(removed, BindingFlags.NonPublic | BindingFlags.Instance) == null, "Editor entry still compiled");
        foreach (string removed in new[] { "Save", "Capture", "Restore", "Reset" })
            Require(typeof(Options).GetMethod(removed, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static) == null, "Editor config mutation helper still compiled");
        Require(candidate.GetType("TaskReminder.TaskChangedPatch") != null, "Native task refresh patch lost");

    }
    private static void Require(bool pass, string message) { if (!pass) throw new Exception(message); }
}
