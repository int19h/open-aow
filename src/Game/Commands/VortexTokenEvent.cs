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

    private int _angle;
    private Animation _animation;
    public ArmyId ArmyId;

    // GlobalTargetSpells.pas:1899
    public override void Dispose()
    {
        ArmyId?.Dispose();
        base.Dispose();
    }

    // GlobalTargetSpells.pas:1939
    public override void ReadWrite(StorageStream stream)
    {
        base.ReadWrite(stream);
        stream.Created(ref ArmyId, null, 40);
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
            _angle = 0;
            var spell = GamePack.Current.SpellControl.GetSpell(EventSpellId);
            GamePack.Current.SpellControl.GetSpell(EventSpellId).CastingDone(source, spell.BaseManaCost);
            StrategicMap.Current.ValidateObservation(source.Player, Location.X, Location.Y, Location.MapLevel, 2);
            if (source.Player == StrategicMap.Current.SeatedPlayer || StrategicMap.Current.Watching(Location.X, Location.Y, Location.MapLevel, 2))
            {
                GamePack.Current.SpellControl.GetSpell(EventSpellId).CastSounds.Play(0);
            }

            animation = new Animation(null) { Holder = CreationHolder() };
            _animation = animation;
            animation.AnimationLevel = 27;
            animation.OnShow += ShowAnimation;
            animation.AnimationMap = StrategicMap.Current;
            animation.Tag = (StrategicMap.Current.ViewWidth & 65535) | (StrategicMap.Current.ViewHeight << 16);
            animation.Activate();
        }

        if (value == 80)
        {
            if (ArmyId != null && ArmyId.RetrieveArmy(ref army, ref selection))
            {
                log = new ArmyEventLog(null) { Holder = CreationHolder() };
                log.ArmyText = AbilityRules.TranslateResourceString(GetEngine(), s_vortexDamageText);
                log._player = source.Player;
                var vortex = AbilityRules.TranslateResourceString(GetEngine(), GameText.VortexText);
                log.ArmyMessages.Add(vortex);
                log.ArmyIcon = 2;
                log.ArmyView.Setup(army.Army, 255);
                army.Army.BeginUpdate();
                damaged = false;
                for (int I = 0, last = army.Army.Units.Count - 1; I <= last; I++)
                {
                    troopObject = army.Army.Units.GetUnit(I);
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
                army.Army.ValidateTerrain(army.Owner.TerrainType, army.Owner.OverlayType, source.Player);
                army.Army.EndUpdate();
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
        if (StrategicMap.Current.ViewMapLevel == Location.MapLevel)
        {
            HexGeometry.HexToPixel(Location.X, Location.Y, out pixelX, out pixelY);
            pixelX -= pixelXSurface;
            pixelY -= pixelYSurface;
            _angle = this._angle + 20 + _animation.FrameCounter / 5;
            RandSeed = StrategicMap.Current.InitialRandomSeed;
            for (int I = 0; I <= 200; I++)
            {
                direction = Random(360);
                if (animation.FrameCounter >= (int)(((uint)(200 - I)) >> 2) && animation.FrameCounter <= (int)(((uint)I) >> 4) + 68)
                {
                    radius = Round((double)((double)2.997 - Math.Log((double)((double)(I * 19) * 0.005) + 1)) * 35) + 18;
                    height = -16;
                    xShift = 16;
                    var pi = Math.PI;
                    angle = (float)((double)((double)-(direction + _angle) * pi) / 180);
                    if (GetEngine().AnimationDetail == 0)
                    {
                        count = 1;
                    }
                    else
                    {
                        if (GetEngine().AnimationDetail == 1)
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
                        GamePack.Current.SpellControl.GetSpell(70).CastImages.Show(0, pixelX + X, pixelY - Y, 0, lockedSurface);
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
