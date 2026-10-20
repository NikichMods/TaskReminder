// SPDX-License-Identifier: MPL-2.0
using BepInEx.Configuration;

namespace TaskReminder
{
    internal enum NativeFont { Journal, Items }
    internal enum TextEffect { None, Shadow, Outline }
    internal sealed class Options
    {
        internal readonly ConfigEntry<int> X, Y, Width, FontSize, HeadingSize, LineSpacing, TaskGap, SectionGap, CalendarGap, EffectSize;
        internal readonly ConfigEntry<float> TomorrowAlpha, TextAlpha, EffectAlpha;
        internal readonly ConfigEntry<NativeFont> Font;
        internal readonly ConfigEntry<TextEffect> Effect;
        private readonly ConfigFile _config;
        internal Options(ConfigFile config)
        {
            _config = config; config.SaveOnConfigSet = false;
            X = Integer("OffsetX", 0, -600, 600, "Positive moves right, in native HUD units.");
            Y = Integer("OffsetY", 0, -600, 600, "Positive moves up, in native HUD units.");
            Width = Integer("Width", 260, 120, 600, "Whole text block width in native HUD units.");
            FontSize = Integer("FontSize", 16, 8, 36, "Nominal NGUI body font size.");
            HeadingSize = Integer("HeadingSize", 18, 8, 40, "Nominal NGUI heading font size.");
            LineSpacing = Integer("LineSpacing", -2, -6, 16, "Additional native line spacing.");
            TaskGap = Integer("TaskGap", 7, 0, 40, "Gap between objectives.");
            SectionGap = Integer("SectionGap", 20, 4, 100, "Gap between today and tomorrow.");
            CalendarGap = Integer("CalendarGap", 14, 0, 100, "Gap below the visible calendar.");
            EffectSize = Integer("EffectSize", 1, 1, 4, "Shadow/outline offset.");
            TomorrowAlpha = Real("TomorrowOpacity", .78f, .3f, 1f);
            TextAlpha = Real("TextOpacity", 1f, .4f, 1f);
            EffectAlpha = Real("EffectOpacity", .85f, 0f, 1f);
            Font = config.Bind("Appearance", "Font", NativeFont.Journal, "Native journal or item-name font; language fonts stay game-owned.");
            Effect = config.Bind("Appearance", "Effect", TextEffect.Shadow, "Native text effect.");
        }
        private ConfigEntry<int> Integer(string key, int value, int min, int max, string description)
        { return _config.Bind("Appearance", key, value, new ConfigDescription(description, new AcceptableValueRange<int>(min, max))); }
        private ConfigEntry<float> Real(string key, float value, float min, float max)
        { return _config.Bind("Appearance", key, value, new ConfigDescription(key, new AcceptableValueRange<float>(min, max))); }
        internal void Save() { _config.Save(); }
        internal void Reset()
        {
            foreach (var entry in _config) entry.Value.BoxedValue = entry.Value.DefaultValue;
        }
    }
}
