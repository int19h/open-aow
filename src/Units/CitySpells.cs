// Translated from CitySpells.pas.
using System.Runtime.CompilerServices;
using Aow.Game.Ai;
using Aow.Game.Audio;
using Aow.Game.Combat;
using Aow.Game.Commands;
using Aow.Game.Draw;
using Aow.Game.Draw.Animation;
using Aow.Game.Engine;
using Aow.Game.Engine.Buffers;
using Aow.Game.Engine.Nodes;
using Aow.Game.Engine.Storage;
using Aow.Game.Events;
using Aow.Game.Magic;
using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.Magic.Spells.City;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Game.World.Structures;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Magic.Spells.City
{

    public enum CityTargetModeElement : byte
    {
        Own = 0,
        Enemy = 1,
        SkipSource = 2,
        Razed = 3
    }

    public enum CityEnchantmentCastSpellSituationElement : byte
    {
        LeaderInCity = 0,
        ccs4HexCity = 1,
        ccs3HexCity = 2
    }
}

namespace Aow.Game.Commands
{

    // { ot=1 }
    // packed target coordinates. target owner
    public partial class CityTargetSpellTE : SpellTE
    {
        public CityTargetSpellTE(ECustomNode owner) : base(owner)
        { }

        // CitySpells.pas:170
        public new const int ClassID = 135949;

        public XYL FTargetLocation;
        public sbyte FTargetPlayer;

        // CitySpells.pas:175
        public override void ReadWrite(EStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwInteger(ref FTargetLocation.Packed, 20);
            Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FTargetPlayer), 21);
        }

        // CitySpells.pas:189
        public override bool Validate(Hero source)
        {
            City TargetCity;
            bool Result = base.Validate(source);
            if (Result)
            {
                TargetCity = GetSelectedCity();
                if (TargetCity == null || TargetCity.FPlayer != FTargetPlayer)
                {
                    Result = false;
                }
                else
                {
                    Result = true;
                }
            }

            return Result;
        }

        // CitySpells.pas:182
        public void SetLocation(sbyte X, sbyte Y, sbyte L)
        {
            FTargetLocation.X = X;
            FTargetLocation.Y = Y;
            FTargetLocation.L = L;
        }

        // CitySpells.pas:204
        public City GetSelectedCity() => (City)Aow.Game.World.Map.AoWHSMap.Current.MapContainer.GetFieldXYL(FTargetLocation.Packed).FindNoneTransparentHS(typeof(City));

        // NetworkE.pas:230

    }
}

namespace Aow.Game.Magic.Spells.City
{

    // source unit ID. city target modes
    public partial class CityTargetSpell : Aow.Game.Magic.Spells.Spell
    {
        public int FSourceID;
        public Set<CityTargetModeElement> FCityTargetMode;

        // CitySpells.pas:209
        public CityTargetSpell(ECustomNode owner) : base(owner)
        {
            FSpellType = Aow.Game.Magic.Spells.SpellType.Global;
        }

        // CitySpells.pas:377
        public override bool Activate(AbstractUnit Source)
        {
            bool Result;
            string Reason = "";
            if (CanActivate(Source, ref Reason))
            {
                FSourceID = Source.FUnitID;
                SetupTargetSelection(Source, Aow.Game.World.Map.AoWHSMap.Current.FTargetSelection);
                Aow.Game.World.Map.AoWHSMap.Current.FTargetSelection.Activate();
                Result = true;
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // CitySpells.pas:288
        public virtual void OnTargetSelectionChange(TargetSelectionControl TargetSelection)
        {
            AbstractUnit Source = default;
            bool Valid = default;
            Source = Aow.Game.World.Map.AoWHSMap.Current.FUnitControl.FindUnit(FSourceID);
            if (Source != null)
            {
                switch (TargetSelection.FChange)
                {
                    case TargetSelectionChange.Deactivated:
                        TargetSelection.FOnChange -= OnTargetSelectionChange;
                        TargetSelection.Deactivate();
                        break;
                    case TargetSelectionChange.Selected:
                        ValidateTarget(TargetSelection, TargetSelection.FTargetX, TargetSelection.FTargetY, TargetSelection.FTargetL, ref Valid);
                        if (Valid)
                        {
                            if (TargetSelected(Source, TargetSelection.FTargetX, TargetSelection.FTargetY, TargetSelection.FTargetL))
                            {
                                TargetSelection.FOnChange -= OnTargetSelectionChange;
                                TargetSelection.Deactivate();
                            }
                        }

                        break;
                }
            }
            else
            {
                TargetSelection.FOnChange -= OnTargetSelectionChange;
                TargetSelection.Deactivate();
            }
        }

        // CitySpells.pas:215
        public virtual void PlayDefaultCityEffects(AbstractUnit Source, global::Aow.Game.World.Structures.City TargetCity)
        {
            MapLevel Level;
            ImageSequenceAnimation A;
            HNWalkData Walk = default;
            bool PlaySound;
            PlaySound = false;
            var lhx = TargetCity.GetLhx();
            Level = Aow.Game.World.Map.AoWHSMap.Current.MapContainer.GetMapLevel(lhx);
            var yhx = TargetCity.GetYhx();
            var xhx = TargetCity.GetXhx();
            Aow.Game.World.Movement.HNWalkData.InitHNWalk(ref Walk, xhx, yhx, 0);
            while (Walk.HN < TargetCity.Resource.TerrainList.Count)
            {
                if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < Level.Width && Walk.Y < Level.Height && TargetCity.Resource.TerrainList.Used[Walk.HN])
                {
                    bool condition = Source.FPlayer == Aow.Game.World.Map.AoWHSMap.Current.FSeatedPlayer;
                    if (!condition)
                    {
                        var targetCityLhx = TargetCity.GetLhx();
                        condition = Aow.Game.World.Map.AoWHSMap.Current.TerrainVisible((sbyte)Walk.X, (sbyte)Walk.Y, targetCityLhx, 0);
                    }

                    if (condition)
                    {
                        // { Original passes the city layer before the radius. }
                        A = new ImageSequenceAnimation(null);
                        A.SetImageSequence(Aow.Game.Engine.AoWHSSet.Current.FSpellControl.GetSpell(FSpellID).FCastImages.ImageSequences[0]);
                        A.FFrameCounter = Random(5);
                        A.FAnimationLevel = 27;
                        var targetCityLhx = TargetCity.GetLhx();
                        Aow.Game.World.Map.AoWHSMap.Current.PlaceHX(A, Walk.X, Walk.Y, targetCityLhx);
                        bool test = Source.FPlayer == Aow.Game.World.Map.AoWHSMap.Current.FSeatedPlayer;
                        if (!test)
                        {
                            var lhx2 = TargetCity.GetLhx();
                            test = Aow.Game.World.Map.AoWHSMap.Current.WatchingTerrain((sbyte)Walk.X, (sbyte)Walk.Y, lhx2, 0);
                        }

                        if (test)
                        {
                            PlaySound = true;
                        }
                    }
                }

                Aow.Game.World.Movement.HNWalkData.WalkToNextHN(ref Walk);
            }

            if (PlaySound)
            {
                Aow.Game.Engine.AoWHSSet.Current.FSpellControl.GetSpell(FSpellID).FCastSounds.Play(0);
            }
        }

        // CitySpells.pas:323
        public virtual void SetupTargetSelection(AbstractUnit Source, TargetSelectionControl TargetSelection)
        {
            sbyte X = default;
            sbyte Y = default;
            sbyte L = default;
            TargetSelection.Initialize(Source);
            TargetSelection.FTargetMode = TargetSelection.FTargetMode + Set<TargetSelectionModeElement>.Of(TargetSelectionModeElement.ValidateDistance, TargetSelectionModeElement.ExternalFloater);
            var format = RtlUnit.Format(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Spells.City.CityTargetSpell.SelectTargetCityForXOnMapRStr), [FSpellName]);
            TargetSelection.FText.Text = format;
            TargetSelection.FTitle = FSpellName;
            TargetSelection.FCancelText = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.CancelRStr);
            TargetSelection.FSourceImage = Aow.Game.Engine.AoWHSSet.Current.FSpellControl.GetSpell(FSpellID).FCastImages.ImageSequences[20].GetPreviewImage();
            TargetSelection.FValidTargetImage = Aow.Game.Engine.AoWHSSet.Current.FSpellControl.GetSpell(FSpellID).FCastImages.ImageSequences[21].GetPreviewImage();
            TargetSelection.FInvalidTargetImage = Aow.Game.Engine.AoWHSSet.Current.FSpellControl.GetSpell(FSpellID).FCastImages.ImageSequences[22].GetPreviewImage();
            Source.GetLocation(ref X, ref Y, ref L);
            TargetSelection.FSourceX = X;
            TargetSelection.FSourceY = Y;
            TargetSelection.FSourceL = L;
            TargetSelection.FRange = 12;
            if (TargetSelection.FOnChangeStore.Count > 1)
            {
                // { Original rejects two existing listeners. }
                throw new InvalidOperationException("oohps");
            }

