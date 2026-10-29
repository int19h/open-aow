// Translated from CitySpells.pas.
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
using Aow.Units.City;
using static Aow.Units.City.City;
using Aow.Units.EventLog;
using static Aow.Units.EventLog.EventLog;
using Aow.Units.Animation;
using static Aow.Units.Animation.Animation;
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
    // { TCityTargetSpellTE (instance size 40, VMT 557AF390):
//     $20 packed target coordinates; $24 target owner. }
    public partial class TCityTargetSpellTE : Aow.Units.AoWE.TSpellTE
    {
        public Aow.Units.HSEngine.TXYL FTargetLocation;
        // { $20 }
        public sbyte FTargetPlayer;
        // { $24 }
        public Aow.Units.FixedArray_byte_3 F_CityTargetSpellTE_25;
        // CitySpells.pas:175
        public override void ReadWrite(Aow.Units.Engine.TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwInteger(ref FTargetLocation.XYL, 20);
            Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FTargetPlayer), 21);
        }

        // CitySpells.pas:189
        public override bool Validate(Aow.Units.AoWE.THero A1)
        {
            bool Result = default;
            Aow.Units.City.TCity TargetCity = default;
            Result = base.Validate(A1);
            if (Result)
            {
                TargetCity = GetSelectedCity();
                if ((TargetCity == null) || (TargetCity.FPlayer != FTargetPlayer))
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
            FTargetLocation.X = unchecked((sbyte)X);
            FTargetLocation.Y = unchecked((sbyte)Y);
            FTargetLocation.L = unchecked((sbyte)L);
        }

        // CitySpells.pas:204
        public Aow.Units.City.TCity GetSelectedCity()
        {
            Aow.Units.City.TCity Result = default;
            var call__1 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.GetFieldXYL(FTargetLocation.XYL);
            var call__2 = call__1.FindNoneTransparentHS(typeof(Aow.Units.City.TCity));
            Result = (Aow.Units.City.TCity)call__2;
            return Result;
        }

        // NetworkE.pas:230
        public static new Aow.Units.CitySpells.TCityTargetSpellTE Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.CitySpells.TCityTargetSpellTE o = new Aow.Units.CitySpells.TCityTargetSpellTE();
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
            // CitySpells.pas:170
            public override int ClassID()
            {
                int Result = default;
                Result = 135949;
                return Result;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.CitySpells.TCityTargetSpellTE => (Meta)MetaRegistry.Of(typeof(T));
            // NetworkE.pas:230
            public new Aow.Units.CitySpells.TCityTargetSpellTE Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.CitySpells.TCityTargetSpellTE o = (Aow.Units.CitySpells.TCityTargetSpellTE)NewInstance();
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

    // { TCityTargetSpell (instance size 60, VMT 557AF4C8):
//     $34 source unit ID; $38 city target modes. }
    public partial class TCityTargetSpell : Aow.Units.AoWE.TSpell
    {
        public int FSourceID;
        // { $34 }
        public TSet<Aow.Units.CitySpells.TCityTargetModeElement> FCityTargetMode;
        // { $38 }
        public Aow.Units.FixedArray_byte_3 F_CityTargetSpell_39;
        // CitySpells.pas:209
        private protected TCityTargetSpell(Aow.Rtl.ConstructionContext context, Aow.Units.Engine.TECustomNode AOwner) : base(context, AOwner)
        {
            FSpellType = Aow.Units.AoWE.TSpellType.stGlobal;
        }

        // CitySpells.pas:377
        public override bool Activate(Aow.Units.AoWE.TAbstractUnit A1)
        {
            bool Result = default;
            Aow.Rtl.AnsiString Reason = default;
            if (CanActivate(A1, ref Reason))
            {
                FSourceID = A1.FUnitID;
                SetupTargetSelection(A1, Aow.Units.AoWE.AoWE.AoWHSMap.FTargetSelection);
                Aow.Units.AoWE.AoWE.AoWHSMap.FTargetSelection.Activate();
                Result = true;
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // CitySpells.pas:288
        public virtual void OnTargetSelectionChange(Aow.Rtl.TObject Sender)
        {
            Aow.Units.AoWE.TTargetSelectionControl TargetSelection = default;
            Aow.Units.AoWE.TAbstractUnit Source = default;
            bool Valid = default;
            TargetSelection = (Aow.Units.AoWE.TTargetSelectionControl)Sender;
            var call__3 = Aow.Units.AoWE.AoWE.AoWHSMap.FUnitControl.FindUnit(FSourceID);
            Source = call__3;
            if (Source != null)
            {
                switch (TargetSelection.FChange)
                {
                    case Aow.Units.AoWE.TTargetSelectionChange.tscDeactivated:
                        TargetSelection.FOnChange.Remove(OnTargetSelectionChange);
                        TargetSelection.Deactivate();
                        break;
                    case Aow.Units.AoWE.TTargetSelectionChange.tscSelected:
                        ValidateTarget(TargetSelection, unchecked((sbyte)TargetSelection.FTargetX), unchecked((sbyte)TargetSelection.FTargetY), unchecked((sbyte)TargetSelection.FTargetL), ref Valid);
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
        // { +09C }
        public virtual void PlayDefaultCityEffects(Aow.Units.AoWE.TAbstractUnit Source, Aow.Units.City.TCity TargetCity)
        {
            Aow.Units.HSEngine.TMapLevel Level = default;
            Aow.Units.Animation.TImageSequenceAnimation A = default;
            Aow.Units.HSEngine.THNWalkData Walk = default;
            bool PlaySound = default;
            PlaySound = false;
            var call__5 = TargetCity.GetLhx();
            var call__6 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.GetMapLevel(call__5);
            Level = call__6;
            var call__7 = TargetCity.GetYhx();
            var call__8 = TargetCity.GetXhx();
            Aow.Units.HSEngine.HSEngine.InitHNWalk(ref Walk, call__8, call__7, 0);
            while (Walk.HN < ((Aow.Units.AoWE.TStructureResource)TargetCity.Resource).TerrainList.Count)
            {
                bool bool__9 = (((Walk.X >= 0) && (Walk.Y >= 0)) && (Walk.X < Level.Width)) && (Walk.Y < Level.Height);
                if (bool__9)
                {
                    var call__10 = ((Aow.Units.AoWE.TStructureResource)TargetCity.Resource).TerrainList.Used[Walk.HN];
                    bool__9 = call__10;
                }

                if (bool__9)
                {
                    bool bool__11 = (Source.FPlayer == Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
                    if (!bool__11)
                    {
                        var call__12 = TargetCity.GetLhx();
                        var call__13 = Aow.Units.AoWE.AoWE.AoWHSMap.TerrainVisible(unchecked((sbyte)Walk.X), unchecked((sbyte)Walk.Y), unchecked((sbyte)call__12), 0);
                        bool__11 = call__13;
                    }

                    if (bool__11)
                    {
                        // { Original passes the city layer before the radius. }
                        A = Aow.Units.Animation.TImageSequenceAnimation.Create(null);
                        var call__14 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellID);
                        var call__15 = call__14.FCastImages.ImageSequences[0];
                        A.SetImageSequence(call__15);
                        var call__16 = Aow.Rtl.Rtl.Random(5);
                        A.FFrameCounter = call__16;
                        A.FAnimationLevel = 27;
                        var call__17 = TargetCity.GetLhx();
                        Aow.Units.AoWE.AoWE.AoWHSMap.PlaceHX(A, Walk.X, Walk.Y, call__17);
                        bool bool__18 = (Source.FPlayer == Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
                        if (!bool__18)
                        {
                            var call__19 = TargetCity.GetLhx();
                            var call__20 = Aow.Units.AoWE.AoWE.AoWHSMap.WatchingTerrain(unchecked((sbyte)Walk.X), unchecked((sbyte)Walk.Y), unchecked((sbyte)call__19), 0);
                            bool__18 = call__20;
                        }

                        if (bool__18)
                        {
                            PlaySound = true;
                        }
                    }
                }

                Aow.Units.HSEngine.HSEngine.WalkToNextHN(ref Walk);
            }

            if (PlaySound)
            {
                var call__21 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellID);
                call__21.FCastSounds.Play(0);
            }
        }

        // CitySpells.pas:323
        // { +0A0 }
        public virtual void SetupTargetSelection(Aow.Units.AoWE.TAbstractUnit Source, Aow.Units.AoWE.TTargetSelectionControl TargetSelection)
        {
            sbyte X = default;
            sbyte Y = default;
            sbyte L = default;
            TargetSelection.Initialize(Source);
            TargetSelection.FTargetMode = TargetSelection.FTargetMode + TSet<Aow.Units.AoWE.TTargetSelectionModeElement>.Of(Aow.Units.AoWE.TTargetSelectionModeElement.tmValidateDistance, Aow.Units.AoWE.TTargetSelectionModeElement.tmExternalFloater);
            var call__22 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.SelectTargetCityForXOnMapRStr);
            var call__23 = Aow.Rtl.Rtl.Format(call__22, [FSpellName]);
            TargetSelection.FText.Text = call__23;
            TargetSelection.FTitle = FSpellName;
            var call__24 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.CancelRStr);
            TargetSelection.FCancelText = call__24;
            var call__25 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellID);
            var call__26 = call__25.FCastImages.ImageSequences[20];
            var call__27 = call__26.GetPreviewImage();
            TargetSelection.FSourceImage = call__27;
            var call__28 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellID);
            var call__29 = call__28.FCastImages.ImageSequences[21];
            var call__30 = call__29.GetPreviewImage();
            TargetSelection.FValidTargetImage = call__30;
            var call__31 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellID);
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
                throw Aow.Rtl.Exception.Create((AnsiString)"oohps");
            }

            TargetSelection.FOnChange.Add(OnTargetSelectionChange);
            TargetSelection.FOnValidateTarget = ValidateTarget;
        }

        // CitySpells.pas:359
        // { +0A4 }
        public virtual bool TargetSelected(Aow.Units.AoWE.TAbstractUnit Source, int X, int Y, int L)
        {
            bool Result = default;
            Aow.Units.CitySpells.TCityTargetSpellTE Event = default;
            var call__34 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Source.FPlayer);
            var call__35 = call__34.GetBusy();
            if (!call__35)
            {
                Event = CreateTE();
                if (SetupTE(Event, Source, X, Y, L))
                {
                    var call__36 = Aow.Units.AoWE.AoWE.AoWHSMap.FTokenManager.GetTokenControl(Source.FPlayer);
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
        // { +0A8 }
        public virtual bool ValidTargetCity(Aow.Units.AoWE.TAbstractUnit Source, Aow.Units.City.TCity TargetCity)
        {
            bool Result = default;
            sbyte X = default;
            sbyte Y = default;
            sbyte L = default;
            Aow.Units.City.TCity Found = default;
            Result = (FCityTargetMode.Contains(Aow.Units.CitySpells.TCityTargetModeElement.ctmOwn) && (TargetCity.FPlayer == Source.FPlayer)) || (FCityTargetMode.Contains(Aow.Units.CitySpells.TCityTargetModeElement.ctmEnemy) && (TargetCity.FPlayer != Source.FPlayer));
            bool bool__37 = Result;
            if (bool__37)
            {
                var call__38 = TargetCity.GetRazed();
                bool__37 = (!call__38 || FCityTargetMode.Contains(Aow.Units.CitySpells.TCityTargetModeElement.ctmRazed));
            }

            Result = bool__37;
            if (Result && FCityTargetMode.Contains(Aow.Units.CitySpells.TCityTargetModeElement.ctmSkipSource))
            {
                Source.GetLocation(ref X, ref Y, ref L);
                var call__39 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.GetField(X, Y, L);
                var call__40 = call__39.FindNoneTransparentHS(typeof(Aow.Units.City.TCity));
                Found = (Aow.Units.City.TCity)call__40;
                Result = TargetCity != Found;
            }

            return Result;
        }

        // CitySpells.pas:346
        // { +0AC }
        public virtual Aow.Units.CitySpells.TCityTargetSpellTE CreateTE()
        {
            Aow.Units.CitySpells.TCityTargetSpellTE Result = default;
            Result = Aow.Units.CitySpells.TCityTargetSpellTE.Create(null);
            return Result;
        }

        // CitySpells.pas:351
        // { +0B0 }
        public virtual bool SetupTE(Aow.Units.CitySpells.TCityTargetSpellTE Event, Aow.Units.AoWE.TAbstractUnit Source, int X, int Y, int L)
        {
            bool Result = default;
            Event.Initialize(FSpellID, unchecked((byte)Source.FPlayer), Source.FUnitID, FBaseManaCost);
            Event.SetLocation(unchecked((sbyte)X), unchecked((sbyte)Y), unchecked((sbyte)L));
            var call__41 = Event.GetSelectedCity();
            Event.FTargetPlayer = unchecked((sbyte)call__41.FPlayer);
            Result = true;
            return Result;
        }

        // CitySpells.pas:262
        // { +0B4 }
        public void ValidateTarget(Aow.Units.AoWE.TTargetSelectionControl TargetSelection, sbyte X, sbyte Y, sbyte L, ref bool Valid)
        {
            Aow.Units.AoWE.TAbstractUnit Source = default;
            Aow.Units.HSEngine.TMapLevel Level = default;
            Aow.Units.AoWE.TAoWMapField Field = default;
            Aow.Units.City.TCity TargetCity = default;
            var call__42 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.GetMapLevel(L);
            Level = call__42;
            var call__43 = Aow.Units.AoWE.AoWE.AoWHSMap.FUnitControl.FindUnit(FSourceID);
            Source = call__43;
            if (((((Source != null) && (X >= 0)) && (Y >= 0)) && (X < Level.Width)) && (Y < Level.Height))
            {
                Field = (Aow.Units.AoWE.TAoWMapField)Level.GetField(X, Y);
                bool bool__44 = !Aow.Units.AoWE.AoWE.AoWHSMap.FExplorationEnabled;
                if (!bool__44)
                {
                    var call__45 = Field.GetExplored(Source.FPlayer);
                    bool__44 = call__45;
                }

                Valid = bool__44;
                if (Valid)
                {
                    TargetCity = (Aow.Units.City.TCity)Field.FindNoneTransparentHS(typeof(Aow.Units.City.TCity));
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

        public static new Aow.Units.CitySpells.TCityTargetSpell Create(Aow.Units.Engine.TECustomNode AOwner)
            => Aow.Rtl.ConstructionContext.Create(AOwner, static (context, owner) => new TCityTargetSpell(context, owner));
    }

    // { TTownGateTE (instance size 48, VMT 557AF604): $28 army ID; $2C destination coordinates. }
    public partial class TTownGateTE : Aow.Units.CitySpells.TCityTargetSpellTE
    {
        public Aow.Units.AoWE.TArmyID FArmyID;
        // { $28 }
        public Aow.Units.HSEngine.TXYL FTeleportLocation;
        // CitySpells.pas:391
        // { $2C }
        public override void Destroy()
        {
            FArmyID.Free();
            base.Destroy();
        }

        // CitySpells.pas:400
        public override void ReadWrite(Aow.Units.Engine.TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwInteger(ref FTeleportLocation.XYL, 30);
            Stream.rwCreateEObject(ref FArmyID, null, 31);
        }

        // CitySpells.pas:407
        public override bool Validate(Aow.Units.AoWE.THero A1)
        {
            bool Result = default;
            byte Selection = default;
            Aow.Units.AoWE.TArmyHS Army = default;
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
                var call__49 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FEventSpellID);
                var call__50 = ((Aow.Units.CitySpells.TTownGate)call__49).ValidTeleportLocation(Army, unchecked((byte)Selection), unchecked((sbyte)FTeleportLocation.X), unchecked((sbyte)FTeleportLocation.Y), unchecked((sbyte)FTeleportLocation.L));
                Result = call__50;
            }

            return Result;
        }

        // NetworkE.pas:230
        public static new Aow.Units.CitySpells.TTownGateTE Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.CitySpells.TTownGateTE o = new Aow.Units.CitySpells.TTownGateTE();
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

        public new partial class Meta : Aow.Units.CitySpells.TCityTargetSpellTE.Meta
        {
            // CitySpells.pas:397
            public override int ClassID()
            {
                int Result = default;
                Result = 135950;
                return Result;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.CitySpells.TTownGateTE => (Meta)MetaRegistry.Of(typeof(T));
            // NetworkE.pas:230
            public new Aow.Units.CitySpells.TTownGateTE Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.CitySpells.TTownGateTE o = (Aow.Units.CitySpells.TTownGateTE)NewInstance();
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

    // { TTownGate (instance size 60, VMT 557AF6D4) }
    public partial class TTownGate : Aow.Units.CitySpells.TCityTargetSpell
    {
        // CitySpells.pas:421
        private protected TTownGate(Aow.Rtl.ConstructionContext context, Aow.Units.Engine.TECustomNode AOwner) : base(context, AOwner)
        {
            var call__51 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.TownGateRStr);
            FSpellName = call__51;
            FSpellID = 38;
            FCityTargetMode = TSet<Aow.Units.CitySpells.TCityTargetModeElement>.Of(Aow.Units.CitySpells.TCityTargetModeElement.ctmOwn, Aow.Units.CitySpells.TCityTargetModeElement.ctmSkipSource);
            FBaseManaCost = 5;
            FSpellLevel = 3;
            FMagicSphere = Aow.Units.AoWE.TMagicSphere.msCosmos;
        }

        // CitySpells.pas:486
        public override bool ExecuteTE(Aow.Units.AoWE.TSpellTE A1, Aow.Units.AoWE.TAbstractUnit A2, int A3)
        {
            bool Result = default;
            bool ShowSource = default;
            bool ShowTarget = default;
            byte Selection = default;
            Aow.Units.AoWE.TArmyHS Army = default;
            Aow.Units.AoWE.TAoWMapField Field = default;
            Aow.Units.AoWE.TArmyHS NewArmy = default;
            sbyte X = default;
            sbyte Y = default;
            sbyte L = default;
            Aow.Units.Animation.TImageSequenceAnimation A = default;
            A2.GetLocation(ref X, ref Y, ref L);
            ((Aow.Units.CitySpells.TTownGateTE)A1).FArmyID.RetrieveArmy(ref Army, ref Selection);
            var call__52 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.GetFieldXYL(((Aow.Units.CitySpells.TTownGateTE)A1).FTeleportLocation.XYL);
            Field = (Aow.Units.AoWE.TAoWMapField)call__52;
            Army.FArmy.ResetDestinationPos();
            var call__53 = Aow.Units.AoWE.AoWE.AoWHSMap.FSelectedArmy.GetArmyHS();
            if (call__53 == Army)
            {
                Aow.Units.AoWE.AoWE.AoWHSMap.FSelectedArmy.Select(Army);
            }

            var call__54 = Army.FArmy.ValidateSelection(unchecked((byte)Selection));
            var call__55 = Army.FArmy.ValidateSelection(255);
            if (call__54 != call__55)
            {
                NewArmy = Aow.Units.AoWE.TArmyHS.Create(null);
                NewArmy.FArmy.SetPlayer(unchecked((sbyte)Army.FArmy.FPlayer));
                NewArmy.FArmy.AddUnitSelection(Army.FArmy, unchecked((byte)Selection));
                Aow.Units.AoWE.AoWE.AoWHSMap.PlaceHX(NewArmy, ((Aow.Units.CitySpells.TTownGateTE)A1).FTeleportLocation.X, ((Aow.Units.CitySpells.TTownGateTE)A1).FTeleportLocation.Y, ((Aow.Units.CitySpells.TTownGateTE)A1).FTeleportLocation.L);
            }
            else
            {
                if (Field.GetArmyHS() != null)
                {
                    var call__56 = Field.GetArmyHS();
                    call__56.FArmy.AddUnitSelection(Army.FArmy, unchecked((byte)Selection));
                }
                else
                {
                    Army.MoveTo(Field, 0);
                }
            }

            bool bool__57 = (Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer == A2.FPlayer);
            if (!bool__57)
            {
                var call__58 = Aow.Units.AoWE.AoWE.AoWHSMap.Visible(unchecked((sbyte)X), unchecked((sbyte)Y), unchecked((sbyte)L), 0);
                bool__57 = call__58;
            }

            ShowSource = bool__57;
            bool bool__59 = (Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer == A2.FPlayer);
            if (!bool__59)
            {
                var call__60 = Aow.Units.AoWE.AoWE.AoWHSMap.Visible(unchecked((sbyte)Field.Xhx), unchecked((sbyte)Field.Yhx), unchecked((sbyte)Field.Lhx), 0);
                bool__59 = call__60;
            }

            ShowTarget = bool__59;
            if (ShowSource)
            {
                var call__61 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellID);
                var call__62 = call__61.FCastImages.ImageSequences[0];
                if (call__62 != null)
                {
                    A = Aow.Units.Animation.TImageSequenceAnimation.Create(null);
                    var call__63 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellID);
                    var call__64 = call__63.FCastImages.ImageSequences[0];
                    A.SetImageSequence(call__64);
                    Aow.Units.AoWE.AoWE.AoWHSMap.PlaceHX(A, X, Y, L);
                }
            }

            if (ShowTarget)
            {
                var call__65 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellID);
                var call__66 = call__65.FCastImages.ImageSequences[0];
                if (call__66 != null)
                {
                    A = Aow.Units.Animation.TImageSequenceAnimation.Create(null);
                    var call__67 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellID);
                    var call__68 = call__67.FCastImages.ImageSequences[0];
                    A.SetImageSequence(call__68);
                    Aow.Units.AoWE.AoWE.AoWHSMap.PlaceHX(A, Field.Xhx, Field.Yhx, Field.Lhx);
                }
            }

            if (Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer == A2.FPlayer)
            {
                Aow.Units.AoWE.AoWE.AoWHSMap.CenterViewTo(Field.Xhx, Field.Yhx, Field.Lhx);
            }
            else
            {
                var call__69 = Aow.Units.AoWE.AoWE.AoWHSMap.ValidateObservation(unchecked((sbyte)A2.FPlayer), unchecked((sbyte)Field.Xhx), unchecked((sbyte)Field.Yhx), unchecked((sbyte)Field.Lhx), 0);
                if (!call__69)
                {
                    Aow.Units.AoWE.AoWE.AoWHSMap.ValidateObservation(unchecked((sbyte)A2.FPlayer), unchecked((sbyte)X), unchecked((sbyte)Y), unchecked((sbyte)L), 0);
                }
            }

            bool bool__70 = (Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer == A2.FPlayer);
            if (!bool__70)
            {
                var call__71 = Aow.Units.AoWE.AoWE.AoWHSMap.Watching(unchecked((sbyte)X), unchecked((sbyte)Y), unchecked((sbyte)L), 0);
                bool__70 = call__71;
            }

            bool bool__72 = bool__70;
            if (!bool__72)
            {
                var call__73 = Aow.Units.AoWE.AoWE.AoWHSMap.Watching(unchecked((sbyte)Field.Xhx), unchecked((sbyte)Field.Yhx), unchecked((sbyte)Field.Lhx), 0);
                bool__72 = call__73;
            }

            if (bool__72)
            {
                var call__74 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellID);
                call__74.FCastSounds.Play(0);
            }

            CastingDone(A2, A1.FEventMana);
            Result = false;
            return Result;
        }

        // CitySpells.pas:540
        public override bool CanActivate(Aow.Units.AoWE.TAbstractUnit A1, ref Aow.Rtl.AnsiString A2)
        {
            bool Result = default;
            Result = base.CanActivate(A1, ref A2);
            return Result;
        }

        // CitySpells.pas:545
        public override void SetupTargetSelection(Aow.Units.AoWE.TAbstractUnit Source, Aow.Units.AoWE.TTargetSelectionControl TargetSelection)
        {
            base.SetupTargetSelection(Source, TargetSelection);
            TargetSelection.FTargetMode = TargetSelection.FTargetMode - TSet<Aow.Units.AoWE.TTargetSelectionModeElement>.Of(Aow.Units.AoWE.TTargetSelectionModeElement.tmFixedLevel, Aow.Units.AoWE.TTargetSelectionModeElement.tmValidateDistance);
        }

        // CitySpells.pas:445
        public override Aow.Units.CitySpells.TCityTargetSpellTE CreateTE()
        {
            Aow.Units.CitySpells.TCityTargetSpellTE Result = default;
            Result = Aow.Units.CitySpells.TTownGateTE.Create(null);
            return Result;
        }

        // CitySpells.pas:450
        public override bool SetupTE(Aow.Units.CitySpells.TCityTargetSpellTE Event, Aow.Units.AoWE.TAbstractUnit Source, int X, int Y, int L)
        {
            bool Result = default;
            byte Selection = default;
            Aow.Units.AoWE.TArmyHS Army = default;
            bool Found = default;
            Aow.Units.HSEngine.TMapLevel Level = default;
            Aow.Units.HSEngine.THNWalkData Walk = default;
            Result = base.SetupTE(Event, Source, X, Y, L);
            if (Result)
            {
                var call__75 = ((Aow.Units.AoWE.TArmy)Source.Owner).GetArmyHS();
                Army = call__75;
                bool bool__76 = (Army.FArmy.FPlayer == Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
                if (bool__76)
                {
                    var call__77 = Aow.Units.AoWE.AoWE.AoWHSMap.FSelectedArmy.GetArmyHS();
                    bool__76 = (call__77 == Army);
                }

                if (bool__76)
                {
                    var call__78 = Aow.Units.AoWE.AoWE.AoWHSMap.FSelectedArmy.GetSelection();
                    Selection = unchecked((byte)call__78);
                }
                else
                {
                    Selection = 255;
                }

                Aow.Units.HSEngine.HSEngine.InitHNWalk(ref Walk, X, Y, 0);
                Found = false;
                var call__79 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.GetMapLevel(L);
                Level = call__79;
                while ((Walk.Radius < 4) && !Found)
                {
                    if ((((Walk.X >= 0) && (Walk.Y >= 0)) && (Walk.X < Level.Width)) && (Walk.Y < Level.Height))
                    {
                        var call__80 = ValidTeleportLocation(Army, unchecked((byte)Selection), unchecked((sbyte)Walk.X), unchecked((sbyte)Walk.Y), unchecked((sbyte)L));
                        Found = call__80;
                    }

                    if (!Found)
                    {
                        Aow.Units.HSEngine.HSEngine.WalkToNextHN(ref Walk);
                    }
                }

                if (Found)
                {
                    var call__81 = Army.FArmy.CreateID(unchecked((byte)Selection));
                    ((Aow.Units.CitySpells.TTownGateTE)Event).FArmyID = call__81;
                    var call__82 = Aow.Units.HSEngine.HSEngine.PackXYL(unchecked((sbyte)Walk.X), unchecked((sbyte)Walk.Y), unchecked((sbyte)L));
                    ((Aow.Units.CitySpells.TTownGateTE)Event).FTeleportLocation = call__82;
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
        public bool ValidTeleportLocation(Aow.Units.AoWE.TArmyHS Army, byte Selection, sbyte X, sbyte Y, sbyte L)
        {
            bool Result = default;
            Aow.Units.AoWE.TAoWMapField Field = default;
            var call__83 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.GetField(X, Y, L);
            Field = (Aow.Units.AoWE.TAoWMapField)call__83;
            var call__84 = Field.GetPlayer();
            bool bool__85 = (call__84 != -1);
            if (bool__85)
            {
                var call__86 = Field.GetPlayer();
                bool__85 = (call__86 != Army.FArmy.FPlayer);
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
                var call__87 = Army.FArmy.ValidTerrainEx(unchecked((byte)Selection), unchecked((sbyte)Field.TerrainType), unchecked((sbyte)Field.OverlayType));
                Result = call__87;
                // { CanAddUnitSelection returns arOK (zero) when the destination accepts the selection. }
                bool bool__88 = Result;
                if (bool__88)
                {
                    var call__89 = Field.GetArmyHS();
                    bool bool__90 = (call__89 == null);
                    if (!bool__90)
                    {
                        var call__91 = Field.GetArmyHS();
                        var call__92 = call__91.FArmy.CanAddUnitSelection(Army.FArmy, unchecked((byte)Selection));
                        bool__90 = (call__92 == Aow.Units.AoWE.TAddUnitsResult.arOK);
                    }

                    bool__88 = bool__90;
                }

                Result = bool__88;
            }

            return Result;
        }

        public static new Aow.Units.CitySpells.TTownGate Create(Aow.Units.Engine.TECustomNode AOwner)
            => Aow.Rtl.ConstructionContext.Create(AOwner, static (context, owner) => new TTownGate(context, owner));
    }

    // { TCityEnchantmentSpellTE (instance size 40, VMT 557AF800) }
    public partial class TCityEnchantmentSpellTE : Aow.Units.CitySpells.TCityTargetSpellTE
    {
        // NetworkE.pas:230
        public static new Aow.Units.CitySpells.TCityEnchantmentSpellTE Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.CitySpells.TCityEnchantmentSpellTE o = new Aow.Units.CitySpells.TCityEnchantmentSpellTE();
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

        public new partial class Meta : Aow.Units.CitySpells.TCityTargetSpellTE.Meta
        {
            // CitySpells.pas:551
            public override int ClassID()
            {
                int Result = default;
                Result = 131821;
                return Result;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.CitySpells.TCityEnchantmentSpellTE => (Meta)MetaRegistry.Of(typeof(T));
            // NetworkE.pas:230
            public new Aow.Units.CitySpells.TCityEnchantmentSpellTE Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.CitySpells.TCityEnchantmentSpellTE o = (Aow.Units.CitySpells.TCityEnchantmentSpellTE)NewInstance();
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

    // { TCityEnchantmentSpell (instance size 80, VMT 557AF958): $3C enchantment ID,
//     $40 enchantment setting, $44 AI priority, $48 AI budget, $4C AI cast situations. }
    public partial class TCityEnchantmentSpell : Aow.Units.CitySpells.TCityTargetSpell
    {
        public Aow.Units.City.TCityEnchantmentID FEnchantmentID;
        // { $3C }
        public Aow.Units.FixedArray_byte_3 F_CityEnchantmentSpell_3D;
        public int FEnchantmentSpellID;
        // { $40 }
        public int FAIPriority;
        // { $44 }
        public int FAIBudget;
        // { $48 }
        public TSet<Aow.Units.CitySpells.TCityEnchantmentCastSpellSituationElement> FAICastSituation;
        // { $4C }
        public Aow.Units.FixedArray_byte_3 F_CityEnchantmentSpell_4D;
        // CitySpells.pas:554
        private protected TCityEnchantmentSpell(Aow.Rtl.ConstructionContext context, Aow.Units.Engine.TECustomNode AOwner) : base(context, AOwner)
        {
            FAIPriority = -1;
        }

        // CitySpells.pas:576
        public override bool ExecuteTE(Aow.Units.AoWE.TSpellTE A1, Aow.Units.AoWE.TAbstractUnit A2, int A3)
        {
            bool Result = default;
            Aow.Units.City.TCity TargetCity = default;
            Aow.Units.City.TCityEnchantment Enchantment = default;
            TargetCity = ((Aow.Units.CitySpells.TCityTargetSpellTE)A1).GetSelectedCity();
            Enchantment = Aow.Units.City.TCityEnchantment.Create(null);
            Enchantment.FEnchantmentID = FEnchantmentID;
            Enchantment.FEnchantmentManaUpkeep = FManaUpkeep;
            var call__93 = ((Aow.Units.HSEngine.TMapField)TargetCity.Owner).GetXYL();
            Enchantment.FEnchantmentLocation.XYL = call__93;
            Enchantment.FDispelMana = FEnchantmentSpellID;
            Enchantment.FPlayerIndex = unchecked((sbyte)A2.FPlayer);
            TargetCity.ActivateEnchantment(Enchantment);
            var call__94 = A1.GetSource();
            PlayDefaultCityEffects(call__94, TargetCity);
            CastingDone(A2, A1.FEventMana);
            Result = false;
            return Result;
        }

        // CitySpells.pas:594
        public override void AIPrefetchCastSpellActions(Aow.Units.AoWE.TAbstractUnit A1, Aow.Units.AoWE.TAICastSpellActionList A2)
        {
            bool Valid = default;
            Aow.Units.AoWE.TAICastSpellAction Action = default;
            Aow.Rtl.AnsiString Reason = default;
            sbyte X = default;
            sbyte Y = default;
            sbyte L = default;
            Aow.Units.City.TCity TargetCity = default;
            if (FAIPriority != -1)
            {
                A1.GetLocation(ref X, ref Y, ref L);
                var call__95 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.GetField(X, Y, L);
                var call__96 = call__95.FindNoneTransparentHS(typeof(Aow.Units.City.TCity));
                TargetCity = (Aow.Units.City.TCity)call__96;
                bool bool__97 = (TargetCity != null);
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
                    var call__101 = ((Aow.Units.AoWE.THero)A1).CanCastSpell(FSpellID, ref Reason);
                    bool__100 = call__101;
                }

                Valid = bool__100;
                if (Valid)
                {
                    bool bool__102 = (FAICastSituation.Contains(Aow.Units.CitySpells.TCityEnchantmentCastSpellSituationElement.ccsLeaderInCity) && (A1 is Aow.Units.AoWE.TLeader));
                    if (!bool__102)
                    {
                        bool bool__103 = FAICastSituation.Contains(Aow.Units.CitySpells.TCityEnchantmentCastSpellSituationElement.ccs4HexCity);
                        if (bool__103)
                        {
                            var call__104 = TargetCity.GetSize();
                            bool__103 = (call__104 == 4);
                        }

                        bool__102 = bool__103;
                    }

                    bool bool__105 = bool__102;
                    if (!bool__105)
                    {
                        bool bool__106 = FAICastSituation.Contains(Aow.Units.CitySpells.TCityEnchantmentCastSpellSituationElement.ccs3HexCity);
                        if (bool__106)
                        {
                            var call__107 = TargetCity.GetSize();
                            bool__106 = (call__107 == 3);
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
        public override bool AIExecuteCastSpellAction(Aow.Units.AoWE.TAbstractUnit A1)
        {
            bool Result = default;
            Aow.Rtl.AnsiString Reason = default;
            sbyte X = default;
            sbyte Y = default;
            sbyte L = default;
            Aow.Units.City.TCity TargetCity = default;
            A1.GetLocation(ref X, ref Y, ref L);
            var call__108 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.GetField(X, Y, L);
            var call__109 = call__108.FindNoneTransparentHS(typeof(Aow.Units.City.TCity));
            TargetCity = (Aow.Units.City.TCity)call__109;
            bool bool__110 = (TargetCity != null);
            if (bool__110)
            {
                var call__111 = ValidTargetCity(A1, TargetCity);
                bool__110 = call__111;
            }

            bool bool__112 = bool__110;
            if (bool__112)
            {
                var call__113 = ((Aow.Units.AoWE.THero)A1).CanCastSpell(FSpellID, ref Reason);
                bool__112 = call__113;
            }

            if (bool__112)
            {
                AICastSpellExpenseMade(A1, FBaseManaCost, FManaUpkeep, FAIBudget);
                var hero = (Aow.Units.AoWE.THero)A1;
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
        public override bool ValidTargetCity(Aow.Units.AoWE.TAbstractUnit Source, Aow.Units.City.TCity TargetCity)
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
        public override Aow.Units.CitySpells.TCityTargetSpellTE CreateTE()
        {
            Aow.Units.CitySpells.TCityTargetSpellTE Result = default;
            Result = Aow.Units.CitySpells.TCityEnchantmentSpellTE.Create(null);
            return Result;
        }

        // CitySpells.pas:565
        public override bool SetupTE(Aow.Units.CitySpells.TCityTargetSpellTE Event, Aow.Units.AoWE.TAbstractUnit Source, int X, int Y, int L)
        {
            bool Result = default;
            Result = base.SetupTE(Event, Source, X, Y, L);
            return Result;
        }

        public static new Aow.Units.CitySpells.TCityEnchantmentSpell Create(Aow.Units.Engine.TECustomNode AOwner)
            => Aow.Rtl.ConstructionContext.Create(AOwner, static (context, owner) => new TCityEnchantmentSpell(context, owner));
    }

    // { TFountainOfLife (instance size 80, VMT 557AFA9C) }
    public partial class TFountainOfLife : Aow.Units.CitySpells.TCityEnchantmentSpell
    {
        // CitySpells.pas:649
        private protected TFountainOfLife(Aow.Rtl.ConstructionContext context, Aow.Units.Engine.TECustomNode AOwner) : base(context, AOwner)
        {
            var call__116 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.FountainOfLifeRStr);
            FSpellName = call__116;
            FSpellID = 52;
            FCityTargetMode = TSet<Aow.Units.CitySpells.TCityTargetModeElement>.Of(Aow.Units.CitySpells.TCityTargetModeElement.ctmOwn);
            FBaseManaCost = 1;
            FSpellLevel = 4;
            FMagicSphere = Aow.Units.AoWE.TMagicSphere.msWater;
            FEnchantmentID = Aow.Units.City.TCityEnchantmentID.ceiFountainOfLife;
            FCastDuration = 20;
            FAIPriority = 500;
            FAICastSituation = TSet<Aow.Units.CitySpells.TCityEnchantmentCastSpellSituationElement>.Of(Aow.Units.CitySpells.TCityEnchantmentCastSpellSituationElement.ccsLeaderInCity);
            FAIBudget = 600;
            FEnchantmentSpellID = 50;
        }

        // CitySpells.pas:692
        public override bool ExecuteTE(Aow.Units.AoWE.TSpellTE A1, Aow.Units.AoWE.TAbstractUnit A2, int A3)
        {
            bool Result = default;
            Aow.Units.City.TCity TargetCity = default;
            Aow.Units.Animation.TAnimation A = default;
            Result = base.ExecuteTE(A1, A2, A3);
            if (A3 == 0)
            {
                TargetCity = ((Aow.Units.CitySpells.TCityTargetSpellTE)A1).GetSelectedCity();
                A = Aow.Units.Animation.TAnimation.Create(null);
                A.FAnimationLevel = 27;
                A.FOnShow = ShowFountain;
                A.FAnimationMap = Aow.Units.AoWE.AoWE.AoWHSMap;
                var call__117 = TargetCity.GetXhx();
                var call__118 = TargetCity.GetYhx();
                A.FTag = (int)(((uint)((int)call__117)) | ((uint)((int)(call__118 << 8))));
                A.Activate();
            }

            return Result;
        }

        // CitySpells.pas:666
        public void ShowFountain(Aow.Units.Animation.TAnimation Animation, int X, int Y, in Aow.Units.GFXE.TSurfaceDesc SD)
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
            Aow.Units.HSEngine.HSEngine.HXtoHP(Animation.FTag & 255, (int)(((uint)Animation.FTag) >> 8), ref CX, ref CY);
            CX = (CX + 10) - X;
            CY = (CY - Y) + 10;
            // { Original resets the process-wide random seed for every draw. }
            Aow.Rtl.Rtl.RandSeed = 0;
            {
                I = 0;
                byte end__119 = 100;
                for (; I <= end__119; I += 1)
                {
                    VX = Aow.Rtl.Rtl.Random(100) - 50;
                    VY = Aow.Rtl.Rtl.Random(5) + 7;
                    Offset = Aow.Rtl.Rtl.Round(((double)VX) * 0.1);
                    T = Aow.Rtl.Rtl.Random(30);
                    if ((T < Animation.FFrameCounter) && ((Animation.FFrameCounter - T) < 90))
                    {
                        T = (Animation.FFrameCounter - T) % 30;
                        DX = Aow.Rtl.Rtl.Round(((double)(T * VX)) / 30);
                        DY = (VY * T) - Aow.Rtl.Rtl.Round(((double)(((double)T) * 0.25)) * T);
                        if (T > 0)
                        {
                            var call__120 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(52);
                            call__120.FCastImages.Show(0, (CX + DX) + Offset, CY - DY, 0, SD);
                        }
                    }
                }
            }

            if (Animation.FFrameCounter == 120)
            {
                Animation.Free();
            }
        }

        public static new Aow.Units.CitySpells.TFountainOfLife Create(Aow.Units.Engine.TECustomNode AOwner)
            => Aow.Rtl.ConstructionContext.Create(AOwner, static (context, owner) => new TFountainOfLife(context, owner));
    }

    // { TGoldRush (instance size 80, VMT 557AFBD0) }
    public partial class TGoldRush : Aow.Units.CitySpells.TCityEnchantmentSpell
    {
        // CitySpells.pas:710
        private protected TGoldRush(Aow.Rtl.ConstructionContext context, Aow.Units.Engine.TECustomNode AOwner) : base(context, AOwner)
        {
            var call__121 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.GoldRushRStr);
            FSpellName = call__121;
            FSpellID = 53;
            FCityTargetMode = TSet<Aow.Units.CitySpells.TCityTargetModeElement>.Of(Aow.Units.CitySpells.TCityTargetModeElement.ctmOwn);
            FBaseManaCost = 1;
            FSpellLevel = 4;
            FMagicSphere = Aow.Units.AoWE.TMagicSphere.msWater;
            FEnchantmentID = Aow.Units.City.TCityEnchantmentID.ceiGoldRush;
            FCastDuration = 20;
            FAIPriority = 500;
            FAICastSituation = TSet<Aow.Units.CitySpells.TCityEnchantmentCastSpellSituationElement>.Of(Aow.Units.CitySpells.TCityEnchantmentCastSpellSituationElement.ccs4HexCity);
            FAIBudget = 600;
            FEnchantmentSpellID = 25;
        }

        public static new Aow.Units.CitySpells.TGoldRush Create(Aow.Units.Engine.TECustomNode AOwner)
            => Aow.Rtl.ConstructionContext.Create(AOwner, static (context, owner) => new TGoldRush(context, owner));
    }

    // { TAntiMagicShell (instance size 80, VMT 557AFCFC) }
    public partial class TAntiMagicShell : Aow.Units.CitySpells.TCityEnchantmentSpell
    {
        // CitySpells.pas:727
        private protected TAntiMagicShell(Aow.Rtl.ConstructionContext context, Aow.Units.Engine.TECustomNode AOwner) : base(context, AOwner)
        {
            var call__122 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.AntiMagicShellRStr);
            FSpellName = call__122;
            FSpellID = 54;
            FCityTargetMode = TSet<Aow.Units.CitySpells.TCityTargetModeElement>.Of(Aow.Units.CitySpells.TCityTargetModeElement.ctmOwn);
            FBaseManaCost = 1;
            FSpellLevel = 4;
            FMagicSphere = Aow.Units.AoWE.TMagicSphere.msCosmos;
            FEnchantmentID = Aow.Units.City.TCityEnchantmentID.ceiAntiMagicShell;
            FCastDuration = 40;
            FAIPriority = 500;
            FAICastSituation = TSet<Aow.Units.CitySpells.TCityEnchantmentCastSpellSituationElement>.Of(Aow.Units.CitySpells.TCityEnchantmentCastSpellSituationElement.ccsLeaderInCity);
            FEnchantmentSpellID = 100;
        }

        // CitySpells.pas:743
        public override void PlayDefaultCityEffects(Aow.Units.AoWE.TAbstractUnit Source, Aow.Units.City.TCity TargetCity)
        {
            base.PlayDefaultCityEffects(Source, TargetCity);
            bool bool__123 = (Source.FPlayer == Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
            if (!bool__123)
            {
                var call__124 = TargetCity.GetLhx();
                var call__125 = TargetCity.GetYhx();
                var call__126 = TargetCity.GetXhx();
                var call__127 = Aow.Units.AoWE.AoWE.AoWHSMap.WatchingTerrain(unchecked((sbyte)call__126), unchecked((sbyte)call__125), unchecked((sbyte)call__124), 1);
                bool__123 = call__127;
            }

            if (bool__123)
            {
                var call__128 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellID);
                call__128.FCastSounds.PlayEx(1, -1, false, 30, Aow.Units.Sound.TSamplePanning.spCenter, null);
            }
        }

        public static new Aow.Units.CitySpells.TAntiMagicShell Create(Aow.Units.Engine.TECustomNode AOwner)
            => Aow.Rtl.ConstructionContext.Create(AOwner, static (context, owner) => new TAntiMagicShell(context, owner));
    }

    // { TWarmonger (instance size 80, VMT 557AFE30) }
    public partial class TWarmonger : Aow.Units.CitySpells.TCityEnchantmentSpell
    {
        // CitySpells.pas:751
        private protected TWarmonger(Aow.Rtl.ConstructionContext context, Aow.Units.Engine.TECustomNode AOwner) : base(context, AOwner)
        {
            var call__129 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.WarmongerRStr);
            FSpellName = call__129;
            FSpellID = 58;
            FCityTargetMode = TSet<Aow.Units.CitySpells.TCityTargetModeElement>.Of(Aow.Units.CitySpells.TCityTargetModeElement.ctmOwn);
            FEnchantmentID = Aow.Units.City.TCityEnchantmentID.ceiWarmonger;
            FCastDuration = 30;
            FAIPriority = 600;
            FAICastSituation = TSet<Aow.Units.CitySpells.TCityEnchantmentCastSpellSituationElement>.Of(Aow.Units.CitySpells.TCityEnchantmentCastSpellSituationElement.ccs4HexCity, Aow.Units.CitySpells.TCityEnchantmentCastSpellSituationElement.ccs3HexCity);
            FEnchantmentSpellID = 50;
        }

        public static new Aow.Units.CitySpells.TWarmonger Create(Aow.Units.Engine.TECustomNode AOwner)
            => Aow.Rtl.ConstructionContext.Create(AOwner, static (context, owner) => new TWarmonger(context, owner));
    }

    // { TTownQuake (instance size 60, VMT 557AFF5C) }
    public partial class TTownQuake : Aow.Units.CitySpells.TCityTargetSpell
    {
        // CitySpells.pas:764
        private protected TTownQuake(Aow.Rtl.ConstructionContext context, Aow.Units.Engine.TECustomNode AOwner) : base(context, AOwner)
        {
            var call__130 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.TownQuakeRStr);
            FSpellName = call__130;
            FSpellID = 71;
            FCityTargetMode = TSet<Aow.Units.CitySpells.TCityTargetModeElement>.Of(Aow.Units.CitySpells.TCityTargetModeElement.ctmEnemy);
        }

        // CitySpells.pas:820
        public override bool ExecuteTE(Aow.Units.AoWE.TSpellTE A1, Aow.Units.AoWE.TAbstractUnit A2, int A3)
        {
            bool Result = default;
            Aow.Units.City.TCity TargetCity = default;
            Aow.Units.AoWE.TArmyHSList Armies = default;
            int I = default;
            if (A3 == 0)
            {
                TargetCity = ((Aow.Units.CitySpells.TCityTargetSpellTE)A1).GetSelectedCity();
                PlayDefaultCityEffects(A2, TargetCity);
                TargetCity.StartTownQuake();
                TargetCity.ValidateObservation(unchecked((sbyte)A2.FPlayer));
                Result = true;
            }
            else
            {
                if (A3 == 50)
                {
                    TargetCity = ((Aow.Units.CitySpells.TCityTargetSpellTE)A1).GetSelectedCity();
                    TargetCity.StopTownQuake();
                    Armies = new Aow.Units.AoWE.TArmyHSList(null);
                    var call__131 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(10);
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
        public override bool ValidTargetCity(Aow.Units.AoWE.TAbstractUnit Source, Aow.Units.City.TCity TargetCity)
        {
            bool Result = default;
            Result = base.ValidTargetCity(Source, TargetCity);
            if (!Result || TargetCity.FEnchantments.Contains(Aow.Units.City.TCityEnchantmentID.ceiAntiMagicShell))
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
        public void TriggerArmyDamage(Aow.Units.AoWE.TAbstractUnit Source, Aow.Units.AoWE.TArmyHS Army)
        {
            Aow.Units.AoWE.TArmyEventLog Log = default;
            bool Damaged = default;
            int I = default;
            int Damage = default;
            Aow.Units.AoWE.TAbstractUnit AUnit = default;
            Log = Aow.Units.AoWE.TArmyEventLog.Create(null);
            var call__134 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.TownQuakeDamageRStr);
            Log.FArmyText = call__134;
            Log.FPlayer = unchecked((sbyte)Source.FPlayer);
            var call__135 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.TownQuakeRStr);
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
                    Damage = AUnit.ExecuteDamageRole(7, 5, TSet<Aow.Units.AoWE.TDamageType>.Of(Aow.Units.AoWE.TDamageType.dtPhysical));
                    if (Damage > AUnit.GetHitPoints())
                    {
                        Damage = AUnit.GetHitPoints();
                    }

                    if (Damage > 0)
                    {
                        var call__138 = AUnit.GetHitPoints();
                        AUnit.SetHitPoints(unchecked((sbyte)(call__138 - Damage)));
                        Damaged = true;
                    }

                    Log.FDamageValues.Add(unchecked((byte)Damage));
                    Log.FDamageTypes.Add(7);
                }
            }

            if (Damaged)
            {
                Log.ValidateGameOver();
                var call__139 = Army.GetYhx();
                var call__140 = Army.GetLhx();
                var call__141 = Army.GetXhx();
                Aow.Units.EventLog.EventLog.DistributeLocationEventLog(Log, unchecked((sbyte)Army.FArmy.FPlayer), unchecked((sbyte)call__141), unchecked((sbyte)call__139), unchecked((sbyte)call__140), true);
            }

            Log.Release();
            Army.FArmy.Validate();
            Army.FArmy.EndUpdate();
        }

        public static new Aow.Units.CitySpells.TTownQuake Create(Aow.Units.Engine.TECustomNode AOwner)
            => Aow.Rtl.ConstructionContext.Create(AOwner, static (context, owner) => new TTownQuake(context, owner));
    }

    // { TAnarchy (instance size 60, VMT 557B0088) }
    public partial class TAnarchy : Aow.Units.CitySpells.TCityTargetSpell
    {
        // CitySpells.pas:849
        private protected TAnarchy(Aow.Rtl.ConstructionContext context, Aow.Units.Engine.TECustomNode AOwner) : base(context, AOwner)
        {
            var call__142 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.AnarchyRStr);
            FSpellName = call__142;
            FSpellID = 72;
            FCityTargetMode = TSet<Aow.Units.CitySpells.TCityTargetModeElement>.Of(Aow.Units.CitySpells.TCityTargetModeElement.ctmEnemy);
        }

        // CitySpells.pas:875
        public override bool ExecuteTE(Aow.Units.AoWE.TSpellTE A1, Aow.Units.AoWE.TAbstractUnit A2, int A3)
        {
            bool Result = default;
            Aow.Units.City.TCity TargetCity = default;
            Aow.Units.EventLog.TLocationTextEventLog Log = default;
            Aow.Units.Animation.TImageSequenceAnimation A = default;
            if (A3 == 0)
            {
                TargetCity = ((Aow.Units.CitySpells.TCityTargetSpellTE)A1).GetSelectedCity();
                Log = Aow.Units.EventLog.TLocationTextEventLog.Create(null);
                Log.FPlayer = unchecked((sbyte)A2.FPlayer);
                var call__143 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.XCastRStr);
                var call__144 = Aow.Rtl.Rtl.Format(call__143, [FSpellName]);
                Log.FText = call__144;
                var call__145 = TargetCity.GetLhx();
                var call__146 = TargetCity.GetYhx();
                var call__147 = TargetCity.GetXhx();
                Log.SetLocation(unchecked((sbyte)call__147), unchecked((sbyte)call__146), unchecked((sbyte)call__145));
                var call__148 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(TargetCity.FPlayer);
                call__148.FEventLogbook.AddEvent(Log);
                Log.Release();
                PlayDefaultCityEffects(A2, TargetCity);
                TargetCity.ValidateObservation(unchecked((sbyte)A2.FPlayer));
                var call__149 = TargetCity.Resource.ImageSequenceList.ImageSequences[16];
                if (call__149 != null)
                {
                    A = Aow.Units.Animation.TImageSequenceAnimation.Create(null);
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
                    Aow.Units.AoWE.AoWE.AoWHSMap.PlaceHX(A, call__153, call__152, call__151);
                }

                Result = true;
            }
            else
            {
                if (A3 == 50)
                {
                    TargetCity = ((Aow.Units.CitySpells.TCityTargetSpellTE)A1).GetSelectedCity();
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
        public override bool ValidTargetCity(Aow.Units.AoWE.TAbstractUnit Source, Aow.Units.City.TCity TargetCity)
        {
            bool Result = default;
            Result = base.ValidTargetCity(Source, TargetCity);
            if ((!Result || (TargetCity.FPlayer == 0)) || TargetCity.FEnchantments.Contains(Aow.Units.City.TCityEnchantmentID.ceiAntiMagicShell))
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
        public void ShowAnimation(Aow.Units.Animation.TAnimation Animation, int X, int Y, in Aow.Units.GFXE.TSurfaceDesc SD)
        {
            // { Unmatched: tried direct sequence receiver and uncast inherited frame field; stage sequence before reading frame counter; stage frame counter before sequence call (5 variants total). }
            var imageSequenceAnimation = (Aow.Units.Animation.TImageSequenceAnimation)Animation;
            var call__155 = imageSequenceAnimation.FImageSequence.GetUnloopedFrameIndex(imageSequenceAnimation.FFrameCounter);
            if ((call__155 == 0) && (((int)Aow.Units.AoWE.AoWE.AoWEngine.FAnimationDetail) != 0))
            {
                var call__156 = Aow.Rtl.Rtl.Random(2);
                Aow.Units.Sound.Sound.PlaySample(Aow.Units.AoWE.AoWE.ExplosionSFX[call__156]);
            }
        }

        public static new Aow.Units.CitySpells.TAnarchy Create(Aow.Units.Engine.TECustomNode AOwner)
            => Aow.Rtl.ConstructionContext.Create(AOwner, static (context, owner) => new TAnarchy(context, owner));
    }

    // { TAnimateRuins (instance size 60, VMT 557B01B4) }
    public partial class TAnimateRuins : Aow.Units.CitySpells.TCityTargetSpell
    {
        // CitySpells.pas:915
        private protected TAnimateRuins(Aow.Rtl.ConstructionContext context, Aow.Units.Engine.TECustomNode AOwner) : base(context, AOwner)
        {
            var call__157 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.AnimateRuinsRStr);
            FSpellName = call__157;
            FSpellID = 76;
            FCityTargetMode = TSet<Aow.Units.CitySpells.TCityTargetModeElement>.Of(Aow.Units.CitySpells.TCityTargetModeElement.ctmEnemy, Aow.Units.CitySpells.TCityTargetModeElement.ctmRazed);
        }

        // CitySpells.pas:938
        public override bool ExecuteTE(Aow.Units.AoWE.TSpellTE A1, Aow.Units.AoWE.TAbstractUnit A2, int A3)
        {
            bool Result = default;
            Aow.Units.City.TCity TargetCity = default;
            Aow.Units.EventLog.TLocationTextEventLog Log = default;
            Aow.Units.Animation.TImageSequenceAnimation A = default;
            int I = default;
            int Count = default;
            Aow.Units.AoWE.TUnit AUnit = default;
            if (A3 == 0)
            {
                TargetCity = ((Aow.Units.CitySpells.TCityTargetSpellTE)A1).GetSelectedCity();
                Log = Aow.Units.EventLog.TLocationTextEventLog.Create(null);
                Log.FPlayer = unchecked((sbyte)A2.FPlayer);
                var call__158 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.AnimateRuinsRStr);
                var call__159 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.XCastRStr);
                var call__160 = Aow.Rtl.Rtl.Format(call__159, [call__158]);
                Log.FText = call__160;
                var call__161 = TargetCity.GetLhx();
                var call__162 = TargetCity.GetYhx();
                var call__163 = TargetCity.GetXhx();
                Log.SetLocation(unchecked((sbyte)call__163), unchecked((sbyte)call__162), unchecked((sbyte)call__161));
                var call__164 = TargetCity.GetYhx();
                var call__165 = TargetCity.GetLhx();
                var call__166 = TargetCity.GetXhx();
                Aow.Units.EventLog.EventLog.DistributeLocationEventLog(Log, unchecked((sbyte)A2.FPlayer), unchecked((sbyte)call__166), unchecked((sbyte)call__164), unchecked((sbyte)call__165), false);
                Log.Release();
                PlayDefaultCityEffects(A2, TargetCity);
                TargetCity.ValidateObservation(unchecked((sbyte)A2.FPlayer));
                var call__167 = TargetCity.Resource.ImageSequenceList.ImageSequences[20];
                if (call__167 != null)
                {
                    A = Aow.Units.Animation.TImageSequenceAnimation.Create(null);
                    var call__168 = TargetCity.Resource.ImageSequenceList.ImageSequences[20];
                    A.SetImageSequence(call__168);
                    A.SetXhpOffset(A.FXhpOffset - 16);
                    A.SetYhpOffset(A.FYhpOffset - 16);
                    A.FOnShow = ShowAnimation;
                    var call__169 = TargetCity.GetLhx();
                    var call__170 = TargetCity.GetYhx();
                    var call__171 = TargetCity.GetXhx();
                    Aow.Units.AoWE.AoWE.AoWHSMap.PlaceHX(A, call__171, call__170, call__169);
                }

                Result = true;
            }
            else
            {
                if (A3 == 50)
                {
                    TargetCity = ((Aow.Units.CitySpells.TCityTargetSpellTE)A1).GetSelectedCity();
                    var call__172 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(A2.FPlayer);
                    TargetCity.SetRace(unchecked((sbyte)call__172.FPlayerRace));
                    TargetCity.SetRazed(false);
                    var call__173 = TargetCity.GetSize();
                    var call__174 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(call__173 * 2);
                    Count = call__174 + 1;
                    {
                        I = 0;
                        int end__175 = Count;
                        for (; I <= end__175; I += 1)
                        {
                            AUnit = new Aow.Units.AoWE.TUnit(null);
                            var call__176 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(2);
                            var call__177 = Aow.Units.AoWE.AoWE.AoWHSSet.FUnitResources.GetUnitResource(Aow.Units.CitySpells.CitySpells.TAnimateRuins_ExecuteTE__UndeadUnits[call__176]);
                            AUnit.SetUnitResource(call__177);
                            var call__178 = TargetCity.GetLhx();
                            var call__179 = TargetCity.GetYhx();
                            var call__180 = TargetCity.GetXhx();
                            AUnit.Place(call__180, call__179, call__178, unchecked((sbyte)A2.FPlayer), TSet<Aow.Units.AoWE.TUnitPlaceModeElement>.Of(Aow.Units.AoWE.TUnitPlaceModeElement.pmScanArea));
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
        public override bool ValidTargetCity(Aow.Units.AoWE.TAbstractUnit Source, Aow.Units.City.TCity TargetCity)
        {
            bool Result = default;
            Result = base.ValidTargetCity(Source, TargetCity);
            Result = Result && TargetCity.GetRazed();
            return Result;
        }

        // CitySpells.pas:930
        public void ShowAnimation(Aow.Units.Animation.TAnimation Animation, int X, int Y, in Aow.Units.GFXE.TSurfaceDesc SD)
        {
            // { Unmatched: tried direct sequence receiver and uncast inherited frame field; stage sequence before reading frame counter; stage frame counter before sequence call (5 variants total). }
            var imageSequenceAnimation = (Aow.Units.Animation.TImageSequenceAnimation)Animation;
            var call__182 = imageSequenceAnimation.FImageSequence.GetUnloopedFrameIndex(imageSequenceAnimation.FFrameCounter);
            if ((call__182 == 0) && (((int)Aow.Units.AoWE.AoWE.AoWEngine.FAnimationDetail) != 0))
            {
                var call__183 = Aow.Rtl.Rtl.Random(2);
                Aow.Units.Sound.Sound.PlaySample(Aow.Units.AoWE.AoWE.ExplosionSFX[call__183]);
            }
        }

        public static new Aow.Units.CitySpells.TAnimateRuins Create(Aow.Units.Engine.TECustomNode AOwner)
            => Aow.Rtl.ConstructionContext.Create(AOwner, static (context, owner) => new TAnimateRuins(context, owner));
    }

    public static partial class CitySpells
    {
        public static Aow.Units.FixedArray_int_2 TAnimateRuins_ExecuteTE__UndeadUnits = Aow.Units.FixedArray_int_2.From([198, 201]);
        // CitySpells.pas:988
        public static void RegisterCitySpells(Aow.Units.AoWE.TAoWEngine A1)
        {
            A1.RegisterEClasses([Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.CitySpells.TCityEnchantmentSpellTE>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.CitySpells.TCityTargetSpellTE>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.CitySpells.TTownGateTE>()]);
            var spellControl = A1.FGameSet.FSpellControl;
            var call__185 = Aow.Units.CitySpells.TAnarchy.Create(null);
            spellControl.RegisterSpell(call__185);
            var call__186 = Aow.Units.CitySpells.TTownQuake.Create(null);
            spellControl.RegisterSpell(call__186);
            var call__187 = Aow.Units.CitySpells.TTownGate.Create(null);
            spellControl.RegisterSpell(call__187);
            var call__188 = Aow.Units.CitySpells.TFountainOfLife.Create(null);
            spellControl.RegisterSpell(call__188);
            var call__189 = Aow.Units.CitySpells.TGoldRush.Create(null);
            spellControl.RegisterSpell(call__189);
            var call__190 = Aow.Units.CitySpells.TAntiMagicShell.Create(null);
            spellControl.RegisterSpell(call__190);
            var call__191 = Aow.Units.CitySpells.TWarmonger.Create(null);
            spellControl.RegisterSpell(call__191);
            var call__192 = Aow.Units.CitySpells.TAnimateRuins.Create(null);
            spellControl.RegisterSpell(call__192);
        }

        // CitySpells.pas:1004
        public static void UnregisterCitySpells(Aow.Units.AoWE.TAoWEngine A1)
        {
            A1.UnregisterEClasses([Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.CitySpells.TCityEnchantmentSpellTE>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.CitySpells.TCityTargetSpellTE>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.CitySpells.TTownGateTE>()]);
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
