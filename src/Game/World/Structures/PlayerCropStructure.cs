#nullable disable
// Translated from Crops.pas.
using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Game.Draw;

using Aow.Game.Session;

using Aow.Game.World.Map;

using Aow.Game.World.Terrain;
using Aow.Rtl;

namespace Aow.Game.World.Structures;

// Income, ChangingCrops, IncomeSource
public class PlayerCropStructure : PlayerStructure
{

    public PlayerCropStructure(CustomNode owner) : base(owner)
    {
        _changingCrops = false;
        _incomeSource = new PlayerStructureIncomeSource(null) { Holder = CreationHolder() };
        _incomeSource.Setup(this);
    }

    // Crops.pas:284
    public new const int ClassId = 131_918;

    public override PlayerCropStructureResource Resource => GetResource<PlayerCropStructureResource>();

    public byte Income { get; private set; }

    private bool _changingCrops;
    private readonly PlayerStructureIncomeSource _incomeSource;

    // Crops.pas:278
    public override void Dispose()
    {
        base.Dispose();
        _incomeSource?.Dispose();
    }

    // Crops.pas:528
    public override void NeighbourTerrainChanged(int hexNumber, in HexSpriteTerrainInfo info)
    {
        if (((HexSpriteMapStatuses.Editting | HexSpriteMapStatuses.Activated) & ~StrategicMap.Current.Status) == HexSpriteMapStatuses.None && !_changingCrops)
        {
            UpdateCrops();
        }
    }

    // Crops.pas:534
    public override void Remove()
    {
        if (!EditMode)
        {
            SetCropCount(0);
        }

        base.Remove();
    }

    // Crops.pas:357
    public override void Activate()
    {
        base.Activate();
        sbyte player = Player;
        if (player != -1)
        {
            StrategicMap.Current.Players.GetPlayers(player).RegisterIncomeSource(_incomeSource);
        }

        if (!EditMode)
        {
            LinkToCrops();
            UpdateCrops();
        }
    }

    // Crops.pas:372
    public override void Deactivate()
    {
        sbyte player = Player;
        if (player != -1)
        {
            StrategicMap.Current.Players.GetPlayers(player).UnregisterIncomeSource(_incomeSource);
        }

        base.Deactivate();
    }

    // Crops.pas:560
    public override bool CanPlace(MapContainer mapContainer, sbyte hexX, sbyte hexY, sbyte hexLevel)
    {
        bool result = base.CanPlace(mapContainer, hexX, hexY, hexLevel);
        if (result && !EditMode)
        {
            MapLevel mapLevel = mapContainer.GetMapLevel(hexLevel);
            int i = Resource.CropLocations.Count;
            int x = default;
            int y = default;
            while (result && i != 0)
            {
                i -= 1;
                HexGeometry.CenterHexNumberToHex(hexX, hexY, hexNumber: Resource.CropLocations.List[i], ref x, ref y);
                int hexXLocal = x;
                int hexYLocal = y;
                int hexNumber = 0;
                while (result && hexNumber != 7)
                {
                    if (hexXLocal >= 0 && hexXLocal < mapLevel.Width && hexYLocal >= 0 && hexYLocal < mapLevel.Height)
                    {
                        result = mapLevel.GetField(hexXLocal, hexYLocal).FindNoneTransparentHexSprite(typeof(PlayerCropStructure)) is null;
                    }

                    hexNumber += 1;
                    HexGeometry.CenterHexNumberToHex(x, y, hexNumber, ref hexXLocal, ref hexYLocal);
                }
            }
        }

        return result;
    }

    // Crops.pas:586
    public override bool PlaceHex(MapContainer mapContainer, sbyte hexX, sbyte hexY, sbyte hexLevel)
    {
        bool result = base.PlaceHex(mapContainer, hexX, hexY, hexLevel);
        if (result)
        {
            if (!EditMode)
            {
                UpdateCrops();
            }
        }

        return result;
    }

    // Crops.pas:325
    public override void NewTurn(sbyte player)
    {
        base.NewTurn(player);
        if (Player == player)
        {
            if (CropsEnabled())
            {
                RegrowCrops();
            }
        }
    }

