// Translated from GlobalTargetSpells.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
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
    // GlobalTargetSpells.pas:1559
    public new const int ClassId = 135972;

    public int Angle;
    public Animation Animation;
    public ArmyId ArmyId;

    // GlobalTargetSpells.pas:1553
    public override void Dispose()
    {
        ArmyId?.Dispose();
        base.Dispose();
    }

    // GlobalTargetSpells.pas:1593
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwCreateEObject(ref ArmyId, null, 40);
    }

    // GlobalTargetSpells.pas:1547
    public TornadoTokenEvent(ECustomNode owner) : base(owner)
    {
        ArmyId = null;
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
        AoWMapField mapField = default;
        ArmyEventLog log = default;
        ImageSequenceAnimation animation = default;
        ArmyHexSprite placed = default;
        AIGroup group = default;
        if (value == 0)
        {
            Angle = 0;
            var spell = AoWHSSet.Current.SpellControl.GetSpell(EventSpellId);
            AoWHSSet.Current.SpellControl.GetSpell(EventSpellId).CastingDone(source, spell.BaseManaCost);
            AoWHSMap.Current.ValidateObservation(source.Player, Location.X, Location.Y, Location.MapLevel, 2);
            if (source.Player == AoWHSMap.Current.SeatedPlayer || AoWHSMap.Current.Watching(Location.X, Location.Y, Location.MapLevel, 2))
            {
                AoWHSSet.Current.SpellControl.GetSpell(EventSpellId).CastSounds.Play(0);
            }

            animation = (ImageSequenceAnimation)new Animation(null);
            Animation = animation;
            animation.AnimationLevel = 27;
            animation.OnShow = ShowAnimation;
            animation.AnimationMap = AoWHSMap.Current;
            animation.Tag = (AoWHSMap.Current.ViewWidth & 65535) | (AoWHSMap.Current.ViewHeight << 16);
            animation.Activate();
        }

        if (value == 80)
        {
            mapField = (AoWMapField)AoWHSMap.Current.MapContainer.GetField(Location.X, Location.Y, Location.MapLevel);
            if (ArmyId != null && !mapField.GetMagicShieldActive())
            {
                ArmyId.RetrieveArmy(ref army, ref selection);
                exposed = !(Set<byte>.WithBounds(0, 255).With(1).Contains((byte)mapField.OverlayType) || mapField.FindNoneTransparentHexSprite(typeof(Structure)) != null);
                for (int I = 0, last = army.Army.Count - 1; I <= last; I++)
                {
                    if ((selection & (1 << I)) != 0)
                    {
                        troopObject = army.Army.GetUnit(I);
                        switch (troopObject.GetUnitLevel())
                        {
                            case 1:
                                chance = 90;
                                break;
                            case 2:
                                chance = 70;
                                break;
                            case 3:
                                chance = 50;
                                break;
                            case 4:
                                chance = 30;
                                break;
                            default:
                                chance = 30;
                                break;
                        }

                        if (!exposed)
                        {
                            chance = chance / 2;
                        }

                        if (chance < AoWHSMap.Current.Random(100))
                        {
                            selection = (byte)(selection & ~(1 << I));
                        }
                    }
                }

                if (army.Army.Transporter(255) != null)
                {
                    if (army.Army.Transporter(selection) != null)
                    {
                        selection = 255;
                    }
                    else
                    {
                        selection = 0;
                    }
                }

                if (selection != 0)
                {
                    log = new ArmyEventLog(null);
                    log._player = source.Player;
                    var tornado = AbilityRules.TranslateResourceString(GameText.s_tornadoText);
                    log.ArmyText = RtlUnit.Format(AbilityRules.TranslateResourceString(GameText.s_castFormat), [tornado]);
                    var tornadoText = AbilityRules.TranslateResourceString(GameText.s_tornadoText);
                    log.ArmyMessages.Add(tornadoText);
                    log.ArmyIcon = 2;
                    log.ArmyView.Setup(army.Army, selection);
                    var hexY = army.HexY;
                    var hexX = army.HexX;
                    EventLog.DistributeLocationEventLog(log, (sbyte)EventPlayerIndex, hexX, hexY, army.HexLevel, true);
                    log.Release();
                    location.Packed = ((Tornado)AoWHSSet.Current.SpellControl.GetSpell(68)).CalculateWarpLocation(army, selection);
                    if (location.Packed != 16777215)
                    {
                        if (source.Player == AoWHSMap.Current.SeatedPlayer || AoWHSMap.Current.Watching(Location.X, Location.Y, Location.MapLevel, 2))
                        {
                            AoWHSSet.Current.SpellControl.GetSpell(EventSpellId).CastSounds.Play(1);
                        }

                        if (AoWHSMap.Current.SeatedPlayer == source.Player || AoWHSMap.Current.VisibleForPlayer(Location.X, Location.Y, Location.MapLevel, AoWHSMap.Current.SeatedPlayer))
                        {
                            if (AoWHSSet.Current.SpellControl.GetSpell(EventSpellId).CastImages.ImageSequences[1] != null)
                            {
                                animation = new ImageSequenceAnimation(null);
                                animation.SetImageSequence(AoWHSSet.Current.SpellControl.GetSpell(EventSpellId).CastImages.ImageSequences[1]);
                                AoWHSMap.Current.PlaceHex(animation, Location.X, Location.Y, Location.MapLevel);
                            }
                        }

                        if (AoWHSMap.Current.SeatedPlayer == source.Player || AoWHSMap.Current.VisibleForPlayer(location.X, location.Y, location.MapLevel, AoWHSMap.Current.SeatedPlayer))
                        {
                            if (AoWHSSet.Current.SpellControl.GetSpell(EventSpellId).CastImages.ImageSequences[1] != null)
                            {
                                animation = new ImageSequenceAnimation(null);
                                animation.SetImageSequence(AoWHSSet.Current.SpellControl.GetSpell(EventSpellId).CastImages.ImageSequences[1]);
                                AoWHSMap.Current.PlaceHex(animation, location.X, location.Y, location.MapLevel);
                            }
                        }

                        movedUnits = new UnitUserList(null);
                        owner = (byte)army.Army.Player;
                        mapField = (AoWMapField)AoWHSMap.Current.MapContainer.GetFieldMapCoordinate(location.Packed);
                        if (army.Army.ValidateSelection(255) != selection)
                        {
                            for (int I = army.Army.Count - 1; I >= 0; I--)
                            {
                                if ((selection & (1 << I)) != 0)
                                {
                                    troopObject = army.Army.GetUnit(I);
                                    movedUnits.AddUnit(troopObject);
                                    placed = troopObject.Place(location.X, location.Y, location.MapLevel, (sbyte)(byte)army.Army.Player, Set<UnitPlaceModeElement>.Of());
                                    if (placed != null)
                                    {
                                        placed.Army.UpdateExploration();
                                    }
                                }
                            }
                        }
                        else
                        {
                            movedUnits.AddUnits(army.Army);
                            army.MoveTo(mapField, 0);
                        }

                        if (owner == 0)
                        {
                            var unitsToIndependentRelation = UnitList.UnitsToIndependentRelation(movedUnits, 5);
                            group = AoWHSMap.Current.AIGroupManager.CreateGroup(0, unitsToIndependentRelation, GroupBehavior.Hunt);
                            group.AddUnits(movedUnits);
                            ((HuntAIGroup)group).TargetUnitIds.Add(source.UnitId);
                            group.Release();
                        }

                        movedUnits?.Dispose();
                    }
                }
            }

            Animation?.Dispose();
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
        int pixelX = default;
        int pixelY = default;
        int xShift;
        int height;
        int X;
        int Y;
        float angle;
        int direction;
        int radius;
        int last;
        if (AoWHSMap.Current.ViewL == Location.MapLevel)
        {
            HexGeometry.HexToPixel(Location.X, Location.Y, ref pixelX, ref pixelY);
            pixelX -= pixelXSurface;
            pixelY -= pixelYSurface;
            Angle = this.Angle + 20 + Animation.FrameCounter / 5;
            RandSeed = AoWHSMap.Current.InitialRandomSeed;
            for (int I = 0; I <= 200; I++)
            {
                direction = Random(360);
                if (animation.FrameCounter >= (int)(((uint)I) >> 2) && animation.FrameCounter <= (int)(((uint)I) >> 4) + 68)
                {
                    radius = Round((double)((double)2.997 - Math.Log((double)((double)(I * 19) * 0.005) + 1)) * 35) + 7;
                    height = 200 - I - 16;
                    xShift = Round((double)((double)Math.Sin((double)((double)Animation.FrameCounter * 0.1) + (double)I * 0.02) * (200 - I)) * 0.25) + 16;
                    var pi = Math.PI;
                    angle = (float)((double)((double)(direction + Angle) * pi) / 180);
                    if (GameTables.GameEngine.AnimationDetail == 0)
                    {
                        last = 1;
                    }
                    else
                    {
                        if (GameTables.GameEngine.AnimationDetail == 1)
                        {
                            last = (int)(((uint)(200 - I)) >> 5);
                        }
                        else
                        {
                            last = (int)(((uint)(200 - I)) >> 4);
                        }
                    }

                    for (int count = last; count >= 0; count--)
                    {
                        X = Round((double)Math.Cos(angle) * radius) + xShift;
                        Y = Round((double)((double)Math.Sin(angle) * radius) * 0.5) + height;
                        AoWHSSet.Current.SpellControl.GetSpell(68).CastImages.Show(0, pixelX + X, pixelY - Y, 0, lockedSurface);
                        angle = (float)((double)0.035 + angle);
                    }
                }
            }
        }
    }

    // GlobalTargetSpells.pas:215
    public override bool Validate(Hero source) => base.Validate(source) && (ArmyId == null || ArmyId.Valid());
}
