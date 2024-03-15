﻿using LogicSimulator.Utils;
using LogicSimulator.ViewModels.AnchorableViewModels;
using LogicSimulator.ViewModels.ObjectViewModels.Base;
using LogicSimulator.ViewModels.Tools.Base;
using SharpDX;

namespace LogicSimulator.ViewModels.Tools;

public class ObjectPlacingToolViewModel<T> : BasePlacingToolViewModel<T> where T : BaseObjectViewModel
{
    public ObjectPlacingToolViewModel(SchemeViewModel scheme, Func<T> objectFactory) : base(scheme, objectFactory) => 
        FirstStep = new PlacingStep<T>(UpdateLocation, null, UpdateLocation, _ => null);

    protected override PlacingStep<T> FirstStep { get; }

    private void UpdateLocation(T obj, Vector2 pos) => obj.Location = pos.ApplyGrid(Scheme.GridStep);
}