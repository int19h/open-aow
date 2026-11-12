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
        tcMap.OnSelected += TCMapSelected;
        tcMap.OnDeselected += TCMapDeselected;
        tcMap.OnMouseOverTarget += CombatMapMouseOverTarget;
        tcMap.OnUnitChanged += CombatMapUnitChanged;
        tcMap.OnShowMessage += CombatMapShowMessage;
        tcMap.OnTurnChanged += _ => CombatMapTurnChanged();
        tcMap.OnSeatedPlayerChanged += CombatMapSeatedPlayerChanged;
        tcMap.OnMapChanged += CombatMapMapChanged;
        tcMap.OnActivateMap += _ => CombatMapActivateMap();
        tcMap.OnDeActivateMap += _ => CombatMapDeactivateMap();
        tcMap.OnCastSpell += CombatMapCastSpell;
        tcMap.OnActivateAbility += CombatMapActivateAbility;
        session.EndRead(tcMap);
        session.EndRead(this);
        Loaded();
        tcMap.Loaded();
        session.Resolve(this);
    }
}
