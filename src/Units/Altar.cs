// Translated from Altar.pas.
using Aow.Rtl;
using Aow.Units.Engine;
using Aow.Units.NetworkE;
using Aow.Units.HSEngine;
using Aow.Units.Storms;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;

namespace Aow.Units.Altar;

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

        FImageIndex = GetTerrainTypeImage(FTerrainType);
    }

    // Altar.pas:243
    public override bool GetRazed() => FMode == TAltarMode.amRazed;

    // Altar.pas:78
    public override TExecuteResult ExecuteTE(TStructureTE TE, TTokenExecuter Executer)
    {
        string Reason = "";
        TExecuteResult Result = base.ExecuteTE(TE, Executer);
        if (TE.FAction == 0)
        {
            if (Executer.ExecuteCount == 0 && CanTarget(ref Reason))
            {
                FStorm = CreateStorm();
                var player = AoWHSMap.FPlayers.GetPlayers(FPlayer);
                var targetMana = TargetMana();
                AoWHSMap.FPlayers.GetPlayers(FPlayer).FMagicControl.SetMana(player.FMagicControl.FMana - targetMana);
                FRechargeDays = 0;
                FStorm.InitializeStorm(FPlayer, (sbyte)(TE.FParam & 255), (sbyte)(((int)(((uint)TE.FParam) >> 8)) & 255), (sbyte)(((int)(((uint)TE.FParam) >> 16)) & 255));
            }

            if (FStorm != null)
            {
                if (!FStorm.UpdateStorm())
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
        if (GetRechargeDays() != FRechargeDays)
        {
            FRechargeDays = (byte)(FRechargeDays + 1);
        }
    }

    // Altar.pas:70
    public override string GetCurrentActivityText()
    {
        string Result;
        if (GetRechargeDays() != FRechargeDays)
        {
            var fmtDaysLeftStr = FmtDaysLeftStr(GetRechargeDays() - FRechargeDays);
            Result = TranslateRStr(ChargingRStr) + " (" + fmtDaysLeftStr + ")";
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
        TStorm Storm;
        TXYL Target = default;
        TBattleFieldInfo Friendly = default;
        TBattleFieldInfo Enemy = default;
        TStormDamageInfo Damage = default;
        int Value;
        if (Player == FPlayer && (Phase == 1 || Phase == 2))
        {
            Storm = CreateStorm();
            var lhx = GetLhx();
            var yhx = GetYhx();
            if (Storm.GetBestTarget(Player, GetXhx(), yhx, lhx, 12, ref Target, ref Damage))
            {
                Value = Damage.EnemyDamage - Damage.FriendlyDamage;
                if ((double)Value > (double)TargetMana() * 0.75)
                {
                    var playerAIBudget = AoWHSMap.FPlayers.GetPlayers(Player);
                    var targetMana = TargetMana();
                    if (AoWHSMap.FPlayers.GetPlayers(Player).FAIBudget.RequestBudgetExpense(0, targetMana, playerAIBudget.FAIBudget.FDefaultBudget, true))
                    {
                        SetupBattleFieldInfo(ref Friendly, Player, 0, TSet<TBattleFieldInfoRequestsElement>.Of(), dtAll - TSet<TDamageType>.Of(TDamageType.dtWall));
                        Friendly.EnemyRelations = TSet<byte>.WithBounds(0, 7);
                        var lhx2 = GetLhx();
                        var yhx2 = GetYhx();
                        var xhx = GetXhx();
                        AoWHSMap.RetrieveBattleFieldInfo(xhx, yhx2, lhx2, 1, ref Friendly);
                        SetupBattleFieldInfo(ref Enemy, Player, 0, TSet<TBattleFieldInfoRequestsElement>.Of(), dtAll - TSet<TDamageType>.Of(TDamageType.dtWall));
                        var lhx3 = GetLhx();
                        var yhx3 = GetYhx();
                        var xhx2 = GetXhx();
                        AoWHSMap.RetrieveBattleFieldInfo(xhx2, yhx3, lhx3, 8, ref Enemy);
                        if (Friendly.FriendlyStrength < Enemy.Strength || Value > 100)
                        {
                            if (StormTargetSelected(Target.X, Target.Y, Target.L))
                            {
                                var player = AoWHSMap.FPlayers.GetPlayers(Player);
                                var targetMana2 = TargetMana();
                                AoWHSMap.FPlayers.GetPlayers(Player).FAIBudget.BudgetExpenseMade(0, targetMana2, player.FAIBudget.FDefaultBudget, TBudgetExpenseMode.bemPoolThenBudget);
                            }
                        }
                    }
                }
            }

            Storm.Free();
        }

        return false;
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
    public override int GetDefaultAITargetPriority() => FRechargeDays * 10 + 10;

    // Altar.pas:62
    public virtual TStorm CreateStorm() => null;

    // Altar.pas:110
    public virtual bool StormTargetSelected(sbyte X, sbyte Y, sbyte L)
    {
        bool Result;
        string Reason = "";
        TStructureTE Event;
        if (CanTarget(ref Reason) && !AoWHSMap.FPlayers.GetPlayers(FPlayer).GetBusy())
        {
            Event = CreateTE();
            SetupTE(Event);
            Event.FParam = X + (Y << 8) + (L << 16);
            Event.FAction = 0;
            AoWHSMap.FTokenManager.GetTokenControl(FPlayer).AddEvent(Event);
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
    public virtual byte GetRechargeDays() => 10;

    // Altar.pas:153
    public virtual bool CanTarget(ref string Reason)
    {
        bool Result;
        if (TargetMana() <= AoWHSMap.FPlayers.GetPlayers(FPlayer).FMagicControl.FMana)
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
        string Reason = "";
        Selection = AoWHSMap.FTargetSelection;
        if (!GetRazed() && FPlayer >= 0 && TargetMana() <= AoWHSMap.FPlayers.GetPlayers(FPlayer).FMagicControl.FMana && CanTarget(ref Reason) && FStorm == null)
        {
            Selection.Initialize(this);
            Selection.FTargetMode = Selection.FTargetMode + TSet<TTargetSelectionModeElement>.Of(TTargetSelectionModeElement.tmValidateDistance, TTargetSelectionModeElement.tmExternalFloater);
            Selection.FSourceX = GetXhx();
            Selection.FSourceY = GetYhx();
            Selection.FSourceL = GetLhx();
            Selection.FRange = 12;
            Selection.FOnChange.Add(OnTargetSelectionChange);
            FStorm = CreateStorm();
            FStorm.UpdateTargetSelection(AoWHSMap.FTargetSelection);
            var name = FStorm.Name();
            var format = Rtl.Rtl.Format(TranslateRStr(SelectTargetLocationForXOnMapRStr), [name]);
            Selection.FText.Text = format;
            Selection.FTitle = EditName();
            Selection.FCancelText = TranslateRStr(CancelRStr);
            Selection.FSourceImage = GetTopBorderImage();
            Selection.FValidTargetImage = GetMiddleBorderImage();
            Selection.FInvalidTargetImage = GetBottomBorderImage();
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
        bool Result;
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
    public virtual int TargetMana() => (GetRechargeDays() - FRechargeDays) * 50;

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
                if (StormTargetSelected(Selection.FTargetX, Selection.FTargetY, Selection.FTargetL))
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
    public bool GetTargeting() => AoWHSMap.FTargetSelection.FStatus.Contains(TTargetSelectionStatusElement.tsActivated) && this == AoWHSMap.FTargetSelection.FSource;

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
    public override int ResourceUserClassID() => 132081;

    // Altar.pas:49


    // Altar.pas:304
    public override string ResourceEditName() => "";

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
