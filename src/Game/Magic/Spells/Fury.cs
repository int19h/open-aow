#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
// Translated from UnitSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.Session;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

public class Fury : UnitSpell
{
    // UnitSpells.pas:336
    public Fury(CustomNode owner, EngineObject holder) : base(owner, holder: holder)
    {
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), S: GameText.FuryText);
        SpellId = 33;
        AIValue = 175;
        BaseManaCost = 5;
        SpellLevel = 1;
        MagicSphere = MagicSphere.Fire;
        TargetRange = 12;
        SpellControlTypes = SpellControlTypes.Global | SpellControlTypes.Combat;
        UnitDispelResistance = 3;
    }

    // UnitSpells.pas:369
    public override void PlayDefaultCastEffects(PlayerId? player, int X, int Y, int layer)
    {
        base.PlayDefaultCastEffects(player, X, Y, layer);
        StrategicMap strategicMap = GetEngine().StrategicMap;
        if (player == strategicMap.SeatedPlayer || strategicMap.Watching(X, Y, L: layer, radius: 0))
        {
            _ = GetEngine().GamePack.SpellControl[SpellId].CastSounds.Play(1);
        }
    }

    // UnitSpells.pas:376
    public override bool ValidTargetUnit(Troop source, Troop target)
    {
        bool result = base.ValidTargetUnit(source, target);
        result = result && !target.GetAbilityEnabled(156);
        if (result)
        {
            if (GetEngine().StrategicMap.Players.GetPlayers(target.Player).PlayerType != PlayerType.Human && target is Leader)
            {
                return false;
            }
        }

        return result;
    }

    // UnitSpells.pas:350
    public override bool ExecuteSpell(Troop source, Troop target, int mana, Span<int> data)
    {
        if (ValidTargetUnit(source, target))
        {
            if (target.ExpandAbility(156))
            {
                UnitEnchantment enchantment = target.GetAbilityData<UnitEnchantmentAbilityData>(156).Enchantment;
                enchantment.SetUpkeep(ManaUpkeep);
                enchantment.SetSource(source.Player);
                enchantment.DispelResistance = UnitDispelResistance.TruncateToSByte();
            }

            // { Original succeeds even when expansion fails. }
            return true;
        }
        else
        {
            return false;
        }
    }
}
