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

    public new const int ClassId = 135447;

    public int UnitId;
    public sbyte Player;

    // MoveT.PAS:3532
    public override void ReadWrite(StorageStream stream)
    {
        base.ReadWrite(stream);
        stream.Byte(ref Unsafe.As<sbyte, byte>(ref Player), 20);
        stream.Int32(ref UnitId, 21);
    }

    // MoveT.PAS:3539
    public override ExecuteResult Execute(TokenExecutor tokenEvent)
    {
        Troop troop;
        PlayerStructure structure;
        ArmyHexSprite armyHexSprite;
        GroupBehavior behavior;
        int I;
        int N;
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        troop = StrategicMap.Current.UnitControl.FindUnit(UnitId);
        if (troop != null && troop.Player == Player)
        {
            troop.AddRef();
            if (troop.GetRace() == 5 && troop.Player > 0 && StrategicMap.Current.Players.GetPlayers(troop.Player).PlayerRace == 5 && (troop.GetAbilityEnabled(162) || troop.GetAbilityEnabled(164) || troop.GetAbilityEnabled(165)))
            {
                StrategicMap.Current.Players.GetPlayers(troop.Player).HeroEmerging = true;
            }

            if (!troop.GetAbilityEnabled(155))
            {
                troop.GetLocation(ref X, ref Y, ref L);
                structure = (PlayerStructure)StrategicMap.Current.MapContainer.GetField(X, Y, L).FindNoneTransparentHexSprite(typeof(PlayerStructure));
                if (!(troop.Owner is Army) || troop.Owner.Count > 1 || (structure != null && structure.Player > 0))
                {
                    troop.GetLocation(ref X, ref Y, ref L);
                    troop.SetOwner(null);
                    armyHexSprite = troop.Place(X, Y, L, 0, Set<UnitPlaceModeElement>.Of(UnitPlaceModeElement.ScanArea));
                }
                else
                {
                    troop.ArmyOwner.SetPlayer(0);
                    armyHexSprite = troop.ArmyOwner.GetArmyHexSprite();
                }

                if (armyHexSprite != null)
                {
                    N = StrategicMap.Current.Random(10);
                    if (N == 0)
                    {
                        behavior = GroupBehavior.GuardArea;
                    }
                    else
                    {
                        if (N < 7)
                        {
                            behavior = GroupBehavior.Scout;
                        }
                        else
                        {
                            behavior = GroupBehavior.Raid;
                        }
                    }

                    I = armyHexSprite.Army.IndexOfBehavior(behavior);
                    if (I == -1)
                    {
                        var indexOf = armyHexSprite.Army.IndexOf(troop);
                        armyHexSprite.Army.SetBehavior((byte)(1 << indexOf), behavior);
                    }
                    else
                    {
                        var unit = armyHexSprite.Army.GetUnit(I);
                        var indexOf = armyHexSprite.Army.IndexOf(troop);
                        armyHexSprite.Army.SetGroup((byte)(1 << indexOf), unit.Group);
                    }
                }
            }
            else
            {
                troop.Killed();
            }

            troop.RemoveEnchantments();
            troop.Release();
            if (StrategicMap.Current.SeatedPlayer == Player)
            {
                StrategicMap.Current.SelectedArmy.Unselect();
            }

            StrategicMap.Current.Players.GetPlayers(Player).UpdateIncome();
        }

        return ExecuteResult.Done;
    }

    // NetworkE.pas:230

}
