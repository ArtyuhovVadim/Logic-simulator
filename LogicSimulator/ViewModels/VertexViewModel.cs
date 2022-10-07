using LogicSimulator.ViewModels.Base;
using SharpDX;

namespace LogicSimulator.ViewModels;

public class VertexViewModel : BindableBase
{
    #region Position

    private Vector2 _position = Vector2.Zero;

    public Vector2 Position
    {
        get => _position;
        set => Set(ref _position, value);
    }

    #endregion

    #region Index

    private int _index;

    public int Index
    {
        get => _index;
        set => Set(ref _index, value);
    }

    #endregion
}