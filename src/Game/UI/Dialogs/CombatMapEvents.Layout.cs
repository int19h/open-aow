// Layout translated from recovered Delphi forms.
#nullable disable
using Aow.Game.Layout;
using Aow.Game.UI.Grids;

namespace Aow.Game.UI.Dialogs;

public partial class CombatMapEvents : ILayoutModule
{
    public void InitializeComponent()
    {
        InitializeComponent(LayoutSession.Default);
    }

    public void InitializeComponent(LayoutSession session)
    {
        session.Begin(this);
        Name = "TCMapEvents";
        CombatMapControl tcMap = new CombatMapControl(this);
        session.BeginRead(tcMap);
        tcMap.Name = "TCMap";
        tcMap.OnSelected = TCMapSelected;
        tcMap.OnDeselected = TCMapDeselected;
        tcMap.OnMouseOverTarget = CombatMapMouseOverTarget;
        tcMap.OnUnitChanged = CombatMapUnitChanged;
        tcMap.OnShowMessage = CombatMapShowMessage;
        tcMap.OnTurnChanged = Layout_TCMapTurnChanged_1;
        tcMap.OnSeatedPlayerChanged = CombatMapSeatedPlayerChanged;
        tcMap.OnMapChanged = CombatMapMapChanged;
        tcMap.OnActivateMap = _ => Layout_TCMapActivateMap_2();
        tcMap.OnDeActivateMap = _ => Layout_TCMapDeActivateMap_3();
        tcMap.OnCastSpell = CombatMapCastSpell;
        tcMap.OnActivateAbility = CombatMapActivateAbility;
        session.EndRead(tcMap);
        session.EndRead(this);
        Loaded();
        tcMap.Loaded();
        session.Resolve(this);
    }

    private void Layout_TCMapTurnChanged_1(int p0)
    {
        CombatMapTurnChanged();
    }

    private void Layout_TCMapActivateMap_2()
    {
        CombatMapActivateMap();
    }

    private void Layout_TCMapDeActivateMap_3()
    {
        CombatMapDeactivateMap();
    }
}
