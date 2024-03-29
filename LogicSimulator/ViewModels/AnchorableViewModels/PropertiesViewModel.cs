﻿using LogicSimulator.ViewModels.AnchorableViewModels.Base;
using LogicSimulator.ViewModels.EditorViewModels.Base;

namespace LogicSimulator.ViewModels.AnchorableViewModels;

public class PropertiesViewModel : ToolViewModel
{
    public override string Title => "Свойства";

    #region CurrentEditorViewModel

    private EditorViewModel? _currentEditorViewModel;

    public EditorViewModel? CurrentEditorViewModel
    {
        get => _currentEditorViewModel;
        set => Set(ref _currentEditorViewModel, value);
    }

    #endregion
}