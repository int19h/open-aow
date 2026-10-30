// Translated from Teleport.pas.
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
using Aow.Units.Animation;
using static Aow.Units.Animation.Animation;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;

namespace Aow.Units.Teleport
{
    public enum TTeleportStateElement : byte
    {
        tsRazed = 0,
        ts1 = 1,
        ts2 = 2,
        ts3 = 3,
        ts4 = 4,
        ts5 = 5,
        ts6 = 6,
        ts7 = 7
    }

    public enum TTeleportMoveFlagsElement : byte
    {
        tm0 = 0,
        tm1 = 1,
        tm2 = 2,
        tm3 = 3,
        tm4 = 4,
        tm5 = 5,
        tm6 = 6,
        tm7 = 7
    }

    // FDestinationX, FDestinationY, FDestinationL, FState (bit 0 razed)
    public partial class TTeleport : Aow.Units.AoWE.TStructure
    {
        public TTeleport(TECustomNode owner) : base(owner)
        {
            FDestinationX = 0;
            FDestinationY = 0;
            FDestinationL = 0;
        }

        // Teleport.pas:105
        public new const int ClassID = 131332;

        public sbyte FDestinationX;
        public sbyte FDestinationY;
        public sbyte FDestinationL;
        public TSet<Aow.Units.Teleport.TTeleportStateElement> FState;
        // Teleport.pas:122
        public override void ReadWrite(Aow.Units.Engine.TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FDestinationX), 7);
            Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FDestinationY), 8);
            Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FDestinationL), 9);
            Aow.Rtl.Rtl.RwSet(ref FState, 1, 10, Stream.rwData, 0);
        }

        // Teleport.pas:97


        // Teleport.pas:157
        public override void MapFieldMsgProc(ref Aow.Units.HSEngine.TMapFieldMsg Msg)
        {
            base.MapFieldMsgProc(ref Msg);
            if (((((Msg.HN == 0) && (Msg.MsgID == 4416)) && (Msg.Param[1].As<Aow.Units.HSEngine.TMapField>().Xhx == FDestinationX)) && (Msg.Param[1].As<Aow.Units.HSEngine.TMapField>().Yhx == FDestinationY)) && (Msg.Param[1].As<Aow.Units.HSEngine.TMapField>().Lhx == FDestinationL))
            {
                bool bool__1 = !Aow.Units.AoWE.AoWE.AoWHSMap.FFogEnabled;
                if (!bool__1)
                {
                    var call__2 = GetLhx();
                    var call__3 = GetYhx();
                    var call__4 = GetXhx();
                    var call__5 = Aow.Units.AoWE.AoWE.AoWHSMap.VisibleForPlayer(call__4, call__3, call__2, unchecked((sbyte)Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer));
                    bool__1 = call__5;
                }

                bool bool__6 = bool__1;
                if (!bool__6)
                {
                    var call__7 = Aow.Units.AoWE.AoWE.AoWHSMap.VisibleForPlayer(FDestinationX, FDestinationY, FDestinationL, unchecked((sbyte)Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer));
                    bool__6 = call__7;
                }

                if (bool__6)
                {
                    Aow.Units.Sound.Sound.PlaySample((AnsiString)"SFX\\PLACES\\TELEPORT.WAV");
                }

                ShowTeleportAnimations();
            }
        }

        // Teleport.pas:138
        public override void MoveExclusive(in Aow.Units.HSEngine.THSSenderInfo Info)
        {
            if ((Info.HN == 0) && !FState.Contains(Aow.Units.Teleport.TTeleportStateElement.tsRazed))
            {
                var call__8 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.GetField(FDestinationX, FDestinationY, FDestinationL);
                ((Aow.Units.AoWE.TMoveControl)Info.Sender).ExecuteExternalMove((Aow.Units.AoWE.TAoWMapField)Info.MapField, (Aow.Units.AoWE.TAoWMapField)call__8, 0);
            }
        }

        // Teleport.pas:175
        public override void Activate()
        {
            base.Activate();
            ((Aow.Units.HSEngine.TMapField)Owner).UpdateMapField();
        }

        // Teleport.pas:178
        public override void Deactivate()
        {
            base.Deactivate();
            FStatus = FStatus - TSet<Aow.Units.HSEngine.TMapObjectStatusElement>.Of(Aow.Units.HSEngine.TMapObjectStatusElement.moActivated);
            ((Aow.Units.HSEngine.TMapField)Owner).UpdateMapField();
        }

        // Teleport.pas:131
        public override void UpdateMapField(in Aow.Units.HSEngine.THSMapFieldInfo Info)
        {
            if (Info.HN == 0)
            {
                ((Aow.Units.AoWE.TAoWMapField)((System.Object)Info.MapField)).FFieldFlags = ((Aow.Units.AoWE.TAoWMapField)((System.Object)Info.MapField)).FFieldFlags | 32768;
            }
        }

        // Teleport.pas:115
        public override void SetRazed(bool Value)
        {
            if (Value)
            {
                FState = FState + TSet<Aow.Units.Teleport.TTeleportStateElement>.Of(Aow.Units.Teleport.TTeleportStateElement.tsRazed);
            }
            else
            {
                FState = FState - TSet<Aow.Units.Teleport.TTeleportStateElement>.Of(Aow.Units.Teleport.TTeleportStateElement.tsRazed);
            }

            var call__9 = GetTerrainTypeImage(TerrainType);
            this.FImageIndex = call__9;
        }

        // Teleport.pas:279
        public override bool ValidateMap(Aow.Rtl.TStrings A1)
        {
            bool Result = default;
            if ((FDestinationX == 0) || (FDestinationY == 0))
            {
                var call__10 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.WarningRStr);
                var call__11 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.InvalidLocationSpecifiedRStr);
                A1.Add((call__10 + ((AnsiString)": ")) + call__11);
                Result = false;
            }
            else
            {
                Result = true;
            }

            return Result;
        }

        // Teleport.pas:145
        public void ShowTeleportAnimations()
        {
            Aow.Units.Animation.TImageSequenceAnimation Anim = default;
            Anim = new Aow.Units.Animation.TImageSequenceAnimation(null);
            var call__12 = Aow.Units.AoWE.AoWE.AoWHSSet.FImageSequences.ImageSequences[13];
            Anim.SetImageSequence(call__12);
            var call__13 = GetLhx();
            var call__14 = GetYhx();
            var call__15 = GetXhx();
            Aow.Units.AoWE.AoWE.AoWHSMap.PlaceHX(Anim, call__15, call__14, call__13);
            Anim = new Aow.Units.Animation.TImageSequenceAnimation(null);
            var call__16 = Aow.Units.AoWE.AoWE.AoWHSSet.FImageSequences.ImageSequences[12];
            Anim.SetImageSequence(call__16);
            Aow.Units.AoWE.AoWE.AoWHSMap.PlaceHX(Anim, FDestinationX, FDestinationY, FDestinationL);
        }

        // Teleport.pas:185
        public bool CanTeleportArmy(Aow.Units.AoWE.TArmyHS ArmyHS)
        {
            bool Result = default;
            Aow.Units.AoWE.TAoWMapField Field = default;
            Aow.Units.AoWE.TArmyHS OtherArmy = default;
            Result = false;
            if ((((((FDestinationL >= 0) && (FDestinationL < Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.LevelCount)) && (FDestinationX >= 0)) && (FDestinationX < Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.Width)) && (FDestinationY >= 0)) && (FDestinationY < Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.Height))
            {
                var call__17 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.GetField(FDestinationX, FDestinationY, FDestinationL);
                Field = (Aow.Units.AoWE.TAoWMapField)call__17;
                var call__18 = ArmyHS.FArmy.ValidTerrain(unchecked((sbyte)unchecked((sbyte)Field.TerrainType)), unchecked((sbyte)unchecked((sbyte)Field.OverlayType)));
                if (call__18)
                {
                    OtherArmy = Field.GetArmyHS();
                    // { Original rejects a same-player army when CanAddUnits succeeds. }
                    bool bool__19 = (OtherArmy != null) && (unchecked((byte)ArmyHS.FArmy.FPlayer) == unchecked((byte)OtherArmy.FArmy.FPlayer));
                    if (bool__19)
                    {
                        var call__20 = ((byte)OtherArmy.FArmy.CanAddUnits(((Aow.Units.AoWE.TUnitList)ArmyHS.FArmy))) != 0;
                        bool__19 = call__20;
                    }

                    if (bool__19)
                    {
                        return Result;
                    }

                    Result = true;
                }
            }

            return Result;
        }

        // Teleport.pas:208
        public bool CanTeleport(sbyte Player)
        {
            bool Result = default;
            Aow.Units.AoWE.TArmyHS ArmyHS = default;
            var call__21 = Owner.FindOwnedChild(131607);
            ArmyHS = (Aow.Units.AoWE.TArmyHS)call__21;
            // { Original only checks destination X here; X=0 is rejected. }
            Result = (((ArmyHS != null) && (Player == ArmyHS.FArmy.FPlayer)) && !FState.Contains(Aow.Units.Teleport.TTeleportStateElement.tsRazed)) && (FDestinationX > 0);
            return Result;
        }

        // Teleport.pas:218
        public bool Teleport(sbyte Player)
        {
            bool Result = default;
            Aow.Units.AoWE.TArmyHS ArmyHS = default;
            Aow.Units.AoWE.TMovepath Path = default;
            Aow.Units.AoWE.TAoWMapField Field = default;
            Aow.Units.NetworkE.TTokenEvent Event = default;
            Aow.Rtl.AnsiString MessageText = default;
            if (CanTeleport(unchecked((sbyte)Player)))
            {
                var call__22 = Owner.FindOwnedChild(131607);
                ArmyHS = (Aow.Units.AoWE.TArmyHS)call__22;
                if (CanTeleportArmy(ArmyHS))
                {
                    ArmyHS.FArmy.ResetDestinationPos();
                    Path = new Aow.Units.AoWE.TMovepath();
                    Path.AddXYL(unchecked((sbyte)FDestinationX), unchecked((sbyte)FDestinationY), unchecked((sbyte)FDestinationL));
                    var call__23 = GetLhx();
                    var call__24 = GetYhx();
                    var call__25 = GetXhx();
                    Path.AddXYL(unchecked((sbyte)call__25), unchecked((sbyte)call__24), unchecked((sbyte)call__23));
                    var call__26 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.GetField(FDestinationX, FDestinationY, FDestinationL);
                    Field = (Aow.Units.AoWE.TAoWMapField)call__26;
                    var call__27 = Field.GetMoveAction(unchecked((sbyte)ArmyHS.FArmy.FPlayer));
                    bool bool__28 = (call__27 != Aow.Units.AoWE.TMoveAction.maAttack);
                    if (!bool__28)
                    {
                        var call__29 = Field.GetPlayer();
                        bool__28 = (call__29 <= 0);
                    }

                    bool bool__30 = bool__28;
                    if (!bool__30)
                    {
                        var call__31 = Field.GetPlayer();
                        var call__32 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetDiplomaticRelation(unchecked((sbyte)ArmyHS.FArmy.FPlayer), unchecked((sbyte)call__31));
                        bool__30 = TSet<Aow.Units.AoWE.TDiplomaticRelation>.Of(Aow.Units.AoWE.TDiplomaticRelation.drWar, Aow.Units.AoWE.TDiplomaticRelation.drUnknown).Contains(call__32);
                    }

                    if (bool__30)
                    {
                        var call__33 = Field.GetPlayer();
                        bool bool__34 = (call__33 > -1);
                        if (bool__34)
                        {
                            var call__35 = Field.GetPlayer();
                            bool__34 = (call__35 != ArmyHS.FArmy.FPlayer);
                        }

                        if (bool__34)
                        {
                            var call__36 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(ArmyHS.FArmy.FPlayer);
                            var call__37 = call__36.GetBusy();
                            if (!call__37)
                            {
                                // { Original passes action before mode. }
                                Event = Aow.Units.AoWE.AoWE.CreateArmyMoveTE(ArmyHS, 255, Path, Aow.Units.AoWE.TMoveAction.maAttack, TSet<Aow.Units.AoWE.TArmyMoveModeElement>.Of());
                                if (Event != null)
                                {
                                    var call__38 = Aow.Units.AoWE.AoWE.AoWHSMap.FTokenManager.GetTokenControl(ArmyHS.FArmy.FPlayer);
                                    call__38.AddEvent(Event);
                                    Event.Release();
                                    Result = true;
                                }
                                else
                                {
                                    Result = false;
                                }
                            }
                            else
                            {
                                Result = false;
                            }
                        }
                        else
                        {
                            Result = Aow.Units.AoWE.AoWE.MoveArmyEx(ArmyHS, 255, Path, TSet<Aow.Units.AoWE.TArmyMoveModeElement>.Of(), false);
                        }

                        if (Result && (unchecked((byte)Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer) == unchecked((byte)ArmyHS.FArmy.FPlayer)))
                        {
                            Aow.Units.AoWE.AoWE.AoWHSMap.CenterViewTo(FDestinationX, FDestinationY, FDestinationL);
                            Aow.Units.AoWE.AoWE.AoWHSMap.FSelectedArmy.Select(ArmyHS);
                        }
                    }
                    else
                    {
                        var call__39 = Field.GetPlayer();
                        var call__40 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(call__39);
                        var call__41 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.RaceNamesPluralRStr[call__40.FPlayerRace]);
                        var call__42 = Aow.Units.AoWE.AoWE.NoWarDeclaredtoXRStr;
                        var call__43 = Aow.Units.AoWE.AoWE.TranslateRStr(call__42);
                        var call__44 = Aow.Rtl.Rtl.Format(call__43, [call__41]);
                        MessageText = call__44;
                        ((Aow.Units.AoWE.TAbstractAoWHSMap)Aow.Units.AoWE.AoWE.AoWHSMap).ShowMessage(MessageText);
                        Result = false;
                    }

                    // { Original frees the path only on normal completion. }
                    Path.Free();
                }
                else
                {
                    Result = false;
                }
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // Teleport.pas:26


    }

    public partial class TTeleportResource : Aow.Units.AoWE.TStructureResource
    {
        public TTeleportResource(TECustomNode owner) : base(owner)
        {
            FRazeDefenderBudget = 200;
            FRazeable = true;
            FRebuildGold = 25;
            FRebuildDays = 2;
        }

        // Teleport.pas:299
        public new const int ClassID = 131333;

        // Teleport.pas:290


        // Teleport.pas:305
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 131332;
            return Result;
        }

        // Teleport.pas:45


        // Teleport.pas:302
        public override Aow.Rtl.AnsiString ResourceEditName()
        {
            Aow.Rtl.AnsiString Result = default;
            var call__45 = Aow.Units.AoWE.AoWE.TeleportRStr;
            var call__46 = Aow.Units.AoWE.AoWE.TranslateRStr(call__45);
            Result = call__46;
            return Result;
        }

    }

    [StructLayout(LayoutKind.Explicit, Size = 28)]
    public struct TTeleportFieldFlags
    {
        [FieldOffset(0)]
        public Aow.Units.FixedArray_byte_24 Reserved;
        [FieldOffset(24)]
        public int Flags;
    }

    [StructLayout(LayoutKind.Explicit, Size = 166)]
    public struct TTeleportMapFields
    {
        [FieldOffset(0)]
        public Aow.Units.FixedArray_byte_165 Reserved;
        [FieldOffset(165)]
        public sbyte SeatedPlayer;
    }

    [StructLayout(LayoutKind.Explicit, Size = 166)]
    public struct TTeleportPlayerFields
    {
        [FieldOffset(0)]
        public Aow.Units.FixedArray_byte_165 Reserved;
        [FieldOffset(165)]
        public sbyte Race;
    }

    [StructLayout(LayoutKind.Explicit, Size = 24)]
    public struct TTeleportImageFields
    {
        [FieldOffset(0)]
        public Aow.Units.FixedArray_byte_20 Reserved;
        [FieldOffset(20)]
        public int ImageIndex;
    }

    public static partial class Teleport
    {
        // Teleport.pas:308
        public static void RegisterTeleport(Aow.Units.Engine.TEngine AEngine)
        {
            AEngine.RegisterEClasses([Aow.Units.Engine.EClass.Of<Aow.Units.Teleport.TTeleport>(), Aow.Units.Engine.EClass.Of<Aow.Units.Teleport.TTeleportResource>()]);
        }

        // Original AoWE resource initialization table
        public static Aow.Rtl.AnsiString[] RaceNames = [(AnsiString)"Humans", (AnsiString)"Azracs", (AnsiString)"Lizards", (AnsiString)"Frostlings", (AnsiString)"Elves", (AnsiString)"Halflings", (AnsiString)"Dwarves", (AnsiString)"Highmen", (AnsiString)"Dark Elves", (AnsiString)"Orcs", (AnsiString)"Goblins", (AnsiString)"Undead(plural)", (AnsiString)"Independents"];
        public static readonly Aow.Rtl.AnsiString TeleportNoWarRStr = (AnsiString)"Not at war with the %s";
        // { original RT_STRING $FCD6 }
        public static readonly Aow.Rtl.AnsiString TeleportWarningRStr = (AnsiString)"Warning";

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
