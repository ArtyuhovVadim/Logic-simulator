using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using LogicSimulator.Infrastructure.Commands;
using LogicSimulator.Scene.SceneObjects.Base;
using LogicSimulator.ViewModels.Base;
using SharpDX;
using Rectangle = LogicSimulator.Scene.SceneObjects.Rectangle;

namespace LogicSimulator.ViewModels;

public class MainWindowViewModel : BindableBase
{
    public MainWindowViewModel()
    {
        var rand = Random.Shared;

        for (var i = 0; i < 30; i++)
        {
            Objects.Add(new Rectangle
            {
                Location = new Vector2(rand.Next(0, 1500), rand.Next(0, 1500)),
                Width = rand.Next(rand.Next(0, 500)),
                Height = rand.Next(rand.Next(0, 500)),
                StrokeWidth = 1,
                StrokeColor = new Color4(0, 0, 0, 1),
                FillColor = new Color4(rand.NextFloat(0, 1), rand.NextFloat(0, 1), rand.NextFloat(0, 1), 1)
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

    #region TestCommand

    private ICommand _testCommand;

    public ICommand TestCommand => _testCommand ??= new LambdaCommand(_ =>
    {
        Objects.Clear();
    }, _ => true);

    #endregion
}