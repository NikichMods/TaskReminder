// SPDX-License-Identifier: MPL-2.0
using System;
using System.Collections.Generic;
using UnityEngine;

namespace TaskReminder
{
    internal sealed class ReminderView
    {
        private readonly Options _options;
        private readonly Appearance _style;
        private readonly Transform _plannerSpace;
        private readonly GameObject _root;
        private readonly HUD _hud;
        private readonly UIWidget _wheel;
        private readonly UILabel _journal, _items;
        private readonly UILabel _dayIcons;
        private readonly TextDecorations _decorations;
        private UILabel _lastLabel;
        private readonly List<UILabel> _labels = new List<UILabel>();
        private int _cursor;
        internal int Height { get; private set; }
        internal bool Alive { get { return _root != null && _hud != null && _wheel != null; } }
        internal HUD Hud { get { return _hud; } }
        internal ReminderView(Options options, GUIElements gui)
        {
            _options = options; _style = new Appearance(options); _hud = gui.hud;
            _plannerSpace = gui.screen_size_widget == null ? null : gui.screen_size_widget.transform.parent;
            Transform circle = _hud.sins_circle.transform.Find("test circle (1)");
            if (circle == null) throw new InvalidOperationException("Native calendar backing missing");
            _wheel = circle.GetComponent<UIWidget>();
            if (_wheel == null || _wheel.width != 120 || _wheel.height != 120) throw new InvalidOperationException("Native calendar backing contract changed");
            _journal = gui.npcs_list.prefab.quest_prefab.txt;
            _items = gui.craft.craft_item_prefab.label_name;
            _dayIcons = gui.npcs_list.prefab.top_icon_txt;
            if (_journal == null || _items == null) throw new InvalidOperationException("Native font templates unavailable");
            _root = new GameObject("TaskReminder.Text"); _root.layer = _hud.gameObject.layer;
            _root.SetActive(false); _root.transform.SetParent(_hud.transform, false);
            _decorations = new TextDecorations(_root.transform, _root.layer, _journal.depth + 18, options);
        }
        internal void SetVisible(bool visible) { if (_root != null && _root.activeSelf != visible) _root.SetActive(visible); }
        internal void Destroy() { _decorations.Destroy(); if (_root != null) UnityEngine.Object.Destroy(_root); }
        internal void Place()
        {
            if (!Alive) return;
            if (_style.Matched)
            {
                if (_plannerSpace == null) throw new InvalidOperationException("Planner screen reference missing");
                Vector3 right = _hud.transform.InverseTransformVector(_plannerSpace.TransformVector(Vector3.right));
                Vector3 up = _hud.transform.InverseTransformVector(_plannerSpace.TransformVector(Vector3.up));
                _root.transform.localScale = Appearance.InHudScale(right, up, _style.Scale);
            }
            else _root.transform.localScale = new Vector3(_style.Scale, _style.Scale, 1);
            // Stable parent frame: the native sins holder rotates during day transitions.
            Transform holder = _hud.sins_circle.transform;
            Vector3 corner = Vector3.Scale(_wheel.localCorners[0], _wheel.transform.localScale) + _wheel.transform.localPosition;
            Vector3 point = holder.parent.TransformPoint(holder.localPosition + Vector3.Scale(corner, holder.localScale));
            Vector3 native = _hud.transform.InverseTransformPoint(point);
            _root.transform.localPosition = new Vector3(Mathf.Round(native.x + _options.X.Value), Mathf.Round(native.y + _options.Y.Value - _options.CalendarGap.Value), 0);
        }
        internal void Render(GameSave save)
        {
            _cursor = 0; int y = 0; _decorations.Begin();
            bool ru = GJL.GetCurLng() == "ru";
            for (int ahead = 0; ahead < 2; ahead++)
            {
                string id = ReminderPolicy.NpcForDay(save.day, ahead);
                KnownNPC npc = save.known_npcs.GetNPC(id);
                float alpha = ahead == 0 ? 1f : _style.TomorrowAlpha;
                string day = ahead == 0 ? (ru ? "СЕГОДНЯ" : "TODAY") : (ru ? "ЗАВТРА" : "TOMORROW");
                int captionHeight = 0; float captionWidth = 0;
                if (_options.ShowCaptions.Value)
                {
                    captionHeight = Line(day, y, _style.CaptionSize, new Color(.83f, .81f, .72f), alpha, 0);
                    captionWidth = _lastLabel.printedSize.x * _lastLabel.transform.localScale.x;
                }
                int rowHeight = ReminderPolicy.CaptionRowHeight(_options.ShowCaptions.Value, _options.ShowDividers.Value, captionHeight, _options.DividerThickness.Value);
                if (_options.ShowDividers.Value)
                    _decorations.Divider(_options.ShowCaptions.Value ? captionWidth + 8 : 0, y + Math.Max(captionHeight, _options.DividerThickness.Value) / 2f, alpha);
                y += rowHeight;
                if (npc != null && _options.ShowNames.Value)
                {
                    ObjectDefinition definition = GameBalance.me.GetDataOrNull<ObjectDefinition>(id);
                    string symbol = definition == null ? null : definition.day_icon;
                    int inset = string.IsNullOrEmpty(symbol) ? 0 : _options.IconSize.Value + 5;
                    int nameHeight = Line(GJL.L(id), y, _style.HeadingSize, new Color(1f, .78f, .25f), alpha, inset);
                    if (inset > 0) nameHeight = Math.Max(nameHeight, Line(symbol, y, _options.IconSize.Value, Color.white, alpha, 0, _options.IconSize.Value + 4, true, _dayIcons));
                    y += nameHeight + 7;
                }
                var tasks = ReminderPolicy.VisibleTasks(npc);
                if (tasks.Count == 0) y += Line(ru ? "Нет известных задач" : "No known tasks", y, _style.FontSize, new Color(.84f, .81f, .73f), alpha, 0);
                for (int i = 0; i < tasks.Count; i++)
                {
                    KnownNPC.TaskState task = tasks[i];
                    string marker = task.is_dlc_stories_task ? "(*2)" : task.is_dlc_refugee_task ? "(q_marker_refugee)" : task.is_dlc_souls_task ? "(q_souls)" : "(*)";
                    int markerHeight = Line(marker, y, _options.QuestIconSize.Value, new Color(1f, .78f, .25f), alpha, 0, _options.QuestIconSize.Value + 4, true, null, true);
                    int inset = Mathf.CeilToInt(_lastLabel.printedSize.x * _lastLabel.transform.localScale.x) + 4;
                    int textHeight = Line(task.GetTaskText(), y, _style.FontSize, new Color(.96f, .87f, .68f), alpha, inset);
                    y += Math.Max(markerHeight, textHeight);
                    if (i + 1 < tasks.Count) y += _style.TaskGap;
                }
                if (ahead == 0) y += _style.SectionGap;
            }
            for (int i = _cursor; i < _labels.Count; i++) _labels[i].gameObject.SetActive(false);
            _decorations.End(); Height = y; Place();
        }
        private int Line(string text, int y, int size, Color color, float alpha, int indent, int forcedWidth = 0, bool symbol = false, UILabel fontOverride = null, bool questMarker = false)
        {
            UILabel label;
            if (_cursor == _labels.Count)
            {
                var go = new GameObject("TaskReminder.Row" + _cursor); go.layer = _root.layer; go.transform.SetParent(_root.transform, false);
                label = go.AddComponent<UILabel>(); _labels.Add(label);
            }
            label = _labels[_cursor++]; label.gameObject.SetActive(true);
            UILabel template = fontOverride != null ? fontOverride : _style.Font == NativeFont.Items ? _items : _journal;
            if (template.bitmapFont != null) label.bitmapFont = template.bitmapFont; else label.trueTypeFont = template.trueTypeFont;
            label.fontSize = template.fontSize; label.fontStyle = template.fontStyle; label.symbolStyle = template.symbolStyle;
            label.spacingX = template.spacingX; label.spacingY = template.spacingY;
            if (fontOverride == null) GJL.EnsureLabelHasCorrectFont(label, false);
            if (!symbol) label.symbolStyle = NGUIText.SymbolStyle.NoEffect;
            float scale = _style.RasterProfile && !symbol ? size / 24f : 1f;
            label.fontSize = _style.RasterProfile && !symbol ? 24 : size;
            label.spacingY = symbol || _style.Matched ? template.spacingY : Mathf.RoundToInt(_style.LineSpacing / scale);
            label.pivot = UIWidget.Pivot.TopLeft; label.alignment = NGUIText.Alignment.Left;
            label.overflowMethod = symbol ? UILabel.Overflow.ResizeFreely : UILabel.Overflow.ResizeHeight; label.maxLineCount = 0;
            label.width = Mathf.Max(1, Mathf.RoundToInt((forcedWidth > 0 ? forcedWidth : _style.Width - indent) / scale)); label.height = label.fontSize + 4;
            if (fontOverride != null) color = template.color;
            if (!symbol) color = _style.TextColor(color);
            color.a = alpha * _style.TextAlpha; label.color = color;
            label.effectStyle = symbol ? (questMarker && _options.QuestIconShadow.Value ? UILabel.Effect.Shadow : UILabel.Effect.None) : _style.Effect == TextEffect.Outline ? UILabel.Effect.Outline : _style.Effect == TextEffect.Shadow ? UILabel.Effect.Shadow : UILabel.Effect.None;
            label.effectColor = _style.EffectColor;
            label.effectDistance = new Vector2(_style.EffectSize, _style.EffectSize);
            label.depth = _journal.depth + 20;
            label.transform.localPosition = new Vector3(indent, -y, 0);
            label.transform.localScale = new Vector3(scale, scale, 1);
            label.text = text; label.ProcessText(false, true);
            if (symbol)
            {
                scale = size / Math.Max(1f, label.printedSize.y);
                label.transform.localScale = new Vector3(scale, scale, 1);
            }
            if (!symbol) _decorations.Shade(label, alpha, indent > 0 ? Math.Min(3, _style.ShadePaddingX) : _style.ShadePaddingX);
            _lastLabel = label;
            return Mathf.CeilToInt(Math.Max(size, label.height * scale));
        }
    }
}
