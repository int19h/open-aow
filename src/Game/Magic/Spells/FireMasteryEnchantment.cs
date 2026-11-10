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
    public new const int ClassID = 135969;

    // GlobalEnchantmentSpells.pas:1639
    public FireMasteryEnchantment(ECustomNode owner) : base(owner)
    {
        FName = AbilityRules.TranslateRStr(GameText.FireMasteryRStr);
        FEnchantmentType = GlobalEnchantmentType.FireMastery;
        FDispelMana = 175;
    }

    // GlobalEnchantmentSpells.pas:1724
    public override void MsgProc(ref EMsg Msg, EObject Sender)
    {
        base.MsgProc(ref Msg, Sender);
        if (Msg.MsgID == 537001987)
        {
            if (Msg.Payload.Int != 0)
            {
                if (AoWHSMap.Current.FPlayers.GetPlayers(Msg.Payload.Int).FMagicControl.FActiveEnchantments.Contains(PlayerEnchantmentType.FireMastery))
                {
                    UpdateFire(2, !AoWHSMap.Current.FRequestCombatType);
                }
            }
        }
    }

    // GlobalEnchantmentSpells.pas:1650
    public override int GetAIUpkeepPriority() => 1250;

    // GlobalEnchantmentSpells.pas:1680
    public override bool Deactivate(byte Player)
    {
        Player APlayer;
        base.Deactivate(Player);
        APlayer = AoWHSMap.Current.FPlayers.GetPlayers(FPlayerIndex);
        var playerMagicControl = APlayer.FMagicControl;
        playerMagicControl.SetActiveEnchantments(playerMagicControl.FActiveEnchantments - Set<PlayerEnchantmentType>.Of(PlayerEnchantmentType.FireMastery));

        for (int I = APlayer.FArmies.Count - 1; I >= 0; I--)
        {
            for (int J = APlayer.FArmies.GetArmy(I).Count - 1; J >= 0; J--)
            {
                APlayer.FArmies.GetArmy(I).GetUnit(J).ResetFireMasteryProtection();
            }
        }

        // { Original doubles Water. }
        AoWHSMap.Current.FGlobalMagicControl.DecDoubleSphereManaCount(MagicSphere.Water);
        return true;
    }

    // GlobalEnchantmentSpells.pas:1653
    public override bool CanActivate(sbyte Player, ref string Reason)
    {
        bool Result;
        if (AoWHSMap.Current.FPlayers.GetPlayers(Player).FMagicControl.FActiveEnchantments.Contains(PlayerEnchantmentType.FireMastery))
        {
            var xIsAlreadyActive = AbilityRules.TranslateRStr(GameText.XIsAlreadyActiveRStr);
            var name = GetName();
            Reason = RtlUnit.Format(xIsAlreadyActive, [name]);
            Result = false;
        }
        else
        {
            Result = true;
        }

        return Result;
    }

    // GlobalEnchantmentSpells.pas:1663
    public override bool Activate(sbyte Player)
    {
        Player APlayer;
        base.Activate(Player);
        // { Original doubles Water. }
        AoWHSMap.Current.FGlobalMagicControl.IncDoubleSphereManaCount(MagicSphere.Water);
        APlayer = AoWHSMap.Current.FPlayers.GetPlayers(FPlayerIndex);
        var playerMagicControl = APlayer.FMagicControl;
        playerMagicControl.SetActiveEnchantments(playerMagicControl.FActiveEnchantments + Set<PlayerEnchantmentType>.Of(PlayerEnchantmentType.FireMastery));

        for (int I = APlayer.FArmies.Count - 1; I >= 0; I--)
        {
            for (int J = APlayer.FArmies.GetArmy(I).Count - 1; J >= 0; J--)
            {
                APlayer.FArmies.GetArmy(I).GetUnit(J).SetFireMasteryProtection();
            }
        }

        UpdateFire(15, true);
        return true;
    }

    // GlobalEnchantmentSpells.pas:1696
    public void UpdateFire(int Factor, bool Explosions)
    {
        int Chance;
        MapLevel MapLevel;
        Fire Fire;
        RandSeed = AoWHSMap.Current.Random(16777215);
        for (int L = 0, last = AoWHSMap.Current.MapContainer.LevelCount - 1; L <= last; L++)
        {
            MapLevel = AoWHSMap.Current.MapContainer.GetMapLevel(L);
            for (int Y = 1, lastY = MapLevel.Height - 1; Y <= lastY; Y++)
            {
                for (int X = 1, lastX = MapLevel.Width - 2; X <= lastX; X++)
                {
                    Chance = AoWMapField.IgnitionChance((AoWMapField)MapLevel.GetField(X, Y));
                    if (Chance > 0)
                    {
                        if (AoWHSMap.Current.Random(10000) < Chance * Factor)
                        {
                            Fire = AoWHSSet.Current.FFireControl.PlaceFire(X, Y, L, FireType.Large);
                            if (Fire != null)
                            {
                                if (Explosions)
                                {
                                    Fire.SetupExplosion(Random(30));
                                }

                                Fire.FTurnsLeft = 3;
                            }
                        }
                    }
                }
            }
        }
    }

}
