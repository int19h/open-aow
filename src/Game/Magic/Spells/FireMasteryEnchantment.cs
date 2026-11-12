// Translated from GlobalEnchantmentSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.Session;

using Aow.Game.World.Map;

using Aow.Game.World.Terrain;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Magic.Spells;

public partial class FireMasteryEnchantment : GlobalEnchantment
{
    // GlobalEnchantmentSpells.pas:1647
    public new const int ClassId = 135969;

    // GlobalEnchantmentSpells.pas:1639
    public FireMasteryEnchantment(ECustomNode owner) : base(owner)
    {
        Name = AbilityRules.TranslateResourceString(GameText.FireMasteryText);
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
                if (AoWHSMap.Current.Players.GetPlayers(message.Payload.Int).MagicControl.ActiveEnchantments.Contains(PlayerEnchantmentType.FireMastery))
                {
                    UpdateFire(2, !AoWHSMap.Current.RequestCombatType);
                }
            }
        }
    }

    // GlobalEnchantmentSpells.pas:1650
    public override int GetAIUpkeepPriority() => 1250;

    // GlobalEnchantmentSpells.pas:1680
    public override bool Deactivate(byte player)
    {
        Player playerLocal;
        base.Deactivate(player);
        playerLocal = AoWHSMap.Current.Players.GetPlayers(PlayerIndex);
        var playerMagicControl = playerLocal.MagicControl;
        playerMagicControl.SetActiveEnchantments(playerMagicControl.ActiveEnchantments - Set<PlayerEnchantmentType>.Of(PlayerEnchantmentType.FireMastery));

        for (int I = playerLocal.Armies.Count - 1; I >= 0; I--)
        {
            for (int J = playerLocal.Armies.GetArmy(I).Count - 1; J >= 0; J--)
            {
                playerLocal.Armies.GetArmy(I).GetUnit(J).ResetFireMasteryProtection();
            }
        }

        // { Original doubles Water. }
        AoWHSMap.Current.GlobalMagicControl.DecrementDoubleSphereManaCount(MagicSphere.Water);
        return true;
    }

    // GlobalEnchantmentSpells.pas:1653
    public override bool CanActivate(sbyte player, ref string reason)
    {
        bool result;
        if (AoWHSMap.Current.Players.GetPlayers(player).MagicControl.ActiveEnchantments.Contains(PlayerEnchantmentType.FireMastery))
        {
            var xIsAlreadyActive = AbilityRules.TranslateResourceString(GameText.IsAlreadyActiveFormat);
            var name = GetName();
            reason = RtlUnit.Format(xIsAlreadyActive, [name]);
            result = false;
        }
        else
        {
            result = true;
        }

        return result;
    }

    // GlobalEnchantmentSpells.pas:1663
    public override bool Activate(sbyte player)
    {
        Player playerLocal;
        base.Activate(player);
        // { Original doubles Water. }
        AoWHSMap.Current.GlobalMagicControl.IncrementDoubleSphereManaCount(MagicSphere.Water);
        playerLocal = AoWHSMap.Current.Players.GetPlayers(PlayerIndex);
        var playerMagicControl = playerLocal.MagicControl;
        playerMagicControl.SetActiveEnchantments(playerMagicControl.ActiveEnchantments + Set<PlayerEnchantmentType>.Of(PlayerEnchantmentType.FireMastery));

        for (int I = playerLocal.Armies.Count - 1; I >= 0; I--)
        {
            for (int J = playerLocal.Armies.GetArmy(I).Count - 1; J >= 0; J--)
            {
                playerLocal.Armies.GetArmy(I).GetUnit(J).SetFireMasteryProtection();
            }
        }

        UpdateFire(15, true);
        return true;
    }

    // GlobalEnchantmentSpells.pas:1696
    public void UpdateFire(int factor, bool explosions)
    {
        int chance;
        MapLevel mapLevel;
        Fire fire;
        RandSeed = AoWHSMap.Current.Random(16777215);
        for (int L = 0, last = AoWHSMap.Current.MapContainer.LevelCount - 1; L <= last; L++)
        {
            mapLevel = AoWHSMap.Current.MapContainer.GetMapLevel(L);
            for (int Y = 1, lastY = mapLevel.Height - 1; Y <= lastY; Y++)
            {
                for (int X = 1, lastX = mapLevel.Width - 2; X <= lastX; X++)
                {
                    chance = AoWMapField.IgnitionChance((AoWMapField)mapLevel.GetField(X, Y));
                    if (chance > 0)
                    {
                        if (AoWHSMap.Current.Random(10000) < chance * factor)
                        {
                            fire = AoWHSSet.Current.FireControl.PlaceFire(X, Y, L, FireType.Large);
                            if (fire != null)
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
