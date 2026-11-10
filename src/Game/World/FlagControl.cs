// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Draw;

using Aow.Game.Draw.Images;

using Aow.Game.Session;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.World;

// { Interface declaration run owned by P1. }
// race resources, flag library
public partial class FlagControl : EControlStyleObject
{

    public RaceResourceList FRaceResources;
    public CustomImageLibrary FFlags;

    // PlayerC.PAS:1920
    public override void Dispose()
    {
        AoWHSSet.Current.ImageLibraryManager.ReleaseIL(Set<ILManagerCategoryElement>.Of(ILManagerCategoryElement.Disk), "Flags.ILB");
        FFlags = null;
        base.Dispose();
    }

    // PlayerC.PAS:1909
    private protected FlagControl(ConstructionContext context, ECustomNode owner) : base(context, owner)
    {
        var gameSet = (AoWHSSet)owner;
        FFlags = gameSet.ImageLibraryManager.LinkToIL(Set<ILManagerCategoryElement>.Of(ILManagerCategoryElement.Disk), "Flags.ILB");
        this.FRaceResources = gameSet.FRaceResources;

        SetControlStyle(Set<EControlStyleElement>.Of(EControlStyleElement.FixedChild));
    }

    // PlayerC.PAS:1927
    public void ShowFlag(int X, int Y, int FrameOffset, byte FlagID, in SurfaceDesc SurfaceDesc)
    {
        FFlags[4].Show(X, Y, SurfaceDesc);
        FrameOffset = (int)(((uint)FrameOffset) >> 2) % 12;
        if ((FlagID & 32) != 0)
        {
            FFlags[FrameOffset + 22].Show(X - 21, Y - 16, SurfaceDesc);
        }
        else
        {
            FRaceResources.GetRaceResource(FlagID & 31).FImageLibrary[FrameOffset + 12].Show(X - 21, Y - 16, SurfaceDesc);
        }
    }

    // PlayerC.PAS:1937
    public void ShowFlagEx(int X, int Y, int FrameOffset, byte FlagID, in SurfaceDesc SurfaceDesc, bool ShowBase)
    {
        if (ShowBase)
        {
            FFlags[4].Show(X, Y, SurfaceDesc);
        }

        FrameOffset = (int)(((uint)FrameOffset) >> 2) % 12;
        if ((FlagID & 32) != 0)
        {
            FFlags[FrameOffset + 22].Show(X - 21, Y - 16, SurfaceDesc);
        }
        else
        {
            FRaceResources.GetRaceResource(FlagID & 31).FImageLibrary[FrameOffset + 12].Show(X - 21, Y - 16, SurfaceDesc);
        }
    }

    // PlayerC.PAS:1947
    public void ShowStructureFlag(int X, int Y, int FrameOffset, byte FlagID, in SurfaceDesc SurfaceDesc)
    {
        FFlags[(int)(((uint)FlagID) >> 6)].Show(X, Y, SurfaceDesc);
        FrameOffset = (int)(((uint)FrameOffset) >> 1) % 12;
        if ((FlagID & 32) != 0)
        {
            FFlags[FrameOffset + 10].Show(X - 21, Y - 16, SurfaceDesc);
        }
        else
        {
            FRaceResources.GetRaceResource(FlagID & 31).FImageLibrary[FrameOffset].Show(X - 21, Y - 16, SurfaceDesc);
        }
    }

    // PlayerC.PAS:1957
    public void ShowStructureFlagEx(int X, int Y, int FrameOffset, byte FlagID, in SurfaceDesc SurfaceDesc, bool ShowBase)
    {
        if (ShowBase)
        {
            FFlags[(int)(((uint)FlagID) >> 6)].Show(X, Y, SurfaceDesc);
        }

        FrameOffset = (int)(((uint)FrameOffset) >> 1) % 12;
        if ((FlagID & 32) != 0)
        {
            FFlags[FrameOffset + 10].Show(X - 21, Y - 16, SurfaceDesc);
        }
        else
        {
            FRaceResources.GetRaceResource(FlagID & 31).FImageLibrary[FrameOffset].Show(X - 21, Y - 16, SurfaceDesc);
        }
    }

    // PlayerC.PAS:1909
    public static FlagControl Create(ECustomNode owner) =>
        ConstructionContext.Create(owner, static (context, owner) => new FlagControl(context, owner ?? throw new NullReferenceException()));
}
