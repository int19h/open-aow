// Translated from PoisonP.pas.
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
using Aow.Units.GlobalTargetSpells;
using static Aow.Units.GlobalTargetSpells.GlobalTargetSpells;
using Aow.Units.Animation;
using static Aow.Units.Animation.Animation;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;

namespace Aow.Units.PoisonP
{
    public enum TPoisonPlantStatusElement : byte
    {
        ppsSummoning = 0
    }

    // { TPoisonPlant (instance size 32, VMT 557C3AB8):
//     $1C summoning status; $1D caster player; $1E remaining turns; $1F original overlay. }
    public partial class TPoisonPlant : Aow.Units.ILTer.TILTerrainMO
    {
        public TSet<Aow.Units.PoisonP.TPoisonPlantStatusElement> FPlantStatus;
        // { $1C }
        public sbyte FCasterPlayer;
        // { $1D }
        public sbyte FDaysRemaining;
        // { $1E }
        public sbyte FOriginalOverlay;
        // PoisonP.pas:97
        // { $1F }
        public override void ReadWrite(Aow.Units.Engine.TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwShortintEx(ref FCasterPlayer, unchecked((sbyte)-1), 40);
            Stream.rwShortintEx(ref FDaysRemaining, unchecked((sbyte)-1), 41);
            Stream.rwShortintEx(ref FOriginalOverlay, unchecked((sbyte)-1), 42);
        }

        // PoisonP.pas:78
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            FCasterPlayer = unchecked((sbyte)-1);
            FOriginalOverlay = unchecked((sbyte)-1);
            FDaysRemaining = unchecked((sbyte)-1);
            FPlantStatus = TSet<Aow.Units.PoisonP.TPoisonPlantStatusElement>.Of();
        }

        // PoisonP.pas:255
        public override void MsgProc(ref Aow.Units.Engine.TEMsg Msg, Aow.Units.Engine.TEObject Sender)
        {
            base.MsgProc(ref Msg, Sender);
            if (Msg.MsgID == 537001986)
            {
                NewTurn(unchecked((sbyte)Msg.Value));
            }
        }

        // PoisonP.pas:261
        public override void Show(int XhpSurf, int YhpSurf, in Aow.Units.GFXE.TSurfaceDesc SurfaceDesc)
        {
            if (!FPlantStatus.Contains(Aow.Units.PoisonP.TPoisonPlantStatusElement.ppsSummoning))
            {
                base.Show(XhpSurf, YhpSurf, SurfaceDesc);
            }
        }

        // PoisonP.pas:240
        public override bool PlaceOnMap()
        {
            bool Result = default;
            Result = base.PlaceOnMap();
            if (Result)
            {
                var call__1 = GetYhx();
                var call__2 = GetXhx();
                UpdateMapFields((Aow.Units.HSEngine.TMapLevel)Owner.Owner, call__2, call__1);
            }

            return Result;
        }

        // PoisonP.pas:246
        public override void RemoveFromMap()
        {
            Aow.Units.HSEngine.TMapLevel Level = default;
            Level = (Aow.Units.HSEngine.TMapLevel)Owner.Owner;
            base.RemoveFromMap();
            UpdateMapFields(Level, FXhx, FYhx);
        }

