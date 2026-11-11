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
    public new const int ClassID = 135968;

    // GlobalEnchantmentSpells.pas:1475
    public DeathMasteryEnchantment(ECustomNode owner) : base(owner)
    {
        FName = AbilityRules.TranslateRStr(GameText.DeathMasteryRStr);
        FEnchantmentType = GlobalEnchantmentType.None;
        FDispelMana = 175;
    }

    // GlobalEnchantmentSpells.pas:1568
    public override void MsgProc(ref EngineMessage Msg, EngineObject Sender)
    {
        base.MsgProc(ref Msg, Sender);
        if (Msg.MsgID == 537001986)
        {
            if (Msg.Payload.Int != 0)
            {
                if (AoWHSMap.Current.FPlayers.GetPlayers(Msg.Payload.Int).FMagicControl.FEnemyEnchantments.Contains(PlayerEnchantmentType.DeathMastery))
                {
                    NewTurn((sbyte)Msg.Payload.Int);
                }
            }
        }
    }

    // GlobalEnchantmentSpells.pas:1486
    public override int GetAIUpkeepPriority() => 1250;

    // GlobalEnchantmentSpells.pas:1509
    public override bool Deactivate(byte Player)
    {
        EngineMessage Msg = default;
        base.Deactivate(Player);
        // { Original doubles Life. }
        AoWHSMap.Current.FGlobalMagicControl.DecDoubleSphereManaCount(MagicSphere.Life);
        var player = AoWHSMap.Current.FPlayers.GetPlayers(FPlayerIndex);
        AoWHSMap.Current.FPlayers.GetPlayers(FPlayerIndex).FMagicControl.SetActiveEnchantments(player.FMagicControl.FActiveEnchantments - Set<PlayerEnchantmentType>.Of(PlayerEnchantmentType.DeathMastery));
        EngineMessage.InitMsg(ref Msg, 537002241, this);
        AoWHSMap.Current.SendMsg(ref Msg, this);
        return true;
    }

    // GlobalEnchantmentSpells.pas:1489
    public override bool CanActivate(sbyte Player, ref string Reason)
    {
        bool Result = base.CanActivate(Player, ref Reason);
        if (Result)
        {
            if (AoWHSMap.Current.FPlayers.GetPlayers(Player).FMagicControl.FActiveEnchantments.Contains(PlayerEnchantmentType.DeathMastery))
            {
                var xIsAlreadyActive = AbilityRules.TranslateRStr(GameText.XIsAlreadyActiveRStr);
                var name = GetName();
                Reason = RtlUnit.Format(xIsAlreadyActive, [name]);
                Result = false;
            }
        }

        return Result;
    }

    // GlobalEnchantmentSpells.pas:1500
    public override bool Activate(sbyte Player)
    {
        base.Activate(Player);
        // { Original doubles Life. }
        AoWHSMap.Current.FGlobalMagicControl.IncDoubleSphereManaCount(MagicSphere.Life);
        var player = AoWHSMap.Current.FPlayers.GetPlayers(FPlayerIndex);
        AoWHSMap.Current.FPlayers.GetPlayers(FPlayerIndex).FMagicControl.SetActiveEnchantments(player.FMagicControl.FActiveEnchantments + Set<PlayerEnchantmentType>.Of(PlayerEnchantmentType.DeathMastery));
        return true;
    }

    // GlobalEnchantmentSpells.pas:1522
    public void NewTurn(sbyte Player)
    {
        int Index;
        byte[] Flags;
        int Mask;
        MapLevel MapLevel;
        HexNumberWalkData Walk = default;
        bool Border;
        Flags = [];
        Mask = 1 << (Player - 1);
        for (int L = 0, last = AoWHSMap.Current.MapContainer.LevelCount - 1; L <= last; L++)
        {
            MapLevel = AoWHSMap.Current.MapContainer.GetMapLevel(L);
            Index = 0;
            Array.Resize(ref Flags, MapLevel.Width * MapLevel.Height);
            Array.Clear(Flags);
            for (int Y = 1, lastY = MapLevel.Height - 1; Y <= lastY; Y++)
            {
                for (int X = 1, lastX = MapLevel.Width - 2; X <= lastX; X++)
                {
                    if ((((AoWMapField)MapLevel.GetField(X, Y)).FFieldFlags & Mask) != 0)
                    {
                        Border = false;
                        HexNumberWalkData.InitHNWalk(ref Walk, X, Y, 1);
                        while (Walk.Radius == 1 && !Border)
                        {
                            if (Walk.X >= 0 && Walk.X < MapLevel.Width && Walk.Y >= 0 && Walk.Y < MapLevel.Height)
                            {
                                Border = (((AoWMapField)MapLevel.GetField(Walk.X, Walk.Y)).FFieldFlags & Mask) == 0;
                            }

                            HexNumberWalkData.WalkToNextHN(ref Walk);
                        }

                        if (Border)
                        {
                            Flags[Index] = 1;
                        }
                    }

                    Index += 1;
                }
            }

            Index = 0;
            for (int Y = 1, lastY = MapLevel.Height - 1; Y <= lastY; Y++)
            {
                for (int X = 1, lastX = MapLevel.Width - 2; X <= lastX; X++)
                {
                    if (Flags[Index] != 0)
                    {
                        ((AoWMapField)MapLevel.GetField(X, Y)).SetExplored(Player, false);
                    }

                    Index += 1;
                }
            }
        }

        AoWHSMap.Current.UpdateVisibility();
        Array.Resize(ref Flags, 0);
    }

}
