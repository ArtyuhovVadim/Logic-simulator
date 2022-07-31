using System.Linq;
using LogicSimulator.Scene.Components;
using LogicSimulator.Scene.ExtensionMethods;
using LogicSimulator.Scene.SceneObjects;
using LogicSimulator.Scene.SceneObjects.Base;
using SharpDX;
using SharpDX.Direct2D1;
using Ellipse = LogicSimulator.Scene.SceneObjects.Ellipse;
using Rectangle = LogicSimulator.Scene.SceneObjects.Rectangle;

namespace LogicSimulator.Scene;

public class Renderer : ResourceDependentObject
{
    public static readonly Resource SelectionBrushResource = Resource.Register<Renderer, SolidColorBrush>(nameof(SelectionBrushResource),
        (target, o) => new SolidColorBrush(target, ((Renderer)o).SelectionColor));

    public static readonly Resource SelectionStyleResource = Resource.Register<Renderer, StrokeStyle>(nameof(SelectionStyleResource),
        (target, _) =>
        {
            var properties = new StrokeStyleProperties
            {
                DashStyle = DashStyle.Custom,
                DashCap = CapStyle.Flat
            };

            return new StrokeStyle(target.Factory, properties, new[] { 2f, 2f });
        });

    private Color4 _selectionColor = new(0, 1, 0, 1);

    private readonly RenderTarget _renderTarget;

    public Renderer(RenderTarget renderTarget) => _renderTarget = renderTarget;

    //TODO: Перенести в SelectionRenderingComponent
    public Color4 SelectionColor
    {
        get => _selectionColor;
        set
        {
            _selectionColor = value;
            RequireUpdate(SelectionBrushResource);
        }
    }

    public void Render(Scene2D scene, Rectangle rectangle)
    {
        var strokeBrush = rectangle.GetResourceValue<SolidColorBrush>(Rectangle.StrokeBrushResource, _renderTarget);
        var geometry = rectangle.GetResourceValue<RectangleGeometry>(Rectangle.RectangleGeometryResource, _renderTarget);

        if (rectangle.IsFilled)
        {
            var fillBrush = rectangle.GetResourceValue<SolidColorBrush>(Rectangle.FillBrushResource, _renderTarget);
            _renderTarget.FillGeometry(geometry, fillBrush);
        }

        _renderTarget.DrawGeometry(geometry, strokeBrush, rectangle.StrokeThickness / scene.Scale);
    }

    public void Render(Scene2D scene, Ellipse ellipse)
    {
        var strokeBrush = ellipse.GetResourceValue<SolidColorBrush>(Ellipse.StrokeBrushResource, _renderTarget);
        var geometry = ellipse.GetResourceValue<EllipseGeometry>(Ellipse.EllipseGeometryResource, _renderTarget);

        if (ellipse.IsFilled)
        {
            var fillBrush = ellipse.GetResourceValue<SolidColorBrush>(Ellipse.FillBrushResource, _renderTarget);
            _renderTarget.FillGeometry(geometry, fillBrush);
        }

        _renderTarget.DrawGeometry(geometry, strokeBrush, ellipse.StrokeThickness / scene.Scale);
    }

    public void RenderSelection(Scene2D scene, Rectangle rectangle)
    {
        var geometry = rectangle.GetResourceValue<RectangleGeometry>(Rectangle.RectangleGeometryResource, _renderTarget);
        var brush = GetResourceValue<SolidColorBrush>(SelectionBrushResource, _renderTarget);
        var style = GetResourceValue<StrokeStyle>(SelectionStyleResource, _renderTarget);

        _renderTarget.DrawGeometry(geometry, brush, 1f / scene.Scale, style);
    }

    public void RenderSelection(Scene2D scene, Ellipse ellipse)
    {
        var geometry = ellipse.GetResourceValue<EllipseGeometry>(Ellipse.EllipseGeometryResource, _renderTarget);
        var brush = GetResourceValue<SolidColorBrush>(SelectionBrushResource, _renderTarget);
        var style = GetResourceValue<StrokeStyle>(SelectionStyleResource, _renderTarget);

        _renderTarget.DrawGeometry(geometry, brush, 1f / scene.Scale, style);
        _renderTarget.DrawLine(ellipse.Center, ellipse.Center + new Vector2(ellipse.RadiusX, 0), brush, 1f / scene.Scale, style);
        _renderTarget.DrawLine(ellipse.Center, ellipse.Center + new Vector2(0, -ellipse.RadiusY), brush, 1f / scene.Scale, style);
    }

    public void Render(Scene2D scene, GridRenderingComponent component)
    {
        var strokeWidth = component.LineThickness / scene.Scale;

        var rect = new RectangleF(0, 0, component.Width, component.Height);

        var backgroundBrush = component.GetResourceValue<SolidColorBrush>(GridRenderingComponent.BackgroundBrushResource, _renderTarget);
        var lineBrush = component.GetResourceValue<SolidColorBrush>(GridRenderingComponent.LineBrushResource, _renderTarget);
        var boldLineBrush = component.GetResourceValue<SolidColorBrush>(GridRenderingComponent.BoldLineBrushResource, _renderTarget);

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

    public void Render(Scene2D scene, SceneObjectsRenderingComponent component)
    {
        foreach (var sceneObject in scene.Objects)
        {
            sceneObject.Render(scene, this);
        }
    }

    //TODO: Ввести SelectionGeometry или что-то подобное
    public void Render(Scene2D scene, SelectionRenderingComponent component)
    {
        foreach (var sceneObject in scene.Objects)
        {
            if (sceneObject.IsSelected)
            {
                sceneObject.RenderSelection(scene, this);
            }
        }
    }

    public void Render(Scene2D scene, NodeRenderingComponent component)
    {
        var strokeColor = component.GetResourceValue<SolidColorBrush>(NodeRenderingComponent.StrokeBrushResource, _renderTarget);
        var selectedColor = component.GetResourceValue<SolidColorBrush>(NodeRenderingComponent.SelectBrushResource, _renderTarget);
        var unselectedColor = component.GetResourceValue<SolidColorBrush>(NodeRenderingComponent.UnselectBrushResource, _renderTarget);

        var size = Node.NodeSize / scene.Scale;

        foreach (var sceneObject in scene.Objects.OfType<EditableSceneObject>().Where(x => x.IsSelected))
        {
            foreach (var node in sceneObject.Nodes)
            {
                var rect = node.Location.RectangleRelativePointAsCenter(size);

                _renderTarget.FillRectangle(rect, node.IsSelected ? selectedColor : unselectedColor);
                _renderTarget.DrawRectangle(rect, strokeColor, 1f / scene.Scale);
            }
        }
    }

    public void Render(Scene2D scene, SelectionRectangleRenderingComponent component)
    {
        var brush = component.GetResourceValue<SolidColorBrush>(
            component.IsSecant ? SelectionRectangleRenderingComponent.SecantBrushResource : SelectionRectangleRenderingComponent.NormalBrushResource,
            _renderTarget);

        var location = component.StartPosition;
        var size = component.EndPosition - component.StartPosition;

        _renderTarget.DrawRectangle(new RectangleF { Location = location, Width = size.X, Height = size.Y }, brush, 1f / scene.Scale);
    }
}