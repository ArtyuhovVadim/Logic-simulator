using MathExpressionParser;

namespace LogicSimulator.Utils;

public static class MathParserExtensionMethods
{
    public static bool TryParse(this MathParser parser, string text, out double number, out Exception? e)
    {
        try
        {
            number = parser.Parse(text);
            e = null;
            return true;
        }
        catch (Exception err)
        {
            number = double.NaN;
            e = err;
            return false;
        }
    }

    public static bool TryParse(this MathParser parser, string text, out double number) => parser.TryParse(text, out number, out _);
}