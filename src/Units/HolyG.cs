// Translated from HolyG.pas.
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

namespace Aow.Units.HolyG
{
    public enum AnonymousEnum_hgSummoning : byte
    {
        hgSummoning = 0
    }

    // { THolyGround (instance size 32, VMT 557C79A0): $1C FMode, $1D FPlayer,
//     $1E FDuration, $1F FOriginalOverlay. }
    public partial class THolyGround : Aow.Units.ILTer.TILTerrainMO
    {
        public THolyGround(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
        {}

        // HolyG.pas:92
        public new const int ClassID = 131987;

        public TSet<Aow.Units.HolyG.AnonymousEnum_hgSummoning> FMode;
        // { $1C }
        public sbyte FPlayer;
        // { $1D }
        public sbyte FDuration;
        // { $1E }
        public sbyte FOriginalOverlay;
        // HolyG.pas:201
        // { $1F }
        public override void ReadWrite(Aow.Units.Engine.TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwShortintEx(ref FPlayer, unchecked((sbyte)-1), 40);
            Stream.rwShortintEx(ref FDuration, unchecked((sbyte)-1), 41);
            Stream.rwShortintEx(ref FOriginalOverlay, unchecked((sbyte)-1), 42);
        }

        // HolyG.pas:243
        public override void MsgProc(ref Aow.Units.Engine.TEMsg Msg, Aow.Units.Engine.TEObject Sender)
        {
            base.MsgProc(ref Msg, Sender);
            if (Msg.MsgID == 537001986)
            {
                NewTurn(unchecked((sbyte)Msg.Value));
            }
        }

        // HolyG.pas:249
        public override void Show(int XhpSurf, int YhpSurf, in Aow.Units.GFXE.TSurfaceDesc SurfaceDesc)
        {
            if (!FMode.Contains(Aow.Units.HolyG.AnonymousEnum_hgSummoning.hgSummoning))
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
                UpdateMapFields((Aow.Units.HSEngine.TMapLevel)Owner.Owner, call__2, call__1);
            }

            return Result;
        }

        // HolyG.pas:193
        public override void RemoveFromMap()
        {
            Aow.Units.HSEngine.TMapLevel Level = default;
            Level = (Aow.Units.HSEngine.TMapLevel)Owner.Owner;
            base.RemoveFromMap();
            UpdateMapFields(Level, FXhx, FYhx);
        }

        // HolyG.pas:219
        public override void MapFieldMsgProc(ref Aow.Units.HSEngine.TMapFieldMsg Msg)
        {
            base.MapFieldMsgProc(ref Msg);
            if (Msg.MsgID == 4418)
            {
                var call__3 = FResource.TerrainList.Terrain[Msg.HN];
                if ((call__3 >= 0) && !Aow.Units.AoWE.AoWE.AoWHSMap.Status.Contains(Aow.Units.HSEngine.THSMapStatusElement.msEditting))
                {
                    ArmyPlaced(Msg.Param[0].As<Aow.Units.AoWE.TArmyHS>());
                    return;
                }

                return;
            }

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

        // HolyG.pas:209
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

        // HolyG.pas:181
        public override void UpdateMapField(in Aow.Units.HSEngine.THSMapFieldInfo Info)
        {
            var call__9 = FResource.TerrainList.Terrain[Info.HN];
            if (call__9 >= 0)
            {
                ((Aow.Units.AoWE.TAoWMapField)Info.MapField).FFieldFlags = ((Aow.Units.AoWE.TAoWMapField)Info.MapField).FFieldFlags | 8192;
            }
        }

        // HolyG.pas:95
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
            var call__11 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.HolyGroundDamageRStr);
            Log.FArmyText = call__11;
            var call__12 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.HolyGroundDamageRStr);
            Log.FArmyMessages.Add(call__12);
            Log.FArmyIcon = 2;
            Log.FPlayer = unchecked((sbyte)Army.FArmy.FPlayer);
            Log.FArmyView.Setup(Army.FArmy, 255);
            var army = Army.FArmy;
            {
                I = 0;
                int end__14 = army.Count - 1;
                for (; I <= end__14; I += 1)
                {
                    AUnit = army.GetUnit(I);
                    var call__15 = AUnit.GetAlignment();
                    bool bool__16 = (call__15 != Aow.Units.AoWE.TAlignment.alNone);
                    if (bool__16)
                    {
                        var call__17 = AUnit.GetAlignment();
                        bool__16 = Aow.Units.AoWE.AoWE.alsPureEvil.Contains(call__17);
                    }

                    bool bool__18 = bool__16;
                    if (bool__18)
                    {
                        var call__19 = AUnit.GetAbilityEnabled(1);
                        bool__18 = !call__19;
                    }

                    if (bool__18)
                    {
                        Damage = AUnit.ExecuteDamageRole(3, 4, TSet<Aow.Units.AoWE.TDamageType>.Of(Aow.Units.AoWE.TDamageType.dtHoly));
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
                        Log.FDamageTypes.Add(unchecked((byte)((int)Aow.Units.AoWE.TDamageType.dtHoly)));
                    }
                    else
                    {
                        Log.FDamageValues.Add(0);
                        Log.FDamageTypes.Add(unchecked((byte)((int)Aow.Units.AoWE.TDamageType.dtNone)));
                    }
                }
            }

