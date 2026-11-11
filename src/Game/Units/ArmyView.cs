// Translated from AoWE.pas.
using System.Runtime.InteropServices;
using Aow.Engine;
using Aow.Engine.Buffers;

using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Formats.Tree;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

namespace Aow.Game.Units;

// FUnitConcealment. FConcealedForPlayers. FMoraleValues
public partial class ArmyView : Army
{
    public new const int ClassID = 131609;

    public ArmyView(ECustomNode owner) : base(owner)
    { }

    public UInt16Array8 FUnitConcealment;
    public ushort FConcealedForPlayers;
    public Int32Array8 FMoraleValues;

    // Army.PAS:122
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwData(MemoryMarshal.AsBytes((Span<ushort>)FUnitConcealment).Slice(0, 16), 16, 40);
        Stream.rwWord(ref FConcealedForPlayers, 41);
        Stream.rwData(MemoryMarshal.AsBytes((Span<int>)FMoraleValues).Slice(0, 32), 32, 42);
    }

    // Army.PAS:130
    public override void MainReadWrite(EStorageStream Stream)
    {
        base.MainReadWrite(Stream);
        if (Stream.StorageMode().Contains(StorageModeElement.Read))
        {
            for (int I = 0, last = GetCount() - 1; I <= last; I++)
            {
                GetUnit(I).FMoraleValue = (sbyte)FMoraleValues[I];
            }
        }
    }

    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        PersistSupport.UInt16Array(persister, "UnitConcealment", FUnitConcealment);
        persister.UShort("ConcealedForPlayers", ref FConcealedForPlayers);
        PersistSupport.Int32Array(persister, "MoraleValues", FMoraleValues);
    }

    public override void MainPersist(IPersister persister)
    {
        base.MainPersist(persister);
        if (!persister.Loading) return;
        for (int i = 0; i < GetCount(); i++)
            GetUnit(i).FMoraleValue = (sbyte)FMoraleValues[i];
    }

    // Army.PAS:138
    public override void GetLocation(ref sbyte Xhx, ref sbyte Yhx, ref sbyte Lhx)
    {
        Xhx = FX;
        Yhx = FY;
        Lhx = FL;
    }

    // Army.PAS:89
    public override bool GetUnitConcealedForPlayer(int Index, sbyte Player) => (((int)(((uint)FUnitConcealment[Index]) >> (Player - 1))) & 1) != 0;

    // Army.PAS:78
    public override bool GetConcealedForPlayer(sbyte Player) => (((int)(((uint)FConcealedForPlayers) >> (Player - 1))) & 1) != 0;

    // Army.PAS:100
    public virtual void Setup(Army Army, byte Selection)
    {
        Troop AUnit;
        AssignSettings(Army);
        FX = Army.FX;
        FY = Army.FY;
        FL = Army.FL;
        for (int I = 0, last = Army.GetCount() - 1; I <= last; I++)
        {
            if ((Selection & 1) != 0)
            {
                AUnit = (Troop)Army.GetUnit(I).Copy(GameTables.AoWEngine);
                AUnit.SetOwner(this);
                AUnit.Release();
                var unit = Army.GetUnit(I);
                var count = GetCount();
                FMoraleValues[count - 1] = unit.FMoraleValue;
                for (int Player = 1, lastPlayer = AoWHSMap.Current.FPlayers.GetCount() - 1; Player <= lastPlayer; Player++)
                {
                    var unitConcealedForPlayer = Army.GetUnitConcealedForPlayer(I, (sbyte)Player);
                    SetUnitConcealedForPlayer(GetCount() - 1, (sbyte)Player, unitConcealedForPlayer);
                }
            }

            Selection = (byte)(int)(((uint)Selection) >> 1);
        }
    }

    // Army.PAS:81
    public void SetConcealedForPlayer(sbyte Player, bool Value)
    {
        int Mask;
        Mask = 1 << (Player - 1);
        if (Value)
        {
            FConcealedForPlayers = (ushort)(FConcealedForPlayers | (ushort)Mask);
        }
        else
        {
            FConcealedForPlayers = (ushort)(FConcealedForPlayers & ~(ushort)Mask);
        }
    }

    // Army.PAS:92
    public void SetUnitConcealedForPlayer(int Index, sbyte Player, bool Value)
    {
        int Mask;
        Mask = 1 << (Player - 1);
        if (Value)
        {
            FUnitConcealment[Index] = (ushort)(FUnitConcealment[Index] | (ushort)Mask);
        }
        else
        {
            FUnitConcealment[Index] = (ushort)(FUnitConcealment[Index] & ~(ushort)Mask);
        }
    }

    // intf_P2_07.inc:445

}
