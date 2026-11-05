// Translated from GlobalSpells.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using System.Runtime.InteropServices;
using Aow.Units.Engine;
using Aow.Units.GFXE;
using static Aow.Units.GFXE.GFXE;
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngine;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.EventLog;
using static Aow.Units.EventLog.EventLog;
using Aow.Units.Animation;

namespace Aow.Units.GlobalSpells
{
    // X/Y/layer explored hexagons, animation frame
    public partial class TConcealAreaHS : TFixedVisibleHS
    {
        public sbyte FX;
        public sbyte FY;
        public sbyte FLayer;
        public TIntegerList FHexagons;
        public int FFrame;
        // GlobalSpells.pas:205
        public override void Destroy()
        {
            FHexagons.Free();
            base.Destroy();
        }

        // GlobalSpells.pas:199
        public TConcealAreaHS(TECustomNode owner) : base(owner)
        {
            var call__1 = new TIntegerList();
            FHexagons = call__1;
        }

        // GlobalSpells.pas:211
        public override int GetShowPriority()
        {
            int Result = default;
            Result = 2013265920;
            return Result;
        }

        // GlobalSpells.pas:244
        public override void Show(int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
        {
            int Xhp = default;
            int Yhp = default;
            int HX = default;
            int HY = default;
            int Intensity = default;
            int Radius = default;
            int Direction = default;
            int X = default;
            int Y = default;
            int I = default;
            int Delay = default;
            if (AoWHSMap.ViewL == FLayer)
            {
                FFrame += 1;
                if (AoWEngine.FAnimationDetail == 2 && AoWHSMap.ViewL == FLayer)
                {
                    Intensity = 60 - Math.Abs(FFrame * 3 - 60);
                    HXtoHP(FX, FY, ref Xhp, ref Yhp);
                    Xhp = Xhp - XhpSurf - 8;
                    Yhp -= YhpSurf;
                    {
                        I = FHexagons.Count - 1;
                        byte end__2 = 0;
                        for (; I >= end__2; I -= 1)
                        {
                            var call__3 = FHexagons[I];
                            HNtoHP(call__3, ref HX, ref HY);
                            HX += Xhp;
                            HY += Yhp;
                            var call__4 = AoWHSSet.FGeneralImages[5];
                            call__4.ShowBlended(HX, HY, SurfaceDesc, AlphaBlendTable.LookupTables[Intensity]);
                        }
                    }
                }

                // { Original bug: Xhp/Yhp are uninitialized outside high detail. }
                RandSeed = AoWHSMap.FInitialRandomSeed;
                {
                    I = 0;
                    byte end__5 = 200;
                    for (; I <= end__5; I += 1)
                    {
                        Radius = Random(448);
                        Direction = Random(360);
                        Delay = Random(20);
                        if (FFrame - Delay >= 0)
                        {
                            DegreesRadToCo(Direction, Radius, ref X, ref Y);
                            var call__6 = AoWHSSet.FSpellControl.GetSpell(57);
                            call__6.FCastImages.Show(0, Xhp + X, Yhp + Y, FFrame - Delay, SurfaceDesc);
                        }
                    }
                }

                if (FFrame == 40)
                {
                    StopAnimation();
                }
            }
        }

        // GlobalSpells.pas:214
        public void StartAnimation(sbyte X, sbyte Y, sbyte Layer, sbyte Radius)
        {
            THNWalkData Walk = default;
            TMapLevel Level = default;
            FX = X;
            FY = Y;
            FLayer = Layer;
            FFrame = 0;
            var call__7 = AoWHSSet.FSpellControl.GetSpell(1);
            call__7.FCastSounds.Play(0);
            AoWHSMap.DynamicFixedVisibleHS.AddChild(this);
            InitHNWalk(ref Walk, FX, FY, 0);
            var call__8 = AoWHSMap.MapContainer.MapLevels[FLayer];
            Level = call__8;
            while (Walk.Radius <= Radius)
            {
                if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < Level.Width && Walk.Y < Level.Height)
                {
                    var call__9 = Level.GetField(Walk.X, Walk.Y);
                    var call__10 = ((TAoWMapField)call__9).GetExplored(AoWHSMap.FSeatedPlayer);
                    if (call__10)
                    {
                        FHexagons.Add(Walk.HN);
                    }
                }

                WalkToNextHN(ref Walk);
            }
        }

        // GlobalSpells.pas:237
        public void StopAnimation()
        {
            AoWHSMap.DynamicFixedVisibleHS.RemoveChild(this);
            AoWHSMap.DynamicVisibleHS.RemoveChild(this);
            Destroy();
        }

        // GlobalSpells.pas:39

    }

    // inherited name, spell ID, mana, sphere, level, type
    public partial class TConcealArea : TSpell
    {
        // GlobalSpells.pas:286
        public TConcealArea(TECustomNode owner) : base(owner)
        {
            var call__11 = TranslateRStr(ConcealAreaRStr);
            FSpellName = call__11;
            FSpellID = 1;
            FSpellType = TSpellType.stGlobal;
            FBaseManaCost = 5;
            FSpellLevel = 1;
            FMagicSphere = TMagicSphere.msEarth;
        }

        // GlobalSpells.pas:302
        public override bool ExecuteTE(TSpellTE A1, TAbstractUnit A2, int A3)
        {
            bool Result = default;
            TAoWMapField MapF = default;
            sbyte X = default;
            sbyte Y = default;
            sbyte Layer = default;
            int Player = default;
            THNWalkData Walk = default;
            TMapLevel Level = default;
            TLocationTextEventLog Log = default;
            TConcealAreaHS Conceal = default;
            base.ExecuteTE(A1, A2, A3);
            CastingDone(A2, A1.FEventMana);
            A2.GetLocation(ref X, ref Y, ref Layer);
            if (A2.FPlayer == AoWHSMap.FSeatedPlayer)
            {
                Conceal = new TConcealAreaHS(null);
                Conceal.StartAnimation(X, Y, Layer, 14);
            }

            {
                Player = 1;
                int end__12 = AoWHSMap.FPlayers.Count - 1;
                for (; Player <= end__12; Player += 1)
                {
                    var call__13 = AoWHSMap.MapContainer.MapLevels[Layer];
                    Level = call__13;
                    var call__14 = AoWHSMap.FPlayers.GetDiplomaticRelation(A2.FPlayer, (sbyte)Player);
                    if (call__14 == TDiplomaticRelation.drWar && Player != A2.FPlayer)
                    {
                        Log = null;
                        InitHNWalk(ref Walk, X, Y, 0);
                        while (Walk.Radius <= 14)
                        {
                            if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < Level.Width && Walk.Y < Level.Height)
                            {
                                var call__15 = Level.GetField(Walk.X, Walk.Y);
                                MapF = (TAoWMapField)call__15;
                                var call__16 = MapF.GetExplored(Player);
                                bool bool__17 = call__16;
                                if (bool__17)
                                {
                                    var call__18 = AoWHSMap.VisibleForPlayer(Walk.X, Walk.Y, Layer, (sbyte)Player);
                                    bool__17 = !call__18;
                                }

                                if (bool__17)
                                {
                                    MapF.SetExplored(Player, false);
                                    if (Log == null)
                                    {
                                        Log = new TLocationTextEventLog(null);
                                        Log.FXhx = (sbyte)Walk.X;
                                        Log.FYhx = (sbyte)Walk.Y;
                                        Log.FLhx = Layer;
                                        Log.FPlayer = A2.FPlayer;
                                        var call__19 = TranslateRStr(ConcealAreaRStr);
                                        var call__20 = TranslateRStr(XCastRStr);
                                        var call__21 = Rtl.Rtl.Format(call__20, [call__19]);
                                        Log.FText = call__21;
                                        var call__22 = AoWHSMap.FPlayers.GetPlayers(Player);
                                        call__22.FEventLogbook.AddEvent(Log);
                                        Log.Release();
                                    }
                                }
                            }

                            WalkToNextHN(ref Walk);
                        }
                    }
                    else
                    {
                        var call__23 = AoWHSMap.VisibleForPlayer(X, Y, Layer, (sbyte)Player);
                        if (call__23)
                        {
                            Log = new TLocationTextEventLog(null);
                            Log.FXhx = X;
                            Log.FYhx = Y;
                            Log.FLhx = Layer;
                            Log.FPlayer = A2.FPlayer;
                            var call__24 = TranslateRStr(ConcealAreaRStr);
                            var call__25 = TranslateRStr(XCastRStr);
                            var call__26 = Rtl.Rtl.Format(call__25, [call__24]);
                            Log.FText = call__26;
                            var call__27 = AoWHSMap.FPlayers.GetPlayers(Player);
                            call__27.FEventLogbook.AddEvent(Log);
                            Log.Release();
                        }
                    }
                }
            }

            Result = false;
            return Result;
        }

        // GlobalSpells.pas:297
        public override bool GetEnabled()
        {
            bool Result = default;
            Result = AoWHSMap.FExplorationEnabled;
            return Result;
        }

