// Translated from GlobalEnchantmentSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.Session;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

public partial class DeathMasteryEnchantment : GlobalEnchantment
{
    // GlobalEnchantmentSpells.pas:1483
    public new const int ClassId = 135968;

    // GlobalEnchantmentSpells.pas:1475
    public DeathMasteryEnchantment(ECustomNode owner) : base(owner)
    {
        Name = AbilityRules.TranslateResourceString(GameText.s_deathMasteryText);
        EnchantmentType = GlobalEnchantmentType.None;
        DispelMana = 175;
    }

    // GlobalEnchantmentSpells.pas:1568
    public override void MessageProc(ref EngineMessage message, EngineObject sender)
    {
        base.MessageProc(ref message, sender);
        if (message.MessageId == 537001986)
        {
            if (message.Payload.Int != 0)
            {
                if (AoWHSMap.Current.Players.GetPlayers(message.Payload.Int).MagicControl.EnemyEnchantments.Contains(PlayerEnchantmentType.DeathMastery))
                {
                    NewTurn((sbyte)message.Payload.Int);
                }
            }
        }
    }

    // GlobalEnchantmentSpells.pas:1486
    public override int GetAIUpkeepPriority() => 1250;

    // GlobalEnchantmentSpells.pas:1509
    public override bool Deactivate(byte player)
    {
        EngineMessage message = default;
        base.Deactivate(player);
        // { Original doubles Life. }
        AoWHSMap.Current.GlobalMagicControl.DecrementDoubleSphereManaCount(MagicSphere.Life);
        var playerLocal = AoWHSMap.Current.Players.GetPlayers(PlayerIndex);
        AoWHSMap.Current.Players.GetPlayers(PlayerIndex).MagicControl.SetActiveEnchantments(playerLocal.MagicControl.ActiveEnchantments - Set<PlayerEnchantmentType>.Of(PlayerEnchantmentType.DeathMastery));
        EngineMessage.InitializeMessage(ref message, 537002241, this);
        AoWHSMap.Current.SendMessage(ref message, this);
        return true;
    }

    // GlobalEnchantmentSpells.pas:1489
    public override bool CanActivate(sbyte player, ref string reason)
    {
        bool result = base.CanActivate(player, ref reason);
        if (result)
        {
            if (AoWHSMap.Current.Players.GetPlayers(player).MagicControl.ActiveEnchantments.Contains(PlayerEnchantmentType.DeathMastery))
            {
                var xIsAlreadyActive = AbilityRules.TranslateResourceString(GameText.s_isAlreadyActiveFormat);
                var name = GetName();
                reason = RtlUnit.Format(xIsAlreadyActive, [name]);
                result = false;
            }
        }

        return result;
    }

    // GlobalEnchantmentSpells.pas:1500
    public override bool Activate(sbyte player)
    {
        base.Activate(player);
        // { Original doubles Life. }
        AoWHSMap.Current.GlobalMagicControl.IncrementDoubleSphereManaCount(MagicSphere.Life);
        var playerLocal = AoWHSMap.Current.Players.GetPlayers(PlayerIndex);
        AoWHSMap.Current.Players.GetPlayers(PlayerIndex).MagicControl.SetActiveEnchantments(playerLocal.MagicControl.ActiveEnchantments + Set<PlayerEnchantmentType>.Of(PlayerEnchantmentType.DeathMastery));
        return true;
    }

    // GlobalEnchantmentSpells.pas:1522
    public void NewTurn(sbyte player)
    {
        int index;
        byte[] flags;
        int mask;
        MapLevel mapLevel;
        HexNumberWalkData walk = default;
        bool border;
        flags = [];
        mask = 1 << (player - 1);
        for (int L = 0, last = AoWHSMap.Current.MapContainer.LevelCount - 1; L <= last; L++)
        {
            mapLevel = AoWHSMap.Current.MapContainer.GetMapLevel(L);
            index = 0;
            Array.Resize(ref flags, mapLevel.Width * mapLevel.Height);
            Array.Clear(flags);
            for (int Y = 1, lastY = mapLevel.Height - 1; Y <= lastY; Y++)
            {
                for (int X = 1, lastX = mapLevel.Width - 2; X <= lastX; X++)
                {
                    if ((((AoWMapField)mapLevel.GetField(X, Y)).FieldFlags & mask) != 0)
                    {
                        border = false;
                        HexNumberWalkData.InitializeHexNumberWalk(ref walk, X, Y, 1);
                        while (walk.Radius == 1 && !border)
                        {
                            if (walk.X >= 0 && walk.X < mapLevel.Width && walk.Y >= 0 && walk.Y < mapLevel.Height)
                            {
                                border = (((AoWMapField)mapLevel.GetField(walk.X, walk.Y)).FieldFlags & mask) == 0;
                            }

                            HexNumberWalkData.WalkToNextHexNumber(ref walk);
                        }

                        if (border)
                        {
                            flags[index] = 1;
                        }
                    }

                    index += 1;
                }
            }

            index = 0;
            for (int Y = 1, lastY = mapLevel.Height - 1; Y <= lastY; Y++)
            {
                for (int X = 1, lastX = mapLevel.Width - 2; X <= lastX; X++)
                {
                    if (flags[index] != 0)
                    {
                        ((AoWMapField)mapLevel.GetField(X, Y)).SetExplored(player, false);
                    }

                    index += 1;
                }
            }
        }

        AoWHSMap.Current.UpdateVisibility();
        Array.Resize(ref flags, 0);
    }

}
