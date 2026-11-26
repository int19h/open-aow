#nullable disable warnings
using Aow.Engine.Nodes;
using Aow.Game.Draw;

using Aow.Game.World.Movement;
using Aow.Rtl;

namespace Aow.Game.World.Map;

public partial class MapField
{
    private byte[] _directions = [];

    public void Resize(int capacity)
    {
        if (capacity is > 0 and < 3)
        {
            capacity = 3;
        }

        Array.Resize(ref _hexSpriteList, newSize: capacity);
        Array.Resize(ref _directions, newSize: capacity);
        Capacity = capacity;
    }

    public int GetHexNumber(int index) => index >= StoredCount ? throw new IndexOutOfRangeException("Index out of bounds") : _directions[index];

    public void SetHexNumber(int index, int value)
    {
        if (index >= StoredCount)
        {
            throw new IndexOutOfRangeException("Index out of bounds");
        }

        _directions[index] = (byte)value;
    }

    public int AddHexSprite(HexSprite? sprite, int hexNumber)
    {
        try
        {
            if (sprite is null)
            {
                throw new ArgumentException("No nil HS allowed in TMapField");
            }

            if (FindChildIndex(sprite) != -1)
            {
                throw new InvalidOperationException("HS already defined");
            }
        }
        catch
        {
        }
        if (Capacity == StoredCount)
        {
            Resize((Capacity + 1).TruncateToByte());
        }

        int priority = sprite.GetShowPriority();
        int index = Status.Contains(MapFieldStatuses.Comparing) ? StoredCount : 0;
        if (!Status.Contains(MapFieldStatuses.Comparing))
        {
            while (index < StoredCount && priority > _hexSpriteList[index].GetShowPriority())
            {
                index++;
            }
        }

        if (index < StoredCount)
        {
            Array.Copy(_hexSpriteList, sourceIndex: index, destinationArray: _hexSpriteList, destinationIndex: index + 1, length: StoredCount - index);
            Array.Copy(_directions, sourceIndex: index, destinationArray: _directions, destinationIndex: index + 1, length: StoredCount - index);
        }
        if (priority >>> 26 <= 15)
        {
            StaticCount = (StaticCount + 1).TruncateToByte();
        }

        _hexSpriteList[index] = sprite;
        _directions[index] = (byte)hexNumber;
        StoredCount = (StoredCount + 1).TruncateToByte();
        return index;
    }

    public int RemoveHexSprite(HexSprite? sprite)
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

