// Translated from HolyG.pas.
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

namespace Aow.Units.HolyG
{
    public enum AnonymousEnum_hgSummoning : byte
    {
        hgSummoning = 0
    }

    // FMode, FPlayer, FDuration, FOriginalOverlay
    public partial class THolyGround : TILTerrainMO
    {
        public THolyGround(TECustomNode owner) : base(owner)
        {}

        // HolyG.pas:92
        public new const int ClassID = 131987;

        public TSet<AnonymousEnum_hgSummoning> FMode;
        public sbyte FPlayer;
        public sbyte FDuration;
        public sbyte FOriginalOverlay;
        // HolyG.pas:201
        public override void ReadWrite(TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwShortintEx(ref FPlayer, -1, 40);
            Stream.rwShortintEx(ref FDuration, -1, 41);
            Stream.rwShortintEx(ref FOriginalOverlay, -1, 42);
        }

        // HolyG.pas:243
        public override void MsgProc(ref TEMsg Msg, TEObject Sender)
        {
            base.MsgProc(ref Msg, Sender);
            if (Msg.MsgID == 537001986)
            {
                NewTurn((sbyte)Msg.Value);
            }
        }

        // HolyG.pas:249
        public override void Show(int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
        {
            if (!FMode.Contains(AnonymousEnum_hgSummoning.hgSummoning))
            {
                base.Show(XhpSurf, YhpSurf, SurfaceDesc);
            }
        }

        // HolyG.pas:187
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

        // HolyG.pas:193
        public override void RemoveFromMap()
        {
            TMapLevel Level = default;
            Level = (TMapLevel)Owner.Owner;
            base.RemoveFromMap();
            UpdateMapFields(Level, FXhx, FYhx);
        }

        // HolyG.pas:219
        public override void MapFieldMsgProc(ref TMapFieldMsg Msg)
        {
            base.MapFieldMsgProc(ref Msg);
            if (Msg.MsgID == 4418)
            {
                var call__3 = FResource.TerrainList.Terrain[Msg.HN];
                if (call__3 >= 0 && !AoWHSMap.Status.Contains(THSMapStatusElement.msEditting))
                {
                    ArmyPlaced(Msg.Param[0].As<TArmyHS>());
                    return;
                }

                return;
            }

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

        // HolyG.pas:209
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

        // HolyG.pas:181
        public override void UpdateMapField(in THSMapFieldInfo Info)
        {
            var call__9 = FResource.TerrainList.Terrain[Info.HN];
            if (call__9 >= 0)
            {
                ((TAoWMapField)Info.MapField).FFieldFlags = ((TAoWMapField)Info.MapField).FFieldFlags | 8192;
            }
        }

        // HolyG.pas:95
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
            var call__11 = TranslateRStr(HolyGroundDamageRStr);
            Log.FArmyText = call__11;
            var call__12 = TranslateRStr(HolyGroundDamageRStr);
            Log.FArmyMessages.Add(call__12);
            Log.FArmyIcon = 2;
            Log.FPlayer = Army.FArmy.FPlayer;
            Log.FArmyView.Setup(Army.FArmy, 255);
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
                        bool__16 = alsPureEvil.Contains(call__17);
                    }

                    bool bool__18 = bool__16;
                    if (bool__18)
                    {
                        var call__19 = AUnit.GetAbilityEnabled(1);
                        bool__18 = !call__19;
                    }

                    if (bool__18)
                    {
                        Damage = AUnit.ExecuteDamageRole(3, 4, TSet<TDamageType>.Of(TDamageType.dtHoly));
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
                        Log.FDamageTypes.Add((int)TDamageType.dtHoly);
                    }
                    else
                    {
                        Log.FDamageValues.Add(0);
                        Log.FDamageTypes.Add((int)TDamageType.dtNone);
                    }
                }
            }

            Log.ValidateGameOver();
            if (Damaged != 0)
            {
                var call__21 = GetYhx();
                var call__22 = GetLhx();
                var call__23 = GetXhx();
                DistributeLocationEventLog(Log, Log.FPlayer, call__23, call__21, call__22, true);
            }

