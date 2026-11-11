// Translated from AoWE.pas.
using System.Runtime.CompilerServices;

using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Game.AI.Groups;
using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.Net;

using Aow.Game.Units;

using Aow.Game.World.Map;

using Aow.Game.World.Structures;

using Aow.Rtl;

namespace Aow.Game.Commands;

// unit ID, player
public partial class DisbandUnitTokenEvent : TokenEvent
{
    public DisbandUnitTokenEvent(ECustomNode owner) : base(owner)
    { }

    public new const int ClassID = 135447;

    public int FUnitID;
    public sbyte FPlayer;

    // MoveT.PAS:3532
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FPlayer), 20);
        Stream.rwInteger(ref FUnitID, 21);
    }

    // MoveT.PAS:3539
    public override ExecuteResult Execute(TokenExecutor TE)
    {
        Troop AUnit;
        PlayerStructure Structure;
        ArmyHexSprite ArmyHS;
        GroupBehavior Behavior;
        int I;
        int N;
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        AUnit = AoWHSMap.Current.FUnitControl.FindUnit(FUnitID);
        if (AUnit != null && AUnit.FPlayer == FPlayer)
        {
            AUnit.AddRef();
            if (AUnit.GetRace() == 5 && AUnit.FPlayer > 0 && AoWHSMap.Current.FPlayers.GetPlayers(AUnit.FPlayer).FPlayerRace == 5 && (AUnit.GetAbilityEnabled(162) || AUnit.GetAbilityEnabled(164) || AUnit.GetAbilityEnabled(165)))
            {
                AoWHSMap.Current.FPlayers.GetPlayers(AUnit.FPlayer).FHeroEmerging = true;
            }

            if (!AUnit.GetAbilityEnabled(155))
            {
                AUnit.GetLocation(ref X, ref Y, ref L);
                Structure = (PlayerStructure)AoWHSMap.Current.MapContainer.GetField(X, Y, L).FindNoneTransparentHS(typeof(PlayerStructure));
                if (!(AUnit.Owner is Army) || AUnit.Owner.Count > 1 || (Structure != null && Structure.FPlayer > 0))
                {
                    AUnit.GetLocation(ref X, ref Y, ref L);
                    AUnit.SetOwner(null);
                    ArmyHS = AUnit.Place(X, Y, L, 0, Set<UnitPlaceModeElement>.Of(UnitPlaceModeElement.ScanArea));
                }
                else
                {
                    AUnit.ArmyOwner.SetPlayer(0);
                    ArmyHS = AUnit.ArmyOwner.GetArmyHS();
                }

                if (ArmyHS != null)
                {
                    N = AoWHSMap.Current.Random(10);
                    if (N == 0)
                    {
                        Behavior = GroupBehavior.GuardArea;
                    }
                    else
                    {
                        if (N < 7)
                        {
                            Behavior = GroupBehavior.Scout;
                        }
                        else
                        {
                            Behavior = GroupBehavior.Raid;
                        }
                    }

                    I = ArmyHS.FArmy.IndexOfBehavior(Behavior);
                    if (I == -1)
                    {
                        var indexOf = ArmyHS.FArmy.IndexOf(AUnit);
                        ArmyHS.FArmy.SetBehavior((byte)(1 << indexOf), Behavior);
                    }
                    else
                    {
                        var unit = ArmyHS.FArmy.GetUnit(I);
                        var indexOf = ArmyHS.FArmy.IndexOf(AUnit);
                        ArmyHS.FArmy.SetGroup((byte)(1 << indexOf), unit.FGroup);
                    }
                }
            }
            else
            {
                AUnit.Killed();
            }

            AUnit.RemoveEnchantments();
            AUnit.Release();
            if (AoWHSMap.Current.FSeatedPlayer == FPlayer)
            {
                AoWHSMap.Current.FSelectedArmy.Unselect();
            }

            AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).UpdateIncome();
        }

        return ExecuteResult.Done;
    }

    // NetworkE.pas:230

}
