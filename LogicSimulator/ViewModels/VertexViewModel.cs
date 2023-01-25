using LogicSimulator.ViewModels.Base;
using SharpDX;

namespace LogicSimulator.ViewModels;

public class VertexViewModel : BindableBase
{
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
        set
        {
            if (Set(ref _x, value))
            {
                OnPropertyChanged(nameof(Position));
            }
        }
    }

    #endregion

    #region Y

    private float _y;

    public float Y
    {
        get => _y;
        set
        {
            if (Set(ref _y, value))
            {
                OnPropertyChanged(nameof(Position));
            }
        }
    }

    #endregion

    #region Position

    public Vector2 Position
    {
        get => new(_x, _y);
        set
        {
            X = value.X;
            Y = value.Y;
        }
    }

    #endregion
}