namespace LogicSimulator.ViewModels.EditorViewModels.Base.Properties;

public class VerticesPropertyViewModel : SinglePropertyViewModel
{
    protected override object GetPropertyValue(IEnumerable<object> objects)
    {
        return Enumerable.Empty<VertexViewModel>().ToList();
    }

    protected override void SetPropertyValue(IEnumerable<object> objects, object value) { }

    public override PropertyViewModel MakeCopy(EditorViewModel editor) => 
        new VerticesPropertyViewModel { PropertyName = PropertyName, EditorViewModel = editor };
}