// Translated from Cave.pas.
using System.Runtime.InteropServices;
using Aow.Engine.Buffers;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Game.Commands;
using Aow.Game.Draw;
using Aow.Game.Draw.Images;

using Aow.Game.Events;
using Aow.Game.Magic.Abilities;

using Aow.Game.Units;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

using Aow.Rtl;

namespace Aow.Game.World.Structures;

// { ot=1 }
// Entrance
public partial class Cave : Structure
{
    public Cave(ECustomNode owner) : base(owner)
    {
        Entrance = CaveEntrance.Down;
    }

    // Cave.pas:160
    public new const int ClassId = 131966;

    public CaveEntrance Entrance;

    // Cave.pas:134
    public override void Dispose()
    {
        CaveEntrance oldEntrance;
        Cave partner;
        oldEntrance = Entrance;
        Entrance = CaveEntrance.Idle;
        if (Status.Contains(MapObjectStatusElement.PlacedOnMap) && Owner != null)
        {
            if (!GetEditMode())
            {
                if (oldEntrance == CaveEntrance.Up)
                {
                    partner = (Cave)StrategicMap.Current.MapContainer.GetField(_hexX, _hexY, Owner.HexLevel - 1).FindOwnedChild(131966);
                    if (partner != null && partner.Entrance == CaveEntrance.Down)
                    {
                        partner.Dispose();
                    }
                }
                else
                {
                    partner = (Cave)StrategicMap.Current.MapContainer.GetField(_hexX, _hexY, Owner.HexLevel + 1).FindOwnedChild(131966);
                    if (partner != null && partner.Entrance == CaveEntrance.Up)
                    {
                        partner.Dispose();
                    }
                }
            }
        }

        base.Dispose();
    }

    // Cave.pas:244
    public override void ReadWrite(StorageStream stream)
    {
        base.ReadWrite(stream);
        stream.Bytes(Bytes.Of(ref Entrance, 1), 1, 6);
    }

    // Cave.pas:128

    // Cave.pas:273
    public override sbyte GetOverlay(int index)
    {
        sbyte result = _resource.TerrainList.Terrain[index];
        if (result >= 0)
        {
            result = -1;
        }

        return result;
    }

