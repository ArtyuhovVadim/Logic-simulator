using LogicSimulator.Scene.Cache;
using SharpDX;

namespace LogicSimulator.Scene.DirectX;

public class D2DContext : DisposableObject
{
    private D2DResourceFactory _resourceFactory;
    private D2DDrawingContext _drawingContext;
    private ResourceCache _cache;

    public D2DResourceFactory ResourceFactory => _resourceFactory;
    public D2DDrawingContext DrawingContext => _drawingContext;
    public ResourceCache Cache => _cache;

    public D2DContext(DirectXContext context)
    {
        _resourceFactory = new D2DResourceFactory(context);
        _drawingContext = new D2DDrawingContext(context);
        _cache = new ResourceCache(_resourceFactory);
    }

    protected override void Dispose(bool disposingManaged)
    {
        if (disposingManaged)
        {
            Utilities.Dispose(ref _resourceFactory);
            Utilities.Dispose(ref _cache);
        }

        _drawingContext = null!;

        base.Dispose(disposingManaged);
    }
}