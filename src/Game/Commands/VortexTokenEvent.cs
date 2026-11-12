// Translated from GlobalTargetSpells.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Combat;

using Aow.Game.Draw;
using Aow.Game.Draw.Animations;

using Aow.Game.Events;

using Aow.Game.Magic.Abilities;

using Aow.Game.Units;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Commands;

// angle, animation, selected army identity
public partial class VortexTokenEvent : GlobalTargetSpellTokenEvent
{
    // GlobalTargetSpells.pas:1905
    public new const int ClassId = 135973;

    public int Angle;
    public Animation Animation;
    public ArmyId ArmyId;

    // GlobalTargetSpells.pas:1899
    public override void Dispose()
    {
        ArmyId?.Dispose();
        base.Dispose();
    }

    // GlobalTargetSpells.pas:1939
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwCreateEObject(ref ArmyId, null, 40);
    }

    // GlobalTargetSpells.pas:1893
    public VortexTokenEvent(ECustomNode owner) : base(owner)
    {
        ArmyId = null;
    }

    // GlobalTargetSpells.pas:1991
    public override bool Process(Troop source, int value)
    {
        bool result = default;
        Animation animation = default;
        ArmyEventLog log = default;
        byte selection = default;
        bool damaged = default;
        int damage = default;
        ArmyHexSprite army = default;
        Troop troopObject = default;
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

            animation = new Animation(null);
            Animation = animation;
            animation.AnimationLevel = 27;
            animation.OnShow = ShowAnimation;
            animation.AnimationMap = AoWHSMap.Current;
            animation.Tag = (AoWHSMap.Current.ViewWidth & 65535) | (AoWHSMap.Current.ViewHeight << 16);
            animation.Activate();
        }

        if (value == 80)
        {
            if (ArmyId != null && ArmyId.RetrieveArmy(ref army, ref selection))
            {
                log = new ArmyEventLog(null);
                log.ArmyText = AbilityRules.TranslateResourceString(s_vortexDamageText);
                log._player = source.Player;
                var vortex = AbilityRules.TranslateResourceString(GameText.s_vortexText);
                log.ArmyMessages.Add(vortex);
                log.ArmyIcon = 2;
                log.ArmyView.Setup(army.Army, 255);
                army.Army.BeginUpdate();
                damaged = false;
                for (int I = 0, last = army.Army.Count - 1; I <= last; I++)
                {
                    troopObject = army.Army.GetUnit(I);
                    if (!troopObject.GetMoveTypes().Contains(MoveType.Flying))
                    {
                        if (troopObject.GetAbilityEnabled(2))
                        {
                            damage = 7;
                        }
                        else
                        {
                            if (troopObject.GetAbilityEnabled(23))
                            {
                                damage = 10;
                            }
                            else
                            {
                                damage = 0;
                            }
                        }

                        damage = troopObject.ExecuteDamageRole(8, damage, Set<DamageType>.Of(DamageType.Physical));
                        if (damage > troopObject.GetHitPoints())
                        {
                            damage = troopObject.GetHitPoints();
                        }

                        if (damage > 0)
                        {
                            troopObject.SetHitPoints((sbyte)(troopObject.GetHitPoints() - damage));
                            damaged = true;
                        }
                    }
                    else
                    {
                        damage = 0;
                    }

                    log.DamageValues.Add((byte)damage);
                    log.DamageTypes.Add(7);
                }

                if (damaged)
                {
                    log.ValidateGameOver();
                    var hexY = army.HexY;
                    var hexX = army.HexX;
                    EventLog.DistributeLocationEventLog(log, army.Army.Player, hexX, hexY, army.HexLevel, true);
                }

                log.Release();
                army.Army.Validate();
                army.Army.ValidateTerrain(((MapField)army.Owner).TerrainType, ((MapField)army.Owner).OverlayType, source.Player);
                army.Army.EndUpdate();
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

    // GlobalTargetSpells.pas:1945
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
        int count;
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
                if (animation.FrameCounter >= (int)(((uint)(200 - I)) >> 2) && animation.FrameCounter <= (int)(((uint)I) >> 4) + 68)
                {
                    radius = Round((double)((double)2.997 - Math.Log((double)((double)(I * 19) * 0.005) + 1)) * 35) + 18;
                    height = -16;
                    xShift = 16;
                    var pi = Math.PI;
                    angle = (float)((double)((double)-(direction + Angle) * pi) / 180);
                    if (GameTables.GameEngine.AnimationDetail == 0)
                    {
                        count = 1;
                    }
                    else
                    {
                        if (GameTables.GameEngine.AnimationDetail == 1)
                        {
                            count = (int)(((uint)(200 - I)) >> 5);
                        }
                        else
                        {
                            count = (int)(((uint)(200 - I)) >> 4);
                        }
                    }

                    for (int J = count; J >= 0; J--)
                    {
                        X = Round((double)Math.Cos(angle) * radius) + xShift;
                        Y = Round((double)((double)Math.Sin(angle) * radius) / 1.75) + height;
                        AoWHSSet.Current.SpellControl.GetSpell(70).CastImages.Show(0, pixelX + X, pixelY - Y, 0, lockedSurface);
                        angle = (float)((double)0.035 + angle);
                    }
                }
            }
        }
    }

    // GlobalTargetSpells.pas:242
    public override bool Validate(Hero source) => base.Validate(source) && (ArmyId == null || ArmyId.Valid());

    private static readonly string s_vortexDamageText = "Vortex Damage";
}
