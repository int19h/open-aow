#nullable disable
using Aow.Engine.Nodes;
using Aow.Game.Draw;

using Aow.Game.World.Movement;
using Aow.Rtl;

namespace Aow.Game.World.Map;

public partial class MapField
{
    private byte[] _directions = [];

    public void Resize(byte capacity)
    {
        if (capacity is > 0 and < 3)
        {
            capacity = 3;
        }

        Array.Resize(ref _hexSpriteList, capacity);
        Array.Resize(ref _directions, capacity);
        _capacity = capacity;
    }

    public byte GetHexNumber(int index) => index >= StoredCount ? throw new IndexOutOfRangeException("Index out of bounds") : _directions[index];

    public void SetHexNumber(int index, byte value)
    {
        if (index >= StoredCount)
        {
            throw new IndexOutOfRangeException("Index out of bounds");
        }

        _directions[index] = value;
    }

    public int AddHexSprite(HexSprite sprite, int hexNumber)
    {
        try
        {
            if (sprite == null)
            {
                throw new ArgumentException("No nil HS allowed in TMapField");
            }

            if (FindChildIndex(sprite) != -1)
            {
                throw new InvalidOperationException("HS already defined");
            }
        }
        catch { }
        if (_capacity == StoredCount)
        {
            Resize((byte)(_capacity + 1));
        }

        int priority = sprite.GetShowPriority();
        int index = _status.Contains(MapFieldStatusElement.Comparing) ? StoredCount : 0;
        if (!_status.Contains(MapFieldStatusElement.Comparing))
        {
            while (index < StoredCount && priority > _hexSpriteList[index].GetShowPriority())
            {
                index++;
            }
        }

        if (index < StoredCount)
        {
            Array.Copy(_hexSpriteList, index, _hexSpriteList, index + 1, StoredCount - index);
            Array.Copy(_directions, index, _directions, index + 1, StoredCount - index);
        }
        if ((uint)priority >> 26 <= 15)
        {
            _staticCount++;
        }

        _hexSpriteList[index] = sprite;
        _directions[index] = (byte)hexNumber;
        StoredCount++;
        return index;
    }

    public int RemoveHexSprite(HexSprite sprite)
    {
        int index = 0;
        while (index < StoredCount && sprite != _hexSpriteList[index])
        {
            index++;
        }

        if (index == StoredCount)
        {
            return -1;
        }

        StoredCount--;
        if (index < StoredCount)
        {
            Array.Copy(_directions, index + 1, _directions, index, StoredCount - index);
            Array.Copy(_hexSpriteList, index + 1, _hexSpriteList, index, StoredCount - index);
        }
        if ((uint)sprite.GetShowPriority() >> 26 <= 15)
        {
            if (_staticCount != 0)
            {
                _staticCount--;
            }
            else
            {
                try { throw new ArgumentException($"Invalid StaticCount ({sprite.GetType().Name})"); }
                catch { }
            }
        }
        return StoredCount;
    }

    public void Remove(int index)
    {
        StoredCount--;
        if (index < StoredCount)
        {
            Array.Copy(_directions, index + 1, _directions, index, StoredCount - index);
            Array.Copy(_hexSpriteList, index + 1, _hexSpriteList, index, StoredCount - index);
        }
        if (_capacity > StoredCount + 1)
        {
            Resize(StoredCount);
        }
    }

    public void Sort()
    {
        bool changed = true;
        while (changed)
        {
            changed = false;
            int index = Count - 2;
            while (!changed && index >= 0)
            {
                if (_hexSpriteList[index].GetShowPriority() > _hexSpriteList[index + 1].GetShowPriority())
                {
                    changed = true;
                    (_hexSpriteList[index], _hexSpriteList[index + 1]) = (_hexSpriteList[index + 1], _hexSpriteList[index]);
                    (_directions[index], _directions[index + 1]) = (_directions[index + 1], _directions[index]);
                }
                index--;
            }
        }
        _staticCount = 0;
        while (_staticCount != StoredCount && (uint)_hexSpriteList[_staticCount].GetShowPriority() >> 26 <= 15)
        {
            _staticCount++;
        }
    }

    public void Loaded()
    {
        int count = StoredCount;
        for (int i = 0; i < count; i++)
        {
            s_loadedList[i] = _hexSpriteList[i];
        }

        for (int i = count - 1; i >= 0; i--)
        {
            if (s_loadedList[i].Owner == this && !s_loadedList[i].MainLoaded())
            {
                s_loadedList[i]?.Dispose();
            }
        }
    }

    // AoWHSMap.PAS:85 clears the flags, then the engine body updates the sprites.
    public virtual void UpdateMapField()
    {
        FieldFlags &= ~MoveFlags;
        HexSpriteMapFieldInfo info = new() { MapField = this };
        for (int i = StoredCount - 1; i >= 0; i--)
        {
            info.HexNumber = (sbyte)_directions[i];
            _hexSpriteList[i].UpdateMapField(info);
        }
    }

