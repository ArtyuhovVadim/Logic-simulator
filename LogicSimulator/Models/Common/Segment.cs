using SharpDX;

namespace LogicSimulator.Models.Common;

public struct Segment
{
    public Segment() { }

    public Segment(Vector2 a, Vector2 b)
    {
        A = a;
        B = b;
    }

    public Vector2 A { get; set; }

    public Vector2 B { get; set; }

    public bool ContainsPoint(Vector2 p)
    {
        // Точка лежит на отрезке если:
        // [AB, AP] = 0 – косое произведение (точка лежит на прямой)
        // (PA, PB) ≤ 0 – скалярное произведение (точка лежит между A и B)

        var ab = B - A;
        var ap = p - A;
        var pa = A - p;
        var pb = B - p;

        return MathUtil.IsZero(ab.X * ap.Y - ap.X * ab.Y) && Vector2.Dot(pa, pb) <= 0;
    }
}