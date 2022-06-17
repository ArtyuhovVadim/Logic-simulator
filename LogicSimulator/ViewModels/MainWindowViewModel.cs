using System;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;
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

        for (var i = 0; i < 100; i++)
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
        new Rectangle { Location = new Vector2(100, 100), Width = 100, Height = 150, StrokeThickness = 1 },
        new Rectangle { Location = new Vector2(200, 300), Width = 100, Height = 150, StrokeThickness = 1 },
        new Rectangle { Location = new Vector2(800, 500), Width = 100, Height = 150, StrokeThickness = 1 }
    };
    public ObservableCollection<BaseSceneObject> Objects
    {
        get => _objects;
        set => Set(ref _objects, value);
    }

    #endregion
}