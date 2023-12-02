namespace LogicSimulator.ViewModels.EditorViewModels.Base.Properties;

public class FontPropertyViewModel : MultiPropertyViewModel
{
    public override PropertyViewModel MakeCopy(EditorViewModel editor)
    {
        var prop = new FontPropertyViewModel { EditorViewModel = editor };

        CopySinglePropertiesToOther(prop);

        return prop;
    }
}