    public void Initialize()
    {
        sbyte overlay = -1, terrain = -1;
        for (int i = StoredCount - 1; i >= 0; i--)
        {
            if (terrain < 0)
            {
                terrain = _hexSpriteList[i].GetTerrain(_directions[i]);
            }

            if (overlay < 0)
            {
                overlay = _hexSpriteList[i].GetOverlay(_directions[i]);
            }
        }
        _overlayType = Math.Max(overlay, (sbyte)-1);
        _terrainType = Math.Max(terrain, (sbyte)-1);
        HexSpriteTerrainInfo info = new() { MapField = this, TerrainType = _terrainType, OverlayType = _overlayType };
        for (int i = StoredCount - 1; i >= 0; i--)
        {
            info.HexNumber = (sbyte)_directions[i];
            _hexSpriteList[i].TerrainChanged(info);
        }
        UpdateMapField();
    }

    public int SetHexNumberOfHexSprite(HexSprite sprite, byte hexNumber)
    {
        int index = FindChildIndex(sprite);
        if (index != -1)
        {
            _directions[index] = hexNumber;
        }

        return index;
    }

    public int GetHexNumberOfHexSprite(HexSprite sprite)
    {
        int index = FindChildIndex(sprite);
        return index == -1 ? -1 : _directions[index];
    }

    private HexSprite FindTop(Func<HexSprite, byte, bool> matches)
    {
        for (int i = StoredCount - 1; i >= 0; i--)
        {
            if (matches(_hexSpriteList[i], _directions[i]))
            {
                return _hexSpriteList[i];
            }
        }

        return null;
    }

    public HexSprite GetMaxLevelNoneTransparentHexSprite() => FindTop((sprite, hexNumber) => sprite.GetTerrain(hexNumber) >= 0 || sprite.GetOverlay(hexNumber) >= 0);
    public HexSprite GetMaxLevelTerrainHexSprite() => FindTop((sprite, hexNumber) => sprite.GetTerrain(hexNumber) >= 0);
    public HexSprite GetMaxLevelOverlayHexSprite() => FindTop((sprite, hexNumber) => sprite.GetOverlay(hexNumber) >= 0);

    private (sbyte Terrain, sbyte Overlay) ReadTerrain()
    {
        sbyte terrain = -1, overlay = -1;
        for (int i = StoredCount - 1; i >= 0; i--)
        {
            if (terrain < 0)
            {
                terrain = _hexSpriteList[i].GetTerrain(_directions[i]);
            }

            if (overlay < 0)
            {
                overlay = _hexSpriteList[i].GetOverlay(_directions[i]);
            }
        }
        return (Math.Max(terrain, (sbyte)-1), Math.Max(overlay, (sbyte)-1));
    }

    public void UpdateTerrain()
    {
        if (_status.Contains(MapFieldStatusElement.TerrainLocked))
        {
            return;
        }

        (sbyte terrain, sbyte overlay) = ReadTerrain();
        if (terrain == _terrainType && overlay == _overlayType)
        {
            return;
        }

        _terrainType = terrain;
        _overlayType = overlay;
        TerrainChanged();
    }

    public void UpdateTerrainType()
    {
        if (_status.Contains(MapFieldStatusElement.TerrainLocked))
        {
            return;
        }

        sbyte terrain = -1;
        for (int i = StoredCount - 1; i >= 0 && terrain < 0; i--)
        {
            terrain = _hexSpriteList[i].GetTerrain(_directions[i]);
        }

        terrain = Math.Max(terrain, (sbyte)-1);
        if (terrain == _terrainType)
        {
            return;
        }

        _terrainType = terrain;
        TerrainChanged();
    }

    public void UpdateOverlayType()
    {
        if (_status.Contains(MapFieldStatusElement.TerrainLocked))
        {
            return;
        }

        sbyte overlay = -1;
        for (int i = StoredCount - 1; i >= 0 && overlay < 0; i--)
        {
            overlay = _hexSpriteList[i].GetOverlay(_directions[i]);
        }

        overlay = Math.Max(overlay, (sbyte)-1);
        if (overlay == _overlayType)
        {
            return;
        }

        _overlayType = overlay;
        TerrainChanged();
    }

    public void TerrainChanged()
    {
        Set<MapFieldStatusElement> locked = Set<MapFieldStatusElement>.Of(MapFieldStatusElement.TerrainLocked);
        if (_status.Contains(MapFieldStatusElement.TerrainLocked)) { _status += locked; return; }
        _status += locked;
        HexSpriteTerrainInfo info = new() { MapField = this, TerrainType = _terrainType, OverlayType = _overlayType };
        for (int i = StoredCount - 1; i >= 0; i--)
        {
            info.HexNumber = (sbyte)_directions[i];
            _hexSpriteList[i].ChangeTerrain(info);
        }
        _terrainType = -1;
        _overlayType = -1;
        (_terrainType, _overlayType) = ReadTerrain();
        _status -= locked;
        info.TerrainType = _terrainType;
        info.OverlayType = _overlayType;
        for (int i = StoredCount - 1; i >= 0; i--)
        {
            info.HexNumber = (sbyte)_directions[i];
            _hexSpriteList[i].TerrainChanged(info);
        }
        _status += locked;
        UpdateNeighbourTerrain();
        _status -= locked;
    }

