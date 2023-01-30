using System.Windows;

namespace LogicSimulator.Controls.Themes;

public abstract class Theme : DependencyObject
{
    public abstract Uri GetResourceUri();
}