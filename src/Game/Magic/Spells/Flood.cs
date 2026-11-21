#nullable disable
// Translated from GlobalEnchantmentSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Commands;

using Aow.Game.Draw.Animations;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

namespace Aow.Game.Magic.Spells;

public class Flood : GlobalEnchantmentSpell
{
    // GlobalEnchantmentSpells.pas:1356
    public Flood(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), S: GameText.FloodText);
        SpellId = 50;
        SpellType = SpellType.Global;
        BaseManaCost = 1;
        SpellLevel = 4;
        MagicSphere = MagicSphere.Water;
    }

    // GlobalEnchantmentSpells.pas:1370
    public override bool ExecuteTokenEvent(SpellTokenEvent eventValue, Troop source, int executeCount)
    {
        _ = base.ExecuteTokenEvent(eventValue, source, executeCount);
        MapCoordinate coordinate67 = source.GetLocation();
        sbyte x = coordinate67.X;
        sbyte y = coordinate67.Y;
        sbyte l = coordinate67.MapLevel;
        _ = StrategicMap.Current.ValidateObservation(source.Player, x, y, l, radius: 0);
        TwinkleAnimation twinkle = new(null) { Holder = CreationHolder() };
        twinkle.Setup(GamePack.Current.SpellControl[50].CastImages.ImageSequences[0], mode: TwinkleModes.Rotate, count: 50, radius: 64, duration: 50, startDelay: 30);
        _ = StrategicMap.Current.PlaceHex(twinkle, x, y, l);
        _ = CastSounds.Play(0);
        CastingDone(source, mana: eventValue.EventMana);
        return false;
    }

    // GlobalEnchantmentSpells.pas:1353
    public override GlobalEnchantment CreateEnchantment() => new FloodEnchantment(null, holder: CreationHolder());

    // GlobalEnchantmentSpells.pas:1367
    public void ShowAnimation(int animation, int pixelXSurface, int pixelYSurface, int surfaceDescription)
    {
    }
}
