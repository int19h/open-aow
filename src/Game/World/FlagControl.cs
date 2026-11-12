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
public partial class FlagControl : ControlStyleObject
{

    private readonly RaceResourceList _raceResources;

    public CustomImageLibrary Flags { get; set; }

    // PlayerC.PAS:1920
    public override void Dispose()
    {
        GamePack.Current.ImageLibraryManager.ReleaseImageLibrary(Set<ImageLibraryManagerCategoryElement>.Of(ImageLibraryManagerCategoryElement.Disk), "Flags.ILB");
        Flags = null;
        base.Dispose();
    }

    // PlayerC.PAS:1909
    private protected FlagControl(ConstructionContext context, ECustomNode owner) : base(context, owner)
    {
        var gameSet = (GamePack)owner;
        Flags = gameSet.ImageLibraryManager.LinkToImageLibrary(Set<ImageLibraryManagerCategoryElement>.Of(ImageLibraryManagerCategoryElement.Disk), "Flags.ILB");
        this._raceResources = gameSet.RaceResources;

        SetControlStyle(Set<ControlStyleElement>.Of(ControlStyleElement.FixedChild));
    }

    // PlayerC.PAS:1927
    public void ShowFlag(int X, int Y, int frameOffset, byte flagId, in LockedSurface lockedSurface)
    {
        Flags[4].Show(X, Y, lockedSurface);
        frameOffset = (int)(((uint)frameOffset) >> 2) % 12;
        if ((flagId & 32) != 0)
        {
            Flags[frameOffset + 22].Show(X - 21, Y - 16, lockedSurface);
        }
        else
        {
            _raceResources.GetRaceResource(flagId & 31).ImageLibrary[frameOffset + 12].Show(X - 21, Y - 16, lockedSurface);
        }
    }

    // PlayerC.PAS:1937
    public void ShowFlag(int X, int Y, int frameOffset, byte flagId, in LockedSurface lockedSurface, bool showBase)
    {
        if (showBase)
        {
            Flags[4].Show(X, Y, lockedSurface);
        }

        frameOffset = (int)(((uint)frameOffset) >> 2) % 12;
        if ((flagId & 32) != 0)
        {
            Flags[frameOffset + 22].Show(X - 21, Y - 16, lockedSurface);
        }
        else
        {
            _raceResources.GetRaceResource(flagId & 31).ImageLibrary[frameOffset + 12].Show(X - 21, Y - 16, lockedSurface);
        }
    }

    // PlayerC.PAS:1947
    public void ShowStructureFlag(int X, int Y, int frameOffset, byte flagId, in LockedSurface lockedSurface)
    {
        Flags[(int)(((uint)flagId) >> 6)].Show(X, Y, lockedSurface);
        frameOffset = (int)(((uint)frameOffset) >> 1) % 12;
        if ((flagId & 32) != 0)
        {
            Flags[frameOffset + 10].Show(X - 21, Y - 16, lockedSurface);
        }
        else
        {
            _raceResources.GetRaceResource(flagId & 31).ImageLibrary[frameOffset].Show(X - 21, Y - 16, lockedSurface);
        }
    }

    // PlayerC.PAS:1957
    public void ShowStructureFlag(int X, int Y, int frameOffset, byte flagId, in LockedSurface lockedSurface, bool showBase)
    {
        if (showBase)
        {
            Flags[(int)(((uint)flagId) >> 6)].Show(X, Y, lockedSurface);
        }

        frameOffset = (int)(((uint)frameOffset) >> 1) % 12;
        if ((flagId & 32) != 0)
        {
            Flags[frameOffset + 10].Show(X - 21, Y - 16, lockedSurface);
        }
        else
        {
            _raceResources.GetRaceResource(flagId & 31).ImageLibrary[frameOffset].Show(X - 21, Y - 16, lockedSurface);
        }
    }

    // PlayerC.PAS:1909
    public static FlagControl Create(ECustomNode owner) =>
        ConstructionContext.Create(owner, static (context, owner) => new FlagControl(context, owner ?? throw new NullReferenceException()));
}
