// Translated from AoWE.pas.
using System.Runtime.InteropServices;

using Aow.Engine.Buffers;
using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

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

    private UnitUserList _targetUnits;
    public byte TargetPlayer;
    public Int32Array2 SpellData;

    // Spells.PAS:1126
    public override void Dispose()
    {
        TargetUnitIds?.Dispose();
        _targetUnits?.Dispose();
        base.Dispose();
    }

    // Spells.PAS:1136
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwStreamProc(30, TargetUnitIds.LoadFromStream, TargetUnitIds.SaveToStream);
        stream.rwData(MemoryMarshal.AsBytes((Span<int>)SpellData).Slice(0, 8), 8, 31);
        stream.rwByte(ref TargetPlayer, 32);
    }

    // Spells.PAS:1120
    public UnitSpellTokenEvent(ECustomNode owner) : base(owner)
    {
        TargetUnitIds = new Int32List();
        _targetUnits = new UnitUserList(null);
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
        source.GetLocation(ref X, ref Y, ref L);
        I = TargetUnitIds.Count;
        while (result && I != 0)
        {
            I -= 1;
            var item = TargetUnitIds[I];
            target = AoWHSMap.Current.UnitControl.FindUnit(item);
            if (target != null)
            {
                target.GetLocation(ref targetX, ref targetY, ref targetL);
                result = target.GetHitPoints() != 0 && (byte)target.Player == TargetPlayer;
                if (result)
                {
                    if (((UnitSpell)AoWHSSet.Current.SpellControl.GetSpell(EventSpellId)).TargetRange != -1)
                    {
                        result = HexGeometry.DeltaHexToRadius(X, Y, targetX, targetY) <= ((UnitSpell)AoWHSSet.Current.SpellControl.GetSpell(EventSpellId)).TargetRange;
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
            _targetUnits.GetUnit(0).GetLocation(ref X, ref Y, ref L);
            AoWHSSet.Current.SpellControl.GetSpell(EventSpellId).PlayDefaultCastEffects(source.Player, X, Y, L);
            for (int I = 0, last = _targetUnits.GetCount() - 1; I <= last; I++)
            {
                target = _targetUnits.GetUnit(I);
                target.AddRef();
                if (((UnitSpell)AoWHSSet.Current.SpellControl.GetSpell(EventSpellId)).ExecuteSpell(source, target, EventMana, ref SpellData))
                {
                    AoWHSSet.Current.SpellControl.GetSpell(EventSpellId).CastingDone(source, EventMana);
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
