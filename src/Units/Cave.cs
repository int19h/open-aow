// Translated from Cave.pas.
using Aow.Rtl;
using System.Runtime.InteropServices;
using Aow.Units.Engine;
using Aow.Units.NetworkE;
using Aow.Units.HSEngine;
using Aow.Units.EventLog;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;

namespace Aow.Units.Cave
{
    public enum TCaveMoveFlagsElement : byte
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

    // { ot=1 }
    // FEntrance
    public partial class TCave : TStructure
    {
        public TCave(TECustomNode owner) : base(owner)
        {
            FEntrance = TCaveEntrance.ceDown;
        }

        // Cave.pas:160
        public new const int ClassID = 131966;

        public TCaveEntrance FEntrance;
        // Cave.pas:134
        public override void Destroy()
        {
            TCaveEntrance OldEntrance = default;
            TCave Partner = default;
            OldEntrance = FEntrance;
            FEntrance = TCaveEntrance.ceIdle;
            if (FStatus.Contains(TMapObjectStatusElement.moPlacedOnMap) && Owner != null)
            {
                if (!GetEditMode())
                {
                    if (OldEntrance == TCaveEntrance.ceUp)
                    {
                        var call__1 = AoWHSMap.MapContainer.GetField(FXhx, FYhx, ((TMapField)Owner).Lhx - 1);
                        var call__2 = call__1.FindOwnedChild(131966);
                        Partner = (TCave)call__2;
                        if (Partner != null && Partner.FEntrance == TCaveEntrance.ceDown)
                        {
                            Partner.Destroy();
                        }
                    }
                    else
                    {
                        var call__3 = AoWHSMap.MapContainer.GetField(FXhx, FYhx, ((TMapField)Owner).Lhx + 1);
                        var call__4 = call__3.FindOwnedChild(131966);
                        Partner = (TCave)call__4;
                        if (Partner != null && Partner.FEntrance == TCaveEntrance.ceUp)
                        {
                            Partner.Destroy();
                        }
                    }
                }
            }

            base.Destroy();
        }

        // Cave.pas:244
        public override void ReadWrite(TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwData(Bytes.Of(ref FEntrance, 1), 1, 6);
        }

        // Cave.pas:128


        // Cave.pas:273
        public override sbyte GetOverlay(int Index)
        {
            sbyte Result = default;
            var call__5 = FResource.TerrainList.Terrain[Index];
            Result = call__5;
            if (Result >= 0)
            {
                Result = -1;
            }

            return Result;
        }

        // Cave.pas:257
        public override void MoveExclusive(in THSSenderInfo Info)
        {
            TMapField Field = default;
            if (Info.HN == 0)
            {
                switch (FEntrance)
                {
                    case TCaveEntrance.ceUp:
                        var call__6 = AoWHSMap.MapContainer.GetField(FXhx, FYhx, ((TMapField)Owner).Lhx - 1);
                        Field = call__6;
                        break;
                    case TCaveEntrance.ceDown:
                        var call__7 = AoWHSMap.MapContainer.GetField(FXhx, FYhx, ((TMapField)Owner).Lhx + 1);
                        Field = call__7;
                        break;
                    default:
                        Field = null;
                        break;
                }

                // { Original dereferences the destination even for an idle entrance. }
                ((TMoveControl)Info.Sender).ExecuteExternalMove((TAoWMapField)Info.MapField, (TAoWMapField)Field, MemoryMarshal.Cast<FixedArray_sbyte_16, sbyte>(((TMoveControl)(object)Info.Sender).FMovePointTable)[1 + 16 * Field.TerrainType + Field.OverlayType]);
            }
        }

        // Cave.pas:279
        public override void Activate()
        {
            base.Activate();
            ((TAoWMapField)Owner).UpdateMapField();
        }

        // Cave.pas:285
        public override void Deactivate()
        {
            base.Deactivate();
            FStatus = FStatus - TSet<TMapObjectStatusElement>.Of(TMapObjectStatusElement.moActivated);
            ((TAoWMapField)Owner).UpdateMapField();
        }

        // Cave.pas:250
        public override void UpdateMapField(in THSMapFieldInfo Info)
        {
            if (Info.HN == 0)
            {
                ((TAoWMapField)(object)Info.MapField).FFieldFlags = ((TAoWMapField)(object)Info.MapField).FFieldFlags | 32768;
            }
        }

