// Layout translated from recovered Delphi forms.
#nullable disable
using Aow.Game.Layout;
using Aow.Game.UI.Grids;

namespace Aow.Game.UI.Dialogs;

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
        MapControl theMap = new MapControl(this);
        session.BeginRead(theMap);
        theMap.Name = "TheMap";
        theMap.OnArmySelected = TheMapArmySelected;
        theMap.OnArmyDeselected = _ => TheMapArmyDeselected();
        theMap.OnArmyChanged = TheMapArmyChanged;
        theMap.OnArmyDestroyed = TheMapArmyDestroyed;
        theMap.OnStructureSelected = TheMapStructureSelected;
        theMap.OnStructureDeselected = _ => TheMapStructureDeselected();
        theMap.OnStructureDoubleClick = TheMapStructureDblClick;
        theMap.OnStructureChanged = TheMapStructureChanged;
        theMap.OnStructureDestroyed = TheMapStructureDestroyed;
        theMap.OnUnitChanged = TheMapUnitChanged;
        theMap.OnDeactivateHsm = _ => Layout_TheMapDeactivateHSM_10();
        theMap.OnGemsChanged = TheMapGemsChanged;
        theMap.OnIncomeChanged = TheMapIncomeChanged;
        theMap.OnUpkeepChanged = TheMapIncomeChanged;
        theMap.OnShowMessage = TheMapShowMessage;
        theMap.OnDayChanged = TheMapDayChanged;
        theMap.OnEndTurn = TheMapEndTurn;
        theMap.OnHexSpriteChanged = TheMapHSChanged;
        theMap.OnSeatedPlayerChanged = TheMapSeatedPlayerChanged;
        theMap.OnMapChanged = TheMapMapChanged;
        theMap.OnScannerIconsChanged = TheMapScannerIconsChanged;
        theMap.OnLogbookChanged = TheMapLogbookChanged;
        theMap.OnTargetSelectionChange = TheMapTargetSelectionChange;
        theMap.OnCastSpell = TheMapCastSpell;
        theMap.OnActivateAbility = TheMapActivateAbility;
        theMap.OnPlayerMagicChanged = TheMapPlayerMagicChanged;
        theMap.OnGlobalMagicChanged = _ => Layout_TheMapGlobalMagicChanged_17();
        theMap.OnRelationsChanged = _ => Layout_TheMapRelationsChanged_18();
        theMap.OnItemsSelected = TheMapItemsSelected;
        theMap.OnItemsDeselected = _ => Layout_TheMapItemsDeselected_20();
        theMap.OnGameOver = _ => Layout_TheMapGameOver_21();
        theMap.OnPlayerTerminated = TheMapPlayerTerminated;
        theMap.OnExecuteEventLog = TheMapExecuteEventLog;
        theMap.OnDiplomaticChange = _ => Layout_TheMapDiplomaticChange_23();
        session.EndRead(theMap);
        session.EndRead(this);
        Loaded();
        theMap.Loaded();
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