            Log.ValidateGameOver();
            if (Damaged != 0)
            {
                var call__21 = GetYhx();
                var call__22 = GetLhx();
                var call__23 = GetXhx();
                Aow.Units.EventLog.EventLog.DistributeLocationEventLog(Log, unchecked((sbyte)Log.FPlayer), unchecked((sbyte)call__23), unchecked((sbyte)call__21), unchecked((sbyte)call__22), true);
            }

            Log.Release();
            Army.FArmy.Validate();
        }

        // HolyG.pas:139
        public void ArmyPlaced(Aow.Units.AoWE.TArmyHS Army)
        {
            if (!Aow.Units.AoWE.AoWE.AoWHSMap.Status.Contains(Aow.Units.HSEngine.THSMapStatusElement.msEditting))
            {
                TriggerArmyDamage(Army);
            }
        }

        // HolyG.pas:144
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

        // HolyG.pas:154
        public void NewTurn(sbyte Player)
        {
            Aow.Units.AoWE.TArmyHS Army = default;
            var call__24 = Owner.FindOwnedChild(131607);
            Army = (Aow.Units.AoWE.TArmyHS)call__24;
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

        // HolyG.pas:166
        public bool CanDoDamage(Aow.Units.AoWE.TUnitList Units)
        {
            bool Result = default;
            int I = default;
            Aow.Units.AoWE.TAbstractUnit AUnit = default;
            Result = false;
            var call__25 = Units.Count;
            I = call__25;
            while (!Result && (I != 0))
            {
                I -= 1;
                AUnit = Units.GetUnit(I);
                var call__26 = AUnit.GetImmunityTypes();
                bool bool__27 = !call__26.Contains(Aow.Units.AoWE.TDamageType.dtHoly);
                if (bool__27)
                {
                    var call__28 = AUnit.GetAlignment();
                    bool__27 = (call__28 != Aow.Units.AoWE.TAlignment.alNone);
                }

                bool bool__29 = bool__27;
                if (bool__29)
                {
                    var call__30 = AUnit.GetAlignment();
                    bool__29 = Aow.Units.AoWE.AoWE.alsPureEvil.Contains(call__30);
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
        public void ShowSummonAnimation(Aow.Units.Animation.TAnimation Anim, int XhpSurf, int YhpSurf, in Aow.Units.GFXE.TSurfaceDesc SurfaceDesc)
        {
            Aow.Units.HSEngine.TMapField Field = default;
            var call__33 = Anim.GetLhx();
            var call__34 = Anim.GetYhx();
            var call__35 = Anim.GetXhx();
            var call__36 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.GetField(call__35, call__34, call__33);
            Field = call__36;
            var call__37 = Field.FindOwnedChild(131987);
            if (((this == call__37) && (((int)Aow.Units.AoWE.AoWE.AoWEngine.FAnimationDetail) != 0)) && (Anim.FFrameCounter == 0))
            {
                bool bool__38 = (Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer == Anim.FTag);
                if (!bool__38)
                {
                    var call__39 = GetLhx();
                    var call__40 = GetYhx();
                    var call__41 = GetXhx();
                    var call__42 = Aow.Units.AoWE.AoWE.AoWHSMap.WatchingTerrain(unchecked((sbyte)call__41), unchecked((sbyte)call__40), unchecked((sbyte)call__39), 0);
                    bool__38 = call__42;
                }

                if (bool__38)
                {
                    var call__43 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(13);
                    call__43.FCastSounds.Play(0);
                }
            }
        }

        // HolyG.pas:265
        public void NewFrameSummonAnimation(Aow.Units.Animation.TAnimation Anim, int FrameCounter)
        {
            Aow.Units.HSEngine.TMapField Field = default;
            var call__44 = Anim.GetLhx();
            var call__45 = Anim.GetYhx();
            var call__46 = Anim.GetXhx();
            var call__47 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.GetField(call__46, call__45, call__44);
            Anim = (Aow.Units.Animation.TAnimation)((System.Object)call__47);
            if ((this == ((Aow.Units.HSEngine.TMapField)((System.Object)Anim)).FindOwnedChild(131987)) && (FrameCounter == 9))
            {
                FMode = FMode - TSet<Aow.Units.HolyG.AnonymousEnum_hgSummoning>.Of(Aow.Units.HolyG.AnonymousEnum_hgSummoning.hgSummoning);
            }
        }

        public new partial class Meta : Aow.Units.ILTer.TILTerrainMO.Meta
        {
            // HolyG.pas:92
            public override int ClassID() => Aow.Units.HolyG.THolyGround.ClassID;

            public static new Meta Of<T>()
                where T : Aow.Units.HolyG.THolyGround => (Meta)MetaRegistry.Of(typeof(T));
            // ILTer.pas:20
            public new Aow.Units.HolyG.THolyGround Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.HolyG.THolyGround)base.Create(AOwner);
        }
    }

    // { THolyGroundResource (instance size 68, VMT 557C7B48) }
    public partial class THolyGroundResource : Aow.Units.ILTer.TILTerrainMOR
    {
        public THolyGroundResource(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
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
        public override Aow.Rtl.AnsiString ResourceEditName()
        {
            Aow.Rtl.AnsiString Result = default;
            Result = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.HolyGroundRStr);
            return Result;
        }

        public new partial class Meta : Aow.Units.ILTer.TILTerrainMOR.Meta
        {
            // HolyG.pas:276
            public override int ClassID() => Aow.Units.HolyG.THolyGroundResource.ClassID;

            public static new Meta Of<T>()
                where T : Aow.Units.HolyG.THolyGroundResource => (Meta)MetaRegistry.Of(typeof(T));
            // HSEngine.pas:1194
            public new Aow.Units.HolyG.THolyGroundResource Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.HolyG.THolyGroundResource)base.Create(AOwner);
        }
    }

    // { THolyGroundSpellTE (instance size 36, VMT 557C7C40) }
    public partial class THolyGroundSpellTE : Aow.Units.GlobalTargetSpells.TGlobalTargetSpellTE
    {
        public THolyGroundSpellTE(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
        {}

        // HolyG.pas:282
        public new const int ClassID = 135942;

        // HolyG.pas:329
        public override bool Process(Aow.Units.AoWE.TAbstractUnit A1, int A2)
        {
            bool Result = default;
            base.Process(A1, A2);
            CreateHolyGround(A1);
            Aow.Units.AoWE.AoWE.AoWHSMap.ValidateTerrainObservation(unchecked((sbyte)A1.FPlayer), unchecked((sbyte)FLocation.X), unchecked((sbyte)FLocation.Y), unchecked((sbyte)FLocation.L), 1);
            var call__48 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
            call__48.CastingDone(A1, FEventMana);
            Result = false;
            return Result;
        }

        // HolyG.pas:285
        public void CreateHolyGround(Aow.Units.AoWE.TAbstractUnit Caster)
        {
            Aow.Units.HSEngine.TMapLevel Level = default;
            Aow.Units.HSEngine.THNWalkData Walk = default;
            Aow.Units.HolyG.THolyGroundResource Resource = default;
            Aow.Units.AoWE.TAoWMapField Field = default;
            Aow.Units.HolyG.THolyGround Ground = default;
            Aow.Units.Animation.TImageSequenceAnimation Anim = default;
            var call__49 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.GetMapLevel(FLocation.L);
            Level = call__49;
            Aow.Units.HSEngine.HSEngine.InitHNWalk(ref Walk, FLocation.X, FLocation.Y, 0);
            var call__50 = Aow.Units.AoWE.AoWE.AoWHSSet.MapObjectResourceList.FindChild(131988);
            Resource = (Aow.Units.HolyG.THolyGroundResource)call__50;
            while (Walk.Radius <= 1)
            {
                if ((((Walk.X >= 0) && (Walk.Y >= 0)) && (Walk.X < Level.Width)) && (Walk.Y < Level.Height))
                {
                    var call__51 = Level.Field[Walk.X, Walk.Y];
                    Field = (Aow.Units.AoWE.TAoWMapField)call__51;
                    var call__52 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(13);
                    var call__53 = ((Aow.Units.HolyG.THolyGroundSpell)call__52).ValidTargetMapF(Field);
                    if (call__53)
                    {
                        if (Field.GetArmyHS() == null)
                        {
                            Field.RestoreTerrain();
                            Ground = (Aow.Units.HolyG.THolyGround)Resource.CreateResourceUser();
                            var call__54 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(11);
                            Ground.FDuration = unchecked((sbyte)(((call__54 - 5) / 5) + 3));
                            Ground.FPlayer = unchecked((sbyte)Caster.FPlayer);
                            Ground.FOriginalOverlay = unchecked((sbyte)Field.OverlayType);
                            Aow.Units.AoWE.AoWE.AoWHSMap.ClearTerrain(Walk.X, Walk.Y, FLocation.L, 0, -1);
                            var call__55 = Aow.Units.AoWE.AoWE.AoWHSMap.PlaceHX(Ground, Walk.X, Walk.Y, FLocation.L);
                            if (call__55)
                            {
                                Ground.FMode = TSet<Aow.Units.HolyG.AnonymousEnum_hgSummoning>.Of(Aow.Units.HolyG.AnonymousEnum_hgSummoning.hgSummoning);
                                Anim = new Aow.Units.Animation.TImageSequenceAnimation(null);
                                var call__56 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(13);
                                var call__57 = call__56.FCastImages.ImageSequences[0];
                                Anim.SetImageSequence(call__57);
                                Anim.FOnShow = Ground.ShowSummonAnimation;
                                Anim.FOnNewFrame = Ground.NewFrameSummonAnimation;
                                var call__58 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(15);
                                Anim.FFrameCounter = -call__58;
                                Anim.FTag = Caster.FPlayer;
                                Aow.Units.AoWE.AoWE.AoWHSMap.PlaceHX(Anim, Walk.X, Walk.Y, FLocation.L);
                            }
                            else
                            {
                                Ground.Free();
                            }
                        }
                    }
                }

                Aow.Units.HSEngine.HSEngine.WalkToNextHN(ref Walk);
            }
        }

        // NetworkE.pas:230


        public new partial class Meta : Aow.Units.GlobalTargetSpells.TGlobalTargetSpellTE.Meta
        {
            // HolyG.pas:282
            public override int ClassID() => Aow.Units.HolyG.THolyGroundSpellTE.ClassID;

            public static new Meta Of<T>()
                where T : Aow.Units.HolyG.THolyGroundSpellTE => (Meta)MetaRegistry.Of(typeof(T));
            // NetworkE.pas:230
            public new Aow.Units.HolyG.THolyGroundSpellTE Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.HolyG.THolyGroundSpellTE)base.Create(AOwner);
        }
    }

    // { THolyGroundSpell (instance size 60, VMT 557C7D1C) }
    public partial class THolyGroundSpell : Aow.Units.GlobalTargetSpells.TGlobalTargetSpell
    {
        private protected THolyGroundSpell(Aow.Rtl.ConstructionContext context, Aow.Units.Engine.TECustomNode AOwner) : base(context, AOwner)
        {
            var call__59 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.HolyGroundsRStr);
            FSpellName = call__59;
            FSpellID = 13;
            FBaseManaCost = 5;
            FSpellLevel = 2;
            FMagicSphere = Aow.Units.AoWE.TMagicSphere.msLife;
            FAboveGroundOnly = false;
        }

        public static new THolyGroundSpell Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            return Aow.Rtl.ConstructionContext.Create(AOwner, static (context, owner) => new THolyGroundSpell(context, owner));
        }

        // HolyG.pas:339


        // HolyG.pas:375
        public override void SetupTargetSelection(Aow.Units.AoWE.TAbstractUnit A1, Aow.Units.AoWE.TTargetSelectionControl A2)
        {
            int I = default;
            base.SetupTargetSelection(A1, A2);
            A2.FHNs.Clear();
            {
                I = 0;
                int end__60 = Aow.Units.HSEngine.HSEngine.RadToHN(2) - 1;
                for (; I <= end__60; I += 1)
                {
                    A2.FHNs.Add(0);
                }
            }
        }

        // HolyG.pas:384
        public override bool TargetSelected(Aow.Units.AoWE.TAbstractUnit A1, int A2, int A3, int A4)
        {
            bool Result = default;
            Aow.Units.HolyG.THolyGroundSpellTE Event = default;
            Event = new Aow.Units.HolyG.THolyGroundSpellTE(null);
            Event.Initialize(FSpellID, unchecked((byte)A1.FPlayer), A1.FUnitID, FBaseManaCost);
            Event.SetLocation(unchecked((sbyte)A2), unchecked((sbyte)A3), unchecked((sbyte)A4));
            var call__61 = Aow.Units.AoWE.AoWE.AoWHSMap.FTokenManager.GetTokenControl(A1.FPlayer);
            call__61.AddEvent(Event);
            Event.Release();
            Result = true;
            return Result;
        }

        // HolyG.pas:360
        public override bool ValidTargetSelectionMapF(Aow.Units.HSEngine.TMapField A1)
        {
            bool Result = default;
            Aow.Units.AoWE.TArmyHS Army = default;
            var call__62 = base.ValidTargetMapF(A1);
            Result = call__62 && TSet<byte>.WithBounds(0, 255).With(12).WithRange(1, 5).Contains((byte)((Aow.Units.AoWE.TAoWMapField)A1).TerrainType);
            // { Original overlay test allows these overlays even when the terrain check failed. }
            Result = (Result && (((Aow.Units.AoWE.TAoWMapField)A1).OverlayType == -1)) || TSet<byte>.WithBounds(0, 255).With(9).WithRange(1, 4).Contains((byte)((Aow.Units.AoWE.TAoWMapField)A1).OverlayType);
            Result = Result && (((Aow.Units.AoWE.TAoWMapField)A1).FindNoneTransparentHS(typeof(Aow.Units.AoWE.TStructure)) == null);
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

        // HolyG.pas:350
        public override bool ValidTargetMapF(Aow.Units.HSEngine.TMapField A1)
        {
            bool Result = default;
            var call__63 = base.ValidTargetMapF(A1);
            Result = call__63 && TSet<byte>.WithBounds(0, 255).With(12).WithRange(1, 5).Contains((byte)((Aow.Units.AoWE.TAoWMapField)A1).TerrainType);
            // { Original overlay test allows these overlays even when the terrain check failed. }
            Result = (Result && (((Aow.Units.AoWE.TAoWMapField)A1).OverlayType == -1)) || TSet<byte>.WithBounds(0, 255).With(9).WithRange(1, 4).Contains((byte)((Aow.Units.AoWE.TAoWMapField)A1).OverlayType);
            bool bool__64 = Result;
            if (bool__64)
            {
                var call__65 = ((Aow.Units.AoWE.TAoWMapField)A1).FindNoneTransparentHS(typeof(Aow.Units.AoWE.TStructure));
                bool__64 = (call__65 == null);
            }

            bool bool__66 = bool__64;
            if (bool__66)
            {
                var call__67 = ((Aow.Units.AoWE.TAoWMapField)A1).GetArmyHS();
                bool__66 = (call__67 == null);
            }

            Result = bool__66;
            return Result;
        }

        // HolyG.pas:63

    }

    public static partial class HolyG
    {
        // HolyG.pas:395
        public static void RegisterHolyGround(Aow.Units.Engine.TEngine Engine)
        {
            Engine.RegisterEClasses([Aow.Units.Engine.EClass.Of<Aow.Units.HolyG.THolyGround>(), Aow.Units.Engine.EClass.Of<Aow.Units.HolyG.THolyGroundResource>(), Aow.Units.Engine.EClass.Of<Aow.Units.HolyG.THolyGroundSpellTE>()]);
            var call__68 = Aow.Units.HolyG.THolyGroundSpell.Create(null);
            ((Aow.Units.AoWE.TAoWEngine)Engine).FGameSet.FSpellControl.RegisterSpell(call__68);
        }

        // HolyG.pas:401
        public static void UnRegisterHolyGround(Aow.Units.Engine.TEngine Engine)
        {
            Engine.UnregisterEClasses([Aow.Units.Engine.EClass.Of<Aow.Units.HolyG.THolyGround>(), Aow.Units.Engine.EClass.Of<Aow.Units.HolyG.THolyGroundResource>(), Aow.Units.Engine.EClass.Of<Aow.Units.HolyG.THolyGroundSpellTE>()]);
            ((Aow.Units.AoWE.TAoWEngine)Engine).FGameSet.FSpellControl.UnregisterSpellID(13);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
