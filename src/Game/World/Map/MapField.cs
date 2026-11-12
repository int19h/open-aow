// Translated from HSEngine.pas and AoWE.pas.

using System.Runtime.CompilerServices;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Game.Combat;
using Aow.Game.Draw;
using Aow.Game.Magic;
using Aow.Game.World.Movement;
using Aow.Game.World.Structures;
using Aow.Rtl;

namespace Aow.Game.World.Map;

// _hexSpriteList, _capacity, _count, _staticCount, _status, _hexX, _hexY, _hexLevel, FReserved, _terrainType,
// FOverlayType. A hexagon field of a map level. The original pooled all fields of a level in
// one block and freed it with the level. The port allocates each field through the base
// allocator. The sprites are sorted by their show priority. The first FStaticCount (priority
// shr 26 <= 15) are the static ones
public partial class MapField : ECustomNode, IOwned<MapLevel>, IOwnedSink<MapLevel>
{
    // AoWHSMap.PAS (TAoWMapField). HSEngine.pas:1933 gives the engine field 65793.
    public new const int ClassId = 131090;

    public MapField(ECustomNode owner) : base(owner)
    {
        _terrainType = -1;
        _overlayType = -1;
        _status = Set<MapFieldStatusElement>.Of();
        Resize(3);
    }

    internal HexSprite[] _hexSpriteList = [];
    private byte _capacity;
    internal byte _count;
    private byte _staticCount;
    private Set<MapFieldStatusElement> _status;
    private sbyte _hexX;
    private sbyte _hexY;
    private sbyte _hexLevel;
    private sbyte _terrainType;
    private sbyte _overlayType;

    // HSEngine.pas:1907
    public override void Dispose()
    {
        Array.Resize(ref _hexSpriteList, 0);
        _count = 0;
    }

    // HSEngine.pas:1928
    public override Set<ControlStyleElement> GetControlStyle() => Set<ControlStyleElement>.Of(EngineServices.Stored, EngineServices.Shared);

    // HSEngine.pas:1923
    public override void SetOwner(ECustomNode owner)
    {
        SetOwnerWithoutNotify(owner);
    }

    // The level the field belongs to. AcceptsOwner admits only a level, so the cast never fails.
    public new MapLevel Owner { get => (MapLevel)base.Owner; set => SetOwner(value); }

    protected sealed override bool AcceptsOwner(ECustomNode owner) => owner is MapLevel;

    void IOwnedSink<MapLevel>.SetOwner(MapLevel owner) => SetOwner(owner);

    // HSEngine.pas:2029, then AoWHSMap.PAS:63 stores the flags.
    // { Original tests the entire ECX register for a Boolean passed in CL. }
    public override void ReadWrite(StorageStream stream)
    {
        int J;
        int I;
        EngineObject hexSprite = default;
        if ((stream.StorageMode() & Set<StorageModeElement>.Of(EngineServices.Text)) == Set<StorageModeElement>.Of())
        {
            if (stream.StorageMode().Contains(StorageModeElement.Read))
            {
                if (stream.StorageMode().Contains(StorageModeElement.Compare))
                {
                    J = 0;
                    I = stream.PropertyTable.First;
                    int last = stream.PropertyTable.Count - 1;
                    for (; I <= last; I += 1)
                    {
                        if (stream.PropertyTable.Items[I].Id >= 50)
                        {
                            hexSprite = _hexSpriteList[J];
                            stream.Created(ref hexSprite, this, stream.PropertyTable.Items[I].Id);
                            J += 1;
                        }
                    }
                }
                else
                {
                    I = stream.PropertyTable.First;
                    int last = stream.PropertyTable.Count - 1;
                    for (; I <= last; I += 1)
                    {
                        J = stream.PropertyTable.Items[I].Id;
                        if (J >= 50)
                        {
                            stream.Created(ref hexSprite, this, J);
                        }
                    }
                }
            }
            else
            {
                I = 50;
                J = 0;
                int last = _count - 1;
                for (; J <= last; J += 1)
                {
                    hexSprite = _hexSpriteList[J];
                    if (hexSprite != null && hexSprite.Owner == this && !hexSprite.GetControlStyle().Contains(EngineServices.Shared) && hexSprite.GetControlStyle().Contains(EngineServices.Stored))
                    {
                        stream.Created(ref hexSprite, this, I);
                        I += 1;
                    }
                }
            }
        }
        else
        {
            ReadWriteChildren(stream);
        }

        stream.UInt16(ref Unsafe.As<int, LongRec>(ref FieldFlags).Lo, 4);
    }

