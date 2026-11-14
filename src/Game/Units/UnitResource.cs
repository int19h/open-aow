// Translated from AoWE.pas.
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

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
            var unitResourceAbilityOwner = new UnitResourceAbilityOwner(null) { Holder = CreationHolder() };
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

    public sbyte Race { get; set; }

    public bool Champion;
    private string _nickname = "";

    public Alignment Alignment { get; private set; }
    public sbyte Attack { get; set; }
    public sbyte Defense { get; private set; }
    public sbyte Damage { get; private set; }
    public sbyte Hits { get; private set; }
    public sbyte Moves { get; private set; }
    public sbyte Resistance { get; private set; }
    public sbyte Level { get; set; }
    public UnitType UnitType { get; private set; }
    public UnitGender Gender { get; private set; }

    public readonly AbilityOwnerArray3 Abilities;
    private byte _transportCapacity;

    public StringList Description { get; }

    public int ObtainValue { get; set; }
    public UnitSize UnitSize { get; private set; }
    public BloodType BloodType { get; private set; }

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
    public override void PersistFields(IPersister persister)
    {
        Unit.Rank rank;
        base.PersistFields(persister);
        _name = persister.String("Name", value: _name);
        _nickname = persister.String("Nickname", value: _nickname);
        Alignment = persister.Enum("Alignment", value: Alignment);
        Race = PersistSupport.UnsignedByte(persister, name: "Race", value: Race);
        Attack = PersistSupport.UnsignedByte(persister, name: "Attack", value: Attack);
        Defense = PersistSupport.UnsignedByte(persister, name: "Defense", value: Defense);
        Damage = PersistSupport.UnsignedByte(persister, name: "Damage", value: Damage);
        Hits = PersistSupport.UnsignedByte(persister, name: "Hits", value: Hits);
        Moves = PersistSupport.UnsignedByte(persister, name: "Moves", value: Moves);
        Resistance = PersistSupport.UnsignedByte(persister, name: "Resistance", value: Resistance);
        Level = PersistSupport.UnsignedByte(persister, name: "Level", value: Level);
        UnitType = persister.Enum("UnitType", value: UnitType);
        Champion = persister.RawBool("Champion", value: Champion);
        Gender = persister.Enum("Gender", value: Gender);
        _transportCapacity = persister.Byte("TransportCapacity", value: _transportCapacity);
        persister.Object("NoRankAbilities", objectValue: Abilities[(int)Unit.Rank.None]);
        PersistSupport.Lines(persister, name: "Description", lines: Description);
        ObtainValue = persister.Int32("ObtainValue", value: ObtainValue);
        UnitSize = persister.Enum("UnitSize", value: UnitSize);
        BloodType = persister.Enum("BloodType", value: BloodType);
        persister.Object("SilverRankAbilities", objectValue: Abilities[(int)Unit.Rank.Silver]);
        persister.Object("GoldRankAbilities", objectValue: Abilities[(int)Unit.Rank.Gold]);
        // The translate stream's mode holds read, so the original validates the abilities there too.
        if (persister.Loading || PersistSupport.Translating(persister))
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

        unit = new Unit(null) { Holder = CreationHolder() };
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
