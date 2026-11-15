#nullable disable
// Translated from AoWE.pas.
using System.Runtime.CompilerServices;
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
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
public partial class DisbandUnitTokenEvent : TokenEvent
{
    public DisbandUnitTokenEvent(CustomNode owner) : base(owner)
    { }

    public new const int ClassId = 135447;

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
        Troop troop;
        PlayerStructure structure;
        ArmyHexSprite armyHexSprite;
        GroupBehavior behavior;
        int i;
        int n;
        sbyte x = default;
        sbyte y = default;
        sbyte l = default;
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
                troop.GetLocation(out x, out y, out l);
                structure = (PlayerStructure)StrategicMap.Current.MapContainer.GetField(x, y, l).FindNoneTransparentHexSprite(typeof(PlayerStructure));
                if (!(troop.Owner is Army) || troop.Owner.Count > 1 || (structure != null && structure.Player > 0))
                {
                    troop.GetLocation(out x, out y, out l);
                    troop.SetOwner(null);
                    armyHexSprite = troop.Place(x, y, l, 0, Set<UnitPlaceModeElement>.Of(UnitPlaceModeElement.ScanArea));
                }
                else
                {
                    troop.ArmyOwner.SetPlayer(0);
                    armyHexSprite = troop.ArmyOwner.GetArmyHexSprite();
                }

                if (armyHexSprite != null)
                {
                    n = StrategicMap.Current.Random(10);
                    if (n == 0)
                    {
                        behavior = GroupBehavior.GuardArea;
                    }
                    else
                    {
                        if (n < 7)
                        {
                            behavior = GroupBehavior.Scout;
                        }
                        else
                        {
                            behavior = GroupBehavior.Raid;
                        }
                    }

                    i = armyHexSprite.Army.Units.IndexOfBehavior(behavior);
                    if (i == -1)
                    {
                        var indexOf = armyHexSprite.Army.Units.IndexOf(troop);
                        armyHexSprite.Army.SetBehavior((byte)(1 << indexOf), behavior);
                    }
                    else
                    {
                        var unit = armyHexSprite.Army.Units.GetUnit(i);
                        var indexOf = armyHexSprite.Army.Units.IndexOf(troop);
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