    public void NeighbourTerrainChanged(ref HexSpriteTerrainInfo info)
    {
        for (int i = StoredCount - 1; i >= 0; i--)
        {
            _hexSpriteList[i].NeighbourTerrainChanged(_directions[i], info);
        }
    }

    public bool CanChangeTerrain(EngineObject sender)
    {
        if (_status.Contains(MapFieldStatusElement.TerrainLocked))
        {
            return false;
        }

        HexSpriteSenderInfo info = new() { MapField = this, Sender = sender };
        for (int i = StoredCount - 1; i >= 0; i--)
        {
            info.HexNumber = (sbyte)_directions[i];
            if (!_hexSpriteList[i].CanChangeTerrain(info))
            {
                return false;
            }
        }
        return true;
    }

    public bool CanChangeTerrain(EngineObject sender, sbyte terrain, sbyte overlay)
    {
        if (_status.Contains(MapFieldStatusElement.TerrainLocked))
        {
            return false;
        }

        sbyte oldTerrain = _terrainType, oldOverlay = _overlayType;
        if (terrain >= 0)
        {
            _terrainType = terrain;
        }

        if (overlay >= 0)
        {
            _overlayType = overlay;
        }

        bool result = CanChangeTerrain(sender);
        if (terrain >= 0)
        {
            _terrainType = oldTerrain;
        }

        if (overlay >= 0)
        {
            _overlayType = oldOverlay;
        }

        return result;
    }

    // A handler answers by changing Result, and the first answer ends the delivery. The test
    // compares the slot as a MsgArg, whose equality is that of the original word: an int or a
    // reference answer stops the loop alike, and a handler that writes 0 into an empty slot has
    // not answered.
    public void SendMapFieldMessage(ref MapFieldMessage message)
    {
        MsgArg oldResult = message.Result;
        int i = StoredCount;
        message.MapField = this;
        while (i != 0 && oldResult == message.Result)
        {
            i--;
            if (i >= Count)
            {
                continue;
            }

            message.HexNumber = _directions[i];
            _hexSpriteList[i].MapFieldMessageProc(ref message);
        }
    }

    public int CanMoveOn(object sender, int defaultCode, int moveOverCode)
    {
        HexSpriteMoveOnInfo info = new() { Sender = sender, MapField = this, MoveOverCode = moveOverCode };
        HexSpriteMoveOnResult result = new() { Code = defaultCode, Done = false, Data = 0 };
        int i = StoredCount;
        while (!result.Done && i != 0)
        {
            i--;
            info.HexNumber = (sbyte)_directions[i];
            _hexSpriteList[i].CanMoveOn(info, ref result);
        }
        return result.Code;
    }

    public int CanMoveOver(object sender)
    {
        HexSpriteSenderInfo info = new() { Sender = sender, MapField = this };
        int result = 1, i = StoredCount;
        while (result == 1 && i != 0)
        {
            i--;
            info.HexNumber = (sbyte)_directions[i];
            result = _hexSpriteList[i].CanMoveOver(info);
        }
        return result;
    }

    public void MoveExclusive(object sender)
    {
        HexSpriteSenderInfo info = new() { Sender = sender, MapField = this };
        for (int i = StoredCount - 1; i >= 0; i--)
        {
            info.HexNumber = (sbyte)_directions[i];
            _hexSpriteList[i].MoveExclusive(info);
        }
    }

    public HexSprite FindNoneTransparentHexSprite(Type type)
    {
        for (int i = 0; i < StoredCount; i++)
        {
            if (type.IsInstanceOfType(_hexSpriteList[i]) && _hexSpriteList[i].GetTerrain(_directions[i]) >= 0)
            {
                return _hexSpriteList[i];
            }
        }

        return null;
    }

    // The type selects the sprite, the same test typeof used. Another class throws, and null stays null.
    public T FindNoneTransparentHexSprite<T>() where T : HexSprite => (T)FindNoneTransparentHexSprite(typeof(T));

    public HexSprite FindNoneTransparentHexSpriteClassId(int classId)
    {
        for (int i = 0; i < StoredCount; i++)
        {
            if (_hexSpriteList[i].EngineClass.ClassId == classId && _hexSpriteList[i].GetTerrain(_directions[i]) >= 0)
            {
                return _hexSpriteList[i];
            }
        }

        return null;
    }
}