    // HSEngine.pas:2075
    public override void MainReadWrite(StorageStream stream)
    {
        ReadWrite(stream);
    }

    // HSEngine.pas:1898

    // HSEngine.pas:1943
    public override EngineObject GetChild(int index)
    {
        if (index >= _count)
        {
            throw new IndexOutOfRangeException("Index out of bounds");
        }

        return _hexSpriteList[index];
    }

    // HSEngine.pas:1949
    public override void SetChild(int index, EngineObject child)
    {
        if (index >= _count)
        {
            throw new IndexOutOfRangeException("Index out of bounds");
        }

        _hexSpriteList[index] = (HexSprite)child;
    }

    // HSEngine.pas:1972
    public override int GetCount() => _count;

    // HSEngine.pas:2226
    public override int RemoveChild(EngineObject child) => RemoveHexSprite((HexSprite)child);

    // HSEngine.pas:2221
    public override int AddChild(EngineObject child) => AddHexSprite((HexSprite)child, 0);

    // HSEngine.pas:2154
    public override EngineObject FindChild(int classId)
    {
        EngineObject result;
        int I;
        I = 0;
        while (I < _count && _hexSpriteList[I].EngineClass.ClassId != classId)
        {
            I += 1;
        }

        if (I < _count)
        {
            result = _hexSpriteList[I];
        }
        else
        {
            result = null;
        }

        return result;
    }

    // HSEngine.pas:2211
    public override EngineObject FindOwnedChild(int classId)
    {
        EngineObject result;
        int I;
        I = 0;
        while (I < _count && !(_hexSpriteList[I].EngineClass.ClassId == classId && _hexSpriteList[I].Owner == this))
        {
            I += 1;
        }

        if (I < _count)
        {
            result = _hexSpriteList[I];
        }
        else
        {
            result = null;
        }

        return result;
    }

    // HSEngine.pas:1938
    public int GetMapCoordinate() => _hexX + (_hexY << 8) + (_hexLevel << 16);

    // HSEngine.pas:1955
    public int HexSpriteListDataSize() => (_capacity << 2) + ((_capacity + 3) & 252);

    // HSEngine.pas:2163
    public HexSprite FindHexSprite(Type classValue)
    {
        HexSprite result;
        int I;
        I = 0;
        while (I < _count && !classValue.IsInstanceOfType(_hexSpriteList[I]))
        {
            I += 1;
        }

        if (I < _count)
        {
            result = _hexSpriteList[I];
        }
        else
        {
            result = null;
        }

        return result;
    }

    // HSEngine.pas:2172
    public HexSprite FindOwnedHexSprite(Type classValue)
    {
        int I;
        HexSprite hexSprite;
        I = 0;
        HexSprite result = null;
        while (I < _count && result == null)
        {
            hexSprite = _hexSpriteList[I];
            if (classValue.IsInstanceOfType(hexSprite) && hexSprite.Owner == this)
            {
                result = hexSprite;
            }

            I += 1;
        }

        return result;
    }

    // HSEngine.pas:2300
    public void CopyDynamicVisibleHexSprite(VisibilityList list)
    {
        byte last = _staticCount;
        for (int I = _count - 1; I >= last; I--)
        {
            _hexSpriteList[I].MakeVisible(list);
        }
    }

    // HSEngine.pas:2308
    public void CopyStaticVisibleHexSprite(VisibilityList list)
    {
        for (int I = _staticCount - 1; I >= 0; I--)
        {
            _hexSpriteList[I].MakeVisible(list);
        }
    }