    // Cave.pas:257
    public override void MoveExclusive(in HexSpriteSenderInfo info)
    {
        MapField field;
        if (info.HexNumber == 0)
        {
            switch (Entrance)
            {
                case CaveEntrance.Up:
                    field = StrategicMap.Current.MapContainer.GetField(_hexX, _hexY, Owner.HexLevel - 1);
                    break;
                case CaveEntrance.Down:
                    field = StrategicMap.Current.MapContainer.GetField(_hexX, _hexY, Owner.HexLevel + 1);
                    break;
                default:
                    field = null;
                    break;
            }

            // { Original dereferences the destination even for an idle entrance. }
            ((MoveControl)info.Sender).ExecuteExternalMove(info.MapField, field, ((MoveControl)info.Sender).MovePointTable.AtOffset(1 + 16 * field.TerrainType + field.OverlayType));
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
        Status = Status - Set<MapObjectStatusElement>.Of(MapObjectStatusElement.Activated);
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
    public override bool CanPlace(MapContainer mapContainer, sbyte hexX, sbyte hexY, sbyte hexLevel)
    {
        // { Original permits an upward cave on level zero; there is no lower-bound test. }
        return base.CanPlace(mapContainer, hexX, hexY, hexLevel) && (hexLevel + 1 < mapContainer.LevelCount || Entrance == CaveEntrance.Up);
    }

    // Cave.pas:211
    public override bool PlaceHex(MapContainer mapContainer, sbyte hexX, sbyte hexY, sbyte hexLevel)
    {
        Cave up;
        Cave down;
        bool result = base.PlaceHex(mapContainer, hexX, hexY, hexLevel);
        if (result)
        {
            if (!GetEditMode())
            {
                if (Entrance == CaveEntrance.Down)
                {
                    up = (Cave)mapContainer.GetField(hexX, hexY, hexLevel + 1).FindOwnedChild(131966);
                    if (up == null)
                    {
                        up = (Cave)_resource.CreateResourceUser();
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
                    down = (Cave)mapContainer.GetField(hexX, hexY, hexLevel - 1).FindOwnedChild(131966);
                    if (down == null)
                    {
                        down = (Cave)_resource.CreateResourceUser();
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
        int result;
        if (Entrance == CaveEntrance.Down)
        {
            result = terrainType;
        }
        else
        {
            result = terrainType + 30;
        }

        return result;
    }

    // Cave.pas:163
    public override LibraryImage GetTopBorderImage()
    {
        LibraryImage result;
        if (Entrance == CaveEntrance.Up)
        {
            if (_resource.ImageSequenceList != null && _resource.ImageSequenceList.ImageSequences[33] != null)
            {
                result = _resource.ImageSequenceList.ImageSequences[33].GetPreviewImage();
            }
            else
            {
                result = null;
            }
        }
        else
        {
            result = base.GetTopBorderImage();
        }

        return result;
    }

    // Cave.pas:175
    public override LibraryImage GetMiddleBorderImage()
    {
        LibraryImage result;
        if (Entrance == CaveEntrance.Up)
        {
            if (_resource.ImageSequenceList != null && _resource.ImageSequenceList.ImageSequences[34] != null)
            {
                result = _resource.ImageSequenceList.ImageSequences[34].GetPreviewImage();
            }
            else
            {
                result = null;
            }
        }
        else
        {
            result = base.GetMiddleBorderImage();
        }

        return result;
    }

    // Cave.pas:187
    public override LibraryImage GetBottomBorderImage()
    {
        LibraryImage result;
        if (Entrance == CaveEntrance.Up)
        {
            if (_resource.ImageSequenceList != null && _resource.ImageSequenceList.ImageSequences[35] != null)
            {
                result = _resource.ImageSequenceList.ImageSequences[35].GetPreviewImage();
            }
            else
            {
                result = null;
            }
        }
        else
        {
            result = base.GetBottomBorderImage();
        }

        return result;
    }

    // Cave.pas:412
    public override void ArmyPlaced(ArmyHexSprite placedArmy)
    {
        int L;
        ArmyHexSprite armyHexSprite;
        base.ArmyPlaced(placedArmy);
        armyHexSprite = placedArmy;
        if (armyHexSprite != null)
        {
            if (Entrance == CaveEntrance.Down)
            {
                L = GetHexLevel() + 1;
            }
            else
            {
                L = GetHexLevel() - 1;
            }

            var visibilityRange = armyHexSprite.Army.VisibilityRange();
            var hexY = armyHexSprite.GetHexY();
            var hexX = armyHexSprite.GetHexX();
            StrategicMap.Current.MapContainer.GetMapLevel(L).ExploreArea(hexX, hexY, armyHexSprite.Army.Player, visibilityRange);
        }
    }

    // Cave.pas:292
    public sbyte EnterMovePoints(Army army, byte selection)
    {
        MapField field;
        int L;
        if (Entrance == CaveEntrance.Down)
        {
            L = GetHexLevel() + 1;
        }
        else
        {
            L = GetHexLevel() - 1;
        }

        var hexY = GetHexY();
        var hexX = GetHexX();
        field = StrategicMap.Current.MapContainer.GetField(hexX, hexY, L);
        return (sbyte)army.MoveOnMovePoints(selection, field.TerrainType, field.OverlayType);
    }

    // Cave.pas:300
    public byte CanEnterSelection(UnitList units)
    {
        int L;
        MapField field;
        Troop troop;
        if (Entrance == CaveEntrance.Down)
        {
            L = GetHexLevel() + 1;
        }
        else
        {
            L = GetHexLevel() - 1;
        }

        var hexY = GetHexY();
        var hexX = GetHexX();
        field = StrategicMap.Current.MapContainer.GetField(hexX, hexY, L);
        byte result = 0;
        for (int I = 0, last = units.Count - 1; I <= last; I++)
        {
            troop = units.GetUnit(I);
            if (troop.MovePointCost(field.TerrainType, field.OverlayType) <= troop.GetMovePoints())
            {
                result = (byte)(result + (1 << I));
            }
        }

        return result;
    }

    // Cave.pas:317
    public bool CanEnter(sbyte player)
    {
        ArmyHexSprite armyHexSprite;
        armyHexSprite = (ArmyHexSprite)Owner.FindOwnedChild(131607);
        return armyHexSprite != null && player == armyHexSprite.Army.Player && CanEnterSelection(armyHexSprite.Army) != 0;
    }

    // Cave.pas:325
    public bool Enter()
    {
        bool result;
        byte selection;
        MovePath path;
        sbyte L;
        string messageText;
        ArmyHexSprite armyHexSprite;
        MapField field;
        TokenEvent eventValue;
        armyHexSprite = (ArmyHexSprite)Owner.FindOwnedChild(131607);
        if (armyHexSprite != null && CanEnter(armyHexSprite.Army.Player))
        {
            selection = CanEnterSelection(armyHexSprite.Army);
            L = 0;
            path = new MovePath();
            switch (Entrance)
            {
                case CaveEntrance.Down:
                    L = (sbyte)(GetHexLevel() + 1);
                    break;
                case CaveEntrance.Up:
                    L = (sbyte)(GetHexLevel() - 1);
                    break;
            }

            var hexY = GetHexY();
            path.AddMapCoordinate(GetHexX(), hexY, L);
            var enterMovePoints = EnterMovePoints(armyHexSprite.Army, selection);
            path.List[0] = path.List[0] with { MoveCost = enterMovePoints };
            var hexLevel = GetHexLevel();
            var hexY2 = GetHexY();
            path.AddMapCoordinate(GetHexX(), hexY2, hexLevel);
            var hexY3 = GetHexY();
            var hexX = GetHexX();
            field = StrategicMap.Current.MapContainer.GetField(hexX, hexY3, L);
            bool condition = field.GetMoveAction(armyHexSprite.Army.Player) != MoveAction.Attack || field.GetPlayer() <= 0;
            if (!condition)
            {
                var player = field.GetPlayer();
                var diplomaticRelation = StrategicMap.Current.Players.GetDiplomaticRelation(armyHexSprite.Army.Player, player);
                condition = Set<byte>.WithBounds(0, 255).With(1).With(3).Contains((byte)(int)diplomaticRelation);
            }

            if (condition)
            {
                if (field.GetPlayer() > -1 && field.GetPlayer() != armyHexSprite.Army.Player)
                {
                    if (!StrategicMap.Current.Players.GetPlayers(armyHexSprite.Army.Player).GetBusy())
                    {
                        // { Original passes action before mode. }
                        eventValue = Draw.ArmyHexSprite.CreateArmyMoveTokenEvent(armyHexSprite, selection, path, MoveAction.Attack, Set<ArmyMoveModeElement>.Of());
                        if (eventValue != null)
                        {
                            StrategicMap.Current.TokenManager.GetTokenControl(armyHexSprite.Army.Player).AddEvent(eventValue);
                            eventValue.Release();
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
                    result = Draw.ArmyHexSprite.MoveArmy(armyHexSprite, selection, path, Set<ArmyMoveModeElement>.Of(), false);
                }

                if (result && (byte)StrategicMap.Current.SeatedPlayer == (byte)armyHexSprite.Army.Player)
                {
                    StrategicMap.Current.ViewLevel(L);
                    StrategicMap.Current.SelectedArmy.Select(armyHexSprite);
                }
            }
            else
            {
                // { Original only warns the seated player. }
                if (armyHexSprite.Army.Player == StrategicMap.Current.SeatedPlayer)
                {
                    var player = field.GetPlayer();
                    var playerPlayerRace = StrategicMap.Current.Players.GetPlayers(player);
                    var raceNamesPlural = AbilityRules.TranslateResourceString(GetEngine(), GameTables.RaceNamesPluralTexts[playerPlayerRace.PlayerRace]);
                    messageText = RtlUnit.Format(AbilityRules.TranslateResourceString(GetEngine(), GameText.NoWarDeclaredFormat), [raceNamesPlural]);
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
        bool result = default;
        ArmyHexSprite armyHexSprite;
        EnterCaveEventLog log;
        if (CanEnter(player))
        {
            armyHexSprite = (ArmyHexSprite)Owner.FindOwnedChild(131607);
            if (CanEnterSelection(armyHexSprite.Army) != armyHexSprite.Army.ValidateSelection(255))
            {
                log = new EnterCaveEventLog(null, holder: CreationHolder());
                log.Cave = this;
                var splitOnEnterCaveWarning = AbilityRules.TranslateResourceString(GetEngine(), s_splitOnEnterCaveWarningText);
                log.Message.Text = splitOnEnterCaveWarning;
                log.Buttons = MessageEventLogButtons.YesNo;
                log.Execute(true, ExecuteTrigger.User, null);
                log.Release();
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
        public Resource(ECustomNode owner) : base(owner)
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
