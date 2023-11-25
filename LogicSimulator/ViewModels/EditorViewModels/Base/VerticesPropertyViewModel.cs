using System.Collections.Specialized;
using System.ComponentModel;
using LogicSimulator.Infrastructure.Commands;
using LogicSimulator.ViewModels.ObjectViewModels;
using SharpDX;

namespace LogicSimulator.ViewModels.EditorViewModels.Base;

public class VerticesPropertyViewModel : SinglePropertyViewModel
{
    private readonly ObservableCollection<VertexViewModel> _vertexes = new();
    private LineViewModel _line;
    private bool _suppressVertexChanged;

    #region SelectedVertexIndex

    private int _selectedVertexIndex = -1;

    public int SelectedVertexIndex
    {
        get => _selectedVertexIndex;
        set => Set(ref _selectedVertexIndex, value);
    }

    #endregion

    #region AddVertexCommand

    private ICommand _addVertexCommand;

    public ICommand AddVertexCommand => _addVertexCommand ??= new LambdaCommand(_ =>
    {
        _line.Vertexes.Add(_line.Vertexes.Last());
        SynchronizeVertexes(_line);
        OnPropertyChanged(nameof(Value));
        SelectedVertexIndex = _vertexes.Count - 1;
    }, _ => true);

    #endregion

    #region RemoveVertexCommand

    private ICommand _removeVertexCommand;

    public ICommand RemoveVertexCommand => _removeVertexCommand ??= new LambdaCommand(_ =>
    {
        _line.Vertexes.RemoveAt(SelectedVertexIndex);
        SynchronizeVertexes(_line);
        OnPropertyChanged(nameof(Value));
    }, _ => SelectedVertexIndex != -1 && _vertexes.Count > 1 && SelectedVertexIndex < _vertexes.Count);

    #endregion

    protected override void OnStartEdit(IEnumerable<object> objects)
    {
        foreach (var line in objects.Cast<LineViewModel>())
        {
            line.Vertexes.CollectionChanged += OnVertexesChanged;
        }
    }

    protected override void OnEndEdit(IEnumerable<object> objects)
    {
        foreach (var line in objects.Cast<LineViewModel>())
        {
            line.Vertexes.CollectionChanged -= OnVertexesChanged;
        }

        foreach (var vertex in _vertexes)
        {
            vertex.PropertyChanged -= OnVertexChanged;
        }

        _line = null;
        _vertexes.Clear();
    }

    protected override object GetPropertyValue(IEnumerable<object> objects)
    {
        if (objects.Count() != 1)
            return null;

        _line = (LineViewModel)objects.First();
        SynchronizeVertexes(_line);

        return _vertexes;
    }

    private void OnVertexesChanged(object sender, NotifyCollectionChangedEventArgs e) => OnPropertyChanged(nameof(Value));

    protected override void SetPropertyValue(IEnumerable<object> objects, object value) { }

    private void SynchronizeVertexes(LineViewModel line)
    {
        _suppressVertexChanged = true;
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
            _vertexes[i].X = line.Vertexes[i].X;
            _vertexes[i].Y = line.Vertexes[i].Y;
        }
        _suppressVertexChanged = false;
    }

    private void OnVertexChanged(object sender, PropertyChangedEventArgs e)
    {
        if (_suppressVertexChanged)
            return;

        if (e.PropertyName is nameof(VertexViewModel.X) or nameof(VertexViewModel.Y))
        {
            var vertex = (VertexViewModel)sender;
            _line.Vertexes[vertex.Index] = new Vector2(vertex.X, vertex.Y);
        }
    }
}