// Translated from GlobalTargetSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;
using Aow.Game.Commands;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

namespace Aow.Game.Magic.Spells;

public partial class HealingShowers : GlobalTargetSpell
{
    // GlobalTargetSpells.pas:2277
    public HealingShowers(ECustomNode owner) : base(owner)
    {
        this.FSpellName = AbilityRules.TranslateRStr(HealingShowersRStr);
        FSpellID = 73;
        FBaseManaCost = 5;
        FSpellLevel = 3;
        FMagicSphere = MagicSphere.Water;
    }

    // GlobalTargetSpells.pas:2295
    public override void SetupTargetSelection(AbstractUnit Source, TargetSelectionControl TargetSelection)
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

    // GlobalTargetSpells.pas:2308
    public override bool TargetSelected(AbstractUnit Source, int X, int Y, int L)
    {
        HealingShowersTE Event;
        Event = new HealingShowersTE(null);
        Event.Initialize(FSpellID, (byte)Source.FPlayer, Source.FUnitID, FBaseManaCost);
        Event.SetLocation((sbyte)X, (sbyte)Y, (sbyte)L);
        AoWHSMap.Current.FTokenManager.GetTokenControl(Source.FPlayer).AddEvent(Event);
        Event.Release();
        return true;
    }

    // GlobalTargetSpells.pas:2291
    public override bool ValidTargetSelectionMapF(MapField Field) => base.ValidTargetSelectionMapF(Field);

    // GlobalTargetSpells.pas:2287
    public override bool ValidTargetMapF(MapField Field) => base.ValidTargetMapF(Field);

    private static readonly string HealingShowersRStr = "Healing Showers";
}
