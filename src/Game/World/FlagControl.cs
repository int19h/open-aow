#nullable disable warnings
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
public class FlagControl : ControlStyleObject
{

    private readonly RaceResourceList _raceResources;

    public CustomImageLibrary? Flags { get; set; }

    // PlayerC.PAS:1920. The flags go back to the pack that owns this control, the one the
    // constructor linked them from (issue 335, second-engine ownership). A detached or already
    // disposed control has no pack to release from.
    public override void Dispose()
    {
        if (Owner is GamePack gamePack)
        {
            gamePack.ImageLibraryManager?.ReleaseImageLibrary(ImageLibraryManagerCategories.Disk, fileName: "Flags.ILB");
        }

        Flags = null;
        base.Dispose();
    }

    // PlayerC.PAS:1909
    private protected FlagControl(ConstructionContext context, CustomNode owner) : base(context, owner)
    {
        // Both creation paths pass the game set: GamePack.Create and a game set child load.
        GamePack gameSet = GetOwner<GamePack>();
        Flags = gameSet.ImageLibraryManager.LinkToImageLibrary(ImageLibraryManagerCategories.Disk, fileName: "Flags.ILB");
        _raceResources = gameSet.RaceResources;

        SetControlStyle(ControlStyles.FixedChild);
    }

    // PlayerC.PAS:1927
    public void ShowFlag(int X, int Y, int frameOffset, byte flagId, in LockedSurface lockedSurface)
    {
        Flags[4].Show(X, Y, lockedSurface);
        frameOffset = (frameOffset >>> 2) % 12;
        if ((flagId & 0b0010_0000) != 0)
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

        frameOffset = (frameOffset >>> 2) % 12;
        if ((flagId & 0b0010_0000) != 0)
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
        Flags[flagId >>> 6].Show(X, Y, lockedSurface);
        frameOffset = (frameOffset >>> 1) % 12;
        if ((flagId & 0b0010_0000) != 0)
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
            Flags[flagId >>> 6].Show(X, Y, lockedSurface);
        }

        frameOffset = (frameOffset >>> 1) % 12;
        if ((flagId & 0b0010_0000) != 0)
        {
            Flags[frameOffset + 10].Show(X - 21, Y - 16, lockedSurface);
        }
        else
        {
            _raceResources.GetRaceResource(flagId & 31).ImageLibrary[frameOffset].Show(X - 21, Y - 16, lockedSurface);
        }
    }

    // PlayerC.PAS:1909
    public static FlagControl Create(CustomNode? owner)
        => ConstructionContext.Create(owner, static (context, owner) => new FlagControl(context, owner: owner ?? throw new NullReferenceException()));
}
