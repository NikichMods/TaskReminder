// SPDX-License-Identifier: MPL-2.0
using System;
using System.Reflection;
using BepInEx;
using HarmonyLib;
using UnityEngine;

namespace TaskReminder
{
    [BepInPlugin(Guid, "Task Reminder", Version)]
    public sealed class Plugin : BaseUnityPlugin
    {
        public const string Guid = "nikich.gyk.taskreminder";
        public const string Version = "0.1.1";
        internal static Plugin Instance;
        private readonly FieldInfo _inventory = AccessTools.Field(typeof(GameSave), "_inventory");
        private readonly FieldInfo _playerData = AccessTools.Field(typeof(WorldGameObject), "_data");
        private readonly FieldInfo _currentLanguage = AccessTools.Field(typeof(GJL), "cur_lng");
        private Harmony _harmony;
        private Options _options;
        private SettingsPanel _settings;
        private ReminderView _view;
        private GameSave _save;
        private object _language;
        private int _day = -1;
        private float _nextCheck;
        private bool _dirty = true, _failed, _logged;
        internal bool PanelOpen { get { return _settings != null && _settings.IsOpen; } }
        internal bool InputBlocked { get { return _settings != null && _settings.BlocksInput; } }
        internal static void Dirty() { if (Instance != null) Instance._dirty = true; }
        private void Awake()
        {
            Instance = this;
            try
            {
                _options = new Options(Config); _settings = new SettingsPanel(this, _options);
                if (_inventory == null || _playerData == null || _currentLanguage == null) throw new MissingFieldException("Current-save/language ownership fields unavailable");
                _harmony = new Harmony(Guid); _harmony.PatchAll(typeof(Plugin).Assembly);
                Logger.LogInfo("TR_START version=" + Version + " host_mvid=" + typeof(GameSave).Module.ModuleVersionId + " state=waiting-for-game");
            }
            catch (Exception ex) { Fail(ex); if (_harmony != null) _harmony.UnpatchSelf(); }
        }
        private bool Ready()
        {
            MainGame game = MainGame.me;
            if (!MainGame.game_started || MainGame.game_starting || game == null || game.save == null || game.player == null || GUIElements.me == null || GUIElements.me.hud == null || _currentLanguage.GetValue(null) == null) return false;
            object inventory = _inventory.GetValue(game.save);
            return inventory != null && ReferenceEquals(inventory, _playerData.GetValue(game.player)) && game.save.known_npcs != null;
        }
        internal bool CanOpenPanel()
        { return !_failed && Ready() && GUIElements.me.hud.gameObject.activeInHierarchy && GUIElements.me.hud.panel.alpha > .05f && BaseGUI.all_guis_closed; }
        private void Update()
        {
            if (_failed) return;
            try
            {
                if (_settings.IsOpen && !CanOpenPanel()) _settings.Close();
                if (Input.GetKeyDown(KeyCode.F2)) { if (_settings.IsOpen) _settings.Close(); else if (CanOpenPanel()) _settings.Open(); }
                if (_settings.IsOpen && Input.GetKeyDown(KeyCode.Escape)) _settings.Close();
                if (Time.unscaledTime < _nextCheck) return;
                _nextCheck = Time.unscaledTime + .25f;
                if (!Ready()) { if (_view != null) _view.SetVisible(false); _save = null; return; }
                GUIElements gui = GUIElements.me;
                if (_view == null || !_view.Alive || _view.Hud != gui.hud)
                { if (_view != null) _view.Destroy(); _view = new ReminderView(_options, gui); _dirty = true; }
                GameSave save = MainGame.me.save;
                object language = _currentLanguage.GetValue(null);
                if (!ReferenceEquals(_save, save) || _day != save.day || !ReferenceEquals(_language, language)) _dirty = true;
                _save = save; _day = save.day; _language = language;
                if (_dirty) { _view.Render(save); _dirty = false; }
                if (!_logged) { Logger.LogInfo("TR_READY version=" + Version + " journal_font=" + gui.npcs_list.prefab.quest_prefab.txt.bitmapFont.name + " item_font=" + gui.craft.craft_item_prefab.label_name.bitmapFont.name); _logged = true; }
            }
            catch (Exception ex) { Fail(ex); }
        }
        private void LateUpdate()
        {
            if (_failed || _view == null || !_view.Alive) return;
            try
            {
                bool visible = MainGame.game_started && !MainGame.game_starting && _save != null && ReferenceEquals(_save, MainGame.me.save) && _view.Hud.gameObject.activeInHierarchy && BaseGUI.all_guis_closed;
                _view.SetVisible(visible); if (visible) _view.Place();
            }
            catch (Exception ex) { Fail(ex); }
        }
        private void OnGUI() { if (!_failed && _settings != null) try { _settings.Draw(); } catch (Exception ex) { Fail(ex); } }
        internal void Changed() { _dirty = true; }
        internal int DisplayHeight { get { return _view == null ? 0 : _view.Height; } }
        internal void LogSettings() { Logger.LogInfo("TR_SETTINGS x=" + _options.X.Value + " y=" + _options.Y.Value + " width=" + _options.Width.Value + " font=" + _options.Font.Value + " size=" + _options.FontSize.Value + " effect=" + _options.Effect.Value + " caption=" + _options.ShowCaptions.Value + " divider=" + _options.ShowDividers.Value + " profile=" + _options.PlannerProfile.Value + " shade=" + _options.SoftShade.Value); }
        private void Fail(Exception ex)
        {
            if (_failed) return; _failed = true;
            try { if (_settings != null) _settings.Close(); } catch (Exception close) { Logger.LogError("TR_INPUT_RESTORE_FAILED " + close); }
            if (_view != null) _view.SetVisible(false);
            Logger.LogError("TR_DISABLED action=disabled-for-session vanilla=preserved " + ex);
        }
        private void OnDisable()
        {
            try { if (_settings != null) _settings.Close(); }
            catch (Exception ex) { Logger.LogError("TR_SETTINGS_SAVE_FAILED input=restored " + ex); }
            finally { if (_view != null) _view.SetVisible(false); }
        }
        private void OnDestroy()
        {
            try { if (_settings != null) _settings.Close(); }
            catch (Exception ex) { Logger.LogError("TR_SETTINGS_SAVE_FAILED input=restored " + ex); }
            finally
            {
                if (_view != null) _view.Destroy();
                if (_harmony != null) _harmony.UnpatchSelf(); if (Instance == this) Instance = null;
            }
        }
    }
    [HarmonyPatch(typeof(KnownNPC), "SetQuestState")]
    internal static class TaskChangedPatch { private static void Postfix() { Plugin.Dirty(); } }
}
