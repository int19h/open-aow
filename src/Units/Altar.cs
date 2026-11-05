// Translated from Altar.pas.
using Aow.Rtl;
using Aow.Units.Engine;
using Aow.Units.NetworkE;
using Aow.Units.HSEngine;
using Aow.Units.Storms;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;

namespace Aow.Units.Altar
{
    // { ot=1 }
    // FStorm, FMode, FRechargeDays
    public partial class TAltar : TPlayerStructure
    {
        public TAltar(TECustomNode owner) : base(owner)
        {}

        // Altar.pas:59
        public new const int ClassID = 132081;

        public TStorm FStorm;
        public TAltarMode FMode;
        public byte FRechargeDays;
        // Altar.pas:226
        public override void ReadWrite(TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwByte(ref FRechargeDays, 15);
            Stream.rwData(Bytes.Of(ref FMode, 1), 1, 16);
        }

        // Altar.pas:248
        public override void SetRazed(bool Value)
        {
            if (Value)
            {
                SetPlayer(-1);
                FMode = TAltarMode.amRazed;
            }
            else
            {
                FMode = TAltarMode.amIdle;
            }

            var call__1 = GetTerrainTypeImage(FTerrainType);
            FImageIndex = call__1;
        }

        // Altar.pas:243
        public override bool GetRazed()
        {
            bool Result = default;
            Result = FMode == TAltarMode.amRazed;
            return Result;
        }

