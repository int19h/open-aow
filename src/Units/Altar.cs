// Translated from Altar.pas.
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
using Aow.Units.Storms;
using static Aow.Units.Storms.Storms;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;

namespace Aow.Units.Altar
{
    // { ot=1 }
    // { TAltar (instance size 68): $3C FStorm, $40 FMode, $41 FRechargeDays;
//     $42..$43 alignment padding. VMT 557CE7C4. }
    public partial class TAltar : Aow.Units.AoWE.TPlayerStructure
    {
        public TAltar(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
        {}

        // Altar.pas:59
        public new const int ClassID = 132081;

        public Aow.Units.Storms.TStorm FStorm;
        // { $3C }
        public Aow.Units.Altar.TAltarMode FMode;
        // { $40 }
        public byte FRechargeDays;
        // Altar.pas:226
        // { $41 }
        // { $42..$43 alignment padding }
        public override void ReadWrite(Aow.Units.Engine.TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwByte(ref FRechargeDays, 15);
            Stream.rwData(Aow.Rtl.Bytes.Of(ref FMode, 1), 1, 16);
        }

        // Altar.pas:248
        public override void SetRazed(bool Value)
        {
            if (Value)
            {
                SetPlayer(unchecked((sbyte)-1));
                FMode = Aow.Units.Altar.TAltarMode.amRazed;
            }
            else
            {
                FMode = Aow.Units.Altar.TAltarMode.amIdle;
            }

            var call__1 = GetTerrainTypeImage(FTerrainType);
            FImageIndex = call__1;
        }

        // Altar.pas:243
        public override bool GetRazed()
        {
            bool Result = default;
            Result = FMode == Aow.Units.Altar.TAltarMode.amRazed;
            return Result;
        }

        // Altar.pas:78
        public override Aow.Units.NetworkE.TExecuteResult ExecuteTE(Aow.Units.AoWE.TStructureTE A1, Aow.Units.NetworkE.TTokenExecuter Executer)
        {
            Aow.Units.NetworkE.TExecuteResult Result = default;
            Aow.Rtl.AnsiString Reason = default;
            Result = base.ExecuteTE(A1, Executer);
            if (A1.FAction == 0)
            {
                bool bool__2 = (Executer.ExecuteCount == 0);
                if (bool__2)
                {
                    var call__3 = CanTarget(ref Reason);
                    bool__2 = call__3;
                }

                if (bool__2)
                {
                    var call__4 = CreateStorm();
                    FStorm = call__4;
                    var call__5 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayer);
                    var call__6 = TargetMana();
                    var call__7 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayer);
                    call__7.FMagicControl.SetMana(call__5.FMagicControl.FMana - call__6);
                    FRechargeDays = 0;
                    FStorm.InitializeStorm(unchecked((sbyte)FPlayer), unchecked((sbyte)unchecked((sbyte)(A1.FParam & 255))), unchecked((sbyte)unchecked((sbyte)(((int)(((uint)A1.FParam) >> 8)) & 255))), unchecked((sbyte)unchecked((sbyte)(((int)(((uint)A1.FParam) >> 16)) & 255))));
                }

                if (FStorm != null)
                {
                    var call__8 = FStorm.UpdateStorm();
                    if (!call__8)
                    {
                        FStorm.FinalizeStorm();
                        FStorm.Free();
                        FStorm = null;
                        Result = Aow.Units.NetworkE.TExecuteResult.erDone;
                        Update();
                    }
                    else
                    {
                        Result = Aow.Units.NetworkE.TExecuteResult.erRetry;
                    }
                }
                else
                {
                    Result = Aow.Units.NetworkE.TExecuteResult.erDoneHalt;
                }
            }

            return Result;
        }

        // Altar.pas:220
        public override void NewDay()
        {
            base.NewDay();
            var call__9 = GetRechargeDays();
            if (call__9 != FRechargeDays)
            {
                FRechargeDays = (byte)(((int)FRechargeDays) + 1);
            }
        }

