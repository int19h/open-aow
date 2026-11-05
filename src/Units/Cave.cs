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
            TCaveEntrance OldEntrance;
            TCave Partner;
            OldEntrance = FEntrance;
            FEntrance = TCaveEntrance.ceIdle;
            if (FStatus.Contains(TMapObjectStatusElement.moPlacedOnMap) && Owner != null)
            {
                if (!GetEditMode())
                {
                    if (OldEntrance == TCaveEntrance.ceUp)
                    {
                        Partner = (TCave)AoWHSMap.MapContainer.GetField(FXhx, FYhx, ((TMapField)Owner).Lhx - 1).FindOwnedChild(131966);
                        if (Partner != null && Partner.FEntrance == TCaveEntrance.ceDown)
                        {
                            Partner.Destroy();
                        }
                    }
                    else
                    {
                        Partner = (TCave)AoWHSMap.MapContainer.GetField(FXhx, FYhx, ((TMapField)Owner).Lhx + 1).FindOwnedChild(131966);
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
            sbyte Result = FResource.TerrainList.Terrain[Index];
            if (Result >= 0)
            {
                Result = -1;
            }

            return Result;
        }

        // Cave.pas:257
        public override void MoveExclusive(in THSSenderInfo Info)
        {
            TMapField Field;
            if (Info.HN == 0)
            {
                switch (FEntrance)
                {
                    case TCaveEntrance.ceUp:
                        Field = AoWHSMap.MapContainer.GetField(FXhx, FYhx, ((TMapField)Owner).Lhx - 1);
                        break;
                    case TCaveEntrance.ceDown:
                        Field = AoWHSMap.MapContainer.GetField(FXhx, FYhx, ((TMapField)Owner).Lhx + 1);
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
            // { Original permits an upward cave on level zero; there is no lower-bound test. }
            return base.CanPlace(MapContainer, Xhx, Yhx, Lhx) && (Lhx + 1 < MapContainer.LevelCount || FEntrance == TCaveEntrance.ceUp);
        }

        // Cave.pas:211
        public override bool PlaceHX(TMapContainer MapContainer, sbyte Xhx, sbyte Yhx, sbyte Lhx)
        {
            TCave Up;
            TCave Down;
            bool Result = base.PlaceHX(MapContainer, Xhx, Yhx, Lhx);
            if (Result)
            {
                if (!GetEditMode())
                {
                    if (FEntrance == TCaveEntrance.ceDown)
                    {
                        Up = (TCave)MapContainer.GetField(Xhx, Yhx, Lhx + 1).FindOwnedChild(131966);
                        if (Up == null)
                        {
                            Up = (TCave)FResource.CreateResourceUser();
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
                        Down = (TCave)MapContainer.GetField(Xhx, Yhx, Lhx - 1).FindOwnedChild(131966);
                        if (Down == null)
                        {
                            Down = (TCave)FResource.CreateResourceUser();
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
            int Result;
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
            ImageLib.TLibraryImage Result;
            if (FEntrance == TCaveEntrance.ceUp)
            {
                if (FResource.ImageSequenceList != null && FResource.ImageSequenceList.ImageSequences[33] != null)
                {
                    Result = FResource.ImageSequenceList.ImageSequences[33].GetPreviewImage();
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
            ImageLib.TLibraryImage Result;
            if (FEntrance == TCaveEntrance.ceUp)
            {
                if (FResource.ImageSequenceList != null && FResource.ImageSequenceList.ImageSequences[34] != null)
                {
                    Result = FResource.ImageSequenceList.ImageSequences[34].GetPreviewImage();
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
            ImageLib.TLibraryImage Result;
            if (FEntrance == TCaveEntrance.ceUp)
            {
                if (FResource.ImageSequenceList != null && FResource.ImageSequenceList.ImageSequences[35] != null)
                {
                    Result = FResource.ImageSequenceList.ImageSequences[35].GetPreviewImage();
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
            int L;
            TArmyHS ArmyHS;
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

                var visibilityRange = ArmyHS.FArmy.VisibilityRange();
                var yhx = ArmyHS.GetYhx();
                var xhx = ArmyHS.GetXhx();
                ((TAoWMapLevel)AoWHSMap.MapContainer.GetMapLevel(L)).ExploreArea(xhx, yhx, ArmyHS.FArmy.FPlayer, visibilityRange);
            }
        }

        // Cave.pas:292
        public sbyte EnterMovePoints(TArmy Army, byte Selection)
        {
            TMapField Field;
            int L;
            if (FEntrance == TCaveEntrance.ceDown)
            {
                L = GetLhx() + 1;
            }
            else
            {
                L = GetLhx() - 1;
            }

            var yhx = GetYhx();
            var xhx = GetXhx();
            Field = AoWHSMap.MapContainer.GetField(xhx, yhx, L);
            return (sbyte)Army.MoveOnMovePoints(Selection, Field.TerrainType, Field.OverlayType);
        }

        // Cave.pas:300
        public byte CanEnterSelection(TUnitList Units)
        {
            int L;
            TMapField Field;
            TAbstractUnit AUnit;
            if (FEntrance == TCaveEntrance.ceDown)
            {
                L = GetLhx() + 1;
            }
            else
            {
                L = GetLhx() - 1;
            }

            var yhx = GetYhx();
            var xhx = GetXhx();
            Field = AoWHSMap.MapContainer.GetField(xhx, yhx, L);
            byte Result = 0;
            for (int I = 0, last = Units.Count - 1; I <= last; I++)
            {
                AUnit = Units.GetUnit(I);
                if (AUnit.MovePointCost(Field.TerrainType, Field.OverlayType) <= AUnit.GetMovePoints())
                {
                    Result = (byte)(Result + (1 << I));
                }
            }

            return Result;
        }

        // Cave.pas:317
        public bool CanEnter(sbyte Player)
        {
            TArmyHS ArmyHS;
            ArmyHS = (TArmyHS)Owner.FindOwnedChild(131607);
            return ArmyHS != null && Player == ArmyHS.FArmy.FPlayer && CanEnterSelection(ArmyHS.FArmy) != 0;
        }

        // Cave.pas:325
        public bool EnterEx()
        {
            bool Result;
            byte Selection;
            TMovepath Path;
            sbyte L;
            AnsiString MessageText;
            TArmyHS ArmyHS;
            TAoWMapField Field;
            TTokenEvent Event;
            ArmyHS = (TArmyHS)Owner.FindOwnedChild(131607);
            if (ArmyHS != null && CanEnter(ArmyHS.FArmy.FPlayer))
            {
                Selection = CanEnterSelection(ArmyHS.FArmy);
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

                var yhx = GetYhx();
                Path.AddXYL(GetXhx(), yhx, L);
                var enterMovePoints = EnterMovePoints(ArmyHS.FArmy, Selection);
                Path.List[0].Flag = enterMovePoints;
                var lhx = GetLhx();
                var yhx2 = GetYhx();
                Path.AddXYL(GetXhx(), yhx2, lhx);
                var yhx3 = GetYhx();
                var xhx = GetXhx();
                Field = (TAoWMapField)AoWHSMap.MapContainer.GetField(xhx, yhx3, L);
                bool condition = Field.GetMoveAction(ArmyHS.FArmy.FPlayer) != TMoveAction.maAttack || Field.GetPlayer() <= 0;
                if (!condition)
                {
                    var player = Field.GetPlayer();
                    var diplomaticRelation = AoWHSMap.FPlayers.GetDiplomaticRelation(ArmyHS.FArmy.FPlayer, player);
                    condition = TSet<byte>.WithBounds(0, 255).With(1).With(3).Contains((byte)(int)diplomaticRelation);
                }

                if (condition)
                {
                    if (Field.GetPlayer() > -1 && Field.GetPlayer() != ArmyHS.FArmy.FPlayer)
                    {
                        if (!AoWHSMap.FPlayers.GetPlayers(ArmyHS.FArmy.FPlayer).GetBusy())
                        {
                            // { Original passes action before mode. }
                            Event = CreateArmyMoveTE(ArmyHS, Selection, Path, TMoveAction.maAttack, TSet<TArmyMoveModeElement>.Of());
                            if (Event != null)
                            {
                                AoWHSMap.FTokenManager.GetTokenControl(ArmyHS.FArmy.FPlayer).AddEvent(Event);
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
                        var player = Field.GetPlayer();
                        var playerPlayerRace = AoWHSMap.FPlayers.GetPlayers(player);
                        var raceNamesPlural = TranslateRStr(RaceNamesPluralRStr[playerPlayerRace.FPlayerRace]);
                        MessageText = Rtl.Rtl.Format(TranslateRStr(NoWarDeclaredtoXRStr), [raceNamesPlural]);
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
            TArmyHS ArmyHS;
            TEnterCaveEventLog Log;
            if (CanEnter(Player))
            {
                ArmyHS = (TArmyHS)Owner.FindOwnedChild(131607);
                if (CanEnterSelection(ArmyHS.FArmy) != ArmyHS.FArmy.ValidateSelection(255))
                {
                    Log = new TEnterCaveEventLog(null);
                    Log.FCave = this;
                    var splitOnEnterCaveWarning = TranslateRStr(SplitOnEnterCaveWarningRStr);
                    Log.FMessage.Text = splitOnEnterCaveWarning;
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
        public override int ResourceUserClassID() => 131966;

        // Cave.pas:49


        // Cave.pas:433
        public override AnsiString ResourceEditName() => TranslateRStr(CaveRStr);

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
            FCave.EnterEx();
            return true;
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
