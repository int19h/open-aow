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
        FSpellName = AbilityRules.TranslateRStr(GameText.AntiMagicShellRStr);
        FSpellID = 54;
        FCityTargetMode = Set<CityTargetModeElement>.Of(CityTargetModeElement.Own);
        FBaseManaCost = 1;
        FSpellLevel = 4;
        FMagicSphere = MagicSphere.Cosmos;
        FEnchantmentID = CityEnchantmentID.AntiMagicShell;
        FCastDuration = 40;
        FAIPriority = 500;
        FAICastSituation = Set<CityEnchantmentCastSpellSituationElement>.Of(CityEnchantmentCastSpellSituationElement.LeaderInCity);
        FEnchantmentSpellID = 100;
    }

    // CitySpells.pas:743
    public override void PlayDefaultCityEffects(AbstractUnit Source, City TargetCity)
    {
        base.PlayDefaultCityEffects(Source, TargetCity);
        bool condition = Source.FPlayer == AoWHSMap.Current.FSeatedPlayer;
        if (!condition)
        {
            var lhx = TargetCity.GetLhx();
            var yhx = TargetCity.GetYhx();
            var xhx = TargetCity.GetXhx();
            condition = AoWHSMap.Current.WatchingTerrain(xhx, yhx, lhx, 1);
        }

        if (condition)
        {
            AoWHSSet.Current.FSpellControl.GetSpell(FSpellID).FCastSounds.PlayEx(1, -1, false, 30, SamplePanning.Center, null);
        }
    }
}
