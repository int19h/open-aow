// Translated from GlobalSpells.pas.
using System.Runtime.InteropServices;
using Aow.Rtl;
using Aow.Units.Animation;
using Aow.Units.AoWE;
using Aow.Units.Engine;
using Aow.Units.EventLog;
using Aow.Units.GFXE;
using Aow.Units.HSEngine;
using static Aow.Rtl.RtlUnit;
using static Aow.Units.AoWE.AoWEUnit;
using static Aow.Units.EventLog.EventLogUnit;
using static Aow.Units.GFXE.GFXEUnit;
using static Aow.Units.HSEngine.HSEngineUnit;

namespace Aow.Units.GlobalSpells;

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
        FHexagons = new TIntegerList();
    }

    // GlobalSpells.pas:211
    public override int GetShowPriority() => 2013265920;

    // GlobalSpells.pas:244
    public override void Show(int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
    {
        int Xhp = default;
        int Yhp = default;
        int HX = default;
        int HY = default;
        int Intensity;
        int Radius;
        int Direction;
        int X = default;
        int Y = default;
        int Delay;
        if (AoWHSMap.ViewL == FLayer)
        {
            FFrame += 1;
            if (AoWEngine.FAnimationDetail == 2 && AoWHSMap.ViewL == FLayer)
            {
                Intensity = 60 - Math.Abs(FFrame * 3 - 60);
                HXtoHP(FX, FY, ref Xhp, ref Yhp);
                Xhp = Xhp - XhpSurf - 8;
                Yhp -= YhpSurf;
                for (int I = FHexagons.Count - 1; I >= 0; I--)
                {
                    HNtoHP(FHexagons[I], ref HX, ref HY);
                    HX += Xhp;
                    HY += Yhp;
                    AoWHSSet.FGeneralImages[5].ShowBlended(HX, HY, SurfaceDesc, AlphaBlendTable.LookupTables[Intensity]);
                }
            }

            // { Original bug: Xhp/Yhp are uninitialized outside high detail. }
            RandSeed = AoWHSMap.FInitialRandomSeed;
            for (int I = 0; I <= 200; I++)
            {
                Radius = Random(448);
                Direction = Random(360);
                Delay = Random(20);
                if (FFrame - Delay >= 0)
                {
                    DegreesRadToCo(Direction, Radius, ref X, ref Y);
                    AoWHSSet.FSpellControl.GetSpell(57).FCastImages.Show(0, Xhp + X, Yhp + Y, FFrame - Delay, SurfaceDesc);
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
        TMapLevel Level;
        FX = X;
        FY = Y;
        FLayer = Layer;
        FFrame = 0;
        AoWHSSet.FSpellControl.GetSpell(1).FCastSounds.Play(0);
        AoWHSMap.DynamicFixedVisibleHS.AddChild(this);
        InitHNWalk(ref Walk, FX, FY, 0);
        Level = AoWHSMap.MapContainer.MapLevels[FLayer];
        while (Walk.Radius <= Radius)
        {
            if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < Level.Width && Walk.Y < Level.Height)
            {
                if (((TAoWMapField)Level.GetField(Walk.X, Walk.Y)).GetExplored(AoWHSMap.FSeatedPlayer))
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
        FSpellName = TranslateRStr(ConcealAreaRStr);
        FSpellID = 1;
        FSpellType = TSpellType.stGlobal;
        FBaseManaCost = 5;
        FSpellLevel = 1;
        FMagicSphere = TMagicSphere.msEarth;
    }

    // GlobalSpells.pas:302
    public override bool ExecuteTE(TSpellTE Event, TAbstractUnit Source, int ExecuteCount)
    {
        TAoWMapField MapF;
        sbyte X = default;
        sbyte Y = default;
        sbyte Layer = default;
        THNWalkData Walk = default;
        TMapLevel Level;
        TLocationTextEventLog Log;
        TConcealAreaHS Conceal;
        base.ExecuteTE(Event, Source, ExecuteCount);
        CastingDone(Source, Event.FEventMana);
        Source.GetLocation(ref X, ref Y, ref Layer);
        if (Source.FPlayer == AoWHSMap.FSeatedPlayer)
        {
            Conceal = new TConcealAreaHS(null);
            Conceal.StartAnimation(X, Y, Layer, 14);
        }

        for (int Player = 1, last = AoWHSMap.FPlayers.Count - 1; Player <= last; Player++)
        {
            Level = AoWHSMap.MapContainer.MapLevels[Layer];
            if (AoWHSMap.FPlayers.GetDiplomaticRelation(Source.FPlayer, (sbyte)Player) == TDiplomaticRelation.drWar && Player != Source.FPlayer)
            {
                Log = null;
                InitHNWalk(ref Walk, X, Y, 0);
                while (Walk.Radius <= 14)
                {
                    if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < Level.Width && Walk.Y < Level.Height)
                    {
                        MapF = (TAoWMapField)Level.GetField(Walk.X, Walk.Y);
                        if (MapF.GetExplored(Player) && !AoWHSMap.VisibleForPlayer(Walk.X, Walk.Y, Layer, (sbyte)Player))
                        {
                            MapF.SetExplored(Player, false);
                            if (Log == null)
                            {
                                Log = new TLocationTextEventLog(null);
                                Log.FXhx = (sbyte)Walk.X;
                                Log.FYhx = (sbyte)Walk.Y;
                                Log.FLhx = Layer;
                                Log.FPlayer = Source.FPlayer;
                                var concealArea = TranslateRStr(ConcealAreaRStr);
                                Log.FText = RtlUnit.Format(TranslateRStr(XCastRStr), [concealArea]);
                                AoWHSMap.FPlayers.GetPlayers(Player).FEventLogbook.AddEvent(Log);
                                Log.Release();
                            }
                        }
                    }

                    WalkToNextHN(ref Walk);
                }
            }
            else
            {
                if (AoWHSMap.VisibleForPlayer(X, Y, Layer, (sbyte)Player))
                {
                    Log = new TLocationTextEventLog(null);
                    Log.FXhx = X;
                    Log.FYhx = Y;
                    Log.FLhx = Layer;
                    Log.FPlayer = Source.FPlayer;
                    var concealArea = TranslateRStr(ConcealAreaRStr);
                    Log.FText = RtlUnit.Format(TranslateRStr(XCastRStr), [concealArea]);
                    AoWHSMap.FPlayers.GetPlayers(Player).FEventLogbook.AddEvent(Log);
                    Log.Release();
                }
            }
        }

        return false;
    }

    // GlobalSpells.pas:297
    public override bool GetEnabled() => AoWHSMap.FExplorationEnabled;

    // GlobalSpells.pas:368
    public override bool CanActivate(TAbstractUnit Source, ref string MessageText)
    {
        bool War;
        bool Result = base.CanActivate(Source, ref MessageText);
        if (Result)
        {
            War = false;
            for (int Player = 1, last = AoWHSMap.FPlayers.Count - 1; Player <= last; Player++)
            {
                War = War || AoWHSMap.FPlayers.GetPlayers(Source.FPlayer).FDiplomaticRelations.GetDiplomaticRelation((sbyte)Player) == TDiplomaticRelation.drWar;
            }

            if (!War)
            {
                var concealArea = TranslateRStr(ConcealAreaRStr);
                MessageText = RtlUnit.Format(TranslateRStr(AWarRelationIsRequiredForXRStr), [concealArea]);
                Result = false;
            }
        }

        return Result;
    }

    // GlobalSpells.pas:388
    public override bool Activate(TAbstractUnit Source)
    {
        bool Result;
        string Reason = "";
        TSpellTE Event;
        base.Activate(Source);
        if (CanActivate(Source, ref Reason) && !AoWHSMap.FPlayers.GetPlayers(Source.FPlayer).GetBusy())
        {
            Event = new TSpellTE(null);
            Event.Initialize(FSpellID, (byte)Source.FPlayer, Source.FUnitID, FBaseManaCost);
            AoWHSMap.FTokenManager.GetTokenControl(Source.FPlayer).AddEvent(Event);
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
        FHexagons = new TIntegerList();
    }

    // GlobalSpells.pas:418
    public override int GetShowPriority() => 2013265920;

    // GlobalSpells.pas:451
    public override void Show(int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
    {
        int Xhp = default;
        int Yhp = default;
        int HX = default;
        int HY = default;
        int Intensity;
        int Radius;
        int Direction;
        int X = default;
        int Y = default;
        int Delay;
        if (AoWHSMap.ViewL == FLayer)
        {
            FFrame += 1;
            Intensity = 100 - FFrame * 2;
            HXtoHP(FX, FY, ref Xhp, ref Yhp);
            Xhp = Xhp - XhpSurf - 8;
            Yhp -= YhpSurf;
            for (int I = 0, last = FHexagons.Count - 1; I <= last; I++)
            {
                HNtoHP(FHexagons[I], ref HX, ref HY);
                HX += Xhp;
                HY += Yhp;
                AoWHSSet.FGeneralImages[5].ShowBlended(HX, HY, SurfaceDesc, AlphaBlendTable.LookupTables[Intensity]);
            }

            RandSeed = AoWHSMap.FInitialRandomSeed;
            for (int I = 0; I <= 200; I++)
            {
                Radius = Random(448);
                Direction = Random(360);
                Delay = Random(30);
                if (FFrame - Delay >= 0)
                {
                    DegreesRadToCo(Direction, Radius, ref X, ref Y);
                    AoWHSSet.FSpellControl.GetSpell(57).FCastImages.Show(0, Xhp + X, Yhp + Y, FFrame - Delay, SurfaceDesc);
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
        TMapLevel Level;
        FX = X;
        FY = Y;
        FLayer = Layer;
        FFrame = 0;
        AoWHSSet.FSpellControl.GetSpell(2).FCastSounds.Play(0);
        InitHNWalk(ref Walk, X, Y, 0);
        Level = AoWHSMap.MapContainer.MapLevels[Layer];
        while (Walk.Radius <= Radius)
        {
            if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < Level.Width && Walk.Y < Level.Height)
            {
                if (!((TAoWMapField)Level.GetField(Walk.X, Walk.Y)).GetExplored(AoWHSMap.FSeatedPlayer))
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
        FSpellName = TranslateRStr(BirdsViewRStr);
        FSpellID = 2;
        FSpellType = TSpellType.stGlobal;
        FBaseManaCost = 5;
        FSpellLevel = 1;
        FMagicSphere = TMagicSphere.msAir;
    }

    // GlobalSpells.pas:505
    public override bool ExecuteTE(TSpellTE Event, TAbstractUnit Source, int ExecuteCount)
    {
        sbyte X = default;
        sbyte Y = default;
        sbyte Layer = default;
        base.ExecuteTE(Event, Source, ExecuteCount);
        CastingDone(Source, Event.FEventMana);
        Source.GetLocation(ref X, ref Y, ref Layer);
        if (AoWHSMap.FSeatedPlayer == Source.FPlayer)
        {
            var birdsViewHS = new TBirdsViewHS(null);
            birdsViewHS.StartAnimation(X, Y, Layer, 14);
        }

        AoWHSMap.ExploreArea(X, Y, Layer, Source.FPlayer, 14);
        return false;
    }

    // GlobalSpells.pas:500
    public override bool GetEnabled() => AoWHSMap.FExplorationEnabled;

    // GlobalSpells.pas:518
    public override bool CanActivate(TAbstractUnit Source, ref string MessageText)
    {
        sbyte X = default;
        sbyte Y = default;
        sbyte Layer = default;
        bool Result = base.CanActivate(Source, ref MessageText);
        if (Result)
        {
            Source.GetLocation(ref X, ref Y, ref Layer);
            if (Layer != 0)
            {
                MessageText = TranslateRStr(CannotCastSpellInUndergoundRStr);
                Result = false;
            }
        }

        return Result;
    }

    // GlobalSpells.pas:534
    public override bool Activate(TAbstractUnit Source)
    {
        bool Result;
        string Reason = "";
        TSpellTE Event;
        base.Activate(Source);
        if (CanActivate(Source, ref Reason) && !AoWHSMap.FPlayers.GetPlayers(Source.FPlayer).GetBusy())
        {
            Event = new TSpellTE(null);
            Event.Initialize(FSpellID, (byte)Source.FPlayer, Source.FUnitID, FBaseManaCost);
            AoWHSMap.FTokenManager.GetTokenControl(Source.FPlayer).AddEvent(Event);
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
    public override bool Validate(THero source)
    {
        byte Selection = default;
        TArmyHS Army = default;
        bool Result = base.Validate(source) && FArmyID.Valid();
        if (Result)
        {
            FArmyID.RetrieveArmy(ref Army, ref Selection);
            Result = ((TWarpParty)AoWHSSet.FSpellControl.GetSpell(FEventSpellID)).ValidTarget(Army, FWarpLocation.X, FWarpLocation.Y, FWarpLocation.L);
        }

        return Result;
    }

    // GlobalSpells.pas:590
    public override bool Process(TAbstractUnit source, int value)
    {
        bool ShowSource;
        bool ShowTarget;
        byte Selection = default;
        sbyte X = default;
        sbyte Y = default;
        sbyte Layer = default;
        TArmyHS Army = default;
        TMapField Field;
        TImageSequenceAnimation Animation;
        base.Process(source, value);
        source.GetLocation(ref X, ref Y, ref Layer);
        FArmyID.RetrieveArmy(ref Army, ref Selection);
        Field = AoWHSMap.MapContainer.GetFieldXYL(FWarpLocation.XYL);
        Army.MoveTo(Field, 0);
        ShowSource = AoWHSMap.FSeatedPlayer == source.FPlayer || AoWHSMap.VisibleForPlayer(X, Y, Layer, AoWHSMap.FSeatedPlayer);
        ShowTarget = AoWHSMap.FSeatedPlayer == source.FPlayer || AoWHSMap.VisibleForPlayer(Field.Xhx, Field.Yhx, Field.Lhx, AoWHSMap.FSeatedPlayer);
        if (ShowSource && AoWHSSet.FSpellControl.GetSpell(FEventSpellID).FCastImages.ImageSequences[0] != null)
        {
            Animation = new TImageSequenceAnimation(null);
            Animation.SetImageSequence(AoWHSSet.FSpellControl.GetSpell(FEventSpellID).FCastImages.ImageSequences[0]);
            AoWHSMap.PlaceHX(Animation, X, Y, Layer);
        }

        if (ShowTarget && AoWHSSet.FSpellControl.GetSpell(FEventSpellID).FCastImages.ImageSequences[0] != null)
        {
            Animation = new TImageSequenceAnimation(null);
            Animation.SetImageSequence(AoWHSSet.FSpellControl.GetSpell(FEventSpellID).FCastImages.ImageSequences[0]);
            AoWHSMap.PlaceHX(Animation, Field.Xhx, Field.Yhx, Field.Lhx);
        }

        if (source.FPlayer == AoWHSMap.FSeatedPlayer)
        {
            AoWHSMap.CenterViewTo(Field.Xhx, Field.Yhx, Field.Lhx);
        }
        else
        {
            AoWHSMap.ValidateObservation(source.FPlayer, Field.Xhx, Field.Yhx, Field.Lhx, 0);
        }

        if (source.FPlayer == AoWHSMap.FSeatedPlayer || AoWHSMap.Watching(X, Y, Layer, 0) || AoWHSMap.Watching(Field.Xhx, Field.Yhx, Field.Lhx, 0))
        {
            AoWHSSet.FSpellControl.GetSpell(FEventSpellID).FCastSounds.Play(0);
        }

        var spell = AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
        AoWHSSet.FSpellControl.GetSpell(FEventSpellID).CastingDone(source, spell.FBaseManaCost);
        return false;
    }

    // GlobalSpells.pas:92

}

// inherited name, ID, mana, sphere, level, spell type
public partial class TWarpParty : TSpell
{
    // GlobalSpells.pas:632
    public TWarpParty(TECustomNode owner) : base(owner)
    {
        FSpellName = TranslateRStr(WarpPartyRStr);
        FSpellID = 34;
        FSpellType = TSpellType.stGlobal;
        FBaseManaCost = 1;
        FSpellLevel = 2;
        FMagicSphere = TMagicSphere.msFire;
    }

    // GlobalSpells.pas:719
    public override bool Activate(TAbstractUnit Source)
    {
        bool Result;
        string Reason = "";
        TWarpPartyTE Event;
        base.Activate(Source);
        if (CanActivate(Source, ref Reason) && !AoWHSMap.FPlayers.GetPlayers(Source.FPlayer).GetBusy())
        {
            Event = new TWarpPartyTE(null);
            Event.Initialize(FSpellID, (byte)Source.FPlayer, Source.FUnitID, FBaseManaCost);
            Event.FArmyID = ((TArmy)Source.Owner).CreateID(255);
            var warpLocation = CalculateWarpLocation(((TArmy)Source.Owner).GetArmyHS());
            Event.FWarpLocation.XYL = warpLocation;
            AoWHSMap.FSelectedArmy.Unselect();
            if (Event.FWarpLocation.XYL != 16777215)
            {
                AoWHSMap.FTokenManager.GetTokenControl(Source.FPlayer).AddEvent(Event);
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
        TAoWMapField Field;
        TMapFieldMsg Msg = default;
        Field = (TAoWMapField)AoWHSMap.MapContainer.GetField(X, Y, Layer);
        bool Result = Field.GetArmyHS() == null && Field.GetPlayer() == -1;
        if (Result)
        {
            Msg.MsgID = 4384;
            Msg.Param[0] = Army;
            Msg.Result = 0;
            Field.SendMapFieldMsg(ref Msg);
            Result = Msg.Result.Int == 0 && Army.FArmy.CanPlace(X, Y, Layer);
        }

        return Result;
    }

    // GlobalSpells.pas:660
    public int CalculateWarpLocation(TArmyHS Army)
    {
        TList Candidates;
        FixedArray_Aow_Units_FixedArray_sbyte_16_16 Table = default;
        TMapFieldMsg Msg = default;
        THNWalkData Walk = default;
        TMapLevel Level;
        TAoWMapField Field;
        int I;
        bool Found;
        Candidates = new TList();
        Army.FArmy.CreateMovePointTable(255, Bytes.Of(ref Table));
        var yhx = Army.Yhx;
        var xhx = Army.Xhx;
        InitHNWalk(ref Walk, xhx, yhx, 10);
        Level = AoWHSMap.MapContainer.MapLevels[Army.Lhx];
        while (Walk.Radius < 20)
        {
            if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < Level.Width && Walk.Y < Level.Height)
            {
                Field = (TAoWMapField)Level.GetField(Walk.X, Walk.Y);
                if (Table[Field.TerrainType][Field.OverlayType - -1] != -1)
                {
                    Candidates.Add(Field);
                }
            }

            WalkToNextHN(ref Walk);
        }

        for (int L = 0, last = AoWHSMap.MapContainer.LevelCount - 1; L <= last; L++)
        {
            if (Math.Abs(L - Army.Lhx) == 1)
            {
                Level = AoWHSMap.MapContainer.MapLevels[L];
                var armyYhx = Army.Yhx;
                var armyXhx = Army.Xhx;
                InitHNWalk(ref Walk, armyXhx, armyYhx, 0);
                while (Walk.Radius < 10)
                {
                    if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < Level.Width && Walk.Y < Level.Height)
                    {
                        Field = (TAoWMapField)Level.GetField(Walk.X, Walk.Y);
                        if (Table[Field.TerrainType][Field.OverlayType - -1] != -1)
                        {
                            Candidates.Add(Field);
                        }
                    }

                    WalkToNextHN(ref Walk);
                }
            }
        }

        Found = false;
        int Result = 16777215;
        while (Candidates.Count != 0 && !Found)
        {
            I = Random(Candidates.Count);
            Field = (TAoWMapField)Candidates[I];
            if (Field.GetArmyHS() == null && Field.GetPlayer() == -1)
            {
                Msg.MsgID = 4384;
                Msg.Param[0] = Army;
                Msg.Result = 0;
                Field.SendMapFieldMsg(ref Msg);
                Found = Msg.Result.Int == 0 && Army.FArmy.CanPlace(Field.Xhx, Field.Yhx, Field.Lhx);
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
        FSpellName = TranslateRStr(ResurrectHeroRStr);
        FSpellID = 66;
        FSpellType = TSpellType.stGlobal;
        FBaseManaCost = 0;
        FSpellLevel = 2;
        FMagicSphere = TMagicSphere.msCosmos;
    }

    // GlobalSpells.pas:991
    public override bool ExecuteTE(TSpellTE Event, TAbstractUnit Source, int ExecuteCount)
    {
        bool Result = default;
        sbyte X = default;
        sbyte Y = default;
        sbyte Layer = default;
        TTwinkleAnimation Animation = default;
        THeroList Heroes;
        THero Hero;
        TArmyHS Army;
        TLocationTextEventLog Log;
        THeroEmergeEventLog EmergeLog = default;
        TArmyInfoEventLog InfoLog = default;
        if (ExecuteCount == 0)
        {
            Source.GetLocation(ref X, ref Y, ref Layer);
            AoWHSMap.ValidateObservation(Source.FPlayer, X, Y, Layer, 0);
            Animation = new TTwinkleAnimation(null);
            Animation.Setup(FCastImages.GetImageSequence(0), TSet<TTwinkleModeElement>.Of(TTwinkleModeElement.tmRotate), 50, 64, 50, 30);
            AoWHSMap.PlaceHX(Animation, X, Y, Layer);
            if (Source.FPlayer == AoWHSMap.FSeatedPlayer || AoWHSMap.Watching(X, Y, Layer, 0))
            {
                FCastSounds.Play(0);
            }
        }

        if (ExecuteCount == 20)
        {
            Heroes = new THeroList(null);
            AoWHSMap.FHeroControl.ListDeadHeroes(Heroes);
            for (int I = Heroes.Count - 1; I >= 0; I--)
            {
                if (Heroes.GetHero(I) is TLeader || Heroes.GetHero(I).GetLevel() > AoWHSMap.FPlayers.GetPlayers(Source.FPlayer).FBaseHeroJoinLevel + 2)
                {
                    Heroes.RemoveChild(Heroes.GetHero(I));
                }
            }

            if (Heroes.Count != 0)
            {
                Source.GetLocation(ref X, ref Y, ref Layer);
                var count = Heroes.Count;
                Hero = Heroes.GetHero(AoWHSMap.Random(count));
                Hero.Resurrect();
                Army = Hero.Place(X, Y, Layer, 0, TSet<TUnitPlaceModeElement>.Of(TUnitPlaceModeElement.pmScanArea, TUnitPlaceModeElement.pmCreateNewArmy));
                Log = new TLocationTextEventLog(null);
                Log.FPlayer = Source.FPlayer;
                Log.FText = RtlUnit.Format(TranslateRStr(XResurrectedRStr), [Hero.FHeroName]);
                var lhx = Army.GetLhx();
                var yhx = Army.GetYhx();
                Log.SetLocation(Army.GetXhx(), yhx, lhx);
                var armyYhx = Army.GetYhx();
                var armyLhx = Army.GetLhx();
                var xhx = Army.GetXhx();
                DistributeLocationEventLog(Log, Source.FPlayer, xhx, armyYhx, armyLhx, false);
                Log.Release();
                if (Hero.OfferToJoin(Source.FPlayer))
                {
                    EmergeLog = new THeroEmergeEventLog(null);
                    EmergeLog.Setup(Hero, Source.FPlayer, THeroJoinMode.hjmResurrected);
                    EmergeLog.Execute(true, TExecuteTrigger.etEvent, null);
                    EmergeLog.Release();
                }
                else
                {
                    InfoLog = new TArmyInfoEventLog(null);
                    var indexOf = Army.FArmy.IndexOf(Hero);
                    InfoLog.FArmyView.Setup(Army.FArmy, (byte)(1 << indexOf));
                    InfoLog.FSelection = 255;
                    InfoLog.FPlayer = Source.FPlayer;
                    var noJoinResurrection = TranslateRStr(NoJoinResurrectionRStr);
                    InfoLog.FMessages.Text = noJoinResurrection;
                    InfoLog.FTriggers = TSet<TExecuteTrigger>.Of(TExecuteTrigger.etEvent);
                    InfoLog.Execute(true, TExecuteTrigger.etEvent, null);
                    InfoLog.Release();
                }
            }
            else
            {
                var noHeroAnswersYourCall = TranslateRStr(NoHeroAnswersYourCallRStr);
                AoWHSMap.ShowMessage(noHeroAnswersYourCall);
            }

            CastingDone(Source, Event.FEventMana);
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
    public override bool Activate(TAbstractUnit Source)
    {
        bool Result;
        string Reason = "";
        TSpellTE Event;
        base.Activate(Source);
        if (CanActivate(Source, ref Reason) && !AoWHSMap.FPlayers.GetPlayers(Source.FPlayer).GetBusy())
        {
            Event = new TSpellTE(null);
            Event.Initialize(FSpellID, (byte)Source.FPlayer, Source.FUnitID, FBaseManaCost);
            AoWHSMap.FTokenManager.GetTokenControl(Source.FPlayer).AddEvent(Event);
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
        FSpellName = TranslateRStr(AnimateHeroRStr);
        FSpellID = 67;
        FSpellType = TSpellType.stGlobal;
        FBaseManaCost = 0;
        FSpellLevel = 2;
        FMagicSphere = TMagicSphere.msCosmos;
    }

    // GlobalSpells.pas:1091
    public override bool ExecuteTE(TSpellTE Event, TAbstractUnit Source, int ExecuteCount)
    {
        bool Result = default;
        sbyte X = default;
        sbyte Y = default;
        sbyte Layer = default;
        TTwinkleAnimation Animation = default;
        THeroList Heroes;
        THero Hero;
        TArmyHS Army;
        TLocationTextEventLog Log;
        THeroEmergeEventLog EmergeLog = default;
        TArmyInfoEventLog InfoLog = default;
        if (ExecuteCount == 0)
        {
            Source.GetLocation(ref X, ref Y, ref Layer);
            AoWHSMap.ValidateObservation(Source.FPlayer, X, Y, Layer, 0);
            Animation = new TTwinkleAnimation(null);
            Animation.Setup(FCastImages.GetImageSequence(0), TSet<TTwinkleModeElement>.Of(TTwinkleModeElement.tmRotate), 50, 64, 50, 30);
            AoWHSMap.PlaceHX(Animation, X, Y, Layer);
            if (Source.FPlayer == AoWHSMap.FSeatedPlayer || AoWHSMap.Watching(X, Y, Layer, 0))
            {
                FCastSounds.Play(0);
            }
        }

        if (ExecuteCount == 20)
        {
            Heroes = new THeroList(null);
            AoWHSMap.FHeroControl.ListDeadHeroes(Heroes);
            for (int I = Heroes.Count - 1; I >= 0; I--)
            {
                if (Heroes.GetHero(I) is TLeader || Heroes.GetHero(I).GetLevel() > AoWHSMap.FPlayers.GetPlayers(Source.FPlayer).FBaseHeroJoinLevel + 2)
                {
                    Heroes.RemoveChild(Heroes.GetHero(I));
                }
            }

            if (Heroes.Count != 0)
            {
                Source.GetLocation(ref X, ref Y, ref Layer);
                var count = Heroes.Count;
                Hero = Heroes.GetHero(AoWHSMap.Random(count));
                Hero.Animate();
                Army = Hero.Place(X, Y, Layer, 0, TSet<TUnitPlaceModeElement>.Of(TUnitPlaceModeElement.pmScanArea, TUnitPlaceModeElement.pmCreateNewArmy));
                Log = new TLocationTextEventLog(null);
                Log.FPlayer = Source.FPlayer;
                Log.FText = RtlUnit.Format(TranslateRStr(XAnimatedRStr), [Hero.FHeroName]);
                var lhx = Army.GetLhx();
                var yhx = Army.GetYhx();
                Log.SetLocation(Army.GetXhx(), yhx, lhx);
                var armyYhx = Army.GetYhx();
                var armyLhx = Army.GetLhx();
                var xhx = Army.GetXhx();
                DistributeLocationEventLog(Log, Source.FPlayer, xhx, armyYhx, armyLhx, false);
                Log.Release();
                if (Hero.OfferToJoin(Source.FPlayer))
                {
                    EmergeLog = new THeroEmergeEventLog(null);
                    EmergeLog.Setup(Hero, Source.FPlayer, THeroJoinMode.hjmResurrected);
                    EmergeLog.Execute(true, TExecuteTrigger.etEvent, null);
                    EmergeLog.Release();
                }
                else
                {
                    InfoLog = new TArmyInfoEventLog(null);
                    var indexOf = Army.FArmy.IndexOf(Hero);
                    InfoLog.FArmyView.Setup(Army.FArmy, (byte)(1 << indexOf));
                    InfoLog.FSelection = 255;
                    InfoLog.FPlayer = Source.FPlayer;
                    var noJoinAnimate = TranslateRStr(NoJoinAnimateRStr);
                    InfoLog.FMessages.Text = noJoinAnimate;
                    InfoLog.FTriggers = TSet<TExecuteTrigger>.Of(TExecuteTrigger.etEvent);
                    InfoLog.Execute(true, TExecuteTrigger.etEvent, null);
                    InfoLog.Release();
                }
            }
            else
            {
                if (Source.FPlayer == AoWHSMap.FSeatedPlayer)
                {
                    var noHeroAnswersYourCall = TranslateRStr(NoHeroAnswersYourCallRStr);
                    AoWHSMap.ShowMessage(noHeroAnswersYourCall);
                    AoWHSSet.FSpellControl.GetSpell(FSpellID).FCastSounds.Play(0);
                }
            }

            CastingDone(Source, Event.FEventMana);
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
    public override bool Activate(TAbstractUnit Source)
    {
        bool Result;
        string Reason = "";
        TSpellTE Event;
        base.Activate(Source);
        if (CanActivate(Source, ref Reason) && !AoWHSMap.FPlayers.GetPlayers(Source.FPlayer).GetBusy())
        {
            Event = new TSpellTE(null);
            Event.Initialize(FSpellID, (byte)Source.FPlayer, Source.FUnitID, FBaseManaCost);
            AoWHSMap.FTokenManager.GetTokenControl(Source.FPlayer).AddEvent(Event);
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
        FSpellName = TranslateRStr(CallHeroRStr);
        FSpellID = 69;
        FSpellType = TSpellType.stGlobal;
        FBaseManaCost = 0;
        FSpellLevel = 2;
        FMagicSphere = TMagicSphere.msCosmos;
    }

    // GlobalSpells.pas:1250
    public override bool ExecuteTE(TSpellTE Event, TAbstractUnit Source, int ExecuteCount)
    {
        bool ShowSource;
        bool ShowTarget;
        sbyte X = default;
        sbyte Y = default;
        sbyte Layer = default;
        THero Hero;
        TArmyHS Army;
        TImageSequenceAnimation Animation;
        base.ExecuteTE(Event, Source, ExecuteCount);
        Source.GetLocation(ref X, ref Y, ref Layer);
        Hero = FindCallHero(Source);
        if (Hero != null)
        {
            if (Hero.Owner == null)
            {
                Hero.SetHitPoints(Hero.GetHits());
            }

            Army = Hero.Place(X, Y, Layer, 0, TSet<TUnitPlaceModeElement>.Of(TUnitPlaceModeElement.pmScanArea, TUnitPlaceModeElement.pmCreateNewArmy));
            ShowSource = AoWHSMap.FSeatedPlayer == Source.FPlayer || AoWHSMap.VisibleForPlayer(X, Y, Layer, AoWHSMap.FSeatedPlayer);
            bool condition = AoWHSMap.FSeatedPlayer == Source.FPlayer;
            if (!condition)
            {
                var lhx = Army.GetLhx();
                var yhx = Army.GetYhx();
                var xhx = Army.GetXhx();
                condition = AoWHSMap.VisibleForPlayer(xhx, yhx, lhx, AoWHSMap.FSeatedPlayer);
            }

            ShowTarget = condition;
            if (ShowSource)
            {
                if (AoWHSSet.FSpellControl.GetSpell(FSpellID).FCastImages.GetImageSequence(0) != null)
                {
                    Animation = new TImageSequenceAnimation(null);
                    Animation.SetImageSequence(AoWHSSet.FSpellControl.GetSpell(FSpellID).FCastImages.GetImageSequence(0));
                    AoWHSMap.PlaceHX(Animation, X, Y, Layer);
                }
            }

            if (ShowTarget)
            {
                if (AoWHSSet.FSpellControl.GetSpell(FSpellID).FCastImages.GetImageSequence(0) != null)
                {
                    Animation = new TImageSequenceAnimation(null);
                    Animation.SetImageSequence(AoWHSSet.FSpellControl.GetSpell(FSpellID).FCastImages.GetImageSequence(0));
                    var lhx = Army.GetLhx();
                    var yhx = Army.GetYhx();
                    var xhx = Army.GetXhx();
                    AoWHSMap.PlaceHX(Animation, xhx, yhx, lhx);
                }
            }

            if (AoWHSMap.FSeatedPlayer == Source.FPlayer)
            {
                var lhx = Army.GetLhx();
                var yhx = Army.GetYhx();
                var xhx = Army.GetXhx();
                AoWHSMap.CenterViewTo(xhx, yhx, lhx);
            }
            else
            {
                var lhx = Army.GetLhx();
                var yhx = Army.GetYhx();
                var xhx = Army.GetXhx();
                AoWHSMap.ValidateObservation(Source.FPlayer, xhx, yhx, lhx, 0);
            }

            bool test = AoWHSMap.FSeatedPlayer == Source.FPlayer || AoWHSMap.Watching(X, Y, Layer, 0);
            if (!test)
            {
                var lhx = Army.GetLhx();
                var yhx = Army.GetYhx();
                var xhx = Army.GetXhx();
                test = AoWHSMap.Watching(xhx, yhx, lhx, 0);
            }

            if (test)
            {
                AoWHSSet.FSpellControl.GetSpell(FSpellID).FCastSounds.Play(0);
            }
        }
        else
        {
            if (AoWHSMap.FSeatedPlayer == Source.FPlayer)
            {
                var noHeroAnsweredYourCall = TranslateRStr(NoHeroAnsweredYourCallRStr);
                AoWHSMap.ShowMessage(noHeroAnsweredYourCall);
                AoWHSSet.FSpellControl.GetSpell(FSpellID).FCastSounds.Play(0);
            }
        }

        var spell = AoWHSSet.FSpellControl.GetSpell(FSpellID);
        AoWHSSet.FSpellControl.GetSpell(FSpellID).CastingDone(Source, spell.FBaseManaCost);
        return false;
    }

    // GlobalSpells.pas:1195
    public override bool Activate(TAbstractUnit Source)
    {
        bool Result;
        string Reason = "";
        TSpellTE Event;
        if (CanActivate(Source, ref Reason) && !AoWHSMap.FPlayers.GetPlayers(Source.FPlayer).GetBusy())
        {
            Event = new TSpellTE(null);
            Event.Initialize(FSpellID, (byte)Source.FPlayer, Source.FUnitID, FBaseManaCost);
            AoWHSMap.FTokenManager.GetTokenControl(Source.FPlayer).AddEvent(Event);
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
        THero Result;
        THero Hero;
        TDoubleItemList<THero> Heroes;
        int Index;
        Heroes = new TDoubleItemList<THero>();
        for (int I = AoWHSMap.FHeroControl.GetCount() - 1; I >= 0; I--)
        {
            Hero = AoWHSMap.FHeroControl.GetHero(I);
            if (Hero.FPlayer == 0 && Hero.Owner is TArmy && ((TArmy)Hero.Owner).FX != -1)
            {
                if (Hero.OfferToJoin(Source.FPlayer))
                {
                    Heroes.AddAB(Hero, 10);
                }
                else
                {
                    Heroes.AddAB(Hero, 1);
                }
            }
        }

        if (Heroes.Count == 0)
        {
            for (int I = AoWHSMap.FHeroControl.GetCount() - 1; I >= 0; I--)
            {
                Hero = AoWHSMap.FHeroControl.GetHero(I);
                if (!Hero.FUnitStatus.Contains(TUnitStatusElement.usKilled) && Hero.FPlayer <= 0 && Hero.Owner == null)
                {
                    if (Hero.OfferToJoin(Source.FPlayer))
                    {
                        Heroes.AddAB(Hero, 10);
                    }
                    else
                    {
                        Heroes.AddAB(Hero, 1);
                    }
                }
            }
        }

        if (Heroes.Count != 0)
        {
            RandSeed = AoWHSMap.Random(16777215);
            Index = Heroes.RandomChanceItemB(0);
            Result = Heroes[Index].A;
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
    public override bool Validate(THero source)
    {
        int Index;
        int Mana;
        int RequiredMana;
        bool Result = base.Validate(source);
        Index = AoWHSMap.FGlobalMagicControl.IndexOfID(FEnchantmentID);
        Result = Result && Index != -1;
        if (Result)
        {
            var globalEnchantment = AoWHSMap.FGlobalMagicControl.FEnchantments.GetEnchantments(Index);
            Mana = AoWHSMap.FPlayers.GetPlayers(source.FPlayer).FMagicControl.FMana + source.FCastingProgress;
            RequiredMana = Round((double)((double)globalEnchantment.FDispelMana * 0.02) * FDispelChance) + FEventMana;
            Result = RequiredMana <= Mana;
        }

        return Result;
    }

    // GlobalSpells.pas:772
    public override bool Process(TAbstractUnit source, int value)
    {
        TGlobalEnchantment Enchantment;
        int Index;
        sbyte X = default;
        sbyte Y = default;
        sbyte Layer = default;
        TTwinkleAnimation Twinkle;
        int Mana;
        base.Process(source, value);
        Index = AoWHSMap.FGlobalMagicControl.IndexOfID(FEnchantmentID);
        Enchantment = AoWHSMap.FGlobalMagicControl.FEnchantments.GetEnchantments(Index);
        source.GetLocation(ref X, ref Y, ref Layer);
        AoWHSMap.ValidateObservation(source.FPlayer, X, Y, Layer, 0);
        Twinkle = new TTwinkleAnimation(null);
        Twinkle.Setup(AoWHSSet.FSpellControl.GetSpell(51).FCastImages.ImageSequences[0], TSet<TTwinkleModeElement>.Of(TTwinkleModeElement.tmExpand, TTwinkleModeElement.tmExplode), FEventMana + 50, 192, 50, 30);
        AoWHSMap.PlaceHX(Twinkle, X, Y, Layer);
        AoWHSSet.FSpellControl.GetSpell(FEventSpellID).FCastSounds.Play(0);
        Mana = Round((double)((double)Enchantment.FDispelMana * 0.02) * FDispelChance);
        var player = AoWHSMap.FPlayers.GetPlayers(source.FPlayer);
        AoWHSMap.FPlayers.GetPlayers(source.FPlayer).FMagicControl.SetMana(player.FMagicControl.FMana - Mana);
        if (AoWHSMap.Random(100) < FDispelChance)
        {
            if (AoWHSMap.FSeatedPlayer == source.FPlayer)
            {
                var name = Enchantment.GetName();
                var format = RtlUnit.Format(TranslateRStr(XSuccessfullyDispelledRStr), [name]);
                AoWHSMap.ShowMessage(format);
            }

            AoWHSMap.FGlobalMagicControl.ExecuteDispelEnchantment(source.FPlayer, FEnchantmentID);
        }
        else
        {
            if (AoWHSMap.FSeatedPlayer == source.FPlayer)
            {
                // { Original formats this message with an unused enchantment-name argument. }
                var name = Enchantment.GetName();
                var format = RtlUnit.Format(TranslateRStr(DispelMagicFailedRStr), [name]);
                AoWHSMap.ShowMessage(format);
            }
        }

        AoWHSSet.FSpellControl.GetSpell(FEventSpellID).CastingDone(source, FEventMana);
        return false;
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
    public override bool CanCast(ref string Reason)
    {
        // FPlayer is on TAbstractUnit. FCastingProgress is on THero, so only that read tests for a hero.
        TAbstractUnit Source;
        bool Result = base.CanCast(ref Reason);
        Source = AoWHSMap.FUnitControl.FindUnit(FCasterSourceID);
        if (Result && SelectedEnchantment() != null)
        {
            if (!AoWHSMap.FGlobalMagicControl.FActiveEnchantments.Contains(TGlobalEnchantmentType.getSpellWard) || SelectedEnchantment().FEnchantmentType == TGlobalEnchantmentType.getSpellWard)
            {
                if (!AoWHSMap.FPlayers.GetPlayers(Source.FPlayer).FMagicControl.GetCheatEnabled())
                {
                    if (Source is THero hero)
                    {
                        var spell = AoWHSSet.FSpellControl.GetSpell(51);
                        var mana = GetMana();
                        if (AoWHSMap.FPlayers.GetPlayers(Source.FPlayer).FMagicControl.FMana + hero.FCastingProgress >= spell.FBaseManaCost + mana)
                        {
                            Result = true;
                        }
                        else
                        {
                            Result = false;
                            Reason = TranslateRStr(NotEnoughManaRStr);
                        }
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
        bool Result;
        string Reason = "";
        TAbstractUnit Source;
        TDisjunctionTE Event;
        if (CanCast(ref Reason))
        {
            Source = AoWHSMap.FUnitControl.FindUnit(FCasterSourceID);
            if (!AoWHSMap.FPlayers.GetPlayers(Source.FPlayer).GetBusy())
            {
                Event = new TDisjunctionTE(null);
                var spell = AoWHSSet.FSpellControl.GetSpell(51);
                Event.Initialize(FCasterSpellID, (byte)Source.FPlayer, Source.FUnitID, spell.FBaseManaCost);
                Event.FEnchantmentID = FEnchantmentID;
                Event.FDispelChance = FDispelChance;
                AoWHSMap.FTokenManager.GetTokenControl(Source.FPlayer).AddEvent(Event);
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
    public void SetEnchantmentID(int Value)
    {
        FEnchantmentID = Value;
    }

    // GlobalSpells.pas:817
    public TGlobalEnchantment SelectedEnchantment()
    {
        TGlobalEnchantment Result;
        int Index;
        Index = AoWHSMap.FGlobalMagicControl.IndexOfID(FEnchantmentID);
        if (Index != -1)
        {
            Result = AoWHSMap.FGlobalMagicControl.FEnchantments.GetEnchantments(Index);
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
        int Result;
        TGlobalEnchantment Enchantment;
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
    public void SetDispelChance(int Value)
    {
        FDispelChance = Value;
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
    public int GetDispelChanceMax() => 75;

    // GlobalSpells.pas:851
    public bool GetEnoughMana()
    {
        // FPlayer is on TAbstractUnit. FCastingProgress is on THero, so only that read tests for a hero.
        TAbstractUnit Source;
        int Mana;
        bool Result = false;
        Source = AoWHSMap.FUnitControl.FindUnit(FCasterSourceID);
        if (SelectedEnchantment() != null)
        {
            if (!AoWHSMap.FPlayers.GetPlayers(Source.FPlayer).FMagicControl.GetCheatEnabled())
            {
                if (Source is THero hero)
                {
                    Mana = AoWHSMap.FPlayers.GetPlayers(Source.FPlayer).FMagicControl.FMana + hero.FCastingProgress;
                    var spell = AoWHSSet.FSpellControl.GetSpell(51);
                    var mana = GetMana();
                    if (Mana >= spell.FBaseManaCost + mana)
                    {
                        Result = true;
                    }
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
        FSpellName = TranslateRStr(DisjunctionRStr);
        FSpellID = 51;
        FSpellType = TSpellType.stGlobal;
        FBaseManaCost = 0;
        FSpellLevel = 2;
        FMagicSphere = TMagicSphere.msCosmos;
    }

    // GlobalSpells.pas:942
    public override bool CanActivate(TAbstractUnit Source, ref string MessageText)
    {
        bool Result;
        int Count;
        string Reason = "";
        Count = 0;
        for (int I = 0, last = AoWHSMap.FGlobalMagicControl.FEnchantments.Count - 1; I <= last; I++)
        {
            if (AoWHSMap.FGlobalMagicControl.FEnchantments.GetEnchantments(I).FPlayerIndex != Source.FPlayer && AoWHSMap.FGlobalMagicControl.FEnchantments.GetEnchantments(I).VisibleForPlayer(Source.FPlayer) && AoWHSMap.FGlobalMagicControl.FEnchantments.GetEnchantments(I).CanDispel(Source.FPlayer, ref Reason))
            {
                Count += 1;
            }
        }

        if (Count == 0)
        {
            MessageText = TranslateRStr(NoGlobalOpponentEnchantmentsActiveRStr);
            Result = false;
        }
        else
        {
            Result = true;
        }

        return Result;
    }

    // GlobalSpells.pas:962
    public override bool Activate(TAbstractUnit Source)
    {
        bool Result;
        string Reason = "";
        TDisjunctionSpellCaster Caster;
        if (CanActivate(Source, ref Reason))
        {
            Caster = new TDisjunctionSpellCaster(null);
            Caster.FCasterSpellID = FSpellID;
            Caster.FCasterSourceID = Source.FUnitID;
            AoWHSMap.FCosmeticSurgeryEventsStore.Raise(Caster);
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
        FSpellName = TranslateRStr(CrusadeRStr);
        FSpellID = 75;
        FSpellType = TSpellType.stGlobal;
        FBaseManaCost = 0;
        FSpellLevel = 4;
        FMagicSphere = TMagicSphere.msLife;
    }

    // GlobalSpells.pas:1340
    public override bool ExecuteTE(TSpellTE Event, TAbstractUnit Source, int ExecuteCount)
    {
        int Count;
        int UnitIndex;
        int Xhp = default;
        int Yhp = default;
        sbyte X = default;
        sbyte Y = default;
        sbyte Layer = default;
        TArmyHS Party;
        TUnit NewUnit;
        if (ExecuteCount == 0)
        {
            Source.GetLocation(ref X, ref Y, ref Layer);
            AoWHSMap.ValidateObservation(Source.FPlayer, X, Y, Layer, 0);
            HXtoHP(X, Y, ref Xhp, ref Yhp);
            var worldFXHS = AoWHSMap.AddEffect(TworldFXType.fxCrusade);
            worldFXHS.FX = Xhp + 16;
            worldFXHS.FY = Yhp + 16;
            worldFXHS.FLayer = Layer;

            if (AoWHSMap.Random(100) < 20)
            {
                Party = new TArmyHS(null);
                Party.FArmy.SetPlayer(0);
                Count = 1;
                UnitIndex = AoWHSMap.Random(1);
                for (int J = 0; J <= Count - 1; J++)
                {
                    NewUnit = new TUnit(null);
                    NewUnit.SetUnitResource(AoWHSSet.FUnitResources.GetUnitResource(GlobalSpellsUnit.TCrusade_ExecuteTE__LargeUnits[UnitIndex]));
                    NewUnit.ExpandAbility(101);
                    Party.FArmy.AddUnit(NewUnit);
                    NewUnit.Release();
                }

                if (!PlaceParty(Source, Party))
                {
                    Party.Free();
                }
            }
            else
            {
                for (int I = 0; I <= 1; I++)
                {
                    Party = new TArmyHS(null);
                    Party.FArmy.SetPlayer(0);
                    Count = AoWHSMap.Random(3) + 4;
                    for (int J = 0; J <= Count - 1; J++)
                    {
                        NewUnit = new TUnit(null);
                        UnitIndex = AoWHSMap.Random(2);
                        NewUnit.SetUnitResource(AoWHSSet.FUnitResources.GetUnitResource(GlobalSpellsUnit.TCrusade_ExecuteTE__FootUnits[UnitIndex]));
                        NewUnit.ExpandAbility(101);
                        Party.FArmy.AddUnit(NewUnit);
                        NewUnit.Release();
                    }

                    if (!PlaceParty(Source, Party))
                    {
                        Party.Free();
                    }
                }

                Party = new TArmyHS(null);
                Party.FArmy.SetPlayer(0);
                UnitIndex = AoWHSMap.Random(3);
                Count = 2;
                if (UnitIndex == 2)
                {
                    Count += 1;
                }

                for (int J = 0; J <= Count - 1; J++)
                {
                    NewUnit = new TUnit(null);
                    NewUnit.SetUnitResource(AoWHSSet.FUnitResources.GetUnitResource(GlobalSpellsUnit.TCrusade_ExecuteTE__MountedUnits[UnitIndex]));
                    NewUnit.ExpandAbility(101);
                    Party.FArmy.AddUnit(NewUnit);
                    NewUnit.Release();
                }

                if (!PlaceParty(Source, Party))
                {
                    Party.Free();
                }
            }

            AoWHSSet.FSpellControl.GetSpell(FSpellID).CastingDone(Source, FBaseManaCost);
            AoWHSSet.FSpellControl.GetSpell(FSpellID).FCastSounds.Play(0);
        }

        return false;
    }

    // GlobalSpells.pas:1417
    public override bool Activate(TAbstractUnit Source)
    {
        bool Result;
        string Reason = "";
        TSpellTE Event;
        base.Activate(Source);
        if (CanActivate(Source, ref Reason) && !AoWHSMap.FPlayers.GetPlayers(Source.FPlayer).GetBusy())
        {
            Event = new TSpellTE(null);
            Event.Initialize(FSpellID, (byte)Source.FPlayer, Source.FUnitID, FBaseManaCost);
            AoWHSMap.FTokenManager.GetTokenControl(Source.FPlayer).AddEvent(Event);
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
        bool Result;
        int TargetX = default;
        int TargetY = default;
        sbyte X = default;
        sbyte Y = default;
        sbyte Layer = default;
        TImageSequenceAnimation Animation;
        Source.GetLocation(ref X, ref Y, ref Layer);
        var radToHN = RadToHN(5);
        var random = AoWHSMap.Random(radToHN);
        CenterHNtoHX(X, Y, random, ref TargetX, ref TargetY);
        if (Party.PlaceEx((sbyte)TargetX, (sbyte)TargetY, Layer, TSet<TArmyPlaceModeElement>.Of(TArmyPlaceModeElement.apSafe, TArmyPlaceModeElement.apScanArea)))
        {
            Party.Activate();
            Party.FArmy.SetBehavior(255, TGroupBehavior.gbSuicidal);
            var lhx = Party.GetLhx();
            var yhx = Party.GetYhx();
            var xhx = Party.GetXhx();
            if (AoWHSMap.Watching(xhx, yhx, lhx, 0))
            {
                if (AoWHSSet.FSpellControl.GetSpell(34).FCastImages.GetImageSequence(0) != null)
                {
                    // { Original uses Warp Party's images for Crusade parties. }
                    Animation = new TImageSequenceAnimation(null);
                    Animation.SetImageSequence(AoWHSSet.FSpellControl.GetSpell(34).FCastImages.GetImageSequence(0));
                    Animation.FAnimationLevel = 28;
                    var partyLhx = Party.GetLhx();
                    var partyYhx = Party.GetYhx();
                    var partyXhx = Party.GetXhx();
                    AoWHSMap.PlaceHX(Animation, partyXhx, partyYhx, partyLhx);
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

public static partial class GlobalSpellsUnit
{
    public static FixedArray_int_1 TCrusade_ExecuteTE__LargeUnits = FixedArray_int_1.From([133]);
    public static FixedArray_int_3 TCrusade_ExecuteTE__MountedUnits = FixedArray_int_3.From([132, 134, 128]);
    public static FixedArray_int_2 TCrusade_ExecuteTE__FootUnits = FixedArray_int_2.From([129, 126]);

    // GlobalSpells.pas:1435
    public static void RegisterGlobalSpells(TAoWEngine Engine)
    {
        TSpellControl Control;
        Control = Engine.FGameSet.FSpellControl;
        Engine.RegisterEClasses([EClass.Of<TWarpPartyTE>(), EClass.Of<TDisjunctionTE>()]);
        Control.RegisterSpell(new TConcealArea(null));
        Control.RegisterSpell(new TBirdsView(null));
        Control.RegisterSpell(new TWarpParty(null));
        Control.RegisterSpell(new TDisjunction(null));
        Control.RegisterSpell(new TResurrectHero(null));
        Control.RegisterSpell(new TAnimateHero(null));
        Control.RegisterSpell(new TCallHero(null));
        Control.RegisterSpell(new TCrusade(null));
    }

    // GlobalSpells.pas:1451
    public static void UnregisterGlobalSpells(TAoWEngine Engine)
    {
        TSpellControl Control;
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
