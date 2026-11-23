#nullable disable warnings
// Translated from Cave.pas.
using Aow.Engine.Nodes;
using Aow.Formats.Tree;
using Aow.Game.Commands;
using Aow.Game.Draw;
using Aow.Game.Draw.Images;

using Aow.Game.Events;
using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Game.Units;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

using Aow.Rtl;

namespace Aow.Game.World.Structures;

// { ot=1 }
// Entrance
public partial class Cave(CustomNode owner) : Structure(owner)
{

    // Cave.pas:160
    public new const int ClassId = 131_966;

    public CaveEntrance Entrance { get; private set; } = CaveEntrance.Down;

    // Cave.pas:134
    public override void Dispose()
    {
        CaveEntrance oldEntrance = Entrance;
        Entrance = CaveEntrance.Idle;
        if (Status.Contains(MapObjectStatuses.PlacedOnMap) && Owner is not null)
        {
            if (!EditMode)
            {
                Cave partner;
                if (oldEntrance == CaveEntrance.Up)
                {
                    partner = StrategicMap.Current.MapContainer.GetField(StoredHexX, StoredHexY, Owner.HexLevel - 1).FindOwnedChild<Cave>(ClassId);
                    if (partner is not null && partner.Entrance == CaveEntrance.Down)
                    {
                        partner.Dispose();
                    }
                }
                else
                {
                    partner = StrategicMap.Current.MapContainer.GetField(StoredHexX, StoredHexY, Owner.HexLevel + 1).FindOwnedChild<Cave>(ClassId);
                    if (partner is not null && partner.Entrance == CaveEntrance.Up)
                    {
                        partner.Dispose();
                    }
                }
            }
        }

        base.Dispose();
    }

