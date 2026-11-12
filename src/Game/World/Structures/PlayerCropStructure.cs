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
public partial class PlayerCropStructure : PlayerStructure
{

    public PlayerCropStructure(ECustomNode owner) : base(owner)
    {
        _changingCrops = false;
        _incomeSource = new PlayerStructureIncomeSource(null);
        _incomeSource.Setup(this);
    }

    // Crops.pas:284
    public new const int ClassId = 131918;

    public byte Income { get; private set; }

    private bool _changingCrops;
    private readonly PlayerStructureIncomeSource _incomeSource;

    // Crops.pas:278
    public override void Dispose()
    {
        base.Dispose();
        _incomeSource?.Dispose();
    }

    // Crops.pas:270

    // Crops.pas:528
    public override void NeighbourTerrainChanged(int hexNumber, in HexSpriteTerrainInfo info)
    {
        if (Set<HexSpriteMapStatusElement>.Of(HexSpriteMapStatusElement.Editting, HexSpriteMapStatusElement.Activated).IsSubsetOf(StrategicMap.Current.Status) && !_changingCrops)
        {
            UpdateCrops();
        }
    }

    // Crops.pas:534
    public override void Remove()
    {
        if (!GetEditMode())
        {
            SetCropCount(0);
        }

        base.Remove();
    }

    // Crops.pas:357
    public override void Activate()
    {
        sbyte player;
        base.Activate();
        player = Player;
        if (player != -1)
        {
            StrategicMap.Current.Players.GetPlayers(player).RegisterIncomeSource(_incomeSource);
        }

        if (!GetEditMode())
        {
            LinkToCrops();
            UpdateCrops();
        }
    }

    // Crops.pas:372
    public override void Deactivate()
    {
        sbyte player;
        player = Player;
        if (player != -1)
        {
            StrategicMap.Current.Players.GetPlayers(player).UnregisterIncomeSource(_incomeSource);
        }

        base.Deactivate();
    }

