using System.Collections.Specialized;
using System.ComponentModel;
using SharpDX;
using WpfExtensions.Mvvm.Commands;

namespace LogicSimulator.ViewModels.EditorViewModels.Base.Properties;

public class VerticesPropertyViewModel : SinglePropertyViewModel
{
    private bool _suppressVertexChanged;
    private object _firstObject = null!;
    private readonly ObservableCollection<VertexViewModel> _vertexes = [];

    #region SelectedVertexIndex

    private int _selectedVertexIndex;

    public int SelectedVertexIndex
    {
        get => _selectedVertexIndex;
        set => Set(ref _selectedVertexIndex, value);
    }

    #endregion

    #region AddVertexCommand

    private ICommand? _addVertexCommand;

    public ICommand AddVertexCommand => _addVertexCommand ??= new LambdaCommand(_ =>
    {
        var vertexes = GetValue<ObservableCollection<Vector2>>(_firstObject);
        vertexes.Insert(SelectedVertexIndex, _vertexes[SelectedVertexIndex].Position);
        SelectedVertexIndex++;
    });

    #endregion

    #region RemoveVertexCommand

    private ICommand? _removeVertexCommand;

    public ICommand RemoveVertexCommand => _removeVertexCommand ??= new LambdaCommand(_ =>
    {
        var tmpIndex = SelectedVertexIndex;
        var vertexes = GetValue<ObservableCollection<Vector2>>(_firstObject);
        vertexes.RemoveAt(SelectedVertexIndex);
        SelectedVertexIndex = tmpIndex == vertexes.Count ? tmpIndex - 1 : tmpIndex;
    }, _ => SelectedVertexIndex != -1 && _vertexes.Count > 1 && SelectedVertexIndex < _vertexes.Count);

    #endregion

    protected override void OnStartEdit(IEnumerable<object> objects)
    {
        _firstObject = objects.First();
        var vertexes = GetValue<ObservableCollection<Vector2>>(_firstObject);
        vertexes.CollectionChanged += OnVertexesCollectionChanged;
        SelectedVertexIndex = vertexes.Count - 1;
    }

    protected override object GetPropertyValue(IEnumerable<object> objects)
    {
        if (objects.Count() > 1)
            return null!;

        var vertexes = GetValue<ObservableCollection<Vector2>>(_firstObject);

        SynchronizeVertexes(vertexes);

        return _vertexes;
    }

    protected override void SetPropertyValue(IEnumerable<object> objects, object value) { }

    protected override void OnEndEdit(IEnumerable<object> objects)
    {
        var vertexes = GetValue<ObservableCollection<Vector2>>(_firstObject);
        vertexes.CollectionChanged -= OnVertexesCollectionChanged;
        foreach (var vertex in _vertexes)
        {
            vertex.PropertyChanged -= OnVertexChanged;
        }
        _firstObject = null!;
        _vertexes.Clear();
        SelectedVertexIndex = -1;
    }

    private void OnVertexesCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e) => OnPropertyChanged(nameof(Value));

    private void SynchronizeVertexes(IReadOnlyList<Vector2> vertexes)
    {
        _suppressVertexChanged = true;
        var diff = Math.Abs(_vertexes.Count - vertexes.Count);

        if (diff != 0)
        {
            if (_vertexes.Count < vertexes.Count)
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
            _vertexes[i].X = vertexes[i].X;
            _vertexes[i].Y = vertexes[i].Y;
        }
        _suppressVertexChanged = false;
    }

    private void OnVertexChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (_suppressVertexChanged || e.PropertyName is not (nameof(VertexViewModel.X) or nameof(VertexViewModel.Y))) return;

        var vertex = (VertexViewModel)sender!;
        var vertexes = GetValue<ObservableCollection<Vector2>>(_firstObject);
        vertexes[vertex.Index] = new Vector2(vertex.X, vertex.Y);
    }

    public override PropertyViewModel MakeCopy(EditorViewModel editor) =>
        new VerticesPropertyViewModel { PropertyName = PropertyName, EditorViewModel = editor };
}