// Translated from GlobalTargetSpells.pas.
using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Magic;
using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;

using Aow.Game.Session;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

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
    private Int32List _unexplored;
    private bool _visibleForSeatedPlayer;

    // GlobalTargetSpells.pas:1213
    public override void Dispose()
    {
        base.Dispose();
        _unexplored?.Dispose();
        Enchantment.Release();
    }

    // GlobalTargetSpells.pas:1344
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwEObject(Enchantment, 40);
    }

    // GlobalTargetSpells.pas:1203
    public WatcherHexSprite(ECustomNode owner) : base(owner)
    {
        Enchantment = new WatcherEnchantment(null);
        Enchantment.Watcher = this;
        _unexplored = new Int32List();
        _visibleForSeatedPlayer = false;
        _animationFrame = -1;
    }

    // GlobalTargetSpells.pas:1422
    public override void MessageProc(ref EngineMessage message, EngineObject sender)
    {
        int id;
        base.MessageProc(ref message, sender);
        id = message.MessageId;
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
        int X = default;
        int Y = default;
        int deltaX = default;
        int deltaY = default;
        int radius;
        int angle;
        int pixelX = default;
        int pixelY = default;
        int intensity;
        int delay;
        bool condition = AoWHSMap.Current.ViewL == HexLevel;
        if (condition)
        {
            var hexY = HexY;
            var hexX = HexX;
            condition = AoWHSMap.Current.Visible(hexX, hexY, HexLevel, 0);
        }

        if (condition)
        {
            var hexY = HexY;
            HexGeometry.HexToPixel(HexX, hexY, ref X, ref Y);
            X = X - pixelXSurface + 16;
            Y = Y - pixelYSurface + 16;
            if (AoWHSSet.Current.SpellControl.GetSpell(57).CastImages != null)
            {
                AoWHSSet.Current.SpellControl.GetSpell(57).CastImages.ShowLooped(1, X, Y, AoWHSMap.Current.Frame, lockedSurface);
            }

            if (_animationFrame != -1)
            {
                _animationFrame += 1;
                // The original reads the shared AoWEngine animation detail. The map engine is
                // not the global read by this routine.
                if (GameTables.GameEngine.AnimationDetail == 2 && AoWHSMap.Current.ViewL == HexLevel)
                {
                    intensity = 100 - _animationFrame;
                    var hexY2 = HexY;
                    HexGeometry.HexToPixel(HexX, hexY2, ref X, ref Y);
                    X = X - pixelXSurface - 8;
                    Y = Y - pixelYSurface;
                    for (int I = _unexplored.Count - 1; I >= 0; I--)
                    {
                        HexGeometry.HexNumberToPixel(_unexplored[I], ref pixelX, ref pixelY);
                        pixelX += X;
                        pixelY += Y;
                        AoWHSSet.Current.GeneralImages[5].ShowBlended(pixelX, pixelY, lockedSurface, GraphicsState.AlphaBlendTable.LookupTables[intensity]);
                    }
                }

                // { Original resets the shared random generator for deterministic particles. }
                RandSeed = AoWHSMap.Current.InitialRandomSeed;
                for (int I = 0; I <= 300; I++)
                {
                    radius = Random(VisibilityRange() << 5);
                    angle = Random(360);
                    delay = Random(80);
                    if (_animationFrame - delay >= 0)
                    {
                        AbilityRules.PolarToCartesian(angle, radius, ref deltaX, ref deltaY);
                        AoWHSSet.Current.SpellControl.GetSpell(57).CastImages.Show(0, X + deltaX, Y + deltaY, _animationFrame - delay, lockedSurface);
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
        MapField mapField;
        mapField = Owner;
        base.RemoveFromMap();
        mapField.UpdateMapField();
    }

    // GlobalTargetSpells.pas:1438
    public override void MapFieldMessageProc(ref MapFieldMessage message)
    {
        int ownerPlayer;
        ref var mapFieldMessage = ref message;
        base.MapFieldMessageProc(ref message);
        if (mapFieldMessage.MessageId == 4363)
        {
            if (Activated())
            {
                ownerPlayer = Enchantment.PlayerIndex;
                if (ownerPlayer == mapFieldMessage.Param[1].Int || (byte)AoWHSMap.Current.Players.GetPlayers(ownerPlayer << 0).DiplomaticRelations.GetDiplomaticRelation((sbyte)(byte)mapFieldMessage.Param[1].Int) == 2)
                {
                    var visibilityRange = VisibilityRange();
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
                if (ownerPlayer == mapFieldMessage.Param[1].Int || (byte)AoWHSMap.Current.Players.GetPlayers(ownerPlayer << 0).DiplomaticRelations.GetDiplomaticRelation((sbyte)(byte)mapFieldMessage.Param[1].Int) == 2)
                {
                    var visibilityRange = VisibilityRange();
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
                AoWHSMap.Current.UpdatePlayerSeen(Enchantment.PlayerIndex, Owner.HexX, Owner.HexY, Owner.HexLevel, mapFieldMessage.Param[0].Int, mapFieldMessage.Param[0].Int);
            }

            UpdateVisibleForSeatedPlayer();
        }
    }

    // GlobalTargetSpells.pas:1350
    public override void Activate()
    {
        MapField mapFieldLocal;
        base.Activate();
        WatcherState = (byte)(Set<StateElement>.FromMask(WatcherState) + Set<StateElement>.Of(StateElement.Active)).Image;
        Enchantment.Activate(Enchantment.PlayerIndex);
        var mapField = Owner;
        AoWHSMap.Current.NotifyPlayerChanged(mapField.HexX, mapField.HexY, mapField.HexLevel);

        mapFieldLocal = Owner;
        var visibilityRange = VisibilityRange();
        var visibilityRange2 = VisibilityRange();
        AoWHSMap.Current.UpdatePlayerSeen(Enchantment.PlayerIndex, mapFieldLocal.HexX, mapFieldLocal.HexY, mapFieldLocal.HexLevel, visibilityRange, visibilityRange2);
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
            Enchantment.Deactivate((byte)Enchantment.PlayerIndex);
        }

        AoWHSMap.Current.NotifyPlayerChanged(Owner.HexX, Owner.HexY, Owner.HexLevel);
        UpdateUnfog();
    }

    // GlobalTargetSpells.pas:1403
    public override void UpdateMapField(in HexSpriteMapFieldInfo info)
    {
        if (info.HexNumber == 0)
        {
            ((AoWMapField)(object)info.MapField).FieldFlags = ((AoWMapField)(object)info.MapField).FieldFlags | 4096 | 16777216;
        }
    }

    // GlobalTargetSpells.pas:1226
    public void StartAnimation(sbyte X, sbyte Y, sbyte layer)
    {
        HexNumberWalkData walk = default;
        MapLevel level;
        _animationFrame = 0;
        AoWHSMap.Current.DynamicFixedVisibleHexSprite.AddChild(this);
        AoWHSSet.Current.SpellControl.GetSpell(57).CastSounds.Play(0);
        _unexplored.Clear();
        if (AoWHSMap.Current.ExplorationEnabled)
        {
            HexNumberWalkData.InitializeHexNumberWalk(ref walk, X, Y, 0);
            level = AoWHSMap.Current.MapContainer.MapLevels[layer];
            while (true)
            {
                var visibilityRange = VisibilityRange();
                if (!(walk.Radius <= visibilityRange))
                    break;
                if (walk.X >= 0 && walk.Y >= 0 && walk.X < level.Width && walk.Y < level.Height)
                {
                    if (!((AoWMapField)level.Field[walk.X, walk.Y]).GetExplored(AoWHSMap.Current.SeatedPlayer))
                    {
                        _unexplored.Add(walk.HexNumber);
                    }
                }

                HexNumberWalkData.WalkToNextHexNumber(ref walk);
            }
        }
    }

    // GlobalTargetSpells.pas:1248
    public void StopAnimation()
    {
        AoWHSMap.Current.DynamicFixedVisibleHexSprite.RemoveChild(this);
        _animationFrame = -1;
        _unexplored.Clear();
    }

    // GlobalTargetSpells.pas:1305
    public void UpdateExploration()
    {
        int ownerPlayer;
        if (Activated())
        {
            for (int I = 1, last = AoWHSMap.Current.Players.Count - 1; I <= last; I++)
            {
                ownerPlayer = Enchantment.PlayerIndex;
                if (ownerPlayer == I || (byte)AoWHSMap.Current.Players.GetPlayers(ownerPlayer).DiplomaticRelations.GetDiplomaticRelation((sbyte)(byte)I) == 2)
                {
                    var visibilityRange = VisibilityRange();
                    var hexY = HexY;
                    var hexX = HexX;
                    AoWHSMap.Current.ExploreArea(hexX, hexY, HexLevel, (sbyte)I, visibilityRange);
                }
            }
        }
    }

    // GlobalTargetSpells.pas:1321
    public void UpdateUnfog()
    {
        int radius;
        if ((byte)AoWHSMap.Current.SeatedPlayer != 255 && (Enchantment.PlayerIndex == AoWHSMap.Current.SeatedPlayer || (byte)AoWHSMap.Current.Players.GetPlayers(Enchantment.PlayerIndex).DiplomaticRelations.GetDiplomaticRelation((sbyte)(byte)AoWHSMap.Current.SeatedPlayer) == 2) && Activated())
        {
            radius = VisibilityRange();
        }
        else
        {
            radius = 0;
        }

        if (radius != _unfogRadius)
        {
            if (_unfogRadius != 0)
            {
                var hexY = HexY;
                var hexX = HexX;
                AoWHSMap.Current.FogArea(hexX, hexY, HexLevel, _unfogRadius);
            }

            _unfogRadius = (byte)radius;
            if (_unfogRadius != 0)
            {
                var hexY = HexY;
                var hexX = HexX;
                AoWHSMap.Current.UnFogArea(hexX, hexY, HexLevel, _unfogRadius);
            }
        }
    }

    // GlobalTargetSpells.pas:1376
    public int VisibilityRange()
    {
        int result = 8;
        if (HexLevel != 0 || Enchantment.PlayerIndex > 0 && AoWHSMap.Current.Players.GetPlayers(Enchantment.PlayerIndex).MagicControl.EnemyEnchantments.Contains(PlayerEnchantmentType.WaterMastery))
        {
            result += 1;
            result = (int)(((uint)result) >> 1);
        }

        return result;
    }

    // GlobalTargetSpells.pas:1410
    public void UpdateVisibleForSeatedPlayer()
    {
        bool visible;
        visible = Enchantment.VisibleForPlayer(AoWHSMap.Current.SeatedPlayer);
        if (_visibleForSeatedPlayer ^ visible)
        {
            _visibleForSeatedPlayer = visible;
            AoWHSMap.Current.GlobalMagicControl.TriggerChangedEvent();
        }
    }

    public enum StateElement : byte
    {
        Active = 0
    }

    // GlobalTargetSpells.pas:172

}
