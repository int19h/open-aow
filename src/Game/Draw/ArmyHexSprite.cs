#nullable disable
// Translated from AoWE.pas.

using Aow.Engine.Nodes;
using Aow.Formats.Tree;
using Aow.Game.AI;
using Aow.Game.Combat;

using Aow.Game.Commands;

using Aow.Game.Draw.Animations;

using Aow.Game.Events;

using Aow.Game.Magic.Abilities;

using Aow.Game.Magic.Spells;

using Aow.Game.Session;

using Aow.Game.Units;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Game.World.Structures;

using Aow.Rtl;

using static Aow.Rtl.DelphiAssert;

namespace Aow.Game.Draw;

// Army. UnitIndex. UnitYOffset. MoveSteps
public partial class ArmyHexSprite : UnitHexSprite
{

    public new const int ClassId = 131607;

    public Army Army { get; private set; }

    private byte _unitIndex;
    private sbyte _unitYOffset;

    public sbyte MoveSteps { get; private set; }

    // Army.PAS:2654
    public override void Dispose()
    {
        Army.ClearUpdateHandler();
        base.Dispose();
        Army?.Dispose();
        Army = null;
    }

    // Army.PAS:3054
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        persister.Object("Army", objectValue: Army);
    }

    // Army.PAS:2648
    public ArmyHexSprite(CustomNode owner) : base(owner) => Army = new Army(null) { Holder = CreationHolder() };

    // Army.PAS:2684
    public override void Clear()
    {
        base.Clear();
        Army.Clear();
    }

    // Army.PAS:3414
    public override void MessageProc(ref EngineMessage message, EngineObject sender)
    {
        base.MessageProc(ref message, sender);
        if ((message.MessageId & 1073741824) == 0 && Army is not null)
        {
            Army.MessageProc(ref message, sender);
        }

        if (message.MessageId == 537001989)
        {
            UpdateFlagId();
        }
    }

    // Army.PAS:2740
    public override bool GetSelected() => Army.Status.Contains(ArmyStatusElement.Selected);

    // Army.PAS:3060
    public override bool CanChangeTerrain(in HexSpriteSenderInfo info)
    {
        bool result = info.HexNumber != 0 || (info.MapField.TerrainType == 0 && info.Sender is not null && (((EngineObject)info.Sender).EngineClass.ClassId == 135742 || ((EngineObject)info.Sender).EngineClass.ClassId == 131436)) || Army.ValidTerrain(info.MapField.TerrainType, info.MapField.OverlayType);
        return result;
    }

    // Army.PAS:3069
    public override void TerrainChanged(in HexSpriteTerrainInfo info)
    {
        base.TerrainChanged(info);
        if (info.HexNumber == 0 && Army.Status.Contains(ArmyStatusElement.Activated))
        {
            using (Army.BeginUpdate())
            {
                Army.ValidateTerrain(info.TerrainType, info.OverlayType);
            }
        }
    }

    // Army.PAS:2690
    public override bool PlaceOnMap()
    {
        MapFieldMessage message = default;
        bool result = base.PlaceOnMap();
        if (result)
        {
            using (Army.BeginUpdate())
            {
                MapField mapField = Owner;
                Army.SetMapCoordinate(mapField.HexX, mapField.HexY, mapField.HexLevel);

                Army.UpdateUnfog();
                Army.UpdateExploration();
                message.MessageId = 4354;
                MapFieldMessageParameters param = message.Param;
                param[0] = this;
                message.Param = param;
                Owner.SendMapFieldMessage(ref message);
            }
        }

        return result;
    }

    // Army.PAS:2707
    public override void RemoveFromMap()
    {
        MapFieldMessage message = default;
        MapField field = Owner;
        if (Owner is not null)
        {
            Army.TurnUnFogOff();
        }

        base.RemoveFromMap();
        if (field is not null && StrategicMap.Current is not null && !StrategicMap.Current.Status.Contains(HexSpriteMapStatusElement.Destroying))
        {
            message.MessageId = 4355;
            MapFieldMessageParameters param = message.Param;
            param[0] = this;
            message.Param = param;
            field.SendMapFieldMessage(ref message);
        }
    }

    // Army.PAS:3421
    public override void MapFieldMessageProc(ref MapFieldMessage message)
    {
        base.MapFieldMessageProc(ref message);
        if (message.HexNumber == 0)
        {
            if (message.MessageId == 4364)
            {
                if (!Army.Status.Contains(ArmyStatusElement.FullyConcealed) && (!StrategicMap.Current.FogEnabled || Owner.FogCount != 0))
                {
                    if (Army.Player != 0)
                    {
                        Player players = StrategicMap.Current.Players.GetPlayers(Army.Player);
                        MapFieldMessageParameters param = message.Param;
                        param[0] = param[0].Int | (players.PlayerRace + 1);
                        message.Param = param;
                    }
                    else
                    {
                        MapFieldMessageParameters param2 = message.Param;
                        param2[0] = param2[0].Int | 13;
                        message.Param = param2;
                    }
                }
            }
            else
            {
                if (message.MessageId == 4608)
                {
                    AITargetData data = message.Param[0].As<AITargetQuery>().Data;
                    Army.UpdateAITarget(message.Param[0].As<AITargetQuery>().Info, ref data);
                    message.Param[0].As<AITargetQuery>().Data = data;
                    if (message.Param[0].As<AITargetQuery>().Data.Priority < 0)
                    {
                        message.Result = 1;
                    }
                }
                else
                {
                    if (message.MessageId == 4360)
                    {
                        BattleFieldInfo info = message.Param[0].As<BattleFieldQuery>().Info;
                        Army.ReturnBattleFieldInfo(ref info);
                        message.Param[0].As<BattleFieldQuery>().Info = info;
                    }
                    else
                    {
                        if (message.MessageId == 4353)
                        {
                            MoveAction moveAction = Army.GetMoveAction((sbyte)message.Param[0].Int);
                            MapFieldMessageParameters param3 = message.Param;
                            param3[1] = (int)moveAction;
                            message.Param = param3;
                        }
                        else
                        {
                            if (message.MessageId == 4352)
                            {
                                message.Result = Army.Player;
                            }
                            else
                            {
                                if (message.MessageId == 4389)
                                {
                                    if (Army.Player == 0)
                                    {
                                        message.Result = Army.IndependentRelation;
                                    }
                                }
                                else
                                {
                                    if (message.MessageId == 4400)
                                    {
                                        if (message.Param[1].Int == 1)
                                        {
                                            message.Result = Army.Player;
                                        }
                                        else
                                        {
                                            if (Army.ConcealedUnits != AbilityRules.ArmySelectionMasks[Army.Units.Count])
                                            {
                                                message.Result = Army.Player;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (message.MessageId == 4362)
                                        {
                                            if (Army.Player == message.Param[1].Int || StrategicMap.Current.Players.GetPlayers(Army.Player).DiplomaticRelations.GetDiplomaticRelation((sbyte)message.Param[1].Int) == DiplomaticRelation.Alliance)
                                            {
                                                byte visibilityRange = Army.VisibilityRange();
                                                if (message.Param[0].Int <= visibilityRange)
                                                {
                                                    message.Result = 1;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (message.MessageId == 4363)
                                            {
                                                if (Army.Player == message.Param[1].Int || StrategicMap.Current.Players.GetPlayers(Army.Player).DiplomaticRelations.GetDiplomaticRelation((sbyte)message.Param[1].Int) == DiplomaticRelation.Alliance)
                                                {
                                                    byte trueVisionRange = Army.TrueVisionRange();
                                                    if (message.Param[0].Int <= trueVisionRange)
                                                    {
                                                        message.Result = 1;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (message.MessageId == 4365)
                                                {
                                                    if (message.Param[0].Int <= 8)
                                                    {
                                                        Army.UpdateConcealment();
                                                    }

                                                    bool condition = Army.Player > 0;
                                                    if (condition)
                                                    {
                                                        byte visibilityRange = Army.VisibilityRange();
                                                        condition = message.Param[0].Int <= visibilityRange;
                                                    }

                                                    if (condition)
                                                    {
                                                        byte trueVisionRange = Army.TrueVisionRange();
                                                        StrategicMap.Current.UpdatePlayerSeen(Army.Player, message.MapField.HexX, message.MapField.HexY, message.MapField.HexLevel, message.Param[0].Int, trueVisionRange);
                                                    }
                                                }
                                                else
                                                {
                                                    if (message.MessageId == 4366)
                                                    {
                                                        if (Army.IncomingNegotiateRequest(message.Param[0].As<NegotiateRequestEventLog>()))
                                                        {
                                                            MapFieldMessageParameters param4 = message.Param;
                                                            param4[1] = 1;
                                                            message.Param = param4;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (message.MessageId == 4367)
                                                        {
                                                            if (Army.IncomingNegotiateOffer(message.Param[0].As<NegotiateTokenEvent>()))
                                                            {
                                                                message.Result = 1;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (message.MessageId == 4432 && !message.MapField.GetMagicShieldActive())
                                                            {
                                                                Army.IncomingStorm((StormType)message.Param[0].Int, message.Param[1].Int, (sbyte)message.Param[2].Int);
                                                                return;
                                                            }
                                                            else
                                                            {
                                                                if (message.MessageId == 4433 && !message.MapField.GetMagicShieldActive())
                                                                {
                                                                    StormDamageInfo info2 = message.Param[2].As<StormDamageQuery>().Info;
                                                                    Army.RetrieveStormDamageInfo((StormType)message.Param[0].Int, message.Param[1].Int, ref info2);
                                                                    message.Param[2].As<StormDamageQuery>().Info = info2;
                                                                    return;
                                                                }
                                                                else
                                                                {
                                                                    if (message.MessageId == 4357)
                                                                    {
                                                                        Army.TriggerFireDamage();
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    // Army.PAS:3114
    public override void CanMoveOn(in HexSpriteMoveOnInfo info, ref HexSpriteMoveOnResult moveResult)
    {
        if (info.HexNumber == 0)
        {
            MoveControl control = (MoveControl)info.Sender;
            if (Army.Player == control.Player)
            {
                if (!(control.Mode.Contains(MoveModeElement.Surround) && info.MoveOverCode != 1 && Owner.HexX == control.Destination.X && Owner.HexY == control.Destination.Y && Owner.HexLevel == control.Destination.MapLevel && !Army.CanCombine(control.ArmyHexSprite.Army, (byte)control.Selection)))
                {
                    if (control.ArmyHexSprite is not null && Army.IndexOfTransporter(255) != -1 && control.ArmyHexSprite.Army.Transporter((byte)control.Selection) is null)
                    {
                        moveResult.Code = 4;
                        moveResult.Done = true;
                    }
                }
                else
                {
                    moveResult.Code = -1;
                    moveResult.Done = true;
                }
            }
            else
            {
                if (info.MoveOverCode != 1)
                {
                    moveResult.Code = -1;
                    moveResult.Done = true;
                }
            }
        }
    }

    // Army.PAS:3080
    public override int CanMoveOver(in HexSpriteSenderInfo info)
    {
        int result;
        if (info.HexNumber == 0)
        {
            if (!((MoveControl)info.Sender).Mode.Contains(MoveModeElement.AI) && (GetFogEnabled() || Army.Status.Contains(ArmyStatusElement.FullyConcealed) || ((MoveControl)info.Sender).Mode.Contains(MoveModeElement.IgnoreArmies)))
            {
                return 1;
            }

            if (((MoveControl)info.Sender).Player == Army.Player)
            {
                if (!((MoveControl)info.Sender).Mode.Contains(MoveModeElement.AvoidArmies) || this == ((MoveControl)info.Sender).TransporterArmy)
                {
                    if (Army.Player != 0)
                    {
                        if (((MoveControl)info.Sender).Mode.Contains(MoveModeElement.AI) && Army.IndexOfTransporter(255) != -1)
                        {
                            return 2;
                        }

                        result = ((MoveControl)info.Sender).ArmyHexSprite is not null && Army.CanCombine(((MoveControl)info.Sender).ArmyHexSprite.Army, (byte)((MoveControl)info.Sender).Selection)
                            ? 1
                            : 2;
                    }
                    else
                    {
                        result = 2;
                    }
                }
                else
                {
                    result = 0;
                }
            }
            else
            {
                result = StrategicMap.Current.Players.GetDiplomaticRelation(((MoveControl)info.Sender).Player, Army.Player) == DiplomaticRelation.Alliance
                    ? info.MapField.OverlayType != 7 || info.MapField.FindNoneTransparentHexSprite(typeof(PlayerStructure)) is null ? 2 : 0
                    : 0;
            }
        }
        else
        {
            result = 1;
        }

        return result;
    }

    // Army.PAS:3030
    public override void Activate()
    {
        base.Activate();
        sbyte hexLevel = GetHexLevel();
        sbyte hexY = GetHexY();
        sbyte hexX = GetHexX();
        Army.SetMapCoordinate(hexX, hexY, hexLevel);
        Army.Activate();
        Army.SetUpdateHandler(ArmyUpdated);
        if (this == Owner.GetArmyHexSprite())
        {
            if (Army.Units.Count != 0)
            {
                UpdateUnitGraphicsResource();
            }
            else
            {
                if (!StrategicMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting))
                {
                    Dispose();
                    return;
                }
            }
        }
        else
        {
            Dispose();
        }
    }

    // Army.PAS:3048
    public override void Deactivate()
    {
        base.Deactivate();
        Army.Deactivate();
    }

    // Army.PAS:3135
    public override void UpdateMapField(in HexSpriteMapFieldInfo info)
    {
        if (info.HexNumber == 0)
        {
            info.MapField.FieldFlags = info.MapField.FieldFlags | MapField.MoveOverFlag | MapField.ViewerFlag | MapField.PlayerChangeListenerFlag | MapField.MoveOnFlag | MapField.OwnerFlag | MapField.BattleFieldInfoFlag | MapField.AITargetFlag;
        }
    }

    // Army.PAS:2734
    public override bool CanSelect(int pixelX, int pixelY)
    {
        bool result = !GetFogEnabled() && StrategicMap.Current.SeatedPlayer != -1 && !Army.Status.Contains(ArmyStatusElement.FullyConcealed) && base.CanSelect(pixelX, pixelY);
        return result;
    }

    // Army.PAS:2721
    public override void Select()
    {
        Army.Select();
        if (this != StrategicMap.Current.SelectedArmy.GetArmyHexSprite())
        {
            StrategicMap.Current.SelectedArmy.SetArmyHexSprite(this);
        }
    }

    // Army.PAS:2727
    public override void Unselect()
    {
        base.Unselect();
        Army.Unselect();
        if (this == StrategicMap.Current.SelectedArmy.GetArmyHexSprite())
        {
            StrategicMap.Current.SelectedArmy.SetArmyHexSprite(null);
        }
    }

    // Army.PAS:2679
    public override string EditName() => AbilityRules.TranslateResourceString(GetEngine(), s_armyText);

    // Army.PAS:3140
    public override bool MoveTo(MapField field, int moveCost)
    {
        bool result;
        MapFieldMessage message = default;
        using (Army.BeginUpdate())
        {
            MapField oldField = Owner;
            if (oldField is not null)
            {
                message.MessageId = 4416;
                MapFieldMessageParameters param = message.Param;
                param[0] = this;
                message.Param = param;
                MapFieldMessageParameters param2 = message.Param;
                param2[1] = field;
                message.Param = param2;
                message.Result = 0;
                oldField.SendMapFieldMessage(ref message);
            }

            if (oldField is null || message.Result.Int == 0)
            {
                message.MessageId = 4417;
                MapFieldMessageParameters param3 = message.Param;
                param3[0] = this;
                message.Param = param3;
                MapFieldMessageParameters param4 = message.Param;
                param4[1] = oldField;
                message.Param = param4;
                message.Result = 0;
                field.SendMapFieldMessage(ref message);
                if (message.Result.Int == 0)
                {
                    _ = base.MoveTo(field, moveCost);
                    if (oldField is not null && (field.HexLevel != oldField.HexLevel || !ConnectingHex(oldField.HexX, oldField.HexY, field.HexX, field.HexY)))
                    {
                        StrategicMap.Current.NotifyPlayerChanged(oldField.HexX, oldField.HexY, oldField.HexLevel);
                        StrategicMap.Current.NotifyPlayerChanged(field.HexX, field.HexY, field.HexLevel);
                    }
                    else
                    {
                        StrategicMap.Current.NotifyPlayerChanged(field.HexX, field.HexY, field.HexLevel, 13);
                    }

                    Army.Validate();
                    Army.MovedTo(oldField, field, moveCost);
                    message.MessageId = 4418;
                    MapFieldMessageParameters param5 = message.Param;
                    param5[0] = this;
                    message.Param = param5;
                    MapFieldMessageParameters param6 = message.Param;
                    param6[1] = oldField;
                    message.Param = param6;
                    message.Result = 0;
                    field.SendMapFieldMessage(ref message);
                    if (Army.Player > 0)
                    {
                        byte visibilityRange = Army.VisibilityRange();
                        byte trueVisionRange = Army.TrueVisionRange();
                        StrategicMap.Current.UpdatePlayerSeen(Army.Player, field.HexX, field.HexY, field.HexLevel, visibilityRange, trueVisionRange);
                    }

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
        return result;
    }

    // Army.PAS:3187
    public override bool PlaceHex(MapContainer map, sbyte hexX, sbyte hexY, sbyte hexLevel)
    {
        bool result;
        if (Army.CanPlace(hexX, hexY, hexLevel))
        {
            MapField field = map.GetField(hexX, hexY, hexLevel);
            EngineObject army = field.FindOwnedChild(131607);
            result = (army is null || GetEditMode()) && base.PlaceHex(map, hexX, hexY, hexLevel);
        }
        else
        {
            result = false;
        }

        return result;
    }

    // Army.PAS:2760
    public override void UpdateFlagId()
    {
        if (Army.Player > 0)
        {
            FlagId = (byte)StrategicMap.Current.Players.GetPlayers(Army.Player).PlayerRace;
            return;
        }

        FlagId = 32;
    }

    // Army.PAS:3517
    public override sbyte MoveFirst(int direction, bool instant)
    {
        sbyte[] resultCell = new sbyte[1];
        ref sbyte result = ref resultCell[0];
        result = base.MoveFirst(direction, instant);
        switch (GetEngine().Registry.GetMovementSpeed())
        {
            case GameSettings.MovementSpeed.Normal:
                MoveSteps = (sbyte)base.GetMoveSteps();
                break;
            case GameSettings.MovementSpeed.Fast:
                MoveSteps = (sbyte)base.GetMoveSteps();
                MoveSteps = MoveSteps > 8 ? (sbyte)(MoveSteps - 4) : (sbyte)4;

                break;
            case GameSettings.MovementSpeed.Turbo:
                MoveSteps = 2;
                break;
            default:
                break;
        }

        if (StrategicMap.Current.FPS > 0)
        {
            if (StrategicMap.Current.FPS < 15)
            {
                MoveSteps = (sbyte)(MoveSteps / 2);
            }
            else
            {
                if (StrategicMap.Current.FPS < 25)
                {
                    MoveSteps = (sbyte)(MoveSteps - 2);
                }
            }

            if (MoveSteps < 2)
            {
                MoveSteps = 2;
            }
        }

        if (GetHexLevel() > 0)
        {
            MapLevel level = Owner.Owner;
            sbyte hexY = GetHexY();
            int x = default;
            int y = default;
            HexGeometry.CenterHexNumberToHex(GetHexX(), hexY, direction, ref x, ref y);
            if (x >= 0 && y >= 0 && x < level.Width && y < level.Height)
            {
                if (level.GetField(x, y).TerrainType == 7)
                {
                    if (GamePack.Current.AbilityControl.GetAbility(42).ImageSequences.GetImageSequence(0) is not null)
                    {
                        ImageSequenceAnimation animation = new(null) { Holder = CreationHolder() };
                        animation.SetImageSequence(GamePack.Current.AbilityControl.GetAbility(42).ImageSequences.GetImageSequence(0));
                        animation.AnimationLevel = 27;
                        int deltaX = default;
                        int deltaY = default;
                        HexGeometry.HexNumberToPixel(direction, ref deltaX, ref deltaY);
                        animation.SetPixelXOffset(deltaX / 2);
                        animation.SetPixelYOffset(deltaY / 2);
                        sbyte hexLevel = GetHexLevel();
                        sbyte hexY2 = GetHexY();
                        sbyte hexX = GetHexX();
                        _ = StrategicMap.Current.PlaceHex(animation, hexX, hexY2, hexLevel);
                    }
                }
            }
        }

        return result;
    }

    // Army.PAS:3555
    public override int GetMoveSteps() => Moving() ? MoveSteps : base.GetMoveSteps();

    // Army.PAS:2667
    public bool GetFogEnabled() => StrategicMap.Current.FogEnabled && Owner.FogCount == 0;

    // Army.PAS:2673
    public bool VisibleForSeatedPlayer() => (!StrategicMap.Current.FogEnabled || Owner.FogCount != 0) && !Army.GetFullyConcealed();

    // Army.PAS:2745
    public void ArmyUpdated(object sender)
    {
        MapFieldMessage message = default;
        if (Army.Units.Count != 0)
        {
            message.MessageId = 4388;
            MapFieldMessageParameters param = message.Param;
            param[0] = this;
            message.Param = param;
            Owner.SendMapFieldMessage(ref message);
            UpdateUnitGraphicsResource();
            FlagId = Army.Player != 0 ? (byte)StrategicMap.Current.Players.GetPlayers(Army.Player).PlayerRace : (byte)32;
        }
        else
        {
            if (sender == Army)
            {
                Dispose();
            }
        }
    }

    // Army.PAS:2770
    public void UpdateUnitGraphicsResource()
    {
        int i;
        if (Army.Status.Contains(ArmyStatusElement.Concealed))
        {
            i = 0;
            while ((((int)(((uint)Army.ConcealedUnits) >> i)) & 1) == 1)
            {
                i += 1;
            }

            if (i >= Army.Units.Count)
            {
                i = 0;
            }
        }
        else
        {
            i = 0;
        }

        _unitIndex = (byte)i;
        SetUnitGraphicsResourceIndex(Army.Units.GetUnit(i).GetUnitGraphicsResourceIndex());
        _unitYOffset = Army.Units.GetUnit(i).GetMoveTypes().Contains(MoveType.Flying) ? (sbyte)-8 : (sbyte)0;

        Assert(UnitGraphicsResource is not null, "", "Army.PAS", 3027);
    }

    // Army.PAS:3200
    public bool PlacePrivate(MapContainer map, sbyte X, sbyte Y, sbyte L)
    {
        bool result = base.PlaceHex(map, X, Y, L);
        if (result)
        {
            Activate();
        }

        return result;
    }

    // Army.PAS:3252
    public void ShowEnchantments(int X, int Y, in LockedSurface lockedSurface)
    {
        UnitShowInfo info = default;
        if (Army.Units.Count > _unitIndex)
        {
            info.Overlays = Set<UnitOverlayModesElement>.Of();
            info.Animation = UnitAnimation.None;
            info.Frame = StrategicMap.Current.Frame + (int)(((uint)(X + Y + Y)) >> 4);
            info.Looped = true;
            _ = Army.Units.GetUnit(_unitIndex).Show(X - 16, Y - 20, lockedSurface, info);
        }
    }

    // Army.PAS:3490
    public bool Place(sbyte x, sbyte y, sbyte mapLevel, Set<ArmyPlaceModeElement> mode)
    {
        MapFieldMessage message = default;
        bool result = false;
        MapLevel level = StrategicMap.Current.MapContainer.GetMapLevel(mapLevel);
        HexNumberWalkData.InitializeHexNumberWalk(out HexNumberWalkData walk, x, y, 0);
        while (!result && (walk.Radius == 0 || mode.Contains(ArmyPlaceModeElement.ScanArea)) && walk.Radius < 30)
        {
            if (walk.X >= 0 && walk.Y >= 0 && walk.X < level.Width && walk.Y < level.Height)
            {
                MapField field = level.GetField(walk.X, walk.Y);
                if (field.FindOwnedChild(131607) is null)
                {
                    int player = field.GetPlayer();
                    if (player == -1 || (player == Army.Player && mode.Contains(ArmyPlaceModeElement.EnableOwnPlayerLocation)))
                    {
                        message.MessageId = 4384;
                        MapFieldMessageParameters param = message.Param;
                        param[0] = this;
                        message.Param = param;
                        message.Result = 0;
                        field.SendMapFieldMessage(ref message);
                        if (message.Result.Int == 0)
                        {
                            if (Army.CanPlace((sbyte)walk.X, (sbyte)walk.Y, mapLevel))
                            {
                                _ = MoveTo(field, 0);
                                result = true;
                            }
                        }
                    }
                }
            }

            HexNumberWalkData.WalkToNextHexNumber(ref walk);
        }

        return result;
    }

    private static readonly string s_armyText = "Army";

    private static bool ConnectingHex(int x1, int y1, int x2, int y2)
    {
        bool result = x1 == x2
            ? y1 == y2 + 1 || y1 == y2 - 1
            : (x1 == x2 - 1 || x1 == x2 + 1) && (!((x1 & 1) != 0) ? y1 == y2 || y1 == y2 + 1 : y1 == y2 || y1 == y2 - 1);
        return result;
    }

    internal static ArmyCombatMoveTokenEvent CreateArmyCombatMoveTokenEvent(ArmyHexSprite army, byte selection, MovePath path, Set<ArmyMoveModeElement> mode) =>
        SetupArmyMoveTokenEvent(new ArmyCombatMoveTokenEvent(null) { Holder = army }, army: army, selection: selection, path: path, mode: mode);

    private static T SetupArmyMoveTokenEvent<T>(T move, ArmyHexSprite army, byte selection, MovePath path, Set<ArmyMoveModeElement> mode)
        where T : MoveArmyTokenEvent
    {
        if (move.Setup(army, selection: selection, path: path, mode: mode))
        {
            return move;
        }

        _ = move.Release();
        return null;
    }

    internal static MoveArmyTokenEvent CreateArmyMoveTokenEvent(ArmyHexSprite army, byte selection, MovePath path, MoveAction action, Set<ArmyMoveModeElement> mode)
    {
        MoveArmyTokenEvent move;
        switch (action)
        {
            case MoveAction.Normal:
                move = new ArmyDefaultMoveTokenEvent(null) { Holder = army };
                break;
            case MoveAction.Combine:
                move = new ArmyCombineMoveTokenEvent(null) { Holder = army };
                break;
            case MoveAction.Attack:
                return CreateArmyCombatMoveTokenEvent(army, selection: selection, path: path, mode: mode);
            case MoveAction.Negotiate:
                move = new ArmyNegotiateMoveTokenEvent(null) { Holder = army };
                break;
            default:
                return null;
        }

        return SetupArmyMoveTokenEvent(move, army: army, selection: selection, path: path, mode: mode);
    }

    internal static bool MoveArmy(ArmyHexSprite army, byte selection, MovePath path, Set<ArmyMoveModeElement> mode, bool force)
    {
        bool moved = false;
        if (force || !StrategicMap.Current.Players.GetPlayers(army.Army.Player).GetBusy())
        {
            if (path.Count > 1)
            {
                int position = path.MovePointsToPosition(army.Army.MovePoints(selection));
                MoveAction action;
                if (position == 0)
                {
                    PathStep item = path[position];
                    PathStep pathItem = path[position];
                    PathStep item2 = path[position];
                    action = StrategicMap.Current.MapContainer.GetField(item2.Location.X, pathItem.Location.Y, item.Location.MapLevel).GetMoveAction(army.Army.Player);
                }
                else
                {
                    action = MoveAction.Normal;
                }

                bool condition = action == MoveAction.Attack && StrategicMap.Current.Players.GetPlayers(army.Army.Player).PlayerType == PlayerType.Human;
                if (condition)
                {
                    PathStep item = path[0];
                    PathStep pathItem = path[0];
                    PathStep item2 = path[0];
                    condition = !StrategicMap.Current.VisibleForPlayer(item2.Location.X, pathItem.Location.Y, item.Location.MapLevel, army.Army.Player);
                }

                if (condition)
                {
                    action = MoveAction.Normal;
                }

                MoveArmyTokenEvent move = CreateArmyMoveTokenEvent(army, selection, path, action, mode);
                if (move is not null)
                {
                    StrategicMap.Current.TokenManager.GetTokenControl(army.Army.Player).AddEvent(move);
                    _ = move.Release();
                    moved = true;
                }
            }
        }

        return moved;
    }

    internal static bool MoveArmy(ArmyHexSprite army, byte selection, MovePath path) => MoveArmy(army, selection, path, Set<ArmyMoveModeElement>.Of(ArmyMoveModeElement.SelectEndLocation), false);

    internal static bool MoveArmyMapCoordinate(ArmyHexSprite army, byte selection, sbyte X, sbyte Y, sbyte L, bool force)
    {
        MoveControlSettings settings = new(null) { Holder = army };
        sbyte hexLevel = army.GetHexLevel();
        sbyte hexY = army.GetHexY();
        settings.SetSourcePosition(army.GetHexX(), hexY, hexLevel);
        settings.Player = army.Army.Player;
        settings.ArmyHexSprite = army;
        settings.Selection = selection;
        settings.SetDestinationPosition(X, Y, L);
        _ = settings.MovePointTable;
        army.Army.CreateMovePointTable(selection, out MovePointTable movePointTable);
        settings.MovePointTable = movePointTable;
        StrategicMap.Current.MoveControl.ProcessMove(settings);
        bool result = settings.Path.Count != 0 && MoveArmy(army, selection, settings.Path, Set<ArmyMoveModeElement>.Of(), force);

        _ = settings.Release();
        return result;
    }

}
