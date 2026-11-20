// Translated from AoWE.pas.
using Aow.Engine;

using Aow.Engine.Nodes;

using Aow.Formats.Tree;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

namespace Aow.Game.Units;

// UnitConcealment. ConcealedForPlayers. MoraleValues
public partial class ArmyView(CustomNode owner) : Army(owner)
{
    public new const int ClassId = 131609;

    public ushort[] UnitConcealment { get; } = new ushort[8];

    public ushort ConcealedForPlayers { get; set; }

    public int[] MoraleValues { get; } = new int[8];

    // Army.PAS:122
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        PersistSupport.UInt16Array(persister, "UnitConcealment", UnitConcealment);
        ConcealedForPlayers = persister.UInt16("ConcealedForPlayers", ConcealedForPlayers);
        PersistSupport.Int32Array(persister, "MoraleValues", MoraleValues);
    }

    // Army.PAS:130
    public override void MainPersist(IPersister persister)
    {
        base.MainPersist(persister);
        // The translate stream's mode holds read, so the original sets the morale there too.
        if (!persister.Loading && !PersistSupport.Translating(persister))
        {
            return;
        }

        for (int i = 0; i < Units.Count; i++)
        {
            Units.GetUnit(i).MoraleValue = (sbyte)MoraleValues[i];
        }
    }

    // Army.PAS:138
    public override MapCoordinate GetLocation() => Location;

    // Army.PAS:89
    public override bool GetUnitConcealedForPlayer(int index, sbyte player) => (((int)(((uint)UnitConcealment[index]) >> (player - 1))) & 1) != 0;

    // Army.PAS:78
    public override bool GetConcealedForPlayer(sbyte player) => (((int)(((uint)ConcealedForPlayers) >> (player - 1))) & 1) != 0;

    // Army.PAS:100
    public virtual void Setup(Army army, byte selection)
    {
        AssignSettings(army);
        Location = new MapCoordinate(army.Location.X, army.Location.Y, army.Location.MapLevel);
        for (int i = 0, last = army.Units.Count - 1; i <= last; i++)
        {
            if ((selection & 1) != 0)
            {
                Troop troop = army.Units.GetUnit(i).Copy<Troop>(GetEngine());
                troop.SetOwner(this);
                _ = troop.Release();
                Troop unit = army.Units.GetUnit(i);
                int count = Units.Count;
                MoraleValues[count - 1] = unit.MoraleValue;
                for (int player = 1, lastPlayer = StrategicMap.Current.Players.GetCount() - 1; player <= lastPlayer; player++)
                {
                    SetUnitConcealedForPlayer(value: army.GetUnitConcealedForPlayer(i, (sbyte)player), index: Units.Count - 1, player: (sbyte)player);
                }
            }

            selection = (byte)(int)(((uint)selection) >> 1);
        }
    }

    // Army.PAS:81
    public void SetConcealedForPlayer(sbyte player, bool value)
    {
        int mask = 1 << (player - 1);
        ConcealedForPlayers = value ? (ushort)(ConcealedForPlayers | (ushort)mask) : (ushort)(ConcealedForPlayers & ~(ushort)mask);
    }

    // Army.PAS:92
    public void SetUnitConcealedForPlayer(int index, sbyte player, bool value)
    {
        int mask = 1 << (player - 1);
        UnitConcealment[index] = value ? (ushort)(UnitConcealment[index] | (ushort)mask) : (ushort)(UnitConcealment[index] & ~(ushort)mask);
    }

    // intf_P2_07.inc:445

}
