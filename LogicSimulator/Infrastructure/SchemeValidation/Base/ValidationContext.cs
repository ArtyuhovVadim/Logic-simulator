using LogicSimulator.Models.Logic;
using LogicSimulator.ViewModels.Anchorable;

namespace LogicSimulator.Infrastructure.SchemeValidation.Base;

public record ValidationContext(SchemeViewModel Scheme, IPreprocessedLogicScheme PreprocessedScheme);