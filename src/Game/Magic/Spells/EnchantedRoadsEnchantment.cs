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
    public new const int ClassId = 135955;

    // GlobalEnchantmentSpells.pas:527
    public EnchantedRoadsEnchantment(ECustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        Name = AbilityRules.TranslateResourceString(GetEngine(), GameText.EnchantedRoadsText);
        EnchantmentType = GlobalEnchantmentType.None;
        DispelMana = 50;
    }

    // GlobalEnchantmentSpells.pas:540
    public override int GetAIUpkeepPriority() => 900;

    // GlobalEnchantmentSpells.pas:563
    public override bool Deactivate(byte player)
    {
        base.Deactivate(player);
        var playerLocal = StrategicMap.Current.Players.GetPlayers(PlayerIndex);
        StrategicMap.Current.Players.GetPlayers(PlayerIndex).MagicControl.SetActiveEnchantments(playerLocal.MagicControl.ActiveEnchantments - Set<PlayerEnchantmentType>.Of(PlayerEnchantmentType.EnchantedRoads));
        return true;
    }

    // GlobalEnchantmentSpells.pas:545
    public override bool CanActivate(sbyte player, ref string reason)
    {
        bool result = base.CanActivate(player, ref reason);
        if (result && StrategicMap.Current.Players.GetPlayers(player).MagicControl.ActiveEnchantments.Contains(PlayerEnchantmentType.EnchantedRoads))
        {
            var xIsAlreadyActive = AbilityRules.TranslateResourceString(GetEngine(), GameText.IsAlreadyActiveFormat);
            var name = GetName();
            reason = RtlUnit.Format(xIsAlreadyActive, [name]);
            result = false;
        }

        return result;
    }

    // GlobalEnchantmentSpells.pas:555
    public override bool Activate(sbyte player)
    {
        base.Activate(player);
        var playerLocal = StrategicMap.Current.Players.GetPlayers(PlayerIndex);
        StrategicMap.Current.Players.GetPlayers(PlayerIndex).MagicControl.SetActiveEnchantments(playerLocal.MagicControl.ActiveEnchantments + Set<PlayerEnchantmentType>.Of(PlayerEnchantmentType.EnchantedRoads));
        return true;
    }

}
