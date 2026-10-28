// Translated from UnHolyG.pas.
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

namespace Aow.Units.UnHolyG
{
    public enum AnonymousEnum_hgSummoning : byte
    {
        hgSummoning = 0
    }

    // { TUnHolyGround (instance size 32, VMT 557C8B10): $1C FMode, $1D FPlayer,
//     $1E FDuration, $1F FOriginalOverlay. }
    public partial class TUnHolyGround : Aow.Units.ILTer.TILTerrainMO
    {
        public TSet<Aow.Units.UnHolyG.AnonymousEnum_hgSummoning> FMode;
        // { $1C }
        public sbyte FPlayer;
        // { $1D }
        public sbyte FDuration;
        // { $1E }
        public sbyte FOriginalOverlay;
        // UnHolyG.pas:204
        // { $1F }
        public override void ReadWrite(Aow.Units.Engine.TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwShortintEx(ref FPlayer, unchecked((sbyte)-1), 40);
            Stream.rwShortintEx(ref FDuration, unchecked((sbyte)-1), 41);
            Stream.rwShortintEx(ref FOriginalOverlay, unchecked((sbyte)-1), 42);
        }

        // UnHolyG.pas:243
        public override void MsgProc(ref Aow.Units.Engine.TEMsg Msg, Aow.Units.Engine.TEObject Sender)
        {
            base.MsgProc(ref Msg, Sender);
            if (Msg.MsgID == 537001986)
            {
                NewTurn(unchecked((sbyte)Msg.Value));
            }
        }

        // UnHolyG.pas:249
        public override void Show(int XhpSurf, int YhpSurf, in Aow.Units.GFXE.TSurfaceDesc SurfaceDesc)
        {
            if (!FMode.Contains(Aow.Units.UnHolyG.AnonymousEnum_hgSummoning.hgSummoning))
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
                UpdateMapFields((Aow.Units.HSEngine.TMapLevel)Owner.Owner, call__2, call__1);
            }

            return Result;
        }

        // UnHolyG.pas:196
        public override void RemoveFromMap()
        {
            Aow.Units.HSEngine.TMapLevel Level = default;
            Level = (Aow.Units.HSEngine.TMapLevel)Owner.Owner;
            base.RemoveFromMap();
            UpdateMapFields(Level, FXhx, FYhx);
        }

