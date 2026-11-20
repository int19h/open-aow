#nullable disable
// Translated from GlobalEnchantmentSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Commands;

using Aow.Game.Draw.Animations;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

public class PowerLeak : GlobalEnchantmentSpell
{
    // GlobalEnchantmentSpells.pas:1036
    public PowerLeak(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        SpellType = SpellType.Global;
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), GameText.PowerLeakText);
        SpellId = 48;
        BaseManaCost = 1;
        SpellLevel = 4;
        MagicSphere = MagicSphere.Cosmos;
        CastDuration = 20;
        AIBasePriority = 750;
    }

    // GlobalEnchantmentSpells.pas:1049
    public override bool ExecuteTokenEvent(SpellTokenEvent eventValue, Troop source, int executeCount)
    {
        _ = base.ExecuteTokenEvent(eventValue, source, executeCount);
        MapCoordinate coordinate74 = source.GetLocation();
        sbyte x = coordinate74.X;
        sbyte y = coordinate74.Y;
        sbyte l = coordinate74.MapLevel;
        _ = StrategicMap.Current.ValidateObservation(source.Player, x, y, l, 0);
        TwinkleAnimation animation = new(null) { Holder = CreationHolder() };
        animation.Setup(GamePack.Current.SpellControl[48].CastImages.ImageSequences[0], Set<TwinkleModeElement>.Of(TwinkleModeElement.Implode), 100, 64, 50, 30);
        _ = StrategicMap.Current.PlaceHex(animation, x, y, l);
        _ = CastSounds.Play(0);
        CastingDone(source, eventValue.EventMana);
        return false;
    }

    // GlobalEnchantmentSpells.pas:1031
    public override GlobalEnchantment CreateEnchantment() => new PowerLeakEnchantment(null, holder: CreationHolder());
}
