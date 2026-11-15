#nullable disable
// Translated from HSEngine.pas and AoWE.pas.

using Aow.Engine.Nodes;
using Aow.Formats.Tree;
using Aow.Game.Combat;
using Aow.Game.Draw;
using Aow.Game.Magic;
using Aow.Game.World.Movement;
using Aow.Game.World.Structures;
using Aow.Rtl;

namespace Aow.Game.World.Map;

// _hexSpriteList, _capacity, _count, _staticCount, _status, _location, FReserved, _terrainType,
// FOverlayType. A hexagon field of a map level. The original pooled all fields of a level in
// one block and freed it with the level. The port allocates each field through the base
// allocator. The sprites are sorted by their show priority. The first FStaticCount (priority
// shr 26 <= 15) are the static ones
public partial class MapField : CustomNode, IOwned<MapLevel>, IOwnedSink<MapLevel>
{
    // AoWHSMap.PAS (TAoWMapField). HSEngine.pas:1933 gives the engine field 65793.
    public new const int ClassId = 131090;

    public MapField(CustomNode owner) : base(owner)
    {
        _terrainType = -1;
        _overlayType = -1;
        _status = Set<MapFieldStatusElement>.Of();
        Resize(3);
    }

    private HexSprite[] _hexSpriteList = [];
    private byte _capacity;

    internal byte StoredCount { get; set; }

    private byte _staticCount;
    private Set<MapFieldStatusElement> _status;
    private MapCoordinate _location;
    private sbyte _terrainType;
    private sbyte _overlayType;

    // HSEngine.pas:1907
    public override void Dispose()
    {
        Array.Resize(ref _hexSpriteList, 0);
        StoredCount = 0;
    }

    // HSEngine.pas:1928
    public override Set<ControlStyleElement> GetControlStyle() => Set<ControlStyleElement>.Of(EngineServices.Stored, EngineServices.Shared);

    // HSEngine.pas:1923
    public override void SetOwner(CustomNode owner) => SetOwnerWithoutNotify(owner);

    // The level the field belongs to. AcceptsOwner admits only a level, so the cast never fails.
    public new MapLevel Owner { get => (MapLevel)base.Owner; set => SetOwner(value); }

    protected sealed override bool AcceptsOwner(CustomNode owner) => owner is MapLevel;

    void IOwnedSink<MapLevel>.SetOwner(MapLevel owner) => SetOwner(owner);

    // HSEngine.pas:2075
    // The fields only, with no Clear and no children, as the original main visit.
    public override void MainPersist(IPersister persister) => PersistFields(persister);

    // HSEngine.pas:2029, then AoWHSMap.PAS:63 stores the low word of the flags. The stored sprites
    // are a created sequence at ids 50 and on. A load creates each sprite with this field as owner
    // and then places it, as rwCreateEObject does. The translate stream visits the stored sprites,
    // as the original's text branch visits the children.
    public override void PersistFields(IPersister persister)
    {
        persister.CreatedSequence("HexSpriteList", new NodeChildren(this));
        int flags = FieldFlags;
        FieldFlags = (flags & ~0xFFFF) | persister.UInt16("FieldFlags", (ushort)flags);
    }

    // HSEngine.pas:1898

    // HSEngine.pas:1943
    public override EngineObject GetChild(int index)
    {
        if (index >= StoredCount)
        {
            throw new IndexOutOfRangeException("Index out of bounds");
        }

        return _hexSpriteList[index];
    }

    // HSEngine.pas:1949
    public override void SetChild(int index, EngineObject child)
    {
        if (index >= StoredCount)
        {
            throw new IndexOutOfRangeException("Index out of bounds");
        }

        _hexSpriteList[index] = (HexSprite)child;
    }

    // HSEngine.pas:1972
    public override int GetCount() => StoredCount;

    // HSEngine.pas:2226
    public override int RemoveChild(EngineObject child) => RemoveHexSprite((HexSprite)child);

