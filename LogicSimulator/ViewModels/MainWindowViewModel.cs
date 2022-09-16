using System.Collections.ObjectModel;
using System.Windows.Input;
using LogicSimulator.Infrastructure.Commands;
using LogicSimulator.Infrastructure.Services.Interfaces;
using LogicSimulator.ViewModels.Base;

namespace LogicSimulator.ViewModels;

public class MainWindowViewModel : BindableBase
{
    private readonly ISchemeFileService _schemeFileService;
    private readonly IUserDialogService _userDialogService;

    public MainWindowViewModel(ISchemeFileService schemeFileService, IUserDialogService userDialogService)
    {
        _schemeFileService = schemeFileService;
        _userDialogService = userDialogService;

        AnchorableViewModels.Add(new PropertiesViewModel { Name = "Test 1" });
        AnchorableViewModels.Add(new PropertiesViewModel { Name = "Test 2" });
        AnchorableViewModels.Add(new PropertiesViewModel { Name = "Test 3" });
    }

    #region Width

    private double _width = 0;

    public double Width
    {
        get => _width;
        set => Set(ref _width, value);
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
}