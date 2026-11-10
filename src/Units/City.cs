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

    public enum TCityModeElement : byte
    {
        cmWarmode = 0,
        cmRazed = 1,
        cmTownQuake = 2,
        cmAnarchy = 3
    }

    public enum TMoveOutOfCitySelectionElement : byte
    {
        msSkipOwnPlayer = 0
    }
}

namespace Aow.Game.Draw
{

    public enum AnonymousEnum_saSetup : byte
    {
        saSetup = 0,
        saFade = 1
    }
}

namespace Aow.Game.World.Structures
{

    // { ot=1 }
    // FCity
    public partial class TCityProductionControl : TProductionControl
    {
        public TCityProductionControl(TECustomNode owner) : base(owner)
        { }

        public TCity FCity;

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
            TCity City;
            bool Valid;
            int Count;
            TMapLevel Level;
            THNWalkData Walk = default;
            TCityProductionEventLog Log;
            City = FCity;
            Valid = false;
            Count = City.GetTerrainCount();
            Level = (TMapLevel)City.Owner.Owner;
            var yhx = City.GetYhx();
            var xhx = City.GetXhx();
            Aow.Game.World.Movement.THNWalkData.InitHNWalk(ref Walk, xhx, yhx, 0);
            while (Walk.HN < Count && !Valid)
            {
                if (City.FResource.TerrainList.Terrain[Walk.HN] >= 0 && Walk.X >= 0 && Walk.Y >= 0 && Walk.X < Level.Width && Walk.Y < Level.Height)
                {
                    var lhx = City.GetLhx();
                    Valid = FSettings.FUnit.Place(Walk.X, Walk.Y, lhx, City.FPlayer, TSet<TUnitPlaceModeElement>.Of()) != null;
                }

                Aow.Game.World.Movement.THNWalkData.WalkToNextHN(ref Walk);
            }

            if (!Valid)
            {
                var lhx = City.GetLhx();
                var cityYhx = City.GetYhx();
                var cityXhx = City.GetXhx();
                Valid = FSettings.FUnit.Place(cityXhx, cityYhx, lhx, City.FPlayer, TSet<TUnitPlaceModeElement>.Of(TUnitPlaceModeElement.pmScanArea)) != null;
            }

