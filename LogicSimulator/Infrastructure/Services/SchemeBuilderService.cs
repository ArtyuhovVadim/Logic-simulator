using System.Diagnostics;
using LogicSimulator.Core;
using LogicSimulator.Infrastructure.SchemeValidation.Base;
using LogicSimulator.Infrastructure.Services.Interfaces;
using LogicSimulator.Models.Logic;
using LogicSimulator.ViewModels.Anchorable;
using Microsoft.Extensions.Logging;

namespace LogicSimulator.Infrastructure.Services;

public class SchemeBuilderService : ISchemeBuilderService
{
    private readonly ILogger<SchemeBuilderService> _logger;
    private readonly ISchemePreprocessorService _preprocessorService;
    private readonly ISchemeValidationService _schemeValidationService;

    public SchemeBuilderService(ISchemePreprocessorService preprocessorService, ISchemeValidationService schemeValidationService, ILogger<SchemeBuilderService> logger)
    {
        _preprocessorService = preprocessorService;
        _schemeValidationService = schemeValidationService;
        _logger = logger;
    }

    public void AddValidationRule(ISchemeValidationRule rule) => _schemeValidationService.AddValidationRule(rule);

    public LogicScheme BuildFromSchemeViewModel(SchemeViewModel scheme)
    {
        try
        {
            var sw = Stopwatch.StartNew();
            _logger.LogInformation("Scheme building has been started.");

            var objectModels = scheme.Objects.Select(x => x.Model).ToList();
            var preprocessedLogicScheme = _preprocessorService.Process(objectModels);
            var validationResults = _schemeValidationService.Validate(scheme, preprocessedLogicScheme);

            if (validationResults.Any(x => x.IsCritical))
            {
                _logger.LogError("Scheme building has been stopped because of errors.");
                return new LogicScheme(preprocessedLogicScheme, validationResults);
            }

            if (validationResults.Any(x => x is { Level: ValidationRuleLevel.Warning, IsValid: false }))
            {
                _logger.LogWarning("Scheme has been validated with some warnings.");
            }

            var connections = new List<Connection>();

            foreach (var group in preprocessedLogicScheme.Edges.GroupBy(x => x.Source))
            {
                var outputPort = (OutputPort)group.Key.LogicModel;
                var inputPorts = group.Select(x => x.Recipient.LogicModel).Cast<InputPort>();
                var connection = new Connection(outputPort, inputPorts);
                connections.Add(connection);
            }

            sw.Stop();
            _logger.LogInformation("Scheme has been successfully built in {ms:0.000} ms.", sw.Elapsed.TotalMilliseconds);

            return new LogicScheme(preprocessedLogicScheme, connections, validationResults);
        }
        catch (Exception e)
        {
            _logger.LogError("Unexpected error while building scheme:\n{e}", e);
            throw;
        }
    }
}