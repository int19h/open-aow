// Translated from CitySpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Audio;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using Aow.Game.World.Structures;
using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

public partial class AntiMagicShell : CityEnchantmentSpell
{
    // CitySpells.pas:727
    public AntiMagicShell(ECustomNode owner) : base(owner)
    {
        SpellName = AbilityRules.TranslateResourceString(GameText.s_antiMagicShellText);
        SpellId = 54;
        CityTargetMode = Set<CityTargetModeElement>.Of(CityTargetModeElement.Own);
        BaseManaCost = 1;
        SpellLevel = 4;
        MagicSphere = MagicSphere.Cosmos;
        EnchantmentId = CityEnchantmentId.AntiMagicShell;
        CastDuration = 40;
        AIPriority = 500;
        AICastSituation = Set<CityEnchantmentCastSpellSituationElement>.Of(CityEnchantmentCastSpellSituationElement.LeaderInCity);
        EnchantmentSpellId = 100;
    }

    // CitySpells.pas:743
    public override void PlayDefaultCityEffects(Troop source, City targetCity)
    {
        base.PlayDefaultCityEffects(source, targetCity);
        bool condition = source.Player == AoWHSMap.Current.SeatedPlayer;
        if (!condition)
        {
            var hexLevel = targetCity.GetHexLevel();
            var hexY = targetCity.GetHexY();
            var hexX = targetCity.GetHexX();
            condition = AoWHSMap.Current.WatchingTerrain(hexX, hexY, hexLevel, 1);
        }

        if (condition)
        {
            AoWHSSet.Current.SpellControl.GetSpell(SpellId).CastSounds.Play(1, -1, false, 30, SamplePanning.Center, null);
        }
    }
}
