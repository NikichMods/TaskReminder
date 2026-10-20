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
            IsOpen = true;
            try
            {
                _character.control_enabled = false;
                _events.useMouse = _events.useTouch = _events.useKeyboard = _events.useController = false;
            }
            catch { Close(); throw; }
        }
        internal void Close()
        {
            if (!IsOpen) return;
            IsOpen = false;
            _suppressUntilFrame = Time.frameCount + 1;
            try
            {
                if (_events != null) { _events.useMouse = _mouse; _events.useTouch = _touch; _events.useKeyboard = _keyboard; _events.useController = _controller; }
            }
            finally
            {
                if (_character != null) _character.control_enabled = _control;
                _events = null; _character = null;
            }
            _options.Save(); _plugin.LogSettings();
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
            GUILayout.Label("Изменения видны сразу; сохраняются при закрытии.");
            _scroll = GUILayout.BeginScrollView(_scroll);
            bool previous = GUI.changed; GUI.changed = false;
            GUILayout.Label("Положение — единицы HUD, с учётом масштаба игры");
            Slider("X: вправо →", _options.X, -600, 600);
            Slider("Y: вверх ↑", _options.Y, -600, 600);
            Slider("Отступ под календарём", _options.CalendarGap, 0, 100);
            Slider("Ширина блока", _options.Width, 120, 600);
            GUILayout.Space(6); GUILayout.Label("Шрифт");
            _options.Font.Value = (NativeFont)GUILayout.SelectionGrid((int)_options.Font.Value, new[] { "Журнал / мелкий", "Предметы / крупный" }, 2);
            Slider("Размер задач", _options.FontSize, 8, 36);
            Slider("Размер имён", _options.HeadingSize, 8, 40);
            Slider("Межстрочный отступ", _options.LineSpacing, -6, 16);
            Slider("Между задачами", _options.TaskGap, 0, 40);
            Slider("Между сегодня / завтра", _options.SectionGap, 4, 100);
            GUILayout.Space(6); GUILayout.Label("Читаемость");
            _options.Effect.Value = (TextEffect)GUILayout.SelectionGrid((int)_options.Effect.Value, new[] { "Без эффекта", "Тень", "Обводка" }, 3);
            Slider("Размер эффекта", _options.EffectSize, 1, 4);
            Percent("Сила тени / обводки", _options.EffectAlpha, 0, 1);
            Percent("Яркость текста", _options.TextAlpha, .4f, 1);
            Percent("Яркость завтрашнего блока", _options.TomorrowAlpha, .3f, 1);
            GUILayout.Label("Высота текста: " + _plugin.DisplayHeight + " ед. HUD.\nДлинные задачи переносятся целиком, без сокращения.");
            if (GUILayout.Button("Вернуть исходные настройки")) { _options.Reset(); GUI.changed = true; }
            bool changed = GUI.changed; GUI.changed |= previous;
            if (changed) _plugin.Changed();
            GUILayout.EndScrollView();
            if (GUILayout.Button("Сохранить и закрыть (F2)")) Close();
            GUI.DragWindow(new Rect(0, 0, _window.width, 22));
        }
        private static void Slider(string name, ConfigEntry<int> entry, int min, int max)
        {
            GUILayout.Label(name + ": " + entry.Value);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("−", GUILayout.Width(28))) entry.Value = Math.Max(min, entry.Value - 1);
            entry.Value = Mathf.RoundToInt(GUILayout.HorizontalSlider(entry.Value, min, max));
            if (GUILayout.Button("+", GUILayout.Width(28))) entry.Value = Math.Min(max, entry.Value + 1);
            GUILayout.EndHorizontal();
        }
        private static void Percent(string name, ConfigEntry<float> entry, float min, float max)
        {
            GUILayout.Label(name + ": " + Mathf.RoundToInt(entry.Value * 100) + "%");
            entry.Value = Mathf.Round(GUILayout.HorizontalSlider(entry.Value, min, max) * 100) / 100f;
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