    // HSEngine.pas:2316
    public void ResetDynamicVisibleHexSprite()
    {
        byte last = _staticCount;
        for (int I = _count - 1; I >= last; I--)
        {
            _hexSpriteList[I].SetVisible(false);
        }
    }

    // HSEngine.pas:2324
    public void ResetStaticVisibleHexSprite()
    {
        for (int I = _staticCount - 1; I >= 0; I--)
        {
            _hexSpriteList[I].SetVisible(false);
        }
    }

    // HSEngine.pas:2335
    // { The sprites with a level from MinLevel to MaxLevel that a map field owns are destroyed; the list
    //   is sorted, so the walk stops at the first sprite below MinLevel. The HP position is computed and
    //   never used. }
    public void DestroyLevelHexSprite(int minLevel, int maxLevel)
    {
        int pixelX = default;
        int pixelY = default;
        bool done;
        int I;
        int level;
        HexGeometry.HexToPixel(_hexX, _hexY, out pixelX, out pixelY);
        I = _count - 1;
        // { Preserve the original stack slot. }
        done = false;
        while (!done && I >= 0)
        {
            level = _hexSpriteList[I].GetLevel();
            if (level <= maxLevel)
            {
                if (level >= minLevel)
                {
                    if (_hexSpriteList[I].Owner is MapField)
                    {
                        _hexSpriteList[I].Dispose();
                    }
                }
                else
                {
                    done = true;
                }
            }

            I -= 1;
            if (I >= _count)
            {
                I = _count - 1;
            }
        }
    }

    // HSEngine.pas:2360
    public void DestroyHexSprite()
    {
        for (int I = _count - 1; I >= 0; I--)
        {
            if (I <= _count - 1)
            {
                if (_hexSpriteList[I].Owner == this)
                {
                    _hexSpriteList[I].Dispose();
                }
            }
        }
    }

    // HSEngine.pas:2369
    public void Activate()
    {
        for (int I = _count - 1; I >= 0; I--)
        {
            if (_hexSpriteList[I].Owner == this)
            {
                _hexSpriteList[I].Activate();
            }
        }
    }

    // HSEngine.pas:2581
    public sbyte GetDecorationType()
    {
        MapFieldMessage message = default;
        message.MessageId = 17;
        message.Result = -1;
        SendMapFieldMessage(ref message);
        return (sbyte)message.Result.Int;
    }

    // HSEngine.pas:2591
    public void UpdateNeighbourTerrain()
    {
        HexSpriteTerrainInfo info = default;
        HexNumberWalkData walk = default;
        info.MapField = this;
        info.TerrainType = _terrainType;
        info.OverlayType = _overlayType;
        HexNumberWalkData.InitializeHexNumberWalk(out walk, _hexX, _hexY, 0);
        for (int I = 1; I <= 6; I++)
        {
            HexNumberWalkData.WalkToNextHexNumber(ref walk);
            if (walk.X >= 0 && walk.X < Owner.Width && walk.Y >= 0 && Owner.Height > walk.Y)
            {
                info.HexNumber = (sbyte)OppositeHexDirectionTable[I - 1];
                Owner.GetField(walk.X, walk.Y).NeighbourTerrainChanged(ref info);
            }
        }
    }

    public HexSprite[] HexSpriteList { get => _hexSpriteList; }
    public byte Capacity { get => _capacity; }
    public byte StaticCount { get => _staticCount; }
    public Set<MapFieldStatusElement> Status { get => _status; set => _status = value; }
    public sbyte HexX { get => _hexX; set => _hexX = value; }
    public sbyte HexY { get => _hexY; set => _hexY = value; }
    public sbyte HexLevel { get => _hexLevel; set => _hexLevel = value; }
    public int MapCoordinate { get => GetMapCoordinate(); }
    public sbyte TerrainType { get => _terrainType; set => _terrainType = value; }
    public sbyte OverlayType { get => _overlayType; set => _overlayType = value; }