        // PoisonP.pas:218
        public override void MapFieldMsgProc(ref Aow.Units.HSEngine.TMapFieldMsg Msg)
        {
            base.MapFieldMsgProc(ref Msg);
            if (Msg.MsgID == 4418)
            {
                var call__3 = FResource.TerrainList.GetTerrain(Msg.HN);
                if (call__3 >= 0)
                {
                    ArmyPlaced(Msg.Param[0].As<Aow.Units.AoWE.TArmyHS>());
                }
            }
            else
            {
                if (Msg.MsgID == 4384)
                {
                    var call__4 = FResource.TerrainList.GetTerrain(Msg.HN);
                    bool bool__5 = (call__4 >= 0);
                    if (bool__5)
                    {
                        var call__6 = Msg.Param[0].As<Aow.Units.AoWE.TArmyHS>().FArmy.ImmunityTypes(255);
                        bool__5 = !call__6.Contains(Aow.Units.AoWE.TDamageType.dtPoison);
                    }

                    bool bool__7 = bool__5;
                    if (bool__7)
                    {
                        var call__8 = PoisonPlantActive();
                        bool__7 = call__8;
                    }

                    if (bool__7)
                    {
                        Msg.Result = 1;
                    }
                }
                else
                {
                    if (Msg.MsgID == 4402)
                    {
                        // { Original excludes player zero from early restoration. }
                        var call__9 = FResource.TerrainList.GetTerrain(Msg.HN);
                        if (((call__9 >= 0) && (FCasterPlayer > 0)) && (FDaysRemaining > 0))
                        {
                            RestoreTerrain();
                        }
                    }
                }
            }
        }

        // PoisonP.pas:209
        public override int CanMoveOver(in Aow.Units.HSEngine.THSSenderInfo Info)
        {
            int Result = default;
            var call__10 = FResource.TerrainList.GetTerrain(Info.HN);
            bool bool__11 = (call__10 >= 0);
            if (bool__11)
            {
                var call__12 = PoisonPlantActive();
                bool__11 = call__12;
            }

            if (bool__11)
            {
                var call__13 = CanDoDamage(((Aow.Units.AoWE.TMoveControl)Info.Sender).FSettings.FUnits);
                if (!call__13)
                {
                    Result = 1;
                }
                else
                {
                    Result = 0;
                }
            }
            else
            {
                Result = 1;
            }

            return Result;
        }

        // PoisonP.pas:203
        public override void UpdateMapField(in Aow.Units.HSEngine.THSMapFieldInfo Info)
        {
            var call__14 = FResource.TerrainList.GetTerrain(Info.HN);
            if (call__14 >= 0)
            {
                ((Aow.Units.AoWE.TAoWMapField)Info.MapField).FFieldFlags = ((Aow.Units.AoWE.TAoWMapField)Info.MapField).FFieldFlags | 8192;
            }
        }

        // PoisonP.pas:92
        public bool PoisonPlantActive()
        {
            bool Result = default;
            Result = !TSet<byte>.WithBounds(0, 255).With(3).With(5).Contains((byte)FTerrainType);
            return Result;
        }

        // PoisonP.pas:105
        public bool CanDoDamage(Aow.Units.AoWE.TUnitList Units)
        {
            bool Result = default;
            int I = default;
            Aow.Units.AoWE.TAbstractUnit U = default;
            Result = false;
            var call__15 = Units.Count;
            I = call__15;
            while (!Result && (I != 0))
            {
                I -= 1;
                U = Units.GetUnit(I);
                var call__16 = U.GetImmunityTypes();
                bool bool__17 = !call__16.Contains(Aow.Units.AoWE.TDamageType.dtPoison);
                if (bool__17)
                {
                    var call__18 = U.GetAbilityEnabled(1);
                    bool__17 = !call__18;
                }

                Result = bool__17;
            }

            return Result;
        }

