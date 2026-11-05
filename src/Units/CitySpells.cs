// Translated from CitySpells.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Exception = Aow.Rtl.Exception;
using System.Runtime.CompilerServices;
using Aow.Units.Engine;
using Aow.Units.GFXE;
using Aow.Units.Sound;
using static Aow.Units.Sound.Sound;
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngine;
using Aow.Units.City;
using Aow.Units.EventLog;
using static Aow.Units.EventLog.EventLog;
using Aow.Units.Animation;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;

namespace Aow.Units.CitySpells
{
    public enum TCityTargetModeElement : byte
    {
        ctmOwn = 0,
        ctmEnemy = 1,
        ctmSkipSource = 2,
        ctmRazed = 3
    }

    public enum TCityEnchantmentCastSpellSituationElement : byte
    {
        ccsLeaderInCity = 0,
        ccs4HexCity = 1,
        ccs3HexCity = 2
    }

    // { ot=1 }
    // packed target coordinates. target owner
    public partial class TCityTargetSpellTE : TSpellTE
    {
        public TCityTargetSpellTE(TECustomNode owner) : base(owner)
        { }

        // CitySpells.pas:170
        public new const int ClassID = 135949;

        public TXYL FTargetLocation;
        public sbyte FTargetPlayer;
        // CitySpells.pas:175
        public override void ReadWrite(TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwInteger(ref FTargetLocation.XYL, 20);
            Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FTargetPlayer), 21);
        }

        // CitySpells.pas:189
        public override bool Validate(THero A1)
        {
            bool Result = default;
            TCity TargetCity = default;
            Result = base.Validate(A1);
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
        public TCity GetSelectedCity()
        {
            TCity Result = default;
            var call__1 = AoWHSMap.MapContainer.GetFieldXYL(FTargetLocation.XYL);
            var call__2 = call__1.FindNoneTransparentHS(typeof(TCity));
            Result = (TCity)call__2;
            return Result;
        }

        // NetworkE.pas:230


    }

    // source unit ID. city target modes
    public partial class TCityTargetSpell : TSpell
    {
        public int FSourceID;
        public TSet<TCityTargetModeElement> FCityTargetMode;
        // CitySpells.pas:209
        public TCityTargetSpell(TECustomNode owner) : base(owner)
        {
            FSpellType = TSpellType.stGlobal;
        }

        // CitySpells.pas:377
        public override bool Activate(TAbstractUnit A1)
        {
            bool Result = default;
            AnsiString Reason = default;
            if (CanActivate(A1, ref Reason))
            {
                FSourceID = A1.FUnitID;
                SetupTargetSelection(A1, AoWHSMap.FTargetSelection);
                AoWHSMap.FTargetSelection.Activate();
                Result = true;
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // CitySpells.pas:288
        public virtual void OnTargetSelectionChange(TObject Sender)
        {
            TTargetSelectionControl TargetSelection = default;
            TAbstractUnit Source = default;
            bool Valid = default;
            TargetSelection = (TTargetSelectionControl)Sender;
            var call__3 = AoWHSMap.FUnitControl.FindUnit(FSourceID);
            Source = call__3;
            if (Source != null)
            {
                switch (TargetSelection.FChange)
                {
                    case TTargetSelectionChange.tscDeactivated:
                        TargetSelection.FOnChange.Remove(OnTargetSelectionChange);
                        TargetSelection.Deactivate();
                        break;
                    case TTargetSelectionChange.tscSelected:
                        ValidateTarget(TargetSelection, TargetSelection.FTargetX, TargetSelection.FTargetY, TargetSelection.FTargetL, ref Valid);
                        if (Valid)
                        {
                            var call__4 = TargetSelected(Source, TargetSelection.FTargetX, TargetSelection.FTargetY, TargetSelection.FTargetL);
                            if (call__4)
                            {
                                TargetSelection.FOnChange.Remove(OnTargetSelectionChange);
                                TargetSelection.Deactivate();
                            }
                        }

                        break;
                }
            }
            else
            {
                TargetSelection.FOnChange.Remove(OnTargetSelectionChange);
                TargetSelection.Deactivate();
            }
        }

        // CitySpells.pas:215
        public virtual void PlayDefaultCityEffects(TAbstractUnit Source, TCity TargetCity)
        {
            TMapLevel Level = default;
            TImageSequenceAnimation A = default;
            THNWalkData Walk = default;
            bool PlaySound = default;
            PlaySound = false;
            var call__5 = TargetCity.GetLhx();
            var call__6 = AoWHSMap.MapContainer.GetMapLevel(call__5);
            Level = call__6;
            var call__7 = TargetCity.GetYhx();
            var call__8 = TargetCity.GetXhx();
            InitHNWalk(ref Walk, call__8, call__7, 0);
            while (Walk.HN < ((TStructureResource)TargetCity.Resource).TerrainList.Count)
            {
                bool bool__9 = Walk.X >= 0 && Walk.Y >= 0 && Walk.X < Level.Width && Walk.Y < Level.Height;
                if (bool__9)
                {
                    var call__10 = ((TStructureResource)TargetCity.Resource).TerrainList.Used[Walk.HN];
                    bool__9 = call__10;
                }

                if (bool__9)
                {
                    bool bool__11 = Source.FPlayer == AoWHSMap.FSeatedPlayer;
                    if (!bool__11)
                    {
                        var call__12 = TargetCity.GetLhx();
                        var call__13 = AoWHSMap.TerrainVisible((sbyte)Walk.X, (sbyte)Walk.Y, call__12, 0);
                        bool__11 = call__13;
                    }

                    if (bool__11)
                    {
                        // { Original passes the city layer before the radius. }
                        A = new TImageSequenceAnimation(null);
                        var call__14 = AoWHSSet.FSpellControl.GetSpell(FSpellID);
                        var call__15 = call__14.FCastImages.ImageSequences[0];
                        A.SetImageSequence(call__15);
                        var call__16 = Random(5);
                        A.FFrameCounter = call__16;
                        A.FAnimationLevel = 27;
                        var call__17 = TargetCity.GetLhx();
                        AoWHSMap.PlaceHX(A, Walk.X, Walk.Y, call__17);
                        bool bool__18 = Source.FPlayer == AoWHSMap.FSeatedPlayer;
                        if (!bool__18)
                        {
                            var call__19 = TargetCity.GetLhx();
                            var call__20 = AoWHSMap.WatchingTerrain((sbyte)Walk.X, (sbyte)Walk.Y, call__19, 0);
                            bool__18 = call__20;
                        }

                        if (bool__18)
                        {
                            PlaySound = true;
                        }
                    }
                }

                WalkToNextHN(ref Walk);
            }

            if (PlaySound)
            {
                var call__21 = AoWHSSet.FSpellControl.GetSpell(FSpellID);
                call__21.FCastSounds.Play(0);
            }
        }

        // CitySpells.pas:323
        public virtual void SetupTargetSelection(TAbstractUnit Source, TTargetSelectionControl TargetSelection)
        {
            sbyte X = default;
            sbyte Y = default;
            sbyte L = default;
            TargetSelection.Initialize(Source);
            TargetSelection.FTargetMode = TargetSelection.FTargetMode + TSet<TTargetSelectionModeElement>.Of(TTargetSelectionModeElement.tmValidateDistance, TTargetSelectionModeElement.tmExternalFloater);
            var call__22 = TranslateRStr(SelectTargetCityForXOnMapRStr);
            var call__23 = Rtl.Rtl.Format(call__22, [FSpellName]);
            TargetSelection.FText.Text = call__23;
            TargetSelection.FTitle = FSpellName;
            var call__24 = TranslateRStr(CancelRStr);
            TargetSelection.FCancelText = call__24;
            var call__25 = AoWHSSet.FSpellControl.GetSpell(FSpellID);
            var call__26 = call__25.FCastImages.ImageSequences[20];
            var call__27 = call__26.GetPreviewImage();
            TargetSelection.FSourceImage = call__27;
            var call__28 = AoWHSSet.FSpellControl.GetSpell(FSpellID);
            var call__29 = call__28.FCastImages.ImageSequences[21];
            var call__30 = call__29.GetPreviewImage();
            TargetSelection.FValidTargetImage = call__30;
            var call__31 = AoWHSSet.FSpellControl.GetSpell(FSpellID);
            var call__32 = call__31.FCastImages.ImageSequences[22];
            var call__33 = call__32.GetPreviewImage();
            TargetSelection.FInvalidTargetImage = call__33;
            Source.GetLocation(ref X, ref Y, ref L);
            TargetSelection.FSourceX = X;
            TargetSelection.FSourceY = Y;
            TargetSelection.FSourceL = L;
            TargetSelection.FRange = 12;
            if (TargetSelection.FOnChange.Count > 1)
            {
                // { Original rejects two existing listeners. }
                throw Exception.Create((AnsiString)"oohps");
            }

            TargetSelection.FOnChange.Add(OnTargetSelectionChange);
            TargetSelection.FOnValidateTarget = ValidateTarget;
        }

        // CitySpells.pas:359
        public virtual bool TargetSelected(TAbstractUnit Source, int X, int Y, int L)
        {
            bool Result = default;
            TCityTargetSpellTE Event = default;
            var call__34 = AoWHSMap.FPlayers.GetPlayers(Source.FPlayer);
            var call__35 = call__34.GetBusy();
            if (!call__35)
            {
                Event = CreateTE();
                if (SetupTE(Event, Source, X, Y, L))
                {
                    var call__36 = AoWHSMap.FTokenManager.GetTokenControl(Source.FPlayer);
                    call__36.AddEvent(Event);
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
        public virtual bool ValidTargetCity(TAbstractUnit Source, TCity TargetCity)
        {
            bool Result = default;
            sbyte X = default;
            sbyte Y = default;
            sbyte L = default;
            TCity Found = default;
            Result = (FCityTargetMode.Contains(TCityTargetModeElement.ctmOwn) && TargetCity.FPlayer == Source.FPlayer) || (FCityTargetMode.Contains(TCityTargetModeElement.ctmEnemy) && TargetCity.FPlayer != Source.FPlayer);
            bool bool__37 = Result;
            if (bool__37)
            {
                var call__38 = TargetCity.GetRazed();
                bool__37 = !call__38 || FCityTargetMode.Contains(TCityTargetModeElement.ctmRazed);
            }

            Result = bool__37;
            if (Result && FCityTargetMode.Contains(TCityTargetModeElement.ctmSkipSource))
            {
                Source.GetLocation(ref X, ref Y, ref L);
                var call__39 = AoWHSMap.MapContainer.GetField(X, Y, L);
                var call__40 = call__39.FindNoneTransparentHS(typeof(TCity));
                Found = (TCity)call__40;
                Result = TargetCity != Found;
            }

            return Result;
        }

        // CitySpells.pas:346
        public virtual TCityTargetSpellTE CreateTE()
        {
            TCityTargetSpellTE Result = default;
            Result = new TCityTargetSpellTE(null);
            return Result;
        }

        // CitySpells.pas:351
        public virtual bool SetupTE(TCityTargetSpellTE Event, TAbstractUnit Source, int X, int Y, int L)
        {
            bool Result = default;
            Event.Initialize(FSpellID, (byte)Source.FPlayer, Source.FUnitID, FBaseManaCost);
            Event.SetLocation((sbyte)X, (sbyte)Y, (sbyte)L);
            var call__41 = Event.GetSelectedCity();
            Event.FTargetPlayer = call__41.FPlayer;
            Result = true;
            return Result;
        }

        // CitySpells.pas:262
        public void ValidateTarget(TTargetSelectionControl TargetSelection, sbyte X, sbyte Y, sbyte L, ref bool Valid)
        {
            TAbstractUnit Source = default;
            TMapLevel Level = default;
            TAoWMapField Field = default;
            TCity TargetCity = default;
            var call__42 = AoWHSMap.MapContainer.GetMapLevel(L);
            Level = call__42;
            var call__43 = AoWHSMap.FUnitControl.FindUnit(FSourceID);
            Source = call__43;
            if (Source != null && X >= 0 && Y >= 0 && X < Level.Width && Y < Level.Height)
            {
                Field = (TAoWMapField)Level.GetField(X, Y);
                bool bool__44 = !AoWHSMap.FExplorationEnabled;
                if (!bool__44)
                {
                    var call__45 = Field.GetExplored(Source.FPlayer);
                    bool__44 = call__45;
                }

                Valid = bool__44;
                if (Valid)
                {
                    TargetCity = (TCity)Field.FindNoneTransparentHS(typeof(TCity));
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
    }

    // army ID. destination coordinates
    public partial class TTownGateTE : TCityTargetSpellTE
    {
        public TTownGateTE(TECustomNode owner) : base(owner)
        { }

        // CitySpells.pas:397
        public new const int ClassID = 135950;

        public TArmyID FArmyID;
        public TXYL FTeleportLocation;
        // CitySpells.pas:391
        public override void Destroy()
        {
            FArmyID.Free();
            base.Destroy();
        }

        // CitySpells.pas:400
        public override void ReadWrite(TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwInteger(ref FTeleportLocation.XYL, 30);
            Stream.rwCreateEObject(ref FArmyID, null, 31);
        }

        // CitySpells.pas:407
        public override bool Validate(THero A1)
        {
            bool Result = default;
            byte Selection = default;
            TArmyHS Army = default;
            var call__46 = base.Validate(A1);
            bool bool__47 = call__46;
            if (bool__47)
            {
                var call__48 = FArmyID.Valid();
                bool__47 = call__48;
            }

            Result = bool__47;
            if (Result)
            {
                FArmyID.RetrieveArmy(ref Army, ref Selection);
                var call__49 = AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
                var call__50 = ((TTownGate)call__49).ValidTeleportLocation(Army, Selection, FTeleportLocation.X, FTeleportLocation.Y, FTeleportLocation.L);
                Result = call__50;
            }

            return Result;
        }

        // NetworkE.pas:230


    }

    public partial class TTownGate : TCityTargetSpell
    {
        // CitySpells.pas:421
        public TTownGate(TECustomNode owner) : base(owner)
        {
            var call__51 = TranslateRStr(TownGateRStr);
            FSpellName = call__51;
            FSpellID = 38;
            FCityTargetMode = TSet<TCityTargetModeElement>.Of(TCityTargetModeElement.ctmOwn, TCityTargetModeElement.ctmSkipSource);
            FBaseManaCost = 5;
            FSpellLevel = 3;
            FMagicSphere = TMagicSphere.msCosmos;
        }

        // CitySpells.pas:486
        public override bool ExecuteTE(TSpellTE A1, TAbstractUnit A2, int A3)
        {
            bool Result = default;
            bool ShowSource = default;
            bool ShowTarget = default;
            byte Selection = default;
            TArmyHS Army = default;
            TAoWMapField Field = default;
            TArmyHS NewArmy = default;
            sbyte X = default;
            sbyte Y = default;
            sbyte L = default;
            TImageSequenceAnimation A = default;
            A2.GetLocation(ref X, ref Y, ref L);
            ((TTownGateTE)A1).FArmyID.RetrieveArmy(ref Army, ref Selection);
            var call__52 = AoWHSMap.MapContainer.GetFieldXYL(((TTownGateTE)A1).FTeleportLocation.XYL);
            Field = (TAoWMapField)call__52;
            Army.FArmy.ResetDestinationPos();
            var call__53 = AoWHSMap.FSelectedArmy.GetArmyHS();
            if (call__53 == Army)
            {
                AoWHSMap.FSelectedArmy.Select(Army);
            }

            var call__54 = Army.FArmy.ValidateSelection(Selection);
            var call__55 = Army.FArmy.ValidateSelection(255);
            if (call__54 != call__55)
            {
                NewArmy = new TArmyHS(null);
                NewArmy.FArmy.SetPlayer(Army.FArmy.FPlayer);
                NewArmy.FArmy.AddUnitSelection(Army.FArmy, Selection);
                AoWHSMap.PlaceHX(NewArmy, ((TTownGateTE)A1).FTeleportLocation.X, ((TTownGateTE)A1).FTeleportLocation.Y, ((TTownGateTE)A1).FTeleportLocation.L);
            }
            else
            {
                if (Field.GetArmyHS() != null)
                {
                    var call__56 = Field.GetArmyHS();
                    call__56.FArmy.AddUnitSelection(Army.FArmy, Selection);
                }
                else
                {
                    Army.MoveTo(Field, 0);
                }
            }

            bool bool__57 = AoWHSMap.FSeatedPlayer == A2.FPlayer;
            if (!bool__57)
            {
                var call__58 = AoWHSMap.Visible(X, Y, L, 0);
                bool__57 = call__58;
            }

            ShowSource = bool__57;
            bool bool__59 = AoWHSMap.FSeatedPlayer == A2.FPlayer;
            if (!bool__59)
            {
                var call__60 = AoWHSMap.Visible(Field.Xhx, Field.Yhx, Field.Lhx, 0);
                bool__59 = call__60;
            }

            ShowTarget = bool__59;
            if (ShowSource)
            {
                var call__61 = AoWHSSet.FSpellControl.GetSpell(FSpellID);
                var call__62 = call__61.FCastImages.ImageSequences[0];
                if (call__62 != null)
                {
                    A = new TImageSequenceAnimation(null);
                    var call__63 = AoWHSSet.FSpellControl.GetSpell(FSpellID);
                    var call__64 = call__63.FCastImages.ImageSequences[0];
                    A.SetImageSequence(call__64);
                    AoWHSMap.PlaceHX(A, X, Y, L);
                }
            }

            if (ShowTarget)
            {
                var call__65 = AoWHSSet.FSpellControl.GetSpell(FSpellID);
                var call__66 = call__65.FCastImages.ImageSequences[0];
                if (call__66 != null)
                {
                    A = new TImageSequenceAnimation(null);
                    var call__67 = AoWHSSet.FSpellControl.GetSpell(FSpellID);
                    var call__68 = call__67.FCastImages.ImageSequences[0];
                    A.SetImageSequence(call__68);
                    AoWHSMap.PlaceHX(A, Field.Xhx, Field.Yhx, Field.Lhx);
                }
            }

            if (AoWHSMap.FSeatedPlayer == A2.FPlayer)
            {
                AoWHSMap.CenterViewTo(Field.Xhx, Field.Yhx, Field.Lhx);
            }
            else
            {
                var call__69 = AoWHSMap.ValidateObservation(A2.FPlayer, Field.Xhx, Field.Yhx, Field.Lhx, 0);
                if (!call__69)
                {
                    AoWHSMap.ValidateObservation(A2.FPlayer, X, Y, L, 0);
                }
            }

            bool bool__70 = AoWHSMap.FSeatedPlayer == A2.FPlayer;
            if (!bool__70)
            {
                var call__71 = AoWHSMap.Watching(X, Y, L, 0);
                bool__70 = call__71;
            }

            bool bool__72 = bool__70;
            if (!bool__72)
            {
                var call__73 = AoWHSMap.Watching(Field.Xhx, Field.Yhx, Field.Lhx, 0);
                bool__72 = call__73;
            }

            if (bool__72)
            {
                var call__74 = AoWHSSet.FSpellControl.GetSpell(FSpellID);
                call__74.FCastSounds.Play(0);
            }

            CastingDone(A2, A1.FEventMana);
            Result = false;
            return Result;
        }

        // CitySpells.pas:540
        public override bool CanActivate(TAbstractUnit A1, ref AnsiString A2)
        {
            bool Result = default;
            Result = base.CanActivate(A1, ref A2);
            return Result;
        }

        // CitySpells.pas:545
        public override void SetupTargetSelection(TAbstractUnit Source, TTargetSelectionControl TargetSelection)
        {
            base.SetupTargetSelection(Source, TargetSelection);
            TargetSelection.FTargetMode = TargetSelection.FTargetMode - TSet<TTargetSelectionModeElement>.Of(TTargetSelectionModeElement.tmFixedLevel, TTargetSelectionModeElement.tmValidateDistance);
        }

        // CitySpells.pas:445
        public override TCityTargetSpellTE CreateTE()
        {
            TCityTargetSpellTE Result = default;
            Result = new TTownGateTE(null);
            return Result;
        }

        // CitySpells.pas:450
        public override bool SetupTE(TCityTargetSpellTE Event, TAbstractUnit Source, int X, int Y, int L)
        {
            bool Result = default;
            byte Selection = default;
            TArmyHS Army = default;
            bool Found = default;
            TMapLevel Level = default;
            THNWalkData Walk = default;
            Result = base.SetupTE(Event, Source, X, Y, L);
            if (Result)
            {
                var call__75 = ((TArmy)Source.Owner).GetArmyHS();
                Army = call__75;
                bool bool__76 = Army.FArmy.FPlayer == AoWHSMap.FSeatedPlayer;
                if (bool__76)
                {
                    var call__77 = AoWHSMap.FSelectedArmy.GetArmyHS();
                    bool__76 = call__77 == Army;
                }

                if (bool__76)
                {
                    var call__78 = AoWHSMap.FSelectedArmy.GetSelection();
                    Selection = call__78;
                }
                else
                {
                    Selection = 255;
                }

                InitHNWalk(ref Walk, X, Y, 0);
                Found = false;
                var call__79 = AoWHSMap.MapContainer.GetMapLevel(L);
                Level = call__79;
                while (Walk.Radius < 4 && !Found)
                {
                    if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < Level.Width && Walk.Y < Level.Height)
                    {
                        var call__80 = ValidTeleportLocation(Army, Selection, (sbyte)Walk.X, (sbyte)Walk.Y, (sbyte)L);
                        Found = call__80;
                    }

                    if (!Found)
                    {
                        WalkToNextHN(ref Walk);
                    }
                }

                if (Found)
                {
                    var call__81 = Army.FArmy.CreateID(Selection);
                    ((TTownGateTE)Event).FArmyID = call__81;
                    var call__82 = PackXYL((sbyte)Walk.X, (sbyte)Walk.Y, (sbyte)L);
                    ((TTownGateTE)Event).FTeleportLocation = call__82;
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
        public bool ValidTeleportLocation(TArmyHS Army, byte Selection, sbyte X, sbyte Y, sbyte L)
        {
            bool Result = default;
            TAoWMapField Field = default;
            var call__83 = AoWHSMap.MapContainer.GetField(X, Y, L);
            Field = (TAoWMapField)call__83;
            var call__84 = Field.GetPlayer();
            bool bool__85 = call__84 != -1;
            if (bool__85)
            {
                var call__86 = Field.GetPlayer();
                bool__85 = call__86 != Army.FArmy.FPlayer;
            }

            if (bool__85)
            {
                Result = false;
            }
            else
            {
                Result = true;
            }

            if (Result)
            {
                var call__87 = Army.FArmy.ValidTerrainEx(Selection, Field.TerrainType, Field.OverlayType);
                Result = call__87;
                // { CanAddUnitSelection returns arOK (zero) when the destination accepts the selection. }
                bool bool__88 = Result;
                if (bool__88)
                {
                    var call__89 = Field.GetArmyHS();
                    bool bool__90 = call__89 == null;
                    if (!bool__90)
                    {
                        var call__91 = Field.GetArmyHS();
                        var call__92 = call__91.FArmy.CanAddUnitSelection(Army.FArmy, Selection);
                        bool__90 = call__92 == TAddUnitsResult.arOK;
                    }

                    bool__88 = bool__90;
                }

                Result = bool__88;
            }

            return Result;
        }
    }

    public partial class TCityEnchantmentSpellTE : TCityTargetSpellTE
    {
        public TCityEnchantmentSpellTE(TECustomNode owner) : base(owner)
        { }

        // CitySpells.pas:551
        public new const int ClassID = 131821;

        // NetworkE.pas:230


    }

    // enchantment ID, enchantment setting, AI priority, AI budget, AI cast situations
    public partial class TCityEnchantmentSpell : TCityTargetSpell
    {
        public TCityEnchantmentID FEnchantmentID;
        public int FEnchantmentSpellID;
        public int FAIPriority;
        public int FAIBudget;
        public TSet<TCityEnchantmentCastSpellSituationElement> FAICastSituation;
        // CitySpells.pas:554
        public TCityEnchantmentSpell(TECustomNode owner) : base(owner)
        {
            FAIPriority = -1;
        }

        // CitySpells.pas:576
        public override bool ExecuteTE(TSpellTE A1, TAbstractUnit A2, int A3)
        {
            bool Result = default;
            TCity TargetCity = default;
            TCityEnchantment Enchantment = default;
            TargetCity = ((TCityTargetSpellTE)A1).GetSelectedCity();
            Enchantment = new TCityEnchantment(null);
            Enchantment.FEnchantmentID = FEnchantmentID;
            Enchantment.FEnchantmentManaUpkeep = FManaUpkeep;
            var call__93 = ((TMapField)TargetCity.Owner).GetXYL();
            Enchantment.FEnchantmentLocation.XYL = call__93;
            Enchantment.FDispelMana = FEnchantmentSpellID;
            Enchantment.FPlayerIndex = A2.FPlayer;
            TargetCity.ActivateEnchantment(Enchantment);
            var call__94 = A1.GetSource();
            PlayDefaultCityEffects(call__94, TargetCity);
            CastingDone(A2, A1.FEventMana);
            Result = false;
            return Result;
        }

        // CitySpells.pas:594
        public override void AIPrefetchCastSpellActions(TAbstractUnit A1, TAICastSpellActionList A2)
        {
            bool Valid = default;
            TAICastSpellAction Action = default;
            AnsiString Reason = default;
            sbyte X = default;
            sbyte Y = default;
            sbyte L = default;
            TCity TargetCity = default;
            if (FAIPriority != -1)
            {
                A1.GetLocation(ref X, ref Y, ref L);
                var call__95 = AoWHSMap.MapContainer.GetField(X, Y, L);
                var call__96 = call__95.FindNoneTransparentHS(typeof(TCity));
                TargetCity = (TCity)call__96;
                bool bool__97 = TargetCity != null;
                if (bool__97)
                {
                    var call__98 = TargetCity.AICityEnchantmentUpkeepPriority(FEnchantmentID);
                    var call__99 = AIRequestCastSpellExpense(A1, FBaseManaCost, FManaUpkeep, call__98);
                    bool__97 = call__99;
                }

                Valid = bool__97;
                // { Original uses mana upkeep for mana and city priority for upkeep. }
                Valid = Valid && ValidTargetCity(A1, TargetCity);
                bool bool__100 = Valid;
                if (bool__100)
                {
                    var call__101 = ((THero)A1).CanCastSpell(FSpellID, ref Reason);
                    bool__100 = call__101;
                }

                Valid = bool__100;
                if (Valid)
                {
                    bool bool__102 = FAICastSituation.Contains(TCityEnchantmentCastSpellSituationElement.ccsLeaderInCity) && A1 is TLeader;
                    if (!bool__102)
                    {
                        bool bool__103 = FAICastSituation.Contains(TCityEnchantmentCastSpellSituationElement.ccs4HexCity);
                        if (bool__103)
                        {
                            var call__104 = TargetCity.GetSize();
                            bool__103 = call__104 == 4;
                        }

                        bool__102 = bool__103;
                    }

                    bool bool__105 = bool__102;
                    if (!bool__105)
                    {
                        bool bool__106 = FAICastSituation.Contains(TCityEnchantmentCastSpellSituationElement.ccs3HexCity);
                        if (bool__106)
                        {
                            var call__107 = TargetCity.GetSize();
                            bool__106 = call__107 == 3;
                        }

                        bool__105 = bool__106;
                    }

                    if (bool__105)
                    {
                        Action = CreateCastSpellAction();
                        SetupCastSpellAction(A1, Action);
                        Action.FPriority = FAIPriority;
                        Action.SetOwner(A2);
                    }
                }
            }
        }

        // CitySpells.pas:623
        public override bool AIExecuteCastSpellAction(TAbstractUnit A1)
        {
            bool Result = default;
            AnsiString Reason = default;
            sbyte X = default;
            sbyte Y = default;
            sbyte L = default;
            TCity TargetCity = default;
            A1.GetLocation(ref X, ref Y, ref L);
            var call__108 = AoWHSMap.MapContainer.GetField(X, Y, L);
            var call__109 = call__108.FindNoneTransparentHS(typeof(TCity));
            TargetCity = (TCity)call__109;
            bool bool__110 = TargetCity != null;
            if (bool__110)
            {
                var call__111 = ValidTargetCity(A1, TargetCity);
                bool__110 = call__111;
            }

            bool bool__112 = bool__110;
            if (bool__112)
            {
                var call__113 = ((THero)A1).CanCastSpell(FSpellID, ref Reason);
                bool__112 = call__113;
            }

            if (bool__112)
            {
                AICastSpellExpenseMade(A1, FBaseManaCost, FManaUpkeep, FAIBudget);
                var hero = (THero)A1;
                var call__115 = hero.CanCastSpellInstantly(FSpellID, FBaseManaCost, ref Reason);
                if (call__115)
                {
                    Result = TargetSelected(A1, X, Y, L);
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
        public override bool ValidTargetCity(TAbstractUnit Source, TCity TargetCity)
        {
            bool Result = default;
            Result = base.ValidTargetCity(Source, TargetCity);
            if (Result)
            {
                Result = !TargetCity.FEnchantments.Contains(FEnchantmentID);
            }

            return Result;
        }

        // CitySpells.pas:560
        public override TCityTargetSpellTE CreateTE()
        {
            TCityTargetSpellTE Result = default;
            Result = new TCityEnchantmentSpellTE(null);
            return Result;
        }

        // CitySpells.pas:565
        public override bool SetupTE(TCityTargetSpellTE Event, TAbstractUnit Source, int X, int Y, int L)
        {
            bool Result = default;
            Result = base.SetupTE(Event, Source, X, Y, L);
            return Result;
        }
    }

    public partial class TFountainOfLife : TCityEnchantmentSpell
    {
        // CitySpells.pas:649
        public TFountainOfLife(TECustomNode owner) : base(owner)
        {
            var call__116 = TranslateRStr(FountainOfLifeRStr);
            FSpellName = call__116;
            FSpellID = 52;
            FCityTargetMode = TSet<TCityTargetModeElement>.Of(TCityTargetModeElement.ctmOwn);
            FBaseManaCost = 1;
            FSpellLevel = 4;
            FMagicSphere = TMagicSphere.msWater;
            FEnchantmentID = TCityEnchantmentID.ceiFountainOfLife;
            FCastDuration = 20;
            FAIPriority = 500;
            FAICastSituation = TSet<TCityEnchantmentCastSpellSituationElement>.Of(TCityEnchantmentCastSpellSituationElement.ccsLeaderInCity);
            FAIBudget = 600;
            FEnchantmentSpellID = 50;
        }

        // CitySpells.pas:692
        public override bool ExecuteTE(TSpellTE A1, TAbstractUnit A2, int A3)
        {
            bool Result = default;
            TCity TargetCity = default;
            TAnimation A = default;
            Result = base.ExecuteTE(A1, A2, A3);
            if (A3 == 0)
            {
                TargetCity = ((TCityTargetSpellTE)A1).GetSelectedCity();
                A = new TAnimation(null);
                A.FAnimationLevel = 27;
                A.FOnShow = ShowFountain;
                A.FAnimationMap = AoWHSMap;
                var call__117 = TargetCity.GetXhx();
                var call__118 = TargetCity.GetYhx();
                A.FTag = (int)(((uint)(int)call__117) | ((uint)(call__118 << 8)));
                A.Activate();
            }

            return Result;
        }

        // CitySpells.pas:666
        public void ShowFountain(TAnimation Animation, int X, int Y, in TSurfaceDesc SD)
        {
            int I = default;
            int DX = default;
            int VY = default;
            int Offset = default;
            int CY = default;
            int CX = default;
            int VX = default;
            int T = default;
            int DY = default;
            HXtoHP(Animation.FTag & 255, (int)(((uint)Animation.FTag) >> 8), ref CX, ref CY);
            CX = CX + 10 - X;
            CY = CY - Y + 10;
            // { Original resets the process-wide random seed for every draw. }
            RandSeed = 0;
            {
                I = 0;
                byte end__119 = 100;
                for (; I <= end__119; I += 1)
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
                            var call__120 = AoWHSSet.FSpellControl.GetSpell(52);
                            call__120.FCastImages.Show(0, CX + DX + Offset, CY - DY, 0, SD);
                        }
                    }
                }
            }

            if (Animation.FFrameCounter == 120)
            {
                Animation.Free();
            }
        }
    }

    public partial class TGoldRush : TCityEnchantmentSpell
    {
        // CitySpells.pas:710
        public TGoldRush(TECustomNode owner) : base(owner)
        {
            var call__121 = TranslateRStr(GoldRushRStr);
            FSpellName = call__121;
            FSpellID = 53;
            FCityTargetMode = TSet<TCityTargetModeElement>.Of(TCityTargetModeElement.ctmOwn);
            FBaseManaCost = 1;
            FSpellLevel = 4;
            FMagicSphere = TMagicSphere.msWater;
            FEnchantmentID = TCityEnchantmentID.ceiGoldRush;
            FCastDuration = 20;
            FAIPriority = 500;
            FAICastSituation = TSet<TCityEnchantmentCastSpellSituationElement>.Of(TCityEnchantmentCastSpellSituationElement.ccs4HexCity);
            FAIBudget = 600;
            FEnchantmentSpellID = 25;
        }
    }

    public partial class TAntiMagicShell : TCityEnchantmentSpell
    {
        // CitySpells.pas:727
        public TAntiMagicShell(TECustomNode owner) : base(owner)
        {
            var call__122 = TranslateRStr(AntiMagicShellRStr);
            FSpellName = call__122;
            FSpellID = 54;
            FCityTargetMode = TSet<TCityTargetModeElement>.Of(TCityTargetModeElement.ctmOwn);
            FBaseManaCost = 1;
            FSpellLevel = 4;
            FMagicSphere = TMagicSphere.msCosmos;
            FEnchantmentID = TCityEnchantmentID.ceiAntiMagicShell;
            FCastDuration = 40;
            FAIPriority = 500;
            FAICastSituation = TSet<TCityEnchantmentCastSpellSituationElement>.Of(TCityEnchantmentCastSpellSituationElement.ccsLeaderInCity);
            FEnchantmentSpellID = 100;
        }

        // CitySpells.pas:743
        public override void PlayDefaultCityEffects(TAbstractUnit Source, TCity TargetCity)
        {
            base.PlayDefaultCityEffects(Source, TargetCity);
            bool bool__123 = Source.FPlayer == AoWHSMap.FSeatedPlayer;
            if (!bool__123)
            {
                var call__124 = TargetCity.GetLhx();
                var call__125 = TargetCity.GetYhx();
                var call__126 = TargetCity.GetXhx();
                var call__127 = AoWHSMap.WatchingTerrain(call__126, call__125, call__124, 1);
                bool__123 = call__127;
            }

            if (bool__123)
            {
                var call__128 = AoWHSSet.FSpellControl.GetSpell(FSpellID);
                call__128.FCastSounds.PlayEx(1, -1, false, 30, TSamplePanning.spCenter, null);
            }
        }
    }

    public partial class TWarmonger : TCityEnchantmentSpell
    {
        // CitySpells.pas:751
        public TWarmonger(TECustomNode owner) : base(owner)
        {
            var call__129 = TranslateRStr(WarmongerRStr);
            FSpellName = call__129;
            FSpellID = 58;
            FCityTargetMode = TSet<TCityTargetModeElement>.Of(TCityTargetModeElement.ctmOwn);
            FEnchantmentID = TCityEnchantmentID.ceiWarmonger;
            FCastDuration = 30;
            FAIPriority = 600;
            FAICastSituation = TSet<TCityEnchantmentCastSpellSituationElement>.Of(TCityEnchantmentCastSpellSituationElement.ccs4HexCity, TCityEnchantmentCastSpellSituationElement.ccs3HexCity);
            FEnchantmentSpellID = 50;
        }
    }

    public partial class TTownQuake : TCityTargetSpell
    {
        // CitySpells.pas:764
        public TTownQuake(TECustomNode owner) : base(owner)
        {
            var call__130 = TranslateRStr(TownQuakeRStr);
            FSpellName = call__130;
            FSpellID = 71;
            FCityTargetMode = TSet<TCityTargetModeElement>.Of(TCityTargetModeElement.ctmEnemy);
        }

        // CitySpells.pas:820
        public override bool ExecuteTE(TSpellTE A1, TAbstractUnit A2, int A3)
        {
            bool Result = default;
            TCity TargetCity = default;
            TArmyHSList Armies = default;
            int I = default;
            if (A3 == 0)
            {
                TargetCity = ((TCityTargetSpellTE)A1).GetSelectedCity();
                PlayDefaultCityEffects(A2, TargetCity);
                TargetCity.StartTownQuake();
                TargetCity.ValidateObservation(A2.FPlayer);
                Result = true;
            }
            else
            {
                if (A3 == 50)
                {
                    TargetCity = ((TCityTargetSpellTE)A1).GetSelectedCity();
                    TargetCity.StopTownQuake();
                    Armies = new TArmyHSList(null);
                    var call__131 = AoWHSMap.Random(10);
                    if (call__131 < 7)
                    {
                        TargetCity.SetWallType(0);
                    }

                    TargetCity.ListArmies(Armies);
                    {
                        I = 0;
                        int end__132 = Armies.Count - 1;
                        for (; I <= end__132; I += 1)
                        {
                            var call__133 = Armies.GetArmyHS(I);
                            TriggerArmyDamage(A2, call__133);
                        }
                    }

                    Armies.Free();
                    CastingDone(A2, A1.FEventMana);
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
        public override bool ValidTargetCity(TAbstractUnit Source, TCity TargetCity)
        {
            bool Result = default;
            Result = base.ValidTargetCity(Source, TargetCity);
            if (!Result || TargetCity.FEnchantments.Contains(TCityEnchantmentID.ceiAntiMagicShell))
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
        public void TriggerArmyDamage(TAbstractUnit Source, TArmyHS Army)
        {
            TArmyEventLog Log = default;
            bool Damaged = default;
            int I = default;
            int Damage = default;
            TAbstractUnit AUnit = default;
            Log = new TArmyEventLog(null);
            var call__134 = TranslateRStr(TownQuakeDamageRStr);
            Log.FArmyText = call__134;
            Log.FPlayer = Source.FPlayer;
            var call__135 = TranslateRStr(TownQuakeRStr);
            Log.FArmyMessages.Add(call__135);
            Log.FArmyIcon = 2;
            Log.FArmyView.Setup(Army.FArmy, 255);
            Army.FArmy.BeginUpdate();
            Damaged = false;
            {
                I = 0;
                int end__136 = Army.FArmy.Count - 1;
                for (; I <= end__136; I += 1)
                {
                    var call__137 = Army.FArmy.GetUnit(I);
                    AUnit = call__137;
                    Damage = AUnit.ExecuteDamageRole(7, 5, TSet<TDamageType>.Of(TDamageType.dtPhysical));
                    if (Damage > AUnit.GetHitPoints())
                    {
                        Damage = AUnit.GetHitPoints();
                    }

                    if (Damage > 0)
                    {
                        var call__138 = AUnit.GetHitPoints();
                        AUnit.SetHitPoints((sbyte)(call__138 - Damage));
                        Damaged = true;
                    }

                    Log.FDamageValues.Add((byte)Damage);
                    Log.FDamageTypes.Add(7);
                }
            }

            if (Damaged)
            {
                Log.ValidateGameOver();
                var call__139 = Army.GetYhx();
                var call__140 = Army.GetLhx();
                var call__141 = Army.GetXhx();
                DistributeLocationEventLog(Log, Army.FArmy.FPlayer, call__141, call__139, call__140, true);
            }

            Log.Release();
            Army.FArmy.Validate();
            Army.FArmy.EndUpdate();
        }
    }

    public partial class TAnarchy : TCityTargetSpell
    {
        // CitySpells.pas:849
        public TAnarchy(TECustomNode owner) : base(owner)
        {
            var call__142 = TranslateRStr(AnarchyRStr);
            FSpellName = call__142;
            FSpellID = 72;
            FCityTargetMode = TSet<TCityTargetModeElement>.Of(TCityTargetModeElement.ctmEnemy);
        }

        // CitySpells.pas:875
        public override bool ExecuteTE(TSpellTE A1, TAbstractUnit A2, int A3)
        {
            bool Result = default;
            TCity TargetCity = default;
            TLocationTextEventLog Log = default;
            TImageSequenceAnimation A = default;
            if (A3 == 0)
            {
                TargetCity = ((TCityTargetSpellTE)A1).GetSelectedCity();
                Log = new TLocationTextEventLog(null);
                Log.FPlayer = A2.FPlayer;
                var call__143 = TranslateRStr(XCastRStr);
                var call__144 = Rtl.Rtl.Format(call__143, [FSpellName]);
                Log.FText = call__144;
                var call__145 = TargetCity.GetLhx();
                var call__146 = TargetCity.GetYhx();
                var call__147 = TargetCity.GetXhx();
                Log.SetLocation(call__147, call__146, call__145);
                var call__148 = AoWHSMap.FPlayers.GetPlayers(TargetCity.FPlayer);
                call__148.FEventLogbook.AddEvent(Log);
                Log.Release();
                PlayDefaultCityEffects(A2, TargetCity);
                TargetCity.ValidateObservation(A2.FPlayer);
                var call__149 = TargetCity.Resource.ImageSequenceList.ImageSequences[16];
                if (call__149 != null)
                {
                    A = new TImageSequenceAnimation(null);
                    var call__150 = TargetCity.Resource.ImageSequenceList.ImageSequences[16];
                    A.SetImageSequence(call__150);
                    A.SetXhpOffset(A.FXhpOffset - 16);
                    A.SetYhpOffset(A.FYhpOffset - 16);
                    A.FOnShow = ShowAnimation;
                    A.FLooped = true;
                    A.FFrameCount = 120;
                    var call__151 = TargetCity.GetLhx();
                    var call__152 = TargetCity.GetYhx();
                    var call__153 = TargetCity.GetXhx();
                    AoWHSMap.PlaceHX(A, call__153, call__152, call__151);
                }

                Result = true;
            }
            else
            {
                if (A3 == 50)
                {
                    TargetCity = ((TCityTargetSpellTE)A1).GetSelectedCity();
                    TargetCity.ExecuteAnarchy();
                    CastingDone(A2, A1.FEventMana);
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
        public override bool ValidTargetCity(TAbstractUnit Source, TCity TargetCity)
        {
            bool Result = default;
            Result = base.ValidTargetCity(Source, TargetCity);
            if (!Result || TargetCity.FPlayer == 0 || TargetCity.FEnchantments.Contains(TCityEnchantmentID.ceiAntiMagicShell))
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
        public void ShowAnimation(TAnimation Animation, int X, int Y, in TSurfaceDesc SD)
        {
            var imageSequenceAnimation = (TImageSequenceAnimation)Animation;
            var call__155 = imageSequenceAnimation.FImageSequence.GetUnloopedFrameIndex(imageSequenceAnimation.FFrameCounter);
            if (call__155 == 0 && AoWEngine.FAnimationDetail != 0)
            {
                var call__156 = Random(2);
                PlaySample(ExplosionSFX[call__156]);
            }
        }
    }

    public partial class TAnimateRuins : TCityTargetSpell
    {
        // CitySpells.pas:915
        public TAnimateRuins(TECustomNode owner) : base(owner)
        {
            var call__157 = TranslateRStr(AnimateRuinsRStr);
            FSpellName = call__157;
            FSpellID = 76;
            FCityTargetMode = TSet<TCityTargetModeElement>.Of(TCityTargetModeElement.ctmEnemy, TCityTargetModeElement.ctmRazed);
        }

        // CitySpells.pas:938
        public override bool ExecuteTE(TSpellTE A1, TAbstractUnit A2, int A3)
        {
            bool Result = default;
            TCity TargetCity = default;
            TLocationTextEventLog Log = default;
            TImageSequenceAnimation A = default;
            int I = default;
            int Count = default;
            TUnit AUnit = default;
            if (A3 == 0)
            {
                TargetCity = ((TCityTargetSpellTE)A1).GetSelectedCity();
                Log = new TLocationTextEventLog(null);
                Log.FPlayer = A2.FPlayer;
                var call__158 = TranslateRStr(AnimateRuinsRStr);
                var call__159 = TranslateRStr(XCastRStr);
                var call__160 = Rtl.Rtl.Format(call__159, [call__158]);
                Log.FText = call__160;
                var call__161 = TargetCity.GetLhx();
                var call__162 = TargetCity.GetYhx();
                var call__163 = TargetCity.GetXhx();
                Log.SetLocation(call__163, call__162, call__161);
                var call__164 = TargetCity.GetYhx();
                var call__165 = TargetCity.GetLhx();
                var call__166 = TargetCity.GetXhx();
                DistributeLocationEventLog(Log, A2.FPlayer, call__166, call__164, call__165, false);
                Log.Release();
                PlayDefaultCityEffects(A2, TargetCity);
                TargetCity.ValidateObservation(A2.FPlayer);
                var call__167 = TargetCity.Resource.ImageSequenceList.ImageSequences[20];
                if (call__167 != null)
                {
                    A = new TImageSequenceAnimation(null);
                    var call__168 = TargetCity.Resource.ImageSequenceList.ImageSequences[20];
                    A.SetImageSequence(call__168);
                    A.SetXhpOffset(A.FXhpOffset - 16);
                    A.SetYhpOffset(A.FYhpOffset - 16);
                    A.FOnShow = ShowAnimation;
                    var call__169 = TargetCity.GetLhx();
                    var call__170 = TargetCity.GetYhx();
                    var call__171 = TargetCity.GetXhx();
                    AoWHSMap.PlaceHX(A, call__171, call__170, call__169);
                }

                Result = true;
            }
            else
            {
                if (A3 == 50)
                {
                    TargetCity = ((TCityTargetSpellTE)A1).GetSelectedCity();
                    var call__172 = AoWHSMap.FPlayers.GetPlayers(A2.FPlayer);
                    TargetCity.SetRace(call__172.FPlayerRace);
                    TargetCity.SetRazed(false);
                    var call__173 = TargetCity.GetSize();
                    var call__174 = AoWHSMap.Random(call__173 * 2);
                    Count = call__174 + 1;
                    {
                        I = 0;
                        int end__175 = Count;
                        for (; I <= end__175; I += 1)
                        {
                            AUnit = new TUnit(null);
                            var call__176 = AoWHSMap.Random(2);
                            var call__177 = AoWHSSet.FUnitResources.GetUnitResource(CitySpells.TAnimateRuins_ExecuteTE__UndeadUnits[call__176]);
                            AUnit.SetUnitResource(call__177);
                            var call__178 = TargetCity.GetLhx();
                            var call__179 = TargetCity.GetYhx();
                            var call__180 = TargetCity.GetXhx();
                            AUnit.Place(call__180, call__179, call__178, A2.FPlayer, TSet<TUnitPlaceModeElement>.Of(TUnitPlaceModeElement.pmScanArea));
                            AUnit.Release();
                        }
                    }

                    CastingDone(A2, A1.FEventMana);
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
        public override bool ValidTargetCity(TAbstractUnit Source, TCity TargetCity)
        {
            bool Result = default;
            Result = base.ValidTargetCity(Source, TargetCity);
            Result = Result && TargetCity.GetRazed();
            return Result;
        }

        // CitySpells.pas:930
        public void ShowAnimation(TAnimation Animation, int X, int Y, in TSurfaceDesc SD)
        {
            var imageSequenceAnimation = (TImageSequenceAnimation)Animation;
            var call__182 = imageSequenceAnimation.FImageSequence.GetUnloopedFrameIndex(imageSequenceAnimation.FFrameCounter);
            if (call__182 == 0 && AoWEngine.FAnimationDetail != 0)
            {
                var call__183 = Random(2);
                PlaySample(ExplosionSFX[call__183]);
            }
        }
    }

    public static partial class CitySpells
    {
        public static FixedArray_int_2 TAnimateRuins_ExecuteTE__UndeadUnits = FixedArray_int_2.From([198, 201]);
        // CitySpells.pas:988
        public static void RegisterCitySpells(TAoWEngine A1)
        {
            A1.RegisterEClasses([EClass.Of<TCityEnchantmentSpellTE>(), EClass.Of<TCityTargetSpellTE>(), EClass.Of<TTownGateTE>()]);
            var spellControl = A1.FGameSet.FSpellControl;
            var call__185 = new TAnarchy(null);
            spellControl.RegisterSpell(call__185);
            var call__186 = new TTownQuake(null);
            spellControl.RegisterSpell(call__186);
            var call__187 = new TTownGate(null);
            spellControl.RegisterSpell(call__187);
            var call__188 = new TFountainOfLife(null);
            spellControl.RegisterSpell(call__188);
            var call__189 = new TGoldRush(null);
            spellControl.RegisterSpell(call__189);
            var call__190 = new TAntiMagicShell(null);
            spellControl.RegisterSpell(call__190);
            var call__191 = new TWarmonger(null);
            spellControl.RegisterSpell(call__191);
            var call__192 = new TAnimateRuins(null);
            spellControl.RegisterSpell(call__192);
        }

        // CitySpells.pas:1004
        public static void UnregisterCitySpells(TAoWEngine A1)
        {
            A1.UnregisterEClasses([EClass.Of<TCityEnchantmentSpellTE>(), EClass.Of<TCityTargetSpellTE>(), EClass.Of<TTownGateTE>()]);
            var spellControl = A1.FGameSet.FSpellControl;
            spellControl.UnregisterSpellID(72);
            // { Original leaves Town Quake ($47) registered. }
            spellControl.UnregisterSpellID(38);
            spellControl.UnregisterSpellID(52);
            spellControl.UnregisterSpellID(53);
            spellControl.UnregisterSpellID(54);
            spellControl.UnregisterSpellID(58);
            spellControl.UnregisterSpellID(76);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
