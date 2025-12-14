using Typotrainer.Core.Models;

namespace Typotrainer.Views;

public class StatsGraphDrawable : IDrawable
{
    private readonly List<ExerciseSessionResult> _data;

    public StatsGraphDrawable(List<ExerciseSessionResult> data)
    {
        _data = data;
    }

    public void Draw(ICanvas canvas, RectF rect)
    {
        canvas.FillColor = Colors.White;
        canvas.FillRectangle(rect);

        if (_data == null || _data.Count == 0)
            return;

        float padding = 40;
        float width = rect.Width - padding * 2;
        float height = rect.Height - padding * 2;

        // Axes
        canvas.StrokeColor = Colors.Black;
        canvas.StrokeSize = 1;
        canvas.DrawLine(padding, rect.Height - padding, rect.Width - padding, rect.Height - padding);
        canvas.DrawLine(padding, padding, padding, rect.Height - padding);

        DrawLine(
            canvas,
            _data.Select(d => d.Wpm).ToList(),
            Colors.Blue,
            padding, rect, width, height);

        DrawLine(
            canvas,
            _data.Select(d => d.Accuracy).ToList(),
            Colors.Green,
            padding, rect, width, height);

        DrawLine(
            canvas,
            _data.Select(d => (double)d.Mistakes).ToList(),
            Colors.Red,
            padding, rect, width, height);

        DrawLine(
            canvas,
            _data.Select(d => d.TotalTime.TotalMinutes).ToList(),
            Colors.Orange,
            padding, rect, width, height);

        DrawLegend(canvas, rect);
    }

    private void DrawLine(
        ICanvas canvas,
        List<double> values,
        Color color,
        float padding,
        RectF rect,
        float width,
        float height)
    {
        if (values.Count < 2)
            return;

        double max = values.Max();
        if (max <= 0)
            max = 1;

        canvas.StrokeColor = color;
        canvas.StrokeSize = 2;

        for (int i = 0; i < values.Count - 1; i++)
        {
            float x1 = padding + (i / (float)(values.Count - 1)) * width;
            float y1 = rect.Height - padding - (float)(values[i] / max) * height;

            float x2 = padding + ((i + 1) / (float)(values.Count - 1)) * width;
            float y2 = rect.Height - padding - (float)(values[i + 1] / max) * height;

            canvas.DrawLine(x1, y1, x2, y2);
        }
    }

    private void DrawLegend(ICanvas canvas, RectF rect)
    {
        float startX = rect.Width - 160;
        float startY = 20;
        float lineHeight = 20;

        DrawLegendItem(canvas, startX, startY + lineHeight * 0, Colors.Blue, "WPM");
        DrawLegendItem(canvas, startX, startY + lineHeight * 1, Colors.Green, "Accuracy (%)");
        DrawLegendItem(canvas, startX, startY + lineHeight * 2, Colors.Red, "Mistakes");
        DrawLegendItem(canvas, startX, startY + lineHeight * 3, Colors.Orange, "Time (min)");
    }

    private void DrawLegendItem(ICanvas canvas, float x, float y, Color color, string text)
    {
        canvas.StrokeColor = color;
        canvas.StrokeSize = 3;
        canvas.DrawLine(x, y + 6, x + 20, y + 6);

        canvas.FontColor = Colors.Black;
        canvas.FontSize = 12;
        canvas.DrawString(text, x + 25, y, HorizontalAlignment.Left);
    }
}
