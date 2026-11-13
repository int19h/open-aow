// Translated from AoWE.pas.

using Aow.Engine.Nodes;
using Aow.Engine.Storage;
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

using static Aow.Rtl.RtlUnit;

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
    public override void ReadWrite(StorageStream stream)
    {
        base.ReadWrite(stream);
        stream.Object(Army, 10);
    }

    // Army.PAS:2648
    public ArmyHexSprite(ECustomNode owner) : base(owner)
    {
        Army = new Army(null) { Holder = CreationHolder() };
    }

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
        if ((message.MessageId & 1073741824) == 0 && Army != null)
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
        bool result;
        if (info.HexNumber == 0)
        {
            result = info.MapField.TerrainType == 0 && info.Sender != null && (((EngineObject)info.Sender).EngineClass.ClassId == 135742 || ((EngineObject)info.Sender).EngineClass.ClassId == 131436) || Army.ValidTerrain(info.MapField.TerrainType, info.MapField.OverlayType);
        }
        else
        {
            result = true;
        }

        return result;
    }

    // Army.PAS:3069
    public override void TerrainChanged(in HexSpriteTerrainInfo info)
    {
        base.TerrainChanged(info);
        if (info.HexNumber == 0 && Army.Status.Contains(ArmyStatusElement.Activated))
        {
            Army.BeginUpdate();
            Army.ValidateTerrain(info.TerrainType, info.OverlayType);
            Army.EndUpdate();
        }
    }

    // Army.PAS:2690
    public override bool PlaceOnMap()
    {
        MapFieldMessage message = default;
        bool result = base.PlaceOnMap();
        if (result)
        {
            Army.BeginUpdate();
            var mapField = Owner;
            Army.SetMapCoordinate(mapField.HexX, mapField.HexY, mapField.HexLevel);

            Army.UpdateUnfog();
            Army.UpdateExploration();
            message.MessageId = 4354;
            message.Param[0] = this;
            Owner.SendMapFieldMessage(ref message);
            Army.EndUpdate();
        }

        return result;
    }

    // Army.PAS:2707
    public override void RemoveFromMap()
    {
        MapField field;
        MapFieldMessage message = default;
        field = Owner;
        if (Owner != null)
        {
            Army.TurnUnFogOff();
        }

        base.RemoveFromMap();
        if (field != null && StrategicMap.Current != null && !StrategicMap.Current.Status.Contains(HexSpriteMapStatusElement.Destroying))
        {
            message.MessageId = 4355;
            message.Param[0] = this;
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
                        var players = StrategicMap.Current.Players.GetPlayers(Army.Player);
                        message.Param[0] = message.Param[0].Int | (players.PlayerRace + 1);
                    }
                    else
                    {
                        message.Param[0] = message.Param[0].Int | 13;
                    }
                }
            }
            else
            {
                if (message.MessageId == 4608)
                {
                    Army.UpdateAITarget(message.Param[0].As<AITargetQuery>().Info, ref message.Param[0].As<AITargetQuery>().Data);
                    if (message.Param[0].As<AITargetQuery>().Data.Priority < 0)
                    {
                        message.Result = 1;
                    }
                }
                else
                {
                    if (message.MessageId == 4360)
                    {
                        Army.ReturnBattleFieldInfo(ref message.Param[0].As<BattleFieldQuery>().Info);
                    }
                    else
                    {
                        if (message.MessageId == 4353)
                        {
                            var moveAction = Army.GetMoveAction((sbyte)message.Param[0].Int);
                            message.Param[1] = (int)moveAction;
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
                                            if (Army.ConcealedUnits != AbilityRules.ArmySelectionMasks[Army.Count])
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
                                                var visibilityRange = Army.VisibilityRange();
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
                                                    var trueVisionRange = Army.TrueVisionRange();
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
                                                        var visibilityRange = Army.VisibilityRange();
                                                        condition = message.Param[0].Int <= visibilityRange;
                                                    }

                                                    if (condition)
                                                    {
                                                        var trueVisionRange = Army.TrueVisionRange();
                                                        StrategicMap.Current.UpdatePlayerSeen(Army.Player, message.MapField.HexX, message.MapField.HexY, message.MapField.HexLevel, message.Param[0].Int, trueVisionRange);
                                                    }
                                                }
                                                else
                                                {
                                                    if (message.MessageId == 4366)
                                                    {
                                                        if (Army.IncomingNegotiateRequest(message.Param[0].As<NegotiateRequestEventLog>()))
                                                        {
                                                            message.Param[1] = 1;
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
                                                                    Army.RetrieveStormDamageInfo((StormType)message.Param[0].Int, message.Param[1].Int, ref message.Param[2].As<StormDamageQuery>().Info);
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
        MoveControl control;
        if (info.HexNumber == 0)
        {
            control = (MoveControl)info.Sender;
            if (Army.Player == control.Player)
            {
                if (!(control.Mode.Contains(MoveModeElement.Surround) && info.MoveOverCode != 1 && Owner.HexX == control.DestinationX && Owner.HexY == control.DestinationY && Owner.HexLevel == control.DestinationL && !Army.CanCombine(control.ArmyHexSprite.Army, (byte)control.Selection)))
                {
                    if (control.ArmyHexSprite != null && Army.IndexOfTransporter(255) != -1 && control.ArmyHexSprite.Army.Transporter((byte)control.Selection) == null)
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

                        if (((MoveControl)info.Sender).ArmyHexSprite != null && Army.CanCombine(((MoveControl)info.Sender).ArmyHexSprite.Army, (byte)((MoveControl)info.Sender).Selection))
                        {
                            result = 1;
                        }
                        else
                        {
                            result = 2;
                        }
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
                if (StrategicMap.Current.Players.GetDiplomaticRelation(((MoveControl)info.Sender).Player, Army.Player) == DiplomaticRelation.Alliance)
                {
                    if (info.MapField.OverlayType != 7 || info.MapField.FindNoneTransparentHexSprite(typeof(PlayerStructure)) == null)
                    {
                        result = 2;
                    }
                    else
                    {
                        result = 0;
                    }
                }
                else
                {
                    result = 0;
                }
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
        var hexLevel = GetHexLevel();
        var hexY = GetHexY();
        var hexX = GetHexX();
        Army.SetMapCoordinate(hexX, hexY, hexLevel);
        Army.Activate();
        Army.SetUpdateHandler(ArmyUpdated);
        if (this == Owner.GetArmyHexSprite())
        {
            if (Army.Count != 0)
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
        bool result;
        if (!GetFogEnabled() && StrategicMap.Current.SeatedPlayer != -1)
        {
            if (Army.Status.Contains(ArmyStatusElement.FullyConcealed) || !base.CanSelect(pixelX, pixelY))
            {
                result = false;
            }
            else
            {
                result = true;
            }
        }
        else
        {
            result = false;
        }

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
        MapField oldField;
        MapFieldMessage message = default;
        Army.BeginUpdate();
        oldField = Owner;
        if (oldField != null)
        {
            message.MessageId = 4416;
            message.Param[0] = this;
            message.Param[1] = field;
            message.Result = 0;
            oldField.SendMapFieldMessage(ref message);
        }

        if (oldField == null || message.Result.Int == 0)
        {
            message.MessageId = 4417;
            message.Param[0] = this;
            message.Param[1] = oldField;
            message.Result = 0;
            field.SendMapFieldMessage(ref message);
            if (message.Result.Int == 0)
            {
                base.MoveTo(field, moveCost);
                if (oldField != null && (field.HexLevel != oldField.HexLevel || !ConnectingHex(oldField.HexX, oldField.HexY, field.HexX, field.HexY)))
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
                message.Param[0] = this;
                message.Param[1] = oldField;
                message.Result = 0;
                field.SendMapFieldMessage(ref message);
                if (Army.Player > 0)
                {
                    var visibilityRange = Army.VisibilityRange();
                    var trueVisionRange = Army.TrueVisionRange();
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

        Army.EndUpdate();
        return result;
    }

    // Army.PAS:3187
    public override bool PlaceHex(MapContainer map, sbyte hexX, sbyte hexY, sbyte hexLevel)
    {
        bool result;
        MapField field;
        EngineObject army;
        if (Army.CanPlace(hexX, hexY, hexLevel))
        {
            field = map.GetField(hexX, hexY, hexLevel);
            army = field.FindOwnedChild(131607);
            if (army == null || GetEditMode())
            {
                result = base.PlaceHex(map, hexX, hexY, hexLevel);
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
        MapLevel level;
        int X = default;
        int Y = default;
        int deltaX = default;
        int deltaY = default;
        ImageSequenceAnimation animation;
        result = base.MoveFirst(direction, instant);
        switch (GetEngine().Registry.GetMovementSpeed())
        {
            case GameSettings.MovementSpeed.Normal:
                MoveSteps = (sbyte)base.GetMoveSteps();
                break;
            case GameSettings.MovementSpeed.Fast:
                MoveSteps = (sbyte)base.GetMoveSteps();
                if (MoveSteps > 8)
                {
                    MoveSteps = (sbyte)(MoveSteps - 4);
                }
                else
                {
                    MoveSteps = 4;
                }

                break;
            case GameSettings.MovementSpeed.Turbo:
                MoveSteps = 2;
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
            level = Owner.Owner;
            var hexY = GetHexY();
            HexGeometry.CenterHexNumberToHex(GetHexX(), hexY, direction, ref X, ref Y);
            if (X >= 0 && Y >= 0 && X < level.Width && Y < level.Height)
            {
                if (level.GetField(X, Y).TerrainType == 7)
                {
                    if (GamePack.Current.AbilityControl.GetAbility(42).ImageSequences.GetImageSequence(0) != null)
                    {
                        animation = new ImageSequenceAnimation(null) { Holder = CreationHolder() };
                        animation.SetImageSequence(GamePack.Current.AbilityControl.GetAbility(42).ImageSequences.GetImageSequence(0));
                        animation.AnimationLevel = 27;
                        HexGeometry.HexNumberToPixel(direction, ref deltaX, ref deltaY);
                        animation.SetPixelXOffset(deltaX / 2);
                        animation.SetPixelYOffset(deltaY / 2);
                        var hexLevel = GetHexLevel();
                        var hexY2 = GetHexY();
                        var hexX = GetHexX();
                        StrategicMap.Current.PlaceHex(animation, hexX, hexY2, hexLevel);
                    }
                }
            }
        }

        return result;
    }

    // Army.PAS:3555
    public override int GetMoveSteps()
    {
        if (Moving())
        {
            return MoveSteps;
        }

        return base.GetMoveSteps();
    }

    // Army.PAS:2667
    public bool GetFogEnabled() => StrategicMap.Current.FogEnabled && Owner.FogCount == 0;

    // Army.PAS:2673
    public bool VisibleForSeatedPlayer() => (!StrategicMap.Current.FogEnabled || Owner.FogCount != 0) && !Army.GetFullyConcealed();

    // Army.PAS:2745
    public void ArmyUpdated(object sender)
    {
        MapFieldMessage message = default;
        if (Army.Count != 0)
        {
            message.MessageId = 4388;
            message.Param[0] = this;
            Owner.SendMapFieldMessage(ref message);
            UpdateUnitGraphicsResource();
            if (Army.Player != 0)
            {
                FlagId = (byte)StrategicMap.Current.Players.GetPlayers(Army.Player).PlayerRace;
            }
            else
            {
                FlagId = 32;
            }
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
        int I;
        if (Army.Status.Contains(ArmyStatusElement.Concealed))
        {
            I = 0;
            while ((((int)(((uint)Army.ConcealedUnits) >> I)) & 1) == 1)
            {
                I += 1;
            }

            if (I >= Army.Count)
            {
                I = 0;
            }
        }
        else
        {
            I = 0;
        }

        _unitIndex = (byte)I;
        SetUnitGraphicsResourceIndex(Army.GetUnit(I).GetUnitGraphicsResourceIndex());
        if (Army.GetUnit(I).GetMoveTypes().Contains(MoveType.Flying))
        {
            _unitYOffset = -8;
        }
        else
        {
            _unitYOffset = 0;
        }

        Assert(UnitGraphicsResource != null, "", "Army.PAS", 3027);
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
        if (Army.Count > _unitIndex)
        {
            info.Overlays = Set<UnitOverlayModesElement>.Of();
            info.Animation = UnitAnimation.None;
            info.Frame = StrategicMap.Current.Frame + (int)(((uint)(X + Y + Y)) >> 4);
            info.Looped = true;
            Army.GetUnit(_unitIndex).Show(X - 16, Y - 20, lockedSurface, info);
        }
    }

    // Army.PAS:3490
    public bool Place(sbyte X, sbyte Y, sbyte L, Set<ArmyPlaceModeElement> mode)
    {
        MapLevel level;
        MapField field;
        MapFieldMessage message = default;
        HexNumberWalkData walk = default;
        int player;
        bool result = false;
        level = StrategicMap.Current.MapContainer.GetMapLevel(L);
        HexNumberWalkData.InitializeHexNumberWalk(out walk, X, Y, 0);
        while (!result && (walk.Radius == 0 || mode.Contains(ArmyPlaceModeElement.ScanArea)) && walk.Radius < 30)
        {
            if (walk.X >= 0 && walk.Y >= 0 && walk.X < level.Width && walk.Y < level.Height)
            {
                field = level.GetField(walk.X, walk.Y);
                if (field.FindOwnedChild(131607) == null)
                {
                    player = field.GetPlayer();
                    if (player == -1 || (player == Army.Player && mode.Contains(ArmyPlaceModeElement.EnableOwnPlayerLocation)))
                    {
                        message.MessageId = 4384;
                        message.Param[0] = this;
                        message.Result = 0;
                        field.SendMapFieldMessage(ref message);
                        if (message.Result.Int == 0)
                        {
                            if (Army.CanPlace((sbyte)walk.X, (sbyte)walk.Y, L))
                            {
                                MoveTo(field, 0);
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
        bool result;
        if (x1 == x2)
        {
            result = y1 == y2 + 1 || y1 == y2 - 1;
        }
        else
        {
            if (x1 == x2 - 1 || x1 == x2 + 1)
            {
                if (!((x1 & 1) != 0))
                {
                    result = y1 == y2 || y1 == y2 + 1;
                }
                else
                {
                    result = y1 == y2 || y1 == y2 - 1;
                }
            }
            else
            {
                result = false;
            }
        }

        return result;
    }

    internal static MoveArmyTokenEvent CreateArmyMoveTokenEvent(ArmyHexSprite army, byte selection, MovePath path, MoveAction action, Set<ArmyMoveModeElement> mode)
    {
        MoveArmyTokenEvent move;
        MoveArmyTokenEvent result = null;
        switch (action)
        {
            case MoveAction.Normal:
                move = new ArmyDefaultMoveTokenEvent(null) { Holder = army };
                break;
            case MoveAction.Combine:
                move = new ArmyCombineMoveTokenEvent(null) { Holder = army };
                break;
            case MoveAction.Attack:
                move = new ArmyCombatMoveTokenEvent(null) { Holder = army };
                break;
            case MoveAction.Negotiate:
                move = new ArmyNegotiateMoveTokenEvent(null) { Holder = army };
                break;
            default:
                move = null;
                break;
        }

        if (move != null)
        {
            if (!move.Setup(army, selection, path, mode))
            {
                result = null;
                move.Release();
            }
            else
            {
                result = move;
            }
        }

        return result;
    }

    internal static bool MoveArmy(ArmyHexSprite army, byte selection, MovePath path, Set<ArmyMoveModeElement> mode, bool force)
    {
        int position;
        MoveArmyTokenEvent move;
        MoveAction action;
        bool moved;
        moved = false;
        if (force || !StrategicMap.Current.Players.GetPlayers(army.Army.Player).GetBusy())
        {
            if (path.Count > 1)
            {
                position = path.MovePointsToPosition(army.Army.MovePoints(selection));
                if (position == 0)
                {
                    var item = path[position];
                    var pathItem = path[position];
                    var item2 = path[position];
                    action = StrategicMap.Current.MapContainer.GetField(item2.X, pathItem.Y, item.MapLevel).GetMoveAction(army.Army.Player);
                }
                else
                {
                    action = MoveAction.Normal;
                }

                bool condition = action == MoveAction.Attack && StrategicMap.Current.Players.GetPlayers(army.Army.Player).PlayerType == PlayerType.Human;
                if (condition)
                {
                    var item = path[0];
                    var pathItem = path[0];
                    var item2 = path[0];
                    condition = !StrategicMap.Current.VisibleForPlayer(item2.X, pathItem.Y, item.MapLevel, army.Army.Player);
                }

                if (condition)
                {
                    action = MoveAction.Normal;
                }

                move = CreateArmyMoveTokenEvent(army, selection, path, action, mode);
                if (move != null)
                {
                    StrategicMap.Current.TokenManager.GetTokenControl(army.Army.Player).AddEvent(move);
                    move.Release();
                    moved = true;
                }
            }
        }

        return moved;
    }

    internal static bool MoveArmy(ArmyHexSprite army, byte selection, MovePath path) => MoveArmy(army, selection, path, Set<ArmyMoveModeElement>.Of(ArmyMoveModeElement.SelectEndLocation), false);

    internal static bool MoveArmyMapCoordinate(ArmyHexSprite army, byte selection, sbyte X, sbyte Y, sbyte L, bool force)
    {
        bool result;
        MoveControlSettings settings;
        settings = new MoveControlSettings(null) { Holder = army };
        var hexLevel = army.GetHexLevel();
        var hexY = army.GetHexY();
        settings.SetSourcePosition(army.GetHexX(), hexY, hexLevel);
        settings.Player = army.Army.Player;
        settings.ArmyHexSprite = army;
        settings.Selection = selection;
        settings.SetDestinationPosition(X, Y, L);
        army.Army.CreateMovePointTable(selection, out settings.MovePointTable);
        StrategicMap.Current.MoveControl.ProcessMove(settings);
        if (settings.Path.Count != 0)
        {
            result = MoveArmy(army, selection, settings.Path, Set<ArmyMoveModeElement>.Of(), force);
        }
        else
        {
            result = false;
        }

        settings.Release();
        return result;
    }

}
