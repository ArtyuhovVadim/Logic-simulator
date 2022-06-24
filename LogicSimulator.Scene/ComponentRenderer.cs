using LogicSimulator.Scene.Components;
using SharpDX;
using SharpDX.Direct2D1;

namespace LogicSimulator.Scene;

public class ComponentRenderer
{
    private readonly RenderTarget _renderTarget;

    public ComponentRenderer(RenderTarget renderTarget) => _renderTarget = renderTarget;

    public void Render(GridRenderingComponent component)
    {
        var strokeWidth = component.LineThickness / _renderTarget.Transform.M11;

        var rect = new RectangleF(0, 0, component.Width, component.Height);

        var backgroundBrush = component.GetResourceValue<SolidColorBrush>(GridRenderingComponent.BackgroundResource, _renderTarget);
        var lineBrush = component.GetResourceValue<SolidColorBrush>(GridRenderingComponent.LineResource, _renderTarget);
        var boldLineBrush = component.GetResourceValue<SolidColorBrush>(GridRenderingComponent.BoldLineResource, _renderTarget);

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
}