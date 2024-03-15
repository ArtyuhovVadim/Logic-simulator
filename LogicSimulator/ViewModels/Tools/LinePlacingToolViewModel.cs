using LogicSimulator.Utils;
using LogicSimulator.ViewModels.AnchorableViewModels;
using LogicSimulator.ViewModels.ObjectViewModels;
using LogicSimulator.ViewModels.Tools.Base;
using SharpDX;
using WpfExtensions.Mvvm.Commands;

namespace LogicSimulator.ViewModels.Tools;

public class LinePlacingToolViewModel : BasePlacingToolViewModel<LineViewModel>
{
    private int _currentVertexIndex = -1;

    private readonly PlacingStep<LineViewModel> _addVertexStep;

    public LinePlacingToolViewModel(SchemeViewModel scheme) : base(scheme, () => new LineViewModel())
    {
        FirstStep = new PlacingStep<LineViewModel>(UpdateLocation, null, UpdateLocation, LocationStepTransition);
        _addVertexStep = new PlacingStep<LineViewModel>(EnterAddVertexStep, ExitAddVertexStep, UpdateVertexPosition, AddVertexStepTransition);
    }

    #region ApplyCommand

    private ICommand? _applyCommand;

    public ICommand ApplyCommand => _applyCommand ??= new LambdaCommand(OnApply);

    #endregion

    protected override PlacingStep<LineViewModel> FirstStep { get; }

    protected override void OnStartObjectPlacing(LineViewModel obj) => _currentVertexIndex = -1;

    protected override bool OnObjectPlaced(LineViewModel obj) => obj.Vertexes.Count != 0;

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

    private void UpdateLocation(LineViewModel obj, Vector2 pos)
    {
        pos = pos.ApplyGrid(Scheme.GridStep);
        obj.Location = pos;
    }

    private PlacingStep<LineViewModel> LocationStepTransition(LineViewModel obj) => _addVertexStep;

    private void EnterAddVertexStep(LineViewModel obj, Vector2 pos)
    {
        pos = pos.ApplyGrid(Scheme.GridStep);
        obj.Vertexes.Add(pos - obj.Location);
        _currentVertexIndex++;
    }

    private void UpdateVertexPosition(LineViewModel obj, Vector2 pos)
    {
        pos = pos.ApplyGrid(Scheme.GridStep);
        obj.Vertexes[_currentVertexIndex] = pos - obj.Location;
    }

    private void ExitAddVertexStep(LineViewModel obj, Vector2 pos)
    {
        if (obj.Vertexes.Count > 1 && obj.Vertexes[^1] == obj.Vertexes[^2])
        {
            obj.Vertexes.RemoveAt(_currentVertexIndex);
            _currentVertexIndex--;
        }
    }

    private PlacingStep<LineViewModel> AddVertexStepTransition(LineViewModel obj) => _addVertexStep;
}