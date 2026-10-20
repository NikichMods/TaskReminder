// SPDX-License-Identifier: MPL-2.0
using System;
using System.Collections.Generic;
using UnityEngine;

namespace TaskReminder
{
    internal sealed class TextDecorations
    {
        private sealed class Band
        {
            internal UITexture Widget;
            internal Texture2D Texture;
            internal int Width, Height, Feather;
        }
        private readonly Transform _root;
        private readonly int _layer, _depth;
        private readonly Options _options;
        private readonly UILabel _measure;
        private readonly List<Band> _bands = new List<Band>();
        private readonly List<UITexture> _dividers = new List<UITexture>();
        private int _bandCursor, _dividerCursor;
        internal TextDecorations(Transform root, int layer, int depth, Options options)
        {
            _root = root; _layer = layer; _depth = depth; _options = options;
            _measure = Child("TaskReminder.Measure").AddComponent<UILabel>();
            _measure.gameObject.SetActive(false);
        }
        private GameObject Child(string name)
        { var go = new GameObject(name); go.layer = _layer; go.transform.SetParent(_root, false); return go; }
        internal void Begin() { _bandCursor = 0; _dividerCursor = 0; }
        internal void End()
        {
            for (int i = _bandCursor; i < _bands.Count; i++) _bands[i].Widget.gameObject.SetActive(false);
            for (int i = _dividerCursor; i < _dividers.Count; i++) _dividers[i].gameObject.SetActive(false);
        }
        internal void Divider(float x, float centerY, float alpha)
        {
            int width = Math.Min(_options.DividerLength.Value, Mathf.FloorToInt(_options.Width.Value - x));
            if (width < 1) return;
            if (_dividerCursor == _dividers.Count)
            {
                var widget = Child("TaskReminder.Divider").AddComponent<UITexture>();
                widget.mainTexture = Texture2D.whiteTexture; widget.pivot = UIWidget.Pivot.TopLeft; widget.depth = _depth + 1;
                _dividers.Add(widget);
            }
            var line = _dividers[_dividerCursor++]; line.gameObject.SetActive(true);
            line.width = width; line.height = _options.DividerThickness.Value;
            line.transform.localPosition = new Vector3(Mathf.Round(x), -Mathf.Round(centerY - line.height / 2f), 0);
            line.color = new Color(.83f, .81f, .72f, alpha * _options.DividerAlpha.Value);
        }
        private Vector2 Measure(string text)
        { _measure.text = text; _measure.ProcessText(false, true); return _measure.printedSize; }
        internal void Shade(UILabel source, float alpha, int leftPadding)
        {
            if (!_options.SoftShade.Value || _options.ShadeAlpha.Value <= 0) return;
            if (source.bitmapFont != null) _measure.bitmapFont = source.bitmapFont; else _measure.trueTypeFont = source.trueTypeFont;
            _measure.fontSize = source.fontSize; _measure.fontStyle = source.fontStyle; _measure.symbolStyle = source.symbolStyle;
            _measure.spacingX = source.spacingX; _measure.spacingY = source.spacingY;
            _measure.overflowMethod = UILabel.Overflow.ResizeFreely; _measure.maxLineCount = 0;
            _measure.effectStyle = UILabel.Effect.None;
            float step = Measure("M\nM").y - Measure("M").y;
            if (step <= 0) step = source.fontSize;
            float scale = source.transform.localScale.x;
            string[] lines = source.processedText.Split('\n');
            for (int i = 0; i < lines.Length; i++)
            {
                Vector2 size = Measure(lines[i].TrimEnd('\r')) * scale;
                if (size.x < 1 || size.y < 1) continue;
                int px = _options.ShadePaddingX.Value, py = _options.ShadePaddingY.Value;
                Vector3 pos = source.transform.localPosition + new Vector3(-leftPadding, py + _options.ShadeOffsetY.Value - i * step * scale, 0);
                AddBand(pos, Mathf.CeilToInt(size.x) + px + leftPadding, Mathf.CeilToInt(size.y) + py * 2, alpha);
            }
        }
        private void AddBand(Vector3 position, int width, int height, float alpha)
        {
            if (_bandCursor == _bands.Count)
            {
                var widget = Child("TaskReminder.LineShade").AddComponent<UITexture>();
                widget.pivot = UIWidget.Pivot.TopLeft; widget.depth = _depth;
                _bands.Add(new Band { Widget = widget });
            }
            Band band = _bands[_bandCursor++]; int feather = _options.ShadeFeather.Value;
            if (band.Texture == null || band.Width != width || band.Height != height || band.Feather != feather)
            {
                var texture = new Texture2D(Math.Max(1, width), Math.Max(1, height), TextureFormat.RGBA32, false);
                texture.name = "TaskReminder.SoftLine"; texture.wrapMode = TextureWrapMode.Clamp; texture.filterMode = FilterMode.Bilinear;
                var pixels = new Color32[width * height];
                for (int y = 0; y < height; y++) for (int x = 0; x < width; x++)
                {
                    float dx = Math.Min(x + .5f, width - x - .5f), dy = Math.Min(y + .5f, height - y - .5f);
                    float a = Edge(dx / feather) * Edge(dy / feather);
                    pixels[y * width + x] = new Color32(0, 0, 0, (byte)Mathf.RoundToInt(255 * a));
                }
                texture.SetPixels32(pixels); texture.Apply(false, true);
                Texture2D old = band.Texture; band.Texture = texture; band.Widget.mainTexture = texture;
                band.Width = width; band.Height = height; band.Feather = feather;
                if (old != null) UnityEngine.Object.Destroy(old);
            }
            band.Widget.gameObject.SetActive(true); band.Widget.width = width; band.Widget.height = height;
            band.Widget.transform.localPosition = position;
            band.Widget.color = new Color(1, 1, 1, alpha * _options.ShadeAlpha.Value);
        }
        private static float Edge(float x) { x = Mathf.Clamp01(x); return x * x * (3 - 2 * x); }
        internal void Destroy() { foreach (Band band in _bands) if (band.Texture != null) UnityEngine.Object.Destroy(band.Texture); }
    }
}
