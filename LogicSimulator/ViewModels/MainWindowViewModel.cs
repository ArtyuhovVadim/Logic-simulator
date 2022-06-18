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
        var rand = new Random();

        for (var i = 0; i < 1000; i++)
        {
            Objects.Add(new Rectangle
            {
                Location = new Vector2(rand.Next(0, 2000), rand.Next(0, 2000)),
                Width = rand.Next(0, 300),
                Height = rand.Next(0, 300),
                FillColor = new Color4(rand.NextFloat(0, 1), rand.NextFloat(0, 1), rand.NextFloat(0, 1), rand.NextFloat(0, 1))
            });
        }
    }

    #region Objects

    private ObservableCollection<BaseSceneObject> _objects = new()
    {
        new Rectangle { Location = new Vector2(100, 100), Width = 100, Height = 150, StrokeThickness = 1, FillColor = new Color4(1, 0, 0, 1) },
    };
    public ObservableCollection<BaseSceneObject> Objects
    {
        get => _objects;
        set => Set(ref _objects, value);
    }

    #endregion

    #region TestCommand

    private ICommand _TestCommand;

    public ICommand TestCommand => _TestCommand ??= new LambdaCommand(_ =>
    {
        var rand = new Random();

        for (var i = 0; i < 1000; i++)
        {
            var rect = Objects[i] as Rectangle;

            rect.Location = new Vector2(rand.Next(0, 2000), rand.Next(0, 2000));
            rect.Width = rand.Next(0, 300);
            rect.Height = rand.Next(0, 300);
            rect.FillColor = new Color4(rand.NextFloat(0, 1), rand.NextFloat(0, 1), rand.NextFloat(0, 1),
                rand.NextFloat(0, 1));

        }
    }, _ => true);

    #endregion
}