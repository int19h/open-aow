// Translated from GlobalEnchantmentSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.Session;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

public partial class EnchantedRoadsEnchantment : GlobalEnchantment
{
    // GlobalEnchantmentSpells.pas:535
    public new const int ClassID = 135955;

    // GlobalEnchantmentSpells.pas:527
    public EnchantedRoadsEnchantment(ECustomNode owner) : base(owner)
    {
        FName = AbilityRules.TranslateRStr(GameText.EnchantedRoadsRStr);
        FEnchantmentType = GlobalEnchantmentType.None;
        FDispelMana = 50;
    }

    // GlobalEnchantmentSpells.pas:540
    public override int GetAIUpkeepPriority() => 900;

    // GlobalEnchantmentSpells.pas:563
    public override bool Deactivate(byte Player)
    {
        base.Deactivate(Player);
        var player = AoWHSMap.Current.FPlayers.GetPlayers(FPlayerIndex);
        AoWHSMap.Current.FPlayers.GetPlayers(FPlayerIndex).FMagicControl.SetActiveEnchantments(player.FMagicControl.FActiveEnchantments - Set<PlayerEnchantmentType>.Of(PlayerEnchantmentType.EnchantedRoads));
        return true;
    }

    // GlobalEnchantmentSpells.pas:545
    public override bool CanActivate(sbyte Player, ref string Reason)
    {
        bool Result = base.CanActivate(Player, ref Reason);
        if (Result && AoWHSMap.Current.FPlayers.GetPlayers(Player).FMagicControl.FActiveEnchantments.Contains(PlayerEnchantmentType.EnchantedRoads))
        {
            var xIsAlreadyActive = AbilityRules.TranslateRStr(GameText.XIsAlreadyActiveRStr);
            var name = GetName();
            Reason = RtlUnit.Format(xIsAlreadyActive, [name]);
            Result = false;
        }

        return Result;
    }

    // GlobalEnchantmentSpells.pas:555
    public override bool Activate(sbyte Player)
    {
        base.Activate(Player);
        var player = AoWHSMap.Current.FPlayers.GetPlayers(FPlayerIndex);
        AoWHSMap.Current.FPlayers.GetPlayers(FPlayerIndex).FMagicControl.SetActiveEnchantments(player.FMagicControl.FActiveEnchantments + Set<PlayerEnchantmentType>.Of(PlayerEnchantmentType.EnchantedRoads));
        return true;
    }

}
