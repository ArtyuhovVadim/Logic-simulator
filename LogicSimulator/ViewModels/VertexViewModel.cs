using LogicSimulator.ViewModels.Base;
using SharpDX;

namespace LogicSimulator.ViewModels;

public class VertexViewModel : BindableBase
{
    public VertexViewModel()
    {
        
    }

    public VertexViewModel(Vector2 pos, int index)
    {
        X = pos.X;
        Y = pos.Y;
        Index = index;
    }

    #region Index

    private int _index;

    public int Index
    {
        get => _index;
        set => Set(ref _index, value);
    }

    #endregion

    #region X

    private float _x;

    public float X
    {
        get => _x;
        set => Set(ref _x, value);
    }

    #endregion

    #region Y

    private float _y;

    public float Y
    {
        get => _y;
        set => Set(ref _y, value);
    }

    #endregion
}