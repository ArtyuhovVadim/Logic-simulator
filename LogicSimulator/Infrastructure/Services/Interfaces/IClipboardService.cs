using LogicSimulator.ViewModels.Objects.Base;

namespace LogicSimulator.Infrastructure.Services.Interfaces;

public interface IClipboardService
{
    bool HasCopiedObjects { get; }

    void Copy(IEnumerable<BaseObjectViewModel> objects);

    List<BaseObjectViewModel> Paste();

    List<BaseObjectViewModel> Duplicate(IEnumerable<BaseObjectViewModel> objects);
}
