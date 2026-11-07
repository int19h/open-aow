using Aow.Units.GFXE;
using Aow.Units.HSEngine;
using RtlApi = Aow.Rtl.RtlUnit;
using G = Aow.Units.GFXE.GFXEUnit;

namespace Aow.Units.Tests;

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
    private sealed class PaintedMap() : THSMap(null!)
    {
        public override void UpdateStaticScene() => FStaticSceneInvalid = false;
        public override void UpdateDynamicScene() => FDynamicSceneInvalid = false;
        public override void DrawDynamicScene(ref TSurfaceDesc SD)
        {
        }

        public override void DrawStaticScene(ref TSurfaceDesc SD)
        {
            G.ColorFillSD(RtlApi.Rect(0, 0, SD.Width, SD.Height), 0xFFFFFF, SD);
            G.ColorFillSD(RtlApi.Rect(16 - FViewXhp, 0, 17 - FViewXhp, SD.Height), 0x0000FF, SD);
            G.ColorFillSD(RtlApi.Rect(0, 16 - FViewYhp, SD.Width, 17 - FViewYhp), 0x0000FF, SD);
        }
    }

    private sealed class Fixture : IDisposable
    {
        public readonly PaintedMap Map = new();
        public readonly THSMapViewer Viewer = new(null!);
        public readonly TDataSurface Target = new(420, 320, G.pfRGB555, null!);

        public Fixture(int sceneWidth, int sceneHeight)
        {
            Map.FMapContainer = new TMapContainer(null!) { FWidth = MapWidth, FHeight = MapHeight, FLevelCount = 1 };
            Viewer.HSMap = Map;
            Viewer.SceneWidth = sceneWidth;
            Viewer.SceneHeight = sceneHeight;
            Viewer.ChangeScene(100, 30, 0);
            TSurfaceDesc sd = default;
            Assert.Equal(G.GFXE_OK, Target.Lock(ref sd));
            G.ColorFillSD(RtlApi.Rect(0, 0, sd.Width, sd.Height), 0x00FF00, sd);
            Target.Unlock();
            Viewer.ShowScene(SceneLeft, SceneTop, Target);
        }

        public int Pixel(int x, int y)
        {
            TSurfaceDesc sd = default;
            Target.Lock(ref sd);
            var data = RtlApi.BytePointer(sd.Data);
            int at = y * sd.Pitch + x * 2;
            int value = data[at] | data[at + 1] << 8;
            Target.Unlock();
            return value;
        }

        // Every target pixel: stale outside the scene, black outside the shown map, the map inside.
        public void AssertPicture(Aow.Rtl.TRect shown, int edgeX, int edgeY)
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
            Viewer.Destroy();
            Map.FMapContainer.Free();
            Map.Destroy();
            Target.Destroy();
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
        Assert.Equal(-80, fixture.Map.ViewXhp);
        Assert.Equal(RtlApi.Rect(96, 0, 400, 200), viewer.GetMapSceneRect());
        Assert.Equal(RtlApi.Rect(48, 0, 352, 200), viewer.GetMapDisplayRect());
        fixture.AssertPicture(RtlApi.Rect(48, 0, 352, 200), 48, -1);

        int x = 48, y = 5;
        Assert.True(viewer.SceneToMapMouse(ref x, ref y));
        Assert.Equal((96, 5), (x, y));
        x = 47;
        Assert.False(viewer.SceneToMapMouse(ref x, ref y));
        x = 352;
        y = 5;
        Assert.True(viewer.SceneToMapMouse(ref x, ref y));
        Assert.Equal(400, x);

        // The scene point 390 lies past MaxXhp 336 and inside the map at 310.
        var mouse = fixture.Map.Mouse;
        mouse.Move(default, 390, 5);
        Assert.Equal(310, mouse.GetXhp());
        Assert.Equal(35, mouse.GetYhp());
    }

    [Fact]
    public void LargerSceneCentersTheMapOnBothAxes()
    {
        using var fixture = new Fixture(400, 300);
        var viewer = fixture.Viewer;
        Assert.True(viewer.GetSceneExceedsMap());
        Assert.Equal(-80, viewer.SceneX);
        Assert.Equal(272 - 300 - 16, viewer.SceneY);
        Assert.Equal(RtlApi.Rect(96, 60, 400, 300), viewer.GetMapSceneRect());
        Assert.Equal(RtlApi.Rect(48, 30, 352, 270), viewer.GetMapDisplayRect());
        fixture.AssertPicture(RtlApi.Rect(48, 30, 352, 270), 48, 30);

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
        Assert.Equal(RtlApi.Rect(0, 0, 300, 200), viewer.GetMapSceneRect());
        Assert.Equal(RtlApi.Rect(0, 0, 300, 200), viewer.GetMapDisplayRect());
        Assert.Null(viewer.FCenteredSurface);
        fixture.AssertPicture(RtlApi.Rect(0, 0, 300, 200), -4, -1);

        int x = 299, y = 199;
        Assert.True(viewer.SceneToMapMouse(ref x, ref y));
        Assert.Equal((299, 199), (x, y));
    }
}
