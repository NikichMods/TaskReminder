// SPDX-License-Identifier: MPL-2.0
using System;
using System.Collections.Generic;
using System.Reflection;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;

namespace TaskReminder
{
    internal sealed class SettingsPanel
    {
        private readonly Plugin _plugin;
        private readonly Options _options;
        private BaseCharacterComponent _character;
        private UICamera _events;
        private Dictionary<ConfigEntryBase, object> _snapshot;
        private bool _control, _mouse, _touch, _keyboard, _controller;
        private Rect _window;
        private bool _placed;
        private Vector2 _scroll;
        private int _suppressUntilFrame = -1;
        internal bool IsOpen { get; private set; }
        internal bool BlocksInput { get { return IsOpen || Time.frameCount <= _suppressUntilFrame; } }
        internal SettingsPanel(Plugin plugin, Options options) { _plugin = plugin; _options = options; }
        internal void Open()
        {
            if (IsOpen) return;
            var player = MainGame.me.player;
            if (player.components == null || player.components.character == null) return;
            _character = player.components.character;
            _events = UICamera.FindCameraForLayer(GUIElements.me.hud.gameObject.layer);
            if (_events == null) { _character = null; return; }
            _control = _character.control_enabled;
            _mouse = _events.useMouse; _touch = _events.useTouch; _keyboard = _events.useKeyboard; _controller = _events.useController;
            _snapshot = _options.Capture();
            IsOpen = true;
            try
            {
                _character.control_enabled = false;
                _events.useMouse = _events.useTouch = _events.useKeyboard = _events.useController = false;
            }
            catch { Close(false); throw; }
        }
        internal void Close(bool save)
        {
            if (!IsOpen) return;
            IsOpen = false;
            _suppressUntilFrame = Time.frameCount + 1;
            var snapshot = _snapshot; _snapshot = null;
            try
            {
                if (_events != null) { _events.useMouse = _mouse; _events.useTouch = _touch; _events.useKeyboard = _keyboard; _events.useController = _controller; }
            }
            finally
            {
                try { if (_character != null) _character.control_enabled = _control; }
                finally
                {
                    _events = null; _character = null;
                    if (!save) { _options.Restore(snapshot); _plugin.Changed(); }
                }
            }
            if (save) { _options.Save(); _plugin.LogSettings(); }
        }
        internal void Draw()
        {
            if (!IsOpen) return;
            float width = Math.Min(420, Screen.width - 24), height = Math.Min(720, Screen.height - 40);
            if (!_placed) { _window = new Rect(Screen.width - width - 20, 20, width, height); _placed = true; }
            _window.width = width; _window.height = height;
            _window.x = Mathf.Clamp(_window.x, 0, Math.Max(0, Screen.width - width));
            _window.y = Mathf.Clamp(_window.y, 0, Math.Max(0, Screen.height - height));
            _window = GUI.Window(197032, _window, Window, "Task Reminder " + Plugin.Version + " — F2 / Esc");
        }
        private void Window(int id)
        {
            GUILayout.Label("Предпросмотр сразу. F2 — сохранить; Esc — отменить.");
            _scroll = GUILayout.BeginScrollView(_scroll);
            bool previous = GUI.changed; GUI.changed = false;
            _options.MatchPlanner.Value = GUILayout.Toggle(_options.MatchPlanner.Value, "Стиль Building Planner (вкл. / выкл.)");
            GUILayout.Label(_options.MatchPlanner.Value ? "Шрифт 24 × 65%, обводка и цвет Planner.\nРучной стиль сохранён; выключи галочку для его настройки." : "Ручной стиль. Галочка Planner сохраняет эти значения.");
            Slider("Масштаб блока относительно стиля (%)", _options.ScalePercent, 60, 150);
            _options.ShowNames.Value = GUILayout.Toggle(_options.ShowNames.Value, "Показывать имена и значки дней");
            Slider("Размер квестовых кругляшков", _options.QuestIconSize, 8, 48);
            _options.QuestIconShadow.Value = GUILayout.Toggle(_options.QuestIconShadow.Value, "Тень квестовых кругляшков");
            bool enabled = GUI.enabled;
            GUILayout.Label("Положение — единицы HUD, с учётом масштаба игры");
            Slider("X: вправо →", _options.X, -600, 600);
            Slider("Y: вверх ↑", _options.Y, -600, 600);
            Slider("Отступ под календарём", _options.CalendarGap, 0, 100);
            GUI.enabled = enabled && !_options.MatchPlanner.Value;
            Slider("Ширина блока", _options.Width, 120, 600);
            GUI.enabled = enabled;
            GUILayout.Space(6); GUILayout.Label("Строки «Сегодня / Завтра»");
            _options.ShowCaptions.Value = GUILayout.Toggle(_options.ShowCaptions.Value, "Показывать надписи Сегодня / Завтра");
            GUI.enabled = enabled && !_options.MatchPlanner.Value;
            Slider("Размер этих надписей", _options.CaptionSize, 8, 36);
            GUI.enabled = enabled;
            _options.ShowDividers.Value = GUILayout.Toggle(_options.ShowDividers.Value, "Показывать линии независимо от надписей");
            Slider("Длина линий", _options.DividerLength, 8, 300);
            Slider("Толщина линий", _options.DividerThickness, 1, 3);
            Percent("Яркость линий", _options.DividerAlpha, 0, 1);
            Slider("Размер значков дней", _options.IconSize, 8, 36);
            GUILayout.Space(6); GUILayout.Label("Шрифт");
            GUI.enabled = enabled && !_options.MatchPlanner.Value;
            _options.Font.Value = (NativeFont)GUILayout.SelectionGrid((int)_options.Font.Value, new[] { "Журнал / мелкий", "Предметы / крупный" }, 2);
            _options.PlannerProfile.Value = GUILayout.Toggle(_options.PlannerProfile.Value, "Растеризация 24 с подгонкой размера");
            Slider("Размер задач", _options.FontSize, 8, 36);
            Slider("Размер имён", _options.HeadingSize, 8, 40);
            Slider("Межстрочный отступ", _options.LineSpacing, -6, 16);
            Slider("Между задачами", _options.TaskGap, 0, 40);
            Slider("Между сегодня / завтра", _options.SectionGap, 4, 100);
            GUILayout.Space(6); GUILayout.Label("Читаемость");
            _options.Effect.Value = (TextEffect)GUILayout.SelectionGrid((int)_options.Effect.Value, new[] { "Без эффекта", "Тень", "Обводка" }, 3);
            GUILayout.Label("Смещение тени / обводки: " + _options.EffectSize.Value.ToString("0.00"));
            GUILayout.BeginHorizontal();
            _options.EffectSize.Value = Mathf.Round(GUILayout.HorizontalSlider(_options.EffectSize.Value, .25f, 4f) * 20f) / 20f;
            ResetButton(_options.EffectSize);
            GUILayout.EndHorizontal();
            Percent("Сила тени / обводки", _options.EffectAlpha, 0, 1);
            Percent("Яркость текста", _options.TextAlpha, .4f, 1);
            Percent("Яркость завтрашнего блока", _options.TomorrowAlpha, .3f, 1);
            GUILayout.Space(6); GUILayout.Label("Мягкая тень под строками — без значков");
            _options.SoftShade.Value = GUILayout.Toggle(_options.SoftShade.Value, "Включить мягкую тень под каждой строкой");
            Percent("Сила мягкой тени", _options.ShadeAlpha, 0, .65f);
            Slider("Положение тени: вверх ↑", _options.ShadeOffsetY, -24, 24);
            Slider("Запас по ширине", _options.ShadePaddingX, 0, 12);
            Slider("Запас по высоте", _options.ShadePaddingY, 0, 8);
            Slider("Мягкость краёв", _options.ShadeFeather, 1, 12);
            GUI.enabled = enabled;
            GUILayout.Label("Высота текста: " + _plugin.DisplayHeight + " ед. HUD.\nДлинные задачи переносятся целиком, без сокращения.");
            if (GUILayout.Button("Вернуть исходные настройки")) { _options.Reset(); GUI.changed = true; }
            bool changed = GUI.changed; GUI.changed |= previous;
            if (changed) _plugin.Changed();
            GUILayout.EndScrollView();
            if (GUILayout.Button("Сохранить и закрыть (F2)")) Close(true);
            if (GUILayout.Button("Отменить и закрыть (Esc)")) Close(false);
            GUI.DragWindow(new Rect(0, 0, _window.width, 22));
        }
        private static void Slider(string name, ConfigEntry<int> entry, int min, int max)
        {
            GUILayout.Label(name + ": " + entry.Value);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("−", GUILayout.Width(28))) entry.Value = Math.Max(min, entry.Value - 1);
            entry.Value = Mathf.RoundToInt(GUILayout.HorizontalSlider(entry.Value, min, max));
            if (GUILayout.Button("+", GUILayout.Width(28))) entry.Value = Math.Min(max, entry.Value + 1);
            ResetButton(entry);
            GUILayout.EndHorizontal();
        }
        private static void ResetButton(ConfigEntryBase entry)
        {
            if (GUILayout.Button("Сброс", GUILayout.Width(58))) { Options.Reset(entry); GUI.changed = true; }
        }
        private static void Percent(string name, ConfigEntry<float> entry, float min, float max)
        {
            GUILayout.Label(name + ": " + Mathf.RoundToInt(entry.Value * 100) + "%");
            GUILayout.BeginHorizontal();
            entry.Value = Mathf.Round(GUILayout.HorizontalSlider(entry.Value, min, max) * 100) / 100f;
            ResetButton(entry);
            GUILayout.EndHorizontal();
        }
    }
    // Same bounded input-family guard as the accepted planner calibration pattern.
    [HarmonyPatch]
    internal static class SettingsKeyGuard
    {
        private static IEnumerable<MethodBase> TargetMethods()
        {
            foreach (var method in typeof(LazyInput).GetMethods(BindingFlags.Static | BindingFlags.Public))
                if (method.ReturnType == typeof(bool) && (method.Name == "GetKey" || method.Name == "GetKeyDown" || method.Name == "AnyClick" || method.Name == "AnyKeyDown")) yield return method;
        }
        private static bool Prefix(ref bool __result)
        { if (Plugin.Instance == null || !Plugin.Instance.InputBlocked) return true; __result = false; return false; }
    }
    [HarmonyPatch]
    internal static class SettingsDirectionGuard
    {
        private static IEnumerable<MethodBase> TargetMethods()
        {
            foreach (var method in typeof(LazyInput).GetMethods(BindingFlags.Static | BindingFlags.Public))
                if (method.ReturnType == typeof(Vector2) && (method.Name == "GetDirection" || method.Name == "GetDirection2")) yield return method;
        }
        private static bool Prefix(ref Vector2 __result)
        { if (Plugin.Instance == null || !Plugin.Instance.InputBlocked) return true; __result = Vector2.zero; return false; }
    }
}
