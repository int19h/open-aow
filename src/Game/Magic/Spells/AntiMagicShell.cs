#nullable disable warnings
// Translated from CitySpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Audio;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using Aow.Game.World.Structures;

namespace Aow.Game.Magic.Spells;

public class AntiMagicShell : CityEnchantmentSpell
{
    // CitySpells.pas:727
    public AntiMagicShell(CustomNode? owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), S: GameText.AntiMagicShellText);
        SpellId = 54;
        CityTargetMode = CityTargetModes.Own;
        BaseManaCost = 1;
        SpellLevel = 4;
        MagicSphere = MagicSphere.Cosmos;
        EnchantmentId = CityEnchantmentId.AntiMagicShell;
        CastDuration = 40;
        AIPriority = 500;
        AICastSituation = CityEnchantmentCastSpellSituations.LeaderInCity;
        EnchantmentSpellId = 100;
    }

    // CitySpells.pas:743
    public override void PlayDefaultCityEffects(Troop source, City targetCity)
    {
        base.PlayDefaultCityEffects(source, targetCity);
        bool condition = source.Player == StrategicMap.Current.SeatedPlayer;
        if (!condition)
        {
            int hexLevel = targetCity.HexLevel;
            int hexY = targetCity.HexY;
            int hexX = targetCity.HexX;
            condition = StrategicMap.Current.WatchingTerrain(hexX, hexY, L: hexLevel, radius: 1);
        }

        if (condition)
        {
            _ = GamePack.Current.SpellControl[SpellId].CastSounds.Play(1, volume: -1, looped: false, delay: 30, panning: SamplePanning.Center, callback: null);
        }
    }
}
