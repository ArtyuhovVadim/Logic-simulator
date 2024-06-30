using LogicSimulator.Models;
using LogicSimulator.Utils;
using LogicSimulator.ViewModels.AnchorableViewModels;
using LogicSimulator.ViewModels.ObjectViewModels.Base;
using LogicSimulator.ViewModels.Tools.Base;
using SharpDX;
using WpfExtensions.Mvvm.Commands;

namespace LogicSimulator.ViewModels.Tools;

public class SegmentedObjectPlacingToolViewModel<T> : BasePlacingToolViewModel<T> where T : BaseObjectViewModel, ISegmentedObject
{
    private int _currentVertexIndex = -1;

    private readonly PlacingStep<T> _addVertexStep;

    public SegmentedObjectPlacingToolViewModel(SchemeViewModel scheme, Func<T> objectFactory) : base(scheme, objectFactory)
    {
        FirstStep = new PlacingStep<T>(UpdateLocation, null, UpdateLocation, LocationStepTransition);
        _addVertexStep = new PlacingStep<T>(EnterAddVertexStep, ExitAddVertexStep, UpdateVertexPosition, AddVertexStepTransition);
    }

    #region ApplyCommand

    private ICommand? _applyCommand;

    public ICommand ApplyCommand => _applyCommand ??= new LambdaCommand(OnApply);

    #endregion

    protected override PlacingStep<T> FirstStep { get; }

    protected override void OnStartObjectPlacing(T obj) => _currentVertexIndex = -1;

    protected override bool OnObjectPlaced(T obj) => obj.Vertexes.Count != 0;

    private void OnApply()
    {
        if (_currentVertexIndex == -1)
        {
            Reject();
        }
        else
        {
            Object!.Vertexes.RemoveAt(_currentVertexIndex);
            GoToStep(null);
        }
    }

    private void UpdateLocation(T obj, Vector2 pos)
    {
        pos = pos.ApplyGrid(Scheme.GridStep);
        obj.Location = pos;
    }

    private PlacingStep<T> LocationStepTransition(T obj) => _addVertexStep;

    private void EnterAddVertexStep(T obj, Vector2 pos)
    {
        pos = pos.ApplyGrid(Scheme.GridStep);
        obj.Vertexes.Add(pos - obj.Location);
        _currentVertexIndex++;
    }

    private void UpdateVertexPosition(T obj, Vector2 pos)
    {
        pos = pos.ApplyGrid(Scheme.GridStep);
        obj.Vertexes[_currentVertexIndex] = pos - obj.Location;
    }

    private void ExitAddVertexStep(T obj, Vector2 pos)
    {
        if (obj.Vertexes.Count > 1 && obj.Vertexes[^1] == obj.Vertexes[^2])
        {
            obj.Vertexes.RemoveAt(_currentVertexIndex);
            _currentVertexIndex--;
        }
    }

    private PlacingStep<T> AddVertexStepTransition(T obj) => _addVertexStep;
}