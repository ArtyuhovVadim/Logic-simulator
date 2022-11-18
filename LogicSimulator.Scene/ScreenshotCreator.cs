using System.IO;
using SharpDX;

namespace LogicSimulator.Scene;

public class ScreenshotCreator
{
    private readonly SceneRenderer _renderer;

    internal ScreenshotCreator(SceneRenderer renderer)
    {
        _renderer = renderer;
    }

    public void Create(Stream stream, int width, int height, bool isAliased = true, float scale = 1f, Vector2 translation = default)
    {
        _renderer.RenderToStream(stream, width, height, isAliased, scale, translation);
    }
}