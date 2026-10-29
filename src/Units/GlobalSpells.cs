// Translated from GlobalSpells.pas.
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
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.EventLog;
using static Aow.Units.EventLog.EventLog;
using Aow.Units.Animation;
using static Aow.Units.Animation.Animation;

namespace Aow.Units.GlobalSpells
{
    // { TConcealAreaHS (instance size 24, VMT 557E8FBC): $0C/$0D/$0E
//     X/Y/layer, $0F padding, $10 explored hexagons, $14 animation frame. }
    public partial class TConcealAreaHS : Aow.Units.HSEngine.TFixedVisibleHS
    {
        public sbyte FX;
        public sbyte FY;
        public sbyte FLayer;
        public Aow.Units.FixedArray_byte_1 F_ConcealAreaHS_0F;
        public Aow.Units.Engine.TIntegerList FHexagons;
        public int FFrame;
        // GlobalSpells.pas:205
        public override void Destroy()
        {
            FHexagons.Free();
            base.Destroy();
        }

        // GlobalSpells.pas:199
        // { needs: pointer and Boolean results on the shared procedure drafts. }
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            var call__1 = Aow.Units.Engine.TIntegerList.Create();
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
        public override void Show(int XhpSurf, int YhpSurf, in Aow.Units.GFXE.TSurfaceDesc SurfaceDesc)
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
            if (Aow.Units.AoWE.AoWE.AoWHSMap.ViewL == FLayer)
            {
                FFrame += 1;
                if ((Aow.Units.AoWE.AoWE.AoWEngine.FAnimationDetail == 2) && (Aow.Units.AoWE.AoWE.AoWHSMap.ViewL == FLayer))
                {
                    Intensity = 60 - System.Math.Abs((FFrame * 3) - 60);
                    Aow.Units.HSEngine.HSEngine.HXtoHP(FX, FY, ref Xhp, ref Yhp);
                    Xhp = (Xhp - XhpSurf) - 8;
                    Yhp -= YhpSurf;
                    {
                        I = FHexagons.Count - 1;
                        byte end__2 = 0;
                        for (; I >= end__2; I -= 1)
                        {
                            var call__3 = FHexagons[I];
                            Aow.Units.HSEngine.HSEngine.HNtoHP(call__3, ref HX, ref HY);
                            HX += Xhp;
                            HY += Yhp;
                            var call__4 = Aow.Units.AoWE.AoWE.AoWHSSet.FGeneralImages[5];
                            call__4.ShowBlended(HX, HY, SurfaceDesc, Aow.Units.GFXE.GFXE.AlphaBlendTable.LookupTables[Intensity]);
                        }
                    }
                }

                // { Original bug: Xhp/Yhp are uninitialized outside high detail. }
                Aow.Rtl.Rtl.RandSeed = Aow.Units.AoWE.AoWE.AoWHSMap.FInitialRandomSeed;
                {
                    I = 0;
                    byte end__5 = 200;
                    for (; I <= end__5; I += 1)
                    {
                        Radius = Aow.Rtl.Rtl.Random(448);
                        Direction = Aow.Rtl.Rtl.Random(360);
                        Delay = Aow.Rtl.Rtl.Random(20);
                        if ((FFrame - Delay) >= 0)
                        {
                            Aow.Units.AoWE.AoWE.DegreesRadToCo(Direction, Radius, ref X, ref Y);
                            var call__6 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(57);
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
            Aow.Units.HSEngine.THNWalkData Walk = default;
            Aow.Units.HSEngine.TMapLevel Level = default;
            FX = unchecked((sbyte)X);
            FY = unchecked((sbyte)Y);
            FLayer = unchecked((sbyte)Layer);
            FFrame = 0;
            var call__7 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(1);
            call__7.FCastSounds.Play(0);
            Aow.Units.AoWE.AoWE.AoWHSMap.DynamicFixedVisibleHS.AddChild(this);
            Aow.Units.HSEngine.HSEngine.InitHNWalk(ref Walk, FX, FY, 0);
            var call__8 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.MapLevels[FLayer];
            Level = call__8;
            while (Walk.Radius <= Radius)
            {
                if ((((Walk.X >= 0) && (Walk.Y >= 0)) && (Walk.X < Level.Width)) && (Walk.Y < Level.Height))
                {
                    var call__9 = Level.GetField(Walk.X, Walk.Y);
                    var call__10 = ((Aow.Units.AoWE.TAoWMapField)call__9).GetExplored(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
                    if (call__10)
                    {
                        FHexagons.Add(Walk.HN);
                    }
                }

                Aow.Units.HSEngine.HSEngine.WalkToNextHN(ref Walk);
            }
        }

        // GlobalSpells.pas:237
        public void StopAnimation()
        {
            Aow.Units.AoWE.AoWE.AoWHSMap.DynamicFixedVisibleHS.RemoveChild(this);
            Aow.Units.AoWE.AoWE.AoWHSMap.DynamicVisibleHS.RemoveChild(this);
            Destroy();
        }

        // GlobalSpells.pas:39
        public static new Aow.Units.GlobalSpells.TConcealAreaHS Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.GlobalSpells.TConcealAreaHS o = new Aow.Units.GlobalSpells.TConcealAreaHS();
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

    // { draft }
    // { TConcealArea (instance size 52, VMT 557E9144): inherited $08 name, $10 spell ID, $14 mana, $20 sphere, $21 level, $22 type. }
    public partial class TConcealArea : Aow.Units.AoWE.TSpell
    {
        // GlobalSpells.pas:286
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            var call__11 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.ConcealAreaRStr);
            FSpellName = call__11;
            FSpellID = 1;
            FSpellType = Aow.Units.AoWE.TSpellType.stGlobal;
            FBaseManaCost = 5;
            FSpellLevel = 1;
            FMagicSphere = Aow.Units.AoWE.TMagicSphere.msEarth;
        }

        // GlobalSpells.pas:302
        public override bool ExecuteTE(Aow.Units.AoWE.TSpellTE A1, Aow.Units.AoWE.TAbstractUnit A2, int A3)
        {
            bool Result = default;
            Aow.Units.AoWE.TAoWMapField MapF = default;
            sbyte X = default;
            sbyte Y = default;
            sbyte Layer = default;
            int Player = default;
            Aow.Units.HSEngine.THNWalkData Walk = default;
            Aow.Units.HSEngine.TMapLevel Level = default;
            Aow.Units.EventLog.TLocationTextEventLog Log = default;
            Aow.Units.GlobalSpells.TConcealAreaHS Conceal = default;
            base.ExecuteTE(A1, A2, A3);
            CastingDone(A2, A1.FEventMana);
            A2.GetLocation(ref X, ref Y, ref Layer);
            if (A2.FPlayer == Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer)
            {
                Conceal = Aow.Units.GlobalSpells.TConcealAreaHS.Create(null);
                Conceal.StartAnimation(unchecked((sbyte)X), unchecked((sbyte)Y), unchecked((sbyte)Layer), 14);
            }

            {
                Player = 1;
                int end__12 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.Count - 1;
                for (; Player <= end__12; Player += 1)
                {
                    var call__13 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.MapLevels[Layer];
                    Level = call__13;
                    var call__14 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetDiplomaticRelation(unchecked((sbyte)A2.FPlayer), unchecked((sbyte)Player));
                    if ((call__14 == Aow.Units.AoWE.TDiplomaticRelation.drWar) && (Player != A2.FPlayer))
                    {
                        Log = null;
                        Aow.Units.HSEngine.HSEngine.InitHNWalk(ref Walk, X, Y, 0);
                        while (Walk.Radius <= 14)
                        {
                            if ((((Walk.X >= 0) && (Walk.Y >= 0)) && (Walk.X < Level.Width)) && (Walk.Y < Level.Height))
                            {
                                var call__15 = Level.GetField(Walk.X, Walk.Y);
                                MapF = (Aow.Units.AoWE.TAoWMapField)call__15;
                                var call__16 = MapF.GetExplored(Player);
                                bool bool__17 = call__16;
                                if (bool__17)
                                {
                                    var call__18 = Aow.Units.AoWE.AoWE.AoWHSMap.VisibleForPlayer(Walk.X, Walk.Y, Layer, unchecked((sbyte)Player));
                                    bool__17 = !call__18;
                                }

                                if (bool__17)
                                {
                                    MapF.SetExplored(Player, false);
                                    if (Log == null)
                                    {
                                        Log = Aow.Units.EventLog.TLocationTextEventLog.Create(null);
                                        Log.FXhx = unchecked((sbyte)Walk.X);
                                        Log.FYhx = unchecked((sbyte)Walk.Y);
                                        Log.FLhx = unchecked((sbyte)Layer);
                                        Log.FPlayer = unchecked((sbyte)A2.FPlayer);
                                        var call__19 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.ConcealAreaRStr);
                                        var call__20 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.XCastRStr);
                                        var call__21 = Aow.Rtl.Rtl.Format(call__20, [call__19]);
                                        Log.FText = call__21;
                                        var call__22 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Player);
                                        call__22.FEventLogbook.AddEvent(Log);
                                        Log.Release();
                                    }
                                }
                            }

                            Aow.Units.HSEngine.HSEngine.WalkToNextHN(ref Walk);
                        }
                    }
                    else
                    {
                        var call__23 = Aow.Units.AoWE.AoWE.AoWHSMap.VisibleForPlayer(X, Y, Layer, unchecked((sbyte)Player));
                        if (call__23)
                        {
                            Log = Aow.Units.EventLog.TLocationTextEventLog.Create(null);
                            Log.FXhx = unchecked((sbyte)X);
                            Log.FYhx = unchecked((sbyte)Y);
                            Log.FLhx = unchecked((sbyte)Layer);
                            Log.FPlayer = unchecked((sbyte)A2.FPlayer);
                            var call__24 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.ConcealAreaRStr);
                            var call__25 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.XCastRStr);
                            var call__26 = Aow.Rtl.Rtl.Format(call__25, [call__24]);
                            Log.FText = call__26;
                            var call__27 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Player);
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
            Result = Aow.Units.AoWE.AoWE.AoWHSMap.FExplorationEnabled;
            return Result;
        }

        // GlobalSpells.pas:368
        public override bool CanActivate(Aow.Units.AoWE.TAbstractUnit A1, ref Aow.Rtl.AnsiString A2)
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
                    int end__28 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.Count - 1;
                    for (; Player <= end__28; Player += 1)
                    {
                        bool bool__29 = War;
                        if (!bool__29)
                        {
                            var call__30 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(A1.FPlayer);
                            var call__31 = call__30.FDiplomaticRelations.GetDiplomaticRelation(unchecked((sbyte)Player));
                            bool__29 = (call__31 == Aow.Units.AoWE.TDiplomaticRelation.drWar);
                        }

                        War = bool__29;
                    }
                }