            Log = new TCityProductionEventLog(null);
            Log.FPlayer = City.FPlayer;
            var name = FSettings.FUnit.GetName();
            var fCityName = FCity.GetName();
            Log.FText = RtlUnit.Format(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.XProducedAtYRStr), [name, fCityName]);
            var cityLhx = City.GetLhx();
            var yhx2 = City.GetYhx();
            Log.SetLocation(City.GetXhx(), yhx2, cityLhx);
            Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Log.FPlayer).FEventLogbook.AddEvent(Log);
            Log.Release();
            return Valid;
        }

        // City.pas:658
        public override void UnitResearched()
        {
            TCity City;
            TCityProductionEventLog Log;
            base.UnitResearched();
            City = FCity;
            Log = new TCityProductionEventLog(null);
            Log.FPlayer = City.FPlayer;
            var unitProduction = FUnitProductions.GetItem(FSettings.FUnitProductionIndex);
            var unitName = Aow.Game.Engine.TAoWHSSet.AoWHSSet.FUnitResources.GetUnitResource(unitProduction.FUnitIndex).GetUnitName();
            var name = FCity.GetName();
            Log.FText = RtlUnit.Format(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.XInstalledAtYRStr), [unitName, name]);
            var lhx = City.GetLhx();
            var yhx = City.GetYhx();
            Log.SetLocation(City.GetXhx(), yhx, lhx);
            Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Log.FPlayer).FEventLogbook.AddEvent(Log);
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
        public override TUnit CreateUnitProduction(int Index)
        {
            TUnit Result = base.CreateUnitProduction(Index);
            if (Result != null && FCity.FEnchantments.Contains(TCityEnchantmentID.ceiWarmonger))
            {
                Result.SetRank(TUnitRank.urSilver);
            }

            return Result;
        }

        // City.pas:710
        public override int UnitProductionTime(int Index)
        {
            int UnitIndex;
            UnitIndex = FUnitProductions.GetItem(Index).FUnitIndex;
            var unitResource = Aow.Game.Engine.TAoWHSSet.AoWHSSet.FUnitResources.GetUnitResource(UnitIndex);
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
    public partial class TCityOverlayHS : TSingleHS
    {
        public TCityOverlayHS(TECustomNode owner) : base(owner)
        { }

        public TCity FCity;
        public int FModeStartFrame;
        public TCityOverlayMode FMode;
        public AnonymousEnum_saSetup FShellAnimation;
        public int FShellEndFrame;

        // City.pas:718
        public override int GetShowPriority() => FCity.GetShowPriority() + 134217728;

        // City.pas:733
        public override void Show(int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
        {
            int Xhp = default;
            int Yhp = default;
            int Intensity;
            int Index;
            int Frame;
            TImageSequence Sequence;
            Aow.Game.World.Map.HexGeometry.HXtoHP(FCity.FXhx, FCity.FYhx, ref Xhp, ref Yhp);
            XhpSurf = Xhp - XhpSurf;
            YhpSurf = Yhp - YhpSurf;
            if (FCity.FEnchantmentsVisible)
            {
                if (FCity.FEnchantments.Contains(TCityEnchantmentID.ceiWarmonger))
                {
                    Intensity = Math.Abs((int)(((uint)Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Frame) >> 1) % 20 - 10) + 50;
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

                if (FCity.FEnchantments.Contains(TCityEnchantmentID.ceiGoldRush))
                {
                    Intensity = Math.Abs((int)(((uint)Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Frame) >> 1) % 20 - 10) + 70;
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
                            Index = Sequence.GetFrameIndex(0, Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Frame);
                            if (Sequence.ImageLibrary.ImageExist(Index))
                            {
                                Sequence.ImageLibrary[Index].Show(XhpSurf + Sequence.dXshift, YhpSurf + Sequence.dYshift, SurfaceDesc);
                            }
                        }

                        Sequence = Sequence.Next;
                    }
                }

                if (FCity.FEnchantments.Contains(TCityEnchantmentID.ceiAntiMagicShell))
                {
                    Sequence = FCity.FResource.ImageSequenceList.ImageSequences[19];
                    if (Sequence != null)
                    {
                        Intensity = 50;
                        Index = Sequence.GetFrameIndex(0, 0);
                        if (FShellEndFrame != -1 && Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Frame < FShellEndFrame)
                        {
                            switch (FShellAnimation)
                            {
                                case AnonymousEnum_saSetup.saSetup:
                                    Frame = FShellEndFrame - Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Frame;
                                    if (Frame <= 50)
                                    {
                                        Intensity = 50 - Frame;
                                    }
                                    else
                                    {
                                        FShellEndFrame = -1;
                                    }

                                    break;
                                case AnonymousEnum_saSetup.saFade:
                                    Frame = FShellEndFrame - Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Frame;
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

                if (FMode == TCityOverlayMode.omLooting)
                {
                    FCity.FResource.ShowComplexImage(XhpSurf, YhpSurf, SurfaceDesc, 22, (uint)(((TMapField)FCity.Owner).Xhx + ((TMapField)FCity.Owner).Yhx * 16));
                }
            }
        }

        // City.pas:721
        public void PlaySetupShellAnimation()
        {
            FShellAnimation = AnonymousEnum_saSetup.saSetup;
            FShellEndFrame = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Frame + 50;
        }

        // City.pas:727
        public void PlayFadeShellAnimation()
        {
            FShellAnimation = AnonymousEnum_saSetup.saFade;
            FShellEndFrame = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Frame + 20;
        }

        // City.pas:815
        public void SetMode(TCityOverlayMode Mode)
        {
            if (Mode != FMode)
            {
                FModeStartFrame = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Frame;
                FMode = Mode;
            }
        }

        // Engine.pas:524

    }
}

namespace Aow.Game.Units
{

    // FRace, FTime, FRelation
    public partial class TMigrationItem : TEObject
    {
        public TMigrationItem(TECustomNode owner) : base(owner)
        { }

        public sbyte FRace;
        public int FTime;
        public TStatus FRelation;
    }
}

namespace Aow.Game.World.Structures
{

    public partial class TMigrationList : TENode
    {
        public TMigrationList(TECustomNode owner) : base(owner)
        { }

        // City.pas:2711
        public TMigrationItem GetItems(int Index) => (TMigrationItem)FChildren.List[Index];

    }

    // FEnchantmentID, FEnchantmentLocation
    public partial class TCityEnchantment : TGlobalEnchantment
    {
        // City.pas:2722
        public new const int ClassID = 131820;

        public new TCityEnchantmentID FEnchantmentID;
        public TXYL FEnchantmentLocation;

        // City.pas:2727
        public override void ReadWrite(TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwData(Bytes.Of(ref FEnchantmentID, 1), 1, 20);
            Stream.rwInteger(ref FEnchantmentLocation.XYL, 21);
        }

        public override void PersistFields(IPersister persister)
        {
            base.PersistFields(persister);
            persister.Enum("TCityEnchantment.EnchantmentID", ref FEnchantmentID);
            persister.Int("EnchantmentLocation", ref FEnchantmentLocation.XYL);
        }

        // City.pas:2716
        public TCityEnchantment(TECustomNode owner) : base(owner)
        {
            FEnchantmentMode = FEnchantmentMode - TSet<TGlobalEnchantmentModeElement>.Of(TGlobalEnchantmentModeElement.gemTrueGlobal, TGlobalEnchantmentModeElement.gemOwned);
        }

        // City.pas:2762
        public override string GetName() => Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.World.Structures.TCityEnchantment.TCityEnchantmentIDRStr[(int)FEnchantmentID]);

        // City.pas:2739
        public override TXYL GetLocation()
        {
            TXYL Result = default;
            Result.X = FEnchantmentLocation.X;
            Result.Y = FEnchantmentLocation.Y;
            Result.L = FEnchantmentLocation.L;
            return Result;
        }

        // City.pas:2767
        public override int GetAIUpkeepPriority()
        {
            int Result;
            TCity City;
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
            if (FEnchantmentStatus.Contains(TGlobalEnchantmentStatusElement.getActivated))
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
        public TCity GetCity() => (TCity)Aow.Game.World.Map.TAoWHSMap.AoWHSMap.MapContainer.GetField(FEnchantmentLocation.X, FEnchantmentLocation.Y, FEnchantmentLocation.L).FindNoneTransparentHS(typeof(TCity));

        private static global::Aow.Game.Engine.Buffers.FixedArray_string_4 TCityEnchantmentIDRStr = global::Aow.Game.Engine.Buffers.FixedArray_string_4.From(["Fountain Of Life", "Gold Rush", "Anti Magic Shell", "Warmonger"]);

    }

    // FMode, FRace, FName, FWallType, FUpgradeLevel, FProduction, FFlagID, FEnchantmentList,
    // FEnchantments, FOverlay, FEnchantmentsVisible, FWallBackImage, FWallFrontImage, FCityImage,
    // FDefendersMode
    public partial class TCity : TPlayerCropStructure
    {
        public TCity(TECustomNode owner) : base(owner)
        {
            FProduction = new TCityProductionControl(null);
            FProduction.FCity = this;
            FDefendersMode = TDefendersGenerateMode.dgAverage;
            FRace = -1;
            FName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.World.Structures.TCity.NonameRStr);
            FUpgradeLevel = 1;
            FOverlay = new TCityOverlayHS(null);
            FOverlay.FCity = this;
            FEnchantmentList = new TGlobalEnchantmentList(null);
            FEnchantmentsVisible = false;
        }

        // City.pas:849
        public new const int ClassID = 131336;

        public TSet<TCityModeElement> FMode;
        public sbyte FRace;
        public string FName = "";
        public byte FWallType;
        public byte FUpgradeLevel;
        public TCityProductionControl FProduction;
        public new byte FFlagID;
        public TGlobalEnchantmentList FEnchantmentList;
        public TSet<TCityEnchantmentID> FEnchantments;
        public TCityOverlayHS FOverlay;
        public bool FEnchantmentsVisible;
        public Aow.Game.Draw.Images.TLibraryImage FWallBackImage;
        public Aow.Game.Draw.Images.TLibraryImage FWallFrontImage;
        public Aow.Game.Draw.Images.TLibraryImage FCityImage;
        public TDefendersGenerateMode FDefendersMode;

        // City.pas:839
        public override void Dispose()
        {
            if (GetVisible())
            {
                Aow.Game.World.Map.TAoWHSMap.AoWHSMap.DynamicVisibleHS.RemoveChild(FOverlay);
            }

            FOverlay?.Dispose();
            base.Dispose();
            FProduction?.Dispose();
            FEnchantmentList?.Dispose();
            FEnchantmentList = null;
        }

        // City.pas:2157
        public override void ReadWrite(TEStorageStream Stream)
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
        public override void MsgProc(ref TEMsg Msg, TEObject Sender)
        {
            base.MsgProc(ref Msg, Sender);
            if (Msg.MsgID == 537001989)
            {
                if (FRace >= 0)
                {
                    if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FRaces.GetRace(FRace) == null || !Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FRaces.GetRace(FRace).FEnabled)
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
        public override void TerrainChanged(in THSTerrainInfo Info)
        {
            base.TerrainChanged(Info);
            UpdateImages();
        }

        // City.pas:2132
        public override void Show(int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
        {
            int X = default;
            int Y = default;
            Aow.Game.World.Map.HexGeometry.HXtoHP(FXhx, FYhx, ref X, ref Y);
            XhpSurf = X - XhpSurf;
            YhpSurf = Y - YhpSurf;
            if (!FMode.Contains(TCityModeElement.cmRazed) || GetBuilding())
            {
                if ((Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Status.Contains(THSMapStatusElement.msEditting) || !FFogged || !Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FFogEnabled) && FFlagID != 63 && (((TPlayerStructureResource)FResource).FClaimed || (FFlagID & 32) == 0))
                {
                    var cityResource = (TPlayerStructureResource)FResource;
                    if (cityResource.FFlagType <= 1)
                    {
                        Aow.Game.Engine.TAoWHSSet.AoWHSSet.FFlagControl.ShowFlag(cityResource.FFlagX + XhpSurf, cityResource.FFlagY + YhpSurf, cityResource.Engine.HSMap.Frame + X, this.FFlagID, SurfaceDesc);
                    }
                    else
                    {
                        Aow.Game.Engine.TAoWHSSet.AoWHSSet.FFlagControl.ShowStructureFlag(cityResource.FFlagX + XhpSurf, cityResource.FFlagY + YhpSurf, cityResource.Engine.HSMap.Frame + X, this.FFlagID, SurfaceDesc);
                    }
                }
            }

            if (FMode.Contains(TCityModeElement.cmTownQuake))
            {
                XhpSurf = XhpSurf + Random(5) - 2;
                YhpSurf = YhpSurf + Random(5) - 2;
            }

            if (!FMode.Contains(TCityModeElement.cmRazed))
            {
                ((TCityResource)FResource).ShowCity(XhpSurf, YhpSurf, SurfaceDesc, this);
            }
            else
            {
                ((TStructureResource)FResource).Show(XhpSurf, YhpSurf, SurfaceDesc, this);
            }
        }

        // City.pas:2170
        public override bool PlaceOnMap()
        {
            bool Result = base.PlaceOnMap();
            if (Result)
            {
                UpdateMapFields((TMapLevel)Owner.Owner, FXhx, FYhx);
            }

            return Result;
        }

        // City.pas:2176
        public override void RemoveFromMap()
        {
            TMapLevel MapLevel;
            MapLevel = (TMapLevel)Owner.Owner;
            base.RemoveFromMap();
            UpdateMapFields(MapLevel, FXhx, FYhx);
        }

        // City.pas:2378
        public override void MapFieldMsgProc(ref TMapFieldMsg Msg)
        {
            if (!(Msg.MsgID == 4364 && FResource.TerrainList.Terrain[Msg.HN] >= 0))
            {
                base.MapFieldMsgProc(ref Msg);
            }

            if (FResource.TerrainList.Terrain[Msg.HN] >= 0)
            {
                if (Msg.MsgID == 4364 && Msg.HN == 0)
                {
                    if ((!Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FFogEnabled || !FFogged) && FPlayer >= 0)
                    {
                        if (FPlayer == 0)
                        {
                            Msg.Param[0] = Msg.Param[0].Int + 851968;
                        }
                        else
                        {
                            var player = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(FPlayer);
                            Msg.Param[0] = Msg.Param[0].Int + (player.FPlayerRace + 1) * 65536;
                        }
                    }
                    else
                    {
                        Msg.Param[0] = Msg.Param[0].Int + 917504;
                    }

                    Msg.Param[0] = Msg.Param[0].Int + (((TCityResource)FResource).FSize - 1) * 4194304;
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
                                InitCombat(Msg.Param[0].As<TCombat>(), Msg.Param[1].As<TArmyHS>());
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
                                        if (FWallType != 0 && !FMode.Contains(TCityModeElement.cmWarmode) && !GetRazed())
                                        {
                                            if (this == Msg.Param[0].As<TMapField>().FindNoneTransparentHS(typeof(TCity)))
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
                                                if (IncommingNegotiateRequest(Msg.Param[0].As<TNegotiateRequestEventLog>()))
                                                {
                                                    Msg.Param[1] = 1;
                                                }
                                            }
                                            else
                                            {
                                                if (Msg.MsgID == 4367)
                                                {
                                                    if (IncommingNegotiateOffer(Msg.Param[0].As<TNegotiateTE>()))
                                                    {
                                                        Msg.Result = 1;
                                                    }
                                                }
                                                else
                                                {
                                                    if (Msg.MsgID == 4393 && FEnchantments.Contains(TCityEnchantmentID.ceiAntiMagicShell))
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
                Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(FPlayer).FProductionControls.Remove(FProduction);
            }

            FProduction.Deactivate();
            base.Deactivate();
        }

        // City.pas:2184
        public override void UpdateMapField(in THSMapFieldInfo Info)
        {
            base.UpdateMapField(Info);
            if (FResource.TerrainList.GetTerrain(Info.HN) >= 0)
            {
                ((TAoWMapField)Info.MapField).FFieldFlags = ((TAoWMapField)Info.MapField).FFieldFlags | 67108864;
            }
        }

        // City.pas:1246
        public override void MakeVisible(TVisibilityList VisibilityList)
        {
            if (!FStatus.Contains(Aow.Game.World.TMapObjectStatusElement.moVisible))
            {
                base.MakeVisible(VisibilityList);
                if ((TSet<TCityEnchantmentID>.Of(TCityEnchantmentID.ceiGoldRush, TCityEnchantmentID.ceiAntiMagicShell, TCityEnchantmentID.ceiWarmonger) & FEnchantments) != TSet<TCityEnchantmentID>.Of())
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
        public override sbyte GetValidTerrainType(TMapLevel MapLevel, sbyte Xhx, sbyte Yhx) => GetMostFrequentMapFieldTerrainType(MapLevel, Xhx, Yhx, -1);

        // City.pas:2422
        public override Aow.Game.Draw.Images.TLibraryImage GetTopBorderImage()
        {
            int Index;
            TImageSequenceList Sequences;
            Aow.Game.Draw.Images.TLibraryImage Result = GetTopCityActionImage(GetActiveOrder());
            if (Result == null)
            {
                if (FRace != -1)
                {
                    if (GetLhx() == 0)
                    {
                        Result = Aow.Game.Engine.TAoWHSSet.AoWHSSet.FRaceResources.GetRaceResource(FRace).FImageLibrary.GetSafe(40);
                    }
                    else
                    {
                        Result = Aow.Game.Engine.TAoWHSSet.AoWHSSet.FRaceResources.GetRaceResource(FRace).FImageLibrary.GetSafe(43);
                    }
                }
                else
                {
                    Result = null;
                }
            }

            if (FMode.Contains(TCityModeElement.cmRazed))
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
        public override Aow.Game.Draw.Images.TLibraryImage GetMiddleBorderImage()
        {
            int Index;
            TImageSequenceList Sequences;
            Aow.Game.Draw.Images.TLibraryImage Result = GetMiddleCityActionImage(GetActiveOrder());
            if (Result == null)
            {
                if (FRace != -1)
                {
                    if (GetLhx() == 0)
                    {
                        Result = Aow.Game.Engine.TAoWHSSet.AoWHSSet.FRaceResources.GetRaceResource(FRace).FImageLibrary.GetSafe(41);
                    }
                    else
                    {
                        Result = Aow.Game.Engine.TAoWHSSet.AoWHSSet.FRaceResources.GetRaceResource(FRace).FImageLibrary.GetSafe(44);
                    }
                }
                else
                {
                    Result = null;
                }
            }

            if (FMode.Contains(TCityModeElement.cmRazed))
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
        public override Aow.Game.Draw.Images.TLibraryImage GetBottomBorderImage()
        {
            int Index;
            TImageSequenceList Sequences;
            Aow.Game.Draw.Images.TLibraryImage Result = GetBottomCityActionImage(GetActiveOrder());
            if (Result == null)
            {
                if (FRace >= 0)
                {
                    if (GetLhx() == 0)
                    {
                        Result = Aow.Game.Engine.TAoWHSSet.AoWHSSet.FRaceResources.GetRaceResource(FRace).FImageLibrary.GetSafe(42);
                    }
                    else
                    {
                        Result = Aow.Game.Engine.TAoWHSSet.AoWHSSet.FRaceResources.GetRaceResource(FRace).FImageLibrary.GetSafe(45);
                    }
                }
                else
                {
                    Result = null;
                }
            }

            if (FMode.Contains(TCityModeElement.cmRazed))
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
                    FMode = FMode + TSet<TCityModeElement>.Of(TCityModeElement.cmRazed);
                }
                else
                {
                    FMode = FMode - TSet<TCityModeElement>.Of(TCityModeElement.cmRazed);
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
        public override bool GetRazed() => FMode.Contains(TCityModeElement.cmRazed);

        // City.pas:1111
        public override void SetupRazeDefenderAG(TUnitList Armies, TUnitList Units)
        {
            TAIGroup Group;
            if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Random(8) == 0)
            {
                var unitsToIndependentRelation = Aow.Game.Units.TUnitList.UnitsToIndependentRelation(Units, 5);
                Group = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FAIGroupManager.CreateGroup(0, unitsToIndependentRelation, TGroupBehavior.gbRefuge);
                Group.AddUnits(Units);
                Group.Release();
            }
            else
            {
                base.SetupRazeDefenderAG(Armies, Units);
            }
        }

        // City.pas:864
        public override TExecuteResult ExecuteTE(TStructureTE TE, TTokenExecuter Executer) => base.ExecuteTE(TE, Executer);

        // City.pas:1134
        public override bool GetDefenseRequirements(sbyte Player, ref TDefenseRequirements Requirements)
        {
            bool Result;
            TBattleFieldInfo Info = default;
            int Maximum;
            int Minimum;
            TStatus Relation;
            if ((FMode & TSet<TCityModeElement>.Of(TCityModeElement.cmWarmode, TCityModeElement.cmRazed)) == TSet<TCityModeElement>.Of())
            {
                Aow.Game.Combat.TBattleFieldInfo.SetupBattleFieldInfo(ref Info, Player, 0, TSet<TBattleFieldInfoRequestsElement>.Of(), Aow.Game.Magic.Abilities.AbilityRules.dtAll - TSet<TDamageType>.Of(TDamageType.dtWall));
                Info.Requests = Info.Requests + TSet<TBattleFieldInfoRequestsElement>.Of(TBattleFieldInfoRequestsElement.bfrIgnorePassiveIndependents);
                var lhx = GetLhx();
                var yhx = GetYhx();
                var xhx = GetXhx();
                Aow.Game.World.Map.TAoWHSMap.AoWHSMap.RetrieveBattleFieldInfo(xhx, yhx, lhx, 10, ref Info);
                Requirements.Strength = Info.Strength;
                Maximum = Round(GetSize() * 5000);
                Minimum = Round((double)(GetSize() * 5000) * Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Player).FAIControl.FDefenseStrengthMultiplier);
                if (Player != 0)
                {
                    Relation = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FRaces.GetRace(FRace).GetPlayerRelation(Player);
                }
                else
                {
                    Relation = TStatus.sFriendly;
                }

                // { Original condition makes this neutral-player adjustment unreachable. }
                if (TSet<TStatus>.Of(TStatus.sHostile, TStatus.sWary, TStatus.sNeutral).Contains(Relation) && Player == FPlayer && FPlayer == 0)
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
        public override TDefensePriority GetDefensePriority()
        {
            TDefensePriority Result;
            if ((FMode & TSet<TCityModeElement>.Of(TCityModeElement.cmWarmode, TCityModeElement.cmRazed)) == TSet<TCityModeElement>.Of())
            {
                if (GetSize() == 1)
                {
                    Result = TDefensePriority.dpLow;
                }
                else
                {
                    Result = TDefensePriority.dpAverage;
                }

                if (FProduction.FStatus == TProductionControlStatus.pcsProducing && Result == TDefensePriority.dpLow)
                {
                    Result = TDefensePriority.dpAverage;
                }

                bool condition = FPlayer != 0;
                if (condition)
                {
                    var playerRelation = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FRaces.GetRace(FRace).GetPlayerRelation(FPlayer);
                    condition = TSet<TStatus>.Of(TStatus.sHostile, TStatus.sWary).Contains(playerRelation);
                }

                if (condition)
                {
                    Result = TDefensePriority.dpHigh;
                }
            }
            else
            {
                Result = TDefensePriority.dpNone;
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
                if (FRace < -1 && Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FGameType == TGameType.gtCampaign)
                {
                    I = Math.Abs(FRace) - 3;
                    if (I >= 0 && I < Aow.Game.Magic.Abilities.GameTables.AoWEngine.FCampaign.FRaceHistory.Count)
                    {
                        SetRace((sbyte)Aow.Game.Magic.Abilities.GameTables.AoWEngine.FCampaign.FRaceHistory[I]);
                    }
                    else
                    {
                        SetRace(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FRaces.GetRandomRace());
                    }
                }
                else
                {
                    SetRace(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FRaces.GetRandomRace());
                }
            }

            if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FDay == 1)
            {
                FProduction.FUnitProductions.Clear();
                SetUpgradeLevel(FUpgradeLevel);
                GenerateDefenders();
                if (FWallType == 3)
                {
                    FWallType = (byte)Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Random(3);
                    UpdateImages();
                }
            }

            CheckRebellion();
            if (FOverlay.FMode == TCityOverlayMode.omShowRazeAnimation)
            {
                FOverlay.SetMode(TCityOverlayMode.omIdle);
            }
        }

        // City.pas:1705
        public override void NewTurn(sbyte Player)
        {
            base.NewTurn(Player);
            if (!FMode.Contains(TCityModeElement.cmWarmode))
            {
                FProduction.NewTurn(Player);
            }

            if (Player == FPlayer)
            {
                if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Player).FPlayerType != TPlayerType.ptHuman)
                {
                    UpdateAIMigration();
                }
            }
        }

        // City.pas:1335
        public override void ArmyPlaced(TArmyHS PlacedArmy)
        {
            base.ArmyPlaced(PlacedArmy);
            if (FEnchantments.Contains(TCityEnchantmentID.ceiFountainOfLife))
            {
                ExecuteFountainOfLife();
            }

            if (FEnchantments.Contains(TCityEnchantmentID.ceiAntiMagicShell))
            {
                FOverlay.PlayFadeShellAnimation();
            }
        }

        // City.pas:1342
        public override void ArmyRemoved(TArmyHS Army)
        {
            base.ArmyRemoved(Army);
            UpdatePlayer();
        }

        // City.pas:886
        public override void BuildingDone()
        {
            FUpgradeLevel = 1;
            FWallType = 0;
            SetRace(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(FBuildingPlayer).FPlayerRace);
            base.BuildingDone();
        }

        // City.pas:1604
        public override bool GenerateRazeDefenders(sbyte Player, TUnitList Units)
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
                var playerRelationValue = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FRaces.GetRace(FRace).GetPlayerRelationValue(Player);
                Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FRaces.GetRace(FRace).SetPlayerRelationValue(Player, playerRelationValue + -30);
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
        public override bool ValidateMap(TStrings Messages)
        {
            bool Result = base.ValidateMap(Messages);
            var noname = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.World.Structures.TCity.NonameRStr);
            if (CompareText(FName, noname) == 0 || IsEmpty(FName))
            {
                Messages.Add(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.World.Structures.TCity.CityWarningRStr) + ": " + Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.World.Structures.TCity.NoNameSpecifiedRStr));
                Result = false;
            }

            return Result;
        }

        // City.pas:1162
        public override void UpdateAITarget(in TAITargetInfo Info, ref TAITargetData Data)
        {
            int Amount;
            int Priority;
            if (!FMode.Contains(TCityModeElement.cmRazed))
            {
                switch (Info.Mode)
                {
                    case TAITargetMode.tmAttack:
                    case TAITargetMode.tmRaid:
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
                                    var playerRelation = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FRaces.GetRace(FRace).GetPlayerRelation(Info.Player);
                                    if (TSet<TStatus>.Of(TStatus.sPolite, TStatus.sFriendly).Contains(playerRelation))
                                    {
                                        Amount = JoinAmount(Info.Player);
                                        if (Amount != -1)
                                        {
                                            Data.Cost = Amount;
                                            Priority = 100 + (GetSize() - 1) * 10;
                                            Data.TargetType = TAITargetType.attNegotiate;
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
                                    if (FMode.Contains(TCityModeElement.cmWarmode) && Priority != 0)
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
                    case TAITargetMode.tmProduce:
                        if (Info.Player == FPlayer)
                        {
                            Data.Priority = Data.Priority | 2;
                        }

                        break;
                    case TAITargetMode.tmFortify:
                        bool condition = Info.Player == FPlayer;
                        if (condition)
                        {
                            var playerRelation = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FRaces.GetRace(FRace).GetPlayerRelation(Info.Player);
                            condition = TSet<TStatus>.Of(TStatus.sPolite, TStatus.sFriendly).Contains(playerRelation);
                        }

                        if (condition && GetActiveAction() != TCityAction.caMigrate)
                        {
                            var size = GetSize();
                            Data.Priority = 500 + size * 100 + FWallType * 45;
                            if (FEnchantments.Contains(TCityEnchantmentID.ceiAntiMagicShell))
                            {
                                Data.Priority += 150;
                            }

                            if (GetLhx() != 0)
                            {
                                Data.Priority += 50;
                            }
                        }

                        break;
                    case TAITargetMode.tmDefend:
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
        public override bool CanDblClick() => Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer == FPlayer && !GetRazed() && !GetBuilding();

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
                    Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(FPlayer).FProductionControls.Remove(FProduction);
                }

                base.SetPlayer(Player);
                if (GetActiveAction() == TCityAction.caMigrate)
                {
                    FProduction.ExecuteCancelProduction();
                    FProduction.FQueue.GetItem(0)?.Dispose();
                }

                ValidateAICityAction();
                UpdateAIMigration();
                if (FPlayer != -1)
                {
                    Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(FPlayer).FProductionControls.Add(FProduction);
                }
            }
        }

        // City.pas:2071
        public override void UpdateFlagID()
        {
            if (!FStatus.Contains(Aow.Game.World.TMapObjectStatusElement.moEditMode) && (!FMode.Contains(TCityModeElement.cmRazed) || GetBuilding()) && !FMode.Contains(TCityModeElement.cmWarmode))
            {
                if (FPlayer > 0)
                {
                    FFlagID = (byte)(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(FPlayer).FPlayerRace + (FUpgradeLevel - 1) * 64);
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
            if (GetLhx() != 0 || FPlayer > 0 && (((byte)Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(FPlayer).FMagicControl.FEnemyEnchantments.Image) & 8) != 0)
            {
                Result = (int)(((uint)(Result + 1)) >> 1);
            }

            return Result;
        }

        // City.pas:2225
        public override void FinalizeCombat(TCombat Combat)
        {
            if (Combat.FCombatResult == TCombatResult.crVictory || Combat.FCombatResult == TCombatResult.crNoDefenders)
            {
                var combatData = Combat.FCombatData;
                SetPlayer(combatData.FPlayers.GetPlayers(combatData.FAttackerPlayer).FPlayer);

                UpdatePlayer();
            }
        }

        // City.pas:2214
        public override string GetCombatEventText(TCombat Combat)
        {
            string Result = "";
            if (!GetRazed())
            {
                if (Combat.FCombatResult == TCombatResult.crVictory || Combat.FCombatResult == TCombatResult.crNoDefenders)
                {
                    var cityBattleWon = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.World.Structures.TCity.CityBattleWonRStr);
                    var name = GetName();
                    Result = RtlUnit.Format(cityBattleWon, [name]);
                }
                else
                {
                    if (Combat.FCombatResult == TCombatResult.crDefeat)
                    {
                        var cityBattleLost = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.World.Structures.TCity.CityBattleLostRStr);
                        var name = GetName();
                        Result = RtlUnit.Format(cityBattleLost, [name]);
                    }
                    else
                    {
                        if (Combat.FCombatResult != TCombatResult.crCannotBreachWall)
                        {
                            var cityBattleDrawn = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.World.Structures.TCity.CityBattleDrawnRStr);
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
            TCityProductionEventLog Log;
            if (!GetRazed() && !GetBuilding() && OldPlayer == 0 && !Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Status.Contains(THSMapStatusElement.msEditting) && FStatus.Contains(Aow.Game.World.TMapObjectStatusElement.moActivated) && !((TPlayerStructureResource)FResource).FClaimed)
            {
                Log = new TCityProductionEventLog(null);
                Log.FPlayer = NewPlayer;
                var editName = EditName();
                Log.FText = RtlUnit.Format(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.StructureCapturedRStr), [editName]);
                Log.FXhx = GetXhx();
                Log.FYhx = GetYhx();
                Log.FLhx = GetLhx();
                var yhx = GetYhx();
                var lhx = GetLhx();
                var xhx = GetXhx();
                Aow.Game.Events.TEventLog.DistributeLocationEventLog(Log, FPlayer, xhx, yhx, lhx, false);
                Log.Release();
            }
        }

        // City.pas:2271
        public override TMoveAction GetMoveAction(sbyte Player, TMoveAction Action)
        {
            TMoveAction Result = Action;
            if (FPlayer == 0)
            {
                if (TSet<TMoveAction>.Of(TMoveAction.maNormal, TMoveAction.maAttack).Contains(Action) && !GetRazed() && !FMode.Contains(TCityModeElement.cmWarmode))
                {
                    if (JoinAmount(Player) != -1)
                    {
                        Result = TMoveAction.maNegotiate;
                    }
                    else
                    {
                        Result = TMoveAction.maAttack;
                    }
                }
            }
            else
            {
                if (FPlayer != Player && FPlayer != -1)
                {
                    Result = TMoveAction.maAttack;
                }
            }

            return Result;
        }

        // City.pas:1285
        public override bool UpdatePlayer()
        {
            bool MultiplePlayers;
            TMapLevel MapLevel;
            int Count;
            THNWalkData Walk = default;
            TArmyHS Army;
            sbyte Player;
            if (!(FMode.Contains(TCityModeElement.cmRazed) && !GetBuilding()))
            {
                MultiplePlayers = false;
                Player = -1;
                Count = GetTerrainCount();
                MapLevel = (TMapLevel)Owner.Owner;
                Aow.Game.World.Movement.THNWalkData.InitHNWalk(ref Walk, FXhx, FYhx, 0);
                while (Walk.HN < Count && !MultiplePlayers)
                {
                    if (FResource.TerrainList.GetTerrain(Walk.HN) >= 0 && Walk.X >= 0 && Walk.Y >= 0 && Walk.X < MapLevel.Width && Walk.Y < MapLevel.Height)
                    {
                        Army = (TArmyHS)MapLevel.GetField(Walk.X, Walk.Y).FindOwnedChild(131607);
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

                    Aow.Game.World.Movement.THNWalkData.WalkToNextHN(ref Walk);
                }

                if (MultiplePlayers)
                {
                    if (!FMode.Contains(TCityModeElement.cmWarmode))
                    {
                        FMode = FMode + TSet<TCityModeElement>.Of(TCityModeElement.cmWarmode);
                        SetPlayer(-1);
                        UpdateFlagID();
                        Update();
                    }
                }
                else
                {
                    if (FMode.Contains(TCityModeElement.cmWarmode))
                    {
                        FMode = FMode - TSet<TCityModeElement>.Of(TCityModeElement.cmWarmode);
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
            if (!GetRazed() && GetActiveAction() != TCityAction.caMigrate && !GetBuilding())
            {
                SetIncome((byte)(GetCropCount() * 2 + GetSize() * 5));
                if (TSet<TCityAction>.Of(TCityAction.caNone, TCityAction.caTrade).Contains(GetActiveAction()))
                {
                    SetIncome((byte)(FIncome + Round((double)FIncome * 0.25)));
                }

                if (FEnchantments.Contains(TCityEnchantmentID.ceiGoldRush))
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
                    if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Status.Contains(THSMapStatusElement.msEditting))
                    {
                        Count = ((TPlayerCropStructureResource)FResource).FCropLocations.Count;
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
                    Race = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(FPlayer).FPlayerRace;
                }
                else
                {
                    Race = FRace;
                }

                var playerRelationValue = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FRaces.GetRace(Race).GetPlayerRelationValue(Player);
                Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FRaces.GetRace(Race).SetPlayerRelationValue(Player, playerRelationValue + -5);
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
        public TCityAction GetActiveAction() => GetActiveOrder()?.CityAction ?? TCityAction.caNone;

        // The order the city carries out, null when it produces nothing.
        public ProductionOrder GetActiveOrder() => FProduction.FStatus == TProductionControlStatus.pcsProducing ? FProduction.FQueue.GetItem(0).Order : null;

        // City.pas:894
        public void UpdateActiveEnchantments()
        {
            FEnchantments = TSet<TCityEnchantmentID>.Of();
            for (int I = FEnchantmentList.Count - 1; I >= 0; I--)
            {
                var enchantment = FEnchantmentList.GetEnchantments(I);
                FEnchantments = FEnchantments + TSet<TCityEnchantmentID>.Of(((TCityEnchantment)enchantment).FEnchantmentID);
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
            FEnchantments = TSet<TCityEnchantmentID>.Of();
        }

        // City.pas:924
        public int AICityEnchantmentUpkeepPriority(TCityEnchantmentID ID)
        {
            int Result;
            switch (ID)
            {
                case TCityEnchantmentID.ceiFountainOfLife:
                    Result = GetSize() * 25 + 600;
                    break;
                case TCityEnchantmentID.ceiGoldRush:
                    Result = GetSize() * 25 + 600;
                    break;
                case TCityEnchantmentID.ceiAntiMagicShell:
                    Result = GetSize() * 25 + 700;
                    break;
                case TCityEnchantmentID.ceiWarmonger:
                    Result = GetSize() * 25 + 600;
                    break;
                default:
                    Result = 0;
                    break;
            }

            return Result;
        }

        // City.pas:935
        public void ActivateEnchantment(TCityEnchantment Enchantment)
        {
            Enchantment.Owner = FEnchantmentList;
            Enchantment.Activate(Enchantment.FPlayerIndex);
            FEnchantments = FEnchantments + TSet<TCityEnchantmentID>.Of(Enchantment.FEnchantmentID);
            if (Enchantment.FEnchantmentID == TCityEnchantmentID.ceiFountainOfLife)
            {
                ExecuteFountainOfLife();
            }

            if (Enchantment.FEnchantmentID == TCityEnchantmentID.ceiGoldRush)
            {
                UpdateIncome();
            }

            if (Enchantment.FEnchantmentID == TCityEnchantmentID.ceiAntiMagicShell)
            {
                FOverlay.PlaySetupShellAnimation();
            }
        }

        // City.pas:945
        public void DeactivateEnchantment(TCityEnchantment Enchantment)
        {
            Enchantment.Owner = null;
            Enchantment.Deactivate((byte)Enchantment.FPlayerIndex);
            UpdateActiveEnchantments();
            if (Enchantment.FEnchantmentID == TCityEnchantmentID.ceiGoldRush)
            {
                UpdateIncome();
            }

            Enchantment.Release();
            Update();
        }

        // City.pas:955
        public void ListEnhancementsInfo(TStrings List)
        {
            List.Add(RtlUnit.Format(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.World.Structures.TCity.UpgradeLevelXRStr), [IntToStr(FUpgradeLevel)]));
            if (FWallType == 1)
            {
                List.Add(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.World.Structures.TCity.CityWoodenWallRStr));
            }

            if (FWallType == 2)
            {
                List.Add(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.World.Structures.TCity.CityStoneWallRStr));
            }

            if (FEnchantments.Contains(TCityEnchantmentID.ceiFountainOfLife))
            {
                List.Add(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.FountainOfLifeRStr) + ": " + Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameTables.EnchantmentRStr));
            }

            if (FEnchantments.Contains(TCityEnchantmentID.ceiGoldRush))
            {
                List.Add(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.GoldRushRStr) + ": " + Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameTables.EnchantmentRStr));
            }

            if (FEnchantments.Contains(TCityEnchantmentID.ceiAntiMagicShell))
            {
                List.Add(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.AntiMagicShellRStr) + ": " + Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameTables.EnchantmentRStr));
            }

            if (FEnchantments.Contains(TCityEnchantmentID.ceiWarmonger))
            {
                List.Add(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.WarmongerRStr) + ": " + Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameTables.EnchantmentRStr));
            }
        }

        // City.pas:970
        public void StartTownQuake()
        {
            FMode = FMode + TSet<TCityModeElement>.Of(TCityModeElement.cmTownQuake);
        }

        // City.pas:973
        public void StopTownQuake()
        {
            FMode = FMode - TSet<TCityModeElement>.Of(TCityModeElement.cmTownQuake);
        }

        // City.pas:976
        public void ExecuteAnarchy()
        {
            FMode = FMode + TSet<TCityModeElement>.Of(TCityModeElement.cmAnarchy);
            CheckRebellion();
            FMode = FMode - TSet<TCityModeElement>.Of(TCityModeElement.cmAnarchy);
        }

        // City.pas:983
        public bool ExecuteFountainOfLife()
        {
            TArmyHSList Armies;
            TImageSequenceAnimation Animation;
            bool PlaySound;
            Armies = new TArmyHSList(null);
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
                    if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.VisibleForPlayer(xhx, yhx, lhx, Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer))
                    {
                        Animation = new TImageSequenceAnimation(null);
                        Animation.SetImageSequence(Aow.Game.Engine.TAoWHSSet.AoWHSSet.FAbilityControl.GetAbility(47).FImageSequences.ImageSequences[0]);
                        Animation.FAnimationLevel = 27;
                        var lhx2 = Armies.GetArmyHS(I).GetLhx();
                        var yhx2 = Armies.GetArmyHS(I).GetYhx();
                        var xhx2 = Armies.GetArmyHS(I).GetXhx();
                        Aow.Game.World.Map.TAoWHSMap.AoWHSMap.PlaceHX(Animation, xhx2, yhx2, lhx2);
                        bool condition = PlaySound || Armies.GetArmyHS(I).FArmy.FPlayer == Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer;
                        if (!condition)
                        {
                            var lhx3 = Armies.GetArmyHS(I).GetLhx();
                            var yhx3 = Armies.GetArmyHS(I).GetYhx();
                            var xhx3 = Armies.GetArmyHS(I).GetXhx();
                            condition = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Watching(xhx3, yhx3, lhx3, 1);
                        }

                        PlaySound = condition;
                    }

                    Result = true;
                }
            }

            if (PlaySound)
            {
                Aow.Game.Engine.TAoWHSSet.AoWHSSet.FAbilityControl.GetAbility(47).FSFXLibrary.Play(0);
            }

            Armies?.Dispose();
            return Result;
        }

        // City.pas:1015
        public void UpdateAIMigration()
        {
            TMigrationList Selection;
            int Value;
            int BestIndex;
            int BestValue;
            TProductionItem Item;
            if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FDay != 0 && !Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Status.Contains(THSMapStatusElement.msEditting))
            {
                if (FPlayer > 0)
                {
                    bool condition = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(FPlayer).FPlayerType != TPlayerType.ptHuman && (TSet<TCityModeElement>.Of(TCityModeElement.cmWarmode, TCityModeElement.cmRazed) & FMode) == TSet<TCityModeElement>.Of();
                    if (condition)
                    {
                        var activeAction = GetActiveAction();
                        condition = TSet<TCityAction>.Of(TCityAction.caNone, TCityAction.caTrade).Contains(activeAction);
                    }

                    bool test = condition;
                    if (test)
                    {
                        var playerRelation = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FRaces.GetRace(FRace).GetPlayerRelation(FPlayer);
                        test = TSet<TStatus>.Of(TStatus.sHostile, TStatus.sWary, TStatus.sNeutral).Contains(playerRelation);
                    }

                    if (test)
                    {
                        Selection = CreateMigrationSelection();
                        BestValue = 0;
                        BestIndex = -1;
                        for (int I = 0, last = Selection.Count - 1; I <= last; I++)
                        {
                            var itemRelation = Selection.GetItems(I);
                            if (TSet<TStatus>.Of(TStatus.sPolite, TStatus.sFriendly).Contains(itemRelation.FRelation))
                            {
                                var itemRace = Selection.GetItems(I);
                                bool passed = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.RaceToPlayer(itemRace.FRace) == -1;
                                if (!passed)
                                {
                                    var migrationItem = Selection.GetItems(I);
                                    var player = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(FPlayer);
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
                            if (Value > BestValue || Value == BestValue && Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Random(2) == 1)
                            {
                                BestValue = Value;
                                BestIndex = I;
                            }
                        }

                        if (BestIndex != -1)
                        {
                            FProduction.ExecuteDeleteProductions();
                            Item = new TProductionItem(null);
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
                        condition = TSet<TCityAction>.Of(TCityAction.caMigrate, TCityAction.caLoot).Contains(activeAction);
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
            bool condition = FPlayer > 0 && Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(FPlayer).FPlayerType != TPlayerType.ptHuman && (TSet<TCityModeElement>.Of(TCityModeElement.cmWarmode, TCityModeElement.cmRazed) & FMode) == TSet<TCityModeElement>.Of();
            if (condition)
            {
                var activeAction = GetActiveAction();
                condition = !TSet<TCityAction>.Of(TCityAction.caNone, TCityAction.caTrade).Contains(activeAction);
            }

            if (condition)
            {
                var playerRelation = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FRaces.GetRace(FRace).GetPlayerRelation(FPlayer);
                bool contains = TSet<TStatus>.Of(TStatus.sHostile, TStatus.sWary, TStatus.sNeutral).Contains(playerRelation);
                if (contains)
                {
                    bool test = false;
                    if (GetActiveOrder() is Migrate migrate)
                    {
                        var playerRelation2 = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FRaces.GetRace(migrate.Race).GetPlayerRelation(FPlayer);
                        test = TSet<TStatus>.Of(TStatus.sPolite, TStatus.sFriendly).Contains(playerRelation2);
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
        public bool MoveArmiesOutOfCity(TSet<TMoveOutOfCitySelectionElement> Selection)
        {
            TMapLevel MapLevel;
            TArmyHS Army;
            int Count;
            THNWalkData Walk = default;
            bool Result = true;
            Count = GetTerrainCount();
            MapLevel = (TMapLevel)Owner.Owner;
            // { This eliminated copy preserves the original saved registers. }
            Aow.Game.World.Movement.THNWalkData.InitHNWalk(ref Walk, FXhx, FYhx, 0);
            while (Walk.HN < Count && Result)
            {
                if (FResource.TerrainList.GetTerrain(Walk.HN) >= 0 && Walk.X >= 0 && Walk.Y >= 0 && Walk.X < MapLevel.Width && Walk.Y < MapLevel.Height)
                {
                    Army = (TArmyHS)MapLevel.GetField(Walk.X, Walk.Y).FindOwnedChild(131607);
                    if (Army != null && (!Selection.Contains(TMoveOutOfCitySelectionElement.msSkipOwnPlayer) || Army.FArmy.FPlayer != FPlayer))
                    {
                        var lhx = GetLhx();
                        Result = Army.PlaceEx((sbyte)Walk.X, (sbyte)Walk.Y, lhx, TSet<TArmyPlaceModeElement>.Of(TArmyPlaceModeElement.apSafe, TArmyPlaceModeElement.apScanArea));
                    }
                }

                Aow.Game.World.Movement.THNWalkData.WalkToNextHN(ref Walk);
            }

            return Result;
        }

        // City.pas:1256
        public int GetSize() => ((TCityResource)FResource).FSize;

        // City.pas:1259
        public int GetObtainUnitsValue()
        {
            int Count;
            TMapLevel MapLevel;
            THNWalkData Walk = default;
            TArmyHS Army;
            ref var walk = ref Walk;
            int Result = 0;
            Count = GetTerrainCount();
            MapLevel = (TMapLevel)Owner.Owner;
            var yhx = GetYhx();
            var xhx = GetXhx();
            Aow.Game.World.Movement.THNWalkData.InitHNWalk(ref Walk, xhx, yhx, 0);
            while (walk.HN < Count)
            {
                if (FResource.TerrainList.GetTerrain(walk.HN) >= 0 && walk.X >= 0 && walk.Y >= 0 && walk.X < MapLevel.Width && walk.Y < MapLevel.Height)
                {
                    Army = (TArmyHS)MapLevel.GetField(walk.X, walk.Y).FindOwnedChild(131607);
                    if (Army != null)
                    {
                        Result += Army.FArmy.ObtainValue(255);
                    }
                }

                Aow.Game.World.Movement.THNWalkData.WalkToNextHN(ref Walk);
            }

            return Result;
        }

        // City.pas:1348
        public int UpgradeCost()
        {
            var size = GetSize();
            return Aow.Game.World.Structures.TCity.UpgradePrices[size - 1];
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
                Reason = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.World.Structures.TCity.MaxUpgradeLevelReachedRStr);
            }

            return Result;
        }

        // City.pas:1371
        public bool Upgrade(bool Queue)
        {
            bool Result;
            TProductionItem Item;
            string Reason = "";
            if (CanUpgrade(Queue, ref Reason) && !Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(FPlayer).GetBusy())
            {
                Item = new TProductionItem(null);
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
                    Result = Aow.Game.World.Structures.TCity.WoodWallPrices[size - 1];
                    break;
                case 1:
                    var size2 = GetSize();
                    Result = Aow.Game.World.Structures.TCity.StoneWallPrices[size2 - 1];
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
                Reason = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.World.Structures.TCity.MaxFortifyLevelReachedRStr);
            }

            return Result;
        }

        // City.pas:1415
        public bool Fortify(bool Queue)
        {
            bool Result;
            TProductionItem Item;
            string Reason = "";
            if (CanFortify(Queue, ref Reason) && !Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(FPlayer).GetBusy())
            {
                Item = new TProductionItem(null);
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
            TProductionItem Item;
            string Reason = "";
            if (CanTrade(Queue, ref Reason) && !Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(FPlayer).GetBusy())
            {
                Item = new TProductionItem(null);
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
        public bool CanLoot(ref string Reason) => !FMode.Contains(TCityModeElement.cmRazed);

        // City.pas:1499
        public bool Loot()
        {
            bool Result;
            TProductionItem Item;
            string Reason = "";
            if (CanLoot(ref Reason) && !Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(FPlayer).GetBusy())
            {
                Item = new TProductionItem(null);
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
            THNWalkData Walk = default;
            TMapLevel MapLevel;
            int Radius;
            TCity City;
            if (CanMigrate(Race, ref Reason))
            {
                if (Race != FRace)
                {
                    Radius = -1;
                    MapLevel = (TMapLevel)Owner.Owner;
                    Aow.Game.World.Movement.THNWalkData.InitHNWalk(ref Walk, FXhx, FYhx, 1);
                    while (Walk.Radius <= 20 && Radius == -1)
                    {
                        if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < MapLevel.Width && Walk.Y < MapLevel.Height)
                        {
                            City = (TCity)MapLevel.GetField(Walk.X, Walk.Y).FindNoneTransparentHS(typeof(TCity));
                            if (City != null && City.FRace == Race)
                            {
                                Radius = Walk.Radius;
                            }
                        }

                        Aow.Game.World.Movement.THNWalkData.WalkToNextHN(ref Walk);
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
                Result = Aow.Game.World.Structures.TCity.RaceInCity(Race, FPlayer);
                I = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.Count;
                while (I != 1 && !Result)
                {
                    I -= 1;
                    Player = (sbyte)I;
                    Result = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetDiplomaticRelation((sbyte)(Player << 0), FPlayer) == TDiplomaticRelation.drAlliance && Aow.Game.World.Structures.TCity.RaceInCity(Race, Player);
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
            TProductionItem Item;
            string Reason = "";
            if (CanMigrate(Race, ref Reason) && !Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(FPlayer).GetBusy())
            {
                Item = new TProductionItem(null);
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
        public TMigrationItem CreateMigrationItem(sbyte Race)
        {
            TMigrationItem Result;
            string Reason = "";
            if (CanMigrate(Race, ref Reason))
            {
                Result = new TMigrationItem(null);
                Result.FRace = Race;
                Result.FTime = MigrationTime(Race);
                Result.FRelation = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FRaces.GetRace(Race).GetPlayerRelation(FPlayer);
            }
            else
            {
                Result = null;
            }

            return Result;
        }

        // City.pas:1589
        public TMigrationList CreateMigrationSelection()
        {
            TMigrationItem Item;
            TMigrationList Result = new TMigrationList(null);
            for (int I = 0, last = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FRaces.Count - 1; I <= last; I++)
            {
                if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FRaces.GetRace(I).FEnabled)
                {
                    Item = CreateMigrationItem((sbyte)I);
                    if (Item != null && TSet<TStatus>.Of(TStatus.sNeutral, TStatus.sPolite, TStatus.sFriendly).Contains(Item.FRelation))
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
            TCivilStatus Status;
            int Chance;
            TCityRebellionEventLog Log;
            TUnitList Units;
            if (FPlayer != 0)
            {
                Status = GetCivilStatus();
                if (TSet<TCivilStatus>.Of(TCivilStatus.csUnruly, TCivilStatus.csUnrest).Contains(Status) && !GetRazed())
                {
                    if (Status == TCivilStatus.csUnruly)
                    {
                        Chance = 50;
                    }
                    else
                    {
                        Chance = 10;
                    }

                    if (Chance >= Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Random(100))
                    {
                        MoveArmiesOutOfCity(TSet<TMoveOutOfCitySelectionElement>.Of());
                        Log = new TCityRebellionEventLog(null);
                        Log.FPlayer = FPlayer;
                        Log.FCityName = GetName();
                        Log.FXhx = GetXhx();
                        Log.FYhx = GetYhx();
                        Log.FLhx = GetLhx();
                        var yhx = GetYhx();
                        var lhx = GetLhx();
                        var xhx = GetXhx();
                        Aow.Game.Events.TEventLog.DistributeLocationEventLog(Log, FPlayer, xhx, yhx, lhx, false);
                        Log.Release();
                        SetPlayer(0);
                        Units = new TUnitList(null);
                        GenerateRebelUnits(Units);
                        for (int I = Units.Count - 1; I >= 0; I--)
                        {
                            var lhx2 = GetLhx();
                            var yhx2 = GetYhx();
                            var xhx2 = GetXhx();
                            Units.GetUnit(I).Place(xhx2, yhx2, lhx2, 0, TSet<TUnitPlaceModeElement>.Of());
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
            TArmyHSList Armies;
            if (!GetRazed() && FDefendersMode != TDefendersGenerateMode.dgNone && !FMode.Contains(TCityModeElement.cmWarmode) && FPlayer == 0)
            {
                Armies = new TArmyHSList(null);
                ListArmies(Armies);
                if (FDefendersMode == TDefendersGenerateMode.dgRandom)
                {
                    FDefendersMode = (TDefendersGenerateMode)(Random(3) + 2);
                }

                Value = ((int)FDefendersMode - 2) * 25 + 25;
                Value += (GetSize() - 1) * Value / 2;
                for (int I = 0, last = Armies.Count - 1; I <= last; I++)
                {
                    Value -= Armies.GetArmyHS(I).FArmy.ObtainValue(255);
                }

                if (Value > 0)
                {
                    RandSeed = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Random(1048575);
                    var raceResource = Aow.Game.Engine.TAoWHSSet.AoWHSSet.FRaceResources.GetRaceResource(FRace);
                    var lhx = GetLhx();
                    var yhx = GetYhx();
                    Aow.Game.Magic.Abilities.AbilityRules.PlaceRandomUnits(GetXhx(), yhx, lhx, 0, raceResource.FUnitIndexes, FUpgradeLevel, Value, TSet<TUnitType>.Of(TUnitType.utHumanoid, TUnitType.utCreature));
                }

                Armies?.Dispose();
            }
        }

        // City.pas:1755
        public void ListIncomeInfo(TStrings Names, TStrings Values)
        {
            int Income;
            Income = GetSize() * 5;
            Names.Add(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.BaseIncomeRStr));
            Values.Add(IntToStr(Income));
            Income += GetCropCount() * 2;
            var cropCount = GetCropCount();
            Names.Add(RtlUnit.Format(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.World.Structures.TCity.XCropFieldsRStr), [IntToStr(cropCount)]));
            Values.Add(IntToStr(GetCropCount() * 2));
            if (TSet<TCityAction>.Of(TCityAction.caNone, TCityAction.caTrade).Contains(GetActiveAction()))
            {
                Names.Add(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.ProducingMerchandiseRStr));
                Values.Add(IntToStr(Round((double)Income * 0.25)));
                Income += Round((double)Income * 0.25);
            }

            if (FEnchantments.Contains(TCityEnchantmentID.ceiGoldRush))
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
        public void ListCivilStatusInfo(TStrings List)
        {
            TCombatPredictor Predictor;
            TArmyHSList Armies;
            TUnitList Units;
            TAbstractUnit U;
            if (FPlayer != 0 && FRace >= 0)
            {
                var playerRelation = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FRaces.GetRace(FRace).GetPlayerRelation(FPlayer);
                var status = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameTables.TStatusRStr[(int)playerRelation]);
                List.Add(RtlUnit.Format(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.World.Structures.TCity.RaceRelationXRStr), [status]));
                if (Aow.Game.World.Structures.TCity.RaceTerrain[FRace].Penalty.Contains((byte)TerrainType))
                {
                    var terrainPenaltyX = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.World.Structures.TCity.TerrainPenaltyXRStr);
                    var terrainType = Aow.Game.Engine.TAoWHSSet.AoWHSSet.TerrainTypes[TerrainType];
                    List.Add(RtlUnit.Format(terrainPenaltyX, [terrainType]));
                }

                if (Aow.Game.World.Structures.TCity.RaceTerrain[FRace].Bonus.Contains((byte)TerrainType))
                {
                    var terrainBonusX = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.World.Structures.TCity.TerrainBonusXRStr);
                    var terrainType = Aow.Game.Engine.TAoWHSSet.AoWHSSet.TerrainTypes[TerrainType];
                    List.Add(RtlUnit.Format(terrainBonusX, [terrainType]));
                }

                Predictor = new TCombatPredictor();
                Armies = new TArmyHSList(null);
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

                Units = new TUnitList(null);
                GenerateRebelUnits(Units);
                for (int J = 0, last = Units.Count - 1; J <= last; J++)
                {
                    Predictor.AddOpponent(Units.GetUnit(J), 0);
                }

                if (Predictor.Execute() == TCombatResult.crVictory)
                {
                    if (Predictor.GetSuperiority() / 4 >= 75)
                    {
                        var strong = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.World.Structures.TCity.StrongRStr);
                        List.Add(RtlUnit.Format(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.World.Structures.TCity.OccupationForcesXRStr), [strong]));
                    }
                    else
                    {
                        var average = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.World.Structures.TCity.AverageRStr);
                        List.Add(RtlUnit.Format(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.World.Structures.TCity.OccupationForcesXRStr), [average]));
                    }
                }
                else
                {
                    var weak = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.World.Structures.TCity.WeakRStr);
                    List.Add(RtlUnit.Format(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.World.Structures.TCity.OccupationForcesXRStr), [weak]));
                }

                Predictor?.Dispose();
                Units?.Dispose();
                Armies?.Dispose();
            }
        }

        // City.pas:1861
        public TCivilStatus GetCivilStatus()
        {
            TCivilStatus Result;
            TStatus Relation;
            int Value;
            int ArmyValue;
            int Bonus;
            TCombatPredictor Predictor;
            TArmyHSList Armies;
            TUnitList Units;
            TAbstractUnit U;
            if (FPlayer != 0 && !FMode.Contains(TCityModeElement.cmWarmode))
            {
                if (FMode.Contains(TCityModeElement.cmAnarchy))
                {
                    Relation = TStatus.sHostile;
                }
                else
                {
                    Relation = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FRaces.GetRace(FRace).GetPlayerRelation(FPlayer);
                }

                switch (Relation)
                {
                    case TStatus.sHostile:
                        Value = -35;
                        break;
                    case TStatus.sWary:
                        Value = -25;
                        break;
                    case TStatus.sNeutral:
                        Value = 0;
                        break;
                    case TStatus.sPolite:
                        Value = 15;
                        break;
                    case TStatus.sFriendly:
                        Value = 25;
                        break;
                    default:
                        Value = 0;
                        break;
                }

                Value = Value + FWallType * 5 + FUpgradeLevel * 5;
                if (FRace >= 0)
                {
                    if (Aow.Game.World.Structures.TCity.RaceTerrain[FRace].Penalty.Contains((byte)TerrainType))
                    {
                        Value += -10;
                    }

                    if (Aow.Game.World.Structures.TCity.RaceTerrain[FRace].Bonus.Contains((byte)TerrainType))
                    {
                        Value += 10;
                    }

                    Predictor = new TCombatPredictor();
                    ArmyValue = 0;
                    Armies = new TArmyHSList(null);
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
                                if (FMode.Contains(TCityModeElement.cmAnarchy))
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
                    if (TSet<TStatus>.Of(TStatus.sHostile, TStatus.sWary).Contains(Relation))
                    {
                        Units = new TUnitList(null);
                        GenerateRebelUnits(Units);
                        for (int I = 0, last = Units.Count - 1; I <= last; I++)
                        {
                            Predictor.AddOpponent(Units.GetUnit(I), 0);
                            if (FMode.Contains(TCityModeElement.cmAnarchy))
                            {
                                Predictor.AddOpponent(Units.GetUnit(I), 0);
                            }
                        }

                        if (Predictor.Execute() == TCombatResult.crVictory)
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
                        if (TSet<TStatus>.Of(TStatus.sPolite, TStatus.sFriendly).Contains(Relation))
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
                        Result = TCivilStatus.csUnruly;
                    }
                    else
                    {
                        if (Value < -10)
                        {
                            Result = TCivilStatus.csUnrest;
                        }
                        else
                        {
                            if (Value < 10)
                            {
                                Result = TCivilStatus.csStable;
                            }
                            else
                            {
                                if (Value < 30)
                                {
                                    if (TSet<TStatus>.Of(TStatus.sHostile, TStatus.sWary).Contains(Relation))
                                    {
                                        Result = TCivilStatus.csOppressed;
                                    }
                                    else
                                    {
                                        Result = TCivilStatus.csContent;
                                    }
                                }
                                else
                                {
                                    if (TSet<TStatus>.Of(TStatus.sHostile, TStatus.sWary).Contains(Relation))
                                    {
                                        Result = TCivilStatus.csSlavery;
                                    }
                                    else
                                    {
                                        Result = TCivilStatus.csCheerful;
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    Result = TCivilStatus.csStable;
                }
            }
            else
            {
                Result = TCivilStatus.csStable;
            }

            return Result;
        }

        // City.pas:2034
        public void SetUpgradeLevel(byte Level)
        {
            TRaceResource RaceResource = default;
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
                RaceResource = Aow.Game.Engine.TAoWHSSet.AoWHSSet.FRaceResources.GetRaceResource(FRace);
                for (int I = RaceResource.FUnitIndexes.Count - 1; I >= 0; I--)
                {
                    if (RaceResource.FUnitIndexes[I] != -1)
                    {
                        var unitIndex = RaceResource.FUnitIndexes[I];
                        if (FProduction.FUnitProductions.IndexOf(unitIndex) == -1)
                        {
                            var unitIndex2 = RaceResource.FUnitIndexes[I];
                            var unitResource = Aow.Game.Engine.TAoWHSSet.AoWHSSet.FUnitResources.GetUnitResource(unitIndex2);
                            var size = GetSize();
                            UnitLevel = Aow.Game.World.Structures.TCity.UnitLevelToCityLevel(unitResource.FLevel, size);
                            if (UnitLevel != -1 && UnitLevel <= FUpgradeLevel)
                            {
                                Researched = UnitLevel == 1 && !(RaceResource.FSpecialUnitIndexes.Count >= 4 && RaceResource.FUnitIndexes[I] == RaceResource.FSpecialUnitIndexes[3]);
                                var unitIndex3 = RaceResource.FUnitIndexes[I];
                                FProduction.FUnitProductions.Add(unitIndex3, Researched);
                            }
                        }
                    }
                }

                Aow.Game.World.Structures.TCity.SortingCity = this;
                FProduction.FUnitProductions.FChildren.Sort(Aow.Game.World.Structures.TCity.CompareCityProductions);
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
        public void InitCombat(TCombat Combat, TArmyHS Army)
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
            int Result = Aow.Game.World.Structures.TCity.UpgradePrices[size - 1] * FUpgradeLevel;
            if (FWallType == 1)
            {
                var size2 = GetSize();
                Result += Aow.Game.World.Structures.TCity.WoodWallPrices[size2 - 1];
            }

            if (FWallType == 2)
            {
                var size2 = GetSize();
                var size3 = GetSize();
                Result = Result + Aow.Game.World.Structures.TCity.WoodWallPrices[size2 - 1] + Aow.Game.World.Structures.TCity.StoneWallPrices[size3 - 1];
            }

            return Result;
        }

        // City.pas:2241
        public int JoinAmount(sbyte Player)
        {
            TArmyHSList Armies;
            int I;
            TArmy Army;
            bool CanJoin;
            int Value;
            byte Relation;
            byte Selection;
            int Result = 0;
            Armies = new TArmyHSList(null);
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
                    Relation = (byte)Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FRaces.GetRace(FRace).GetPlayerRelation(Player);
                    Result += Aow.Game.World.Structures.TCity.TCity_JoinAmount__JoinPercent[(int)(TStatus)Relation] * Value / 100;
                }
                else
                {
                    Result = -1;
                }
            }
            else
            {
                var playerRelation = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FRaces.GetRace(FRace).GetPlayerRelation(Player);
                if (TSet<TStatus>.Of(TStatus.sPolite, TStatus.sFriendly).Contains(playerRelation))
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
        public bool IncommingNegotiateRequest(TNegotiateRequestEventLog Request)
        {
            bool Result;
            int Amount;
            if (FPlayer == 0 && !FMode.Contains(TCityModeElement.cmWarmode))
            {
                Amount = JoinAmount(Request.FPlayer);
                if (Amount != -1)
                {
                    Request.FObject = TNegotiateObject.noCity;
                    var name = GetName();
                    Request.FText = RtlUnit.Format(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.World.Structures.TCity.CityJoinHeaderRStr), [name]);
                    Request.FGold = Amount;
                    if (Request.FGold == 0)
                    {
                        var cityFreeJoin = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.World.Structures.TCity.CityFreeJoinRStr);
                        Request.FJoinMessage.Text = cityFreeJoin;
                    }
                    else
                    {
                        var format = RtlUnit.Format(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.World.Structures.TCity.CityTributeJoinRStr), [IntToStr(Request.FGold)]);
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
        public bool IncommingNegotiateOffer(TNegotiateTE Offer)
        {
            bool Result;
            int Amount;
            TArmyHSList Armies;
            TLocationTextEventLog Log;
            if (Offer.FObject == TNegotiateObject.noCity)
            {
                Amount = JoinAmount(Offer.FPlayer);
                if (Offer.FOffer == TNegotiateOffer.noOfferBribe && Amount <= Offer.FGold && Amount != -1)
                {
                    if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Offer.FPlayer).FGems >= Offer.FGold)
                    {
                        SetPlayer(Offer.FPlayer);
                        Armies = new TArmyHSList(null);
                        ListArmies(Armies);
                        for (int I = 0, last = Armies.Count - 1; I <= last; I++)
                        {
                            if (Armies.GetArmyHS(I).FArmy.FPlayer == 0)
                            {
                                Armies.GetArmyHS(I).FArmy.SetPlayer(Offer.FPlayer);
                            }
                        }

                        Armies?.Dispose();
                        Log = new TLocationTextEventLog(null);
                        Log.FPlayer = FPlayer;
                        var name = GetName();
                        Log.FText = RtlUnit.Format(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.World.Structures.TCity.XOverthrownRStr), [name]);
                        var lhx = GetLhx();
                        var yhx = GetYhx();
                        Log.SetLocation(GetXhx(), yhx, lhx);
                        var yhx2 = GetYhx();
                        var lhx2 = GetLhx();
                        var xhx = GetXhx();
                        Aow.Game.Events.TEventLog.DistributeLocationEventLog(Log, FPlayer, xhx, yhx2, lhx2, false);
                        Log.Release();
                        Offer.Accept();
                    }
                    else
                    {
                        var notEnoughGold = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.NotEnoughGoldRStr);
                        Aow.Game.World.Map.TAoWHSMap.AoWHSMap.ShowMessage(notEnoughGold);
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
            TUnitUserList Units;
            Units = new TUnitUserList(null);
            ListUnits(Units);
            for (int I = Units.Count - 1; I >= 0; I--)
            {
                if (Units.GetUnit(I).CanJoin(Player))
                {
                    Units.DeleteUnit(Units.GetUnit(I));
                }
            }

            // { Original uses the race index as the default relation. }
            sbyte Result = Aow.Game.Units.TUnitList.UnitsToIndependentRelation(Units, FRace);
            Units?.Dispose();
            return Result;
        }

        // City.pas:2345
        public sbyte GetIndependentRelation()
        {
            TUnitUserList Units;
            Units = new TUnitUserList(null);
            ListUnits(Units);
            // { Original uses the race index as the default relation. }
            sbyte Result = Aow.Game.Units.TUnitList.UnitsToIndependentRelation(Units, FRace);
            Units?.Dispose();
            return Result;
        }

        // City.pas:2354
        public void ReturnBattleFieldInfo(ref TBattleFieldInfo Info)
        {
            int Value;
            TDiplomaticRelation Relation;
            if (Info.Requests.Contains(TBattleFieldInfoRequestsElement.bfrEffectiveStrength) && FPlayer > 0)
            {
                Value = 5000;
                var playerRelation = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FRaces.GetRace(FRace).GetPlayerRelation(FPlayer);
                if (TSet<TStatus>.Of(TStatus.sHostile, TStatus.sWary).Contains(playerRelation))
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
            Visible = VisibleForPlayer(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer);
            if (FEnchantmentsVisible ^ Visible)
            {
                FEnchantmentsVisible = Visible;
                if (FEnchantments != TSet<TCityEnchantmentID>.Of())
                {
                    Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FGlobalMagicControl.TriggerChangedEvent();
                }
            }
        }

        // City.pas:2503
        public Aow.Game.Draw.Images.TLibraryImage GetTopCityActionImage(ProductionOrder Order) => GetCityActionImage(Order, 0, FRace != -1, 40, 43);

        // City.pas:2539
        public Aow.Game.Draw.Images.TLibraryImage GetMiddleCityActionImage(ProductionOrder Order) => GetCityActionImage(Order, 1, FRace != -1, 41, 44);

        // City.pas:2575
        public Aow.Game.Draw.Images.TLibraryImage GetBottomCityActionImage(ProductionOrder Order) => GetCityActionImage(Order, 2, FRace >= 0, 42, 45);

        // Row 0 to 2 of the city's map image while Order runs: an image sequence of the city when the
        // order names one, else the race image (Image on the surface, UndergroundImage below it).
        private Aow.Game.Draw.Images.TLibraryImage GetCityActionImage(ProductionOrder Order, int Row, bool HasRace, int Image, int UndergroundImage)
        {
            Aow.Game.Draw.Images.TLibraryImage Result;
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
                        Result = Aow.Game.Engine.TAoWHSSet.AoWHSSet.FRaceResources.GetRaceResource(FRace).FImageLibrary.GetSafe(Image);
                    }
                    else
                    {
                        Result = Aow.Game.Engine.TAoWHSSet.AoWHSSet.FRaceResources.GetRaceResource(FRace).FImageLibrary.GetSafe(UndergroundImage);
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
        public void GenerateRebelUnits(TUnitList units)
        {
            int saved = RandSeed;
            RandSeed = GetXhx() * 10 + Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FInitialRandomSeed + GetYhx();
            Aow.Game.Engine.Collections.TIntegerList.FillWithRandomUnits(Aow.Game.Engine.TAoWHSSet.AoWHSSet.FRaceResources.GetRaceResource(FRace).FUnitIndexes, units, 2, GetSize() * 25, TSet<TUnitType>.Of(TUnitType.utHumanoid, TUnitType.utCreature));
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

        internal static global::Aow.Game.Session.TRaceTerrainInfo[] RaceTerrain = new global::Aow.Game.Session.TRaceTerrainInfo[12]
        {
            new global::Aow.Game.Session.TRaceTerrainInfo
            {
                Bonus = TSet<byte>.WithBounds(0, 15),
                Penalty = TSet<byte>.WithBounds(0, 15)
            },
            new global::Aow.Game.Session.TRaceTerrainInfo
            {
                Bonus = TSet<byte>.WithBounds(0, 15).With((byte)2),
                Penalty = TSet<byte>.WithBounds(0, 15).With((byte)3).With((byte)6)
                    .With((byte)13)
            },
            new global::Aow.Game.Session.TRaceTerrainInfo
            {
                Bonus = TSet<byte>.WithBounds(0, 15).With((byte)0),
                Penalty = TSet<byte>.WithBounds(0, 15).With((byte)2).With((byte)5)
            },
            new global::Aow.Game.Session.TRaceTerrainInfo
            {
                Bonus = TSet<byte>.WithBounds(0, 15).With((byte)3).With((byte)6)
                    .With((byte)13),
                Penalty = TSet<byte>.WithBounds(0, 15).With((byte)2)
            },
            new global::Aow.Game.Session.TRaceTerrainInfo
            {
                Bonus = TSet<byte>.WithBounds(0, 15).With((byte)1),
                Penalty = TSet<byte>.WithBounds(0, 15).With((byte)5)
            },
            new global::Aow.Game.Session.TRaceTerrainInfo
            {
                Bonus = TSet<byte>.WithBounds(0, 15).With((byte)1),
                Penalty = TSet<byte>.WithBounds(0, 15).With((byte)5)
            },
            new global::Aow.Game.Session.TRaceTerrainInfo
            {
                Bonus = TSet<byte>.WithBounds(0, 15).With((byte)12),
                Penalty = TSet<byte>.WithBounds(0, 15)
            },
            new global::Aow.Game.Session.TRaceTerrainInfo
            {
                Bonus = TSet<byte>.WithBounds(0, 15),
                Penalty = TSet<byte>.WithBounds(0, 15)
            },
            new global::Aow.Game.Session.TRaceTerrainInfo
            {
                Bonus = TSet<byte>.WithBounds(0, 15).With((byte)12),
                Penalty = TSet<byte>.WithBounds(0, 15)
            },
            new global::Aow.Game.Session.TRaceTerrainInfo
            {
                Bonus = TSet<byte>.WithBounds(0, 15),
                Penalty = TSet<byte>.WithBounds(0, 15)
            },
            new global::Aow.Game.Session.TRaceTerrainInfo
            {
                Bonus = TSet<byte>.WithBounds(0, 15).With((byte)12),
                Penalty = TSet<byte>.WithBounds(0, 15)
            },
            new global::Aow.Game.Session.TRaceTerrainInfo
            {
                Bonus = TSet<byte>.WithBounds(0, 15).With((byte)5),
                Penalty = TSet<byte>.WithBounds(0, 15).With((byte)1)
            }
        };

        private static global::Aow.Game.Engine.Buffers.FixedArray_int_5 TCity_JoinAmount__JoinPercent = global::Aow.Game.Engine.Buffers.FixedArray_int_5.From([200, 175, 150, 125, 100]);
        private static global::Aow.Game.Engine.Buffers.FixedArray_byte_4 UpgradePrices = global::Aow.Game.Engine.Buffers.FixedArray_byte_4.From((global::System.ReadOnlySpan<byte>)new byte[4] { 250, 250, 250, 250 });
        private static global::Aow.Game.Engine.Buffers.FixedArray_byte_4 WoodWallPrices = global::Aow.Game.Engine.Buffers.FixedArray_byte_4.From((global::System.ReadOnlySpan<byte>)new byte[4] { 20, 40, 60, 80 });
        private static global::Aow.Game.Engine.Buffers.FixedArray_byte_4 StoneWallPrices = global::Aow.Game.Engine.Buffers.FixedArray_byte_4.From((global::System.ReadOnlySpan<byte>)new byte[4] { 20, 40, 60, 80 });
        private static readonly string CityWarningRStr = "Warning";
        private static readonly string CityWoodenWallRStr = "Wooden Wall";
        private static readonly string CityStoneWallRStr = "Stone Wall";
        private static global::Aow.Game.World.Structures.TCity SortingCity;

        private static bool RaceInCity(sbyte Race, sbyte Player)
        {
            bool flag = false;
            global::Aow.Game.Session.TPlayer players = global::Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Player);
            for (int i = 0; i != players.FStructures.Count; i++)
            {
                if (flag)
                {
                    break;
                }
                if (players.FStructures.GetStructure(i) is global::Aow.Game.World.Structures.TCity)
                {
                    flag = !players.FStructures.GetStructure(i).GetBuilding() && ((global::Aow.Game.World.Structures.TCity)players.FStructures.GetStructure(i)).FRace == Race;
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
            global::Aow.Game.Units.TUnitProduction tUnitProduction = (global::Aow.Game.Units.TUnitProduction)Item1;
            int num;
            if (global::Aow.Game.Engine.TAoWHSSet.AoWHSSet.FUnitResources.GetUnitResource(tUnitProduction.FUnitIndex) != null)
            {
                num = global::Aow.Game.Engine.TAoWHSSet.AoWHSSet.FUnitResources.GetUnitResource(tUnitProduction.FUnitIndex).FLevel * 2;
                if (SortingCity.FRace != -1 && global::Aow.Game.Engine.TAoWHSSet.AoWHSSet.FRaceResources.GetRaceResource(SortingCity.FRace).FSpecialUnitIndexes.Count > 3 && global::Aow.Game.Engine.TAoWHSSet.AoWHSSet.FRaceResources.GetRaceResource(SortingCity.FRace).FSpecialUnitIndexes[3] == tUnitProduction.FUnitIndex)
                {
                    num++;
                }
            }
            else
            {
                num = 0;
            }
            global::Aow.Game.Units.TUnitProduction tUnitProduction2 = (global::Aow.Game.Units.TUnitProduction)Item2;
            int num2;
            if (global::Aow.Game.Engine.TAoWHSSet.AoWHSSet.FUnitResources.GetUnitResource(tUnitProduction2.FUnitIndex) != null)
            {
                num2 = global::Aow.Game.Engine.TAoWHSSet.AoWHSSet.FUnitResources.GetUnitResource(tUnitProduction2.FUnitIndex).FLevel * 2;
                if (SortingCity.FRace != -1 && global::Aow.Game.Engine.TAoWHSSet.AoWHSSet.FRaceResources.GetRaceResource(SortingCity.FRace).FSpecialUnitIndexes.Count > 3 && global::Aow.Game.Engine.TAoWHSSet.AoWHSSet.FRaceResources.GetRaceResource(SortingCity.FRace).FSpecialUnitIndexes[3] == tUnitProduction2.FUnitIndex)
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
                num3 = ((global::Aow.Game.Units.TUnitProduction)Item1).FUnitIndex - ((global::Aow.Game.Units.TUnitProduction)Item2).FUnitIndex;
            }
            return num3;
        }
    }

    // FSize
    public partial class TCityResource : TPlayerCropStructureResource
    {
        public TCityResource(TECustomNode owner) : base(owner)
        {
            FRazeable = true;
        }

        // City.pas:2628
        public new const int ClassID = 131337;

        public byte FSize;

        // City.pas:2622

        // City.pas:2669
        public override void MsgProc(ref TEMsg Msg, TEObject Sender)
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
        public void ShowCity(int X, int Y, in TSurfaceDesc SurfaceDesc, TCity City)
        {
            int dX;
            int dY;
            Aow.Game.Draw.Images.TLibraryImage Image;
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
                if (City.GetSelected() && (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Frame & 6) == 6)
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
        public override string ResourceEditName() => Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.World.Structures.TCityResource.CityRStr);

        private static readonly string CityRStr = "City";

    }
}

namespace Aow.Game.Events
{

    // FXhx, FYhx, FLhx, FCityName
    public partial class TCityRebellionEventLog : TEventLog
    {
        public TCityRebellionEventLog(TECustomNode owner) : base(owner)
        { }

        // City.pas:2675
        public new const int ClassID = 131346;

        public int FXhx;
        public int FYhx;
        public int FLhx;
        public string FCityName = "";

        // City.pas:2680
        public override void ReadWrite(TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwInteger(ref FXhx, 20);
            Stream.rwInteger(ref FYhx, 21);
            Stream.rwInteger(ref FLhx, 22);
            Stream.rwString(ref FCityName, 23);
        }

        // City.pas:2689
        public override string GetText() => RtlUnit.Format(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Events.TCityRebellionEventLog.CityRebellionRStr), [FCityName]);

        // City.pas:2699
        public override bool Execute(bool Mode, TExecuteTrigger Trigger, TExecuteDoneEvent Done)
        {
            bool Result = default;
            bool ShowEvent;
            ShowEvent = Trigger == TExecuteTrigger.etUser;
            ShowEvent = ShowEvent || Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayMode == TPlayMode.pmTurn && Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.GetClassicWindowPopupEvents().Contains(TWindowPopupEvent.wpRebellion);
            ShowEvent = ShowEvent || Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayMode == TPlayMode.pmSimultaneous && Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.GetSimultaneousWindowPopupEvents().Contains(TWindowPopupEvent.wpRebellion);
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

    public partial class TAICityUpgradeBudget : TAIBudget
    {
        // City.pas:2775
        public new const int ClassID = 131349;

        public TAICityUpgradeBudget(TECustomNode owner) : base(owner)
        { }

    }

    public partial class TAICityResearchBudget : TAIBudget
    {
        // City.pas:2780
        public new const int ClassID = 131350;

        public TAICityResearchBudget(TECustomNode owner) : base(owner)
        { }

    }

    public partial class TAICityFortifyBudget : TAIBudget
    {
        // City.pas:2785
        public new const int ClassID = 131351;

        public TAICityFortifyBudget(TECustomNode owner) : base(owner)
        { }

    }

    // FCityCount, FProductionLevelCounts, FMaxProductionLevel, FProductionRatio
    public partial class TCityAIPA : TAIPlayerAction, IAIPlayerActionClass
    {
        public TCityAIPA(TECustomNode owner) : base(owner)
        { }

        public int FCityCount;
        public FixedArray_int_4 FProductionLevelCounts;
        public int FMaxProductionLevel;
        public float FProductionRatio;

        // City.pas:3130
        public override bool Process()
        {
            TPlayer Player;
            TAIBudgetManager Manager;
            TAIBudget UpgradeBudget;
            TAIBudget ResearchBudget;
            TAIBudget FortifyBudget;
            UpdateStatistics();
            Player = FControl.FPlayer;
            if (Player.FPlayerIndex != 0)
            {
                Manager = Player.FAIBudget;
                UpgradeBudget = (TAIBudget)Manager.FBudgets.FindChild(131349);
                if (UpgradeBudget == null)
                {
                    UpgradeBudget = new TAICityUpgradeBudget(null);
                    UpgradeBudget.Owner = Manager.FBudgets;
                    UpgradeBudget.FRequiredGold = 250;
                    UpgradeBudget.FDays = 7;
                }

                ResearchBudget = (TAIBudget)Manager.FBudgets.FindChild(131350);
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
                    ResearchBudget = new TAICityResearchBudget(null);
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
                FortifyBudget = (TAIBudget)Manager.FBudgets.FindChild(131351);
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
                    if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FDay > 5)
                    {
                        FortifyBudget = new TAICityFortifyBudget(null);
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
        public bool ValidResearchCity(TCity City)
        {
            bool Result = City.FProduction.GetProduceUnitLevelMax() < City.FUpgradeLevel;
            bool result = Result;
            if (result)
            {
                var activeAction = City.GetActiveAction();
                bool contains = TSet<TCityAction>.Of(TCityAction.caNone, TCityAction.caTrade).Contains(activeAction);
                if (!contains)
                {
                    var cityActiveAction = City.GetActiveAction();
                    contains = TSet<TCityAction>.Of(TCityAction.caProduce).Contains(cityActiveAction) && City.FProduction.FSettings.FTurnsLeft == City.FProduction.FSettings.FTurnsTotal;
                }

                result = contains;
            }

            return result;
        }

        // City.pas:2802
        public bool ValidResearchBudgetCity(TCity City) => City.FProduction.GetProduceUnitLevelMax() != City.FUpgradeLevel || City.GetActiveAction() == TCityAction.caUpgrade;

        // City.pas:2807
        public void ValidateResearchBudget(TAIBudget Budget)
        {
            TPlayer Player;
            int Count;
            int Total;
            int Priority;
            TPlayerStructure Structure;
            Player = FControl.FPlayer;
            Count = 0;
            Total = 0;
            for (int I = Player.FStructures.Count - 1; I >= 0; I--)
            {
                Structure = Player.FStructures.GetStructure(I);
                if (Structure is TCity)
                {
                    Total += 1;
                    if (ValidResearchBudgetCity((TCity)Structure))
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
        public bool ResearchProduction(TAIBudget ResearchBudget, TAIBudget UpgradeBudget)
        {
            TDoubleItemList<TCity> Cities;
            bool Valid;
            TPlayer Player;
            int UnitIndex;
            int Index;
            int Value;
            int Priority;
            TPlayerStructure Structure;
            TCity City;
            TProductionItem Production;
            bool Result = false;
            Player = FControl.FPlayer;
            Cities = new TDoubleItemList<TCity>();
            for (int I = Player.FStructures.Count - 1; I >= 0; I--)
            {
                Structure = Player.FStructures.GetStructure(I);
                if (Structure is TCity && ValidResearchCity((TCity)Structure))
                {
                    City = (TCity)Structure;
                    Valid = false;
                    for (int J = City.FProduction.FUnitProductions.Count - 1; J >= 0; J--)
                    {
                        UnitIndex = City.FProduction.FUnitProductions.GetItem(J).FUnitIndex;
                        if (UnitIndex != -1)
                        {
                            Valid = Valid || Aow.Game.Engine.TAoWHSSet.AoWHSSet.FUnitResources.GetUnitResource(UnitIndex).FLevel == City.FUpgradeLevel && !City.FProduction.FUnitProductions.GetItem(J).FResearched && City.ResearchCost(J) <= ResearchBudget.FGold + UpgradeBudget.FGold;
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

                Index = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Random(Index + 1);
                City = Cities[Index].A;
                RandSeed = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Random(16777215);
                Index = Cities[Index].A.FProduction.FindOptimalResearchProduction(null, ResearchBudget.FGold + UpgradeBudget.FGold);
                if (Index != -1)
                {
                    City.FProduction.ExecuteDeleteProductions();
                    Production = new TProductionItem(null);
                    Production.Order = new ResearchSpell(Index);
                    City.FProduction.ExecuteAddProduction(Production);
                    Production?.Dispose();
                    Index = City.ResearchCost(Index);
                    if (Index > ResearchBudget.FGold)
                    {
                        Index -= ResearchBudget.FGold;
                        Player.FAIBudget.BudgetExpenseMade(ResearchBudget.FGold, 0, ResearchBudget, TBudgetExpenseMode.bemBudget);
                        Player.FAIBudget.BudgetExpenseMade(Index, 0, UpgradeBudget, TBudgetExpenseMode.bemBudget);
                    }
                    else
                    {
                        Player.FAIBudget.BudgetExpenseMade(Index, 0, ResearchBudget, TBudgetExpenseMode.bemBudget);
                    }

                    Result = true;
                }
            }

            Cities?.Dispose();
            return Result;
        }

        // City.pas:2909
        public bool ValidUpgradeCity(TCity City)
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
                bool contains = TSet<TCityAction>.Of(TCityAction.caNone, TCityAction.caTrade).Contains(activeAction);
                if (!contains)
                {
                    var cityActiveAction = City.GetActiveAction();
                    contains = TSet<TCityAction>.Of(TCityAction.caProduce).Contains(cityActiveAction) && City.FProduction.FSettings.FTurnsLeft == City.FProduction.FSettings.FTurnsTotal;
                }

                result = contains;
            }

            return result;
        }

        // City.pas:2921
        public bool ValidUpgradeBudgetCity(TCity City)
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
        public bool UpgradeCity(TAIBudget Budget)
        {
            bool Result;
            TPlayer Player;
            TDoubleItemList<TCity> Cities;
            int Priority;
            TPlayerStructure Structure;
            TCity City;
            TProductionItem Production;
            Player = FControl.FPlayer;
            Cities = new TDoubleItemList<TCity>();
            for (int I = Player.FStructures.Count - 1; I >= 0; I--)
            {
                // { Preserve the original runtime class check. }
                Structure = Player.FStructures.GetStructure(I);
                if (Structure is TCity && ValidUpgradeCity((TCity)Structure))
                {
                    City = (TCity)Structure;
                    Priority = City.GetSize() * 2 + (City.FProductionPriority + City.FWallType * 3);
                    Cities.AddAB(City, Priority);
                }
            }

            Cities.SortDescendingItemB();
            if (Cities.Count != 0)
            {
                City = Cities[0].A;
                City.FProduction.ExecuteDeleteProductions();
                Production = new TProductionItem(null);
                Production.Order = new Aow.Game.World.Structures.Upgrade();
                City.FProduction.ExecuteAddProduction(Production);
                Production?.Dispose();
                Player.FAIBudget.BudgetExpenseMade(250, 0, Budget, TBudgetExpenseMode.bemBudget);

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
        public void ValidateUpgradeBudget(TAIBudget Budget)
        {
            TPlayer Player;
            int Count;
            int Priority;
            TPlayerStructure Structure;
            Player = FControl.FPlayer;
            Count = 0;
            for (int I = Player.FStructures.Count - 1; I >= 0; I--)
            {
                Structure = Player.FStructures.GetStructure(I);
                if (Structure is TCity)
                {
                    if (ValidUpgradeBudgetCity((TCity)Structure))
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
        public bool ValidFortifyCity(TCity City)
        {
            string Reason = "";
            bool Result = City.CanFortify(false, ref Reason);
            Result = Result && City.FWallType == 0 || City.FWallType == 1 && City.GetSize() > 2 && City.FUpgradeLevel > 1 && Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FDay >= 30;
            bool result = Result;
            if (result)
            {
                var activeAction = City.GetActiveAction();
                bool contains = TSet<TCityAction>.Of(TCityAction.caNone, TCityAction.caTrade).Contains(activeAction);
                if (!contains)
                {
                    var cityActiveAction = City.GetActiveAction();
                    contains = TSet<TCityAction>.Of(TCityAction.caProduce).Contains(cityActiveAction) && City.FProduction.FSettings.FTurnsLeft == City.FProduction.FSettings.FTurnsTotal;
                }

                result = contains;
            }

            return result;
        }

        // City.pas:3009
        public bool ValidFortifyBudgetCity(TCity City) => City.FWallType == 0 || City.FWallType == 1 && City.GetSize() > 2 && City.FUpgradeLevel > 1 && Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FDay >= 30;

        // City.pas:3015
        public bool FortifyCity(TAIBudget Budget)
        {
            bool Result;
            TPlayer Player;
            TDoubleItemList<TCity> Cities;
            int Priority;
            TPlayerStructure Structure;
            TCity City;
            TProductionItem Production;
            Player = FControl.FPlayer;
            Cities = new TDoubleItemList<TCity>();
            for (int I = Player.FStructures.Count - 1; I >= 0; I--)
            {
                // { Preserve the original runtime class check. }
                Structure = Player.FStructures.GetStructure(I);
                if (Structure is TCity && ValidFortifyCity((TCity)Structure))
                {
                    City = (TCity)Structure;
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
                Player.FAIBudget.BudgetExpenseMade(fortifyCost, 0, Budget, TBudgetExpenseMode.bemBudget);
                Production = new TProductionItem(null);
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
        public void ValidateFortifyBudget(TAIBudget Budget)
        {
            TPlayer Player;
            int Count;
            int Priority;
            TPlayerStructure Structure;
            Player = FControl.FPlayer;
            Count = 0;
            for (int I = Player.FStructures.Count - 1; I >= 0; I--)
            {
                Structure = Player.FStructures.GetStructure(I);
                if (Structure is TCity)
                {
                    if (ValidFortifyBudgetCity((TCity)Structure))
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
            TPlayerStructure Structure;
            Count = 0;
            for (int I = FControl.FPlayer.FStructures.Count - 1; I >= 0; I--)
            {
                Structure = FControl.FPlayer.FStructures.GetStructure(I);
                if (Structure is TCity)
                {
                    _ = (TCity)Structure;
                    var city = (TCity)Structure;
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
            TCity City;
            FCityCount = 0;
            FMaxProductionLevel = 0;
            Available = 0;
            Busy = 0;
            FProductionLevelCounts = default;
            for (int I = FControl.FPlayer.FStructures.Count - 1; I >= 0; I--)
            {
                if (FControl.FPlayer.FStructures.GetStructure(I) is TCity)
                {
                    FCityCount += 1;
                    City = (TCity)FControl.FPlayer.FStructures.GetStructure(I);
                    if (TSet<TCityAction>.Of(TCityAction.caNone, TCityAction.caTrade, TCityAction.caProduce).Contains(City.GetActiveAction()))
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

    public partial class TCityProductionEventLog : TProductionEventLog
    {
        public TCityProductionEventLog(TECustomNode owner) : base(owner)
        { }

        // City.pas:3195
        public new const int ClassID = 131834;

        // City.pas:3205
        public override bool Execute(bool Mode, TExecuteTrigger Trigger, TExecuteDoneEvent Done)
        {
            TCity City;
            if (Trigger == TExecuteTrigger.etUser)
            {
                City = GetCity();
                if (City != null && Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer == City.FPlayer)
                {
                    Aow.Game.World.Map.TAoWHSMap.AoWHSMap.CenterViewTo(FXhx, FYhx, FLhx);
                    Aow.Game.World.Map.TAoWHSMap.AoWHSMap.TriggerExecuteEventLog(this, true, Done);
                }
            }

            return false;
        }

        // City.pas:3200
        public TCity GetCity() => (TCity)Aow.Game.World.Map.TAoWHSMap.AoWHSMap.MapContainer.GetField(FXhx, FYhx, FLhx).FindNoneTransparentHS(typeof(TCity));

    }
}

namespace Aow.Game.World.Structures
{

    [LegacyObject]
    public partial class TRegistrator
    {
        // City.pas:3306
        public static void MapLoaded(THSMap Sender)
        {
            if (Sender == Aow.Game.World.Map.TAoWHSMap.AoWHSMap)
            {
                Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FAIPlayerActionManager.RegisterClasses([AIPlayerActionClass.Of<TCityAIPA>()]);
            }
        }

        public TRegistrator()
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

    public enum TCivilStatus : byte
    {
        csUnruly = 0,
        csUnrest = 1,
        csStable = 2,
        csContent = 3,
        csCheerful = 4,
        csOppressed = 5,
        csSlavery = 6
    }

    public enum TCityEnchantmentID : byte
    {
        ceiFountainOfLife = 0,
        ceiGoldRush = 1,
        ceiAntiMagicShell = 2,
        ceiWarmonger = 3
    }

    public enum TCityAction : byte
    {
        caNone = 0,
        caTrade = 1,
        caUpgrade = 2,
        caFortify = 3,
        caMigrate = 4,
        caLoot = 5,
        caRaze = 6,
        caResearch = 7,
        caProduce = 8
    }
}

namespace Aow.Game.Draw
{

    public enum TCityOverlayMode : byte
    {
        omIdle = 0,
        omShowRazeAnimation = 1,
        omLooting = 2
    }
}

namespace Aow.Game.World.Structures
{

    public enum TDefendersGenerateMode : byte
    {
        dgNone = 0,
        dgRandom = 1,
        dgWeak = 2,
        dgAverage = 3,
        dgStrong = 4
    }
}
