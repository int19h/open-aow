#nullable disable warnings
// Translated from GlobalEnchantmentSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Commands;

using Aow.Game.Draw.Animations;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

namespace Aow.Game.Magic.Spells;

public class LifeMastery : GlobalEnchantmentSpell
{
    // GlobalEnchantmentSpells.pas:1441
    public LifeMastery(CustomNode? owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        SpellType = SpellType.Global;
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), S: GameText.LifeMasteryText);
        SpellId = 61;
        BaseManaCost = 1;
        SpellLevel = 4;
        // { Original assigns Cosmos to Life Mastery. }
        MagicSphere = MagicSphere.Cosmos;
        CastDuration = 40;
        AIBasePriority = 1_000;
    }

    // GlobalEnchantmentSpells.pas:1457
    public override bool ExecuteTokenEvent(SpellTokenEvent eventValue, Troop source, int executeCount)
    {
        _ = base.ExecuteTokenEvent(eventValue, source, executeCount);
        SpellTargets.NotifySpellSphere(source.Player, sphere: MagicSphere.Life);
        MapCoordinate coordinate73 = source.GetLocation();
        int x = coordinate73.X;
        int y = coordinate73.Y;
        int l = coordinate73.MapLevel;
        _ = StrategicMap.Current.ValidateObservation(source.Player, x, y, l, radius: 0);
        TwinkleAnimation twinkle = new(null) { Holder = CreationHolder() };
        twinkle.Setup(GamePack.Current.SpellControl[61].CastImages.ImageSequences[0], mode: TwinkleModes.Rotate, count: 100, radius: 64, duration: 50, startDelay: 30);
        _ = StrategicMap.Current.PlaceHex(twinkle, x, y, l);
        _ = CastSounds.Play(0);
        CastingDone(source, mana: eventValue.EventMana);
        return false;
    }

    // GlobalEnchantmentSpells.pas:1454
    public override GlobalEnchantment CreateEnchantment() => new LifeMasteryEnchantment(null, holder: CreationHolder());
}
