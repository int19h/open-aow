// Translated from UnHolyG.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Aow.Units.Engine;
using Aow.Units.GFXE;
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngine;
using Aow.Units.ILTer;
using static Aow.Units.EventLog.EventLog;
using Aow.Units.GlobalTargetSpells;
using Aow.Units.Animation;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;

namespace Aow.Units.UnHolyG
{
    public enum AnonymousEnum_hgSummoning : byte
    {
        hgSummoning = 0
    }

    // FMode, FPlayer, FDuration, FOriginalOverlay
    public partial class TUnHolyGround : TILTerrainMO
    {
        public TUnHolyGround(TECustomNode owner) : base(owner)
        {}

        // UnHolyG.pas:93
        public new const int ClassID = 131985;

        public TSet<AnonymousEnum_hgSummoning> FMode;
        public sbyte FPlayer;
        public sbyte FDuration;
        public sbyte FOriginalOverlay;
        // UnHolyG.pas:204
        public override void ReadWrite(TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwShortintEx(ref FPlayer, -1, 40);
            Stream.rwShortintEx(ref FDuration, -1, 41);
            Stream.rwShortintEx(ref FOriginalOverlay, -1, 42);
        }

        // UnHolyG.pas:243
        public override void MsgProc(ref TEMsg Msg, TEObject Sender)
        {
            base.MsgProc(ref Msg, Sender);
            if (Msg.MsgID == 537001986)
            {
                NewTurn((sbyte)Msg.Value);
            }
        }

