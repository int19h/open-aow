// Translated from GlobalTargetSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;
using Aow.Game.Commands;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using Aow.Game.World.Structures;

namespace Aow.Game.Magic.Spells;

public partial class FreezeWater : GlobalTargetSpell
{
    // GlobalTargetSpells.pas:710
    public FreezeWater(ECustomNode owner) : base(owner)
    {
        this.FSpellName = AbilityRules.TranslateRStr(FreezeWaterRStr);
        FSpellID = 10;
        FBaseManaCost = 5;
        FSpellLevel = 3;
        FMagicSphere = MagicSphere.Water;
    }

    // GlobalTargetSpells.pas:811
    // { Original Boolean-return ABI; foreign P7 overrides remain procedures. }
    public override void SetupTargetSelection(Troop Source, TargetSelectionControl TargetSelection)
    {
        TargetSelectionControl Target;
        Target = TargetSelection;
        base.SetupTargetSelection(Source, Target);
        Target.FHNs.Clear();
        for (int I = 0, last = HexGeometry.RadToHN(2) - 1; I <= last; I++)
        {
            Target.FHNs.Add(0);
        }
    }

    // GlobalTargetSpells.pas:823
    public override bool TargetSelected(Troop Source, int X, int Y, int L)
    {
        FreezeWaterTokenEvent Event;
        Event = new FreezeWaterTokenEvent(null);
        Event.Initialize(FSpellID, (byte)Source.FPlayer, Source.FUnitID, FBaseManaCost);
        Event.SetLocation((sbyte)X, (sbyte)Y, (sbyte)L);
        AoWHSMap.Current.FTokenManager.GetTokenControl(Source.FPlayer).AddEvent(Event);
        Event.Release();
        return true;
    }

    public override bool ValidTargetMapF(MapField field)
    {
        if (!base.ValidTargetMapF(field)) return false;
        sbyte terrain = field.FTerrainType;
        return (terrain == 0 || terrain == 10) && field.FindNoneTransparentHS(typeof(Structure)) == null && ((AoWMapField)field).GetArmyHS() == null;
    }

    public override bool ValidTargetSelectionMapF(MapField field)
    {
        if (!base.ValidTargetSelectionMapF(field)) return false;
        sbyte terrain = field.FTerrainType;
        if ((terrain != 0 && terrain != 10) || field.FindNoneTransparentHS(typeof(Structure)) != null) return false;
        var army = ((AoWMapField)field).GetArmyHS();
        return army == null || !army.VisibleForSeatedPlayer();
    }

    private static readonly string FreezeWaterRStr = "Freeze Water";
}
