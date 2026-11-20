#nullable disable
// Translated from Teleport.pas.
using Aow.Engine;
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
public partial class Teleport(CustomNode owner) : Structure(owner)
{

    // Teleport.pas:105
    public new const int ClassId = 131332;

    private MapCoordinate Destination { get; set; }

    public Set<StateElement> State { get; set; }

    // Teleport.pas:122
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        Destination = PersistSupport.Coordinate(persister, name: "Destination", value: Destination, width: CoordinateWidth.Byte);
        State = persister.Set("State", value: State);
    }

    // Teleport.pas:157
    public override void MapFieldMessageProc(ref MapFieldMessage message)
    {
        base.MapFieldMessageProc(ref message);
        if (message.HexNumber == 0 && message.MessageId == 4416 && message.Param[1].As<MapField>().HexX == Destination.X && message.Param[1].As<MapField>().HexY == Destination.Y && message.Param[1].As<MapField>().HexLevel == Destination.MapLevel)
        {
            bool notFogEnabled = !StrategicMap.Current.FogEnabled;
            if (!notFogEnabled)
            {
                sbyte hexLevel = GetHexLevel();
                sbyte hexY = GetHexY();
                sbyte hexX = GetHexX();
                notFogEnabled = StrategicMap.Current.VisibleForPlayer(hexX, hexY, hexLevel, StrategicMap.Current.SeatedPlayer);
            }

            if (notFogEnabled || StrategicMap.Current.VisibleForPlayer(Destination.X, Destination.Y, Destination.MapLevel, StrategicMap.Current.SeatedPlayer))
            {
                _ = AudioPlayback.PlaySample("SFX\\PLACES\\TELEPORT.WAV");
            }

            ShowTeleportAnimations();
        }
    }

    // Teleport.pas:138
    public override void MoveExclusive(in HexSpriteSenderInfo info)
    {
        if (info.HexNumber == 0 && !State.Contains(StateElement.Razed))
        {
            MapField fieldValue = StrategicMap.Current.MapContainer.GetField(Destination.X, Destination.Y, Destination.MapLevel);
            ((MoveControl)info.Sender).ExecuteExternalMove(info.MapField, fieldValue, 0);
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
        Status -= Set<MapObjectStatusElement>.Of(MapObjectStatusElement.Activated);
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
        State = value ? State + Set<StateElement>.Of(StateElement.Razed) : State - Set<StateElement>.Of(StateElement.Razed);

        ImageIndex = GetTerrainTypeImage(TerrainType);
    }

    // Teleport.pas:279
    public override bool ValidateMap(DelphiStringCollection messages)
    {
        if (Destination.X == 0 || Destination.Y == 0)
        {
            _ = messages.Add($"{AbilityRules.TranslateResourceString(GetEngine(), GameText.WarningText)}: {AbilityRules.TranslateResourceString(GetEngine(), s_invalidLocationSpecifiedText)}");
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
        animation.SetImageSequence(GamePack.Current.ImageSequences.ImageSequences[13]);
        sbyte hexLevel = GetHexLevel();
        sbyte hexY = GetHexY();
        sbyte hexX = GetHexX();
        _ = StrategicMap.Current.PlaceHex(animation, hexX, hexY, hexLevel);
        animation = new ImageSequenceAnimation(null) { Holder = CreationHolder() };
        animation.SetImageSequence(GamePack.Current.ImageSequences.ImageSequences[12]);
        _ = StrategicMap.Current.PlaceHex(animation, Destination.X, Destination.Y, Destination.MapLevel);
    }

    // Teleport.pas:185
    public bool CanTeleportArmy(ArmyHexSprite armyHexSprite)
    {
        if (Destination.MapLevel >= 0 && Destination.MapLevel < StrategicMap.Current.MapContainer.LevelCount && Destination.X >= 0 && Destination.X < StrategicMap.Current.MapContainer.Width && Destination.Y >= 0 && Destination.Y < StrategicMap.Current.MapContainer.Height)
        {
            MapField field = StrategicMap.Current.MapContainer.GetField(Destination.X, Destination.Y, Destination.MapLevel);
            if (armyHexSprite.Army.ValidTerrain(field.TerrainType, field.OverlayType))
            {
                ArmyHexSprite otherArmy = field.GetArmyHexSprite();
                // { Original rejects a same-player army when CanAddUnits succeeds. }
                return otherArmy is null || (byte)armyHexSprite.Army.Player != (byte)otherArmy.Army.Player || otherArmy.Army.CanAddUnits(armyHexSprite.Army.Units) == 0;
            }
        }

        return false;
    }

    // Teleport.pas:208
    public bool CanTeleport(sbyte player)
    {
        ArmyHexSprite armyHexSprite = Owner.GetArmyHexSprite();
        // { Original only checks destination X here; X=0 is rejected. }
        return armyHexSprite is not null && player == armyHexSprite.Army.Player && !State.Contains(StateElement.Razed) && Destination.X > 0;
    }

    // Teleport.pas:218
    public bool TeleportPlayer(sbyte player)
    {
        bool result;
        if (CanTeleport(player))
        {
            ArmyHexSprite armyHexSprite = Owner.GetArmyHexSprite();
            if (CanTeleportArmy(armyHexSprite))
            {
                armyHexSprite.Army.ResetDestinationPosition();
                MovePath path = new();
                _ = path.AddMapCoordinate(Destination.X, Destination.Y, Destination.MapLevel);
                _ = path.AddMapCoordinate(mapLevel: GetHexLevel(), y: GetHexY(), x: GetHexX());
                MapField field = StrategicMap.Current.MapContainer.GetField(Destination.X, Destination.Y, Destination.MapLevel);
                bool condition = field.GetMoveAction(armyHexSprite.Army.Player) != MoveAction.Attack || field.GetPlayer() <= 0;
                if (!condition)
                {
                    sbyte playerLocal = field.GetPlayer();
                    DiplomaticRelation diplomaticRelation = StrategicMap.Current.Players.GetDiplomaticRelation(armyHexSprite.Army.Player, playerLocal);
                    condition = Set<DiplomaticRelation>.Of(DiplomaticRelation.War, DiplomaticRelation.Unknown).Contains(diplomaticRelation);
                }

                if (condition)
                {
                    if (field.GetPlayer() > -1 && field.GetPlayer() != armyHexSprite.Army.Player)
                    {
                        if (!StrategicMap.Current.Players.GetPlayers(armyHexSprite.Army.Player).GetBusy())
                        {
                            // { Original passes action before mode. }
                            TokenEvent eventValue = ArmyHexSprite.CreateArmyMoveTokenEvent(armyHexSprite, 255, path, MoveAction.Attack, Set<ArmyMoveModeElement>.Of());
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
                        result = ArmyHexSprite.MoveArmy(armyHexSprite, 255, path, Set<ArmyMoveModeElement>.Of(), false);
                    }

                    if (result && (byte)StrategicMap.Current.SeatedPlayer == (byte)armyHexSprite.Army.Player)
                    {
                        StrategicMap.Current.CenterViewTo(Destination.X, Destination.Y, Destination.MapLevel);
                        StrategicMap.Current.SelectedArmy.Select(armyHexSprite);
                    }
                }
                else
                {
                    sbyte playerLocal = field.GetPlayer();
                    Player playerPlayerRace = StrategicMap.Current.Players.GetPlayers(playerLocal);
                    string raceNamesPlural = AbilityRules.TranslateResourceString(GetEngine(), GameTables.RaceNamesPluralTexts[playerPlayerRace.PlayerRace]);
                    string messageText = DelphiFormat.Format(AbilityRules.TranslateResourceString(GetEngine(), GameText.NoWarDeclaredFormat), [raceNamesPlural]);
                    StrategicMap.Current.ShowMessage(messageText);
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
    public override bool GetRazed() => (State.Image & 1) != 0;

    private static readonly string s_invalidLocationSpecifiedText = "invalid location specified";

    public enum StateElement : byte
    {
        Razed = 0, Bit1 = 1, Bit2 = 2, Bit3 = 3, Bit4 = 4, Bit5 = 5, Bit6 = 6, Bit7 = 7
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
        public new const int ClassId = 131333;

        // Teleport.pas:305
        public override int ResourceUserClassId() => 131332;

        // Teleport.pas:302
        public override string ResourceEditName() => AbilityRules.TranslateResourceString(GetEngine(), TeleportText);

        internal static string TeleportText { get; } = "Teleport";

    }
}
