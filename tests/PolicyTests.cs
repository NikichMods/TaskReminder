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
        try { Run(); Console.WriteLine("PASS native calendar/week-wrap, read-only visible selection, header collapse, old-config migration/persistence, input signatures"); return 0; }
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
        Require(options.EffectSize.Value == 1f && options.CaptionSize.Value == 13, "Effect/caption migration mismatch");
        options.ShowCaptions.Value = false; options.ShowDividers.Value = true; options.Save();
        var restored = new Options(new BepInEx.Configuration.ConfigFile(configPath, false));
        Require(!restored.ShowCaptions.Value && restored.ShowDividers.Value && restored.Width.Value == 154, "Independent header settings did not persist");
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
