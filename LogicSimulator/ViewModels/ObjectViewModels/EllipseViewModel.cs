﻿using System.Windows.Media;
using LogicSimulator.ViewModels.ObjectViewModels.Base;

namespace LogicSimulator.ViewModels.ObjectViewModels;

public class EllipseViewModel : BaseEditableObjectViewModel
{
    #region RadiusX

    private float _radiusX;

    public float RadiusX
    {
        get => _radiusX;
        set => Set(ref _radiusX, value);
    }

    #endregion

    #region RadiusY

    private float _radiusY;

    public float RadiusY
    {
        get => _radiusY;
        set => Set(ref _radiusY, value);
    }

    #endregion

    #region FillColor

    private Color _fillColor = Colors.White;

    public Color FillColor
    {
        get => _fillColor;
        set => Set(ref _fillColor, value);
    }

    #endregion

    #region StrokeColor

    private Color _strokeColor = Colors.Black;

    public Color StrokeColor
    {
        get => _strokeColor;
        set => Set(ref _strokeColor, value);
    }

    #endregion

    #region StrokeThickness

    private float _strokeThickness = 1f;

    public float StrokeThickness
    {
        get => _strokeThickness;
        set => Set(ref _strokeThickness, value);
    }

    #endregion

    #region IsFilled

    private bool _isFilled = true;

    public bool IsFilled
    {
        get => _isFilled;
        set => Set(ref _isFilled, value);
    }

    #endregion
}