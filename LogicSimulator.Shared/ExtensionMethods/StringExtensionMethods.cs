namespace LogicSimulator.Shared.ExtensionMethods;

public static class StringExtensionMethods
{
    public static string ReturnIfNotEmpty(this string str, string alt = " ") => string.IsNullOrWhiteSpace(str) ? alt : $" {str} ";
}