    public override void SendMessage(ref EngineMessage message, EngineObject sender)
    {
        if ((message.MessageId & 0x10000000) != 0)
        {
            for (int i = _count - 1; i >= 0; i--)
            {
                var sprite = _hexSpriteList[i];
                if (sprite.GetLastMessage() == message.Sequence) continue;
                if ((message.MessageId & 0x20000000) != 0 && sprite.Owner != this) continue;
                sprite.SetLastMessage(message.Sequence);
                sprite.MessageProc(ref message, this);
            }
        }
        else if ((message.MessageId & 0x20000000) == 0)
        {
            for (int i = Count - 1; i >= 0; i--) _hexSpriteList[i].MessageProc(ref message, this);
        }
        else
        {
            int count = _count;
            for (int i = 0; i < count; i++) s_sendMessageList[i] = _hexSpriteList[i];
            for (int i = count - 1; i >= 0; i--)
                if (s_sendMessageList[i].Owner == this) s_sendMessageList[i].MessageProc(ref message, this);
        }
    }

    private static ReadOnlySpan<byte> OppositeHexDirectionTable => [4, 5, 6, 1, 2, 3];

    private static readonly HexSprite[] s_sendMessageList = new HexSprite[256];
    private static readonly HexSprite[] s_loadedList = new HexSprite[256];

    // TAoWMapField: the flags, the fog count in byte 2 and bit $02000000 for the owner query.
    public int FieldFlags;

    // AoWHSMap.PAS:68
    public void FogChanged()
    {
        MapFieldMessage message = default;
        message.MessageId = 4358;
        SendMapFieldMessage(ref message);
    }

    // AoWHSMap.PAS:75
    public void SetFogCount(byte value)
    {
        if ((value == 0 && Unsafe.As<ushort, WordRec>(ref Unsafe.As<int, LongRec>(ref FieldFlags).Hi).Lo != 0) || (value != 0 && Unsafe.As<ushort, WordRec>(ref Unsafe.As<int, LongRec>(ref FieldFlags).Hi).Lo == 0))
        {
            Unsafe.As<ushort, WordRec>(ref Unsafe.As<int, LongRec>(ref FieldFlags).Hi).Lo = value;
            FogChanged();
        }
        else
        {
            Unsafe.As<ushort, WordRec>(ref Unsafe.As<int, LongRec>(ref FieldFlags).Hi).Lo = value;
        }
    }

    // AoWHSMap.PAS:90
    public sbyte GetPlayer()
    {
        sbyte result;
        MapFieldMessage message = default;
        if ((FieldFlags & 33554432) != 0)
        {
            message.MessageId = 4352;
            message.Result = -1;
            SendMapFieldMessage(ref message);
            result = (sbyte)message.Result.Int;
        }
        else
        {
            result = -1;
        }

        return result;
    }

    // AoWHSMap.PAS:103
    public int GetScannerIcon()
    {
        MapFieldMessage message = default;
        message.MessageId = 4364;
        message.Param[0] = 0;
        message.Result = 0;
        SendMapFieldMessage(ref message);
        return message.Param[0].Int;
    }

    // AoWHSMap.PAS:113
    public int GetCombatTerrainTag()
    {
        MapFieldMessage message = default;
        message.MessageId = 4386;
        message.Result = 0;
        SendMapFieldMessage(ref message);
        return message.Result.Int;
    }

    // AoWHSMap.PAS:122
    public bool GetConcealmentActive()
    {
        MapFieldMessage message = default;
        message.MessageId = 4392;
        message.Result = 0;
        message.Param[0] = 0;
        SendMapFieldMessage(ref message);
        return message.Param[0].Int != 0;
    }

    // AoWHSMap.PAS:132
    public bool GetMagicShieldActive()
    {
        MapFieldMessage message = default;
        message.MessageId = 4393;
        message.Result = 0;
        message.Param[0] = 0;
        SendMapFieldMessage(ref message);
        return message.Param[0].Int != 0;
    }