        // Altar.pas:70
        public override Aow.Rtl.AnsiString GetCurrentActivityText()
        {
            Aow.Rtl.AnsiString Result = default;
            var call__10 = GetRechargeDays();
            if (call__10 != FRechargeDays)
            {
                var call__11 = GetRechargeDays();
                var call__12 = Aow.Units.AoWE.AoWE.FmtDaysLeftStr(call__11 - FRechargeDays);
                var call__13 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.ChargingRStr);
                Result = ((call__13 + ((AnsiString)" (")) + call__12) + AnsiString.FromBytes(41);
            }
            else
            {
                Result = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.ReadyRStr);
            }

            return Result;
        }

        // Altar.pas:259
        public override bool ExecuteAI(sbyte Player, int Phase)
        {
            bool Result = default;
            Aow.Units.Storms.TStorm Storm = default;
            Aow.Units.HSEngine.TXYL Target = default;
            Aow.Units.AoWE.TBattleFieldInfo Friendly = default;
            Aow.Units.AoWE.TBattleFieldInfo Enemy = default;
            Aow.Units.AoWE.TStormDamageInfo Damage = default;
            int Value = default;
            if ((Player == FPlayer) && ((Phase == 1) || (Phase == 2)))
            {
                Storm = CreateStorm();
                var call__14 = GetLhx();
                var call__15 = GetYhx();
                var call__16 = GetXhx();
                var call__17 = Storm.GetBestTarget(unchecked((sbyte)Player), call__16, call__15, call__14, 12, ref Target, ref Damage);
                if (call__17)
                {
                    Value = Damage.EnemyDamage - Damage.FriendlyDamage;
                    if (((double)Value) > (((double)TargetMana()) * 0.75))
                    {
                        var call__18 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Player);
                        var call__19 = TargetMana();
                        var call__20 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Player);
                        var call__21 = call__20.FAIBudget.RequestBudgetExpense(0, call__19, call__18.FAIBudget.FDefaultBudget, true);
                        if (call__21)
                        {
                            Aow.Units.AoWE.AoWE.SetupBattleFieldInfo(ref Friendly, unchecked((sbyte)Player), 0, TSet<Aow.Units.AoWE.TBattleFieldInfoRequestsElement>.Of(), Aow.Units.AoWE.AoWE.dtAll - TSet<Aow.Units.AoWE.TDamageType>.Of(Aow.Units.AoWE.TDamageType.dtWall));
                            Friendly.EnemyRelations = TSet<byte>.WithBounds(0, 7);
                            var call__22 = GetLhx();
                            var call__23 = GetYhx();
                            var call__24 = GetXhx();
                            Aow.Units.AoWE.AoWE.AoWHSMap.RetrieveBattleFieldInfo(call__24, call__23, call__22, 1, ref Friendly);
                            Aow.Units.AoWE.AoWE.SetupBattleFieldInfo(ref Enemy, unchecked((sbyte)Player), 0, TSet<Aow.Units.AoWE.TBattleFieldInfoRequestsElement>.Of(), Aow.Units.AoWE.AoWE.dtAll - TSet<Aow.Units.AoWE.TDamageType>.Of(Aow.Units.AoWE.TDamageType.dtWall));
                            var call__25 = GetLhx();
                            var call__26 = GetYhx();
                            var call__27 = GetXhx();
                            Aow.Units.AoWE.AoWE.AoWHSMap.RetrieveBattleFieldInfo(call__27, call__26, call__25, 8, ref Enemy);
                            if ((Friendly.FriendlyStrength < Enemy.Strength) || (Value > 100))
                            {
                                var call__28 = StormTargetSelected(unchecked((sbyte)Target.X), unchecked((sbyte)Target.Y), unchecked((sbyte)Target.L));
                                if (call__28)
                                {
                                    var call__29 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Player);
                                    var call__30 = TargetMana();
                                    var call__31 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Player);
                                    call__31.FAIBudget.BudgetExpenseMade(0, call__30, call__29.FAIBudget.FDefaultBudget, Aow.Units.AoWE.TBudgetExpenseMode.bemPoolThenBudget);
                                }
                            }
                        }
                    }
                }

                Storm.Free();
            }

            Result = false;
            return Result;
        }

        // Altar.pas:233
        public override void SetPlayer(sbyte Player)
        {
            if (Player != FPlayer)
            {
                base.SetPlayer(unchecked((sbyte)Player));
                if ((Player == FPlayer) && (this == Aow.Units.AoWE.AoWE.AoWHSMap.FTargetSelection.FSource))
                {
                    Aow.Units.AoWE.AoWE.AoWHSMap.FTargetSelection.Deactivate();
                }
            }
        }

        // Altar.pas:65
        public override int GetDefaultAITargetPriority()
        {
            int Result = default;
            Result = (FRechargeDays * 10) + 10;
            return Result;
        }

        // Altar.pas:62
        public virtual Aow.Units.Storms.TStorm CreateStorm()
        {
            Aow.Units.Storms.TStorm Result = default;
            Result = null;
            return Result;
        }

        // Altar.pas:110
        public virtual bool StormTargetSelected(sbyte X, sbyte Y, sbyte L)
        {
            bool Result = default;
            Aow.Rtl.AnsiString Reason = default;
            Aow.Units.AoWE.TStructureTE Event = default;
            var call__32 = CanTarget(ref Reason);
            bool bool__33 = call__32;
            if (bool__33)
            {
                var call__34 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayer);
                var call__35 = call__34.GetBusy();
                bool__33 = !call__35;
            }

            if (bool__33)
            {
                Event = CreateTE();
                SetupTE(Event);
                Event.FParam = (X + (Y << 8)) + (L << 16);
                Event.FAction = 0;
                var call__36 = Aow.Units.AoWE.AoWE.AoWHSMap.FTokenManager.GetTokenControl(FPlayer);
                call__36.AddEvent(Event);
                Event.Release();
                Result = true;
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // Altar.pas:212
        public virtual byte GetRechargeDays()
        {
            byte Result = default;
            Result = 10;
            return Result;
        }

        // Altar.pas:153
        public virtual bool CanTarget(ref Aow.Rtl.AnsiString Reason)
        {
            bool Result = default;
            var call__37 = TargetMana();
            var call__38 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayer);
            if (call__37 <= call__38.FMagicControl.FMana)
            {
                Result = true;
            }
            else
            {
                Result = false;
                Reason = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.NotEnoughManaRStr);
            }

            return Result;
        }

        // Altar.pas:164
        public virtual bool Target()
        {
            bool Result = default;
            Aow.Units.AoWE.TTargetSelectionControl Selection = default;
            Aow.Rtl.AnsiString Reason = default;
            Selection = Aow.Units.AoWE.AoWE.AoWHSMap.FTargetSelection;
            var call__39 = GetRazed();
            bool bool__40 = !call__39 && (FPlayer >= 0);
            if (bool__40)
            {
                var call__41 = TargetMana();
                var call__42 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayer);
                bool__40 = (call__41 <= call__42.FMagicControl.FMana);
            }

            bool bool__43 = bool__40;
            if (bool__43)
            {
                var call__44 = CanTarget(ref Reason);
                bool__43 = call__44;
            }

            if (bool__43 && (FStorm == null))
            {
                Selection.Initialize(this);
                Selection.FTargetMode = Selection.FTargetMode + TSet<Aow.Units.AoWE.TTargetSelectionModeElement>.Of(Aow.Units.AoWE.TTargetSelectionModeElement.tmValidateDistance, Aow.Units.AoWE.TTargetSelectionModeElement.tmExternalFloater);
                var call__45 = GetXhx();
                Selection.FSourceX = call__45;
                var call__46 = GetYhx();
                Selection.FSourceY = call__46;
                var call__47 = GetLhx();
                Selection.FSourceL = call__47;
                Selection.FRange = 12;
                Selection.FOnChange.Add(OnTargetSelectionChange);
                var call__48 = CreateStorm();
                FStorm = call__48;
                FStorm.UpdateTargetSelection(Aow.Units.AoWE.AoWE.AoWHSMap.FTargetSelection);
                var call__49 = FStorm.Name();
                var call__50 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.SelectTargetLocationForXOnMapRStr);
                var call__51 = Aow.Rtl.Rtl.Format(call__50, [call__49]);
                Selection.FText.Text = call__51;
                var call__52 = EditName();
                Selection.FTitle = call__52;
                var call__53 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.CancelRStr);
                Selection.FCancelText = call__53;
                var call__54 = GetTopBorderImage();
                Selection.FSourceImage = call__54;
                var call__55 = GetMiddleBorderImage();
                Selection.FValidTargetImage = call__55;
                var call__56 = GetBottomBorderImage();
                Selection.FInvalidTargetImage = call__56;
                Selection.Activate();
                Result = true;
                Update();
            }
            else
            {
                // { Original reports success even when targeting cannot start. }
                Result = true;
            }

            return Result;
        }

        // Altar.pas:202
        public virtual bool CancelTargeting()
        {
            bool Result = default;
            if (GetTargeting())
            {
                Aow.Units.AoWE.AoWE.AoWHSMap.FTargetSelection.Deactivate();
                Result = true;
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // Altar.pas:215
        public virtual int TargetMana()
        {
            int Result = default;
            var call__57 = GetRechargeDays();
            Result = (call__57 - FRechargeDays) * 50;
            return Result;
        }

        // Altar.pas:128
        public void OnTargetSelectionChange(Aow.Rtl.TObject Sender)
        {
            Aow.Units.AoWE.TTargetSelectionControl Selection = default;
            Selection = (Aow.Units.AoWE.TTargetSelectionControl)Sender;
            switch (Selection.FChange)
            {
                case Aow.Units.AoWE.TTargetSelectionChange.tscDeactivated:
                    FStorm.Free();
                    FStorm = null;
                    Selection.FOnChange.Remove(OnTargetSelectionChange);
                    Update();
                    break;
                case Aow.Units.AoWE.TTargetSelectionChange.tscSelected:
                    Selection.FOnChange.Remove(OnTargetSelectionChange);
                    var call__58 = StormTargetSelected(unchecked((sbyte)Selection.FTargetX), unchecked((sbyte)Selection.FTargetY), unchecked((sbyte)Selection.FTargetL));
                    if (call__58)
                    {
                        Selection.Deactivate();
                    }

                    FStorm.Free();
                    FStorm = null;
                    Update();
                    break;
            }
        }

        // Altar.pas:196
        public bool GetTargeting()
        {
            bool Result = default;
            Result = Aow.Units.AoWE.AoWE.AoWHSMap.FTargetSelection.FStatus.Contains(Aow.Units.AoWE.TTargetSelectionStatusElement.tsActivated) && (this == Aow.Units.AoWE.AoWE.AoWHSMap.FTargetSelection.FSource);
            return Result;
        }

        public new partial class Meta : Aow.Units.AoWE.TPlayerStructure.Meta
        {
            // Altar.pas:59
            public override int ClassID() => Aow.Units.Altar.TAltar.ClassID;

            public static new Meta Of<T>()
                where T : Aow.Units.Altar.TAltar => (Meta)MetaRegistry.Of(typeof(T));
            // intf_P1_09.inc:249
            public new Aow.Units.Altar.TAltar Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.Altar.TAltar)base.Create(AOwner);
        }
    }

    // { TAltarResource (instance size 116, no added fields, VMT 557CEA6C) }
    public partial class TAltarResource : Aow.Units.AoWE.TPlayerStructureResource
    {
        public TAltarResource(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
        {
            FClaimed = false;
            FRazeable = true;
            FRazeDefenderBudget = 150;
            FRebuildGold = 50;
            FRebuildDays = 2;
        }

        // Altar.pas:307
        public new const int ClassID = 132082;

        // Altar.pas:294


        // Altar.pas:310
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 132081;
            return Result;
        }

        // Altar.pas:49


        // Altar.pas:304
        public override Aow.Rtl.AnsiString ResourceEditName()
        {
            Aow.Rtl.AnsiString Result = default;
            Result = (AnsiString)"";
            return Result;
        }

        public new partial class Meta : Aow.Units.AoWE.TPlayerStructureResource.Meta
        {
            // Altar.pas:307
            public override int ClassID() => Aow.Units.Altar.TAltarResource.ClassID;

            // Altar.pas:49
            public new Aow.Units.Altar.TAltarResource Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.Altar.TAltarResource)base.Create(AOwner);

            public static new Meta Of<T>()
                where T : Aow.Units.Altar.TAltarResource => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    public enum TAltarMode : byte
    {
        amIdle = 0,
        amRazed = 1,
        amRebuilding = 2
    }

    public static partial class Altar
    {
        // Altar.pas:313
        public static void RegisterAltar(Aow.Units.Engine.TEngine AEngine)
        {
            AEngine.RegisterEClasses([Aow.Units.Engine.EClass.Of<Aow.Units.Altar.TAltar>(), Aow.Units.Engine.EClass.Of<Aow.Units.Altar.TAltarResource>()]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
