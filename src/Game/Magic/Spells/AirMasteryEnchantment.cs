// Translated from GlobalEnchantmentSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.Session;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

public partial class AirMasteryEnchantment : GlobalEnchantment
{
    // GlobalEnchantmentSpells.pas:649
    public new const int ClassID = 135966;

    // GlobalEnchantmentSpells.pas:641
    public AirMasteryEnchantment(ECustomNode owner) : base(owner)
    {
        FName = AbilityRules.TranslateRStr(GameText.AirMasteryRStr);
        FEnchantmentType = GlobalEnchantmentType.None;
        FDispelMana = 175;
    }

    // GlobalEnchantmentSpells.pas:654
    public override int GetAIUpkeepPriority() => 1250;

    // GlobalEnchantmentSpells.pas:679
    public override bool Deactivate(byte Player)
    {
        base.Deactivate(Player);
        var player = AoWHSMap.Current.FPlayers.GetPlayers(FPlayerIndex);
        AoWHSMap.Current.FPlayers.GetPlayers(FPlayerIndex).FMagicControl.SetActiveEnchantments(player.FMagicControl.FActiveEnchantments - Set<PlayerEnchantmentType>.Of(PlayerEnchantmentType.AirMastery));
        // { Original uses Earth. }
        AoWHSMap.Current.FGlobalMagicControl.DecDoubleSphereManaCount(MagicSphere.Earth);
        return true;
    }

    // GlobalEnchantmentSpells.pas:659
    public override bool CanActivate(sbyte Player, ref string Reason)
    {
        bool Result = base.CanActivate(Player, ref Reason);
        if (Result)
        {
            if (AoWHSMap.Current.FPlayers.GetPlayers(Player).FMagicControl.FActiveEnchantments.Contains(PlayerEnchantmentType.AirMastery))
            {
                var xIsAlreadyActive = AbilityRules.TranslateRStr(GameText.XIsAlreadyActiveRStr);
                var name = GetName();
                Reason = RtlUnit.Format(xIsAlreadyActive, [name]);
                Result = false;
            }
        }

        return Result;
    }

    // GlobalEnchantmentSpells.pas:670
    public override bool Activate(sbyte Player)
    {
        base.Activate(Player);
        var player = AoWHSMap.Current.FPlayers.GetPlayers(FPlayerIndex);
        AoWHSMap.Current.FPlayers.GetPlayers(FPlayerIndex).FMagicControl.SetActiveEnchantments(player.FMagicControl.FActiveEnchantments + Set<PlayerEnchantmentType>.Of(PlayerEnchantmentType.AirMastery));
        // { Original uses Earth. }
        AoWHSMap.Current.FGlobalMagicControl.IncDoubleSphereManaCount(MagicSphere.Earth);
        return true;
    }

}
