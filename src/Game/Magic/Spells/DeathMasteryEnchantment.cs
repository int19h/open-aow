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
    public DeathMasteryEnchantment(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        Name = AbilityRules.TranslateResourceString(GetEngine(), GameText.DeathMasteryText);
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
                if (StrategicMap.Current.Players.GetPlayers(message.Payload.Int).MagicControl.EnemyEnchantments.Contains(PlayerEnchantmentType.DeathMastery))
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
        StrategicMap.Current.GlobalMagicControl.DecrementDoubleSphereManaCount(MagicSphere.Life);
        Player playerLocal = StrategicMap.Current.Players.GetPlayers(PlayerIndex);
        StrategicMap.Current.Players.GetPlayers(PlayerIndex).MagicControl.SetActiveEnchantments(playerLocal.MagicControl.ActiveEnchantments - Set<PlayerEnchantmentType>.Of(PlayerEnchantmentType.DeathMastery));
        EngineMessage.InitializeMessage(out message, 537002241, this);
        StrategicMap.Current.SendMessage(ref message, this);
        return true;
    }

    // GlobalEnchantmentSpells.pas:1489
    public override bool CanActivate(sbyte player, ref string reason)
    {
        bool result = base.CanActivate(player, ref reason);
        if (result)
        {
            if (StrategicMap.Current.Players.GetPlayers(player).MagicControl.ActiveEnchantments.Contains(PlayerEnchantmentType.DeathMastery))
            {
                string xIsAlreadyActive = AbilityRules.TranslateResourceString(GetEngine(), GameText.IsAlreadyActiveFormat);
                string name = GetName();
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
        StrategicMap.Current.GlobalMagicControl.IncrementDoubleSphereManaCount(MagicSphere.Life);
        Player playerLocal = StrategicMap.Current.Players.GetPlayers(PlayerIndex);
        StrategicMap.Current.Players.GetPlayers(PlayerIndex).MagicControl.SetActiveEnchantments(playerLocal.MagicControl.ActiveEnchantments + Set<PlayerEnchantmentType>.Of(PlayerEnchantmentType.DeathMastery));
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
        mask = MapField.ExploredFlag(player);
        for (int l = 0, last = StrategicMap.Current.MapContainer.LevelCount - 1; l <= last; l++)
        {
            mapLevel = StrategicMap.Current.MapContainer.GetMapLevel(l);
            index = 0;
            Array.Resize(ref flags, mapLevel.Width * mapLevel.Height);
            Array.Clear(flags);
            for (int y = 1, lastY = mapLevel.Height - 1; y <= lastY; y++)
            {
                for (int x = 1, lastX = mapLevel.Width - 2; x <= lastX; x++)
                {
                    if ((mapLevel.GetField(x, y).FieldFlags & mask) != 0)
                    {
                        border = false;
                        HexNumberWalkData.InitializeHexNumberWalk(out walk, x, y, 1);
                        while (walk.Radius == 1 && !border)
                        {
                            if (walk.X >= 0 && walk.X < mapLevel.Width && walk.Y >= 0 && walk.Y < mapLevel.Height)
                            {
                                border = (mapLevel.GetField(walk.X, walk.Y).FieldFlags & mask) == 0;
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
            for (int y = 1, lastY = mapLevel.Height - 1; y <= lastY; y++)
            {
                for (int x = 1, lastX = mapLevel.Width - 2; x <= lastX; x++)
                {
                    if (flags[index] != 0)
                    {
                        mapLevel.GetField(x, y).SetExplored(player, false);
                    }

                    index += 1;
                }
            }
        }

        StrategicMap.Current.UpdateVisibility();
        Array.Resize(ref flags, 0);
    }

}
