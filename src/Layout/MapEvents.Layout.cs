// Layout translated from recovered Delphi forms.
#nullable disable
using Aow.Game.Layout;
using Aow.Game.Ui.Grids;

namespace Aow.Game.Ui.Dialogs;

public partial class MapEvents : ILayoutModule
{
    public void InitializeComponent()
    {
        InitializeComponent(LayoutSession.Default);
    }

    public void InitializeComponent(LayoutSession session)
    {
        session.Begin(this);
        Name = "MapEvents";
        TheMap = new AoWMapControl(this);
        session.BeginRead(TheMap);
        TheMap.Name = "TheMap";
        TheMap.OnArmySelected = TheMapArmySelected;
        TheMap.OnArmyDeselected = _ => TheMapArmyDeselected();
        TheMap.OnArmyChanged = TheMapArmyChanged;
        TheMap.OnArmyDestroyed = TheMapArmyDestroyed;
        TheMap.OnStructureSelected = TheMapStructureSelected;
        TheMap.OnStructureDeselected = _ => TheMapStructureDeselected();
        TheMap.OnStructureDblClick = TheMapStructureDblClick;
        TheMap.OnStructureChanged = TheMapStructureChanged;
        TheMap.OnStructureDestroyed = TheMapStructureDestroyed;
        TheMap.OnUnitChanged = TheMapUnitChanged;
        TheMap.OnDeactivateHSM = _ => Layout_TheMapDeactivateHSM_10();
        TheMap.OnGemsChanged = TheMapGemsChanged;
        TheMap.OnIncomeChanged = TheMapIncomeChanged;
        TheMap.OnUpkeepChanged = TheMapIncomeChanged;
        TheMap.OnShowMessage = TheMapShowMessage;
        TheMap.OnDayChanged = TheMapDayChanged;
        TheMap.OnEndTurn = TheMapEndTurn;
        TheMap.OnHSChanged = TheMapHSChanged;
        TheMap.OnSeatedPlayerChanged = TheMapSeatedPlayerChanged;
        TheMap.OnMapChanged = TheMapMapChanged;
        TheMap.OnScannerIconsChanged = TheMapScannerIconsChanged;
        TheMap.OnLogbookChanged = TheMapLogbookChanged;
        TheMap.OnTargetSelectionChange = TheMapTargetSelectionChange;
        TheMap.OnCastSpell = TheMapCastSpell;
        TheMap.OnActivateAbility = TheMapActivateAbility;
        TheMap.OnPlayerMagicChanged = TheMapPlayerMagicChanged;
        TheMap.OnGlobalMagicChanged = _ => Layout_TheMapGlobalMagicChanged_17();
        TheMap.OnRelationsChanged = _ => Layout_TheMapRelationsChanged_18();
        TheMap.OnItemsSelected = TheMapItemsSelected;
        TheMap.OnItemsDeselected = _ => Layout_TheMapItemsDeselected_20();
        TheMap.OnGameOver = _ => Layout_TheMapGameOver_21();
        TheMap.OnPlayerTerminated = TheMapPlayerTerminated;
        TheMap.OnExecuteEventLog = TheMapExecuteEventLog;
        TheMap.OnDiplomaticChange = _ => Layout_TheMapDiplomaticChange_23();
        session.EndRead(TheMap);
        session.EndRead(this);
        Loaded();
        TheMap.Loaded();
        session.Resolve(this);
    }

    private void Layout_TheMapDeactivateHSM_10()
    {
        TheMapDeactivateHSM();
    }

    private void Layout_TheMapGlobalMagicChanged_17()
    {
        TheMapGlobalMagicChanged();
    }

    private void Layout_TheMapRelationsChanged_18()
    {
        TheMapRelationsChanged();
    }

    private void Layout_TheMapItemsDeselected_20()
    {
        TheMapItemsDeselected();
    }

    private void Layout_TheMapGameOver_21()
    {
        TheMapGameOver();
    }

    private void Layout_TheMapDiplomaticChange_23()
    {
        TheMapDiplomaticChange();
    }
}
