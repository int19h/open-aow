// Layout translated from recovered Delphi forms.
#nullable disable
using System;
using Aow.Rtl;
using Aow.Layout;

namespace Aow.Units.aowTCMapEvents
{
    public partial class TTCMapEvents
    {
        public void InitializeComponent()
        {
            InitializeComponent(LayoutSession.Default);
        }

        public void InitializeComponent(LayoutSession session)
        {
            session.Begin(this);
            Name = "TCMapEvents";
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
            TCMap = new Aow.Units.CMapCtrl.TAoWCombatMapControl(this)
            {
                Name = "TCMap",
                OnSelected = TCMapSelected,
                OnDeselected = TCMapDeselected,
                OnMouseOverTarget = TCMapMouseOverTarget,
                OnUnitChanged = Layout_TCMapUnitChanged_0,
                OnShowMessage = TCMapShowMessage,
                OnTurnChanged = Layout_TCMapTurnChanged_1,
                OnSeatedPlayerChanged = TCMapSeatedPlayerChanged,
                OnMapChanged = TCMapMapChanged,
                OnActivateMap = Layout_TCMapActivateMap_2,
                OnDeActivateMap = Layout_TCMapDeActivateMap_3,
                OnCastSpell = TCMapCastSpell,
                OnActivateAbility = TCMapActivateAbility
            };
            LayoutSession.EndRead(TCMap);
            ComponentState = ComponentState.Without(TComponentState.csReading);
            Loaded();
            TCMap.Loaded();
            session.Resolve(this);
        }

        private void Layout_TCMapUnitChanged_0(Aow.Units.HSEngine.THexagonSprite p0)
        {
            TCMapUnitChanged((Aow.Units.AoWTC.TTacticalCombatUnitHS)p0);
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
}