        // UnHolyG.pas:249
        public override void Show(int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
        {
            if (!FMode.Contains(AnonymousEnum_hgSummoning.hgSummoning))
            {
                base.Show(XhpSurf, YhpSurf, SurfaceDesc);
            }
        }

        // UnHolyG.pas:190
        public override bool PlaceOnMap()
        {
            bool Result = default;
            Result = base.PlaceOnMap();
            if (Result)
            {
                var call__1 = GetYhx();
                var call__2 = GetXhx();
                UpdateMapFields((TMapLevel)Owner.Owner, call__2, call__1);
            }

            return Result;
        }

        // UnHolyG.pas:196
        public override void RemoveFromMap()
        {
            TMapLevel Level = default;
            Level = (TMapLevel)Owner.Owner;
            base.RemoveFromMap();
            UpdateMapFields(Level, FXhx, FYhx);
        }

        // UnHolyG.pas:222
        public override void MapFieldMsgProc(ref TMapFieldMsg Msg)
        {
            base.MapFieldMsgProc(ref Msg);
            if (Msg.MsgID == 4418)
            {
                var call__3 = FResource.TerrainList.Terrain[Msg.HN];
                if (call__3 >= 0)
                {
                    ArmyPlaced(Msg.Param[0].As<TArmyHS>());
                    return;
                }
            }
            else
            {
                if (Msg.MsgID == 4384)
                {
                    var call__4 = FResource.TerrainList.Terrain[Msg.HN];
                    if (call__4 >= 0)
                    {
                        var call__5 = CanDoDamage(Msg.Param[0].As<TArmyHS>().FArmy);
                        if (call__5)
                        {
                            Msg.Result = 1;
                        }
                    }
                }
                else
                {
                    if (Msg.MsgID == 4402)
                    {
                        var call__6 = FResource.TerrainList.Terrain[Msg.HN];
                        if (call__6 >= 0 && FPlayer > 0 && FDuration > 0)
                        {
                            RestoreTerrain();
                        }
                    }
                }
            }
        }

        // UnHolyG.pas:212
        public override int CanMoveOver(in THSSenderInfo Info)
        {
            int Result = default;
            var call__7 = FResource.TerrainList.Terrain[Info.HN];
            if (call__7 >= 0)
            {
                var call__8 = CanDoDamage(((TMoveControl)Info.Sender).FSettings.FUnits);
                if (!call__8)
                {
                    Result = 1;
                }
                else
                {
                    Result = 0;
                    return Result;
                }
            }
            else
            {
                Result = 1;
            }

            return Result;
        }

        // UnHolyG.pas:184
        public override void UpdateMapField(in THSMapFieldInfo Info)
        {
            var call__9 = FResource.TerrainList.Terrain[Info.HN];
            if (call__9 >= 0)
            {
                ((TAoWMapField)Info.MapField).FFieldFlags = ((TAoWMapField)Info.MapField).FFieldFlags | 8192;
            }
        }

        // UnHolyG.pas:96
        public void TriggerArmyDamage(TArmyHS Army)
        {
            TArmyEventLog Log = default;
            int I = default;
            int Damage = default;
            int Damaged = default;
            TAbstractUnit AUnit = default;
            Damaged = 0;
            var call__10 = AoWHSMap.Random(16777215);
            RandSeed = call__10;
            Log = new TArmyEventLog(null);
            var call__11 = TranslateRStr(UnholyGroundDamageRStr);
            Log.FArmyText = call__11;
            var call__12 = TranslateRStr(UnholyGroundDamageRStr);
            Log.FArmyMessages.Add(call__12);
            Log.FArmyIcon = 2;
            var army = Army.FArmy;
            {
                I = 0;
                int end__14 = army.Count - 1;
                for (; I <= end__14; I += 1)
                {
                    AUnit = army.GetUnit(I);
                    var call__15 = AUnit.GetAlignment();
                    bool bool__16 = call__15 != TAlignment.alNone;
                    if (bool__16)
                    {
                        var call__17 = AUnit.GetAlignment();
                        bool__16 = alsPureGood.Contains(call__17);
                    }

                    bool bool__18 = bool__16;
                    if (bool__18)
                    {
                        var call__19 = AUnit.GetAbilityEnabled(1);
                        bool__18 = !call__19;
                    }

                    if (bool__18)
                    {
                        Damage = AUnit.ExecuteDamageRole(4, 3, TSet<TDamageType>.Of(TDamageType.dtDeath));
                        if (Damage > AUnit.GetHitPoints())
                        {
                            Damage = AUnit.GetHitPoints();
                        }

                        if (Damage != 0)
                        {
                            var call__20 = AUnit.GetHitPoints();
                            AUnit.SetHitPoints((sbyte)(call__20 - Damage));
                            Damaged += 1;
                        }

                        Log.FDamageValues.Add((byte)Damage);
                        Log.FDamageTypes.Add((int)TDamageType.dtDeath);
                    }
                    else
                    {
                        Log.FDamageValues.Add(0);
                        Log.FDamageTypes.Add((int)TDamageType.dtDeath);
                    }
                }
            }

            Log.FPlayer = army.FPlayer;
            var call__21 = army.GetArmyHS();
            Log.FArmyView.Setup(call__21.FArmy, 255);

            Log.ValidateGameOver();
            if (Damaged != 0)
            {
                var call__22 = GetYhx();
                var call__23 = GetLhx();
                var call__24 = GetXhx();
                DistributeLocationEventLog(Log, Log.FPlayer, call__24, call__22, call__23, true);
            }

            Log.Release();
            Army.FArmy.Validate();
        }

        // UnHolyG.pas:142
        public void ArmyPlaced(TArmyHS Army)
        {
            if (!AoWHSMap.Status.Contains(THSMapStatusElement.msEditting))
            {
                TriggerArmyDamage(Army);
            }
        }

        // UnHolyG.pas:147
        public void RestoreTerrain()
        {
            TMapField Field = default;
            sbyte Overlay = default;
            Field = (TMapField)Owner;
            Overlay = FOriginalOverlay;
            Destroy();
            if (Overlay > 0)
            {
                AoWHSMap.PlaceTerrain(Field.Xhx, Field.Yhx, Field.Lhx, Field.TerrainType, Overlay);
            }
        }

        // UnHolyG.pas:157
        public void NewTurn(sbyte Player)
        {
            TArmyHS Army = default;
            var call__25 = Owner.FindOwnedChild(131607);
            Army = (TArmyHS)call__25;
            if (Army != null && Player == Army.FArmy.FPlayer)
            {
                TriggerArmyDamage(Army);
            }

            if (Player == FPlayer && FDuration > 0)
            {
                FDuration = (sbyte)(FDuration - 1);
                if (FDuration == 0)
                {
                    RestoreTerrain();
                }
            }
        }

        // UnHolyG.pas:169
        public bool CanDoDamage(TUnitList Units)
        {
            bool Result = default;
            int I = default;
            TAbstractUnit AUnit = default;
            Result = false;
            var call__26 = Units.Count;
            I = call__26;
            while (!Result && I != 0)
            {
                I -= 1;
                AUnit = Units.GetUnit(I);
                var call__27 = AUnit.GetImmunityTypes();
                bool bool__28 = !call__27.Contains(TDamageType.dtDeath);
                if (bool__28)
                {
                    var call__29 = AUnit.GetAlignment();
                    bool__28 = call__29 != TAlignment.alNone;
                }

                bool bool__30 = bool__28;
                if (bool__30)
                {
                    var call__31 = AUnit.GetAlignment();
                    bool__30 = alsPureGood.Contains(call__31);
                }

                bool bool__32 = bool__30;
                if (bool__32)
                {
                    var call__33 = AUnit.GetAbilityEnabled(1);
                    bool__32 = !call__33;
                }

                Result = bool__32;
            }

            return Result;
        }

        // UnHolyG.pas:254
        public void ShowSummonAnimation(TAnimation Anim, int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
        {
            TMapField Field = default;
            var call__34 = Anim.GetLhx();
            var call__35 = Anim.GetYhx();
            var call__36 = Anim.GetXhx();
            var call__37 = AoWHSMap.MapContainer.GetField(call__36, call__35, call__34);
            Field = call__37;
            var call__38 = Field.FindOwnedChild(131985);
            if (this == call__38 && AoWEngine.FAnimationDetail != 0 && Anim.FFrameCounter == 0)
            {
                bool bool__39 = AoWHSMap.FSeatedPlayer == Anim.FTag;
                if (!bool__39)
                {
                    var call__40 = GetLhx();
                    var call__41 = GetYhx();
                    var call__42 = GetXhx();
                    var call__43 = AoWHSMap.WatchingTerrain(call__42, call__41, call__40, 0);
                    bool__39 = call__43;
                }

                if (bool__39)
                {
                    var call__44 = AoWHSSet.FSpellControl.GetSpell(14);
                    call__44.FCastSounds.Play(0);
                }
            }
        }

        // UnHolyG.pas:265
        public void NewFrameSummonAnimation(TAnimation Anim, int FrameCounter)
        {
            TMapField Field = default;
            var call__45 = Anim.GetLhx();
            var call__46 = Anim.GetYhx();
            var call__47 = Anim.GetXhx();
            var call__48 = AoWHSMap.MapContainer.GetField(call__47, call__46, call__45);
            Anim = (TAnimation)(object)call__48;
            if (this == ((TMapField)(object)Anim).FindOwnedChild(131985) && FrameCounter == 9)
            {
                FMode = FMode - TSet<AnonymousEnum_hgSummoning>.Of(AnonymousEnum_hgSummoning.hgSummoning);
            }
        }

    }

    public partial class TUnHolyGroundResource : TILTerrainMOR
    {
        public TUnHolyGroundResource(TECustomNode owner) : base(owner)
        {}

        // UnHolyG.pas:278
        public new const int ClassID = 131986;

        // UnHolyG.pas:283
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 131985;
            return Result;
        }

        // UnHolyG.pas:273
        public override AnsiString ResourceEditName()
        {
            AnsiString Result = default;
            var call__49 = UnholyGroundRStr;
            var call__50 = TranslateRStr(call__49);
            Result = call__50;
            return Result;
        }

    }

