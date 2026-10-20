// SPDX-License-Identifier: MPL-2.0
using System;
using System.Collections.Generic;

namespace TaskReminder
{
    internal static class ReminderPolicy
    {
        private static readonly string[] Npcs = { "npc_astrologer", "npc_bishop", "npc_actress", "npc_merchant", "npc_cultist", "npc_inquisitor" };
        internal static string NpcForDay(int day, int ahead)
        {
            if (day < 0 || ahead < 0 || ahead > 1) throw new ArgumentOutOfRangeException();
            return Npcs[((day % 6) + ahead) % 6];
        }
        internal static List<KnownNPC.TaskState> VisibleTasks(KnownNPC npc)
        {
            var result = new List<KnownNPC.TaskState>();
            if (npc == null || npc.tasks == null) return result;
            foreach (var task in npc.tasks)
                if (task != null && !string.IsNullOrEmpty(task.id) && task.state == KnownNPC.TaskState.State.Visible) result.Add(task);
            return result;
        }
        internal static int CaptionRowHeight(bool text, bool line, int textHeight, int lineHeight)
        { return text || line ? Math.Max(text ? textHeight : 0, line ? lineHeight : 0) + 4 : 0; }
    }
}
