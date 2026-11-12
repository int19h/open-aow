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

// Name. Race. Champion. Nickname. unit statistics. Abilities[rank]. TransportCapacity.
// Description. ObtainValue. size. blood
public partial class UnitResource : TroopResource
{
    public new const int ClassId = 131602;

    // UnitRes.PAS:73
    public UnitResource(ECustomNode owner) : base(owner)
    {
        Unit.Rank rank;

        Description = new StringList();
        for (int rankOrdinal = (int)(Unit.Rank)0; rankOrdinal <= (int)(Unit.Rank)2; rankOrdinal++)
        {
            rank = (Unit.Rank)rankOrdinal;
            var unitResourceAbilityOwner = new UnitResourceAbilityOwner(null);
            Abilities[(int)rank] = unitResourceAbilityOwner;
        }

        Moves = 16;
        Hits = 5;
        Attack = 3;
        Defense = 3;
        Resistance = 3;
        Damage = 3;
        UnitSize = UnitSize.Medium;
        BloodType = BloodType.Red;
    }

    private string _name = "";
    public sbyte Race;
    public bool Champion;
    private string _nickname = "";
    public Alignment Alignment;
    public sbyte Attack;
    public sbyte Defense;
    public sbyte Damage;
    public sbyte Hits;
    public sbyte Moves;
    public sbyte Resistance;
    public sbyte Level;
    public UnitType UnitType;
    public UnitGender Gender;
    public AbilityOwnerArray3 Abilities;
    private byte _transportCapacity;

    public StringList Description { get; }

    public int ObtainValue;
    public UnitSize UnitSize;
    public BloodType BloodType;

    // UnitRes.PAS:92
    public override void Dispose()
    {
        Unit.Rank rank;
        Description?.Dispose();
        for (int rankOrdinal = (int)(Unit.Rank)0; rankOrdinal <= (int)(Unit.Rank)2; rankOrdinal++)
        {
            rank = (Unit.Rank)rankOrdinal;
            Abilities[(int)rank]?.Dispose();
        }

        base.Dispose();
    }

    // UnitRes.PAS:156
    public override void ReadWrite(EStorageStream stream)
    {
        Unit.Rank rank;
        base.ReadWrite(stream);
        stream.rwString(ref _name, 10);
        stream.rwString(ref _nickname, 11);
        stream.rwData(Bytes.Of(ref Alignment, 1), 1, 12);
        stream.rwByte(ref Unsafe.As<sbyte, byte>(ref Race), 13);
        stream.rwByte(ref Unsafe.As<sbyte, byte>(ref Attack), 14);
        stream.rwByte(ref Unsafe.As<sbyte, byte>(ref Defense), 15);
        stream.rwByte(ref Unsafe.As<sbyte, byte>(ref Damage), 16);
        stream.rwByte(ref Unsafe.As<sbyte, byte>(ref Hits), 17);
        stream.rwByte(ref Unsafe.As<sbyte, byte>(ref Moves), 18);
        stream.rwByte(ref Unsafe.As<sbyte, byte>(ref Resistance), 19);
        stream.rwByte(ref Unsafe.As<sbyte, byte>(ref Level), 20);
        stream.rwData(Bytes.Of(ref UnitType, 1), 1, 21);
        stream.rwData(Bytes.Of(ref Champion, 1), 1, 22);
        stream.rwData(Bytes.Of(ref Gender, 1), 1, 23);
        stream.rwByte(ref _transportCapacity, 24);
        stream.rwEObject(Abilities[(int)Unit.Rank.None], 25);
        stream.rwStrings(Description, 26);
        stream.rwInteger(ref ObtainValue, 27);
        stream.rwData(Bytes.Of(ref UnitSize, 1), 1, 28);
        stream.rwData(Bytes.Of(ref BloodType, 1), 1, 29);
        stream.rwEObject(Abilities[(int)Unit.Rank.Silver], 30);
        stream.rwEObject(Abilities[(int)Unit.Rank.Gold], 31);
        if (stream.StorageMode().Contains(StorageModeElement.Read))
        {
            for (int rankOrdinal = (int)(Unit.Rank)0; rankOrdinal <= (int)(Unit.Rank)2; rankOrdinal++)
            {
                rank = (Unit.Rank)rankOrdinal;
                Abilities[(int)rank].ValidateOwnerTypeAbilities();
            }
        }
    }

    // UnitRes.PAS:130
    public override int ResourceUserClassId() => 131604;

    // UnitRes.PAS:134
    public override EngineObject CreateResourceUser()
    {
        EngineObject result = base.CreateResourceUser();
        if (result != null)
        {
            ((UnitHexSprite)result).SetUnitGraphicsResourceIndex(UnitGraphicsResourceIndex);
        }

        return result;
    }

    // UnitRes.PAS:188
    public override bool DropUnit(int hexX, int hexY, int hexLevel)
    {
        sbyte player;
        Unit unit;
        if (StrategicMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting))
        {
            player = StrategicMap.Current.MapContainer.GetField(hexX, hexY, hexLevel).GetPlayer();
            if (player == -1)
            {
                player = StrategicMap.Current.EditorPlayer;
            }
        }
        else
        {
            player = -1;
        }

        unit = new Unit(null);
        unit.SetUnitResource(this);
        bool result = unit.Place(hexX, hexY, hexLevel, player, Set<UnitPlaceModeElement>.Of()) != null;
        unit.Release();
        return result;
    }

    // UnitRes.PAS:122
    public override int GetObtainValue() => ObtainValue;

    // UnitRes.PAS:102
    public Set<MoveType> GetMoveTypes() => Abilities[(int)Unit.Rank.None].GetAbilityMoveTypesAll();

    // UnitRes.PAS:111
    public byte GetTransportCapacity()
    {
        byte result = (byte)GamePack.Current.AbilityControl.GetAbility(50).GetLevel(Abilities[(int)Unit.Rank.None]);
        if (result < _transportCapacity)
        {
            result = _transportCapacity;
        }

        return result;
    }

    // UnitRes.PAS:118
    public sbyte GetUpkeep() => (sbyte)(Level + 1);

    // UnitRes.PAS:126
    public void SetDescription(StringCollection value)
    {
        Description.Assign(value);
    }

    // UnitRes.PAS:141
    public int UnitResourceIndex() => Owner.FindChildIndex(this);

    // UnitRes.PAS:149
    public string GetUnitName()
    {
        string result = _name;
        if (!IsEmpty(_nickname))
        {
            result = result + " " + _nickname;
        }

        return result;
    }

    // intf_P2_05.inc:243

}