        // PoisonP.pas:120
        public void TriggerArmyDamage(Aow.Units.AoWE.TArmyHS ArmyHS)
        {
            int I = default;
            int Count = default;
            Aow.Units.AoWE.TUnitList Units = default;
            Aow.Units.AoWE.TArmyEventLog Log = default;
            Aow.Units.AoWE.TAbstractUnit U = default;
            int Damage = default;
            Count = 0;
            var call__19 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
            Aow.Rtl.Rtl.RandSeed = call__19;
            Log = Aow.Units.AoWE.TArmyEventLog.Create(null);
            var call__20 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.PoisonPlantDamageRStr);
            Log.FArmyText = call__20;
            Log.FPlayer = unchecked((sbyte)ArmyHS.FArmy.FPlayer);
            var call__21 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.PoisonPlantDamageRStr);
            Log.FArmyMessages.Add(call__21);
            Log.FArmyIcon = 1;
            Log.FArmyView.Setup(ArmyHS.FArmy, 255);
            Units = ArmyHS.FArmy;
            {
                I = 0;
                int end__22 = Units.Count - 1;
                for (; I <= end__22; I += 1)
                {
                    U = Units.GetUnit(I);
                    U.AddRef();
                    var call__23 = U.GetImmunityTypes();
                    bool bool__24 = !call__23.Contains(Aow.Units.AoWE.TDamageType.dtPoison);
                    if (bool__24)
                    {
                        var call__25 = U.GetAbilityEnabled(1);
                        bool__24 = !call__25;
                    }

                    if (bool__24)
                    {
                        Damage = U.ExecuteDamageRole(4, 3, TSet<Aow.Units.AoWE.TDamageType>.Of(Aow.Units.AoWE.TDamageType.dtPoison));
                        if (Damage > U.GetHitPoints())
                        {
                            Damage = U.GetHitPoints();
                        }

                        if (Damage != 0)
                        {
                            var call__26 = U.GetHitPoints();
                            U.SetHitPoints(unchecked((sbyte)(call__26 - Damage)));
                            if (U.GetHitPoints() != 0)
                            {
                                U.ExecuteDamageEffects(TSet<Aow.Units.AoWE.TDamageType>.Of(Aow.Units.AoWE.TDamageType.dtPoison));
                            }

                            Count += 1;
                        }

                        Log.FDamageValues.Add(unchecked((byte)Damage));
                        Log.FDamageTypes.Add(unchecked((byte)((int)Aow.Units.AoWE.TDamageType.dtPoison)));
                    }
                    else
                    {
                        Log.FDamageValues.Add(0);
                        Log.FDamageTypes.Add(unchecked((byte)((int)Aow.Units.AoWE.TDamageType.dtNone)));
                    }

                    U.Release();
                }
            }

            Log.ValidateGameOver();
            if (Count != 0)
            {
                var call__27 = GetYhx();
                var call__28 = GetLhx();
                var call__29 = GetXhx();
                Aow.Units.EventLog.EventLog.DistributeLocationEventLog(Log, unchecked((sbyte)Log.FPlayer), unchecked((sbyte)call__29), unchecked((sbyte)call__27), unchecked((sbyte)call__28), true);
            }

            ArmyHS.FArmy.Validate();
            Log.Release();
        }

        // PoisonP.pas:168
        public void ArmyPlaced(Aow.Units.AoWE.TArmyHS ArmyHS)
        {
            var call__30 = PoisonPlantActive();
            if (call__30 && !Aow.Units.AoWE.AoWE.AoWHSMap.Status.Contains(Aow.Units.HSEngine.THSMapStatusElement.msEditting))
            {
                TriggerArmyDamage(ArmyHS);
            }
        }

        // PoisonP.pas:174
        public void RestoreTerrain()
        {
            Aow.Units.HSEngine.TMapField MapF = default;
            sbyte OriginalOverlay = default;
            MapF = (Aow.Units.HSEngine.TMapField)Owner;
            OriginalOverlay = unchecked((sbyte)FOriginalOverlay);
            Destroy();
            if (OriginalOverlay > 0)
            {
                Aow.Units.AoWE.AoWE.AoWHSMap.PlaceTerrain(unchecked((sbyte)MapF.Xhx), unchecked((sbyte)MapF.Yhx), unchecked((sbyte)MapF.Lhx), unchecked((sbyte)MapF.TerrainType), unchecked((sbyte)OriginalOverlay));
            }
        }

        // PoisonP.pas:186
        public void NewTurn(sbyte Player)
        {
            Aow.Units.AoWE.TArmyHS ArmyHS = default;
            if (PoisonPlantActive())
            {
                var call__31 = Owner.FindOwnedChild(131607);
                ArmyHS = (Aow.Units.AoWE.TArmyHS)call__31;
                if ((ArmyHS != null) && (Player == ArmyHS.FArmy.FPlayer))
                {
                    TriggerArmyDamage(ArmyHS);
                }
            }

            if ((Player == FCasterPlayer) && (FDaysRemaining > 0))
            {
                FDaysRemaining = (sbyte)(((int)FDaysRemaining) - 1);
                if (FDaysRemaining == 0)
                {
                    RestoreTerrain();
                }
            }
        }

        // PoisonP.pas:267
        public void ShowSummonAnimation(Aow.Units.Animation.TAnimation Animation, int XhpSurf, int YhpSurf, in Aow.Units.GFXE.TSurfaceDesc SurfaceDesc)
        {
            Aow.Units.HSEngine.TMapField MapF = default;
            var call__32 = Animation.GetLhx();
            var call__33 = Animation.GetYhx();
            var call__34 = Animation.GetXhx();
            var call__35 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.GetField(call__34, call__33, call__32);
            MapF = call__35;
            var call__36 = MapF.FindOwnedChild(131998);
            if (((this == call__36) && (((int)Aow.Units.AoWE.AoWE.AoWEngine.FAnimationDetail) != 0)) && (Animation.FFrameCounter == 0))
            {
                bool bool__37 = (Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer == Animation.FTag);
                if (!bool__37)
                {
                    var call__38 = GetLhx();
                    var call__39 = GetYhx();
                    var call__40 = GetXhx();
                    var call__41 = Aow.Units.AoWE.AoWE.AoWHSMap.WatchingTerrain(unchecked((sbyte)call__40), unchecked((sbyte)call__39), unchecked((sbyte)call__38), 0);
                    bool__37 = call__41;
                }

                if (bool__37)
                {
                    var call__42 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(12);
                    call__42.FCastSounds.Play(0);
                }
            }
        }

        // PoisonP.pas:279
        public void NewFrameSummonAnimation(Aow.Units.Animation.TAnimation Animation, int FrameCounter)
        {
            Aow.Units.HSEngine.TMapField MapF = default;
            var call__43 = Animation.GetLhx();
            var call__44 = Animation.GetYhx();
            var call__45 = Animation.GetXhx();
            var call__46 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.GetField(call__45, call__44, call__43);
            Animation = (Aow.Units.Animation.TAnimation)((System.Object)call__46);
            if ((this == ((Aow.Units.HSEngine.TMapField)((System.Object)Animation)).FindOwnedChild(131998)) && (FrameCounter == 9))
            {
                FPlantStatus = FPlantStatus - TSet<Aow.Units.PoisonP.TPoisonPlantStatusElement>.Of(Aow.Units.PoisonP.TPoisonPlantStatusElement.ppsSummoning);
            }
        }

        // PoisonP.pas:28
        public static new Aow.Units.PoisonP.TPoisonPlant Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.PoisonP.TPoisonPlant o = new Aow.Units.PoisonP.TPoisonPlant();
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

        public new partial class Meta : Aow.Units.ILTer.TILTerrainMO.Meta
        {
            // PoisonP.pas:87
            public override int ClassID()
            {
                int Result = default;
                Result = 131998;
                return Result;
            }

            // PoisonP.pas:28
            public new Aow.Units.PoisonP.TPoisonPlant Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.PoisonP.TPoisonPlant o = (Aow.Units.PoisonP.TPoisonPlant)NewInstance();
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
                where T : Aow.Units.PoisonP.TPoisonPlant => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { TPoisonPlantResource (instance size 68, VMT 557C3C68) }
    public partial class TPoisonPlantResource : Aow.Units.ILTer.TILTerrainMOR
    {
        // PoisonP.pas:298
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 131998;
            return Result;
        }

        public new partial class Meta : Aow.Units.ILTer.TILTerrainMOR.Meta
        {
            // PoisonP.pas:293
            public override int ClassID()
            {
                int Result = default;
                Result = 131999;
                return Result;
            }

            // PoisonP.pas:288
            public override Aow.Rtl.AnsiString ResourceEditName()
            {
                Aow.Rtl.AnsiString Result = default;
                Result = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.PoisonPlantRStr);
                return Result;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.PoisonP.TPoisonPlantResource => (Meta)MetaRegistry.Of(typeof(T));
            // HSEngine.pas:1194
            public new Aow.Units.PoisonP.TPoisonPlantResource Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.PoisonP.TPoisonPlantResource o = (Aow.Units.PoisonP.TPoisonPlantResource)NewInstance();
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
    }

    // { TPoisonPlantSpellTE (instance size 36, VMT 557C3D68) }
    public partial class TPoisonPlantSpellTE : Aow.Units.GlobalTargetSpells.TGlobalTargetSpellTE
    {
        // PoisonP.pas:361
        public override bool Process(Aow.Units.AoWE.TAbstractUnit A1, int A2)
        {
            bool Result = default;
            base.Process(A1, A2);
            CreatePoisonPlant(A1);
            Aow.Units.AoWE.AoWE.AoWHSMap.ValidateTerrainObservation(unchecked((sbyte)A1.FPlayer), unchecked((sbyte)FLocation.X), unchecked((sbyte)FLocation.Y), unchecked((sbyte)FLocation.L), 1);
            var call__47 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
            call__47.CastingDone(A1, FEventMana);
            Result = false;
            return Result;
        }

        // PoisonP.pas:308
        public void CreatePoisonPlant(Aow.Units.AoWE.TAbstractUnit Source)
        {
            Aow.Units.HSEngine.TMapLevel Level = default;
            Aow.Units.HSEngine.THNWalkData Walk = default;
            Aow.Units.PoisonP.TPoisonPlantResource Resource = default;
            Aow.Units.AoWE.TAoWMapField MapF = default;
            Aow.Units.PoisonP.TPoisonPlant Plant = default;
            Aow.Units.Animation.TImageSequenceAnimation Animation = default;
            Aow.Units.Animation.TTwinkleAnimation Twinkle = default;
            ref var walk = ref Walk;
            var call__49 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.MapLevels[FLocation.L];
            Level = call__49;
            Aow.Units.HSEngine.HSEngine.InitHNWalk(ref Walk, FLocation.X, FLocation.Y, 0);
            var call__50 = Aow.Units.AoWE.AoWE.AoWHSSet.MapObjectResourceList.FindChild(131999);
            Resource = (Aow.Units.PoisonP.TPoisonPlantResource)call__50;
            while (walk.Radius <= 1)
            {
                if ((((walk.X >= 0) && (walk.Y >= 0)) && (walk.X < Level.Width)) && (walk.Y < Level.Height))
                {
                    var call__51 = Level.GetField(walk.X, walk.Y);
                    MapF = (Aow.Units.AoWE.TAoWMapField)call__51;
                    var call__52 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(12);
                    var call__53 = ((Aow.Units.GlobalTargetSpells.TGlobalTargetSpell)call__52).ValidTargetMapF(MapF);
                    bool bool__54 = call__53;
                    if (bool__54)
                    {
                        var call__55 = MapF.GetArmyHS();
                        bool__54 = (call__55 == null);
                    }

                    if (bool__54)
                    {
                        MapF.RestoreTerrain();
                        Plant = (Aow.Units.PoisonP.TPoisonPlant)Resource.CreateResourceUser();
                        var call__56 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(11);
                        Plant.FDaysRemaining = unchecked((sbyte)(3 + ((call__56 - 5) / 5)));
                        Plant.FCasterPlayer = unchecked((sbyte)Source.FPlayer);
                        Plant.FOriginalOverlay = unchecked((sbyte)MapF.OverlayType);
                        Aow.Units.AoWE.AoWE.AoWHSMap.ClearTerrain(walk.X, walk.Y, FLocation.L, 0, -1);
                        var call__57 = Aow.Units.AoWE.AoWE.AoWHSMap.PlaceHX(Plant, walk.X, walk.Y, FLocation.L);
                        if (call__57)
                        {
                            Plant.FPlantStatus = TSet<Aow.Units.PoisonP.TPoisonPlantStatusElement>.Of(Aow.Units.PoisonP.TPoisonPlantStatusElement.ppsSummoning);
                            Animation = Aow.Units.Animation.TImageSequenceAnimation.Create(null);
                            var call__58 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(12);
                            var call__59 = call__58.FCastImages.ImageSequences[0];
                            Animation.SetImageSequence(call__59);
                            Animation.FOnShow = Plant.ShowSummonAnimation;
                            Animation.FOnNewFrame = Plant.NewFrameSummonAnimation;
                            var call__60 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(15);
                            Animation.FFrameCounter = -call__60;
                            Animation.FAnimationLevel = 27;
                            Aow.Units.AoWE.AoWE.AoWHSMap.PlaceHX(Animation, walk.X, walk.Y, FLocation.L);
                            Twinkle = Aow.Units.Animation.TTwinkleAnimation.Create(null);
                            var call__61 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(2);
                            var call__62 = call__61.FCastImages.ImageSequences[0];
                            Twinkle.Setup(call__62, TSet<Aow.Units.Animation.TTwinkleModeElement>.Of(), 2, 6, System.Math.Abs(Animation.FFrameCounter + 5), System.Math.Abs(Animation.FFrameCounter + 5) / 2);
                            Aow.Units.AoWE.AoWE.AoWHSMap.PlaceHX(Twinkle, walk.X, walk.Y, FLocation.L);
                        }
                        else
                        {
                            Plant.Free();
                        }
                    }
                }

                Aow.Units.HSEngine.HSEngine.WalkToNextHN(ref Walk);
            }
        }

        // NetworkE.pas:230
        public static new Aow.Units.PoisonP.TPoisonPlantSpellTE Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.PoisonP.TPoisonPlantSpellTE o = new Aow.Units.PoisonP.TPoisonPlantSpellTE();
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

        public new partial class Meta : Aow.Units.GlobalTargetSpells.TGlobalTargetSpellTE.Meta
        {
            // PoisonP.pas:303
            public override int ClassID()
            {
                int Result = default;
                Result = 135941;
                return Result;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.PoisonP.TPoisonPlantSpellTE => (Meta)MetaRegistry.Of(typeof(T));
            // NetworkE.pas:230
            public new Aow.Units.PoisonP.TPoisonPlantSpellTE Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.PoisonP.TPoisonPlantSpellTE o = (Aow.Units.PoisonP.TPoisonPlantSpellTE)NewInstance();
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
    }

    // { TPoisonPlantSpell (instance size 60, VMT 557C3E48) }
    public partial class TPoisonPlantSpell : Aow.Units.GlobalTargetSpells.TGlobalTargetSpell
    {
        // PoisonP.pas:371
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            var call__63 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.PoisonPlantsRStr);
            FSpellName = call__63;
            FSpellID = 12;
            FBaseManaCost = 5;
            FSpellLevel = 2;
            FMagicSphere = Aow.Units.AoWE.TMagicSphere.msEarth;
            FAboveGroundOnly = false;
        }

        // PoisonP.pas:405
        public override void SetupTargetSelection(Aow.Units.AoWE.TAbstractUnit A1, Aow.Units.AoWE.TTargetSelectionControl A2)
        {
            int I = default;
            base.SetupTargetSelection(A1, A2);
            A2.FHNs.Clear();
            {
                I = 0;
                int end__64 = Aow.Units.HSEngine.HSEngine.RadToHN(2) - 1;
                for (; I <= end__64; I += 1)
                {
                    A2.FHNs.Add(0);
                }
            }
        }

        // PoisonP.pas:414
        public override bool TargetSelected(Aow.Units.AoWE.TAbstractUnit A1, int A2, int A3, int A4)
        {
            bool Result = default;
            Aow.Units.PoisonP.TPoisonPlantSpellTE Event = default;
            Event = Aow.Units.PoisonP.TPoisonPlantSpellTE.Create(null);
            Event.Initialize(FSpellID, unchecked((byte)A1.FPlayer), A1.FUnitID, FBaseManaCost);
            Event.SetLocation(unchecked((sbyte)A2), unchecked((sbyte)A3), unchecked((sbyte)A4));
            var call__65 = Aow.Units.AoWE.AoWE.AoWHSMap.FTokenManager.GetTokenControl(A1.FPlayer);
            call__65.AddEvent(Event);
            Event.Release();
            Result = true;
            return Result;
        }

        // PoisonP.pas:390
        public override bool ValidTargetSelectionMapF(Aow.Units.HSEngine.TMapField A1)
        {
            bool Result = default;
            Aow.Units.AoWE.TArmyHS ArmyHS = default;
            var call__66 = base.ValidTargetSelectionMapF(A1);
            Result = call__66 && TSet<byte>.WithBounds(0, 255).With(12).WithRange(1, 5).Contains((byte)A1.TerrainType);
            // { Original precedence also accepts these overlays without the terrain test. }
            Result = (Result && (A1.OverlayType == -1)) || TSet<byte>.WithBounds(0, 255).With(9).WithRange(1, 4).Contains((byte)A1.OverlayType);
            bool bool__67 = Result;
            if (bool__67)
            {
                var call__68 = A1.FindNoneTransparentHS(typeof(Aow.Units.AoWE.TStructure));
                bool__67 = (call__68 == null);
            }

            bool bool__69 = bool__67;
            if (bool__69)
            {
                var call__70 = ((Aow.Units.AoWE.TAoWMapField)A1).GetArmyHS();
                bool__69 = (call__70 == null);
            }

            Result = bool__69;
            if (Result)
            {
                ArmyHS = ((Aow.Units.AoWE.TAoWMapField)A1).GetArmyHS();
                if (ArmyHS != null)
                {
                    Result = !ArmyHS.VisibleForSeatedPlayer();
                }
            }

            return Result;
        }

        // PoisonP.pas:382
        public override bool ValidTargetMapF(Aow.Units.HSEngine.TMapField A1)
        {
            bool Result = default;
            var call__71 = base.ValidTargetMapF(A1);
            Result = call__71 && TSet<byte>.WithBounds(0, 255).With(12).WithRange(1, 5).Contains((byte)A1.TerrainType);
            // { Original precedence also accepts these overlays without the terrain test. }
            Result = (Result && (A1.OverlayType == -1)) || TSet<byte>.WithBounds(0, 255).With(9).WithRange(1, 4).Contains((byte)A1.OverlayType);
            bool bool__72 = Result;
            if (bool__72)
            {
                var call__73 = A1.FindNoneTransparentHS(typeof(Aow.Units.AoWE.TStructure));
                bool__72 = (call__73 == null);
            }

            bool bool__74 = bool__72;
            if (bool__74)
            {
                var call__75 = ((Aow.Units.AoWE.TAoWMapField)A1).GetArmyHS();
                bool__74 = (call__75 == null);
            }

            Result = bool__74;
            return Result;
        }

        // PoisonP.pas:66
        public static new Aow.Units.PoisonP.TPoisonPlantSpell Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.PoisonP.TPoisonPlantSpell o = new Aow.Units.PoisonP.TPoisonPlantSpell();
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

    public static partial class PoisonP
    {
        // PoisonP.pas:426
        public static void RegisterPoisonPlant(Aow.Units.AoWE.TAoWEngine AEngine)
        {
            AEngine.RegisterEClasses([Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.PoisonP.TPoisonPlant>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.PoisonP.TPoisonPlantResource>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.PoisonP.TPoisonPlantSpellTE>()]);
            var call__76 = Aow.Units.PoisonP.TPoisonPlantSpell.Create(null);
            AEngine.FGameSet.FSpellControl.RegisterSpell(call__76);
        }

        // PoisonP.pas:432
        public static void UnRegisterPoisonPlant(Aow.Units.AoWE.TAoWEngine AEngine)
        {
            AEngine.UnregisterEClasses([Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.PoisonP.TPoisonPlant>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.PoisonP.TPoisonPlantResource>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.PoisonP.TPoisonPlantSpellTE>()]);
            AEngine.FGameSet.FSpellControl.UnregisterSpellID(12);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