    // AoWHSMap.PAS:142
    public ProductionControl GetProductionControl()
    {
        MapFieldMessage message = default;
        message.MessageId = 4609;
        message.Result = 0;
        SendMapFieldMessage(ref message);
        return message.Result.As<ProductionControl>();
    }

    // AoWHSMap.PAS:165
    public MoveAction GetMoveAction(sbyte player)
    {
        MapFieldMessage message = default;
        message.MessageId = 4353;
        message.Param[0] = player;
        message.Param[1] = 0;
        SendMapFieldMessage(ref message);
        return (MoveAction)message.Param[1].Int;
    }

    // AoWHSMap.PAS:175
    public byte GetIndependentRelation()
    {
        MapFieldMessage message = default;
        message.MessageId = 4389;
        message.Result = -1;
        SendMapFieldMessage(ref message);
        return (byte)message.Result.Int;
    }

    // AoWHSMap.PAS:184
    public byte GetDefensePriority()
    {
        MapFieldMessage message = default;
        message.MessageId = 4390;
        message.Result = 0;
        SendMapFieldMessage(ref message);
        return (byte)message.Result.Int;
    }

    // AoWHSMap.PAS:204
    public WallType GetWallType()
    {
        MapFieldMessage message = default;
        message.MessageId = 4387;
        message.Param[0] = this;
        message.Result = 0;
        SendMapFieldMessage(ref message);
        return (WallType)message.Result.Int;
    }

    // AoWHSMap.PAS:214
    public ArmyHexSprite GetArmyHexSprite() => (ArmyHexSprite)FindOwnedChild(131607);

    // AoWHSMap.PAS:218
    public void RestoreTerrain()
    {
        MapFieldMessage message = default;
        message.MessageId = 4402;
        message.Param[0] = this;
        message.Result = 0;
        SendMapFieldMessage(ref message);
    }

    public bool GetExplored(int player)
    {
        int count = player & 31;
        return count == 0 || ((uint)FieldFlags & (1u << (count - 1))) != 0;
    }

    public void SetExplored(int player, bool value) => SetExploredRaw(player, value ? 1 : 0);

    // The original tests ECX, including the bits above the Boolean byte.
    public void SetExploredRaw(int player, int value)
    {
        int mask = 1 << ((player - 1) & 31);
        if (value != 0) FieldFlags |= mask;
        else FieldFlags &= ~mask;
    }

    internal static int IgnitionChance(MapField mapField)
    {
        int result;
        if (Set<byte>.WithBounds(0, 255).With(12).WithRange(1, 5).Contains((byte)mapField.TerrainType) && mapField.FindOwnedChild(132990) == null && !mapField.GetMagicShieldActive())
        {
            if (mapField.OverlayType == 1)
            {
                if (mapField.TerrainType == 3)
                {
                    result = 40;
                }
                else
                {
                    if (mapField.TerrainType == 2)
                    {
                        result = 80;
                    }
                    else
                    {
                        result = 60;
                    }
                }
            }
            else
            {
                if (mapField.OverlayType == 3)
                {
                    if (mapField.TerrainType == 3)
                    {
                        result = 10;
                    }
                    else
                    {
                        if (mapField.TerrainType == 2)
                        {
                            result = 30;
                        }
                        else
                        {
                            result = 20;
                        }
                    }
                }
                else
                {
                    if (mapField.TerrainType == 3)
                    {
                        result = 1;
                    }
                    else
                    {
                        result = 5;
                    }
                }
            }

            if (mapField.FindNoneTransparentHexSprite(typeof(Structure)) != null)
            {
                result += 5;
                if (StrategicMap.Current != null && StrategicMap.Current.GlobalMagicControl.ActiveEnchantments.Contains(GlobalEnchantmentType.FireMastery))
                {
                    result += 5;
                }
            }

            if (result < 0)
            {
                result = 0;
            }
        }
        else
        {
            result = 0;
        }

        return result;
    }
}
