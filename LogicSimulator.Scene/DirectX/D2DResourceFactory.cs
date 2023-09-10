using System.Diagnostics.CodeAnalysis;
using System.IO;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using SharpDX.Mathematics.Interop;
using SharpDX.WIC;
using Bitmap = SharpDX.Direct2D1.Bitmap;
using PixelFormat = SharpDX.WIC.PixelFormat;

namespace LogicSimulator.Scene.DirectX;

public class D2DResourceFactory : DisposableObject
{
    private readonly DirectXContext _context;

    private PathGeometry? _tempPathGeometry;
    private GeometrySink? _tempGeometrySink;

    private readonly FakeTessellationSink _fakeTessellationSink = new();

    public D2DResourceFactory(DirectXContext context)
    {
        _context = context;
    }

    public SolidColorBrush CreateSolidColorBrush(in Color4 color)
    {
        return new SolidColorBrush(_context.D2DDeviceContext, color);
    }

    public LinearGradientBrush CreateLinearGradientBrush(in LinearGradientBrushProperties properties, in GradientStop[] gradientStops)
    {
        var gradientStopCollection = new GradientStopCollection(_context.D2DDeviceContext, gradientStops);

        var linearGradientBrush = new LinearGradientBrush(_context.D2DDeviceContext, properties, gradientStopCollection);

        gradientStopCollection.Dispose();

        return linearGradientBrush;
    }

    public RectangleGeometry CreateRectangleGeometry(in RectangleF rectangle)
    {
        return new RectangleGeometry(_context.D2DFactory, rectangle);
    }

    public RectangleGeometry CreateRectangleGeometry(float width, float height)
    {
        return new RectangleGeometry(_context.D2DFactory, new RawRectangleF(0, 0, width, height));
    }

    public RoundedRectangleGeometry CreateRoundedRectangleGeometry(in RoundedRectangle roundedRectangle)
    {
        return new RoundedRectangleGeometry(_context.D2DFactory, roundedRectangle);
    }

    public EllipseGeometry CreateEllipseGeometry(in Ellipse ellipse)
    {
        return new EllipseGeometry(_context.D2DFactory, ellipse);
    }

    public PathGeometry CreatePolylineGeometry(Vector2 location, List<Vector2> vertices)
    {
        var path = new PathGeometry(_context.D2DFactory);

        var sink = path.Open();
        sink.BeginFigure(location, FigureBegin.Hollow);

        for (var i = 0; i < vertices.Count; i++)
        {
            sink.AddLine(vertices[i]);
        }

        sink.EndFigure(FigureEnd.Open);
        sink.Close();
        sink.Dispose();

        return path;
    }

    public PathGeometry CreateBezierCurveGeometry(in Vector2 p0, in Vector2 p1, in Vector2 p2, in Vector2 p3)
    {
        var pathGeometry = new PathGeometry(_context.D2DFactory);
        var sink = pathGeometry.Open();

        sink.BeginFigure(p0, FigureBegin.Hollow);
        sink.AddBezier(new BezierSegment { Point1 = p1, Point2 = p2, Point3 = p3 });
        sink.EndFigure(FigureEnd.Open);

        sink.Close();
        sink.Dispose();

        return pathGeometry;
    }

    public Geometry CreateArcGeometry(Vector2 center, float radiusX, float radiusY, float startAngle, float endAngle)
    {
        var startAnglePos = MathHelper.GetPositionFromAngle(center, radiusX, radiusY, startAngle);
        var endAnglePos = MathHelper.GetPositionFromAngle(center, radiusX, radiusY, endAngle);

        if (Math.Abs(startAnglePos.X - endAnglePos.X) < 0.0001f && Math.Abs(startAnglePos.Y - endAnglePos.Y) < 0.0001f)
            return CreateEllipseGeometry(new Ellipse(center, radiusX, radiusY));

        var pathGeometry = new PathGeometry(_context.D2DFactory);
        var sink = pathGeometry.Open();
        sink.BeginFigure(startAnglePos, FigureBegin.Hollow);

        sink.AddArc(new ArcSegment
        {
            ArcSize = MathHelper.NormalizeAngle(startAngle - endAngle) < 180f ? ArcSize.Small : ArcSize.Large,
            Point = endAnglePos,
            RotationAngle = 0f,
            Size = new Size2F(radiusX, radiusY),
            SweepDirection = SweepDirection.CounterClockwise
        });

        sink.EndFigure(FigureEnd.Open);
        sink.Close();
        sink.Dispose();

        return pathGeometry;
    }

