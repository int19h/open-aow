#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
// Translated from GlobalEnchantmentSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.Session;

using Aow.Game.World.Map;

using Aow.Game.World.Terrain;
using static Aow.Rtl.DelphiFormat;
using static Aow.Rtl.DelphiRandom;

namespace Aow.Game.Magic.Spells;

public class FireMasteryEnchantment : GlobalEnchantment
{
    // GlobalEnchantmentSpells.pas:1647
    public new const int ClassId = 135_969;

    // GlobalEnchantmentSpells.pas:1639
    public FireMasteryEnchantment(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        Name = AbilityRules.TranslateResourceString(GetEngine(), S: GameText.FireMasteryText);
        EnchantmentType = GlobalEnchantmentType.FireMastery;
        DispelMana = 175;
    }

    // GlobalEnchantmentSpells.pas:1724
    public override void MessageProc(ref EngineMessage message, EngineObject sender)
    {
        base.MessageProc(ref message, sender);
        if (message.MessageId == 537001987)
        {
            if (message.Payload.Int != 0)
            {
                if (StrategicMap.Current.Players.GetPlayers(message.Payload.Int).MagicControl.ActiveEnchantments.Contains(PlayerEnchantmentTypes.FireMastery))
                {
                    UpdateFire(2, explosions: !StrategicMap.Current.RequestCombatType);
                }
            }
        }
    }

    // GlobalEnchantmentSpells.pas:1650
    public override int GetAIUpkeepPriority() => 1_250;

    // GlobalEnchantmentSpells.pas:1680
    public override bool Deactivate(PlayerId? player)
    {
        _ = base.Deactivate(player);
        Player playerLocal = StrategicMap.Current.Players.GetPlayers(PlayerIndex);
        PlayerMagicControl playerMagicControl = playerLocal.MagicControl;
        playerMagicControl.SetActiveEnchantments(playerMagicControl.ActiveEnchantments & ~PlayerEnchantmentTypes.FireMastery);

        for (int i = playerLocal.Armies.Count - 1; i >= 0; i--)
        {
            for (int j = playerLocal.Armies.GetArmy(i).Units.Count - 1; j >= 0; j--)
            {
                playerLocal.Armies.GetArmy(i).Units.GetUnit(j).ResetFireMasteryProtection();
            }
        }

        // { Original doubles Water. }
        StrategicMap.Current.GlobalMagicControl.DecrementDoubleSphereManaCount(MagicSphere.Water);
        return true;
    }

    // GlobalEnchantmentSpells.pas:1653
    public override bool CanActivate(PlayerId? player, ref string reason)
    {
        if (StrategicMap.Current.Players.GetPlayers(player).MagicControl.ActiveEnchantments.Contains(PlayerEnchantmentTypes.FireMastery))
        {
            string xIsAlreadyActive = AbilityRules.TranslateResourceString(GetEngine(), S: GameText.IsAlreadyActiveFormat);
            string name = GetName();
            reason = Format(xIsAlreadyActive, args: [name]);
            return false;
        }
        else
        {
            return true;
        }
    }

    // GlobalEnchantmentSpells.pas:1663
    public override bool Activate(PlayerId? player)
    {
        _ = base.Activate(player);
        // { Original doubles Water. }
        StrategicMap.Current.GlobalMagicControl.IncrementDoubleSphereManaCount(MagicSphere.Water);
        Player playerLocal = StrategicMap.Current.Players.GetPlayers(PlayerIndex);
        PlayerMagicControl playerMagicControl = playerLocal.MagicControl;
        playerMagicControl.SetActiveEnchantments(playerMagicControl.ActiveEnchantments | PlayerEnchantmentTypes.FireMastery);

        for (int i = playerLocal.Armies.Count - 1; i >= 0; i--)
        {
            for (int j = playerLocal.Armies.GetArmy(i).Units.Count - 1; j >= 0; j--)
            {
                playerLocal.Armies.GetArmy(i).Units.GetUnit(j).SetFireMasteryProtection();
            }
        }

        UpdateFire(15, explosions: true);
        return true;
    }

    // GlobalEnchantmentSpells.pas:1696
    public void UpdateFire(int factor, bool explosions)
    {
        RandSeed = StrategicMap.Current.Random(16777215);
        for (int l = 0, last = StrategicMap.Current.MapContainer.LevelCount - 1; l <= last; l++)
        {
            MapLevel mapLevel = StrategicMap.Current.MapContainer.GetMapLevel(l);
            for (int y = 1, lastY = mapLevel.Height - 1; y <= lastY; y++)
            {
                for (int x = 1, lastX = mapLevel.Width - 2; x <= lastX; x++)
                {
                    int chance = MapField.IgnitionChance(mapLevel.GetField(x, y));
                    if (chance > 0)
                    {
                        if (StrategicMap.Current.Random(10_000) < chance * factor)
                        {
                            Fire fire = GamePack.Current.FireControl.PlaceFire(x, y, l, fireType: FireType.Large);
                            if (fire is not null)
                            {
                                if (explosions)
                                {
                                    fire.SetupExplosion(Random(30));
                                }

                                fire.TurnsLeft = 3;
                            }
                        }
                    }
                }
            }
        }
    }
}
