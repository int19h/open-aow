// Translated from Altar.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Formats.Tree;
using Aow.Game.AI;
using Aow.Game.Combat;
using Aow.Game.Commands;
using Aow.Game.Draw.Animations;

using Aow.Game.Magic.Abilities;
using Aow.Game.Net;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.World.Structures;

// { ot=1 }
// Storm, Mode, RechargeDays
public partial class Altar : PlayerStructure
{
    public Altar(ECustomNode owner) : base(owner)
    { }

    // Altar.pas:59
    public new const int ClassId = 132081;

    private Storm _storm;
    public AltarMode Mode;
    public byte RechargeDays;

    // Altar.pas:226
    public override void ReadWrite(StorageStream stream)
    {
        base.ReadWrite(stream);
        stream.Byte(ref RechargeDays, 15);
        stream.Bytes(Bytes.Of(ref Mode, 1), 1, 16);
    }

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
        string reason = "";
        ExecuteResult result = base.ExecuteTokenEvent(tokenEvent, executor);
        if (tokenEvent.Action == 0)
        {
            if (executor.ExecuteCount == 0 && CanTarget(ref reason))
            {
                _storm = CreateStorm();
                var player = StrategicMap.Current.Players.GetPlayers(Player);
                var targetMana = TargetMana();
                StrategicMap.Current.Players.GetPlayers(Player).MagicControl.SetMana(player.MagicControl.Mana - targetMana);
                RechargeDays = 0;
                _storm.InitializeStorm(Player, (sbyte)(tokenEvent.Param & 255), (sbyte)(((int)(((uint)tokenEvent.Param) >> 8)) & 255), (sbyte)(((int)(((uint)tokenEvent.Param) >> 16)) & 255));
            }

            if (_storm != null)
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
            var formatDaysLeftString = DurationText.FormatDaysLeftString(GetEngine(), days: GetRechargeDays() - RechargeDays);
            result = AbilityRules.TranslateResourceString(GetEngine(), s_chargingText) + " (" + formatDaysLeftString + ")";
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
        Storm storm;
        MapCoordinate target = default;
        BattleFieldInfo friendly = default;
        BattleFieldInfo enemy = default;
        StormDamageInfo damage = default;
        int value;
        if (player == Player && (phase == 1 || phase == 2))
        {
            storm = CreateStorm();
            var hexLevel = GetHexLevel();
            var hexY = GetHexY();
            if (storm.GetBestTarget(player, GetHexX(), hexY, hexLevel, 12, ref target, out damage))
            {
                value = damage.EnemyDamage - damage.FriendlyDamage;
                if ((double)value > (double)TargetMana() * 0.75)
                {
                    var playerAIBudget = StrategicMap.Current.Players.GetPlayers(player);
                    var targetMana = TargetMana();
                    if (StrategicMap.Current.Players.GetPlayers(player).AIBudget.RequestBudgetExpense(0, targetMana, playerAIBudget.AIBudget.DefaultBudget, true))
                    {
                        BattleFieldInfo.SetupBattleFieldInfo(out friendly, player, 0, Set<BattleFieldInfoRequestsElement>.Of(), AbilityRules.s_allDamageTypes - Set<DamageType>.Of(DamageType.Wall));
                        friendly.EnemyRelations = Set<byte>.WithBounds(0, 7);
                        var hexLevel2 = GetHexLevel();
                        var hexY2 = GetHexY();
                        var hexX = GetHexX();
                        StrategicMap.Current.RetrieveBattleFieldInfo(hexX, hexY2, hexLevel2, 1, ref friendly);
                        BattleFieldInfo.SetupBattleFieldInfo(out enemy, player, 0, Set<BattleFieldInfoRequestsElement>.Of(), AbilityRules.s_allDamageTypes - Set<DamageType>.Of(DamageType.Wall));
                        var hexLevel3 = GetHexLevel();
                        var hexY3 = GetHexY();
                        var hexX2 = GetHexX();
                        StrategicMap.Current.RetrieveBattleFieldInfo(hexX2, hexY3, hexLevel3, 8, ref enemy);
                        if (friendly.FriendlyStrength < enemy.Strength || value > 100)
                        {
                            if (StormTargetSelected(target.X, target.Y, target.MapLevel))
                            {
                                var playerLocal = StrategicMap.Current.Players.GetPlayers(player);
                                var targetMana2 = TargetMana();
                                StrategicMap.Current.Players.GetPlayers(player).AIBudget.BudgetExpenseMade(0, targetMana2, playerLocal.AIBudget.DefaultBudget, AIBudgetManager.ExpenseMode.PoolThenBudget);
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
    public override int GetDefaultAITargetPriority() => RechargeDays * 10 + 10;

    // Altar.pas:62
    public virtual Storm CreateStorm() => null;

    // Altar.pas:110
    public virtual bool StormTargetSelected(sbyte X, sbyte Y, sbyte L)
    {
        bool result;
        string reason = "";
        StructureTokenEvent eventValue;
        if (CanTarget(ref reason) && !StrategicMap.Current.Players.GetPlayers(Player).GetBusy())
        {
            eventValue = CreateTokenEvent();
            SetupTokenEvent(eventValue);
            eventValue.Param = X + (Y << 8) + (L << 16);
            eventValue.Action = 0;
            StrategicMap.Current.TokenManager.GetTokenControl(Player).AddEvent(eventValue);
            eventValue.Release();
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
        bool result = default;
        TargetSelectionControl selection = default;
        string reason = "";
        selection = StrategicMap.Current.TargetSelection;
        if (!GetRazed() && Player >= 0 && TargetMana() <= StrategicMap.Current.Players.GetPlayers(Player).MagicControl.Mana && CanTarget(ref reason) && _storm == null)
        {
            selection.Initialize(this);
            selection.TargetMode = selection.TargetMode + Set<TargetSelectionModeElement>.Of(TargetSelectionModeElement.ValidateDistance, TargetSelectionModeElement.ExternalFloater);
            selection.SourceX = GetHexX();
            selection.SourceY = GetHexY();
            selection.SourceL = GetHexLevel();
            selection.Range = 12;
            selection.Changed += OnTargetSelectionChange;
            _storm = CreateStorm();
            _storm.UpdateTargetSelection(StrategicMap.Current.TargetSelection);
            var name = _storm.Name();
            var format = RtlUnit.Format(AbilityRules.TranslateResourceString(GetEngine(), GameText.SelectTargetLocationForOnMapFormat), [name]);
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
                if (StormTargetSelected(selection.TargetX, selection.TargetY, selection.TargetL))
                {
                    selection.Deactivate();
                }

                _storm?.Dispose();
                _storm = null;
                Update();
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
