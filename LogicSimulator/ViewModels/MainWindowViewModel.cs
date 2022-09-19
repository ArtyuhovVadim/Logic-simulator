﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using LogicSimulator.Infrastructure.Commands;
using LogicSimulator.Infrastructure.Services.Interfaces;
using LogicSimulator.ViewModels.Base;
using SharpDX;
using Rectangle = LogicSimulator.Scene.SceneObjects.Rectangle;

namespace LogicSimulator.ViewModels;

public class MainWindowViewModel : BindableBase
{
    private readonly ISchemeFileService _schemeFileService;
    private readonly IUserDialogService _userDialogService;

    public MainWindowViewModel(ISchemeFileService schemeFileService, IUserDialogService userDialogService, PropertiesViewModel propertiesViewModel)
    {
        _schemeFileService = schemeFileService;
        _userDialogService = userDialogService;

        AnchorableViewModels.Add(propertiesViewModel);
    }

    #region SomeDouble

    private double _someDouble = 1f;

    public double SomeDouble
    {
        get => _someDouble;
        set => Set(ref _someDouble, value);
    }

    #endregion

    #region ActiveContent

    private BindableBase _activeContent;

    public BindableBase ActiveContent
    {
        get => _activeContent;
        set => Set(ref _activeContent, value);
    }

    #endregion

    #region SchemeViewModels

    private ObservableCollection<SchemeViewModel> _schemeViewModels = new();

    public ObservableCollection<SchemeViewModel> SchemeViewModels
    {
        get => _schemeViewModels;
        private set => Set(ref _schemeViewModels, value);
    }

    #endregion

    #region AnchorableViewModels

    private ObservableCollection<BindableBase> _anchorableViewModels = new();

    public ObservableCollection<BindableBase> AnchorableViewModels
    {
        get => _anchorableViewModels;
        private set => Set(ref _anchorableViewModels, value);
    }

    #endregion

    #region LoadExampleCommand

    private ICommand _loadExampleCommand;

    public ICommand LoadExampleCommand => _loadExampleCommand ??= new LambdaCommand(_ =>
    {
        var path = "Data/Example.lss";

        if (!_schemeFileService.ReadFromFile(path, out var scheme))
        {
            _userDialogService.ShowErrorMessage("Ошибка загрузки файла", $"Не удалось загрузить файл: {path}");
            return;
        }

        SchemeViewModels.Add(new SchemeViewModel(scheme));
    }, _ => true);

    #endregion

    #region SaveExampleCommand

    private ICommand _saveExampleCommand;

    public ICommand SaveExampleCommand => _saveExampleCommand ??= new LambdaCommand(_ =>
    {
        //var path = "Data/Example.lss";
        //
        //if (!_schemeFileService.SaveToFile("Data/Example.lss", _scheme))
        //{
        //    _userDialogService.ShowErrorMessage("Ошибка сохранения файла", $"Не удалось сохранить файл: {path}");
        //}
    }, _ => true);

    #endregion

    #region TestCommand

    private ICommand _testCommand;

    public ICommand TestCommand => _testCommand ??= new LambdaCommand(_ =>
    {
        for (int i = 0; i < 500; i++)
        {
            SchemeViewModels.First().Objects.Add(new Rectangle() { Width = 100, Height = 100, Location = Random.Shared.NextVector2(new Vector2(0, 0), new Vector2(3000, 3000)) });
        }
    }, _ => true);

    #endregion
}