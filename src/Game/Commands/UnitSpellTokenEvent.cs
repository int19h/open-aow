// Translated from AoWE.pas.
using System.Runtime.InteropServices;

using Aow.Engine;
using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Formats.Tree;

using Aow.Game.Magic.Abilities;

using Aow.Game.Magic.Spells;

using Aow.Game.Units;

using Aow.Game.World.Map;

namespace Aow.Game.Commands;

// (target IDs, target users, target player, spell data)
public partial class UnitSpellTokenEvent : SpellTokenEvent
{
    public new const int ClassId = 135944;

    public Int32List TargetUnitIds { get; }

    private readonly UnitUserList _targetUnits;

    public byte TargetPlayer { get; set; }

    public int[] SpellData { get; } = new int[2];

    // Spells.PAS:1126
    public override void Dispose()
    {
        TargetUnitIds?.Dispose();
        _targetUnits?.Dispose();
        base.Dispose();
    }

    // Spells.PAS:1136
    // Spells.PAS:1136
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        PersistSupport.Int32List(persister, name: "TargetUnitIds", list: TargetUnitIds);
        PersistSupport.Int32Array(persister, name: "SpellData", values: SpellData);
        TargetPlayer = persister.Byte("TargetPlayer", TargetPlayer);
    }

    // Spells.PAS:1120
    public UnitSpellTokenEvent(ECustomNode owner) : base(owner)
    {
        TargetUnitIds = new Int32List();
        _targetUnits = new UnitUserList(null) { Holder = CreationHolder() };
    }

    // Spells.PAS:1159
    public override bool Validate(Hero source)
    {
        int I;
        Troop target;
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        sbyte targetX = default;
        sbyte targetY = default;
        sbyte targetL = default;
        bool result = base.Validate(source);
        source.GetLocation(out X, out Y, out L);
        I = TargetUnitIds.Count;
        while (result && I != 0)
        {
            I -= 1;
            var item = TargetUnitIds[I];
            target = StrategicMap.Current.UnitControl.FindUnit(item);
            if (target != null)
            {
                target.GetLocation(out targetX, out targetY, out targetL);
                result = target.GetHitPoints() != 0 && (byte)target.Player == TargetPlayer;
                if (result)
                {
                    if (((UnitSpell)GamePack.Current.SpellControl.GetSpell(EventSpellId)).TargetRange != -1)
                    {
                        result = HexGeometry.DeltaHexToRadius(X, Y, targetX, targetY) <= ((UnitSpell)GamePack.Current.SpellControl.GetSpell(EventSpellId)).TargetRange;
                    }
                }

                _targetUnits.AddUnit(target);
            }
            else
            {
                result = false;
            }
        }

        return result;
    }

    // Spells.PAS:1185
    public override bool Process(Troop source, int value)
    {
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        Troop target;
        base.Process(source, value);
        if (_targetUnits.GetCount() != 0)
        {
            _targetUnits.GetUnit(0).GetLocation(out X, out Y, out L);
            GamePack.Current.SpellControl.GetSpell(EventSpellId).PlayDefaultCastEffects(source.Player, X, Y, L);
            for (int I = 0, last = _targetUnits.GetCount() - 1; I <= last; I++)
            {
                target = _targetUnits.GetUnit(I);
                target.AddRef();
                if (((UnitSpell)GamePack.Current.SpellControl.GetSpell(EventSpellId)).ExecuteSpell(source, target, EventMana, SpellData))
                {
                    GamePack.Current.SpellControl.GetSpell(EventSpellId).CastingDone(source, EventMana);
                    target.Changed();
                }

                target.Release();
            }
        }

        return false;
    }

    // Spells.PAS:1143
    public void Initialize(int spellId, Troop source, UnitList targets, int mana)
    {
        EventSpellId = spellId;
        EventPlayerIndex = (byte)source.Player;
        EventSourceId = source.UnitId;
        EventMana = mana;
        if (targets != null && targets.GetCount() != 0)
        {
            TargetPlayer = (byte)targets.GetUnit(0).Player;
            for (int I = 0, last = targets.GetCount() - 1; I <= last; I++)
            {
                var troop = targets.GetUnit(I);
                TargetUnitIds.Add(troop.UnitId);
            }
        }
        else
        {
            TargetPlayer = 255;
        }
    }

    // intf_P4_02.inc:1059

}
