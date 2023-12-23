namespace LogicSimulator.ViewModels.EditorViewModels.Base.Properties;

public class StrokePropertiesViewModel : MultiPropertyViewModel
{
    #region IsMoreThenOneObject

    private bool _isMoreThenOneObject;

    public bool IsMoreThenOneObject
    {
        get => _isMoreThenOneObject;
        set => Set(ref _isMoreThenOneObject, value);
    }

    #endregion

    protected override void OnStartEdit(IEnumerable<object> objects) => IsMoreThenOneObject = objects.Count() > 1;

    public override PropertyViewModel MakeCopy(EditorViewModel editor)
    {
        var prop = new StrokePropertiesViewModel { EditorViewModel = editor };

        CopySinglePropertiesToOther(prop);

        return prop;
    }
}