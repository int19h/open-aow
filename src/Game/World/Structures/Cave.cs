#nullable disable
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
    public new const int ClassId = 131966;

    public CaveEntrance Entrance { get; private set; } = CaveEntrance.Down;

    // Cave.pas:134
    public override void Dispose()
    {
        CaveEntrance oldEntrance = Entrance;
        Entrance = CaveEntrance.Idle;
        if (Status.Contains(MapObjectStatusElement.PlacedOnMap) && Owner is not null)
        {
            if (!GetEditMode())
            {
                Cave partner;
                if (oldEntrance == CaveEntrance.Up)
                {
                    partner = StrategicMap.Current.MapContainer.GetField(StoredHexX, StoredHexY, Owner.HexLevel - 1).FindOwnedChild<Cave>(131966);
                    if (partner is not null && partner.Entrance == CaveEntrance.Down)
                    {
                        partner.Dispose();
                    }
                }
                else
                {
                    partner = StrategicMap.Current.MapContainer.GetField(StoredHexX, StoredHexY, Owner.HexLevel + 1).FindOwnedChild<Cave>(131966);
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

    // Cave.pas:128

    // Cave.pas:273
    public override sbyte GetOverlay(int index)
    {
        sbyte result = base.Resource.TerrainList.Terrain[index];
        if (result >= 0)
        {
            result = -1;
        }

        return result;
    }

    // Cave.pas:257
    public override void MoveExclusive(in HexSpriteSenderInfo info)
    {
        if (info.HexNumber == 0)
        {
            MapField field = Entrance switch
            {
                CaveEntrance.Up => StrategicMap.Current.MapContainer.GetField(StoredHexX, StoredHexY, Owner.HexLevel - 1),
                CaveEntrance.Down => StrategicMap.Current.MapContainer.GetField(StoredHexX, StoredHexY, Owner.HexLevel + 1),
                CaveEntrance.Idle or _ => null,
            };

            // { Original dereferences the destination even for an idle entrance. }
            ((MoveControl)info.Sender).ExecuteExternalMove(info.MapField, field, ((MoveControl)info.Sender).MovePointTable.AtOffset(1 + (16 * field.TerrainType) + field.OverlayType));
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
        Status -= Set<MapObjectStatusElement>.Of(MapObjectStatusElement.Activated);
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
    public override bool CanPlace(MapContainer mapContainer, sbyte hexX, sbyte hexY, sbyte hexLevel) =>
        // { Original permits an upward cave on level zero; there is no lower-bound test. }
        base.CanPlace(mapContainer, hexX, hexY, hexLevel) && (hexLevel + 1 < mapContainer.LevelCount || Entrance == CaveEntrance.Up);

    // Cave.pas:211
    public override bool PlaceHex(MapContainer mapContainer, sbyte hexX, sbyte hexY, sbyte hexLevel)
    {
        bool result = base.PlaceHex(mapContainer, hexX, hexY, hexLevel);
        if (result)
        {
            if (!GetEditMode())
            {
                if (Entrance == CaveEntrance.Down)
                {
                    Cave up = mapContainer.GetField(hexX, hexY, hexLevel + 1).FindOwnedChild<Cave>(131966);
                    if (up is null)
                    {
                        up = base.Resource.CreateResourceUser<Cave>();
                        up.Entrance = CaveEntrance.Up;
                        result = up.PlaceHex(mapContainer, hexX, hexY, (sbyte)(hexLevel + 1));
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
                    Cave down = mapContainer.GetField(hexX, hexY, hexLevel - 1).FindOwnedChild<Cave>(131966);
                    if (down is null)
                    {
                        down = base.Resource.CreateResourceUser<Cave>();
                        down.Entrance = CaveEntrance.Down;
                        result = down.PlaceHex(mapContainer, hexX, hexY, (sbyte)(hexLevel - 1));
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
    public override int GetTerrainTypeImage(int terrainType)
    {
        int result = Entrance == CaveEntrance.Down ? terrainType : terrainType + 30;
        return result;
    }

    // Cave.pas:163
    public override LibraryImage GetTopBorderImage()
    {
        LibraryImage result = Entrance == CaveEntrance.Up
            ? base.Resource.ImageSequenceList is not null && base.Resource.ImageSequenceList.ImageSequences[33] is not null
                ? base.Resource.ImageSequenceList.ImageSequences[33].GetPreviewImage()
                : null
            : base.GetTopBorderImage();
        return result;
    }

    // Cave.pas:175
    public override LibraryImage GetMiddleBorderImage()
    {
        LibraryImage result = Entrance == CaveEntrance.Up
            ? base.Resource.ImageSequenceList is not null && base.Resource.ImageSequenceList.ImageSequences[34] is not null
                ? base.Resource.ImageSequenceList.ImageSequences[34].GetPreviewImage()
                : null
            : base.GetMiddleBorderImage();
        return result;
    }

    // Cave.pas:187
    public override LibraryImage GetBottomBorderImage()
    {
        LibraryImage result = Entrance == CaveEntrance.Up
            ? base.Resource.ImageSequenceList is not null && base.Resource.ImageSequenceList.ImageSequences[35] is not null
                ? base.Resource.ImageSequenceList.ImageSequences[35].GetPreviewImage()
                : null
            : base.GetBottomBorderImage();
        return result;
    }

    // Cave.pas:412
    public override void ArmyPlaced(ArmyHexSprite placedArmy)
    {
        base.ArmyPlaced(placedArmy);
        ArmyHexSprite armyHexSprite = placedArmy;
        if (armyHexSprite is not null)
        {
            int l = Entrance == CaveEntrance.Down ? GetHexLevel() + 1 : GetHexLevel() - 1;

            byte visibilityRange = armyHexSprite.Army.VisibilityRange();
            sbyte hexY = armyHexSprite.GetHexY();
            sbyte hexX = armyHexSprite.GetHexX();
            _ = StrategicMap.Current.MapContainer.GetMapLevel(l).ExploreArea(hexX, hexY, armyHexSprite.Army.Player, visibilityRange);
        }
    }

    // Cave.pas:292
    public sbyte EnterMovePoints(Army army, byte selection)
    {
        int l = Entrance == CaveEntrance.Down ? GetHexLevel() + 1 : GetHexLevel() - 1;
        sbyte hexY = GetHexY();
        sbyte hexX = GetHexX();
        MapField field = StrategicMap.Current.MapContainer.GetField(hexX, hexY, l);
        return (sbyte)army.MoveOnMovePoints(selection, field.TerrainType, field.OverlayType);
    }

    // Cave.pas:300
    public byte CanEnterSelection(UnitCollection units)
    {
        int l = Entrance == CaveEntrance.Down ? GetHexLevel() + 1 : GetHexLevel() - 1;

        sbyte hexY = GetHexY();
        sbyte hexX = GetHexX();
        MapField field = StrategicMap.Current.MapContainer.GetField(hexX, hexY, l);
        byte result = 0;
        for (int i = 0, last = units.Count - 1; i <= last; i++)
        {
            Troop troop = units.GetUnit(i);
            if (troop.MovePointCost(field.TerrainType, field.OverlayType) <= troop.GetMovePoints())
            {
                result = (byte)(result + (1 << i));
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
            byte selection = CanEnterSelection(armyHexSprite.Army.Units);
            sbyte l = 0;
            MovePath path = new();
            switch (Entrance)
            {
                case CaveEntrance.Down:
                    l = (sbyte)(GetHexLevel() + 1);
                    break;
                case CaveEntrance.Up:
                    l = (sbyte)(GetHexLevel() - 1);
                    break;
                case CaveEntrance.Idle:
                default:
                    break;
            }
            _ = path.AddMapCoordinate(y: GetHexY(), x: GetHexX(), mapLevel: l);
            sbyte enterMovePoints = EnterMovePoints(armyHexSprite.Army, selection);
            path.List[0] = path.List[0] with { MoveCost = enterMovePoints };
            _ = path.AddMapCoordinate(mapLevel: GetHexLevel(), y: GetHexY(), x: GetHexX());
            sbyte hexY3 = GetHexY();
            sbyte hexX = GetHexX();
            MapField field = StrategicMap.Current.MapContainer.GetField(hexX, hexY3, l);
            bool condition = field.GetMoveAction(armyHexSprite.Army.Player) != MoveAction.Attack || field.GetPlayer() <= 0;
            if (!condition)
            {
                sbyte player = field.GetPlayer();
                DiplomaticRelation diplomaticRelation = StrategicMap.Current.Players.GetDiplomaticRelation(armyHexSprite.Army.Player, player);
                condition = Set<byte>.WithBounds(0, 255).With(1).With(3).Contains((byte)(int)diplomaticRelation);
            }

            if (condition)
            {
                if (field.GetPlayer() > -1 && field.GetPlayer() != armyHexSprite.Army.Player)
                {
                    if (!StrategicMap.Current.Players.GetPlayers(armyHexSprite.Army.Player).GetBusy())
                    {
                        // { Original passes action before mode. }
                        TokenEvent eventValue = ArmyHexSprite.CreateArmyMoveTokenEvent(armyHexSprite, selection, path, MoveAction.Attack, Set<ArmyMoveModeElement>.Of());
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
                    result = ArmyHexSprite.MoveArmy(armyHexSprite, selection, path, Set<ArmyMoveModeElement>.Of(), false);
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
                    Player playerPlayerRace = StrategicMap.Current.Players.GetPlayers(player);
                    string raceNamesPlural = AbilityRules.TranslateResourceString(GetEngine(), GameTables.RaceNamesPluralTexts[playerPlayerRace.PlayerRace]);
                    string messageText = DelphiFormat.Format(AbilityRules.TranslateResourceString(GetEngine(), GameText.NoWarDeclaredFormat), [raceNamesPlural]);
                    StrategicMap.Current.ShowMessage(messageText);
                }

                result = false;
            }

            // { Original frees the path only on normal completion. }
            path?.Dispose();
        }
        else
        {
            result = false;
        }

        return result;
    }

    // Cave.pas:388
    public bool Enter(sbyte player)
    {
        bool result;
        if (CanEnter(player))
        {
            ArmyHexSprite armyHexSprite = Owner.GetArmyHexSprite();
            if (CanEnterSelection(armyHexSprite.Army.Units) != armyHexSprite.Army.ValidateSelection(255))
            {
                EnterCaveEventLog log = new(null, holder: CreationHolder())
                {
                    Cave = this
                };
                string splitOnEnterCaveWarning = AbilityRules.TranslateResourceString(GetEngine(), s_splitOnEnterCaveWarningText);
                log.Message.Text = splitOnEnterCaveWarning;
                log.Buttons = MessageEventLogButtons.YesNo;
                _ = log.Execute(true, ExecuteTrigger.User, null);
                _ = log.Release();
                result = true;
            }
            else
            {
                result = Enter();
            }
        }
        else
        {
            result = false;
        }

        return result;
    }

    private static readonly string s_splitOnEnterCaveWarningText = "Entering the cave will split your party. Do you wish to continue?";

    public new partial class Resource : StructureResource
    {
        public Resource(CustomNode owner) : base(owner)
        {
            TerrainBound = false;
            ConnectRoads = true;
        }

        // Cave.pas:436
        public new const int ClassId = 131967;

        // Cave.pas:426

        // Cave.pas:439
        public override int ResourceUserClassId() => 131966;

        // Cave.pas:49

        // Cave.pas:433
        public override string ResourceEditName() => AbilityRules.TranslateResourceString(GetEngine(), s_caveText);

        private static readonly string s_caveText = "Cave";

    }

    // Cave.pas:25

    // Cave.pas:25

}

public enum CaveEntrance : byte
{
    Up = 0, Down = 1, Idle = 2
}
