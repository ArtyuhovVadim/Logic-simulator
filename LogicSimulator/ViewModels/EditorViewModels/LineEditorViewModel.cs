using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using LogicSimulator.Infrastructure.Commands;
using LogicSimulator.Scene.SceneObjects;
using LogicSimulator.ViewModels.EditorViewModels.Base;
using SharpDX;

namespace LogicSimulator.ViewModels.EditorViewModels;

//TODO: FirstObject работает неправильно
public class LineEditorViewModel : BaseEditorViewModel<Line>
{
    public LineEditorViewModel()
    {
        PropertyChanged += (_, args) =>
        {
            if (args.PropertyName == nameof(Objects))
            {
                OnPropertyChanged(nameof(IsVertexEditorVisible));
            }
        };
    }

    #region Vertexes

    private readonly ObservableCollection<VertexViewModel> _vertexes = new();

    public ObservableCollection<VertexViewModel> Vertexes => GetSynchronizedVertexes();

    #endregion

    #region IsVertexEditorVisible

    public bool IsVertexEditorVisible => Objects.Count == 1;

    #endregion

    #region SelectedVertexIndex

    private int _selectedVertexIndex = -1;

    public int SelectedVertexIndex
    {
        get => _selectedVertexIndex;
        set => Set(ref _selectedVertexIndex, value);
    }

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

    #region VertexChangedCommand

    private ICommand _vertexChangedCommand;

    public ICommand VertexChangedCommand => _vertexChangedCommand ??= new LambdaCommand(obj =>
    {
        if (obj is not VertexViewModel vertexViewModel) return;

        FirstObject.ModifyVertex(vertexViewModel.Index, vertexViewModel.Position);
    }, _ => true);

    #endregion

    #region AddVertexCommand

    private ICommand _addVertexCommand;

    public ICommand AddVertexCommand => _addVertexCommand ??= new LambdaCommand(_ =>
    {
        FirstObject.AddVertex(_vertexes.Last().Position);
        SelectedVertexIndex = _vertexes.Count - 1;
    }, _ => true);

    #endregion

    #region RemoveVertexCommand

    private ICommand _removeVertexCommand;

    public ICommand RemoveVertexCommand => _removeVertexCommand ??= new LambdaCommand(_ =>
    {
        FirstObject.RemoveVertex(_vertexes[SelectedVertexIndex].Position);
    }, _ => SelectedVertexIndex != -1 && _vertexes.Count > 2);

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