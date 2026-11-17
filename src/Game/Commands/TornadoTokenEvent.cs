#nullable disable
// Translated from GlobalTargetSpells.pas.
using Aow.Engine.Nodes;
using Aow.Formats.Tree;
using Aow.Game.AI.Groups;
using Aow.Game.Draw;
using Aow.Game.Draw.Animations;
using Aow.Game.Events;
using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Game.World.Structures;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Commands;

// angle, animation, selected army identity
public partial class TornadoTokenEvent : GlobalTargetSpellTokenEvent
{
    public TornadoTokenEvent(CustomNode owner) : base(owner) => ArmyId = null;

    // GlobalTargetSpells.pas:1559
    public new const int ClassId = 135972;

    private int _angle;
    private Animation _animation;

    public ArmyId ArmyId { get; set; }

    // GlobalTargetSpells.pas:1553
    public override void Dispose()
    {
        ArmyId?.Dispose();
        base.Dispose();
    }

    // GlobalTargetSpells.pas:1593
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        ArmyId = persister.Created("ArmyId", objectValue: ArmyId);
    }

    // GlobalTargetSpells.pas:1660
    public override bool Process(Troop source, int value)
    {
        bool result = default;
        UnitUserList movedUnits = default;
        byte owner = default;
        byte selection = default;
        MapCoordinate location = default;
        int chance = default;
        bool exposed = default;
        ArmyHexSprite army = default;
        Troop troopObject = default;
        MapField mapField = default;
        ArmyEventLog log = default;
        ImageSequenceAnimation animation = default;
        ArmyHexSprite placed = default;
        if (value == 0)
        {
            _angle = 0;
            Spell spell = GamePack.Current.SpellControl.GetSpell(EventSpellId);
            GamePack.Current.SpellControl.GetSpell(EventSpellId).CastingDone(source, spell.BaseManaCost);
            _ = StrategicMap.Current.ValidateObservation(source.Player, Location.X, Location.Y, Location.MapLevel, 2);
            if (source.Player == StrategicMap.Current.SeatedPlayer || StrategicMap.Current.Watching(Location.X, Location.Y, Location.MapLevel, 2))
            {
                _ = GamePack.Current.SpellControl.GetSpell(EventSpellId).CastSounds.Play(0);
            }

            // The original constructs a plain animation and stores it on the event. The later warp uses an image sequence.
            _animation = new Animation(null)
            {
                Holder = CreationHolder(),
                AnimationLevel = 27
            };
            _animation.SetShowHandler(ShowAnimation);
            _animation.AnimationMap = StrategicMap.Current;
            _animation.Tag = (StrategicMap.Current.ViewWidth & 65535) | (StrategicMap.Current.ViewHeight << 16);
            _animation.Activate();
        }

        if (value == 80)
        {
            mapField = StrategicMap.Current.MapContainer.GetField(Location.X, Location.Y, Location.MapLevel);
            if (ArmyId != null && !mapField.GetMagicShieldActive())
            {
                _ = ArmyId.RetrieveArmy(ref army, ref selection);
                exposed = !(Set<byte>.WithBounds(0, 255).With(1).Contains((byte)mapField.OverlayType) || mapField.FindNoneTransparentHexSprite(typeof(Structure)) != null);
                for (int i = 0, last = army.Army.Units.Count - 1; i <= last; i++)
                {
                    if ((selection & (1 << i)) != 0)
                    {
                        troopObject = army.Army.Units.GetUnit(i);
                        chance = troopObject.GetUnitLevel() switch
                        {
                            1 => 90,
                            2 => 70,
                            3 => 50,
                            4 => 30,
                            _ => 30,
                        };
                        if (!exposed)
                        {
                            chance /= 2;
                        }

                        if (chance < StrategicMap.Current.Random(100))
                        {
                            selection = (byte)(selection & ~(1 << i));
                        }
                    }
                }

                if (army.Army.Transporter(255) != null)
                {
                    selection = army.Army.Transporter(selection) != null ? (byte)255 : (byte)0;
                }

                if (selection != 0)
                {
                    log = new ArmyEventLog(null)
                    {
                        Holder = CreationHolder(),
                        Player = source.Player
                    };
                    string tornado = AbilityRules.TranslateResourceString(GetEngine(), GameText.TornadoText);
                    log.ArmyText = Format(AbilityRules.TranslateResourceString(GetEngine(), GameText.CastFormat), [tornado]);
                    string tornadoText = AbilityRules.TranslateResourceString(GetEngine(), GameText.TornadoText);
                    _ = log.ArmyMessages.Add(tornadoText);
                    log.ArmyIcon = 2;
                    log.ArmyView.Setup(army.Army, selection);
                    EventLog.DistributeLocationEventLog(y: army.HexY, x: army.HexX, log: log, player: (sbyte)EventPlayerIndex, level: army.HexLevel, execute: true);
                    _ = log.Release();
                    location = GamePack.Current.SpellControl.GetSpell<Tornado>(68).CalculateWarpLocation(army, selection);
                    if (location != MapCoordinate.None)
                    {
                        if (source.Player == StrategicMap.Current.SeatedPlayer || StrategicMap.Current.Watching(Location.X, Location.Y, Location.MapLevel, 2))
                        {
                            _ = GamePack.Current.SpellControl.GetSpell(EventSpellId).CastSounds.Play(1);
                        }

                        if (StrategicMap.Current.SeatedPlayer == source.Player || StrategicMap.Current.VisibleForPlayer(Location.X, Location.Y, Location.MapLevel, StrategicMap.Current.SeatedPlayer))
                        {
                            if (GamePack.Current.SpellControl.GetSpell(EventSpellId).CastImages.ImageSequences[1] != null)
                            {
                                animation = new ImageSequenceAnimation(null) { Holder = CreationHolder() };
                                animation.SetImageSequence(GamePack.Current.SpellControl.GetSpell(EventSpellId).CastImages.ImageSequences[1]);
                                _ = StrategicMap.Current.PlaceHex(animation, Location.X, Location.Y, Location.MapLevel);
                            }
                        }

                        if (StrategicMap.Current.SeatedPlayer == source.Player || StrategicMap.Current.VisibleForPlayer(location.X, location.Y, location.MapLevel, StrategicMap.Current.SeatedPlayer))
                        {
                            if (GamePack.Current.SpellControl.GetSpell(EventSpellId).CastImages.ImageSequences[1] != null)
                            {
                                animation = new ImageSequenceAnimation(null) { Holder = CreationHolder() };
                                animation.SetImageSequence(GamePack.Current.SpellControl.GetSpell(EventSpellId).CastImages.ImageSequences[1]);
                                _ = StrategicMap.Current.PlaceHex(animation, location.X, location.Y, location.MapLevel);
                            }
                        }

                        movedUnits = new UnitUserList(null) { Holder = CreationHolder() };
                        owner = (byte)army.Army.Player;
                        mapField = StrategicMap.Current.MapContainer.GetFieldMapCoordinate(location);
                        if (army.Army.ValidateSelection(255) != selection)
                        {
                            for (int i = army.Army.Units.Count - 1; i >= 0; i--)
                            {
                                if ((selection & (1 << i)) != 0)
                                {
                                    troopObject = army.Army.Units.GetUnit(i);
                                    _ = movedUnits.AddUnit(troopObject);
                                    placed = troopObject.Place(location.X, location.Y, location.MapLevel, (sbyte)(byte)army.Army.Player, Set<UnitPlaceModeElement>.Of());
                                    placed?.Army.UpdateExploration();
                                }
                            }
                        }
                        else
                        {
                            _ = movedUnits.AddUnits(army.Army.Units);
                            _ = army.MoveTo(mapField, 0);
                        }

                        if (owner == 0)
                        {
                            sbyte unitsToIndependentRelation = UnitList.UnitsToIndependentRelation(movedUnits, 5);
                            HuntAIGroup group = StrategicMap.Current.AIGroupManager.CreateGroup<HuntAIGroup>(0, relation: unitsToIndependentRelation);
                            _ = group.AddUnits(movedUnits.Units);
                            _ = group.TargetUnitIds.Add(source.UnitId);
                            _ = group.Release();
                        }

                        movedUnits?.Dispose();
                    }
                }
            }

            _animation?.Dispose();
            result = false;
        }
        else
        {
            result = true;
        }

        return result;
    }

    // GlobalTargetSpells.pas:1599
    public void ShowAnimation(Animation animation, int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        int xShift;
        int height;
        int x;
        int y;
        float angle;
        int direction;
        int radius;
        int last;
        if (StrategicMap.Current.ViewMapLevel == Location.MapLevel)
        {
            HexGeometry.HexToPixel(Location.X, Location.Y, out int pixelX, out int pixelY);
            pixelX -= pixelXSurface;
            pixelY -= pixelYSurface;
            _angle = _angle + 20 + (_animation.FrameCounter / 5);
            RandSeed = StrategicMap.Current.InitialRandomSeed;
            for (int i = 0; i <= 200; i++)
            {
                direction = Random(360);
                if (animation.FrameCounter >= (int)(((uint)i) >> 2) && animation.FrameCounter <= (int)(((uint)i) >> 4) + 68)
                {
                    radius = Round((double)((double)2.997 - Math.Log((double)(i * 19 * 0.005) + 1)) * 35) + 7;
                    height = 200 - i - 16;
                    xShift = Round((double)((double)Math.Sin((double)(_animation.FrameCounter * 0.1) + (i * 0.02)) * (200 - i)) * 0.25) + 16;
                    double pi = Math.PI;
                    angle = (float)((double)((direction + _angle) * pi) / 180);
                    last = GetEngine().AnimationDetail == 0
                        ? 1
                        : GetEngine().AnimationDetail == 1 ? (int)(((uint)(200 - i)) >> 5) : (int)(((uint)(200 - i)) >> 4);

                    for (int count = last; count >= 0; count--)
                    {
                        x = Round((double)Math.Cos(angle) * radius) + xShift;
                        y = Round((double)((double)Math.Sin(angle) * radius) * 0.5) + height;
                        _ = GamePack.Current.SpellControl.GetSpell(68).CastImages.Show(0, pixelX + x, pixelY - y, 0, lockedSurface);
                        angle = (float)((double)0.035 + angle);
                    }
                }
            }
        }
    }

    // GlobalTargetSpells.pas:215
    public override bool Validate(Hero source) => base.Validate(source) && (ArmyId == null || ArmyId.Valid());
}
