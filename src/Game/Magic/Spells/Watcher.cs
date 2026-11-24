#nullable disable warnings
// Translated from GlobalTargetSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;
using Aow.Game.Commands;

using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

public class Watcher : GlobalTargetSpell
{
    // GlobalTargetSpells.pas:1498
    public Watcher(CustomNode? owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), S: s_watcherText);
        SpellId = 57;
        AboveGroundOnly = false;
    }

    // GlobalTargetSpells.pas:1523
    public override void SetupTargetSelection(Troop source, TargetSelectionControl targetSelection)
    {
        base.SetupTargetSelection(source, targetSelection);
        targetSelection.TargetMode = (TargetSelectionModes)(ulong)targetSelection.TargetMode & ~(TargetSelectionModes.FixedLevel | TargetSelectionModes.ValidateDistance);
        targetSelection.HexNumbers.Clear();
    }

    // GlobalTargetSpells.pas:1506
    public override bool TargetSelected(Troop source, int X, int Y, int L)
    {
        if (!StrategicMap.Current.Players.GetPlayers(source.Player).GetBusy())
        {
            WatcherTokenEvent eventValue = new(null) { Holder = CreationHolder() };
            eventValue.Initialize(SpellId, playerIndex: PlayerId.FromOriginal((byte)PlayerIdExtensions.ToOriginal(source.Player)), sourceId: source.UnitId, mana: BaseManaCost);
            eventValue.SetLocation(X.TruncateToSByte(), Y.TruncateToSByte(), layer: L.TruncateToSByte());
            StrategicMap.Current.TokenManager.GetTokenControl(source.Player).AddEvent(eventValue);
            // Original queues through AddEvent
            _ = eventValue.Release();
            return true;
        }
        else
        {
            return false;
        }
    }

    private static readonly string s_watcherText = "Watcher";
}
