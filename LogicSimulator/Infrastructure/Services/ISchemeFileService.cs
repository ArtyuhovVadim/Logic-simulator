using LogicSimulator.Models;

namespace LogicSimulator.Infrastructure.Services;

public interface ISchemeFileService
{
    void SaveToFile(string path, Scheme scheme);

    Scheme ReadFromFile(string path);
}