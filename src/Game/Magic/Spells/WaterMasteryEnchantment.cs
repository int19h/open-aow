// Translated from GlobalEnchantmentSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.Session;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

public partial class WaterMasteryEnchantment : GlobalEnchantment
{
    // GlobalEnchantmentSpells.pas:760
    public new const int ClassID = 135956;

    // GlobalEnchantmentSpells.pas:752
    public WaterMasteryEnchantment(ECustomNode owner) : base(owner)
    {
        FName = AbilityRules.TranslateRStr(GameText.WaterMasteryRStr);
        FEnchantmentType = GlobalEnchantmentType.None;
        FDispelMana = 175;
    }

    // GlobalEnchantmentSpells.pas:765
    public override int GetAIUpkeepPriority() => 1250;

    // GlobalEnchantmentSpells.pas:791
    public override bool Deactivate(byte Player)
    {
        base.Deactivate(Player);
        var player = AoWHSMap.Current.FPlayers.GetPlayers(FPlayerIndex);
        AoWHSMap.Current.FPlayers.GetPlayers(FPlayerIndex).FMagicControl.SetActiveEnchantments(player.FMagicControl.FActiveEnchantments - Set<PlayerEnchantmentType>.Of(PlayerEnchantmentType.WaterMastery));
        AoWHSMap.Current.UpdateVisibility();
        // { Original uses Fire for Water Mastery. }
        AoWHSMap.Current.FGlobalMagicControl.DecDoubleSphereManaCount(MagicSphere.Fire);
        return true;
    }

    // GlobalEnchantmentSpells.pas:770
    public override bool CanActivate(sbyte Player, ref string Reason)
    {
        bool Result = base.CanActivate(Player, ref Reason);
        if (Result)
        {
            if (AoWHSMap.Current.FPlayers.GetPlayers(Player).FMagicControl.FActiveEnchantments.Contains(PlayerEnchantmentType.WaterMastery))
            {
                var xIsAlreadyActive = AbilityRules.TranslateRStr(GameText.XIsAlreadyActiveRStr);
                var name = GetName();
                Reason = RtlUnit.Format(xIsAlreadyActive, [name]);
                Result = false;
            }
        }

        return Result;
    }

    // GlobalEnchantmentSpells.pas:781
    public override bool Activate(sbyte Player)
    {
        base.Activate(Player);
        var player = AoWHSMap.Current.FPlayers.GetPlayers(FPlayerIndex);
        AoWHSMap.Current.FPlayers.GetPlayers(FPlayerIndex).FMagicControl.SetActiveEnchantments(player.FMagicControl.FActiveEnchantments + Set<PlayerEnchantmentType>.Of(PlayerEnchantmentType.WaterMastery));
        AoWHSMap.Current.UpdateVisibility();
        // { Original uses Fire for Water Mastery. }
        AoWHSMap.Current.FGlobalMagicControl.IncDoubleSphereManaCount(MagicSphere.Fire);
        return true;
    }

}
