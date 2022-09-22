using System.Collections.Generic;
using LogicSimulator.ViewModels.EditorViewModels.Base;
using SharpDX;
using Rectangle = LogicSimulator.Scene.SceneObjects.Rectangle;

namespace LogicSimulator.ViewModels.EditorViewModels;

public class RectangleEditorViewModel : BaseEditorViewModel<Rectangle>
{
    #region UndefinedPropertiesMap

    public Dictionary<string, bool> UndefinedPropertiesMap { get; } = new()
    {
        { nameof(Width), false }
    };

    #endregion

    #region Width

    public float Width
    {
        get
        {
            UndefinedPropertiesMap[nameof(Width)] = false;

            foreach (var o in Objects)
            {
                if (!MathUtil.NearEqual(o.Width, FirstObject.Width))
                {
                    UndefinedPropertiesMap[nameof(Width)] = true;
                    break;
                }
            }

            OnPropertyChanged(nameof(UndefinedPropertiesMap));

            return FirstObject.Width;
        }
        set
        {
            if (UndefinedPropertiesMap[nameof(Width)]) return;

            foreach (var o in Objects)
            {
                o.Width = value;
            }
        }
    }

    #endregion

    protected override void NotifyPropertiesChange()
    {
        OnPropertyChanged(nameof(Width));
        OnPropertyChanged(nameof(UndefinedPropertiesMap));
    }
}