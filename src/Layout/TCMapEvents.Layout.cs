// Layout translated from recovered Delphi forms.
#nullable disable
using Aow.Layout;
using Aow.Rtl;
using Aow.Units.AoWTC;
using Aow.Units.CMapCtrl;
using Aow.Units.HSEngine;

namespace Aow.Units.aowTCMapEvents;

public partial class TTCMapEvents : ILayoutModule
{
    public void InitializeComponent()
    {
        InitializeComponent(LayoutSession.Default);
    }

    public void InitializeComponent(LayoutSession session)
    {
        session.Begin(this);
        Name = "TCMapEvents";
        TCMap = new TAoWCombatMapControl(this);
        session.BeginRead(TCMap);
        TCMap.Name = "TCMap";
        TCMap.OnSelected = TCMapSelected;
        TCMap.OnDeselected = TCMapDeselected;
        TCMap.OnMouseOverTarget = TCMapMouseOverTarget;
        TCMap.OnUnitChanged = Layout_TCMapUnitChanged_0;
        TCMap.OnShowMessage = TCMapShowMessage;
        TCMap.OnTurnChanged = Layout_TCMapTurnChanged_1;
        TCMap.OnSeatedPlayerChanged = TCMapSeatedPlayerChanged;
        TCMap.OnMapChanged = TCMapMapChanged;
        TCMap.OnActivateMap = Layout_TCMapActivateMap_2;
        TCMap.OnDeActivateMap = Layout_TCMapDeActivateMap_3;
        TCMap.OnCastSpell = TCMapCastSpell;
        TCMap.OnActivateAbility = TCMapActivateAbility;
        session.EndRead(TCMap);
        session.EndRead(this);
        Loaded();
        TCMap.Loaded();
        session.Resolve(this);
    }

    private void Layout_TCMapUnitChanged_0(THexagonSprite p0)
    {
        TCMapUnitChanged((TTacticalCombatUnitHS)p0);
    }

    private void Layout_TCMapTurnChanged_1(int p0)
    {
        TCMapTurnChanged();
    }

    private void Layout_TCMapActivateMap_2(TObject p0)
    {
        TCMapActivateMap();
    }

    private void Layout_TCMapDeActivateMap_3(TObject p0)
    {
        TCMapDeActivateMap();
    }
}
