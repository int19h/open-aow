// Layout translated from recovered Delphi forms.
#nullable disable
using Aow.Game.Layout;
using Aow.Game.UI.Grids;

namespace Aow.Game.UI.Dialogs;

public sealed partial class MapEvents : ILayoutModule
{
    public void InitializeComponent() => InitializeComponent(LayoutSession.Default);

    public void InitializeComponent(LayoutSession session)
    {
        session.Begin(this);
        Name = "MapEvents";
        MapControl theMap = new(this);
        session.BeginRead(theMap);
        theMap.Name = "TheMap";
        theMap.ArmySelected += TheMapArmySelected;
        theMap.ArmyDeselected += _ => TheMapArmyDeselected();
        theMap.ArmyChanged += TheMapArmyChanged;
        theMap.ArmyDestroyed += TheMapArmyDestroyed;
        theMap.StructureSelected += TheMapStructureSelected;
        theMap.StructureDeselected += _ => TheMapStructureDeselected();
        theMap.StructureDoubleClick += TheMapStructureDoubleClick;
        theMap.StructureChanged += TheMapStructureChanged;
        theMap.StructureDestroyed += TheMapStructureDestroyed;
        theMap.UnitChanged += TheMapUnitChanged;
        theMap.DeactivateHsm += _ => TheMapDeactivateHexSpriteMap();
        theMap.GemsChanged += TheMapGemsChanged;
        theMap.IncomeChanged += TheMapIncomeChanged;
        theMap.UpkeepChanged += TheMapIncomeChanged;
        theMap.ShowMessage += TheMapShowMessage;
        theMap.DayChanged += TheMapDayChanged;
        theMap.EndTurn += TheMapEndTurn;
        theMap.HexSpriteChanged += TheMapHexSpriteChanged;
        theMap.SeatedPlayerChanged += TheMapSeatedPlayerChanged;
        theMap.MapChanged += TheMapMapChanged;
        theMap.ScannerIconsChanged += TheMapScannerIconsChanged;
        theMap.LogbookChanged += TheMapLogbookChanged;
        theMap.TargetSelectionChange += TheMapTargetSelectionChange;
        theMap.OnCastSpell += TheMapCastSpell;
        theMap.ActivateAbility += TheMapActivateAbility;
        theMap.PlayerMagicChanged += TheMapPlayerMagicChanged;
        theMap.GlobalMagicChanged += _ => TheMapGlobalMagicChanged();
        theMap.RelationsChanged += _ => TheMapRelationsChanged();
        theMap.ItemsSelected += TheMapItemsSelected;
        theMap.ItemsDeselected += _ => TheMapItemsDeselected();
        theMap.GameOver += _ => TheMapGameOver();
        theMap.PlayerTerminated += TheMapPlayerTerminated;
        theMap.ExecuteEventLog += TheMapExecuteEventLog;
        theMap.DiplomaticChange += _ => TheMapDiplomaticChange();
        session.EndRead(theMap);
        session.EndRead(this);
        Loaded();
        theMap.Loaded();
        session.Resolve(this);
    }
}