        // Cave.pas:204
        public override bool CanPlace(TMapContainer MapContainer, sbyte Xhx, sbyte Yhx, sbyte Lhx)
        {
            bool Result = default;
            // { Original permits an upward cave on level zero; there is no lower-bound test. }
            var call__8 = base.CanPlace(MapContainer, Xhx, Yhx, Lhx);
            Result = call__8 && (Lhx + 1 < MapContainer.LevelCount || FEntrance == TCaveEntrance.ceUp);
            return Result;
        }

        // Cave.pas:211
        public override bool PlaceHX(TMapContainer MapContainer, sbyte Xhx, sbyte Yhx, sbyte Lhx)
        {
            bool Result = default;
            TCave Up = default;
            TCave Down = default;
            Result = base.PlaceHX(MapContainer, Xhx, Yhx, Lhx);
            if (Result)
            {
                if (!GetEditMode())
                {
                    if (FEntrance == TCaveEntrance.ceDown)
                    {
                        var call__9 = MapContainer.GetField(Xhx, Yhx, Lhx + 1);
                        var call__10 = call__9.FindOwnedChild(131966);
                        Up = (TCave)call__10;
                        if (Up == null)
                        {
                            var call__11 = FResource.CreateResourceUser();
                            Up = (TCave)call__11;
                            Up.FEntrance = TCaveEntrance.ceUp;
                            Result = Up.PlaceHX(MapContainer, Xhx, Yhx, (sbyte)(Lhx + 1));
                        }
                        else
                        {
                            Result = Up.FEntrance == TCaveEntrance.ceUp;
                        }

                        // { Original destroys an existing partner when its entrance direction is wrong. }
                        if (!Result)
                        {
                            Up.Destroy();
                        }
                        else
                        {
                            OverrideTerrain();
                        }
                    }
                    else
                    {
                        var call__12 = MapContainer.GetField(Xhx, Yhx, Lhx - 1);
                        var call__13 = call__12.FindOwnedChild(131966);
                        Down = (TCave)call__13;
                        if (Down == null)
                        {
                            var call__14 = FResource.CreateResourceUser();
                            Down = (TCave)call__14;
                            Down.FEntrance = TCaveEntrance.ceDown;
                            Result = Down.PlaceHX(MapContainer, Xhx, Yhx, (sbyte)(Lhx - 1));
                        }
                        else
                        {
                            Result = Down.FEntrance == TCaveEntrance.ceDown;
                        }

                        if (!Result)
                        {
                            Down.Destroy();
                        }
                        else
                        {
                            OverrideTerrain();
                        }
                    }
                }
            }

            return Result;
        }

        // Cave.pas:199
        public override int GetTerrainTypeImage(int TerrainType)
        {
            int Result = default;
            if (FEntrance == TCaveEntrance.ceDown)
            {
                Result = TerrainType;
            }
            else
            {
                Result = TerrainType + 30;
            }

            return Result;
        }

        // Cave.pas:163
        public override ImageLib.TLibraryImage GetTopBorderImage()
        {
            ImageLib.TLibraryImage Result = default;
            if (FEntrance == TCaveEntrance.ceUp)
            {
                bool bool__15 = FResource.ImageSequenceList != null;
                if (bool__15)
                {
                    var call__16 = FResource.ImageSequenceList.ImageSequences[33];
                    bool__15 = call__16 != null;
                }

                if (bool__15)
                {
                    var call__17 = FResource.ImageSequenceList.ImageSequences[33];
                    var call__18 = call__17.GetPreviewImage();
                    Result = call__18;
                }
                else
                {
                    Result = null;
                }
            }
            else
            {
                Result = base.GetTopBorderImage();
            }

            return Result;
        }

        // Cave.pas:175
        public override ImageLib.TLibraryImage GetMiddleBorderImage()
        {
            ImageLib.TLibraryImage Result = default;
            if (FEntrance == TCaveEntrance.ceUp)
            {
                bool bool__19 = FResource.ImageSequenceList != null;
                if (bool__19)
                {
                    var call__20 = FResource.ImageSequenceList.ImageSequences[34];
                    bool__19 = call__20 != null;
                }

                if (bool__19)
                {
                    var call__21 = FResource.ImageSequenceList.ImageSequences[34];
                    var call__22 = call__21.GetPreviewImage();
                    Result = call__22;
                }
                else
                {
                    Result = null;
                }
            }
            else
            {
                Result = base.GetMiddleBorderImage();
            }

            return Result;
        }

