using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using LogicSimulator.Infrastructure.Commands;
using LogicSimulator.Scene.Components;
using LogicSimulator.Scene.Components.Base;
using LogicSimulator.Scene.SceneObjects.Base;
using LogicSimulator.ViewModels.Base;
using SharpDX;
using Rectangle = LogicSimulator.Scene.SceneObjects.Rectangle;

namespace LogicSimulator.ViewModels;

public class MainWindowViewModel : BindableBase
{
    public MainWindowViewModel()
    {
        var rand = new Random();

        for (var i = 0; i < 10; i++)
        {
            Objects.Add(new Rectangle
            {
                Location = new Vector2(rand.Next(0, 1500), rand.Next(0, 800)),
                Width = rand.Next(50, 300),
                Height = rand.Next(50, 300),
                FillColor = new Color4(rand.NextFloat(0, 1), rand.NextFloat(0, 1), rand.NextFloat(0, 1), rand.NextFloat(0, 1))
            });
        }
    }

    #region Objects

    private ObservableCollection<BaseSceneObject> _objects = new();
    public ObservableCollection<BaseSceneObject> Objects
    {
        get => _objects;
        set => Set(ref _objects, value);
    }

    #endregion

    #region RenderingComponents

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
        }
    };
    public ObservableCollection<BaseRenderingComponent> RenderingComponents
    {
        get => _renderingComponents;
        set => Set(ref _renderingComponents, value);
    }

    #endregion

    #region TestCommand

    private ICommand _TestCommand;

    public ICommand TestCommand => _TestCommand ??= new LambdaCommand(_ =>
    {

    }, _ => true);

    #endregion
}