            TargetSelection.FOnChange += OnTargetSelectionChange;
            TargetSelection.FOnValidateTarget = ValidateTarget;
        }

        // CitySpells.pas:359
        public virtual bool TargetSelected(AbstractUnit Source, int X, int Y, int L)
        {
            bool Result;
            CityTargetSpellTE Event;
            if (!Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(Source.FPlayer).GetBusy())
            {
                Event = CreateTE();
                if (SetupTE(Event, Source, X, Y, L))
                {
                    Aow.Game.World.Map.AoWHSMap.Current.FTokenManager.GetTokenControl(Source.FPlayer).AddEvent(Event);
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

        // CitySpells.pas:248
        public virtual bool ValidTargetCity(AbstractUnit Source, global::Aow.Game.World.Structures.City TargetCity)
        {
            sbyte X = default;
            sbyte Y = default;
            sbyte L = default;
            global::Aow.Game.World.Structures.City Found;
            bool Result = (FCityTargetMode.Contains(CityTargetModeElement.Own) && TargetCity.FPlayer == Source.FPlayer) || (FCityTargetMode.Contains(CityTargetModeElement.Enemy) && TargetCity.FPlayer != Source.FPlayer);
            Result = Result && (!TargetCity.GetRazed() || FCityTargetMode.Contains(CityTargetModeElement.Razed));
            if (Result && FCityTargetMode.Contains(CityTargetModeElement.SkipSource))
            {
                Source.GetLocation(ref X, ref Y, ref L);
                Found = (global::Aow.Game.World.Structures.City)Aow.Game.World.Map.AoWHSMap.Current.MapContainer.GetField(X, Y, L).FindNoneTransparentHS(typeof(global::Aow.Game.World.Structures.City));
                Result = TargetCity != Found;
            }

            return Result;
        }

        // CitySpells.pas:346
        public virtual CityTargetSpellTE CreateTE() => new CityTargetSpellTE(null);

        // CitySpells.pas:351
        public virtual bool SetupTE(CityTargetSpellTE Event, AbstractUnit Source, int X, int Y, int L)
        {
            Event.Initialize(FSpellID, (byte)Source.FPlayer, Source.FUnitID, FBaseManaCost);
            Event.SetLocation((sbyte)X, (sbyte)Y, (sbyte)L);
            Event.FTargetPlayer = Event.GetSelectedCity().FPlayer;
            return true;
        }

        // CitySpells.pas:262
        public void ValidateTarget(TargetSelectionControl TargetSelection, sbyte X, sbyte Y, sbyte L, ref bool Valid)
        {
            AbstractUnit Source;
            MapLevel Level;
            AoWMapField Field;
            global::Aow.Game.World.Structures.City TargetCity;
            Level = Aow.Game.World.Map.AoWHSMap.Current.MapContainer.GetMapLevel(L);
            Source = Aow.Game.World.Map.AoWHSMap.Current.FUnitControl.FindUnit(FSourceID);
            if (Source != null && X >= 0 && Y >= 0 && X < Level.Width && Y < Level.Height)
            {
                Field = (AoWMapField)Level.GetField(X, Y);
                Valid = !Aow.Game.World.Map.AoWHSMap.Current.FExplorationEnabled || Field.GetExplored(Source.FPlayer);
                if (Valid)
                {
                    TargetCity = (global::Aow.Game.World.Structures.City)Field.FindNoneTransparentHS(typeof(global::Aow.Game.World.Structures.City));
                    if (TargetCity != null)
                    {
                        Valid = ValidTargetCity(Source, TargetCity);
                    }
                    else
                    {
                        Valid = false;
                    }
                }
            }
            else
            {
                Valid = false;
            }
        }

        private static readonly string SelectTargetCityForXOnMapRStr = "Select target city for %s on map.";
    }
}

namespace Aow.Game.Commands
{

    // army ID. destination coordinates
    public partial class TownGateTE : CityTargetSpellTE
    {
        public TownGateTE(ECustomNode owner) : base(owner)
        { }

        // CitySpells.pas:397
        public new const int ClassID = 135950;

        public ArmyID FArmyID;
        public XYL FTeleportLocation;

        // CitySpells.pas:391
        public override void Dispose()
        {
            FArmyID?.Dispose();
            base.Dispose();
        }

        // CitySpells.pas:400
        public override void ReadWrite(EStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwInteger(ref FTeleportLocation.Packed, 30);
            Stream.rwCreateEObject(ref FArmyID, null, 31);
        }

        // CitySpells.pas:407
        public override bool Validate(Hero source)
        {
            byte Selection = default;
            ArmyHS Army = default;
            bool Result = base.Validate(source) && FArmyID.Valid();
            if (Result)
            {
                FArmyID.RetrieveArmy(ref Army, ref Selection);
                Result = ((TownGate)Aow.Game.Engine.AoWHSSet.Current.FSpellControl.GetSpell(FEventSpellID)).ValidTeleportLocation(Army, Selection, FTeleportLocation.X, FTeleportLocation.Y, FTeleportLocation.L);
            }

            return Result;
        }

        // NetworkE.pas:230

    }
}

namespace Aow.Game.Magic.Spells.City
{

    public partial class TownGate : CityTargetSpell
    {
        // CitySpells.pas:421
        public TownGate(ECustomNode owner) : base(owner)
        {
            FSpellName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Spells.City.TownGate.TownGateRStr);
            FSpellID = 38;
            FCityTargetMode = Set<CityTargetModeElement>.Of(CityTargetModeElement.Own, CityTargetModeElement.SkipSource);
            FBaseManaCost = 5;
            FSpellLevel = 3;
            FMagicSphere = Aow.Game.Magic.MagicSphere.Cosmos;
        }

        // CitySpells.pas:486
        public override bool ExecuteTE(SpellTE Event, AbstractUnit Source, int ExecuteCount)
        {
            bool ShowSource;
            bool ShowTarget;
            byte Selection = default;
            ArmyHS Army = default;
            AoWMapField Field;
            ArmyHS NewArmy;
            sbyte X = default;
            sbyte Y = default;
            sbyte L = default;
            ImageSequenceAnimation A;
            Source.GetLocation(ref X, ref Y, ref L);
            ((TownGateTE)Event).FArmyID.RetrieveArmy(ref Army, ref Selection);
            Field = (AoWMapField)Aow.Game.World.Map.AoWHSMap.Current.MapContainer.GetFieldXYL(((TownGateTE)Event).FTeleportLocation.Packed);
            Army.FArmy.ResetDestinationPos();
            if (Aow.Game.World.Map.AoWHSMap.Current.FSelectedArmy.GetArmyHS() == Army)
            {
                Aow.Game.World.Map.AoWHSMap.Current.FSelectedArmy.Select(Army);
            }

            if (Army.FArmy.ValidateSelection(Selection) != Army.FArmy.ValidateSelection(255))
            {
                NewArmy = new ArmyHS(null);
                NewArmy.FArmy.SetPlayer(Army.FArmy.FPlayer);
                NewArmy.FArmy.AddUnitSelection(Army.FArmy, Selection);
                Aow.Game.World.Map.AoWHSMap.Current.PlaceHX(NewArmy, ((TownGateTE)Event).FTeleportLocation.X, ((TownGateTE)Event).FTeleportLocation.Y, ((TownGateTE)Event).FTeleportLocation.L);
            }
            else
            {
                if (Field.GetArmyHS() != null)
                {
                    Field.GetArmyHS().FArmy.AddUnitSelection(Army.FArmy, Selection);
                }
                else
                {
                    Army.MoveTo(Field, 0);
                }
            }

            ShowSource = Aow.Game.World.Map.AoWHSMap.Current.FSeatedPlayer == Source.FPlayer || Aow.Game.World.Map.AoWHSMap.Current.Visible(X, Y, L, 0);
            ShowTarget = Aow.Game.World.Map.AoWHSMap.Current.FSeatedPlayer == Source.FPlayer || Aow.Game.World.Map.AoWHSMap.Current.Visible(Field.Xhx, Field.Yhx, Field.Lhx, 0);
            if (ShowSource)
            {
                if (Aow.Game.Engine.AoWHSSet.Current.FSpellControl.GetSpell(FSpellID).FCastImages.ImageSequences[0] != null)
                {
                    A = new ImageSequenceAnimation(null);
                    A.SetImageSequence(Aow.Game.Engine.AoWHSSet.Current.FSpellControl.GetSpell(FSpellID).FCastImages.ImageSequences[0]);
                    Aow.Game.World.Map.AoWHSMap.Current.PlaceHX(A, X, Y, L);
                }
            }

            if (ShowTarget)
            {
                if (Aow.Game.Engine.AoWHSSet.Current.FSpellControl.GetSpell(FSpellID).FCastImages.ImageSequences[0] != null)
                {
                    A = new ImageSequenceAnimation(null);
                    A.SetImageSequence(Aow.Game.Engine.AoWHSSet.Current.FSpellControl.GetSpell(FSpellID).FCastImages.ImageSequences[0]);
                    Aow.Game.World.Map.AoWHSMap.Current.PlaceHX(A, Field.Xhx, Field.Yhx, Field.Lhx);
                }
            }

            if (Aow.Game.World.Map.AoWHSMap.Current.FSeatedPlayer == Source.FPlayer)
            {
                Aow.Game.World.Map.AoWHSMap.Current.CenterViewTo(Field.Xhx, Field.Yhx, Field.Lhx);
            }
            else
            {
                if (!Aow.Game.World.Map.AoWHSMap.Current.ValidateObservation(Source.FPlayer, Field.Xhx, Field.Yhx, Field.Lhx, 0))
                {
                    Aow.Game.World.Map.AoWHSMap.Current.ValidateObservation(Source.FPlayer, X, Y, L, 0);
                }
            }

            if (Aow.Game.World.Map.AoWHSMap.Current.FSeatedPlayer == Source.FPlayer || Aow.Game.World.Map.AoWHSMap.Current.Watching(X, Y, L, 0) || Aow.Game.World.Map.AoWHSMap.Current.Watching(Field.Xhx, Field.Yhx, Field.Lhx, 0))
            {
                Aow.Game.Engine.AoWHSSet.Current.FSpellControl.GetSpell(FSpellID).FCastSounds.Play(0);
            }

            CastingDone(Source, Event.FEventMana);
            return false;
        }

        // CitySpells.pas:540
        public override bool CanActivate(AbstractUnit Source, ref string MessageText) => base.CanActivate(Source, ref MessageText);

        // CitySpells.pas:545
        public override void SetupTargetSelection(AbstractUnit Source, TargetSelectionControl TargetSelection)
        {
            base.SetupTargetSelection(Source, TargetSelection);
            TargetSelection.FTargetMode = TargetSelection.FTargetMode - Set<TargetSelectionModeElement>.Of(TargetSelectionModeElement.FixedLevel, TargetSelectionModeElement.ValidateDistance);
        }

        // CitySpells.pas:445
        public override CityTargetSpellTE CreateTE() => new TownGateTE(null);

        // CitySpells.pas:450
        public override bool SetupTE(CityTargetSpellTE Event, AbstractUnit Source, int X, int Y, int L)
        {
            byte Selection;
            ArmyHS Army;
            bool Found;
            MapLevel Level;
            HNWalkData Walk = default;
            bool Result = base.SetupTE(Event, Source, X, Y, L);
            if (Result)
            {
                Army = ((global::Aow.Game.Units.Army)Source.Owner).GetArmyHS();
                if (Army.FArmy.FPlayer == Aow.Game.World.Map.AoWHSMap.Current.FSeatedPlayer && Aow.Game.World.Map.AoWHSMap.Current.FSelectedArmy.GetArmyHS() == Army)
                {
                    Selection = Aow.Game.World.Map.AoWHSMap.Current.FSelectedArmy.GetSelection();
                }
                else
                {
                    Selection = 255;
                }

                Aow.Game.World.Movement.HNWalkData.InitHNWalk(ref Walk, X, Y, 0);
                Found = false;
                Level = Aow.Game.World.Map.AoWHSMap.Current.MapContainer.GetMapLevel(L);
                while (Walk.Radius < 4 && !Found)
                {
                    if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < Level.Width && Walk.Y < Level.Height)
                    {
                        Found = ValidTeleportLocation(Army, Selection, (sbyte)Walk.X, (sbyte)Walk.Y, (sbyte)L);
                    }

                    if (!Found)
                    {
                        Aow.Game.World.Movement.HNWalkData.WalkToNextHN(ref Walk);
                    }
                }

                if (Found)
                {
                    var id = Army.FArmy.CreateID(Selection);
                    ((TownGateTE)Event).FArmyID = id;
                    var packXYL = Aow.Game.World.Map.HexGeometry.PackXYL((sbyte)Walk.X, (sbyte)Walk.Y, (sbyte)L);
                    ((TownGateTE)Event).FTeleportLocation = packXYL;
                    Result = true;
                }
                else
                {
                    Result = false;
                }
            }

            return Result;
        }

        // CitySpells.pas:432
        public bool ValidTeleportLocation(ArmyHS Army, byte Selection, sbyte X, sbyte Y, sbyte L)
        {
            bool Result;
            AoWMapField Field;
            Field = (AoWMapField)Aow.Game.World.Map.AoWHSMap.Current.MapContainer.GetField(X, Y, L);
            if (Field.GetPlayer() != -1 && Field.GetPlayer() != Army.FArmy.FPlayer)
            {
                Result = false;
            }
            else
            {
                Result = true;
            }

            if (Result)
            {
                Result = Army.FArmy.ValidTerrainEx(Selection, Field.TerrainType, Field.OverlayType);
                // { CanAddUnitSelection returns arOK (zero) when the destination accepts the selection. }
                Result = Result && (Field.GetArmyHS() == null || Field.GetArmyHS().FArmy.CanAddUnitSelection(Army.FArmy, Selection) == AddUnitsResult.OK);
            }

            return Result;
        }

        private static readonly string TownGateRStr = "Town Gate";
    }
}

