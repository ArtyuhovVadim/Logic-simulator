using LogicSimulator.Infrastructure.Services.Interfaces;
using LogicSimulator.ViewModels.Objects.Base;

namespace LogicSimulator.Infrastructure.Services;

public class ClipboardService : IClipboardService
{
    private List<BaseObjectViewModel> _buffer = [];

    public bool HasCopiedObjects => _buffer.Count > 0;

    public void Copy(IEnumerable<BaseObjectViewModel> objects) => _buffer = Clone(objects);

    public List<BaseObjectViewModel> Paste() => Clone(_buffer);

    public List<BaseObjectViewModel> Duplicate(IEnumerable<BaseObjectViewModel> objects) => Clone(objects);

    private List<BaseObjectViewModel> Clone(IEnumerable<BaseObjectViewModel> objects) => objects.Select(x => x.MakeClone()).ToList();
}