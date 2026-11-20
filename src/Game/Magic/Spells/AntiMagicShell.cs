#nullable disable
// Translated from CitySpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Audio;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using Aow.Game.World.Structures;
using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

public class AntiMagicShell : CityEnchantmentSpell
{
    // CitySpells.pas:727
    public AntiMagicShell(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), GameText.AntiMagicShellText);
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
        bool condition = source.Player == StrategicMap.Current.SeatedPlayer;
        if (!condition)
        {
            sbyte hexLevel = targetCity.GetHexLevel();
            sbyte hexY = targetCity.GetHexY();
            sbyte hexX = targetCity.GetHexX();
            condition = StrategicMap.Current.WatchingTerrain(hexX, hexY, hexLevel, 1);
        }

        if (condition)
        {
            _ = GamePack.Current.SpellControl.GetSpell(SpellId).CastSounds.Play(1, -1, false, 30, SamplePanning.Center, null);
        }
    }
}
