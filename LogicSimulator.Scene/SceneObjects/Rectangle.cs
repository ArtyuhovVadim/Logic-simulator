using LogicSimulator.Scene.SceneObjects.Base;
using SharpDX;
using SharpDX.Direct2D1;

namespace LogicSimulator.Scene.SceneObjects;

public class Rectangle : BaseSceneObject
{
    private Color4 _strokeColor = Color4.Black;
    private Color4 _fillColor = Color4.Black;
    private Vector2 _location = Vector2.Zero;
    private float _width = 1f;
    private float _height = 1f;

    public Vector2 Location
    {
        get => _location;
        set
        {
            _location = value;
            OnResourceChanged(RectangleGeometryResource);
        }
    }

    public float Width
    {
        get => _width;
        set
        {
            _width = value;
            OnResourceChanged(RectangleGeometryResource);
        }
    }

    public float Height
    {
        get => _height;
        set
        {
            _height = value;
            OnResourceChanged(RectangleGeometryResource);
        }
    }

    public float StrokeWidth { get; set; } = 1f;

    public Color4 StrokeColor
    {
        get => _strokeColor;
        set
        {
            _strokeColor = value;
            OnResourceChanged(StrokeBrushResource);
        }
    }

    public Color4 FillColor
    {
        get => _fillColor;
        set
        {
            _fillColor = value;
            OnResourceChanged(FillBrushResource);
        }
    }

    #region StrokeBrushResource

    public static readonly Resource StrokeBrushResource = Resource.Register<SolidColorBrush>(typeof(Rectangle), UpdateStrokeBrushResource);

    private static object UpdateStrokeBrushResource(ObjectRenderer renderer, ResourceDependentObject o) =>
        renderer.CreateSolidColorBrush(((Rectangle)o).StrokeColor);

    #endregion

    #region FillBrushResource

    public static readonly Resource FillBrushResource = Resource.Register<SolidColorBrush>(typeof(Rectangle), UpdateFillBrushResource);

    private static object UpdateFillBrushResource(ObjectRenderer renderer, ResourceDependentObject o) =>
        renderer.CreateSolidColorBrush(((Rectangle)o).FillColor);

    #endregion

    #region RectangleGeometryResource

    public static readonly Resource RectangleGeometryResource = Resource.Register<RectangleGeometry>(typeof(Rectangle), UpdateRectangleGeometryResource);

    private static object UpdateRectangleGeometryResource(ObjectRenderer renderer, ResourceDependentObject o)
    {
        var rect = (Rectangle)o;

        return renderer.CreateRectangleGeometry(new RectangleF(rect.Location.X, rect.Location.Y, rect.Width, rect.Height));
    }

    #endregion

    public override void Render(ObjectRenderer objectRenderer)
    {
        objectRenderer.Render(this);
    }
}