    public partial class TUnholyGroundSpellTE : TGlobalTargetSpellTE
    {
        public TUnholyGroundSpellTE(TECustomNode owner) : base(owner)
        {}

        // UnHolyG.pas:288
        public new const int ClassID = 135943;

        // UnHolyG.pas:340
        public override bool Process(TAbstractUnit A1, int A2)
        {
            bool Result = default;
            base.Process(A1, A2);
            CreateUnholyGround(A1);
            AoWHSMap.ValidateTerrainObservation(A1.FPlayer, FLocation.X, FLocation.Y, FLocation.L, 1);
            var call__51 = AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
            call__51.CastingDone(A1, FEventMana);
            Result = false;
            return Result;
        }

        // UnHolyG.pas:293
        public void CreateUnholyGround(TAbstractUnit Source)
        {
            TMapLevel Level = default;
            THNWalkData[] Walk__cell = new THNWalkData[1];
            ref THNWalkData Walk = ref Walk__cell[0];
            TUnHolyGroundResource Resource = default;
            TAoWMapField MapF = default;
            TUnHolyGround Ground = default;
            TImageSequenceAnimation Anim = default;
            var walk = Walk;
            var call__53 = AoWHSMap.MapContainer.MapLevels[FLocation.L];
            Level = call__53;
            InitHNWalk(ref Walk, FLocation.X, FLocation.Y, 0);
            var call__54 = AoWHSSet.MapObjectResourceList.FindChild(131986);
            Resource = (TUnHolyGroundResource)call__54;
            while (walk.Radius <= 1)
            {
                if (walk.X >= 0 && walk.Y >= 0 && walk.X < Level.Width && walk.Y < Level.Height)
                {
                    var call__55 = Level.GetField(walk.X, walk.Y);
                    MapF = (TAoWMapField)call__55;
                    var call__56 = AoWHSSet.FSpellControl.GetSpell(14);
                    var call__57 = ((TGlobalTargetSpell)call__56).ValidTargetMapF(MapF);
                    bool bool__58 = call__57;
                    if (bool__58)
                    {
                        var call__59 = MapF.GetArmyHS();
                        bool__58 = call__59 == null;
                    }

                    if (bool__58)
                    {
                        MapF.RestoreTerrain();
                        Ground = (TUnHolyGround)Resource.CreateResourceUser();
                        var call__60 = AoWHSMap.Random(11);
                        Ground.FDuration = (sbyte)(3 + (call__60 - 5) / 5);
                        Ground.FPlayer = Source.FPlayer;
                        Ground.FOriginalOverlay = MapF.OverlayType;
                        AoWHSMap.ClearTerrain(walk.X, walk.Y, FLocation.L, 0, -1);
                        var call__61 = AoWHSMap.PlaceHX(Ground, walk.X, walk.Y, FLocation.L);
                        if (call__61)
                        {
                            Ground.FMode = TSet<AnonymousEnum_hgSummoning>.Of(AnonymousEnum_hgSummoning.hgSummoning);
                            Anim = new TImageSequenceAnimation(null);
                            var call__62 = AoWHSSet.FSpellControl.GetSpell(14);
                            var call__63 = call__62.FCastImages.ImageSequences[0];
                            Anim.SetImageSequence(call__63);
                            Anim.FOnShow = Ground.ShowSummonAnimation;
                            Anim.FOnNewFrame = Ground.NewFrameSummonAnimation;
                            var call__64 = AoWHSMap.Random(15);
                            Anim.FFrameCounter = -call__64;
                            Anim.FTag = Source.FPlayer;
                            AoWHSMap.PlaceHX(Anim, walk.X, walk.Y, FLocation.L);
                        }
                        else
                        {
                            Ground.Free();
                        }
                    }
                }

                WalkToNextHN(ref Walk);
            }
        }

