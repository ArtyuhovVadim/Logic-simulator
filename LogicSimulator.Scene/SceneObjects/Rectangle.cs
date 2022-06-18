using LogicSimulator.Scene.SceneObjects.Base;
using SharpDX;
using SharpDX.Direct2D1;

namespace LogicSimulator.Scene.SceneObjects;

public class Rectangle : BaseSceneObject
{
    private Color4 _fillColor = Color4.White;
    private Color4 _strokeColor = Color4.Black;
    private Vector2 _location = Vector2.Zero;
    private float _width;
    private float _height;

    public Vector2 Location
    {
        get => _location;
        set
        {
            _location = value;
            RequireUpdate(RectangleGeometryResource);
        }
    }

    public float Width
    {
        get => _width;
        set
        {
            _width = value;
            RequireUpdate(RectangleGeometryResource);
        }
    }

    public float Height
    {
        get => _height;
        set
        {
            _height = value;
            RequireUpdate(RectangleGeometryResource);
        }
    }

    public float StrokeThickness { get; set; } = 1f;

    public Color4 FillColor
    {
        get => _fillColor;
        set
        {
            _fillColor = value;
            RequireUpdate(FillBrushResource);
        }
    }

    public Color4 StrokeColor
    {
        get => _strokeColor;
        set
        {
            _strokeColor = value;
            RequireUpdate(StrokeBrushResource);
        }
    }

    public static Resource FillBrushResource = Resource.Register<Rectangle, SolidColorBrush>(nameof(FillBrushResource), (target, o) =>
        new SolidColorBrush(target, ((Rectangle)o).FillColor));

    public static Resource StrokeBrushResource = Resource.Register<Rectangle, SolidColorBrush>(nameof(StrokeBrushResource), (target, o) =>
        new SolidColorBrush(target, ((Rectangle)o).StrokeColor));

    public static Resource RectangleGeometryResource = Resource.Register<Rectangle, RectangleGeometry>(nameof(RectangleGeometryResource), (target, o) =>
    {
        var rectangle = (Rectangle)o;

        return new RectangleGeometry(target.Factory, new RectangleF(rectangle.Location.X, rectangle.Location.Y, rectangle.Width, rectangle.Height));
    });

    public override void Render(ObjectRenderer renderer) => renderer.Render(this);
}