    // Crops.pas:560
    public override bool CanPlace(MapContainer mapContainer, sbyte hexX, sbyte hexY, sbyte hexLevel)
    {
        MapLevel mapLevel;
        int I;
        int hexNumber;
        int X = default;
        int Y = default;
        int hexXLocal = default;
        int hexYLocal = default;
        bool result = base.CanPlace(mapContainer, hexX, hexY, hexLevel);
        if (result && !GetEditMode())
        {
            mapLevel = mapContainer.GetMapLevel(hexLevel);
            I = ((PlayerCropStructureResource)_resource).CropLocations.Count;
            while (result && I != 0)
            {
                I -= 1;
                HexGeometry.CenterHexNumberToHex(hexX, hexY, ((PlayerCropStructureResource)_resource).CropLocations.List[I], ref X, ref Y);
                hexXLocal = X;
                hexYLocal = Y;
                hexNumber = 0;
                while (result && hexNumber != 7)
                {
                    if (hexXLocal >= 0 && hexXLocal < mapLevel.Width && hexYLocal >= 0 && hexYLocal < mapLevel.Height)
                    {
                        result = mapLevel.GetField(hexXLocal, hexYLocal).FindNoneTransparentHexSprite(typeof(PlayerCropStructure)) == null;
                    }

                    hexNumber += 1;
                    HexGeometry.CenterHexNumberToHex(X, Y, hexNumber, ref hexXLocal, ref hexYLocal);
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
            if (!GetEditMode())
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
        ArmyHexSprite army;
        HexSprite hexSprite;
        if (!GetRazed())
        {
            army = (ArmyHexSprite)field.FindOwnedChild(131607);
            result = army == null || StrategicMap.Current.Players.GetDiplomaticRelation(army.Army.Player, Player) != DiplomaticRelation.War;
            if (result)
            {
                hexSprite = field.GetMaxLevelNoneTransparentHexSprite();
                result = hexSprite == null || hexSprite.GetLevel() < 26;
            }

            if (result)
            {
                result = Set<byte>.WithBounds(0, 255).With(12).WithRange(1, 4).Contains((byte)field.TerrainType) || field.TerrainType == 5 && GetRace() == 11;
            }
        }
        else
        {
            result = false;
        }

        return result;
    }

    // Crops.pas:320
    public virtual bool CropsEnabled() => !GetRazed() && !GetBuilding();

    // Crops.pas:314
    public virtual void RegrowCrops()
    {
        if (GetCropCount() < ((PlayerCropStructureResource)_resource).CropLocations.Count)
        {
            SetCropCount(GetCropCount() + 1);
        }
    }

    // Crops.pas:332
    public virtual void UpdateIncome()
    {
        if (!GetRazed() && !GetBuilding())
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
        if (!GetEditMode() && Status.Contains(MapObjectStatusElement.Activated))
        {
            if (StrategicMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting))
            {
                if (CropsEnabled())
                {
                    SetCropCount(((PlayerCropStructureResource)_resource).CropLocations.Count);
                }
            }

            UpdateCropSettings();
            UpdateIncome();
        }
    }

    // Crops.pas:413
    public virtual sbyte GetRace()
    {
        sbyte result;
        if (Player > 0)
        {
            result = StrategicMap.Current.Players.GetPlayers(Player).PlayerRace;
        }
        else
        {
            result = -1;
        }

        return result;
    }

    // Crops.pas:339
    public virtual void CropsChanged()
    {
        UpdateIncome();
    }

    // Crops.pas:290
    public virtual void CropTrampled(sbyte player)
    {
        int race;
        if (player != Player && Player > 0 && player > 0)
        {
            race = StrategicMap.Current.Players.GetPlayers(Player).PlayerRace;
            var playerRelationValue = StrategicMap.Current.Races.GetRace(race).GetPlayerRelationValue(player);
            StrategicMap.Current.Races.GetRace(race).SetPlayerRelationValue(player, playerRelationValue - -5);
        }
    }

    // Crops.pas:302
    public void SetIncome(byte value)
    {
        sbyte player;
        if (value != this.Income && this.Status.Contains(MapObjectStatusElement.Activated))
        {
            this.Income = value;
            player = this.Player;
            if (player != -1)
            {
                StrategicMap.Current.Players.GetPlayers(player).UpdateIncome();
            }
        }
    }

    // Crops.pas:382
    public void UpdateCropSettings()
    {
        MapLevel mapLevel;
        int X = default;
        int Y = default;
        Crop crop;
        mapLevel = Owner.Owner;
        for (int I = ((PlayerCropStructureResource)_resource).CropLocations.Count - 1; I >= 0; I--)
        {
            HexGeometry.CenterHexNumberToHex(_hexX, _hexY, ((PlayerCropStructureResource)_resource).CropLocations.List[I], ref X, ref Y);
            if (X >= 0 && mapLevel.Width > X && Y >= 0 && mapLevel.Height > Y)
            {
                crop = (Crop)mapLevel.GetField(X, Y).FindOwnedChild(131946);
                if (crop != null)
                {
                    crop.UpdateSettings();
                }
            }
        }
    }

    // Crops.pas:420
    public int GetCropCount()
    {
        MapLevel mapLevel;
        int X = default;
        int Y = default;
        Crop crop;
        int result = 0;
        mapLevel = Owner.Owner;
        for (int I = ((PlayerCropStructureResource)_resource).CropLocations.Count - 1; I >= 0; I--)
        {
            HexGeometry.CenterHexNumberToHex(_hexX, _hexY, ((PlayerCropStructureResource)_resource).CropLocations.List[I], ref X, ref Y);
            if (X >= 0 && mapLevel.Width > X && Y >= 0 && mapLevel.Height > Y)
            {
                crop = (Crop)mapLevel.GetField(X, Y).FindChild(131946);
                if (crop != null)
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
        MapLevel mapLevel;
        int I;
        Crop.Resource resource;
        MapField field;
        int X = default;
        int Y = default;
        DelphiList<Crop> crops;
        Crop crop;
        UInt16List locations;
        if (Status.Contains(MapObjectStatusElement.PlacedOnMap) && !_changingCrops)
        {
            _changingCrops = true;
            mapLevel = Owner.Owner;
            crops = new DelphiList<Crop>();
            locations = new UInt16List();
            locations.Assign(((PlayerCropStructureResource)_resource).CropLocations);
            I = locations.Count - 1;
            for (; I >= 0; I -= 1)
            {
                HexGeometry.CenterHexNumberToHex(_hexX, _hexY, locations.List[I], ref X, ref Y);
                if (X >= 0 && X < mapLevel.Width && Y >= 0 && Y < mapLevel.Height)
                {
                    field = mapLevel.GetField(X, Y);
                    crop = (Crop)field.FindOwnedChild(131946);
                    if (crop != null || !CanPlaceCrop(field))
                    {
                        if (crop != null && crop.CropOwner == this)
                        {
                            crops.Add(crop);
                        }

                        locations.Delete(I);
                    }
                }
                else
                {
                    locations.Delete(I);
                }
            }

            if (crops.Count < value)
            {
                resource = (Crop.Resource)_resource.Engine.HexSpriteSet.MapObjectResourceList.FindChild(131947);
                if (resource != null)
                {
                    while (crops.Count != value && locations.Count != 0)
                    {
                        I = StrategicMap.Current.Random(locations.Count);
                        HexGeometry.CenterHexNumberToHex(_hexX, _hexY, locations.List[I], ref X, ref Y);
                        crop = resource.PlaceCrop((sbyte)X, (sbyte)Y, (sbyte)mapLevel.HexLevel, this);
                        if (crop != null)
                        {
                            crops.Add(crop);
                            crop.SetCropOwner(this);
                        }

                        locations.Delete(I);
                    }
                }
            }
            else
            {
                if (crops.Count > value)
                {
                    while (crops.Count != value && crops.Count != 0)
                    {
                        I = StrategicMap.Current.Random(crops.Count);
                        ((crops[I] ?? throw new NullReferenceException()) as IDisposable)?.Dispose();
                        crops.Delete(I);
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
        MapLevel mapLevel;
        int X = default;
        int Y = default;
        Crop crop;
        if (GetEditMode())
        {
            return;
        }

        mapLevel = Owner.Owner;
        for (int I = ((PlayerCropStructureResource)_resource).CropLocations.Count - 1; I >= 0; I--)
        {
            HexGeometry.CenterHexNumberToHex(_hexX, _hexY, ((PlayerCropStructureResource)_resource).CropLocations.List[I], ref X, ref Y);
            if (X >= 0 && mapLevel.Width > X && Y >= 0 && mapLevel.Height > Y)
            {
                crop = (Crop)mapLevel.GetField(X, Y).FindChild(131946);
                if (crop != null)
                {
                    crop.SetCropOwner(this);
                }
            }
        }
    }

    // Crops.pas:27
    public override int GetIncome() => Income;
}