        // Altar.pas:78
        public override TExecuteResult ExecuteTE(TStructureTE A1, TTokenExecuter Executer)
        {
            TExecuteResult Result = default;
            AnsiString Reason = default;
            Result = base.ExecuteTE(A1, Executer);
            if (A1.FAction == 0)
            {
                bool bool__2 = Executer.ExecuteCount == 0;
                if (bool__2)
                {
                    var call__3 = CanTarget(ref Reason);
                    bool__2 = call__3;
                }

                if (bool__2)
                {
                    var call__4 = CreateStorm();
                    FStorm = call__4;
                    var call__5 = AoWHSMap.FPlayers.GetPlayers(FPlayer);
                    var call__6 = TargetMana();
                    var call__7 = AoWHSMap.FPlayers.GetPlayers(FPlayer);
                    call__7.FMagicControl.SetMana(call__5.FMagicControl.FMana - call__6);
                    FRechargeDays = 0;
                    FStorm.InitializeStorm(FPlayer, (sbyte)(A1.FParam & 255), (sbyte)(((int)(((uint)A1.FParam) >> 8)) & 255), (sbyte)(((int)(((uint)A1.FParam) >> 16)) & 255));
                }

                if (FStorm != null)
                {
                    var call__8 = FStorm.UpdateStorm();
                    if (!call__8)
                    {
                        FStorm.FinalizeStorm();
                        FStorm.Free();
                        FStorm = null;
                        Result = TExecuteResult.erDone;
                        Update();
                    }
                    else
                    {
                        Result = TExecuteResult.erRetry;
                    }
                }
                else
                {
                    Result = TExecuteResult.erDoneHalt;
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
                FRechargeDays = (byte)(FRechargeDays + 1);
            }
        }

        // Altar.pas:70
        public override AnsiString GetCurrentActivityText()
        {
            AnsiString Result = default;
            var call__10 = GetRechargeDays();
            if (call__10 != FRechargeDays)
            {
                var call__11 = GetRechargeDays();
                var call__12 = FmtDaysLeftStr(call__11 - FRechargeDays);
                var call__13 = TranslateRStr(ChargingRStr);
                Result = call__13 + (AnsiString)" (" + call__12 + AnsiString.FromBytes(41);
            }
            else
            {
                Result = TranslateRStr(ReadyRStr);
            }

            return Result;
        }

        // Altar.pas:259
        public override bool ExecuteAI(sbyte Player, int Phase)
        {
            bool Result = default;
            TStorm Storm = default;
            TXYL Target = default;
            TBattleFieldInfo Friendly = default;
            TBattleFieldInfo Enemy = default;
            TStormDamageInfo Damage = default;
            int Value = default;
            if (Player == FPlayer && (Phase == 1 || Phase == 2))
            {
                Storm = CreateStorm();
                var call__14 = GetLhx();
                var call__15 = GetYhx();
                var call__16 = GetXhx();
                var call__17 = Storm.GetBestTarget(Player, call__16, call__15, call__14, 12, ref Target, ref Damage);
                if (call__17)
                {
                    Value = Damage.EnemyDamage - Damage.FriendlyDamage;
                    if ((double)Value > (double)TargetMana() * 0.75)
                    {
                        var call__18 = AoWHSMap.FPlayers.GetPlayers(Player);
                        var call__19 = TargetMana();
                        var call__20 = AoWHSMap.FPlayers.GetPlayers(Player);
                        var call__21 = call__20.FAIBudget.RequestBudgetExpense(0, call__19, call__18.FAIBudget.FDefaultBudget, true);
                        if (call__21)
                        {
                            SetupBattleFieldInfo(ref Friendly, Player, 0, TSet<TBattleFieldInfoRequestsElement>.Of(), dtAll - TSet<TDamageType>.Of(TDamageType.dtWall));
                            Friendly.EnemyRelations = TSet<byte>.WithBounds(0, 7);
                            var call__22 = GetLhx();
                            var call__23 = GetYhx();
                            var call__24 = GetXhx();
                            AoWHSMap.RetrieveBattleFieldInfo(call__24, call__23, call__22, 1, ref Friendly);
                            SetupBattleFieldInfo(ref Enemy, Player, 0, TSet<TBattleFieldInfoRequestsElement>.Of(), dtAll - TSet<TDamageType>.Of(TDamageType.dtWall));
                            var call__25 = GetLhx();
                            var call__26 = GetYhx();
                            var call__27 = GetXhx();
                            AoWHSMap.RetrieveBattleFieldInfo(call__27, call__26, call__25, 8, ref Enemy);
                            if (Friendly.FriendlyStrength < Enemy.Strength || Value > 100)
                            {
                                var call__28 = StormTargetSelected(Target.X, Target.Y, Target.L);
                                if (call__28)
                                {
                                    var call__29 = AoWHSMap.FPlayers.GetPlayers(Player);
                                    var call__30 = TargetMana();
                                    var call__31 = AoWHSMap.FPlayers.GetPlayers(Player);
                                    call__31.FAIBudget.BudgetExpenseMade(0, call__30, call__29.FAIBudget.FDefaultBudget, TBudgetExpenseMode.bemPoolThenBudget);
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
                base.SetPlayer(Player);
                if (Player == FPlayer && this == AoWHSMap.FTargetSelection.FSource)
                {
                    AoWHSMap.FTargetSelection.Deactivate();
                }
            }
        }

        // Altar.pas:65
        public override int GetDefaultAITargetPriority()
        {
            int Result = default;
            Result = FRechargeDays * 10 + 10;
            return Result;
        }

        // Altar.pas:62
        public virtual TStorm CreateStorm()
        {
            TStorm Result = default;
            Result = null;
            return Result;
        }

        // Altar.pas:110
        public virtual bool StormTargetSelected(sbyte X, sbyte Y, sbyte L)
        {
            bool Result = default;
            AnsiString Reason = default;
            TStructureTE Event = default;
            var call__32 = CanTarget(ref Reason);
            bool bool__33 = call__32;
            if (bool__33)
            {
                var call__34 = AoWHSMap.FPlayers.GetPlayers(FPlayer);
                var call__35 = call__34.GetBusy();
                bool__33 = !call__35;
            }

            if (bool__33)
            {
                Event = CreateTE();
                SetupTE(Event);
                Event.FParam = X + (Y << 8) + (L << 16);
                Event.FAction = 0;
                var call__36 = AoWHSMap.FTokenManager.GetTokenControl(FPlayer);
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
        public virtual bool CanTarget(ref AnsiString Reason)
        {
            bool Result = default;
            var call__37 = TargetMana();
            var call__38 = AoWHSMap.FPlayers.GetPlayers(FPlayer);
            if (call__37 <= call__38.FMagicControl.FMana)
            {
                Result = true;
            }
            else
            {
                Result = false;
                Reason = TranslateRStr(NotEnoughManaRStr);
            }

            return Result;
        }

        // Altar.pas:164
        public virtual bool Target()
        {
            bool Result = default;
            TTargetSelectionControl Selection = default;
            AnsiString Reason = default;
            Selection = AoWHSMap.FTargetSelection;
            var call__39 = GetRazed();
            bool bool__40 = !call__39 && FPlayer >= 0;
            if (bool__40)
            {
                var call__41 = TargetMana();
                var call__42 = AoWHSMap.FPlayers.GetPlayers(FPlayer);
                bool__40 = call__41 <= call__42.FMagicControl.FMana;
            }

            bool bool__43 = bool__40;
            if (bool__43)
            {
                var call__44 = CanTarget(ref Reason);
                bool__43 = call__44;
            }

            if (bool__43 && FStorm == null)
            {
                Selection.Initialize(this);
                Selection.FTargetMode = Selection.FTargetMode + TSet<TTargetSelectionModeElement>.Of(TTargetSelectionModeElement.tmValidateDistance, TTargetSelectionModeElement.tmExternalFloater);
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
                FStorm.UpdateTargetSelection(AoWHSMap.FTargetSelection);
                var call__49 = FStorm.Name();
                var call__50 = TranslateRStr(SelectTargetLocationForXOnMapRStr);
                var call__51 = Rtl.Rtl.Format(call__50, [call__49]);
                Selection.FText.Text = call__51;
                var call__52 = EditName();
                Selection.FTitle = call__52;
                var call__53 = TranslateRStr(CancelRStr);
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
                AoWHSMap.FTargetSelection.Deactivate();
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
        public void OnTargetSelectionChange(TObject Sender)
        {
            TTargetSelectionControl Selection = default;
            Selection = (TTargetSelectionControl)Sender;
            switch (Selection.FChange)
            {
                case TTargetSelectionChange.tscDeactivated:
                    FStorm.Free();
                    FStorm = null;
                    Selection.FOnChange.Remove(OnTargetSelectionChange);
                    Update();
                    break;
                case TTargetSelectionChange.tscSelected:
                    Selection.FOnChange.Remove(OnTargetSelectionChange);
                    var call__58 = StormTargetSelected(Selection.FTargetX, Selection.FTargetY, Selection.FTargetL);
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
            Result = AoWHSMap.FTargetSelection.FStatus.Contains(TTargetSelectionStatusElement.tsActivated) && this == AoWHSMap.FTargetSelection.FSource;
            return Result;
        }

    }

    public partial class TAltarResource : TPlayerStructureResource
    {
        public TAltarResource(TECustomNode owner) : base(owner)
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
        public override AnsiString ResourceEditName()
        {
            AnsiString Result = default;
            Result = (AnsiString)"";
            return Result;
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
        public static void RegisterAltar(TEngine AEngine)
        {
            AEngine.RegisterEClasses([EClass.Of<TAltar>(), EClass.Of<TAltarResource>()]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
