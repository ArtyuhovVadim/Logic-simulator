using System.Diagnostics;
using LogicSimulator.Scene.DirectX;
using SharpDX;
using SharpDX.Direct2D1;

namespace LogicSimulator.Scene;

public static class RenderDebugger
{
    private static Scene2D? _currentScene = null!;
    private static readonly Stopwatch FrameTimeStopwatch = new();
    private static readonly Stopwatch BetweenFramesTimeStopwatch = new();

    private static readonly Dictionary<Scene2D, Statistics> Stats = new();
    private static D2DContext _context = null!;

    public static IReadOnlyDictionary<Scene2D, Statistics> StatisticsMap => Stats;

    [Conditional("DEBUG")]
    public static void BeginRender(Scene2D scene, D2DContext context)
    {
        _context = context;
        _currentScene = scene;

        Stats.TryAdd(_currentScene, new Statistics());

        FrameTimeStopwatch.Restart();

        Stats[_currentScene].BetweenFramesTime = BetweenFramesTimeStopwatch.Elapsed.TotalMilliseconds;
        Stats[_currentScene].DrawRectangleCalledCount = 0;
        Stats[_currentScene].FillRectangleCalledCount = 0;
        Stats[_currentScene].DrawLineCalledCount = 0;
        Stats[_currentScene].DrawEllipseCalledCount = 0;
        Stats[_currentScene].FillEllipseCalledCount = 0;
        Stats[_currentScene].DrawGeometryCalledCount = 0;
        Stats[_currentScene].FillGeometryCalledCount = 0;
        Stats[_currentScene].DrawTextCalledCount = 0;
        Stats[_currentScene].DrawTextLayoutCalledCount = 0;
        Stats[_currentScene].DrawBitmapCalledCount = 0;
        Stats[_currentScene].DrawTrianglesCalledCount = 0;
        Stats[_currentScene].DrawRoundedRectangleCalledCount = 0;
        Stats[_currentScene].FillRoundedRectangleCalledCount = 0;

        BetweenFramesTimeStopwatch.Restart();
    }

    [Conditional("DEBUG")]
    public static void EndRender()
    {
        FrameTimeStopwatch.Stop();

        Stats[_currentScene!].FrameTime = FrameTimeStopwatch.Elapsed.TotalMilliseconds;
        Stats[_currentScene!].FramesCount = _context.DrawingContext.RenderedFramesCount;

        _currentScene = null!;
        _context = null!;
    }

    [Conditional("DEBUG")]
    public static void DrawRectangleCalled()
    {
        if (_currentScene is null)
            return;

        Stats[_currentScene].DrawRectangleCalledCount++;
    }

    [Conditional("DEBUG")]
    public static void FillRectangleCalled()
    {
        if (_currentScene is null)
            return;

        Stats[_currentScene].FillRectangleCalledCount++;
    }

    [Conditional("DEBUG")]
    public static void DrawLineCalled()
    {
        if (_currentScene is null)
            return;

        Stats[_currentScene].DrawLineCalledCount++;
    }

    [Conditional("DEBUG")]
    public static void DrawEllipseCalled()
    {
        if (_currentScene is null)
            return;

        Stats[_currentScene].DrawEllipseCalledCount++;
    }

    [Conditional("DEBUG")]
    public static void FillEllipseCalled()
    {
        if (_currentScene is null)
            return;

        Stats[_currentScene].FillEllipseCalledCount++;
    }

    [Conditional("DEBUG")]
    public static void DrawGeometryCalled()
    {
        if (_currentScene is null)
            return;

        Stats[_currentScene].DrawGeometryCalledCount++;
    }

    [Conditional("DEBUG")]
    public static void FillGeometryCalled()
    {
        if (_currentScene is null)
            return;

        Stats[_currentScene].FillGeometryCalledCount++;
    }

    [Conditional("DEBUG")]
    public static void DrawTextCalled()
    {
        if (_currentScene is null)
            return;

        Stats[_currentScene].DrawTextCalledCount++;
    }

    [Conditional("DEBUG")]
    public static void DrawTextLayoutCalled()
    {
        if (_currentScene is null)
            return;

        Stats[_currentScene].DrawTextLayoutCalledCount++;
    }