        // NetworkE.pas:230


    }

    public partial class TUnholyGroundSpell : TGlobalTargetSpell
    {
        public TUnholyGroundSpell(TECustomNode owner) : base(owner)
        {
            var call__65 = UnholyGroundsRStr;
            var call__66 = TranslateRStr(call__65);
            FSpellName = call__66;
            FSpellID = 14;
            FBaseManaCost = 5;
            FSpellLevel = 2;
            FMagicSphere = TMagicSphere.msDeath;
            FAboveGroundOnly = false;
        }

        // UnHolyG.pas:349


        // UnHolyG.pas:383
        public override void SetupTargetSelection(TAbstractUnit A1, TTargetSelectionControl A2)
        {
            int I = default;
            base.SetupTargetSelection(A1, A2);
            A2.FHNs.Clear();
            {
                I = 0;
                int end__67 = RadToHN(2) - 1;
                for (; I <= end__67; I += 1)
                {
                    A2.FHNs.Add(0);
                }
            }
        }

        // UnHolyG.pas:392
        public override bool TargetSelected(TAbstractUnit A1, int A2, int A3, int A4)
        {
            bool Result = default;
            TUnholyGroundSpellTE Event = default;
            Event = new TUnholyGroundSpellTE(null);
            Event.Initialize(FSpellID, (byte)A1.FPlayer, A1.FUnitID, FBaseManaCost);
            Event.SetLocation((sbyte)A2, (sbyte)A3, (sbyte)A4);
            var call__68 = AoWHSMap.FTokenManager.GetTokenControl(A1.FPlayer);
            call__68.AddEvent(Event);
            Event.Release();
            Result = true;
            return Result;
        }

