#nullable disable
// Translated from GlobalEnchantmentSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Commands;

using Aow.Game.Draw.Animations;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

public partial class LifeMastery : GlobalEnchantmentSpell
{
    // GlobalEnchantmentSpells.pas:1441
    public LifeMastery(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        SpellType = SpellType.Global;
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), GameText.LifeMasteryText);
        SpellId = 61;
        BaseManaCost = 1;
        SpellLevel = 4;
        // { Original assigns Cosmos to Life Mastery. }
        MagicSphere = MagicSphere.Cosmos;
        CastDuration = 40;
        AIBasePriority = 1000;
    }

    // GlobalEnchantmentSpells.pas:1457
    public override bool ExecuteTokenEvent(SpellTokenEvent eventValue, Troop source, int executeCount)
    {
        _ = base.ExecuteTokenEvent(eventValue, source, executeCount);
        SpellTargets.NotifySpellSphere(source.Player, MagicSphere.Life);
        MapCoordinate coordinate73 = source.GetLocation();
        sbyte x = coordinate73.X;
        sbyte y = coordinate73.Y;
        sbyte l = coordinate73.MapLevel;
        _ = StrategicMap.Current.ValidateObservation(source.Player, x, y, l, 0);
        TwinkleAnimation twinkle = new(null) { Holder = CreationHolder() };
        twinkle.Setup(GamePack.Current.SpellControl.GetSpell(61).CastImages.ImageSequences[0], Set<TwinkleModeElement>.Of(TwinkleModeElement.Rotate), 100, 64, 50, 30);
        _ = StrategicMap.Current.PlaceHex(twinkle, x, y, l);
        _ = CastSounds.Play(0);
        CastingDone(source, eventValue.EventMana);
        return false;
    }

    // GlobalEnchantmentSpells.pas:1454
    public override GlobalEnchantment CreateEnchantment() => new LifeMasteryEnchantment(null, holder: CreationHolder());
}