                if (!War)
                {
                    var call__32 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.ConcealAreaRStr);
                    var call__33 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.AWarRelationIsRequiredForXRStr);
                    var call__34 = Aow.Rtl.Rtl.Format(call__33, [call__32]);
                    A2 = call__34;
                    Result = false;
                }
            }

            return Result;
        }

        // GlobalSpells.pas:388
        public override bool Activate(Aow.Units.AoWE.TAbstractUnit A1)
        {
            bool Result = default;
            Aow.Rtl.AnsiString Reason = default;
            Aow.Units.AoWE.TSpellTE Event = default;
            base.Activate(A1);
            var call__35 = CanActivate(A1, ref Reason);
            bool bool__36 = call__35;
            if (bool__36)
            {
                var call__37 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(A1.FPlayer);
                var call__38 = call__37.GetBusy();
                bool__36 = !call__38;
            }

            if (bool__36)
            {
                Event = Aow.Units.AoWE.TSpellTE.Create(null);
                Event.Initialize(FSpellID, unchecked((byte)A1.FPlayer), A1.FUnitID, FBaseManaCost);
                var call__39 = Aow.Units.AoWE.AoWE.AoWHSMap.FTokenManager.GetTokenControl(A1.FPlayer);
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

        // GlobalSpells.pas:49
        public static new Aow.Units.GlobalSpells.TConcealArea Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.GlobalSpells.TConcealArea o = new Aow.Units.GlobalSpells.TConcealArea();
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

    // { TBirdsViewHS (instance size 24, VMT 557E925C): $0C/$0D/$0E
//     X/Y/layer, $0F padding, $10 unexplored hexagons, $14 animation frame. }
    public partial class TBirdsViewHS : Aow.Units.HSEngine.TFixedVisibleHS
    {
        public sbyte FX;
        public sbyte FY;
        public sbyte FLayer;
        public Aow.Units.FixedArray_byte_1 F_BirdsViewHS_0F;
        public Aow.Units.Engine.TIntegerList FHexagons;
        public int FFrame;
        // GlobalSpells.pas:412
        public override void Destroy()
        {
            FHexagons.Free();
            base.Destroy();
        }

        // GlobalSpells.pas:406
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            var call__40 = Aow.Units.Engine.TIntegerList.Create();
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
        public override void Show(int XhpSurf, int YhpSurf, in Aow.Units.GFXE.TSurfaceDesc SurfaceDesc)
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
            if (Aow.Units.AoWE.AoWE.AoWHSMap.ViewL == FLayer)
            {
                FFrame += 1;
                Intensity = 100 - (FFrame * 2);
                Aow.Units.HSEngine.HSEngine.HXtoHP(FX, FY, ref Xhp, ref Yhp);
                Xhp = (Xhp - XhpSurf) - 8;
                Yhp -= YhpSurf;
                {
                    I = 0;
                    int end__41 = FHexagons.Count - 1;
                    for (; I <= end__41; I += 1)
                    {
                        var call__42 = FHexagons[I];
                        Aow.Units.HSEngine.HSEngine.HNtoHP(call__42, ref HX, ref HY);
                        HX += Xhp;
                        HY += Yhp;
                        var call__43 = Aow.Units.AoWE.AoWE.AoWHSSet.FGeneralImages[5];
                        call__43.ShowBlended(HX, HY, SurfaceDesc, Aow.Units.GFXE.GFXE.AlphaBlendTable.LookupTables[Intensity]);
                    }
                }

                Aow.Rtl.Rtl.RandSeed = Aow.Units.AoWE.AoWE.AoWHSMap.FInitialRandomSeed;
                {
                    I = 0;
                    byte end__44 = 200;
                    for (; I <= end__44; I += 1)
                    {
                        Radius = Aow.Rtl.Rtl.Random(448);
                        Direction = Aow.Rtl.Rtl.Random(360);
                        Delay = Aow.Rtl.Rtl.Random(30);
                        if ((FFrame - Delay) >= 0)
                        {
                            Aow.Units.AoWE.AoWE.DegreesRadToCo(Direction, Radius, ref X, ref Y);
                            var call__45 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(57);
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
            Aow.Units.HSEngine.THNWalkData Walk = default;
            Aow.Units.HSEngine.TMapLevel Level = default;
            FX = unchecked((sbyte)X);
            FY = unchecked((sbyte)Y);
            FLayer = unchecked((sbyte)Layer);
            FFrame = 0;
            var call__46 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(2);
            call__46.FCastSounds.Play(0);
            Aow.Units.HSEngine.HSEngine.InitHNWalk(ref Walk, X, Y, 0);
            var call__47 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.MapLevels[Layer];
            Level = call__47;
            while (Walk.Radius <= Radius)
            {
                if ((((Walk.X >= 0) && (Walk.Y >= 0)) && (Walk.X < Level.Width)) && (Walk.Y < Level.Height))
                {
                    var call__48 = Level.GetField(Walk.X, Walk.Y);
                    var call__49 = ((Aow.Units.AoWE.TAoWMapField)call__48).GetExplored(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
                    if (!call__49)
                    {
                        FHexagons.Add(Walk.HN);
                    }
                }

                Aow.Units.HSEngine.HSEngine.WalkToNextHN(ref Walk);
            }

            Aow.Units.AoWE.AoWE.AoWHSMap.DynamicFixedVisibleHS.AddChild(this);
        }

        // GlobalSpells.pas:444
        public void StopAnimation()
        {
            Aow.Units.AoWE.AoWE.AoWHSMap.DynamicFixedVisibleHS.RemoveChild(this);
            Aow.Units.AoWE.AoWE.AoWHSMap.DynamicVisibleHS.RemoveChild(this);
            Destroy();
        }

        // GlobalSpells.pas:67
        public static new Aow.Units.GlobalSpells.TBirdsViewHS Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.GlobalSpells.TBirdsViewHS o = new Aow.Units.GlobalSpells.TBirdsViewHS();
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

    // { draft }
    // { TBirdsView (instance size 52, VMT 557E93E0): inherited $08 name, $10 spell ID,
//     $14 mana, $20 sphere, $21 level, $22 spell type. }
    public partial class TBirdsView : Aow.Units.AoWE.TSpell
    {
        // GlobalSpells.pas:489
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            var call__50 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.BirdsViewRStr);
            FSpellName = call__50;
            FSpellID = 2;
            FSpellType = Aow.Units.AoWE.TSpellType.stGlobal;
            FBaseManaCost = 5;
            FSpellLevel = 1;
            FMagicSphere = Aow.Units.AoWE.TMagicSphere.msAir;
        }

        // GlobalSpells.pas:505
        public override bool ExecuteTE(Aow.Units.AoWE.TSpellTE A1, Aow.Units.AoWE.TAbstractUnit A2, int A3)
        {
            bool Result = default;
            sbyte X = default;
            sbyte Y = default;
            sbyte Layer = default;
            base.ExecuteTE(A1, A2, A3);
            CastingDone(A2, A1.FEventMana);
            A2.GetLocation(ref X, ref Y, ref Layer);
            if (Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer == A2.FPlayer)
            {
                var birdsViewHS = Aow.Units.GlobalSpells.TBirdsViewHS.Create(null);
                birdsViewHS.StartAnimation(unchecked((sbyte)X), unchecked((sbyte)Y), unchecked((sbyte)Layer), 14);
            }

            Aow.Units.AoWE.AoWE.AoWHSMap.ExploreArea(X, Y, Layer, unchecked((sbyte)A2.FPlayer), 14);
            Result = false;
            return Result;
        }

        // GlobalSpells.pas:500
        public override bool GetEnabled()
        {
            bool Result = default;
            Result = Aow.Units.AoWE.AoWE.AoWHSMap.FExplorationEnabled;
            return Result;
        }

        // GlobalSpells.pas:518
        public override bool CanActivate(Aow.Units.AoWE.TAbstractUnit A1, ref Aow.Rtl.AnsiString A2)
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
                    A2 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.CannotCastSpellInUndergoundRStr);
                    Result = false;
                }
            }

            return Result;
        }

        // GlobalSpells.pas:534
        public override bool Activate(Aow.Units.AoWE.TAbstractUnit A1)
        {
            bool Result = default;
            Aow.Rtl.AnsiString Reason = default;
            Aow.Units.AoWE.TSpellTE Event = default;
            base.Activate(A1);
            var call__52 = CanActivate(A1, ref Reason);
            bool bool__53 = call__52;
            if (bool__53)
            {
                var call__54 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(A1.FPlayer);
                var call__55 = call__54.GetBusy();
                bool__53 = !call__55;
            }

            if (bool__53)
            {
                Event = Aow.Units.AoWE.TSpellTE.Create(null);
                Event.Initialize(FSpellID, unchecked((byte)A1.FPlayer), A1.FUnitID, FBaseManaCost);
                var call__56 = Aow.Units.AoWE.AoWE.AoWHSMap.FTokenManager.GetTokenControl(A1.FPlayer);
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

        // GlobalSpells.pas:78
        public static new Aow.Units.GlobalSpells.TBirdsView Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.GlobalSpells.TBirdsView o = new Aow.Units.GlobalSpells.TBirdsView();
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

    // { TWarpPartyTE (instance size 40, VMT 557E94F4): $20 army ID, $24 packed destination. }
    public partial class TWarpPartyTE : Aow.Units.AoWE.TSpellTE
    {
        public Aow.Units.AoWE.TArmyID FArmyID;
        // { $20 }
        public Aow.Units.HSEngine.TXYL FWarpLocation;
        // GlobalSpells.pas:558
        // { $24 }
        public override void Destroy()
        {
            FArmyID.Free();
            base.Destroy();
        }

        // GlobalSpells.pas:569
        public override void ReadWrite(Aow.Units.Engine.TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwInteger(ref FWarpLocation.XYL, 30);
            Stream.rwCreateEObject(ref FArmyID, null, 31);
        }

        // GlobalSpells.pas:552
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            FArmyID = null;
        }

        // GlobalSpells.pas:576
        public override bool Validate(Aow.Units.AoWE.THero A1)
        {
            bool Result = default;
            byte Selection = default;
            Aow.Units.AoWE.TArmyHS Army = default;
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
                var call__60 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
                var call__61 = ((Aow.Units.GlobalSpells.TWarpParty)call__60).ValidTarget(Army, unchecked((sbyte)FWarpLocation.X), unchecked((sbyte)FWarpLocation.Y), unchecked((sbyte)FWarpLocation.L));
                Result = call__61;
            }

            return Result;
        }

        // GlobalSpells.pas:590
        public override bool Process(Aow.Units.AoWE.TAbstractUnit A1, int A2)
        {
            bool Result = default;
            bool ShowSource = default;
            bool ShowTarget = default;
            byte Selection = default;
            sbyte X = default;
            sbyte Y = default;
            sbyte Layer = default;
            Aow.Units.AoWE.TArmyHS Army = default;
            Aow.Units.HSEngine.TMapField Field = default;
            Aow.Units.Animation.TImageSequenceAnimation Animation = default;
            base.Process(A1, A2);
            A1.GetLocation(ref X, ref Y, ref Layer);
            FArmyID.RetrieveArmy(ref Army, ref Selection);
            var call__62 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.GetFieldXYL(FWarpLocation.XYL);
            Field = call__62;
            Army.MoveTo(Field, 0);
            bool bool__63 = (Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer == A1.FPlayer);
            if (!bool__63)
            {
                var call__64 = Aow.Units.AoWE.AoWE.AoWHSMap.VisibleForPlayer(X, Y, Layer, unchecked((sbyte)Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer));
                bool__63 = call__64;
            }

            ShowSource = bool__63;
            bool bool__65 = (Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer == A1.FPlayer);
            if (!bool__65)
            {
                var call__66 = Aow.Units.AoWE.AoWE.AoWHSMap.VisibleForPlayer(Field.Xhx, Field.Yhx, Field.Lhx, unchecked((sbyte)Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer));
                bool__65 = call__66;
            }

            ShowTarget = bool__65;
            bool bool__67 = ShowSource;
            if (bool__67)
            {
                var call__68 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
                var call__69 = call__68.FCastImages.ImageSequences[0];
                bool__67 = (call__69 != null);
            }

            if (bool__67)
            {
                Animation = Aow.Units.Animation.TImageSequenceAnimation.Create(null);
                var call__70 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
                var call__71 = call__70.FCastImages.ImageSequences[0];
                Animation.SetImageSequence(call__71);
                Aow.Units.AoWE.AoWE.AoWHSMap.PlaceHX(Animation, X, Y, Layer);
            }

            bool bool__72 = ShowTarget;
            if (bool__72)
            {
                var call__73 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
                var call__74 = call__73.FCastImages.ImageSequences[0];
                bool__72 = (call__74 != null);
            }

            if (bool__72)
            {
                Animation = Aow.Units.Animation.TImageSequenceAnimation.Create(null);
                var call__75 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
                var call__76 = call__75.FCastImages.ImageSequences[0];
                Animation.SetImageSequence(call__76);
                Aow.Units.AoWE.AoWE.AoWHSMap.PlaceHX(Animation, Field.Xhx, Field.Yhx, Field.Lhx);
            }

            if (A1.FPlayer == Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer)
            {
                Aow.Units.AoWE.AoWE.AoWHSMap.CenterViewTo(Field.Xhx, Field.Yhx, Field.Lhx);
            }
            else
            {
                Aow.Units.AoWE.AoWE.AoWHSMap.ValidateObservation(unchecked((sbyte)A1.FPlayer), unchecked((sbyte)Field.Xhx), unchecked((sbyte)Field.Yhx), unchecked((sbyte)Field.Lhx), 0);
            }

            bool bool__77 = (A1.FPlayer == Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
            if (!bool__77)
            {
                var call__78 = Aow.Units.AoWE.AoWE.AoWHSMap.Watching(unchecked((sbyte)X), unchecked((sbyte)Y), unchecked((sbyte)Layer), 0);
                bool__77 = call__78;
            }

            bool bool__79 = bool__77;
            if (!bool__79)
            {
                var call__80 = Aow.Units.AoWE.AoWE.AoWHSMap.Watching(unchecked((sbyte)Field.Xhx), unchecked((sbyte)Field.Yhx), unchecked((sbyte)Field.Lhx), 0);
                bool__79 = call__80;
            }

            if (bool__79)
            {
                var call__81 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
                call__81.FCastSounds.Play(0);
            }

            var call__82 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
            var call__83 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
            call__83.CastingDone(A1, call__82.FBaseManaCost);
            Result = false;
            return Result;
        }

        // GlobalSpells.pas:92
        public static new Aow.Units.GlobalSpells.TWarpPartyTE Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.GlobalSpells.TWarpPartyTE o = new Aow.Units.GlobalSpells.TWarpPartyTE();
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

        public new partial class Meta : Aow.Units.AoWE.TSpellTE.Meta
        {
            // GlobalSpells.pas:564
            public override int ClassID()
            {
                int Result = default;
                Result = 135948;
                return Result;
            }

            // GlobalSpells.pas:92
            public new Aow.Units.GlobalSpells.TWarpPartyTE Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.GlobalSpells.TWarpPartyTE o = (Aow.Units.GlobalSpells.TWarpPartyTE)NewInstance();
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
                where T : Aow.Units.GlobalSpells.TWarpPartyTE => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { TWarpParty (instance size 52, VMT 557E95CC): inherited $08 name, $10 ID,
//     $14 mana, $20 sphere, $21 level, $22 spell type. }
    public partial class TWarpParty : Aow.Units.AoWE.TSpell
    {
        // GlobalSpells.pas:632
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            var call__84 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.WarpPartyRStr);
            FSpellName = call__84;
            FSpellID = 34;
            FSpellType = Aow.Units.AoWE.TSpellType.stGlobal;
            FBaseManaCost = 1;
            FSpellLevel = 2;
            FMagicSphere = Aow.Units.AoWE.TMagicSphere.msFire;
        }

        // GlobalSpells.pas:719
        public override bool Activate(Aow.Units.AoWE.TAbstractUnit A1)
        {
            bool Result = default;
            Aow.Rtl.AnsiString Reason = default;
            Aow.Units.GlobalSpells.TWarpPartyTE Event = default;
            base.Activate(A1);
            var call__85 = CanActivate(A1, ref Reason);
            bool bool__86 = call__85;
            if (bool__86)
            {
                var call__87 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(A1.FPlayer);
                var call__88 = call__87.GetBusy();
                bool__86 = !call__88;
            }

            if (bool__86)
            {
                Event = Aow.Units.GlobalSpells.TWarpPartyTE.Create(null);
                Event.Initialize(FSpellID, unchecked((byte)A1.FPlayer), A1.FUnitID, FBaseManaCost);
                var call__89 = ((Aow.Units.AoWE.TArmy)A1.Owner).CreateID(255);
                Event.FArmyID = call__89;
                var call__90 = ((Aow.Units.AoWE.TArmy)A1.Owner).GetArmyHS();
                var call__91 = CalculateWarpLocation(call__90);
                Event.FWarpLocation.XYL = call__91;
                Aow.Units.AoWE.AoWE.AoWHSMap.FSelectedArmy.Unselect();
                if (Event.FWarpLocation.XYL != 16777215)
                {
                    var call__92 = Aow.Units.AoWE.AoWE.AoWHSMap.FTokenManager.GetTokenControl(A1.FPlayer);
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
        public bool ValidTarget(Aow.Units.AoWE.TArmyHS Army, sbyte X, sbyte Y, sbyte Layer)
        {
            bool Result = default;
            Aow.Units.AoWE.TAoWMapField Field = default;
            Aow.Units.HSEngine.TMapFieldMsg Msg = default;
            var call__93 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.GetField(X, Y, Layer);
            Field = (Aow.Units.AoWE.TAoWMapField)call__93;
            var call__94 = Field.GetArmyHS();
            bool bool__95 = (call__94 == null);
            if (bool__95)
            {
                var call__96 = Field.GetPlayer();
                bool__95 = (call__96 == -1);
            }

            Result = bool__95;
            if (Result)
            {
                Msg.MsgID = 4384;
                Msg.Param[0] = Army;
                Msg.Result = 0;
                Field.SendMapFieldMsg(ref Msg);
                bool bool__97 = (Msg.Result.Value == 0);
                if (bool__97)
                {
                    var call__98 = Army.FArmy.CanPlace(unchecked((sbyte)X), unchecked((sbyte)Y), unchecked((sbyte)Layer));
                    bool__97 = call__98;
                }

                Result = bool__97;
            }

            return Result;
        }

        // GlobalSpells.pas:660
        public int CalculateWarpLocation(Aow.Units.AoWE.TArmyHS Army)
        {
            int Result = default;
            Aow.Rtl.TList Candidates = default;
            Aow.Units.FixedArray_Aow_Units_FixedArray_sbyte_16_16 Table = default;
            Aow.Units.HSEngine.TMapFieldMsg Msg = default;
            Aow.Units.HSEngine.THNWalkData Walk = default;
            Aow.Units.HSEngine.TMapLevel Level = default;
            Aow.Units.AoWE.TAoWMapField Field = default;
            int L = default;
            int I = default;
            bool Found = default;
            Candidates = new Aow.Rtl.TList();
            Army.FArmy.CreateMovePointTable(255, Aow.Rtl.Bytes.Of(ref Table));
            var call__99 = Army.Yhx;
            var call__100 = Army.Xhx;
            Aow.Units.HSEngine.HSEngine.InitHNWalk(ref Walk, call__100, call__99, 10);
            var call__101 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.MapLevels[Army.Lhx];
            Level = call__101;
            while (Walk.Radius < 20)
            {
                if ((((Walk.X >= 0) && (Walk.Y >= 0)) && (Walk.X < Level.Width)) && (Walk.Y < Level.Height))
                {
                    var call__102 = Level.GetField(Walk.X, Walk.Y);
                    Field = (Aow.Units.AoWE.TAoWMapField)call__102;
                    if (Table[Field.TerrainType][Field.OverlayType - -1] != -1)
                    {
                        Candidates.Add(Field);
                    }
                }

                Aow.Units.HSEngine.HSEngine.WalkToNextHN(ref Walk);
            }

            {
                L = 0;
                int end__103 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.LevelCount - 1;
                for (; L <= end__103; L += 1)
                {
                    if (System.Math.Abs(L - Army.Lhx) == 1)
                    {
                        var call__104 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.MapLevels[L];
                        Level = call__104;
                        var call__105 = Army.Yhx;
                        var call__106 = Army.Xhx;
                        Aow.Units.HSEngine.HSEngine.InitHNWalk(ref Walk, call__106, call__105, 0);
                        while (Walk.Radius < 10)
                        {
                            if ((((Walk.X >= 0) && (Walk.Y >= 0)) && (Walk.X < Level.Width)) && (Walk.Y < Level.Height))
                            {
                                var call__107 = Level.GetField(Walk.X, Walk.Y);
                                Field = (Aow.Units.AoWE.TAoWMapField)call__107;
                                if (Table[Field.TerrainType][Field.OverlayType - -1] != -1)
                                {
                                    Candidates.Add(Field);
                                }
                            }

                            Aow.Units.HSEngine.HSEngine.WalkToNextHN(ref Walk);
                        }
                    }
                }
            }

            Found = false;
            Result = 16777215;
            while ((Candidates.Count != 0) && !Found)
            {
                var call__108 = Aow.Rtl.Rtl.Random(Candidates.Count);
                I = call__108;
                var call__109 = Candidates[I];
                Field = (Aow.Units.AoWE.TAoWMapField)call__109;
                var call__110 = Field.GetArmyHS();
                bool bool__111 = (call__110 == null);
                if (bool__111)
                {
                    var call__112 = Field.GetPlayer();
                    bool__111 = (call__112 == -1);
                }

                if (bool__111)
                {
                    Msg.MsgID = 4384;
                    Msg.Param[0] = Army;
                    Msg.Result = 0;
                    Field.SendMapFieldMsg(ref Msg);
                    bool bool__113 = (Msg.Result.Value == 0);
                    if (bool__113)
                    {
                        var call__114 = Army.FArmy.CanPlace(unchecked((sbyte)Field.Xhx), unchecked((sbyte)Field.Yhx), unchecked((sbyte)Field.Lhx));
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

        // GlobalSpells.pas:102
        public static new Aow.Units.GlobalSpells.TWarpParty Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.GlobalSpells.TWarpParty o = new Aow.Units.GlobalSpells.TWarpParty();
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

    // { TResurrectHero (instance size 52, VMT 557E96E0): inherited $08 name, $10 spell ID, $14 mana, $20 sphere, $21 level, $22 type, $28 sounds, $2C images. }
    public partial class TResurrectHero : Aow.Units.AoWE.TSpell
    {
        // GlobalSpells.pas:980
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            var call__115 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.ResurrectHeroRStr);
            FSpellName = call__115;
            FSpellID = 66;
            FSpellType = Aow.Units.AoWE.TSpellType.stGlobal;
            FBaseManaCost = 0;
            FSpellLevel = 2;
            FMagicSphere = Aow.Units.AoWE.TMagicSphere.msCosmos;
        }

        // GlobalSpells.pas:991
        public override bool ExecuteTE(Aow.Units.AoWE.TSpellTE A1, Aow.Units.AoWE.TAbstractUnit A2, int A3)
        {
            bool Result = default;
            sbyte X = default;
            sbyte Y = default;
            sbyte Layer = default;
            Aow.Units.Animation.TTwinkleAnimation Animation = default;
            Aow.Units.AoWE.THeroList Heroes = default;
            int I = default;
            Aow.Units.AoWE.THero Hero = default;
            Aow.Units.AoWE.TArmyHS Army = default;
            Aow.Units.EventLog.TLocationTextEventLog Log = default;
            Aow.Units.AoWE.THeroEmergeEventLog EmergeLog = default;
            Aow.Units.AoWE.TArmyInfoEventLog InfoLog = default;
            if (A3 == 0)
            {
                A2.GetLocation(ref X, ref Y, ref Layer);
                Aow.Units.AoWE.AoWE.AoWHSMap.ValidateObservation(unchecked((sbyte)A2.FPlayer), unchecked((sbyte)X), unchecked((sbyte)Y), unchecked((sbyte)Layer), 0);
                Animation = Aow.Units.Animation.TTwinkleAnimation.Create(null);
                var call__116 = FCastImages.GetImageSequence(0);
                Animation.Setup(call__116, TSet<Aow.Units.Animation.TTwinkleModeElement>.Of(Aow.Units.Animation.TTwinkleModeElement.tmRotate), 50, 64, 50, 30);
                Aow.Units.AoWE.AoWE.AoWHSMap.PlaceHX(Animation, X, Y, Layer);
                bool bool__117 = (A2.FPlayer == Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
                if (!bool__117)
                {
                    var call__118 = Aow.Units.AoWE.AoWE.AoWHSMap.Watching(unchecked((sbyte)X), unchecked((sbyte)Y), unchecked((sbyte)Layer), 0);
                    bool__117 = call__118;
                }

                if (bool__117)
                {
                    FCastSounds.Play(0);
                }
            }

            if (A3 == 20)
            {
                Heroes = Aow.Units.AoWE.THeroList.Create(null);
                Aow.Units.AoWE.AoWE.AoWHSMap.FHeroControl.ListDeadHeroes(Heroes);
                {
                    I = Heroes.Count - 1;
                    byte end__119 = 0;
                    for (; I >= end__119; I -= 1)
                    {
                        var call__120 = Heroes.GetHero(I);
                        bool bool__121 = (call__120 is Aow.Units.AoWE.TLeader);
                        if (!bool__121)
                        {
                            var call__122 = Heroes.GetHero(I);
                            var call__123 = call__122.GetLevel();
                            var call__124 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(A2.FPlayer);
                            bool__121 = (call__123 > (call__124.FBaseHeroJoinLevel + 2));
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
                    var call__128 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(call__127);
                    var call__129 = Heroes.GetHero(call__128);
                    Hero = call__129;
                    Hero.Resurrect();
                    Army = Hero.Place(X, Y, Layer, 0, TSet<Aow.Units.AoWE.TUnitPlaceModeElement>.Of(Aow.Units.AoWE.TUnitPlaceModeElement.pmScanArea, Aow.Units.AoWE.TUnitPlaceModeElement.pmCreateNewArmy));
                    Log = Aow.Units.EventLog.TLocationTextEventLog.Create(null);
                    Log.FPlayer = unchecked((sbyte)A2.FPlayer);
                    var call__130 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.XResurrectedRStr);
                    var call__131 = Aow.Rtl.Rtl.Format(call__130, [Hero.FHeroName]);
                    Log.FText = call__131;
                    var call__132 = Army.GetLhx();
                    var call__133 = Army.GetYhx();
                    var call__134 = Army.GetXhx();
                    Log.SetLocation(unchecked((sbyte)call__134), unchecked((sbyte)call__133), unchecked((sbyte)call__132));
                    var call__135 = Army.GetYhx();
                    var call__136 = Army.GetLhx();
                    var call__137 = Army.GetXhx();
                    Aow.Units.EventLog.EventLog.DistributeLocationEventLog(Log, unchecked((sbyte)A2.FPlayer), unchecked((sbyte)call__137), unchecked((sbyte)call__135), unchecked((sbyte)call__136), false);
                    Log.Release();
                    var call__138 = Hero.OfferToJoin(unchecked((sbyte)A2.FPlayer));
                    if (call__138)
                    {
                        EmergeLog = Aow.Units.AoWE.THeroEmergeEventLog.Create(null);
                        EmergeLog.Setup(Hero, unchecked((sbyte)A2.FPlayer), Aow.Units.AoWE.THeroJoinMode.hjmResurrected);
                        EmergeLog.Execute(true, Aow.Units.EventLog.TExecuteTrigger.etEvent, null);
                        EmergeLog.Release();
                    }
                    else
                    {
                        InfoLog = Aow.Units.AoWE.TArmyInfoEventLog.Create(null);
                        var call__139 = Army.FArmy.IndexOf(Hero);
                        InfoLog.FArmyView.Setup(Army.FArmy, unchecked((byte)(1 << call__139)));
                        InfoLog.FSelection = 255;
                        InfoLog.FPlayer = unchecked((sbyte)A2.FPlayer);
                        var call__140 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.NoJoinResurrectionRStr);
                        InfoLog.FMessages.Text = call__140;
                        InfoLog.FTriggers = TSet<Aow.Units.EventLog.TExecuteTrigger>.Of(Aow.Units.EventLog.TExecuteTrigger.etEvent);
                        InfoLog.Execute(true, Aow.Units.EventLog.TExecuteTrigger.etEvent, null);
                        InfoLog.Release();
                    }
                }
                else
                {
                    var call__141 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.NoHeroAnswersYourCallRStr);
                    Aow.Units.AoWE.AoWE.AoWHSMap.ShowMessage(call__141);
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
        public override bool Activate(Aow.Units.AoWE.TAbstractUnit A1)
        {
            bool Result = default;
            Aow.Rtl.AnsiString Reason = default;
            Aow.Units.AoWE.TSpellTE Event = default;
            base.Activate(A1);
            var call__142 = CanActivate(A1, ref Reason);
            bool bool__143 = call__142;
            if (bool__143)
            {
                var call__144 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(A1.FPlayer);
                var call__145 = call__144.GetBusy();
                bool__143 = !call__145;
            }

            if (bool__143)
            {
                Event = Aow.Units.AoWE.TSpellTE.Create(null);
                Event.Initialize(FSpellID, unchecked((byte)A1.FPlayer), A1.FUnitID, FBaseManaCost);
                var call__146 = Aow.Units.AoWE.AoWE.AoWHSMap.FTokenManager.GetTokenControl(A1.FPlayer);
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

        // GlobalSpells.pas:111
        public static new Aow.Units.GlobalSpells.TResurrectHero Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.GlobalSpells.TResurrectHero o = new Aow.Units.GlobalSpells.TResurrectHero();
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

    // { TAnimateHero (instance size 52, VMT 557E97FC): inherited $08 name, $10 spell ID, $14 mana, $20 sphere, $21 level, $22 type, $28 sounds, $2C images. }
    public partial class TAnimateHero : Aow.Units.AoWE.TSpell
    {
        // GlobalSpells.pas:1080
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            var call__147 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.AnimateHeroRStr);
            FSpellName = call__147;
            FSpellID = 67;
            FSpellType = Aow.Units.AoWE.TSpellType.stGlobal;
            FBaseManaCost = 0;
            FSpellLevel = 2;
            FMagicSphere = Aow.Units.AoWE.TMagicSphere.msCosmos;
        }

        // GlobalSpells.pas:1091
        public override bool ExecuteTE(Aow.Units.AoWE.TSpellTE A1, Aow.Units.AoWE.TAbstractUnit A2, int A3)
        {
            bool Result = default;
            sbyte X = default;
            sbyte Y = default;
            sbyte Layer = default;
            Aow.Units.Animation.TTwinkleAnimation Animation = default;
            Aow.Units.AoWE.THeroList Heroes = default;
            int I = default;
            Aow.Units.AoWE.THero Hero = default;
            Aow.Units.AoWE.TArmyHS Army = default;
            Aow.Units.EventLog.TLocationTextEventLog Log = default;
            Aow.Units.AoWE.THeroEmergeEventLog EmergeLog = default;
            Aow.Units.AoWE.TArmyInfoEventLog InfoLog = default;
            if (A3 == 0)
            {
                A2.GetLocation(ref X, ref Y, ref Layer);
                Aow.Units.AoWE.AoWE.AoWHSMap.ValidateObservation(unchecked((sbyte)A2.FPlayer), unchecked((sbyte)X), unchecked((sbyte)Y), unchecked((sbyte)Layer), 0);
                Animation = Aow.Units.Animation.TTwinkleAnimation.Create(null);
                var call__148 = FCastImages.GetImageSequence(0);
                Animation.Setup(call__148, TSet<Aow.Units.Animation.TTwinkleModeElement>.Of(Aow.Units.Animation.TTwinkleModeElement.tmRotate), 50, 64, 50, 30);
                Aow.Units.AoWE.AoWE.AoWHSMap.PlaceHX(Animation, X, Y, Layer);
                bool bool__149 = (A2.FPlayer == Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
                if (!bool__149)
                {
                    var call__150 = Aow.Units.AoWE.AoWE.AoWHSMap.Watching(unchecked((sbyte)X), unchecked((sbyte)Y), unchecked((sbyte)Layer), 0);
                    bool__149 = call__150;
                }

                if (bool__149)
                {
                    FCastSounds.Play(0);
                }
            }

            if (A3 == 20)
            {
                Heroes = Aow.Units.AoWE.THeroList.Create(null);
                Aow.Units.AoWE.AoWE.AoWHSMap.FHeroControl.ListDeadHeroes(Heroes);
                {
                    I = Heroes.Count - 1;
                    byte end__151 = 0;
                    for (; I >= end__151; I -= 1)
                    {
                        var call__152 = Heroes.GetHero(I);
                        bool bool__153 = (call__152 is Aow.Units.AoWE.TLeader);
                        if (!bool__153)
                        {
                            var call__154 = Heroes.GetHero(I);
                            var call__155 = call__154.GetLevel();
                            var call__156 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(A2.FPlayer);
                            bool__153 = (call__155 > (call__156.FBaseHeroJoinLevel + 2));
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
                    var call__160 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(call__159);
                    var call__161 = Heroes.GetHero(call__160);
                    Hero = call__161;
                    Hero.Animate();
                    Army = Hero.Place(X, Y, Layer, 0, TSet<Aow.Units.AoWE.TUnitPlaceModeElement>.Of(Aow.Units.AoWE.TUnitPlaceModeElement.pmScanArea, Aow.Units.AoWE.TUnitPlaceModeElement.pmCreateNewArmy));
                    Log = Aow.Units.EventLog.TLocationTextEventLog.Create(null);
                    Log.FPlayer = unchecked((sbyte)A2.FPlayer);
                    var call__162 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.XAnimatedRStr);
                    var call__163 = Aow.Rtl.Rtl.Format(call__162, [Hero.FHeroName]);
                    Log.FText = call__163;
                    var call__164 = Army.GetLhx();
                    var call__165 = Army.GetYhx();
                    var call__166 = Army.GetXhx();
                    Log.SetLocation(unchecked((sbyte)call__166), unchecked((sbyte)call__165), unchecked((sbyte)call__164));
                    var call__167 = Army.GetYhx();
                    var call__168 = Army.GetLhx();
                    var call__169 = Army.GetXhx();
                    Aow.Units.EventLog.EventLog.DistributeLocationEventLog(Log, unchecked((sbyte)A2.FPlayer), unchecked((sbyte)call__169), unchecked((sbyte)call__167), unchecked((sbyte)call__168), false);
                    Log.Release();
                    var call__170 = Hero.OfferToJoin(unchecked((sbyte)A2.FPlayer));
                    if (call__170)
                    {
                        EmergeLog = Aow.Units.AoWE.THeroEmergeEventLog.Create(null);
                        EmergeLog.Setup(Hero, unchecked((sbyte)A2.FPlayer), Aow.Units.AoWE.THeroJoinMode.hjmResurrected);
                        EmergeLog.Execute(true, Aow.Units.EventLog.TExecuteTrigger.etEvent, null);
                        EmergeLog.Release();
                    }
                    else
                    {
                        InfoLog = Aow.Units.AoWE.TArmyInfoEventLog.Create(null);
                        var call__171 = Army.FArmy.IndexOf(Hero);
                        InfoLog.FArmyView.Setup(Army.FArmy, unchecked((byte)(1 << call__171)));
                        InfoLog.FSelection = 255;
                        InfoLog.FPlayer = unchecked((sbyte)A2.FPlayer);
                        var call__172 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.NoJoinAnimateRStr);
                        InfoLog.FMessages.Text = call__172;
                        InfoLog.FTriggers = TSet<Aow.Units.EventLog.TExecuteTrigger>.Of(Aow.Units.EventLog.TExecuteTrigger.etEvent);
                        InfoLog.Execute(true, Aow.Units.EventLog.TExecuteTrigger.etEvent, null);
                        InfoLog.Release();
                    }
                }
                else
                {
                    if (A2.FPlayer == Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer)
                    {
                        var call__173 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.NoHeroAnswersYourCallRStr);
                        Aow.Units.AoWE.AoWE.AoWHSMap.ShowMessage(call__173);
                        var call__174 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellID);
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
        public override bool Activate(Aow.Units.AoWE.TAbstractUnit A1)
        {
            bool Result = default;
            Aow.Rtl.AnsiString Reason = default;
            Aow.Units.AoWE.TSpellTE Event = default;
            base.Activate(A1);
            var call__175 = CanActivate(A1, ref Reason);
            bool bool__176 = call__175;
            if (bool__176)
            {
                var call__177 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(A1.FPlayer);
                var call__178 = call__177.GetBusy();
                bool__176 = !call__178;
            }

            if (bool__176)
            {
                Event = Aow.Units.AoWE.TSpellTE.Create(null);
                Event.Initialize(FSpellID, unchecked((byte)A1.FPlayer), A1.FUnitID, FBaseManaCost);
                var call__179 = Aow.Units.AoWE.AoWE.AoWHSMap.FTokenManager.GetTokenControl(A1.FPlayer);
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

        // GlobalSpells.pas:119
        public static new Aow.Units.GlobalSpells.TAnimateHero Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.GlobalSpells.TAnimateHero o = new Aow.Units.GlobalSpells.TAnimateHero();
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

    // { TCallHero (instance size 52, VMT 557E9914): inherited $08 name, $10 spell ID, $14 mana, $20 sphere, $21 level, $22 type, $28 sounds, $2C images. }
    public partial class TCallHero : Aow.Units.AoWE.TSpell
    {
        // GlobalSpells.pas:1184
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            var call__180 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.CallHeroRStr);
            FSpellName = call__180;
            FSpellID = 69;
            FSpellType = Aow.Units.AoWE.TSpellType.stGlobal;
            FBaseManaCost = 0;
            FSpellLevel = 2;
            FMagicSphere = Aow.Units.AoWE.TMagicSphere.msCosmos;
        }

        // GlobalSpells.pas:1250
        public override bool ExecuteTE(Aow.Units.AoWE.TSpellTE A1, Aow.Units.AoWE.TAbstractUnit A2, int A3)
        {
            bool Result = default;
            bool ShowSource = default;
            bool ShowTarget = default;
            sbyte X = default;
            sbyte Y = default;
            sbyte Layer = default;
            Aow.Units.AoWE.THero Hero = default;
            Aow.Units.AoWE.TArmyHS Army = default;
            Aow.Units.Animation.TImageSequenceAnimation Animation = default;
            base.ExecuteTE(A1, A2, A3);
            A2.GetLocation(ref X, ref Y, ref Layer);
            Hero = FindCallHero(A2);
            if (Hero != null)
            {
                if (Hero.Owner == null)
                {
                    var call__181 = Hero.GetHits();
                    Hero.SetHitPoints(unchecked((sbyte)call__181));
                }

                Army = Hero.Place(X, Y, Layer, 0, TSet<Aow.Units.AoWE.TUnitPlaceModeElement>.Of(Aow.Units.AoWE.TUnitPlaceModeElement.pmScanArea, Aow.Units.AoWE.TUnitPlaceModeElement.pmCreateNewArmy));
                bool bool__182 = (Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer == A2.FPlayer);
                if (!bool__182)
                {
                    var call__183 = Aow.Units.AoWE.AoWE.AoWHSMap.VisibleForPlayer(X, Y, Layer, unchecked((sbyte)Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer));
                    bool__182 = call__183;
                }

                ShowSource = bool__182;
                bool bool__184 = (Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer == A2.FPlayer);
                if (!bool__184)
                {
                    var call__185 = Army.GetLhx();
                    var call__186 = Army.GetYhx();
                    var call__187 = Army.GetXhx();
                    var call__188 = Aow.Units.AoWE.AoWE.AoWHSMap.VisibleForPlayer(call__187, call__186, call__185, unchecked((sbyte)Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer));
                    bool__184 = call__188;
                }

                ShowTarget = bool__184;
                if (ShowSource)
                {
                    var call__189 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellID);
                    var call__190 = call__189.FCastImages.GetImageSequence(0);
                    if (call__190 != null)
                    {
                        Animation = Aow.Units.Animation.TImageSequenceAnimation.Create(null);
                        var call__191 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellID);
                        var call__192 = call__191.FCastImages.GetImageSequence(0);
                        Animation.SetImageSequence(call__192);
                        Aow.Units.AoWE.AoWE.AoWHSMap.PlaceHX(Animation, X, Y, Layer);
                    }
                }

                if (ShowTarget)
                {
                    var call__193 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellID);
                    var call__194 = call__193.FCastImages.GetImageSequence(0);
                    if (call__194 != null)
                    {
                        Animation = Aow.Units.Animation.TImageSequenceAnimation.Create(null);
                        var call__195 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellID);
                        var call__196 = call__195.FCastImages.GetImageSequence(0);
                        Animation.SetImageSequence(call__196);
                        var call__197 = Army.GetLhx();
                        var call__198 = Army.GetYhx();
                        var call__199 = Army.GetXhx();
                        Aow.Units.AoWE.AoWE.AoWHSMap.PlaceHX(Animation, call__199, call__198, call__197);
                    }
                }

                if (Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer == A2.FPlayer)
                {
                    var call__200 = Army.GetLhx();
                    var call__201 = Army.GetYhx();
                    var call__202 = Army.GetXhx();
                    Aow.Units.AoWE.AoWE.AoWHSMap.CenterViewTo(call__202, call__201, call__200);
                }
                else
                {
                    var call__203 = Army.GetLhx();
                    var call__204 = Army.GetYhx();
                    var call__205 = Army.GetXhx();
                    Aow.Units.AoWE.AoWE.AoWHSMap.ValidateObservation(unchecked((sbyte)A2.FPlayer), unchecked((sbyte)call__205), unchecked((sbyte)call__204), unchecked((sbyte)call__203), 0);
                }

                bool bool__206 = (Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer == A2.FPlayer);
                if (!bool__206)
                {
                    var call__207 = Aow.Units.AoWE.AoWE.AoWHSMap.Watching(unchecked((sbyte)X), unchecked((sbyte)Y), unchecked((sbyte)Layer), 0);
                    bool__206 = call__207;
                }

                bool bool__208 = bool__206;
                if (!bool__208)
                {
                    var call__209 = Army.GetLhx();
                    var call__210 = Army.GetYhx();
                    var call__211 = Army.GetXhx();
                    var call__212 = Aow.Units.AoWE.AoWE.AoWHSMap.Watching(unchecked((sbyte)call__211), unchecked((sbyte)call__210), unchecked((sbyte)call__209), 0);
                    bool__208 = call__212;
                }

                if (bool__208)
                {
                    var call__213 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellID);
                    call__213.FCastSounds.Play(0);
                }
            }
            else
            {
                if (Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer == A2.FPlayer)
                {
                    var call__214 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.NoHeroAnsweredYourCallRStr);
                    Aow.Units.AoWE.AoWE.AoWHSMap.ShowMessage(call__214);
                    var call__215 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellID);
                    call__215.FCastSounds.Play(0);
                }
            }

            var call__216 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellID);
            var call__217 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellID);
            call__217.CastingDone(A2, call__216.FBaseManaCost);
            Result = false;
            return Result;
        }

        // GlobalSpells.pas:1195
        public override bool Activate(Aow.Units.AoWE.TAbstractUnit A1)
        {
            bool Result = default;
            Aow.Rtl.AnsiString Reason = default;
            Aow.Units.AoWE.TSpellTE Event = default;
            var call__218 = CanActivate(A1, ref Reason);
            bool bool__219 = call__218;
            if (bool__219)
            {
                var call__220 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(A1.FPlayer);
                var call__221 = call__220.GetBusy();
                bool__219 = !call__221;
            }

            if (bool__219)
            {
                Event = Aow.Units.AoWE.TSpellTE.Create(null);
                Event.Initialize(FSpellID, unchecked((byte)A1.FPlayer), A1.FUnitID, FBaseManaCost);
                var call__222 = Aow.Units.AoWE.AoWE.AoWHSMap.FTokenManager.GetTokenControl(A1.FPlayer);
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
        public Aow.Units.AoWE.THero FindCallHero(Aow.Units.AoWE.TAbstractUnit Source)
        {
            Aow.Units.AoWE.THero Result = default;
            Aow.Units.AoWE.THero Hero = default;
            int I = default;
            Aow.Units.Engine.TDoubleItemList Heroes = default;
            int Index = default;
            Heroes = new Aow.Units.Engine.TDoubleItemList();
            {
                I = Aow.Units.AoWE.AoWE.AoWHSMap.FHeroControl.GetCount() - 1;
                byte end__223 = 0;
                for (; I >= end__223; I -= 1)
                {
                    var call__224 = Aow.Units.AoWE.AoWE.AoWHSMap.FHeroControl.GetHero(I);
                    Hero = call__224;
                    if (((Hero.FPlayer == 0) && (Hero.Owner is Aow.Units.AoWE.TArmy)) && (((Aow.Units.AoWE.TArmy)Hero.Owner).FX != -1))
                    {
                        var call__225 = Hero.OfferToJoin(unchecked((sbyte)Source.FPlayer));
                        if (call__225)
                        {
                            Heroes.AddAB(Aow.Rtl.Rtl.ObjectToInt(Hero), 10);
                        }
                        else
                        {
                            Heroes.AddAB(Aow.Rtl.Rtl.ObjectToInt(Hero), 1);
                        }
                    }
                }
            }

            if (Heroes.Count == 0)
            {
                {
                    I = Aow.Units.AoWE.AoWE.AoWHSMap.FHeroControl.GetCount() - 1;
                    byte end__226 = 0;
                    for (; I >= end__226; I -= 1)
                    {
                        var call__227 = Aow.Units.AoWE.AoWE.AoWHSMap.FHeroControl.GetHero(I);
                        Hero = call__227;
                        if ((!Hero.FUnitStatus.Contains(Aow.Units.AoWE.TUnitStatusElement.usKilled) && (Hero.FPlayer <= 0)) && (Hero.Owner == null))
                        {
                            var call__228 = Hero.OfferToJoin(unchecked((sbyte)Source.FPlayer));
                            if (call__228)
                            {
                                Heroes.AddAB(Aow.Rtl.Rtl.ObjectToInt(Hero), 10);
                            }
                            else
                            {
                                Heroes.AddAB(Aow.Rtl.Rtl.ObjectToInt(Hero), 1);
                            }
                        }
                    }
                }
            }

            if (Heroes.Count != 0)
            {
                var call__229 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
                Aow.Rtl.Rtl.RandSeed = call__229;
                Index = Heroes.RandomChanceItemB(0);
                var call__230 = Heroes[Index];
                Result = Aow.Rtl.Rtl.ReferenceFromId<Aow.Units.AoWE.THero>((int)call__230.A);
            }
            else
            {
                Result = null;
            }

            Heroes.Free();
            return Result;
        }

        // GlobalSpells.pas:127
        public static new Aow.Units.GlobalSpells.TCallHero Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.GlobalSpells.TCallHero o = new Aow.Units.GlobalSpells.TCallHero();
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

    // { TDisjunctionTE (instance size 40, VMT 557E9A24): $20 dispel chance, $24 enchantment ID. }
    public partial class TDisjunctionTE : Aow.Units.AoWE.TSpellTE
    {
        public int FDispelChance;
        // { $20 }
        public int FEnchantmentID;
        // GlobalSpells.pas:750
        // { $24 }
        public override void ReadWrite(Aow.Units.Engine.TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwInteger(ref FEnchantmentID, 20);
            Stream.rwInteger(ref FDispelChance, 21);
        }

        // GlobalSpells.pas:757
        public override bool Validate(Aow.Units.AoWE.THero A1)
        {
            bool Result = default;
            int Index = default;
            int Mana = default;
            int RequiredMana = default;
            Result = base.Validate(A1);
            var call__231 = Aow.Units.AoWE.AoWE.AoWHSMap.FGlobalMagicControl.IndexOfID(FEnchantmentID);
            Index = call__231;
            Result = Result && (Index != -1);
            if (Result)
            {
                var globalEnchantment = Aow.Units.AoWE.AoWE.AoWHSMap.FGlobalMagicControl.FEnchantments.GetEnchantments(Index);
                var call__233 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(A1.FPlayer);
                Mana = call__233.FMagicControl.FMana + A1.FCastingProgress;
                RequiredMana = Aow.Rtl.Rtl.Round(((double)(((double)globalEnchantment.FDispelMana) * 0.02)) * FDispelChance) + FEventMana;
                Result = RequiredMana <= Mana;
            }

            return Result;
        }

        // GlobalSpells.pas:772
        public override bool Process(Aow.Units.AoWE.TAbstractUnit A1, int A2)
        {
            bool Result = default;
            Aow.Units.AoWE.TGlobalEnchantment Enchantment = default;
            int Index = default;
            sbyte X = default;
            sbyte Y = default;
            sbyte Layer = default;
            Aow.Units.Animation.TTwinkleAnimation Twinkle = default;
            int Mana = default;
            base.Process(A1, A2);
            var call__234 = Aow.Units.AoWE.AoWE.AoWHSMap.FGlobalMagicControl.IndexOfID(FEnchantmentID);
            Index = call__234;
            var call__235 = Aow.Units.AoWE.AoWE.AoWHSMap.FGlobalMagicControl.FEnchantments.GetEnchantments(Index);
            Enchantment = call__235;
            A1.GetLocation(ref X, ref Y, ref Layer);
            Aow.Units.AoWE.AoWE.AoWHSMap.ValidateObservation(unchecked((sbyte)A1.FPlayer), unchecked((sbyte)X), unchecked((sbyte)Y), unchecked((sbyte)Layer), 0);
            Twinkle = Aow.Units.Animation.TTwinkleAnimation.Create(null);
            var call__236 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(51);
            var call__237 = call__236.FCastImages.ImageSequences[0];
            Twinkle.Setup(call__237, TSet<Aow.Units.Animation.TTwinkleModeElement>.Of(Aow.Units.Animation.TTwinkleModeElement.tmExpand, Aow.Units.Animation.TTwinkleModeElement.tmExplode), FEventMana + 50, 192, 50, 30);
            Aow.Units.AoWE.AoWE.AoWHSMap.PlaceHX(Twinkle, X, Y, Layer);
            var call__238 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
            call__238.FCastSounds.Play(0);
            Mana = Aow.Rtl.Rtl.Round(((double)(((double)Enchantment.FDispelMana) * 0.02)) * FDispelChance);
            var call__239 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(A1.FPlayer);
            var call__240 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(A1.FPlayer);
            call__240.FMagicControl.SetMana(call__239.FMagicControl.FMana - Mana);
            var call__241 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(100);
            if (call__241 < FDispelChance)
            {
                if (Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer == A1.FPlayer)
                {
                    var call__242 = Enchantment.GetName();
                    var call__243 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.XSuccessfullyDispelledRStr);
                    var call__244 = Aow.Rtl.Rtl.Format(call__243, [call__242]);
                    Aow.Units.AoWE.AoWE.AoWHSMap.ShowMessage(call__244);
                }

                Aow.Units.AoWE.AoWE.AoWHSMap.FGlobalMagicControl.ExecuteDispelEnchantment(unchecked((sbyte)A1.FPlayer), FEnchantmentID);
            }
            else
            {
                if (Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer == A1.FPlayer)
                {
                    // { Original formats this message with an unused enchantment-name argument. }
                    var call__245 = Enchantment.GetName();
                    var call__246 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.DispelMagicFailedRStr);
                    var call__247 = Aow.Rtl.Rtl.Format(call__246, [call__245]);
                    Aow.Units.AoWE.AoWE.AoWHSMap.ShowMessage(call__247);
                }
            }

            var call__248 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
            call__248.CastingDone(A1, FEventMana);
            Result = false;
            return Result;
        }

        // NetworkE.pas:230
        public static new Aow.Units.GlobalSpells.TDisjunctionTE Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.GlobalSpells.TDisjunctionTE o = new Aow.Units.GlobalSpells.TDisjunctionTE();
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

        public new partial class Meta : Aow.Units.AoWE.TSpellTE.Meta
        {
            // GlobalSpells.pas:745
            public override int ClassID()
            {
                int Result = default;
                Result = 135961;
                return Result;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.GlobalSpells.TDisjunctionTE => (Meta)MetaRegistry.Of(typeof(T));
            // NetworkE.pas:230
            public new Aow.Units.GlobalSpells.TDisjunctionTE Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.GlobalSpells.TDisjunctionTE o = (Aow.Units.GlobalSpells.TDisjunctionTE)NewInstance();
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

    // { TDisjunctionSpellCaster (instance size 28, VMT 557E9B00): $14 enchantment ID,
//     $18 dispel chance; inherited $08 spell ID, $0C source unit ID, $10 references. }
    public partial class TDisjunctionSpellCaster : Aow.Units.AoWE.TSpellCaster
    {
        public int FEnchantmentID;
        // { $14 }
        public int FDispelChance;
        // GlobalSpells.pas:806
        // { $18 }
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            FDispelChance = 50;
        }

        // GlobalSpells.pas:868
        public override bool CanCast(ref Aow.Rtl.AnsiString Reason)
        {
            bool Result = default;
            Aow.Units.AoWE.THero Source = default;
            Result = base.CanCast(ref Reason);
            var call__249 = Aow.Units.AoWE.AoWE.AoWHSMap.FUnitControl.FindUnit(FCasterSourceID);
            Source = (Aow.Units.AoWE.THero)call__249;
            if (Result && (SelectedEnchantment() != null))
            {
                bool bool__250 = !Aow.Units.AoWE.AoWE.AoWHSMap.FGlobalMagicControl.FActiveEnchantments.Contains(Aow.Units.AoWE.TGlobalEnchantmentType.getSpellWard);
                if (!bool__250)
                {
                    var call__251 = SelectedEnchantment();
                    bool__250 = (call__251.FEnchantmentType == Aow.Units.AoWE.TGlobalEnchantmentType.getSpellWard);
                }

                if (bool__250)
                {
                    var call__252 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Source.FPlayer);
                    var call__253 = call__252.FMagicControl.GetCheatEnabled();
                    if (!call__253)
                    {
                        var call__254 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(51);
                        var call__255 = GetMana();
                        var call__256 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Source.FPlayer);
                        if ((call__256.FMagicControl.FMana + Source.FCastingProgress) >= (call__254.FBaseManaCost + call__255))
                        {
                            Result = true;
                        }
                        else
                        {
                            Result = false;
                            Reason = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.NotEnoughManaRStr);
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
                    Reason = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.CannotDispelWhenSpellWardIsActiveRStr);
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
            Aow.Rtl.AnsiString Reason = default;
            Aow.Units.AoWE.TAbstractUnit Source = default;
            Aow.Units.GlobalSpells.TDisjunctionTE Event = default;
            if (CanCast(ref Reason))
            {
                var call__257 = Aow.Units.AoWE.AoWE.AoWHSMap.FUnitControl.FindUnit(FCasterSourceID);
                Source = call__257;
                var call__258 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Source.FPlayer);
                var call__259 = call__258.GetBusy();
                if (!call__259)
                {
                    Event = Aow.Units.GlobalSpells.TDisjunctionTE.Create(null);
                    var call__260 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(51);
                    Event.Initialize(FCasterSpellID, unchecked((byte)Source.FPlayer), Source.FUnitID, call__260.FBaseManaCost);
                    Event.FEnchantmentID = FEnchantmentID;
                    Event.FDispelChance = FDispelChance;
                    var call__261 = Aow.Units.AoWE.AoWE.AoWHSMap.FTokenManager.GetTokenControl(Source.FPlayer);
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
                Aow.Units.AoWE.AoWE.AoWHSMap.ShowMessage(Reason);
            }

            return Result;
        }

        // GlobalSpells.pas:812
        public void SetEnchantmentID(int A1)
        {
            FEnchantmentID = A1;
        }

        // GlobalSpells.pas:817
        public Aow.Units.AoWE.TGlobalEnchantment SelectedEnchantment()
        {
            Aow.Units.AoWE.TGlobalEnchantment Result = default;
            int Index = default;
            var call__262 = Aow.Units.AoWE.AoWE.AoWHSMap.FGlobalMagicControl.IndexOfID(FEnchantmentID);
            Index = call__262;
            if (Index != -1)
            {
                var call__263 = Aow.Units.AoWE.AoWE.AoWHSMap.FGlobalMagicControl.FEnchantments.GetEnchantments(Index);
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
            Aow.Units.AoWE.TGlobalEnchantment Enchantment = default;
            Enchantment = SelectedEnchantment();
            if (Enchantment != null)
            {
                Result = Aow.Rtl.Rtl.Round(((double)(((double)Enchantment.FDispelMana) * 0.02)) * FDispelChance);
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
            Aow.Units.AoWE.THero Source = default;
            int Mana = default;
            Result = false;
            var call__264 = Aow.Units.AoWE.AoWE.AoWHSMap.FUnitControl.FindUnit(FCasterSourceID);
            Source = (Aow.Units.AoWE.THero)call__264;
            if (SelectedEnchantment() != null)
            {
                var call__265 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Source.FPlayer);
                var call__266 = call__265.FMagicControl.GetCheatEnabled();
                if (!call__266)
                {
                    var call__267 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Source.FPlayer);
                    Mana = call__267.FMagicControl.FMana + Source.FCastingProgress;
                    var call__268 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(51);
                    var call__269 = GetMana();
                    if (Mana >= (call__268.FBaseManaCost + call__269))
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

        // GlobalSpells.pas:150
        public static new Aow.Units.GlobalSpells.TDisjunctionSpellCaster Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.GlobalSpells.TDisjunctionSpellCaster o = new Aow.Units.GlobalSpells.TDisjunctionSpellCaster();
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

    // { TDisjunction (instance size 52, VMT 557E9BE4): inherited $08 name, $10 spell ID, $14 mana, $20 sphere, $21 level, $22 type, $28 sounds, $2C images. }
    public partial class TDisjunction : Aow.Units.AoWE.TSpell
    {
        // GlobalSpells.pas:931
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            var call__270 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.DisjunctionRStr);
            FSpellName = call__270;
            FSpellID = 51;
            FSpellType = Aow.Units.AoWE.TSpellType.stGlobal;
            FBaseManaCost = 0;
            FSpellLevel = 2;
            FMagicSphere = Aow.Units.AoWE.TMagicSphere.msCosmos;
        }

        // GlobalSpells.pas:942
        public override bool CanActivate(Aow.Units.AoWE.TAbstractUnit A1, ref Aow.Rtl.AnsiString A2)
        {
            bool Result = default;
            int Count = default;
            int I = default;
            Aow.Rtl.AnsiString Reason = default;
            Count = 0;
            {
                I = 0;
                int end__271 = Aow.Units.AoWE.AoWE.AoWHSMap.FGlobalMagicControl.FEnchantments.Count - 1;
                for (; I <= end__271; I += 1)
                {
                    var call__272 = Aow.Units.AoWE.AoWE.AoWHSMap.FGlobalMagicControl.FEnchantments.GetEnchantments(I);
                    bool bool__273 = (call__272.FPlayerIndex != A1.FPlayer);
                    if (bool__273)
                    {
                        var call__274 = Aow.Units.AoWE.AoWE.AoWHSMap.FGlobalMagicControl.FEnchantments.GetEnchantments(I);
                        var call__275 = call__274.VisibleForPlayer(unchecked((sbyte)A1.FPlayer));
                        bool__273 = call__275;
                    }

                    bool bool__276 = bool__273;
                    if (bool__276)
                    {
                        var call__277 = Aow.Units.AoWE.AoWE.AoWHSMap.FGlobalMagicControl.FEnchantments.GetEnchantments(I);
                        var call__278 = call__277.CanDispel(unchecked((sbyte)A1.FPlayer), ref Reason);
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
                A2 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.NoGlobalOpponentEnchantmentsActiveRStr);
                Result = false;
            }
            else
            {
                Result = true;
            }

            return Result;
        }

        // GlobalSpells.pas:962
        public override bool Activate(Aow.Units.AoWE.TAbstractUnit A1)
        {
            bool Result = default;
            Aow.Rtl.AnsiString Reason = default;
            Aow.Units.GlobalSpells.TDisjunctionSpellCaster Caster = default;
            if (CanActivate(A1, ref Reason))
            {
                Caster = Aow.Units.GlobalSpells.TDisjunctionSpellCaster.Create(null);
                Caster.FCasterSpellID = FSpellID;
                Caster.FCasterSourceID = A1.FUnitID;
                Aow.Units.AoWE.AoWE.AoWHSMap.FCosmeticSurgeryEvents.TriggerNotifyEvents(Caster);
                Caster.Release();
                Result = true;
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // GlobalSpells.pas:164
        public static new Aow.Units.GlobalSpells.TDisjunction Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.GlobalSpells.TDisjunction o = new Aow.Units.GlobalSpells.TDisjunction();
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

    // { TCrusade (instance size 52, VMT 557E9CFC): inherited $08 name, $10 spell ID, $14 mana, $20 sphere, $21 level, $22 type, $28 sounds, $2C images. }
    public partial class TCrusade : Aow.Units.AoWE.TSpell
    {
        // GlobalSpells.pas:1302
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            var call__279 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.CrusadeRStr);
            FSpellName = call__279;
            FSpellID = 75;
            FSpellType = Aow.Units.AoWE.TSpellType.stGlobal;
            FBaseManaCost = 0;
            FSpellLevel = 4;
            FMagicSphere = Aow.Units.AoWE.TMagicSphere.msLife;
        }

        // GlobalSpells.pas:1340
        public override bool ExecuteTE(Aow.Units.AoWE.TSpellTE A1, Aow.Units.AoWE.TAbstractUnit A2, int A3)
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
            Aow.Units.AoWE.TArmyHS Party = default;
            Aow.Units.AoWE.TUnit NewUnit = default;
            if (A3 == 0)
            {
                A2.GetLocation(ref X, ref Y, ref Layer);
                Aow.Units.AoWE.AoWE.AoWHSMap.ValidateObservation(unchecked((sbyte)A2.FPlayer), unchecked((sbyte)X), unchecked((sbyte)Y), unchecked((sbyte)Layer), 0);
                Aow.Units.HSEngine.HSEngine.HXtoHP(X, Y, ref Xhp, ref Yhp);
                var worldFXHS = Aow.Units.AoWE.AoWE.AoWHSMap.AddEffect(Aow.Units.AoWE.TworldFXType.fxCrusade);
                worldFXHS.FX = Xhp + 16;
                worldFXHS.FY = Yhp + 16;
                worldFXHS.FLayer = Layer;

                var call__281 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(100);
                if (call__281 < 20)
                {
                    Party = Aow.Units.AoWE.TArmyHS.Create(null);
                    Party.FArmy.SetPlayer(0);
                    Count = 1;
                    var call__282 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(1);
                    UnitIndex = call__282;
                    {
                        J = 0;
                        int end__283 = Count - 1;
                        for (; J <= end__283; J += 1)
                        {
                            NewUnit = Aow.Units.AoWE.TUnit.Create(null);
                            var call__284 = Aow.Units.AoWE.AoWE.AoWHSSet.FUnitResources.GetUnitResource(Aow.Units.GlobalSpells.GlobalSpells.TCrusade_ExecuteTE__LargeUnits[UnitIndex]);
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
                            Party = Aow.Units.AoWE.TArmyHS.Create(null);
                            Party.FArmy.SetPlayer(0);
                            var call__286 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(3);
                            Count = call__286 + 4;
                            {
                                J = 0;
                                int end__287 = Count - 1;
                                for (; J <= end__287; J += 1)
                                {
                                    NewUnit = Aow.Units.AoWE.TUnit.Create(null);
                                    var call__288 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(2);
                                    UnitIndex = call__288;
                                    var call__289 = Aow.Units.AoWE.AoWE.AoWHSSet.FUnitResources.GetUnitResource(Aow.Units.GlobalSpells.GlobalSpells.TCrusade_ExecuteTE__FootUnits[UnitIndex]);
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

                    Party = Aow.Units.AoWE.TArmyHS.Create(null);
                    Party.FArmy.SetPlayer(0);
                    var call__290 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(3);
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
                            NewUnit = Aow.Units.AoWE.TUnit.Create(null);
                            var call__292 = Aow.Units.AoWE.AoWE.AoWHSSet.FUnitResources.GetUnitResource(Aow.Units.GlobalSpells.GlobalSpells.TCrusade_ExecuteTE__MountedUnits[UnitIndex]);
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

                var call__293 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellID);
                call__293.CastingDone(A2, FBaseManaCost);
                var call__294 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellID);
                call__294.FCastSounds.Play(0);
            }

            Result = false;
            return Result;
        }

        // GlobalSpells.pas:1417
        public override bool Activate(Aow.Units.AoWE.TAbstractUnit A1)
        {
            bool Result = default;
            Aow.Rtl.AnsiString Reason = default;
            Aow.Units.AoWE.TSpellTE Event = default;
            base.Activate(A1);
            var call__295 = CanActivate(A1, ref Reason);
            bool bool__296 = call__295;
            if (bool__296)
            {
                var call__297 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(A1.FPlayer);
                var call__298 = call__297.GetBusy();
                bool__296 = !call__298;
            }

            if (bool__296)
            {
                Event = Aow.Units.AoWE.TSpellTE.Create(null);
                Event.Initialize(FSpellID, unchecked((byte)A1.FPlayer), A1.FUnitID, FBaseManaCost);
                var call__299 = Aow.Units.AoWE.AoWE.AoWHSMap.FTokenManager.GetTokenControl(A1.FPlayer);
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
        public bool PlaceParty(Aow.Units.AoWE.TAbstractUnit Source, Aow.Units.AoWE.TArmyHS Party)
        {
            bool Result = default;
            int TargetX = default;
            int TargetY = default;
            sbyte X = default;
            sbyte Y = default;
            sbyte Layer = default;
            Aow.Units.Animation.TImageSequenceAnimation Animation = default;
            Source.GetLocation(ref X, ref Y, ref Layer);
            var call__300 = Aow.Units.HSEngine.HSEngine.RadToHN(5);
            var call__301 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(call__300);
            Aow.Units.HSEngine.HSEngine.CenterHNtoHX(X, Y, call__301, ref TargetX, ref TargetY);
            if (Party.PlaceEx(unchecked((sbyte)TargetX), unchecked((sbyte)TargetY), unchecked((sbyte)Layer), TSet<Aow.Units.AoWE.TArmyPlaceModeElement>.Of(Aow.Units.AoWE.TArmyPlaceModeElement.apSafe, Aow.Units.AoWE.TArmyPlaceModeElement.apScanArea)))
            {
                Party.Activate();
                Party.FArmy.SetBehavior(255, Aow.Units.AoWE.TGroupBehavior.gbSuicidal);
                var call__302 = Party.GetLhx();
                var call__303 = Party.GetYhx();
                var call__304 = Party.GetXhx();
                var call__305 = Aow.Units.AoWE.AoWE.AoWHSMap.Watching(unchecked((sbyte)call__304), unchecked((sbyte)call__303), unchecked((sbyte)call__302), 0);
                if (call__305)
                {
                    var call__306 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(34);
                    var call__307 = call__306.FCastImages.GetImageSequence(0);
                    if (call__307 != null)
                    {
                        // { Original uses Warp Party's images for Crusade parties. }
                        Animation = Aow.Units.Animation.TImageSequenceAnimation.Create(null);
                        var call__308 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(34);
                        var call__309 = call__308.FCastImages.GetImageSequence(0);
                        Animation.SetImageSequence(call__309);
                        Animation.FAnimationLevel = 28;
                        var call__310 = Party.GetLhx();
                        var call__311 = Party.GetYhx();
                        var call__312 = Party.GetXhx();
                        Aow.Units.AoWE.AoWE.AoWHSMap.PlaceHX(Animation, call__312, call__311, call__310);
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

        // GlobalSpells.pas:172
        public static new Aow.Units.GlobalSpells.TCrusade Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.GlobalSpells.TCrusade o = new Aow.Units.GlobalSpells.TCrusade();
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

    [StructLayout(LayoutKind.Explicit, Size = 110)]
    public struct TGlobalSpellEngineFields
    {
        [FieldOffset(0)]
        public Aow.Units.FixedArray_byte_109 Reserved;
        [FieldOffset(109)]
        public byte Detail;
    }

    public static partial class GlobalSpells
    {
        public static Aow.Units.FixedArray_int_1 TCrusade_ExecuteTE__LargeUnits = Aow.Units.FixedArray_int_1.From([133]);
        public static Aow.Units.FixedArray_int_3 TCrusade_ExecuteTE__MountedUnits = Aow.Units.FixedArray_int_3.From([132, 134, 128]);
        public static Aow.Units.FixedArray_int_2 TCrusade_ExecuteTE__FootUnits = Aow.Units.FixedArray_int_2.From([129, 126]);
        // GlobalSpells.pas:1435
        public static void RegisterGlobalSpells(Aow.Units.AoWE.TAoWEngine Engine)
        {
            Aow.Units.AoWE.TSpellControl Control = default;
            Control = Engine.FGameSet.FSpellControl;
            Engine.RegisterEClasses([Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.GlobalSpells.TWarpPartyTE>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.GlobalSpells.TDisjunctionTE>()]);
            var call__313 = Aow.Units.GlobalSpells.TConcealArea.Create(null);
            Control.RegisterSpell(call__313);
            var call__314 = Aow.Units.GlobalSpells.TBirdsView.Create(null);
            Control.RegisterSpell(call__314);
            var call__315 = Aow.Units.GlobalSpells.TWarpParty.Create(null);
            Control.RegisterSpell(call__315);
            var call__316 = Aow.Units.GlobalSpells.TDisjunction.Create(null);
            Control.RegisterSpell(call__316);
            var call__317 = Aow.Units.GlobalSpells.TResurrectHero.Create(null);
            Control.RegisterSpell(call__317);
            var call__318 = Aow.Units.GlobalSpells.TAnimateHero.Create(null);
            Control.RegisterSpell(call__318);
            var call__319 = Aow.Units.GlobalSpells.TCallHero.Create(null);
            Control.RegisterSpell(call__319);
            var call__320 = Aow.Units.GlobalSpells.TCrusade.Create(null);
            Control.RegisterSpell(call__320);
        }

        // GlobalSpells.pas:1451
        public static void UnregisterGlobalSpells(Aow.Units.AoWE.TAoWEngine Engine)
        {
            Aow.Units.AoWE.TSpellControl Control = default;
            Control = Engine.FGameSet.FSpellControl;
            Engine.UnregisterEClasses([Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.GlobalSpells.TWarpPartyTE>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.GlobalSpells.TDisjunctionTE>()]);
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
