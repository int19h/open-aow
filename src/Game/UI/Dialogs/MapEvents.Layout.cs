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
        theMap.OnArmySelected += TheMapArmySelected;
        theMap.OnArmyDeselected += _ => TheMapArmyDeselected();
        theMap.OnArmyChanged += TheMapArmyChanged;
        theMap.OnArmyDestroyed += TheMapArmyDestroyed;
        theMap.OnStructureSelected += TheMapStructureSelected;
        theMap.OnStructureDeselected += _ => TheMapStructureDeselected();
        theMap.OnStructureDoubleClick += TheMapStructureDoubleClick;
        theMap.OnStructureChanged += TheMapStructureChanged;
        theMap.OnStructureDestroyed += TheMapStructureDestroyed;
        theMap.OnUnitChanged += TheMapUnitChanged;
        theMap.OnDeactivateHsm += _ => TheMapDeactivateHexSpriteMap();
        theMap.OnGemsChanged += TheMapGemsChanged;
        theMap.OnIncomeChanged += TheMapIncomeChanged;
        theMap.OnUpkeepChanged += TheMapIncomeChanged;
        theMap.OnShowMessage += TheMapShowMessage;
        theMap.OnDayChanged += TheMapDayChanged;
        theMap.OnEndTurn += TheMapEndTurn;
        theMap.OnHexSpriteChanged += TheMapHexSpriteChanged;
        theMap.OnSeatedPlayerChanged += TheMapSeatedPlayerChanged;
        theMap.OnMapChanged += TheMapMapChanged;
        theMap.OnScannerIconsChanged += TheMapScannerIconsChanged;
        theMap.OnLogbookChanged += TheMapLogbookChanged;
        theMap.OnTargetSelectionChange += TheMapTargetSelectionChange;
        theMap.OnCastSpell += TheMapCastSpell;
        theMap.OnActivateAbility += TheMapActivateAbility;
        theMap.OnPlayerMagicChanged += TheMapPlayerMagicChanged;
        theMap.OnGlobalMagicChanged += _ => TheMapGlobalMagicChanged();
        theMap.OnRelationsChanged += _ => TheMapRelationsChanged();
        theMap.OnItemsSelected += TheMapItemsSelected;
        theMap.OnItemsDeselected += _ => TheMapItemsDeselected();
        theMap.OnGameOver += _ => TheMapGameOver();
        theMap.OnPlayerTerminated += TheMapPlayerTerminated;
        theMap.OnExecuteEventLog += TheMapExecuteEventLog;
        theMap.OnDiplomaticChange += _ => TheMapDiplomaticChange();
        session.EndRead(theMap);
        session.EndRead(this);
        Loaded();
        theMap.Loaded();
        session.Resolve(this);
    }
}
