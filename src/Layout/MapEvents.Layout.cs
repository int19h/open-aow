// Layout translated from recovered Delphi forms.
#nullable disable
using System;
using Aow.Rtl;
using Aow.Layout;

namespace Aow.Units.aowMapEvents
{
    public partial class TMapEvents
    {
        public void InitializeComponent()
        {
            InitializeComponent(LayoutSession.Default);
        }

        public void InitializeComponent(LayoutSession session)
        {
            session.Begin(this);
            Name = "MapEvents";
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
            TheMap = new Aow.Units.MapCtrl.TAoWMapControl(this)
            {
                Name = "TheMap",
                OnArmySelected = Layout_TheMapArmySelected_0,
                OnArmyDeselected = Layout_TheMapArmyDeselected_1,
                OnArmyChanged = Layout_TheMapArmyChanged_2,
                OnArmyDestroyed = Layout_TheMapArmyDestroyed_3,
                OnStructureSelected = Layout_TheMapStructureSelected_4,
                OnStructureDeselected = Layout_TheMapStructureDeselected_5,
                OnStructureDblClick = Layout_TheMapStructureDblClick_6,
                OnStructureChanged = Layout_TheMapStructureChanged_7,
                OnStructureDestroyed = Layout_TheMapStructureDestroyed_8,
                OnUnitChanged = Layout_TheMapUnitChanged_9,
                OnDeactivateHSM = Layout_TheMapDeactivateHSM_10,
                OnGemsChanged = TheMapGemsChanged,
                OnIncomeChanged = TheMapIncomeChanged,
                OnUpkeepChanged = TheMapIncomeChanged,
                OnShowMessage = Layout_TheMapShowMessage_11,
                OnDayChanged = TheMapDayChanged,
                OnEndTurn = TheMapEndTurn,
                OnHSChanged = Layout_TheMapHSChanged_12,
                OnSeatedPlayerChanged = TheMapSeatedPlayerChanged,
                OnMapChanged = Layout_TheMapMapChanged_13,
                OnScannerIconsChanged = Layout_TheMapScannerIconsChanged_14,
                OnLogbookChanged = TheMapLogbookChanged,
                OnTargetSelectionChange = TheMapTargetSelectionChange,
                OnCastSpell = Layout_TheMapCastSpell_15,
                OnActivateAbility = Layout_TheMapActivateAbility_16,
                OnPlayerMagicChanged = TheMapPlayerMagicChanged,
                OnGlobalMagicChanged = Layout_TheMapGlobalMagicChanged_17,
                OnRelationsChanged = Layout_TheMapRelationsChanged_18,
                OnItemsSelected = Layout_TheMapItemsSelected_19,
                OnItemsDeselected = Layout_TheMapItemsDeselected_20,
                OnGameOver = Layout_TheMapGameOver_21,
                OnPlayerTerminated = Layout_TheMapPlayerTerminated_22,
                OnExecuteEventLog = TheMapExecuteEventLog,
                OnDiplomaticChange = Layout_TheMapDiplomaticChange_23
            };
            LayoutSession.EndRead(TheMap);
            ComponentState = ComponentState.Without(TComponentState.csReading);
            Loaded();
            TheMap.Loaded();
            session.Resolve(this);
        }

        private void Layout_TheMapArmySelected_0(Aow.Rtl.TObject p0)
        {
            TheMapArmySelected((Aow.Units.AoWE.TArmy)p0);
        }

        private void Layout_TheMapArmyDeselected_1(Aow.Rtl.TObject p0)
        {
            TheMapArmyDeselected((Aow.Units.AoWE.TArmy)p0);
        }

        private void Layout_TheMapArmyChanged_2(Aow.Rtl.TObject p0)
        {
            TheMapArmyChanged((Aow.Units.AoWE.TArmy)p0);
        }

        private void Layout_TheMapArmyDestroyed_3(Aow.Rtl.TObject p0)
        {
            TheMapArmyDestroyed((Aow.Units.AoWE.TArmy)p0);
        }

        private void Layout_TheMapStructureSelected_4(Aow.Rtl.TObject p0)
        {
            TheMapStructureSelected((Aow.Units.AoWE.TStructure)p0);
        }

        private void Layout_TheMapStructureDeselected_5(Aow.Rtl.TObject p0)
        {
            TheMapStructureDeselected((Aow.Units.AoWE.TStructure)p0);
        }

        private void Layout_TheMapStructureDblClick_6(Aow.Rtl.TObject p0)
        {
            TheMapStructureDblClick((Aow.Units.AoWE.TStructure)p0);
        }

        private void Layout_TheMapStructureChanged_7(Aow.Rtl.TObject p0)
        {
            TheMapStructureChanged((Aow.Units.AoWE.TStructure)p0);
        }

        private void Layout_TheMapStructureDestroyed_8(Aow.Rtl.TObject p0)
        {
            TheMapStructureDestroyed((Aow.Units.AoWE.TStructure)p0);
        }

        private void Layout_TheMapUnitChanged_9(Aow.Rtl.TObject p0)
        {
            TheMapUnitChanged((Aow.Units.AoWE.TAbstractUnit)p0);
        }

        private void Layout_TheMapDeactivateHSM_10(TObject p0)
        {
            TheMapDeactivateHSM();
        }

        private void Layout_TheMapShowMessage_11(TObject p0)
        {
            TheMapShowMessage(ref Aow.Rtl.Ptr<Aow.Units.AoWE.TShowMessageInfo>.Unbox(p0).Value);
        }

        private void Layout_TheMapHSChanged_12(TObject p0)
        {
            TheMapHSChanged((Aow.Units.HSEngine.THexagonSprite)p0);
        }

        private void Layout_TheMapMapChanged_13(TObject p0)
        {
            TheMapMapChanged(ref Aow.Rtl.Ptr<Aow.Units.AoWE.TMapChangedInfo>.Unbox(p0).Value);
        }

        private void Layout_TheMapScannerIconsChanged_14(TObject p0)
        {
            TheMapScannerIconsChanged(ref Aow.Rtl.Ptr<Aow.Units.AoWE.TMapChangedInfo>.Unbox(p0).Value);
        }

        private void Layout_TheMapCastSpell_15(TObject p0)
        {
            TheMapCastSpell((Aow.Units.AoWE.TSpellCaster)p0);
        }

        private void Layout_TheMapActivateAbility_16(TObject p0)
        {
            TheMapActivateAbility((Aow.Units.AoWE.TActivateAbilityInfo)p0);
        }

        private void Layout_TheMapGlobalMagicChanged_17(TObject p0)
        {
            TheMapGlobalMagicChanged();
        }

        private void Layout_TheMapRelationsChanged_18(TObject p0)
        {
            TheMapRelationsChanged();
        }

        private void Layout_TheMapItemsSelected_19(Aow.Rtl.TObject p0)
        {
            TheMapItemsSelected((Aow.Units.AoWE.TItemHS)p0);
        }

        private void Layout_TheMapItemsDeselected_20(Aow.Rtl.TObject p0)
        {
            TheMapItemsDeselected();
        }

        private void Layout_TheMapGameOver_21(TObject p0)
        {
            TheMapGameOver();
        }

        private void Layout_TheMapPlayerTerminated_22(TObject p0)
        {
            TheMapPlayerTerminated((sbyte)Aow.Rtl.Rtl.ObjectToInt(p0));
        }

        private void Layout_TheMapDiplomaticChange_23(TObject p0)
        {
            TheMapDiplomaticChange();
        }
    }
}
