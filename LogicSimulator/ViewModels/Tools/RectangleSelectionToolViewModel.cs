using LogicSimulator.Infrastructure.Tools;
using LogicSimulator.Shared.ExtensionMethods;
using LogicSimulator.Shared.Models;
using LogicSimulator.ViewModels.Anchorable;
using SharpDX;
using SharpDX.Direct2D1;

namespace LogicSimulator.ViewModels.Tools;

public class RectangleSelectionToolViewModel : BaseToolViewModel
{
    private readonly SchemeViewModel _scheme;
    private bool _isSelectionChanged;

    public RectangleSelectionToolViewModel(SchemeViewModel scheme) => _scheme = scheme;

    public event Action? SelectedObjectsChanged;

    public Key MultipleSelectionKey { get; set; } = Key.LeftShift;

    #region IsSelectionStarted

    private bool _isSelectionStarted;

    public bool IsSelectionStarted
    {
        get => _isSelectionStarted;
        set => Set(ref _isSelectionStarted, value);
    }

    #endregion

    #region StartPosition

    private Vector2 _startPosition;

    public Vector2 StartPosition
    {
        get => _startPosition;
        set => Set(ref _startPosition, value);
    }

    #endregion

    #region EndPosition

    private Vector2 _endPosition;

    public Vector2 EndPosition
    {
        get => _endPosition;
        set => Set(ref _endPosition, value);
    }

    #endregion

    protected override void OnKeyDown(KeyInputArgs args)
    {
        if (args.Key != CanselKey) return;

        ToolSwitcher.SwitchToDefaultTool(true);
    }

    protected override void OnMouseLeftButtonDown(InputArgs args)
    {
        StartPosition = args.Position;
        EndPosition = args.Position;
        IsSelectionStarted = true;
    }

    protected override void OnMouseLeftButtonDragged(DragInputArgs args)
    {
        EndPosition = args.Position;
    }

    protected override void OnMouseLeftButtonUp(InputArgs args)
    {
        var isManySelectionKeyDown = Keyboard.IsKeyDown(MultipleSelectionKey);

        if (!isManySelectionKeyDown)
        {
            foreach (var obj in _scheme.Objects)
                obj.IsSelected = false;
            _isSelectionChanged = true;
        }

        var rect = new RectangleF(StartPosition.X, StartPosition.Y, EndPosition.X - StartPosition.X, EndPosition.Y - StartPosition.Y);

        foreach (var (compareResult, obj) in _scheme.HitTester.HitTest<ISelectable>(rect.Normalize()))
        {
            if (compareResult is GeometryRelation.Disjoint or GeometryRelation.Unknown)
                continue;

            switch (EndPosition.X < StartPosition.X)
            {
                case true when compareResult is GeometryRelation.IsContained or GeometryRelation.Overlap:
                case false when compareResult is GeometryRelation.IsContained:
                    obj.Select();
                    _isSelectionChanged = true;
                    break;
            }
        }

        if (_isSelectionChanged)
            SelectedObjectsChanged?.Invoke();

        if (ActivatedFromOtherTool)
            ToolSwitcher.SwitchToDefaultTool(true);
        else
            IsSelectionStarted = false;
    }

    protected override void OnDeactivated() => IsSelectionStarted = false;
}