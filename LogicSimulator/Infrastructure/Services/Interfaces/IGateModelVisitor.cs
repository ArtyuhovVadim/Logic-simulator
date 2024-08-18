using LogicSimulator.Models.Logic.Gates;

namespace LogicSimulator.Infrastructure.Services.Interfaces;

public interface IGateModelVisitor
{
    void Visit(InputGateModel gate);

    void Visit(OutputGateModel gate);

    void Visit(AndGateModel gate);
}