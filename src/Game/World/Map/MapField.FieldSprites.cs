using Aow.Engine.Nodes;
using Aow.Game.Draw;

using Aow.Game.World.Movement;
using Aow.Rtl;

namespace Aow.Game.World.Map;

public partial class MapField
{
    private byte[] directions = [];

    public void Resize(byte capacity)
    {
        if (capacity is > 0 and < 3) capacity = 3;
        Array.Resize(ref FHSList, capacity);
        Array.Resize(ref directions, capacity);
        FCapacity = capacity;
    }

    public byte GetHN(int index)
    {
        if (index >= FCount) throw new IndexOutOfRangeException("Index out of bounds");
        return directions[index];
    }

    public void SetHN(int index, byte value)
    {
        if (index >= FCount) throw new IndexOutOfRangeException("Index out of bounds");
        directions[index] = value;
    }

    public int AddHS(HexagonSprite sprite, int hn)
    {
        try
        {
            if (sprite == null) throw new ArgumentException("No nil HS allowed in TMapField");
            if (FindChildIndex(sprite) != -1) throw new InvalidOperationException("HS already defined");
        }
        catch { }
        if (FCapacity == FCount) Resize((byte)(FCapacity + 1));
        int priority = sprite.GetShowPriority();
        int index = FStatus.Contains(MapFieldStatusElement.Comparing) ? FCount : 0;
        if (!FStatus.Contains(MapFieldStatusElement.Comparing))
            while (index < FCount && priority > FHSList[index].GetShowPriority()) index++;
        if (index < FCount)
        {
            Array.Copy(FHSList, index, FHSList, index + 1, FCount - index);
            Array.Copy(directions, index, directions, index + 1, FCount - index);
        }
        if ((uint)priority >> 26 <= 15) FStaticCount++;
        FHSList[index] = sprite;
        directions[index] = (byte)hn;
        FCount++;
        return index;
    }

    public int RemoveHS(HexagonSprite sprite)
    {
        int index = 0;
        while (index < FCount && sprite != FHSList[index]) index++;
        if (index == FCount) return -1;
        FCount--;
        if (index < FCount)
        {
            Array.Copy(directions, index + 1, directions, index, FCount - index);
            Array.Copy(FHSList, index + 1, FHSList, index, FCount - index);
        }
        if ((uint)sprite.GetShowPriority() >> 26 <= 15)
        {
            if (FStaticCount != 0) FStaticCount--;
            else
            {
                try { throw new ArgumentException($"Invalid StaticCount ({sprite.GetType().Name})"); }
                catch { }
            }
        }
        return FCount;
    }