namespace Aow.Game.Commands
{

    public partial class CityEnchantmentSpellTE : CityTargetSpellTE
    {
        public CityEnchantmentSpellTE(ECustomNode owner) : base(owner)
        { }

        // CitySpells.pas:551
        public new const int ClassID = 131821;

        // NetworkE.pas:230

    }
}

namespace Aow.Game.Magic.Spells.City
{

    // enchantment ID, enchantment setting, AI priority, AI budget, AI cast situations
    public partial class CityEnchantmentSpell : CityTargetSpell
    {
        public CityEnchantmentID FEnchantmentID;
        public int FEnchantmentSpellID;
        public int FAIPriority;
        public int FAIBudget;
        public Set<CityEnchantmentCastSpellSituationElement> FAICastSituation;

        // CitySpells.pas:554
        public CityEnchantmentSpell(ECustomNode owner) : base(owner)
        {
            FAIPriority = -1;
        }

        // CitySpells.pas:576
        public override bool ExecuteTE(SpellTE Event, AbstractUnit Source, int ExecuteCount)
        {
            global::Aow.Game.World.Structures.City TargetCity;
            CityEnchantment Enchantment;
            TargetCity = ((CityTargetSpellTE)Event).GetSelectedCity();
            Enchantment = new CityEnchantment(null);
            Enchantment.FEnchantmentID = FEnchantmentID;
            Enchantment.FEnchantmentManaUpkeep = FManaUpkeep;
            var xyl = ((MapField)TargetCity.Owner).GetXYL();
            Enchantment.FEnchantmentLocation.Packed = xyl;
            Enchantment.FDispelMana = FEnchantmentSpellID;
            Enchantment.FPlayerIndex = Source.FPlayer;
            TargetCity.ActivateEnchantment(Enchantment);
            PlayDefaultCityEffects(Event.GetSource(), TargetCity);
            CastingDone(Source, Event.FEventMana);
            return false;
        }