    // Crops.pas:342
    public override void SetPlayer(sbyte player)
    {
        if (player != Player)
        {
            if (Player != -1)
            {
                StrategicMap.Current.Players.GetPlayers(Player).UnregisterIncomeSource(_incomeSource);
            }

            base.SetPlayer(player);
            UpdateCrops();
            if (Player != -1)
            {
                StrategicMap.Current.Players.GetPlayers(Player).RegisterIncomeSource(_incomeSource);
            }
        }
    }

    // Crops.pas:440
    public virtual bool CanPlaceCrop(MapField field)
    {
        bool result;
        if (!GetRazed())
        {
            ArmyHexSprite army = field.GetArmyHexSprite();
            result = army is null || StrategicMap.Current.Players.GetDiplomaticRelation(army.Army.Player, otherPlayer: Player) != DiplomaticRelation.War;
            if (result)
            {
                HexSprite hexSprite = field.GetMaxLevelNoneTransparentHexSprite();
                result = hexSprite is null || hexSprite.Level < 26;
            }

            if (result)
            {
                return field.TerrainType is (>= (sbyte)Map.TerrainType.Grass and <= (sbyte)Map.TerrainType.Steppe) or (sbyte)Map.TerrainType.Dirt || (field.TerrainType == 5 && GetRace() == 11);
            }
        }
        else
        {
            return false;
        }

        return result;
    }

    // Crops.pas:320
    public virtual bool CropsEnabled() => !GetRazed() && !Building;

    // Crops.pas:314
    public virtual void RegrowCrops()
    {
        if (GetCropCount() < Resource.CropLocations.Count)
        {
            SetCropCount(GetCropCount() + 1);
        }
    }

    // Crops.pas:332
    public virtual void UpdateIncome()
    {
        if (!GetRazed() && !Building)
        {
            SetIncome((byte)(GetCropCount() * 2));
        }
        else
        {
            SetIncome(0);
        }
    }

    // Crops.pas:401
    public virtual void UpdateCrops()
    {
        if (!EditMode && Status.Contains(MapObjectStatuses.Activated))
        {
            if (StrategicMap.Current.Status.Contains(HexSpriteMapStatuses.Editting))
            {
                if (CropsEnabled())
                {
                    SetCropCount(Resource.CropLocations.Count);
                }
            }

            UpdateCropSettings();
            UpdateIncome();
        }
    }

    // Crops.pas:413
    public virtual sbyte GetRace() => Player > 0 ? StrategicMap.Current.Players.GetPlayers(Player).PlayerRace : (sbyte)-1;

    // Crops.pas:339
    public virtual void CropsChanged() => UpdateIncome();

    // Crops.pas:290
    public virtual void CropTrampled(sbyte player)
    {
        if (player != Player && Player > 0 && player > 0)
        {
            int race = StrategicMap.Current.Players.GetPlayers(Player).PlayerRace;
            int playerRelationValue = StrategicMap.Current.Races.GetRace(race).GetPlayerRelationValue(player);
            StrategicMap.Current.Races.GetRace(race).SetPlayerRelationValue(player, value: playerRelationValue - -5);
        }
    }

    // Crops.pas:302
    public void SetIncome(byte value)
    {
        if (value != Income && Status.Contains(MapObjectStatuses.Activated))
        {
            Income = value;
            sbyte player = Player;
            if (player != -1)
            {
                StrategicMap.Current.Players.GetPlayers(player).UpdateIncome();
            }
        }
    }

    // Crops.pas:382
    public void UpdateCropSettings()
    {
        int x = default;
        int y = default;
        MapLevel mapLevel = Owner.Owner;
        for (int i = Resource.CropLocations.Count - 1; i >= 0; i--)
        {
            HexGeometry.CenterHexNumberToHex(StoredHexX, StoredHexY, hexNumber: Resource.CropLocations.List[i], ref x, ref y);
            if (x >= 0 && mapLevel.Width > x && y >= 0 && mapLevel.Height > y)
            {
                Crop crop = mapLevel.GetField(x, y).FindOwnedChild<Crop>(Crop.ClassId);
                crop?.UpdateSettings();
            }
        }
    }

