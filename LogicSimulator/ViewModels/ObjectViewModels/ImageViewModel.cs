﻿using System.Windows.Media;
using LogicSimulator.Infrastructure;
using LogicSimulator.ViewModels.ObjectViewModels.Base;

namespace LogicSimulator.ViewModels.ObjectViewModels;

public class ImageViewModel : BaseEditableObjectViewModel
{
    #region FilePath

    private string _filePath = string.Empty;
    
    [Editable]
    public string FilePath
    {
        get => _filePath;
        set => Set(ref _filePath, value);
    }

    #endregion

    #region Width

    private float _width;
    
    [Editable]
    public float Width
    {
        get => _width;
        set => Set(ref _width, value);
    }

    #endregion

    #region Height

    private float _height;
    
    [Editable]
    public float Height
    {
        get => _height;
        set => Set(ref _height, value);
    }

    #endregion

    #region StrokeColor

    private Color _strokeColor = Colors.Black;
    
    [Editable]
    public Color StrokeColor
    {
        get => _strokeColor;
        set => Set(ref _strokeColor, value);
    }

    #endregion

    #region StrokeThickness

    private float _strokeThickness = 1f;
    
    [Editable]
    public float StrokeThickness
    {
        get => _strokeThickness;
        set => Set(ref _strokeThickness, value);
    }

    #endregion

    #region IsBordered

    private bool _isBordered = true;
    
    [Editable]
    public bool IsBordered
    {
        get => _isBordered;
        set => Set(ref _isBordered, value);
    }

    #endregion
}