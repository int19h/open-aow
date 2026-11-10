// Translated from Altar.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Game.Ai;
using Aow.Game.Combat;
using Aow.Game.Commands;
using Aow.Game.Draw.Animations;
using Aow.Game.Engine;
using Aow.Game.Engine.Nodes;
using Aow.Game.Magic.Abilities;
using Aow.Game.Net;
using Aow.Game.Session;
using Aow.Game.World.Map;
using Aow.Game.World.Structures;
using Aow.Rtl;

namespace Aow.Game.World.Structures;

// { ot=1 }
// FStorm, FMode, FRechargeDays
public partial class Altar : PlayerStructure
{
    public Altar(ECustomNode owner) : base(owner)
    { }

    // Altar.pas:59
    public new const int ClassID = 132081;

    public Storm FStorm;
    public Mode FMode;
    public byte FRechargeDays;

    // Altar.pas:226
    public override void ReadWrite(EStorageStream Stream)
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
            FMode = Mode.Razed;
        }
        else
        {
            FMode = Mode.Idle;
        }

        FImageIndex = GetTerrainTypeImage(FTerrainType);
    }

    // Altar.pas:243
    public override bool GetRazed() => FMode == Mode.Razed;

    // Altar.pas:78
    public override ExecuteResult ExecuteTE(StructureTE TE, TokenExecuter Executer)
    {
        string Reason = "";
        ExecuteResult Result = base.ExecuteTE(TE, Executer);
        if (TE.FAction == 0)
        {
            if (Executer.ExecuteCount == 0 && CanTarget(ref Reason))
            {
                FStorm = CreateStorm();
                var player = AoWHSMap.Current.FPlayers.GetPlayers(FPlayer);
                var targetMana = TargetMana();
                AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FMagicControl.SetMana(player.FMagicControl.FMana - targetMana);
                FRechargeDays = 0;
                FStorm.InitializeStorm(FPlayer, (sbyte)(TE.FParam & 255), (sbyte)(((int)(((uint)TE.FParam) >> 8)) & 255), (sbyte)(((int)(((uint)TE.FParam) >> 16)) & 255));
            }

            if (FStorm != null)
            {
                if (!FStorm.UpdateStorm())
                {
                    FStorm.FinalizeStorm();
                    FStorm?.Dispose();
                    FStorm = null;
                    Result = ExecuteResult.Done;
                    Update();
                }
                else
                {
                    Result = ExecuteResult.Retry;
                }
            }
            else
            {
                Result = ExecuteResult.DoneHalt;
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
            var fmtDaysLeftStr = DurationText.FmtDaysLeftStr(GetRechargeDays() - FRechargeDays);
            Result = AbilityRules.TranslateRStr(ChargingRStr) + " (" + fmtDaysLeftStr + ")";
        }
        else
        {
            Result = AbilityRules.TranslateRStr(ReadyRStr);
        }

        return Result;
    }

    // Altar.pas:259
    public override bool ExecuteAI(sbyte Player, int Phase)
    {
        Storm Storm;
        XYL Target = default;
        BattleFieldInfo Friendly = default;
        BattleFieldInfo Enemy = default;
        StormDamageInfo Damage = default;
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
                    var playerAIBudget = AoWHSMap.Current.FPlayers.GetPlayers(Player);
                    var targetMana = TargetMana();
                    if (AoWHSMap.Current.FPlayers.GetPlayers(Player).FAIBudget.RequestBudgetExpense(0, targetMana, playerAIBudget.FAIBudget.FDefaultBudget, true))
                    {
                        BattleFieldInfo.SetupBattleFieldInfo(ref Friendly, Player, 0, Set<BattleFieldInfoRequestsElement>.Of(), AbilityRules.dtAll - Set<DamageType>.Of(DamageType.Wall));
                        Friendly.EnemyRelations = Set<byte>.WithBounds(0, 7);
                        var lhx2 = GetLhx();
                        var yhx2 = GetYhx();
                        var xhx = GetXhx();
                        AoWHSMap.Current.RetrieveBattleFieldInfo(xhx, yhx2, lhx2, 1, ref Friendly);
                        BattleFieldInfo.SetupBattleFieldInfo(ref Enemy, Player, 0, Set<BattleFieldInfoRequestsElement>.Of(), AbilityRules.dtAll - Set<DamageType>.Of(DamageType.Wall));
                        var lhx3 = GetLhx();
                        var yhx3 = GetYhx();
                        var xhx2 = GetXhx();
                        AoWHSMap.Current.RetrieveBattleFieldInfo(xhx2, yhx3, lhx3, 8, ref Enemy);
                        if (Friendly.FriendlyStrength < Enemy.Strength || Value > 100)
                        {
                            if (StormTargetSelected(Target.X, Target.Y, Target.L))
                            {
                                var player = AoWHSMap.Current.FPlayers.GetPlayers(Player);
                                var targetMana2 = TargetMana();
                                AoWHSMap.Current.FPlayers.GetPlayers(Player).FAIBudget.BudgetExpenseMade(0, targetMana2, player.FAIBudget.FDefaultBudget, AIBudgetManager.BudgetExpenseMode.PoolThenBudget);
                            }
                        }
                    }
                }
            }

            Storm?.Dispose();
        }

        return false;
    }

    // Altar.pas:233
    public override void SetPlayer(sbyte Player)
    {
        if (Player != FPlayer)
        {
            base.SetPlayer(Player);
            if (Player == FPlayer && this == AoWHSMap.Current.FTargetSelection.FSource)
            {
                AoWHSMap.Current.FTargetSelection.Deactivate();
            }
        }
    }

    // Altar.pas:65
    public override int GetDefaultAITargetPriority() => FRechargeDays * 10 + 10;

    // Altar.pas:62
    public virtual Storm CreateStorm() => null;

    // Altar.pas:110
    public virtual bool StormTargetSelected(sbyte X, sbyte Y, sbyte L)
    {
        bool Result;
        string Reason = "";
        StructureTE Event;
        if (CanTarget(ref Reason) && !AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).GetBusy())
        {
            Event = CreateTE();
            SetupTE(Event);
            Event.FParam = X + (Y << 8) + (L << 16);
            Event.FAction = 0;
            AoWHSMap.Current.FTokenManager.GetTokenControl(FPlayer).AddEvent(Event);
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
        if (TargetMana() <= AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FMagicControl.FMana)
        {
            Result = true;
        }
        else
        {
            Result = false;
            Reason = AbilityRules.TranslateRStr(GameText.NotEnoughManaRStr);
        }

        return Result;
    }

    // Altar.pas:164
    public virtual bool Target()
    {
        bool Result = default;
        TargetSelectionControl Selection = default;
        string Reason = "";
        Selection = AoWHSMap.Current.FTargetSelection;
        if (!GetRazed() && FPlayer >= 0 && TargetMana() <= AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FMagicControl.FMana && CanTarget(ref Reason) && FStorm == null)
        {
            Selection.Initialize(this);
            Selection.FTargetMode = Selection.FTargetMode + Set<TargetSelectionModeElement>.Of(TargetSelectionModeElement.ValidateDistance, TargetSelectionModeElement.ExternalFloater);
            Selection.FSourceX = GetXhx();
            Selection.FSourceY = GetYhx();
            Selection.FSourceL = GetLhx();
            Selection.FRange = 12;
            Selection.FOnChange += OnTargetSelectionChange;
            FStorm = CreateStorm();
            FStorm.UpdateTargetSelection(AoWHSMap.Current.FTargetSelection);
            var name = FStorm.Name();
            var format = RtlUnit.Format(AbilityRules.TranslateRStr(GameText.SelectTargetLocationForXOnMapRStr), [name]);
            Selection.FText.Text = format;
            Selection.FTitle = EditName();
            Selection.FCancelText = AbilityRules.TranslateRStr(GameText.CancelRStr);
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
            AoWHSMap.Current.FTargetSelection.Deactivate();
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
    public void OnTargetSelectionChange(TargetSelectionControl Selection)
    {
        switch (Selection.FChange)
        {
            case TargetSelectionChange.Deactivated:
                FStorm?.Dispose();
                FStorm = null;
                Selection.FOnChange -= OnTargetSelectionChange;
                Update();
                break;
            case TargetSelectionChange.Selected:
                Selection.FOnChange -= OnTargetSelectionChange;
                if (StormTargetSelected(Selection.FTargetX, Selection.FTargetY, Selection.FTargetL))
                {
                    Selection.Deactivate();
                }

                FStorm?.Dispose();
                FStorm = null;
                Update();
                break;
        }
    }

    // Altar.pas:196
    public bool GetTargeting() => AoWHSMap.Current.FTargetSelection.FStatus.Contains(TargetSelectionStatusElement.Activated) && this == AoWHSMap.Current.FTargetSelection.FSource;

    private static readonly string ChargingRStr = "Charging";
    private static readonly string ReadyRStr = "Ready";

    public enum Mode : byte
    {
        Idle = 0, Razed = 1, Rebuilding = 2
    }

}

public partial class AltarResource : PlayerStructureResource
{
    public AltarResource(ECustomNode owner) : base(owner)
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
