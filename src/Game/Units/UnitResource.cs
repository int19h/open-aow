// Translated from AoWE.pas.
using System.Runtime.CompilerServices;

using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Units;

// FName. FRace. FChampion. FNickname. unit statistics. FAbilities[rank]. FTransportCapacity.
// FDescription. FObtainValue. size. blood
public partial class UnitResource : TroopResource
{
    public new const int ClassID = 131602;

    // UnitRes.PAS:73
    public UnitResource(ECustomNode owner) : base(owner)
    {
        Unit.Rank Rank;

        FDescription = new StringList();
        for (int rankOrdinal = (int)(Unit.Rank)0; rankOrdinal <= (int)(Unit.Rank)2; rankOrdinal++)
        {
            Rank = (Unit.Rank)rankOrdinal;
            var unitResourceAbilityOwner = new UnitResourceAbilityOwner(null);
            FAbilities[(int)Rank] = unitResourceAbilityOwner;
        }

        FMoves = 16;
        FHits = 5;
        FAttack = 3;
        FDefense = 3;
        FResistance = 3;
        FDamage = 3;
        FUnitSize = UnitSize.Medium;
        FBloodType = BloodType.Red;
    }

    public string FName = "";
    public sbyte FRace;
    public bool FChampion;
    public string FNickname = "";
    public Alignment FAlignment;
    public sbyte FAttack;
    public sbyte FDefense;
    public sbyte FDamage;
    public sbyte FHits;
    public sbyte FMoves;
    public sbyte FResistance;
    public sbyte FLevel;
    public UnitType FUnitType;
    public UnitGender FGender;
    public AbilityOwnerArray3 FAbilities;
    public byte FTransportCapacity;
    public StringList FDescription;
    public int FObtainValue;
    public UnitSize FUnitSize;
    public BloodType FBloodType;

    // UnitRes.PAS:92
    public override void Dispose()
    {
        Unit.Rank Rank;
        FDescription?.Dispose();
        for (int rankOrdinal = (int)(Unit.Rank)0; rankOrdinal <= (int)(Unit.Rank)2; rankOrdinal++)
        {
            Rank = (Unit.Rank)rankOrdinal;
            FAbilities[(int)Rank]?.Dispose();
        }

        base.Dispose();
    }

    // UnitRes.PAS:156
    public override void ReadWrite(EStorageStream Stream)
    {
        Unit.Rank Rank;
        base.ReadWrite(Stream);
        Stream.rwString(ref FName, 10);
        Stream.rwString(ref FNickname, 11);
        Stream.rwData(Bytes.Of(ref FAlignment, 1), 1, 12);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FRace), 13);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FAttack), 14);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FDefense), 15);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FDamage), 16);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FHits), 17);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FMoves), 18);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FResistance), 19);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FLevel), 20);
        Stream.rwData(Bytes.Of(ref FUnitType, 1), 1, 21);
        Stream.rwData(Bytes.Of(ref FChampion, 1), 1, 22);
        Stream.rwData(Bytes.Of(ref FGender, 1), 1, 23);
        Stream.rwByte(ref FTransportCapacity, 24);
        Stream.rwEObject(FAbilities[(int)Unit.Rank.None], 25);
        Stream.rwStrings(FDescription, 26);
        Stream.rwInteger(ref FObtainValue, 27);
        Stream.rwData(Bytes.Of(ref FUnitSize, 1), 1, 28);
        Stream.rwData(Bytes.Of(ref FBloodType, 1), 1, 29);
        Stream.rwEObject(FAbilities[(int)Unit.Rank.Silver], 30);
        Stream.rwEObject(FAbilities[(int)Unit.Rank.Gold], 31);
        if (Stream.StorageMode().Contains(StorageModeElement.Read))
        {
            for (int rankOrdinal = (int)(Unit.Rank)0; rankOrdinal <= (int)(Unit.Rank)2; rankOrdinal++)
            {
                Rank = (Unit.Rank)rankOrdinal;
                FAbilities[(int)Rank].ValidateOwnerTypeAbilities();
            }
        }
    }

    // UnitRes.PAS:130
    public override int ResourceUserClassID() => 131604;

    // UnitRes.PAS:134
    public override EngineObject CreateResourceUser()
    {
        EngineObject Result = base.CreateResourceUser();
        if (Result != null)
        {
            ((UnitHexSprite)Result).SetUnitGFXResourceIndex(FUnitGFXResourceIndex);
        }

        return Result;
    }

    // UnitRes.PAS:188
    public override bool DropUnit(int Xhx, int Yhx, int Lhx)
    {
        sbyte Player;
        Unit AUnit;
        if (AoWHSMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting))
        {
            Player = ((AoWMapField)AoWHSMap.Current.MapContainer.GetField(Xhx, Yhx, Lhx)).GetPlayer();
            if (Player == -1)
            {
                Player = AoWHSMap.Current.FEditorPlayer;
            }
        }
        else
        {
            Player = -1;
        }

        AUnit = new Unit(null);
        AUnit.SetUnitResource(this);
        bool Result = AUnit.Place(Xhx, Yhx, Lhx, Player, Set<UnitPlaceModeElement>.Of()) != null;
        AUnit.Release();
        return Result;
    }

    // UnitRes.PAS:122
    public override int GetObtainValue() => FObtainValue;

    // UnitRes.PAS:102
    public Set<MoveType> GetMoveTypes() => FAbilities[(int)Unit.Rank.None].GetAbMoveTypesAll();

    // UnitRes.PAS:111
    public byte GetTransportCapacity()
    {
        byte Result = (byte)AoWHSSet.Current.FAbilityControl.GetAbility(50).GetLevel(FAbilities[(int)Unit.Rank.None]);
        if (Result < FTransportCapacity)
        {
            Result = FTransportCapacity;
        }

        return Result;
    }

    // UnitRes.PAS:118
    public sbyte GetUpkeep() => (sbyte)(FLevel + 1);

    // UnitRes.PAS:126
    public void SetDescription(StringCollection Value)
    {
        FDescription.Assign(Value);
    }

    // UnitRes.PAS:141
    public int UnitResourceIndex() => Owner.FindChildIndex(this);

    // UnitRes.PAS:149
    public string GetUnitName()
    {
        string Result = FName;
        if (!IsEmpty(FNickname))
        {
            Result = Result + " " + FNickname;
        }

        return Result;
    }

    // intf_P2_05.inc:243

}
