// Translated from GlobalEnchantmentSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Commands;

using Aow.Game.Draw.Animations;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

public partial class Flood : GlobalEnchantmentSpell
{
    // GlobalEnchantmentSpells.pas:1356
    public Flood(ECustomNode owner) : base(owner)
    {
        SpellName = AbilityRules.TranslateResourceString(GameText.FloodText);
        SpellId = 50;
        SpellType = SpellType.Global;
        BaseManaCost = 1;
        SpellLevel = 4;
        MagicSphere = MagicSphere.Water;
    }

    // GlobalEnchantmentSpells.pas:1370
    public override bool ExecuteTokenEvent(SpellTokenEvent eventValue, Troop source, int executeCount)
    {
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        TwinkleAnimation twinkle;
        base.ExecuteTokenEvent(eventValue, source, executeCount);
        source.GetLocation(ref X, ref Y, ref L);
        StrategicMap.Current.ValidateObservation(source.Player, X, Y, L, 0);
        twinkle = new TwinkleAnimation(null);
        twinkle.Setup(GamePack.Current.SpellControl.GetSpell(50).CastImages.ImageSequences[0], Set<TwinkleModeElement>.Of(TwinkleModeElement.Rotate), 50, 64, 50, 30);
        StrategicMap.Current.PlaceHex(twinkle, X, Y, L);
        CastSounds.Play(0);
        CastingDone(source, eventValue.EventMana);
        return false;
    }

    // GlobalEnchantmentSpells.pas:1353
    public override GlobalEnchantment CreateEnchantment() => new FloodEnchantment(null);

    // GlobalEnchantmentSpells.pas:1367
    public void ShowAnimation(int animation, int pixelXSurface, int pixelYSurface, int surfaceDescription)
    {
    }
}
