using LogicSimulator.Models;

namespace LogicSimulator.Infrastructure.Services.Interfaces;

public interface ISchemeFileService
{
    Task SaveToFileAsync(string path, Scheme scheme);

    Task<Scheme> ReadFromFileAsync(string path);
}