        // CitySpells.pas:594
        public override void AIPrefetchCastSpellActions(AbstractUnit Source, AICastSpellActionList Actions)
        {
            bool Valid;
            AICastSpellAction Action;
            string Reason = "";
            sbyte X = default;
            sbyte Y = default;
            sbyte L = default;
            global::Aow.Game.World.Structures.City TargetCity;
            if (FAIPriority != -1)
            {
                Source.GetLocation(ref X, ref Y, ref L);
                TargetCity = (global::Aow.Game.World.Structures.City)Aow.Game.World.Map.AoWHSMap.Current.MapContainer.GetField(X, Y, L).FindNoneTransparentHS(typeof(global::Aow.Game.World.Structures.City));
                bool condition = TargetCity != null;
                if (condition)
                {
                    var aiCityEnchantmentUpkeepPriority = TargetCity.AICityEnchantmentUpkeepPriority(FEnchantmentID);
                    condition = AIRequestCastSpellExpense(Source, FBaseManaCost, FManaUpkeep, aiCityEnchantmentUpkeepPriority);
                }

                Valid = condition;
                // { Original uses mana upkeep for mana and city priority for upkeep. }
                Valid = Valid && ValidTargetCity(Source, TargetCity);
                Valid = Valid && ((Hero)Source).CanCastSpell(FSpellID, ref Reason);
                if (Valid)
                {
                    if (FAICastSituation.Contains(CityEnchantmentCastSpellSituationElement.LeaderInCity) && Source is Leader || FAICastSituation.Contains(CityEnchantmentCastSpellSituationElement.ccs4HexCity) && TargetCity.GetSize() == 4 || FAICastSituation.Contains(CityEnchantmentCastSpellSituationElement.ccs3HexCity) && TargetCity.GetSize() == 3)
                    {
                        Action = CreateCastSpellAction();
                        SetupCastSpellAction(Source, Action);
                        Action.FPriority = FAIPriority;
                        Action.SetOwner(Actions);
                    }
                }
            }
        }

