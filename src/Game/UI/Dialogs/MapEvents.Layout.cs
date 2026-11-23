// Layout translated from recovered Delphi forms.
#nullable disable warnings
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
        theMap.StructureDoubleClicked += TheMapStructureDoubleClick;
        theMap.StructureChanged += TheMapStructureChanged;
        theMap.StructureDestroyed += TheMapStructureDestroyed;
        theMap.UnitChanged += TheMapUnitChanged;
        theMap.HexSpriteMapClosing += _ => TheMapDeactivateHexSpriteMap();
        theMap.GemsChanged += TheMapGemsChanged;
        theMap.IncomeChanged += TheMapIncomeChanged;
        theMap.UpkeepChanged += TheMapIncomeChanged;
        theMap.ShowingMessage += TheMapShowMessage;
        theMap.DayChanged += TheMapDayChanged;
        theMap.TurnEnded += TheMapEndTurn;
        theMap.HexSpriteChanged += TheMapHexSpriteChanged;
        theMap.SeatedPlayerChanged += TheMapSeatedPlayerChanged;
        theMap.MapChanged += TheMapMapChanged;
        theMap.ScannerIconsChanged += TheMapScannerIconsChanged;
        theMap.LogbookChanged += TheMapLogbookChanged;
        theMap.TargetSelectionChanged += TheMapTargetSelectionChange;
        theMap.AbilityActivated += TheMapActivateAbility;
        theMap.PlayerMagicChanged += TheMapPlayerMagicChanged;
        theMap.GlobalMagicChanged += _ => TheMapGlobalMagicChanged();
        theMap.PlayerChanged += _ => TheMapRelationsChanged();
        theMap.ItemsSelected += TheMapItemsSelected;
        theMap.ItemsDeselected += _ => TheMapItemsDeselected();
        theMap.GameTerminated += _ => TheMapGameOver();
        theMap.PlayerTerminated += TheMapPlayerTerminated;
        theMap.ExecutingEventLog += TheMapExecuteEventLog;
        theMap.DiplomaticRelationsChanged += _ => TheMapDiplomaticChange();
        session.EndRead(theMap);
        session.EndRead(this);
        Loaded();
        theMap.Loaded();
        session.Resolve(this);
    }
}