            Log.Release();
            Army.FArmy.Validate();
        }

        // HolyG.pas:139
        public void ArmyPlaced(TArmyHS Army)
        {
            if (!AoWHSMap.Status.Contains(THSMapStatusElement.msEditting))
            {
                TriggerArmyDamage(Army);
            }
        }

        // HolyG.pas:144
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

        // HolyG.pas:154
        public void NewTurn(sbyte Player)
        {
            TArmyHS Army = default;
            var call__24 = Owner.FindOwnedChild(131607);
            Army = (TArmyHS)call__24;
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

        // HolyG.pas:166
        public bool CanDoDamage(TUnitList Units)
        {
            bool Result = default;
            int I = default;
            TAbstractUnit AUnit = default;
            Result = false;
            var call__25 = Units.Count;
            I = call__25;
            while (!Result && I != 0)
            {
                I -= 1;
                AUnit = Units.GetUnit(I);
                var call__26 = AUnit.GetImmunityTypes();
                bool bool__27 = !call__26.Contains(TDamageType.dtHoly);
                if (bool__27)
                {
                    var call__28 = AUnit.GetAlignment();
                    bool__27 = call__28 != TAlignment.alNone;
                }

                bool bool__29 = bool__27;
                if (bool__29)
                {
                    var call__30 = AUnit.GetAlignment();
                    bool__29 = alsPureEvil.Contains(call__30);
                }

                bool bool__31 = bool__29;
                if (bool__31)
                {
                    var call__32 = AUnit.GetAbilityEnabled(1);
                    bool__31 = !call__32;
                }

                Result = bool__31;
            }

            return Result;
        }

        // HolyG.pas:254
        public void ShowSummonAnimation(TAnimation Anim, int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
        {
            TMapField Field = default;
            var call__33 = Anim.GetLhx();
            var call__34 = Anim.GetYhx();
            var call__35 = Anim.GetXhx();
            var call__36 = AoWHSMap.MapContainer.GetField(call__35, call__34, call__33);
            Field = call__36;
            var call__37 = Field.FindOwnedChild(131987);
            if (this == call__37 && AoWEngine.FAnimationDetail != 0 && Anim.FFrameCounter == 0)
            {
                bool bool__38 = AoWHSMap.FSeatedPlayer == Anim.FTag;
                if (!bool__38)
                {
                    var call__39 = GetLhx();
                    var call__40 = GetYhx();
                    var call__41 = GetXhx();
                    var call__42 = AoWHSMap.WatchingTerrain(call__41, call__40, call__39, 0);
                    bool__38 = call__42;
                }

                if (bool__38)
                {
                    var call__43 = AoWHSSet.FSpellControl.GetSpell(13);
                    call__43.FCastSounds.Play(0);
                }
            }
        }

        // HolyG.pas:265
        public void NewFrameSummonAnimation(TAnimation Anim, int FrameCounter)
        {
            TMapField Field = default;
            var call__44 = Anim.GetLhx();
            var call__45 = Anim.GetYhx();
            var call__46 = Anim.GetXhx();
            var call__47 = AoWHSMap.MapContainer.GetField(call__46, call__45, call__44);
            Anim = (TAnimation)(object)call__47;
            if (this == ((TMapField)(object)Anim).FindOwnedChild(131987) && FrameCounter == 9)
            {
                FMode = FMode - TSet<AnonymousEnum_hgSummoning>.Of(AnonymousEnum_hgSummoning.hgSummoning);
            }
        }

    }

    public partial class THolyGroundResource : TILTerrainMOR
    {
        public THolyGroundResource(TECustomNode owner) : base(owner)
        {}

        // HolyG.pas:276
        public new const int ClassID = 131988;

        // HolyG.pas:279
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 131987;
            return Result;
        }

        // HolyG.pas:273
        public override AnsiString ResourceEditName()
        {
            AnsiString Result = default;
            Result = TranslateRStr(HolyGroundRStr);
            return Result;
        }

    }

    public partial class THolyGroundSpellTE : TGlobalTargetSpellTE
    {
        public THolyGroundSpellTE(TECustomNode owner) : base(owner)
        {}

        // HolyG.pas:282
        public new const int ClassID = 135942;

        // HolyG.pas:329
        public override bool Process(TAbstractUnit A1, int A2)
        {
            bool Result = default;
            base.Process(A1, A2);
            CreateHolyGround(A1);
            AoWHSMap.ValidateTerrainObservation(A1.FPlayer, FLocation.X, FLocation.Y, FLocation.L, 1);
            var call__48 = AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
            call__48.CastingDone(A1, FEventMana);
            Result = false;
            return Result;
        }

        // HolyG.pas:285
        public void CreateHolyGround(TAbstractUnit Caster)
        {
            TMapLevel Level = default;
            THNWalkData Walk = default;
            THolyGroundResource Resource = default;
            TAoWMapField Field = default;
            THolyGround Ground = default;
            TImageSequenceAnimation Anim = default;
            var call__49 = AoWHSMap.MapContainer.GetMapLevel(FLocation.L);
            Level = call__49;
            InitHNWalk(ref Walk, FLocation.X, FLocation.Y, 0);
            var call__50 = AoWHSSet.MapObjectResourceList.FindChild(131988);
            Resource = (THolyGroundResource)call__50;
            while (Walk.Radius <= 1)
            {
                if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < Level.Width && Walk.Y < Level.Height)
                {
                    var call__51 = Level.Field[Walk.X, Walk.Y];
                    Field = (TAoWMapField)call__51;
                    var call__52 = AoWHSSet.FSpellControl.GetSpell(13);
                    var call__53 = ((THolyGroundSpell)call__52).ValidTargetMapF(Field);
                    if (call__53)
                    {
                        if (Field.GetArmyHS() == null)
                        {
                            Field.RestoreTerrain();
                            Ground = (THolyGround)Resource.CreateResourceUser();
                            var call__54 = AoWHSMap.Random(11);
                            Ground.FDuration = (sbyte)((call__54 - 5) / 5 + 3);
                            Ground.FPlayer = Caster.FPlayer;
                            Ground.FOriginalOverlay = Field.OverlayType;
                            AoWHSMap.ClearTerrain(Walk.X, Walk.Y, FLocation.L, 0, -1);
                            var call__55 = AoWHSMap.PlaceHX(Ground, Walk.X, Walk.Y, FLocation.L);
                            if (call__55)
                            {
                                Ground.FMode = TSet<AnonymousEnum_hgSummoning>.Of(AnonymousEnum_hgSummoning.hgSummoning);
                                Anim = new TImageSequenceAnimation(null);
                                var call__56 = AoWHSSet.FSpellControl.GetSpell(13);
                                var call__57 = call__56.FCastImages.ImageSequences[0];
                                Anim.SetImageSequence(call__57);
                                Anim.FOnShow = Ground.ShowSummonAnimation;
                                Anim.FOnNewFrame = Ground.NewFrameSummonAnimation;
                                var call__58 = AoWHSMap.Random(15);
                                Anim.FFrameCounter = -call__58;
                                Anim.FTag = Caster.FPlayer;
                                AoWHSMap.PlaceHX(Anim, Walk.X, Walk.Y, FLocation.L);
                            }
                            else
                            {
                                Ground.Free();
                            }
                        }
                    }
                }

                WalkToNextHN(ref Walk);
            }
        }

        // NetworkE.pas:230


    }

    public partial class THolyGroundSpell : TGlobalTargetSpell
    {
        public THolyGroundSpell(TECustomNode owner) : base(owner)
        {
            var call__59 = TranslateRStr(HolyGroundsRStr);
            FSpellName = call__59;
            FSpellID = 13;
            FBaseManaCost = 5;
            FSpellLevel = 2;
            FMagicSphere = TMagicSphere.msLife;
            FAboveGroundOnly = false;
        }

        // HolyG.pas:339


        // HolyG.pas:375
        public override void SetupTargetSelection(TAbstractUnit A1, TTargetSelectionControl A2)
        {
            int I = default;
            base.SetupTargetSelection(A1, A2);
            A2.FHNs.Clear();
            {
                I = 0;
                int end__60 = RadToHN(2) - 1;
                for (; I <= end__60; I += 1)
                {
                    A2.FHNs.Add(0);
                }
            }
        }

        // HolyG.pas:384
        public override bool TargetSelected(TAbstractUnit A1, int A2, int A3, int A4)
        {
            bool Result = default;
            THolyGroundSpellTE Event = default;
            Event = new THolyGroundSpellTE(null);
            Event.Initialize(FSpellID, (byte)A1.FPlayer, A1.FUnitID, FBaseManaCost);
            Event.SetLocation((sbyte)A2, (sbyte)A3, (sbyte)A4);
            var call__61 = AoWHSMap.FTokenManager.GetTokenControl(A1.FPlayer);
            call__61.AddEvent(Event);
            Event.Release();
            Result = true;
            return Result;
        }

        // HolyG.pas:360
        public override bool ValidTargetSelectionMapF(TMapField A1)
        {
            bool Result = default;
            TArmyHS Army = default;
            var call__62 = base.ValidTargetMapF(A1);
            Result = call__62 && TSet<byte>.WithBounds(0, 255).With(12).WithRange(1, 5).Contains((byte)((TAoWMapField)A1).TerrainType);
            // { Original overlay test allows these overlays even when the terrain check failed. }
            Result = (Result && ((TAoWMapField)A1).OverlayType == -1) || TSet<byte>.WithBounds(0, 255).With(9).WithRange(1, 4).Contains((byte)((TAoWMapField)A1).OverlayType);
            Result = Result && ((TAoWMapField)A1).FindNoneTransparentHS(typeof(TStructure)) == null;
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

        // HolyG.pas:350
        public override bool ValidTargetMapF(TMapField A1)
        {
            bool Result = default;
            var call__63 = base.ValidTargetMapF(A1);
            Result = call__63 && TSet<byte>.WithBounds(0, 255).With(12).WithRange(1, 5).Contains((byte)((TAoWMapField)A1).TerrainType);
            // { Original overlay test allows these overlays even when the terrain check failed. }
            Result = (Result && ((TAoWMapField)A1).OverlayType == -1) || TSet<byte>.WithBounds(0, 255).With(9).WithRange(1, 4).Contains((byte)((TAoWMapField)A1).OverlayType);
            bool bool__64 = Result;
            if (bool__64)
            {
                var call__65 = ((TAoWMapField)A1).FindNoneTransparentHS(typeof(TStructure));
                bool__64 = call__65 == null;
            }

            bool bool__66 = bool__64;
            if (bool__66)
            {
                var call__67 = ((TAoWMapField)A1).GetArmyHS();
                bool__66 = call__67 == null;
            }

            Result = bool__66;
            return Result;
        }

        // HolyG.pas:63

    }

    public static partial class HolyG
    {
        // HolyG.pas:395
        public static void RegisterHolyGround(TEngine Engine)
        {
            Engine.RegisterEClasses([EClass.Of<THolyGround>(), EClass.Of<THolyGroundResource>(), EClass.Of<THolyGroundSpellTE>()]);
            var call__68 = new THolyGroundSpell(null);
            ((TAoWEngine)Engine).FGameSet.FSpellControl.RegisterSpell(call__68);
        }

        // HolyG.pas:401
        public static void UnRegisterHolyGround(TEngine Engine)
        {
            Engine.UnregisterEClasses([EClass.Of<THolyGround>(), EClass.Of<THolyGroundResource>(), EClass.Of<THolyGroundSpellTE>()]);
            ((TAoWEngine)Engine).FGameSet.FSpellControl.UnregisterSpellID(13);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
