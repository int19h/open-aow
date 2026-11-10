// Translated from UnHolyG.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;
using Aow.Game.Commands;
using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using Aow.Game.World.Structures;

using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

public partial class UnholyGroundSpell : GlobalTargetSpell
{
    public UnholyGroundSpell(ECustomNode owner) : base(owner)
    {
        FSpellName = AbilityRules.TranslateRStr(UnholyGroundsRStr);
        FSpellID = 14;
        FBaseManaCost = 5;
        FSpellLevel = 2;
        FMagicSphere = MagicSphere.Death;
        FAboveGroundOnly = false;
    }

    // UnHolyG.pas:349

    // UnHolyG.pas:383
    public override void SetupTargetSelection(AbstractUnit Source, TargetSelectionControl TargetSelection)
    {
        base.SetupTargetSelection(Source, TargetSelection);
        TargetSelection.FHNs.Clear();
        for (int I = 0, last = HexGeometry.RadToHN(2) - 1; I <= last; I++)
        {
            TargetSelection.FHNs.Add(0);
        }
    }

    // UnHolyG.pas:392
    public override bool TargetSelected(AbstractUnit Source, int X, int Y, int L)
    {
        UnholyGroundSpellTE Event;
        Event = new UnholyGroundSpellTE(null);
        Event.Initialize(FSpellID, (byte)Source.FPlayer, Source.FUnitID, FBaseManaCost);
        Event.SetLocation((sbyte)X, (sbyte)Y, (sbyte)L);
        AoWHSMap.Current.FTokenManager.GetTokenControl(Source.FPlayer).AddEvent(Event);
        Event.Release();
        return true;
    }

    // UnHolyG.pas:368
    public override bool ValidTargetSelectionMapF(MapField Field)
    {
        ArmyHS Army;
        bool Result = base.ValidTargetMapF(Field) && Set<byte>.WithBounds(0, 255).With(12).WithRange(1, 5).Contains((byte)Field.TerrainType);
        // { Original precedence accepts these overlays without the terrain test. }
        Result = (Result && Field.OverlayType == -1) || Set<byte>.WithBounds(0, 255).With(9).WithRange(1, 4).Contains((byte)Field.OverlayType);
        Result = Result && Field.FindNoneTransparentHS(typeof(Structure)) == null;
        if (Result)
        {
            Army = ((AoWMapField)Field).GetArmyHS();
            if (Army != null)
            {
                Result = !Army.VisibleForSeatedPlayer();
            }
        }

        return Result;
    }

    // UnHolyG.pas:360
    public override bool ValidTargetMapF(MapField Field)
    {
        bool Result = base.ValidTargetMapF(Field) && Set<byte>.WithBounds(0, 255).With(12).WithRange(1, 5).Contains((byte)Field.TerrainType);
        // { Original precedence accepts these overlays without the terrain test. }
        Result = (Result && Field.OverlayType == -1) || Set<byte>.WithBounds(0, 255).With(9).WithRange(1, 4).Contains((byte)Field.OverlayType);
        return Result && Field.FindNoneTransparentHS(typeof(Structure)) == null && ((AoWMapField)Field).GetArmyHS() == null;
    }

    private static readonly string UnholyGroundsRStr = "Evil Woods";

    // UnHolyG.pas:62

    // UnHolyG.pas:62

}