    // HSEngine.pas:2221
    public override int AddChild(EngineObject child) => AddHexSprite((HexSprite)child, 0);

    // HSEngine.pas:2154
    public override EngineObject FindChild(int classId)
    {
        EngineObject result;
        int i;
        i = 0;
        while (i < StoredCount && _hexSpriteList[i].EngineClass.ClassId != classId)
        {
            i += 1;
        }

        if (i < StoredCount)
        {
            result = _hexSpriteList[i];
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
        int i;
        i = 0;
        while (i < StoredCount && !(_hexSpriteList[i].EngineClass.ClassId == classId && _hexSpriteList[i].Owner == this))
        {
            i += 1;
        }

        if (i < StoredCount)
        {
            result = _hexSpriteList[i];
        }
        else
        {
            result = null;
        }

        return result;
    }

    // HSEngine.pas:1938
    public MapCoordinate GetMapCoordinate() => _location;

    // HSEngine.pas:1955
    public int HexSpriteListDataSize() => (_capacity << 2) + ((_capacity + 3) & 252);

    // HSEngine.pas:2163
    public HexSprite FindHexSprite(Type classValue)
    {
        HexSprite result;
        int i;
        i = 0;
        while (i < StoredCount && !classValue.IsInstanceOfType(_hexSpriteList[i]))
        {
            i += 1;
        }

        if (i < StoredCount)
        {
            result = _hexSpriteList[i];
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
        int i;
        HexSprite hexSprite;
        i = 0;
        HexSprite result = null;
        while (i < StoredCount && result == null)
        {
            hexSprite = _hexSpriteList[i];
            if (classValue.IsInstanceOfType(hexSprite) && hexSprite.Owner == this)
            {
                result = hexSprite;
            }

            i += 1;
        }

        return result;
    }

    // HSEngine.pas:2300
    public void CopyDynamicVisibleHexSprite(VisibilityList list)
    {
        byte last = _staticCount;
        for (int i = StoredCount - 1; i >= last; i--)
        {
            _hexSpriteList[i].MakeVisible(list);
        }
    }

    // HSEngine.pas:2308
    public void CopyStaticVisibleHexSprite(VisibilityList list)
    {
        for (int i = _staticCount - 1; i >= 0; i--)
        {
            _hexSpriteList[i].MakeVisible(list);
        }
    }

    // HSEngine.pas:2316
    public void ResetDynamicVisibleHexSprite()
    {
        byte last = _staticCount;
        for (int i = StoredCount - 1; i >= last; i--)
        {
            _hexSpriteList[i].SetVisible(false);
        }
    }

    // HSEngine.pas:2324
    public void ResetStaticVisibleHexSprite()
    {
        for (int i = _staticCount - 1; i >= 0; i--)
        {
            _hexSpriteList[i].SetVisible(false);
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
        int i;
        int level;
        HexGeometry.HexToPixel(_location.X, _location.Y, out pixelX, out pixelY);
        i = StoredCount - 1;
        // { Preserve the original stack slot. }
        done = false;
        while (!done && i >= 0)
        {
            level = _hexSpriteList[i].GetLevel();
            if (level <= maxLevel)
            {
                if (level >= minLevel)
                {
                    if (_hexSpriteList[i].Owner is MapField)
                    {
                        _hexSpriteList[i].Dispose();
                    }
                }
                else
                {
                    done = true;
                }
            }

            i -= 1;
            if (i >= StoredCount)
            {
                i = StoredCount - 1;
            }
        }
    }

    // HSEngine.pas:2360
    public void DestroyHexSprite()
    {
        for (int i = StoredCount - 1; i >= 0; i--)
        {
            if (i <= StoredCount - 1)
            {
                if (_hexSpriteList[i].Owner == this)
                {
                    _hexSpriteList[i].Dispose();
                }
            }
        }
    }

    // HSEngine.pas:2369
    public void Activate()
    {
        for (int i = StoredCount - 1; i >= 0; i--)
        {
            if (_hexSpriteList[i].Owner == this)
            {
                _hexSpriteList[i].Activate();
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
        HexNumberWalkData.InitializeHexNumberWalk(out walk, _location.X, _location.Y, 0);
        for (int i = 1; i <= 6; i++)
        {
            HexNumberWalkData.WalkToNextHexNumber(ref walk);
            if (walk.X >= 0 && walk.X < Owner.Width && walk.Y >= 0 && Owner.Height > walk.Y)
            {
                info.HexNumber = (sbyte)OppositeHexDirectionTable[i - 1];
                Owner.GetField(walk.X, walk.Y).NeighbourTerrainChanged(ref info);
            }
        }
    }

    public HexSprite[] HexSpriteList { get => _hexSpriteList; internal set => _hexSpriteList = value; }
    public byte Capacity => _capacity;
    public byte StaticCount => _staticCount;
    public Set<MapFieldStatusElement> Status { get => _status; set => _status = value; }
    public sbyte HexX { get => _location.X; set => _location = _location with { X = value }; }
    public sbyte HexY { get => _location.Y; set => _location = _location with { Y = value }; }
    public sbyte HexLevel { get => _location.MapLevel; set => _location = _location with { MapLevel = value }; }
    public MapCoordinate Location => _location;
    public sbyte TerrainType { get => _terrainType; set => _terrainType = value; }
    public sbyte OverlayType { get => _overlayType; set => _overlayType = value; }

    public override void SendMessage(ref EngineMessage message, EngineObject sender)
    {
        if ((message.MessageId & 0x10000000) != 0)
        {
            for (int i = StoredCount - 1; i >= 0; i--)
            {
                var sprite = _hexSpriteList[i];
                if (sprite.GetLastMessage() == message.Sequence)
                {
                    continue;
                }

                if ((message.MessageId & 0x20000000) != 0 && sprite.Owner != this)
                {
                    continue;
                }

                sprite.SetLastMessage(message.Sequence);
                sprite.MessageProc(ref message, this);
            }
        }
        else if ((message.MessageId & 0x20000000) == 0)
        {
            for (int i = Count - 1; i >= 0; i--)
            {
                _hexSpriteList[i].MessageProc(ref message, this);
            }
        }
        else
        {
            int count = StoredCount;
            for (int i = 0; i < count; i++)
            {
                s_sendMessageList[i] = _hexSpriteList[i];
            }

            for (int i = count - 1; i >= 0; i--)
            {
                if (s_sendMessageList[i].Owner == this)
                {
                    s_sendMessageList[i].MessageProc(ref message, this);
                }
            }
        }
    }

    private static ReadOnlySpan<byte> OppositeHexDirectionTable => [4, 5, 6, 1, 2, 3];

    private static readonly HexSprite[] s_sendMessageList = new HexSprite[256];
    private static readonly HexSprite[] s_loadedList = new HexSprite[256];

    // TAoWMapField.FFieldFlags (intf_P1_03.inc:353). Bits 0 to 11 mark the field explored by
    // players 1 to 12, bits 12 to 15 and 24 to 27 tell which map field messages a sprite on the
    // field answers (UpdateMapField sets them), and byte 2 is the fog count. Only the low word is
    // stored (id 4).
    public int FieldFlags { get; set; }

    // The explored bits of players 1 to 12. The border hexagon clears them on the first day
    // (AoWHex.pas:264, $FFFFF000).
    public const int ExploredFlags = 0x00000FFF;

    // A sprite answers the sight queries: $110A, visible for a player within a range
    // (AoWHSMap.PAS:2420 and 2457), and $110B, true sight (Army.PAS:1285).
    public const int ViewerFlag = 0x00001000;

    // A sprite answers CanMoveOver, the move over a field (MoveT.PAS:391).
    public const int MoveOverFlag = 0x00002000;

    // A sprite answers CanMoveOn, the move onto a field (MoveT.PAS:304).
    public const int MoveOnFlag = 0x00004000;

    // A sprite answers MoveExclusive, which the bridge, the cave and the teleport handle
    // (MoveT.PAS:427).
    public const int MoveExclusiveFlag = 0x00008000;

    // The three move bits, cleared by UpdateMapField before the sprites set them again
    // (AoWHSMap.PAS:87, $FFFF1FFF).
    public const int MoveFlags = MoveOverFlag | MoveOnFlag | MoveExclusiveFlag;

    // A sprite answers $110D, the notice that a player changed nearby (AoWHSMap.PAS:2348).
    public const int PlayerChangeListenerFlag = 0x01000000;

    // A sprite answers $1100, the owning player (AoWHSMap.PAS:94), and $1130, a player seen
    // (AoWHSMap.PAS:2376).
    public const int OwnerFlag = 0x02000000;

    // A sprite answers $1108, the battle field strengths (AoWHSMap.PAS:3012, MainAG.pas:310,
    // PlayerC.PAS:2672).
    public const int BattleFieldInfoFlag = 0x04000000;

    // A sprite answers $1200, the AI target query (AIGroup.pas:6564 and 6643).
    public const int AITargetFlag = 0x08000000;

    // The explored bit of a player, as SetExplored shifts it (AoWHSMap.PAS:46).
    public static int ExploredFlag(int player) => 1 << ((player - 1) & 31);

    // Byte 2 of the flags: how many unfogged areas cover the field. FogArea counts it down and
    // UnFogArea up, and zero is fogged (AoWHSMap.PAS:247 and 265, WordRec(LongRec(FFieldFlags).Hi).Lo).
    public byte FogCount
    {
        get => (byte)(FieldFlags >> 16);
        private set => FieldFlags = (FieldFlags & ~0x00FF0000) | (value << 16);
    }

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
        if ((value == 0 && FogCount != 0) || (value != 0 && FogCount == 0))
        {
            FogCount = value;
            FogChanged();
        }
        else
        {
            FogCount = value;
        }
    }

    // AoWHSMap.PAS:90
    public sbyte GetPlayer()
    {
        sbyte result;
        MapFieldMessage message = default;
        if ((FieldFlags & OwnerFlag) != 0)
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
        var param = message.Param;
        param[0] = 0;
        message.Param = param;
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
        var param = message.Param;
        param[0] = 0;
        message.Param = param;
        SendMapFieldMessage(ref message);
        return message.Param[0].Int != 0;
    }

    // AoWHSMap.PAS:132
    public bool GetMagicShieldActive()
    {
        MapFieldMessage message = default;
        message.MessageId = 4393;
        message.Result = 0;
        var param = message.Param;
        param[0] = 0;
        message.Param = param;
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
        var param = message.Param;
        param[0] = player;
        message.Param = param;
        var param2 = message.Param;
        param2[1] = 0;
        message.Param = param2;
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
        var param = message.Param;
        param[0] = this;
        message.Param = param;
        message.Result = 0;
        SendMapFieldMessage(ref message);
        return (WallType)message.Result.Int;
    }

    // AoWHSMap.PAS:214
    // Only ArmyHexSprite registers 131607, and the game has no subclass of it.
    public ArmyHexSprite GetArmyHexSprite() => FindOwnedChild<ArmyHexSprite>(131607);

    // AoWHSMap.PAS:218
    public void RestoreTerrain()
    {
        MapFieldMessage message = default;
        message.MessageId = 4402;
        var param = message.Param;
        param[0] = this;
        message.Param = param;
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
        int mask = ExploredFlag(player);
        if (value != 0)
        {
            FieldFlags |= mask;
        }
        else
        {
            FieldFlags &= ~mask;
        }
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
