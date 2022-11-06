﻿using System.Windows;
using System.Windows.Controls;
using LogicSimulator.ViewModels;
using LogicSimulator.ViewModels.AnchorableViewModels;

namespace LogicSimulator.Infrastructure;

public class PanesTemplateSelector : DataTemplateSelector
{
    public DataTemplate SchemeViewTemplate { get; set; }

    public DataTemplate PropertiesViewTemplate { get; set; }

    public DataTemplate ProjectExplorerViewTemplate { get; set; }

    public override DataTemplate SelectTemplate(object item, DependencyObject container) => item switch
    {
        SchemeViewModel => SchemeViewTemplate,
        PropertiesViewModel => PropertiesViewTemplate, 
        ProjectExplorerViewModel => ProjectExplorerViewTemplate,
        _ => base.SelectTemplate(item, container)
    };
}