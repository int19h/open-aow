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

public partial class LevelTerrain : GlobalTargetSpell
{
    // GlobalTargetSpells.pas:978
    public LevelTerrain(ECustomNode owner) : base(owner)
    {
        this.FSpellName = AbilityRules.TranslateRStr(LevelTerrainRStr);
        FSpellID = 18;
        FBaseManaCost = 5;
        FSpellLevel = 3;
        FMagicSphere = MagicSphere.Earth;
    }

    // GlobalTargetSpells.pas:1010
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

    // GlobalTargetSpells.pas:1022
    public override bool TargetSelected(AbstractUnit Source, int X, int Y, int L)
    {
        LevelTerrainTE Event;
        Event = new LevelTerrainTE(null);
        Event.Initialize(FSpellID, (byte)Source.FPlayer, Source.FUnitID, FBaseManaCost);
        Event.SetLocation((sbyte)X, (sbyte)Y, (sbyte)L);
        AoWHSMap.Current.FTokenManager.GetTokenControl(Source.FPlayer).AddEvent(Event);
        Event.Release();
        return true;
    }

    // GlobalTargetSpells.pas:996
    public override bool ValidTargetSelectionMapF(MapField Field)
    {
        ArmyHS Army;
        bool Result = base.ValidTargetSelectionMapF(Field) && !Set<byte>.WithBounds(0, 255).With(0).With(6).With(8).With(10).With(13).Contains((byte)Field.TerrainType) && Field.FindNoneTransparentHS(typeof(Structure)) == null;
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

    // GlobalTargetSpells.pas:988
    public override bool ValidTargetMapF(MapField Field) => base.ValidTargetMapF(Field) && !Set<byte>.WithBounds(0, 255).With(0).With(6).With(8).With(10).With(13).Contains((byte)Field.TerrainType) && Field.FindNoneTransparentHS(typeof(Structure)) == null && ((AoWMapField)Field).GetArmyHS() == null;

    private static readonly string LevelTerrainRStr = "Level Terrain";
}
