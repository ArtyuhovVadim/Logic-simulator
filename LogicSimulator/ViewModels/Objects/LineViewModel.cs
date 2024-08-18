using LogicSimulator.Infrastructure.Collections;
using LogicSimulator.Models.Common;
using LogicSimulator.Models.Objects;
using LogicSimulator.Shared.Models;
using LogicSimulator.ViewModels.Objects.Base;
using SharpDX;
using Color = System.Windows.Media.Color;

namespace LogicSimulator.ViewModels.Objects;

public class LineViewModel : BaseObjectViewModel, ISegmentedObject
{
    public LineViewModel() : this(new LineModel()) { }

    public LineViewModel(LineModel model)
    {
        Model = model;
        _vertexes = new SynchronizedObservableCollection<Vector2, Vector2>(model.Vertexes, vec2 => vec2, vec2 => vec2);
    }

    public override LineModel Model { get; }

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

    public override LineViewModel MakeClone() => new(Model.MakeClone());
}