        // UnHolyG.pas:222
        public override void MapFieldMsgProc(ref Aow.Units.HSEngine.TMapFieldMsg Msg)
        {
            base.MapFieldMsgProc(ref Msg);
            if (Msg.MsgID == 4418)
            {
                var call__3 = FResource.TerrainList.Terrain[Msg.HN];
                if (call__3 >= 0)
                {
                    ArmyPlaced(Msg.Param[0].As<Aow.Units.AoWE.TArmyHS>());
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
                        var call__5 = CanDoDamage(Msg.Param[0].As<Aow.Units.AoWE.TArmyHS>().FArmy);
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
                        if (((call__6 >= 0) && (FPlayer > 0)) && (FDuration > 0))
                        {
                            RestoreTerrain();
                        }
                    }
                }
            }
        }

        // UnHolyG.pas:212
        public override int CanMoveOver(in Aow.Units.HSEngine.THSSenderInfo Info)
        {
            int Result = default;
            var call__7 = FResource.TerrainList.Terrain[Info.HN];
            if (call__7 >= 0)
            {
                var call__8 = CanDoDamage(((Aow.Units.AoWE.TMoveControl)Info.Sender).FSettings.FUnits);
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
        public override void UpdateMapField(in Aow.Units.HSEngine.THSMapFieldInfo Info)
        {
            var call__9 = FResource.TerrainList.Terrain[Info.HN];
            if (call__9 >= 0)
            {
                ((Aow.Units.AoWE.TAoWMapField)Info.MapField).FFieldFlags = ((Aow.Units.AoWE.TAoWMapField)Info.MapField).FFieldFlags | 8192;
            }
        }

        // UnHolyG.pas:96
        public void TriggerArmyDamage(Aow.Units.AoWE.TArmyHS Army)
        {
            Aow.Units.AoWE.TArmyEventLog Log = default;
            int I = default;
            int Damage = default;
            int Damaged = default;
            Aow.Units.AoWE.TAbstractUnit AUnit = default;
            Damaged = 0;
            var call__10 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
            Aow.Rtl.Rtl.RandSeed = call__10;
            Log = Aow.Units.AoWE.TArmyEventLog.Create(null);
            var call__11 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.UnholyGroundDamageRStr);
            Log.FArmyText = call__11;
            var call__12 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.UnholyGroundDamageRStr);
            Log.FArmyMessages.Add(call__12);
            Log.FArmyIcon = 2;
            {
                var w__13 = Army.FArmy;
                {
                    I = 0;
                    int end__14 = w__13.Count - 1;
                    for (; I <= end__14; I += 1)
                    {
                        AUnit = w__13.GetUnit(I);
                        var call__15 = AUnit.GetAlignment();
                        bool bool__16 = (call__15 != Aow.Units.AoWE.TAlignment.alNone);
                        if (bool__16)
                        {
                            var call__17 = AUnit.GetAlignment();
                            bool__16 = Aow.Units.AoWE.AoWE.alsPureGood.Contains(call__17);
                        }

                        bool bool__18 = bool__16;
                        if (bool__18)
                        {
                            var call__19 = AUnit.GetAbilityEnabled(1);
                            bool__18 = !call__19;
                        }

                        if (bool__18)
                        {
                            Damage = AUnit.ExecuteDamageRole(4, 3, TSet<Aow.Units.AoWE.TDamageType>.Of(Aow.Units.AoWE.TDamageType.dtDeath));
                            if (Damage > AUnit.GetHitPoints())
                            {
                                Damage = AUnit.GetHitPoints();
                            }

                            if (Damage != 0)
                            {
                                var call__20 = AUnit.GetHitPoints();
                                AUnit.SetHitPoints(unchecked((sbyte)(call__20 - Damage)));
                                Damaged += 1;
                            }

                            Log.FDamageValues.Add(unchecked((byte)Damage));
                            Log.FDamageTypes.Add(unchecked((byte)((int)Aow.Units.AoWE.TDamageType.dtDeath)));
                        }
                        else
                        {
                            Log.FDamageValues.Add(0);
                            Log.FDamageTypes.Add(unchecked((byte)((int)Aow.Units.AoWE.TDamageType.dtDeath)));
                        }
                    }
                }

                Log.FPlayer = unchecked((sbyte)w__13.FPlayer);
                var call__21 = w__13.GetArmyHS();
                Log.FArmyView.Setup(call__21.FArmy, 255);
            }

            Log.ValidateGameOver();
            if (Damaged != 0)
            {
                var call__22 = GetYhx();
                var call__23 = GetLhx();
                var call__24 = GetXhx();
                Aow.Units.EventLog.EventLog.DistributeLocationEventLog(Log, unchecked((sbyte)Log.FPlayer), unchecked((sbyte)call__24), unchecked((sbyte)call__22), unchecked((sbyte)call__23), true);
            }

            Log.Release();
            Army.FArmy.Validate();
        }

        // UnHolyG.pas:142
        public void ArmyPlaced(Aow.Units.AoWE.TArmyHS Army)
        {
            if (!Aow.Units.AoWE.AoWE.AoWHSMap.Status.Contains(Aow.Units.HSEngine.THSMapStatusElement.msEditting))
            {
                TriggerArmyDamage(Army);
            }
        }

        // UnHolyG.pas:147
        public void RestoreTerrain()
        {
            Aow.Units.HSEngine.TMapField Field = default;
            sbyte Overlay = default;
            Field = (Aow.Units.HSEngine.TMapField)Owner;
            Overlay = unchecked((sbyte)FOriginalOverlay);
            Destroy();
            if (Overlay > 0)
            {
                Aow.Units.AoWE.AoWE.AoWHSMap.PlaceTerrain(unchecked((sbyte)Field.Xhx), unchecked((sbyte)Field.Yhx), unchecked((sbyte)Field.Lhx), unchecked((sbyte)Field.TerrainType), unchecked((sbyte)Overlay));
            }
        }

        // UnHolyG.pas:157
        public void NewTurn(sbyte Player)
        {
            Aow.Units.AoWE.TArmyHS Army = default;
            var call__25 = Owner.FindOwnedChild(131607);
            Army = (Aow.Units.AoWE.TArmyHS)call__25;
            if ((Army != null) && (Player == Army.FArmy.FPlayer))
            {
                TriggerArmyDamage(Army);
            }

            if ((Player == FPlayer) && (FDuration > 0))
            {
                FDuration = (sbyte)(((int)FDuration) - 1);
                if (FDuration == 0)
                {
                    RestoreTerrain();
                }
            }
        }

        // UnHolyG.pas:169
        public bool CanDoDamage(Aow.Units.AoWE.TUnitList Units)
        {
            bool Result = default;
            int I = default;
            Aow.Units.AoWE.TAbstractUnit AUnit = default;
            Result = false;
            var call__26 = Units.Count;
            I = call__26;
            while (!Result && (I != 0))
            {
                I -= 1;
                AUnit = Units.GetUnit(I);
                var call__27 = AUnit.GetImmunityTypes();
                bool bool__28 = !call__27.Contains(Aow.Units.AoWE.TDamageType.dtDeath);
                if (bool__28)
                {
                    var call__29 = AUnit.GetAlignment();
                    bool__28 = (call__29 != Aow.Units.AoWE.TAlignment.alNone);
                }

                bool bool__30 = bool__28;
                if (bool__30)
                {
                    var call__31 = AUnit.GetAlignment();
                    bool__30 = Aow.Units.AoWE.AoWE.alsPureGood.Contains(call__31);
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
        public void ShowSummonAnimation(Aow.Units.Animation.TAnimation Anim, int XhpSurf, int YhpSurf, in Aow.Units.GFXE.TSurfaceDesc SurfaceDesc)
        {
            Aow.Units.HSEngine.TMapField Field = default;
            var call__34 = Anim.GetLhx();
            var call__35 = Anim.GetYhx();
            var call__36 = Anim.GetXhx();
            var call__37 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.GetField(call__36, call__35, call__34);
            Field = call__37;
            var call__38 = Field.FindOwnedChild(131985);
            if (((this == call__38) && (((int)Aow.Units.AoWE.AoWE.AoWEngine.FAnimationDetail) != 0)) && (Anim.FFrameCounter == 0))
            {
                bool bool__39 = (Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer == Anim.FTag);
                if (!bool__39)
                {
                    var call__40 = GetLhx();
                    var call__41 = GetYhx();
                    var call__42 = GetXhx();
                    var call__43 = Aow.Units.AoWE.AoWE.AoWHSMap.WatchingTerrain(unchecked((sbyte)call__42), unchecked((sbyte)call__41), unchecked((sbyte)call__40), 0);
                    bool__39 = call__43;
                }

                if (bool__39)
                {
                    var call__44 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(14);
                    call__44.FCastSounds.Play(0);
                }
            }
        }

        // UnHolyG.pas:265
        public void NewFrameSummonAnimation(Aow.Units.Animation.TAnimation Anim, int FrameCounter)
        {
            Aow.Units.HSEngine.TMapField Field = default;
            var call__45 = Anim.GetLhx();
            var call__46 = Anim.GetYhx();
            var call__47 = Anim.GetXhx();
            var call__48 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.GetField(call__47, call__46, call__45);
            Anim = (Aow.Units.Animation.TAnimation)((System.Object)call__48);
            if ((this == ((Aow.Units.HSEngine.TMapField)((System.Object)Anim)).FindOwnedChild(131985)) && (FrameCounter == 9))
            {
                FMode = FMode - TSet<Aow.Units.UnHolyG.AnonymousEnum_hgSummoning>.Of(Aow.Units.UnHolyG.AnonymousEnum_hgSummoning.hgSummoning);
            }
        }

        public new partial class Meta : Aow.Units.ILTer.TILTerrainMO.Meta
        {
            // UnHolyG.pas:93
            public override int ClassID()
            {
                int Result = default;
                Result = 131985;
                return Result;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.UnHolyG.TUnHolyGround => (Meta)MetaRegistry.Of(typeof(T));
            // ILTer.pas:20
            public new Aow.Units.UnHolyG.TUnHolyGround Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.UnHolyG.TUnHolyGround o = (Aow.Units.UnHolyG.TUnHolyGround)NewInstance();
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

    // { TUnHolyGroundResource (instance size 68, VMT 557C8CC0) }
    public partial class TUnHolyGroundResource : Aow.Units.ILTer.TILTerrainMOR
    {
        // UnHolyG.pas:283
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 131985;
            return Result;
        }

        public new partial class Meta : Aow.Units.ILTer.TILTerrainMOR.Meta
        {
            // UnHolyG.pas:278
            public override int ClassID()
            {
                int Result = default;
                Result = 131986;
                return Result;
            }

            // UnHolyG.pas:273
            public override Aow.Rtl.AnsiString ResourceEditName()
            {
                Aow.Rtl.AnsiString Result = default;
                var call__49 = Aow.Units.AoWE.AoWE.UnholyGroundRStr;
                var call__50 = Aow.Units.AoWE.AoWE.TranslateRStr(call__49);
                Result = call__50;
                return Result;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.UnHolyG.TUnHolyGroundResource => (Meta)MetaRegistry.Of(typeof(T));
            // HSEngine.pas:1194
            public new Aow.Units.UnHolyG.TUnHolyGroundResource Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.UnHolyG.TUnHolyGroundResource o = (Aow.Units.UnHolyG.TUnHolyGroundResource)NewInstance();
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

    // { TUnholyGroundSpellTE (instance size 36, VMT 557C8DC0) }
    public partial class TUnholyGroundSpellTE : Aow.Units.GlobalTargetSpells.TGlobalTargetSpellTE
    {
        // UnHolyG.pas:340
        public override bool Process(Aow.Units.AoWE.TAbstractUnit A1, int A2)
        {
            bool Result = default;
            base.Process(A1, A2);
            CreateUnholyGround(A1);
            Aow.Units.AoWE.AoWE.AoWHSMap.ValidateTerrainObservation(unchecked((sbyte)A1.FPlayer), unchecked((sbyte)FLocation.X), unchecked((sbyte)FLocation.Y), unchecked((sbyte)FLocation.L), 1);
            var call__51 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
            call__51.CastingDone(A1, FEventMana);
            Result = false;
            return Result;
        }

        // UnHolyG.pas:293
        public void CreateUnholyGround(Aow.Units.AoWE.TAbstractUnit Source)
        {
            Aow.Units.HSEngine.TMapLevel Level = default;
            Aow.Units.HSEngine.THNWalkData[] Walk__cell = new Aow.Units.HSEngine.THNWalkData[1];
            ref Aow.Units.HSEngine.THNWalkData Walk = ref Walk__cell[0];
            Aow.Units.UnHolyG.TUnHolyGroundResource Resource = default;
            Aow.Units.AoWE.TAoWMapField MapF = default;
            Aow.Units.UnHolyG.TUnHolyGround Ground = default;
            Aow.Units.Animation.TImageSequenceAnimation Anim = default;
            {
                var w__52 = Walk;
                var call__53 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.MapLevels[FLocation.L];
                Level = call__53;
                Aow.Units.HSEngine.HSEngine.InitHNWalk(ref Walk, FLocation.X, FLocation.Y, 0);
                var call__54 = Aow.Units.AoWE.AoWE.AoWHSSet.MapObjectResourceList.FindChild(131986);
                Resource = (Aow.Units.UnHolyG.TUnHolyGroundResource)call__54;
                while (w__52.Radius <= 1)
                {
                    if ((((w__52.X >= 0) && (w__52.Y >= 0)) && (w__52.X < Level.Width)) && (w__52.Y < Level.Height))
                    {
                        var call__55 = Level.GetField(w__52.X, w__52.Y);
                        MapF = (Aow.Units.AoWE.TAoWMapField)call__55;
                        var call__56 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(14);
                        var call__57 = ((Aow.Units.GlobalTargetSpells.TGlobalTargetSpell)call__56).ValidTargetMapF(MapF);
                        bool bool__58 = call__57;
                        if (bool__58)
                        {
                            var call__59 = MapF.GetArmyHS();
                            bool__58 = (call__59 == null);
                        }

                        if (bool__58)
                        {
                            MapF.RestoreTerrain();
                            Ground = (Aow.Units.UnHolyG.TUnHolyGround)Resource.CreateResourceUser();
                            var call__60 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(11);
                            Ground.FDuration = unchecked((sbyte)(3 + ((call__60 - 5) / 5)));
                            Ground.FPlayer = unchecked((sbyte)Source.FPlayer);
                            Ground.FOriginalOverlay = unchecked((sbyte)MapF.OverlayType);
                            Aow.Units.AoWE.AoWE.AoWHSMap.ClearTerrain(w__52.X, w__52.Y, FLocation.L, 0, -1);
                            var call__61 = Aow.Units.AoWE.AoWE.AoWHSMap.PlaceHX(Ground, w__52.X, w__52.Y, FLocation.L);
                            if (call__61)
                            {
                                Ground.FMode = TSet<Aow.Units.UnHolyG.AnonymousEnum_hgSummoning>.Of(Aow.Units.UnHolyG.AnonymousEnum_hgSummoning.hgSummoning);
                                Anim = Aow.Units.Animation.TImageSequenceAnimation.Create(null);
                                var call__62 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(14);
                                var call__63 = call__62.FCastImages.ImageSequences[0];
                                Anim.SetImageSequence(call__63);
                                Anim.FOnShow = Ground.ShowSummonAnimation;
                                Anim.FOnNewFrame = Ground.NewFrameSummonAnimation;
                                var call__64 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(15);
                                Anim.FFrameCounter = -call__64;
                                Anim.FTag = Source.FPlayer;
                                Aow.Units.AoWE.AoWE.AoWHSMap.PlaceHX(Anim, w__52.X, w__52.Y, FLocation.L);
                            }
                            else
                            {
                                Ground.Free();
                            }
                        }
                    }

                    Aow.Units.HSEngine.HSEngine.WalkToNextHN(ref Walk);
                }
            }
        }

        // NetworkE.pas:230
        public static new Aow.Units.UnHolyG.TUnholyGroundSpellTE Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.UnHolyG.TUnholyGroundSpellTE o = new Aow.Units.UnHolyG.TUnholyGroundSpellTE();
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
            // UnHolyG.pas:288
            public override int ClassID()
            {
                int Result = default;
                Result = 135943;
                return Result;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.UnHolyG.TUnholyGroundSpellTE => (Meta)MetaRegistry.Of(typeof(T));
            // NetworkE.pas:230
            public new Aow.Units.UnHolyG.TUnholyGroundSpellTE Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.UnHolyG.TUnholyGroundSpellTE o = (Aow.Units.UnHolyG.TUnholyGroundSpellTE)NewInstance();
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

    // { TUnholyGroundSpell (instance size 60, VMT 557C8EA4) }
    public partial class TUnholyGroundSpell : Aow.Units.GlobalTargetSpells.TGlobalTargetSpell
    {
        // UnHolyG.pas:349
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            var call__65 = Aow.Units.AoWE.AoWE.UnholyGroundsRStr;
            var call__66 = Aow.Units.AoWE.AoWE.TranslateRStr(call__65);
            FSpellName = call__66;
            FSpellID = 14;
            FBaseManaCost = 5;
            FSpellLevel = 2;
            FMagicSphere = Aow.Units.AoWE.TMagicSphere.msDeath;
            FAboveGroundOnly = false;
        }

        // UnHolyG.pas:383
        public override void SetupTargetSelection(Aow.Units.AoWE.TAbstractUnit A1, Aow.Units.AoWE.TTargetSelectionControl A2)
        {
            int I = default;
            base.SetupTargetSelection(A1, A2);
            A2.FHNs.Clear();
            {
                I = 0;
                int end__67 = Aow.Units.HSEngine.HSEngine.RadToHN(2) - 1;
                for (; I <= end__67; I += 1)
                {
                    A2.FHNs.Add(0);
                }
            }
        }

        // UnHolyG.pas:392
        public override bool TargetSelected(Aow.Units.AoWE.TAbstractUnit A1, int A2, int A3, int A4)
        {
            bool Result = default;
            Aow.Units.UnHolyG.TUnholyGroundSpellTE Event = default;
            Event = Aow.Units.UnHolyG.TUnholyGroundSpellTE.Create(null);
            Event.Initialize(FSpellID, unchecked((byte)A1.FPlayer), A1.FUnitID, FBaseManaCost);
            Event.SetLocation(unchecked((sbyte)A2), unchecked((sbyte)A3), unchecked((sbyte)A4));
            var call__68 = Aow.Units.AoWE.AoWE.AoWHSMap.FTokenManager.GetTokenControl(A1.FPlayer);
            call__68.AddEvent(Event);
            Event.Release();
            Result = true;
            return Result;
        }

        // UnHolyG.pas:368
        public override bool ValidTargetSelectionMapF(Aow.Units.HSEngine.TMapField A1)
        {
            bool Result = default;
            Aow.Units.AoWE.TArmyHS Army = default;
            var call__69 = base.ValidTargetMapF(A1);
            Result = call__69 && TSet<byte>.WithBounds(0, 255).With(12).WithRange(1, 5).Contains((byte)A1.TerrainType);
            // { Original precedence accepts these overlays without the terrain test. }
            Result = (Result && (A1.OverlayType == -1)) || TSet<byte>.WithBounds(0, 255).With(9).WithRange(1, 4).Contains((byte)A1.OverlayType);
            Result = Result && (A1.FindNoneTransparentHS(typeof(Aow.Units.AoWE.TStructure)) == null);
            if (Result)
            {
                Army = ((Aow.Units.AoWE.TAoWMapField)A1).GetArmyHS();
                if (Army != null)
                {
                    Result = !Army.VisibleForSeatedPlayer();
                }
            }

            return Result;
        }

        // UnHolyG.pas:360
        public override bool ValidTargetMapF(Aow.Units.HSEngine.TMapField A1)
        {
            bool Result = default;
            var call__70 = base.ValidTargetMapF(A1);
            Result = call__70 && TSet<byte>.WithBounds(0, 255).With(12).WithRange(1, 5).Contains((byte)A1.TerrainType);
            // { Original precedence accepts these overlays without the terrain test. }
            Result = (Result && (A1.OverlayType == -1)) || TSet<byte>.WithBounds(0, 255).With(9).WithRange(1, 4).Contains((byte)A1.OverlayType);
            bool bool__71 = Result;
            if (bool__71)
            {
                var call__72 = A1.FindNoneTransparentHS(typeof(Aow.Units.AoWE.TStructure));
                bool__71 = (call__72 == null);
            }

            bool bool__73 = bool__71;
            if (bool__73)
            {
                var call__74 = ((Aow.Units.AoWE.TAoWMapField)A1).GetArmyHS();
                bool__73 = (call__74 == null);
            }

            Result = bool__73;
            return Result;
        }

        // UnHolyG.pas:62
        public static new Aow.Units.UnHolyG.TUnholyGroundSpell Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.UnHolyG.TUnholyGroundSpell o = new Aow.Units.UnHolyG.TUnholyGroundSpell();
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

    public static partial class UnHolyG
    {
        // UnHolyG.pas:404
        public static void RegisterUnHolyGround(Aow.Units.AoWE.TAoWEngine AEngine)
        {
            AEngine.RegisterEClasses([Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.UnHolyG.TUnHolyGround>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.UnHolyG.TUnHolyGroundResource>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.UnHolyG.TUnholyGroundSpellTE>()]);
            var call__75 = Aow.Units.UnHolyG.TUnholyGroundSpell.Create(null);
            AEngine.FGameSet.FSpellControl.RegisterSpell(call__75);
        }

        // UnHolyG.pas:410
        public static void UnRegisterUnholyGround(Aow.Units.AoWE.TAoWEngine AEngine)
        {
            AEngine.UnregisterEClasses([Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.UnHolyG.TUnHolyGround>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.UnHolyG.TUnHolyGroundResource>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.UnHolyG.TUnholyGroundSpellTE>()]);
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
