using System.Collections.ObjectModel;
using LogicSimulator.Scene.Components;
using LogicSimulator.Scene.Components.Base;
using LogicSimulator.Scene.SceneObjects.Base;
using LogicSimulator.Scene.Tools;
using LogicSimulator.Scene.Tools.Base;
using LogicSimulator.ViewModels.Base;
using SharpDX;

namespace LogicSimulator.ViewModels;

public class SchemeSceneViewModel : BindableBase
{
    #region Components

    private ObservableCollection<BaseRenderingComponent> _renderingComponents = new()
    {
        new GridRenderingComponent
        {
            Width = 3000,
            Height = 3000,
            CellSize = 25,
            Background = new Color4(1, 252f / 255f, 248f / 255f, 1f),
            LineColor = new Color4(240f / 255f, 240f / 255f, 235f / 255f, 1f),
            BoldLineColor = new Color4(220f / 255f, 220f / 255f, 215f / 255f, 1f),
        },
        new SceneObjectsRenderingComponent(),
        new SelectionRenderingComponent(),
        new SelectionRectangleRenderingComponent(),
        new NodeRenderingComponent()
    };
    public ObservableCollection<BaseRenderingComponent> RenderingComponents
    {
        get => _renderingComponents;
        set => Set(ref _renderingComponents, value);
    }

    #endregion

    #region Tools

    private ObservableCollection<BaseTool> _tools = new()
    {
        new SelectionTool(),
        new DragTool(),
        new RectangleSelectionTool(),
        new NodeDragTool()
    };
    public ObservableCollection<BaseTool> Tools
    {
        get => _tools;
        set => Set(ref _tools, value);
    }

    #endregion

    #region Objects

    private ObservableCollection<BaseSceneObject> _objects = new();

    public ObservableCollection<BaseSceneObject> Objects
    {
        get => _objects;
        set => Set(ref _objects, value);
    }

    #endregion
}