using System.Globalization;
using System.Text.RegularExpressions;
using SharpDX;
using SharpDX.Direct2D1;

namespace LogicSimulator.Scene.DirectX;

public static partial class PathGeometryParser
{
    private static readonly Dictionary<char, int> Length = new()
    {
        { 'a', 7 },
        { 'c', 6 },
        { 'h', 1 },
        { 'l', 2 },
        { 'm', 2 },
        { 'q', 4 },
        { 's', 4 },
        { 't', 2 },
        { 'v', 1 },
        { 'z', 0 },
    };

    private static bool _isFigureOpened;
    private static GeometrySink _sink = null!;
    private static Vector2 _prevPoint = Vector2.Zero;
    private static Vector2 _lastStartPoint = Vector2.Zero;

    public static bool TryParseToSink(string path, GeometrySink sink, out Exception? exception)
    {
        try
        {
            ParseToSink(path, sink);
            exception = null;
            return true;
        }
        catch (Exception e)
        {
            exception = e;
            return false;
        }
    }

    public static void ParseToSink(string path, GeometrySink sink)
    {
        var commands = Parse(path);
        ExecuteCommandsOnSink(commands, sink);
    }

    private static void ExecuteCommandsOnSink(IEnumerable<Command> commands, GeometrySink sink)
    {
        _isFigureOpened = false;
        _sink = sink;
        _lastStartPoint = Vector2.Zero;
        _prevPoint = Vector2.Zero;
        
        foreach (var command in commands)
        {
            switch (command.Type)
            {
                case CommandType.Move: HandleMove(command); break;
                case CommandType.Line: HandleLine(command); break;
                case CommandType.HorizontalLine: HandleHorizontalLine(command); break;
                case CommandType.VerticalLine: HandleVerticalLine(command); break;
                case CommandType.EllipticalArc: HandleEllipticalArc(command); break;
                case CommandType.BezierCurve: HandleBezierCurve(command); break;
                case CommandType.ShorthandBezierCurve: HandleShorthandBezierCurve(command); break;
                case CommandType.QuadraticBezierCurve: HandleQuadraticBezierCurve(command); break;
                case CommandType.ShorthandQuadraticBezierCurve: HandleShorthandQuadraticBezierCurve(command); break;
                case CommandType.Close: HandleClose(FigureEnd.Closed); break;
                case CommandType.Unknown: throw new InvalidOperationException("Unknown command type.");
                default: throw new ArgumentOutOfRangeException();
            }
        }

        if(_isFigureOpened)
            HandleClose(FigureEnd.Open);

        _sink = null!;
    }

    private static void HandleMove(Command command)
    {
        if (_isFigureOpened)
            HandleClose(FigureEnd.Open);

        var point = new Vector2(command.Args[0], command.Args[1]);

        if (!command.IsAbsolute)
            point += _lastStartPoint;

        _lastStartPoint = point;
        _prevPoint = point;
        _sink.BeginFigure(point, FigureBegin.Filled);
        _isFigureOpened = true;
    }

    private static void HandleLine(Command command)
    {
        var point = new Vector2(command.Args[0], command.Args[1]);

        if (!command.IsAbsolute)
            point += _prevPoint;

        _prevPoint = point;

        _sink.AddLine(point);
    }

    private static void HandleHorizontalLine(Command command)
    {
        var point = new Vector2(command.Args[0], _prevPoint.Y);

        if (!command.IsAbsolute)
            point.X += _prevPoint.X;

        _prevPoint = point;

        _sink.AddLine(point);
    }

    private static void HandleVerticalLine(Command command)
    {
        var point = new Vector2(_prevPoint.X, command.Args[0]);

        if (!command.IsAbsolute)
            point.Y += _prevPoint.Y;

        _prevPoint = point;

        _sink.AddLine(point);
    }

    private static void HandleEllipticalArc(Command command)
    {
        var point = new Vector2(command.Args[5], command.Args[6]);

        if (!command.IsAbsolute)
            point += _prevPoint;

        _sink.AddArc(new ArcSegment
        {
            Size = new Size2F(command.Args[0], command.Args[1]),
            RotationAngle = command.Args[2],
            ArcSize = command.Args[3] == 0 ? ArcSize.Small : ArcSize.Large,
            SweepDirection = command.Args[4] == 0 ? SweepDirection.CounterClockwise : SweepDirection.Clockwise,
            Point = point
        });

        _prevPoint = point;
    }

    private static void HandleBezierCurve(Command command)
    {
        var p1 = new Vector2(command.Args[0], command.Args[1]);
        var p2 = new Vector2(command.Args[2], command.Args[3]);
        var p3 = new Vector2(command.Args[4], command.Args[5]);
        
        if (!command.IsAbsolute)
        {
            p1 += _prevPoint;
            p2 += _prevPoint;
            p3 += _prevPoint;
        }

        _sink.AddBezier(new BezierSegment
        {
            Point1 = p1,
            Point2 = p2,
            Point3 = p3
        });

        _prevPoint = p3;
    }

