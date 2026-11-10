// Translated from PoisonP.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;
using Aow.Game.Commands;
using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using Aow.Game.World.Structures;

using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

public partial class PoisonPlantSpell : GlobalTargetSpell
{
    public PoisonPlantSpell(ECustomNode owner) : base(owner)
    {
        FSpellName = AbilityRules.TranslateRStr(PoisonPlantsRStr);
        FSpellID = 12;
        FBaseManaCost = 5;
        FSpellLevel = 2;
        FMagicSphere = MagicSphere.Earth;
        FAboveGroundOnly = false;
    }

    // PoisonP.pas:371

    // PoisonP.pas:405
    public override void SetupTargetSelection(AbstractUnit Source, TargetSelectionControl TargetSelection)
    {
        base.SetupTargetSelection(Source, TargetSelection);
        TargetSelection.FHNs.Clear();
        for (int I = 0, last = HexGeometry.RadToHN(2) - 1; I <= last; I++)
        {
            TargetSelection.FHNs.Add(0);
        }
    }

    // PoisonP.pas:414
    public override bool TargetSelected(AbstractUnit Source, int X, int Y, int L)
    {
        PoisonPlantSpellTE Event;
        Event = new PoisonPlantSpellTE(null);
        Event.Initialize(FSpellID, (byte)Source.FPlayer, Source.FUnitID, FBaseManaCost);
        Event.SetLocation((sbyte)X, (sbyte)Y, (sbyte)L);
        AoWHSMap.Current.FTokenManager.GetTokenControl(Source.FPlayer).AddEvent(Event);
        Event.Release();
        return true;
    }

    // PoisonP.pas:390
    public override bool ValidTargetSelectionMapF(MapField Field)
    {
        ArmyHS ArmyHS;
        bool Result = base.ValidTargetSelectionMapF(Field) && Set<byte>.WithBounds(0, 255).With(12).WithRange(1, 5).Contains((byte)Field.TerrainType);
        // { Original precedence also accepts these overlays without the terrain test. }
        Result = (Result && Field.OverlayType == -1) || Set<byte>.WithBounds(0, 255).With(9).WithRange(1, 4).Contains((byte)Field.OverlayType);
        Result = Result && Field.FindNoneTransparentHS(typeof(Structure)) == null && ((AoWMapField)Field).GetArmyHS() == null;
        if (Result)
        {
            ArmyHS = ((AoWMapField)Field).GetArmyHS();
            if (ArmyHS != null)
            {
                Result = !ArmyHS.VisibleForSeatedPlayer();
            }
        }

        return Result;
    }

    // PoisonP.pas:382
    public override bool ValidTargetMapF(MapField Field)
    {
        bool Result = base.ValidTargetMapF(Field) && Set<byte>.WithBounds(0, 255).With(12).WithRange(1, 5).Contains((byte)Field.TerrainType);
        // { Original precedence also accepts these overlays without the terrain test. }
        Result = (Result && Field.OverlayType == -1) || Set<byte>.WithBounds(0, 255).With(9).WithRange(1, 4).Contains((byte)Field.OverlayType);
        return Result && Field.FindNoneTransparentHS(typeof(Structure)) == null && ((AoWMapField)Field).GetArmyHS() == null;
    }

    private static readonly string PoisonPlantsRStr = "Poison Plants";

    // PoisonP.pas:66

    // PoisonP.pas:66

}
