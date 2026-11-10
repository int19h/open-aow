// Translated from MoveOnEvents.pas.
using Aow.Game.Draw;
using Aow.Game.Engine;
using Aow.Game.Engine.Nodes;
using Aow.Game.Engine.Storage;
using Aow.Game.Events;
using Aow.Game.World;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.World.Movement;

public partial class MoveOnEvent : Aow.Game.World.MapObject
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
        if (!FStatus.Contains(Aow.Game.World.MapObjectStatusElement.PlacedOnMap))
        {
            FStatus = FStatus + Set<Aow.Game.World.MapObjectStatusElement>.Of(Aow.Game.World.MapObjectStatusElement.PlacedOnMap);
            var mapField = (global::Aow.Game.World.Map.MapField)Owner;
            MapLevel = (global::Aow.Game.World.Map.MapLevel)mapField.Owner;
            Aow.Game.World.Movement.HNWalkData.InitHNWalk(ref Walk, mapField.Xhx, mapField.Yhx, 0);

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

                Aow.Game.World.Movement.HNWalkData.WalkToNextHN(ref Walk);
            }
        }
    }

    // MoveOnEvents.pas:223
    public override void Disconnect()
    {
        HNWalkData Walk = default;
        MapLevel MapLevel;
        if (FStatus.Contains(Aow.Game.World.MapObjectStatusElement.PlacedOnMap))
        {
            MapLevel = (global::Aow.Game.World.Map.MapLevel)Owner.Owner;
            FStatus = FStatus - Set<Aow.Game.World.MapObjectStatusElement>.Of(Aow.Game.World.MapObjectStatusElement.PlacedOnMap);
            Aow.Game.World.Movement.HNWalkData.InitHNWalk(ref Walk, ((MapField)Owner).Xhx, ((MapField)Owner).Yhx, 0);
            while (Walk.Radius <= FRadius)
            {
                if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < MapLevel.Width && Walk.Y < MapLevel.Height)
                {
                    MapLevel.GetField(Walk.X, Walk.Y).RemoveHS(this);
                }

                Aow.Game.World.Movement.HNWalkData.WalkToNextHN(ref Walk);
            }

            FStatus = FStatus - Set<Aow.Game.World.MapObjectStatusElement>.Of(Aow.Game.World.MapObjectStatusElement.PlacedOnMap);
            SetOwner(null);
        }
    }

    // MoveOnEvents.pas:168
    public override void Show(int XhpSurf, int YhpSurf, in SurfaceDesc SurfaceDesc)
    {
        if (Aow.Game.World.Map.AoWHSMap.Current.Status.Contains(HSMapStatusElement.Editting))
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
    public override bool CanSelect(int Xhp, int Yhp) => Aow.Game.World.Map.AoWHSMap.Current.Status.Contains(HSMapStatusElement.Editting);

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
            if (FStatus.Contains(Aow.Game.World.MapObjectStatusElement.PlacedOnMap))
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
        if (FTriggerPlayers == 0 && !Aow.Game.World.Map.AoWHSMap.Current.Status.Contains(HSMapStatusElement.Editting)) Dispose();
    }
}

public partial class MoveOnEventResource : Aow.Game.World.MapObjectResource
{
    public MoveOnEventResource(ECustomNode owner) : base(owner)
    {
        Level = 27;
    }

    // MoveOnEvents.pas:261

    // MoveOnEvents.pas:267
    public override void SetLevel(byte Value)
    {
        base.SetLevel(27);
    }

    // MoveOnEvents.pas:48

    // MoveOnEvents.pas:272
    public override string ResourceEditName() => "MoveOnEvent";

}

public partial class ChangeSongMoveOnEvent : MoveOnEvent
{
    public ChangeSongMoveOnEvent(ECustomNode owner) : base(owner)
    { }

    // MoveOnEvents.pas:277
    public new const int ClassID = 132427;

    public string FSongFileName = "";

