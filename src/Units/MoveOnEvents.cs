// Translated from MoveOnEvents.pas.
using System;
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Exception = Aow.Rtl.Exception;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Aow.Units.Engine;
using static Aow.Units.Engine.Engine;
using Aow.Units.GFXE;
using static Aow.Units.GFXE.GFXE;
using Aow.Units.ImageLib;
using static Aow.Units.ImageLib.ImageLib;
using Aow.Units.Sound;
using static Aow.Units.Sound.Sound;
using Aow.Units.NetworkE;
using static Aow.Units.NetworkE.NetworkE;
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngine;
using Aow.Units.IsoHex;
using static Aow.Units.IsoHex.IsoHex;
using Aow.Units.ILTer;
using static Aow.Units.ILTer.ILTer;
using Aow.Units.Terrain;
using static Aow.Units.Terrain.Terrain;
using Aow.Units.Hexagon;
using static Aow.Units.Hexagon.Hexagon;
using Aow.Units.Road;
using static Aow.Units.Road.Road;
using Aow.Units.BorderHx;
using static Aow.Units.BorderHx.BorderHx;
using Aow.Units.EventLog;
using static Aow.Units.EventLog.EventLog;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;

namespace Aow.Units.MoveOnEvents
{
    // { TMoveOnEvent (instance size 24, VMT 557D2A34) }
    public partial class TMoveOnEvent : Aow.Units.HSEngine.TMapObject
    {
        public TMoveOnEvent(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
        {}

        public byte FRadius;
        // { $10 }
        public Aow.Units.FixedArray_byte_3 FReserved11;
        // { $11 }
        public int FTriggerPlayers;
        // MoveOnEvents.pas:151
        // { $14 }
        public override void ReadWrite(Aow.Units.Engine.TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwInteger(ref FTriggerPlayers, 30);
            Stream.rwByte(ref FRadius, 31);
        }

        // MoveOnEvents.pas:193
        public override void Connect()
        {
            Aow.Units.HSEngine.THNWalkData Walk = default;
            Aow.Units.HSEngine.TMapLevel MapLevel = default;
            Aow.Units.HSEngine.TMapField MapField = default;
            if (!FStatus.Contains(Aow.Units.HSEngine.TMapObjectStatusElement.moPlacedOnMap))
            {
                FStatus = FStatus + TSet<Aow.Units.HSEngine.TMapObjectStatusElement>.Of(Aow.Units.HSEngine.TMapObjectStatusElement.moPlacedOnMap);
                var mapField = (Aow.Units.HSEngine.TMapField)Owner;
                MapLevel = (Aow.Units.HSEngine.TMapLevel)mapField.Owner;
                Aow.Units.HSEngine.HSEngine.InitHNWalk(ref Walk, mapField.Xhx, mapField.Yhx, 0);

                ref var walk = ref Walk;
                while (walk.Radius <= FRadius)
                {
                    if ((((walk.X >= 0) && (walk.Y >= 0)) && (walk.X < MapLevel.Width)) && (walk.Y < MapLevel.Height))
                    {
                        var call__3 = MapLevel.GetField(walk.X, walk.Y);
                        MapField = call__3;
                        if (MapField == Owner)
                        {
                            MapField.RemoveHS(this);
                        }

                        MapField.AddHS(this, walk.HN);
                    }

                    Aow.Units.HSEngine.HSEngine.WalkToNextHN(ref Walk);
                }
            }
        }

        // MoveOnEvents.pas:223
        public override void Disconnect()
        {
            Aow.Units.HSEngine.THNWalkData Walk = default;
            Aow.Units.HSEngine.TMapLevel MapLevel = default;
            if (FStatus.Contains(Aow.Units.HSEngine.TMapObjectStatusElement.moPlacedOnMap))
            {
                MapLevel = (Aow.Units.HSEngine.TMapLevel)Owner.Owner;
                FStatus = FStatus - TSet<Aow.Units.HSEngine.TMapObjectStatusElement>.Of(Aow.Units.HSEngine.TMapObjectStatusElement.moPlacedOnMap);
                Aow.Units.HSEngine.HSEngine.InitHNWalk(ref Walk, ((Aow.Units.HSEngine.TMapField)Owner).Xhx, ((Aow.Units.HSEngine.TMapField)Owner).Yhx, 0);
                while (Walk.Radius <= FRadius)
                {
                    if ((((Walk.X >= 0) && (Walk.Y >= 0)) && (Walk.X < MapLevel.Width)) && (Walk.Y < MapLevel.Height))
                    {
                        var call__4 = MapLevel.GetField(Walk.X, Walk.Y);
                        call__4.RemoveHS(this);
                    }

                    Aow.Units.HSEngine.HSEngine.WalkToNextHN(ref Walk);
                }

                FStatus = FStatus - TSet<Aow.Units.HSEngine.TMapObjectStatusElement>.Of(Aow.Units.HSEngine.TMapObjectStatusElement.moPlacedOnMap);
                SetOwner(null);
            }
        }

        // MoveOnEvents.pas:168
        public override void Show(int XhpSurf, int YhpSurf, in Aow.Units.GFXE.TSurfaceDesc SurfaceDesc)
        {
            if (Aow.Units.AoWE.AoWE.AoWHSMap.Status.Contains(Aow.Units.HSEngine.THSMapStatusElement.msEditting))
            {
                base.Show(XhpSurf, YhpSurf, SurfaceDesc);
            }
        }

        // MoveOnEvents.pas:187
        public override void MapFieldMsgProc(ref Aow.Units.HSEngine.TMapFieldMsg Msg)
        {
            base.MapFieldMsgProc(ref Msg);
            if (Msg.MsgID == 4354)
            {
                ArmyPlaced(Msg.Param[0].Ref);
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
        public override bool CanSelect(int Xhp, int Yhp)
        {
            bool Result = default;
            Result = Aow.Units.AoWE.AoWE.AoWHSMap.Status.Contains(Aow.Units.HSEngine.THSMapStatusElement.msEditting);
            return Result;
        }

        // MoveOnEvents.pas:129
        public override bool CanPlace(Aow.Units.HSEngine.TMapContainer MapContainer, sbyte Xhx, sbyte Yhx, sbyte Lhx)
        {
            bool Result = default;
            Result = true;
            return Result;
        }

        // MoveOnEvents.pas:173
        public virtual void MovedOnTrigger(sbyte Player)
        {
        }

        // MoveOnEvents.pas:139
        // { +120 }
        public bool GetTriggerPlayer(sbyte Player)
        {
            bool Result = default;
            Result = (Player > 0) && ((FTriggerPlayers & (1 << (Player - 1))) != 0);
            return Result;
        }

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
            Aow.Units.Engine.TECustomNode MapField = default;
            if (Value > 10)
            {
                Value = 10;
            }

            if (Value != FRadius)
            {
                if (FStatus.Contains(Aow.Units.HSEngine.TMapObjectStatusElement.moPlacedOnMap))
                {
                    MapField = Owner;
                    Disconnect();
                    FRadius = unchecked((byte)Value);
                    SetOwner(MapField);
                    Connect();
                }
                else
                {
                    FRadius = unchecked((byte)Value);
                }
            }
        }
    }

    // { TMoveOnEventResource (instance size 52, VMT 557D2BD4) }
    public partial class TMoveOnEventResource : Aow.Units.HSEngine.TMapObjectResource
    {
        public TMoveOnEventResource(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
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
        public override Aow.Rtl.AnsiString ResourceEditName()
        {
            Aow.Rtl.AnsiString Result = default;
            Result = (AnsiString)"MoveOnEvent";
            return Result;
        }

    }

    // { TChangeSongMoveOnEvent (instance size 28, VMT 557D2CD0) }
    public partial class TChangeSongMoveOnEvent : Aow.Units.MoveOnEvents.TMoveOnEvent
    {
        public TChangeSongMoveOnEvent(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
        {}

        // MoveOnEvents.pas:277
        public new const int ClassID = 132427;

        public Aow.Rtl.AnsiString FSongFileName;
        // MoveOnEvents.pas:280
        // { $18 }
        public override void ReadWrite(Aow.Units.Engine.TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            if (!Stream.StorageMode().Contains(Aow.Units.Engine.TStorageModeElement.smTextOnly))
            {
                Stream.rwString(ref FSongFileName, 40);
            }
        }

        // MoveOnEvents.pas:286
        public override void MovedOnTrigger(sbyte Player)
        {
            // { needs: seated-player field at AoWHSMap +$A5 (557D3878). }
            if (Player == Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer)
            {
                var call__5 = Aow.Units.Sound.Sound.PlayingSongFileName();
                if (Aow.Rtl.Rtl.CompareText(call__5, FSongFileName) != 0)
                {
                    Aow.Units.Sound.Sound.PlaySongEx(FSongFileName, 2, 0, false);
                }
            }

            SetTriggerPlayer(unchecked((sbyte)Player), false);
        }

    }

    // { TChangeSongMoveOnEventResource (instance size 52, VMT 557D2E98) }
    public partial class TChangeSongMoveOnEventResource : Aow.Units.MoveOnEvents.TMoveOnEventResource
    {
        public TChangeSongMoveOnEventResource(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
        {}

        // MoveOnEvents.pas:301
        public new const int ClassID = 132428;

        // MoveOnEvents.pas:304
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 132427;
            return Result;
        }

        // MoveOnEvents.pas:295
        public override Aow.Rtl.AnsiString ResourceEditName()
        {
            Aow.Rtl.AnsiString Result = default;
            // { needs: TranslateRStr(const S: string): string; explicit hidden result. }
            Result = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.ChangeSongEventRStr);
            return Result;
        }

    }

    // { TPlaySampleMoveOnEvent (instance size 28, VMT 557D2FA8) }
    public partial class TPlaySampleMoveOnEvent : Aow.Units.MoveOnEvents.TMoveOnEvent
    {
        public TPlaySampleMoveOnEvent(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
        {}

        // MoveOnEvents.pas:307
        public new const int ClassID = 132445;

        public Aow.Rtl.AnsiString FSampleFileName;
        // MoveOnEvents.pas:310
        // { $18 }
        public override void ReadWrite(Aow.Units.Engine.TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            if (!Stream.StorageMode().Contains(Aow.Units.Engine.TStorageModeElement.smTextOnly))
            {
                Stream.rwString(ref FSampleFileName, 40);
            }
        }

        // MoveOnEvents.pas:316
        public override void MovedOnTrigger(sbyte Player)
        {
            if (Player == Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer)
            {
                Aow.Units.Sound.Sound.PlaySample(FSampleFileName);
            }

            SetTriggerPlayer(unchecked((sbyte)Player), false);
        }

    }

    // { TPlaySampleMoveOnEventResource (instance size 52, VMT 557D3170) }
    public partial class TPlaySampleMoveOnEventResource : Aow.Units.MoveOnEvents.TMoveOnEventResource
    {
        public TPlaySampleMoveOnEventResource(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
        {}

        // MoveOnEvents.pas:328
        public new const int ClassID = 132446;

        // MoveOnEvents.pas:331
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 132445;
            return Result;
        }

        // MoveOnEvents.pas:322
        public override Aow.Rtl.AnsiString ResourceEditName()
        {
            Aow.Rtl.AnsiString Result = default;
            // { needs: TranslateRStr(const S: string): string; explicit hidden result. }
            Result = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.PlaySampleEventRStr);
            return Result;
        }

    }

    // { TShowMessageMoveOnEvent (instance size 28, VMT 557D3280) }
    public partial class TShowMessageMoveOnEvent : Aow.Units.MoveOnEvents.TMoveOnEvent
    {
        public TShowMessageMoveOnEvent(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
        {
            var call__6 = new Aow.Rtl.TStringList();
            FMessage = call__6;
        }

        // MoveOnEvents.pas:346
        public new const int ClassID = 132429;

        public Aow.Rtl.TStringList FMessage;
        // MoveOnEvents.pas:340
        // { $18 }
        public override void Destroy()
        {
            FMessage.Free();
            base.Destroy();
        }

        // MoveOnEvents.pas:349
        public override void ReadWrite(Aow.Units.Engine.TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwStrings(FMessage, 40);
        }

        // MoveOnEvents.pas:334


        // MoveOnEvents.pas:355
        public override void MovedOnTrigger(sbyte Player)
        {
            Aow.Units.EventLog.TShowMessageEventLog Log = default;
            if (Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer == Player)
            {
                Log = new Aow.Units.EventLog.TShowMessageEventLog(null);
                var call__7 = FMessage.Text;
                Log.FMessage.Text = call__7;
                Aow.Units.AoWE.AoWE.AoWHSMap.TriggerExecuteEventLog(Log, false, null);
                Log.Release();
            }

            SetTriggerPlayer(unchecked((sbyte)Player), false);
        }

        // MoveOnEvents.pas:93


    }

    // { TShowMessageMoveOnEventResource (instance size 52, VMT 557D3434) }
    public partial class TShowMessageMoveOnEventResource : Aow.Units.MoveOnEvents.TMoveOnEventResource
    {
        public TShowMessageMoveOnEventResource(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
        {}

        // MoveOnEvents.pas:375
        public new const int ClassID = 132430;

        // MoveOnEvents.pas:378
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 132429;
            return Result;
        }

        // MoveOnEvents.pas:369
        public override Aow.Rtl.AnsiString ResourceEditName()
        {
            Aow.Rtl.AnsiString Result = default;
            // { needs: TranslateRStr(const S: string): string; explicit hidden result. }
            Result = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.ShowMessageEventRStr);
            return Result;
        }

    }

    public static partial class MoveOnEvents
    {
        // MoveOnEvents.pas:381
        public static void RegisterMoveOnEvents(Aow.Units.Engine.TEngine AEngine)
        {
            AEngine.RegisterEClasses([Aow.Units.Engine.EClass.Of<Aow.Units.MoveOnEvents.TChangeSongMoveOnEventResource>(), Aow.Units.Engine.EClass.Of<Aow.Units.MoveOnEvents.TChangeSongMoveOnEvent>()]);
            AEngine.RegisterEClasses([Aow.Units.Engine.EClass.Of<Aow.Units.MoveOnEvents.TPlaySampleMoveOnEventResource>(), Aow.Units.Engine.EClass.Of<Aow.Units.MoveOnEvents.TPlaySampleMoveOnEvent>()]);
            AEngine.RegisterEClasses([Aow.Units.Engine.EClass.Of<Aow.Units.MoveOnEvents.TShowMessageMoveOnEventResource>(), Aow.Units.Engine.EClass.Of<Aow.Units.MoveOnEvents.TShowMessageMoveOnEvent>()]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
