// SPDX-License-Identifier: MPL-2.0
using System;
using UnityEngine;

namespace TaskReminder
{
    // Local documented profile, not a dependency on Building Planner's runtime.
    internal sealed class Appearance
    {
        private readonly Options _options;
        internal Appearance(Options options) { _options = options; }
        internal bool Matched { get { return _options.MatchPlanner.Value; } }
        internal int Width { get { return Matched ? 286 : _options.Width.Value; } }
        internal int FontSize { get { return Matched ? 24 : _options.FontSize.Value; } }
        internal int HeadingSize { get { return Matched ? 24 : _options.HeadingSize.Value; } }
        internal int CaptionSize { get { return Matched ? 21 : _options.CaptionSize.Value; } }
        internal int LineSpacing { get { return Matched ? 0 : _options.LineSpacing.Value; } }
        internal int TaskGap { get { return Matched ? 3 : _options.TaskGap.Value; } }
        internal int SectionGap { get { return Matched ? 12 : _options.SectionGap.Value; } }
        internal NativeFont Font { get { return Matched ? NativeFont.Items : _options.Font.Value; } }
        internal bool RasterProfile { get { return !Matched && _options.PlannerProfile.Value; } }
        internal TextEffect Effect { get { return Matched ? TextEffect.Outline : _options.Effect.Value; } }
        internal float EffectSize { get { return Matched ? 1 : _options.EffectSize.Value; } }
        internal float EffectAlpha { get { return Matched ? .8f : _options.EffectAlpha.Value; } }
        internal float TextAlpha { get { return Matched ? 1 : _options.TextAlpha.Value; } }
        internal float TomorrowAlpha { get { return Matched ? 1 : _options.TomorrowAlpha.Value; } }
        internal bool SoftShade { get { return Matched || _options.SoftShade.Value; } }
        internal float ShadeAlpha { get { return Matched ? .39f : _options.ShadeAlpha.Value; } }
        internal int ShadePaddingX { get { return Matched ? 12 : _options.ShadePaddingX.Value; } }
        internal int ShadePaddingY { get { return Matched ? 1 : _options.ShadePaddingY.Value; } }
        internal int ShadeOffsetY { get { return Matched ? 0 : _options.ShadeOffsetY.Value; } }
        internal int Feather(int height) { return Matched ? Math.Max(1, Math.Min(28, height / 2)) : _options.ShadeFeather.Value; }
        internal Color TextColor(Color manual) { return Matched ? new Color(.94f, .90f, .80f) : manual; }
        internal Color EffectColor { get { return Matched ? new Color(.065f, .065f, .055f, EffectAlpha) : new Color(.05f, .055f, .06f, EffectAlpha); } }
        internal Color ShadeColor(float alpha) { return Matched ? new Color(.07f, .067f, .055f, alpha * ShadeAlpha) : new Color(0, 0, 0, alpha * ShadeAlpha); }
        internal float Scale { get { return (Matched ? .65f : 1f) * _options.ScalePercent.Value / 100f; } }
        internal static Vector3 InHudScale(Vector3 right, Vector3 up, float scale)
        {
            if (!Positive(right.x) || !Positive(up.y) || !Zero(right.y) || !Zero(right.z) || !Zero(up.x) || !Zero(up.z) || !Positive(scale))
                throw new InvalidOperationException("Planner reference frame must have positive axis-aligned scale");
            return new Vector3(right.x * scale, up.y * scale, 1);
        }
        private static bool Zero(float value) { return Math.Abs(value) <= .001f; }
        private static bool Positive(float value) { return value > 0 && !float.IsInfinity(value) && !float.IsNaN(value); }
    }
}
