// Translated from GlobalEnchantmentSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.Session;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

public partial class EarthMasteryEnchantment : GlobalEnchantment
{
    // GlobalEnchantmentSpells.pas:1828
    public new const int ClassID = 135970;

    // GlobalEnchantmentSpells.pas:1820
    public EarthMasteryEnchantment(ECustomNode owner) : base(owner)
    {
        FName = AbilityRules.TranslateRStr(GameText.EarthMasteryRStr);
        FEnchantmentType = GlobalEnchantmentType.None;
        FDispelMana = 175;
    }

    // GlobalEnchantmentSpells.pas:1831
    public override int GetAIUpkeepPriority() => 1250;

    // GlobalEnchantmentSpells.pas:1861
    public override bool Deactivate(byte Player)
    {
        Player APlayer;
        base.Deactivate(Player);
        APlayer = AoWHSMap.Current.FPlayers.GetPlayers(FPlayerIndex);
        var playerMagicControl = APlayer.FMagicControl;
        playerMagicControl.SetActiveEnchantments(playerMagicControl.FActiveEnchantments - Set<PlayerEnchantmentType>.Of(PlayerEnchantmentType.EarthMastery));

        for (int I = APlayer.FArmies.Count - 1; I >= 0; I--)
        {
            for (int J = APlayer.FArmies.GetArmy(I).Count - 1; J >= 0; J--)
            {
                APlayer.FArmies.GetArmy(I).GetUnit(J).ResetEarthMasteryStoneSkin();
            }
        }

        // { Original doubles Air. }
        AoWHSMap.Current.FGlobalMagicControl.DecDoubleSphereManaCount(MagicSphere.Air);
        return true;
    }

    // GlobalEnchantmentSpells.pas:1834
    public override bool CanActivate(sbyte Player, ref string Reason)
    {
        bool Result = base.CanActivate(Player, ref Reason);
        if (Result)
        {
            if (AoWHSMap.Current.FPlayers.GetPlayers(Player).FMagicControl.FActiveEnchantments.Contains(PlayerEnchantmentType.EarthMastery))
            {
                Reason = RtlUnit.Format(AbilityRules.TranslateRStr(GameText.XIsAlreadyActiveRStr), [FName]);
                Result = false;
            }
        }

        return Result;
    }

    // GlobalEnchantmentSpells.pas:1845
    public override bool Activate(sbyte Player)
    {
        Player APlayer;
        base.Activate(Player);
        // { Original doubles Air. }
        AoWHSMap.Current.FGlobalMagicControl.IncDoubleSphereManaCount(MagicSphere.Air);
        APlayer = AoWHSMap.Current.FPlayers.GetPlayers(FPlayerIndex);
        var playerMagicControl = APlayer.FMagicControl;
        playerMagicControl.SetActiveEnchantments(playerMagicControl.FActiveEnchantments + Set<PlayerEnchantmentType>.Of(PlayerEnchantmentType.EarthMastery));

        for (int I = APlayer.FArmies.Count - 1; I >= 0; I--)
        {
            for (int J = APlayer.FArmies.GetArmy(I).Count - 1; J >= 0; J--)
            {
                APlayer.FArmies.GetArmy(I).GetUnit(J).SetEarthMasteryStoneSkin();
            }
        }

        return true;
    }

}
