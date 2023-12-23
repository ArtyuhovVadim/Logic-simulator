using System.Reflection;
using LogicSimulator.Infrastructure.Services.Interfaces;
using LogicSimulator.ViewModels.AnchorableViewModels;
using LogicSimulator.ViewModels.EditorViewModels;
using LogicSimulator.ViewModels.EditorViewModels.Base;
using LogicSimulator.ViewModels.ObjectViewModels.Base;

namespace LogicSimulator.Infrastructure.Services;

public class EditorSelectionService : IEditorSelectionService
{
    private static readonly Dictionary<Type, EditorViewModel> EditorsMap = [];

    private readonly PropertiesViewModel _propertiesViewModel;

    static EditorSelectionService() => FillEditorMap();

    public EditorSelectionService(PropertiesViewModel propertiesViewModel)
    {
        _propertiesViewModel = propertiesViewModel;
    }

    public void SetObjectsEditor(ICollection<BaseObjectViewModel> objects)
    {
        if (objects.Count == 0)
        {
            SetEmptyEditor();
            return;
        }

        var firstObjectType = objects.First().GetType();

        if (objects.Any(x => x.GetType() != firstObjectType))
        {
            var layouts = objects.Select(x => x.GetType()).Distinct().Select(x => EditorsMap[x]).Select(x => x.Layout);

            var multiEditor = new MultiObjectsEditorViewModel(layouts);

            _propertiesViewModel.CurrentEditorViewModel?.StopObjectsEdit();
            multiEditor.SetObjectsToEdit(objects);
            _propertiesViewModel.CurrentEditorViewModel = multiEditor;

            return;
        }

        if (!EditorsMap.TryGetValue(firstObjectType, out var editor))
        {
            SetEmptyEditor();
            return;
        }

        _propertiesViewModel.CurrentEditorViewModel?.StopObjectsEdit();
        editor.SetObjectsToEdit(objects);
        _propertiesViewModel.CurrentEditorViewModel = editor;
    }

    public void SetSchemeEditor(SchemeViewModel schemeViewModel)
    {
        if (!EditorsMap.TryGetValue(typeof(SchemeViewModel), out var editor))
        {
            SetEmptyEditor();
            return;
        }

        _propertiesViewModel.CurrentEditorViewModel?.StopObjectsEdit();
        editor.SetObjectsToEdit([schemeViewModel]);
        _propertiesViewModel.CurrentEditorViewModel = editor;
    }

    public void SetEmptyEditor()
    {
        _propertiesViewModel.CurrentEditorViewModel?.SetObjectsToEdit(Enumerable.Empty<BaseObjectViewModel>().ToList());
        _propertiesViewModel.CurrentEditorViewModel = null;
    }

    private static void FillEditorMap()
    {
        var assembly = Assembly.GetEntryAssembly();

        foreach (var type in assembly!.DefinedTypes)
        {
            var attribute = Attribute.GetCustomAttribute(type, typeof(EditorAttribute));

            if (attribute is EditorAttribute editorAttribute)
            {
                EditorsMap.Add(editorAttribute.ObjectType, (EditorViewModel)Activator.CreateInstance(type)!);
            }
        }
    }
}