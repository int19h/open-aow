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
public partial class Teleport : Structure
{
    public Teleport(ECustomNode owner) : base(owner)
    {
        _destinationX = 0;
        _destinationY = 0;
        _destinationMapLevel = 0;
    }

    // Teleport.pas:105
    public new const int ClassId = 131332;

    private sbyte _destinationX;
    private sbyte _destinationY;
    private sbyte _destinationMapLevel;

    public Set<StateElement> State { get; set; }

    // Teleport.pas:122
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        _destinationX = PersistSupport.UnsignedByte(persister, name: "DestinationX", value: _destinationX);
        _destinationY = PersistSupport.UnsignedByte(persister, name: "DestinationY", value: _destinationY);
        _destinationMapLevel = PersistSupport.UnsignedByte(persister, name: "DestinationMapLevel", value: _destinationMapLevel);
        State = persister.Set("State", value: State);
    }

    // Teleport.pas:97

    // Teleport.pas:157
    public override void MapFieldMessageProc(ref MapFieldMessage message)
    {
        base.MapFieldMessageProc(ref message);
        if (message.HexNumber == 0 && message.MessageId == 4416 && message.Param[1].As<MapField>().HexX == _destinationX && message.Param[1].As<MapField>().HexY == _destinationY && message.Param[1].As<MapField>().HexLevel == _destinationMapLevel)
        {
            bool notFogEnabled = !StrategicMap.Current.FogEnabled;
            if (!notFogEnabled)
            {
                var hexLevel = GetHexLevel();
                var hexY = GetHexY();
                var hexX = GetHexX();
                notFogEnabled = StrategicMap.Current.VisibleForPlayer(hexX, hexY, hexLevel, StrategicMap.Current.SeatedPlayer);
            }

            if (notFogEnabled || StrategicMap.Current.VisibleForPlayer(_destinationX, _destinationY, _destinationMapLevel, StrategicMap.Current.SeatedPlayer))
            {
                AudioPlayback.PlaySample("SFX\\PLACES\\TELEPORT.WAV");
            }

            ShowTeleportAnimations();
        }
    }

    // Teleport.pas:138
    public override void MoveExclusive(in HexSpriteSenderInfo info)
    {
        if (info.HexNumber == 0 && !State.Contains(StateElement.Razed))
        {
            var fieldValue = StrategicMap.Current.MapContainer.GetField(_destinationX, _destinationY, _destinationMapLevel);
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
        Status = Status - Set<MapObjectStatusElement>.Of(MapObjectStatusElement.Activated);
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
        if (value)
        {
            State = State + Set<StateElement>.Of(StateElement.Razed);
        }
        else
        {
            State = State - Set<StateElement>.Of(StateElement.Razed);
        }

        this.ImageIndex = GetTerrainTypeImage(TerrainType);
    }

    // Teleport.pas:279
    public override bool ValidateMap(StringCollection messages)
    {
        bool result;
        if (_destinationX == 0 || _destinationY == 0)
        {
            messages.Add(AbilityRules.TranslateResourceString(GetEngine(), GameText.WarningText) + ": " + AbilityRules.TranslateResourceString(GetEngine(), s_invalidLocationSpecifiedText));
            result = false;
        }
        else
        {
            result = true;
        }

        return result;
    }

    // Teleport.pas:145
    public void ShowTeleportAnimations()
    {
        ImageSequenceAnimation animation;
        animation = new ImageSequenceAnimation(null) { Holder = CreationHolder() };
        animation.SetImageSequence(GamePack.Current.ImageSequences.ImageSequences[13]);
        var hexLevel = GetHexLevel();
        var hexY = GetHexY();
        var hexX = GetHexX();
        StrategicMap.Current.PlaceHex(animation, hexX, hexY, hexLevel);
        animation = new ImageSequenceAnimation(null) { Holder = CreationHolder() };
        animation.SetImageSequence(GamePack.Current.ImageSequences.ImageSequences[12]);
        StrategicMap.Current.PlaceHex(animation, _destinationX, _destinationY, _destinationMapLevel);
    }

    // Teleport.pas:185
    public bool CanTeleportArmy(ArmyHexSprite armyHexSprite)
    {
        MapField field;
        ArmyHexSprite otherArmy;
        bool result = false;
        if (_destinationMapLevel >= 0 && _destinationMapLevel < StrategicMap.Current.MapContainer.LevelCount && _destinationX >= 0 && _destinationX < StrategicMap.Current.MapContainer.Width && _destinationY >= 0 && _destinationY < StrategicMap.Current.MapContainer.Height)
        {
            field = StrategicMap.Current.MapContainer.GetField(_destinationX, _destinationY, _destinationMapLevel);
            if (armyHexSprite.Army.ValidTerrain(field.TerrainType, field.OverlayType))
            {
                otherArmy = field.GetArmyHexSprite();
                // { Original rejects a same-player army when CanAddUnits succeeds. }
                if (otherArmy != null && (byte)armyHexSprite.Army.Player == (byte)otherArmy.Army.Player && otherArmy.Army.CanAddUnits(armyHexSprite.Army) != 0)
                {
                    return result;
                }

                result = true;
            }
        }

        return result;
    }

    // Teleport.pas:208
    public bool CanTeleport(sbyte player)
    {
        ArmyHexSprite armyHexSprite;
        armyHexSprite = (ArmyHexSprite)Owner.FindOwnedChild(131607);
        // { Original only checks destination X here; X=0 is rejected. }
        return armyHexSprite != null && player == armyHexSprite.Army.Player && !State.Contains(StateElement.Razed) && _destinationX > 0;
    }

    // Teleport.pas:218
    public bool TeleportPlayer(sbyte player)
    {
        bool result;
        ArmyHexSprite armyHexSprite;
        MovePath path;
        MapField field;
        TokenEvent eventValue;
        string messageText;
        if (CanTeleport(player))
        {
            armyHexSprite = (ArmyHexSprite)Owner.FindOwnedChild(131607);
            if (CanTeleportArmy(armyHexSprite))
            {
                armyHexSprite.Army.ResetDestinationPosition();
                path = new MovePath();
                path.AddMapCoordinate(_destinationX, _destinationY, _destinationMapLevel);
                var hexLevel = GetHexLevel();
                var hexY = GetHexY();
                path.AddMapCoordinate(GetHexX(), hexY, hexLevel);
                field = StrategicMap.Current.MapContainer.GetField(_destinationX, _destinationY, _destinationMapLevel);
                bool condition = field.GetMoveAction(armyHexSprite.Army.Player) != MoveAction.Attack || field.GetPlayer() <= 0;
                if (!condition)
                {
                    var playerLocal = field.GetPlayer();
                    var diplomaticRelation = StrategicMap.Current.Players.GetDiplomaticRelation(armyHexSprite.Army.Player, playerLocal);
                    condition = Set<DiplomaticRelation>.Of(DiplomaticRelation.War, DiplomaticRelation.Unknown).Contains(diplomaticRelation);
                }

                if (condition)
                {
                    if (field.GetPlayer() > -1 && field.GetPlayer() != armyHexSprite.Army.Player)
                    {
                        if (!StrategicMap.Current.Players.GetPlayers(armyHexSprite.Army.Player).GetBusy())
                        {
                            // { Original passes action before mode. }
                            eventValue = Draw.ArmyHexSprite.CreateArmyMoveTokenEvent(armyHexSprite, 255, path, MoveAction.Attack, Set<ArmyMoveModeElement>.Of());
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
                        result = Draw.ArmyHexSprite.MoveArmy(armyHexSprite, 255, path, Set<ArmyMoveModeElement>.Of(), false);
                    }

                    if (result && (byte)StrategicMap.Current.SeatedPlayer == (byte)armyHexSprite.Army.Player)
                    {
                        StrategicMap.Current.CenterViewTo(_destinationX, _destinationY, _destinationMapLevel);
                        StrategicMap.Current.SelectedArmy.Select(armyHexSprite);
                    }
                }
                else
                {
                    var playerLocal = field.GetPlayer();
                    var playerPlayerRace = StrategicMap.Current.Players.GetPlayers(playerLocal);
                    var raceNamesPlural = AbilityRules.TranslateResourceString(GetEngine(), GameTables.RaceNamesPluralTexts[playerPlayerRace.PlayerRace]);
                    messageText = RtlUnit.Format(AbilityRules.TranslateResourceString(GetEngine(), GameText.NoWarDeclaredFormat), [raceNamesPlural]);
                    StrategicMap.Current.ShowMessage(messageText);
                    result = false;
                }

                // { Original frees the path only on normal completion. }
                path?.Dispose();
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

        return result;
    }

    // Teleport.pas:26
    public override bool GetRazed() => (State.Image & 1) != 0;

    private static readonly string s_invalidLocationSpecifiedText = "invalid location specified";

    public enum StateElement : byte
    {
        Razed = 0, Bit1 = 1, Bit2 = 2, Bit3 = 3, Bit4 = 4, Bit5 = 5, Bit6 = 6, Bit7 = 7
    }

    public new partial class Resource : StructureResource
    {
        public Resource(ECustomNode owner) : base(owner)
        {
            RazeDefenderBudget = 200;
            Razeable = true;
            RebuildGold = 25;
            RebuildDays = 2;
        }

        // Teleport.pas:299
        public new const int ClassId = 131333;

        // Teleport.pas:290

        // Teleport.pas:305
        public override int ResourceUserClassId() => 131332;

        // Teleport.pas:45

        // Teleport.pas:302
        public override string ResourceEditName() => AbilityRules.TranslateResourceString(GetEngine(), TeleportText);

        internal static string TeleportText { get; } = "Teleport";

    }
}
