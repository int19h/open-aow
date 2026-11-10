// Translated from GlobalTargetSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;
using Aow.Game.Commands;
using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using Aow.Game.World.Structures;

using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

public partial class FireBarrier : GlobalTargetSpell
{
    // GlobalTargetSpells.pas:882
    public FireBarrier(ECustomNode owner) : base(owner)
    {
        this.FSpellName = AbilityRules.TranslateRStr(FireBarrierRStr);
        FSpellID = 11;
        FBaseManaCost = 5;
        FSpellLevel = 3;
        FMagicSphere = MagicSphere.Fire;
    }

    // GlobalTargetSpells.pas:914
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

    // GlobalTargetSpells.pas:926
    public override bool TargetSelected(AbstractUnit Source, int X, int Y, int L)
    {
        FireBarrierTE Event;
        Event = new FireBarrierTE(null);
        Event.Initialize(FSpellID, (byte)Source.FPlayer, Source.FUnitID, FBaseManaCost);
        Event.SetLocation((sbyte)X, (sbyte)Y, (sbyte)L);
        AoWHSMap.Current.FTokenManager.GetTokenControl(Source.FPlayer).AddEvent(Event);
        Event.Release();
        return true;
    }

    // GlobalTargetSpells.pas:900
    public override bool ValidTargetSelectionMapF(MapField Field)
    {
        ArmyHS Army;
        bool Result = base.ValidTargetSelectionMapF(Field) && !Set<byte>.WithBounds(0, 255).With(0).With(6).With(10).With(13).Contains((byte)Field.TerrainType) && Field.FindNoneTransparentHS(typeof(Structure)) == null;
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

    // GlobalTargetSpells.pas:892
    public override bool ValidTargetMapF(MapField Field) => base.ValidTargetMapF(Field) && !Set<byte>.WithBounds(0, 255).With(0).With(6).With(10).With(13).Contains((byte)Field.TerrainType) && Field.FindNoneTransparentHS(typeof(Structure)) == null && ((AoWMapField)Field).GetArmyHS() == null;

    private static readonly string FireBarrierRStr = "Fire Barrier";
}
