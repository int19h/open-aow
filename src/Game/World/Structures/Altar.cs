#nullable disable
// Translated from Altar.pas.
using Aow.Engine.Nodes;
using Aow.Formats.Tree;
using Aow.Game.AI;
using Aow.Game.Combat;
using Aow.Game.Commands;
using Aow.Game.Draw.Animations;

using Aow.Game.Magic.Abilities;
using Aow.Game.Net;
using Aow.Game.Session;
using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.World.Structures;

// { ot=1 }
// Storm, Mode, RechargeDays
public partial class Altar(CustomNode owner) : PlayerStructure(owner)
{

    // Altar.pas:59
    public new const int ClassId = 132081;

    private Storm _storm;

    public AltarMode Mode { get; set; }
    public byte RechargeDays { get; private set; }

    // Altar.pas:226
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        RechargeDays = persister.Byte("RechargeDays", value: RechargeDays);
        Mode = persister.Enum("Mode", value: Mode);
    }

    // Altar.pas:248
    public override void SetRazed(bool value)
    {
        if (value)
        {
            SetPlayer(-1);
            Mode = AltarMode.Razed;
        }
        else
        {
            Mode = AltarMode.Idle;
        }

        ImageIndex = GetTerrainTypeImage(TerrainType);
    }

    // Altar.pas:243
    public override bool GetRazed() => Mode == AltarMode.Razed;

    // Altar.pas:78
    public override ExecuteResult ExecuteTokenEvent(StructureTokenEvent tokenEvent, TokenExecutor executor)
    {
        ExecuteResult result = base.ExecuteTokenEvent(tokenEvent, executor);
        if (tokenEvent.Action == 0)
        {
            string reason = "";
            if (executor.ExecuteCount == 0 && CanTarget(ref reason))
            {
                _storm = CreateStorm();
                Player player = StrategicMap.Current.Players.GetPlayers(Player);
                int targetMana = TargetMana();
                StrategicMap.Current.Players.GetPlayers(Player).MagicControl.SetMana(player.MagicControl.Mana - targetMana);
                RechargeDays = 0;
                _storm.InitializeStorm(Player, (sbyte)(tokenEvent.Param & 255), (sbyte)(((int)(((uint)tokenEvent.Param) >> 8)) & 255), (sbyte)(((int)(((uint)tokenEvent.Param) >> 16)) & 255));
            }

            if (_storm is not null)
            {
                if (!_storm.UpdateStorm())
                {
                    _storm.FinalizeStorm();
                    _storm?.Dispose();
                    _storm = null;
                    result = ExecuteResult.Done;
                    Update();
                }
                else
                {
                    result = ExecuteResult.Retry;
                }
            }
            else
            {
                result = ExecuteResult.DoneHalt;
            }
        }

        return result;
    }

    // Altar.pas:220
    public override void NewDay()
    {
        base.NewDay();
        if (GetRechargeDays() != RechargeDays)
        {
            RechargeDays = (byte)(RechargeDays + 1);
        }
    }

    // Altar.pas:70
    public override string GetCurrentActivityText()
    {
        string result;
        if (GetRechargeDays() != RechargeDays)
        {
            string formatDaysLeftString = DurationText.FormatDaysLeftString(GetEngine(), days: GetRechargeDays() - RechargeDays);
            result = $"{AbilityRules.TranslateResourceString(GetEngine(), s_chargingText)} ({formatDaysLeftString})";
        }
        else
        {
            result = AbilityRules.TranslateResourceString(GetEngine(), s_readyText);
        }

        return result;
    }

    // Altar.pas:259
    public override bool ExecuteAI(sbyte player, int phase)
    {
        if (player == Player && (phase is 1 or 2))
        {
            Storm storm = CreateStorm();
            sbyte hexLevel = GetHexLevel();
            sbyte hexY = GetHexY();
            if (storm.GetBestTarget(player, GetHexX(), hexY, hexLevel, 12, out MapCoordinate target, out StormDamageInfo damage))
            {
                int value = damage.EnemyDamage - damage.FriendlyDamage;
                if (value > TargetMana() * 0.75)
                {
                    Player playerAIBudget = StrategicMap.Current.Players.GetPlayers(player);
                    int targetMana = TargetMana();
                    if (StrategicMap.Current.Players.GetPlayers(player).AIBudget.RequestBudgetExpense(0, targetMana, playerAIBudget.AIBudget.DefaultBudget, true))
                    {
                        BattleFieldInfo.SetupBattleFieldInfo(out BattleFieldInfo friendly, player, 0, Set<BattleFieldInfoRequestsElement>.Of(), AbilityRules.AllDamageTypes - Set<DamageType>.Of(DamageType.Wall));
                        friendly.EnemyRelations = Set<byte>.WithBounds(0, 7);
                        sbyte hexLevel2 = GetHexLevel();
                        sbyte hexY2 = GetHexY();
                        sbyte hexX = GetHexX();
                        StrategicMap.Current.RetrieveBattleFieldInfo(hexX, hexY2, hexLevel2, 1, ref friendly);
                        BattleFieldInfo.SetupBattleFieldInfo(out BattleFieldInfo enemy, player, 0, Set<BattleFieldInfoRequestsElement>.Of(), AbilityRules.AllDamageTypes - Set<DamageType>.Of(DamageType.Wall));
                        sbyte hexLevel3 = GetHexLevel();
                        sbyte hexY3 = GetHexY();
                        sbyte hexX2 = GetHexX();
                        StrategicMap.Current.RetrieveBattleFieldInfo(hexX2, hexY3, hexLevel3, 8, ref enemy);
                        if (friendly.FriendlyStrength < enemy.Strength || value > 100)
                        {
                            if (StormTargetSelected(target.X, target.Y, target.MapLevel))
                            {
                                Player playerLocal = StrategicMap.Current.Players.GetPlayers(player);
                                int targetMana2 = TargetMana();
                                _ = StrategicMap.Current.Players.GetPlayers(player).AIBudget.BudgetExpenseMade(0, targetMana2, playerLocal.AIBudget.DefaultBudget, AIBudgetManager.ExpenseMode.PoolThenBudget);
                            }
                        }
                    }
                }
            }

            storm?.Dispose();
        }

        return false;
    }

    // Altar.pas:233
    public override void SetPlayer(sbyte player)
    {
        if (player != Player)
        {
            base.SetPlayer(player);
            if (player == Player && this == StrategicMap.Current.TargetSelection.Source)
            {
                StrategicMap.Current.TargetSelection.Deactivate();
            }
        }
    }

    // Altar.pas:65
    public override int GetDefaultAITargetPriority() => (RechargeDays * 10) + 10;

    // Altar.pas:62
    public virtual Storm CreateStorm() => null;

    // Altar.pas:110
    public virtual bool StormTargetSelected(sbyte X, sbyte Y, sbyte L)
    {
        bool result;
        string reason = "";
        if (CanTarget(ref reason) && !StrategicMap.Current.Players.GetPlayers(Player).GetBusy())
        {
            StructureTokenEvent eventValue = CreateTokenEvent();
            SetupTokenEvent(eventValue);
            eventValue.Param = X + (Y << 8) + (L << 16);
            eventValue.Action = 0;
            StrategicMap.Current.TokenManager.GetTokenControl(Player).AddEvent(eventValue);
            _ = eventValue.Release();
            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // Altar.pas:212
    public virtual byte GetRechargeDays() => 10;

    // Altar.pas:153
    public virtual bool CanTarget(ref string reason)
    {
        bool result;
        if (TargetMana() <= StrategicMap.Current.Players.GetPlayers(Player).MagicControl.Mana)
        {
            result = true;
        }
        else
        {
            result = false;
            reason = AbilityRules.TranslateResourceString(GetEngine(), GameText.NotEnoughManaText);
        }

        return result;
    }

    // Altar.pas:164
    public virtual bool Target()
    {
        string reason = "";
        TargetSelectionControl selection = StrategicMap.Current.TargetSelection;
        bool result;
        if (!GetRazed() && Player >= 0 && TargetMana() <= StrategicMap.Current.Players.GetPlayers(Player).MagicControl.Mana && CanTarget(ref reason) && _storm is null)
        {
            selection.Initialize(this);
            selection.TargetMode += Set<TargetSelectionModeElement>.Of(TargetSelectionModeElement.ValidateDistance, TargetSelectionModeElement.ExternalFloater);
            selection.SourceLocation = new MapCoordinate(GetHexX(), GetHexY(), GetHexLevel());
            selection.Range = 12;
            selection.Changed += OnTargetSelectionChange;
            _storm = CreateStorm();
            _storm.UpdateTargetSelection(StrategicMap.Current.TargetSelection);
            string name = _storm.Name();
            string format = DelphiFormat.Format(AbilityRules.TranslateResourceString(GetEngine(), GameText.SelectTargetLocationForOnMapFormat), [name]);
            selection.Text.Text = format;
            selection.Title = EditName();
            selection.CancelText = AbilityRules.TranslateResourceString(GetEngine(), GameText.CancelText);
            selection.SourceImage = GetTopBorderImage();
            selection.ValidTargetImage = GetMiddleBorderImage();
            selection.InvalidTargetImage = GetBottomBorderImage();
            selection.Activate();
            result = true;
            Update();
        }
        else
        {
            // { Original reports success even when targeting cannot start. }
            result = true;
        }

        return result;
    }

    // Altar.pas:202
    public virtual bool CancelTargeting()
    {
        bool result;
        if (GetTargeting())
        {
            StrategicMap.Current.TargetSelection.Deactivate();
            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // Altar.pas:215
    public virtual int TargetMana() => (GetRechargeDays() - RechargeDays) * 50;

    // Altar.pas:128
    public void OnTargetSelectionChange(TargetSelectionControl selection)
    {
        switch (selection.Change)
        {
            case TargetSelectionChange.Deactivated:
                _storm?.Dispose();
                _storm = null;
                selection.Changed -= OnTargetSelectionChange;
                Update();
                break;
            case TargetSelectionChange.Selected:
                selection.Changed -= OnTargetSelectionChange;
                if (StormTargetSelected(selection.Target.X, selection.Target.Y, selection.Target.MapLevel))
                {
                    selection.Deactivate();
                }

                _storm?.Dispose();
                _storm = null;
                Update();
                break;
            case TargetSelectionChange.None:
            case TargetSelectionChange.Activated:
            default:
                break;
        }
    }

    // Altar.pas:196
    public bool GetTargeting() => StrategicMap.Current.TargetSelection.Status.Contains(TargetSelectionStatusElement.Activated) && this == StrategicMap.Current.TargetSelection.Source;

    private static readonly string s_chargingText = "Charging";
    private static readonly string s_readyText = "Ready";

}

public enum AltarMode : byte
{
    Idle = 0, Razed = 1, Rebuilding = 2
}
