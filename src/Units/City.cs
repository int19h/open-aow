// Translated from City.pas.
using System.Runtime.CompilerServices;
using Aow.Formats.Tree;
using Aow.Game.Ai;
using Aow.Game.Ai.Groups;
using Aow.Game.Combat;
using Aow.Game.Commands;
using Aow.Game.Draw;
using Aow.Game.Draw.Animation;
using Aow.Game.Draw.Images;
using Aow.Game.Engine;
using Aow.Game.Engine.Buffers;
using Aow.Game.Engine.Collections;
using Aow.Game.Engine.Nodes;
using Aow.Game.Engine.Storage;
using Aow.Game.Events;
using Aow.Game.Magic;
using Aow.Game.Magic.Abilities;
using Aow.Game.Net;
using Aow.Game.Session;
using Aow.Game.Units;
using Aow.Game.World;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Game.World.Structures;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.World.Structures
{

    // { ot=1 }
    // FCity
    public partial class CityProductionControl : ProductionControl
    {
        public CityProductionControl(ECustomNode owner) : base(owner)
        { }

        public City FCity;

        // City.pas:645
        public override void GetLocation(ref sbyte X, ref sbyte Y, ref sbyte L)
        {
            X = FCity.GetXhx();
            Y = FCity.GetYhx();
            L = FCity.GetLhx();
        }

        // City.pas:707
        public override sbyte GetPlayer() => FCity.FPlayer;

        // City.pas:675
        public override bool UnitProduced()
        {
            City City;
            bool Valid;
            int Count;
            MapLevel Level;
            HNWalkData Walk = default;
            CityProductionEventLog Log;
            City = FCity;
            Valid = false;
            Count = City.GetTerrainCount();
            Level = (MapLevel)City.Owner.Owner;
            var yhx = City.GetYhx();
            var xhx = City.GetXhx();
            Aow.Game.World.Movement.HNWalkData.InitHNWalk(ref Walk, xhx, yhx, 0);
            while (Walk.HN < Count && !Valid)
            {
                if (City.FResource.TerrainList.Terrain[Walk.HN] >= 0 && Walk.X >= 0 && Walk.Y >= 0 && Walk.X < Level.Width && Walk.Y < Level.Height)
                {
                    var lhx = City.GetLhx();
                    Valid = FSettings.FUnit.Place(Walk.X, Walk.Y, lhx, City.FPlayer, Set<UnitPlaceModeElement>.Of()) != null;
                }

                Aow.Game.World.Movement.HNWalkData.WalkToNextHN(ref Walk);
            }

            if (!Valid)
            {
                var lhx = City.GetLhx();
                var cityYhx = City.GetYhx();
                var cityXhx = City.GetXhx();
                Valid = FSettings.FUnit.Place(cityXhx, cityYhx, lhx, City.FPlayer, Set<UnitPlaceModeElement>.Of(UnitPlaceModeElement.ScanArea)) != null;
            }

            Log = new CityProductionEventLog(null);
            Log.FPlayer = City.FPlayer;
            var name = FSettings.FUnit.GetName();
            var fCityName = FCity.GetName();
            Log.FText = RtlUnit.Format(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.XProducedAtYRStr), [name, fCityName]);
            var cityLhx = City.GetLhx();
            var yhx2 = City.GetYhx();
            Log.SetLocation(City.GetXhx(), yhx2, cityLhx);
            Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(Log.FPlayer).FEventLogbook.AddEvent(Log);
            Log.Release();
            return Valid;
        }

        // City.pas:658
        public override void UnitResearched()
        {
            City City;
            CityProductionEventLog Log;
            base.UnitResearched();
            City = FCity;
            Log = new CityProductionEventLog(null);
            Log.FPlayer = City.FPlayer;
            var unitProduction = FUnitProductions.GetItem(FSettings.FUnitProductionIndex);
            var unitName = Aow.Game.Engine.AoWHSSet.Current.FUnitResources.GetUnitResource(unitProduction.FUnitIndex).GetUnitName();
            var name = FCity.GetName();
            Log.FText = RtlUnit.Format(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.XInstalledAtYRStr), [unitName, name]);
            var lhx = City.GetLhx();
            var yhx = City.GetYhx();
            Log.SetLocation(City.GetXhx(), yhx, lhx);
            Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(Log.FPlayer).FEventLogbook.AddEvent(Log);
            Log.Release();
        }

        // City.pas:639
        public override void Update()
        {
            base.Update();
            if (FUpdateCount == 0)
            {
                FCity.Update();
            }
        }

        // City.pas:652
        public override Unit CreateUnitProduction(int Index)
        {
            Unit Result = base.CreateUnitProduction(Index);
            if (Result != null && FCity.FEnchantments.Contains(CityEnchantmentID.Warmonger))
            {
                Result.SetRank(Aow.Game.Magic.Abilities.Unit.Rank.Silver);
            }

            return Result;
        }

        // City.pas:710
        public override int UnitProductionTime(int Index)
        {
            int UnitIndex;
            UnitIndex = FUnitProductions.GetItem(Index).FUnitIndex;
            var unitResource = Aow.Game.Engine.AoWHSSet.Current.FUnitResources.GetUnitResource(UnitIndex);
            var size = FCity.GetSize();
            int Result = unitResource.FLevel - size / 2 + 1;
            if (Result < 1)
            {
                Result = 1;
            }

            return Result;
        }

        // intf_P1_02.inc:120

    }
}

namespace Aow.Game.Draw
{

    // FCity, FModeStartFrame, FMode, FShellAnimation, FShellEndFrame
    public partial class CityOverlayHS : SingleHS
    {
        public CityOverlayHS(ECustomNode owner) : base(owner)
        { }

        public City FCity;
        public int FModeStartFrame;
        public CityOverlayMode FMode;
        public AnonymousEnum_saSetup FShellAnimation;
        public int FShellEndFrame;

        // City.pas:718
        public override int GetShowPriority() => FCity.GetShowPriority() + 134217728;

        // City.pas:733
        public override void Show(int XhpSurf, int YhpSurf, in SurfaceDesc SurfaceDesc)
        {
            int Xhp = default;
            int Yhp = default;
            int Intensity;
            int Index;
            int Frame;
            ImageSequence Sequence;
            Aow.Game.World.Map.HexGeometry.HXtoHP(FCity.FXhx, FCity.FYhx, ref Xhp, ref Yhp);
            XhpSurf = Xhp - XhpSurf;
            YhpSurf = Yhp - YhpSurf;
            if (FCity.FEnchantmentsVisible)
            {
                if (FCity.FEnchantments.Contains(CityEnchantmentID.Warmonger))
                {
                    Intensity = Math.Abs((int)(((uint)Aow.Game.World.Map.AoWHSMap.Current.Frame) >> 1) % 20 - 10) + 50;
                    Sequence = FCity.FResource.ImageSequenceList.ImageSequences[17];
                    while (Sequence != null)
                    {
                        Index = Sequence.GetFrameIndex(0, 0);
                        if (Sequence == FCity.FResource.ImageSequenceList.ImageSequences[17])
                        {
                            Sequence.ImageLibrary[Index].ShowBlended(XhpSurf + Sequence.dXshift, YhpSurf + Sequence.dYshift, SurfaceDesc, Aow.Game.Draw.GfxState.IntensityBlendTable.LookupTables[Intensity]);
                        }
                        else
                        {
                            Sequence.ImageLibrary[Index].ShowBlended(XhpSurf + Sequence.dXshift, YhpSurf + Sequence.dYshift, SurfaceDesc, Aow.Game.Draw.GfxState.AlphaBlendTable.LookupTables[Intensity]);
                        }

                        Sequence = Sequence.Next;
                    }
                }

                if (FCity.FEnchantments.Contains(CityEnchantmentID.GoldRush))
                {
                    Intensity = Math.Abs((int)(((uint)Aow.Game.World.Map.AoWHSMap.Current.Frame) >> 1) % 20 - 10) + 70;
                    Sequence = FCity.FResource.ImageSequenceList.ImageSequences[18];
                    while (Sequence != null)
                    {
                        if (Sequence == FCity.FResource.ImageSequenceList.ImageSequences[18])
                        {
                            Index = Sequence.GetFrameIndex(0, 0);
                            if (Sequence.ImageLibrary.ImageExist(Index))
                            {
                                Sequence.ImageLibrary[Index].ShowBlended(XhpSurf + Sequence.dXshift, YhpSurf + Sequence.dYshift, SurfaceDesc, Aow.Game.Draw.GfxState.IntensityBlendTable.LookupTables[Intensity]);
                            }
                        }
                        else
                        {
                            Index = Sequence.GetFrameIndex(0, Aow.Game.World.Map.AoWHSMap.Current.Frame);
                            if (Sequence.ImageLibrary.ImageExist(Index))
                            {
                                Sequence.ImageLibrary[Index].Show(XhpSurf + Sequence.dXshift, YhpSurf + Sequence.dYshift, SurfaceDesc);
                            }
                        }

                        Sequence = Sequence.Next;
                    }
                }

                if (FCity.FEnchantments.Contains(CityEnchantmentID.AntiMagicShell))
                {
                    Sequence = FCity.FResource.ImageSequenceList.ImageSequences[19];
                    if (Sequence != null)
                    {
                        Intensity = 50;
                        Index = Sequence.GetFrameIndex(0, 0);
                        if (FShellEndFrame != -1 && Aow.Game.World.Map.AoWHSMap.Current.Frame < FShellEndFrame)
                        {
                            switch (FShellAnimation)
                            {
                                case AnonymousEnum_saSetup.Setup:
                                    Frame = FShellEndFrame - Aow.Game.World.Map.AoWHSMap.Current.Frame;
                                    if (Frame <= 50)
                                    {
                                        Intensity = 50 - Frame;
                                    }
                                    else
                                    {
                                        FShellEndFrame = -1;
                                    }

                                    break;
                                case AnonymousEnum_saSetup.Fade:
                                    Frame = FShellEndFrame - Aow.Game.World.Map.AoWHSMap.Current.Frame;
                                    if (Frame <= 20)
                                    {
                                        Intensity = Intensity + (10 - Math.Abs(Frame - 10)) * 3;
                                    }
                                    else
                                    {
                                        FShellEndFrame = -1;
                                    }

                                    break;
                            }
                        }

                        Sequence.ImageLibrary[Index].ShowBlended(XhpSurf + Sequence.dXshift, YhpSurf + Sequence.dYshift, SurfaceDesc, Aow.Game.Draw.GfxState.IntensityBlendTable.LookupTables[Intensity]);
                    }
                }

                if (FMode == CityOverlayMode.Looting)
                {
                    FCity.FResource.ShowComplexImage(XhpSurf, YhpSurf, SurfaceDesc, 22, (uint)(((MapField)FCity.Owner).Xhx + ((MapField)FCity.Owner).Yhx * 16));
                }
            }
        }

        // City.pas:721
        public void PlaySetupShellAnimation()
        {
            FShellAnimation = AnonymousEnum_saSetup.Setup;
            FShellEndFrame = Aow.Game.World.Map.AoWHSMap.Current.Frame + 50;
        }

        // City.pas:727
        public void PlayFadeShellAnimation()
        {
            FShellAnimation = AnonymousEnum_saSetup.Fade;
            FShellEndFrame = Aow.Game.World.Map.AoWHSMap.Current.Frame + 20;
        }

        // City.pas:815
        public void SetMode(CityOverlayMode Mode)
        {
            if (Mode != FMode)
            {
                FModeStartFrame = Aow.Game.World.Map.AoWHSMap.Current.Frame;
                FMode = Mode;
            }
        }

        public enum AnonymousEnum_saSetup : byte
        {
            Setup = 0, Fade = 1
        }

        public enum CityOverlayMode : byte
        {
            Idle = 0, ShowRazeAnimation = 1, Looting = 2
        }

        // Engine.pas:524

    }
}

namespace Aow.Game.Units
{

    // FRace, FTime, FRelation
    public partial class MigrationItem : EObject
    {
        public MigrationItem(ECustomNode owner) : base(owner)
        { }

        public sbyte FRace;
        public int FTime;
        public Status FRelation;
    }
}

namespace Aow.Game.World.Structures
{

    public partial class MigrationList : ENode
    {
        public MigrationList(ECustomNode owner) : base(owner)
        { }

        // City.pas:2711
        public MigrationItem GetItems(int Index) => (MigrationItem)FChildren.List[Index];

    }

    // FEnchantmentID, FEnchantmentLocation
    public partial class CityEnchantment : GlobalEnchantment
    {
        // City.pas:2722
        public new const int ClassID = 131820;

        public new CityEnchantmentID FEnchantmentID;
        public XYL FEnchantmentLocation;

        // City.pas:2727
        public override void ReadWrite(EStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwData(Bytes.Of(ref FEnchantmentID, 1), 1, 20);
            Stream.rwInteger(ref FEnchantmentLocation.Packed, 21);
        }

        public override void PersistFields(IPersister persister)
        {
            base.PersistFields(persister);
            persister.Enum("TCityEnchantment.EnchantmentID", ref FEnchantmentID);
            persister.Int("EnchantmentLocation", ref FEnchantmentLocation.Packed);
        }

        // City.pas:2716
        public CityEnchantment(ECustomNode owner) : base(owner)
        {
            FEnchantmentMode = FEnchantmentMode - Set<GlobalEnchantmentModeElement>.Of(GlobalEnchantmentModeElement.TrueGlobal, GlobalEnchantmentModeElement.Owned);
        }

        // City.pas:2762
        public override string GetName() => Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.World.Structures.CityEnchantment.TCityEnchantmentIDRStr[(int)FEnchantmentID]);

        // City.pas:2739
        public override XYL GetLocation()
        {
            XYL Result = default;
            Result.X = FEnchantmentLocation.X;
            Result.Y = FEnchantmentLocation.Y;
            Result.L = FEnchantmentLocation.L;
            return Result;
        }

        // City.pas:2767
        public override int GetAIUpkeepPriority()
        {
            int Result;
            City City;
            City = GetCity();
            if (City != null)
            {
                Result = City.AICityEnchantmentUpkeepPriority(FEnchantmentID);
            }
            else
            {
                Result = 0;
            }

            return Result;
        }

