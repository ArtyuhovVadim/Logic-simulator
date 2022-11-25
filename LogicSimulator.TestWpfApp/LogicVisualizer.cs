using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using LogicSimulator.Core.LogicComponents.Base;

namespace LogicSimulator.TestWpfApp;

public class LogicVisualizer : FrameworkElement
{
    #region Components

    public ObservableCollection<LogicComponent> Components
    {
        get => (ObservableCollection<LogicComponent>)GetValue(ComponentsProperty);
        set => SetValue(ComponentsProperty, value);
    }

    public static readonly DependencyProperty ComponentsProperty =
        DependencyProperty.Register(nameof(Components), typeof(ObservableCollection<LogicComponent>), typeof(LogicVisualizer), new FrameworkPropertyMetadata(default(ObservableCollection<LogicComponent>), FrameworkPropertyMetadataOptions.AffectsRender));

    #endregion

    private readonly Brush _nodeBrush = Brushes.Black;
    private readonly Brush _wireBrush = Brushes.Black;
    private readonly Brush _gateBrush = Brushes.Black;
    private readonly Brush _foregroundBrush = Brushes.White;

    private readonly Typeface _typeface = new(new FontFamily("Calibry"), FontStyles.Normal, FontWeights.Normal, FontStretches.Normal);

    protected override void OnRender(DrawingContext dc)
    {
        DrawNode(dc, new Point(100, 100));

        DrawWire(dc, new Point(300, 300), new Point(500, 300));

        DrawGate(dc, new Point(500, 500), "NOR");
    }

    private void DrawNode(DrawingContext dc, Point center)
    {
        dc.DrawEllipse(_nodeBrush, null, center, 5, 5);
    }

    private void DrawWire(DrawingContext dc, Point p1, Point p2)
    {
        dc.DrawLine(new Pen(_wireBrush, 2), p1, p2);
    }

    private void DrawGate(DrawingContext dc, Point center, string text)
    {
        var width = 100f;
        var height = 100f;

        var formattedText = new FormattedText("text", CultureInfo.InvariantCulture, FlowDirection.LeftToRight, _typeface,
            20, _foregroundBrush, 120f);

        dc.DrawRectangle(_gateBrush, null, new Rect(center.X - width / 2, center.Y - width / 2, width, height));
        dc.DrawText(formattedText, new Point(center.X - formattedText.Width / 2, center.Y - formattedText.Height / 2));
    }
}