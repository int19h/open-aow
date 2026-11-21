// Layout translated from recovered Delphi forms.
#nullable disable
using Aow.Game.Layout;
using Aow.Game.UI.Grids;

namespace Aow.Game.UI.Dialogs;

public sealed partial class CombatMapEvents : ILayoutModule
{
    public void InitializeComponent() => InitializeComponent(LayoutSession.Default);

    public void InitializeComponent(LayoutSession session)
    {
        session.Begin(this);
        Name = "TCMapEvents";
        CombatMapControl tcMap = new(this);
        session.BeginRead(tcMap);
        tcMap.Name = "TCMap";
        tcMap.Selected += TCMapSelected;
        tcMap.Deselected += TCMapDeselected;
        tcMap.MouseOverTarget += CombatMapMouseOverTarget;
        tcMap.UnitChanged += CombatMapUnitChanged;
        tcMap.ShowMessage += CombatMapShowMessage;
        tcMap.TurnChanged += _ => CombatMapTurnChanged();
        tcMap.SeatedPlayerChanged += CombatMapSeatedPlayerChanged;
        tcMap.MapChanged += CombatMapMapChanged;
        tcMap.ActivateMap += _ => CombatMapActivateMap();
        tcMap.DeActivateMap += _ => CombatMapDeactivateMap();
        tcMap.OnCastSpell += CombatMapCastSpell;
        tcMap.ActivateAbility += CombatMapActivateAbility;
        session.EndRead(tcMap);
        session.EndRead(this);
        Loaded();
        tcMap.Loaded();
        session.Resolve(this);
    }
}
