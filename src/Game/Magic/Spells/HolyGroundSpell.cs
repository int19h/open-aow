// Translated from HolyG.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;
using Aow.Game.Commands;
using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using Aow.Game.World.Structures;

using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

public partial class HolyGroundSpell : GlobalTargetSpell
{
    public HolyGroundSpell(ECustomNode owner) : base(owner)
    {
        FSpellName = AbilityRules.TranslateRStr(HolyGroundsRStr);
        FSpellID = 13;
        FBaseManaCost = 5;
        FSpellLevel = 2;
        FMagicSphere = MagicSphere.Life;
        FAboveGroundOnly = false;
    }

    // HolyG.pas:339

    // HolyG.pas:375
    public override void SetupTargetSelection(AbstractUnit Source, TargetSelectionControl TargetSelection)
    {
        base.SetupTargetSelection(Source, TargetSelection);
        TargetSelection.FHNs.Clear();
        for (int I = 0, last = HexGeometry.RadToHN(2) - 1; I <= last; I++)
        {
            TargetSelection.FHNs.Add(0);
        }
    }

    // HolyG.pas:384
    public override bool TargetSelected(AbstractUnit Source, int X, int Y, int L)
    {
        HolyGroundSpellTE Event;
        Event = new HolyGroundSpellTE(null);
        Event.Initialize(FSpellID, (byte)Source.FPlayer, Source.FUnitID, FBaseManaCost);
        Event.SetLocation((sbyte)X, (sbyte)Y, (sbyte)L);
        AoWHSMap.Current.FTokenManager.GetTokenControl(Source.FPlayer).AddEvent(Event);
        Event.Release();
        return true;
    }

    // HolyG.pas:360
    public override bool ValidTargetSelectionMapF(MapField Field)
    {
        ArmyHS Army;
        bool Result = base.ValidTargetMapF(Field) && Set<byte>.WithBounds(0, 255).With(12).WithRange(1, 5).Contains((byte)Field.TerrainType);
        // { Original overlay test allows these overlays even when the terrain check failed. }
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

    // HolyG.pas:350
    public override bool ValidTargetMapF(MapField Field)
    {
        bool Result = base.ValidTargetMapF(Field) && Set<byte>.WithBounds(0, 255).With(12).WithRange(1, 5).Contains((byte)Field.TerrainType);
        // { Original overlay test allows these overlays even when the terrain check failed. }
        Result = (Result && Field.OverlayType == -1) || Set<byte>.WithBounds(0, 255).With(9).WithRange(1, 4).Contains((byte)Field.OverlayType);
        return Result && Field.FindNoneTransparentHS(typeof(Structure)) == null && ((AoWMapField)Field).GetArmyHS() == null;
    }

    private static readonly string HolyGroundsRStr = "Holy Woods";

    // HolyG.pas:63

    // HolyG.pas:63

}
