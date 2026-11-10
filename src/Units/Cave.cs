// Translated from Cave.pas.
using System.Runtime.InteropServices;
using Aow.Game.Commands;
using Aow.Game.Draw;
using Aow.Game.Engine;
using Aow.Game.Engine.Buffers;
using Aow.Game.Engine.Nodes;
using Aow.Game.Engine.Storage;
using Aow.Game.Events;
using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Game.Units;
using Aow.Game.World;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Game.World.Structures;
using Aow.Rtl;

namespace Aow.Game.World.Structures
{

    // { ot=1 }
    // FEntrance
    public partial class Cave : Structure
    {
        public Cave(ECustomNode owner) : base(owner)
        {
            FEntrance = Entrance.Down;
        }

        // Cave.pas:160
        public new const int ClassID = 131966;

        public Entrance FEntrance;

        // Cave.pas:134
        public override void Dispose()
        {
            Entrance OldEntrance;
            Cave Partner;
            OldEntrance = FEntrance;
            FEntrance = Entrance.Idle;
            if (FStatus.Contains(MapObjectStatusElement.PlacedOnMap) && Owner != null)
            {
                if (!GetEditMode())
                {
                    if (OldEntrance == Entrance.Up)
                    {
                        Partner = (Cave)Aow.Game.World.Map.AoWHSMap.Current.MapContainer.GetField(FXhx, FYhx, ((MapField)Owner).Lhx - 1).FindOwnedChild(131966);
                        if (Partner != null && Partner.FEntrance == Entrance.Down)
                        {
                            Partner.Dispose();
                        }
                    }
                    else
                    {
                        Partner = (Cave)Aow.Game.World.Map.AoWHSMap.Current.MapContainer.GetField(FXhx, FYhx, ((MapField)Owner).Lhx + 1).FindOwnedChild(131966);
                        if (Partner != null && Partner.FEntrance == Entrance.Up)
                        {
                            Partner.Dispose();
                        }
                    }
                }
            }

            base.Dispose();
        }

        // Cave.pas:244
        public override void ReadWrite(EStorageStream Stream)
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
        public override void MoveExclusive(in HSSenderInfo Info)
        {
            MapField Field;
            if (Info.HN == 0)
            {
                switch (FEntrance)
                {
                    case Entrance.Up:
                        Field = Aow.Game.World.Map.AoWHSMap.Current.MapContainer.GetField(FXhx, FYhx, ((MapField)Owner).Lhx - 1);
                        break;
                    case Entrance.Down:
                        Field = Aow.Game.World.Map.AoWHSMap.Current.MapContainer.GetField(FXhx, FYhx, ((MapField)Owner).Lhx + 1);
                        break;
                    default:
                        Field = null;
                        break;
                }

                // { Original dereferences the destination even for an idle entrance. }
                ((MoveControl)Info.Sender).ExecuteExternalMove((AoWMapField)Info.MapField, (AoWMapField)Field, MemoryMarshal.Cast<FixedArray_sbyte_16, sbyte>(((MoveControl)(object)Info.Sender).FMovePointTable)[1 + 16 * Field.TerrainType + Field.OverlayType]);
            }
        }

        // Cave.pas:279
        public override void Activate()
        {
            base.Activate();
            ((MapField)Owner).UpdateMapField();
        }

        // Cave.pas:285
        public override void Deactivate()
        {
            base.Deactivate();
            FStatus = FStatus - Set<MapObjectStatusElement>.Of(MapObjectStatusElement.Activated);
            ((MapField)Owner).UpdateMapField();
        }

        // Cave.pas:250
        public override void UpdateMapField(in HSMapFieldInfo Info)
        {
            if (Info.HN == 0)
            {
                ((AoWMapField)(object)Info.MapField).FFieldFlags = ((AoWMapField)(object)Info.MapField).FFieldFlags | 32768;
            }
        }

        // Cave.pas:204
        public override bool CanPlace(MapContainer MapContainer, sbyte Xhx, sbyte Yhx, sbyte Lhx)
        {
            // { Original permits an upward cave on level zero; there is no lower-bound test. }
            return base.CanPlace(MapContainer, Xhx, Yhx, Lhx) && (Lhx + 1 < MapContainer.LevelCount || FEntrance == Entrance.Up);
        }

