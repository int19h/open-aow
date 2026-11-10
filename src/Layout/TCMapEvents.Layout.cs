// Layout translated from recovered Delphi forms.
#nullable disable
using Aow.Game.Layout;
using Aow.Game.Ui.Grids;

namespace Aow.Game.Ui.Dialogs;

public partial class TCMapEvents : ILayoutModule
{
    public void InitializeComponent()
    {
        InitializeComponent(LayoutSession.Default);
    }

    public void InitializeComponent(LayoutSession session)
    {
        session.Begin(this);
        Name = "TCMapEvents";
        TCMap = new AoWCombatMapControl(this);
        session.BeginRead(TCMap);
        TCMap.Name = "TCMap";
        TCMap.OnSelected = TCMapSelected;
        TCMap.OnDeselected = TCMapDeselected;
        TCMap.OnMouseOverTarget = TCMapMouseOverTarget;
        TCMap.OnUnitChanged = TCMapUnitChanged;
        TCMap.OnShowMessage = TCMapShowMessage;
        TCMap.OnTurnChanged = Layout_TCMapTurnChanged_1;
        TCMap.OnSeatedPlayerChanged = TCMapSeatedPlayerChanged;
        TCMap.OnMapChanged = TCMapMapChanged;
        TCMap.OnActivateMap = _ => Layout_TCMapActivateMap_2();
        TCMap.OnDeActivateMap = _ => Layout_TCMapDeActivateMap_3();
        TCMap.OnCastSpell = TCMapCastSpell;
        TCMap.OnActivateAbility = TCMapActivateAbility;
        session.EndRead(TCMap);
        session.EndRead(this);
        Loaded();
        TCMap.Loaded();
        session.Resolve(this);
    }

    private void Layout_TCMapTurnChanged_1(int p0)
    {
        TCMapTurnChanged();
    }

    private void Layout_TCMapActivateMap_2()
    {
        TCMapActivateMap();
    }

    private void Layout_TCMapDeActivateMap_3()
    {
        TCMapDeActivateMap();
    }
}
