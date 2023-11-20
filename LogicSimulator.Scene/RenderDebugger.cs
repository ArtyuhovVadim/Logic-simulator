using System.Diagnostics;
using System.Runtime.CompilerServices;
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

        ClearMethodCalledStatistic();

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
    public static void StartMethodCall([CallerMemberName] string? methodName = null)
    {
        if (_currentScene is null || methodName is null)
            return;

        if (!Stats[_currentScene].MethodsCallStatistics.TryGetValue(methodName, out var stats))
        {
            stats = new MethodCallStatistics();
            Stats[_currentScene].MethodsCallStatistics[methodName] = stats;
        }

        stats.Name = methodName;
        stats.StartTime = FrameTimeStopwatch.Elapsed.TotalMilliseconds;
    }

    [Conditional("DEBUG")]
    public static void EndMethodCall([CallerMemberName] string? methodName = null)
    {
        if (_currentScene is null || methodName is null)
            return;

        var stats = Stats[_currentScene].MethodsCallStatistics[methodName] ?? throw new InvalidOperationException();

        stats.Count++;
        stats.TotalTime += FrameTimeStopwatch.Elapsed.TotalMilliseconds - stats.StartTime;
    }

    [Conditional("DEBUG")]
    public static void ClearMethodCalledStatistic()
    {
        if (_currentScene is null)
            return;

        Stats[_currentScene].MethodsCallStatistics.Clear();
    }

    [Conditional("DEBUG")]
    public static void DrawStatistics(Scene2D scene, D2DContext context, Vector2 pos)
    {
        if (!Stats.TryGetValue(scene, out var stats))
            return;

        var renderCallsTotalTime = stats.MethodsCallStatistics.Sum(x => x.Value.TotalTime);

        var text = $"""
                   Frame index: {stats.FramesCount}
                   Frame time: {stats.FrameTime:0.00 ms} [{1000d / stats.FrameTime:0.0 FPS}]
                   Time between frames: {stats.BetweenFramesTime:0.00 ms} [{1000d / stats.BetweenFramesTime:0.0 FPS}]
                   
                   Render calls total time: {renderCallsTotalTime:0.00 ms}
                   Other time {stats.FrameTime - renderCallsTotalTime:0.00 ms};
                   
                   {string.Join('\n', stats.MethodsCallStatistics.OrderBy(x => x.Key).Select(x => $"{x.Key} count: {x.Value.Count} [{x.Value.TotalTime:0.000 ms}]"))}
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

    public class MethodCallStatistics
    {
        public string Name { get; set; } = string.Empty;

        public int Count { get; set; }

        public double StartTime { get; set; }

        public double TotalTime { get; set; }
    }

    public class Statistics
    {
        public double FrameTime { get; set; }

        public double BetweenFramesTime { get; set; }

        public int FramesCount { get; set; }

        public readonly Dictionary<string, MethodCallStatistics> MethodsCallStatistics = new();
    }
}