        // Cave.pas:211
        public override bool PlaceHX(MapContainer MapContainer, sbyte Xhx, sbyte Yhx, sbyte Lhx)
        {
            Cave Up;
            Cave Down;
            bool Result = base.PlaceHX(MapContainer, Xhx, Yhx, Lhx);
            if (Result)
            {
                if (!GetEditMode())
                {
                    if (FEntrance == Entrance.Down)
                    {
                        Up = (Cave)MapContainer.GetField(Xhx, Yhx, Lhx + 1).FindOwnedChild(131966);
                        if (Up == null)
                        {
                            Up = (Cave)FResource.CreateResourceUser();
                            Up.FEntrance = Entrance.Up;
                            Result = Up.PlaceHX(MapContainer, Xhx, Yhx, (sbyte)(Lhx + 1));
                        }
                        else
                        {
                            Result = Up.FEntrance == Entrance.Up;
                        }

                        // { Original destroys an existing partner when its entrance direction is wrong. }
                        if (!Result)
                        {
                            Up.Dispose();
                        }
                        else
                        {
                            OverrideTerrain();
                        }
                    }
                    else
                    {
                        Down = (Cave)MapContainer.GetField(Xhx, Yhx, Lhx - 1).FindOwnedChild(131966);
                        if (Down == null)
                        {
                            Down = (Cave)FResource.CreateResourceUser();
                            Down.FEntrance = Entrance.Down;
                            Result = Down.PlaceHX(MapContainer, Xhx, Yhx, (sbyte)(Lhx - 1));
                        }
                        else
                        {
                            Result = Down.FEntrance == Entrance.Down;
                        }

                        if (!Result)
                        {
                            Down.Dispose();
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
            if (FEntrance == Entrance.Down)
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
        public override Draw.Images.LibraryImage GetTopBorderImage()
        {
            Draw.Images.LibraryImage Result;
            if (FEntrance == Entrance.Up)
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
        public override Draw.Images.LibraryImage GetMiddleBorderImage()
        {
            Draw.Images.LibraryImage Result;
            if (FEntrance == Entrance.Up)
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
        public override Draw.Images.LibraryImage GetBottomBorderImage()
        {
            Draw.Images.LibraryImage Result;
            if (FEntrance == Entrance.Up)
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
        public override void ArmyPlaced(ArmyHS PlacedArmy)
        {
            int L;
            ArmyHS ArmyHS;
            base.ArmyPlaced(PlacedArmy);
            ArmyHS = PlacedArmy;
            if (ArmyHS != null)
            {
                if (FEntrance == Entrance.Down)
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
                ((AoWMapLevel)Aow.Game.World.Map.AoWHSMap.Current.MapContainer.GetMapLevel(L)).ExploreArea(xhx, yhx, ArmyHS.FArmy.FPlayer, visibilityRange);
            }
        }

        // Cave.pas:292
        public sbyte EnterMovePoints(Army Army, byte Selection)
        {
            MapField Field;
            int L;
            if (FEntrance == Entrance.Down)
            {
                L = GetLhx() + 1;
            }
            else
            {
                L = GetLhx() - 1;
            }

            var yhx = GetYhx();
            var xhx = GetXhx();
            Field = Aow.Game.World.Map.AoWHSMap.Current.MapContainer.GetField(xhx, yhx, L);
            return (sbyte)Army.MoveOnMovePoints(Selection, Field.TerrainType, Field.OverlayType);
        }

        // Cave.pas:300
        public byte CanEnterSelection(UnitList Units)
        {
            int L;
            MapField Field;
            AbstractUnit AUnit;
            if (FEntrance == Entrance.Down)
            {
                L = GetLhx() + 1;
            }
            else
            {
                L = GetLhx() - 1;
            }

            var yhx = GetYhx();
            var xhx = GetXhx();
            Field = Aow.Game.World.Map.AoWHSMap.Current.MapContainer.GetField(xhx, yhx, L);
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
            ArmyHS ArmyHS;
            ArmyHS = (ArmyHS)Owner.FindOwnedChild(131607);
            return ArmyHS != null && Player == ArmyHS.FArmy.FPlayer && CanEnterSelection(ArmyHS.FArmy) != 0;
        }

        // Cave.pas:325
        public bool EnterEx()
        {
            bool Result;
            byte Selection;
            Movepath Path;
            sbyte L;
            string MessageText;
            ArmyHS ArmyHS;
            AoWMapField Field;
            TokenEvent Event;
            ArmyHS = (ArmyHS)Owner.FindOwnedChild(131607);
            if (ArmyHS != null && CanEnter(ArmyHS.FArmy.FPlayer))
            {
                Selection = CanEnterSelection(ArmyHS.FArmy);
                L = 0;
                Path = new Movepath();
                switch (FEntrance)
                {
                    case Entrance.Down:
                        L = (sbyte)(GetLhx() + 1);
                        break;
                    case Entrance.Up:
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
                Field = (AoWMapField)Aow.Game.World.Map.AoWHSMap.Current.MapContainer.GetField(xhx, yhx3, L);
                bool condition = Field.GetMoveAction(ArmyHS.FArmy.FPlayer) != MoveAction.Attack || Field.GetPlayer() <= 0;
                if (!condition)
                {
                    var player = Field.GetPlayer();
                    var diplomaticRelation = Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetDiplomaticRelation(ArmyHS.FArmy.FPlayer, player);
                    condition = Set<byte>.WithBounds(0, 255).With(1).With(3).Contains((byte)(int)diplomaticRelation);
                }

                if (condition)
                {
                    if (Field.GetPlayer() > -1 && Field.GetPlayer() != ArmyHS.FArmy.FPlayer)
                    {
                        if (!Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(ArmyHS.FArmy.FPlayer).GetBusy())
                        {
                            // { Original passes action before mode. }
                            Event = global::Aow.Game.Draw.ArmyHS.CreateArmyMoveTE(ArmyHS, Selection, Path, MoveAction.Attack, Set<ArmyMoveModeElement>.Of());
                            if (Event != null)
                            {
                                Aow.Game.World.Map.AoWHSMap.Current.FTokenManager.GetTokenControl(ArmyHS.FArmy.FPlayer).AddEvent(Event);
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
                        Result = global::Aow.Game.Draw.ArmyHS.MoveArmyEx(ArmyHS, Selection, Path, Set<ArmyMoveModeElement>.Of(), false);
                    }

                    if (Result && (byte)AoWHSMap.Current.FSeatedPlayer == (byte)ArmyHS.FArmy.FPlayer)
                    {
                        Aow.Game.World.Map.AoWHSMap.Current.ViewLevel(L);
                        Aow.Game.World.Map.AoWHSMap.Current.FSelectedArmy.Select(ArmyHS);
                    }
                }
                else
                {
                    // { Original only warns the seated player. }
                    if (ArmyHS.FArmy.FPlayer == AoWHSMap.Current.FSeatedPlayer)
                    {
                        var player = Field.GetPlayer();
                        var playerPlayerRace = Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(player);
                        var raceNamesPlural = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameTables.RaceNamesPluralRStr[playerPlayerRace.FPlayerRace]);
                        MessageText = RtlUnit.Format(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameText.NoWarDeclaredtoXRStr), [raceNamesPlural]);
                        Aow.Game.World.Map.AoWHSMap.Current.ShowMessage(MessageText);
                    }

                    Result = false;
                }

                // { Original frees the path only on normal completion. }
                Path?.Dispose();
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
            ArmyHS ArmyHS;
            EnterCaveEventLog Log;
            if (CanEnter(Player))
            {
                ArmyHS = (ArmyHS)Owner.FindOwnedChild(131607);
                if (CanEnterSelection(ArmyHS.FArmy) != ArmyHS.FArmy.ValidateSelection(255))
                {
                    Log = new EnterCaveEventLog(null);
                    Log.FCave = this;
                    var splitOnEnterCaveWarning = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(SplitOnEnterCaveWarningRStr);
                    Log.FMessage.Text = splitOnEnterCaveWarning;
                    Log.FButtons = MessageEventLog.Buttons.YesNo;
                    Log.Execute(true, ExecuteTrigger.User, null);
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

        private static readonly string SplitOnEnterCaveWarningRStr = "Entering the cave will split your party. Do you wish to continue?";

        public new partial class Resource : StructureResource
        {
            public Resource(ECustomNode owner) : base(owner)
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
            public override string ResourceEditName() => Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(CaveRStr);

            private static readonly string CaveRStr = "Cave";

        }

        public enum Entrance : byte
        {
            Up = 0, Down = 1, Idle = 2
        }

        // Cave.pas:25

        // Cave.pas:25

    }
}

namespace Aow.Game.Events
{

    // FCave
    public partial class EnterCaveEventLog : MessageEventLog
    {
        public EnterCaveEventLog(ECustomNode owner) : base(owner)
        { }

        public Cave FCave;

        // Cave.pas:442
        public override bool Yes()
        {
            FCave.EnterEx();
            return true;
        }

        // Cave.pas:448
        public override bool No() => true;

    }
}

namespace Aow.Game.World.Terrain
{

    [StructLayout(LayoutKind.Explicit, Size = 28)]
    public struct CaveFieldFlags
    {
        [FieldOffset(0)]
        public FixedArray_byte_24 Reserved;
        [FieldOffset(24)]
        public int Flags;
    }

    [StructLayout(LayoutKind.Explicit, Size = 166)]
    public struct CaveMapFields
    {
        [FieldOffset(0)]
        public FixedArray_byte_165 Reserved;
        [FieldOffset(165)]
        public sbyte SeatedPlayer;
    }

    [StructLayout(LayoutKind.Explicit, Size = 166)]
    public struct CavePlayerFields
    {
        [FieldOffset(0)]
        public FixedArray_byte_165 Reserved;
        [FieldOffset(165)]
        public sbyte Race;
    }

    [StructLayout(LayoutKind.Explicit, Size = 305)]
    public struct CaveMoveData
    {
        [FieldOffset(0)]
        public FixedArray_byte_49 Reserved;
        [FieldOffset(49)]
        public FixedArray_Aow_Units_FixedArray_sbyte_16_16 Costs;
    }
}
