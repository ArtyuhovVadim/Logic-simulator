using LogicSimulator.Models;

namespace LogicSimulator.Infrastructure.Services.Interfaces;

public interface IProjectFileService
{
    public bool ReadFromFile(string path, out Project? project);
}