    [Conditional("DEBUG")]
    public static void DrawBitmapCalled()
    {
        if (_currentScene is null)
            return;

        Stats[_currentScene].DrawBitmapCalledCount++;
    }

    [Conditional("DEBUG")]
    public static void DrawTrianglesCalled()
    {
        if (_currentScene is null)
            return;

        Stats[_currentScene].DrawTrianglesCalledCount++;
    }

    [Conditional("DEBUG")]
    public static void DrawRoundedRectangleCalled()
    {
        if (_currentScene is null)
            return;

        Stats[_currentScene].DrawRoundedRectangleCalledCount++;
    }

    [Conditional("DEBUG")]
    public static void FillRoundedRectangleCalled()
    {
        if (_currentScene is null)
            return;

        Stats[_currentScene].FillRoundedRectangleCalledCount++;
    }

    [Conditional("DEBUG")]
    public static void DrawStatistics(Scene2D scene, D2DContext context, Vector2 pos)
    {
        if (!Stats.TryGetValue(scene, out var stats))
            return;

        var text = $"""
                   Frames count: {stats.FramesCount}
                   Frame time: {stats.FrameTime:0.00 ms} [{1000d / stats.FrameTime:0.0 FPS}]
                   Time between frames: {stats.BetweenFramesTime:0.00 ms} [{1000d / stats.BetweenFramesTime:0.0 FPS}]
                   DrawRectangle calls count: {stats.DrawRectangleCalledCount}
                   FillRectangle calls count: {stats.FillRectangleCalledCount}
                   DrawLine calls count: {stats.DrawLineCalledCount}
                   DrawEllipse calls count: {stats.DrawEllipseCalledCount}
                   FillEllipse calls count: {stats.FillEllipseCalledCount}
                   DrawGeometry calls count: {stats.DrawGeometryCalledCount}
                   FillGeometry calls count: {stats.FillGeometryCalledCount}
                   DrawText calls count: {stats.DrawTextCalledCount}
                   DrawTextLayout calls count: {stats.DrawTextLayoutCalledCount}
                   DrawBitmap calls count: {stats.DrawBitmapCalledCount}
                   DrawTriangles calls count: {stats.DrawTrianglesCalledCount}
                   FillRoundedRectangle calls count: {stats.FillRoundedRectangleCalledCount}
                   DrawRoundedRectangle calls count: {stats.DrawRoundedRectangleCalledCount}
                   """;

        var padding = 5f;

        using var textBrush = context.ResourceFactory.CreateSolidColorBrush(Color4.Black);
        using var fillBrush = context.ResourceFactory.CreateSolidColorBrush(Color4.White);
        using var strokeBrush = context.ResourceFactory.CreateSolidColorBrush(Color4.Black);
        using var textFormat = context.ResourceFactory.CreateTextFormat("Calibry", 12);
        using var textLayout = context.ResourceFactory.CreateTextLayout(text, textFormat);

        var rect = new RectangleF
        {
            Location = pos,
            Width = textLayout.Metrics.Width + padding * 2,
            Height = textLayout.Metrics.Height + padding * 2
        };

        context.DrawingContext.FillRectangle(rect, fillBrush);
        context.DrawingContext.DrawRectangle(rect, strokeBrush, 1f);
        context.DrawingContext.DrawTextLayout(pos + new Vector2(padding), textLayout, textBrush, DrawTextOptions.None);
    }

    public class Statistics
    {
        public double FrameTime { get; set; }

        public double BetweenFramesTime { get; set; }

        public int FramesCount { get; set; }

        public int DrawRectangleCalledCount { get; set; }

        public int FillRectangleCalledCount { get; set; }

        public int DrawLineCalledCount { get; set; }

        public int DrawEllipseCalledCount { get; set; }

        public int FillEllipseCalledCount { get; set; }

        public int DrawGeometryCalledCount { get; set; }

        public int FillGeometryCalledCount { get; set; }

        public int DrawTextCalledCount { get; set; }

        public int DrawTextLayoutCalledCount { get; set; }

        public int DrawBitmapCalledCount { get; set; }

        public int DrawTrianglesCalledCount { get; set; }

        public int FillRoundedRectangleCalledCount { get; set; }

        public int DrawRoundedRectangleCalledCount { get; set; }
    }
}