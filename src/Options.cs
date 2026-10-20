// SPDX-License-Identifier: MPL-2.0
using BepInEx.Configuration;

namespace TaskReminder
{
    internal enum NativeFont { Journal, Items }
    internal enum TextEffect { None, Shadow, Outline }
    internal sealed class Options
    {
        internal readonly ConfigEntry<int> X, Y, Width, FontSize, HeadingSize, LineSpacing, TaskGap, SectionGap, CalendarGap;
        internal readonly ConfigEntry<int> CaptionSize, DividerLength, DividerThickness, IconSize, ShadePaddingX, ShadePaddingY, ShadeFeather, ShadeOffsetY, QuestIconSize, ScalePercent;
        internal readonly ConfigEntry<bool> ShowCaptions, ShowDividers, PlannerProfile, SoftShade, ShowNames, QuestIconShadow, MatchPlanner;
        internal readonly ConfigEntry<float> EffectSize, DividerAlpha, ShadeAlpha;
        internal readonly ConfigEntry<float> TomorrowAlpha, TextAlpha, EffectAlpha;
        internal readonly ConfigEntry<NativeFont> Font;
        internal readonly ConfigEntry<TextEffect> Effect;
        private readonly ConfigFile _config;
        internal Options(ConfigFile config)
        {
            _config = config; config.SaveOnConfigSet = false;
            X = Integer("OffsetX", 10, -600, 600, "Positive moves right, in native HUD units.");
            Y = Integer("OffsetY", 0, -600, 600, "Positive moves up, in native HUD units.");
            Width = Integer("Width", 130, 120, 600, "Whole text block width in native HUD units.");
            FontSize = Integer("FontSize", 14, 8, 36, "Nominal NGUI body font size.");
            HeadingSize = Integer("HeadingSize", 16, 8, 40, "Nominal NGUI heading font size.");
            LineSpacing = Integer("LineSpacing", -2, -6, 16, "Additional native line spacing.");
            TaskGap = Integer("TaskGap", 1, 0, 40, "Gap between objectives.");
            SectionGap = Integer("SectionGap", 12, 4, 100, "Gap between today and tomorrow.");
            CalendarGap = Integer("CalendarGap", 14, 0, 100, "Gap below the visible calendar.");
            EffectSize = Real("EffectSize", 1f, .25f, 4f);
            CaptionSize = Integer("CaptionSize", 8, 8, 36, "Independent Today/Tomorrow caption size.");
            DividerLength = Integer("DividerLength", 45, 8, 300, "Divider length; clamped to available row width.");
            DividerThickness = Integer("DividerThickness", 1, 1, 3, "Divider thickness.");
            IconSize = Integer("DayIconSize", 16, 8, 36, "Native weekday icon size beside NPC names.");
            ShadePaddingX = Integer("ShadePaddingX", 3, 0, 12, "Soft per-line backing horizontal padding.");
            ShadePaddingY = Integer("ShadePaddingY", 0, 0, 8, "Soft per-line backing vertical padding.");
            ShadeFeather = Integer("ShadeFeather", 6, 1, 12, "Soft per-line backing edge feather.");
            ShadeOffsetY = Integer("ShadeOffsetY", -3, -24, 24, "Soft backing vertical offset; positive moves up without moving text.");
            QuestIconSize = Integer("QuestIconSize", 9, 8, 48, "Printed quest symbol height, independent of text and weekday symbols.");
            ScalePercent = Integer("BlockScalePercent", 100, 60, 150, "Whole-block multiplier relative to the selected style, in percent.");
            ShowNames = config.Bind("Appearance", "ShowNpcNames", false, "Show NPC name and weekday symbol row; off releases its space.");
            QuestIconShadow = config.Bind("Appearance", "QuestIconShadow", false, "Native glyph shadow on quest circles only, never a backing rectangle.");
            MatchPlanner = config.Bind("Appearance", "MatchBuildingPlanner", false, "Use the documented Building Planner1.0.4 style without changing manual values or requiring that mod.");
            ShowCaptions = config.Bind("Appearance", "ShowDayCaptions", true, "Show Today/Tomorrow text independently of dividers.");
            ShowDividers = config.Bind("Appearance", "ShowDayDividers", true, "Keep dividers when captions are hidden; both off removes the row.");
            PlannerProfile = config.Bind("Appearance", "PlannerTextProfile", true, "Rasterize text at24 then scale, matching the planner rendering approach.");
            SoftShade = config.Bind("Appearance", "SoftLineShade", true, "Small soft backing per printed text line, never under icons.");
            DividerAlpha = Real("DividerOpacity", 0.45f, 0f, 1f);
            ShadeAlpha = Real("ShadeOpacity", 0.25f, 0f, .65f);
            TomorrowAlpha = Real("TomorrowOpacity", 0.7f, .3f, 1f);
            TextAlpha = Real("TextOpacity", 1f, .4f, 1f);
            EffectAlpha = Real("EffectOpacity", 1f, 0f, 1f);
            Font = config.Bind("Appearance", "Font", NativeFont.Journal, "Native journal or item-name font; language fonts stay game-owned.");
            Effect = config.Bind("Appearance", "Effect", TextEffect.Outline, "Native text effect.");
        }
        private ConfigEntry<int> Integer(string key, int value, int min, int max, string description)
        { return _config.Bind("Appearance", key, value, new ConfigDescription(description, new AcceptableValueRange<int>(min, max))); }
        private ConfigEntry<float> Real(string key, float value, float min, float max)
        { return _config.Bind("Appearance", key, value, new ConfigDescription(key, new AcceptableValueRange<float>(min, max))); }
    }
}