    private static void HandleShorthandBezierCurve(Command command)
    {
        throw new NotImplementedException();
    }

    private static void HandleQuadraticBezierCurve(Command command)
    {
        var p1 = new Vector2(command.Args[0], command.Args[1]);
        var p2 = new Vector2(command.Args[2], command.Args[3]);

        if (!command.IsAbsolute)
        {
            p1 += _prevPoint;
            p2 += _prevPoint;
        }

        _sink.AddQuadraticBezier(new QuadraticBezierSegment
        {
            Point1 = p1,
            Point2 = p2,
        });

        _prevPoint = p2;
    }

    private static void HandleShorthandQuadraticBezierCurve(Command command)
    {
        throw new NotImplementedException();
    }

    private static void HandleClose(FigureEnd endType)
    {
        if (!_isFigureOpened)
            throw new InvalidOperationException("Figure closed");

        _sink.EndFigure(endType);
        _isFigureOpened = false;
    }

    //Source: https://github.com/zHaytam/SvgPathProperties
    private static List<Command> Parse(string path)
    {
        path = string.IsNullOrEmpty(path) ? "M0,0" : path;
        var segments = SvgPartRegex().Matches(path);
        if (segments.Count == 0)
            throw new Exception($"No path elements found in string {path}");

        var result = new List<Command>(segments.Count);
        foreach (var match in segments.Cast<Match>())
        {
            var command = match.Value[0];
            var type = char.ToLowerInvariant(command);
            var args = ParseValues(match.Value[1..]);

            // overloaded moveTo
            if (type == 'm' && args.Count > 2)
            {
                var (commandType, isAbsolute) = CharToCommandType(command);
                result.Add(new Command(commandType, args.Splice(0, 2), isAbsolute));
                type = 'l';
                command = command == 'm' ? 'l' : 'L';
            }

            while (args.Count >= 0)
            {
                if (args.Count == Length[type])
                {
                    var (commandType, isAbsolute) = CharToCommandType(command);
                    result.Add(new Command(commandType, args.Splice(0, Length[type]), isAbsolute));
                    break;
                }

                if (args.Count < Length[type])
                    throw new Exception($"Malformed path data: \"{command}\" must have {Length[type]} elements and has {args.Count}: {match.Value}");

                var (commandType1, isAbsolute1) = CharToCommandType(command);
                result.Add(new Command(commandType1, args.Splice(0, Length[type]), isAbsolute1));
            }
        }

        return result;
    }

    //Source: https://github.com/zHaytam/SvgPathProperties
    private static List<float> ParseValues(string args)
    {
        var numbers = ArgsRegex().Matches(args);
        return numbers.Select(m => float.Parse(m.Value, CultureInfo.InvariantCulture)).ToList();
    }

    //Source: https://github.com/zHaytam/SvgPathProperties
    private static List<T> Splice<T>(this List<T> source, int start, int size)
    {
        var items = source.Skip(start).Take(size).ToList();
        if (source.Count >= size)
            source.RemoveRange(start, size);
        else
            source.Clear();
        return items;
    }

    private static (CommandType Type, bool IsAbsolute) CharToCommandType(char chr)
    {
        var isAbsolute = char.IsUpper(chr);
        var type = char.ToUpper(chr) switch
        {
            'M' => CommandType.Move,
            'L' => CommandType.Line,
            'H' => CommandType.HorizontalLine,
            'V' => CommandType.VerticalLine,
            'A' => CommandType.EllipticalArc,
            'C' => CommandType.BezierCurve,
            'S' => CommandType.ShorthandBezierCurve,
            'Q' => CommandType.QuadraticBezierCurve,
            'T' => CommandType.ShorthandQuadraticBezierCurve,
            'Z' => CommandType.Close,
            _ => throw new ArgumentOutOfRangeException(nameof(chr))
        };

        return (type, isAbsolute);
    }

    [GeneratedRegex(@"-?[0-9]*\.?[0-9]+(?:e[-+]?\d+)?", RegexOptions.IgnoreCase | RegexOptions.Compiled)]
    private static partial Regex ArgsRegex();

    [GeneratedRegex("([astvzqmhlc])([^astvzqmhlc]*)", RegexOptions.IgnoreCase | RegexOptions.Compiled)]
    private static partial Regex SvgPartRegex();

    private enum CommandType
    {
        Unknown,
        Move,                           // M
        Line,                           // L
        HorizontalLine,                 // H
        VerticalLine,                   // V
        EllipticalArc,                  // A
        BezierCurve,                    // C
        ShorthandBezierCurve,           // S
        QuadraticBezierCurve,           // Q
        ShorthandQuadraticBezierCurve,  // T
        Close,                          // Z
    }

    private record Command(CommandType Type, List<float> Args, bool IsAbsolute);
}