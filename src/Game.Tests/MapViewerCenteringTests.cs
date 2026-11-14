using Aow.Game.Draw;
using Aow.Game.World.Map;

namespace Aow.Game.Tests;

// Port extension: a scene larger than the map shows the map centered with black around it. The
// scroll clamp, the view port and the mouse coordinates stay the original's.
public sealed class MapViewerCenteringTests
{
    // A map of 10 by 8 hexagons: MaxXhp 336 and MaxYhp 272, so the clamp shows 304 by 240 pixels.
    private const int MapWidth = 10;
    private const int MapHeight = 8;
    private const int SceneLeft = 10;
    private const int SceneTop = 10;
    private const int Stale = 0x03E0;
    private const int White = 0x7FFF;
    private const int Red = 0x7C00;

    // Draws white over the whole scene and a red column and row on the map's left and top edges.
    private sealed class PaintedMap() : GameMap(null!)
    {
        public override void UpdateStaticScene() => StaticSceneInvalid = false;
        public override void UpdateDynamicScene() => DynamicSceneInvalid = false;

        public override void DrawDynamicScene(ref LockedSurface lockedSurface)
        {
        }

        public override void DrawStaticScene(ref LockedSurface lockedSurface)
        {
            PixelFormats.ColorFillSurfaceDescription(new Rect(0, 0, lockedSurface.Width, lockedSurface.Height), 0xFFFFFF, lockedSurface);
            PixelFormats.ColorFillSurfaceDescription(new Rect(16 - ViewPixelX, 0, 17 - ViewPixelX, lockedSurface.Height), 0x0000FF, lockedSurface);
            PixelFormats.ColorFillSurfaceDescription(new Rect(0, 16 - ViewPixelY, lockedSurface.Width, 17 - ViewPixelY), 0x0000FF, lockedSurface);
        }
    }

    private sealed class Fixture : IDisposable
    {
        public PaintedMap Map { get; } = new();
        public HexSpriteMapViewer Viewer { get; } = new(null!);

        private readonly DataSurface _target = new(420, 320, PixelFormats.pfRGB555, null!);

        public Fixture(int sceneWidth, int sceneHeight)
        {
            Map.MapContainer = new MapContainer(null!) { Holder = EngineFixture.Engine, Width = MapWidth, Height = MapHeight, LevelCount = 1 };
            Viewer.HexSpriteMap = Map;
            Viewer.SceneWidth = sceneWidth;
            Viewer.SceneHeight = sceneHeight;
            Viewer.ChangeScene(100, 30, 0);
            LockedSurface lockedSurface = default;
            Assert.Equal(PixelFormats.GFXE_OK, _target.Lock(ref lockedSurface));
            PixelFormats.ColorFillSurfaceDescription(new Rect(0, 0, lockedSurface.Width, lockedSurface.Height), 0x00FF00, lockedSurface);
            _target.Unlock();
            Viewer.ShowScene(SceneLeft, SceneTop, _target);
        }

        public int Pixel(int x, int y)
        {
            LockedSurface lockedSurface = default;
            _target.Lock(ref lockedSurface);
            var data = lockedSurface.Data;
            int at = lockedSurface.DataIndex + y * lockedSurface.Pitch + x * 2;
            int value = data[at] | data[at + 1] << 8;
            _target.Unlock();
            return value;
        }

        // Every target pixel: stale outside the scene, black outside the shown map, the map inside.
        public void AssertPicture(Aow.Rtl.Rect shown, int edgeX, int edgeY)
        {
            for (int y = 0; y < 320; y++)
            {
                for (int x = 0; x < 420; x++)
                {
                    int sx = x - SceneLeft;
                    int sy = y - SceneTop;
                    int expected;
                    if (sx < 0 || sy < 0 || sx >= Viewer.SceneWidth || sy >= Viewer.SceneHeight)
                        expected = Stale;
                    else if (sx < shown.Left || sx >= shown.Right || sy < shown.Top || sy >= shown.Bottom)
                        expected = 0;
                    else
                        expected = sx == edgeX || sy == edgeY ? Red : White;
                    if (Pixel(x, y) != expected)
                        Assert.Fail($"pixel {x},{y} is {Pixel(x, y):X4}, expected {expected:X4}");
                }
            }
        }

        public void Dispose()
        {
            Viewer.Dispose();
            Map.MapContainer?.Dispose();
            Map.Dispose();
            _target.Dispose();
        }
    }

