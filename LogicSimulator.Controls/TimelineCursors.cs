using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace LogicSimulator.Controls;

public class TimelineCursors : FrameworkElement
{
    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);
        InvalidateVisual();
    }

    protected override void OnRender(DrawingContext drawingContext)
    {
        var mousePosition = Mouse.GetPosition(this);
        var size = new Size(ActualWidth, ActualHeight);

        var pen = new Pen(new SolidColorBrush(Color.FromRgb(244, 75, 86)), 1);

        drawingContext.DrawRectangle(Brushes.Transparent, null, new Rect(size));
        drawingContext.DrawLine(pen, mousePosition with { Y = 0 }, mousePosition with { Y = size.Height });

        VisualXSnappingGuidelines = new DoubleCollection([mousePosition.X + 0.5, mousePosition.X - 0.5]);

        base.OnRender(drawingContext);
    }
}