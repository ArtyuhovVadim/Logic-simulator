using LogicSimulator.ViewModels.Base;

namespace LogicSimulator.ViewModels;

public class PropertiesViewModel : BindableBase
{
    #region Name

    private string _name;

    public string Name
    {
        get => _name;
        set => Set(ref _name, value);
    }

    #endregion
}