        StoredCount = (StoredCount - 1).TruncateToByte();
        if (index < StoredCount)
        {
            Array.Copy(_directions, sourceIndex: index + 1, destinationArray: _directions, destinationIndex: index, length: StoredCount - index);
            Array.Copy(_hexSpriteList, sourceIndex: index + 1, destinationArray: _hexSpriteList, destinationIndex: index, length: StoredCount - index);
        }
        if (sprite.GetShowPriority() >>> 26 <= 15)
        {
            if (StaticCount != 0)
            {
                StaticCount = (StaticCount - 1).TruncateToByte();
            }
            else
            {
                try
                {
                    throw new ArgumentException($"Invalid StaticCount ({sprite.GetType().Name})");
                }
                catch
                {
                }
            }
        }
        return StoredCount;
    }

    public void Remove(int index)
    {
        StoredCount = (StoredCount - 1).TruncateToByte();
        if (index < StoredCount)
        {
            Array.Copy(_directions, sourceIndex: index + 1, destinationArray: _directions, destinationIndex: index, length: StoredCount - index);
            Array.Copy(_hexSpriteList, sourceIndex: index + 1, destinationArray: _hexSpriteList, destinationIndex: index, length: StoredCount - index);
        }
        if (Capacity > StoredCount + 1)
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
        StaticCount = 0;
        while (StaticCount != StoredCount && _hexSpriteList[StaticCount].GetShowPriority() >>> 26 <= 15)
        {
            StaticCount = (StaticCount + 1).TruncateToByte();
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
        OverlayType? overlay = null;
        TerrainType? terrain = null;
        for (int i = StoredCount - 1; i >= 0; i--)
        {
            terrain ??= _hexSpriteList[i].GetTerrain(_directions[i]).Value;
            overlay ??= _hexSpriteList[i].GetOverlay(_directions[i]).Value;
        }
        OverlayType = overlay;
        TerrainType = terrain;
        HexSpriteTerrainInfo info = new() { MapField = this, TerrainType = TerrainType, OverlayType = OverlayType };
        for (int i = StoredCount - 1; i >= 0; i--)
        {
            info.HexNumber = (sbyte)_directions[i];
            _hexSpriteList[i].TerrainChanged(info);
        }
        UpdateMapField();
    }

    public int SetHexNumberOfHexSprite(HexSprite sprite, int hexNumber)
    {
        int index = FindChildIndex(sprite);
        if (index != -1)
        {
            _directions[index] = (byte)hexNumber;
        }

        return index;
    }

    public int GetHexNumberOfHexSprite(HexSprite sprite)
    {
        int index = FindChildIndex(sprite);
        return index == -1 ? -1 : _directions[index];
    }

    private HexSprite? FindTop(Func<HexSprite?, byte, bool> matches)
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

    public HexSprite? GetMaxLevelNoneTransparentHexSprite() => FindTop((sprite, hexNumber) => sprite.GetTerrain(hexNumber).Value is not null || sprite.GetOverlay(hexNumber).Value is not null);
    public HexSprite? GetMaxLevelTerrainHexSprite() => FindTop((sprite, hexNumber) => sprite.GetTerrain(hexNumber).Value is not null);
    public HexSprite? GetMaxLevelOverlayHexSprite() => FindTop((sprite, hexNumber) => sprite.GetOverlay(hexNumber).Value is not null);

    // The terrain and the overlay of the topmost sprites that give one. A sprite that answers outside or
    // keep leaves the search going.
    private (TerrainType? Terrain, OverlayType? Overlay) ReadTerrain()
    {
        TerrainType? terrain = null;
        OverlayType? overlay = null;
        for (int i = StoredCount - 1; i >= 0; i--)
        {
            terrain ??= _hexSpriteList[i].GetTerrain(_directions[i]).Value;
            overlay ??= _hexSpriteList[i].GetOverlay(_directions[i]).Value;
        }
        return (terrain, overlay);
    }

    public void UpdateTerrain()
    {
        if (Status.Contains(MapFieldStatuses.TerrainLocked))
        {
            return;
        }

        (TerrainType? terrain, OverlayType? overlay) = ReadTerrain();
        if (terrain == TerrainType && overlay == OverlayType)
        {
            return;
        }

        TerrainType = terrain;
        OverlayType = overlay;
        TerrainChanged();
    }

    public void UpdateTerrainType()
    {
        if (Status.Contains(MapFieldStatuses.TerrainLocked))
        {
            return;
        }

        TerrainType? terrain = null;
        for (int i = StoredCount - 1; i >= 0 && terrain is null; i--)
        {
            terrain = _hexSpriteList[i].GetTerrain(_directions[i]).Value;
        }

        if (terrain == TerrainType)
        {
            return;
        }

        TerrainType = terrain;
        TerrainChanged();
    }

    public void UpdateOverlayType()
    {
        if (Status.Contains(MapFieldStatuses.TerrainLocked))
        {
            return;
        }

        OverlayType? overlay = null;
        for (int i = StoredCount - 1; i >= 0 && overlay is null; i--)
        {
            overlay = _hexSpriteList[i].GetOverlay(_directions[i]).Value;
        }

        if (overlay == OverlayType)
        {
            return;
        }

        OverlayType = overlay;
        TerrainChanged();
    }

    public void TerrainChanged()
    {
        MapFieldStatuses locked = MapFieldStatuses.TerrainLocked;
        if (Status.Contains(MapFieldStatuses.TerrainLocked))
        {
            Status |= locked;
            return;
        }
        Status |= locked;
        HexSpriteTerrainInfo info = new() { MapField = this, TerrainType = TerrainType, OverlayType = OverlayType };
        for (int i = StoredCount - 1; i >= 0; i--)
        {
            info.HexNumber = (sbyte)_directions[i];
            _hexSpriteList[i].ChangeTerrain(info);
        }
        TerrainType = null;
        OverlayType = null;
        (TerrainType, OverlayType) = ReadTerrain();
        Status &= ~locked;
        info.TerrainType = TerrainType;
        info.OverlayType = OverlayType;
        for (int i = StoredCount - 1; i >= 0; i--)
        {
            info.HexNumber = (sbyte)_directions[i];
            _hexSpriteList[i].TerrainChanged(info);
        }
        Status |= locked;
        UpdateNeighbourTerrain();
        Status &= ~locked;
    }

    public void NeighbourTerrainChanged(ref readonly HexSpriteTerrainInfo info)
    {
        for (int i = StoredCount - 1; i >= 0; i--)
        {
            _hexSpriteList[i].NeighbourTerrainChanged(_directions[i], info);
        }
    }

    public bool CanChangeTerrain(EngineObject? sender)
    {
        if (Status.Contains(MapFieldStatuses.TerrainLocked))
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

    // Asks the sprites whether the field can take a footprint's answers. A covered hexagon that keeps
    // the field's value changes nothing.
    public bool CanChangeTerrain(EngineObject sender, FootprintCell<TerrainType> terrain, FootprintCell<OverlayType> overlay)
    {
        if (Status.Contains(MapFieldStatuses.TerrainLocked))
        {
            return false;
        }

        TerrainType? oldTerrain = TerrainType;
        OverlayType? oldOverlay = OverlayType;
        if (terrain.Value is { } newTerrain)
        {
            TerrainType = newTerrain;
        }

        if (overlay.Value is { } newOverlay)
        {
            OverlayType = newOverlay;
        }

        bool result = CanChangeTerrain(sender);
        if (terrain.Value is not null)
        {
            TerrainType = oldTerrain;
        }

        if (overlay.Value is not null)
        {
            OverlayType = oldOverlay;
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
            _hexSpriteList[i].CanMoveOn(info, moveResult: ref result);
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

    public HexSprite? FindNoneTransparentHexSprite(Type type)
    {
        for (int i = 0; i < StoredCount; i++)
        {
            if (type.IsInstanceOfType(_hexSpriteList[i]) && _hexSpriteList[i].GetTerrain(_directions[i]).Value is not null)
            {
                return _hexSpriteList[i];
            }
        }

        return null;
    }

    // The type selects the sprite, the same test typeof used. Another class throws, and null stays null.
    public T FindNoneTransparentHexSprite<T>() where T : HexSprite => (T)FindNoneTransparentHexSprite(typeof(T));

    public HexSprite? FindNoneTransparentHexSpriteClassId(int classId)
    {
        for (int i = 0; i < StoredCount; i++)
        {
            if (_hexSpriteList[i].EngineClass.ClassId == classId && _hexSpriteList[i].GetTerrain(_directions[i]).Value is not null)
            {
                return _hexSpriteList[i];
            }
        }

        return null;
    }
}