        // City.pas:2746
        public override bool Deactivate(byte Player)
        {
            bool Result;
            if (FEnchantmentStatus.Contains(GlobalEnchantmentStatusElement.Activated))
            {
                Result = base.Deactivate(Player);
                GetCity().DeactivateEnchantment(this);
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // City.pas:2734
        public override bool VisibleForPlayer(sbyte Player) => GetCity().VisibleForPlayer(Player);

        // City.pas:2756
        public City GetCity() => (City)Aow.Game.World.Map.AoWHSMap.Current.MapContainer.GetField(FEnchantmentLocation.X, FEnchantmentLocation.Y, FEnchantmentLocation.L).FindNoneTransparentHS(typeof(City));

        private static global::Aow.Game.Engine.Buffers.FixedArray_string_4 TCityEnchantmentIDRStr = global::Aow.Game.Engine.Buffers.FixedArray_string_4.From(["Fountain Of Life", "Gold Rush", "Anti Magic Shell", "Warmonger"]);

    }

    // FMode, FRace, FName, FWallType, FUpgradeLevel, FProduction, FFlagID, FEnchantmentList,
    // FEnchantments, FOverlay, FEnchantmentsVisible, FWallBackImage, FWallFrontImage, FCityImage,
    // FDefendersMode
    public partial class City : PlayerCropStructure
    {
        public City(ECustomNode owner) : base(owner)
        {
            FProduction = new CityProductionControl(null);
            FProduction.FCity = this;
            FDefendersMode = DefendersGenerateMode.Average;
            FRace = -1;
            FName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.World.Structures.City.NonameRStr);
            FUpgradeLevel = 1;
            FOverlay = new CityOverlayHS(null);
            FOverlay.FCity = this;
            FEnchantmentList = new GlobalEnchantmentList(null);
            FEnchantmentsVisible = false;
        }

        // City.pas:849
        public new const int ClassID = 131336;

        public Set<ModeElement> FMode;
        public sbyte FRace;
        public string FName = "";
        public byte FWallType;
        public byte FUpgradeLevel;
        public CityProductionControl FProduction;
        public new byte FFlagID;
        public GlobalEnchantmentList FEnchantmentList;
        public Set<CityEnchantmentID> FEnchantments;
        public CityOverlayHS FOverlay;
        public bool FEnchantmentsVisible;
        public Aow.Game.Draw.Images.LibraryImage FWallBackImage;
        public Aow.Game.Draw.Images.LibraryImage FWallFrontImage;
        public Aow.Game.Draw.Images.LibraryImage FCityImage;
        public DefendersGenerateMode FDefendersMode;

        // City.pas:839
        public override void Dispose()
        {
            if (GetVisible())
            {
                Aow.Game.World.Map.AoWHSMap.Current.DynamicVisibleHS.RemoveChild(FOverlay);
            }

            FOverlay?.Dispose();
            base.Dispose();
            FProduction?.Dispose();
            FEnchantmentList?.Dispose();
            FEnchantmentList = null;
        }

        // City.pas:2157
        public override void ReadWrite(EStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwString(ref FName, 12);
            Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FRace), 13);
            Stream.rwData(Bytes.Of(ref FWallType, 1), 1, 15);
            Stream.rwByte(ref FUpgradeLevel, 16);
            Stream.rwEObject(FProduction, 18);
            RwSet(ref FMode, 1, 22, Stream.rwData, 0);
            Stream.rwData(Bytes.Of(ref FDefendersMode, 1), 1, 27);
            Stream.rwEObject(FEnchantmentList, 29);
        }

        // City.pas:824

        // City.pas:2191
        public override void MsgProc(ref EMsg Msg, EObject Sender)
        {
            base.MsgProc(ref Msg, Sender);
            if (Msg.MsgID == 537001989)
            {
                if (FRace >= 0)
                {
                    if (Aow.Game.World.Map.AoWHSMap.Current.FRaces.GetRace(FRace) == null || !Aow.Game.World.Map.AoWHSMap.Current.FRaces.GetRace(FRace).FEnabled)
                    {
                        FRace = -1;
                        UpdateImages();
                    }
                }
            }
            else
            {
                if (Msg.MsgID == 537001993)
                {
                    UpdateVisibleForSeatedPlayer();
                }
            }
        }

        // City.pas:2065
        public override void TerrainChanged(in HSTerrainInfo Info)
        {
            base.TerrainChanged(Info);
            UpdateImages();
        }

        // City.pas:2132
        public override void Show(int XhpSurf, int YhpSurf, in SurfaceDesc SurfaceDesc)
        {
            int X = default;
            int Y = default;
            Aow.Game.World.Map.HexGeometry.HXtoHP(FXhx, FYhx, ref X, ref Y);
            XhpSurf = X - XhpSurf;
            YhpSurf = Y - YhpSurf;
            if (!FMode.Contains(ModeElement.Razed) || GetBuilding())
            {
                if ((Aow.Game.World.Map.AoWHSMap.Current.Status.Contains(HSMapStatusElement.Editting) || !FFogged || !Aow.Game.World.Map.AoWHSMap.Current.FFogEnabled) && FFlagID != 63 && (((PlayerStructureResource)FResource).FClaimed || (FFlagID & 32) == 0))
                {
                    var cityResource = (PlayerStructureResource)FResource;
                    if (cityResource.FFlagType <= 1)
                    {
                        Aow.Game.Engine.AoWHSSet.Current.FFlagControl.ShowFlag(cityResource.FFlagX + XhpSurf, cityResource.FFlagY + YhpSurf, cityResource.Engine.HSMap.Frame + X, this.FFlagID, SurfaceDesc);
                    }
                    else
                    {
                        Aow.Game.Engine.AoWHSSet.Current.FFlagControl.ShowStructureFlag(cityResource.FFlagX + XhpSurf, cityResource.FFlagY + YhpSurf, cityResource.Engine.HSMap.Frame + X, this.FFlagID, SurfaceDesc);
                    }
                }
            }

            if (FMode.Contains(ModeElement.TownQuake))
            {
                XhpSurf = XhpSurf + Random(5) - 2;
                YhpSurf = YhpSurf + Random(5) - 2;
            }

            if (!FMode.Contains(ModeElement.Razed))
            {
                ((global::Aow.Game.World.Structures.Resource)FResource).ShowCity(XhpSurf, YhpSurf, SurfaceDesc, this);
            }
            else
            {
                ((StructureResource)FResource).Show(XhpSurf, YhpSurf, SurfaceDesc, this);
            }
        }

        // City.pas:2170
        public override bool PlaceOnMap()
        {
            bool Result = base.PlaceOnMap();
            if (Result)
            {
                UpdateMapFields((MapLevel)Owner.Owner, FXhx, FYhx);
            }

            return Result;
        }

        // City.pas:2176
        public override void RemoveFromMap()
        {
            MapLevel MapLevel;
            MapLevel = (global::Aow.Game.World.Map.MapLevel)Owner.Owner;
            base.RemoveFromMap();
            UpdateMapFields(MapLevel, FXhx, FYhx);
        }

        // City.pas:2378
        public override void MapFieldMsgProc(ref MapFieldMsg Msg)
        {
            if (!(Msg.MsgID == 4364 && FResource.TerrainList.Terrain[Msg.HN] >= 0))
            {
                base.MapFieldMsgProc(ref Msg);
            }

            if (FResource.TerrainList.Terrain[Msg.HN] >= 0)
            {
                if (Msg.MsgID == 4364 && Msg.HN == 0)
                {
                    if ((!Aow.Game.World.Map.AoWHSMap.Current.FFogEnabled || !FFogged) && FPlayer >= 0)
                    {
                        if (FPlayer == 0)
                        {
                            Msg.Param[0] = Msg.Param[0].Int + 851968;
                        }
                        else
                        {
                            var player = Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(FPlayer);
                            Msg.Param[0] = Msg.Param[0].Int + (player.FPlayerRace + 1) * 65536;
                        }
                    }
                    else
                    {
                        Msg.Param[0] = Msg.Param[0].Int + 917504;
                    }

                    Msg.Param[0] = Msg.Param[0].Int + (((global::Aow.Game.World.Structures.Resource)FResource).FSize - 1) * 4194304;
                }
                else
                {
                    if (Msg.MsgID == 4609)
                    {
                        Msg.Result = FProduction;
                    }
                    else
                    {
                        if (Msg.MsgID == 4360)
                        {
                            ReturnBattleFieldInfo(ref Msg.Param[0].As<BattleFieldQuery>().Info);
                        }
                        else
                        {
                            if (Msg.MsgID == 4359)
                            {
                                InitCombat(Msg.Param[0].As<global::Aow.Game.Combat.Combat>(), Msg.Param[1].As<ArmyHS>());
                                Msg.Result = 1;
                            }
                            else
                            {
                                if (Msg.MsgID == 4365)
                                {
                                    UpdateVisibleForSeatedPlayer();
                                }
                                else
                                {
                                    if (Msg.MsgID == 4387)
                                    {
                                        if (FWallType != 0 && !FMode.Contains(ModeElement.Warmode) && !GetRazed())
                                        {
                                            if (this == Msg.Param[0].As<MapField>().FindNoneTransparentHS(typeof(City)))
                                            {
                                                Msg.Result = FWallType;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (Msg.MsgID == 4389)
                                        {
                                            if (FPlayer == 0)
                                            {
                                                var independentRelation = GetIndependentRelation();
                                                Msg.Result = independentRelation;
                                            }
                                        }
                                        else
                                        {
                                            if (Msg.MsgID == 4366)
                                            {
                                                if (IncommingNegotiateRequest(Msg.Param[0].As<NegotiateRequestEventLog>()))
                                                {
                                                    Msg.Param[1] = 1;
                                                }
                                            }
                                            else
                                            {
                                                if (Msg.MsgID == 4367)
                                                {
                                                    if (IncommingNegotiateOffer(Msg.Param[0].As<NegotiateTE>()))
                                                    {
                                                        Msg.Result = 1;
                                                    }
                                                }
                                                else
                                                {
                                                    if (Msg.MsgID == 4393 && FEnchantments.Contains(CityEnchantmentID.AntiMagicShell))
                                                    {
                                                        Msg.Param[0] = 1;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        // City.pas:2124
        public override void Deactivate()
        {
            UnregisterEnchantments();
            if (FPlayer != -1)
            {
                Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FProductionControls.Remove(FProduction);
            }

            FProduction.Deactivate();
            base.Deactivate();
        }

        // City.pas:2184
        public override void UpdateMapField(in HSMapFieldInfo Info)
        {
            base.UpdateMapField(Info);
            if (FResource.TerrainList.GetTerrain(Info.HN) >= 0)
            {
                ((AoWMapField)Info.MapField).FFieldFlags = ((AoWMapField)Info.MapField).FFieldFlags | 67108864;
            }
        }

        // City.pas:1246
        public override void MakeVisible(VisibilityList VisibilityList)
        {
            if (!FStatus.Contains(Aow.Game.World.MapObjectStatusElement.Visible))
            {
                base.MakeVisible(VisibilityList);
                if ((Set<CityEnchantmentID>.Of(CityEnchantmentID.GoldRush, CityEnchantmentID.AntiMagicShell, CityEnchantmentID.Warmonger) & FEnchantments) != Set<CityEnchantmentID>.Of())
                {
                    FOverlay.MakeVisible(VisibilityList);
                }
            }
        }

        // City.pas:2617
        public override void Select()
        {
            base.Select();
        }

        // City.pas:1787
        public override sbyte GetValidTerrainType(MapLevel MapLevel, sbyte Xhx, sbyte Yhx) => GetMostFrequentMapFieldTerrainType(MapLevel, Xhx, Yhx, -1);

        // City.pas:2422
        public override Aow.Game.Draw.Images.LibraryImage GetTopBorderImage()
        {
            int Index;
            ImageSequenceList Sequences;
            Aow.Game.Draw.Images.LibraryImage Result = GetTopCityActionImage(GetActiveOrder());
            if (Result == null)
            {
                if (FRace != -1)
                {
                    if (GetLhx() == 0)
                    {
                        Result = Aow.Game.Engine.AoWHSSet.Current.FRaceResources.GetRaceResource(FRace).FImageLibrary.GetSafe(40);
                    }
                    else
                    {
                        Result = Aow.Game.Engine.AoWHSSet.Current.FRaceResources.GetRaceResource(FRace).FImageLibrary.GetSafe(43);
                    }
                }
                else
                {
                    Result = null;
                }
            }

            if (FMode.Contains(ModeElement.Razed))
            {
                Index = 42;
                Sequences = FResource.ImageSequenceList;
                if (Sequences != null)
                {
                    if (Sequences.GetImageSequence(Index) != null)
                    {
                        Result = FResource.ImageSequenceList.GetImageSequence(Index).GetPreviewImage();
                    }
                    else
                    {
                        Result = null;
                    }
                }
                else
                {
                    Result = null;
                }
            }

            return Result;
        }

        // City.pas:2449
        public override Aow.Game.Draw.Images.LibraryImage GetMiddleBorderImage()
        {
            int Index;
            ImageSequenceList Sequences;
            Aow.Game.Draw.Images.LibraryImage Result = GetMiddleCityActionImage(GetActiveOrder());
            if (Result == null)
            {
                if (FRace != -1)
                {
                    if (GetLhx() == 0)
                    {
                        Result = Aow.Game.Engine.AoWHSSet.Current.FRaceResources.GetRaceResource(FRace).FImageLibrary.GetSafe(41);
                    }
                    else
                    {
                        Result = Aow.Game.Engine.AoWHSSet.Current.FRaceResources.GetRaceResource(FRace).FImageLibrary.GetSafe(44);
                    }
                }
                else
                {
                    Result = null;
                }
            }

            if (FMode.Contains(ModeElement.Razed))
            {
                Index = 43;
                Sequences = FResource.ImageSequenceList;
                if (Sequences != null)
                {
                    if (Sequences.GetImageSequence(Index) != null)
                    {
                        Result = FResource.ImageSequenceList.GetImageSequence(Index).GetPreviewImage();
                    }
                    else
                    {
                        Result = null;
                    }
                }
                else
                {
                    Result = null;
                }
            }

            return Result;
        }

        // City.pas:2476
        public override Aow.Game.Draw.Images.LibraryImage GetBottomBorderImage()
        {
            int Index;
            ImageSequenceList Sequences;
            Aow.Game.Draw.Images.LibraryImage Result = GetBottomCityActionImage(GetActiveOrder());
            if (Result == null)
            {
                if (FRace >= 0)
                {
                    if (GetLhx() == 0)
                    {
                        Result = Aow.Game.Engine.AoWHSSet.Current.FRaceResources.GetRaceResource(FRace).FImageLibrary.GetSafe(42);
                    }
                    else
                    {
                        Result = Aow.Game.Engine.AoWHSSet.Current.FRaceResources.GetRaceResource(FRace).FImageLibrary.GetSafe(45);
                    }
                }
                else
                {
                    Result = null;
                }
            }

            if (FMode.Contains(ModeElement.Razed))
            {
                Index = 44;
                Sequences = FResource.ImageSequenceList;
                if (Sequences != null)
                {
                    if (Sequences.GetImageSequence(Index) != null)
                    {
                        Result = FResource.ImageSequenceList.GetImageSequence(Index).GetPreviewImage();
                    }
                    else
                    {
                        Result = null;
                    }
                }
                else
                {
                    Result = null;
                }
            }

            return Result;
        }

        // City.pas:2080
        public override string GetName() => FName;

        // City.pas:1623
        public override void SetRazed(bool Value)
        {
            if (Value != GetRazed())
            {
                FProduction.ExecuteDeleteProductions();
                if (Value)
                {
                    SetPlayer(-1);
                }

                if (Value)
                {
                    FMode = FMode + Set<ModeElement>.Of(ModeElement.Razed);
                }
                else
                {
                    FMode = FMode - Set<ModeElement>.Of(ModeElement.Razed);
                }

                ImageIndex = GetTerrainTypeImage(TerrainType);
                ResetEnchantments();
                UpdateImages();
                UpdateCrops();
                Update();
                SetWallType(0);
            }
        }

        // City.pas:1618
        public override bool GetRazed() => FMode.Contains(ModeElement.Razed);

        // City.pas:1111
        public override void SetupRazeDefenderAG(UnitList Armies, UnitList Units)
        {
            AIGroup Group;
            if (Aow.Game.World.Map.AoWHSMap.Current.Random(8) == 0)
            {
                var unitsToIndependentRelation = Aow.Game.Units.UnitList.UnitsToIndependentRelation(Units, 5);
                Group = Aow.Game.World.Map.AoWHSMap.Current.FAIGroupManager.CreateGroup(0, unitsToIndependentRelation, GroupBehavior.Refuge);
                Group.AddUnits(Units);
                Group.Release();
            }
            else
            {
                base.SetupRazeDefenderAG(Armies, Units);
            }
        }

        // City.pas:864
        public override ExecuteResult ExecuteTE(StructureTE TE, TokenExecuter Executer) => base.ExecuteTE(TE, Executer);

        // City.pas:1134
        public override bool GetDefenseRequirements(sbyte Player, ref DefenseRequirements Requirements)
        {
            bool Result;
            BattleFieldInfo Info = default;
            int Maximum;
            int Minimum;
            global::Aow.Game.Engine.Status Relation;
            if ((FMode & Set<ModeElement>.Of(ModeElement.Warmode, ModeElement.Razed)) == Set<ModeElement>.Of())
            {
                Aow.Game.Combat.BattleFieldInfo.SetupBattleFieldInfo(ref Info, Player, 0, Set<BattleFieldInfoRequestsElement>.Of(), Aow.Game.Magic.Abilities.AbilityRules.dtAll - Set<DamageType>.Of(DamageType.Wall));
                Info.Requests = Info.Requests + Set<BattleFieldInfoRequestsElement>.Of(BattleFieldInfoRequestsElement.IgnorePassiveIndependents);
                var lhx = GetLhx();
                var yhx = GetYhx();
                var xhx = GetXhx();
                Aow.Game.World.Map.AoWHSMap.Current.RetrieveBattleFieldInfo(xhx, yhx, lhx, 10, ref Info);
                Requirements.Strength = Info.Strength;
                Maximum = Round(GetSize() * 5000);
                Minimum = Round((double)(GetSize() * 5000) * Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(Player).FAIControl.FDefenseStrengthMultiplier);
                if (Player != 0)
                {
                    Relation = Aow.Game.World.Map.AoWHSMap.Current.FRaces.GetRace(FRace).GetPlayerRelation(Player);
                }
                else
                {
                    Relation = global::Aow.Game.Engine.Status.Friendly;
                }

                // { Original condition makes this neutral-player adjustment unreachable. }
                if (Set<global::Aow.Game.Engine.Status>.Of(global::Aow.Game.Engine.Status.Hostile, global::Aow.Game.Engine.Status.Wary, global::Aow.Game.Engine.Status.Neutral).Contains(Relation) && Player == FPlayer && FPlayer == 0)
                {
                    Maximum = Round(Maximum) + 7500;
                    Minimum = Maximum;
                }

                if (Requirements.Strength < Minimum)
                {
                    Requirements.Strength = Minimum;
                }

                if (Maximum < Requirements.Strength)
                {
                    Requirements.Strength = Maximum;
                }

                Result = true;
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // City.pas:1123
        public override DefensePriority GetDefensePriority()
        {
            DefensePriority Result;
            if ((FMode & Set<ModeElement>.Of(ModeElement.Warmode, ModeElement.Razed)) == Set<ModeElement>.Of())
            {
                if (GetSize() == 1)
                {
                    Result = DefensePriority.Low;
                }
                else
                {
                    Result = DefensePriority.Average;
                }

                if (FProduction.FStatus == Aow.Game.World.Structures.ProductionControl.Status.Producing && Result == DefensePriority.Low)
                {
                    Result = DefensePriority.Average;
                }

                bool condition = FPlayer != 0;
                if (condition)
                {
                    var playerRelation = Aow.Game.World.Map.AoWHSMap.Current.FRaces.GetRace(FRace).GetPlayerRelation(FPlayer);
                    condition = Set<global::Aow.Game.Engine.Status>.Of(global::Aow.Game.Engine.Status.Hostile, global::Aow.Game.Engine.Status.Wary).Contains(playerRelation);
                }

                if (condition)
                {
                    Result = DefensePriority.High;
                }
            }
            else
            {
                Result = DefensePriority.None;
            }

            return Result;
        }

        // City.pas:1713
        public override void NewDay()
        {
            int I;
            base.NewDay();
            if (FRace < 0)
            {
                if (FRace < -1 && Aow.Game.World.Map.AoWHSMap.Current.FGameType == GameType.Campaign)
                {
                    I = Math.Abs(FRace) - 3;
                    if (I >= 0 && I < Aow.Game.Magic.Abilities.GameTables.AoWEngine.FCampaign.FRaceHistory.Count)
                    {
                        SetRace((sbyte)Aow.Game.Magic.Abilities.GameTables.AoWEngine.FCampaign.FRaceHistory[I]);
                    }
                    else
                    {
                        SetRace(Aow.Game.World.Map.AoWHSMap.Current.FRaces.GetRandomRace());
                    }
                }
                else
                {
                    SetRace(Aow.Game.World.Map.AoWHSMap.Current.FRaces.GetRandomRace());
                }
            }

            if (Aow.Game.World.Map.AoWHSMap.Current.FDay == 1)
            {
                FProduction.FUnitProductions.Clear();
                SetUpgradeLevel(FUpgradeLevel);
                GenerateDefenders();
                if (FWallType == 3)
                {
                    FWallType = (byte)Aow.Game.World.Map.AoWHSMap.Current.Random(3);
                    UpdateImages();
                }
            }

            CheckRebellion();
            if (FOverlay.FMode == Aow.Game.Draw.CityOverlayHS.CityOverlayMode.ShowRazeAnimation)
            {
                FOverlay.SetMode(Aow.Game.Draw.CityOverlayHS.CityOverlayMode.Idle);
            }
        }

        // City.pas:1705
        public override void NewTurn(sbyte Player)
        {
            base.NewTurn(Player);
            if (!FMode.Contains(ModeElement.Warmode))
            {
                FProduction.NewTurn(Player);
            }

            if (Player == FPlayer)
            {
                if (Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(Player).FPlayerType != PlayerType.Human)
                {
                    UpdateAIMigration();
                }
            }
        }

        // City.pas:1335
        public override void ArmyPlaced(ArmyHS PlacedArmy)
        {
            base.ArmyPlaced(PlacedArmy);
            if (FEnchantments.Contains(CityEnchantmentID.FountainOfLife))
            {
                ExecuteFountainOfLife();
            }

            if (FEnchantments.Contains(CityEnchantmentID.AntiMagicShell))
            {
                FOverlay.PlayFadeShellAnimation();
            }
        }

        // City.pas:1342
        public override void ArmyRemoved(ArmyHS Army)
        {
            base.ArmyRemoved(Army);
            UpdatePlayer();
        }

        // City.pas:886
        public override void BuildingDone()
        {
            FUpgradeLevel = 1;
            FWallType = 0;
            SetRace(Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(FBuildingPlayer).FPlayerRace);
            base.BuildingDone();
        }

        // City.pas:1604
        public override bool GenerateRazeDefenders(sbyte Player, UnitList Units)
        {
            GenerateRebelUnits(Units);
            return true;
        }

        // City.pas:1610
        public override bool ExecuteRaze(sbyte Player)
        {
            bool Result = base.ExecuteRaze(Player);
            if (Result)
            {
                var playerRelationValue = Aow.Game.World.Map.AoWHSMap.Current.FRaces.GetRace(FRace).GetPlayerRelationValue(Player);
                Aow.Game.World.Map.AoWHSMap.Current.FRaces.GetRace(FRace).SetPlayerRelationValue(Player, playerRelationValue + -30);
            }

            return Result;
        }

        // City.pas:2611
        public override string GetCurrentActivityText()
        {
            string Result;
            if (FProduction.FQueue.Count == 0)
            {
                Result = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.ProducingMerchandiseRStr);
            }
            else
            {
                var queue = FProduction.FQueue.GetItem(0);
                Result = FProduction.GetProductionNameProgressive(queue);
            }

            return Result;
        }

        // City.pas:876
        public override bool ValidateMap(StringCollection Messages)
        {
            bool Result = base.ValidateMap(Messages);
            var noname = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.World.Structures.City.NonameRStr);
            if (CompareText(FName, noname) == 0 || IsEmpty(FName))
            {
                Messages.Add(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.World.Structures.City.CityWarningRStr) + ": " + Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.World.Structures.City.NoNameSpecifiedRStr));
                Result = false;
            }

            return Result;
        }

        // City.pas:1162
        public override void UpdateAITarget(in AITargetInfo Info, ref AITargetData Data)
        {
            int Amount;
            int Priority;
            if (!FMode.Contains(ModeElement.Razed))
            {
                switch (Info.Mode)
                {
                    case AITargetMode.Attack:
                    case AITargetMode.Raid:
                        if (Info.Player != FPlayer)
                        {
                            Priority = Data.Priority;
                            if (FPlayer > 0)
                            {
                                if (Aow.Game.Magic.Abilities.AbilityRules.ValidAttackRelation(Info.Player, Info.IndependentRelation, FPlayer, FRace))
                                {
                                    Priority = 100 + (GetSize() - 1) * 10;
                                }
                            }
                            else
                            {
                                if (FPlayer == 0)
                                {
                                    var playerRelation = Aow.Game.World.Map.AoWHSMap.Current.FRaces.GetRace(FRace).GetPlayerRelation(Info.Player);
                                    if (Set<global::Aow.Game.Engine.Status>.Of(global::Aow.Game.Engine.Status.Polite, global::Aow.Game.Engine.Status.Friendly).Contains(playerRelation))
                                    {
                                        Amount = JoinAmount(Info.Player);
                                        if (Amount != -1)
                                        {
                                            Data.Cost = Amount;
                                            Priority = 100 + (GetSize() - 1) * 10;
                                            Data.TargetType = AITargetType.Negotiate;
                                        }
                                        else
                                        {
                                            Priority = 100 + (GetSize() - 1) * 10;
                                        }
                                    }
                                    else
                                    {
                                        Priority = 100 + (GetSize() - 1) * 10;
                                    }
                                }
                                else
                                {
                                    if (FMode.Contains(ModeElement.Warmode) && Priority != 0)
                                    {
                                        Priority = 100 + (GetSize() - 1) * 10;
                                    }
                                }
                            }

                            if (Priority > Data.Priority)
                            {
                                Data.Priority = Priority;
                            }
                        }

                        break;
                    case AITargetMode.Produce:
                        if (Info.Player == FPlayer)
                        {
                            Data.Priority = Data.Priority | 2;
                        }

                        break;
                    case AITargetMode.Fortify:
                        bool condition = Info.Player == FPlayer;
                        if (condition)
                        {
                            var playerRelation = Aow.Game.World.Map.AoWHSMap.Current.FRaces.GetRace(FRace).GetPlayerRelation(Info.Player);
                            condition = Set<global::Aow.Game.Engine.Status>.Of(global::Aow.Game.Engine.Status.Polite, global::Aow.Game.Engine.Status.Friendly).Contains(playerRelation);
                        }

                        if (condition && GetActiveAction() != CityAction.Migrate)
                        {
                            var size = GetSize();
                            Data.Priority = 500 + size * 100 + FWallType * 45;
                            if (FEnchantments.Contains(CityEnchantmentID.AntiMagicShell))
                            {
                                Data.Priority += 150;
                            }

                            if (GetLhx() != 0)
                            {
                                Data.Priority += 50;
                            }
                        }

                        break;
                    case AITargetMode.Defend:
                        if (Info.Player == FPlayer)
                        {
                            var size = GetSize();
                            Data.Priority = 50 + size * 20;
                        }

                        break;
                }
            }
        }

        // City.pas:867
        public override void Update()
        {
            if (FUpdateCount == 0)
            {
                UpdateIncome();
                base.Update();
            }
        }

        // City.pas:852
        public override bool CanDblClick() => Aow.Game.World.Map.AoWHSMap.Current.FSeatedPlayer == FPlayer && !GetRazed() && !GetBuilding();

        // City.pas:1639
        public override void GetRebuildInfo(ref int Cost, ref int Time)
        {
            Cost = GetSize() * 25;
            Time = GetSize() + 1;
        }

        // City.pas:1092
        public override void SetPlayer(sbyte Player)
        {
            if (Player != FPlayer)
            {
                FProduction.PlayerChanged();
                ResetEnchantments();
                if (FPlayer != -1)
                {
                    Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FProductionControls.Remove(FProduction);
                }

                base.SetPlayer(Player);
                if (GetActiveAction() == CityAction.Migrate)
                {
                    FProduction.ExecuteCancelProduction();
                    FProduction.FQueue.GetItem(0)?.Dispose();
                }

                ValidateAICityAction();
                UpdateAIMigration();
                if (FPlayer != -1)
                {
                    Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FProductionControls.Add(FProduction);
                }
            }
        }

        // City.pas:2071
        public override void UpdateFlagID()
        {
            if (!FStatus.Contains(Aow.Game.World.MapObjectStatusElement.EditMode) && (!FMode.Contains(ModeElement.Razed) || GetBuilding()) && !FMode.Contains(ModeElement.Warmode))
            {
                if (FPlayer > 0)
                {
                    FFlagID = (byte)(Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FPlayerRace + (FUpgradeLevel - 1) * 64);
                }
                else
                {
                    FFlagID = (byte)(32 + (FUpgradeLevel - 1) * 64);
                }
            }
            else
            {
                FFlagID = 63;
            }
        }

        // City.pas:1792
        public override int VisibilityRange()
        {
            int Result = 6;
            if (GetLhx() != 0 || FPlayer > 0 && (((byte)Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FMagicControl.FEnemyEnchantments.Image) & 8) != 0)
            {
                Result = (int)(((uint)(Result + 1)) >> 1);
            }

            return Result;
        }

        // City.pas:2225
        public override void FinalizeCombat(global::Aow.Game.Combat.Combat Combat)
        {
            if (Combat.FCombatResult == CombatResult.Victory || Combat.FCombatResult == CombatResult.NoDefenders)
            {
                var combatData = Combat.FCombatData;
                SetPlayer(combatData.FPlayers.GetPlayers(combatData.FAttackerPlayer).FPlayer);

                UpdatePlayer();
            }
        }

        // City.pas:2214
        public override string GetCombatEventText(global::Aow.Game.Combat.Combat Combat)
        {
            string Result = "";
            if (!GetRazed())
            {
                if (Combat.FCombatResult == CombatResult.Victory || Combat.FCombatResult == CombatResult.NoDefenders)
                {
                    var cityBattleWon = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.World.Structures.City.CityBattleWonRStr);
                    var name = GetName();
                    Result = RtlUnit.Format(cityBattleWon, [name]);
                }
                else
                {
                    if (Combat.FCombatResult == CombatResult.Defeat)
                    {
                        var cityBattleLost = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.World.Structures.City.CityBattleLostRStr);
                        var name = GetName();
                        Result = RtlUnit.Format(cityBattleLost, [name]);
                    }
                    else
                    {
                        if (Combat.FCombatResult != CombatResult.CannotBreachWall)
                        {
                            var cityBattleDrawn = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.World.Structures.City.CityBattleDrawnRStr);
                            var name = GetName();
                            Result = RtlUnit.Format(cityBattleDrawn, [name]);
                        }
                    }
                }
            }

            return Result;
        }

        // City.pas:1062
        public override void StructureCaptured(sbyte OldPlayer, sbyte NewPlayer)
        {
            CityProductionEventLog Log;
            if (!GetRazed() && !GetBuilding() && OldPlayer == 0 && !Aow.Game.World.Map.AoWHSMap.Current.Status.Contains(HSMapStatusElement.Editting) && FStatus.Contains(Aow.Game.World.MapObjectStatusElement.Activated) && !((PlayerStructureResource)FResource).FClaimed)
            {
                Log = new CityProductionEventLog(null);
                Log.FPlayer = NewPlayer;
                var editName = EditName();
                Log.FText = RtlUnit.Format(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.StructureCapturedRStr), [editName]);
                Log.FXhx = GetXhx();
                Log.FYhx = GetYhx();
                Log.FLhx = GetLhx();
                var yhx = GetYhx();
                var lhx = GetLhx();
                var xhx = GetXhx();
                Aow.Game.Events.EventLog.DistributeLocationEventLog(Log, FPlayer, xhx, yhx, lhx, false);
                Log.Release();
            }
        }

        // City.pas:2271
        public override MoveAction GetMoveAction(sbyte Player, MoveAction Action)
        {
            MoveAction Result = Action;
            if (FPlayer == 0)
            {
                if (Set<MoveAction>.Of(MoveAction.Normal, MoveAction.Attack).Contains(Action) && !GetRazed() && !FMode.Contains(ModeElement.Warmode))
                {
                    if (JoinAmount(Player) != -1)
                    {
                        Result = MoveAction.Negotiate;
                    }
                    else
                    {
                        Result = MoveAction.Attack;
                    }
                }
            }
            else
            {
                if (FPlayer != Player && FPlayer != -1)
                {
                    Result = MoveAction.Attack;
                }
            }

            return Result;
        }

        // City.pas:1285
        public override bool UpdatePlayer()
        {
            bool MultiplePlayers;
            MapLevel MapLevel;
            int Count;
            HNWalkData Walk = default;
            ArmyHS Army;
            sbyte Player;
            if (!(FMode.Contains(ModeElement.Razed) && !GetBuilding()))
            {
                MultiplePlayers = false;
                Player = -1;
                Count = GetTerrainCount();
                MapLevel = (global::Aow.Game.World.Map.MapLevel)Owner.Owner;
                Aow.Game.World.Movement.HNWalkData.InitHNWalk(ref Walk, FXhx, FYhx, 0);
                while (Walk.HN < Count && !MultiplePlayers)
                {
                    if (FResource.TerrainList.GetTerrain(Walk.HN) >= 0 && Walk.X >= 0 && Walk.Y >= 0 && Walk.X < MapLevel.Width && Walk.Y < MapLevel.Height)
                    {
                        Army = (ArmyHS)MapLevel.GetField(Walk.X, Walk.Y).FindOwnedChild(131607);
                        if (Army != null)
                        {
                            if (Player == -1)
                            {
                                Player = Army.FArmy.FPlayer;
                            }
                            else
                            {
                                MultiplePlayers = Army.FArmy.FPlayer != Player;
                            }
                        }
                    }

                    Aow.Game.World.Movement.HNWalkData.WalkToNextHN(ref Walk);
                }

                if (MultiplePlayers)
                {
                    if (!FMode.Contains(ModeElement.Warmode))
                    {
                        FMode = FMode + Set<ModeElement>.Of(ModeElement.Warmode);
                        SetPlayer(-1);
                        UpdateFlagID();
                        Update();
                    }
                }
                else
                {
                    if (FMode.Contains(ModeElement.Warmode))
                    {
                        FMode = FMode - Set<ModeElement>.Of(ModeElement.Warmode);
                        if (Player != -1)
                        {
                            SetPlayer(Player);
                        }
                        else
                        {
                            SetPlayer(0);
                        }

                        UpdateFlagID();
                        Update();
                    }
                    else
                    {
                        if (Player != -1)
                        {
                            SetPlayer(Player);
                        }
                    }
                }
            }

            return true;
        }

        // City.pas:1777
        public override void UpdateIncome()
        {
            if (!GetRazed() && GetActiveAction() != CityAction.Migrate && !GetBuilding())
            {
                SetIncome((byte)(GetCropCount() * 2 + GetSize() * 5));
                if (Set<CityAction>.Of(CityAction.None, CityAction.Trade).Contains(GetActiveAction()))
                {
                    SetIncome((byte)(FIncome + Round((double)FIncome * 0.25)));
                }

                if (FEnchantments.Contains(CityEnchantmentID.GoldRush))
                {
                    SetIncome((byte)(FIncome * 2));
                }
            }
            else
            {
                SetIncome(0);
            }
        }

        // City.pas:1741
        public override void UpdateCrops()
        {
            int Count;
            if (!GetEditMode())
            {
                if (GetRazed())
                {
                    Count = 0;
                }
                else
                {
                    if (Aow.Game.World.Map.AoWHSMap.Current.Status.Contains(HSMapStatusElement.Editting))
                    {
                        Count = ((PlayerCropStructureResource)FResource).FCropLocations.Count;
                    }
                    else
                    {
                        Count = -1;
                    }
                }

                if (Count != -1)
                {
                    SetCropCount(Count);
                }

                UpdateCropSettings();
                UpdateIncome();
            }
        }

        // City.pas:2108
        public override sbyte GetRace() => FRace;

        // City.pas:1211
        public override void CropTrampled(sbyte Player)
        {
            int Race;
            if (Player != FPlayer && Player != 0)
            {
                if (FPlayer > 0)
                {
                    Race = Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FPlayerRace;
                }
                else
                {
                    Race = FRace;
                }

                var playerRelationValue = Aow.Game.World.Map.AoWHSMap.Current.FRaces.GetRace(Race).GetPlayerRelationValue(Player);
                Aow.Game.World.Map.AoWHSMap.Current.FRaces.GetRace(Race).SetPlayerRelationValue(Player, playerRelationValue + -5);
            }
        }

        // City.pas:2097
        public virtual void SetRace(sbyte Race)
        {
            if (Race != FRace)
            {
                FRace = Race;
                if (FRace >= 0)
                {
                    SetDefaultProductions();
                }

                UpdateCrops();
                UpdateImages();
            }
        }

        // City.pas:857
        public CityAction GetActiveAction() => GetActiveOrder()?.CityAction ?? CityAction.None;

        // The order the city carries out, null when it produces nothing.
        public ProductionOrder GetActiveOrder() => FProduction.FStatus == Aow.Game.World.Structures.ProductionControl.Status.Producing ? FProduction.FQueue.GetItem(0).Order : null;

        // City.pas:894
        public void UpdateActiveEnchantments()
        {
            FEnchantments = Set<CityEnchantmentID>.Of();
            for (int I = FEnchantmentList.Count - 1; I >= 0; I--)
            {
                var enchantment = FEnchantmentList.GetEnchantments(I);
                FEnchantments = FEnchantments + Set<CityEnchantmentID>.Of(((CityEnchantment)enchantment).FEnchantmentID);
            }
        }

        // City.pas:902
        public void RegisterEnchantments()
        {
            for (int I = FEnchantmentList.Count - 1; I >= 0; I--)
            {
                var enchantment = FEnchantmentList.GetEnchantments(I);
                FEnchantmentList.GetEnchantments(I).Activate(enchantment.FPlayerIndex);
            }

            UpdateActiveEnchantments();
        }

        // City.pas:910
        public void UnregisterEnchantments()
        {
            for (int I = FEnchantmentList.Count - 1; I >= 0; I--)
            {
                var enchantment = FEnchantmentList.GetEnchantments(I);
                FEnchantmentList.GetEnchantments(I).Deactivate((byte)enchantment.FPlayerIndex);
            }
        }

        // City.pas:917
        public void ResetEnchantments()
        {
            UnregisterEnchantments();
            FEnchantmentList.Clear();
            FEnchantments = Set<CityEnchantmentID>.Of();
        }

        // City.pas:924
        public int AICityEnchantmentUpkeepPriority(CityEnchantmentID ID)
        {
            int Result;
            switch (ID)
            {
                case CityEnchantmentID.FountainOfLife:
                    Result = GetSize() * 25 + 600;
                    break;
                case CityEnchantmentID.GoldRush:
                    Result = GetSize() * 25 + 600;
                    break;
                case CityEnchantmentID.AntiMagicShell:
                    Result = GetSize() * 25 + 700;
                    break;
                case CityEnchantmentID.Warmonger:
                    Result = GetSize() * 25 + 600;
                    break;
                default:
                    Result = 0;
                    break;
            }

            return Result;
        }

        // City.pas:935
        public void ActivateEnchantment(CityEnchantment Enchantment)
        {
            Enchantment.Owner = FEnchantmentList;
            Enchantment.Activate(Enchantment.FPlayerIndex);
            FEnchantments = FEnchantments + Set<CityEnchantmentID>.Of(Enchantment.FEnchantmentID);
            if (Enchantment.FEnchantmentID == CityEnchantmentID.FountainOfLife)
            {
                ExecuteFountainOfLife();
            }

            if (Enchantment.FEnchantmentID == CityEnchantmentID.GoldRush)
            {
                UpdateIncome();
            }

            if (Enchantment.FEnchantmentID == CityEnchantmentID.AntiMagicShell)
            {
                FOverlay.PlaySetupShellAnimation();
            }
        }

        // City.pas:945
        public void DeactivateEnchantment(CityEnchantment Enchantment)
        {
            Enchantment.Owner = null;
            Enchantment.Deactivate((byte)Enchantment.FPlayerIndex);
            UpdateActiveEnchantments();
            if (Enchantment.FEnchantmentID == CityEnchantmentID.GoldRush)
            {
                UpdateIncome();
            }

            Enchantment.Release();
            Update();
        }

        // City.pas:955
        public void ListEnhancementsInfo(StringCollection List)
        {
            List.Add(RtlUnit.Format(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.World.Structures.City.UpgradeLevelXRStr), [IntToStr(FUpgradeLevel)]));
            if (FWallType == 1)
            {
                List.Add(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.World.Structures.City.CityWoodenWallRStr));
            }

            if (FWallType == 2)
            {
                List.Add(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.World.Structures.City.CityStoneWallRStr));
            }

            if (FEnchantments.Contains(CityEnchantmentID.FountainOfLife))
            {
                List.Add(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.FountainOfLifeRStr) + ": " + Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameTables.EnchantmentRStr));
            }

            if (FEnchantments.Contains(CityEnchantmentID.GoldRush))
            {
                List.Add(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.GoldRushRStr) + ": " + Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameTables.EnchantmentRStr));
            }

            if (FEnchantments.Contains(CityEnchantmentID.AntiMagicShell))
            {
                List.Add(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.AntiMagicShellRStr) + ": " + Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameTables.EnchantmentRStr));
            }

            if (FEnchantments.Contains(CityEnchantmentID.Warmonger))
            {
                List.Add(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.WarmongerRStr) + ": " + Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameTables.EnchantmentRStr));
            }
        }

        // City.pas:970
        public void StartTownQuake()
        {
            FMode = FMode + Set<ModeElement>.Of(ModeElement.TownQuake);
        }

        // City.pas:973
        public void StopTownQuake()
        {
            FMode = FMode - Set<ModeElement>.Of(ModeElement.TownQuake);
        }

        // City.pas:976
        public void ExecuteAnarchy()
        {
            FMode = FMode + Set<ModeElement>.Of(ModeElement.Anarchy);
            CheckRebellion();
            FMode = FMode - Set<ModeElement>.Of(ModeElement.Anarchy);
        }

        // City.pas:983
        public bool ExecuteFountainOfLife()
        {
            ArmyHSList Armies;
            ImageSequenceAnimation Animation;
            bool PlaySound;
            Armies = new ArmyHSList(null);
            ListArmies(Armies);
            bool Result = false;
            PlaySound = false;
            for (int I = 0, last = Armies.Count - 1; I <= last; I++)
            {
                if (Armies.GetArmyHS(I).FArmy.HealUnits(255))
                {
                    var lhx = Armies.GetArmyHS(I).GetLhx();
                    var yhx = Armies.GetArmyHS(I).GetYhx();
                    var xhx = Armies.GetArmyHS(I).GetXhx();
                    if (Aow.Game.World.Map.AoWHSMap.Current.VisibleForPlayer(xhx, yhx, lhx, Aow.Game.World.Map.AoWHSMap.Current.FSeatedPlayer))
                    {
                        Animation = new ImageSequenceAnimation(null);
                        Animation.SetImageSequence(Aow.Game.Engine.AoWHSSet.Current.FAbilityControl.GetAbility(47).FImageSequences.ImageSequences[0]);
                        Animation.FAnimationLevel = 27;
                        var lhx2 = Armies.GetArmyHS(I).GetLhx();
                        var yhx2 = Armies.GetArmyHS(I).GetYhx();
                        var xhx2 = Armies.GetArmyHS(I).GetXhx();
                        Aow.Game.World.Map.AoWHSMap.Current.PlaceHX(Animation, xhx2, yhx2, lhx2);
                        bool condition = PlaySound || Armies.GetArmyHS(I).FArmy.FPlayer == Aow.Game.World.Map.AoWHSMap.Current.FSeatedPlayer;
                        if (!condition)
                        {
                            var lhx3 = Armies.GetArmyHS(I).GetLhx();
                            var yhx3 = Armies.GetArmyHS(I).GetYhx();
                            var xhx3 = Armies.GetArmyHS(I).GetXhx();
                            condition = Aow.Game.World.Map.AoWHSMap.Current.Watching(xhx3, yhx3, lhx3, 1);
                        }

                        PlaySound = condition;
                    }

                    Result = true;
                }
            }

            if (PlaySound)
            {
                Aow.Game.Engine.AoWHSSet.Current.FAbilityControl.GetAbility(47).FSFXLibrary.Play(0);
            }

            Armies?.Dispose();
            return Result;
        }

        // City.pas:1015
        public void UpdateAIMigration()
        {
            MigrationList Selection;
            int Value;
            int BestIndex;
            int BestValue;
            ProductionItem Item;
            if (Aow.Game.World.Map.AoWHSMap.Current.FDay != 0 && !Aow.Game.World.Map.AoWHSMap.Current.Status.Contains(HSMapStatusElement.Editting))
            {
                if (FPlayer > 0)
                {
                    bool condition = Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FPlayerType != PlayerType.Human && (Set<ModeElement>.Of(ModeElement.Warmode, ModeElement.Razed) & FMode) == Set<ModeElement>.Of();
                    if (condition)
                    {
                        var activeAction = GetActiveAction();
                        condition = Set<CityAction>.Of(CityAction.None, CityAction.Trade).Contains(activeAction);
                    }

                    bool test = condition;
                    if (test)
                    {
                        var playerRelation = Aow.Game.World.Map.AoWHSMap.Current.FRaces.GetRace(FRace).GetPlayerRelation(FPlayer);
                        test = Set<global::Aow.Game.Engine.Status>.Of(global::Aow.Game.Engine.Status.Hostile, global::Aow.Game.Engine.Status.Wary, global::Aow.Game.Engine.Status.Neutral).Contains(playerRelation);
                    }

                    if (test)
                    {
                        Selection = CreateMigrationSelection();
                        BestValue = 0;
                        BestIndex = -1;
                        for (int I = 0, last = Selection.Count - 1; I <= last; I++)
                        {
                            var itemRelation = Selection.GetItems(I);
                            if (Set<global::Aow.Game.Engine.Status>.Of(global::Aow.Game.Engine.Status.Polite, global::Aow.Game.Engine.Status.Friendly).Contains(itemRelation.FRelation))
                            {
                                var itemRace = Selection.GetItems(I);
                                bool passed = Aow.Game.World.Map.AoWHSMap.Current.FPlayers.RaceToPlayer(itemRace.FRace) == -1;
                                if (!passed)
                                {
                                    var migrationItem = Selection.GetItems(I);
                                    var player = Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(FPlayer);
                                    passed = migrationItem.FRace == player.FPlayerRace;
                                }

                                if (passed)
                                {
                                    Value = 10;
                                }
                                else
                                {
                                    Value = 5;
                                }
                            }
                            else
                            {
                                Value = 0;
                            }

                            Value += Selection.GetItems(I).FTime;
                            if (Value > BestValue || Value == BestValue && Aow.Game.World.Map.AoWHSMap.Current.Random(2) == 1)
                            {
                                BestValue = Value;
                                BestIndex = I;
                            }
                        }

                        if (BestIndex != -1)
                        {
                            FProduction.ExecuteDeleteProductions();
                            Item = new ProductionItem(null);
                            Item.Order = new Migrate(Selection.GetItems(BestIndex).FRace);
                            FProduction.ExecuteAddProduction(Item);
                            Item?.Dispose();
                        }

                        Selection?.Dispose();
                    }
                }
                else
                {
                    bool condition = FPlayer == 0;
                    if (condition)
                    {
                        var activeAction = GetActiveAction();
                        condition = Set<CityAction>.Of(CityAction.Migrate, CityAction.Loot).Contains(activeAction);
                    }

                    if (condition)
                    {
                        FProduction.ExecuteDeleteProductions();
                    }
                }
            }
        }

        // City.pas:1081
        public void ValidateAICityAction()
        {
            bool Cancel;
            bool condition = FPlayer > 0 && Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FPlayerType != PlayerType.Human && (Set<ModeElement>.Of(ModeElement.Warmode, ModeElement.Razed) & FMode) == Set<ModeElement>.Of();
            if (condition)
            {
                var activeAction = GetActiveAction();
                condition = !Set<CityAction>.Of(CityAction.None, CityAction.Trade).Contains(activeAction);
            }

            if (condition)
            {
                var playerRelation = Aow.Game.World.Map.AoWHSMap.Current.FRaces.GetRace(FRace).GetPlayerRelation(FPlayer);
                bool contains = Set<global::Aow.Game.Engine.Status>.Of(global::Aow.Game.Engine.Status.Hostile, global::Aow.Game.Engine.Status.Wary, global::Aow.Game.Engine.Status.Neutral).Contains(playerRelation);
                if (contains)
                {
                    bool test = false;
                    if (GetActiveOrder() is Migrate migrate)
                    {
                        var playerRelation2 = Aow.Game.World.Map.AoWHSMap.Current.FRaces.GetRace(migrate.Race).GetPlayerRelation(FPlayer);
                        test = Set<global::Aow.Game.Engine.Status>.Of(global::Aow.Game.Engine.Status.Polite, global::Aow.Game.Engine.Status.Friendly).Contains(playerRelation2);
                    }

                    contains = !test;
                }

                Cancel = contains;
                if (Cancel)
                {
                    FProduction.ExecuteDeleteProductions();
                }
            }
        }

        // City.pas:1222
        public bool MoveArmiesOutOfCity(Set<MoveOutOfCitySelectionElement> Selection)
        {
            MapLevel MapLevel;
            ArmyHS Army;
            int Count;
            HNWalkData Walk = default;
            bool Result = true;
            Count = GetTerrainCount();
            MapLevel = (global::Aow.Game.World.Map.MapLevel)Owner.Owner;
            // { This eliminated copy preserves the original saved registers. }
            Aow.Game.World.Movement.HNWalkData.InitHNWalk(ref Walk, FXhx, FYhx, 0);
            while (Walk.HN < Count && Result)
            {
                if (FResource.TerrainList.GetTerrain(Walk.HN) >= 0 && Walk.X >= 0 && Walk.Y >= 0 && Walk.X < MapLevel.Width && Walk.Y < MapLevel.Height)
                {
                    Army = (ArmyHS)MapLevel.GetField(Walk.X, Walk.Y).FindOwnedChild(131607);
                    if (Army != null && (!Selection.Contains(MoveOutOfCitySelectionElement.SkipOwnPlayer) || Army.FArmy.FPlayer != FPlayer))
                    {
                        var lhx = GetLhx();
                        Result = Army.PlaceEx((sbyte)Walk.X, (sbyte)Walk.Y, lhx, Set<ArmyPlaceModeElement>.Of(ArmyPlaceModeElement.Safe, ArmyPlaceModeElement.ScanArea));
                    }
                }

                Aow.Game.World.Movement.HNWalkData.WalkToNextHN(ref Walk);
            }

            return Result;
        }

        // City.pas:1256
        public int GetSize() => ((global::Aow.Game.World.Structures.Resource)FResource).FSize;

        // City.pas:1259
        public int GetObtainUnitsValue()
        {
            int Count;
            MapLevel MapLevel;
            HNWalkData Walk = default;
            ArmyHS Army;
            ref var walk = ref Walk;
            int Result = 0;
            Count = GetTerrainCount();
            MapLevel = (global::Aow.Game.World.Map.MapLevel)Owner.Owner;
            var yhx = GetYhx();
            var xhx = GetXhx();
            Aow.Game.World.Movement.HNWalkData.InitHNWalk(ref Walk, xhx, yhx, 0);
            while (walk.HN < Count)
            {
                if (FResource.TerrainList.GetTerrain(walk.HN) >= 0 && walk.X >= 0 && walk.Y >= 0 && walk.X < MapLevel.Width && walk.Y < MapLevel.Height)
                {
                    Army = (ArmyHS)MapLevel.GetField(walk.X, walk.Y).FindOwnedChild(131607);
                    if (Army != null)
                    {
                        Result += Army.FArmy.ObtainValue(255);
                    }
                }

                Aow.Game.World.Movement.HNWalkData.WalkToNextHN(ref Walk);
            }

            return Result;
        }

        // City.pas:1348
        public int UpgradeCost()
        {
            var size = GetSize();
            return Aow.Game.World.Structures.City.UpgradePrices[size - 1];
        }

        // City.pas:1351
        public int UpgradeTime() => 3;

        // City.pas:1356
        public bool CanUpgrade(bool Queue, ref string Reason)
        {
            bool Result;
            int Level;
            Level = FUpgradeLevel;
            if (Queue)
            {
                for (int I = 0, last = FProduction.FQueue.Count - 1; I <= last; I++)
                {
                    var queue = FProduction.FQueue.GetItem(I);
                    if (queue.Order is Aow.Game.World.Structures.Upgrade)
                    {
                        Level += 1;
                    }
                }
            }

            if (Level < GetSize())
            {
                Result = true;
            }
            else
            {
                Result = false;
                Reason = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.World.Structures.City.MaxUpgradeLevelReachedRStr);
            }

            return Result;
        }

        // City.pas:1371
        public bool Upgrade(bool Queue)
        {
            bool Result;
            ProductionItem Item;
            string Reason = "";
            if (CanUpgrade(Queue, ref Reason) && !Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).GetBusy())
            {
                Item = new ProductionItem(null);
                Item.Order = new Aow.Game.World.Structures.Upgrade();
                FProduction.Produce(Item, Queue);
                Item?.Dispose();
                Result = true;
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // City.pas:1386
        public int FortifyCost()
        {
            int Result;
            switch (FWallType)
            {
                case 0:
                    var size = GetSize();
                    Result = Aow.Game.World.Structures.City.WoodWallPrices[size - 1];
                    break;
                case 1:
                    var size2 = GetSize();
                    Result = Aow.Game.World.Structures.City.StoneWallPrices[size2 - 1];
                    break;
                default:
                    Result = -1;
                    break;
            }

            return Result;
        }

        // City.pas:1395
        public int FortifyTime() => 2;

        // City.pas:1400
        public bool CanFortify(bool Queue, ref string Reason)
        {
            bool Result;
            int Level;
            Level = FWallType;
            if (Queue)
            {
                for (int I = 0, last = FProduction.FQueue.Count - 1; I <= last; I++)
                {
                    var queue = FProduction.FQueue.GetItem(I);
                    if (queue.Order is Aow.Game.World.Structures.Fortify)
                    {
                        Level += 1;
                    }
                }
            }

            if (Level != 2)
            {
                Result = true;
            }
            else
            {
                Result = false;
                Reason = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.World.Structures.City.MaxFortifyLevelReachedRStr);
            }

            return Result;
        }

        // City.pas:1415
        public bool Fortify(bool Queue)
        {
            bool Result;
            ProductionItem Item;
            string Reason = "";
            if (CanFortify(Queue, ref Reason) && !Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).GetBusy())
            {
                Item = new ProductionItem(null);
                Item.Order = new Aow.Game.World.Structures.Fortify();
                FProduction.Produce(Item, Queue);
                Item?.Dispose();
                Result = true;
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // City.pas:1430
        public int ProductionCost(int Index) => FProduction.UnitProductionGold(Index);

        // City.pas:1435
        public int ProductionTime(int Index) => FProduction.UnitProductionTime(Index);

        // City.pas:1440
        public bool CanProduce(int Index, bool Queue, ref string Reason) => FProduction.CanProduceUnit(Index, Queue, ref Reason);

        // City.pas:1445
        public bool Produce(int Index, bool Queue) => FProduction.ProduceUnit(Index, Queue);

        // City.pas:1450
        public int ResearchCost(int Index) => FProduction.UnitResearchGold(Index);

        // City.pas:1455
        public int ResearchTime(int Index) => FProduction.UnitResearchTime(Index);

        // City.pas:1460
        public bool CanResearch(int Index, bool Queue, ref string Reason) => FProduction.CanResearchUnit(Index, Queue, ref Reason);

        // City.pas:1465
        public bool Research(int Index, bool Queue) => FProduction.ResearchUnit(Index, Queue);

        // City.pas:1470
        public bool CanTrade(bool Queue, ref string Reason) => true;

        // City.pas:1475
        public bool Trade(bool Queue)
        {
            bool Result;
            ProductionItem Item;
            string Reason = "";
            if (CanTrade(Queue, ref Reason) && !Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).GetBusy())
            {
                Item = new ProductionItem(null);
                if (Queue)
                {
                    Item.Order = new Aow.Game.World.Structures.Trade(1);
                }
                else
                {
                    Item.Order = new Aow.Game.World.Structures.Trade(-1);
                }

                FProduction.Produce(Item, Queue);
                Item?.Dispose();
                Result = true;
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // City.pas:1491
        public int LootTime() => GetSize();

        // City.pas:1496
        public bool CanLoot(ref string Reason) => !FMode.Contains(ModeElement.Razed);

        // City.pas:1499
        public bool Loot()
        {
            bool Result;
            ProductionItem Item;
            string Reason = "";
            if (CanLoot(ref Reason) && !Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).GetBusy())
            {
                Item = new ProductionItem(null);
                Item.Order = new Aow.Game.World.Structures.Loot();
                FProduction.Produce(Item, false);
                Item?.Dispose();
                Result = true;
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // City.pas:1514
        public int MigrationTime(sbyte Race)
        {
            int Result;
            string Reason = "";
            HNWalkData Walk = default;
            MapLevel MapLevel;
            int Radius;
            City City;
            if (CanMigrate(Race, ref Reason))
            {
                if (Race != FRace)
                {
                    Radius = -1;
                    MapLevel = (global::Aow.Game.World.Map.MapLevel)Owner.Owner;
                    Aow.Game.World.Movement.HNWalkData.InitHNWalk(ref Walk, FXhx, FYhx, 1);
                    while (Walk.Radius <= 20 && Radius == -1)
                    {
                        if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < MapLevel.Width && Walk.Y < MapLevel.Height)
                        {
                            City = (global::Aow.Game.World.Structures.City)MapLevel.GetField(Walk.X, Walk.Y).FindNoneTransparentHS(typeof(global::Aow.Game.World.Structures.City));
                            if (City != null && City.FRace == Race)
                            {
                                Radius = Walk.Radius;
                            }
                        }

                        Aow.Game.World.Movement.HNWalkData.WalkToNextHN(ref Walk);
                    }

                    if (Radius != -1)
                    {
                        Result = Radius / 10 + 3;
                    }
                    else
                    {
                        Result = 5;
                    }
                }
                else
                {
                    Result = 0;
                }
            }
            else
            {
                Result = -1;
            }

            return Result;
        }

        // City.pas:1544
        public bool CanMigrate(sbyte Race, ref string Reason)
        {
            bool Result;
            int I;
            sbyte Player;
            if (FRace != Race)
            {
                Result = Aow.Game.World.Structures.City.RaceInCity(Race, FPlayer);
                I = Aow.Game.World.Map.AoWHSMap.Current.FPlayers.Count;
                while (I != 1 && !Result)
                {
                    I -= 1;
                    Player = (sbyte)I;
                    Result = Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetDiplomaticRelation((sbyte)(Player << 0), FPlayer) == DiplomaticRelation.Alliance && Aow.Game.World.Structures.City.RaceInCity(Race, Player);
                }
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // City.pas:1561
        public bool Migrate(sbyte Race)
        {
            bool Result;
            ProductionItem Item;
            string Reason = "";
            if (CanMigrate(Race, ref Reason) && !Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).GetBusy())
            {
                Item = new ProductionItem(null);
                Item.Order = new Aow.Game.World.Structures.Migrate(Race);
                FProduction.Produce(Item, false);
                Item?.Dispose();
                Result = true;
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // City.pas:1577
        public MigrationItem CreateMigrationItem(sbyte Race)
        {
            MigrationItem Result;
            string Reason = "";
            if (CanMigrate(Race, ref Reason))
            {
                Result = new MigrationItem(null);
                Result.FRace = Race;
                Result.FTime = MigrationTime(Race);
                Result.FRelation = Aow.Game.World.Map.AoWHSMap.Current.FRaces.GetRace(Race).GetPlayerRelation(FPlayer);
            }
            else
            {
                Result = null;
            }

            return Result;
        }

        // City.pas:1589
        public MigrationList CreateMigrationSelection()
        {
            MigrationItem Item;
            MigrationList Result = new MigrationList(null);
            for (int I = 0, last = Aow.Game.World.Map.AoWHSMap.Current.FRaces.Count - 1; I <= last; I++)
            {
                if (Aow.Game.World.Map.AoWHSMap.Current.FRaces.GetRace(I).FEnabled)
                {
                    Item = CreateMigrationItem((sbyte)I);
                    if (Item != null && Set<global::Aow.Game.Engine.Status>.Of(global::Aow.Game.Engine.Status.Neutral, global::Aow.Game.Engine.Status.Polite, global::Aow.Game.Engine.Status.Friendly).Contains(Item.FRelation))
                    {
                        Item.Owner = Result;
                    }
                    else
                    {
                        Item?.Dispose();
                    }
                }
            }

            return Result;
        }

        // City.pas:1645
        public void CheckRebellion()
        {
            CivilStatus Status;
            int Chance;
            CityRebellionEventLog Log;
            UnitList Units;
            if (FPlayer != 0)
            {
                Status = GetCivilStatus();
                if (Set<CivilStatus>.Of(CivilStatus.Unruly, CivilStatus.Unrest).Contains(Status) && !GetRazed())
                {
                    if (Status == CivilStatus.Unruly)
                    {
                        Chance = 50;
                    }
                    else
                    {
                        Chance = 10;
                    }

                    if (Chance >= Aow.Game.World.Map.AoWHSMap.Current.Random(100))
                    {
                        MoveArmiesOutOfCity(Set<MoveOutOfCitySelectionElement>.Of());
                        Log = new CityRebellionEventLog(null);
                        Log.FPlayer = FPlayer;
                        Log.FCityName = GetName();
                        Log.FXhx = GetXhx();
                        Log.FYhx = GetYhx();
                        Log.FLhx = GetLhx();
                        var yhx = GetYhx();
                        var lhx = GetLhx();
                        var xhx = GetXhx();
                        Aow.Game.Events.EventLog.DistributeLocationEventLog(Log, FPlayer, xhx, yhx, lhx, false);
                        Log.Release();
                        SetPlayer(0);
                        Units = new UnitList(null);
                        GenerateRebelUnits(Units);
                        for (int I = Units.Count - 1; I >= 0; I--)
                        {
                            var lhx2 = GetLhx();
                            var yhx2 = GetYhx();
                            var xhx2 = GetXhx();
                            Units.GetUnit(I).Place(xhx2, yhx2, lhx2, 0, Set<UnitPlaceModeElement>.Of());
                        }

                        Units?.Dispose();
                        FProduction.ExecuteDeleteProductions();
                    }
                }
            }
        }

        // City.pas:1681
        public void GenerateDefenders()
        {
            int Value;
            ArmyHSList Armies;
            if (!GetRazed() && FDefendersMode != DefendersGenerateMode.None && !FMode.Contains(ModeElement.Warmode) && FPlayer == 0)
            {
                Armies = new ArmyHSList(null);
                ListArmies(Armies);
                if (FDefendersMode == DefendersGenerateMode.Random)
                {
                    FDefendersMode = (DefendersGenerateMode)(Random(3) + 2);
                }

                Value = ((int)FDefendersMode - 2) * 25 + 25;
                Value += (GetSize() - 1) * Value / 2;
                for (int I = 0, last = Armies.Count - 1; I <= last; I++)
                {
                    Value -= Armies.GetArmyHS(I).FArmy.ObtainValue(255);
                }

                if (Value > 0)
                {
                    RandSeed = Aow.Game.World.Map.AoWHSMap.Current.Random(1048575);
                    var raceResource = Aow.Game.Engine.AoWHSSet.Current.FRaceResources.GetRaceResource(FRace);
                    var lhx = GetLhx();
                    var yhx = GetYhx();
                    Aow.Game.Magic.Abilities.AbilityRules.PlaceRandomUnits(GetXhx(), yhx, lhx, 0, raceResource.FUnitIndexes, FUpgradeLevel, Value, Set<UnitType>.Of(UnitType.Humanoid, UnitType.Creature));
                }

                Armies?.Dispose();
            }
        }

        // City.pas:1755
        public void ListIncomeInfo(StringCollection Names, StringCollection Values)
        {
            int Income;
            Income = GetSize() * 5;
            Names.Add(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.BaseIncomeRStr));
            Values.Add(IntToStr(Income));
            Income += GetCropCount() * 2;
            var cropCount = GetCropCount();
            Names.Add(RtlUnit.Format(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.World.Structures.City.XCropFieldsRStr), [IntToStr(cropCount)]));
            Values.Add(IntToStr(GetCropCount() * 2));
            if (Set<CityAction>.Of(CityAction.None, CityAction.Trade).Contains(GetActiveAction()))
            {
                Names.Add(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.ProducingMerchandiseRStr));
                Values.Add(IntToStr(Round((double)Income * 0.25)));
                Income += Round((double)Income * 0.25);
            }

            if (FEnchantments.Contains(CityEnchantmentID.GoldRush))
            {
                Names.Add(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.GoldRushRStr));
                Values.Add(IntToStr(Income));
            }
        }

        // City.pas:1800
        public void SetDefaultProductions()
        {
            FProduction.FUnitProductions.Clear();
            if (FRace >= 0)
            {
                SetUpgradeLevel(FUpgradeLevel);
            }
        }

        // City.pas:1816
        public void ListCivilStatusInfo(StringCollection List)
        {
            CombatPredictor Predictor;
            ArmyHSList Armies;
            UnitList Units;
            AbstractUnit U;
            if (FPlayer != 0 && FRace >= 0)
            {
                var playerRelation = Aow.Game.World.Map.AoWHSMap.Current.FRaces.GetRace(FRace).GetPlayerRelation(FPlayer);
                var status = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameTables.TStatusRStr[(int)playerRelation]);
                List.Add(RtlUnit.Format(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.World.Structures.City.RaceRelationXRStr), [status]));
                if (Aow.Game.World.Structures.City.RaceTerrain[FRace].Penalty.Contains((byte)TerrainType))
                {
                    var terrainPenaltyX = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.World.Structures.City.TerrainPenaltyXRStr);
                    var terrainType = Aow.Game.Engine.AoWHSSet.Current.TerrainTypes[TerrainType];
                    List.Add(RtlUnit.Format(terrainPenaltyX, [terrainType]));
                }

                if (Aow.Game.World.Structures.City.RaceTerrain[FRace].Bonus.Contains((byte)TerrainType))
                {
                    var terrainBonusX = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.World.Structures.City.TerrainBonusXRStr);
                    var terrainType = Aow.Game.Engine.AoWHSSet.Current.TerrainTypes[TerrainType];
                    List.Add(RtlUnit.Format(terrainBonusX, [terrainType]));
                }

                Predictor = new CombatPredictor();
                Armies = new ArmyHSList(null);
                ListArmies(Armies);
                for (int I = 0, last = Armies.Count - 1; I <= last; I++)
                {
                    for (int J = 0, lastJ = Armies.GetArmyHS(I).FArmy.Count - 1; J <= lastJ; J++)
                    {
                        U = Armies.GetArmyHS(I).FArmy.GetUnit(J);
                        if (U.FMoraleValue <= 40)
                        {
                            Predictor.AddOpponent(U, 0);
                        }
                        else
                        {
                            Predictor.AddUnit(U);
                        }
                    }
                }

                Units = new UnitList(null);
                GenerateRebelUnits(Units);
                for (int J = 0, last = Units.Count - 1; J <= last; J++)
                {
                    Predictor.AddOpponent(Units.GetUnit(J), 0);
                }

                if (Predictor.Execute() == CombatResult.Victory)
                {
                    if (Predictor.GetSuperiority() / 4 >= 75)
                    {
                        var strong = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.World.Structures.City.StrongRStr);
                        List.Add(RtlUnit.Format(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.World.Structures.City.OccupationForcesXRStr), [strong]));
                    }
                    else
                    {
                        var average = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.World.Structures.City.AverageRStr);
                        List.Add(RtlUnit.Format(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.World.Structures.City.OccupationForcesXRStr), [average]));
                    }
                }
                else
                {
                    var weak = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.World.Structures.City.WeakRStr);
                    List.Add(RtlUnit.Format(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.World.Structures.City.OccupationForcesXRStr), [weak]));
                }

                Predictor?.Dispose();
                Units?.Dispose();
                Armies?.Dispose();
            }
        }

        // City.pas:1861
        public CivilStatus GetCivilStatus()
        {
            CivilStatus Result;
            global::Aow.Game.Engine.Status Relation;
            int Value;
            int ArmyValue;
            int Bonus;
            CombatPredictor Predictor;
            ArmyHSList Armies;
            UnitList Units;
            AbstractUnit U;
            if (FPlayer != 0 && !FMode.Contains(ModeElement.Warmode))
            {
                if (FMode.Contains(ModeElement.Anarchy))
                {
                    Relation = global::Aow.Game.Engine.Status.Hostile;
                }
                else
                {
                    Relation = Aow.Game.World.Map.AoWHSMap.Current.FRaces.GetRace(FRace).GetPlayerRelation(FPlayer);
                }

                switch (Relation)
                {
                    case global::Aow.Game.Engine.Status.Hostile:
                        Value = -35;
                        break;
                    case global::Aow.Game.Engine.Status.Wary:
                        Value = -25;
                        break;
                    case global::Aow.Game.Engine.Status.Neutral:
                        Value = 0;
                        break;
                    case global::Aow.Game.Engine.Status.Polite:
                        Value = 15;
                        break;
                    case global::Aow.Game.Engine.Status.Friendly:
                        Value = 25;
                        break;
                    default:
                        Value = 0;
                        break;
                }

                Value = Value + FWallType * 5 + FUpgradeLevel * 5;
                if (FRace >= 0)
                {
                    if (Aow.Game.World.Structures.City.RaceTerrain[FRace].Penalty.Contains((byte)TerrainType))
                    {
                        Value += -10;
                    }

                    if (Aow.Game.World.Structures.City.RaceTerrain[FRace].Bonus.Contains((byte)TerrainType))
                    {
                        Value += 10;
                    }

                    Predictor = new CombatPredictor();
                    ArmyValue = 0;
                    Armies = new ArmyHSList(null);
                    ListArmies(Armies);
                    for (int I = 0, last = Armies.Count - 1; I <= last; I++)
                    {
                        for (int J = 0, lastJ = Armies.GetArmyHS(I).FArmy.Count - 1; J <= lastJ; J++)
                        {
                            U = Armies.GetArmyHS(I).FArmy.GetUnit(J);
                            if (U.FMoraleValue <= 40)
                            {
                                Predictor.AddOpponent(U, 0);
                                // { Original anarchy counts disloyal defenders twice. }
                                if (FMode.Contains(ModeElement.Anarchy))
                                {
                                    Predictor.AddOpponent(U, 0);
                                }
                            }
                            else
                            {
                                ArmyValue += U.GetObtainValue();
                                Predictor.AddUnit(U);
                            }
                        }
                    }

                    Armies?.Dispose();
                    if (Set<global::Aow.Game.Engine.Status>.Of(global::Aow.Game.Engine.Status.Hostile, global::Aow.Game.Engine.Status.Wary).Contains(Relation))
                    {
                        Units = new UnitList(null);
                        GenerateRebelUnits(Units);
                        for (int I = 0, last = Units.Count - 1; I <= last; I++)
                        {
                            Predictor.AddOpponent(Units.GetUnit(I), 0);
                            if (FMode.Contains(ModeElement.Anarchy))
                            {
                                Predictor.AddOpponent(Units.GetUnit(I), 0);
                            }
                        }

                        if (Predictor.Execute() == CombatResult.Victory)
                        {
                            Bonus = Predictor.GetSuperiority() / 4;
                            if (Bonus >= 75)
                            {
                                Bonus = 75;
                            }
                            else
                            {
                                if (Bonus >= 26)
                                {
                                    Bonus = 50;
                                }
                                else
                                {
                                    Bonus = 25;
                                }
                            }
                        }
                        else
                        {
                            Bonus = 0;
                        }

                        Value += Bonus;
                        Units?.Dispose();
                    }
                    else
                    {
                        if (Set<global::Aow.Game.Engine.Status>.Of(global::Aow.Game.Engine.Status.Polite, global::Aow.Game.Engine.Status.Friendly).Contains(Relation))
                        {
                            ArmyValue = ArmyValue / (GetSize() * 10);
                            if (ArmyValue > 10)
                            {
                                ArmyValue = 10;
                            }

                            Value += ArmyValue;
                        }
                    }

                    Predictor?.Dispose();
                    if (Value < -30)
                    {
                        Result = CivilStatus.Unruly;
                    }
                    else
                    {
                        if (Value < -10)
                        {
                            Result = CivilStatus.Unrest;
                        }
                        else
                        {
                            if (Value < 10)
                            {
                                Result = CivilStatus.Stable;
                            }
                            else
                            {
                                if (Value < 30)
                                {
                                    if (Set<global::Aow.Game.Engine.Status>.Of(global::Aow.Game.Engine.Status.Hostile, global::Aow.Game.Engine.Status.Wary).Contains(Relation))
                                    {
                                        Result = CivilStatus.Oppressed;
                                    }
                                    else
                                    {
                                        Result = CivilStatus.Content;
                                    }
                                }
                                else
                                {
                                    if (Set<global::Aow.Game.Engine.Status>.Of(global::Aow.Game.Engine.Status.Hostile, global::Aow.Game.Engine.Status.Wary).Contains(Relation))
                                    {
                                        Result = CivilStatus.Slavery;
                                    }
                                    else
                                    {
                                        Result = CivilStatus.Cheerful;
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    Result = CivilStatus.Stable;
                }
            }
            else
            {
                Result = CivilStatus.Stable;
            }

            return Result;
        }

        // City.pas:2034
        public void SetUpgradeLevel(byte Level)
        {
            RaceResource RaceResource = default;
            int UnitLevel = default;
            bool Researched = default;
            if (Level > GetSize())
            {
                Level = (byte)GetSize();
            }

            if (Level < 1)
            {
                Level = 1;
            }

            FUpgradeLevel = Level;
            if (FRace >= 0)
            {
                RaceResource = Aow.Game.Engine.AoWHSSet.Current.FRaceResources.GetRaceResource(FRace);
                for (int I = RaceResource.FUnitIndexes.Count - 1; I >= 0; I--)
                {
                    if (RaceResource.FUnitIndexes[I] != -1)
                    {
                        var unitIndex = RaceResource.FUnitIndexes[I];
                        if (FProduction.FUnitProductions.IndexOf(unitIndex) == -1)
                        {
                            var unitIndex2 = RaceResource.FUnitIndexes[I];
                            var unitResource = Aow.Game.Engine.AoWHSSet.Current.FUnitResources.GetUnitResource(unitIndex2);
                            var size = GetSize();
                            UnitLevel = Aow.Game.World.Structures.City.UnitLevelToCityLevel(unitResource.FLevel, size);
                            if (UnitLevel != -1 && UnitLevel <= FUpgradeLevel)
                            {
                                Researched = UnitLevel == 1 && !(RaceResource.FSpecialUnitIndexes.Count >= 4 && RaceResource.FUnitIndexes[I] == RaceResource.FSpecialUnitIndexes[3]);
                                var unitIndex3 = RaceResource.FUnitIndexes[I];
                                FProduction.FUnitProductions.Add(unitIndex3, Researched);
                            }
                        }
                    }
                }

                Aow.Game.World.Structures.City.SortingCity = this;
                FProduction.FUnitProductions.FChildren.Sort(Aow.Game.World.Structures.City.CompareCityProductions);
            }
        }

        // City.pas:2085
        public void SetName(string Name)
        {
            FName = Name;
            Update();
        }

        // City.pas:2091
        public void SetWallType(byte WallType)
        {
            FWallType = WallType;
            UpdateImages();
        }

        // City.pas:2206
        public void InitCombat(global::Aow.Game.Combat.Combat Combat, ArmyHS Army)
        {
            if (FPlayer == 0)
            {
                var enemiesIndependentRelation = GetEnemiesIndependentRelation(Army.FArmy.FPlayer);
                Combat.FCombatData.FPlayers.GetPlayers(Combat.FCombatData.FDefenderPlayer).FIndependentRelation = (byte)enemiesIndependentRelation;
            }

            var lhx = Army.GetLhx();
            var yhx = Army.GetYhx();
            var xhx = Army.GetXhx();
            Combat.InitDefaultArmies(Army.FArmy, xhx, yhx, lhx);
        }

        // City.pas:2234
        public int GetObtainValue()
        {
            var size = GetSize();
            int Result = Aow.Game.World.Structures.City.UpgradePrices[size - 1] * FUpgradeLevel;
            if (FWallType == 1)
            {
                var size2 = GetSize();
                Result += Aow.Game.World.Structures.City.WoodWallPrices[size2 - 1];
            }

            if (FWallType == 2)
            {
                var size2 = GetSize();
                var size3 = GetSize();
                Result = Result + Aow.Game.World.Structures.City.WoodWallPrices[size2 - 1] + Aow.Game.World.Structures.City.StoneWallPrices[size3 - 1];
            }

            return Result;
        }

        // City.pas:2241
        public int JoinAmount(sbyte Player)
        {
            ArmyHSList Armies;
            int I;
            Army Army;
            bool CanJoin;
            int Value;
            byte Relation;
            byte Selection;
            int Result = 0;
            Armies = new ArmyHSList(null);
            ListArmies(Armies);
            I = Armies.Count;
            CanJoin = true;
            if (I != 0)
            {
                while (I != 0 && CanJoin)
                {
                    I -= 1;
                    Army = Armies.GetArmyHS(I).FArmy;
                    Selection = Army.GetCanJoinSelection(Player);
                    CanJoin = Selection == Army.ValidateSelection(255);
                    if (CanJoin)
                    {
                        Result += Army.JoinAmount(Player, 255);
                    }
                }

                if (CanJoin)
                {
                    Value = GetObtainValue() / 4;
                    Relation = (byte)Aow.Game.World.Map.AoWHSMap.Current.FRaces.GetRace(FRace).GetPlayerRelation(Player);
                    Result += Aow.Game.World.Structures.City.TCity_JoinAmount__JoinPercent[(int)(global::Aow.Game.Engine.Status)Relation] * Value / 100;
                }
                else
                {
                    Result = -1;
                }
            }
            else
            {
                var playerRelation = Aow.Game.World.Map.AoWHSMap.Current.FRaces.GetRace(FRace).GetPlayerRelation(Player);
                if (Set<global::Aow.Game.Engine.Status>.Of(global::Aow.Game.Engine.Status.Polite, global::Aow.Game.Engine.Status.Friendly).Contains(playerRelation))
                {
                    Result = 0;
                }
                else
                {
                    Result = -1;
                }
            }

            Armies?.Dispose();
            return Result;
        }

        // City.pas:2282
        public bool IncommingNegotiateRequest(NegotiateRequestEventLog Request)
        {
            bool Result;
            int Amount;
            if (FPlayer == 0 && !FMode.Contains(ModeElement.Warmode))
            {
                Amount = JoinAmount(Request.FPlayer);
                if (Amount != -1)
                {
                    Request.FObject = NegotiateObject.City;
                    var name = GetName();
                    Request.FText = RtlUnit.Format(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.World.Structures.City.CityJoinHeaderRStr), [name]);
                    Request.FGold = Amount;
                    if (Request.FGold == 0)
                    {
                        var cityFreeJoin = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.World.Structures.City.CityFreeJoinRStr);
                        Request.FJoinMessage.Text = cityFreeJoin;
                    }
                    else
                    {
                        var format = RtlUnit.Format(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.World.Structures.City.CityTributeJoinRStr), [IntToStr(Request.FGold)]);
                        Request.FJoinMessage.Text = format;
                    }

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

            return Result;
        }

        // City.pas:2302
        public bool IncommingNegotiateOffer(NegotiateTE Offer)
        {
            bool Result;
            int Amount;
            ArmyHSList Armies;
            LocationTextEventLog Log;
            if (Offer.FObject == NegotiateObject.City)
            {
                Amount = JoinAmount(Offer.FPlayer);
                if (Offer.FOffer == Aow.Game.Commands.NegotiateTE.NegotiateOffer.OfferBribe && Amount <= Offer.FGold && Amount != -1)
                {
                    if (Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(Offer.FPlayer).FGems >= Offer.FGold)
                    {
                        SetPlayer(Offer.FPlayer);
                        Armies = new ArmyHSList(null);
                        ListArmies(Armies);
                        for (int I = 0, last = Armies.Count - 1; I <= last; I++)
                        {
                            if (Armies.GetArmyHS(I).FArmy.FPlayer == 0)
                            {
                                Armies.GetArmyHS(I).FArmy.SetPlayer(Offer.FPlayer);
                            }
                        }

                        Armies?.Dispose();
                        Log = new LocationTextEventLog(null);
                        Log.FPlayer = FPlayer;
                        var name = GetName();
                        Log.FText = RtlUnit.Format(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.World.Structures.City.XOverthrownRStr), [name]);
                        var lhx = GetLhx();
                        var yhx = GetYhx();
                        Log.SetLocation(GetXhx(), yhx, lhx);
                        var yhx2 = GetYhx();
                        var lhx2 = GetLhx();
                        var xhx = GetXhx();
                        Aow.Game.Events.EventLog.DistributeLocationEventLog(Log, FPlayer, xhx, yhx2, lhx2, false);
                        Log.Release();
                        Offer.Accept();
                    }
                    else
                    {
                        var notEnoughGold = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.NotEnoughGoldRStr);
                        Aow.Game.World.Map.AoWHSMap.Current.ShowMessage(notEnoughGold);
                    }
                }
                else
                {
                    Offer.Reject();
                }

                Result = true;
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // City.pas:2334
        public sbyte GetEnemiesIndependentRelation(sbyte Player)
        {
            UnitUserList Units;
            Units = new UnitUserList(null);
            ListUnits(Units);
            for (int I = Units.Count - 1; I >= 0; I--)
            {
                if (Units.GetUnit(I).CanJoin(Player))
                {
                    Units.DeleteUnit(Units.GetUnit(I));
                }
            }

            // { Original uses the race index as the default relation. }
            sbyte Result = Aow.Game.Units.UnitList.UnitsToIndependentRelation(Units, FRace);
            Units?.Dispose();
            return Result;
        }

        // City.pas:2345
        public sbyte GetIndependentRelation()
        {
            UnitUserList Units;
            Units = new UnitUserList(null);
            ListUnits(Units);
            // { Original uses the race index as the default relation. }
            sbyte Result = Aow.Game.Units.UnitList.UnitsToIndependentRelation(Units, FRace);
            Units?.Dispose();
            return Result;
        }

        // City.pas:2354
        public void ReturnBattleFieldInfo(ref BattleFieldInfo Info)
        {
            int Value;
            DiplomaticRelation Relation;
            if (Info.Requests.Contains(BattleFieldInfoRequestsElement.EffectiveStrength) && FPlayer > 0)
            {
                Value = 5000;
                var playerRelation = Aow.Game.World.Map.AoWHSMap.Current.FRaces.GetRace(FRace).GetPlayerRelation(FPlayer);
                if (Set<global::Aow.Game.Engine.Status>.Of(global::Aow.Game.Engine.Status.Hostile, global::Aow.Game.Engine.Status.Wary).Contains(playerRelation))
                {
                    Value = Value * 2;
                }

                Relation = Aow.Game.Magic.Abilities.AbilityRules.DiplomaticRelation(Info.Player, Info.IndependentRelation, FPlayer, 0);
                if (Info.FriendlyRelations.Contains((byte)(int)Relation))
                {
                    Info.FriendlyStrength -= Value;
                }

                if (Info.EnemyRelations.Contains((byte)(int)Relation))
                {
                    Info.Strength -= Value;
                }
            }
        }

        // City.pas:2367
        public void UpdateVisibleForSeatedPlayer()
        {
            bool Visible;
            Visible = VisibleForPlayer(Aow.Game.World.Map.AoWHSMap.Current.FSeatedPlayer);
            if (FEnchantmentsVisible ^ Visible)
            {
                FEnchantmentsVisible = Visible;
                if (FEnchantments != Set<CityEnchantmentID>.Of())
                {
                    Aow.Game.World.Map.AoWHSMap.Current.FGlobalMagicControl.TriggerChangedEvent();
                }
            }
        }

        // City.pas:2503
        public Aow.Game.Draw.Images.LibraryImage GetTopCityActionImage(ProductionOrder Order) => GetCityActionImage(Order, 0, FRace != -1, 40, 43);

        // City.pas:2539
        public Aow.Game.Draw.Images.LibraryImage GetMiddleCityActionImage(ProductionOrder Order) => GetCityActionImage(Order, 1, FRace != -1, 41, 44);

        // City.pas:2575
        public Aow.Game.Draw.Images.LibraryImage GetBottomCityActionImage(ProductionOrder Order) => GetCityActionImage(Order, 2, FRace >= 0, 42, 45);

        // Row 0 to 2 of the city's map image while Order runs: an image sequence of the city when the
        // order names one, else the race image (Image on the surface, UndergroundImage below it).
        private Aow.Game.Draw.Images.LibraryImage GetCityActionImage(ProductionOrder Order, int Row, bool HasRace, int Image, int UndergroundImage)
        {
            Aow.Game.Draw.Images.LibraryImage Result;
            int? Sequence = Order?.CitySequence(this, Row);
            if (Sequence is int Index)
            {
                if (FResource.ImageSequenceList != null && Index != -1)
                {
                    if (FResource.ImageSequenceList.GetImageSequence(Index) != null)
                    {
                        Result = FResource.ImageSequenceList.GetImageSequence(Index).GetPreviewImage();
                    }
                    else
                    {
                        Result = null;
                    }
                }
                else
                {
                    Result = null;
                }
            }
            else
            {
                if (HasRace)
                {
                    if (GetLhx() == 0)
                    {
                        Result = Aow.Game.Engine.AoWHSSet.Current.FRaceResources.GetRaceResource(FRace).FImageLibrary.GetSafe(Image);
                    }
                    else
                    {
                        Result = Aow.Game.Engine.AoWHSSet.Current.FRaceResources.GetRaceResource(FRace).FImageLibrary.GetSafe(UndergroundImage);
                    }
                }
                else
                {
                    Result = null;
                }
            }

            return Result;
        }

        // City.pas:138
        public void GenerateRebelUnits(UnitList units)
        {
            int saved = RandSeed;
            RandSeed = GetXhx() * 10 + Aow.Game.World.Map.AoWHSMap.Current.FInitialRandomSeed + GetYhx();
            Aow.Game.Engine.Collections.IntegerList.FillWithRandomUnits(Aow.Game.Engine.AoWHSSet.Current.FRaceResources.GetRaceResource(FRace).FUnitIndexes, units, 2, GetSize() * 25, Set<UnitType>.Of(UnitType.Humanoid, UnitType.Creature));
            // The source restores only after a successful fill.
            RandSeed = saved;
        }

        private static readonly string AverageRStr = "Average";
        private static readonly string CityBattleDrawnRStr = "Battle of %s drawn";
        private static readonly string CityBattleLostRStr = "Battle of %s lost";
        private static readonly string CityBattleWonRStr = "%s captured";
        private static readonly string CityFreeJoinRStr = "The city council eagerly vows its loyalty and awaits your orders.";
        private static readonly string CityJoinHeaderRStr = "A herald from %s:";
        private static readonly string CityTributeJoinRStr = "The city council offers its allegiance for %s pieces of gold.";
        private static readonly string MaxFortifyLevelReachedRStr = "Max. fortify level reached.";
        private static readonly string MaxUpgradeLevelReachedRStr = "Max. upgrade level reached.";
        private static readonly string NoNameSpecifiedRStr = "no name specified";
        private static readonly string NonameRStr = "Noname";
        private static readonly string OccupationForcesXRStr = "Occupation Forces: %s";
        private static readonly string RaceRelationXRStr = "Race Relation: %s";
        private static readonly string StrongRStr = "Strong";
        private static readonly string TerrainBonusXRStr = "Terrain Bonus: %s";
        private static readonly string TerrainPenaltyXRStr = "Terrain Penalty: %s";
        private static readonly string UpgradeLevelXRStr = "Upgrade Level: %s";
        private static readonly string WeakRStr = "Weak";
        private static readonly string XCropFieldsRStr = "%s Crop Fields";
        private static readonly string XOverthrownRStr = "%s overthrown";

        internal static global::Aow.Game.Session.RaceTerrainInfo[] RaceTerrain = new global::Aow.Game.Session.RaceTerrainInfo[12]
        {
            new global::Aow.Game.Session.RaceTerrainInfo
            {
                Bonus = Set<byte>.WithBounds(0, 15),
                Penalty = Set<byte>.WithBounds(0, 15)
            },
            new global::Aow.Game.Session.RaceTerrainInfo
            {
                Bonus = Set<byte>.WithBounds(0, 15).With((byte)2),
                Penalty = Set<byte>.WithBounds(0, 15).With((byte)3).With((byte)6)
                    .With((byte)13)
            },
            new global::Aow.Game.Session.RaceTerrainInfo
            {
                Bonus = Set<byte>.WithBounds(0, 15).With((byte)0),
                Penalty = Set<byte>.WithBounds(0, 15).With((byte)2).With((byte)5)
            },
            new global::Aow.Game.Session.RaceTerrainInfo
            {
                Bonus = Set<byte>.WithBounds(0, 15).With((byte)3).With((byte)6)
                    .With((byte)13),
                Penalty = Set<byte>.WithBounds(0, 15).With((byte)2)
            },
            new global::Aow.Game.Session.RaceTerrainInfo
            {
                Bonus = Set<byte>.WithBounds(0, 15).With((byte)1),
                Penalty = Set<byte>.WithBounds(0, 15).With((byte)5)
            },
            new global::Aow.Game.Session.RaceTerrainInfo
            {
                Bonus = Set<byte>.WithBounds(0, 15).With((byte)1),
                Penalty = Set<byte>.WithBounds(0, 15).With((byte)5)
            },
            new global::Aow.Game.Session.RaceTerrainInfo
            {
                Bonus = Set<byte>.WithBounds(0, 15).With((byte)12),
                Penalty = Set<byte>.WithBounds(0, 15)
            },
            new global::Aow.Game.Session.RaceTerrainInfo
            {
                Bonus = Set<byte>.WithBounds(0, 15),
                Penalty = Set<byte>.WithBounds(0, 15)
            },
            new global::Aow.Game.Session.RaceTerrainInfo
            {
                Bonus = Set<byte>.WithBounds(0, 15).With((byte)12),
                Penalty = Set<byte>.WithBounds(0, 15)
            },
            new global::Aow.Game.Session.RaceTerrainInfo
            {
                Bonus = Set<byte>.WithBounds(0, 15),
                Penalty = Set<byte>.WithBounds(0, 15)
            },
            new global::Aow.Game.Session.RaceTerrainInfo
            {
                Bonus = Set<byte>.WithBounds(0, 15).With((byte)12),
                Penalty = Set<byte>.WithBounds(0, 15)
            },
            new global::Aow.Game.Session.RaceTerrainInfo
            {
                Bonus = Set<byte>.WithBounds(0, 15).With((byte)5),
                Penalty = Set<byte>.WithBounds(0, 15).With((byte)1)
            }
        };

        private static global::Aow.Game.Engine.Buffers.FixedArray_int_5 TCity_JoinAmount__JoinPercent = global::Aow.Game.Engine.Buffers.FixedArray_int_5.From([200, 175, 150, 125, 100]);
        private static global::Aow.Game.Engine.Buffers.FixedArray_byte_4 UpgradePrices = global::Aow.Game.Engine.Buffers.FixedArray_byte_4.From((global::System.ReadOnlySpan<byte>)new byte[4] { 250, 250, 250, 250 });
        private static global::Aow.Game.Engine.Buffers.FixedArray_byte_4 WoodWallPrices = global::Aow.Game.Engine.Buffers.FixedArray_byte_4.From((global::System.ReadOnlySpan<byte>)new byte[4] { 20, 40, 60, 80 });
        private static global::Aow.Game.Engine.Buffers.FixedArray_byte_4 StoneWallPrices = global::Aow.Game.Engine.Buffers.FixedArray_byte_4.From((global::System.ReadOnlySpan<byte>)new byte[4] { 20, 40, 60, 80 });
        private static readonly string CityWarningRStr = "Warning";
        private static readonly string CityWoodenWallRStr = "Wooden Wall";
        private static readonly string CityStoneWallRStr = "Stone Wall";
        private static global::Aow.Game.World.Structures.City SortingCity;

        private static bool RaceInCity(sbyte Race, sbyte Player)
        {
            bool flag = false;
            global::Aow.Game.Session.Player players = global::Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(Player);
            for (int i = 0; i != players.FStructures.Count; i++)
            {
                if (flag)
                {
                    break;
                }
                if (players.FStructures.GetStructure(i) is global::Aow.Game.World.Structures.City)
                {
                    flag = !players.FStructures.GetStructure(i).GetBuilding() && ((global::Aow.Game.World.Structures.City)players.FStructures.GetStructure(i)).FRace == Race;
                }
            }
            return flag;
        }

        private static int UnitLevelToCityLevel(int Level, int Size)
        {
            return Level;
        }

        private static int CompareCityProductions(object Item1, object Item2)
        {
            global::Aow.Game.Units.UnitProduction tUnitProduction = (global::Aow.Game.Units.UnitProduction)Item1;
            int num;
            if (global::Aow.Game.Engine.AoWHSSet.Current.FUnitResources.GetUnitResource(tUnitProduction.FUnitIndex) != null)
            {
                num = global::Aow.Game.Engine.AoWHSSet.Current.FUnitResources.GetUnitResource(tUnitProduction.FUnitIndex).FLevel * 2;
                if (SortingCity.FRace != -1 && global::Aow.Game.Engine.AoWHSSet.Current.FRaceResources.GetRaceResource(SortingCity.FRace).FSpecialUnitIndexes.Count > 3 && global::Aow.Game.Engine.AoWHSSet.Current.FRaceResources.GetRaceResource(SortingCity.FRace).FSpecialUnitIndexes[3] == tUnitProduction.FUnitIndex)
                {
                    num++;
                }
            }
            else
            {
                num = 0;
            }
            global::Aow.Game.Units.UnitProduction tUnitProduction2 = (global::Aow.Game.Units.UnitProduction)Item2;
            int num2;
            if (global::Aow.Game.Engine.AoWHSSet.Current.FUnitResources.GetUnitResource(tUnitProduction2.FUnitIndex) != null)
            {
                num2 = global::Aow.Game.Engine.AoWHSSet.Current.FUnitResources.GetUnitResource(tUnitProduction2.FUnitIndex).FLevel * 2;
                if (SortingCity.FRace != -1 && global::Aow.Game.Engine.AoWHSSet.Current.FRaceResources.GetRaceResource(SortingCity.FRace).FSpecialUnitIndexes.Count > 3 && global::Aow.Game.Engine.AoWHSSet.Current.FRaceResources.GetRaceResource(SortingCity.FRace).FSpecialUnitIndexes[3] == tUnitProduction2.FUnitIndex)
                {
                    num2++;
                }
            }
            else
            {
                num2 = 0;
            }
            int num3 = num - num2;
            if (num3 == 0)
            {
                num3 = ((global::Aow.Game.Units.UnitProduction)Item1).FUnitIndex - ((global::Aow.Game.Units.UnitProduction)Item2).FUnitIndex;
            }
            return num3;
        }

        public enum ModeElement : byte
        {
            Warmode = 0, Razed = 1, TownQuake = 2, Anarchy = 3
        }

        public enum MoveOutOfCitySelectionElement : byte
        {
            SkipOwnPlayer = 0
        }

        public enum CivilStatus : byte
        {
            Unruly = 0, Unrest = 1, Stable = 2, Content = 3, Cheerful = 4, Oppressed = 5, Slavery = 6
        }

        public enum DefendersGenerateMode : byte
        {
            None = 0, Random = 1, Weak = 2, Average = 3, Strong = 4
        }
    }

    // FSize
    public partial class Resource : PlayerCropStructureResource
    {
        public Resource(ECustomNode owner) : base(owner)
        {
            FRazeable = true;
        }

        // City.pas:2628
        public new const int ClassID = 131337;

        public byte FSize;

        // City.pas:2622

        // City.pas:2669
        public override void MsgProc(ref EMsg Msg, EObject Sender)
        {
            base.MsgProc(ref Msg, Sender);
            if (Msg.MsgID == 536870920)
            {
                ActivateMsg();
            }
        }

        // City.pas:2638
        public override int ResourceUserClassID() => 131336;

        // City.pas:2643
        public void ShowCity(int X, int Y, in SurfaceDesc SurfaceDesc, City City)
        {
            int dX;
            int dY;
            Aow.Game.Draw.Images.LibraryImage Image;
            dX = FdXImage + X;
            dY = Y;
            dY += FdYImage;
            Image = City.FWallBackImage;
            if (Image != null)
            {
                Image.Show(dX, dY, SurfaceDesc);
            }

            if (City.FCityImage != null)
            {
                // { Original blinks the selected city from frame bits. }
                if (City.GetSelected() && (Aow.Game.World.Map.AoWHSMap.Current.Frame & 6) == 6)
                {
                    City.FCityImage.ShowTransparent(dX, dY, SurfaceDesc);
                }
                else
                {
                    City.FCityImage.Show(dX, dY, SurfaceDesc);
                }
            }

            Image = City.FWallFrontImage;
            if (Image != null)
            {
                Image.Show(dX, dY, SurfaceDesc);
            }
        }

        // City.pas:2661
        public new void ActivateMsg()
        {
            FSize = 0;
            for (int I = 0, last = FTerrainList.Count - 1; I <= last; I++)
            {
                if (FTerrainList.Terrain[I] >= 0)
                {
                    FSize = (byte)(FSize + 1);
                }
            }
        }

        // City.pas:262

        // City.pas:2633
        public override string ResourceEditName() => Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.World.Structures.Resource.CityRStr);

        private static readonly string CityRStr = "City";

    }
}

namespace Aow.Game.Events
{

    // FXhx, FYhx, FLhx, FCityName
    public partial class CityRebellionEventLog : EventLog
    {
        public CityRebellionEventLog(ECustomNode owner) : base(owner)
        { }

        // City.pas:2675
        public new const int ClassID = 131346;

        public int FXhx;
        public int FYhx;
        public int FLhx;
        public string FCityName = "";

        // City.pas:2680
        public override void ReadWrite(EStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwInteger(ref FXhx, 20);
            Stream.rwInteger(ref FYhx, 21);
            Stream.rwInteger(ref FLhx, 22);
            Stream.rwString(ref FCityName, 23);
        }

        // City.pas:2689
        public override string GetText() => RtlUnit.Format(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Events.CityRebellionEventLog.CityRebellionRStr), [FCityName]);

        // City.pas:2699
        public override bool Execute(bool Mode, ExecuteTrigger Trigger, ExecuteDoneEvent Done)
        {
            bool Result = default;
            bool ShowEvent;
            ShowEvent = Trigger == ExecuteTrigger.User;
            ShowEvent = ShowEvent || Aow.Game.World.Map.AoWHSMap.Current.FPlayMode == PlayMode.Turn && Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.GetClassicWindowPopupEvents().Contains(WindowPopupEvent.Rebellion);
            ShowEvent = ShowEvent || Aow.Game.World.Map.AoWHSMap.Current.FPlayMode == PlayMode.Simultaneous && Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.GetSimultaneousWindowPopupEvents().Contains(WindowPopupEvent.Rebellion);
            if (ShowEvent)
            {
                Result = base.Execute(Mode, Trigger, Done);
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // City.pas:2694
        public override void ViewLocation()
        {
            CenterViewTo((sbyte)FXhx, (sbyte)FYhx, (sbyte)FLhx);
        }

        private static readonly string CityRebellionRStr = "%s rebelled";

    }
}

namespace Aow.Game.Ai
{

    public partial class AICityUpgradeBudget : AIBudget
    {
        // City.pas:2775
        public new const int ClassID = 131349;

        public AICityUpgradeBudget(ECustomNode owner) : base(owner)
        { }

    }

    public partial class AICityResearchBudget : AIBudget
    {
        // City.pas:2780
        public new const int ClassID = 131350;

        public AICityResearchBudget(ECustomNode owner) : base(owner)
        { }

    }

    public partial class AICityFortifyBudget : AIBudget
    {
        // City.pas:2785
        public new const int ClassID = 131351;

        public AICityFortifyBudget(ECustomNode owner) : base(owner)
        { }

    }

    // FCityCount, FProductionLevelCounts, FMaxProductionLevel, FProductionRatio
    public partial class CityAIPA : AIPlayerAction, IAIPlayerActionClass
    {
        public CityAIPA(ECustomNode owner) : base(owner)
        { }

        public int FCityCount;
        public FixedArray_int_4 FProductionLevelCounts;
        public int FMaxProductionLevel;
        public float FProductionRatio;

        // City.pas:3130
        public override bool Process()
        {
            Player Player;
            AIBudgetManager Manager;
            AIBudget UpgradeBudget;
            AIBudget ResearchBudget;
            AIBudget FortifyBudget;
            UpdateStatistics();
            Player = FControl.FPlayer;
            if (Player.FPlayerIndex != 0)
            {
                Manager = Player.FAIBudget;
                UpgradeBudget = (AIBudget)Manager.FBudgets.FindChild(131349);
                if (UpgradeBudget == null)
                {
                    UpgradeBudget = new AICityUpgradeBudget(null);
                    UpgradeBudget.Owner = Manager.FBudgets;
                    UpgradeBudget.FRequiredGold = 250;
                    UpgradeBudget.FDays = 7;
                }

                ResearchBudget = (AIBudget)Manager.FBudgets.FindChild(131350);
                if (ResearchBudget != null)
                {
                    if (ResearchBudget.FGold + UpgradeBudget.FGold > 125)
                    {
                        if (ResearchProduction(ResearchBudget, UpgradeBudget))
                        {
                            ResearchBudget.FDays = 7;
                            ResearchBudget.FRequiredGold = 125;
                        }
                    }
                }
                else
                {
                    ResearchBudget = new AICityResearchBudget(null);
                    ResearchBudget.FDays = 7;
                    ResearchBudget.FRequiredGold = 125;
                    ResearchBudget.Owner = Manager.FBudgets;
                }

                ValidateResearchBudget(ResearchBudget);
                if (UpgradeBudget.GetReady())
                {
                    if (UpgradeCity(UpgradeBudget))
                    {
                        UpgradeBudget.FRequiredGold = 250;
                        UpgradeBudget.FDays = 7;
                    }
                }

                ValidateUpgradeBudget(UpgradeBudget);
                FortifyBudget = (AIBudget)Manager.FBudgets.FindChild(131351);
                if (FortifyBudget != null)
                {
                    if (FortifyBudget.GetReady())
                    {
                        if (FortifyCity(FortifyBudget))
                        {
                            FortifyBudget.FDays = 7;
                            FortifyBudget.FRequiredGold = 80;
                        }
                    }
                }
                else
                {
                    if (Aow.Game.World.Map.AoWHSMap.Current.FDay > 5)
                    {
                        FortifyBudget = new AICityFortifyBudget(null);
                        FortifyBudget.FDays = 7;
                        FortifyBudget.FRequiredGold = 80;
                        FortifyBudget.Owner = Manager.FBudgets;
                    }
                }

                if (FortifyBudget != null)
                {
                    ValidateFortifyBudget(FortifyBudget);
                }
            }

            return false;
        }

        // City.pas:2795
        public bool ValidResearchCity(City City)
        {
            bool Result = City.FProduction.GetProduceUnitLevelMax() < City.FUpgradeLevel;
            bool result = Result;
            if (result)
            {
                var activeAction = City.GetActiveAction();
                bool contains = Set<CityAction>.Of(CityAction.None, CityAction.Trade).Contains(activeAction);
                if (!contains)
                {
                    var cityActiveAction = City.GetActiveAction();
                    contains = Set<CityAction>.Of(CityAction.Produce).Contains(cityActiveAction) && City.FProduction.FSettings.FTurnsLeft == City.FProduction.FSettings.FTurnsTotal;
                }

                result = contains;
            }

            return result;
        }

        // City.pas:2802
        public bool ValidResearchBudgetCity(City City) => City.FProduction.GetProduceUnitLevelMax() != City.FUpgradeLevel || City.GetActiveAction() == CityAction.Upgrade;

        // City.pas:2807
        public void ValidateResearchBudget(AIBudget Budget)
        {
            Player Player;
            int Count;
            int Total;
            int Priority;
            PlayerStructure Structure;
            Player = FControl.FPlayer;
            Count = 0;
            Total = 0;
            for (int I = Player.FStructures.Count - 1; I >= 0; I--)
            {
                Structure = Player.FStructures.GetStructure(I);
                if (Structure is City)
                {
                    Total += 1;
                    if (ValidResearchBudgetCity((City)Structure))
                    {
                        Count += 1;
                    }
                }
            }

            if (Count == 0)
            {
                Player.FAIBudget.ReturnExpense(Budget.FGold, Budget.FMana);
                Budget.FGold = 0;
                Budget.FMana = 0;
                Budget.FRequiredGold = 0;
                // { Original limits days, not priority. }
                Budget.FDays = 7;
            }
            else
            {
                Budget.FRequiredGold = 125;
                Priority = 7 - Count * Total / 3;
                if (Priority < 3)
                {
                    Priority = 3;
                }

                if (Priority < Budget.FDays)
                {
                    Budget.FDays = Priority;
                }
            }
        }

        // City.pas:2842
        public bool ResearchProduction(AIBudget ResearchBudget, AIBudget UpgradeBudget)
        {
            DoubleItemList<City> Cities;
            bool Valid;
            Player Player;
            int UnitIndex;
            int Index;
            int Value;
            int Priority;
            PlayerStructure Structure;
            City City;
            ProductionItem Production;
            bool Result = false;
            Player = FControl.FPlayer;
            Cities = new DoubleItemList<global::Aow.Game.World.Structures.City>();
            for (int I = Player.FStructures.Count - 1; I >= 0; I--)
            {
                Structure = Player.FStructures.GetStructure(I);
                if (Structure is global::Aow.Game.World.Structures.City && ValidResearchCity((global::Aow.Game.World.Structures.City)Structure))
                {
                    City = (global::Aow.Game.World.Structures.City)Structure;
                    Valid = false;
                    for (int J = City.FProduction.FUnitProductions.Count - 1; J >= 0; J--)
                    {
                        UnitIndex = City.FProduction.FUnitProductions.GetItem(J).FUnitIndex;
                        if (UnitIndex != -1)
                        {
                            Valid = Valid || Aow.Game.Engine.AoWHSSet.Current.FUnitResources.GetUnitResource(UnitIndex).FLevel == City.FUpgradeLevel && !City.FProduction.FUnitProductions.GetItem(J).FResearched && City.ResearchCost(J) <= ResearchBudget.FGold + UpgradeBudget.FGold;
                        }
                    }

                    if (Valid)
                    {
                        Priority = City.GetSize() * 2 + (City.FProductionPriority + City.FWallType * 3);
                        Cities.AddAB(City, Priority);
                    }
                }
            }

            Cities.SortDescendingItemB();
            if (Cities.Count != 0)
            {
                Index = 0;
                Value = Round((double)Cities[0].B * 0.75);
                while (Index + 1 < Cities.Count && Value <= Cities[Index + 1].B)
                {
                    Index += 1;
                }

                Index = Aow.Game.World.Map.AoWHSMap.Current.Random(Index + 1);
                City = Cities[Index].A;
                RandSeed = Aow.Game.World.Map.AoWHSMap.Current.Random(16777215);
                Index = Cities[Index].A.FProduction.FindOptimalResearchProduction(null, ResearchBudget.FGold + UpgradeBudget.FGold);
                if (Index != -1)
                {
                    City.FProduction.ExecuteDeleteProductions();
                    Production = new ProductionItem(null);
                    Production.Order = new ResearchSpell(Index);
                    City.FProduction.ExecuteAddProduction(Production);
                    Production?.Dispose();
                    Index = City.ResearchCost(Index);
                    if (Index > ResearchBudget.FGold)
                    {
                        Index -= ResearchBudget.FGold;
                        Player.FAIBudget.BudgetExpenseMade(ResearchBudget.FGold, 0, ResearchBudget, Aow.Game.Ai.AIBudgetManager.BudgetExpenseMode.Budget);
                        Player.FAIBudget.BudgetExpenseMade(Index, 0, UpgradeBudget, Aow.Game.Ai.AIBudgetManager.BudgetExpenseMode.Budget);
                    }
                    else
                    {
                        Player.FAIBudget.BudgetExpenseMade(Index, 0, ResearchBudget, Aow.Game.Ai.AIBudgetManager.BudgetExpenseMode.Budget);
                    }

                    Result = true;
                }
            }

            Cities?.Dispose();
            return Result;
        }

        // City.pas:2909
        public bool ValidUpgradeCity(City City)
        {
            string Reason = "";
            int Level;
            Level = City.FProduction.GetProduceUnitLevelMax();
            bool Result = City.CanUpgrade(false, ref Reason);
            Result = Result && City.GetSize() > City.FUpgradeLevel && !(Level == FMaxProductionLevel && FProductionLevelCounts[Level - 1] == 1) && Level == City.FUpgradeLevel;
            bool result = Result;
            if (result)
            {
                var activeAction = City.GetActiveAction();
                bool contains = Set<CityAction>.Of(CityAction.None, CityAction.Trade).Contains(activeAction);
                if (!contains)
                {
                    var cityActiveAction = City.GetActiveAction();
                    contains = Set<CityAction>.Of(CityAction.Produce).Contains(cityActiveAction) && City.FProduction.FSettings.FTurnsLeft == City.FProduction.FSettings.FTurnsTotal;
                }

                result = contains;
            }

            return result;
        }

        // City.pas:2921
        public bool ValidUpgradeBudgetCity(City City)
        {
            bool Result;
            int Level;
            Level = City.FProduction.GetProduceUnitLevelMax();
            if (City.GetSize() <= City.FUpgradeLevel || Level != City.FUpgradeLevel)
            {
                Result = false;
            }
            else
            {
                Result = true;
            }

            return Result;
        }

        // City.pas:2928
        public bool UpgradeCity(AIBudget Budget)
        {
            bool Result;
            Player Player;
            DoubleItemList<City> Cities;
            int Priority;
            PlayerStructure Structure;
            City City;
            ProductionItem Production;
            Player = FControl.FPlayer;
            Cities = new DoubleItemList<global::Aow.Game.World.Structures.City>();
            for (int I = Player.FStructures.Count - 1; I >= 0; I--)
            {
                // { Preserve the original runtime class check. }
                Structure = Player.FStructures.GetStructure(I);
                if (Structure is global::Aow.Game.World.Structures.City && ValidUpgradeCity((global::Aow.Game.World.Structures.City)Structure))
                {
                    City = (global::Aow.Game.World.Structures.City)Structure;
                    Priority = City.GetSize() * 2 + (City.FProductionPriority + City.FWallType * 3);
                    Cities.AddAB(City, Priority);
                }
            }

            Cities.SortDescendingItemB();
            if (Cities.Count != 0)
            {
                City = Cities[0].A;
                City.FProduction.ExecuteDeleteProductions();
                Production = new ProductionItem(null);
                Production.Order = new Aow.Game.World.Structures.Upgrade();
                City.FProduction.ExecuteAddProduction(Production);
                Production?.Dispose();
                Player.FAIBudget.BudgetExpenseMade(250, 0, Budget, Aow.Game.Ai.AIBudgetManager.BudgetExpenseMode.Budget);

                Result = true;
            }
            else
            {
                Result = false;
            }

            Cities?.Dispose();
            return Result;
        }

        // City.pas:2964
        public void ValidateUpgradeBudget(AIBudget Budget)
        {
            Player Player;
            int Count;
            int Priority;
            PlayerStructure Structure;
            Player = FControl.FPlayer;
            Count = 0;
            for (int I = Player.FStructures.Count - 1; I >= 0; I--)
            {
                Structure = Player.FStructures.GetStructure(I);
                if (Structure is City)
                {
                    if (ValidUpgradeBudgetCity((City)Structure))
                    {
                        Count += 1;
                    }
                }
            }

            if (Count == 0)
            {
                Player.FAIBudget.ReturnExpense(Budget.FGold, Budget.FMana);
                Budget.FGold = 0;
                Budget.FMana = 0;
                Budget.FRequiredGold = 0;
                // { Original limits days, not priority. }
                Budget.FDays = 7;
            }
            else
            {
                Budget.FRequiredGold = 250;
                Priority = 8 - Count / 2;
                if (Priority < 5)
                {
                    Priority = 5;
                }

                if (Priority < Budget.FDays)
                {
                    Budget.FDays = Priority;
                }
            }
        }

        // City.pas:2999
        public bool ValidFortifyCity(City City)
        {
            string Reason = "";
            bool Result = City.CanFortify(false, ref Reason);
            Result = Result && City.FWallType == 0 || City.FWallType == 1 && City.GetSize() > 2 && City.FUpgradeLevel > 1 && Aow.Game.World.Map.AoWHSMap.Current.FDay >= 30;
            bool result = Result;
            if (result)
            {
                var activeAction = City.GetActiveAction();
                bool contains = Set<CityAction>.Of(CityAction.None, CityAction.Trade).Contains(activeAction);
                if (!contains)
                {
                    var cityActiveAction = City.GetActiveAction();
                    contains = Set<CityAction>.Of(CityAction.Produce).Contains(cityActiveAction) && City.FProduction.FSettings.FTurnsLeft == City.FProduction.FSettings.FTurnsTotal;
                }

                result = contains;
            }

            return result;
        }

        // City.pas:3009
        public bool ValidFortifyBudgetCity(City City) => City.FWallType == 0 || City.FWallType == 1 && City.GetSize() > 2 && City.FUpgradeLevel > 1 && Aow.Game.World.Map.AoWHSMap.Current.FDay >= 30;

        // City.pas:3015
        public bool FortifyCity(AIBudget Budget)
        {
            bool Result;
            Player Player;
            DoubleItemList<City> Cities;
            int Priority;
            PlayerStructure Structure;
            City City;
            ProductionItem Production;
            Player = FControl.FPlayer;
            Cities = new DoubleItemList<global::Aow.Game.World.Structures.City>();
            for (int I = Player.FStructures.Count - 1; I >= 0; I--)
            {
                // { Preserve the original runtime class check. }
                Structure = Player.FStructures.GetStructure(I);
                if (Structure is global::Aow.Game.World.Structures.City && ValidFortifyCity((global::Aow.Game.World.Structures.City)Structure))
                {
                    City = (global::Aow.Game.World.Structures.City)Structure;
                    Priority = City.GetSize() * 2 + City.FProductionPriority;
                    if (City.FWallType == 0)
                    {
                        Priority += 3;
                    }

                    Cities.AddAB(City, Priority);
                }
            }

            Cities.SortDescendingItemB();
            if (Cities.Count != 0)
            {
                City = Cities[0].A;
                City.FProduction.ExecuteDeleteProductions();
                var fortifyCost = City.FortifyCost();
                Player.FAIBudget.BudgetExpenseMade(fortifyCost, 0, Budget, Aow.Game.Ai.AIBudgetManager.BudgetExpenseMode.Budget);
                Production = new ProductionItem(null);
                Production.Order = new Aow.Game.World.Structures.Fortify();
                City.FProduction.ExecuteAddProduction(Production);
                Production?.Dispose();

                Result = true;
            }
            else
            {
                Result = false;
            }

            Cities?.Dispose();
            return Result;
        }

        // City.pas:3052
        public void ValidateFortifyBudget(AIBudget Budget)
        {
            Player Player;
            int Count;
            int Priority;
            PlayerStructure Structure;
            Player = FControl.FPlayer;
            Count = 0;
            for (int I = Player.FStructures.Count - 1; I >= 0; I--)
            {
                Structure = Player.FStructures.GetStructure(I);
                if (Structure is City)
                {
                    if (ValidFortifyBudgetCity((City)Structure))
                    {
                        Count += 1;
                    }
                }
            }

            if (Count == 0)
            {
                Player.FAIBudget.ReturnExpense(Budget.FGold, Budget.FMana);
                Budget.FGold = 0;
                Budget.FMana = 0;
                Budget.FRequiredGold = 0;
                // { Original limits days, not priority. }
                Budget.FDays = 7;
            }
            else
            {
                Budget.FRequiredGold = 80;
                Priority = 8 - Count / 2;
                if (Priority < 4)
                {
                    Priority = 4;
                }

                if (Priority < Budget.FDays)
                {
                    Budget.FDays = Priority;
                }
            }
        }

        // City.pas:3087
        public int UpgradeableCities()
        {
            int Count;
            PlayerStructure Structure;
            Count = 0;
            for (int I = FControl.FPlayer.FStructures.Count - 1; I >= 0; I--)
            {
                Structure = FControl.FPlayer.FStructures.GetStructure(I);
                if (Structure is City)
                {
                    _ = (City)Structure;
                    var city = (City)Structure;
                    if (city.GetSize() > city.FUpgradeLevel)
                    {
                        Count += 1;
                    }
                }
            }

            return Count;
        }

        // City.pas:3102
        public void UpdateStatistics()
        {
            int Level;
            int Available;
            int Busy;
            City City;
            FCityCount = 0;
            FMaxProductionLevel = 0;
            Available = 0;
            Busy = 0;
            FProductionLevelCounts = default;
            for (int I = FControl.FPlayer.FStructures.Count - 1; I >= 0; I--)
            {
                if (FControl.FPlayer.FStructures.GetStructure(I) is global::Aow.Game.World.Structures.City)
                {
                    FCityCount += 1;
                    City = (global::Aow.Game.World.Structures.City)FControl.FPlayer.FStructures.GetStructure(I);
                    if (Set<CityAction>.Of(CityAction.None, CityAction.Trade, CityAction.Produce).Contains(City.GetActiveAction()))
                    {
                        Available += (City.GetSize() + 1) / 2;
                        Level = City.FProduction.GetProduceUnitLevelMax();
                        if (Level > 0)
                        {
                            FProductionLevelCounts[Level - 1] += 1;
                        }

                        if (Level > FMaxProductionLevel)
                        {
                            FMaxProductionLevel = Level;
                        }
                    }
                    else
                    {
                        Busy += (City.GetSize() + 1) / 2;
                    }
                }
            }

            if (FCityCount != 0)
            {
                // { Original rounds the fraction before storing it. }
                FProductionRatio = Round((double)Available / (Available + Busy));
            }
            else
            {
                FProductionRatio = 0;
            }
        }

        // City.pas:2790
        public static new int Priority => 13;

    }
}

namespace Aow.Game.Events
{

    public partial class CityProductionEventLog : ProductionEventLog
    {
        public CityProductionEventLog(ECustomNode owner) : base(owner)
        { }

        // City.pas:3195
        public new const int ClassID = 131834;

        // City.pas:3205
        public override bool Execute(bool Mode, ExecuteTrigger Trigger, ExecuteDoneEvent Done)
        {
            City City;
            if (Trigger == ExecuteTrigger.User)
            {
                City = GetCity();
                if (City != null && Aow.Game.World.Map.AoWHSMap.Current.FSeatedPlayer == City.FPlayer)
                {
                    Aow.Game.World.Map.AoWHSMap.Current.CenterViewTo(FXhx, FYhx, FLhx);
                    Aow.Game.World.Map.AoWHSMap.Current.TriggerExecuteEventLog(this, true, Done);
                }
            }

            return false;
        }

        // City.pas:3200
        public City GetCity() => (City)Aow.Game.World.Map.AoWHSMap.Current.MapContainer.GetField(FXhx, FYhx, FLhx).FindNoneTransparentHS(typeof(City));

    }
}

namespace Aow.Game.World.Structures
{

    [LegacyObject]
    public partial class CityRegistrator
    {
        // City.pas:3306
        public static void MapLoaded(HSMap Sender)
        {
            if (Sender == Aow.Game.World.Map.AoWHSMap.Current)
            {
                Aow.Game.World.Map.AoWHSMap.Current.FAIPlayerActionManager.RegisterClasses([AIPlayerActionClass.Of<CityAIPA>()]);
            }
        }

        public CityRegistrator()
        {
            CaptureAddress.Assign(this);
        }

        internal static void SubscribeMapLoaded()
        {
            if (Aow.Game.Magic.Abilities.GameTables.ValidateAoWEngine())
                Aow.Game.Magic.Abilities.GameTables.AoWEngine.OnHSMLoaded += MapLoaded;
        }

        internal static void UnsubscribeMapLoaded()
        {
            if (Aow.Game.Magic.Abilities.GameTables.AoWEngine != null)
                Aow.Game.Magic.Abilities.GameTables.AoWEngine.OnHSMLoaded -= MapLoaded;
        }
    }

    public enum CityEnchantmentID : byte
    {
        FountainOfLife = 0,
        GoldRush = 1,
        AntiMagicShell = 2,
        Warmonger = 3
    }

    public enum CityAction : byte
    {
        None = 0,
        Trade = 1,
        Upgrade = 2,
        Fortify = 3,
        Migrate = 4,
        Loot = 5,
        Raze = 6,
        Research = 7,
        Produce = 8
    }
}
