using LogicSimulator.Infrastructure;
using LogicSimulator.Infrastructure.ExtensionMethods;
using LogicSimulator.Models;
using LogicSimulator.Scene;
using LogicSimulator.ViewModels.ObjectViewModels.Base;
using SharpDX;
using Color = System.Windows.Media.Color;

namespace LogicSimulator.ViewModels.ObjectViewModels;

public class WireViewModel : BaseObjectViewModel, ISegmentedObject
{
    public WireViewModel() : this(new WireModel()) { }


    public WireViewModel(WireModel model)
    {
        Model = model;
        _vertexes = new SynchronizedObservableCollection<Vector2, Vector2>(model.Vertexes, vec2 => vec2, vec2 => vec2);
    }

    public override WireModel Model { get; }

    public IEnumerable<Vector2> AbsoluteVertexes => [Location, .. Vertexes.Select(x => Location + x.Transform(Rotation))];

    #region Vertexes

    private readonly SynchronizedObservableCollection<Vector2, Vector2> _vertexes;

    public ObservableCollection<Vector2> Vertexes => _vertexes;

    #endregion

    #region StrokeColor

    public Color StrokeColor
    {
        get => Model.StrokeColor;
        set => Set(Model.StrokeColor, value, Model, (model, value) => model.StrokeColor = value);
    }

    #endregion

    #region StrokeThickness

    public float StrokeThickness
    {
        get => Model.StrokeThickness;
        set => Set(Model.StrokeThickness, value, Model, (model, value) => model.StrokeThickness = value);
    }

    #endregion

    #region StrokeThicknessType

    public StrokeThicknessType StrokeThicknessType
    {
        get => Model.StrokeThicknessType;
        set => Set(Model.StrokeThicknessType, value, Model, (model, value) => model.StrokeThicknessType = value);
    }

    #endregion

    public override WireViewModel MakeClone() => new(Model.MakeClone());
}