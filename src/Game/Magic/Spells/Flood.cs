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
        FSpellName = AbilityRules.TranslateRStr(GameText.FloodRStr);
        FSpellID = 50;
        FSpellType = SpellType.Global;
        FBaseManaCost = 1;
        FSpellLevel = 4;
        FMagicSphere = MagicSphere.Water;
    }

    // GlobalEnchantmentSpells.pas:1370
    public override bool ExecuteTE(SpellTE Event, AbstractUnit Source, int ExecuteCount)
    {
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        TwinkleAnimation Twinkle;
        base.ExecuteTE(Event, Source, ExecuteCount);
        Source.GetLocation(ref X, ref Y, ref L);
        AoWHSMap.Current.ValidateObservation(Source.FPlayer, X, Y, L, 0);
        Twinkle = new TwinkleAnimation(null);
        Twinkle.Setup(AoWHSSet.Current.FSpellControl.GetSpell(50).FCastImages.ImageSequences[0], Set<TwinkleModeElement>.Of(TwinkleModeElement.Rotate), 50, 64, 50, 30);
        AoWHSMap.Current.PlaceHX(Twinkle, X, Y, L);
        FCastSounds.Play(0);
        CastingDone(Source, Event.FEventMana);
        return false;
    }

    // GlobalEnchantmentSpells.pas:1353
    public override GlobalEnchantment CreateEnchantment() => new FloodEnchantment(null);

    // GlobalEnchantmentSpells.pas:1367
    public void ShowAnimation(int Anim, int XhpSurf, int YhpSurf, int SurfaceDesc)
    {
    }
}