        // Cave.pas:187
        public override ImageLib.TLibraryImage GetBottomBorderImage()
        {
            ImageLib.TLibraryImage Result = default;
            if (FEntrance == TCaveEntrance.ceUp)
            {
                bool bool__23 = FResource.ImageSequenceList != null;
                if (bool__23)
                {
                    var call__24 = FResource.ImageSequenceList.ImageSequences[35];
                    bool__23 = call__24 != null;
                }

                if (bool__23)
                {
                    var call__25 = FResource.ImageSequenceList.ImageSequences[35];
                    var call__26 = call__25.GetPreviewImage();
                    Result = call__26;
                }
                else
                {
                    Result = null;
                }
            }
            else
            {
                Result = base.GetBottomBorderImage();
            }

            return Result;
        }

        // Cave.pas:412
        public override void ArmyPlaced(TArmyHS A1)
        {
            int L = default;
            TArmyHS ArmyHS = default;
            base.ArmyPlaced(A1);
            ArmyHS = A1;
            if (ArmyHS != null)
            {
                if (FEntrance == TCaveEntrance.ceDown)
                {
                    L = GetLhx() + 1;
                }
                else
                {
                    L = GetLhx() - 1;
                }

                var call__27 = ArmyHS.FArmy.VisibilityRange();
                var call__28 = ArmyHS.GetYhx();
                var call__29 = ArmyHS.GetXhx();
                var call__30 = AoWHSMap.MapContainer.GetMapLevel(L);
                ((TAoWMapLevel)call__30).ExploreArea(call__29, call__28, ArmyHS.FArmy.FPlayer, call__27);
            }
        }

        // Cave.pas:292
        public sbyte EnterMovePoints(TArmy Army, byte Selection)
        {
            sbyte Result = default;
            TMapField Field = default;
            int L = default;
            if (FEntrance == TCaveEntrance.ceDown)
            {
                L = GetLhx() + 1;
            }
            else
            {
                L = GetLhx() - 1;
            }

            var call__31 = GetYhx();
            var call__32 = GetXhx();
            var call__33 = AoWHSMap.MapContainer.GetField(call__32, call__31, L);
            Field = call__33;
            var call__34 = (sbyte)Army.MoveOnMovePoints(Selection, Field.TerrainType, Field.OverlayType);
            Result = call__34;
            return Result;
        }

        // Cave.pas:300
        public byte CanEnterSelection(TUnitList Units)
        {
            byte Result = default;
            int L = default;
            int I = default;
            TMapField Field = default;
            TAbstractUnit AUnit = default;
            if (FEntrance == TCaveEntrance.ceDown)
            {
                L = GetLhx() + 1;
            }
            else
            {
                L = GetLhx() - 1;
            }

            var call__35 = GetYhx();
            var call__36 = GetXhx();
            var call__37 = AoWHSMap.MapContainer.GetField(call__36, call__35, L);
            Field = call__37;
            Result = 0;
            {
                I = 0;
                int end__38 = Units.Count - 1;
                for (; I <= end__38; I += 1)
                {
                    AUnit = Units.GetUnit(I);
                    var call__39 = AUnit.MovePointCost(Field.TerrainType, Field.OverlayType);
                    var call__40 = AUnit.GetMovePoints();
                    if (call__39 <= call__40)
                    {
                        Result = (byte)(Result + (1 << I));
                    }
                }
            }

            return Result;
        }

        // Cave.pas:317
        public bool CanEnter(sbyte Player)
        {
            bool Result = default;
            TArmyHS ArmyHS = default;
            var call__41 = Owner.FindOwnedChild(131607);
            ArmyHS = (TArmyHS)call__41;
            bool bool__42 = ArmyHS != null && Player == ArmyHS.FArmy.FPlayer;
            if (bool__42)
            {
                var call__43 = CanEnterSelection(ArmyHS.FArmy);
                bool__42 = call__43 != 0;
            }

            Result = bool__42;
            return Result;
        }

