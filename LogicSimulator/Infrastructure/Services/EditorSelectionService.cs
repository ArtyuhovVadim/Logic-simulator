using System;
using System.Collections.Generic;
using System.Linq;
using LogicSimulator.Infrastructure.Services.Interfaces;
using LogicSimulator.Scene.SceneObjects;
using LogicSimulator.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace LogicSimulator.Infrastructure.Services;

public class EditorSelectionService : IEditorSelectionService
{
    private readonly PropertiesViewModel _propertiesViewModel;

    private readonly Dictionary<Type, Type> _editorTypesMap = new()
    {
        { typeof(Rectangle), typeof(RectangleEditorViewModel) }
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
            _propertiesViewModel.CurrentEditorViewModel = null;
            return;
        }

        var firstObjectType = selectedSceneObjects.First().GetType();

        if (selectedSceneObjects.Count(x => x.GetType() == firstObjectType) != selectedSceneObjects.Count())
        {
            _propertiesViewModel.CurrentEditorViewModel = null;
            return;
        }

        if (!_editorTypesMap.TryGetValue(firstObjectType, out var editorType))
        {
            _propertiesViewModel.CurrentEditorViewModel = null;

            return;
        }

        var editorViewModel = (BaseEditorViewModel)App.Host.Services.GetRequiredService(editorType);

        editorViewModel.SceneObjects = selectedSceneObjects.ToList();

        _propertiesViewModel.CurrentEditorViewModel = editorViewModel;
    }
}