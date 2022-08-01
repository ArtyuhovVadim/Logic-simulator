using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using LogicSimulator.Infrastructure.Commands;
using LogicSimulator.Infrastructure.Services.Interfaces;
using LogicSimulator.Models;
using LogicSimulator.ViewModels.Base;

namespace LogicSimulator.ViewModels;

public class MainWindowViewModel : BindableBase
{
    private readonly ISchemeFileService _schemeFileService;
    private readonly IUserDialogService _userDialogService;

    private readonly ObservableCollection<Scheme> _schemes = new();

    public MainWindowViewModel(ISchemeFileService schemeFileService, IUserDialogService userDialogService)
    {
        _schemeFileService = schemeFileService;
        _userDialogService = userDialogService;

        for (var i = 0; i < 3; i++)
        {
            _schemeFileService.ReadFromFile("Data/Example.lss", out var scheme);

            scheme.Name += $" {i}";

            _schemes.Add(scheme);
        }

        _schemeViewModels = new ObservableCollection<SchemeViewModel>(_schemes.Select(x => new SchemeViewModel(x)));
    }

    #region SchemeViewModels

    private ObservableCollection<SchemeViewModel> _schemeViewModels;

    public ObservableCollection<SchemeViewModel> SchemeViewModels
    {
        get => _schemeViewModels;
        private set => Set(ref _schemeViewModels, value);
    }

    #endregion

    #region LoadExampleCommand

    private ICommand _loadExampleCommand;

    public ICommand LoadExampleCommand => _loadExampleCommand ??= new LambdaCommand(_ =>
    {
        //var path = "Data/Example.lss";
        //
        //if (!_schemeFileService.ReadFromFile(path, out _scheme))
        //{
        //    _userDialogService.ShowErrorMessage("Ошибка загрузки файла", $"Не удалось загрузить файл:{path}");
        //    return;
        //}
        //
        //Objects.Clear();
        //
        //foreach (var o in _scheme.Objects)
        //    Objects.Add(o);
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

    }, _ => true);

    #endregion
}