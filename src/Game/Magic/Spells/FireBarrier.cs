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
    public FireBarrier(ECustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        this.SpellName = AbilityRules.TranslateResourceString(GetEngine(), s_fireBarrierText);
        SpellId = 11;
        BaseManaCost = 5;
        SpellLevel = 3;
        MagicSphere = MagicSphere.Fire;
    }

    // GlobalTargetSpells.pas:914
    public override void SetupTargetSelection(Troop source, TargetSelectionControl targetSelection)
    {
        TargetSelectionControl target;
        target = targetSelection;
        base.SetupTargetSelection(source, target);
        target.HexNumbers.Clear();
        for (int I = 0, last = HexGeometry.RadiusToHexNumber(2) - 1; I <= last; I++)
        {
            target.HexNumbers.Add(0);
        }
    }

    // GlobalTargetSpells.pas:926
    public override bool TargetSelected(Troop source, int X, int Y, int L)
    {
        FireBarrierTokenEvent eventValue;
        eventValue = new FireBarrierTokenEvent(null) { Holder = CreationHolder() };
        eventValue.Initialize(SpellId, (byte)source.Player, source.UnitId, BaseManaCost);
        eventValue.SetLocation((sbyte)X, (sbyte)Y, (sbyte)L);
        StrategicMap.Current.TokenManager.GetTokenControl(source.Player).AddEvent(eventValue);
        eventValue.Release();
        return true;
    }

    // GlobalTargetSpells.pas:900
    public override bool ValidTargetSelectionMapField(MapField field)
    {
        ArmyHexSprite army;
        bool result = base.ValidTargetSelectionMapField(field) && !Set<byte>.WithBounds(0, 255).With(0).With(6).With(10).With(13).Contains((byte)field.TerrainType) && field.FindNoneTransparentHexSprite(typeof(Structure)) == null;
        if (result)
        {
            army = field.GetArmyHexSprite();
            if (army != null)
            {
                result = !army.VisibleForSeatedPlayer();
            }
        }

        return result;
    }

    // GlobalTargetSpells.pas:892
    public override bool ValidTargetMapField(MapField field) => base.ValidTargetMapField(field) && !Set<byte>.WithBounds(0, 255).With(0).With(6).With(10).With(13).Contains((byte)field.TerrainType) && field.FindNoneTransparentHexSprite(typeof(Structure)) == null && field.GetArmyHexSprite() == null;

    private static readonly string s_fireBarrierText = "Fire Barrier";
}
