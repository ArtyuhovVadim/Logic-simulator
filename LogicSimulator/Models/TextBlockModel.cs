using System.Windows.Media;
using LogicSimulator.Models.Base;

namespace LogicSimulator.Models;

public class TextBlockModel : BaseObjectModel
{
    public string Text { get; set; } = "Text";

    public string FontName { get; set; } = "Times New Roman";

    public float FontSize { get; set; } = 24f;

    public bool IsBold { get; set; }

    public bool IsItalic { get; set; }

    public bool IsUnderlined { get; set; }

    public bool IsCross { get; set; }

    public Color TextColor { get; set; } = Colors.Black;

    public override TextBlockModel MakeClone() => (TextBlockModel)MemberwiseClone();
}