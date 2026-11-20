#nullable disable
// Translated from AoWE.pas.
using Aow.Engine;
using Aow.Engine.Collections;
using Aow.Engine.Nodes;
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
    public UnitSpellTokenEvent(CustomNode owner) : base(owner)
    {
        TargetUnitIds = new Int32List();
        _targetUnits = new UnitUserList(null) { Holder = CreationHolder() };
    }

    // Spells.PAS:1159
    public override bool Validate(Hero source)
    {
        bool result = base.Validate(source);
        MapCoordinate coordinate12 = source.GetLocation();
        sbyte x = coordinate12.X;
        sbyte y = coordinate12.Y;
        _ = coordinate12.MapLevel;
        int i = TargetUnitIds.Count;
        while (result && i != 0)
        {
            i -= 1;
            int item = TargetUnitIds[i];
            Troop target = StrategicMap.Current.UnitControl.FindUnit(item);
            if (target is not null)
            {
                MapCoordinate coordinate13 = target.GetLocation();
                sbyte targetX = coordinate13.X;
                sbyte targetY = coordinate13.Y;
                _ = coordinate13.MapLevel;
                result = target.GetHitPoints() != 0 && (byte)target.Player == TargetPlayer;
                if (result)
                {
                    if (GamePack.Current.SpellControl.GetSpell<UnitSpell>(EventSpellId).TargetRange != -1)
                    {
                        result = HexGeometry.DeltaHexToRadius(x, y, targetX, targetY) <= GamePack.Current.SpellControl.GetSpell<UnitSpell>(EventSpellId).TargetRange;
                    }
                }

                _ = _targetUnits.AddUnit(target);
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
        _ = base.Process(source, value);
        if (_targetUnits.GetCount() != 0)
        {
            MapCoordinate coordinate14 = _targetUnits.GetUnit(0).GetLocation();
            sbyte x = coordinate14.X;
            sbyte y = coordinate14.Y;
            sbyte l = coordinate14.MapLevel;
            GamePack.Current.SpellControl.GetSpell(EventSpellId).PlayDefaultCastEffects(source.Player, x, y, l);
            for (int i = 0, last = _targetUnits.GetCount() - 1; i <= last; i++)
            {
                Troop target = _targetUnits.GetUnit(i);
                _ = target.AddRef();
                if (GamePack.Current.SpellControl.GetSpell<UnitSpell>(EventSpellId).ExecuteSpell(source, target, EventMana, SpellData))
                {
                    GamePack.Current.SpellControl.GetSpell(EventSpellId).CastingDone(source, EventMana);
                    target.Changed();
                }

                _ = target.Release();
            }
        }

        return false;
    }

    // Spells.PAS:1143
    public void Initialize(int spellId, Troop source, UnitUserList targets, int mana)
    {
        EventSpellId = spellId;
        EventPlayerIndex = (byte)source.Player;
        EventSourceId = source.UnitId;
        EventMana = mana;
        if (targets is not null && targets.GetCount() != 0)
        {
            TargetPlayer = (byte)targets.GetUnit(0).Player;
            for (int i = 0, last = targets.GetCount() - 1; i <= last; i++)
            {
                Troop troop = targets.GetUnit(i);
                _ = TargetUnitIds.Add(troop.UnitId);
            }
        }
        else
        {
            TargetPlayer = 255;
        }
    }

    // intf_P4_02.inc:1059

}