    // Crops.pas:420
    public int GetCropCount()
    {
        int x = default;
        int y = default;
        int result = 0;
        MapLevel mapLevel = Owner.Owner;
        for (int i = Resource.CropLocations.Count - 1; i >= 0; i--)
        {
            HexGeometry.CenterHexNumberToHex(StoredHexX, StoredHexY, hexNumber: Resource.CropLocations.List[i], ref x, ref y);
            if (x >= 0 && mapLevel.Width > x && y >= 0 && mapLevel.Height > y)
            {
                Crop crop = mapLevel.GetField(x, y).FindChild<Crop>(Crop.ClassId);
                if (crop is not null)
                {
                    if (crop.CropOwner == this)
                    {
                        result += 1;
                    }
                }
            }
        }

        return result;
    }

    // Crops.pas:463
    public void SetCropCount(int value)
    {
        if (Status.Contains(MapObjectStatuses.PlacedOnMap) && !_changingCrops)
        {
            _changingCrops = true;
            MapLevel mapLevel = Owner.Owner;
            DelphiList<Crop> crops = new();
            UInt16List locations = new();
            locations.Assign(Resource.CropLocations);
            int i = locations.Count - 1;
            int x = default;
            int y = default;
            Crop crop;
            for (; i >= 0; i -= 1)
            {
                HexGeometry.CenterHexNumberToHex(StoredHexX, StoredHexY, hexNumber: locations.List[i], ref x, ref y);
                if (x >= 0 && x < mapLevel.Width && y >= 0 && y < mapLevel.Height)
                {
                    MapField field = mapLevel.GetField(x, y);
                    crop = field.FindOwnedChild<Crop>(Crop.ClassId);
                    if (crop is not null || !CanPlaceCrop(field))
                    {
                        if (crop is not null && crop.CropOwner == this)
                        {
                            _ = crops.Add(crop);
                        }

                        locations.Delete(i);
                    }
                }
                else
                {
                    locations.Delete(i);
                }
            }

            if (crops.Count < value)
            {
                Crop.Resource resource = Resource.Engine.HexSpriteSet.MapObjectResourceList.FindChild<Crop.Resource>(Crop.Resource.ClassId);
                if (resource is not null)
                {
                    while (crops.Count != value && locations.Count != 0)
                    {
                        i = StrategicMap.Current.Random(locations.Count);
                        HexGeometry.CenterHexNumberToHex(StoredHexX, StoredHexY, hexNumber: locations.List[i], ref x, ref y);
                        crop = resource.PlaceCrop((sbyte)x, (sbyte)y, L: (sbyte)mapLevel.HexLevel, cropOwner: this);
                        if (crop is not null)
                        {
                            _ = crops.Add(crop);
                            crop.SetCropOwner(this);
                        }

                        locations.Delete(i);
                    }
                }
            }
            else
            {
                if (crops.Count > value)
                {
                    while (crops.Count != value && crops.Count != 0)
                    {
                        i = StrategicMap.Current.Random(crops.Count);
                        ((crops[i] ?? throw new NullReferenceException()) as IDisposable)?.Dispose();
                        crops.Delete(i);
                    }
                }
            }

            // { Original frees and clears the recursion guard only on normal completion. }
            crops?.Dispose();
            locations?.Dispose();
            _changingCrops = false;
            CropsChanged();
        }
    }

    // Crops.pas:540
    public void LinkToCrops()
    {
        int x = default;
        int y = default;
        if (EditMode)
        {
            return;
        }

        MapLevel mapLevel = Owner.Owner;
        for (int i = Resource.CropLocations.Count - 1; i >= 0; i--)
        {
            HexGeometry.CenterHexNumberToHex(StoredHexX, StoredHexY, hexNumber: Resource.CropLocations.List[i], ref x, ref y);
            if (x >= 0 && mapLevel.Width > x && y >= 0 && mapLevel.Height > y)
            {
                Crop crop = mapLevel.GetField(x, y).FindChild<Crop>(Crop.ClassId);
                crop?.SetCropOwner(this);
            }
        }
    }

    // Crops.pas:27
    public override int GetIncome() => Income;
}
