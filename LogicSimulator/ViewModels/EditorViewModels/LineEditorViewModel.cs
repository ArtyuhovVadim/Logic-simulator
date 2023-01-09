using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using LogicSimulator.Infrastructure.Commands;
using LogicSimulator.Scene.SceneObjects;
using LogicSimulator.ViewModels.EditorViewModels.Base;
using LogicSimulator.ViewModels.EditorViewModels.Layout;
using LogicSimulator.ViewModels.EditorViewModels.Layout.Builders;
using SharpDX;

namespace LogicSimulator.ViewModels.EditorViewModels;

//TODO: !!!
public class LineEditorViewModel : BaseEditorViewModel<Line>
{
    public Vector2 Location { get => Get<Vector2>(); set => Set(value); }
    public float StrokeThickness { get => Get<float>(); set => Set(value); }
    public Color4 StrokeColor { get => Get<Color4>(); set => Set(value); }

    protected override EditorLayout CreateLayout() => LayoutBuilder
        .Create()
        .WithName("Ломаная линия")
        .WithGroup(groupBuilder => groupBuilder
            .WithGroupName("Расположение")
            .WithRow(rowBuilder => rowBuilder
                .WithProperty<Vector2>(nameof(Line.Location))))
        .WithGroup(groupBuilder => groupBuilder
            .WithGroupName("Свойства")
            .WithRow(rowBuilder => rowBuilder
                .WithRowName("Граница")
                .WithProperty<float>(nameof(Line.StrokeThickness))
                .WithProperty<Color4>(nameof(Line.StrokeColor))
                .WithLayout(layoutBuilder => layoutBuilder
                    .WithRelativeSize(1)
                    .WithAutoSize())))
        .Build();
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