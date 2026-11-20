#nullable disable
// Translated from AoWE.pas.
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;
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
public class DisbandUnitTokenEvent(CustomNode owner) : TokenEvent(owner)
{
    public new const int ClassId = 135_447;

    public int UnitId { get; set; }
    public sbyte Player { get; set; }

    // MoveT.PAS:3532
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        Player = PersistSupport.UnsignedByte(persister, name: "Player", value: Player);
        UnitId = persister.Int32("UnitId", value: UnitId);
    }

    // MoveT.PAS:3539
    public override ExecuteResult Execute(TokenExecutor tokenEvent)
    {
        Troop troop = StrategicMap.Current.UnitControl.FindUnit(UnitId);
        if (troop is not null && troop.Player == Player)
        {
            _ = troop.AddRef();
            if (troop.GetRace() == 5 && troop.Player > 0 && StrategicMap.Current.Players.GetPlayers(troop.Player).PlayerRace == 5 && (troop.GetAbilityEnabled(162) || troop.GetAbilityEnabled(164) || troop.GetAbilityEnabled(165)))
            {
                StrategicMap.Current.Players.GetPlayers(troop.Player).HeroEmerging = true;
            }

            if (!troop.GetAbilityEnabled(155))
            {
                MapCoordinate coordinate5 = troop.GetLocation();
                sbyte x = coordinate5.X;
                sbyte y = coordinate5.Y;
                sbyte l = coordinate5.MapLevel;
                PlayerStructure structure = StrategicMap.Current.MapContainer.GetField(x, y, l).FindNoneTransparentHexSprite<PlayerStructure>();
                ArmyHexSprite armyHexSprite;
                if (troop.Owner is not Army || troop.Owner.Count > 1 || (structure is not null && structure.Player > 0))
                {
                    MapCoordinate coordinate6 = troop.GetLocation();
                    x = coordinate6.X;
                    y = coordinate6.Y;
                    l = coordinate6.MapLevel;
                    troop.SetOwner(null);
                    armyHexSprite = troop.Place(x, y, l, player: 0, mode: Set<UnitPlaceModeElement>.Of(UnitPlaceModeElement.ScanArea));
                }
                else
                {
                    troop.ArmyOwner.SetPlayer(0);
                    armyHexSprite = troop.ArmyOwner.GetArmyHexSprite();
                }

                if (armyHexSprite is not null)
                {
                    int n = StrategicMap.Current.Random(10);
                    GroupBehavior behavior = n == 0 ? GroupBehavior.GuardArea : n < 7 ? GroupBehavior.Scout : GroupBehavior.Raid;

                    int i = armyHexSprite.Army.Units.IndexOfBehavior(behavior);
                    if (i == -1)
                    {
                        int indexOf = armyHexSprite.Army.Units.IndexOf(troop);
                        armyHexSprite.Army.SetBehavior((byte)(1 << indexOf), behavior);
                    }
                    else
                    {
                        Troop unit = armyHexSprite.Army.Units.GetUnit(i);
                        int indexOf = armyHexSprite.Army.Units.IndexOf(troop);
                        armyHexSprite.Army.SetGroup((byte)(1 << indexOf), unit.Group);
                    }
                }
            }
            else
            {
                troop.Killed();
            }

            troop.RemoveEnchantments();
            _ = troop.Release();
            if (StrategicMap.Current.SeatedPlayer == Player)
            {
                StrategicMap.Current.SelectedArmy.Unselect();
            }

            StrategicMap.Current.Players.GetPlayers(Player).UpdateIncome();
        }

        return ExecuteResult.Done;
    }
}
