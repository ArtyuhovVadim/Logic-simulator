using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using LogicSimulator.Infrastructure.Commands;
using System.Windows.Input;
using LogicSimulator.Scene.SceneObjects;

namespace LogicSimulator.ViewModels.EditorViewModels.Base;

public class VerticesPropertyViewModel : SinglePropertyViewModel
{
    private readonly ObservableCollection<VertexViewModel> _vertexes = new();

    private Line FirstLine => (Line)EditorViewModel.Objects.First();

    public bool IsVisible => _vertexes.Any();

    #region SelectedVertexIndex

    private int _selectedVertexIndex = -1;

    public int SelectedVertexIndex
    {
        get => _selectedVertexIndex;
        set => Set(ref _selectedVertexIndex, value);
    }

    #endregion

    protected override object GetPropertyValue(IEnumerable<object> objects)
    {
        if (objects.Count() > 1)
        {
            _vertexes.Clear();
        }
        else
        {
            SynchronizeVertexes((Line)objects.First());
        }

        OnPropertyChanged(nameof(IsVisible));

        return _vertexes;
    }

    protected override void SetPropertyValue(IEnumerable<object> objects, object value) { }

    #region AddVertexCommand

    private ICommand _addVertexCommand;

    public ICommand AddVertexCommand => _addVertexCommand ??= new LambdaCommand(_ =>
    {
        FirstLine.AddVertex(_vertexes.Last().Position);
        SelectedVertexIndex = _vertexes.Count - 1;
    }, _ => true);

    #endregion

    #region RemoveVertexCommand

    private ICommand _removeVertexCommand;

    public ICommand RemoveVertexCommand => _removeVertexCommand ??= new LambdaCommand(_ =>
    {
        FirstLine.RemoveVertex(_vertexes[SelectedVertexIndex].Position);
    }, _ => SelectedVertexIndex != -1 && _vertexes.Count > 2);

    #endregion

    private void OnVertexChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(VertexViewModel.Position))
        {
            var vertex = (VertexViewModel)sender;
            FirstLine.ModifyVertex(vertex.Index, vertex.Position);
        }
    }

    private void SynchronizeVertexes(Line line)
    {
        var diff = Math.Abs(_vertexes.Count - line.Vertexes.Count);

        if (diff != 0)
        {
            if (_vertexes.Count < line.Vertexes.Count)
            {
                for (var i = 0; i < diff; i++)
                {
                    var vm = new VertexViewModel();
                    vm.PropertyChanged += OnVertexChanged;
                    _vertexes.Add(vm);
                }
            }
            else
            {
                for (var i = 0; i < diff; i++)
                {
                    _vertexes[^1].PropertyChanged -= OnVertexChanged;
                    _vertexes.RemoveAt(_vertexes.Count - 1);
                }
            }
        }

        for (var i = 0; i < _vertexes.Count; i++)
        {
            _vertexes[i].Index = i;
            _vertexes[i].Position = line.Vertexes[i];
        }
    }
}