    public Bitmap? CreateBitmap(string path)
    {
        if (string.IsNullOrWhiteSpace(path)) return null;

        if (!File.Exists(path)) return null;

        using var imagingFactory = new ImagingFactory();
        using var bitmapDecoder = new BitmapDecoder(imagingFactory, path, DecodeOptions.CacheOnLoad);
        using var frame = bitmapDecoder.GetFrame(0);
        using var converter = new FormatConverter(imagingFactory);

        converter.Initialize(frame, PixelFormat.Format32bppPRGBA);

        return Bitmap.FromWicBitmap(_context.D2DDeviceContext, converter);
    }

    public StrokeStyle CreateStrokeStyle(in StrokeStyleProperties properties, in float[] dashes)
    {
        return new StrokeStyle(_context.D2DFactory, properties, dashes);
    }

    public StrokeStyle CreateStrokeStyle(in StrokeStyleProperties properties)
    {
        return new StrokeStyle(_context.D2DFactory, properties);
    }

    public TextFormat CreateTextFormat(in string fontFamily, in FontWeight fontWeight, in FontStyle fontStyle, in FontStretch fontStretch, in float fontSize)
    {
        return new TextFormat(_context.D2DTextFactory, fontFamily, fontWeight, fontStyle, fontStretch, fontSize);
    }

    public TextFormat CreateTextFormat(in string fontFamily, in float fontSize)
    {
        return new TextFormat(_context.D2DTextFactory, fontFamily, FontWeight.Normal, FontStyle.Normal, FontStretch.Normal, fontSize);
    }

    public TextLayout CreateTextLayout(in string text, TextFormat textFormat)
    {
        return new TextLayout(_context.D2DTextFactory, text, textFormat, float.MaxValue, float.MaxValue);
    }

    public GeometrySink BeginPathGeometry()
    {
        _tempPathGeometry = new PathGeometry(_context.D2DFactory);
        _tempGeometrySink = _tempPathGeometry.Open();

        return _tempGeometrySink;
    }

    public PathGeometry EndPathGeometry()
    {
        if (_tempGeometrySink is null || _tempPathGeometry is null)
            throw new ApplicationException("Call BeginPathGeometry before EndPathGeometry.");

        _tempGeometrySink.Close();
        _tempGeometrySink.Dispose();

        var tmp = _tempPathGeometry;

        _tempGeometrySink = null;
        _tempPathGeometry = null;

        return tmp;
    }

    public IEnumerable<Triangle> CreateTriangles(Geometry geometry) => CreateTriangles(geometry, Matrix3x2.Identity);

    public IEnumerable<Triangle> CreateTriangles(Geometry geometry, Matrix3x2 transform, float flatteningTolerance = 0.25f)
    {
        _fakeTessellationSink.Reset();
        geometry.Tessellate(transform, flatteningTolerance, _fakeTessellationSink);
        return _fakeTessellationSink.Triangles;
    }

    public IEnumerable<Triangle> CreateWidenTriangles(Geometry geometry, float stokeWidth) => CreateWidenTriangles(geometry, stokeWidth, Matrix3x2.Identity);

    public IEnumerable<Triangle> CreateWidenTriangles(Geometry geometry, float stokeWidth, Matrix3x2 transform, float flatteningTolerance = 0.25f)
    {
        var path = new PathGeometry(_context.D2DDeviceContext.Factory);
        var sink = path.Open();
        geometry.Widen(stokeWidth, sink);
        sink.Close();
        _fakeTessellationSink.Reset();
        path.Tessellate(transform, flatteningTolerance, _fakeTessellationSink);
        path.Dispose();
        return _fakeTessellationSink.Triangles;
    }

    protected override void Dispose(bool disposingManaged)
    {
        if (disposingManaged)
        {
            Utilities.Dispose(ref _tempPathGeometry);
            Utilities.Dispose(ref _tempGeometrySink);
        }

        base.Dispose(disposingManaged);
    }

    private class FakeTessellationSink : TessellationSink
    {
        private readonly List<Triangle> _triangles = new();

        public IEnumerable<Triangle> Triangles => _triangles;

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IDisposable Shadow { get; set; } = null!;

        public Result QueryInterface(ref Guid guid, [UnscopedRef] out IntPtr comObject)
        {
            throw new NotImplementedException();
        }

        public int AddReference()
        {
            throw new NotImplementedException();
        }

        public int Release()
        {
            throw new NotImplementedException();
        }

        public void Reset() => _triangles.Clear();

        public void AddTriangles(Triangle[] triangles) => _triangles.AddRange(triangles);

        public void Close()
        {
            throw new NotImplementedException();
        }
    }
}