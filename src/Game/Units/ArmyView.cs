// Translated from AoWE.pas.
using Aow.Engine;

using Aow.Engine.Nodes;

using Aow.Formats.Tree;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

namespace Aow.Game.Units;

// UnitConcealment. ConcealedForPlayers. MoraleValues
public partial class ArmyView : Army
{
    public new const int ClassId = 131609;

    public ArmyView(ECustomNode owner) : base(owner)
    { }

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
        if (!persister.Loading && !PersistSupport.Translating(persister)) return;
        for (int i = 0; i < Units.Count; i++)
            Units.GetUnit(i).MoraleValue = (sbyte)MoraleValues[i];
    }

    // Army.PAS:138
    public override void GetLocation(out sbyte hexX, out sbyte hexY, out sbyte hexLevel)
    {
        hexX = X;
        hexY = Y;
        hexLevel = MapLevel;
    }

    // Army.PAS:89
    public override bool GetUnitConcealedForPlayer(int index, sbyte player) => (((int)(((uint)UnitConcealment[index]) >> (player - 1))) & 1) != 0;

    // Army.PAS:78
    public override bool GetConcealedForPlayer(sbyte player) => (((int)(((uint)ConcealedForPlayers) >> (player - 1))) & 1) != 0;

    // Army.PAS:100
    public virtual void Setup(Army army, byte selection)
    {
        Troop troop;
        AssignSettings(army);
        X = army.X;
        Y = army.Y;
        MapLevel = army.MapLevel;
        for (int I = 0, last = army.Units.Count - 1; I <= last; I++)
        {
            if ((selection & 1) != 0)
            {
                troop = (Troop)army.Units.GetUnit(I).Copy(GetEngine());
                troop.SetOwner(this);
                troop.Release();
                var unit = army.Units.GetUnit(I);
                var count = Units.Count;
                MoraleValues[count - 1] = unit.MoraleValue;
                for (int player = 1, lastPlayer = StrategicMap.Current.Players.GetCount() - 1; player <= lastPlayer; player++)
                {
                    var unitConcealedForPlayer = army.GetUnitConcealedForPlayer(I, (sbyte)player);
                    SetUnitConcealedForPlayer(Units.Count - 1, (sbyte)player, unitConcealedForPlayer);
                }
            }

            selection = (byte)(int)(((uint)selection) >> 1);
        }
    }

    // Army.PAS:81
    public void SetConcealedForPlayer(sbyte player, bool value)
    {
        int mask;
        mask = 1 << (player - 1);
        if (value)
        {
            ConcealedForPlayers = (ushort)(ConcealedForPlayers | (ushort)mask);
        }
        else
        {
            ConcealedForPlayers = (ushort)(ConcealedForPlayers & ~(ushort)mask);
        }
    }

    // Army.PAS:92
    public void SetUnitConcealedForPlayer(int index, sbyte player, bool value)
    {
        int mask;
        mask = 1 << (player - 1);
        if (value)
        {
            UnitConcealment[index] = (ushort)(UnitConcealment[index] | (ushort)mask);
        }
        else
        {
            UnitConcealment[index] = (ushort)(UnitConcealment[index] & ~(ushort)mask);
        }
    }

    // intf_P2_07.inc:445

}
