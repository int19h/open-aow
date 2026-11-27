#nullable disable warnings
// Translated from UnitSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Commands;

using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Game.World.Map;

namespace Aow.Game.Magic.Spells;

// replacement face index, target unit ID
public class CosmagicSurgerySpellCaster(CustomNode? owner) : SpellCaster(owner)
{
    public override SpellCasterView InputView => SpellCasterView.CosmagicSurgery;

    public int FaceIndex { get; set; }
    public int TargetUnitId { get; init; }

    // UnitSpells.pas:870
    public override bool CanCast(ref string reason) => base.CanCast(ref reason);

    // UnitSpells.pas:875
    public override bool Cast()
    {
        string reason = "";
        GetEngine().GamePack.SpellControl.GetSpell<UnitSpell>(59).ResetTargetSelection();
        StrategicMap strategicMap = GetEngine().StrategicMap;
        Troop? source = strategicMap.UnitControl.FindUnit(CasterSourceId);
        Troop? target = strategicMap.UnitControl.FindUnit(TargetUnitId);
        if (source is not null && target is not null && CanCast(ref reason) && !strategicMap.Players.GetPlayers(source.Player).GetBusy())
        {
            UnitSpellTokenEvent eventValue = new(null) { Holder = CreationHolder() };
            Spell? spell = GetEngine().GamePack.SpellControl[59];
            eventValue.Initialize(CasterSpellId, source, targets: null, mana: spell.BaseManaCost);
            _ = eventValue.TargetUnitIds.Add(TargetUnitId);
            eventValue.SpellData[0] = FaceIndex;
            eventValue.TargetPlayer = PlayerId.FromOriginal((byte)PlayerIdExtensions.ToOriginal(target.Player));
            strategicMap.TokenManager.GetTokenControl(source.Player).AddEvent(eventValue);
            _ = eventValue.Release();
            return true;
        }
        else
        {
            return false;
        }
    }
}
