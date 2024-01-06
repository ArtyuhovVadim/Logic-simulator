using LogicSimulator.ViewModels.EditorViewModels.Base;

namespace LogicSimulator.ViewModels.EditorViewModels.Layout;

public class EditorLayout
{
    public string ObjectName { get; set; } = string.Empty;

    public List<EditorGroup> Groups { get; set; } = [];

    public EditorViewModel EditorViewModel { get; set; }= null!;

    public void RaisePropertyChangeForAllProperties()
    {
        foreach (var group in Groups)
        {
            foreach (var row in group.EditorRows)
            {
                foreach (var property in row.ObjectProperties)
                {
                    property.RaisePropertyChanged();
                }
            }
        }
    }

    public void PropertyChange(string propName)
    {
        foreach (var group in Groups)
        {
            group.PropertyChange(propName);
        }
    }

    public void StartEdit()
    {
        foreach (var group in Groups)
        {
            group.StartEdit();
        }
    }

    public void EndEdit()
    {
        foreach (var group in Groups)
        {
            group.EndEdit();
        }
    }
}