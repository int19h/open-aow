// Translated from GlobalTargetSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;
using Aow.Game.Commands;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

public partial class Watcher : GlobalTargetSpell
{
    // GlobalTargetSpells.pas:1498
    public Watcher(ECustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        this.SpellName = AbilityRules.TranslateResourceString(GetEngine(), s_watcherText);
        SpellId = 57;
        AboveGroundOnly = false;
    }

    // GlobalTargetSpells.pas:1523
    public override void SetupTargetSelection(Troop source, TargetSelectionControl targetSelection)
    {
        base.SetupTargetSelection(source, targetSelection);
        targetSelection.TargetMode = Set<TargetSelectionModeElement>.FromMask(targetSelection.TargetMode.Mask) - Set<TargetSelectionModeElement>.Of(TargetSelectionModeElement.FixedLevel, TargetSelectionModeElement.ValidateDistance);
        targetSelection.HexNumbers.Clear();
    }

    // GlobalTargetSpells.pas:1506
    public override bool TargetSelected(Troop source, int X, int Y, int L)
    {
        bool result;
        WatcherTokenEvent eventValue;
        if (!StrategicMap.Current.Players.GetPlayers(source.Player).GetBusy())
        {
            eventValue = new WatcherTokenEvent(null) { Holder = CreationHolder() };
            eventValue.Initialize(SpellId, (byte)source.Player, source.UnitId, BaseManaCost);
            eventValue.SetLocation((sbyte)X, (sbyte)Y, (sbyte)L);
            StrategicMap.Current.TokenManager.GetTokenControl(source.Player).AddEvent(eventValue);
            // Original queues through AddEvent
            eventValue.Release();
            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    private static readonly string s_watcherText = "Watcher";
}
