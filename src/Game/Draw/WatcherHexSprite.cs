#nullable disable
// Translated from GlobalTargetSpells.pas.
using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

using Aow.Game.Magic;
using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;

using Aow.Game.Session;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

using Aow.Rtl;
using static Aow.Rtl.DelphiRandom;

namespace Aow.Game.Draw;

// state, unfog radius, enchantment, animation frame, unexplored HNs, visibility flag
public partial class WatcherHexSprite : SingleHexSprite
{
    // GlobalTargetSpells.pas:1220
    public new const int ClassId = 132989;

    public byte WatcherState { get; private set; }

    private byte _unfogRadius;

    public WatcherEnchantment Enchantment { get; }

    private int _animationFrame;
    private readonly Int32List _unexplored;
    private bool _visibleForSeatedPlayer;

    // GlobalTargetSpells.pas:1213
    public override void Dispose()
    {
        base.Dispose();
        _unexplored?.Dispose();
        _ = Enchantment.Release();
    }

    // GlobalTargetSpells.pas:1344
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        persister.Object("Enchantment", objectValue: Enchantment);
    }

    // GlobalTargetSpells.pas:1203
    public WatcherHexSprite(CustomNode owner) : base(owner)
    {
        Enchantment = new WatcherEnchantment(null)
        {
            Holder = CreationHolder(),
            Watcher = this
        };
        _unexplored = new Int32List();
        _visibleForSeatedPlayer = false;
        _animationFrame = -1;
    }

    // GlobalTargetSpells.pas:1422
    public override void MessageProc(ref EngineMessage message, EngineObject sender)
    {
        base.MessageProc(ref message, sender);
        int id = message.MessageId;
        if (id == 537001992)
        {
            UpdateExploration();
            UpdateUnfog();
            return;
        }

        if (id == 537001990)
        {
            UpdateUnfog();
            UpdateVisibleForSeatedPlayer();
        }
    }

    // GlobalTargetSpells.pas:1223
    public override byte GetLevel() => 27;

    // GlobalTargetSpells.pas:1255
    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        bool condition = StrategicMap.Current.ViewMapLevel == HexLevel;
        if (condition)
        {
            sbyte hexY = HexY;
            sbyte hexX = HexX;
            condition = StrategicMap.Current.Visible(hexX, hexY, HexLevel, 0);
        }

        if (condition)
        {
            sbyte hexY = HexY;
            HexGeometry.HexToPixel(HexX, hexY, out int x, out int y);
            x = x - pixelXSurface + 16;
            y = y - pixelYSurface + 16;
            GamePack.Current.SpellControl.GetSpell(57).CastImages?.ShowLooped(1, x, y, StrategicMap.Current.Frame, lockedSurface);

            if (_animationFrame != -1)
            {
                _animationFrame += 1;
                // The original reads the shared AoWEngine animation detail. The map engine is
                // not the global read by this routine.
                if (GetEngine().AnimationDetail == 2 && StrategicMap.Current.ViewMapLevel == HexLevel)
                {
                    int intensity = 100 - _animationFrame;
                    sbyte hexY2 = HexY;
                    HexGeometry.HexToPixel(HexX, hexY2, out x, out y);
                    x = x - pixelXSurface - 8;
                    y -= pixelYSurface;
                    int pixelX = default;
                    int pixelY = default;
                    for (int i = _unexplored.Count - 1; i >= 0; i--)
                    {
                        HexGeometry.HexNumberToPixel(_unexplored[i], ref pixelX, ref pixelY);
                        pixelX += x;
                        pixelY += y;
                        GamePack.Current.GeneralImages[5].ShowBlended(pixelX, pixelY, lockedSurface, GraphicsState.AlphaBlendTable.LookupTables[intensity]);
                    }
                }

                // { Original resets the shared random generator for deterministic particles. }
                RandSeed = StrategicMap.Current.InitialRandomSeed;
                for (int i = 0; i <= 300; i++)
                {
                    int radius = Random(VisibilityRange() << 5);
                    int angle = Random(360);
                    int delay = Random(80);
                    if (_animationFrame - delay >= 0)
                    {
                        AbilityRules.PolarToCartesian(angle, radius, out int deltaX, out int deltaY);
                        _ = GamePack.Current.SpellControl.GetSpell(57).CastImages.Show(0, x + deltaX, y + deltaY, _animationFrame - delay, lockedSurface);
                    }
                }

                if (_animationFrame == 100)
                {
                    StopAnimation();
                }
            }
        }
    }

    // GlobalTargetSpells.pas:1388
    public override bool PlaceOnMap()
    {
        bool result = base.PlaceOnMap();
        Owner.UpdateMapField();
        return result;
    }

    // GlobalTargetSpells.pas:1394
    public override void RemoveFromMap()
    {
        MapField mapField = Owner;
        base.RemoveFromMap();
        mapField.UpdateMapField();
    }

    // GlobalTargetSpells.pas:1438
    public override void MapFieldMessageProc(ref MapFieldMessage message)
    {
        ref MapFieldMessage mapFieldMessage = ref message;
        base.MapFieldMessageProc(ref message);
        int ownerPlayer;
        if (mapFieldMessage.MessageId == 4363)
        {
            if (Activated())
            {
                ownerPlayer = Enchantment.PlayerIndex;
                if (ownerPlayer == mapFieldMessage.Param[1].Int || (byte)StrategicMap.Current.Players.GetPlayers(ownerPlayer << 0).DiplomaticRelations.GetDiplomaticRelation((sbyte)(byte)mapFieldMessage.Param[1].Int) == 2)
                {
                    int visibilityRange = VisibilityRange();
                    if (mapFieldMessage.Param[0].Int <= visibilityRange)
                    {
                        mapFieldMessage.Result = 1;
                    }
                }
            }
        }

        if (mapFieldMessage.MessageId == 4362)
        {
            if (Activated())
            {
                ownerPlayer = Enchantment.PlayerIndex;
                if (ownerPlayer == mapFieldMessage.Param[1].Int || (byte)StrategicMap.Current.Players.GetPlayers(ownerPlayer << 0).DiplomaticRelations.GetDiplomaticRelation((sbyte)(byte)mapFieldMessage.Param[1].Int) == 2)
                {
                    int visibilityRange = VisibilityRange();
                    if (mapFieldMessage.Param[0].Int <= visibilityRange)
                    {
                        mapFieldMessage.Result = 1;
                    }
                }
            }
        }

        if (mapFieldMessage.MessageId == 4365)
        {
            if (VisibilityRange() >= mapFieldMessage.Param[0].Int)
            {
                // { Param[0] is already available. }
                StrategicMap.Current.UpdatePlayerSeen(Enchantment.PlayerIndex, Owner.HexX, Owner.HexY, Owner.HexLevel, mapFieldMessage.Param[0].Int, mapFieldMessage.Param[0].Int);
            }

            UpdateVisibleForSeatedPlayer();
        }
    }

    // GlobalTargetSpells.pas:1350
    public override void Activate()
    {
        base.Activate();
        WatcherState = (byte)(Set<StateElement>.FromMask(WatcherState) + Set<StateElement>.Of(StateElement.Active)).Image;
        _ = Enchantment.Activate(Enchantment.PlayerIndex);
        MapField mapField = Owner;
        StrategicMap.Current.NotifyPlayerChanged(mapField.HexX, mapField.HexY, mapField.HexLevel);

        MapField mapFieldLocal = Owner;
        int visibilityRange = VisibilityRange();
        int visibilityRange2 = VisibilityRange();
        StrategicMap.Current.UpdatePlayerSeen(Enchantment.PlayerIndex, mapFieldLocal.HexX, mapFieldLocal.HexY, mapFieldLocal.HexLevel, visibilityRange, visibilityRange2);
        UpdateExploration();
        UpdateUnfog();
    }

    // GlobalTargetSpells.pas:1366
    public override void Deactivate()
    {
        base.Deactivate();
        WatcherState = (byte)(Set<StateElement>.FromMask(WatcherState) - Set<StateElement>.Of(StateElement.Active)).Image;
        if (Enchantment.EnchantmentStatus.Contains(GlobalEnchantmentStatusElement.Activated))
        {
            _ = Enchantment.Deactivate((byte)Enchantment.PlayerIndex);
        }

        StrategicMap.Current.NotifyPlayerChanged(Owner.HexX, Owner.HexY, Owner.HexLevel);
        UpdateUnfog();
    }

    // GlobalTargetSpells.pas:1403
    public override void UpdateMapField(in HexSpriteMapFieldInfo info)
    {
        if (info.HexNumber == 0)
        {
            info.MapField.FieldFlags = info.MapField.FieldFlags | MapField.ViewerFlag | MapField.PlayerChangeListenerFlag;
        }
    }

    // GlobalTargetSpells.pas:1226
    public void StartAnimation(sbyte X, sbyte Y, sbyte layer)
    {
        _animationFrame = 0;
        _ = StrategicMap.Current.DynamicFixedVisibleHexSprite.AddChild(this);
        _ = GamePack.Current.SpellControl.GetSpell(57).CastSounds.Play(0);
        _unexplored.Clear();
        if (StrategicMap.Current.ExplorationEnabled)
        {
            HexNumberWalkData.InitializeHexNumberWalk(out HexNumberWalkData walk, X, Y, 0);
            MapLevel level = StrategicMap.Current.MapContainer.MapLevels[layer];
            while (true)
            {
                int visibilityRange = VisibilityRange();
                if (!(walk.Radius <= visibilityRange))
                {
                    break;
                }

                if (walk.X >= 0 && walk.Y >= 0 && walk.X < level.Width && walk.Y < level.Height)
                {
                    if (!level.Field[walk.X, walk.Y].GetExplored(StrategicMap.Current.SeatedPlayer))
                    {
                        _ = _unexplored.Add(walk.HexNumber);
                    }
                }

                HexNumberWalkData.WalkToNextHexNumber(ref walk);
            }
        }
    }

    // GlobalTargetSpells.pas:1248
    public void StopAnimation()
    {
        _ = StrategicMap.Current.DynamicFixedVisibleHexSprite.RemoveChild(this);
        _animationFrame = -1;
        _unexplored.Clear();
    }

    // GlobalTargetSpells.pas:1305
    public void UpdateExploration()
    {
        if (Activated())
        {
            for (int i = 1, last = StrategicMap.Current.Players.Count - 1; i <= last; i++)
            {
                int ownerPlayer = Enchantment.PlayerIndex;
                if (ownerPlayer == i || (byte)StrategicMap.Current.Players.GetPlayers(ownerPlayer).DiplomaticRelations.GetDiplomaticRelation((sbyte)(byte)i) == 2)
                {
                    int visibilityRange = VisibilityRange();
                    sbyte hexY = HexY;
                    sbyte hexX = HexX;
                    _ = StrategicMap.Current.ExploreArea(hexX, hexY, HexLevel, (sbyte)i, visibilityRange);
                }
            }
        }
    }

    // GlobalTargetSpells.pas:1321
    public void UpdateUnfog()
    {
        int radius = (byte)StrategicMap.Current.SeatedPlayer != 255 && (Enchantment.PlayerIndex == StrategicMap.Current.SeatedPlayer || (byte)StrategicMap.Current.Players.GetPlayers(Enchantment.PlayerIndex).DiplomaticRelations.GetDiplomaticRelation((sbyte)(byte)StrategicMap.Current.SeatedPlayer) == 2) && Activated()
            ? VisibilityRange()
            : 0;
        if (radius != _unfogRadius)
        {
            if (_unfogRadius != 0)
            {
                sbyte hexY = HexY;
                sbyte hexX = HexX;
                _ = StrategicMap.Current.FogArea(hexX, hexY, HexLevel, _unfogRadius);
            }

            _unfogRadius = (byte)radius;
            if (_unfogRadius != 0)
            {
                sbyte hexY = HexY;
                sbyte hexX = HexX;
                _ = StrategicMap.Current.UnFogArea(hexX, hexY, HexLevel, _unfogRadius);
            }
        }
    }

    // GlobalTargetSpells.pas:1376
    public int VisibilityRange()
    {
        int result = 8;
        if (HexLevel != 0 || (Enchantment.PlayerIndex > 0 && StrategicMap.Current.Players.GetPlayers(Enchantment.PlayerIndex).MagicControl.EnemyEnchantments.Contains(PlayerEnchantmentType.WaterMastery)))
        {
            result += 1;
            return (int)(((uint)result) >> 1);
        }

        return result;
    }

    // GlobalTargetSpells.pas:1410
    public void UpdateVisibleForSeatedPlayer()
    {
        bool visible = Enchantment.VisibleForPlayer(StrategicMap.Current.SeatedPlayer);
        if (_visibleForSeatedPlayer ^ visible)
        {
            _visibleForSeatedPlayer = visible;
            StrategicMap.Current.GlobalMagicControl.TriggerChangedEvent();
        }
    }

    public enum StateElement : byte
    {
        Active = 0
    }

    // GlobalTargetSpells.pas:172

}
