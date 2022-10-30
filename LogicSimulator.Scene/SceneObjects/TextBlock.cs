using LogicSimulator.Scene.SceneObjects.Base;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;

namespace LogicSimulator.Scene.SceneObjects;

public class TextBlock : BaseSceneObject
{
    private static readonly Resource TextLayoutResource = ResourceCache.Register((scene, obj) =>
    {
        var textFormat = ResourceCache.GetOrUpdate<TextFormat>(obj, TextFormatResource, scene);

        return scene.ResourceFactory.CreateTextLayout(((TextBlock)obj).Text, textFormat);
    });

    private static readonly Resource TextFormatResource = ResourceCache.Register((scene, obj) =>
    {
        var textBlock = (TextBlock)obj;

        return scene.ResourceFactory.CreateTextFormat(
               textBlock.FontName,
               textBlock.IsBold ? FontWeight.Bold : FontWeight.Normal,
               textBlock.IsItalic ? FontStyle.Italic : FontStyle.Normal,
               FontStretch.Normal,
               textBlock.FontSize);
    });

    private static readonly Resource TextGeometryResource = ResourceCache.Register((scene, obj) =>
    {
        var textLayout = ResourceCache.GetOrUpdate<TextLayout>(obj, TextLayoutResource, scene);

        var metrics = textLayout.Metrics;

        return scene.ResourceFactory.CreateRectangleGeometry(new RectangleF { Location = ((TextBlock)obj).Location, Width = metrics.Width, Height = metrics.Height });
    });

    private static readonly Resource TextBrushResource = ResourceCache.Register((scene, obj) =>
        scene.ResourceFactory.CreateSolidColorBrush(((TextBlock)obj).TextColor));

    private Vector2 _location = Vector2.Zero;
    private string _text = string.Empty;
    private string _fontName;
    private float _fontSize = 12f;
    private Color4 _textColor = Color4.Black;

    private bool _isBold;
    private bool _isItalic;
    private bool _isUnderlined;
    private bool _isCross;

    private Vector2 _startDragPosition;
    private Vector2 _startDragLocation;

    [Editable]
    public Vector2 Location
    {
        get => _location;
        set => SetAndUpdateResource(ref _location, value, TextGeometryResource);
    }

    [Editable]
    public string Text
    {
        get => _text;
        set
        {
            if (SetAndUpdateResource(ref _text, value, TextLayoutResource))
            {
                ResourceCache.RequestUpdate(this, TextGeometryResource);
            }
        }
    }

    [Editable]
    public string FontName
    {
        get => _fontName;
        set
        {
            if (SetAndUpdateResource(ref _fontName, value, TextFormatResource))
            {
                ResourceCache.RequestUpdate(this, TextLayoutResource);
                ResourceCache.RequestUpdate(this, TextGeometryResource);
            }
        }
    }

    [Editable]
    public float FontSize
    {
        get => _fontSize;
        set
        {
            if (SetAndUpdateResource(ref _fontSize, value, TextFormatResource))
            {
                ResourceCache.RequestUpdate(this, TextLayoutResource);
                ResourceCache.RequestUpdate(this, TextGeometryResource);
            }
        }
    }

    [Editable]
    public bool IsBold
    {
        get => _isBold;
        set
        {
            if (SetAndUpdateResource(ref _isBold, value, TextFormatResource))
            {
                ResourceCache.RequestUpdate(this, TextLayoutResource);
                ResourceCache.RequestUpdate(this, TextGeometryResource);
            }
        }
    }

    [Editable]
    public bool IsItalic
    {
        get => _isItalic;
        set
        {
            if (SetAndUpdateResource(ref _isItalic, value, TextFormatResource))
            {
                ResourceCache.RequestUpdate(this, TextLayoutResource);
                ResourceCache.RequestUpdate(this, TextGeometryResource);
            }
        }
    }

    [Editable]
    public bool IsUnderlined
    {
        get => _isUnderlined;
        set => SetAndRequestRender(ref _isUnderlined, value);
    }

    [Editable]
    public bool IsCross
    {
        get => _isCross;
        set => SetAndRequestRender(ref _isCross, value);
    }

    [Editable]
    public Color4 TextColor
    {
        get => _textColor;
        set => SetAndUpdateResource(ref _textColor, value, TextBrushResource);
    }

    protected override void OnInitialize(Scene2D scene)
    {
        InitializeResource(TextBrushResource);
        InitializeResource(TextFormatResource);
        InitializeResource(TextLayoutResource);
        InitializeResource(TextGeometryResource);
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
        var geometry = GetOrUpdateResource<RectangleGeometry>(TextGeometryResource);

        return geometry.FillContainsPoint(pos, matrix, tolerance);
    }

    public override GeometryRelation CompareWithRectangle(RectangleGeometry rectGeometry, Matrix3x2 matrix, float tolerance = 0.25f)
    {
        var geometry = GetOrUpdateResource<RectangleGeometry>(TextGeometryResource);

        return geometry.Compare(rectGeometry, matrix, tolerance);
    }

    protected override void OnRender(Scene2D scene, RenderTarget renderTarget)
    {
        var textLayout = ResourceCache.GetOrUpdate<TextLayout>(this, TextLayoutResource, scene);
        var textBrush = ResourceCache.GetOrUpdate<SolidColorBrush>(this, TextBrushResource, scene);

        textLayout.SetUnderline(IsUnderlined, new TextRange(0, Text.Length));
        textLayout.SetStrikethrough(IsCross, new TextRange(0, Text.Length));

        renderTarget.DrawTextLayout(Location, textLayout, textBrush, DrawTextOptions.None);
    }

    public override void RenderSelection(Scene2D scene, RenderTarget renderTarget, SolidColorBrush selectionBrush, StrokeStyle selectionStyle)
    {
        var geometry = ResourceCache.GetOrUpdate<RectangleGeometry>(this, TextGeometryResource, scene);

        renderTarget.DrawGeometry(geometry, selectionBrush, 1f / scene.Scale, selectionStyle);
    }
}