        // CitySpells.pas:623
        public override bool AIExecuteCastSpellAction(AbstractUnit Source)
        {
            bool Result;
            string Reason = "";
            sbyte X = default;
            sbyte Y = default;
            sbyte L = default;
            global::Aow.Game.World.Structures.City TargetCity;
            Source.GetLocation(ref X, ref Y, ref L);
            TargetCity = (global::Aow.Game.World.Structures.City)Aow.Game.World.Map.AoWHSMap.Current.MapContainer.GetField(X, Y, L).FindNoneTransparentHS(typeof(global::Aow.Game.World.Structures.City));
            if (TargetCity != null && ValidTargetCity(Source, TargetCity) && ((Hero)Source).CanCastSpell(FSpellID, ref Reason))
            {
                AICastSpellExpenseMade(Source, FBaseManaCost, FManaUpkeep, FAIBudget);
                var hero = (Hero)Source;
                if (hero.CanCastSpellInstantly(FSpellID, FBaseManaCost, ref Reason))
                {
                    Result = TargetSelected(Source, X, Y, L);
                }
                else
                {
                    if (hero.FCastingSpellID != FSpellID)
                    {
                        hero.ExecuteStartCasting(FSpellID);
                        Result = true;
                    }
                    else
                    {
                        Result = true;
                    }
                }
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // CitySpells.pas:570
        public override bool ValidTargetCity(AbstractUnit Source, global::Aow.Game.World.Structures.City TargetCity) => base.ValidTargetCity(Source, TargetCity) && !TargetCity.FEnchantments.Contains(FEnchantmentID);

        // CitySpells.pas:560
        public override CityTargetSpellTE CreateTE() => new CityEnchantmentSpellTE(null);

        // CitySpells.pas:565
        public override bool SetupTE(CityTargetSpellTE Event, AbstractUnit Source, int X, int Y, int L) => base.SetupTE(Event, Source, X, Y, L);
    }

    public partial class FountainOfLife : CityEnchantmentSpell
    {
        // CitySpells.pas:649
        public FountainOfLife(ECustomNode owner) : base(owner)
        {
            FSpellName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.FountainOfLifeRStr);
            FSpellID = 52;
            FCityTargetMode = Set<CityTargetModeElement>.Of(CityTargetModeElement.Own);
            FBaseManaCost = 1;
            FSpellLevel = 4;
            FMagicSphere = Aow.Game.Magic.MagicSphere.Water;
            FEnchantmentID = CityEnchantmentID.FountainOfLife;
            FCastDuration = 20;
            FAIPriority = 500;
            FAICastSituation = Set<CityEnchantmentCastSpellSituationElement>.Of(CityEnchantmentCastSpellSituationElement.LeaderInCity);
            FAIBudget = 600;
            FEnchantmentSpellID = 50;
        }

        // CitySpells.pas:692
        public override bool ExecuteTE(SpellTE Event, AbstractUnit Source, int ExecuteCount)
        {
            bool Result = default;
            global::Aow.Game.World.Structures.City TargetCity = default;
            Animation A = default;
            Result = base.ExecuteTE(Event, Source, ExecuteCount);
            if (ExecuteCount == 0)
            {
                TargetCity = ((CityTargetSpellTE)Event).GetSelectedCity();
                A = new Animation(null);
                A.FAnimationLevel = 27;
                A.FOnShow = ShowFountain;
                A.FAnimationMap = Aow.Game.World.Map.AoWHSMap.Current;
                A.FTag = (int)(((uint)(int)TargetCity.GetXhx()) | ((uint)(TargetCity.GetYhx() << 8)));
                A.Activate();
            }

            return Result;
        }

        // CitySpells.pas:666
        public void ShowFountain(Animation Animation, int X, int Y, in SurfaceDesc SD)
        {
            int DX;
            int VY;
            int Offset;
            int CY = default;
            int CX = default;
            int VX;
            int T;
            int DY;
            Aow.Game.World.Map.HexGeometry.HXtoHP(Animation.FTag & 255, (int)(((uint)Animation.FTag) >> 8), ref CX, ref CY);
            CX = CX + 10 - X;
            CY = CY - Y + 10;
            // { Original resets the process-wide random seed for every draw. }
            RandSeed = 0;
            for (int I = 0; I <= 100; I++)
            {
                VX = Random(100) - 50;
                VY = Random(5) + 7;
                Offset = Round((double)VX * 0.1);
                T = Random(30);
                if (T < Animation.FFrameCounter && Animation.FFrameCounter - T < 90)
                {
                    T = (Animation.FFrameCounter - T) % 30;
                    DX = Round((double)(T * VX) / 30);
                    DY = VY * T - Round((double)((double)T * 0.25) * T);
                    if (T > 0)
                    {
                        Aow.Game.Engine.AoWHSSet.Current.FSpellControl.GetSpell(52).FCastImages.Show(0, CX + DX + Offset, CY - DY, 0, SD);
                    }
                }
            }

            if (Animation.FFrameCounter == 120)
            {
                Animation?.Dispose();
            }
        }
    }