    [Fact]
    public void WiderSceneCentersTheMapHorizontally()
    {
        using var fixture = new Fixture(400, 200);
        var viewer = fixture.Viewer;
        Assert.True(viewer.GetSceneExceedsMap());
        // The original clamp: MaxXhp - SceneWidth - 16, and SceneY as asked.
        Assert.Equal(336 - 400 - 16, viewer.SceneX);
        Assert.Equal(30, viewer.SceneY);
        Assert.Equal(-80, fixture.Map.ViewPixelX);
        Assert.Equal(new Rect(96, 0, 400, 200), viewer.GetMapSceneRectangle());
        Assert.Equal(new Rect(48, 0, 352, 200), viewer.GetMapDisplayRectangle());
        fixture.AssertPicture(new Rect(48, 0, 352, 200), 48, -1);

        int x = 48, y = 5;
        Assert.True(viewer.SceneToMapMouse(ref x, ref y));
        Assert.Equal((96, 5), (x, y));
        x = 47;
        Assert.False(viewer.SceneToMapMouse(ref x, ref y));
        x = 351;
        y = 5;
        Assert.True(viewer.SceneToMapMouse(ref x, ref y));
        Assert.Equal(399, x);
        // The first black column right of the map is outside.
        x = 352;
        Assert.False(viewer.SceneToMapMouse(ref x, ref y));

        // The scene point 390 lies past MaxXhp 336 and inside the map at 310.
        var mouse = fixture.Map.Mouse;
        mouse.Move(default, 390, 5);
        Assert.Equal(310, mouse.GetPixelX());
        Assert.Equal(35, mouse.GetPixelY());
    }

    [Fact]
    public void LargerSceneCentersTheMapOnBothAxes()
    {
        using var fixture = new Fixture(400, 300);
        var viewer = fixture.Viewer;
        Assert.True(viewer.GetSceneExceedsMap());
        Assert.Equal(-80, viewer.SceneX);
        Assert.Equal(272 - 300 - 16, viewer.SceneY);
        Assert.Equal(new Rect(96, 60, 400, 300), viewer.GetMapSceneRectangle());
        Assert.Equal(new Rect(48, 30, 352, 270), viewer.GetMapDisplayRectangle());
        fixture.AssertPicture(new Rect(48, 30, 352, 270), 48, 30);

        int x = 48, y = 29;
        Assert.False(viewer.SceneToMapMouse(ref x, ref y));
        y = 30;
        Assert.True(viewer.SceneToMapMouse(ref x, ref y));
        Assert.Equal((96, 60), (x, y));
    }

    [Fact]
    public void SceneInsideTheMapKeepsTheOriginalClampAndPicture()
    {
        using var fixture = new Fixture(300, 200);
        var viewer = fixture.Viewer;
        Assert.False(viewer.GetSceneExceedsMap());
        Assert.Equal(336 - 300 - 16, viewer.SceneX);
        Assert.Equal(30, viewer.SceneY);
        viewer.SceneX = 0;
        Assert.Equal(16, viewer.SceneX);
        viewer.SceneX = 20;
        Assert.Equal(new Rect(0, 0, 300, 200), viewer.GetMapSceneRectangle());
        Assert.Equal(new Rect(0, 0, 300, 200), viewer.GetMapDisplayRectangle());
        Assert.Null(viewer.CenteredSurface);
        fixture.AssertPicture(new Rect(0, 0, 300, 200), -4, -1);

        int x = 299, y = 199;
        Assert.True(viewer.SceneToMapMouse(ref x, ref y));
        Assert.Equal((299, 199), (x, y));
    }

    // The original's MaxYhp reaches past the last row, so a point at the bottom of a view scrolled
    // to the bottom has a column hex and no row hex. Readers test both before taking a field.
    [Fact]
    public void TheBottomEdgeHasPointsWithAColumnAndNoRow()
    {
        using var fixture = new Fixture(300, 200);
        fixture.Viewer.ChangeScene(100, 1000, 0);
        var mouse = fixture.Map.Mouse;
        bool found = false;
        for (int x = 0; x < 300 && !found; x++)
        {
            mouse.Move(default, x, 200);
            found = mouse.HexX != -1 && mouse.HexY == -1;
        }
        Assert.True(found);
    }
}