    // Cave.pas:244
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        Entrance = persister.Enum("Entrance", value: Entrance);
    }

    // Cave.pas:273
    public override int GetOverlay(int index)
    {
        int result = base.Resource.TerrainList.Terrain[index];
        return result >= 0 ? -1 : result;
    }

    // Cave.pas:257
    public override void MoveExclusive(in HexSpriteSenderInfo info)
    {
        if (info.HexNumber == 0)
        {
            MapField? field = Entrance switch
            {
                CaveEntrance.Up => StrategicMap.Current.MapContainer.GetField(StoredHexX, StoredHexY, Owner.HexLevel - 1),
                CaveEntrance.Down => StrategicMap.Current.MapContainer.GetField(StoredHexX, StoredHexY, Owner.HexLevel + 1),
                CaveEntrance.Idle or _ => null,
            };

            // { Original dereferences the destination even for an idle entrance. }
            ((MoveControl)info.Sender).ExecuteExternalMove(info.MapField, target: field, movePoints: ((MoveControl)info.Sender).MovePointTable.AtOffset(1 + (16 * field.TerrainType) + field.OverlayType));
        }
    }

    // Cave.pas:279
    public override void Activate()
    {
        base.Activate();
        Owner.UpdateMapField();
    }

    // Cave.pas:285
    public override void Deactivate()
    {
        base.Deactivate();
        Status &= ~MapObjectStatuses.Activated;
        Owner.UpdateMapField();
    }

    // Cave.pas:250
    public override void UpdateMapField(in HexSpriteMapFieldInfo info)
    {
        if (info.HexNumber == 0)
        {
            info.MapField.FieldFlags = info.MapField.FieldFlags | MapField.MoveExclusiveFlag;
        }
    }

    // Cave.pas:204
    public override bool CanPlace(MapContainer mapContainer, int hexX, int hexY, int hexLevel)
        // { Original permits an upward cave on level zero; there is no lower-bound test. }
        => base.CanPlace(mapContainer, hexX, hexY, hexLevel) && (hexLevel + 1 < mapContainer.LevelCount || Entrance == CaveEntrance.Up);

    // Cave.pas:211
    public override bool PlaceHex(MapContainer mapContainer, int hexX, int hexY, int hexLevel)
    {
        bool result = base.PlaceHex(mapContainer, hexX, hexY, hexLevel);
        if (result)
        {
            if (!EditMode)
            {
                if (Entrance == CaveEntrance.Down)
                {
                    Cave up = mapContainer.GetField(hexX, hexY, hexLevel + 1).FindOwnedChild<Cave>(ClassId);
                    if (up is null)
                    {
                        up = base.Resource.CreateResourceUser<Cave>();
                        up.Entrance = CaveEntrance.Up;
                        result = up.PlaceHex(mapContainer, hexX, hexY, (hexLevel + 1).TruncateToSByte());
                    }
                    else
                    {
                        result = up.Entrance == CaveEntrance.Up;
                    }

                    // { Original destroys an existing partner when its entrance direction is wrong. }
                    if (!result)
                    {
                        up.Dispose();
                    }
                    else
                    {
                        OverrideTerrain();
                    }
                }
                else
                {
                    Cave down = mapContainer.GetField(hexX, hexY, hexLevel - 1).FindOwnedChild<Cave>(ClassId);
                    if (down is null)
                    {
                        down = base.Resource.CreateResourceUser<Cave>();
                        down.Entrance = CaveEntrance.Down;
                        result = down.PlaceHex(mapContainer, hexX, hexY, (hexLevel - 1).TruncateToSByte());
                    }
                    else
                    {
                        result = down.Entrance == CaveEntrance.Down;
                    }

                    if (!result)
                    {
                        down.Dispose();
                    }
                    else
                    {
                        OverrideTerrain();
                    }
                }
            }
        }

        return result;
    }

    // Cave.pas:199
    public override int GetTerrainTypeImage(int terrainType) => Entrance == CaveEntrance.Down ? terrainType : terrainType + 30;

    // Cave.pas:163
    public override LibraryImage? GetTopBorderImage()
    {
        return Entrance == CaveEntrance.Up
            ? base.Resource.ImageSequenceList is not null && base.Resource.ImageSequenceList.ImageSequences[33] is not null
                ? base.Resource.ImageSequenceList.ImageSequences[33].GetPreviewImage()
                : null
            : base.GetTopBorderImage();
    }

    // Cave.pas:175
    public override LibraryImage? GetMiddleBorderImage()
    {
        return Entrance == CaveEntrance.Up
            ? base.Resource.ImageSequenceList is not null && base.Resource.ImageSequenceList.ImageSequences[34] is not null
                ? base.Resource.ImageSequenceList.ImageSequences[34].GetPreviewImage()
                : null
            : base.GetMiddleBorderImage();
    }

    // Cave.pas:187
    public override LibraryImage? GetBottomBorderImage()
    {
        return Entrance == CaveEntrance.Up
            ? base.Resource.ImageSequenceList is not null && base.Resource.ImageSequenceList.ImageSequences[35] is not null
                ? base.Resource.ImageSequenceList.ImageSequences[35].GetPreviewImage()
                : null
            : base.GetBottomBorderImage();
    }

    // Cave.pas:412
    public override void ArmyPlaced(ArmyHexSprite? placedArmy)
    {
        base.ArmyPlaced(placedArmy);
        ArmyHexSprite? armyHexSprite = placedArmy;
        if (armyHexSprite is not null)
        {
            int l = Entrance == CaveEntrance.Down ? HexLevel + 1 : HexLevel - 1;

            int visibilityRange = armyHexSprite.Army.VisibilityRange();
            int hexY = armyHexSprite.HexY;
            int hexX = armyHexSprite.HexX;
            _ = StrategicMap.Current.MapContainer.GetMapLevel(l).ExploreArea(hexX, hexY, armyHexSprite.Army.Player, radius: visibilityRange);
        }
    }

    // Cave.pas:292
    public int EnterMovePoints(Army army, int selection)
    {
        int l = Entrance == CaveEntrance.Down ? HexLevel + 1 : HexLevel - 1;
        int hexY = HexY;
        int hexX = HexX;
        MapField field = StrategicMap.Current.MapContainer.GetField(hexX, hexY, l);
        return army.MoveOnMovePoints(selection, field.TerrainType, field.OverlayType).TruncateToSByte();
    }

    // Cave.pas:300
    public int CanEnterSelection(UnitCollection units)
    {
        int l = Entrance == CaveEntrance.Down ? HexLevel + 1 : HexLevel - 1;

        int hexY = HexY;
        int hexX = HexX;
        MapField field = StrategicMap.Current.MapContainer.GetField(hexX, hexY, l);
        int result = 0;
        for (int i = 0, last = units.Count - 1; i <= last; i++)
        {
            Troop? troop = units.GetUnit(i);
            if (troop.MovePointCost(field.TerrainType, field.OverlayType) <= troop.GetMovePoints())
            {
                result = (result + (1 << i)).TruncateToByte();
            }
        }

        return result;
    }

    // Cave.pas:317
    public bool CanEnter(sbyte player)
    {
        ArmyHexSprite armyHexSprite = Owner.GetArmyHexSprite();
        return armyHexSprite is not null && player == armyHexSprite.Army.Player && CanEnterSelection(armyHexSprite.Army.Units) != 0;
    }

    // Cave.pas:325
    public bool Enter()
    {
        ArmyHexSprite armyHexSprite = Owner.GetArmyHexSprite();
        bool result;
        if (armyHexSprite is not null && CanEnter(armyHexSprite.Army.Player))
        {
            int selection = CanEnterSelection(armyHexSprite.Army.Units);
            int l = 0;
            MovePath path = [];
            switch (Entrance)
            {
                case CaveEntrance.Down:
                    l = (HexLevel + 1).TruncateToSByte();
                    break;
                case CaveEntrance.Up:
                    l = (HexLevel - 1).TruncateToSByte();
                    break;
                case CaveEntrance.Idle:
                default:
                    break;
            }
            _ = path.AddMapCoordinate(y: HexY, x: HexX, mapLevel: l);
            int enterMovePoints = EnterMovePoints(armyHexSprite.Army, selection);
            path.List[0] = path.List[0] with { MoveCost = (sbyte)enterMovePoints };
            _ = path.AddMapCoordinate(mapLevel: HexLevel, y: HexY, x: HexX);
            int hexY3 = HexY;
            int hexX = HexX;
            MapField field = StrategicMap.Current.MapContainer.GetField(hexX, hexY3, l);
            bool condition = field.GetMoveAction(armyHexSprite.Army.Player) != MoveAction.Attack || field.GetPlayer() <= 0;
            if (!condition)
            {
                sbyte player = field.GetPlayer();
                DiplomaticRelation diplomaticRelation = StrategicMap.Current.Players.GetDiplomaticRelation(armyHexSprite.Army.Player, otherPlayer: player);
                condition = diplomaticRelation is DiplomaticRelation.War or DiplomaticRelation.Unknown;
            }

            if (condition)
            {
                if (field.GetPlayer() > -1 && field.GetPlayer() != armyHexSprite.Army.Player)
                {
                    if (!StrategicMap.Current.Players.GetPlayers(armyHexSprite.Army.Player).GetBusy())
                    {
                        // { Original passes action before mode. }
                        TokenEvent? eventValue = ArmyHexSprite.CreateArmyMoveTokenEvent(armyHexSprite, selection, path, action: MoveAction.Attack, mode: ArmyMoveModes.None);
                        if (eventValue is not null)
                        {
                            StrategicMap.Current.TokenManager.GetTokenControl(armyHexSprite.Army.Player).AddEvent(eventValue);
                            _ = eventValue.Release();
                            result = true;
                        }
                        else
                        {
                            result = false;
                        }
                    }
                    else
                    {
                        result = false;
                    }
                }
                else
                {
                    result = ArmyHexSprite.MoveArmy(armyHexSprite, selection, path, mode: ArmyMoveModes.None, force: false);
                }

                if (result && (byte)StrategicMap.Current.SeatedPlayer == (byte)armyHexSprite.Army.Player)
                {
                    StrategicMap.Current.ViewLevel(l);
                    StrategicMap.Current.SelectedArmy.Select(armyHexSprite);
                }
            }
            else
            {
                // { Original only warns the seated player. }
                if (armyHexSprite.Army.Player == StrategicMap.Current.SeatedPlayer)
                {
                    sbyte player = field.GetPlayer();
                    Player? playerPlayerRace = StrategicMap.Current.Players.GetPlayers(player);
                    string raceNamesPlural = AbilityRules.TranslateResourceString(GetEngine(), S: GameTables.RaceNamesPluralTexts[playerPlayerRace.PlayerRace]);
                    string messageText = DelphiFormat.Format(AbilityRules.TranslateResourceString(GetEngine(), S: GameText.NoWarDeclaredFormat), args: [raceNamesPlural]);
                    StrategicMap.Current.ShowMessage(messageText);
                }

                result = false;
            }

            // { Original frees the path only on normal completion. }
            path?.Dispose();
        }
        else
        {
            return false;
        }

        return result;
    }

    // Cave.pas:388
    public bool Enter(sbyte player)
    {
        if (CanEnter(player))
        {
            ArmyHexSprite armyHexSprite = Owner.GetArmyHexSprite();
            if (CanEnterSelection(armyHexSprite.Army.Units) != armyHexSprite.Army.ValidateSelection(255))
            {
                EnterCaveEventLog log = new(null, holder: CreationHolder())
                {
                    Cave = this
                };
                string splitOnEnterCaveWarning = AbilityRules.TranslateResourceString(GetEngine(), S: s_splitOnEnterCaveWarningText);
                log.Message.Text = splitOnEnterCaveWarning;
                log.Buttons = MessageEventLogButtons.YesNo;
                _ = log.Execute(true, trigger: ExecuteTrigger.User, done: null);
                _ = log.Release();
                return true;
            }
            else
            {
                return Enter();
            }
        }
        else
        {
            return false;
        }
    }

    private static readonly string s_splitOnEnterCaveWarningText = "Entering the cave will split your party. Do you wish to continue?";

    public new class Resource : StructureResource
    {
        public Resource(CustomNode owner) : base(owner)
        {
            TerrainBound = false;
            ConnectRoads = true;
        }

        // Cave.pas:436
        public new const int ClassId = 131_967;

        // Cave.pas:439
        public override int ResourceUserClassId() => Cave.ClassId;

        // Cave.pas:433
        public override string ResourceEditName() => AbilityRules.TranslateResourceString(GetEngine(), S: s_caveText);

        private static readonly string s_caveText = "Cave";

    }
}

public enum CaveEntrance : byte
{
    Up = 0, Down = 1, Idle = 2
}
