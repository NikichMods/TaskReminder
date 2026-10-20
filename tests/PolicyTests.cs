// SPDX-License-Identifier: MPL-2.0
using System;
using System.IO;
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
        try { Run(); Console.WriteLine("PASS native calendar/week-wrap, read-only visible selection, header collapse, old-config migration/persistence, settings rollback/isolated defaults, Planner profile/frame scale, input signatures"); return 0; }
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
        string configPath = Path.Combine(Path.GetDirectoryName(_paths[2]), "migration-check.cfg");
        File.WriteAllText(configPath, "[Appearance]\nOffsetX = 12\nWidth = 154\nFontSize = 13\nHeadingSize = 14\nLineSpacing = -4\nEffectSize = 1\nFont = Items\nEffect = Shadow\n");
        var config = new BepInEx.Configuration.ConfigFile(configPath, false);
        var options = new Options(config);
        Require(options.X.Value == 12 && options.Width.Value == 154 && options.FontSize.Value == 13 && options.LineSpacing.Value == -4 && options.Font.Value == NativeFont.Items, "Existing config values changed");
        Require(options.EffectSize.Value == 1f && options.CaptionSize.Value == 16, "Effect/caption migration mismatch");
        options.ShowCaptions.Value = false; options.ShowDividers.Value = true; options.Save();
        var restored = new Options(new BepInEx.Configuration.ConfigFile(configPath, false));
        Require(!restored.ShowCaptions.Value && restored.ShowDividers.Value && restored.Width.Value == 154, "Independent header settings did not persist");
        string savedBytes = File.ReadAllText(configPath);
        var snapshot = options.Capture();
        foreach (var item in snapshot)
        {
            object value = item.Value;
            if (value is int) item.Key.BoxedValue = (int)value + 1;
            else if (value is float) item.Key.BoxedValue = (float)value - .1f;
            else if (value is bool) item.Key.BoxedValue = !(bool)value;
        }
        options.Font.Value = NativeFont.Journal; options.Effect.Value = TextEffect.Outline;
        options.Restore(snapshot);
        foreach (var item in snapshot) Require(Equals(item.Key.BoxedValue, item.Value), "Cancel failed for " + item.Key.Definition.Key);
        Require(File.ReadAllText(configPath) == savedBytes, "Preview/cancel wrote disk");
        options.CaptionSize.Value = 21; options.ShadeOffsetY.Value = -7; options.Save();
        var reopened = new Options(new BepInEx.Configuration.ConfigFile(configPath, false));
        Require(reopened.CaptionSize.Value == 21 && reopened.ShadeOffsetY.Value == -7, "Saved caption/offset lost");
        var baseline = reopened.Capture();
        foreach (var item in baseline)
        {
            if (!(item.Value is int) && !(item.Value is float)) continue;
            Options.Reset(item.Key);
            Require(Equals(item.Key.BoxedValue, item.Key.DefaultValue), "Slider reset mismatch");
            foreach (var other in baseline)
                if (!ReferenceEquals(other.Key, item.Key)) Require(Equals(other.Key.BoxedValue, other.Value), "Reset affected another entry");
            reopened.Restore(baseline);
        }
        Require((int)reopened.CaptionSize.DefaultValue == 16 && (int)reopened.ShadeOffsetY.DefaultValue == 0, "Unstable defaults");
        var secondSession = reopened.Capture(); reopened.Width.Value = 300; reopened.Restore(secondSession);
        Require(reopened.Width.Value == 154 && reopened.CaptionSize.Value == 21, "Reopened cancel baseline mismatch");
        var manual = reopened.Capture();
        var style = new Appearance(reopened);
        Require(!style.Matched && style.Width == 154 && style.FontSize == 13 && Math.Abs(style.Scale - 1f) < .0001f, "Manual appearance changed");
        reopened.MatchPlanner.Value = true;
        Require(style.Matched && style.Width == 286 && style.FontSize == 24 && style.CaptionSize == 21 && style.Font == NativeFont.Items, "Planner typography mismatch");
        Require(style.Effect == TextEffect.Outline && style.EffectSize == 1 && Math.Abs(style.EffectAlpha - .8f) < .0001f && !style.RasterProfile, "Planner effect pipeline mismatch");
        Require(Math.Abs(style.Scale - .65f) < .0001f && style.TomorrowAlpha == 1 && style.TextAlpha == 1 && style.SoftShade, "Planner scale/opacity mismatch");
        Require(Math.Abs(style.ShadeAlpha - .39f) < .0001f && style.Feather(24) == 12, "Narrow Planner shade mismatch");
        reopened.ScalePercent.Value = 150;
        Require(Math.Abs(style.Scale - .975f) < .0001f, "Whole block multiplier mismatch");
        reopened.ShowNames.Value = false; reopened.QuestIconSize.Value = 36; reopened.QuestIconShadow.Value = true;
        reopened.Save();
        var matchedReload = new Options(new BepInEx.Configuration.ConfigFile(configPath, false));
        Require(matchedReload.MatchPlanner.Value && !matchedReload.ShowNames.Value && matchedReload.QuestIconSize.Value == 36 && matchedReload.QuestIconShadow.Value && matchedReload.ScalePercent.Value == 150, "New settings did not persist");
        reopened.Restore(manual);
        foreach (var item in manual) Require(Equals(item.Key.BoxedValue, item.Value), "Style mode changed manual settings");
        Require(style.Width == 154 && style.FontSize == 13 && style.CaptionSize == 21, "Manual style not restored");
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
        foreach (string name in new[] { "GetKey", "GetKeyDown", "AnyClick", "AnyKeyDown" })
        {
            bool found = false;
            foreach (var method in typeof(LazyInput).GetMethods(BindingFlags.Public | BindingFlags.Static))
                if (method.Name == name && method.ReturnType == typeof(bool)) found = true;
            Require(found, "Missing native input guard family " + name);
        }
        foreach (string name in new[] { "GetDirection", "GetDirection2" })
        {
            bool found = false;
            foreach (var method in typeof(LazyInput).GetMethods(BindingFlags.Public | BindingFlags.Static))
                if (method.Name == name && method.ReturnType == typeof(UnityEngine.Vector2)) found = true;
            Require(found, "Missing native direction guard family " + name);
        }
    }
    private static void Require(bool pass, string message) { if (!pass) throw new Exception(message); }
}
