using LogicSimulator.Models.Logic;
using LogicSimulator.ViewModels.Anchorable;
using WpfExtensions.Mvvm.Messaging;

namespace LogicSimulator.Infrastructure.SchemeValidation.Base;

public record ValidationContext(SchemeViewModel Scheme, IPreprocessedLogicScheme PreprocessedScheme, IMessageBus Bus);