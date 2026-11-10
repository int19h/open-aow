// Translated from GlobalSpells.pas.
using System.Runtime.InteropServices;
using Aow.Engine.Buffers;
using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Game.Ai.Groups;
using Aow.Game.Commands;
using Aow.Game.Draw;
using Aow.Game.Draw.Animations;
using Aow.Game.Engine;
using Aow.Game.Engine.Collections;
using Aow.Game.Engine.Nodes;
using Aow.Game.Events;
using Aow.Game.Magic;
using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.Magic.Spells.Global;
using Aow.Game.Session;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw
{

    // X/Y/layer explored hexagons, animation frame
    public partial class ConcealAreaHS : FixedVisibleHS
    {
        public sbyte FX;
        public sbyte FY;
        public sbyte FLayer;
        public IntegerList FHexagons;
        public int FFrame;

        // GlobalSpells.pas:205
        public override void Dispose()
        {
            FHexagons?.Dispose();
            base.Dispose();
        }

        // GlobalSpells.pas:199
        public ConcealAreaHS(ECustomNode owner) : base(owner)
        {
            FHexagons = new IntegerList();
        }

        // GlobalSpells.pas:211
        public override int GetShowPriority() => 2013265920;

        // GlobalSpells.pas:244
        public override void Show(int XhpSurf, int YhpSurf, in SurfaceDesc SurfaceDesc)
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
            if (AoWHSMap.Current.ViewL == FLayer)
            {
                FFrame += 1;
                if (GameTables.AoWEngine.FAnimationDetail == 2 && AoWHSMap.Current.ViewL == FLayer)
                {
                    Intensity = 60 - Math.Abs(FFrame * 3 - 60);
                    HexGeometry.HXtoHP(FX, FY, ref Xhp, ref Yhp);
                    Xhp = Xhp - XhpSurf - 8;
                    Yhp -= YhpSurf;
                    for (int I = FHexagons.Count - 1; I >= 0; I--)
                    {
                        HexGeometry.HNtoHP(FHexagons[I], ref HX, ref HY);
                        HX += Xhp;
                        HY += Yhp;
                        AoWHSSet.Current.FGeneralImages[5].ShowBlended(HX, HY, SurfaceDesc, GfxState.AlphaBlendTable.LookupTables[Intensity]);
                    }
                }

                // { Original bug: Xhp/Yhp are uninitialized outside high detail. }
                RandSeed = AoWHSMap.Current.FInitialRandomSeed;
                for (int I = 0; I <= 200; I++)
                {
                    Radius = Random(448);
                    Direction = Random(360);
                    Delay = Random(20);
                    if (FFrame - Delay >= 0)
                    {
                        AbilityRules.DegreesRadToCo(Direction, Radius, ref X, ref Y);
                        AoWHSSet.Current.FSpellControl.GetSpell(57).FCastImages.Show(0, Xhp + X, Yhp + Y, FFrame - Delay, SurfaceDesc);
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
            HNWalkData Walk = default;
            MapLevel Level;
            FX = X;
            FY = Y;
            FLayer = Layer;
            FFrame = 0;
            AoWHSSet.Current.FSpellControl.GetSpell(1).FCastSounds.Play(0);
            AoWHSMap.Current.DynamicFixedVisibleHS.AddChild(this);
            HNWalkData.InitHNWalk(ref Walk, FX, FY, 0);
            Level = AoWHSMap.Current.MapContainer.MapLevels[FLayer];
            while (Walk.Radius <= Radius)
            {
                if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < Level.Width && Walk.Y < Level.Height)
                {
                    if (((AoWMapField)Level.GetField(Walk.X, Walk.Y)).GetExplored(AoWHSMap.Current.FSeatedPlayer))
                    {
                        FHexagons.Add(Walk.HN);
                    }
                }

                HNWalkData.WalkToNextHN(ref Walk);
            }
        }

        // GlobalSpells.pas:237
        public void StopAnimation()
        {
            AoWHSMap.Current.DynamicFixedVisibleHS.RemoveChild(this);
            AoWHSMap.Current.DynamicVisibleHS.RemoveChild(this);
            Dispose();
        }

        // GlobalSpells.pas:39

    }
}

namespace Aow.Game.Magic.Spells
{

    // inherited name, spell ID, mana, sphere, level, type
    public partial class ConcealArea : Spell
    {
        // GlobalSpells.pas:286
        public ConcealArea(ECustomNode owner) : base(owner)
        {
            FSpellName = AbilityRules.TranslateRStr(ConcealAreaRStr);
            FSpellID = 1;
            FSpellType = SpellType.Global;
            FBaseManaCost = 5;
            FSpellLevel = 1;
            FMagicSphere = MagicSphere.Earth;
        }

        // GlobalSpells.pas:302
        public override bool ExecuteTE(SpellTE Event, AbstractUnit Source, int ExecuteCount)
        {
            AoWMapField MapF;
            sbyte X = default;
            sbyte Y = default;
            sbyte Layer = default;
            HNWalkData Walk = default;
            MapLevel Level;
            LocationTextEventLog Log;
            ConcealAreaHS Conceal;
            base.ExecuteTE(Event, Source, ExecuteCount);
            CastingDone(Source, Event.FEventMana);
            Source.GetLocation(ref X, ref Y, ref Layer);
            if (Source.FPlayer == AoWHSMap.Current.FSeatedPlayer)
            {
                Conceal = new ConcealAreaHS(null);
                Conceal.StartAnimation(X, Y, Layer, 14);
            }

            for (int Player = 1, last = AoWHSMap.Current.FPlayers.Count - 1; Player <= last; Player++)
            {
                Level = AoWHSMap.Current.MapContainer.MapLevels[Layer];
                if (AoWHSMap.Current.FPlayers.GetDiplomaticRelation(Source.FPlayer, (sbyte)Player) == DiplomaticRelation.War && Player != Source.FPlayer)
                {
                    Log = null;
                    HNWalkData.InitHNWalk(ref Walk, X, Y, 0);
                    while (Walk.Radius <= 14)
                    {
                        if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < Level.Width && Walk.Y < Level.Height)
                        {
                            MapF = (AoWMapField)Level.GetField(Walk.X, Walk.Y);
                            if (MapF.GetExplored(Player) && !AoWHSMap.Current.VisibleForPlayer(Walk.X, Walk.Y, Layer, (sbyte)Player))
                            {
                                MapF.SetExplored(Player, false);
                                if (Log == null)
                                {
                                    Log = new LocationTextEventLog(null);
                                    Log.FXhx = (sbyte)Walk.X;
                                    Log.FYhx = (sbyte)Walk.Y;
                                    Log.FLhx = Layer;
                                    Log.FPlayer = Source.FPlayer;
                                    var concealArea = AbilityRules.TranslateRStr(ConcealAreaRStr);
                                    Log.FText = RtlUnit.Format(AbilityRules.TranslateRStr(GameText.XCastRStr), [concealArea]);
                                    AoWHSMap.Current.FPlayers.GetPlayers(Player).FEventLogbook.AddEvent(Log);
                                    Log.Release();
                                }
                            }
                        }

                        HNWalkData.WalkToNextHN(ref Walk);
                    }
                }
                else
                {
                    if (AoWHSMap.Current.VisibleForPlayer(X, Y, Layer, (sbyte)Player))
                    {
                        Log = new LocationTextEventLog(null);
                        Log.FXhx = X;
                        Log.FYhx = Y;
                        Log.FLhx = Layer;
                        Log.FPlayer = Source.FPlayer;
                        var concealArea = AbilityRules.TranslateRStr(ConcealAreaRStr);
                        Log.FText = RtlUnit.Format(AbilityRules.TranslateRStr(GameText.XCastRStr), [concealArea]);
                        AoWHSMap.Current.FPlayers.GetPlayers(Player).FEventLogbook.AddEvent(Log);
                        Log.Release();
                    }
                }
            }

            return false;
        }

        // GlobalSpells.pas:297
        public override bool GetEnabled() => AoWHSMap.Current.FExplorationEnabled;

        // GlobalSpells.pas:368
        public override bool CanActivate(AbstractUnit Source, ref string MessageText)
        {
            bool War;
            bool Result = base.CanActivate(Source, ref MessageText);
            if (Result)
            {
                War = false;
                for (int Player = 1, last = AoWHSMap.Current.FPlayers.Count - 1; Player <= last; Player++)
                {
                    War = War || AoWHSMap.Current.FPlayers.GetPlayers(Source.FPlayer).FDiplomaticRelations.GetDiplomaticRelation((sbyte)Player) == DiplomaticRelation.War;
                }

                if (!War)
                {
                    var concealArea = AbilityRules.TranslateRStr(ConcealAreaRStr);
                    MessageText = RtlUnit.Format(AbilityRules.TranslateRStr(AWarRelationIsRequiredForXRStr), [concealArea]);
                    Result = false;
                }
            }

            return Result;
        }

        // GlobalSpells.pas:388
        public override bool Activate(AbstractUnit Source)
        {
            bool Result;
            string Reason = "";
            SpellTE Event;
            base.Activate(Source);
            if (CanActivate(Source, ref Reason) && !AoWHSMap.Current.FPlayers.GetPlayers(Source.FPlayer).GetBusy())
            {
                Event = new SpellTE(null);
                Event.Initialize(FSpellID, (byte)Source.FPlayer, Source.FUnitID, FBaseManaCost);
                AoWHSMap.Current.FTokenManager.GetTokenControl(Source.FPlayer).AddEvent(Event);
                Event.Release();
                Result = true;
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        private static readonly string AWarRelationIsRequiredForXRStr = "A state of WAR is required for %s";
        private static readonly string ConcealAreaRStr = "Conceal Area";
    }
}

namespace Aow.Game.Draw
{

    // X/Y/layer unexplored hexagons, animation frame
    public partial class BirdsViewHS : FixedVisibleHS
    {
        public sbyte FX;
        public sbyte FY;
        public sbyte FLayer;
        public IntegerList FHexagons;
        public int FFrame;

        // GlobalSpells.pas:412
        public override void Dispose()
        {
            FHexagons?.Dispose();
            base.Dispose();
        }

        // GlobalSpells.pas:406
        public BirdsViewHS(ECustomNode owner) : base(owner)
        {
            FHexagons = new IntegerList();
        }

        // GlobalSpells.pas:418
        public override int GetShowPriority() => 2013265920;

        // GlobalSpells.pas:451
        public override void Show(int XhpSurf, int YhpSurf, in SurfaceDesc SurfaceDesc)
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
            if (AoWHSMap.Current.ViewL == FLayer)
            {
                FFrame += 1;
                Intensity = 100 - FFrame * 2;
                HexGeometry.HXtoHP(FX, FY, ref Xhp, ref Yhp);
                Xhp = Xhp - XhpSurf - 8;
                Yhp -= YhpSurf;
                for (int I = 0, last = FHexagons.Count - 1; I <= last; I++)
                {
                    HexGeometry.HNtoHP(FHexagons[I], ref HX, ref HY);
                    HX += Xhp;
                    HY += Yhp;
                    AoWHSSet.Current.FGeneralImages[5].ShowBlended(HX, HY, SurfaceDesc, GfxState.AlphaBlendTable.LookupTables[Intensity]);
                }

                RandSeed = AoWHSMap.Current.FInitialRandomSeed;
                for (int I = 0; I <= 200; I++)
                {
                    Radius = Random(448);
                    Direction = Random(360);
                    Delay = Random(30);
                    if (FFrame - Delay >= 0)
                    {
                        AbilityRules.DegreesRadToCo(Direction, Radius, ref X, ref Y);
                        AoWHSSet.Current.FSpellControl.GetSpell(57).FCastImages.Show(0, Xhp + X, Yhp + Y, FFrame - Delay, SurfaceDesc);
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
            HNWalkData Walk = default;
            MapLevel Level;
            FX = X;
            FY = Y;
            FLayer = Layer;
            FFrame = 0;
            AoWHSSet.Current.FSpellControl.GetSpell(2).FCastSounds.Play(0);
            HNWalkData.InitHNWalk(ref Walk, X, Y, 0);
            Level = AoWHSMap.Current.MapContainer.MapLevels[Layer];
            while (Walk.Radius <= Radius)
            {
                if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < Level.Width && Walk.Y < Level.Height)
                {
                    if (!((AoWMapField)Level.GetField(Walk.X, Walk.Y)).GetExplored(AoWHSMap.Current.FSeatedPlayer))
                    {
                        FHexagons.Add(Walk.HN);
                    }
                }

                HNWalkData.WalkToNextHN(ref Walk);
            }

            AoWHSMap.Current.DynamicFixedVisibleHS.AddChild(this);
        }

        // GlobalSpells.pas:444
        public void StopAnimation()
        {
            AoWHSMap.Current.DynamicFixedVisibleHS.RemoveChild(this);
            AoWHSMap.Current.DynamicVisibleHS.RemoveChild(this);
            Dispose();
        }

        // GlobalSpells.pas:67

    }
}

namespace Aow.Game.Magic.Spells
{

    // inherited name, spell ID, mana, sphere, level, spell type
    public partial class BirdsView : Spell
    {
        // GlobalSpells.pas:489
        public BirdsView(ECustomNode owner) : base(owner)
        {
            FSpellName = AbilityRules.TranslateRStr(BirdsViewRStr);
            FSpellID = 2;
            FSpellType = SpellType.Global;
            FBaseManaCost = 5;
            FSpellLevel = 1;
            FMagicSphere = MagicSphere.Air;
        }

        // GlobalSpells.pas:505
        public override bool ExecuteTE(SpellTE Event, AbstractUnit Source, int ExecuteCount)
        {
            sbyte X = default;
            sbyte Y = default;
            sbyte Layer = default;
            base.ExecuteTE(Event, Source, ExecuteCount);
            CastingDone(Source, Event.FEventMana);
            Source.GetLocation(ref X, ref Y, ref Layer);
            if (AoWHSMap.Current.FSeatedPlayer == Source.FPlayer)
            {
                var birdsViewHS = new BirdsViewHS(null);
                birdsViewHS.StartAnimation(X, Y, Layer, 14);
            }

            AoWHSMap.Current.ExploreArea(X, Y, Layer, Source.FPlayer, 14);
            return false;
        }

        // GlobalSpells.pas:500
        public override bool GetEnabled() => AoWHSMap.Current.FExplorationEnabled;

        // GlobalSpells.pas:518
        public override bool CanActivate(AbstractUnit Source, ref string MessageText)
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
                    MessageText = AbilityRules.TranslateRStr(GameText.CannotCastSpellInUndergoundRStr);
                    Result = false;
                }
            }

            return Result;
        }

        // GlobalSpells.pas:534
        public override bool Activate(AbstractUnit Source)
        {
            bool Result;
            string Reason = "";
            SpellTE Event;
            base.Activate(Source);
            if (CanActivate(Source, ref Reason) && !AoWHSMap.Current.FPlayers.GetPlayers(Source.FPlayer).GetBusy())
            {
                Event = new SpellTE(null);
                Event.Initialize(FSpellID, (byte)Source.FPlayer, Source.FUnitID, FBaseManaCost);
                AoWHSMap.Current.FTokenManager.GetTokenControl(Source.FPlayer).AddEvent(Event);
                Event.Release();
                Result = true;
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        private static readonly string BirdsViewRStr = "Bird's View";
    }
}

namespace Aow.Game.Commands
{

    // army ID, packed destination
    public partial class WarpPartyTE : SpellTE
    {
        // GlobalSpells.pas:564
        public new const int ClassID = 135948;

        public ArmyID FArmyID;
        public XYL FWarpLocation;

        // GlobalSpells.pas:558
        public override void Dispose()
        {
            FArmyID?.Dispose();
            base.Dispose();
        }

        // GlobalSpells.pas:569
        public override void ReadWrite(EStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwInteger(ref FWarpLocation.Packed, 30);
            Stream.rwCreateEObject(ref FArmyID, null, 31);
        }

        // GlobalSpells.pas:552
        public WarpPartyTE(ECustomNode owner) : base(owner)
        {
            FArmyID = null;
        }

        // GlobalSpells.pas:576
        public override bool Validate(Hero source)
        {
            byte Selection = default;
            ArmyHS Army = default;
            bool Result = base.Validate(source) && FArmyID.Valid();
            if (Result)
            {
                FArmyID.RetrieveArmy(ref Army, ref Selection);
                Result = ((WarpParty)AoWHSSet.Current.FSpellControl.GetSpell(FEventSpellID)).ValidTarget(Army, FWarpLocation.X, FWarpLocation.Y, FWarpLocation.L);
            }

            return Result;
        }

        // GlobalSpells.pas:590
        public override bool Process(AbstractUnit source, int value)
        {
            bool ShowSource;
            bool ShowTarget;
            byte Selection = default;
            sbyte X = default;
            sbyte Y = default;
            sbyte Layer = default;
            ArmyHS Army = default;
            MapField Field;
            ImageSequenceAnimation Animation;
            base.Process(source, value);
            source.GetLocation(ref X, ref Y, ref Layer);
            FArmyID.RetrieveArmy(ref Army, ref Selection);
            Field = AoWHSMap.Current.MapContainer.GetFieldXYL(FWarpLocation.Packed);
            Army.MoveTo(Field, 0);
            ShowSource = AoWHSMap.Current.FSeatedPlayer == source.FPlayer || AoWHSMap.Current.VisibleForPlayer(X, Y, Layer, AoWHSMap.Current.FSeatedPlayer);
            ShowTarget = AoWHSMap.Current.FSeatedPlayer == source.FPlayer || AoWHSMap.Current.VisibleForPlayer(Field.Xhx, Field.Yhx, Field.Lhx, AoWHSMap.Current.FSeatedPlayer);
            if (ShowSource && AoWHSSet.Current.FSpellControl.GetSpell(FEventSpellID).FCastImages.ImageSequences[0] != null)
            {
                Animation = new ImageSequenceAnimation(null);
                Animation.SetImageSequence(AoWHSSet.Current.FSpellControl.GetSpell(FEventSpellID).FCastImages.ImageSequences[0]);
                AoWHSMap.Current.PlaceHX(Animation, X, Y, Layer);
            }

            if (ShowTarget && AoWHSSet.Current.FSpellControl.GetSpell(FEventSpellID).FCastImages.ImageSequences[0] != null)
            {
                Animation = new ImageSequenceAnimation(null);
                Animation.SetImageSequence(AoWHSSet.Current.FSpellControl.GetSpell(FEventSpellID).FCastImages.ImageSequences[0]);
                AoWHSMap.Current.PlaceHX(Animation, Field.Xhx, Field.Yhx, Field.Lhx);
            }

            if (source.FPlayer == AoWHSMap.Current.FSeatedPlayer)
            {
                AoWHSMap.Current.CenterViewTo(Field.Xhx, Field.Yhx, Field.Lhx);
            }
            else
            {
                AoWHSMap.Current.ValidateObservation(source.FPlayer, Field.Xhx, Field.Yhx, Field.Lhx, 0);
            }

            if (source.FPlayer == AoWHSMap.Current.FSeatedPlayer || AoWHSMap.Current.Watching(X, Y, Layer, 0) || AoWHSMap.Current.Watching(Field.Xhx, Field.Yhx, Field.Lhx, 0))
            {
                AoWHSSet.Current.FSpellControl.GetSpell(FEventSpellID).FCastSounds.Play(0);
            }

            var spell = AoWHSSet.Current.FSpellControl.GetSpell(FEventSpellID);
            AoWHSSet.Current.FSpellControl.GetSpell(FEventSpellID).CastingDone(source, spell.FBaseManaCost);
            return false;
        }

        // GlobalSpells.pas:92

    }
}

namespace Aow.Game.Magic.Spells
{

    // inherited name, ID, mana, sphere, level, spell type
    public partial class WarpParty : Spell
    {
        // GlobalSpells.pas:632
        public WarpParty(ECustomNode owner) : base(owner)
        {
            FSpellName = AbilityRules.TranslateRStr(WarpPartyRStr);
            FSpellID = 34;
            FSpellType = SpellType.Global;
            FBaseManaCost = 1;
            FSpellLevel = 2;
            FMagicSphere = MagicSphere.Fire;
        }

        // GlobalSpells.pas:719
        public override bool Activate(AbstractUnit Source)
        {
            bool Result;
            string Reason = "";
            WarpPartyTE Event;
            base.Activate(Source);
            if (CanActivate(Source, ref Reason) && !AoWHSMap.Current.FPlayers.GetPlayers(Source.FPlayer).GetBusy())
            {
                if (Source.Owner is HeroList)
                {
                    return false;
                }

                Event = new WarpPartyTE(null);
                Event.Initialize(FSpellID, (byte)Source.FPlayer, Source.FUnitID, FBaseManaCost);
                Event.FArmyID = Source.ArmyOwner.CreateID(255);
                var warpLocation = CalculateWarpLocation(Source.ArmyOwner.GetArmyHS());
                Event.FWarpLocation.Packed = warpLocation;
                AoWHSMap.Current.FSelectedArmy.Unselect();
                if (Event.FWarpLocation.Packed != 16777215)
                {
                    AoWHSMap.Current.FTokenManager.GetTokenControl(Source.FPlayer).AddEvent(Event);
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
        public bool ValidTarget(ArmyHS Army, sbyte X, sbyte Y, sbyte Layer)
        {
            AoWMapField Field;
            MapFieldMsg Msg = default;
            Field = (AoWMapField)AoWHSMap.Current.MapContainer.GetField(X, Y, Layer);
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
        public int CalculateWarpLocation(ArmyHS Army)
        {
            DelphiList Candidates;
            FixedArray_Aow_Units_FixedArray_sbyte_16_16 Table = default;
            MapFieldMsg Msg = default;
            HNWalkData Walk = default;
            MapLevel Level;
            AoWMapField Field;
            int I;
            bool Found;
            Candidates = new DelphiList();
            Army.FArmy.CreateMovePointTable(255, Bytes.Of(ref Table));
            var yhx = Army.Yhx;
            var xhx = Army.Xhx;
            HNWalkData.InitHNWalk(ref Walk, xhx, yhx, 10);
            Level = AoWHSMap.Current.MapContainer.MapLevels[Army.Lhx];
            while (Walk.Radius < 20)
            {
                if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < Level.Width && Walk.Y < Level.Height)
                {
                    Field = (AoWMapField)Level.GetField(Walk.X, Walk.Y);
                    if (Table[Field.TerrainType][Field.OverlayType - -1] != -1)
                    {
                        Candidates.Add(Field);
                    }
                }

                HNWalkData.WalkToNextHN(ref Walk);
            }

            for (int L = 0, last = AoWHSMap.Current.MapContainer.LevelCount - 1; L <= last; L++)
            {
                if (Math.Abs(L - Army.Lhx) == 1)
                {
                    Level = AoWHSMap.Current.MapContainer.MapLevels[L];
                    var armyYhx = Army.Yhx;
                    var armyXhx = Army.Xhx;
                    HNWalkData.InitHNWalk(ref Walk, armyXhx, armyYhx, 0);
                    while (Walk.Radius < 10)
                    {
                        if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < Level.Width && Walk.Y < Level.Height)
                        {
                            Field = (AoWMapField)Level.GetField(Walk.X, Walk.Y);
                            if (Table[Field.TerrainType][Field.OverlayType - -1] != -1)
                            {
                                Candidates.Add(Field);
                            }
                        }

                        HNWalkData.WalkToNextHN(ref Walk);
                    }
                }
            }

            Found = false;
            int Result = 16777215;
            while (Candidates.Count != 0 && !Found)
            {
                I = Random(Candidates.Count);
                Field = (AoWMapField)Candidates[I];
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

            Candidates?.Dispose();
            return Result;
        }

        private static readonly string WarpPartyRStr = "Warp Party";
    }

    // inherited name, spell ID, mana, sphere, level, type, sounds, images
    public partial class ResurrectHero : Spell
    {
        // GlobalSpells.pas:980
        public ResurrectHero(ECustomNode owner) : base(owner)
        {
            FSpellName = AbilityRules.TranslateRStr(ResurrectHeroRStr);
            FSpellID = 66;
            FSpellType = SpellType.Global;
            FBaseManaCost = 0;
            FSpellLevel = 2;
            FMagicSphere = MagicSphere.Cosmos;
        }

        // GlobalSpells.pas:991
        public override bool ExecuteTE(SpellTE Event, AbstractUnit Source, int ExecuteCount)
        {
            bool Result = default;
            sbyte X = default;
            sbyte Y = default;
            sbyte Layer = default;
            TwinkleAnimation Animation = default;
            HeroList Heroes;
            Hero Hero;
            ArmyHS Army;
            LocationTextEventLog Log;
            HeroEmergeEventLog EmergeLog = default;
            ArmyInfoEventLog InfoLog = default;
            if (ExecuteCount == 0)
            {
                Source.GetLocation(ref X, ref Y, ref Layer);
                AoWHSMap.Current.ValidateObservation(Source.FPlayer, X, Y, Layer, 0);
                Animation = new TwinkleAnimation(null);
                Animation.Setup(FCastImages.GetImageSequence(0), Set<TwinkleModeElement>.Of(TwinkleModeElement.Rotate), 50, 64, 50, 30);
                AoWHSMap.Current.PlaceHX(Animation, X, Y, Layer);
                if (Source.FPlayer == AoWHSMap.Current.FSeatedPlayer || AoWHSMap.Current.Watching(X, Y, Layer, 0))
                {
                    FCastSounds.Play(0);
                }
            }

            if (ExecuteCount == 20)
            {
                Heroes = new HeroList(null);
                AoWHSMap.Current.FHeroControl.ListDeadHeroes(Heroes);
                for (int I = Heroes.Count - 1; I >= 0; I--)
                {
                    if (Heroes.GetHero(I) is Leader || Heroes.GetHero(I).GetLevel() > AoWHSMap.Current.FPlayers.GetPlayers(Source.FPlayer).FBaseHeroJoinLevel + 2)
                    {
                        Heroes.RemoveChild(Heroes.GetHero(I));
                    }
                }

                if (Heroes.Count != 0)
                {
                    Source.GetLocation(ref X, ref Y, ref Layer);
                    var count = Heroes.Count;
                    Hero = Heroes.GetHero(AoWHSMap.Current.Random(count));
                    Hero.Resurrect();
                    Army = Hero.Place(X, Y, Layer, 0, Set<UnitPlaceModeElement>.Of(UnitPlaceModeElement.ScanArea, UnitPlaceModeElement.CreateNewArmy));
                    Log = new LocationTextEventLog(null);
                    Log.FPlayer = Source.FPlayer;
                    Log.FText = RtlUnit.Format(AbilityRules.TranslateRStr(XResurrectedRStr), [Hero.FHeroName]);
                    var lhx = Army.GetLhx();
                    var yhx = Army.GetYhx();
                    Log.SetLocation(Army.GetXhx(), yhx, lhx);
                    var armyYhx = Army.GetYhx();
                    var armyLhx = Army.GetLhx();
                    var xhx = Army.GetXhx();
                    EventLog.DistributeLocationEventLog(Log, Source.FPlayer, xhx, armyYhx, armyLhx, false);
                    Log.Release();
                    if (Hero.OfferToJoin(Source.FPlayer))
                    {
                        EmergeLog = new HeroEmergeEventLog(null);
                        EmergeLog.Setup(Hero, Source.FPlayer, HeroEmergeEventLog.HeroJoinMode.Resurrected);
                        EmergeLog.Execute(true, ExecuteTrigger.Event, null);
                        EmergeLog.Release();
                    }
                    else
                    {
                        InfoLog = new ArmyInfoEventLog(null);
                        var indexOf = Army.FArmy.IndexOf(Hero);
                        InfoLog.FArmyView.Setup(Army.FArmy, (byte)(1 << indexOf));
                        InfoLog.FSelection = 255;
                        InfoLog.FPlayer = Source.FPlayer;
                        var noJoinResurrection = AbilityRules.TranslateRStr(NoJoinResurrectionRStr);
                        InfoLog.FMessages.Text = noJoinResurrection;
                        InfoLog.FTriggers = Set<ExecuteTrigger>.Of(ExecuteTrigger.Event);
                        InfoLog.Execute(true, ExecuteTrigger.Event, null);
                        InfoLog.Release();
                    }
                }
                else
                {
                    var noHeroAnswersYourCall = AbilityRules.TranslateRStr(GameText.NoHeroAnswersYourCallRStr);
                    AoWHSMap.Current.ShowMessage(noHeroAnswersYourCall);
                }

                CastingDone(Source, Event.FEventMana);
                Heroes?.Dispose();
                Result = false;
            }
            else
            {
                Result = true;
            }

            return Result;
        }

        // GlobalSpells.pas:1062
        public override bool Activate(AbstractUnit Source)
        {
            bool Result;
            string Reason = "";
            SpellTE Event;
            base.Activate(Source);
            if (CanActivate(Source, ref Reason) && !AoWHSMap.Current.FPlayers.GetPlayers(Source.FPlayer).GetBusy())
            {
                Event = new SpellTE(null);
                Event.Initialize(FSpellID, (byte)Source.FPlayer, Source.FUnitID, FBaseManaCost);
                AoWHSMap.Current.FTokenManager.GetTokenControl(Source.FPlayer).AddEvent(Event);
                Event.Release();
                Result = true;
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        private static readonly string NoJoinResurrectionRStr = "I needed that... Goodbye.";
        private static readonly string ResurrectHeroRStr = "Resurrect Hero";
        private static readonly string XResurrectedRStr = "%s resurrected";
    }

    // inherited name, spell ID, mana, sphere, level, type, sounds, images
    public partial class AnimateHero : Spell
    {
        // GlobalSpells.pas:1080
        public AnimateHero(ECustomNode owner) : base(owner)
        {
            FSpellName = AbilityRules.TranslateRStr(AnimateHeroRStr);
            FSpellID = 67;
            FSpellType = SpellType.Global;
            FBaseManaCost = 0;
            FSpellLevel = 2;
            FMagicSphere = MagicSphere.Cosmos;
        }

        // GlobalSpells.pas:1091
        public override bool ExecuteTE(SpellTE Event, AbstractUnit Source, int ExecuteCount)
        {
            bool Result = default;
            sbyte X = default;
            sbyte Y = default;
            sbyte Layer = default;
            TwinkleAnimation Animation = default;
            HeroList Heroes;
            Hero Hero;
            ArmyHS Army;
            LocationTextEventLog Log;
            HeroEmergeEventLog EmergeLog = default;
            ArmyInfoEventLog InfoLog = default;
            if (ExecuteCount == 0)
            {
                Source.GetLocation(ref X, ref Y, ref Layer);
                AoWHSMap.Current.ValidateObservation(Source.FPlayer, X, Y, Layer, 0);
                Animation = new TwinkleAnimation(null);
                Animation.Setup(FCastImages.GetImageSequence(0), Set<TwinkleModeElement>.Of(TwinkleModeElement.Rotate), 50, 64, 50, 30);
                AoWHSMap.Current.PlaceHX(Animation, X, Y, Layer);
                if (Source.FPlayer == AoWHSMap.Current.FSeatedPlayer || AoWHSMap.Current.Watching(X, Y, Layer, 0))
                {
                    FCastSounds.Play(0);
                }
            }

            if (ExecuteCount == 20)
            {
                Heroes = new HeroList(null);
                AoWHSMap.Current.FHeroControl.ListDeadHeroes(Heroes);
                for (int I = Heroes.Count - 1; I >= 0; I--)
                {
                    if (Heroes.GetHero(I) is Leader || Heroes.GetHero(I).GetLevel() > AoWHSMap.Current.FPlayers.GetPlayers(Source.FPlayer).FBaseHeroJoinLevel + 2)
                    {
                        Heroes.RemoveChild(Heroes.GetHero(I));
                    }
                }

                if (Heroes.Count != 0)
                {
                    Source.GetLocation(ref X, ref Y, ref Layer);
                    var count = Heroes.Count;
                    Hero = Heroes.GetHero(AoWHSMap.Current.Random(count));
                    Hero.Animate();
                    Army = Hero.Place(X, Y, Layer, 0, Set<UnitPlaceModeElement>.Of(UnitPlaceModeElement.ScanArea, UnitPlaceModeElement.CreateNewArmy));
                    Log = new LocationTextEventLog(null);
                    Log.FPlayer = Source.FPlayer;
                    Log.FText = RtlUnit.Format(AbilityRules.TranslateRStr(XAnimatedRStr), [Hero.FHeroName]);
                    var lhx = Army.GetLhx();
                    var yhx = Army.GetYhx();
                    Log.SetLocation(Army.GetXhx(), yhx, lhx);
                    var armyYhx = Army.GetYhx();
                    var armyLhx = Army.GetLhx();
                    var xhx = Army.GetXhx();
                    EventLog.DistributeLocationEventLog(Log, Source.FPlayer, xhx, armyYhx, armyLhx, false);
                    Log.Release();
                    if (Hero.OfferToJoin(Source.FPlayer))
                    {
                        EmergeLog = new HeroEmergeEventLog(null);
                        EmergeLog.Setup(Hero, Source.FPlayer, HeroEmergeEventLog.HeroJoinMode.Resurrected);
                        EmergeLog.Execute(true, ExecuteTrigger.Event, null);
                        EmergeLog.Release();
                    }
                    else
                    {
                        InfoLog = new ArmyInfoEventLog(null);
                        var indexOf = Army.FArmy.IndexOf(Hero);
                        InfoLog.FArmyView.Setup(Army.FArmy, (byte)(1 << indexOf));
                        InfoLog.FSelection = 255;
                        InfoLog.FPlayer = Source.FPlayer;
                        var noJoinAnimate = AbilityRules.TranslateRStr(NoJoinAnimateRStr);
                        InfoLog.FMessages.Text = noJoinAnimate;
                        InfoLog.FTriggers = Set<ExecuteTrigger>.Of(ExecuteTrigger.Event);
                        InfoLog.Execute(true, ExecuteTrigger.Event, null);
                        InfoLog.Release();
                    }
                }
                else
                {
                    if (Source.FPlayer == AoWHSMap.Current.FSeatedPlayer)
                    {
                        var noHeroAnswersYourCall = AbilityRules.TranslateRStr(GameText.NoHeroAnswersYourCallRStr);
                        AoWHSMap.Current.ShowMessage(noHeroAnswersYourCall);
                        AoWHSSet.Current.FSpellControl.GetSpell(FSpellID).FCastSounds.Play(0);
                    }
                }

                CastingDone(Source, Event.FEventMana);
                Heroes?.Dispose();
                Result = false;
            }
            else
            {
                Result = true;
            }

            return Result;
        }

        // GlobalSpells.pas:1166
        public override bool Activate(AbstractUnit Source)
        {
            bool Result;
            string Reason = "";
            SpellTE Event;
            base.Activate(Source);
            if (CanActivate(Source, ref Reason) && !AoWHSMap.Current.FPlayers.GetPlayers(Source.FPlayer).GetBusy())
            {
                Event = new SpellTE(null);
                Event.Initialize(FSpellID, (byte)Source.FPlayer, Source.FUnitID, FBaseManaCost);
                AoWHSMap.Current.FTokenManager.GetTokenControl(Source.FPlayer).AddEvent(Event);
                Event.Release();
                Result = true;
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        private static readonly string AnimateHeroRStr = "Animate Hero";
        private static readonly string NoJoinAnimateRStr = "Who dares to call me! Do you really think I would serve you?!";
        private static readonly string XAnimatedRStr = "%s animated";
    }

    // inherited name, spell ID, mana, sphere, level, type, sounds, images
    public partial class CallHero : Spell
    {
        // GlobalSpells.pas:1184
        public CallHero(ECustomNode owner) : base(owner)
        {
            FSpellName = AbilityRules.TranslateRStr(CallHeroRStr);
            FSpellID = 69;
            FSpellType = SpellType.Global;
            FBaseManaCost = 0;
            FSpellLevel = 2;
            FMagicSphere = MagicSphere.Cosmos;
        }

        // GlobalSpells.pas:1250
        public override bool ExecuteTE(SpellTE Event, AbstractUnit Source, int ExecuteCount)
        {
            bool ShowSource;
            bool ShowTarget;
            sbyte X = default;
            sbyte Y = default;
            sbyte Layer = default;
            Hero Hero;
            ArmyHS Army;
            ImageSequenceAnimation Animation;
            base.ExecuteTE(Event, Source, ExecuteCount);
            Source.GetLocation(ref X, ref Y, ref Layer);
            Hero = FindCallHero(Source);
            if (Hero != null)
            {
                if (Hero.Owner == null)
                {
                    Hero.SetHitPoints(Hero.GetHits());
                }

                Army = Hero.Place(X, Y, Layer, 0, Set<UnitPlaceModeElement>.Of(UnitPlaceModeElement.ScanArea, UnitPlaceModeElement.CreateNewArmy));
                ShowSource = AoWHSMap.Current.FSeatedPlayer == Source.FPlayer || AoWHSMap.Current.VisibleForPlayer(X, Y, Layer, AoWHSMap.Current.FSeatedPlayer);
                bool condition = AoWHSMap.Current.FSeatedPlayer == Source.FPlayer;
                if (!condition)
                {
                    var lhx = Army.GetLhx();
                    var yhx = Army.GetYhx();
                    var xhx = Army.GetXhx();
                    condition = AoWHSMap.Current.VisibleForPlayer(xhx, yhx, lhx, AoWHSMap.Current.FSeatedPlayer);
                }

                ShowTarget = condition;
                if (ShowSource)
                {
                    if (AoWHSSet.Current.FSpellControl.GetSpell(FSpellID).FCastImages.GetImageSequence(0) != null)
                    {
                        Animation = new ImageSequenceAnimation(null);
                        Animation.SetImageSequence(AoWHSSet.Current.FSpellControl.GetSpell(FSpellID).FCastImages.GetImageSequence(0));
                        AoWHSMap.Current.PlaceHX(Animation, X, Y, Layer);
                    }
                }

                if (ShowTarget)
                {
                    if (AoWHSSet.Current.FSpellControl.GetSpell(FSpellID).FCastImages.GetImageSequence(0) != null)
                    {
                        Animation = new ImageSequenceAnimation(null);
                        Animation.SetImageSequence(AoWHSSet.Current.FSpellControl.GetSpell(FSpellID).FCastImages.GetImageSequence(0));
                        var lhx = Army.GetLhx();
                        var yhx = Army.GetYhx();
                        var xhx = Army.GetXhx();
                        AoWHSMap.Current.PlaceHX(Animation, xhx, yhx, lhx);
                    }
                }

                if (AoWHSMap.Current.FSeatedPlayer == Source.FPlayer)
                {
                    var lhx = Army.GetLhx();
                    var yhx = Army.GetYhx();
                    var xhx = Army.GetXhx();
                    AoWHSMap.Current.CenterViewTo(xhx, yhx, lhx);
                }
                else
                {
                    var lhx = Army.GetLhx();
                    var yhx = Army.GetYhx();
                    var xhx = Army.GetXhx();
                    AoWHSMap.Current.ValidateObservation(Source.FPlayer, xhx, yhx, lhx, 0);
                }

                bool test = AoWHSMap.Current.FSeatedPlayer == Source.FPlayer || AoWHSMap.Current.Watching(X, Y, Layer, 0);
                if (!test)
                {
                    var lhx = Army.GetLhx();
                    var yhx = Army.GetYhx();
                    var xhx = Army.GetXhx();
                    test = AoWHSMap.Current.Watching(xhx, yhx, lhx, 0);
                }

                if (test)
                {
                    AoWHSSet.Current.FSpellControl.GetSpell(FSpellID).FCastSounds.Play(0);
                }
            }
            else
            {
                if (AoWHSMap.Current.FSeatedPlayer == Source.FPlayer)
                {
                    var noHeroAnsweredYourCall = AbilityRules.TranslateRStr(NoHeroAnsweredYourCallRStr);
                    AoWHSMap.Current.ShowMessage(noHeroAnsweredYourCall);
                    AoWHSSet.Current.FSpellControl.GetSpell(FSpellID).FCastSounds.Play(0);
                }
            }

            var spell = AoWHSSet.Current.FSpellControl.GetSpell(FSpellID);
            AoWHSSet.Current.FSpellControl.GetSpell(FSpellID).CastingDone(Source, spell.FBaseManaCost);
            return false;
        }

        // GlobalSpells.pas:1195
        public override bool Activate(AbstractUnit Source)
        {
            bool Result;
            string Reason = "";
            SpellTE Event;
            if (CanActivate(Source, ref Reason) && !AoWHSMap.Current.FPlayers.GetPlayers(Source.FPlayer).GetBusy())
            {
                Event = new SpellTE(null);
                Event.Initialize(FSpellID, (byte)Source.FPlayer, Source.FUnitID, FBaseManaCost);
                AoWHSMap.Current.FTokenManager.GetTokenControl(Source.FPlayer).AddEvent(Event);
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
        public Hero FindCallHero(AbstractUnit Source)
        {
            Hero Result;
            Hero Hero;
            DoubleItemList<Hero> Heroes;
            int Index;
            Heroes = new DoubleItemList<Hero>();
            for (int I = AoWHSMap.Current.FHeroControl.GetCount() - 1; I >= 0; I--)
            {
                Hero = AoWHSMap.Current.FHeroControl.GetHero(I);
                if (Hero.FPlayer == 0 && Hero.Owner is Army && Hero.ArmyOwner.FX != -1)
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
                for (int I = AoWHSMap.Current.FHeroControl.GetCount() - 1; I >= 0; I--)
                {
                    Hero = AoWHSMap.Current.FHeroControl.GetHero(I);
                    if (!Hero.FUnitStatus.Contains(UnitStatusElement.Killed) && Hero.FPlayer <= 0 && Hero.Owner == null)
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
                RandSeed = AoWHSMap.Current.Random(16777215);
                Index = Heroes.RandomChanceItemB(0);
                Result = Heroes[Index].A;
            }
            else
            {
                Result = null;
            }

            Heroes?.Dispose();
            return Result;
        }

        private static readonly string CallHeroRStr = "Call Hero";
        private static readonly string NoHeroAnsweredYourCallRStr = "No hero answered your call";
    }
}

namespace Aow.Game.Commands
{

    // dispel chance, enchantment ID
    public partial class DisjunctionTE : SpellTE
    {
        public DisjunctionTE(ECustomNode owner) : base(owner)
        { }

        // GlobalSpells.pas:745
        public new const int ClassID = 135961;

        public int FDispelChance;
        public int FEnchantmentID;

        // GlobalSpells.pas:750
        public override void ReadWrite(EStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwInteger(ref FEnchantmentID, 20);
            Stream.rwInteger(ref FDispelChance, 21);
        }

        // GlobalSpells.pas:757
        public override bool Validate(Hero source)
        {
            int Index;
            int Mana;
            int RequiredMana;
            bool Result = base.Validate(source);
            Index = AoWHSMap.Current.FGlobalMagicControl.IndexOfID(FEnchantmentID);
            Result = Result && Index != -1;
            if (Result)
            {
                var globalEnchantment = AoWHSMap.Current.FGlobalMagicControl.FEnchantments.GetEnchantments(Index);
                Mana = AoWHSMap.Current.FPlayers.GetPlayers(source.FPlayer).FMagicControl.FMana + source.FCastingProgress;
                RequiredMana = Round((double)((double)globalEnchantment.FDispelMana * 0.02) * FDispelChance) + FEventMana;
                Result = RequiredMana <= Mana;
            }

            return Result;
        }

        // GlobalSpells.pas:772
        public override bool Process(AbstractUnit source, int value)
        {
            GlobalEnchantment Enchantment;
            int Index;
            sbyte X = default;
            sbyte Y = default;
            sbyte Layer = default;
            TwinkleAnimation Twinkle;
            int Mana;
            base.Process(source, value);
            Index = AoWHSMap.Current.FGlobalMagicControl.IndexOfID(FEnchantmentID);
            Enchantment = AoWHSMap.Current.FGlobalMagicControl.FEnchantments.GetEnchantments(Index);
            source.GetLocation(ref X, ref Y, ref Layer);
            AoWHSMap.Current.ValidateObservation(source.FPlayer, X, Y, Layer, 0);
            Twinkle = new TwinkleAnimation(null);
            Twinkle.Setup(AoWHSSet.Current.FSpellControl.GetSpell(51).FCastImages.ImageSequences[0], Set<TwinkleModeElement>.Of(TwinkleModeElement.Expand, TwinkleModeElement.Explode), FEventMana + 50, 192, 50, 30);
            AoWHSMap.Current.PlaceHX(Twinkle, X, Y, Layer);
            AoWHSSet.Current.FSpellControl.GetSpell(FEventSpellID).FCastSounds.Play(0);
            Mana = Round((double)((double)Enchantment.FDispelMana * 0.02) * FDispelChance);
            var player = AoWHSMap.Current.FPlayers.GetPlayers(source.FPlayer);
            AoWHSMap.Current.FPlayers.GetPlayers(source.FPlayer).FMagicControl.SetMana(player.FMagicControl.FMana - Mana);
            if (AoWHSMap.Current.Random(100) < FDispelChance)
            {
                if (AoWHSMap.Current.FSeatedPlayer == source.FPlayer)
                {
                    var name = Enchantment.GetName();
                    var format = RtlUnit.Format(AbilityRules.TranslateRStr(XSuccessfullyDispelledRStr), [name]);
                    AoWHSMap.Current.ShowMessage(format);
                }

                AoWHSMap.Current.FGlobalMagicControl.ExecuteDispelEnchantment(source.FPlayer, FEnchantmentID);
            }
            else
            {
                if (AoWHSMap.Current.FSeatedPlayer == source.FPlayer)
                {
                    // { Original formats this message with an unused enchantment-name argument. }
                    var name = Enchantment.GetName();
                    var format = RtlUnit.Format(AbilityRules.TranslateRStr(GameText.DispelMagicFailedRStr), [name]);
                    AoWHSMap.Current.ShowMessage(format);
                }
            }

            AoWHSSet.Current.FSpellControl.GetSpell(FEventSpellID).CastingDone(source, FEventMana);
            return false;
        }

        private static readonly string XSuccessfullyDispelledRStr = "%s successfully dispelled";

        // NetworkE.pas:230

        // NetworkE.pas:230

    }
}

namespace Aow.Game.Magic.Spells
{

    // enchantment ID, dispel chance. inherited spell ID, source unit ID, references
    public partial class DisjunctionSpellCaster : SpellCaster
    {
        public int FEnchantmentID;
        public int FDispelChance;

        // GlobalSpells.pas:806
        public DisjunctionSpellCaster(ECustomNode owner) : base(owner)
        {
            FDispelChance = 50;
        }

        // GlobalSpells.pas:868
        public override bool CanCast(ref string Reason)
        {
            // FPlayer is on TAbstractUnit. FCastingProgress is on THero, so only that read tests for a hero.
            AbstractUnit Source;
            bool Result = base.CanCast(ref Reason);
            Source = AoWHSMap.Current.FUnitControl.FindUnit(FCasterSourceID);
            if (Result && SelectedEnchantment() != null)
            {
                if (!AoWHSMap.Current.FGlobalMagicControl.FActiveEnchantments.Contains(GlobalEnchantmentType.SpellWard) || SelectedEnchantment().FEnchantmentType == GlobalEnchantmentType.SpellWard)
                {
                    if (!AoWHSMap.Current.FPlayers.GetPlayers(Source.FPlayer).FMagicControl.GetCheatEnabled())
                    {
                        if (Source is Hero hero)
                        {
                            var spell = AoWHSSet.Current.FSpellControl.GetSpell(51);
                            var mana = GetMana();
                            if (AoWHSMap.Current.FPlayers.GetPlayers(Source.FPlayer).FMagicControl.FMana + hero.FCastingProgress >= spell.FBaseManaCost + mana)
                            {
                                Result = true;
                            }
                            else
                            {
                                Result = false;
                                Reason = AbilityRules.TranslateRStr(GameText.NotEnoughManaRStr);
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
                    Reason = AbilityRules.TranslateRStr(CannotDispelWhenSpellWardIsActiveRStr);
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
            AbstractUnit Source;
            DisjunctionTE Event;
            if (CanCast(ref Reason))
            {
                Source = AoWHSMap.Current.FUnitControl.FindUnit(FCasterSourceID);
                if (!AoWHSMap.Current.FPlayers.GetPlayers(Source.FPlayer).GetBusy())
                {
                    Event = new DisjunctionTE(null);
                    var spell = AoWHSSet.Current.FSpellControl.GetSpell(51);
                    Event.Initialize(FCasterSpellID, (byte)Source.FPlayer, Source.FUnitID, spell.FBaseManaCost);
                    Event.FEnchantmentID = FEnchantmentID;
                    Event.FDispelChance = FDispelChance;
                    AoWHSMap.Current.FTokenManager.GetTokenControl(Source.FPlayer).AddEvent(Event);
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
                AoWHSMap.Current.ShowMessage(Reason);
            }

            return Result;
        }

        // GlobalSpells.pas:812
        public void SetEnchantmentID(int Value)
        {
            FEnchantmentID = Value;
        }

        // GlobalSpells.pas:817
        public GlobalEnchantment SelectedEnchantment()
        {
            GlobalEnchantment Result;
            int Index;
            Index = AoWHSMap.Current.FGlobalMagicControl.IndexOfID(FEnchantmentID);
            if (Index != -1)
            {
                Result = AoWHSMap.Current.FGlobalMagicControl.FEnchantments.GetEnchantments(Index);
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
            GlobalEnchantment Enchantment;
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
            AbstractUnit Source;
            int Mana;
            bool Result = false;
            Source = AoWHSMap.Current.FUnitControl.FindUnit(FCasterSourceID);
            if (SelectedEnchantment() != null)
            {
                if (!AoWHSMap.Current.FPlayers.GetPlayers(Source.FPlayer).FMagicControl.GetCheatEnabled())
                {
                    if (Source is Hero hero)
                    {
                        Mana = AoWHSMap.Current.FPlayers.GetPlayers(Source.FPlayer).FMagicControl.FMana + hero.FCastingProgress;
                        var spell = AoWHSSet.Current.FSpellControl.GetSpell(51);
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

        internal static readonly string CannotDispelWhenSpellWardIsActiveRStr = "Cannot dispel when Spell Ward is active";

    }

    // inherited name, spell ID, mana, sphere, level, type, sounds, images
    public partial class Disjunction : Spell
    {
        // GlobalSpells.pas:931
        public Disjunction(ECustomNode owner) : base(owner)
        {
            FSpellName = AbilityRules.TranslateRStr(DisjunctionRStr);
            FSpellID = 51;
            FSpellType = SpellType.Global;
            FBaseManaCost = 0;
            FSpellLevel = 2;
            FMagicSphere = MagicSphere.Cosmos;
        }

        // GlobalSpells.pas:942
        public override bool CanActivate(AbstractUnit Source, ref string MessageText)
        {
            bool Result;
            int Count;
            string Reason = "";
            Count = 0;
            for (int I = 0, last = AoWHSMap.Current.FGlobalMagicControl.FEnchantments.Count - 1; I <= last; I++)
            {
                if (AoWHSMap.Current.FGlobalMagicControl.FEnchantments.GetEnchantments(I).FPlayerIndex != Source.FPlayer && AoWHSMap.Current.FGlobalMagicControl.FEnchantments.GetEnchantments(I).VisibleForPlayer(Source.FPlayer) && AoWHSMap.Current.FGlobalMagicControl.FEnchantments.GetEnchantments(I).CanDispel(Source.FPlayer, ref Reason))
                {
                    Count += 1;
                }
            }

            if (Count == 0)
            {
                MessageText = AbilityRules.TranslateRStr(NoGlobalOpponentEnchantmentsActiveRStr);
                Result = false;
            }
            else
            {
                Result = true;
            }

            return Result;
        }

        // GlobalSpells.pas:962
        public override bool Activate(AbstractUnit Source)
        {
            bool Result;
            string Reason = "";
            DisjunctionSpellCaster Caster;
            if (CanActivate(Source, ref Reason))
            {
                Caster = new DisjunctionSpellCaster(null);
                Caster.FCasterSpellID = FSpellID;
                Caster.FCasterSourceID = Source.FUnitID;
                AoWHSMap.Current.FCosmeticSurgeryEventsStore.Raise(Caster);
                Caster.Release();
                Result = true;
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        private static readonly string DisjunctionRStr = "Disjunction";
        private static readonly string NoGlobalOpponentEnchantmentsActiveRStr = "No global opponent enchantments active";
    }

    // inherited name, spell ID, mana, sphere, level, type, sounds, images
    public partial class Crusade : Spell
    {
        // GlobalSpells.pas:1302
        public Crusade(ECustomNode owner) : base(owner)
        {
            FSpellName = AbilityRules.TranslateRStr(CrusadeRStr);
            FSpellID = 75;
            FSpellType = SpellType.Global;
            FBaseManaCost = 0;
            FSpellLevel = 4;
            FMagicSphere = MagicSphere.Life;
        }

        // GlobalSpells.pas:1340
        public override bool ExecuteTE(SpellTE Event, AbstractUnit Source, int ExecuteCount)
        {
            int Count;
            int UnitIndex;
            int Xhp = default;
            int Yhp = default;
            sbyte X = default;
            sbyte Y = default;
            sbyte Layer = default;
            ArmyHS Party;
            Unit NewUnit;
            if (ExecuteCount == 0)
            {
                Source.GetLocation(ref X, ref Y, ref Layer);
                AoWHSMap.Current.ValidateObservation(Source.FPlayer, X, Y, Layer, 0);
                HexGeometry.HXtoHP(X, Y, ref Xhp, ref Yhp);
                var worldFXHS = AoWHSMap.Current.AddEffect(TworldFXType.Crusade);
                worldFXHS.FX = Xhp + 16;
                worldFXHS.FY = Yhp + 16;
                worldFXHS.FLayer = Layer;

                if (AoWHSMap.Current.Random(100) < 20)
                {
                    Party = new ArmyHS(null);
                    Party.FArmy.SetPlayer(0);
                    Count = 1;
                    UnitIndex = AoWHSMap.Current.Random(1);
                    for (int J = 0; J <= Count - 1; J++)
                    {
                        NewUnit = new Unit(null);
                        NewUnit.SetUnitResource(AoWHSSet.Current.FUnitResources.GetUnitResource(TCrusade_ExecuteTE__LargeUnits[UnitIndex]));
                        NewUnit.ExpandAbility(101);
                        Party.FArmy.AddUnit(NewUnit);
                        NewUnit.Release();
                    }

                    if (!PlaceParty(Source, Party))
                    {
                        Party?.Dispose();
                    }
                }
                else
                {
                    for (int I = 0; I <= 1; I++)
                    {
                        Party = new ArmyHS(null);
                        Party.FArmy.SetPlayer(0);
                        Count = AoWHSMap.Current.Random(3) + 4;
                        for (int J = 0; J <= Count - 1; J++)
                        {
                            NewUnit = new Unit(null);
                            UnitIndex = AoWHSMap.Current.Random(2);
                            NewUnit.SetUnitResource(AoWHSSet.Current.FUnitResources.GetUnitResource(TCrusade_ExecuteTE__FootUnits[UnitIndex]));
                            NewUnit.ExpandAbility(101);
                            Party.FArmy.AddUnit(NewUnit);
                            NewUnit.Release();
                        }

                        if (!PlaceParty(Source, Party))
                        {
                            Party?.Dispose();
                        }
                    }

                    Party = new ArmyHS(null);
                    Party.FArmy.SetPlayer(0);
                    UnitIndex = AoWHSMap.Current.Random(3);
                    Count = 2;
                    if (UnitIndex == 2)
                    {
                        Count += 1;
                    }

                    for (int J = 0; J <= Count - 1; J++)
                    {
                        NewUnit = new Unit(null);
                        NewUnit.SetUnitResource(AoWHSSet.Current.FUnitResources.GetUnitResource(TCrusade_ExecuteTE__MountedUnits[UnitIndex]));
                        NewUnit.ExpandAbility(101);
                        Party.FArmy.AddUnit(NewUnit);
                        NewUnit.Release();
                    }

                    if (!PlaceParty(Source, Party))
                    {
                        Party?.Dispose();
                    }
                }

                AoWHSSet.Current.FSpellControl.GetSpell(FSpellID).CastingDone(Source, FBaseManaCost);
                AoWHSSet.Current.FSpellControl.GetSpell(FSpellID).FCastSounds.Play(0);
            }

            return false;
        }

        // GlobalSpells.pas:1417
        public override bool Activate(AbstractUnit Source)
        {
            bool Result;
            string Reason = "";
            SpellTE Event;
            base.Activate(Source);
            if (CanActivate(Source, ref Reason) && !AoWHSMap.Current.FPlayers.GetPlayers(Source.FPlayer).GetBusy())
            {
                Event = new SpellTE(null);
                Event.Initialize(FSpellID, (byte)Source.FPlayer, Source.FUnitID, FBaseManaCost);
                AoWHSMap.Current.FTokenManager.GetTokenControl(Source.FPlayer).AddEvent(Event);
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
        public bool PlaceParty(AbstractUnit Source, ArmyHS Party)
        {
            bool Result;
            int TargetX = default;
            int TargetY = default;
            sbyte X = default;
            sbyte Y = default;
            sbyte Layer = default;
            ImageSequenceAnimation Animation;
            Source.GetLocation(ref X, ref Y, ref Layer);
            var radToHN = HexGeometry.RadToHN(5);
            var random = AoWHSMap.Current.Random(radToHN);
            HexGeometry.CenterHNtoHX(X, Y, random, ref TargetX, ref TargetY);
            if (Party.PlaceEx((sbyte)TargetX, (sbyte)TargetY, Layer, Set<ArmyPlaceModeElement>.Of(ArmyPlaceModeElement.Safe, ArmyPlaceModeElement.ScanArea)))
            {
                Party.Activate();
                Party.FArmy.SetBehavior(255, GroupBehavior.Suicidal);
                var lhx = Party.GetLhx();
                var yhx = Party.GetYhx();
                var xhx = Party.GetXhx();
                if (AoWHSMap.Current.Watching(xhx, yhx, lhx, 0))
                {
                    if (AoWHSSet.Current.FSpellControl.GetSpell(34).FCastImages.GetImageSequence(0) != null)
                    {
                        // { Original uses Warp Party's images for Crusade parties. }
                        Animation = new ImageSequenceAnimation(null);
                        Animation.SetImageSequence(AoWHSSet.Current.FSpellControl.GetSpell(34).FCastImages.GetImageSequence(0));
                        Animation.FAnimationLevel = 28;
                        var partyLhx = Party.GetLhx();
                        var partyYhx = Party.GetYhx();
                        var partyXhx = Party.GetXhx();
                        AoWHSMap.Current.PlaceHX(Animation, partyXhx, partyYhx, partyLhx);
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

        private static readonly string CrusadeRStr = "Crusade";
        private static FixedArray_int_1 TCrusade_ExecuteTE__LargeUnits = FixedArray_int_1.From([133]);
        private static FixedArray_int_3 TCrusade_ExecuteTE__MountedUnits = FixedArray_int_3.From([132, 134, 128]);
        private static FixedArray_int_2 TCrusade_ExecuteTE__FootUnits = FixedArray_int_2.From([129, 126]);
    }
}

namespace Aow.Game.Magic.Spells.Global
{

    [StructLayout(LayoutKind.Explicit, Size = 110)]
    public struct GlobalSpellEngineFields
    {
        [FieldOffset(0)]
        public FixedArray_byte_109 Reserved;
        [FieldOffset(109)]
        public byte Detail;
    }
}
