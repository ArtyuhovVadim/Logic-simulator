using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using LogicSimulator.Infrastructure.Commands;
using LogicSimulator.Scene.SceneObjects;
using LogicSimulator.ViewModels.Base;
using LogicSimulator.ViewModels.EditorViewModels.Base;
using SharpDX;

namespace LogicSimulator.ViewModels.EditorViewModels;

public class VertexViewModel : BindableBase
{
    #region Position

    private Vector2 _position = Vector2.Zero;

    public Vector2 Position
    {
        get => _position;
        set => Set(ref _position, value);
    }

    #endregion

    #region Index

    private int _index;

    public int Index
    {
        get => _index;
        set => Set(ref _index, value);
    }

    #endregion
}

public class LineEditorViewModel : BaseEditorViewModel<Line>
{
    #region TestCommand

    private ICommand _vertexChangedCommand;

    public ICommand VertexChangedCommand => _vertexChangedCommand ??= new LambdaCommand(obj =>
    {
        if (obj is not VertexViewModel vertexViewModel) return;

        FirstObject.ModifyVertex(vertexViewModel.Index, vertexViewModel.Position);
    }, _ => true);

    #endregion

    #region Vertexes

    private readonly ObservableCollection<VertexViewModel> _vertexes = new();

    public ObservableCollection<VertexViewModel> Vertexes => GetSynchronizedVertexes();

    #endregion

    #region StrokeThickness

    public float StrokeThickness
    {
        get => Get<float>();
        set => Set(value);
    }

    #endregion

    #region StrokeColor

    public Color4 StrokeColor
    {
        get => Get<Color4>();
        set => Set(value);
    }

    #endregion

    private ObservableCollection<VertexViewModel> GetSynchronizedVertexes()
    {
        var diff = Math.Abs(_vertexes.Count - FirstObject.Vertexes.Count);

        if (diff != 0)
        {
            if (_vertexes.Count < FirstObject.Vertexes.Count)
            {
                for (var i = 0; i < diff; i++)
                {
                    _vertexes.Add(new VertexViewModel());
                }
            }
            else
            {
                for (var i = 0; i < diff; i++)
                {
                    _vertexes.RemoveAt(_vertexes.Count - 1);
                }
            }
        }

        for (var i = 0; i < _vertexes.Count; i++)
        {
            _vertexes[i].Position = FirstObject.Vertexes[i];
            _vertexes[i].Index = i;
        }

        return _vertexes;
    }
}