        // GlobalSpells.pas:368
        public override bool CanActivate(TAbstractUnit A1, ref AnsiString A2)
        {
            bool Result = default;
            bool War = default;
            int Player = default;
            Result = base.CanActivate(A1, ref A2);
            if (Result)
            {
                War = false;
                {
                    Player = 1;
                    int end__28 = AoWHSMap.FPlayers.Count - 1;
                    for (; Player <= end__28; Player += 1)
                    {
                        bool bool__29 = War;
                        if (!bool__29)
                        {
                            var call__30 = AoWHSMap.FPlayers.GetPlayers(A1.FPlayer);
                            var call__31 = call__30.FDiplomaticRelations.GetDiplomaticRelation((sbyte)Player);
                            bool__29 = call__31 == TDiplomaticRelation.drWar;
                        }

                        War = bool__29;
                    }
                }

                if (!War)
                {
                    var call__32 = TranslateRStr(ConcealAreaRStr);
                    var call__33 = TranslateRStr(AWarRelationIsRequiredForXRStr);
                    var call__34 = Rtl.Rtl.Format(call__33, [call__32]);
                    A2 = call__34;
                    Result = false;
                }
            }

            return Result;
        }

        // GlobalSpells.pas:388
        public override bool Activate(TAbstractUnit A1)
        {
            bool Result = default;
            AnsiString Reason = default;
            TSpellTE Event = default;
            base.Activate(A1);
            var call__35 = CanActivate(A1, ref Reason);
            bool bool__36 = call__35;
            if (bool__36)
            {
                var call__37 = AoWHSMap.FPlayers.GetPlayers(A1.FPlayer);
                var call__38 = call__37.GetBusy();
                bool__36 = !call__38;
            }

            if (bool__36)
            {
                Event = new TSpellTE(null);
                Event.Initialize(FSpellID, (byte)A1.FPlayer, A1.FUnitID, FBaseManaCost);
                var call__39 = AoWHSMap.FTokenManager.GetTokenControl(A1.FPlayer);
                call__39.AddEvent(Event);
                Event.Release();
                Result = true;
            }
            else
            {
                Result = false;
            }

            return Result;
        }
    }

    // X/Y/layer unexplored hexagons, animation frame
    public partial class TBirdsViewHS : TFixedVisibleHS
    {
        public sbyte FX;
        public sbyte FY;
        public sbyte FLayer;
        public TIntegerList FHexagons;
        public int FFrame;
        // GlobalSpells.pas:412
        public override void Destroy()
        {
            FHexagons.Free();
            base.Destroy();
        }

        // GlobalSpells.pas:406
        public TBirdsViewHS(TECustomNode owner) : base(owner)
        {
            var call__40 = new TIntegerList();
            FHexagons = call__40;
        }

        // GlobalSpells.pas:418
        public override int GetShowPriority()
        {
            int Result = default;
            Result = 2013265920;
            return Result;
        }

        // GlobalSpells.pas:451
        public override void Show(int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
        {
            int Xhp = default;
            int Yhp = default;
            int HX = default;
            int HY = default;
            int Intensity = default;
            int Radius = default;
            int Direction = default;
            int X = default;
            int Y = default;
            int I = default;
            int Delay = default;
            if (AoWHSMap.ViewL == FLayer)
            {
                FFrame += 1;
                Intensity = 100 - FFrame * 2;
                HXtoHP(FX, FY, ref Xhp, ref Yhp);
                Xhp = Xhp - XhpSurf - 8;
                Yhp -= YhpSurf;
                {
                    I = 0;
                    int end__41 = FHexagons.Count - 1;
                    for (; I <= end__41; I += 1)
                    {
                        var call__42 = FHexagons[I];
                        HNtoHP(call__42, ref HX, ref HY);
                        HX += Xhp;
                        HY += Yhp;
                        var call__43 = AoWHSSet.FGeneralImages[5];
                        call__43.ShowBlended(HX, HY, SurfaceDesc, AlphaBlendTable.LookupTables[Intensity]);
                    }
                }

                RandSeed = AoWHSMap.FInitialRandomSeed;
                {
                    I = 0;
                    byte end__44 = 200;
                    for (; I <= end__44; I += 1)
                    {
                        Radius = Random(448);
                        Direction = Random(360);
                        Delay = Random(30);
                        if (FFrame - Delay >= 0)
                        {
                            DegreesRadToCo(Direction, Radius, ref X, ref Y);
                            var call__45 = AoWHSSet.FSpellControl.GetSpell(57);
                            call__45.FCastImages.Show(0, Xhp + X, Yhp + Y, FFrame - Delay, SurfaceDesc);
                        }
                    }
                }

                if (FFrame == 50)
                {
                    StopAnimation();
                }
            }
        }

        // GlobalSpells.pas:421
        public void StartAnimation(sbyte X, sbyte Y, sbyte Layer, sbyte Radius)
        {
            THNWalkData Walk = default;
            TMapLevel Level = default;
            FX = X;
            FY = Y;
            FLayer = Layer;
            FFrame = 0;
            var call__46 = AoWHSSet.FSpellControl.GetSpell(2);
            call__46.FCastSounds.Play(0);
            InitHNWalk(ref Walk, X, Y, 0);
            var call__47 = AoWHSMap.MapContainer.MapLevels[Layer];
            Level = call__47;
            while (Walk.Radius <= Radius)
            {
                if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < Level.Width && Walk.Y < Level.Height)
                {
                    var call__48 = Level.GetField(Walk.X, Walk.Y);
                    var call__49 = ((TAoWMapField)call__48).GetExplored(AoWHSMap.FSeatedPlayer);
                    if (!call__49)
                    {
                        FHexagons.Add(Walk.HN);
                    }
                }

                WalkToNextHN(ref Walk);
            }

            AoWHSMap.DynamicFixedVisibleHS.AddChild(this);
        }

        // GlobalSpells.pas:444
        public void StopAnimation()
        {
            AoWHSMap.DynamicFixedVisibleHS.RemoveChild(this);
            AoWHSMap.DynamicVisibleHS.RemoveChild(this);
            Destroy();
        }

        // GlobalSpells.pas:67

    }

    // inherited name, spell ID, mana, sphere, level, spell type
    public partial class TBirdsView : TSpell
    {
        // GlobalSpells.pas:489
        public TBirdsView(TECustomNode owner) : base(owner)
        {
            var call__50 = TranslateRStr(BirdsViewRStr);
            FSpellName = call__50;
            FSpellID = 2;
            FSpellType = TSpellType.stGlobal;
            FBaseManaCost = 5;
            FSpellLevel = 1;
            FMagicSphere = TMagicSphere.msAir;
        }

        // GlobalSpells.pas:505
        public override bool ExecuteTE(TSpellTE A1, TAbstractUnit A2, int A3)
        {
            bool Result = default;
            sbyte X = default;
            sbyte Y = default;
            sbyte Layer = default;
            base.ExecuteTE(A1, A2, A3);
            CastingDone(A2, A1.FEventMana);
            A2.GetLocation(ref X, ref Y, ref Layer);
            if (AoWHSMap.FSeatedPlayer == A2.FPlayer)
            {
                var birdsViewHS = new TBirdsViewHS(null);
                birdsViewHS.StartAnimation(X, Y, Layer, 14);
            }

            AoWHSMap.ExploreArea(X, Y, Layer, A2.FPlayer, 14);
            Result = false;
            return Result;
        }

        // GlobalSpells.pas:500
        public override bool GetEnabled()
        {
            bool Result = default;
            Result = AoWHSMap.FExplorationEnabled;
            return Result;
        }

        // GlobalSpells.pas:518
        public override bool CanActivate(TAbstractUnit A1, ref AnsiString A2)
        {
            bool Result = default;
            sbyte X = default;
            sbyte Y = default;
            sbyte Layer = default;
            Result = base.CanActivate(A1, ref A2);
            if (Result)
            {
                A1.GetLocation(ref X, ref Y, ref Layer);
                if (Layer != 0)
                {
                    A2 = TranslateRStr(CannotCastSpellInUndergoundRStr);
                    Result = false;
                }
            }

            return Result;
        }

        // GlobalSpells.pas:534
        public override bool Activate(TAbstractUnit A1)
        {
            bool Result = default;
            AnsiString Reason = default;
            TSpellTE Event = default;
            base.Activate(A1);
            var call__52 = CanActivate(A1, ref Reason);
            bool bool__53 = call__52;
            if (bool__53)
            {
                var call__54 = AoWHSMap.FPlayers.GetPlayers(A1.FPlayer);
                var call__55 = call__54.GetBusy();
                bool__53 = !call__55;
            }

            if (bool__53)
            {
                Event = new TSpellTE(null);
                Event.Initialize(FSpellID, (byte)A1.FPlayer, A1.FUnitID, FBaseManaCost);
                var call__56 = AoWHSMap.FTokenManager.GetTokenControl(A1.FPlayer);
                call__56.AddEvent(Event);
                Event.Release();
                Result = true;
            }
            else
            {
                Result = false;
            }

            return Result;
        }
    }

    // army ID, packed destination
    public partial class TWarpPartyTE : TSpellTE
    {
        // GlobalSpells.pas:564
        public new const int ClassID = 135948;

        public TArmyID FArmyID;
        public TXYL FWarpLocation;
        // GlobalSpells.pas:558
        public override void Destroy()
        {
            FArmyID.Free();
            base.Destroy();
        }

        // GlobalSpells.pas:569
        public override void ReadWrite(TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwInteger(ref FWarpLocation.XYL, 30);
            Stream.rwCreateEObject(ref FArmyID, null, 31);
        }

        // GlobalSpells.pas:552
        public TWarpPartyTE(TECustomNode owner) : base(owner)
        {
            FArmyID = null;
        }

        // GlobalSpells.pas:576
        public override bool Validate(THero A1)
        {
            bool Result = default;
            byte Selection = default;
            TArmyHS Army = default;
            var call__57 = base.Validate(A1);
            bool bool__58 = call__57;
            if (bool__58)
            {
                var call__59 = FArmyID.Valid();
                bool__58 = call__59;
            }

            Result = bool__58;
            if (Result)
            {
                FArmyID.RetrieveArmy(ref Army, ref Selection);
                var call__60 = AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
                var call__61 = ((TWarpParty)call__60).ValidTarget(Army, FWarpLocation.X, FWarpLocation.Y, FWarpLocation.L);
                Result = call__61;
            }

            return Result;
        }

        // GlobalSpells.pas:590
        public override bool Process(TAbstractUnit A1, int A2)
        {
            bool Result = default;
            bool ShowSource = default;
            bool ShowTarget = default;
            byte Selection = default;
            sbyte X = default;
            sbyte Y = default;
            sbyte Layer = default;
            TArmyHS Army = default;
            TMapField Field = default;
            TImageSequenceAnimation Animation = default;
            base.Process(A1, A2);
            A1.GetLocation(ref X, ref Y, ref Layer);
            FArmyID.RetrieveArmy(ref Army, ref Selection);
            var call__62 = AoWHSMap.MapContainer.GetFieldXYL(FWarpLocation.XYL);
            Field = call__62;
            Army.MoveTo(Field, 0);
            bool bool__63 = AoWHSMap.FSeatedPlayer == A1.FPlayer;
            if (!bool__63)
            {
                var call__64 = AoWHSMap.VisibleForPlayer(X, Y, Layer, AoWHSMap.FSeatedPlayer);
                bool__63 = call__64;
            }

            ShowSource = bool__63;
            bool bool__65 = AoWHSMap.FSeatedPlayer == A1.FPlayer;
            if (!bool__65)
            {
                var call__66 = AoWHSMap.VisibleForPlayer(Field.Xhx, Field.Yhx, Field.Lhx, AoWHSMap.FSeatedPlayer);
                bool__65 = call__66;
            }

            ShowTarget = bool__65;
            bool bool__67 = ShowSource;
            if (bool__67)
            {
                var call__68 = AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
                var call__69 = call__68.FCastImages.ImageSequences[0];
                bool__67 = call__69 != null;
            }

            if (bool__67)
            {
                Animation = new TImageSequenceAnimation(null);
                var call__70 = AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
                var call__71 = call__70.FCastImages.ImageSequences[0];
                Animation.SetImageSequence(call__71);
                AoWHSMap.PlaceHX(Animation, X, Y, Layer);
            }

            bool bool__72 = ShowTarget;
            if (bool__72)
            {
                var call__73 = AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
                var call__74 = call__73.FCastImages.ImageSequences[0];
                bool__72 = call__74 != null;
            }

            if (bool__72)
            {
                Animation = new TImageSequenceAnimation(null);
                var call__75 = AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
                var call__76 = call__75.FCastImages.ImageSequences[0];
                Animation.SetImageSequence(call__76);
                AoWHSMap.PlaceHX(Animation, Field.Xhx, Field.Yhx, Field.Lhx);
            }

            if (A1.FPlayer == AoWHSMap.FSeatedPlayer)
            {
                AoWHSMap.CenterViewTo(Field.Xhx, Field.Yhx, Field.Lhx);
            }
            else
            {
                AoWHSMap.ValidateObservation(A1.FPlayer, Field.Xhx, Field.Yhx, Field.Lhx, 0);
            }

            bool bool__77 = A1.FPlayer == AoWHSMap.FSeatedPlayer;
            if (!bool__77)
            {
                var call__78 = AoWHSMap.Watching(X, Y, Layer, 0);
                bool__77 = call__78;
            }

            bool bool__79 = bool__77;
            if (!bool__79)
            {
                var call__80 = AoWHSMap.Watching(Field.Xhx, Field.Yhx, Field.Lhx, 0);
                bool__79 = call__80;
            }

            if (bool__79)
            {
                var call__81 = AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
                call__81.FCastSounds.Play(0);
            }

            var call__82 = AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
            var call__83 = AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
            call__83.CastingDone(A1, call__82.FBaseManaCost);
            Result = false;
            return Result;
        }

        // GlobalSpells.pas:92


    }

    // inherited name, ID, mana, sphere, level, spell type
    public partial class TWarpParty : TSpell
    {
        // GlobalSpells.pas:632
        public TWarpParty(TECustomNode owner) : base(owner)
        {
            var call__84 = TranslateRStr(WarpPartyRStr);
            FSpellName = call__84;
            FSpellID = 34;
            FSpellType = TSpellType.stGlobal;
            FBaseManaCost = 1;
            FSpellLevel = 2;
            FMagicSphere = TMagicSphere.msFire;
        }

        // GlobalSpells.pas:719
        public override bool Activate(TAbstractUnit A1)
        {
            bool Result = default;
            AnsiString Reason = default;
            TWarpPartyTE Event = default;
            base.Activate(A1);
            var call__85 = CanActivate(A1, ref Reason);
            bool bool__86 = call__85;
            if (bool__86)
            {
                var call__87 = AoWHSMap.FPlayers.GetPlayers(A1.FPlayer);
                var call__88 = call__87.GetBusy();
                bool__86 = !call__88;
            }

            if (bool__86)
            {
                Event = new TWarpPartyTE(null);
                Event.Initialize(FSpellID, (byte)A1.FPlayer, A1.FUnitID, FBaseManaCost);
                var call__89 = ((TArmy)A1.Owner).CreateID(255);
                Event.FArmyID = call__89;
                var call__90 = ((TArmy)A1.Owner).GetArmyHS();
                var call__91 = CalculateWarpLocation(call__90);
                Event.FWarpLocation.XYL = call__91;
                AoWHSMap.FSelectedArmy.Unselect();
                if (Event.FWarpLocation.XYL != 16777215)
                {
                    var call__92 = AoWHSMap.FTokenManager.GetTokenControl(A1.FPlayer);
                    call__92.AddEvent(Event);
                    Result = true;
                }
                else
                {
                    Result = false;
                }

                Event.Release();
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // GlobalSpells.pas:643
        public bool ValidTarget(TArmyHS Army, sbyte X, sbyte Y, sbyte Layer)
        {
            bool Result = default;
            TAoWMapField Field = default;
            TMapFieldMsg Msg = default;
            var call__93 = AoWHSMap.MapContainer.GetField(X, Y, Layer);
            Field = (TAoWMapField)call__93;
            var call__94 = Field.GetArmyHS();
            bool bool__95 = call__94 == null;
            if (bool__95)
            {
                var call__96 = Field.GetPlayer();
                bool__95 = call__96 == -1;
            }

            Result = bool__95;
            if (Result)
            {
                Msg.MsgID = 4384;
                Msg.Param[0] = Army;
                Msg.Result = 0;
                Field.SendMapFieldMsg(ref Msg);
                bool bool__97 = Msg.Result.Value == 0;
                if (bool__97)
                {
                    var call__98 = Army.FArmy.CanPlace(X, Y, Layer);
                    bool__97 = call__98;
                }

                Result = bool__97;
            }

            return Result;
        }

        // GlobalSpells.pas:660
        public int CalculateWarpLocation(TArmyHS Army)
        {
            int Result = default;
            TList Candidates = default;
            FixedArray_Aow_Units_FixedArray_sbyte_16_16 Table = default;
            TMapFieldMsg Msg = default;
            THNWalkData Walk = default;
            TMapLevel Level = default;
            TAoWMapField Field = default;
            int L = default;
            int I = default;
            bool Found = default;
            Candidates = new TList();
            Army.FArmy.CreateMovePointTable(255, Bytes.Of(ref Table));
            var call__99 = Army.Yhx;
            var call__100 = Army.Xhx;
            InitHNWalk(ref Walk, call__100, call__99, 10);
            var call__101 = AoWHSMap.MapContainer.MapLevels[Army.Lhx];
            Level = call__101;
            while (Walk.Radius < 20)
            {
                if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < Level.Width && Walk.Y < Level.Height)
                {
                    var call__102 = Level.GetField(Walk.X, Walk.Y);
                    Field = (TAoWMapField)call__102;
                    if (Table[Field.TerrainType][Field.OverlayType - -1] != -1)
                    {
                        Candidates.Add(Field);
                    }
                }

                WalkToNextHN(ref Walk);
            }

            {
                L = 0;
                int end__103 = AoWHSMap.MapContainer.LevelCount - 1;
                for (; L <= end__103; L += 1)
                {
                    if (Math.Abs(L - Army.Lhx) == 1)
                    {
                        var call__104 = AoWHSMap.MapContainer.MapLevels[L];
                        Level = call__104;
                        var call__105 = Army.Yhx;
                        var call__106 = Army.Xhx;
                        InitHNWalk(ref Walk, call__106, call__105, 0);
                        while (Walk.Radius < 10)
                        {
                            if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < Level.Width && Walk.Y < Level.Height)
                            {
                                var call__107 = Level.GetField(Walk.X, Walk.Y);
                                Field = (TAoWMapField)call__107;
                                if (Table[Field.TerrainType][Field.OverlayType - -1] != -1)
                                {
                                    Candidates.Add(Field);
                                }
                            }

                            WalkToNextHN(ref Walk);
                        }
                    }
                }
            }

            Found = false;
            Result = 16777215;
            while (Candidates.Count != 0 && !Found)
            {
                var call__108 = Random(Candidates.Count);
                I = call__108;
                var call__109 = Candidates[I];
                Field = (TAoWMapField)call__109;
                var call__110 = Field.GetArmyHS();
                bool bool__111 = call__110 == null;
                if (bool__111)
                {
                    var call__112 = Field.GetPlayer();
                    bool__111 = call__112 == -1;
                }

                if (bool__111)
                {
                    Msg.MsgID = 4384;
                    Msg.Param[0] = Army;
                    Msg.Result = 0;
                    Field.SendMapFieldMsg(ref Msg);
                    bool bool__113 = Msg.Result.Value == 0;
                    if (bool__113)
                    {
                        var call__114 = Army.FArmy.CanPlace(Field.Xhx, Field.Yhx, Field.Lhx);
                        bool__113 = call__114;
                    }

                    Found = bool__113;
                }

                if (Found)
                {
                    Result = Field.GetXYL();
                }
                else
                {
                    Candidates.Delete(I);
                }
            }

            Candidates.Free();
            return Result;
        }
    }

    // inherited name, spell ID, mana, sphere, level, type, sounds, images
    public partial class TResurrectHero : TSpell
    {
        // GlobalSpells.pas:980
        public TResurrectHero(TECustomNode owner) : base(owner)
        {
            var call__115 = TranslateRStr(ResurrectHeroRStr);
            FSpellName = call__115;
            FSpellID = 66;
            FSpellType = TSpellType.stGlobal;
            FBaseManaCost = 0;
            FSpellLevel = 2;
            FMagicSphere = TMagicSphere.msCosmos;
        }

        // GlobalSpells.pas:991
        public override bool ExecuteTE(TSpellTE A1, TAbstractUnit A2, int A3)
        {
            bool Result = default;
            sbyte X = default;
            sbyte Y = default;
            sbyte Layer = default;
            TTwinkleAnimation Animation = default;
            THeroList Heroes = default;
            int I = default;
            THero Hero = default;
            TArmyHS Army = default;
            TLocationTextEventLog Log = default;
            THeroEmergeEventLog EmergeLog = default;
            TArmyInfoEventLog InfoLog = default;
            if (A3 == 0)
            {
                A2.GetLocation(ref X, ref Y, ref Layer);
                AoWHSMap.ValidateObservation(A2.FPlayer, X, Y, Layer, 0);
                Animation = new TTwinkleAnimation(null);
                var call__116 = FCastImages.GetImageSequence(0);
                Animation.Setup(call__116, TSet<TTwinkleModeElement>.Of(TTwinkleModeElement.tmRotate), 50, 64, 50, 30);
                AoWHSMap.PlaceHX(Animation, X, Y, Layer);
                bool bool__117 = A2.FPlayer == AoWHSMap.FSeatedPlayer;
                if (!bool__117)
                {
                    var call__118 = AoWHSMap.Watching(X, Y, Layer, 0);
                    bool__117 = call__118;
                }

                if (bool__117)
                {
                    FCastSounds.Play(0);
                }
            }

            if (A3 == 20)
            {
                Heroes = new THeroList(null);
                AoWHSMap.FHeroControl.ListDeadHeroes(Heroes);
                {
                    I = Heroes.Count - 1;
                    byte end__119 = 0;
                    for (; I >= end__119; I -= 1)
                    {
                        var call__120 = Heroes.GetHero(I);
                        bool bool__121 = call__120 is TLeader;
                        if (!bool__121)
                        {
                            var call__122 = Heroes.GetHero(I);
                            var call__123 = call__122.GetLevel();
                            var call__124 = AoWHSMap.FPlayers.GetPlayers(A2.FPlayer);
                            bool__121 = call__123 > call__124.FBaseHeroJoinLevel + 2;
                        }

                        if (bool__121)
                        {
                            var call__125 = Heroes.GetHero(I);
                            Heroes.RemoveChild(call__125);
                        }
                    }
                }

                var call__126 = Heroes.Count;
                if (call__126 != 0)
                {
                    A2.GetLocation(ref X, ref Y, ref Layer);
                    var call__127 = Heroes.Count;
                    var call__128 = AoWHSMap.Random(call__127);
                    var call__129 = Heroes.GetHero(call__128);
                    Hero = call__129;
                    Hero.Resurrect();
                    Army = Hero.Place(X, Y, Layer, 0, TSet<TUnitPlaceModeElement>.Of(TUnitPlaceModeElement.pmScanArea, TUnitPlaceModeElement.pmCreateNewArmy));
                    Log = new TLocationTextEventLog(null);
                    Log.FPlayer = A2.FPlayer;
                    var call__130 = TranslateRStr(XResurrectedRStr);
                    var call__131 = Rtl.Rtl.Format(call__130, [Hero.FHeroName]);
                    Log.FText = call__131;
                    var call__132 = Army.GetLhx();
                    var call__133 = Army.GetYhx();
                    var call__134 = Army.GetXhx();
                    Log.SetLocation(call__134, call__133, call__132);
                    var call__135 = Army.GetYhx();
                    var call__136 = Army.GetLhx();
                    var call__137 = Army.GetXhx();
                    DistributeLocationEventLog(Log, A2.FPlayer, call__137, call__135, call__136, false);
                    Log.Release();
                    var call__138 = Hero.OfferToJoin(A2.FPlayer);
                    if (call__138)
                    {
                        EmergeLog = new THeroEmergeEventLog(null);
                        EmergeLog.Setup(Hero, A2.FPlayer, THeroJoinMode.hjmResurrected);
                        EmergeLog.Execute(true, TExecuteTrigger.etEvent, null);
                        EmergeLog.Release();
                    }
                    else
                    {
                        InfoLog = new TArmyInfoEventLog(null);
                        var call__139 = Army.FArmy.IndexOf(Hero);
                        InfoLog.FArmyView.Setup(Army.FArmy, (byte)(1 << call__139));
                        InfoLog.FSelection = 255;
                        InfoLog.FPlayer = A2.FPlayer;
                        var call__140 = TranslateRStr(NoJoinResurrectionRStr);
                        InfoLog.FMessages.Text = call__140;
                        InfoLog.FTriggers = TSet<TExecuteTrigger>.Of(TExecuteTrigger.etEvent);
                        InfoLog.Execute(true, TExecuteTrigger.etEvent, null);
                        InfoLog.Release();
                    }
                }
                else
                {
                    var call__141 = TranslateRStr(NoHeroAnswersYourCallRStr);
                    AoWHSMap.ShowMessage(call__141);
                }

                CastingDone(A2, A1.FEventMana);
                Heroes.Free();
                Result = false;
            }
            else
            {
                Result = true;
            }

            return Result;
        }

        // GlobalSpells.pas:1062
        public override bool Activate(TAbstractUnit A1)
        {
            bool Result = default;
            AnsiString Reason = default;
            TSpellTE Event = default;
            base.Activate(A1);
            var call__142 = CanActivate(A1, ref Reason);
            bool bool__143 = call__142;
            if (bool__143)
            {
                var call__144 = AoWHSMap.FPlayers.GetPlayers(A1.FPlayer);
                var call__145 = call__144.GetBusy();
                bool__143 = !call__145;
            }

            if (bool__143)
            {
                Event = new TSpellTE(null);
                Event.Initialize(FSpellID, (byte)A1.FPlayer, A1.FUnitID, FBaseManaCost);
                var call__146 = AoWHSMap.FTokenManager.GetTokenControl(A1.FPlayer);
                call__146.AddEvent(Event);
                Event.Release();
                Result = true;
            }
            else
            {
                Result = false;
            }

            return Result;
        }
    }

    // inherited name, spell ID, mana, sphere, level, type, sounds, images
    public partial class TAnimateHero : TSpell
    {
        // GlobalSpells.pas:1080
        public TAnimateHero(TECustomNode owner) : base(owner)
        {
            var call__147 = TranslateRStr(AnimateHeroRStr);
            FSpellName = call__147;
            FSpellID = 67;
            FSpellType = TSpellType.stGlobal;
            FBaseManaCost = 0;
            FSpellLevel = 2;
            FMagicSphere = TMagicSphere.msCosmos;
        }

        // GlobalSpells.pas:1091
        public override bool ExecuteTE(TSpellTE A1, TAbstractUnit A2, int A3)
        {
            bool Result = default;
            sbyte X = default;
            sbyte Y = default;
            sbyte Layer = default;
            TTwinkleAnimation Animation = default;
            THeroList Heroes = default;
            int I = default;
            THero Hero = default;
            TArmyHS Army = default;
            TLocationTextEventLog Log = default;
            THeroEmergeEventLog EmergeLog = default;
            TArmyInfoEventLog InfoLog = default;
            if (A3 == 0)
            {
                A2.GetLocation(ref X, ref Y, ref Layer);
                AoWHSMap.ValidateObservation(A2.FPlayer, X, Y, Layer, 0);
                Animation = new TTwinkleAnimation(null);
                var call__148 = FCastImages.GetImageSequence(0);
                Animation.Setup(call__148, TSet<TTwinkleModeElement>.Of(TTwinkleModeElement.tmRotate), 50, 64, 50, 30);
                AoWHSMap.PlaceHX(Animation, X, Y, Layer);
                bool bool__149 = A2.FPlayer == AoWHSMap.FSeatedPlayer;
                if (!bool__149)
                {
                    var call__150 = AoWHSMap.Watching(X, Y, Layer, 0);
                    bool__149 = call__150;
                }

                if (bool__149)
                {
                    FCastSounds.Play(0);
                }
            }

            if (A3 == 20)
            {
                Heroes = new THeroList(null);
                AoWHSMap.FHeroControl.ListDeadHeroes(Heroes);
                {
                    I = Heroes.Count - 1;
                    byte end__151 = 0;
                    for (; I >= end__151; I -= 1)
                    {
                        var call__152 = Heroes.GetHero(I);
                        bool bool__153 = call__152 is TLeader;
                        if (!bool__153)
                        {
                            var call__154 = Heroes.GetHero(I);
                            var call__155 = call__154.GetLevel();
                            var call__156 = AoWHSMap.FPlayers.GetPlayers(A2.FPlayer);
                            bool__153 = call__155 > call__156.FBaseHeroJoinLevel + 2;
                        }

                        if (bool__153)
                        {
                            var call__157 = Heroes.GetHero(I);
                            Heroes.RemoveChild(call__157);
                        }
                    }
                }

                var call__158 = Heroes.Count;
                if (call__158 != 0)
                {
                    A2.GetLocation(ref X, ref Y, ref Layer);
                    var call__159 = Heroes.Count;
                    var call__160 = AoWHSMap.Random(call__159);
                    var call__161 = Heroes.GetHero(call__160);
                    Hero = call__161;
                    Hero.Animate();
                    Army = Hero.Place(X, Y, Layer, 0, TSet<TUnitPlaceModeElement>.Of(TUnitPlaceModeElement.pmScanArea, TUnitPlaceModeElement.pmCreateNewArmy));
                    Log = new TLocationTextEventLog(null);
                    Log.FPlayer = A2.FPlayer;
                    var call__162 = TranslateRStr(XAnimatedRStr);
                    var call__163 = Rtl.Rtl.Format(call__162, [Hero.FHeroName]);
                    Log.FText = call__163;
                    var call__164 = Army.GetLhx();
                    var call__165 = Army.GetYhx();
                    var call__166 = Army.GetXhx();
                    Log.SetLocation(call__166, call__165, call__164);
                    var call__167 = Army.GetYhx();
                    var call__168 = Army.GetLhx();
                    var call__169 = Army.GetXhx();
                    DistributeLocationEventLog(Log, A2.FPlayer, call__169, call__167, call__168, false);
                    Log.Release();
                    var call__170 = Hero.OfferToJoin(A2.FPlayer);
                    if (call__170)
                    {
                        EmergeLog = new THeroEmergeEventLog(null);
                        EmergeLog.Setup(Hero, A2.FPlayer, THeroJoinMode.hjmResurrected);
                        EmergeLog.Execute(true, TExecuteTrigger.etEvent, null);
                        EmergeLog.Release();
                    }
                    else
                    {
                        InfoLog = new TArmyInfoEventLog(null);
                        var call__171 = Army.FArmy.IndexOf(Hero);
                        InfoLog.FArmyView.Setup(Army.FArmy, (byte)(1 << call__171));
                        InfoLog.FSelection = 255;
                        InfoLog.FPlayer = A2.FPlayer;
                        var call__172 = TranslateRStr(NoJoinAnimateRStr);
                        InfoLog.FMessages.Text = call__172;
                        InfoLog.FTriggers = TSet<TExecuteTrigger>.Of(TExecuteTrigger.etEvent);
                        InfoLog.Execute(true, TExecuteTrigger.etEvent, null);
                        InfoLog.Release();
                    }
                }
                else
                {
                    if (A2.FPlayer == AoWHSMap.FSeatedPlayer)
                    {
                        var call__173 = TranslateRStr(NoHeroAnswersYourCallRStr);
                        AoWHSMap.ShowMessage(call__173);
                        var call__174 = AoWHSSet.FSpellControl.GetSpell(FSpellID);
                        call__174.FCastSounds.Play(0);
                    }
                }

                CastingDone(A2, A1.FEventMana);
                Heroes.Free();
                Result = false;
            }
            else
            {
                Result = true;
            }

            return Result;
        }

        // GlobalSpells.pas:1166
        public override bool Activate(TAbstractUnit A1)
        {
            bool Result = default;
            AnsiString Reason = default;
            TSpellTE Event = default;
            base.Activate(A1);
            var call__175 = CanActivate(A1, ref Reason);
            bool bool__176 = call__175;
            if (bool__176)
            {
                var call__177 = AoWHSMap.FPlayers.GetPlayers(A1.FPlayer);
                var call__178 = call__177.GetBusy();
                bool__176 = !call__178;
            }

            if (bool__176)
            {
                Event = new TSpellTE(null);
                Event.Initialize(FSpellID, (byte)A1.FPlayer, A1.FUnitID, FBaseManaCost);
                var call__179 = AoWHSMap.FTokenManager.GetTokenControl(A1.FPlayer);
                call__179.AddEvent(Event);
                Event.Release();
                Result = true;
            }
            else
            {
                Result = false;
            }

            return Result;
        }
    }

    // inherited name, spell ID, mana, sphere, level, type, sounds, images
    public partial class TCallHero : TSpell
    {
        // GlobalSpells.pas:1184
        public TCallHero(TECustomNode owner) : base(owner)
        {
            var call__180 = TranslateRStr(CallHeroRStr);
            FSpellName = call__180;
            FSpellID = 69;
            FSpellType = TSpellType.stGlobal;
            FBaseManaCost = 0;
            FSpellLevel = 2;
            FMagicSphere = TMagicSphere.msCosmos;
        }

        // GlobalSpells.pas:1250
        public override bool ExecuteTE(TSpellTE A1, TAbstractUnit A2, int A3)
        {
            bool Result = default;
            bool ShowSource = default;
            bool ShowTarget = default;
            sbyte X = default;
            sbyte Y = default;
            sbyte Layer = default;
            THero Hero = default;
            TArmyHS Army = default;
            TImageSequenceAnimation Animation = default;
            base.ExecuteTE(A1, A2, A3);
            A2.GetLocation(ref X, ref Y, ref Layer);
            Hero = FindCallHero(A2);
            if (Hero != null)
            {
                if (Hero.Owner == null)
                {
                    var call__181 = Hero.GetHits();
                    Hero.SetHitPoints(call__181);
                }

                Army = Hero.Place(X, Y, Layer, 0, TSet<TUnitPlaceModeElement>.Of(TUnitPlaceModeElement.pmScanArea, TUnitPlaceModeElement.pmCreateNewArmy));
                bool bool__182 = AoWHSMap.FSeatedPlayer == A2.FPlayer;
                if (!bool__182)
                {
                    var call__183 = AoWHSMap.VisibleForPlayer(X, Y, Layer, AoWHSMap.FSeatedPlayer);
                    bool__182 = call__183;
                }

                ShowSource = bool__182;
                bool bool__184 = AoWHSMap.FSeatedPlayer == A2.FPlayer;
                if (!bool__184)
                {
                    var call__185 = Army.GetLhx();
                    var call__186 = Army.GetYhx();
                    var call__187 = Army.GetXhx();
                    var call__188 = AoWHSMap.VisibleForPlayer(call__187, call__186, call__185, AoWHSMap.FSeatedPlayer);
                    bool__184 = call__188;
                }

                ShowTarget = bool__184;
                if (ShowSource)
                {
                    var call__189 = AoWHSSet.FSpellControl.GetSpell(FSpellID);
                    var call__190 = call__189.FCastImages.GetImageSequence(0);
                    if (call__190 != null)
                    {
                        Animation = new TImageSequenceAnimation(null);
                        var call__191 = AoWHSSet.FSpellControl.GetSpell(FSpellID);
                        var call__192 = call__191.FCastImages.GetImageSequence(0);
                        Animation.SetImageSequence(call__192);
                        AoWHSMap.PlaceHX(Animation, X, Y, Layer);
                    }
                }

                if (ShowTarget)
                {
                    var call__193 = AoWHSSet.FSpellControl.GetSpell(FSpellID);
                    var call__194 = call__193.FCastImages.GetImageSequence(0);
                    if (call__194 != null)
                    {
                        Animation = new TImageSequenceAnimation(null);
                        var call__195 = AoWHSSet.FSpellControl.GetSpell(FSpellID);
                        var call__196 = call__195.FCastImages.GetImageSequence(0);
                        Animation.SetImageSequence(call__196);
                        var call__197 = Army.GetLhx();
                        var call__198 = Army.GetYhx();
                        var call__199 = Army.GetXhx();
                        AoWHSMap.PlaceHX(Animation, call__199, call__198, call__197);
                    }
                }

                if (AoWHSMap.FSeatedPlayer == A2.FPlayer)
                {
                    var call__200 = Army.GetLhx();
                    var call__201 = Army.GetYhx();
                    var call__202 = Army.GetXhx();
                    AoWHSMap.CenterViewTo(call__202, call__201, call__200);
                }
                else
                {
                    var call__203 = Army.GetLhx();
                    var call__204 = Army.GetYhx();
                    var call__205 = Army.GetXhx();
                    AoWHSMap.ValidateObservation(A2.FPlayer, call__205, call__204, call__203, 0);
                }

                bool bool__206 = AoWHSMap.FSeatedPlayer == A2.FPlayer;
                if (!bool__206)
                {
                    var call__207 = AoWHSMap.Watching(X, Y, Layer, 0);
                    bool__206 = call__207;
                }

                bool bool__208 = bool__206;
                if (!bool__208)
                {
                    var call__209 = Army.GetLhx();
                    var call__210 = Army.GetYhx();
                    var call__211 = Army.GetXhx();
                    var call__212 = AoWHSMap.Watching(call__211, call__210, call__209, 0);
                    bool__208 = call__212;
                }

                if (bool__208)
                {
                    var call__213 = AoWHSSet.FSpellControl.GetSpell(FSpellID);
                    call__213.FCastSounds.Play(0);
                }
            }
            else
            {
                if (AoWHSMap.FSeatedPlayer == A2.FPlayer)
                {
                    var call__214 = TranslateRStr(NoHeroAnsweredYourCallRStr);
                    AoWHSMap.ShowMessage(call__214);
                    var call__215 = AoWHSSet.FSpellControl.GetSpell(FSpellID);
                    call__215.FCastSounds.Play(0);
                }
            }

            var call__216 = AoWHSSet.FSpellControl.GetSpell(FSpellID);
            var call__217 = AoWHSSet.FSpellControl.GetSpell(FSpellID);
            call__217.CastingDone(A2, call__216.FBaseManaCost);
            Result = false;
            return Result;
        }

        // GlobalSpells.pas:1195
        public override bool Activate(TAbstractUnit A1)
        {
            bool Result = default;
            AnsiString Reason = default;
            TSpellTE Event = default;
            var call__218 = CanActivate(A1, ref Reason);
            bool bool__219 = call__218;
            if (bool__219)
            {
                var call__220 = AoWHSMap.FPlayers.GetPlayers(A1.FPlayer);
                var call__221 = call__220.GetBusy();
                bool__219 = !call__221;
            }

            if (bool__219)
            {
                Event = new TSpellTE(null);
                Event.Initialize(FSpellID, (byte)A1.FPlayer, A1.FUnitID, FBaseManaCost);
                var call__222 = AoWHSMap.FTokenManager.GetTokenControl(A1.FPlayer);
                call__222.AddEvent(Event);
                Event.Release();
                Result = true;
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // GlobalSpells.pas:1212
        public THero FindCallHero(TAbstractUnit Source)
        {
            THero Result = default;
            THero Hero = default;
            int I = default;
            TDoubleItemList Heroes = default;
            int Index = default;
            Heroes = new TDoubleItemList();
            {
                I = AoWHSMap.FHeroControl.GetCount() - 1;
                byte end__223 = 0;
                for (; I >= end__223; I -= 1)
                {
                    var call__224 = AoWHSMap.FHeroControl.GetHero(I);
                    Hero = call__224;
                    if (Hero.FPlayer == 0 && Hero.Owner is TArmy && ((TArmy)Hero.Owner).FX != -1)
                    {
                        var call__225 = Hero.OfferToJoin(Source.FPlayer);
                        if (call__225)
                        {
                            Heroes.AddAB(ObjectToInt(Hero), 10);
                        }
                        else
                        {
                            Heroes.AddAB(ObjectToInt(Hero), 1);
                        }
                    }
                }
            }

            if (Heroes.Count == 0)
            {
                {
                    I = AoWHSMap.FHeroControl.GetCount() - 1;
                    byte end__226 = 0;
                    for (; I >= end__226; I -= 1)
                    {
                        var call__227 = AoWHSMap.FHeroControl.GetHero(I);
                        Hero = call__227;
                        if (!Hero.FUnitStatus.Contains(TUnitStatusElement.usKilled) && Hero.FPlayer <= 0 && Hero.Owner == null)
                        {
                            var call__228 = Hero.OfferToJoin(Source.FPlayer);
                            if (call__228)
                            {
                                Heroes.AddAB(ObjectToInt(Hero), 10);
                            }
                            else
                            {
                                Heroes.AddAB(ObjectToInt(Hero), 1);
                            }
                        }
                    }
                }
            }

            if (Heroes.Count != 0)
            {
                var call__229 = AoWHSMap.Random(16777215);
                RandSeed = call__229;
                Index = Heroes.RandomChanceItemB(0);
                var call__230 = Heroes[Index];
                Result = ReferenceFromId<THero>(call__230.A);
            }
            else
            {
                Result = null;
            }

            Heroes.Free();
            return Result;
        }
    }

    // dispel chance, enchantment ID
    public partial class TDisjunctionTE : TSpellTE
    {
        public TDisjunctionTE(TECustomNode owner) : base(owner)
        { }

        // GlobalSpells.pas:745
        public new const int ClassID = 135961;

        public int FDispelChance;
        public int FEnchantmentID;
        // GlobalSpells.pas:750
        public override void ReadWrite(TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwInteger(ref FEnchantmentID, 20);
            Stream.rwInteger(ref FDispelChance, 21);
        }

        // GlobalSpells.pas:757
        public override bool Validate(THero A1)
        {
            bool Result = default;
            int Index = default;
            int Mana = default;
            int RequiredMana = default;
            Result = base.Validate(A1);
            var call__231 = AoWHSMap.FGlobalMagicControl.IndexOfID(FEnchantmentID);
            Index = call__231;
            Result = Result && Index != -1;
            if (Result)
            {
                var globalEnchantment = AoWHSMap.FGlobalMagicControl.FEnchantments.GetEnchantments(Index);
                var call__233 = AoWHSMap.FPlayers.GetPlayers(A1.FPlayer);
                Mana = call__233.FMagicControl.FMana + A1.FCastingProgress;
                RequiredMana = Round((double)((double)globalEnchantment.FDispelMana * 0.02) * FDispelChance) + FEventMana;
                Result = RequiredMana <= Mana;
            }

            return Result;
        }

        // GlobalSpells.pas:772
        public override bool Process(TAbstractUnit A1, int A2)
        {
            bool Result = default;
            TGlobalEnchantment Enchantment = default;
            int Index = default;
            sbyte X = default;
            sbyte Y = default;
            sbyte Layer = default;
            TTwinkleAnimation Twinkle = default;
            int Mana = default;
            base.Process(A1, A2);
            var call__234 = AoWHSMap.FGlobalMagicControl.IndexOfID(FEnchantmentID);
            Index = call__234;
            var call__235 = AoWHSMap.FGlobalMagicControl.FEnchantments.GetEnchantments(Index);
            Enchantment = call__235;
            A1.GetLocation(ref X, ref Y, ref Layer);
            AoWHSMap.ValidateObservation(A1.FPlayer, X, Y, Layer, 0);
            Twinkle = new TTwinkleAnimation(null);
            var call__236 = AoWHSSet.FSpellControl.GetSpell(51);
            var call__237 = call__236.FCastImages.ImageSequences[0];
            Twinkle.Setup(call__237, TSet<TTwinkleModeElement>.Of(TTwinkleModeElement.tmExpand, TTwinkleModeElement.tmExplode), FEventMana + 50, 192, 50, 30);
            AoWHSMap.PlaceHX(Twinkle, X, Y, Layer);
            var call__238 = AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
            call__238.FCastSounds.Play(0);
            Mana = Round((double)((double)Enchantment.FDispelMana * 0.02) * FDispelChance);
            var call__239 = AoWHSMap.FPlayers.GetPlayers(A1.FPlayer);
            var call__240 = AoWHSMap.FPlayers.GetPlayers(A1.FPlayer);
            call__240.FMagicControl.SetMana(call__239.FMagicControl.FMana - Mana);
            var call__241 = AoWHSMap.Random(100);
            if (call__241 < FDispelChance)
            {
                if (AoWHSMap.FSeatedPlayer == A1.FPlayer)
                {
                    var call__242 = Enchantment.GetName();
                    var call__243 = TranslateRStr(XSuccessfullyDispelledRStr);
                    var call__244 = Rtl.Rtl.Format(call__243, [call__242]);
                    AoWHSMap.ShowMessage(call__244);
                }

                AoWHSMap.FGlobalMagicControl.ExecuteDispelEnchantment(A1.FPlayer, FEnchantmentID);
            }
            else
            {
                if (AoWHSMap.FSeatedPlayer == A1.FPlayer)
                {
                    // { Original formats this message with an unused enchantment-name argument. }
                    var call__245 = Enchantment.GetName();
                    var call__246 = TranslateRStr(DispelMagicFailedRStr);
                    var call__247 = Rtl.Rtl.Format(call__246, [call__245]);
                    AoWHSMap.ShowMessage(call__247);
                }
            }

            var call__248 = AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
            call__248.CastingDone(A1, FEventMana);
            Result = false;
            return Result;
        }

        // NetworkE.pas:230


    }

    // enchantment ID, dispel chance. inherited spell ID, source unit ID, references
    public partial class TDisjunctionSpellCaster : TSpellCaster
    {
        public int FEnchantmentID;
        public int FDispelChance;
        // GlobalSpells.pas:806
        public TDisjunctionSpellCaster(TECustomNode owner) : base(owner)
        {
            FDispelChance = 50;
        }

        // GlobalSpells.pas:868
        public override bool CanCast(ref AnsiString Reason)
        {
            bool Result = default;
            THero Source = default;
            Result = base.CanCast(ref Reason);
            var call__249 = AoWHSMap.FUnitControl.FindUnit(FCasterSourceID);
            Source = (THero)call__249;
            if (Result && SelectedEnchantment() != null)
            {
                bool bool__250 = !AoWHSMap.FGlobalMagicControl.FActiveEnchantments.Contains(TGlobalEnchantmentType.getSpellWard);
                if (!bool__250)
                {
                    var call__251 = SelectedEnchantment();
                    bool__250 = call__251.FEnchantmentType == TGlobalEnchantmentType.getSpellWard;
                }

                if (bool__250)
                {
                    var call__252 = AoWHSMap.FPlayers.GetPlayers(Source.FPlayer);
                    var call__253 = call__252.FMagicControl.GetCheatEnabled();
                    if (!call__253)
                    {
                        var call__254 = AoWHSSet.FSpellControl.GetSpell(51);
                        var call__255 = GetMana();
                        var call__256 = AoWHSMap.FPlayers.GetPlayers(Source.FPlayer);
                        if (call__256.FMagicControl.FMana + Source.FCastingProgress >= call__254.FBaseManaCost + call__255)
                        {
                            Result = true;
                        }
                        else
                        {
                            Result = false;
                            Reason = TranslateRStr(NotEnoughManaRStr);
                        }
                    }
                    else
                    {
                        Result = true;
                    }
                }
                else
                {
                    Result = false;
                    Reason = TranslateRStr(CannotDispelWhenSpellWardIsActiveRStr);
                }
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // GlobalSpells.pas:901
        public override bool Cast()
        {
            bool Result = default;
            AnsiString Reason = default;
            TAbstractUnit Source = default;
            TDisjunctionTE Event = default;
            if (CanCast(ref Reason))
            {
                var call__257 = AoWHSMap.FUnitControl.FindUnit(FCasterSourceID);
                Source = call__257;
                var call__258 = AoWHSMap.FPlayers.GetPlayers(Source.FPlayer);
                var call__259 = call__258.GetBusy();
                if (!call__259)
                {
                    Event = new TDisjunctionTE(null);
                    var call__260 = AoWHSSet.FSpellControl.GetSpell(51);
                    Event.Initialize(FCasterSpellID, (byte)Source.FPlayer, Source.FUnitID, call__260.FBaseManaCost);
                    Event.FEnchantmentID = FEnchantmentID;
                    Event.FDispelChance = FDispelChance;
                    var call__261 = AoWHSMap.FTokenManager.GetTokenControl(Source.FPlayer);
                    call__261.AddEvent(Event);
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
                AoWHSMap.ShowMessage(Reason);
            }

            return Result;
        }

        // GlobalSpells.pas:812
        public void SetEnchantmentID(int A1)
        {
            FEnchantmentID = A1;
        }

        // GlobalSpells.pas:817
        public TGlobalEnchantment SelectedEnchantment()
        {
            TGlobalEnchantment Result = default;
            int Index = default;
            var call__262 = AoWHSMap.FGlobalMagicControl.IndexOfID(FEnchantmentID);
            Index = call__262;
            if (Index != -1)
            {
                var call__263 = AoWHSMap.FGlobalMagicControl.FEnchantments.GetEnchantments(Index);
                Result = call__263;
            }
            else
            {
                Result = null;
            }

            return Result;
        }

        // GlobalSpells.pas:828
        public int GetMana()
        {
            int Result = default;
            TGlobalEnchantment Enchantment = default;
            Enchantment = SelectedEnchantment();
            if (Enchantment != null)
            {
                Result = Round((double)((double)Enchantment.FDispelMana * 0.02) * FDispelChance);
            }
            else
            {
                Result = -1;
            }

            return Result;
        }

        // GlobalSpells.pas:839
        public void SetDispelChance(int A1)
        {
            FDispelChance = A1;
            if (FDispelChance < 1)
            {
                FDispelChance = 1;
            }

            if (FDispelChance > 75)
            {
                FDispelChance = 75;
            }
        }

        // GlobalSpells.pas:846
        public int GetDispelChanceMax()
        {
            int Result = default;
            Result = 75;
            return Result;
        }

        // GlobalSpells.pas:851
        public bool GetEnoughMana()
        {
            bool Result = default;
            THero Source = default;
            int Mana = default;
            Result = false;
            var call__264 = AoWHSMap.FUnitControl.FindUnit(FCasterSourceID);
            Source = (THero)call__264;
            if (SelectedEnchantment() != null)
            {
                var call__265 = AoWHSMap.FPlayers.GetPlayers(Source.FPlayer);
                var call__266 = call__265.FMagicControl.GetCheatEnabled();
                if (!call__266)
                {
                    var call__267 = AoWHSMap.FPlayers.GetPlayers(Source.FPlayer);
                    Mana = call__267.FMagicControl.FMana + Source.FCastingProgress;
                    var call__268 = AoWHSSet.FSpellControl.GetSpell(51);
                    var call__269 = GetMana();
                    if (Mana >= call__268.FBaseManaCost + call__269)
                    {
                        Result = true;
                    }
                }
                else
                {
                    Result = true;
                }
            }

            return Result;
        }

    }

    // inherited name, spell ID, mana, sphere, level, type, sounds, images
    public partial class TDisjunction : TSpell
    {
        // GlobalSpells.pas:931
        public TDisjunction(TECustomNode owner) : base(owner)
        {
            var call__270 = TranslateRStr(DisjunctionRStr);
            FSpellName = call__270;
            FSpellID = 51;
            FSpellType = TSpellType.stGlobal;
            FBaseManaCost = 0;
            FSpellLevel = 2;
            FMagicSphere = TMagicSphere.msCosmos;
        }

        // GlobalSpells.pas:942
        public override bool CanActivate(TAbstractUnit A1, ref AnsiString A2)
        {
            bool Result = default;
            int Count = default;
            int I = default;
            AnsiString Reason = default;
            Count = 0;
            {
                I = 0;
                int end__271 = AoWHSMap.FGlobalMagicControl.FEnchantments.Count - 1;
                for (; I <= end__271; I += 1)
                {
                    var call__272 = AoWHSMap.FGlobalMagicControl.FEnchantments.GetEnchantments(I);
                    bool bool__273 = call__272.FPlayerIndex != A1.FPlayer;
                    if (bool__273)
                    {
                        var call__274 = AoWHSMap.FGlobalMagicControl.FEnchantments.GetEnchantments(I);
                        var call__275 = call__274.VisibleForPlayer(A1.FPlayer);
                        bool__273 = call__275;
                    }

                    bool bool__276 = bool__273;
                    if (bool__276)
                    {
                        var call__277 = AoWHSMap.FGlobalMagicControl.FEnchantments.GetEnchantments(I);
                        var call__278 = call__277.CanDispel(A1.FPlayer, ref Reason);
                        bool__276 = call__278;
                    }

                    if (bool__276)
                    {
                        Count += 1;
                    }
                }
            }

            if (Count == 0)
            {
                A2 = TranslateRStr(NoGlobalOpponentEnchantmentsActiveRStr);
                Result = false;
            }
            else
            {
                Result = true;
            }

            return Result;
        }

        // GlobalSpells.pas:962
        public override bool Activate(TAbstractUnit A1)
        {
            bool Result = default;
            AnsiString Reason = default;
            TDisjunctionSpellCaster Caster = default;
            if (CanActivate(A1, ref Reason))
            {
                Caster = new TDisjunctionSpellCaster(null);
                Caster.FCasterSpellID = FSpellID;
                Caster.FCasterSourceID = A1.FUnitID;
                AoWHSMap.FCosmeticSurgeryEvents.TriggerNotifyEvents(Caster);
                Caster.Release();
                Result = true;
            }
            else
            {
                Result = false;
            }

            return Result;
        }
    }

    // inherited name, spell ID, mana, sphere, level, type, sounds, images
    public partial class TCrusade : TSpell
    {
        // GlobalSpells.pas:1302
        public TCrusade(TECustomNode owner) : base(owner)
        {
            var call__279 = TranslateRStr(CrusadeRStr);
            FSpellName = call__279;
            FSpellID = 75;
            FSpellType = TSpellType.stGlobal;
            FBaseManaCost = 0;
            FSpellLevel = 4;
            FMagicSphere = TMagicSphere.msLife;
        }

        // GlobalSpells.pas:1340
        public override bool ExecuteTE(TSpellTE A1, TAbstractUnit A2, int A3)
        {
            bool Result = default;
            int I = default;
            int J = default;
            int Count = default;
            int UnitIndex = default;
            int Xhp = default;
            int Yhp = default;
            sbyte X = default;
            sbyte Y = default;
            sbyte Layer = default;
            TArmyHS Party = default;
            TUnit NewUnit = default;
            if (A3 == 0)
            {
                A2.GetLocation(ref X, ref Y, ref Layer);
                AoWHSMap.ValidateObservation(A2.FPlayer, X, Y, Layer, 0);
                HXtoHP(X, Y, ref Xhp, ref Yhp);
                var worldFXHS = AoWHSMap.AddEffect(TworldFXType.fxCrusade);
                worldFXHS.FX = Xhp + 16;
                worldFXHS.FY = Yhp + 16;
                worldFXHS.FLayer = Layer;

                var call__281 = AoWHSMap.Random(100);
                if (call__281 < 20)
                {
                    Party = new TArmyHS(null);
                    Party.FArmy.SetPlayer(0);
                    Count = 1;
                    var call__282 = AoWHSMap.Random(1);
                    UnitIndex = call__282;
                    {
                        J = 0;
                        int end__283 = Count - 1;
                        for (; J <= end__283; J += 1)
                        {
                            NewUnit = new TUnit(null);
                            var call__284 = AoWHSSet.FUnitResources.GetUnitResource(GlobalSpells.TCrusade_ExecuteTE__LargeUnits[UnitIndex]);
                            NewUnit.SetUnitResource(call__284);
                            NewUnit.ExpandAbility(101);
                            Party.FArmy.AddUnit(NewUnit);
                            NewUnit.Release();
                        }
                    }

                    if (!PlaceParty(A2, Party))
                    {
                        Party.Free();
                    }
                }
                else
                {
                    {
                        I = 0;
                        byte end__285 = 1;
                        for (; I <= end__285; I += 1)
                        {
                            Party = new TArmyHS(null);
                            Party.FArmy.SetPlayer(0);
                            var call__286 = AoWHSMap.Random(3);
                            Count = call__286 + 4;
                            {
                                J = 0;
                                int end__287 = Count - 1;
                                for (; J <= end__287; J += 1)
                                {
                                    NewUnit = new TUnit(null);
                                    var call__288 = AoWHSMap.Random(2);
                                    UnitIndex = call__288;
                                    var call__289 = AoWHSSet.FUnitResources.GetUnitResource(GlobalSpells.TCrusade_ExecuteTE__FootUnits[UnitIndex]);
                                    NewUnit.SetUnitResource(call__289);
                                    NewUnit.ExpandAbility(101);
                                    Party.FArmy.AddUnit(NewUnit);
                                    NewUnit.Release();
                                }
                            }

                            if (!PlaceParty(A2, Party))
                            {
                                Party.Free();
                            }
                        }
                    }

                    Party = new TArmyHS(null);
                    Party.FArmy.SetPlayer(0);
                    var call__290 = AoWHSMap.Random(3);
                    UnitIndex = call__290;
                    Count = 2;
                    if (UnitIndex == 2)
                    {
                        Count += 1;
                    }

                    {
                        J = 0;
                        int end__291 = Count - 1;
                        for (; J <= end__291; J += 1)
                        {
                            NewUnit = new TUnit(null);
                            var call__292 = AoWHSSet.FUnitResources.GetUnitResource(GlobalSpells.TCrusade_ExecuteTE__MountedUnits[UnitIndex]);
                            NewUnit.SetUnitResource(call__292);
                            NewUnit.ExpandAbility(101);
                            Party.FArmy.AddUnit(NewUnit);
                            NewUnit.Release();
                        }
                    }

                    if (!PlaceParty(A2, Party))
                    {
                        Party.Free();
                    }
                }

                var call__293 = AoWHSSet.FSpellControl.GetSpell(FSpellID);
                call__293.CastingDone(A2, FBaseManaCost);
                var call__294 = AoWHSSet.FSpellControl.GetSpell(FSpellID);
                call__294.FCastSounds.Play(0);
            }

            Result = false;
            return Result;
        }

        // GlobalSpells.pas:1417
        public override bool Activate(TAbstractUnit A1)
        {
            bool Result = default;
            AnsiString Reason = default;
            TSpellTE Event = default;
            base.Activate(A1);
            var call__295 = CanActivate(A1, ref Reason);
            bool bool__296 = call__295;
            if (bool__296)
            {
                var call__297 = AoWHSMap.FPlayers.GetPlayers(A1.FPlayer);
                var call__298 = call__297.GetBusy();
                bool__296 = !call__298;
            }

            if (bool__296)
            {
                Event = new TSpellTE(null);
                Event.Initialize(FSpellID, (byte)A1.FPlayer, A1.FUnitID, FBaseManaCost);
                var call__299 = AoWHSMap.FTokenManager.GetTokenControl(A1.FPlayer);
                call__299.AddEvent(Event);
                Event.Release();
                Result = true;
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // GlobalSpells.pas:1313
        public bool PlaceParty(TAbstractUnit Source, TArmyHS Party)
        {
            bool Result = default;
            int TargetX = default;
            int TargetY = default;
            sbyte X = default;
            sbyte Y = default;
            sbyte Layer = default;
            TImageSequenceAnimation Animation = default;
            Source.GetLocation(ref X, ref Y, ref Layer);
            var call__300 = RadToHN(5);
            var call__301 = AoWHSMap.Random(call__300);
            CenterHNtoHX(X, Y, call__301, ref TargetX, ref TargetY);
            if (Party.PlaceEx((sbyte)TargetX, (sbyte)TargetY, Layer, TSet<TArmyPlaceModeElement>.Of(TArmyPlaceModeElement.apSafe, TArmyPlaceModeElement.apScanArea)))
            {
                Party.Activate();
                Party.FArmy.SetBehavior(255, TGroupBehavior.gbSuicidal);
                var call__302 = Party.GetLhx();
                var call__303 = Party.GetYhx();
                var call__304 = Party.GetXhx();
                var call__305 = AoWHSMap.Watching(call__304, call__303, call__302, 0);
                if (call__305)
                {
                    var call__306 = AoWHSSet.FSpellControl.GetSpell(34);
                    var call__307 = call__306.FCastImages.GetImageSequence(0);
                    if (call__307 != null)
                    {
                        // { Original uses Warp Party's images for Crusade parties. }
                        Animation = new TImageSequenceAnimation(null);
                        var call__308 = AoWHSSet.FSpellControl.GetSpell(34);
                        var call__309 = call__308.FCastImages.GetImageSequence(0);
                        Animation.SetImageSequence(call__309);
                        Animation.FAnimationLevel = 28;
                        var call__310 = Party.GetLhx();
                        var call__311 = Party.GetYhx();
                        var call__312 = Party.GetXhx();
                        AoWHSMap.PlaceHX(Animation, call__312, call__311, call__310);
                    }
                }

                Result = true;
            }
            else
            {
                Result = false;
            }

            return Result;
        }
    }

    [StructLayout(LayoutKind.Explicit, Size = 110)]
    public struct TGlobalSpellEngineFields
    {
        [FieldOffset(0)]
        public FixedArray_byte_109 Reserved;
        [FieldOffset(109)]
        public byte Detail;
    }

    public static partial class GlobalSpells
    {
        public static FixedArray_int_1 TCrusade_ExecuteTE__LargeUnits = FixedArray_int_1.From([133]);
        public static FixedArray_int_3 TCrusade_ExecuteTE__MountedUnits = FixedArray_int_3.From([132, 134, 128]);
        public static FixedArray_int_2 TCrusade_ExecuteTE__FootUnits = FixedArray_int_2.From([129, 126]);
        // GlobalSpells.pas:1435
        public static void RegisterGlobalSpells(TAoWEngine Engine)
        {
            TSpellControl Control = default;
            Control = Engine.FGameSet.FSpellControl;
            Engine.RegisterEClasses([EClass.Of<TWarpPartyTE>(), EClass.Of<TDisjunctionTE>()]);
            var call__313 = new TConcealArea(null);
            Control.RegisterSpell(call__313);
            var call__314 = new TBirdsView(null);
            Control.RegisterSpell(call__314);
            var call__315 = new TWarpParty(null);
            Control.RegisterSpell(call__315);
            var call__316 = new TDisjunction(null);
            Control.RegisterSpell(call__316);
            var call__317 = new TResurrectHero(null);
            Control.RegisterSpell(call__317);
            var call__318 = new TAnimateHero(null);
            Control.RegisterSpell(call__318);
            var call__319 = new TCallHero(null);
            Control.RegisterSpell(call__319);
            var call__320 = new TCrusade(null);
            Control.RegisterSpell(call__320);
        }

        // GlobalSpells.pas:1451
        public static void UnregisterGlobalSpells(TAoWEngine Engine)
        {
            TSpellControl Control = default;
            Control = Engine.FGameSet.FSpellControl;
            Engine.UnregisterEClasses([EClass.Of<TWarpPartyTE>(), EClass.Of<TDisjunctionTE>()]);
            Control.UnregisterSpellID(1);
            Control.UnregisterSpellID(2);
            Control.UnregisterSpellID(34);
            Control.UnregisterSpellID(51);
            Control.UnregisterSpellID(66);
            Control.UnregisterSpellID(67);
            Control.UnregisterSpellID(69);
            Control.UnregisterSpellID(75);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