    public partial class GoldRush : CityEnchantmentSpell
    {
        // CitySpells.pas:710
        public GoldRush(ECustomNode owner) : base(owner)
        {
            FSpellName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.GoldRushRStr);
            FSpellID = 53;
            FCityTargetMode = Set<CityTargetModeElement>.Of(CityTargetModeElement.Own);
            FBaseManaCost = 1;
            FSpellLevel = 4;
            FMagicSphere = Aow.Game.Magic.MagicSphere.Water;
            FEnchantmentID = CityEnchantmentID.GoldRush;
            FCastDuration = 20;
            FAIPriority = 500;
            FAICastSituation = Set<CityEnchantmentCastSpellSituationElement>.Of(CityEnchantmentCastSpellSituationElement.ccs4HexCity);
            FAIBudget = 600;
            FEnchantmentSpellID = 25;
        }
    }

    public partial class AntiMagicShell : CityEnchantmentSpell
    {
        // CitySpells.pas:727
        public AntiMagicShell(ECustomNode owner) : base(owner)
        {
            FSpellName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.AntiMagicShellRStr);
            FSpellID = 54;
            FCityTargetMode = Set<CityTargetModeElement>.Of(CityTargetModeElement.Own);
            FBaseManaCost = 1;
            FSpellLevel = 4;
            FMagicSphere = Aow.Game.Magic.MagicSphere.Cosmos;
            FEnchantmentID = CityEnchantmentID.AntiMagicShell;
            FCastDuration = 40;
            FAIPriority = 500;
            FAICastSituation = Set<CityEnchantmentCastSpellSituationElement>.Of(CityEnchantmentCastSpellSituationElement.LeaderInCity);
            FEnchantmentSpellID = 100;
        }

