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
    private bool _isFilled = true;
    private float _strokeThickness = 1f;

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

    public float StrokeThickness
    {
        get => _strokeThickness;
        set
        {
            _strokeThickness = value;
            RequireRender();
        }
    }

    public bool IsFilled
    {
        get => _isFilled;
        set
        {
            _isFilled = value;
            RequireRender();
        }
    }

    public static readonly Resource FillBrushResource = Resource.Register<Rectangle, SolidColorBrush>(nameof(FillBrushResource), (target, o) =>
        new SolidColorBrush(target, ((Rectangle)o).FillColor));

    public static readonly Resource StrokeBrushResource = Resource.Register<Rectangle, SolidColorBrush>(nameof(StrokeBrushResource), (target, o) =>
        new SolidColorBrush(target, ((Rectangle)o).StrokeColor));

    public static readonly Resource RectangleGeometryResource = Resource.Register<Rectangle, RectangleGeometry>(nameof(RectangleGeometryResource), (target, o) =>
    {
        var rectangle = (Rectangle)o;

        return new RectangleGeometry(target.Factory, new RectangleF(rectangle.Location.X, rectangle.Location.Y, rectangle.Width, rectangle.Height));
    });

    public override bool IsIntersectsPoint(Vector2 pos, Matrix3x2 matrix, float tolerance = 0.25f)
    {
        var geometry = GetCashedResourceValue<RectangleGeometry>(RectangleGeometryResource);

        return IsFilled ? geometry.FillContainsPoint(pos, matrix, tolerance) :
                            geometry.StrokeContainsPoint(pos, StrokeThickness, null, matrix, tolerance);
    }

    public override void Render(ObjectRenderer renderer) => renderer.Render(this);
}