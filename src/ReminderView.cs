// SPDX-License-Identifier: MPL-2.0
using System;
using System.Collections.Generic;
using UnityEngine;

namespace TaskReminder
{
    internal sealed class ReminderView
    {
        private readonly Options _options;
        private readonly GameObject _root;
        private readonly HUD _hud;
        private readonly UIWidget _wheel;
        private readonly UILabel _journal, _items;
        private readonly List<UILabel> _labels = new List<UILabel>();
        private int _cursor;
        internal int Height { get; private set; }
        internal bool Alive { get { return _root != null && _hud != null && _wheel != null; } }
        internal HUD Hud { get { return _hud; } }
        internal ReminderView(Options options, GUIElements gui)
        {
            _options = options; _hud = gui.hud;
            Transform circle = _hud.sins_circle.transform.Find("test circle (1)");
            if (circle == null) throw new InvalidOperationException("Native calendar backing missing");
            _wheel = circle.GetComponent<UIWidget>();
            if (_wheel == null || _wheel.width != 120 || _wheel.height != 120) throw new InvalidOperationException("Native calendar backing contract changed");
            _journal = gui.npcs_list.prefab.quest_prefab.txt;
            _items = gui.craft.craft_item_prefab.label_name;
            if (_journal == null || _items == null) throw new InvalidOperationException("Native font templates unavailable");
            _root = new GameObject("TaskReminder.Text"); _root.layer = _hud.gameObject.layer;
            _root.SetActive(false); _root.transform.SetParent(_hud.transform, false);
        }
        internal void SetVisible(bool visible) { if (_root != null && _root.activeSelf != visible) _root.SetActive(visible); }
        internal void Destroy() { if (_root != null) UnityEngine.Object.Destroy(_root); }
        internal void Place()
        {
            if (!Alive) return;
            // Stable parent frame: the native sins holder rotates during day transitions.
            Transform holder = _hud.sins_circle.transform;
            Vector3 corner = Vector3.Scale(_wheel.localCorners[0], _wheel.transform.localScale) + _wheel.transform.localPosition;
            Vector3 point = holder.parent.TransformPoint(holder.localPosition + Vector3.Scale(corner, holder.localScale));
            Vector3 native = _hud.transform.InverseTransformPoint(point);
            _root.transform.localPosition = new Vector3(Mathf.Round(native.x + _options.X.Value), Mathf.Round(native.y + _options.Y.Value - _options.CalendarGap.Value), 0);
        }
        internal void Render(GameSave save)
        {
            _cursor = 0; int y = 0;
            bool ru = GJL.GetCurLng() == "ru";
            for (int ahead = 0; ahead < 2; ahead++)
            {
                string id = ReminderPolicy.NpcForDay(save.day, ahead);
                KnownNPC npc = save.known_npcs.GetNPC(id);
                float alpha = ahead == 0 ? 1f : _options.TomorrowAlpha.Value;
                string day = ahead == 0 ? (ru ? "СЕГОДНЯ" : "TODAY") : (ru ? "ЗАВТРА" : "TOMORROW");
                y += Line(day, y, _options.FontSize.Value, new Color(.83f, .81f, .72f), alpha, 0) + 4;
                if (npc != null) y += Line(GJL.L(id), y, _options.HeadingSize.Value, new Color(1f, .78f, .25f), alpha, 0) + 7;
                var tasks = ReminderPolicy.VisibleTasks(npc);
                if (tasks.Count == 0) y += Line(ru ? "Нет известных задач" : "No known tasks", y, _options.FontSize.Value, new Color(.84f, .81f, .73f), alpha, 0);
                for (int i = 0; i < tasks.Count; i++)
                {
                    KnownNPC.TaskState task = tasks[i];
                    string marker = task.is_dlc_stories_task ? "(*2)" : task.is_dlc_refugee_task ? "(q_marker_refugee)" : task.is_dlc_souls_task ? "(q_souls)" : "(*)";
                    Line(marker, y, _options.FontSize.Value, new Color(1f, .78f, .25f), alpha, 0, 16);
                    y += Line(task.GetTaskText(), y, _options.FontSize.Value, new Color(.96f, .87f, .68f), alpha, 18);
                    if (i + 1 < tasks.Count) y += _options.TaskGap.Value;
                }
                if (ahead == 0) y += _options.SectionGap.Value;
            }
            for (int i = _cursor; i < _labels.Count; i++) _labels[i].gameObject.SetActive(false);
            Height = y; Place();
        }
        private int Line(string text, int y, int size, Color color, float alpha, int indent, int forcedWidth = 0)
        {
            UILabel label;
            if (_cursor == _labels.Count)
            {
                var go = new GameObject("TaskReminder.Row" + _cursor); go.layer = _root.layer; go.transform.SetParent(_root.transform, false);
                label = go.AddComponent<UILabel>(); _labels.Add(label);
            }
            label = _labels[_cursor++]; label.gameObject.SetActive(true);
            UILabel template = _options.Font.Value == NativeFont.Items ? _items : _journal;
            if (template.bitmapFont != null) label.bitmapFont = template.bitmapFont; else label.trueTypeFont = template.trueTypeFont;
            label.fontSize = template.fontSize; label.fontStyle = template.fontStyle; label.symbolStyle = template.symbolStyle;
            label.spacingX = template.spacingX; label.spacingY = template.spacingY;
            GJL.EnsureLabelHasCorrectFont(label, false);
            label.fontSize = size; label.spacingY = _options.LineSpacing.Value;
            label.pivot = UIWidget.Pivot.TopLeft; label.alignment = NGUIText.Alignment.Left;
            label.overflowMethod = UILabel.Overflow.ResizeHeight; label.maxLineCount = 0;
            label.width = forcedWidth > 0 ? forcedWidth : _options.Width.Value - indent; label.height = size + 4;
            color.a = alpha * _options.TextAlpha.Value; label.color = color;
            label.effectStyle = _options.Effect.Value == TextEffect.Outline ? UILabel.Effect.Outline : _options.Effect.Value == TextEffect.Shadow ? UILabel.Effect.Shadow : UILabel.Effect.None;
            label.effectColor = new Color(.05f, .055f, .06f, _options.EffectAlpha.Value);
            label.effectDistance = new Vector2(_options.EffectSize.Value, _options.EffectSize.Value);
            label.depth = _journal.depth + 20;
            label.transform.localPosition = new Vector3(indent, -y, 0);
            label.text = text; label.ProcessText(false, true);
            return Math.Max(size, label.height);
        }
    }
}
