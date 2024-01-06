namespace LogicSimulator.ViewModels.EditorViewModels.Base.Properties;

public class FontPropertiesViewModel : MultiPropertyViewModel
{
    public override PropertyViewModel MakeCopy(EditorViewModel editor)
    {
        var prop = new FontPropertiesViewModel { EditorViewModel = editor };

        CopySinglePropertiesToOther(prop);

        return prop;
    }
}