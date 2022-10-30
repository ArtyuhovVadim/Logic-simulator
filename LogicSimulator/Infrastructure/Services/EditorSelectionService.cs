using System;
using System.Collections.Generic;
using System.Linq;
using LogicSimulator.Infrastructure.Services.Interfaces;
using LogicSimulator.Scene.SceneObjects;
using LogicSimulator.Scene.SceneObjects.Base;
using LogicSimulator.ViewModels;
using LogicSimulator.ViewModels.EditorViewModels;
using LogicSimulator.ViewModels.EditorViewModels.Base;

namespace LogicSimulator.Infrastructure.Services;

public class EditorSelectionService : IEditorSelectionService
{
    private readonly PropertiesViewModel _propertiesViewModel;

    private readonly Dictionary<Type, AbstractEditorViewModel> _editorsMap = new()
    {
        { typeof(Rectangle), new RectangleEditorViewModel() },
        { typeof(RoundedRectangle), new RoundedRectangleEditorViewModel() },
        { typeof(Ellipse), new EllipseEditorViewModel() },
        { typeof(Line), new LineEditorViewModel() },
        { typeof(BezierCurve), new BezierCurveEditorViewModel() },
    };

    public EditorSelectionService(PropertiesViewModel propertiesViewModel)
    {
        _propertiesViewModel = propertiesViewModel;
    }

    public void Select(SchemeViewModel schemeViewModel)
    {
        var selectedSceneObjects = schemeViewModel.Objects.Where(x => x.IsSelected);

        if (!selectedSceneObjects.Any())
        {
            SetEmptyEditor();
            return;
        }

        var firstObjectType = selectedSceneObjects.First().GetType();

        if (selectedSceneObjects.Any(x => x.GetType() != firstObjectType))
        {
            SetEmptyEditor();
            return;
        }

        if (!_editorsMap.TryGetValue(firstObjectType, out var editor))
        {
            SetEmptyEditor();
            return;
        }

        editor.SetObjectsToEdit(selectedSceneObjects);
        _propertiesViewModel.CurrentEditorViewModel = editor;
    }

    private void SetEmptyEditor()
    {
        _propertiesViewModel.CurrentEditorViewModel?.SetObjectsToEdit(Enumerable.Empty<BaseSceneObject>());
        _propertiesViewModel.CurrentEditorViewModel = null;
    }
}