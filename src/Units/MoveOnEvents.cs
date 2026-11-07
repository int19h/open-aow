// Translated from MoveOnEvents.pas.
using Aow.Rtl;
using Aow.Units.AoWE;
using Aow.Units.Engine;
using Aow.Units.EventLog;
using Aow.Units.GFXE;
using Aow.Units.HSEngine;
using static Aow.Rtl.RtlUnit;
using static Aow.Units.AoWE.AoWEUnit;
using static Aow.Units.HSEngine.HSEngineUnit;
using static Aow.Units.Sound.SoundUnit;

namespace Aow.Units.MoveOnEvents;

public partial class TMoveOnEvent : TMapObject
{
    public TMoveOnEvent(TECustomNode owner) : base(owner)
    { }

    public byte FRadius;
    public int FTriggerPlayers;

    // MoveOnEvents.pas:151
    public override void ReadWrite(TEStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwInteger(ref FTriggerPlayers, 30);
        Stream.rwByte(ref FRadius, 31);
    }

    // MoveOnEvents.pas:193
    public override void Connect()
    {
        THNWalkData Walk = default;
        TMapLevel MapLevel;
        TMapField MapField;
        if (!FStatus.Contains(TMapObjectStatusElement.moPlacedOnMap))
        {
            FStatus = FStatus + TSet<TMapObjectStatusElement>.Of(TMapObjectStatusElement.moPlacedOnMap);
            var mapField = (TMapField)Owner;
            MapLevel = (TMapLevel)mapField.Owner;
            InitHNWalk(ref Walk, mapField.Xhx, mapField.Yhx, 0);

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

                WalkToNextHN(ref Walk);
            }
        }
    }

    // MoveOnEvents.pas:223
    public override void Disconnect()
    {
        THNWalkData Walk = default;
        TMapLevel MapLevel;
        if (FStatus.Contains(TMapObjectStatusElement.moPlacedOnMap))
        {
            MapLevel = (TMapLevel)Owner.Owner;
            FStatus = FStatus - TSet<TMapObjectStatusElement>.Of(TMapObjectStatusElement.moPlacedOnMap);
            InitHNWalk(ref Walk, ((TMapField)Owner).Xhx, ((TMapField)Owner).Yhx, 0);
            while (Walk.Radius <= FRadius)
            {
                if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < MapLevel.Width && Walk.Y < MapLevel.Height)
                {
                    MapLevel.GetField(Walk.X, Walk.Y).RemoveHS(this);
                }

                WalkToNextHN(ref Walk);
            }

            FStatus = FStatus - TSet<TMapObjectStatusElement>.Of(TMapObjectStatusElement.moPlacedOnMap);
            SetOwner(null);
        }
    }

    // MoveOnEvents.pas:168
    public override void Show(int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
    {
        if (AoWHSMap.Status.Contains(THSMapStatusElement.msEditting))
        {
            base.Show(XhpSurf, YhpSurf, SurfaceDesc);
        }
    }

    // MoveOnEvents.pas:187
    public override void MapFieldMsgProc(ref TMapFieldMsg Msg)
    {
        base.MapFieldMsgProc(ref Msg);
        if (Msg.MsgID == 4354)
        {
            ArmyPlaced(Msg.Param[0].As<TArmyHS>());
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
    public override bool CanSelect(int Xhp, int Yhp) => AoWHSMap.Status.Contains(THSMapStatusElement.msEditting);

    // MoveOnEvents.pas:129
    public override bool CanPlace(TMapContainer MapContainer, sbyte Xhx, sbyte Yhx, sbyte Lhx) => true;

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
        TECustomNode MapField;
        if (Value > 10)
        {
            Value = 10;
        }

        if (Value != FRadius)
        {
            if (FStatus.Contains(TMapObjectStatusElement.moPlacedOnMap))
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

    public virtual void ArmyPlaced(TArmyHS army)
    {
        sbyte player = army.FArmy.FPlayer;
        if (!GetTriggerPlayer(player)) return;
        MovedOnTrigger(player);
        if (FTriggerPlayers == 0 && !AoWHSMap.Status.Contains(THSMapStatusElement.msEditting)) Destroy();
    }
}

public partial class TMoveOnEventResource : TMapObjectResource
{
    public TMoveOnEventResource(TECustomNode owner) : base(owner)
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

public partial class TChangeSongMoveOnEvent : TMoveOnEvent
{
    public TChangeSongMoveOnEvent(TECustomNode owner) : base(owner)
    { }

    // MoveOnEvents.pas:277
    public new const int ClassID = 132427;

    public string FSongFileName = "";

    // MoveOnEvents.pas:280
    public override void ReadWrite(TEStorageStream Stream)
    {
        base.ReadWrite(Stream);
        if (!Stream.StorageMode().Contains(TStorageModeElement.smTextOnly))
        {
            Stream.rwString(ref FSongFileName, 40);
        }
    }

    // MoveOnEvents.pas:286
    public override void MovedOnTrigger(sbyte Player)
    {
        // Use the map's seated player.
        if (Player == AoWHSMap.FSeatedPlayer)
        {
            if (CompareText(PlayingSongFileName(), FSongFileName) != 0)
            {
                PlaySongEx(FSongFileName, 2, 0, false);
            }
        }

        SetTriggerPlayer(Player, false);
    }

}

public partial class TChangeSongMoveOnEventResource : TMoveOnEventResource
{
    public TChangeSongMoveOnEventResource(TECustomNode owner) : base(owner)
    { }

    // MoveOnEvents.pas:301
    public new const int ClassID = 132428;

    // MoveOnEvents.pas:304
    public override int ResourceUserClassID() => 132427;

    // MoveOnEvents.pas:295
    public override string ResourceEditName() => TranslateRStr(ChangeSongEventRStr);

}

public partial class TPlaySampleMoveOnEvent : TMoveOnEvent
{
    public TPlaySampleMoveOnEvent(TECustomNode owner) : base(owner)
    { }

    // MoveOnEvents.pas:307
    public new const int ClassID = 132445;

    public string FSampleFileName = "";

    // MoveOnEvents.pas:310
    public override void ReadWrite(TEStorageStream Stream)
    {
        base.ReadWrite(Stream);
        if (!Stream.StorageMode().Contains(TStorageModeElement.smTextOnly))
        {
            Stream.rwString(ref FSampleFileName, 40);
        }
    }

    // MoveOnEvents.pas:316
    public override void MovedOnTrigger(sbyte Player)
    {
        if (Player == AoWHSMap.FSeatedPlayer)
        {
            PlaySample(FSampleFileName);
        }

        SetTriggerPlayer(Player, false);
    }

}

public partial class TPlaySampleMoveOnEventResource : TMoveOnEventResource
{
    public TPlaySampleMoveOnEventResource(TECustomNode owner) : base(owner)
    { }

    // MoveOnEvents.pas:328
    public new const int ClassID = 132446;

    // MoveOnEvents.pas:331
    public override int ResourceUserClassID() => 132445;

    // MoveOnEvents.pas:322
    public override string ResourceEditName() => TranslateRStr(PlaySampleEventRStr);

}

public partial class TShowMessageMoveOnEvent : TMoveOnEvent
{
    public TShowMessageMoveOnEvent(TECustomNode owner) : base(owner)
    {
        FMessage = new TStringList();
    }

    // MoveOnEvents.pas:346
    public new const int ClassID = 132429;

    public TStringList FMessage;

    // MoveOnEvents.pas:340
    public override void Destroy()
    {
        FMessage.Free();
        base.Destroy();
    }

    // MoveOnEvents.pas:349
    public override void ReadWrite(TEStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwStrings(FMessage, 40);
    }

    // MoveOnEvents.pas:334

    // MoveOnEvents.pas:355
    public override void MovedOnTrigger(sbyte Player)
    {
        TShowMessageEventLog Log;
        if (AoWHSMap.FSeatedPlayer == Player)
        {
            Log = new TShowMessageEventLog(null);
            var text = FMessage.Text;
            Log.FMessage.Text = text;
            AoWHSMap.TriggerExecuteEventLog(Log, false, null);
            Log.Release();
        }

        SetTriggerPlayer(Player, false);
    }

    // MoveOnEvents.pas:93

}

public partial class TShowMessageMoveOnEventResource : TMoveOnEventResource
{
    public TShowMessageMoveOnEventResource(TECustomNode owner) : base(owner)
    { }

    // MoveOnEvents.pas:375
    public new const int ClassID = 132430;

    // MoveOnEvents.pas:378
    public override int ResourceUserClassID() => 132429;

    // MoveOnEvents.pas:369
    public override string ResourceEditName() => TranslateRStr(ShowMessageEventRStr);

}

public static partial class MoveOnEventsUnit
{
    // MoveOnEvents.pas:381
    public static void RegisterMoveOnEvents(TEngine AEngine)
    {
        AEngine.RegisterEClasses([EClass.Of<TChangeSongMoveOnEventResource>(), EClass.Of<TChangeSongMoveOnEvent>()]);
        AEngine.RegisterEClasses([EClass.Of<TPlaySampleMoveOnEventResource>(), EClass.Of<TPlaySampleMoveOnEvent>()]);
        AEngine.RegisterEClasses([EClass.Of<TShowMessageMoveOnEventResource>(), EClass.Of<TShowMessageMoveOnEvent>()]);
    }

    public static void Initialize()
    {
    }

    public static void Finalize()
    {
    }
}
