// Translated from MoveOnEvents.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Draw;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.World.Movement;

public partial class MoveOnEvent : MapObject
{
    public MoveOnEvent(ECustomNode owner) : base(owner)
    { }

    public byte FRadius;
    public int FTriggerPlayers;

    // MoveOnEvents.pas:151
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwInteger(ref FTriggerPlayers, 30);
        Stream.rwByte(ref FRadius, 31);
    }

    // MoveOnEvents.pas:193
    public override void Connect()
    {
        HNWalkData Walk = default;
        MapLevel MapLevel;
        MapField MapField;
        if (!FStatus.Contains(MapObjectStatusElement.PlacedOnMap))
        {
            FStatus = FStatus + Set<MapObjectStatusElement>.Of(MapObjectStatusElement.PlacedOnMap);
            var mapField = (MapField)Owner;
            MapLevel = (MapLevel)mapField.Owner;
            HNWalkData.InitHNWalk(ref Walk, mapField.Xhx, mapField.Yhx, 0);

            ref var walk = ref Walk;
            while (walk.Radius <= FRadius)
            {
                if (walk.X >= 0 && walk.Y >= 0 && walk.X < MapLevel.Width && walk.Y < MapLevel.Height)
                {
                    MapField = MapLevel.GetField(walk.X, walk.Y);
                    if (MapField == Owner)
                    {
                        MapField.RemoveHS(this);
                    }

                    MapField.AddHS(this, walk.HN);
                }

                HNWalkData.WalkToNextHN(ref Walk);
            }
        }
    }

    // MoveOnEvents.pas:223
    public override void Disconnect()
    {
        HNWalkData Walk = default;
        MapLevel MapLevel;
        if (FStatus.Contains(MapObjectStatusElement.PlacedOnMap))
        {
            MapLevel = (MapLevel)Owner.Owner;
            FStatus = FStatus - Set<MapObjectStatusElement>.Of(MapObjectStatusElement.PlacedOnMap);
            HNWalkData.InitHNWalk(ref Walk, ((MapField)Owner).Xhx, ((MapField)Owner).Yhx, 0);
            while (Walk.Radius <= FRadius)
            {
                if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < MapLevel.Width && Walk.Y < MapLevel.Height)
                {
                    MapLevel.GetField(Walk.X, Walk.Y).RemoveHS(this);
                }

                HNWalkData.WalkToNextHN(ref Walk);
            }

            FStatus = FStatus - Set<MapObjectStatusElement>.Of(MapObjectStatusElement.PlacedOnMap);
            SetOwner(null);
        }
    }

    // MoveOnEvents.pas:168
    public override void Show(int XhpSurf, int YhpSurf, in SurfaceDesc SurfaceDesc)
    {
        if (AoWHSMap.Current.Status.Contains(HSMapStatusElement.Editting))
        {
            base.Show(XhpSurf, YhpSurf, SurfaceDesc);
        }
    }

    // MoveOnEvents.pas:187
    public override void MapFieldMsgProc(ref MapFieldMsg Msg)
    {
        base.MapFieldMsgProc(ref Msg);
        if (Msg.MsgID == 4354)
        {
            ArmyPlaced(Msg.Param[0].As<ArmyHS>());
        }
    }

    // MoveOnEvents.pas:158
    public override void Activate()
    {
        base.Activate();
    }

    // MoveOnEvents.pas:163
    public override void Deactivate()
    {
        base.Deactivate();
    }

    // MoveOnEvents.pas:134
    public override bool CanSelect(int Xhp, int Yhp) => AoWHSMap.Current.Status.Contains(HSMapStatusElement.Editting);

    // MoveOnEvents.pas:129
    public override bool CanPlace(MapContainer MapContainer, sbyte Xhx, sbyte Yhx, sbyte Lhx) => true;

    // MoveOnEvents.pas:173
    public virtual void MovedOnTrigger(sbyte Player)
    {
    }

    // MoveOnEvents.pas:139
    public bool GetTriggerPlayer(sbyte Player) => Player > 0 && (FTriggerPlayers & (1 << (Player - 1))) != 0;

    // MoveOnEvents.pas:144
    public void SetTriggerPlayer(sbyte Player, bool Value)
    {
        if (Player > 0)
        {
            if (!Value)
            {
                FTriggerPlayers = FTriggerPlayers & ~(1 << (Player - 1));
            }
            else
            {
                FTriggerPlayers = FTriggerPlayers | (1 << (Player - 1));
            }
        }
    }

    // MoveOnEvents.pas:244
    public void SetRadius(byte Value)
    {
        ECustomNode MapField;
        if (Value > 10)
        {
            Value = 10;
        }

        if (Value != FRadius)
        {
            if (FStatus.Contains(MapObjectStatusElement.PlacedOnMap))
            {
                MapField = Owner;
                Disconnect();
                FRadius = Value;
                SetOwner(MapField);
                Connect();
            }
            else
            {
                FRadius = Value;
            }
        }
    }

    public virtual void ArmyPlaced(ArmyHS army)
    {
        sbyte player = army.FArmy.FPlayer;
        if (!GetTriggerPlayer(player)) return;
        MovedOnTrigger(player);
        if (FTriggerPlayers == 0 && !AoWHSMap.Current.Status.Contains(HSMapStatusElement.Editting)) Dispose();
    }
}
