using LogicSimulator.Models;

namespace LogicSimulator.Infrastructure.Services.Interfaces;

public interface ISchemeFileService
{
    bool SaveToFile(string path, Scheme scheme);

    bool ReadFromFile(string path, out Scheme? scheme);
}