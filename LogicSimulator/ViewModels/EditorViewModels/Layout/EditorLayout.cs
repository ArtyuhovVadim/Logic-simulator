using System.Collections.Generic;
using System.Collections.ObjectModel;
using LogicSimulator.ViewModels.Base;

namespace LogicSimulator.ViewModels.EditorViewModels.Layout;

public class EditorLayout : BindableBase
{
    private string _objectName;
    private ObservableCollection<EditorGroup> _groups = new();

    public string ObjectName
    {
        get => _objectName;
        set => Set(ref _objectName, value);
    }

    public ObservableCollection<EditorGroup> Groups
    {
        get => _groups;
        set => Set(ref _groups, value);
    }
}