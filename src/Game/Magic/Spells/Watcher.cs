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
    public Watcher(ECustomNode owner) : base(owner)
    {
        this.FSpellName = AbilityRules.TranslateRStr(WatcherRStr);
        FSpellID = 57;
        FAboveGroundOnly = false;
    }

    // GlobalTargetSpells.pas:1523
    public override void SetupTargetSelection(AbstractUnit Source, TargetSelectionControl TargetSelection)
    {
        base.SetupTargetSelection(Source, TargetSelection);
        TargetSelection.FTargetMode = Set<TargetSelectionModeElement>.FromMask(TargetSelection.FTargetMode.Mask) - Set<TargetSelectionModeElement>.Of(TargetSelectionModeElement.FixedLevel, TargetSelectionModeElement.ValidateDistance);
        TargetSelection.FHNs.Clear();
    }

    // GlobalTargetSpells.pas:1506
    public override bool TargetSelected(AbstractUnit Source, int X, int Y, int L)
    {
        bool Result;
        WatcherTE Event;
        if (!AoWHSMap.Current.FPlayers.GetPlayers(Source.FPlayer).GetBusy())
        {
            Event = new WatcherTE(null);
            Event.Initialize(FSpellID, (byte)Source.FPlayer, Source.FUnitID, FBaseManaCost);
            Event.SetLocation((sbyte)X, (sbyte)Y, (sbyte)L);
            AoWHSMap.Current.FTokenManager.GetTokenControl(Source.FPlayer).AddEvent(Event);
            // Original queues through AddEvent
            Event.Release();
            Result = true;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    private static readonly string WatcherRStr = "Watcher";
}
