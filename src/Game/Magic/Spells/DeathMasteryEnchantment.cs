#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
// Translated from GlobalEnchantmentSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.Session;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

public class DeathMasteryEnchantment : GlobalEnchantment
{
    // GlobalEnchantmentSpells.pas:1483
    public new const int ClassId = 135_968;

    // GlobalEnchantmentSpells.pas:1475
    public DeathMasteryEnchantment(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        Name = AbilityRules.TranslateResourceString(GetEngine(), S: GameText.DeathMasteryText);
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
                if (GetEngine().StrategicMap.Players.GetPlayers(message.Payload.Int).MagicControl.EnemyEnchantments.Contains(PlayerEnchantmentTypes.DeathMastery))
                {
                    NewTurn(PlayerId.FromOriginal((sbyte)message.Payload.Int));
                }
            }
        }
    }

    // GlobalEnchantmentSpells.pas:1486
    public override int GetAIUpkeepPriority() => 1_250;

    // GlobalEnchantmentSpells.pas:1509
    public override bool Deactivate(PlayerId? player)
    {
        _ = base.Deactivate(player);
        // { Original doubles Life. }
        StrategicMap strategicMap = GetEngine().StrategicMap;
        strategicMap.GlobalMagicControl.DecrementDoubleSphereManaCount(MagicSphere.Life);
        Player playerLocal = strategicMap.Players.GetPlayers(PlayerIndex);
        strategicMap.Players.GetPlayers(PlayerIndex).MagicControl.SetActiveEnchantments(playerLocal.MagicControl.ActiveEnchantments & ~PlayerEnchantmentTypes.DeathMastery);
        EngineMessage.InitializeMessage(out EngineMessage message, messageId: 537002241, sender: this);
        strategicMap.SendMessage(ref message, sender: this);
        return true;
    }

    // GlobalEnchantmentSpells.pas:1489
    public override bool CanActivate(PlayerId? player, ref string reason)
    {
        bool result = base.CanActivate(player, ref reason);
        if (result)
        {
            if (GetEngine().StrategicMap.Players.GetPlayers(player).MagicControl.ActiveEnchantments.Contains(PlayerEnchantmentTypes.DeathMastery))
            {
                string xIsAlreadyActive = AbilityRules.TranslateResourceString(GetEngine(), S: GameText.IsAlreadyActiveFormat);
                string name = GetName();
                reason = DelphiFormat.Format(xIsAlreadyActive, args: [name]);
                return false;
            }
        }

        return result;
    }

    // GlobalEnchantmentSpells.pas:1500
    public override bool Activate(PlayerId? player)
    {
        _ = base.Activate(player);
        // { Original doubles Life. }
        StrategicMap strategicMap = GetEngine().StrategicMap;
        strategicMap.GlobalMagicControl.IncrementDoubleSphereManaCount(MagicSphere.Life);
        Player playerLocal = strategicMap.Players.GetPlayers(PlayerIndex);
        strategicMap.Players.GetPlayers(PlayerIndex).MagicControl.SetActiveEnchantments(playerLocal.MagicControl.ActiveEnchantments | PlayerEnchantmentTypes.DeathMastery);
        return true;
    }

    // GlobalEnchantmentSpells.pas:1522
    public void NewTurn(PlayerId? player)
    {
        byte[] flags = [];
        int mask = MapField.ExploredFlag(player);
        StrategicMap strategicMap = GetEngine().StrategicMap;
        for (int l = 0, last = strategicMap.MapContainer.LevelCount - 1; l <= last; l++)
        {
            MapLevel mapLevel = strategicMap.MapContainer.GetMapLevel(l);
            int index = 0;
            Array.Resize(ref flags, newSize: mapLevel.Width * mapLevel.Height);
            Array.Clear(flags);
            for (int y = 1, lastY = mapLevel.Height - 1; y <= lastY; y++)
            {
                for (int x = 1, lastX = mapLevel.Width - 2; x <= lastX; x++)
                {
                    if ((mapLevel.GetField(x, y).FieldFlags & mask) != 0)
                    {
                        bool border = false;
                        HexNumberWalkData.InitializeHexNumberWalk(out HexNumberWalkData walk, x, y, radius: 1);
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
                        mapLevel.GetField(x, y).SetExplored(player, value: false);
                    }

                    index += 1;
                }
            }
        }

        strategicMap.UpdateVisibility();
        Array.Resize(ref flags, newSize: 0);
    }
}
