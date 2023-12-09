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

    public void Select(SchemeViewModel schemeViewModel)
    {
        var selectedSceneObjects = schemeViewModel.Objects.Where(x => x.IsSelected).ToArray();

        if (selectedSceneObjects.Length == 0)
        {
            SetEmptyEditor();
            return;
        }

        var firstObjectType = selectedSceneObjects.First().GetType();

        if (selectedSceneObjects.Any(x => x.GetType() != firstObjectType))
        {
            var layouts = selectedSceneObjects.Select(x => x.GetType()).Distinct().Select(x => EditorsMap[x]).Select(x => x.Layout);

            var multiEditor = new MultiObjectsEditorViewModel(layouts);

            multiEditor.SetObjectsToEdit(selectedSceneObjects);
            _propertiesViewModel.CurrentEditorViewModel = multiEditor;

            return;
        }

        if (!EditorsMap.TryGetValue(firstObjectType, out var editor))
        {
            SetEmptyEditor();
            return;
        }

        editor.SetObjectsToEdit(selectedSceneObjects);
        _propertiesViewModel.CurrentEditorViewModel = editor;
    }

    private void SetEmptyEditor()
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