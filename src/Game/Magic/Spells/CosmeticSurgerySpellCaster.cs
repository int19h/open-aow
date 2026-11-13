// Translated from UnitSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Commands;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

namespace Aow.Game.Magic.Spells;

// replacement face index, target unit ID
public partial class CosmeticSurgerySpellCaster : SpellCaster
{
    public CosmeticSurgerySpellCaster(ECustomNode owner) : base(owner)
    { }

    public int FaceIndex { get; set; }
    public int TargetUnitId { get; set; }

    // UnitSpells.pas:870
    public override bool CanCast(ref string reason) => base.CanCast(ref reason);

    // UnitSpells.pas:875
    public override bool Cast()
    {
        bool result;
        Troop source;
        Troop target;
        UnitSpellTokenEvent eventValue;
        string reason = "";
        ((UnitSpell)GamePack.Current.SpellControl.GetSpell(59)).ResetTargetSelection();
        source = StrategicMap.Current.UnitControl.FindUnit(CasterSourceId);
        target = StrategicMap.Current.UnitControl.FindUnit(TargetUnitId);
        if (source != null && target != null && CanCast(ref reason) && !StrategicMap.Current.Players.GetPlayers(source.Player).GetBusy())
        {
            eventValue = new UnitSpellTokenEvent(null) { Holder = CreationHolder() };
            var spell = GamePack.Current.SpellControl.GetSpell(59);
            eventValue.Initialize(CasterSpellId, source, null, spell.BaseManaCost);
            eventValue.TargetUnitIds.Add(TargetUnitId);
            eventValue.SpellData[0] = FaceIndex;
            eventValue.TargetPlayer = (byte)target.Player;
            StrategicMap.Current.TokenManager.GetTokenControl(source.Player).AddEvent(eventValue);
            eventValue.Release();
            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

}
