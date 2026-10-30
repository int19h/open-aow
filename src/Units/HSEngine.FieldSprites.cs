using Aow.Rtl;

namespace Aow.Units.HSEngine;

public partial class TMapField
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
        if (index >= FCount) throw Rtl.Exception.Create("Index out of bounds");
        return directions[index];
    }

    public void SetHN(int index, byte value)
    {
        if (index >= FCount) throw Rtl.Exception.Create("Index out of bounds");
        directions[index] = value;
    }

    public int AddHS(THexagonSprite sprite, int hn)
    {
        try
        {
            if (sprite == null) throw Rtl.Exception.Create("No nil HS allowed in TMapField");
            if (FindChildIndex(sprite) != -1) throw Rtl.Exception.Create("HS already defined");
        }
        catch (System.Exception) { }
        if (FCapacity == FCount) Resize(unchecked((byte)(FCapacity + 1)));
        int priority = sprite.GetShowPriority();
        int index = FStatus.Contains(TMapFieldStatusElement.msComparing) ? FCount : 0;
        if (!FStatus.Contains(TMapFieldStatusElement.msComparing))
            while (index < FCount && priority > FHSList[index].GetShowPriority()) index++;
        if (index < FCount)
        {
            Array.Copy(FHSList, index, FHSList, index + 1, FCount - index);
            Array.Copy(directions, index, directions, index + 1, FCount - index);
        }
        if ((uint)priority >> 26 <= 15) FStaticCount++;
        FHSList[index] = sprite;
        directions[index] = unchecked((byte)hn);
        FCount++;
        return index;
    }

    public int RemoveHS(THexagonSprite sprite)
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
                try { throw Rtl.Exception.Create($"Invalid StaticCount ({TObject.ClassNameOf(sprite.GetType())})"); }
                catch (System.Exception) { }
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
        for (int i = 0; i < count; i++) HSEngine.LoadedList[i] = FHSList[i];
        for (int i = count - 1; i >= 0; i--)
            if (HSEngine.LoadedList[i].Owner == this && !HSEngine.LoadedList[i].MainLoaded()) HSEngine.LoadedList[i].Free();
    }

    public virtual void UpdateMapField()
    {
        THSMapFieldInfo info = new() { MapField = this };
        for (int i = FCount - 1; i >= 0; i--)
        {
            info.HN = unchecked((sbyte)directions[i]);
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
        THSTerrainInfo info = new() { MapField = this, TerrainType = FTerrainType, OverlayType = FOverlayType };
        for (int i = FCount - 1; i >= 0; i--)
        {
            info.HN = unchecked((sbyte)directions[i]);
            FHSList[i].TerrainChanged(info);
        }
        UpdateMapField();
    }

    public int SetHNofHS(THexagonSprite sprite, byte hn)
    {
        int index = FindChildIndex(sprite);
        if (index != -1) directions[index] = hn;
        return index;
    }

    public int GetHNofHS(THexagonSprite sprite)
    {
        int index = FindChildIndex(sprite);
        return index == -1 ? -1 : directions[index];
    }

    private THexagonSprite FindTop(Func<THexagonSprite, byte, bool> matches)
    {
        for (int i = FCount - 1; i >= 0; i--)
            if (matches(FHSList[i], directions[i])) return FHSList[i];
        return null;
    }

    public THexagonSprite GetMaxLevelNoneTransparentHS() => FindTop((sprite, hn) => sprite.GetTerrain(hn) >= 0 || sprite.GetOverlay(hn) >= 0);
    public THexagonSprite GetMaxLevelTerrainHS() => FindTop((sprite, hn) => sprite.GetTerrain(hn) >= 0);
    public THexagonSprite GetMaxLevelOverlayHS() => FindTop((sprite, hn) => sprite.GetOverlay(hn) >= 0);

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
        if (FStatus.Contains(TMapFieldStatusElement.msTerrainLocked)) return;
        var (terrain, overlay) = ReadTerrain();
        if (terrain == FTerrainType && overlay == FOverlayType) return;
        FTerrainType = terrain;
        FOverlayType = overlay;
        TerrainChanged();
    }

    public void UpdateTerrainType()
    {
        if (FStatus.Contains(TMapFieldStatusElement.msTerrainLocked)) return;
        sbyte terrain = -1;
        for (int i = FCount - 1; i >= 0 && terrain < 0; i--) terrain = FHSList[i].GetTerrain(directions[i]);
        terrain = Math.Max(terrain, (sbyte)-1);
        if (terrain == FTerrainType) return;
        FTerrainType = terrain;
        TerrainChanged();
    }

    public void UpdateOverlayType()
    {
        if (FStatus.Contains(TMapFieldStatusElement.msTerrainLocked)) return;
        sbyte overlay = -1;
        for (int i = FCount - 1; i >= 0 && overlay < 0; i--) overlay = FHSList[i].GetOverlay(directions[i]);
        overlay = Math.Max(overlay, (sbyte)-1);
        if (overlay == FOverlayType) return;
        FOverlayType = overlay;
        TerrainChanged();
    }

    public void TerrainChanged()
    {
        var locked = TSet<TMapFieldStatusElement>.Of(TMapFieldStatusElement.msTerrainLocked);
        if (FStatus.Contains(TMapFieldStatusElement.msTerrainLocked)) { FStatus += locked; return; }
        FStatus += locked;
        THSTerrainInfo info = new() { MapField = this, TerrainType = FTerrainType, OverlayType = FOverlayType };
        for (int i = FCount - 1; i >= 0; i--)
        {
            info.HN = unchecked((sbyte)directions[i]);
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
            info.HN = unchecked((sbyte)directions[i]);
            FHSList[i].TerrainChanged(info);
        }
        FStatus += locked;
        UpdateNeighbourTerrain();
        FStatus -= locked;
    }

    public void NeighbourTerrainChanged(ref THSTerrainInfo info)
    {
        for (int i = FCount - 1; i >= 0; i--) FHSList[i].NeighbourTerrainChanged(directions[i], info);
    }

    public bool CanChangeTerrainEx(Engine.TEObject sender)
    {
        if (FStatus.Contains(TMapFieldStatusElement.msTerrainLocked)) return false;
        THSSenderInfo info = new() { MapField = this, Sender = sender };
        for (int i = FCount - 1; i >= 0; i--)
        {
            info.HN = unchecked((sbyte)directions[i]);
            if (!FHSList[i].CanChangeTerrain(info)) return false;
        }
        return true;
    }

    public bool CanChangeTerrain(Engine.TEObject sender, sbyte terrain, sbyte overlay)
    {
        if (FStatus.Contains(TMapFieldStatusElement.msTerrainLocked)) return false;
        sbyte oldTerrain = FTerrainType, oldOverlay = FOverlayType;
        if (terrain >= 0) FTerrainType = terrain;
        if (overlay >= 0) FOverlayType = overlay;
        bool result = CanChangeTerrainEx(sender);
        if (terrain >= 0) FTerrainType = oldTerrain;
        if (overlay >= 0) FOverlayType = oldOverlay;
        return result;
    }

    public void SendMapFieldMsg(ref TMapFieldMsg message)
    {
        int oldResult = message.Result.Value;
        int i = FCount;
        message.MapField = this;
        while (i != 0 && oldResult == message.Result.Value)
        {
            i--;
            if (i >= Count) continue;
            message.HN = directions[i];
            FHSList[i].MapFieldMsgProc(ref message);
        }
    }

    public int CanMoveOn(TObject sender, int defaultCode, int moveOverCode)
    {
        THSMoveOnInfo info = new() { Sender = sender, MapField = this, MoveOverCode = moveOverCode };
        THSMoveOnResult result = new() { Code = defaultCode, Done = false, Data = 0 };
        int i = FCount;
        while (!result.Done && i != 0)
        {
            i--;
            info.HN = unchecked((sbyte)directions[i]);
            FHSList[i].CanMoveOn(info, ref result);
        }
        return result.Code;
    }

    public int CanMoveOver(TObject sender)
    {
        THSSenderInfo info = new() { Sender = sender, MapField = this };
        int result = 1, i = FCount;
        while (result == 1 && i != 0)
        {
            i--;
            info.HN = unchecked((sbyte)directions[i]);
            result = FHSList[i].CanMoveOver(info);
        }
        return result;
    }

    public void MoveExclusive(TObject sender)
    {
        THSSenderInfo info = new() { Sender = sender, MapField = this };
        for (int i = FCount - 1; i >= 0; i--)
        {
            info.HN = unchecked((sbyte)directions[i]);
            FHSList[i].MoveExclusive(info);
        }
    }

    public THexagonSprite FindNoneTransparentHS(Type type)
    {
        for (int i = 0; i < FCount; i++)
            if (type.IsInstanceOfType(FHSList[i]) && FHSList[i].GetTerrain(directions[i]) >= 0) return FHSList[i];
        return null;
    }

    public THexagonSprite FindNoneTransparentHSCID(int classId)
    {
        for (int i = 0; i < FCount; i++)
            if (FHSList[i].EClass.ClassID == classId && FHSList[i].GetTerrain(directions[i]) >= 0) return FHSList[i];
        return null;
    }
}
