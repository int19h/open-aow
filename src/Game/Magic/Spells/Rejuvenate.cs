// Translated from GlobalTargetSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;
using Aow.Game.Commands;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

public partial class Rejuvenate : GlobalTargetSpell
{
    // GlobalTargetSpells.pas:1074
    public Rejuvenate(ECustomNode owner) : base(owner)
    {
        this.FSpellName = AbilityRules.TranslateRStr(RejuvenateRStr);
        FSpellID = 41;
        FBaseManaCost = 5;
        FSpellLevel = 1;
        FMagicSphere = MagicSphere.Life;
        FAboveGroundOnly = true;
    }

    // GlobalTargetSpells.pas:1095
    public override void SetupTargetSelection(AbstractUnit Source, TargetSelectionControl TargetSelection)
    {
        TargetSelectionControl Target;
        Target = TargetSelection;
        base.SetupTargetSelection(Source, Target);
        Target.FHNs.Clear();
        for (int I = 0, last = HexGeometry.RadToHN(3) - 1; I <= last; I++)
        {
            Target.FHNs.Add(0);
        }
    }

    // GlobalTargetSpells.pas:1107
    public override bool TargetSelected(AbstractUnit Source, int X, int Y, int L)
    {
        RejuvenateTE Event;
        Event = new RejuvenateTE(null);
        Event.Initialize(FSpellID, (byte)Source.FPlayer, Source.FUnitID, FBaseManaCost);
        Event.SetLocation((sbyte)X, (sbyte)Y, (sbyte)L);
        AoWHSMap.Current.FTokenManager.GetTokenControl(Source.FPlayer).AddEvent(Event);
        Event.Release();
        return true;
    }

    // GlobalTargetSpells.pas:1090
    public override bool ValidTargetSelectionMapF(MapField Field) => base.ValidTargetSelectionMapF(Field) && Set<byte>.WithBounds(0, 255).WithRange(2, 5).Contains((byte)Field.TerrainType);

    // GlobalTargetSpells.pas:1085
    public override bool ValidTargetMapF(MapField Field) => base.ValidTargetMapF(Field) && Set<byte>.WithBounds(0, 255).WithRange(2, 5).Contains((byte)Field.TerrainType);

    private static readonly string RejuvenateRStr = "Rejuvenate";
}
