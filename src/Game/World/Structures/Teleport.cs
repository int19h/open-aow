// Translated from Teleport.pas.
using System.Runtime.CompilerServices;

using Aow.Engine.Nodes;
using Aow.Engine.Storage;
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
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.World.Structures;

// DestinationX, DestinationY, DestinationL, State (bit 0 razed)
public partial class Teleport : Structure
{
    public Teleport(ECustomNode owner) : base(owner)
    {
        _destinationX = 0;
        _destinationY = 0;
        _destinationL = 0;
    }

    // Teleport.pas:105
    public new const int ClassId = 131332;

    private sbyte _destinationX;
    private sbyte _destinationY;
    private sbyte _destinationL;
    public Set<StateElement> State;

    // Teleport.pas:122
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwByte(ref Unsafe.As<sbyte, byte>(ref _destinationX), 7);
        stream.rwByte(ref Unsafe.As<sbyte, byte>(ref _destinationY), 8);
        stream.rwByte(ref Unsafe.As<sbyte, byte>(ref _destinationL), 9);
        RwSet(ref State, 1, 10, stream.rwData, 0);
    }

    // Teleport.pas:97

    // Teleport.pas:157
    public override void MapFieldMessageProc(ref MapFieldMessage message)
    {
        base.MapFieldMessageProc(ref message);
        if (message.HexNumber == 0 && message.MessageId == 4416 && message.Param[1].As<MapField>().HexX == _destinationX && message.Param[1].As<MapField>().HexY == _destinationY && message.Param[1].As<MapField>().HexLevel == _destinationL)
        {
            bool notFogEnabled = !AoWHSMap.Current.FogEnabled;
            if (!notFogEnabled)
            {
                var hexLevel = GetHexLevel();
                var hexY = GetHexY();
                var hexX = GetHexX();
                notFogEnabled = AoWHSMap.Current.VisibleForPlayer(hexX, hexY, hexLevel, AoWHSMap.Current.SeatedPlayer);
            }

            if (notFogEnabled || AoWHSMap.Current.VisibleForPlayer(_destinationX, _destinationY, _destinationL, AoWHSMap.Current.SeatedPlayer))
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
            var fieldValue = AoWHSMap.Current.MapContainer.GetField(_destinationX, _destinationY, _destinationL);
            ((MoveControl)info.Sender).ExecuteExternalMove((AoWMapField)info.MapField, (AoWMapField)fieldValue, 0);
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
            ((AoWMapField)(object)info.MapField).FieldFlags = ((AoWMapField)(object)info.MapField).FieldFlags | 32768;
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
            messages.Add(AbilityRules.TranslateResourceString(GameText.WarningText) + ": " + AbilityRules.TranslateResourceString(s_invalidLocationSpecifiedText));
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
        animation = new ImageSequenceAnimation(null);
        animation.SetImageSequence(AoWHSSet.Current.ImageSequences.ImageSequences[13]);
        var hexLevel = GetHexLevel();
        var hexY = GetHexY();
        var hexX = GetHexX();
        AoWHSMap.Current.PlaceHex(animation, hexX, hexY, hexLevel);
        animation = new ImageSequenceAnimation(null);
        animation.SetImageSequence(AoWHSSet.Current.ImageSequences.ImageSequences[12]);
        AoWHSMap.Current.PlaceHex(animation, _destinationX, _destinationY, _destinationL);
    }

    // Teleport.pas:185
    public bool CanTeleportArmy(ArmyHexSprite armyHexSprite)
    {
        AoWMapField field;
        ArmyHexSprite otherArmy;
        bool result = false;
        if (_destinationL >= 0 && _destinationL < AoWHSMap.Current.MapContainer.LevelCount && _destinationX >= 0 && _destinationX < AoWHSMap.Current.MapContainer.Width && _destinationY >= 0 && _destinationY < AoWHSMap.Current.MapContainer.Height)
        {
            field = (AoWMapField)AoWHSMap.Current.MapContainer.GetField(_destinationX, _destinationY, _destinationL);
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
        AoWMapField field;
        TokenEvent eventValue;
        string messageText;
        if (CanTeleport(player))
        {
            armyHexSprite = (ArmyHexSprite)Owner.FindOwnedChild(131607);
            if (CanTeleportArmy(armyHexSprite))
            {
                armyHexSprite.Army.ResetDestinationPosition();
                path = new MovePath();
                path.AddMapCoordinate(_destinationX, _destinationY, _destinationL);
                var hexLevel = GetHexLevel();
                var hexY = GetHexY();
                path.AddMapCoordinate(GetHexX(), hexY, hexLevel);
                field = (AoWMapField)AoWHSMap.Current.MapContainer.GetField(_destinationX, _destinationY, _destinationL);
                bool condition = field.GetMoveAction(armyHexSprite.Army.Player) != MoveAction.Attack || field.GetPlayer() <= 0;
                if (!condition)
                {
                    var playerLocal = field.GetPlayer();
                    var diplomaticRelation = AoWHSMap.Current.Players.GetDiplomaticRelation(armyHexSprite.Army.Player, playerLocal);
                    condition = Set<DiplomaticRelation>.Of(DiplomaticRelation.War, DiplomaticRelation.Unknown).Contains(diplomaticRelation);
                }

                if (condition)
                {
                    if (field.GetPlayer() > -1 && field.GetPlayer() != armyHexSprite.Army.Player)
                    {
                        if (!AoWHSMap.Current.Players.GetPlayers(armyHexSprite.Army.Player).GetBusy())
                        {
                            // { Original passes action before mode. }
                            eventValue = Draw.ArmyHexSprite.CreateArmyMoveTokenEvent(armyHexSprite, 255, path, MoveAction.Attack, Set<ArmyMoveModeElement>.Of());
                            if (eventValue != null)
                            {
                                AoWHSMap.Current.TokenManager.GetTokenControl(armyHexSprite.Army.Player).AddEvent(eventValue);
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

                    if (result && (byte)AoWHSMap.Current.SeatedPlayer == (byte)armyHexSprite.Army.Player)
                    {
                        AoWHSMap.Current.CenterViewTo(_destinationX, _destinationY, _destinationL);
                        AoWHSMap.Current.SelectedArmy.Select(armyHexSprite);
                    }
                }
                else
                {
                    var playerLocal = field.GetPlayer();
                    var playerPlayerRace = AoWHSMap.Current.Players.GetPlayers(playerLocal);
                    var raceNamesPlural = AbilityRules.TranslateResourceString(GameTables.RaceNamesPluralTexts[playerPlayerRace.PlayerRace]);
                    messageText = RtlUnit.Format(AbilityRules.TranslateResourceString(GameText.NoWarDeclaredFormat), [raceNamesPlural]);
                    AoWHSMap.Current.ShowMessage(messageText);
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
        public override string ResourceEditName() => AbilityRules.TranslateResourceString(TeleportText);

        internal static string TeleportText { get; } = "Teleport";

    }
}
