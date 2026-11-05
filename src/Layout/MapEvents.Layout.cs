// Layout translated from recovered Delphi forms.
#nullable disable
using Aow.Rtl;
using Aow.Layout;
using Aow.Units.MapCtrl;
using Aow.Units.AoWE;
using Aow.Units.HSEngine;

namespace Aow.Units.aowMapEvents
{
    public partial class TMapEvents : ILayoutModule
    {
        public void InitializeComponent()
        {
            InitializeComponent(LayoutSession.Default);
        }

        public void InitializeComponent(LayoutSession session)
        {
            session.Begin(this);
            Name = "MapEvents";
            TheMap = new TAoWMapControl(this);
            session.BeginRead(TheMap);
            TheMap.Name = "TheMap";
            TheMap.OnArmySelected = Layout_TheMapArmySelected_0;
            TheMap.OnArmyDeselected = Layout_TheMapArmyDeselected_1;
            TheMap.OnArmyChanged = Layout_TheMapArmyChanged_2;
            TheMap.OnArmyDestroyed = Layout_TheMapArmyDestroyed_3;
            TheMap.OnStructureSelected = Layout_TheMapStructureSelected_4;
            TheMap.OnStructureDeselected = Layout_TheMapStructureDeselected_5;
            TheMap.OnStructureDblClick = Layout_TheMapStructureDblClick_6;
            TheMap.OnStructureChanged = Layout_TheMapStructureChanged_7;
            TheMap.OnStructureDestroyed = Layout_TheMapStructureDestroyed_8;
            TheMap.OnUnitChanged = Layout_TheMapUnitChanged_9;
            TheMap.OnDeactivateHSM = Layout_TheMapDeactivateHSM_10;
            TheMap.OnGemsChanged = TheMapGemsChanged;
            TheMap.OnIncomeChanged = TheMapIncomeChanged;
            TheMap.OnUpkeepChanged = TheMapIncomeChanged;
            TheMap.OnShowMessage = Layout_TheMapShowMessage_11;
            TheMap.OnDayChanged = TheMapDayChanged;
            TheMap.OnEndTurn = TheMapEndTurn;
            TheMap.OnHSChanged = Layout_TheMapHSChanged_12;
            TheMap.OnSeatedPlayerChanged = TheMapSeatedPlayerChanged;
            TheMap.OnMapChanged = Layout_TheMapMapChanged_13;
            TheMap.OnScannerIconsChanged = Layout_TheMapScannerIconsChanged_14;
            TheMap.OnLogbookChanged = TheMapLogbookChanged;
            TheMap.OnTargetSelectionChange = TheMapTargetSelectionChange;
            TheMap.OnCastSpell = Layout_TheMapCastSpell_15;
            TheMap.OnActivateAbility = Layout_TheMapActivateAbility_16;
            TheMap.OnPlayerMagicChanged = TheMapPlayerMagicChanged;
            TheMap.OnGlobalMagicChanged = Layout_TheMapGlobalMagicChanged_17;
            TheMap.OnRelationsChanged = Layout_TheMapRelationsChanged_18;
            TheMap.OnItemsSelected = Layout_TheMapItemsSelected_19;
            TheMap.OnItemsDeselected = Layout_TheMapItemsDeselected_20;
            TheMap.OnGameOver = Layout_TheMapGameOver_21;
            TheMap.OnPlayerTerminated = Layout_TheMapPlayerTerminated_22;
            TheMap.OnExecuteEventLog = TheMapExecuteEventLog;
            TheMap.OnDiplomaticChange = Layout_TheMapDiplomaticChange_23;
            session.EndRead(TheMap);
            session.EndRead(this);
            Loaded();
            TheMap.Loaded();
            session.Resolve(this);
        }

        private void Layout_TheMapArmySelected_0(TObject p0)
        {
            TheMapArmySelected((TArmy)p0);
        }

        private void Layout_TheMapArmyDeselected_1(TObject p0)
        {
            TheMapArmyDeselected((TArmy)p0);
        }

        private void Layout_TheMapArmyChanged_2(TObject p0)
        {
            TheMapArmyChanged((TArmy)p0);
        }

        private void Layout_TheMapArmyDestroyed_3(TObject p0)
        {
            TheMapArmyDestroyed((TArmy)p0);
        }

        private void Layout_TheMapStructureSelected_4(TObject p0)
        {
            TheMapStructureSelected((TStructure)p0);
        }

        private void Layout_TheMapStructureDeselected_5(TObject p0)
        {
            TheMapStructureDeselected((TStructure)p0);
        }

        private void Layout_TheMapStructureDblClick_6(TObject p0)
        {
            TheMapStructureDblClick((TStructure)p0);
        }

        private void Layout_TheMapStructureChanged_7(TObject p0)
        {
            TheMapStructureChanged((TStructure)p0);
        }

        private void Layout_TheMapStructureDestroyed_8(TObject p0)
        {
            TheMapStructureDestroyed((TStructure)p0);
        }

        private void Layout_TheMapUnitChanged_9(TObject p0)
        {
            TheMapUnitChanged((TAbstractUnit)p0);
        }

        private void Layout_TheMapDeactivateHSM_10(TObject p0)
        {
            TheMapDeactivateHSM();
        }

        private void Layout_TheMapShowMessage_11(TObject p0)
        {
            TheMapShowMessage(ref ((ShowMessageNotification)p0).Info);
        }

        private void Layout_TheMapHSChanged_12(TObject p0)
        {
            TheMapHSChanged((THexagonSprite)p0);
        }

        private void Layout_TheMapMapChanged_13(TObject p0)
        {
            TheMapMapChanged(ref ((MapChangedNotification)p0).Info);
        }

        private void Layout_TheMapScannerIconsChanged_14(TObject p0)
        {
            TheMapScannerIconsChanged(ref ((MapChangedNotification)p0).Info);
        }

        private void Layout_TheMapCastSpell_15(TObject p0)
        {
            TheMapCastSpell((TSpellCaster)p0);
        }

        private void Layout_TheMapActivateAbility_16(TObject p0)
        {
            TheMapActivateAbility((TActivateAbilityInfo)p0);
        }

        private void Layout_TheMapGlobalMagicChanged_17(TObject p0)
        {
            TheMapGlobalMagicChanged();
        }

        private void Layout_TheMapRelationsChanged_18(TObject p0)
        {
            TheMapRelationsChanged();
        }

        private void Layout_TheMapItemsSelected_19(TObject p0)
        {
            TheMapItemsSelected((TItemHS)p0);
        }

        private void Layout_TheMapItemsDeselected_20(TObject p0)
        {
            TheMapItemsDeselected();
        }

        private void Layout_TheMapGameOver_21(TObject p0)
        {
            TheMapGameOver();
        }

        private void Layout_TheMapPlayerTerminated_22(TObject p0)
        {
            TheMapPlayerTerminated((sbyte)Rtl.Rtl.ObjectToInt(p0));
        }

        private void Layout_TheMapDiplomaticChange_23(TObject p0)
        {
            TheMapDiplomaticChange();
        }
    }
}