    // MoveOnEvents.pas:280
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        if (!Stream.StorageMode().Contains(StorageModeElement.TextOnly))
        {
            Stream.rwString(ref FSongFileName, 40);
        }
    }

    // MoveOnEvents.pas:286
    public override void MovedOnTrigger(sbyte Player)
    {
        // Use the map's seated player.
        if (Player == Aow.Game.World.Map.AoWHSMap.Current.FSeatedPlayer)
        {
            if (CompareText(Aow.Game.World.Movement.ChangeSongMoveOnEvent.PlayingSongFileName(), FSongFileName) != 0)
            {
                Aow.Game.Audio.AudioPlayback.PlaySongEx(FSongFileName, 2, 0, false);
            }
        }

        SetTriggerPlayer(Player, false);
    }

    private static string PlayingSongFileName()
    {
        if (global::Aow.Game.Audio.AudioHost.SoundSystem != null)
        {
            return global::Aow.Game.Audio.AudioHost.SoundSystem.GetPlayingSongFileName();
        }
        return "";
    }

    public new partial class Resource : MoveOnEventResource
    {
        public Resource(ECustomNode owner) : base(owner)
        { }

        // MoveOnEvents.pas:301
        public new const int ClassID = 132428;

        // MoveOnEvents.pas:304
        public override int ResourceUserClassID() => 132427;

        // MoveOnEvents.pas:295
        public override string ResourceEditName() => Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.World.Movement.ChangeSongMoveOnEvent.Resource.ChangeSongEventRStr);

        private static readonly string ChangeSongEventRStr = "Change Song Event";

    }

}

public partial class PlaySampleMoveOnEvent : MoveOnEvent
{
    public PlaySampleMoveOnEvent(ECustomNode owner) : base(owner)
    { }

    // MoveOnEvents.pas:307
    public new const int ClassID = 132445;

    public string FSampleFileName = "";

    // MoveOnEvents.pas:310
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        if (!Stream.StorageMode().Contains(StorageModeElement.TextOnly))
        {
            Stream.rwString(ref FSampleFileName, 40);
        }
    }

    // MoveOnEvents.pas:316
    public override void MovedOnTrigger(sbyte Player)
    {
        if (Player == Aow.Game.World.Map.AoWHSMap.Current.FSeatedPlayer)
        {
            Aow.Game.Audio.AudioPlayback.PlaySample(FSampleFileName);
        }

        SetTriggerPlayer(Player, false);
    }

    public new partial class Resource : MoveOnEventResource
    {
        public Resource(ECustomNode owner) : base(owner)
        { }

        // MoveOnEvents.pas:328
        public new const int ClassID = 132446;

        // MoveOnEvents.pas:331
        public override int ResourceUserClassID() => 132445;

        // MoveOnEvents.pas:322
        public override string ResourceEditName() => Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.World.Movement.PlaySampleMoveOnEvent.Resource.PlaySampleEventRStr);

        private static readonly string PlaySampleEventRStr = "Play Sample";

    }

}

public partial class ShowMessageMoveOnEvent : MoveOnEvent
{
    public ShowMessageMoveOnEvent(ECustomNode owner) : base(owner)
    {
        FMessage = new StringList();
    }

    // MoveOnEvents.pas:346
    public new const int ClassID = 132429;

    public StringList FMessage;

    // MoveOnEvents.pas:340
    public override void Dispose()
    {
        FMessage?.Dispose();
        base.Dispose();
    }

    // MoveOnEvents.pas:349
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwStrings(FMessage, 40);
    }

    // MoveOnEvents.pas:334

    // MoveOnEvents.pas:355
    public override void MovedOnTrigger(sbyte Player)
    {
        ShowMessageEventLog Log;
        if (Aow.Game.World.Map.AoWHSMap.Current.FSeatedPlayer == Player)
        {
            Log = new ShowMessageEventLog(null);
            var text = FMessage.Text;
            Log.FMessage.Text = text;
            Aow.Game.World.Map.AoWHSMap.Current.TriggerExecuteEventLog(Log, false, null);
            Log.Release();
        }

        SetTriggerPlayer(Player, false);
    }

    public new partial class Resource : MoveOnEventResource
    {
        public Resource(ECustomNode owner) : base(owner)
        { }

        // MoveOnEvents.pas:375
        public new const int ClassID = 132430;

        // MoveOnEvents.pas:378
        public override int ResourceUserClassID() => 132429;

        // MoveOnEvents.pas:369
        public override string ResourceEditName() => Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.World.Movement.ShowMessageMoveOnEvent.Resource.ShowMessageEventRStr);

        private static readonly string ShowMessageEventRStr = "Show Message Event";

    }

    // MoveOnEvents.pas:93

}
