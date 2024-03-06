using LogicSimulator.Models;

namespace LogicSimulator.Infrastructure.Services.Interfaces;

public interface IProjectFileService
{
    public bool SaveToFile(string path, Project project);

    public bool ReadFromFile(string path, out Project? project);
}