        // Cave.pas:325
        public bool EnterEx()
        {
            bool Result = default;
            byte Selection = default;
            TMovepath Path = default;
            sbyte L = default;
            AnsiString MessageText = default;
            TArmyHS ArmyHS = default;
            TAoWMapField Field = default;
            TTokenEvent Event = default;
            var call__44 = Owner.FindOwnedChild(131607);
            ArmyHS = (TArmyHS)call__44;
            bool bool__45 = ArmyHS != null;
            if (bool__45)
            {
                var call__46 = CanEnter(ArmyHS.FArmy.FPlayer);
                bool__45 = call__46;
            }

            if (bool__45)
            {
                var call__47 = CanEnterSelection(ArmyHS.FArmy);
                Selection = call__47;
                L = 0;
                Path = new TMovepath();
                switch (FEntrance)
                {
                    case TCaveEntrance.ceDown:
                        L = (sbyte)(GetLhx() + 1);
                        break;
                    case TCaveEntrance.ceUp:
                        L = (sbyte)(GetLhx() - 1);
                        break;
                }

                var call__48 = GetYhx();
                var call__49 = GetXhx();
                Path.AddXYL(call__49, call__48, L);
                var call__50 = EnterMovePoints(ArmyHS.FArmy, Selection);
                Path.List[0].Flag = call__50;
                var call__51 = GetLhx();
                var call__52 = GetYhx();
                var call__53 = GetXhx();
                Path.AddXYL(call__53, call__52, call__51);
                var call__54 = GetYhx();
                var call__55 = GetXhx();
                var call__56 = AoWHSMap.MapContainer.GetField(call__55, call__54, L);
                Field = (TAoWMapField)call__56;
                var call__57 = Field.GetMoveAction(ArmyHS.FArmy.FPlayer);
                bool bool__58 = call__57 != TMoveAction.maAttack;
                if (!bool__58)
                {
                    var call__59 = Field.GetPlayer();
                    bool__58 = call__59 <= 0;
                }

                bool bool__60 = bool__58;
                if (!bool__60)
                {
                    var call__61 = Field.GetPlayer();
                    var call__62 = AoWHSMap.FPlayers.GetDiplomaticRelation(ArmyHS.FArmy.FPlayer, call__61);
                    bool__60 = TSet<byte>.WithBounds(0, 255).With(1).With(3).Contains((byte)(int)call__62);
                }

                if (bool__60)
                {
                    var call__63 = Field.GetPlayer();
                    bool bool__64 = call__63 > -1;
                    if (bool__64)
                    {
                        var call__65 = Field.GetPlayer();
                        bool__64 = call__65 != ArmyHS.FArmy.FPlayer;
                    }

                    if (bool__64)
                    {
                        var call__66 = AoWHSMap.FPlayers.GetPlayers(ArmyHS.FArmy.FPlayer);
                        var call__67 = call__66.GetBusy();
                        if (!call__67)
                        {
                            // { Original passes action before mode. }
                            Event = CreateArmyMoveTE(ArmyHS, Selection, Path, TMoveAction.maAttack, TSet<TArmyMoveModeElement>.Of());
                            if (Event != null)
                            {
                                var call__68 = AoWHSMap.FTokenManager.GetTokenControl(ArmyHS.FArmy.FPlayer);
                                call__68.AddEvent(Event);
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
                        Result = MoveArmyEx(ArmyHS, Selection, Path, TSet<TArmyMoveModeElement>.Of(), false);
                    }

                    if (Result && (byte)AoWHSMap.FSeatedPlayer == (byte)ArmyHS.FArmy.FPlayer)
                    {
                        AoWHSMap.ViewLevel(L);
                        AoWHSMap.FSelectedArmy.Select(ArmyHS);
                    }
                }
                else
                {
                    // { Original only warns the seated player. }
                    if (ArmyHS.FArmy.FPlayer == AoWHSMap.FSeatedPlayer)
                    {
                        var call__69 = Field.GetPlayer();
                        var call__70 = AoWHSMap.FPlayers.GetPlayers(call__69);
                        var call__71 = TranslateRStr(RaceNamesPluralRStr[call__70.FPlayerRace]);
                        var call__72 = NoWarDeclaredtoXRStr;
                        var call__73 = TranslateRStr(call__72);
                        var call__74 = Rtl.Rtl.Format(call__73, [call__71]);
                        MessageText = call__74;
                        AoWHSMap.ShowMessage(MessageText);
                    }

                    Result = false;
                }

                // { Original frees the path only on normal completion. }
                Path.Free();
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // Cave.pas:388
        public bool Enter(sbyte Player)
        {
            bool Result = default;
            TArmyHS ArmyHS = default;
            TEnterCaveEventLog Log = default;
            if (CanEnter(Player))
            {
                var call__75 = Owner.FindOwnedChild(131607);
                ArmyHS = (TArmyHS)call__75;
                var call__76 = CanEnterSelection(ArmyHS.FArmy);
                var call__77 = ArmyHS.FArmy.ValidateSelection(255);
                if (call__76 != call__77)
                {
                    Log = new TEnterCaveEventLog(null);
                    Log.FCave = this;
                    var call__78 = TranslateRStr(SplitOnEnterCaveWarningRStr);
                    Log.FMessage.Text = call__78;
                    Log.FButtons = TMessageEventLogButtons.mebYesNo;
                    Log.Execute(true, TExecuteTrigger.etUser, null);
                    Log.Release();
                    Result = true;
                }
                else
                {
                    Result = EnterEx();
                }
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // Cave.pas:25


    }

    public partial class TCaveResource : TStructureResource
    {
        public TCaveResource(TECustomNode owner) : base(owner)
        {
            FTerrainBound = false;
            FConnectRoads = true;
        }

        // Cave.pas:436
        public new const int ClassID = 131967;

        // Cave.pas:426


        // Cave.pas:439
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 131966;
            return Result;
        }

        // Cave.pas:49


        // Cave.pas:433
        public override AnsiString ResourceEditName()
        {
            AnsiString Result = default;
            Result = TranslateRStr(CaveRStr);
            return Result;
        }

    }

    // FCave
    public partial class TEnterCaveEventLog : TMessageEventLog
    {
        public TEnterCaveEventLog(TECustomNode owner) : base(owner)
        { }

        public TCave FCave;
        // Cave.pas:442
        public override bool Yes()
        {
            bool Result = default;
            FCave.EnterEx();
            Result = true;
            return Result;
        }

        // Cave.pas:448
        public override bool No() => true;

    }

    public enum TCaveEntrance : byte
    {
        ceUp = 0,
        ceDown = 1,
        ceIdle = 2
    }

    [StructLayout(LayoutKind.Explicit, Size = 28)]
    public struct TCaveFieldFlags
    {
        [FieldOffset(0)]
        public FixedArray_byte_24 Reserved;
        [FieldOffset(24)]
        public int Flags;
    }

    [StructLayout(LayoutKind.Explicit, Size = 166)]
    public struct TCaveMapFields
    {
        [FieldOffset(0)]
        public FixedArray_byte_165 Reserved;
        [FieldOffset(165)]
        public sbyte SeatedPlayer;
    }

    [StructLayout(LayoutKind.Explicit, Size = 166)]
    public struct TCavePlayerFields
    {
        [FieldOffset(0)]
        public FixedArray_byte_165 Reserved;
        [FieldOffset(165)]
        public sbyte Race;
    }

    [StructLayout(LayoutKind.Explicit, Size = 305)]
    public struct TCaveMoveData
    {
        [FieldOffset(0)]
        public FixedArray_byte_49 Reserved;
        [FieldOffset(49)]
        public FixedArray_Aow_Units_FixedArray_sbyte_16_16 Costs;
    }

    public static partial class Cave
    {
        // Cave.pas:451
        public static void RegisterCave(TEngine AEngine)
        {
            AEngine.RegisterEClasses([EClass.Of<TCave>(), EClass.Of<TCaveResource>()]);
        }

        // Original AoWE resource initialization table
        public static AnsiString[] RaceNames = [(AnsiString)"Humans", (AnsiString)"Azracs", (AnsiString)"Lizards", (AnsiString)"Frostlings", (AnsiString)"Elves", (AnsiString)"Halflings", (AnsiString)"Dwarves", (AnsiString)"Highmen", (AnsiString)"Dark Elves", (AnsiString)"Orcs", (AnsiString)"Goblins", (AnsiString)"Undead(plural)", (AnsiString)"Independents"];
        public static readonly AnsiString CaveNoWarRStr = (AnsiString)"Not at war with the %s";

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
