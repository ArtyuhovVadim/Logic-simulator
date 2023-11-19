using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using SharpDX.Mathematics.Interop;
using TextAntialiasMode = SharpDX.Direct2D1.TextAntialiasMode;

namespace LogicSimulator.Scene.DirectX;

public class D2DDrawingContext
{
    private readonly DirectXContext _context;
    private readonly Stack<Matrix3x2> _transforms = new();

    public int RenderedFramesCount { get; private set; }

    public Size2F DrawingSize => _context.D2DDeviceContext.Size;

    public Matrix3x2 Transform
    {
        get => _context.D2DDeviceContext.Transform;
        set => _context.D2DDeviceContext.Transform = value;
    }

    public Vector2 Translation
    {
        get => new(_context.D2DDeviceContext.Transform.M31, _context.D2DDeviceContext.Transform.M32);
        set => _context.D2DDeviceContext.Transform = _context.D2DDeviceContext.Transform with { M31 = value.X, M32 = value.Y };
    }

    public float Scale
    {
        get => _context.D2DDeviceContext.Transform.M11;
        set => _context.D2DDeviceContext.Transform = _context.D2DDeviceContext.Transform with { M11 = value, M22 = value };
    }

    public AntialiasMode AntialiasMode
    {
        get => _context.D2DDeviceContext.AntialiasMode;
        set => _context.D2DDeviceContext.AntialiasMode = value;
    }

    public TextAntialiasMode TextAntialiasMode
    {
        get => _context.D2DDeviceContext.TextAntialiasMode;
        set => _context.D2DDeviceContext.TextAntialiasMode = value;
    }

    public D2DDrawingContext(DirectXContext context) => _context = context;

    public void BeginDraw() => _context.D2DDeviceContext.BeginDraw();

    public void EndDraw()
    {
        _context.D2DDeviceContext.EndDraw();
        RenderedFramesCount++;
    }

    public void PushTransform(Matrix3x2 transform)
    {
        _transforms.Push(transform);
        Transform = _transforms.Peek() * Transform;
    }

    public void PopTransform()
    {
        Transform = Matrix3x2.Invert(_transforms.Peek()) * Transform;
        _transforms.Pop();
    }

    public void Clear(Color4 color) => _context.D2DDeviceContext.Clear(color);

    public void DrawRectangle(RawRectangleF rect, Brush brush, float strokeWidth)
    {
        _context.D2DDeviceContext.DrawRectangle(rect, brush, strokeWidth);
        RenderDebugger.DrawRectangleCalled();
    }

    public void DrawRectangle(RawRectangleF rect, Brush brush, float strokeWidth, StrokeStyle style)
    {
        _context.D2DDeviceContext.DrawRectangle(rect, brush, strokeWidth, style);
        RenderDebugger.DrawRectangleCalled();
    }

    public void FillRectangle(RawRectangleF rect, Brush brush)
    {
        _context.D2DDeviceContext.FillRectangle(rect, brush);
        RenderDebugger.FillRectangleCalled();
    }

    public void DrawLine(Vector2 p1, Vector2 p2, Brush brush, float strokeWidth)
    {
        _context.D2DDeviceContext.DrawLine(p1, p2, brush, strokeWidth);
        RenderDebugger.DrawLineCalled();
    }

    public void DrawLine(Vector2 p1, Vector2 p2, Brush brush, float strokeWidth, StrokeStyle style)
    {
        _context.D2DDeviceContext.DrawLine(p1, p2, brush, strokeWidth, style);
        RenderDebugger.DrawLineCalled();
    }

    public void DrawEllipse(Ellipse ellipse, Brush brush, float strokeWidth)
    {
        _context.D2DDeviceContext.DrawEllipse(ellipse, brush, strokeWidth);
        RenderDebugger.DrawEllipseCalled();
    }

    public void DrawEllipse(Ellipse ellipse, Brush brush, float strokeWidth, StrokeStyle style)
    {
        _context.D2DDeviceContext.DrawEllipse(ellipse, brush, strokeWidth, style);
        RenderDebugger.DrawEllipseCalled();
    }

    public void FillEllipse(Ellipse ellipse, Brush brush)
    {
        _context.D2DDeviceContext.FillEllipse(ellipse, brush);
        RenderDebugger.FillEllipseCalled();
    }

    public void DrawGeometry(Geometry geometry, Brush brush, float strokeWidth)
    {
        _context.D2DDeviceContext.DrawGeometry(geometry, brush, strokeWidth);
        RenderDebugger.DrawGeometryCalled();
    }

    public void DrawGeometry(Geometry geometry, Brush brush, float strokeWidth, StrokeStyle style)
    {
        _context.D2DDeviceContext.DrawGeometry(geometry, brush, strokeWidth, style);
        RenderDebugger.DrawGeometryCalled();
    }

    public void FillGeometry(Geometry geometry, Brush brush)
    {
        _context.D2DDeviceContext.FillGeometry(geometry, brush);
        RenderDebugger.FillGeometryCalled();
    }

    public void DrawText(string text, TextFormat format, RectangleF rect, Brush brush, DrawTextOptions options)
    {
        _context.D2DDeviceContext.DrawText(text, format, rect, brush, options);
        RenderDebugger.DrawTextCalled();
    }

    public void DrawTextLayout(Vector2 pos, TextLayout layout, Brush brush, DrawTextOptions options)
    {
        _context.D2DDeviceContext.DrawTextLayout(pos, layout, brush, options);
        RenderDebugger.DrawTextLayoutCalled();
    }

    public void DrawBitmap(Bitmap bitmap, RectangleF rect, float opacity, BitmapInterpolationMode interpolationMode)
    {
        _context.D2DDeviceContext.DrawBitmap(bitmap, rect, opacity, interpolationMode);
        RenderDebugger.DrawBitmapCalled();
    }

    public void DrawTriangles(IEnumerable<Triangle> triangles, Brush brush, float strokeWidth)
    {
        foreach (var triangle in triangles)
        {
            _context.D2DDeviceContext.DrawLine(triangle.Point1, triangle.Point2, brush, strokeWidth);
            _context.D2DDeviceContext.DrawLine(triangle.Point2, triangle.Point3, brush, strokeWidth);
            _context.D2DDeviceContext.DrawLine(triangle.Point3, triangle.Point1, brush, strokeWidth);
        }
        RenderDebugger.DrawTrianglesCalled();
    }
}