        // CitySpells.pas:743
        public override void PlayDefaultCityEffects(AbstractUnit Source, global::Aow.Game.World.Structures.City TargetCity)
        {
            base.PlayDefaultCityEffects(Source, TargetCity);
            bool condition = Source.FPlayer == Aow.Game.World.Map.AoWHSMap.Current.FSeatedPlayer;
            if (!condition)
            {
                var lhx = TargetCity.GetLhx();
                var yhx = TargetCity.GetYhx();
                var xhx = TargetCity.GetXhx();
                condition = Aow.Game.World.Map.AoWHSMap.Current.WatchingTerrain(xhx, yhx, lhx, 1);
            }

            if (condition)
            {
                Aow.Game.Engine.AoWHSSet.Current.FSpellControl.GetSpell(FSpellID).FCastSounds.PlayEx(1, -1, false, 30, SamplePanning.Center, null);
            }
        }
    }

    public partial class Warmonger : CityEnchantmentSpell
    {
        // CitySpells.pas:751
        public Warmonger(ECustomNode owner) : base(owner)
        {
            FSpellName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.WarmongerRStr);
            FSpellID = 58;
            FCityTargetMode = Set<CityTargetModeElement>.Of(CityTargetModeElement.Own);
            FEnchantmentID = CityEnchantmentID.Warmonger;
            FCastDuration = 30;
            FAIPriority = 600;
            FAICastSituation = Set<CityEnchantmentCastSpellSituationElement>.Of(CityEnchantmentCastSpellSituationElement.ccs4HexCity, CityEnchantmentCastSpellSituationElement.ccs3HexCity);
            FEnchantmentSpellID = 50;
        }
    }

    public partial class TownQuake : CityTargetSpell
    {
        // CitySpells.pas:764
        public TownQuake(ECustomNode owner) : base(owner)
        {
            FSpellName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Spells.City.TownQuake.TownQuakeRStr);
            FSpellID = 71;
            FCityTargetMode = Set<CityTargetModeElement>.Of(CityTargetModeElement.Enemy);
        }

        // CitySpells.pas:820
        public override bool ExecuteTE(SpellTE Event, AbstractUnit Source, int ExecuteCount)
        {
            bool Result;
            global::Aow.Game.World.Structures.City TargetCity;
            ArmyHSList Armies;
            if (ExecuteCount == 0)
            {
                TargetCity = ((CityTargetSpellTE)Event).GetSelectedCity();
                PlayDefaultCityEffects(Source, TargetCity);
                TargetCity.StartTownQuake();
                TargetCity.ValidateObservation(Source.FPlayer);
                Result = true;
            }
            else
            {
                if (ExecuteCount == 50)
                {
                    TargetCity = ((CityTargetSpellTE)Event).GetSelectedCity();
                    TargetCity.StopTownQuake();
                    Armies = new ArmyHSList(null);
                    if (Aow.Game.World.Map.AoWHSMap.Current.Random(10) < 7)
                    {
                        TargetCity.SetWallType(0);
                    }

                    TargetCity.ListArmies(Armies);
                    for (int I = 0, last = Armies.Count - 1; I <= last; I++)
                    {
                        TriggerArmyDamage(Source, Armies.GetArmyHS(I));
                    }

                    Armies?.Dispose();
                    CastingDone(Source, Event.FEventMana);
                    Result = false;
                }
                else
                {
                    Result = true;
                }
            }

            return Result;
        }

        // CitySpells.pas:812
        public override bool ValidTargetCity(AbstractUnit Source, global::Aow.Game.World.Structures.City TargetCity)
        {
            bool Result = base.ValidTargetCity(Source, TargetCity);
            if (!Result || TargetCity.FEnchantments.Contains(CityEnchantmentID.AntiMagicShell))
            {
                Result = false;
            }
            else
            {
                Result = true;
            }

            return Result;
        }

        // CitySpells.pas:773
        public void TriggerArmyDamage(AbstractUnit Source, ArmyHS Army)
        {
            ArmyEventLog Log;
            bool Damaged;
            int Damage;
            AbstractUnit AUnit;
            Log = new ArmyEventLog(null);
            Log.FArmyText = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Spells.City.TownQuake.TownQuakeDamageRStr);
            Log.FPlayer = Source.FPlayer;
            var townQuake = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Spells.City.TownQuake.TownQuakeRStr);
            Log.FArmyMessages.Add(townQuake);
            Log.FArmyIcon = 2;
            Log.FArmyView.Setup(Army.FArmy, 255);
            Army.FArmy.BeginUpdate();
            Damaged = false;
            for (int I = 0, last = Army.FArmy.Count - 1; I <= last; I++)
            {
                AUnit = Army.FArmy.GetUnit(I);
                Damage = AUnit.ExecuteDamageRole(7, 5, Set<DamageType>.Of(DamageType.Physical));
                if (Damage > AUnit.GetHitPoints())
                {
                    Damage = AUnit.GetHitPoints();
                }

                if (Damage > 0)
                {
                    AUnit.SetHitPoints((sbyte)(AUnit.GetHitPoints() - Damage));
                    Damaged = true;
                }

                Log.FDamageValues.Add((byte)Damage);
                Log.FDamageTypes.Add(7);
            }

            if (Damaged)
            {
                Log.ValidateGameOver();
                var yhx = Army.GetYhx();
                var lhx = Army.GetLhx();
                var xhx = Army.GetXhx();
                Aow.Game.Events.EventLog.DistributeLocationEventLog(Log, Army.FArmy.FPlayer, xhx, yhx, lhx, true);
            }

            Log.Release();
            Army.FArmy.Validate();
            Army.FArmy.EndUpdate();
        }

        private static readonly string TownQuakeDamageRStr = "Town Quake Damage";
        private static readonly string TownQuakeRStr = "Town Quake";
    }

    public partial class Anarchy : CityTargetSpell
    {
        // CitySpells.pas:849
        public Anarchy(ECustomNode owner) : base(owner)
        {
            FSpellName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Spells.City.Anarchy.AnarchyRStr);
            FSpellID = 72;
            FCityTargetMode = Set<CityTargetModeElement>.Of(CityTargetModeElement.Enemy);
        }

        // CitySpells.pas:875
        public override bool ExecuteTE(SpellTE Event, AbstractUnit Source, int ExecuteCount)
        {
            bool Result = default;
            global::Aow.Game.World.Structures.City TargetCity = default;
            LocationTextEventLog Log = default;
            ImageSequenceAnimation A = default;
            if (ExecuteCount == 0)
            {
                TargetCity = ((CityTargetSpellTE)Event).GetSelectedCity();
                Log = new LocationTextEventLog(null);
                Log.FPlayer = Source.FPlayer;
                Log.FText = RtlUnit.Format(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.XCastRStr), [FSpellName]);
                var lhx = TargetCity.GetLhx();
                var yhx = TargetCity.GetYhx();
                Log.SetLocation(TargetCity.GetXhx(), yhx, lhx);
                Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(TargetCity.FPlayer).FEventLogbook.AddEvent(Log);
                Log.Release();
                PlayDefaultCityEffects(Source, TargetCity);
                TargetCity.ValidateObservation(Source.FPlayer);
                if (TargetCity.Resource.ImageSequenceList.ImageSequences[16] != null)
                {
                    A = new ImageSequenceAnimation(null);
                    A.SetImageSequence(TargetCity.Resource.ImageSequenceList.ImageSequences[16]);
                    A.SetXhpOffset(A.FXhpOffset - 16);
                    A.SetYhpOffset(A.FYhpOffset - 16);
                    A.FOnShow = ShowAnimation;
                    A.FLooped = true;
                    A.FFrameCount = 120;
                    var targetCityLhx = TargetCity.GetLhx();
                    var targetCityYhx = TargetCity.GetYhx();
                    var xhx = TargetCity.GetXhx();
                    Aow.Game.World.Map.AoWHSMap.Current.PlaceHX(A, xhx, targetCityYhx, targetCityLhx);
                }

                Result = true;
            }
            else
            {
                if (ExecuteCount == 50)
                {
                    TargetCity = ((CityTargetSpellTE)Event).GetSelectedCity();
                    TargetCity.ExecuteAnarchy();
                    CastingDone(Source, Event.FEventMana);
                    Result = false;
                }
                else
                {
                    Result = true;
                }
            }

            return Result;
        }

        // CitySpells.pas:858
        public override bool ValidTargetCity(AbstractUnit Source, global::Aow.Game.World.Structures.City TargetCity)
        {
            bool Result = base.ValidTargetCity(Source, TargetCity);
            if (!Result || TargetCity.FPlayer == 0 || TargetCity.FEnchantments.Contains(CityEnchantmentID.AntiMagicShell))
            {
                Result = false;
            }
            else
            {
                Result = true;
            }

            return Result;
        }

        // CitySpells.pas:867
        public void ShowAnimation(Animation Animation, int X, int Y, in SurfaceDesc SD)
        {
            var imageSequenceAnimation = (ImageSequenceAnimation)Animation;
            if (imageSequenceAnimation.FImageSequence.GetUnloopedFrameIndex(imageSequenceAnimation.FFrameCounter) == 0 && Aow.Game.Magic.Abilities.GameTables.AoWEngine.FAnimationDetail != 0)
            {
                var random = Random(2);
                Aow.Game.Audio.AudioPlayback.PlaySample(Aow.Game.Magic.Abilities.AbilityRules.ExplosionSFX[random]);
            }
        }

        private static readonly string AnarchyRStr = "Anarchy";
    }

    public partial class AnimateRuins : CityTargetSpell
    {
        // CitySpells.pas:915
        public AnimateRuins(ECustomNode owner) : base(owner)
        {
            FSpellName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Spells.City.AnimateRuins.AnimateRuinsRStr);
            FSpellID = 76;
            FCityTargetMode = Set<CityTargetModeElement>.Of(CityTargetModeElement.Enemy, CityTargetModeElement.Razed);
        }

        // CitySpells.pas:938
        public override bool ExecuteTE(SpellTE Event, AbstractUnit Source, int ExecuteCount)
        {
            bool Result = default;
            global::Aow.Game.World.Structures.City TargetCity = default;
            LocationTextEventLog Log = default;
            ImageSequenceAnimation A = default;
            int Count = default;
            global::Aow.Game.Magic.Abilities.Unit AUnit = default;
            if (ExecuteCount == 0)
            {
                TargetCity = ((CityTargetSpellTE)Event).GetSelectedCity();
                Log = new LocationTextEventLog(null);
                Log.FPlayer = Source.FPlayer;
                var animateRuins = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Spells.City.AnimateRuins.AnimateRuinsRStr);
                Log.FText = RtlUnit.Format(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.XCastRStr), [animateRuins]);
                var lhx = TargetCity.GetLhx();
                var yhx = TargetCity.GetYhx();
                Log.SetLocation(TargetCity.GetXhx(), yhx, lhx);
                var targetCityYhx = TargetCity.GetYhx();
                var targetCityLhx = TargetCity.GetLhx();
                var xhx = TargetCity.GetXhx();
                Aow.Game.Events.EventLog.DistributeLocationEventLog(Log, Source.FPlayer, xhx, targetCityYhx, targetCityLhx, false);
                Log.Release();
                PlayDefaultCityEffects(Source, TargetCity);
                TargetCity.ValidateObservation(Source.FPlayer);
                if (TargetCity.Resource.ImageSequenceList.ImageSequences[20] != null)
                {
                    A = new ImageSequenceAnimation(null);
                    A.SetImageSequence(TargetCity.Resource.ImageSequenceList.ImageSequences[20]);
                    A.SetXhpOffset(A.FXhpOffset - 16);
                    A.SetYhpOffset(A.FYhpOffset - 16);
                    A.FOnShow = ShowAnimation;
                    var lhx2 = TargetCity.GetLhx();
                    var yhx2 = TargetCity.GetYhx();
                    var targetCityXhx = TargetCity.GetXhx();
                    Aow.Game.World.Map.AoWHSMap.Current.PlaceHX(A, targetCityXhx, yhx2, lhx2);
                }

                Result = true;
            }
            else
            {
                if (ExecuteCount == 50)
                {
                    TargetCity = ((CityTargetSpellTE)Event).GetSelectedCity();
                    TargetCity.SetRace(Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(Source.FPlayer).FPlayerRace);
                    TargetCity.SetRazed(false);
                    var size = TargetCity.GetSize();
                    Count = Aow.Game.World.Map.AoWHSMap.Current.Random(size * 2) + 1;
                    for (int I = 0; I <= Count; I++)
                    {
                        AUnit = new global::Aow.Game.Magic.Abilities.Unit(null);
                        var random = Aow.Game.World.Map.AoWHSMap.Current.Random(2);
                        AUnit.SetUnitResource(Aow.Game.Engine.AoWHSSet.Current.FUnitResources.GetUnitResource(Aow.Game.Magic.Spells.City.AnimateRuins.TAnimateRuins_ExecuteTE__UndeadUnits[random]));
                        var lhx = TargetCity.GetLhx();
                        var yhx = TargetCity.GetYhx();
                        AUnit.Place(TargetCity.GetXhx(), yhx, lhx, Source.FPlayer, Set<UnitPlaceModeElement>.Of(UnitPlaceModeElement.ScanArea));
                        AUnit.Release();
                    }

                    CastingDone(Source, Event.FEventMana);
                    Result = false;
                }
                else
                {
                    Result = true;
                }
            }

            return Result;
        }

        // CitySpells.pas:924
        public override bool ValidTargetCity(AbstractUnit Source, global::Aow.Game.World.Structures.City TargetCity)
        {
            bool Result = base.ValidTargetCity(Source, TargetCity);
            return Result && TargetCity.GetRazed();
        }

        // CitySpells.pas:930
        public void ShowAnimation(Animation Animation, int X, int Y, in SurfaceDesc SD)
        {
            var imageSequenceAnimation = (ImageSequenceAnimation)Animation;
            if (imageSequenceAnimation.FImageSequence.GetUnloopedFrameIndex(imageSequenceAnimation.FFrameCounter) == 0 && Aow.Game.Magic.Abilities.GameTables.AoWEngine.FAnimationDetail != 0)
            {
                var random = Random(2);
                Aow.Game.Audio.AudioPlayback.PlaySample(Aow.Game.Magic.Abilities.AbilityRules.ExplosionSFX[random]);
            }
        }

        private static readonly string AnimateRuinsRStr = "Animate Ruins";
        private static global::Aow.Game.Engine.Buffers.FixedArray_int_2 TAnimateRuins_ExecuteTE__UndeadUnits = global::Aow.Game.Engine.Buffers.FixedArray_int_2.From([198, 201]);
    }
}
