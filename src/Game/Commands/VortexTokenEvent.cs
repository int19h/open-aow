#nullable disable
// Translated from GlobalTargetSpells.pas.
using Aow.Engine.Nodes;
using Aow.Formats.Tree;
using Aow.Game.Combat;
using Aow.Game.Draw;
using Aow.Game.Draw.Animations;
using Aow.Game.Events;
using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Rtl;
using static Aow.Rtl.DelphiRandom;
using static Aow.Rtl.DelphiRounding;

namespace Aow.Game.Commands;

// angle, animation, selected army identity
public partial class VortexTokenEvent(CustomNode owner) : GlobalTargetSpellTokenEvent(owner)
{

    // GlobalTargetSpells.pas:1905
    public new const int ClassId = 135973;

    private int _angle;
    private Animation _animation;

    public ArmyId ArmyId { get; set; }

    // GlobalTargetSpells.pas:1899
    public override void Dispose()
    {
        ArmyId?.Dispose();
        base.Dispose();
    }

    // GlobalTargetSpells.pas:1939
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        ArmyId = persister.Created("ArmyId", objectValue: ArmyId);
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
            Spell spell = GamePack.Current.SpellControl.GetSpell(EventSpellId);
            GamePack.Current.SpellControl.GetSpell(EventSpellId).CastingDone(source, spell.BaseManaCost);
            _ = StrategicMap.Current.ValidateObservation(source.Player, Location.X, Location.Y, Location.MapLevel, 2);
            if (source.Player == StrategicMap.Current.SeatedPlayer || StrategicMap.Current.Watching(Location.X, Location.Y, Location.MapLevel, 2))
            {
                _ = GamePack.Current.SpellControl.GetSpell(EventSpellId).CastSounds.Play(0);
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
                log = new ArmyEventLog(null)
                {
                    Holder = CreationHolder(),
                    ArmyText = AbilityRules.TranslateResourceString(GetEngine(), s_vortexDamageText),
                    Player = source.Player
                };
                string vortex = AbilityRules.TranslateResourceString(GetEngine(), GameText.VortexText);
                _ = log.ArmyMessages.Add(vortex);
                log.ArmyIcon = 2;
                log.ArmyView.Setup(army.Army, 255);
                using (army.Army.BeginUpdate())
                {
                    damaged = false;
                    for (int i = 0, last = army.Army.Units.Count - 1; i <= last; i++)
                    {
                        troopObject = army.Army.Units.GetUnit(i);
                        if (!troopObject.GetMoveTypes().Contains(MoveType.Flying))
                        {
                            damage = troopObject.GetAbilityEnabled(2) ? 7 : troopObject.GetAbilityEnabled(23) ? 10 : 0;

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

                        _ = log.DamageValues.Add((byte)damage);
                        _ = log.DamageTypes.Add(7);
                    }

                    if (damaged)
                    {
                        log.ValidateGameOver();
                        EventLog.DistributeLocationEventLog(y: army.HexY, x: army.HexX, log: log, player: army.Army.Player, level: army.HexLevel, execute: true);
                    }

                    _ = log.Release();
                    army.Army.Validate();
                    army.Army.ValidateTerrain(army.Owner.TerrainType, army.Owner.OverlayType, source.Player);
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

    // GlobalTargetSpells.pas:1945
    public void ShowAnimation(Animation animation, int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        int xShift;
        int height;
        int x;
        int y;
        float angle;
        int direction;
        int radius;
        int count;
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
                if (animation.FrameCounter >= (int)(((uint)(200 - i)) >> 2) && animation.FrameCounter <= (int)(((uint)i) >> 4) + 68)
                {
                    radius = Round((double)((double)2.997 - Math.Log((double)(i * 19 * 0.005) + 1)) * 35) + 18;
                    height = -16;
                    xShift = 16;
                    double pi = Math.PI;
                    angle = (float)((double)(-(direction + _angle) * pi) / 180);
                    count = GetEngine().AnimationDetail == 0
                        ? 1
                        : GetEngine().AnimationDetail == 1 ? (int)(((uint)(200 - i)) >> 5) : (int)(((uint)(200 - i)) >> 4);

                    for (int j = count; j >= 0; j--)
                    {
                        x = Round((double)Math.Cos(angle) * radius) + xShift;
                        y = Round((double)((double)Math.Sin(angle) * radius) / 1.75) + height;
                        _ = GamePack.Current.SpellControl.GetSpell(70).CastImages.Show(0, pixelX + x, pixelY - y, 0, lockedSurface);
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
