// Translated from Cave.pas.
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
    // { TCave (instance size 52, VMT 557B3018): +$30 FEntrance. }
    public partial class TCave : Aow.Units.AoWE.TStructure
    {
        public Aow.Units.Cave.TCaveEntrance FEntrance;
        public Aow.Units.FixedArray_byte_3 F_Cave_31;
        // Cave.pas:134
        public override void Destroy()
        {
            Aow.Units.Cave.TCaveEntrance OldEntrance = default;
            Aow.Units.Cave.TCave Partner = default;
            OldEntrance = FEntrance;
            FEntrance = Aow.Units.Cave.TCaveEntrance.ceIdle;
            if (FStatus.Contains(Aow.Units.HSEngine.TMapObjectStatusElement.moPlacedOnMap) && (Owner != null))
            {
                if (!GetEditMode())
                {
                    if (OldEntrance == Aow.Units.Cave.TCaveEntrance.ceUp)
                    {
                        var call__1 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.GetField(FXhx, FYhx, ((Aow.Units.HSEngine.TMapField)Owner).Lhx - 1);
                        var call__2 = call__1.FindOwnedChild(131966);
                        Partner = (Aow.Units.Cave.TCave)call__2;
                        if ((Partner != null) && (Partner.FEntrance == Aow.Units.Cave.TCaveEntrance.ceDown))
                        {
                            Partner.Destroy();
                        }
                    }
                    else
                    {
                        var call__3 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.GetField(FXhx, FYhx, ((Aow.Units.HSEngine.TMapField)Owner).Lhx + 1);
                        var call__4 = call__3.FindOwnedChild(131966);
                        Partner = (Aow.Units.Cave.TCave)call__4;
                        if ((Partner != null) && (Partner.FEntrance == Aow.Units.Cave.TCaveEntrance.ceUp))
                        {
                            Partner.Destroy();
                        }
                    }
                }
            }

            base.Destroy();
        }

        // Cave.pas:244
        public override void ReadWrite(Aow.Units.Engine.TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwData(Aow.Rtl.Bytes.Of(ref FEntrance, 1), 1, 6);
        }

        // Cave.pas:128
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            FEntrance = Aow.Units.Cave.TCaveEntrance.ceDown;
        }

        // Cave.pas:273
        public override sbyte GetOverlay(int Index)
        {
            sbyte Result = default;
            var call__5 = FResource.TerrainList.Terrain[Index];
            Result = unchecked((sbyte)call__5);
            if (Result >= 0)
            {
                Result = unchecked((sbyte)-1);
            }

            return Result;
        }

        // Cave.pas:257
        public override void MoveExclusive(in Aow.Units.HSEngine.THSSenderInfo Info)
        {
            Aow.Units.HSEngine.TMapField Field = default;
            if (Info.HN == 0)
            {
                switch (FEntrance)
                {
                    case Aow.Units.Cave.TCaveEntrance.ceUp:
                        var call__6 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.GetField(FXhx, FYhx, ((Aow.Units.HSEngine.TMapField)Owner).Lhx - 1);
                        Field = call__6;
                        break;
                    case Aow.Units.Cave.TCaveEntrance.ceDown:
                        var call__7 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.GetField(FXhx, FYhx, ((Aow.Units.HSEngine.TMapField)Owner).Lhx + 1);
                        Field = call__7;
                        break;
                    default:
                        Field = null;
                        break;
                }

                // { Original dereferences the destination even for an idle entrance. }
                ((Aow.Units.AoWE.TMoveControl)Info.Sender).ExecuteExternalMove((Aow.Units.AoWE.TAoWMapField)Info.MapField, (Aow.Units.AoWE.TAoWMapField)Field, MemoryMarshal.Cast<Aow.Units.FixedArray_sbyte_16, sbyte>((Span<Aow.Units.FixedArray_sbyte_16>)((Aow.Units.AoWE.TMoveControl)((System.Object)Info.Sender)).FMovePointTable)[(1 + (16 * Field.TerrainType)) + Field.OverlayType]);
            }
        }

        // Cave.pas:279
        public override void Activate()
        {
            base.Activate();
            ((Aow.Units.AoWE.TAoWMapField)Owner).UpdateMapField();
        }

        // Cave.pas:285
        public override void Deactivate()
        {
            base.Deactivate();
            FStatus = FStatus - TSet<Aow.Units.HSEngine.TMapObjectStatusElement>.Of(Aow.Units.HSEngine.TMapObjectStatusElement.moActivated);
            ((Aow.Units.AoWE.TAoWMapField)Owner).UpdateMapField();
        }

        // Cave.pas:250
        public override void UpdateMapField(in Aow.Units.HSEngine.THSMapFieldInfo Info)
        {
            if (Info.HN == 0)
            {
                ((Aow.Units.AoWE.TAoWMapField)((System.Object)Info.MapField)).FFieldFlags = ((Aow.Units.AoWE.TAoWMapField)((System.Object)Info.MapField)).FFieldFlags | 32768;
            }
        }

        // Cave.pas:204
        public override bool CanPlace(Aow.Units.HSEngine.TMapContainer MapContainer, sbyte Xhx, sbyte Yhx, sbyte Lhx)
        {
            bool Result = default;
            // { Original permits an upward cave on level zero; there is no lower-bound test. }
            var call__8 = base.CanPlace(MapContainer, unchecked((sbyte)Xhx), unchecked((sbyte)Yhx), unchecked((sbyte)Lhx));
            Result = call__8 && (((Lhx + 1) < MapContainer.LevelCount) || (FEntrance == Aow.Units.Cave.TCaveEntrance.ceUp));
            return Result;
        }

        // Cave.pas:211
        public override bool PlaceHX(Aow.Units.HSEngine.TMapContainer MapContainer, sbyte Xhx, sbyte Yhx, sbyte Lhx)
        {
            bool Result = default;
            Aow.Units.Cave.TCave Up = default;
            Aow.Units.Cave.TCave Down = default;
            Result = base.PlaceHX(MapContainer, unchecked((sbyte)Xhx), unchecked((sbyte)Yhx), unchecked((sbyte)Lhx));
            if (Result)
            {
                if (!GetEditMode())
                {
                    if (FEntrance == Aow.Units.Cave.TCaveEntrance.ceDown)
                    {
                        var call__9 = MapContainer.GetField(Xhx, Yhx, Lhx + 1);
                        var call__10 = call__9.FindOwnedChild(131966);
                        Up = (Aow.Units.Cave.TCave)call__10;
                        if (Up == null)
                        {
                            var call__11 = FResource.CreateResourceUser();
                            Up = (Aow.Units.Cave.TCave)call__11;
                            Up.FEntrance = Aow.Units.Cave.TCaveEntrance.ceUp;
                            Result = Up.PlaceHX(MapContainer, unchecked((sbyte)Xhx), unchecked((sbyte)Yhx), unchecked((sbyte)(Lhx + 1)));
                        }
                        else
                        {
                            Result = Up.FEntrance == Aow.Units.Cave.TCaveEntrance.ceUp;
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
                        Down = (Aow.Units.Cave.TCave)call__13;
                        if (Down == null)
                        {
                            var call__14 = FResource.CreateResourceUser();
                            Down = (Aow.Units.Cave.TCave)call__14;
                            Down.FEntrance = Aow.Units.Cave.TCaveEntrance.ceDown;
                            Result = Down.PlaceHX(MapContainer, unchecked((sbyte)Xhx), unchecked((sbyte)Yhx), unchecked((sbyte)(Lhx - 1)));
                        }
                        else
                        {
                            Result = Down.FEntrance == Aow.Units.Cave.TCaveEntrance.ceDown;
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
            if (FEntrance == Aow.Units.Cave.TCaveEntrance.ceDown)
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
        public override Aow.Units.ImageLib.TLibraryImage GetTopBorderImage()
        {
            Aow.Units.ImageLib.TLibraryImage Result = default;
            if (FEntrance == Aow.Units.Cave.TCaveEntrance.ceUp)
            {
                bool bool__15 = (FResource.ImageSequenceList != null);
                if (bool__15)
                {
                    var call__16 = FResource.ImageSequenceList.ImageSequences[33];
                    bool__15 = (call__16 != null);
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
        public override Aow.Units.ImageLib.TLibraryImage GetMiddleBorderImage()
        {
            Aow.Units.ImageLib.TLibraryImage Result = default;
            if (FEntrance == Aow.Units.Cave.TCaveEntrance.ceUp)
            {
                bool bool__19 = (FResource.ImageSequenceList != null);
                if (bool__19)
                {
                    var call__20 = FResource.ImageSequenceList.ImageSequences[34];
                    bool__19 = (call__20 != null);
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
        public override Aow.Units.ImageLib.TLibraryImage GetBottomBorderImage()
        {
            Aow.Units.ImageLib.TLibraryImage Result = default;
            if (FEntrance == Aow.Units.Cave.TCaveEntrance.ceUp)
            {
                bool bool__23 = (FResource.ImageSequenceList != null);
                if (bool__23)
                {
                    var call__24 = FResource.ImageSequenceList.ImageSequences[35];
                    bool__23 = (call__24 != null);
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
        public override void ArmyPlaced(Aow.Units.AoWE.TArmyHS A1)
        {
            int L = default;
            Aow.Units.AoWE.TArmyHS ArmyHS = default;
            base.ArmyPlaced(A1);
            ArmyHS = A1;
            if (ArmyHS != null)
            {
                if (FEntrance == Aow.Units.Cave.TCaveEntrance.ceDown)
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
                var call__30 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.GetMapLevel(L);
                ((Aow.Units.AoWE.TAoWMapLevel)call__30).ExploreArea(call__29, call__28, unchecked((sbyte)ArmyHS.FArmy.FPlayer), call__27);
            }
        }

        // Cave.pas:292
        public sbyte EnterMovePoints(Aow.Units.AoWE.TArmy Army, byte Selection)
        {
            sbyte Result = default;
            Aow.Units.HSEngine.TMapField Field = default;
            int L = default;
            if (FEntrance == Aow.Units.Cave.TCaveEntrance.ceDown)
            {
                L = GetLhx() + 1;
            }
            else
            {
                L = GetLhx() - 1;
            }

            var call__31 = GetYhx();
            var call__32 = GetXhx();
            var call__33 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.GetField(call__32, call__31, L);
            Field = call__33;
            var call__34 = Aow.Units.Cave.Cave.CaveArmyMovePoints(Army, unchecked((byte)Selection), unchecked((sbyte)Field.TerrainType), unchecked((sbyte)Field.OverlayType));
            Result = unchecked((sbyte)call__34);
            return Result;
        }

        // Cave.pas:300
        public byte CanEnterSelection(Aow.Units.AoWE.TUnitList Units)
        {
            byte Result = default;
            int L = default;
            int I = default;
            Aow.Units.HSEngine.TMapField Field = default;
            Aow.Units.AoWE.TAbstractUnit AUnit = default;
            if (FEntrance == Aow.Units.Cave.TCaveEntrance.ceDown)
            {
                L = GetLhx() + 1;
            }
            else
            {
                L = GetLhx() - 1;
            }

            var call__35 = GetYhx();
            var call__36 = GetXhx();
            var call__37 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.GetField(call__36, call__35, L);
            Field = call__37;
            Result = 0;
            {
                I = 0;
                int end__38 = Units.Count - 1;
                for (; I <= end__38; I += 1)
                {
                    AUnit = Aow.Units.Cave.Cave.CaveUnit(Units, I);
                    var call__39 = Aow.Units.Cave.Cave.CaveUnitCost(AUnit, unchecked((sbyte)Field.TerrainType), unchecked((sbyte)Field.OverlayType));
                    var call__40 = AUnit.GetMovePoints();
                    if (call__39 <= call__40)
                    {
                        Result = (byte)(((int)Result) + (1 << I));
                    }
                }
            }

            return Result;
        }

        // Cave.pas:317
        public bool CanEnter(sbyte Player)
        {
            bool Result = default;
            Aow.Units.AoWE.TArmyHS ArmyHS = default;
            var call__41 = Owner.FindOwnedChild(131607);
            ArmyHS = (Aow.Units.AoWE.TArmyHS)call__41;
            bool bool__42 = (ArmyHS != null) && (Player == ArmyHS.FArmy.FPlayer);
            if (bool__42)
            {
                var call__43 = CanEnterSelection(ArmyHS.FArmy);
                bool__42 = (call__43 != 0);
            }

            Result = bool__42;
            return Result;
        }

        // Cave.pas:325
        public bool EnterEx()
        {
            bool Result = default;
            byte Selection = default;
            Aow.Units.AoWE.TMovepath Path = default;
            sbyte L = default;
            Aow.Rtl.AnsiString MessageText = default;
            Aow.Units.AoWE.TArmyHS ArmyHS = default;
            Aow.Units.AoWE.TAoWMapField Field = default;
            Aow.Units.NetworkE.TTokenEvent Event = default;
            var call__44 = Owner.FindOwnedChild(131607);
            ArmyHS = (Aow.Units.AoWE.TArmyHS)call__44;
            bool bool__45 = (ArmyHS != null);
            if (bool__45)
            {
                var call__46 = CanEnter(unchecked((sbyte)ArmyHS.FArmy.FPlayer));
                bool__45 = call__46;
            }

            if (bool__45)
            {
                var call__47 = CanEnterSelection((Aow.Units.AoWE.TUnitList)ArmyHS.FArmy);
                Selection = unchecked((byte)call__47);
                L = 0;
                Path = Aow.Units.AoWE.TMovepath.Create();
                switch (FEntrance)
                {
                    case Aow.Units.Cave.TCaveEntrance.ceDown:
                        L = unchecked((sbyte)(GetLhx() + 1));
                        break;
                    case Aow.Units.Cave.TCaveEntrance.ceUp:
                        L = unchecked((sbyte)(GetLhx() - 1));
                        break;
                }

                var call__48 = GetYhx();
                var call__49 = GetXhx();
                Path.AddXYL(unchecked((sbyte)call__49), unchecked((sbyte)call__48), unchecked((sbyte)L));
                var call__50 = EnterMovePoints(ArmyHS.FArmy, unchecked((byte)Selection));
                Path.List[0].Flag = unchecked((sbyte)call__50);
                var call__51 = GetLhx();
                var call__52 = GetYhx();
                var call__53 = GetXhx();
                Path.AddXYL(unchecked((sbyte)call__53), unchecked((sbyte)call__52), unchecked((sbyte)call__51));
                var call__54 = GetYhx();
                var call__55 = GetXhx();
                var call__56 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.GetField(call__55, call__54, L);
                Field = (Aow.Units.AoWE.TAoWMapField)call__56;
                var call__57 = Field.GetMoveAction(unchecked((sbyte)ArmyHS.FArmy.FPlayer));
                bool bool__58 = (call__57 != Aow.Units.AoWE.TMoveAction.maAttack);
                if (!bool__58)
                {
                    var call__59 = Field.GetPlayer();
                    bool__58 = (call__59 <= 0);
                }

                bool bool__60 = bool__58;
                if (!bool__60)
                {
                    var call__61 = Field.GetPlayer();
                    var call__62 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetDiplomaticRelation(unchecked((sbyte)ArmyHS.FArmy.FPlayer), unchecked((sbyte)call__61));
                    bool__60 = TSet<byte>.WithBounds(0, 255).With(1).With(3).Contains((byte)((int)call__62));
                }

                if (bool__60)
                {
                    var call__63 = Field.GetPlayer();
                    bool bool__64 = (call__63 > -1);
                    if (bool__64)
                    {
                        var call__65 = Field.GetPlayer();
                        bool__64 = (call__65 != ArmyHS.FArmy.FPlayer);
                    }

                    if (bool__64)
                    {
                        var call__66 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(ArmyHS.FArmy.FPlayer);
                        var call__67 = call__66.GetBusy();
                        if (!call__67)
                        {
                            // { Original passes action before mode. }
                            Event = Aow.Units.AoWE.AoWE.CreateArmyMoveTE(ArmyHS, unchecked((byte)Selection), Path, Aow.Units.AoWE.TMoveAction.maAttack, TSet<Aow.Units.AoWE.TArmyMoveModeElement>.Of());
                            if (Event != null)
                            {
                                var call__68 = Aow.Units.AoWE.AoWE.AoWHSMap.FTokenManager.GetTokenControl(ArmyHS.FArmy.FPlayer);
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
                        Result = Aow.Units.AoWE.AoWE.MoveArmyEx(ArmyHS, unchecked((byte)Selection), Path, TSet<Aow.Units.AoWE.TArmyMoveModeElement>.Of(), false);
                    }

                    if (Result && (unchecked((byte)Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer) == unchecked((byte)ArmyHS.FArmy.FPlayer)))
                    {
                        Aow.Units.AoWE.AoWE.AoWHSMap.ViewLevel(L);
                        Aow.Units.AoWE.AoWE.AoWHSMap.FSelectedArmy.Select(ArmyHS);
                    }
                }
                else
                {
                    // { Original only warns the seated player. }
                    if (ArmyHS.FArmy.FPlayer == Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer)
                    {
                        var call__69 = Field.GetPlayer();
                        var call__70 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(call__69);
                        var call__71 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.RaceNamesPluralRStr[call__70.FPlayerRace]);
                        var call__72 = Aow.Units.AoWE.AoWE.NoWarDeclaredtoXRStr;
                        var call__73 = Aow.Units.AoWE.AoWE.TranslateRStr(call__72);
                        var call__74 = Aow.Rtl.Rtl.Format(call__73, [call__71]);
                        MessageText = call__74;
                        ((Aow.Units.AoWE.TAbstractAoWHSMap)Aow.Units.AoWE.AoWE.AoWHSMap).ShowMessage(MessageText);
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
            Aow.Units.AoWE.TArmyHS ArmyHS = default;
            Aow.Units.Cave.TEnterCaveEventLog Log = default;
            if (CanEnter(unchecked((sbyte)Player)))
            {
                var call__75 = Owner.FindOwnedChild(131607);
                ArmyHS = (Aow.Units.AoWE.TArmyHS)call__75;
                var call__76 = CanEnterSelection((Aow.Units.AoWE.TUnitList)ArmyHS.FArmy);
                var call__77 = Aow.Units.Cave.Cave.CaveValidateSelection(ArmyHS.FArmy, 255);
                if (call__76 != call__77)
                {
                    Log = Aow.Units.Cave.TEnterCaveEventLog.Create(null);
                    Log.FCave = this;
                    var call__78 = Aow.Units.Cave.Cave.TranslateCaveName(Aow.Units.AoWE.AoWE.SplitOnEnterCaveWarningRStr);
                    Log.FMessage.Text = call__78;
                    Log.FButtons = Aow.Units.EventLog.TMessageEventLogButtons.mebYesNo;
                    Log.Execute(true, Aow.Units.EventLog.TExecuteTrigger.etUser, null);
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
        public static new Aow.Units.Cave.TCave Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.Cave.TCave o = new Aow.Units.Cave.TCave();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }

        public new partial class Meta : Aow.Units.AoWE.TStructure.Meta
        {
            // Cave.pas:160
            public override int ClassID()
            {
                int Result = default;
                Result = 131966;
                return Result;
            }

            // Cave.pas:25
            public new Aow.Units.Cave.TCave Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.Cave.TCave o = (Aow.Units.Cave.TCave)NewInstance();
                try
                {
                    o.Create__init(AOwner);
                }
                catch
                {
                    o.Destroy();
                    throw;
                }

                return o;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.Cave.TCave => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { TCaveResource (instance size 100, VMT 557B326C) }
    public partial class TCaveResource : Aow.Units.AoWE.TStructureResource
    {
        // Cave.pas:426
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            FTerrainBound = false;
            FConnectRoads = true;
        }

        // Cave.pas:439
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 131966;
            return Result;
        }

        // Cave.pas:49
        public static new Aow.Units.Cave.TCaveResource Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.Cave.TCaveResource o = new Aow.Units.Cave.TCaveResource();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }

        public new partial class Meta : Aow.Units.AoWE.TStructureResource.Meta
        {
            // Cave.pas:436
            public override int ClassID()
            {
                int Result = default;
                Result = 131967;
                return Result;
            }

            // Cave.pas:433
            public override Aow.Rtl.AnsiString ResourceEditName()
            {
                Aow.Rtl.AnsiString Result = default;
                Result = Aow.Units.Cave.Cave.TranslateCaveName(Aow.Units.AoWE.AoWE.CaveRStr);
                return Result;
            }

            // Cave.pas:49
            public new Aow.Units.Cave.TCaveResource Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.Cave.TCaveResource o = (Aow.Units.Cave.TCaveResource)NewInstance();
                try
                {
                    o.Create__init(AOwner);
                }
                catch
                {
                    o.Destroy();
                    throw;
                }

                return o;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.Cave.TCaveResource => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { TEnterCaveEventLog (instance size 44, VMT 557B335C): +$28 FCave. }
    public partial class TEnterCaveEventLog : Aow.Units.EventLog.TMessageEventLog
    {
        public Aow.Units.Cave.TCave FCave;
        // Cave.pas:442
        public override bool Yes()
        {
            bool Result = default;
            FCave.EnterEx();
            Result = true;
            return Result;
        }

        // Cave.pas:448
        public override bool No()
        {
            bool Result = default;
            Result = true;
            return Result;
        }

        // EventLog.pas:110
        public static new Aow.Units.Cave.TEnterCaveEventLog Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.Cave.TEnterCaveEventLog o = new Aow.Units.Cave.TEnterCaveEventLog();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
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
        public Aow.Units.FixedArray_byte_24 Reserved;
        [FieldOffset(24)]
        public int Flags;
    }

    [StructLayout(LayoutKind.Explicit, Size = 166)]
    public struct TCaveMapFields
    {
        [FieldOffset(0)]
        public Aow.Units.FixedArray_byte_165 Reserved;
        [FieldOffset(165)]
        public sbyte SeatedPlayer;
    }

    [StructLayout(LayoutKind.Explicit, Size = 166)]
    public struct TCavePlayerFields
    {
        [FieldOffset(0)]
        public Aow.Units.FixedArray_byte_165 Reserved;
        [FieldOffset(165)]
        public sbyte Race;
    }

    [StructLayout(LayoutKind.Explicit, Size = 305)]
    public struct TCaveMoveData
    {
        [FieldOffset(0)]
        public Aow.Units.FixedArray_byte_49 Reserved;
        [FieldOffset(49)]
        public Aow.Units.FixedArray_Aow_Units_FixedArray_sbyte_16_16 Costs;
    }

    public static partial class Cave
    {
        // Cave.pas:451
        public static void RegisterCave(Aow.Units.Engine.TEngine AEngine)
        {
            AEngine.RegisterEClasses([Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.Cave.TCave>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.Cave.TCaveResource>()]);
        }

        // { Original AoWE resource initialization table $55798BC0..$557990E4. }
        public static Aow.Rtl.AnsiString[] RaceNames = [(AnsiString)"Humans", (AnsiString)"Azracs", (AnsiString)"Lizards", (AnsiString)"Frostlings", (AnsiString)"Elves", (AnsiString)"Halflings", (AnsiString)"Dwarves", (AnsiString)"Highmen", (AnsiString)"Dark Elves", (AnsiString)"Orcs", (AnsiString)"Goblins", (AnsiString)"Undead(plural)", (AnsiString)"Independents"];
        public static readonly Aow.Rtl.AnsiString CaveNoWarRStr = (AnsiString)"Not at war with the %s";
        // Cave.pas:95
        // { original RT_STRING $FCD6 }
        // { needs: draft AoWE result declarations; retain their proven register ABI. }
        public static Aow.Rtl.AnsiString TranslateCaveName(Aow.Rtl.AnsiString S)
        {
            Aow.Rtl.AnsiString Result = default;
            Result = Aow.Units.AoWE.AoWE.TranslateRStr(S);
            return Result;
        }

        // Cave.pas:97
        public static sbyte CaveFieldPlayer(Aow.Units.AoWE.TAoWMapField Field)
        {
            sbyte Result = default;
            Result = unchecked((sbyte)Field.GetPlayer());
            return Result;
        }

        // Cave.pas:99
        public static byte CaveMoveAction(Aow.Units.AoWE.TAoWMapField Field, byte Player)
        {
            byte Result = default;
            Result = unchecked((byte)Field.GetMoveAction(unchecked((sbyte)Player)));
            return Result;
        }

        // Cave.pas:101
        public static Aow.Units.AoWE.TPlayer CavePlayer(Aow.Units.AoWE.TPlayerList List, int Player)
        {
            Aow.Units.AoWE.TPlayer Result = default;
            Result = List.GetPlayers(Player);
            return Result;
        }

        // Cave.pas:103
        public static bool CavePlayerBusy(Aow.Units.AoWE.TPlayer Player)
        {
            bool Result = default;
            Result = Player.GetBusy();
            return Result;
        }

        // Cave.pas:105
        public static byte CaveRelation(Aow.Units.AoWE.TPlayerList List, sbyte PlayerA, sbyte PlayerB)
        {
            byte Result = default;
            Result = unchecked((byte)List.GetDiplomaticRelation(unchecked((sbyte)PlayerA), unchecked((sbyte)PlayerB)));
            return Result;
        }

        // Cave.pas:114
        public static Aow.Units.AoWE.TAbstractUnit CaveUnit(Aow.Units.AoWE.TUnitList Units, int Index)
        {
            Aow.Units.AoWE.TAbstractUnit Result = default;
            Result = Units.GetUnit(Index);
            return Result;
        }

        // Cave.pas:116
        public static int CaveUnitCost(Aow.Units.AoWE.TAbstractUnit AUnit, sbyte Terrain, sbyte Overlay)
        {
            int Result = default;
            Result = AUnit.MovePointCost(unchecked((sbyte)Terrain), unchecked((sbyte)Overlay));
            return Result;
        }

        // Cave.pas:118
        public static sbyte CaveArmyMovePoints(Aow.Units.AoWE.TArmy Army, byte Selection, sbyte Terrain, sbyte Overlay)
        {
            sbyte Result = default;
            Result = unchecked((sbyte)Army.MoveOnMovePoints(unchecked((byte)Selection), unchecked((sbyte)Terrain), unchecked((sbyte)Overlay)));
            return Result;
        }

        // Cave.pas:122
        public static byte CaveValidateSelection(Aow.Units.AoWE.TArmy Army, byte Selection)
        {
            byte Result = default;
            Result = unchecked((byte)Army.ValidateSelection(unchecked((byte)Selection)));
            return Result;
        }

        // Cave.pas:124
        public static byte CaveVisibility(Aow.Units.AoWE.TArmy Army)
        {
            byte Result = default;
            Result = unchecked((byte)Army.VisibilityRange());
            return Result;
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
