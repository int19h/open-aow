// Translated from GlobalEnchantmentSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.Session;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

public partial class LifeMasteryEnchantment : GlobalEnchantment
{
    // GlobalEnchantmentSpells.pas:1395
    public new const int ClassID = 135967;

    // GlobalEnchantmentSpells.pas:1387
    public LifeMasteryEnchantment(ECustomNode owner) : base(owner)
    {
        FName = AbilityRules.TranslateRStr(GameText.LifeMasteryRStr);
        FEnchantmentType = GlobalEnchantmentType.None;
        FDispelMana = 175;
    }

    // GlobalEnchantmentSpells.pas:1398
    public override int GetAIUpkeepPriority() => 1250;

    // GlobalEnchantmentSpells.pas:1428
    public override bool Deactivate(byte Player)
    {
        EngineMessage Msg = default;
        base.Deactivate(Player);
        // { Original doubles Death. }
        AoWHSMap.Current.FGlobalMagicControl.DecDoubleSphereManaCount(MagicSphere.Death);
        var player = AoWHSMap.Current.FPlayers.GetPlayers(FPlayerIndex);
        AoWHSMap.Current.FPlayers.GetPlayers(FPlayerIndex).FMagicControl.SetActiveEnchantments(player.FMagicControl.FActiveEnchantments - Set<PlayerEnchantmentType>.Of(PlayerEnchantmentType.LifeMastery));
        EngineMessage.InitMsg(ref Msg, 537002240, this);
        AoWHSMap.Current.SendMsg(ref Msg, this);
        return true;
    }

    // GlobalEnchantmentSpells.pas:1401
    public override bool CanActivate(sbyte Player, ref string Reason)
    {
        bool Result = base.CanActivate(Player, ref Reason);
        if (Result)
        {
            if (AoWHSMap.Current.FPlayers.GetPlayers(Player).FMagicControl.FActiveEnchantments.Contains(PlayerEnchantmentType.LifeMastery))
            {
                var xIsAlreadyActive = AbilityRules.TranslateRStr(GameText.XIsAlreadyActiveRStr);
                var name = GetName();
                Reason = RtlUnit.Format(xIsAlreadyActive, [name]);
                Result = false;
            }
        }

        return Result;
    }

    // GlobalEnchantmentSpells.pas:1412
    public override bool Activate(sbyte Player)
    {
        Player APlayer;
        base.Activate(Player);
        // { Original doubles Death. }
        AoWHSMap.Current.FGlobalMagicControl.IncDoubleSphereManaCount(MagicSphere.Death);
        APlayer = AoWHSMap.Current.FPlayers.GetPlayers(FPlayerIndex);
        var playerMagicControl = APlayer.FMagicControl;
        playerMagicControl.SetActiveEnchantments(playerMagicControl.FActiveEnchantments + Set<PlayerEnchantmentType>.Of(PlayerEnchantmentType.LifeMastery));

        for (int I = APlayer.FArmies.Count - 1; I >= 0; I--)
        {
            for (int J = APlayer.FArmies.GetArmy(I).Count - 1; J >= 0; J--)
            {
                APlayer.FArmies.GetArmy(I).GetUnit(J).SetLifeMasteryBlessing();
            }
        }

        return true;
    }

}
