using LogicSimulator.Scene.SceneObjects.Base;
using SharpDX;
using SharpDX.Direct2D1;

namespace LogicSimulator.Scene.SceneObjects;

public class Rectangle : EditableSceneObject
{
    public static readonly Resource FillBrushResource = ResourceCache.Register((target, o) => new SolidColorBrush(target, ((Rectangle)o).FillColor));

    public static readonly Resource StrokeBrushResource = ResourceCache.Register((target, o) => new SolidColorBrush(target, ((Rectangle)o).StrokeColor));

    public static readonly Resource RectangleGeometryResource = ResourceCache.Register((target, o) =>
    {
        var rect = (Rectangle)o;

        return new RectangleGeometry(target.Factory, new RectangleF(rect.Location.X, rect.Location.Y, rect.Width, rect.Height));
    });

    private Color4 _fillColor = Color4.White;
    private Color4 _strokeColor = Color4.Black;
    private Vector2 _location = Vector2.Zero;
    private float _width;
    private float _height;
    private float _strokeThickness = 1f;
    private bool _isFilled = true;

    private Vector2 _startDragPosition = Vector2.Zero;
    private Vector2 _startDragLocation = Vector2.Zero;

    public Rectangle()
    {
        Nodes = new[]
        {
            new Node(() => Location, pos =>
            {
                Width += (Location - pos).X;
                Height += (Location - pos).Y;
                Location = pos;
            }),
            new Node(() => Location + new Vector2(Width, 0), pos =>
            {
                Width = pos.X - Location.X;
                Height = Location.Y + Height - pos.Y;
                Location = new Vector2(Location.X, pos.Y);
            }),
            new Node(() => Location + new Vector2(Width, Height), pos =>
            {
                var size = pos - Location;
                Width = size.X;
                Height = size.Y;
            }),
            new Node(() => Location + new Vector2(0, Height), pos =>
            {
                Width = Location.X + Width- pos.X;
                Height = pos.Y - Location.Y;
                Location = new Vector2(pos.X, Location.Y);
            })
        };
    }

    public override Node[] Nodes { get; }

    public Vector2 Location
    {
        get => _location;
        set
        {
            //TODO: Ненужные присваивания при одинаковых значениях
            _location = value;
            ResourceCache.RequestUpdate(this, RectangleGeometryResource);
        }
    }

    public float Width
    {
        get => _width;
        set
        {
            _width = value;
            ResourceCache.RequestUpdate(this, RectangleGeometryResource);
        }
    }

    public float Height
    {
        get => _height;
        set
        {
            _height = value;
            ResourceCache.RequestUpdate(this, RectangleGeometryResource);
        }
    }

    public Color4 FillColor
    {
        get => _fillColor;
        set
        {
            _fillColor = value;
            ResourceCache.RequestUpdate(this, FillBrushResource);
        }
    }

    public Color4 StrokeColor
    {
        get => _strokeColor;
        set
        {
            _strokeColor = value;
            ResourceCache.RequestUpdate(this, StrokeBrushResource);
        }
    }

    public float StrokeThickness
    {
        get => _strokeThickness;
        set
        {
            _strokeThickness = value;
            RenderNotifier.RequestRender(this);
        }
    }

    public bool IsFilled
    {
        get => _isFilled;
        set
        {
            _isFilled = value;
            RenderNotifier.RequestRender(this);
        }
    }

    public override void StartDrag(Vector2 pos)
    {
        IsDragging = true;

        _startDragPosition = pos;
        _startDragLocation = Location;
    }

    public override void Drag(Vector2 pos)
    {
        Location = _startDragLocation - _startDragPosition + pos;
    }

    public override void EndDrag()
    {
        IsDragging = false;
    }

    public override bool IsIntersectsPoint(Vector2 pos, Matrix3x2 matrix, float tolerance = 0.25f)
    {
        var geometry = ResourceCache.GetCached<RectangleGeometry>(this, RectangleGeometryResource);

        return IsFilled ? geometry.FillContainsPoint(pos, matrix, tolerance) :
                            geometry.StrokeContainsPoint(pos, StrokeThickness, null, matrix, tolerance);
    }

    public override GeometryRelation CompareWithRectangle(RectangleGeometry rectGeometry, Matrix3x2 matrix, float tolerance = 0.25f)
    {
        var geometry = ResourceCache.GetCached<RectangleGeometry>(this, RectangleGeometryResource);

        return geometry.Compare(rectGeometry, matrix, tolerance);
    }

    public override void Render(Scene2D scene, RenderTarget renderTarget)
    {
        var strokeBrush = ResourceCache.GetOrUpdate<SolidColorBrush>(this, StrokeBrushResource, renderTarget);
        var geometry = ResourceCache.GetOrUpdate<RectangleGeometry>(this, RectangleGeometryResource, renderTarget);

        if (IsFilled)
        {
            var fillBrush = ResourceCache.GetOrUpdate<SolidColorBrush>(this, FillBrushResource, renderTarget);

            renderTarget.FillGeometry(geometry, fillBrush);
        }

        renderTarget.DrawGeometry(geometry, strokeBrush, StrokeThickness / scene.Scale);
    }

    public override void RenderSelection(Scene2D scene, RenderTarget renderTarget, SolidColorBrush selectionBrush, StrokeStyle selectionStyle)
    {
        var geometry = ResourceCache.GetOrUpdate<RectangleGeometry>(this, RectangleGeometryResource, renderTarget);

        renderTarget.DrawGeometry(geometry, selectionBrush, 1f / scene.Scale, selectionStyle);
    }
}