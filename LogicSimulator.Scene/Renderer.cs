using LogicSimulator.Scene.Components;
using SharpDX;
using SharpDX.Direct2D1;
using Rectangle = LogicSimulator.Scene.SceneObjects.Rectangle;

namespace LogicSimulator.Scene;

public class Renderer : ResourceDependentObject
{
    public static readonly Resource SelectionBrushResource = Resource.Register<Renderer, SolidColorBrush>(nameof(SelectionBrushResource), (target, o) =>
        new SolidColorBrush(target, ((Renderer)o).SelectionColor));

    public static readonly Resource SelectionStyleResource = Resource.Register<Renderer, StrokeStyle>(nameof(SelectionStyleResource), (target, _) =>
    {
        var properties = new StrokeStyleProperties()
        {
            DashStyle = DashStyle.Custom,
            DashCap = CapStyle.Flat
        };

        return new StrokeStyle(target.Factory, properties, new[] { 2f, 2f });
    });

    private readonly RenderTarget _renderTarget;

    private Color4 _selectionColor = new(0, 1, 0, 1);

    public Renderer(RenderTarget renderTarget) => _renderTarget = renderTarget;

    public Color4 SelectionColor
    {
        get => _selectionColor;
        set
        {
            _selectionColor = value;
            RequireUpdate(SelectionBrushResource);
        }
    }

    public void Render(Rectangle rectangle)
    {
        var strokeBrush = rectangle.GetResourceValue<SolidColorBrush>(Rectangle.StrokeBrushResource, _renderTarget);
        var geometry = rectangle.GetResourceValue<RectangleGeometry>(Rectangle.RectangleGeometryResource, _renderTarget);

        if (rectangle.IsFilled)
        {
            var fillBrush = rectangle.GetResourceValue<SolidColorBrush>(Rectangle.FillBrushResource, _renderTarget);
            _renderTarget.FillGeometry(geometry, fillBrush);
        }

        _renderTarget.DrawGeometry(geometry, strokeBrush, rectangle.StrokeThickness / _renderTarget.Transform.M11);
    }

    public void Render(GridComponent component)
    {
        var strokeWidth = component.LineThickness / _renderTarget.Transform.M11;

        var rect = new RectangleF(0, 0, component.Width, component.Height);

        var backgroundBrush = component.GetResourceValue<SolidColorBrush>(GridComponent.BackgroundBrushResource, _renderTarget);
        var lineBrush = component.GetResourceValue<SolidColorBrush>(GridComponent.LineBrushResource, _renderTarget);
        var boldLineBrush = component.GetResourceValue<SolidColorBrush>(GridComponent.BoldLineBrushResource, _renderTarget);

        _renderTarget.FillRectangle(rect, backgroundBrush);

        _renderTarget.DrawRectangle(rect, lineBrush, strokeWidth);

        for (var i = 0; i <= rect.Height / component.CellSize; i++)
        {
            _renderTarget.DrawLine(
                new Vector2(i * component.CellSize + rect.Left, rect.Top),
                new Vector2(i * component.CellSize + rect.Left, rect.Bottom),
                i % component.BoldLineStep == 0 ? boldLineBrush : lineBrush,
                strokeWidth);
        }

        for (var i = 0; i <= rect.Width / component.CellSize; i++)
        {
            _renderTarget.DrawLine(
                new Vector2(rect.Left, i * component.CellSize + rect.Top),
                new Vector2(rect.Right, i * component.CellSize + rect.Top),
                i % component.BoldLineStep == 0 ? boldLineBrush : lineBrush,
                strokeWidth);
        }
    }

    public void RenderSelection(Rectangle rectangle)
    {
        var geometry = rectangle.GetResourceValue<RectangleGeometry>(Rectangle.RectangleGeometryResource, _renderTarget);
        var brush = GetResourceValue<SolidColorBrush>(SelectionBrushResource, _renderTarget);
        var style = GetResourceValue<StrokeStyle>(SelectionStyleResource, _renderTarget);

        _renderTarget.DrawGeometry(geometry, brush, 1f / _renderTarget.Transform.M11, style);
    }
}