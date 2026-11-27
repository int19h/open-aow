#nullable disable warnings
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
public class VortexTokenEvent(CustomNode? owner) : GlobalTargetSpellTokenEvent(owner)
{

    // GlobalTargetSpells.pas:1905
    public new const int ClassId = 135_973;

    private int _angle;
    private Animation _animation;

    public ArmyId? ArmyId { get; set; }

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

            Animation animation = new(null, show: ShowAnimation) { Holder = CreationHolder() };
            _animation = animation;
            animation.AnimationLevel = 27;
            StrategicMap engineStrategicMap = GetEngine().StrategicMap;
            animation.AnimationMap = engineStrategicMap;
            animation.Tag = (engineStrategicMap.ViewWidth & 0xFFFF) | (engineStrategicMap.ViewHeight << 16);
            animation.Activate();
        }
        if (value == 80)
        {
            int selection = default;
            ArmyHexSprite? army = default;
            if (ArmyId is not null && ArmyId.RetrieveArmy(ref army, ref selection))
            {
                ArmyEventLog log = new(null, holder: CreationHolder())
                {
                    ArmyText = AbilityRules.TranslateResourceString(GetEngine(), S: s_vortexDamageText),
                    Player = source.Player
                };
                string vortex = AbilityRules.TranslateResourceString(GetEngine(), S: GameText.VortexText);
                _ = log.ArmyMessages.Add(vortex);
                log.ArmyIcon = 2;
                log.ArmyView.Setup(army.Army, selection: 255);
                using (army.Army.BeginUpdate())
                {
                    bool damaged = false;
                    for (int i = 0, last = army.Army.Units.Count - 1; i <= last; i++)
                    {
                        Troop? troopObject = army.Army.Units.GetUnit(i);
                        int damage;
                        if (!troopObject.GetMoveTypes().Contains(MoveTypes.Flying))
                        {
                            damage = troopObject.GetAbilityEnabled(2) ? 7 : troopObject.GetAbilityEnabled(23) ? 10 : 0;

                            damage = troopObject.ExecuteDamageRole(8, damage, damageTypes: DamageTypes.Physical);
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

                        _ = log.DamageValues.Add(damage.TruncateToByte());
                        _ = log.DamageTypes.Add(7);
                    }

                    if (damaged)
                    {
                        log.ValidateGameOver();
                        EventLog.DistributeLocationEventLog(y: army.HexY, x: army.HexX, log: log, player: army.Army.Player, level: army.HexLevel, execute: true);
                    }

                    _ = log.Release();
                    army.Army.Validate();
                    army.Army.ValidateTerrain(terrainType: army.Owner.TerrainNumber, overlayType: army.Owner.OverlayType.ToOriginal(), source.Player);
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

    // GlobalTargetSpells.pas:1945
    public void ShowAnimation(Animation animation, int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        StrategicMap strategicMap = GetEngine().StrategicMap;
        if (strategicMap.ViewMapLevel == Location.MapLevel)
        {
            HexGeometry.HexToPixel(Location.X, Location.Y, out int pixelX, out int pixelY);
            pixelX -= pixelXSurface;
            pixelY -= pixelYSurface;
            _angle = _angle + 20 + (_animation.FrameCounter / 5);
            RandSeed = strategicMap.InitialRandomSeed;
            for (int i = 0; i <= 200; i++)
            {
                int direction = Random(360);
                if (animation.FrameCounter >= (200 - i) >>> 2 && animation.FrameCounter <= (i >>> 4) + 68)
                {
                    int radius = Round((double)((double)2.997 - Math.Log((double)(i * 19 * 0.005) + 1)) * 35) + 18;
                    int height = -16;
                    int xShift = 16;
                    double pi = Math.PI;
                    float angle = (float)((double)(-(direction + _angle) * pi) / 180);
                    int count = GetEngine().AnimationDetail == 0
                        ? 1
                        : GetEngine().AnimationDetail == 1 ? (200 - i) >>> 5 : (200 - i) >>> 4;

                    for (int j = count; j >= 0; j--)
                    {
                        int x = Round((double)Math.Cos(angle) * radius) + xShift;
                        int y = Round((double)((double)Math.Sin(angle) * radius) / 1.75) + height;
                        _ = GetEngine().GamePack.SpellControl[70].CastImages.Show(0, pixelX + x, pixelY - y, frame: 0, lockedSurface);
                        angle = (float)((double)0.035 + angle);
                    }
                }
            }
        }
    }

    // GlobalTargetSpells.pas:242
    public override bool Validate(Hero source) => base.Validate(source) && (ArmyId is null || ArmyId.Valid());

    private static readonly string s_vortexDamageText = "Vortex Damage";
}
