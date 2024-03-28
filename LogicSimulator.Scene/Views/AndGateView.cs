using System.Windows;
using LogicSimulator.Core;
using LogicSimulator.Scene.DirectX;
using LogicSimulator.Scene.Views.Base;
using SharpDX;
using SharpDX.Direct2D1;

namespace LogicSimulator.Scene.Views;

public class AndGateView : BaseGateView
{
    #region OutputState

    public SignalType OutputState
    {
        get => (SignalType)GetValue(OutputStateProperty);
        set => SetValue(OutputStateProperty, value);
    }

    public static readonly DependencyProperty OutputStateProperty =
        DependencyProperty.Register(nameof(OutputState), typeof(SignalType), typeof(AndGateView), new PropertyMetadata(default(SignalType), DefaultPropertyChangedHandler));

    #endregion

    #region InputStates

    public IEnumerable<SignalType> InputStates
    {
        get => (IEnumerable<SignalType>)GetValue(InputStatesProperty);
        set => SetValue(InputStatesProperty, value);
    }

    public static readonly DependencyProperty InputStatesProperty =
        DependencyProperty.Register(nameof(InputStates), typeof(IEnumerable<SignalType>), typeof(AndGateView), new PropertyMetadata(Enumerable.Empty<SignalType>(), DefaultPropertyChangedHandler));

    #endregion

    public float Scale => 1f;

    public override RectangleF Bounds => new(0, 0, 50 * Scale, 40 * Scale);

    protected override void OnRender(Scene2D scene, D2DContext context)
    {
        var fillBrush = Cache.Get<SolidColorBrush>(this, FillBrushResource);
        var strokeBrush = Cache.Get<SolidColorBrush>(this, StrokeBrushResource);

        var strokeWidth = this.GetStrokeThickness(scene);

        var inputStates = InputStates.ToList();
        if (inputStates.Count == 0)
        {
            inputStates.Add(SignalType.Low);
            inputStates.Add(SignalType.Low);
        }

        var style = Cache.Get<StrokeStyle>(StrokeStyleResource);

        context.DrawingContext.DrawLine(new Vector2(Bounds.Width * 4f / 5f, Bounds.Height / 2f), new Vector2(Bounds.Width, Bounds.Height / 2f), GetSignalBrush(OutputState), strokeWidth, style);
        context.DrawingContext.DrawLine(new Vector2(0, Bounds.Height / 4f), new Vector2(Bounds.Width / 5f, Bounds.Height / 4f), GetSignalBrush(inputStates[0]), strokeWidth, style);
        context.DrawingContext.DrawLine(new Vector2(0, Bounds.Height * 3f / 4f), new Vector2(Bounds.Width / 5f, Bounds.Height * 3f / 4f), GetSignalBrush(inputStates[1]), strokeWidth, style);

        var sink = context.ResourceFactory.BeginPathGeometry();
        sink.BeginFigure(new Vector2(1f, 0.5f), FigureBegin.Filled);
        sink.AddLine(new Vector2(2.5f, 0.5f));
        sink.AddArc(new ArcSegment { Point = new Vector2(2.5f, 3.5f), ArcSize = ArcSize.Large, Size = new Size2F(1.5f, 1.5f), RotationAngle = (float)Math.PI, SweepDirection = SweepDirection.Clockwise });
        sink.AddLine(new Vector2(1f, 3.5f));
        sink.EndFigure(FigureEnd.Closed);
        using var path = context.ResourceFactory.EndPathGeometry();

        context.DrawingContext.PushAntialiasMode(AntialiasMode.PerPrimitive);
        context.DrawingContext.PushTransform(Matrix3x2.Scaling(Scale * 10, Scale * 10));
        context.DrawingContext.FillGeometry(path, fillBrush);
        context.DrawingContext.DrawGeometry(path, strokeBrush, strokeWidth / 10);
        context.DrawingContext.PopTransform();
        context.DrawingContext.PopAntialiasMode();
    }
}