    public void Remove(int index)
    {
        FCount--;
        if (index < FCount)
        {
            Array.Copy(directions, index + 1, directions, index, FCount - index);
            Array.Copy(FHSList, index + 1, FHSList, index, FCount - index);
        }
        if (FCapacity > FCount + 1) Resize(FCount);
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
                if (FHSList[index].GetShowPriority() > FHSList[index + 1].GetShowPriority())
                {
                    changed = true;
                    (FHSList[index], FHSList[index + 1]) = (FHSList[index + 1], FHSList[index]);
                    (directions[index], directions[index + 1]) = (directions[index + 1], directions[index]);
                }
                index--;
            }
        }
        FStaticCount = 0;
        while (FStaticCount != FCount && (uint)FHSList[FStaticCount].GetShowPriority() >> 26 <= 15) FStaticCount++;
    }

    public void Loaded()
    {
        int count = FCount;
        for (int i = 0; i < count; i++) LoadedList[i] = FHSList[i];
        for (int i = count - 1; i >= 0; i--)
            if (LoadedList[i].Owner == this && !LoadedList[i].MainLoaded()) LoadedList[i]?.Dispose();
    }

    public virtual void UpdateMapField()
    {
        HSMapFieldInfo info = new() { MapField = this };
        for (int i = FCount - 1; i >= 0; i--)
        {
            info.HN = (sbyte)directions[i];
            FHSList[i].UpdateMapField(info);
        }
    }

    public void Initialize()
    {
        sbyte overlay = -1, terrain = -1;
        for (int i = FCount - 1; i >= 0; i--)
        {
            if (terrain < 0) terrain = FHSList[i].GetTerrain(directions[i]);
            if (overlay < 0) overlay = FHSList[i].GetOverlay(directions[i]);
        }
        FOverlayType = Math.Max(overlay, (sbyte)-1);
        FTerrainType = Math.Max(terrain, (sbyte)-1);
        HSTerrainInfo info = new() { MapField = this, TerrainType = FTerrainType, OverlayType = FOverlayType };
        for (int i = FCount - 1; i >= 0; i--)
        {
            info.HN = (sbyte)directions[i];
            FHSList[i].TerrainChanged(info);
        }
        UpdateMapField();
    }

    public int SetHNofHS(HexagonSprite sprite, byte hn)
    {
        int index = FindChildIndex(sprite);
        if (index != -1) directions[index] = hn;
        return index;
    }

    public int GetHNofHS(HexagonSprite sprite)
    {
        int index = FindChildIndex(sprite);
        return index == -1 ? -1 : directions[index];
    }

    private HexagonSprite FindTop(Func<HexagonSprite, byte, bool> matches)
    {
        for (int i = FCount - 1; i >= 0; i--)
            if (matches(FHSList[i], directions[i])) return FHSList[i];
        return null;
    }

    public HexagonSprite GetMaxLevelNoneTransparentHS() => FindTop((sprite, hn) => sprite.GetTerrain(hn) >= 0 || sprite.GetOverlay(hn) >= 0);
    public HexagonSprite GetMaxLevelTerrainHS() => FindTop((sprite, hn) => sprite.GetTerrain(hn) >= 0);
    public HexagonSprite GetMaxLevelOverlayHS() => FindTop((sprite, hn) => sprite.GetOverlay(hn) >= 0);

    private (sbyte Terrain, sbyte Overlay) ReadTerrain()
    {
        sbyte terrain = -1, overlay = -1;
        for (int i = FCount - 1; i >= 0; i--)
        {
            if (terrain < 0) terrain = FHSList[i].GetTerrain(directions[i]);
            if (overlay < 0) overlay = FHSList[i].GetOverlay(directions[i]);
        }
        return (Math.Max(terrain, (sbyte)-1), Math.Max(overlay, (sbyte)-1));
    }

    public void UpdateTerrain()
    {
        if (FStatus.Contains(MapFieldStatusElement.TerrainLocked)) return;
        var (terrain, overlay) = ReadTerrain();
        if (terrain == FTerrainType && overlay == FOverlayType) return;
        FTerrainType = terrain;
        FOverlayType = overlay;
        TerrainChanged();
    }

    public void UpdateTerrainType()
    {
        if (FStatus.Contains(MapFieldStatusElement.TerrainLocked)) return;
        sbyte terrain = -1;
        for (int i = FCount - 1; i >= 0 && terrain < 0; i--) terrain = FHSList[i].GetTerrain(directions[i]);
        terrain = Math.Max(terrain, (sbyte)-1);
        if (terrain == FTerrainType) return;
        FTerrainType = terrain;
        TerrainChanged();
    }

    public void UpdateOverlayType()
    {
        if (FStatus.Contains(MapFieldStatusElement.TerrainLocked)) return;
        sbyte overlay = -1;
        for (int i = FCount - 1; i >= 0 && overlay < 0; i--) overlay = FHSList[i].GetOverlay(directions[i]);
        overlay = Math.Max(overlay, (sbyte)-1);
        if (overlay == FOverlayType) return;
        FOverlayType = overlay;
        TerrainChanged();
    }

    public void TerrainChanged()
    {
        var locked = Set<MapFieldStatusElement>.Of(MapFieldStatusElement.TerrainLocked);
        if (FStatus.Contains(MapFieldStatusElement.TerrainLocked)) { FStatus += locked; return; }
        FStatus += locked;
        HSTerrainInfo info = new() { MapField = this, TerrainType = FTerrainType, OverlayType = FOverlayType };
        for (int i = FCount - 1; i >= 0; i--)
        {
            info.HN = (sbyte)directions[i];
            FHSList[i].ChangeTerrain(info);
        }
        FTerrainType = -1;
        FOverlayType = -1;
        (FTerrainType, FOverlayType) = ReadTerrain();
        FStatus -= locked;
        info.TerrainType = FTerrainType;
        info.OverlayType = FOverlayType;
        for (int i = FCount - 1; i >= 0; i--)
        {
            info.HN = (sbyte)directions[i];
            FHSList[i].TerrainChanged(info);
        }
        FStatus += locked;
        UpdateNeighbourTerrain();
        FStatus -= locked;
    }

    public void NeighbourTerrainChanged(ref HSTerrainInfo info)
    {
        for (int i = FCount - 1; i >= 0; i--) FHSList[i].NeighbourTerrainChanged(directions[i], info);
    }

    public bool CanChangeTerrainEx(EObject sender)
    {
        if (FStatus.Contains(MapFieldStatusElement.TerrainLocked)) return false;
        HSSenderInfo info = new() { MapField = this, Sender = sender };
        for (int i = FCount - 1; i >= 0; i--)
        {
            info.HN = (sbyte)directions[i];
            if (!FHSList[i].CanChangeTerrain(info)) return false;
        }
        return true;
    }

    public bool CanChangeTerrain(EObject sender, sbyte terrain, sbyte overlay)
    {
        if (FStatus.Contains(MapFieldStatusElement.TerrainLocked)) return false;
        sbyte oldTerrain = FTerrainType, oldOverlay = FOverlayType;
        if (terrain >= 0) FTerrainType = terrain;
        if (overlay >= 0) FOverlayType = overlay;
        bool result = CanChangeTerrainEx(sender);
        if (terrain >= 0) FTerrainType = oldTerrain;
        if (overlay >= 0) FOverlayType = oldOverlay;
        return result;
    }

    // A handler answers by changing Result, and the first answer ends the delivery. The test
    // compares the slot as a MsgArg, whose equality is that of the original word: an int or a
    // reference answer stops the loop alike, and a handler that writes 0 into an empty slot has
    // not answered.
    public void SendMapFieldMsg(ref MapFieldMsg message)
    {
        MsgArg oldResult = message.Result;
        int i = FCount;
        message.MapField = this;
        while (i != 0 && oldResult == message.Result)
        {
            i--;
            if (i >= Count) continue;
            message.HN = directions[i];
            FHSList[i].MapFieldMsgProc(ref message);
        }
    }

    public int CanMoveOn(object sender, int defaultCode, int moveOverCode)
    {
        HSMoveOnInfo info = new() { Sender = sender, MapField = this, MoveOverCode = moveOverCode };
        HSMoveOnResult result = new() { Code = defaultCode, Done = false, Data = 0 };
        int i = FCount;
        while (!result.Done && i != 0)
        {
            i--;
            info.HN = (sbyte)directions[i];
            FHSList[i].CanMoveOn(info, ref result);
        }
        return result.Code;
    }

    public int CanMoveOver(object sender)
    {
        HSSenderInfo info = new() { Sender = sender, MapField = this };
        int result = 1, i = FCount;
        while (result == 1 && i != 0)
        {
            i--;
            info.HN = (sbyte)directions[i];
            result = FHSList[i].CanMoveOver(info);
        }
        return result;
    }

    public void MoveExclusive(object sender)
    {
        HSSenderInfo info = new() { Sender = sender, MapField = this };
        for (int i = FCount - 1; i >= 0; i--)
        {
            info.HN = (sbyte)directions[i];
            FHSList[i].MoveExclusive(info);
        }
    }

    public HexagonSprite FindNoneTransparentHS(Type type)
    {
        for (int i = 0; i < FCount; i++)
            if (type.IsInstanceOfType(FHSList[i]) && FHSList[i].GetTerrain(directions[i]) >= 0) return FHSList[i];
        return null;
    }

    public HexagonSprite FindNoneTransparentHSCID(int classId)
    {
        for (int i = 0; i < FCount; i++)
            if (FHSList[i].EClass.ClassID == classId && FHSList[i].GetTerrain(directions[i]) >= 0) return FHSList[i];
        return null;
    }
}