        // UnHolyG.pas:368
        public override bool ValidTargetSelectionMapF(TMapField A1)
        {
            bool Result = default;
            TArmyHS Army = default;
            var call__69 = base.ValidTargetMapF(A1);
            Result = call__69 && TSet<byte>.WithBounds(0, 255).With(12).WithRange(1, 5).Contains((byte)A1.TerrainType);
            // { Original precedence accepts these overlays without the terrain test. }
            Result = (Result && A1.OverlayType == -1) || TSet<byte>.WithBounds(0, 255).With(9).WithRange(1, 4).Contains((byte)A1.OverlayType);
            Result = Result && A1.FindNoneTransparentHS(typeof(TStructure)) == null;
            if (Result)
            {
                Army = ((TAoWMapField)A1).GetArmyHS();
                if (Army != null)
                {
                    Result = !Army.VisibleForSeatedPlayer();
                }
            }

            return Result;
        }

        // UnHolyG.pas:360
        public override bool ValidTargetMapF(TMapField A1)
        {
            bool Result = default;
            var call__70 = base.ValidTargetMapF(A1);
            Result = call__70 && TSet<byte>.WithBounds(0, 255).With(12).WithRange(1, 5).Contains((byte)A1.TerrainType);
            // { Original precedence accepts these overlays without the terrain test. }
            Result = (Result && A1.OverlayType == -1) || TSet<byte>.WithBounds(0, 255).With(9).WithRange(1, 4).Contains((byte)A1.OverlayType);
            bool bool__71 = Result;
            if (bool__71)
            {
                var call__72 = A1.FindNoneTransparentHS(typeof(TStructure));
                bool__71 = call__72 == null;
            }

            bool bool__73 = bool__71;
            if (bool__73)
            {
                var call__74 = ((TAoWMapField)A1).GetArmyHS();
                bool__73 = call__74 == null;
            }

            Result = bool__73;
            return Result;
        }

        // UnHolyG.pas:62

    }

    public static partial class UnHolyG
    {
        // UnHolyG.pas:404
        public static void RegisterUnHolyGround(TAoWEngine AEngine)
        {
            AEngine.RegisterEClasses([EClass.Of<TUnHolyGround>(), EClass.Of<TUnHolyGroundResource>(), EClass.Of<TUnholyGroundSpellTE>()]);
            var call__75 = new TUnholyGroundSpell(null);
            AEngine.FGameSet.FSpellControl.RegisterSpell(call__75);
        }

        // UnHolyG.pas:410
        public static void UnRegisterUnholyGround(TAoWEngine AEngine)
        {
            AEngine.UnregisterEClasses([EClass.Of<TUnHolyGround>(), EClass.Of<TUnHolyGroundResource>(), EClass.Of<TUnholyGroundSpellTE>()]);
            AEngine.FGameSet.FSpellControl.UnregisterSpellID(14);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
