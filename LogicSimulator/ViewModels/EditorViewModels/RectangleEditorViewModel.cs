using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices.ComTypes;
using LogicSimulator.Scene;
using LogicSimulator.ViewModels.Base;
using LogicSimulator.ViewModels.EditorViewModels.Base;
using SharpDX;
using Rectangle = LogicSimulator.Scene.SceneObjects.Rectangle;

namespace LogicSimulator.ViewModels.EditorViewModels;

public class ObjectPropertyViewModel : BindableBase
{
    #region objectPropertyName

    private string _objectPropertyName;

    public string ObjectPropertyName
    {
        get => _objectPropertyName;
        set => Set(ref _objectPropertyName, value);
    }

    #endregion

    #region ObjectPropertyType

    public Type ObjectPropertyType { get; }

    #endregion

    public ObjectPropertyViewModel(string objectPropertyObjectPropertyName, Type objectPropertyObjectPropertyType)
    {
        _objectPropertyName = objectPropertyObjectPropertyName;
        ObjectPropertyType = objectPropertyObjectPropertyType;
    }
}

public class EditorRowViewModel : BindableBase
{
    #region Name

    private string _name = string.Empty;

    public string Name
    {
        get => _name;
        set => Set(ref _name, value);
    }

    #endregion

    #region ObjectProperties

    private readonly ObservableCollection<ObjectPropertyViewModel> _objectProperties;

    public IEnumerable<ObjectPropertyViewModel> ObjectProperties => _objectProperties;

    #endregion

    public EditorRowViewModel(ObjectPropertyViewModel objectProperty)
    {
        _objectProperties = new ObservableCollection<ObjectPropertyViewModel> { objectProperty };
    }

    public EditorRowViewModel(string name, ObjectPropertyViewModel objectProperty)
    {
        _name = name;
        _objectProperties = new ObservableCollection<ObjectPropertyViewModel> { objectProperty };
    }

    public EditorRowViewModel(IEnumerable<ObjectPropertyViewModel> objectProperties)
    {
        _objectProperties = new ObservableCollection<ObjectPropertyViewModel>(objectProperties);
    }

    public EditorRowViewModel(string name, IEnumerable<ObjectPropertyViewModel> objectProperties)
    {
        _name = name;
        _objectProperties = new ObservableCollection<ObjectPropertyViewModel>(objectProperties);
    }
}

public class EditorGroupViewModel : BindableBase
{
    #region Name

    private string _name;

    public string Name
    {
        get => _name;
        set => Set(ref _name, value);
    }

    #endregion

    #region EditorRows

    private readonly ObservableCollection<EditorRowViewModel> _editorRows;

    public IEnumerable<EditorRowViewModel> EditorRows => _editorRows;

    #endregion

    public EditorGroupViewModel(string name, IEnumerable<EditorRowViewModel> editorRows)
    {
        _name = name;
        _editorRows = new ObservableCollection<EditorRowViewModel>(editorRows);
    }
}

public class EditorLayoutViewModel : BindableBase
{
    #region ObjectName

    private string _objectName;

    public string ObjectName
    {
        get => _objectName;
        set => Set(ref _objectName, value);
    }

    #endregion

    #region Groups

    private readonly ObservableCollection<EditorGroupViewModel> _groups;

    public IEnumerable<EditorGroupViewModel> Groups => _groups;

    #endregion

    public EditorLayoutViewModel(string objectName, IEnumerable<EditorGroupViewModel> groups)
    {
        _objectName = objectName;
        _groups = new ObservableCollection<EditorGroupViewModel>(groups);
    }
}

public class RectangleEditorViewModel : BaseEditorViewModel<Rectangle>
{
    public EditorLayoutViewModel Layout { get; }

    public RectangleEditorViewModel()
    {
        Layout = new EditorLayoutViewModel("Прямоугольник", new[]
        {
            new EditorGroupViewModel("Расположение", new []
            {
                new EditorRowViewModel("(X/Y)", new ObjectPropertyViewModel(nameof(Rectangle.Location), typeof(Vector2))),
                new EditorRowViewModel("Поворот", new ObjectPropertyViewModel(nameof(Rectangle.Rotation), typeof(float)))
            }),
            new EditorGroupViewModel("Свойства", new []
            {
                new EditorRowViewModel("Ширина", new ObjectPropertyViewModel(nameof(Rectangle.Width), typeof(float))),
                new EditorRowViewModel("Высота", new ObjectPropertyViewModel(nameof(Rectangle.Height), typeof(float))),
                new EditorRowViewModel("Граница", new ObjectPropertyViewModel[]
                {
                    new (nameof(Rectangle.StrokeThickness), typeof(float)),
                    new (nameof(Rectangle.StrokeColor), typeof(Color4))
                }),
                new EditorRowViewModel("Цвет заливки",new ObjectPropertyViewModel[]
                {
                    new (nameof(Rectangle.FillColor), typeof(Color4)),
                    new (nameof(Rectangle.IsFilled), typeof(bool))
                })
            })
        });
    }

    #region Location

    public Vector2 Location
    {
        get => Get<Vector2>();
        set => Set(value);
    }

    #endregion

    #region Rotation

    public float Rotation
    {
        get => Scene.Utils.RotationToInt(Get<Rotation>());
        set => Set(Scene.Utils.IntToRotation((int)value));
    }

    #endregion

    #region Width

    public float Width
    {
        get => Get<float>();
        set => Set(value);
    }

    #endregion

    #region Height

    public float Height
    {
        get => Get<float>();
        set => Set(value);
    }

    #endregion

    #region StrokeThickness

    public float StrokeThickness
    {
        get => Get<float>();
        set => Set(value);
    }

    #endregion

    #region StrokeColor

    public Color4 StrokeColor
    {
        get => Get<Color4>();
        set => Set(value);
    }

    #endregion

    #region IsFilled

    public bool IsFilled
    {
        get => Get<bool>();
        set => Set(value);
    }

    #endregion

    #region FillColor

    public Color4 FillColor
    {
        get => Get<Color4>();
        set => Set(value);
    }

    #endregion
}