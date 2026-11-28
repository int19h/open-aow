#nullable disable warnings
// Translated from Teleport.pas.
using Aow.Engine.Nodes;
using Aow.Formats.Tree;
using Aow.Game.Audio;
using Aow.Game.Commands;
using Aow.Game.Draw;
using Aow.Game.Draw.Animations;

using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Game.Units;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

using Aow.Rtl;

namespace Aow.Game.World.Structures;

// DestinationX, DestinationY, DestinationMapLevel, State (bit 0 razed)
public partial class Teleport(CustomNode? owner) : Structure(owner)
{

    // Teleport.pas:105
    public new const int ClassId = 131_332;

    private MapCoordinate Destination { get; set; }

    public States State { get; set; }

    // Teleport.pas:122
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        Destination = persister.Coordinate(name: "Destination", value: Destination, width: CoordinateWidth.Byte);
        State = persister.Set("State", value: State);
    }

    // Teleport.pas:157
    public override void MapFieldMessageProc(ref MapFieldMessage message)
    {
        base.MapFieldMessageProc(ref message);
        if (message.HexNumber == 0 && message.MessageId == 4416 && message.Param[1].As<MapField>().HexX == Destination.X && message.Param[1].As<MapField>().HexY == Destination.Y && message.Param[1].As<MapField>().HexLevel == Destination.MapLevel)
        {
            StrategicMap strategicMap = GetEngine().StrategicMap;
            bool notFogEnabled = !strategicMap.FogEnabled;
            if (!notFogEnabled)
            {
                int hexLevel = HexLevel;
                int hexY = HexY;
                int hexX = HexX;
                notFogEnabled = strategicMap.VisibleForPlayer(hexX, hexY, hexLevel, player: strategicMap.SeatedPlayer);
            }

            if (notFogEnabled || GetEngine().StrategicMap.VisibleForPlayer(Destination.X, Destination.Y, Destination.MapLevel, player: GetEngine().StrategicMap.SeatedPlayer))
            {
                _ = AudioPlayback.PlaySample("SFX\\PLACES\\TELEPORT.WAV");
            }

            ShowTeleportAnimations();
        }
    }

    // Teleport.pas:138
    public override void MoveExclusive(in HexSpriteSenderInfo info)
    {
        if (info.HexNumber == 0 && !State.Contains(States.Razed))
        {
            MapField fieldValue = GetEngine().StrategicMap.MapContainer.GetField(Destination.X, Destination.Y, Destination.MapLevel);
            ((MoveControl)info.Sender).ExecuteExternalMove(info.MapField, target: fieldValue, movePoints: 0);
        }
    }

    // Teleport.pas:175
    public override void Activate()
    {
        base.Activate();
        Owner.UpdateMapField();
    }

    // Teleport.pas:178
    public override void Deactivate()
    {
        base.Deactivate();
        Status &= ~MapObjectStatuses.Activated;
        Owner.UpdateMapField();
    }

    // Teleport.pas:131
    public override void UpdateMapField(in HexSpriteMapFieldInfo info)
    {
        if (info.HexNumber == 0)
        {
            info.MapField.FieldFlags = info.MapField.FieldFlags | MapField.MoveExclusiveFlag;
        }
    }

    // Teleport.pas:115
    public override void SetRazed(bool value)
    {
        State = value ? State | States.Razed : State & ~States.Razed;

        ImageIndex = GetTerrainTypeImage(TerrainType);
    }

    // Teleport.pas:279
    public override bool ValidateMap(DelphiStringCollection messages)
    {
        if (Destination.X == 0 || Destination.Y == 0)
        {
            _ = messages.Add($"{AbilityRules.TranslateResourceString(GetEngine(), S: GameText.WarningText)}: {AbilityRules.TranslateResourceString(GetEngine(), S: s_invalidLocationSpecifiedText)}");
            return false;
        }
        else
        {
            return true;
        }
    }

    // Teleport.pas:145
    public void ShowTeleportAnimations()
    {
        ImageSequenceAnimation animation = new(null) { Holder = CreationHolder() };
        animation.SetImageSequence(GetEngine().GamePack.ImageSequences.ImageSequences[13]);
        int hexLevel = HexLevel;
        int hexY = HexY;
        int hexX = HexX;
        _ = GetEngine().StrategicMap.PlaceHex(animation, hexX, hexY, L: hexLevel);
        animation = new ImageSequenceAnimation(null) { Holder = CreationHolder() };
        animation.SetImageSequence(GetEngine().GamePack.ImageSequences.ImageSequences[12]);
        _ = GetEngine().StrategicMap.PlaceHex(animation, Destination.X, Destination.Y, L: Destination.MapLevel);
    }

    // Teleport.pas:185
    public bool CanTeleportArmy(ArmyHexSprite armyHexSprite)
    {
        if (Destination.MapLevel >= 0 && Destination.MapLevel < GetEngine().StrategicMap.MapContainer.LevelCount && Destination.X >= 0 && Destination.X < GetEngine().StrategicMap.MapContainer.Width && Destination.Y >= 0 && Destination.Y < GetEngine().StrategicMap.MapContainer.Height)
        {
            MapField field = GetEngine().StrategicMap.MapContainer.GetField(Destination.X, Destination.Y, Destination.MapLevel);
            if (armyHexSprite.Army.ValidTerrain(terrainType: field.TerrainNumber, overlayType: field.OverlayType.ToOriginal()))
            {
                ArmyHexSprite otherArmy = field.GetArmyHexSprite();
                // { Original rejects a same-player army when CanAddUnits succeeds. }
                return otherArmy is null || (byte)PlayerIdExtensions.ToOriginal(armyHexSprite.Army.Player) != (byte)PlayerIdExtensions.ToOriginal(otherArmy.Army.Player) || otherArmy.Army.CanAddUnits(armyHexSprite.Army.Units) == 0;
            }
        }

        return false;
    }

    // Teleport.pas:208
    public bool CanTeleport(PlayerId? player)
    {
        ArmyHexSprite armyHexSprite = Owner.GetArmyHexSprite();
        // { Original only checks destination X here; X=0 is rejected. }
        return armyHexSprite is not null && player == armyHexSprite.Army.Player && !State.Contains(States.Razed) && Destination.X > 0;
    }

    // Teleport.pas:218
    public bool TeleportPlayer(PlayerId? player)
    {
        bool result;
        if (CanTeleport(player))
        {
            ArmyHexSprite armyHexSprite = Owner.GetArmyHexSprite();
            if (CanTeleportArmy(armyHexSprite))
            {
                armyHexSprite.Army.ResetDestinationPosition();
                MovePath path = [];
                _ = path.AddMapCoordinate(Destination.X, Destination.Y, Destination.MapLevel);
                _ = path.AddMapCoordinate(mapLevel: HexLevel, y: HexY, x: HexX);
                MapField field = GetEngine().StrategicMap.MapContainer.GetField(Destination.X, Destination.Y, Destination.MapLevel);
                bool condition = field.GetMoveAction(armyHexSprite.Army.Player) != MoveAction.Attack || PlayerIdExtensions.ToOriginal(field.GetPlayer()) <= 0;
                if (!condition)
                {
                    sbyte playerLocal = (sbyte)PlayerIdExtensions.ToOriginal(field.GetPlayer());
                    DiplomaticRelation diplomaticRelation = GetEngine().StrategicMap.Players.GetDiplomaticRelation(armyHexSprite.Army.Player, otherPlayer: PlayerId.FromOriginal(playerLocal));
                    condition = diplomaticRelation is DiplomaticRelation.War or DiplomaticRelation.Unknown;
                }

                if (condition)
                {
                    if (PlayerIdExtensions.ToOriginal(field.GetPlayer()) > -1 && field.GetPlayer() != armyHexSprite.Army.Player)
                    {
                        if (!GetEngine().StrategicMap.Players.GetPlayers(armyHexSprite.Army.Player).GetBusy())
                        {
                            // { Original passes action before mode. }
                            TokenEvent? eventValue = ArmyHexSprite.CreateArmyMoveTokenEvent(armyHexSprite, selection: 255, path, action: MoveAction.Attack, mode: ArmyMoveModes.None);
                            if (eventValue is not null)
                            {
                                GetEngine().StrategicMap.TokenManager.GetTokenControl(armyHexSprite.Army.Player).AddEvent(eventValue);
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
                        result = ArmyHexSprite.MoveArmy(armyHexSprite, selection: 255, path, mode: ArmyMoveModes.None, force: false);
                    }

                    if (result && (byte)PlayerIdExtensions.ToOriginal(GetEngine().StrategicMap.SeatedPlayer) == (byte)PlayerIdExtensions.ToOriginal(armyHexSprite.Army.Player))
                    {
                        GetEngine().StrategicMap.CenterViewTo(Destination.X, Destination.Y, Destination.MapLevel);
                        GetEngine().StrategicMap.SelectedArmy.Select(armyHexSprite);
                    }
                }
                else
                {
                    sbyte playerLocal = (sbyte)PlayerIdExtensions.ToOriginal(field.GetPlayer());
                    Player? playerPlayerRace = GetEngine().StrategicMap.Players.GetPlayers(playerLocal);
                    string raceNamesPlural = AbilityRules.TranslateResourceString(GetEngine(), S: GameTables.RaceNamesPluralTexts[playerPlayerRace.PlayerRace]);
                    string messageText = DelphiFormat.Format(AbilityRules.TranslateResourceString(GetEngine(), S: GameText.NoWarDeclaredFormat), args: [raceNamesPlural]);
                    GetEngine().StrategicMap.ShowMessage(messageText);
                    result = false;
                }

                // { Original frees the path only on normal completion. }
                path?.Dispose();
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }

        return result;
    }

    // Teleport.pas:26
    public override bool GetRazed() => ((ulong)State & 1) != 0;

    private static readonly string s_invalidLocationSpecifiedText = "invalid location specified";

    [Flags]
    public enum States : byte
    {
        None = 0,
        Razed = 1 << 0,
        Bit1 = 1 << 1,
        Bit2 = 1 << 2,
        Bit3 = 1 << 3,
        Bit4 = 1 << 4,
        Bit5 = 1 << 5,
        Bit6 = 1 << 6,
        Bit7 = 1 << 7,
    }

    public new class Resource : StructureResource
    {
        public Resource(CustomNode owner) : base(owner)
        {
            RazeDefenderBudget = 200;
            Razeable = true;
            RebuildGold = 25;
            RebuildDays = 2;
        }

        // Teleport.pas:299
        public new const int ClassId = 131_333;

        // Teleport.pas:305
        public override int ResourceUserClassId() => Teleport.ClassId;

        // Teleport.pas:302
        public override string ResourceEditName() => AbilityRules.TranslateResourceString(GetEngine(), S: TeleportText);

        internal static string TeleportText { get; } = "Teleport";

    }
}
