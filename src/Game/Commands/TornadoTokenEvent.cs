#nullable disable warnings
// Translated from GlobalTargetSpells.pas.
using Aow.Engine.Nodes;
using Aow.Formats.Tree;
using Aow.Game.AI.Groups;
using Aow.Game.Draw;
using Aow.Game.Draw.Animations;
using Aow.Game.Events;
using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.Session;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Game.World.Structures;
using static Aow.Rtl.DelphiFormat;
using static Aow.Rtl.DelphiRandom;
using static Aow.Rtl.DelphiRounding;

namespace Aow.Game.Commands;

// angle, animation, selected army identity
public class TornadoTokenEvent(CustomNode? owner) : GlobalTargetSpellTokenEvent(owner)
{

    // GlobalTargetSpells.pas:1559
    public new const int ClassId = 135_972;

    private int _angle;
    private Animation _animation;

    public ArmyId? ArmyId { get; set; }

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
        if (value == 0)
        {
            _angle = 0;
            GamePack gamePack = GetEngine().GamePack;
            Spell? spell = gamePack.SpellControl[EventSpellId];
            gamePack.SpellControl[EventSpellId].CastingDone(source, mana: spell.BaseManaCost);
            _ = GetEngine().StrategicMap.ValidateObservation(source.Player, Location.X, Location.Y, Location.MapLevel, radius: 2);
            StrategicMap strategicMap = GetEngine().StrategicMap;
            if (source.Player == strategicMap.SeatedPlayer || strategicMap.Watching(Location.X, Location.Y, L: Location.MapLevel, radius: 2))
            {
                _ = GetEngine().GamePack.SpellControl[EventSpellId].CastSounds.Play(0);
            }

            // The original constructs a plain animation and stores it on the event. The later warp uses an image sequence.
            _animation = new Animation(null, show: ShowAnimation)
            {
                Holder = CreationHolder(),
                AnimationLevel = 27
,
                AnimationMap = GetEngine().StrategicMap,
                Tag = (GetEngine().StrategicMap.ViewWidth & 0xFFFF) | (GetEngine().StrategicMap.ViewHeight << 16)
            };
            _animation.Activate();
        }
        if (value == 80)
        {
            MapField mapField = GetEngine().StrategicMap.MapContainer.GetField(Location.X, Location.Y, Location.MapLevel);
            if (ArmyId is not null && !mapField.GetMagicShieldActive())
            {
                int selection = default;
                ArmyHexSprite? army = default;
                _ = ArmyId.RetrieveArmy(ref army, ref selection);
                bool exposed = !((byte)mapField.OverlayType == 1 || mapField.FindNoneTransparentHexSprite(typeof(Structure)) is not null);
                Troop? troopObject;
                for (int i = 0, last = army.Army.Units.Count - 1; i <= last; i++)
                {
                    if ((selection & (1 << i)) != 0)
                    {
                        troopObject = army.Army.Units.GetUnit(i);
                        int chance = troopObject.UnitLevel switch
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

                        if (chance < GetEngine().StrategicMap.Random(100))
                        {
                            selection &= ~(1 << i);
                        }
                    }
                }

                if (army.Army.Transporter(255) is not null)
                {
                    selection = army.Army.Transporter(selection) is not null ? 255 : 0;
                }

                if (selection != 0)
                {
                    ArmyEventLog log = new(null)
                    {
                        Holder = CreationHolder(),
                        Player = source.Player
                    };
                    string tornado = AbilityRules.TranslateResourceString(GetEngine(), S: GameText.TornadoText);
                    log.ArmyText = Format(AbilityRules.TranslateResourceString(GetEngine(), S: GameText.CastFormat), args: [tornado]);
                    string tornadoText = AbilityRules.TranslateResourceString(GetEngine(), S: GameText.TornadoText);
                    _ = log.ArmyMessages.Add(tornadoText);
                    log.ArmyIcon = 2;
                    log.ArmyView.Setup(army.Army, selection);
                    EventLog.DistributeLocationEventLog(y: army.HexY, x: army.HexX, log: log, player: PlayerId.FromOriginal((sbyte)PlayerIdExtensions.ToOriginal(EventPlayerIndex)), level: army.HexLevel, execute: true);
                    _ = log.Release();
                    MapCoordinate location = GetEngine().GamePack.SpellControl.GetSpell<Tornado>(68).CalculateWarpLocation(army, selection);
                    if (location != MapCoordinate.None)
                    {
                        StrategicMap engineStrategicMap = GetEngine().StrategicMap;
                        if (source.Player == engineStrategicMap.SeatedPlayer || engineStrategicMap.Watching(Location.X, Location.Y, L: Location.MapLevel, radius: 2))
                        {
                            _ = GetEngine().GamePack.SpellControl[EventSpellId].CastSounds.Play(1);
                        }

                        ImageSequenceAnimation animation;
                        StrategicMap visibleForPlayerStrategicMap = GetEngine().StrategicMap;
                        if (visibleForPlayerStrategicMap.SeatedPlayer == source.Player || visibleForPlayerStrategicMap.VisibleForPlayer(Location.X, Location.Y, Location.MapLevel, player: visibleForPlayerStrategicMap.SeatedPlayer))
                        {
                            GamePack engineGamePack = GetEngine().GamePack;
                            if (engineGamePack.SpellControl[EventSpellId].CastImages.ImageSequences[1] is not null)
                            {
                                animation = new ImageSequenceAnimation(null) { Holder = CreationHolder() };
                                animation.SetImageSequence(engineGamePack.SpellControl[EventSpellId].CastImages.ImageSequences[1]);
                                _ = GetEngine().StrategicMap.PlaceHex(animation, Location.X, Location.Y, L: Location.MapLevel);
                            }
                        }

                        StrategicMap seatedPlayerStrategicMap = GetEngine().StrategicMap;
                        if (seatedPlayerStrategicMap.SeatedPlayer == source.Player || seatedPlayerStrategicMap.VisibleForPlayer(location.X, location.Y, location.MapLevel, player: seatedPlayerStrategicMap.SeatedPlayer))
                        {
                            GamePack setImageSequenceGamePack = GetEngine().GamePack;
                            if (setImageSequenceGamePack.SpellControl[EventSpellId].CastImages.ImageSequences[1] is not null)
                            {
                                animation = new ImageSequenceAnimation(null) { Holder = CreationHolder() };
                                animation.SetImageSequence(setImageSequenceGamePack.SpellControl[EventSpellId].CastImages.ImageSequences[1]);
                                _ = GetEngine().StrategicMap.PlaceHex(animation, location.X, location.Y, L: location.MapLevel);
                            }
                        }

                        UnitUserList movedUnits = new(null) { Holder = CreationHolder() };
                        byte owner = (byte)PlayerIdExtensions.ToOriginal(army.Army.Player);
                        mapField = GetEngine().StrategicMap.MapContainer.GetFieldMapCoordinate(location);
                        if (army.Army.ValidateSelection(255) != selection)
                        {
                            for (int i = army.Army.Units.Count - 1; i >= 0; i--)
                            {
                                if ((selection & (1 << i)) != 0)
                                {
                                    troopObject = army.Army.Units.GetUnit(i);
                                    _ = movedUnits.AddUnit(troopObject);
                                    ArmyHexSprite? placed = troopObject.Place(location.X, location.Y, L: location.MapLevel, player: PlayerId.FromOriginal((sbyte)(byte)PlayerIdExtensions.ToOriginal(army.Army.Player)), mode: UnitPlaceModes.None);
                                    placed?.Army.UpdateExploration();
                                }
                            }
                        }
                        else
                        {
                            _ = movedUnits.AddUnits(army.Army.Units);
                            _ = army.MoveTo(mapField, moveCost: 0);
                        }

                        if (owner == 0)
                        {
                            int unitsToIndependentRelation = UnitList.UnitsToIndependentRelation(movedUnits, defaultRelation: 5);
                            HuntAIGroup group = GetEngine().StrategicMap.AIGroupManager.CreateGroup<HuntAIGroup>(PlayerId.Independent, relation: unitsToIndependentRelation);
                            _ = group.AddUnits(movedUnits.Units);
                            _ = group.TargetUnitIds.Add(source.UnitId);
                            _ = group.Release();
                        }

                        movedUnits?.Dispose();
                    }
                }
            }

            _animation?.Dispose();
            return false;
        }
        else
        {
            return true;
        }
    }

    // GlobalTargetSpells.pas:1599
    public void ShowAnimation(Animation animation, int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        StrategicMap viewMapLevelStrategicMap = GetEngine().StrategicMap;
        if (viewMapLevelStrategicMap.ViewMapLevel == Location.MapLevel)
        {
            HexGeometry.HexToPixel(Location.X, Location.Y, out int pixelX, out int pixelY);
            pixelX -= pixelXSurface;
            pixelY -= pixelYSurface;
            _angle = _angle + 20 + (_animation.FrameCounter / 5);
            RandSeed = viewMapLevelStrategicMap.InitialRandomSeed;
            for (int i = 0; i <= 200; i++)
            {
                int direction = Random(360);
                if (animation.FrameCounter >= i >>> 2 && animation.FrameCounter <= (i >>> 4) + 68)
                {
                    int radius = Round((double)((double)2.997 - Math.Log((double)(i * 19 * 0.005) + 1)) * 35) + 7;
                    int height = 200 - i - 16;
                    int xShift = Round((double)((double)Math.Sin((double)(_animation.FrameCounter * 0.1) + (i * 0.02)) * (200 - i)) * 0.25) + 16;
                    double pi = Math.PI;
                    float angle = (float)((double)((direction + _angle) * pi) / 180);
                    int last = GetEngine().AnimationDetail == 0
                        ? 1
                        : GetEngine().AnimationDetail == 1 ? (200 - i) >>> 5 : (200 - i) >>> 4;

                    for (int count = last; count >= 0; count--)
                    {
                        int x = Round((double)Math.Cos(angle) * radius) + xShift;
                        int y = Round((double)((double)Math.Sin(angle) * radius) * 0.5) + height;
                        _ = GetEngine().GamePack.SpellControl[68].CastImages.Show(0, pixelX + x, pixelY - y, frame: 0, lockedSurface);
                        angle = (float)((double)0.035 + angle);
                    }
                }
            }
        }
    }

    // GlobalTargetSpells.pas:215
    public override bool Validate(Hero source) => base.Validate(source) && (ArmyId is null || ArmyId.Valid());
}
