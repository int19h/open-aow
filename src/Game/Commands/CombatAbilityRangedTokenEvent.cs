#nullable disable
// Translated from CombatTE.pas.
using System.Globalization;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;
using Aow.Game.Audio;
using Aow.Game.Combat;
using Aow.Game.Combat.Actions;
using Aow.Game.Draw;
using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.Net;
using Aow.Game.World.Map;
using Aow.Rtl;
using static Aow.Rtl.DelphiRandom;
using static Aow.Rtl.DelphiRounding;
using static Aow.Rtl.RtlHost;

namespace Aow.Game.Commands;

public class CombatAbilityRangedTokenEvent(CustomNode owner) : TokenEvent(owner)
{

    // CombatTE.pas:917
    public new const int ClassId = 2_236_419;

    private TacticalBattleUnit _unit;

    public int AbilityId { get; set; }
    public int SpellId { get; set; }
    public ushort SourceX { get; set; }
    public ushort SourceY { get; set; }
    public ushort TargetX { get; set; }
    public ushort TargetY { get; set; }

    private BattleEffectType _effectType;
    private int _strikeCount;
    private object _sound;
    private int _delay;
    private int _strikeIndex;
    private int _state;

    // CombatTE.pas:920
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        AbilityId = persister.Int32("AbilityId", value: AbilityId);
        SpellId = persister.Int32("SpellId", value: SpellId);
        TargetX = persister.UInt16("TargetX", value: TargetX);
        TargetY = persister.UInt16("TargetY", value: TargetY);
        SourceX = persister.UInt16("SourceX", value: SourceX);
        SourceY = persister.UInt16("SourceY", value: SourceY);
    }

    // CombatTE.pas:931
    public override ExecuteResult Execute(TokenExecutor tokenEvent)
    {
        MapFieldMessage message = default;
        _ = base.Execute(tokenEvent);
        ExecuteResult result = ExecuteResult.Retry;
        try
        {
            int i;
            TacticalBattleUnitHexSprite hexSprite;
            switch (_state)
            {
                case 0:
                    try
                    {
                        hexSprite = CombatMap.Current.MapContainer.MapLevels[0].GetField(SourceX, SourceY).FindOwnedChild<TacticalBattleUnitHexSprite>(TacticalBattleUnitHexSprite.ClassId);
                        if (hexSprite is null)
                        {
                            result = ExecuteResult.Done;
                        }
                        else
                        {
                            _unit = hexSprite.Unit;
                            _unit.UnitHexSprite.Unselect();
                            _unit.UnitHexSprite.SelectedAbility = AbilityId;
                            _unit.UnitHexSprite.SelectedSpell = SpellId;
                            _effectType = AbilityId == 52 ? TacticalTables.SpellTypes[SpellId] : TacticalTables.AbilityTypes[AbilityId];

                            if (AbilityId == 52)
                            {
                                GamePack.Current.SpellControl[SpellId].CombatSpellCast(_unit);
                            }

                            bool notExplorationEnabled = !CombatMap.Current.ExplorationEnabled;
                            if (!notExplorationEnabled)
                            {
                                sbyte hexY = _unit.UnitHexSprite.HexY;
                                sbyte hexX = _unit.UnitHexSprite.HexX;
                                notExplorationEnabled = CombatMap.Current.MapContainer.GetField(hexX, hexY, 0).GetExplored(CombatMap.Current.SeatedPlayer);
                            }

                            if (notExplorationEnabled)
                            {
                                sbyte hexY = _unit.UnitHexSprite.HexY;
                                sbyte hexX = _unit.UnitHexSprite.HexX;
                                CombatMap.Current.MakeVisible(hexX, hexY, 0, radius: 2);
                            }

                            switch (_effectType)
                            {
                                case BattleEffectType.Breath:
                                    _strikeCount = 20;
                                    break;
                                case BattleEffectType.ScreenNESW:
                                    _strikeCount = 10;
                                    break;
                                case BattleEffectType.ScreenNS:
                                    _strikeCount = SpellId switch
                                    {
                                        116 => 21,
                                        124 => 19,
                                        _ => 35,
                                    };
                                    break;
                                case BattleEffectType.ShockWave:
                                    TargetX = SourceX;
                                    TargetY = SourceY;
                                    _strikeCount = 1;
                                    break;
                                case BattleEffectType.Touch:
                                    _strikeCount = 1;
                                    break;
                                case BattleEffectType.Swarm:
                                case BattleEffectType.ScreenSwarm:
                                    _strikeCount = 30;
                                    break;
                                case BattleEffectType.None:
                                case BattleEffectType.Arrow:
                                case BattleEffectType.Rock:
                                case BattleEffectType.Comet:
                                case BattleEffectType.CometBall:
                                case BattleEffectType.Effect:
                                case BattleEffectType.FBExplosion:
                                case BattleEffectType.Ray:
                                case BattleEffectType.Chain:
                                case BattleEffectType.SFOverlay:
                                case BattleEffectType.DamLightning:
                                case BattleEffectType.DamFire:
                                case BattleEffectType.DamPoison:
                                case BattleEffectType.DamMagic:
                                case BattleEffectType.DamHoly:
                                case BattleEffectType.DamDeath:
                                case BattleEffectType.DamCold:
                                case BattleEffectType.SacredWrath:
                                case BattleEffectType.SWComet:
                                case BattleEffectType.Dispel:
                                case BattleEffectType.WallSmoke:
                                case BattleEffectType.Blood:
                                case BattleEffectType.AltArrow:
                                case BattleEffectType.Tremors:
                                case BattleEffectType.Geyser:
                                case BattleEffectType.GeyserWater:
                                case BattleEffectType.HailImpact:
                                case BattleEffectType.Cloud:
                                case BattleEffectType.CloudPuff:
                                case BattleEffectType.HitGround:
                                case BattleEffectType.HitWater:
                                case BattleEffectType.HighPrayer:
                                case BattleEffectType.PrayerGlow:
                                case BattleEffectType.Resurrect:
                                case BattleEffectType.FrostBeam:
                                case BattleEffectType.Frost:
                                case BattleEffectType.MindDecay:
                                case BattleEffectType.DecayLeg:
                                case BattleEffectType.DecayComet:
                                case BattleEffectType.SacFlame:
                                case BattleEffectType.Winds:
                                case BattleEffectType.SacFire:
                                case BattleEffectType.BreathDam:
                                default:
                                    if (AbilityId == 52)
                                    {
                                        SpellCombatInfo info = default;
                                        GamePack.Current.SpellControl[SpellId].GetCombatInfo(_unit.Troop, ref info);
                                        _strikeCount = info.RepeatCount;
                                    }
                                    else
                                    {
                                        _strikeCount = GamePack.Current.AbilityControl.GetAbility<RangedAttackAbility>(AbilityId).GetAttackRepeatRangedAttack(_unit.Troop);
                                    }

                                    break;
                            }

                            switch (_effectType)
                            {
                                case BattleEffectType.Tremors:
                                    _sound = GamePack.Current.SpellControl[SpellId].CastSounds.Play(0, volume: 64, looped: true, delay: 0, panning: SamplePanning.Center, callback: null);
                                    NextStrike();
                                    break;
                                case BattleEffectType.ScreenSwarm:
                                    _sound = GamePack.Current.SpellControl[SpellId].CastSounds.Play(2, volume: 64, looped: true, delay: 0, panning: SamplePanning.Center, callback: null);
                                    {
                                        i = 0;
                                        int last = _strikeCount - 1;
                                        for (; i <= last; i += 1)
                                        {
                                            NextStrike();
                                        }
                                    }

                                    break;
                                case BattleEffectType.None:
                                case BattleEffectType.Arrow:
                                case BattleEffectType.Rock:
                                case BattleEffectType.Comet:
                                case BattleEffectType.Touch:
                                case BattleEffectType.Breath:
                                case BattleEffectType.CometBall:
                                case BattleEffectType.Effect:
                                case BattleEffectType.FBExplosion:
                                case BattleEffectType.ScreenNESW:
                                case BattleEffectType.ScreenNS:
                                case BattleEffectType.Ray:
                                case BattleEffectType.Chain:
                                case BattleEffectType.Swarm:
                                case BattleEffectType.SFOverlay:
                                case BattleEffectType.DamLightning:
                                case BattleEffectType.DamFire:
                                case BattleEffectType.DamPoison:
                                case BattleEffectType.DamMagic:
                                case BattleEffectType.DamHoly:
                                case BattleEffectType.DamDeath:
                                case BattleEffectType.DamCold:
                                case BattleEffectType.SacredWrath:
                                case BattleEffectType.SWComet:
                                case BattleEffectType.Dispel:
                                case BattleEffectType.WallSmoke:
                                case BattleEffectType.Blood:
                                case BattleEffectType.AltArrow:
                                case BattleEffectType.Geyser:
                                case BattleEffectType.GeyserWater:
                                case BattleEffectType.HailImpact:
                                case BattleEffectType.Cloud:
                                case BattleEffectType.CloudPuff:
                                case BattleEffectType.HitGround:
                                case BattleEffectType.HitWater:
                                case BattleEffectType.HighPrayer:
                                case BattleEffectType.PrayerGlow:
                                case BattleEffectType.Resurrect:
                                case BattleEffectType.FrostBeam:
                                case BattleEffectType.Frost:
                                case BattleEffectType.MindDecay:
                                case BattleEffectType.DecayLeg:
                                case BattleEffectType.ShockWave:
                                case BattleEffectType.DecayComet:
                                case BattleEffectType.SacFlame:
                                case BattleEffectType.Winds:
                                case BattleEffectType.SacFire:
                                case BattleEffectType.BreathDam:
                                default:
                                    NextStrike();
                                    break;
                            }

                            _unit.MovePoints = 0;
                            _state += 1;
                        }
                    }
                    catch
                    {
                        _ = MessageBox(0, text: "Error during Token Start", caption: $"Error - TCRangedToken {AbilityId.ToString(CultureInfo.InvariantCulture)}/{SpellId.ToString(CultureInfo.InvariantCulture)}", flags: 0);
                    }

                    break;
                case 1:
                    try
                    {
                        int effectIndex = 0;
                        int x = default;
                        int y = default;
                        while (effectIndex < CombatMap.Current.Effects.Count)
                        {
                            int endX;
                            int endY;
                            BattleEffectSprite effect = CombatMap.Current.Effects[effectIndex];
                            if (effect.AbilityIndex == 52 && effect.SpellIndex == 117)
                            {
                                if (effect.EffectType == BattleEffectType.Rock)
                                {
                                    if (effect.Done)
                                    {
                                        effect.EffectType = BattleEffectType.FBExplosion;
                                        effect.Done = false;
                                        while (effect.Path.Count > 1)
                                        {
                                            effect.Path.Delete(0);
                                        }
                                    }
                                }
                                else
                                {
                                    switch (effect.Frame)
                                    {
                                        case 5:
                                            message.MessageId = 200707;
                                            MapFieldMessageParameters param = message.Param;
                                            param[0] = _unit;
                                            message.Param = param;
                                            message.Result = -1;
                                            Point3D path = effect.Path[0];
                                            HexGeometry.PixelToHex(effect.Path[0].X, vertical: path.Y, out endX, out endY);
                                            {
                                                i = 0;
                                                for (; i <= 6; i += 1)
                                                {
                                                    HexGeometry.CenterHexNumberToHex(endX, endY, hexNumber: i, ref x, ref y);
                                                    CombatMap.Current.MapContainer.MapLevels[0].GetField(x, y).SendMapFieldMessage(ref message);
                                                    EffectPaths.CreateFire(50, x, y, engine: GetEngine());
                                                }
                                            }

                                            break;
                                        default:
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                if (effect.AbilityIndex == 52 && effect.SpellIndex == 107)
                                {
                                    if (effect.Frame != -1 && effect.Path[effect.Frame].Z == 15)
                                    {
                                        Point3D path = effect.Path[effect.Frame];
                                        HexGeometry.PixelToHex(effect.Path[effect.Frame].X, vertical: path.Y, out x, out y);
                                        hexSprite = CombatMap.Current.MapContainer.MapLevels[0].GetField(x, y).FindOwnedChild<TacticalBattleUnitHexSprite>(TacticalBattleUnitHexSprite.ClassId);
                                        if (hexSprite is not null)
                                        {
                                            RandSeed = StrategicMap.Current.Random(16777215);
                                            // Spell 107 builds a damage action. Another action class fails in the typed create.
                                            DamageCombatAction damage = GamePack.Current.SpellControl[effect.SpellIndex].CreateCombatAction<DamageCombatAction>(_unit, target: hexSprite.Unit, chargeMana: false);
                                            effect.CombatAction = damage;
                                            sbyte hexY = hexSprite.HexY;
                                            HexGeometry.HexToPixel(hexSprite.HexX, hexY, out i, out x);
                                            if ((damage.DamageTypes & ~damage.Effects) != DamageTypes.None)
                                            {
                                                CombatEvents.MakeDamageEffectHexSpriteList(i, x, damageTypes: damage.DamageTypes & ~damage.Effects, resisted: false);
                                            }

                                            if (damage.Effects != DamageTypes.None)
                                            {
                                                CombatEvents.MakeDamageEffectHexSpriteList(i, x, damageTypes: damage.Effects, resisted: true);
                                            }

                                            CombatMap.Current.Combat.ExecuteCombatAction(effect.CombatAction);
                                            _ = effect.CombatAction.Release();
                                            effect.CombatAction = null;
                                        }
                                    }
                                }
                                else
                                {
                                    if (effect.AbilityIndex == 52 && effect.SpellIndex == 112)
                                    {
                                        switch (effect.Frame)
                                        {
                                            case 89:
                                                message.MessageId = 200708;
                                                MapFieldMessageParameters param2 = message.Param;
                                                param2[0] = _unit;
                                                message.Param = param2;
                                                MapFieldMessageParameters param3 = message.Param;
                                                param3[1] = 0;
                                                message.Param = param3;
                                                message.Result = -1;
                                                {
                                                    i = 6;
                                                    for (; i <= 59; i += 1)
                                                    {
                                                        x = 6;
                                                        for (; x <= 52; x += 1)
                                                        {
                                                            CombatMap.Current.MapContainer.MapLevels[0].GetField(i, x).SendMapFieldMessage(ref message);
                                                        }
                                                    }
                                                }

                                                break;
                                            default:
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        if (effect.AbilityIndex == 52 && effect.EffectType == BattleEffectType.Resurrect && (effect.SpellIndex is 130 or 129))
                                        {
                                            switch (effect.Frame)
                                            {
                                                case 0:
                                                    message.MessageId = 200712;
                                                    MapFieldMessageParameters param4 = message.Param;
                                                    param4[0] = _unit;
                                                    message.Param = param4;
                                                    MapFieldMessageParameters param5 = message.Param;
                                                    param5[1] = SpellId;
                                                    message.Param = param5;
                                                    message.Result = -1;
                                                    {
                                                        i = 0;
                                                        for (; i <= 6; i += 1)
                                                        {
                                                            HexGeometry.CenterHexNumberToHex(TargetX, TargetY, hexNumber: i, ref x, ref y);
                                                            CombatMap.Current.MapContainer.MapLevels[0].GetField(x, y).SendMapFieldMessage(ref message);
                                                        }
                                                    }

                                                    break;
                                                case 99:
                                                    message.MessageId = 200713;
                                                    MapFieldMessageParameters param6 = message.Param;
                                                    param6[0] = _unit;
                                                    message.Param = param6;
                                                    MapFieldMessageParameters param7 = message.Param;
                                                    param7[1] = SpellId;
                                                    message.Param = param7;
                                                    message.Result = -1;
                                                    {
                                                        i = 0;
                                                        for (; i <= 6; i += 1)
                                                        {
                                                            HexGeometry.CenterHexNumberToHex(TargetX, TargetY, hexNumber: i, ref x, ref y);
                                                            CombatMap.Current.MapContainer.MapLevels[0].GetField(x, y).SendMapFieldMessage(ref message);
                                                        }
                                                    }

                                                    break;
                                                default:
                                                    break;
                                            }
                                        }
                                        else
                                        {
                                            if (effect.AbilityIndex == 52 && effect.SpellIndex == 104)
                                            {
                                                switch (effect.Frame)
                                                {
                                                    case 199:
                                                        message.MessageId = 200709;
                                                        MapFieldMessageParameters param8 = message.Param;
                                                        param8[0] = _unit;
                                                        message.Param = param8;
                                                        message.Result = -1;
                                                        {
                                                            i = 0;
                                                            for (; i <= 6; i += 1)
                                                            {
                                                                HexGeometry.CenterHexNumberToHex(TargetX, TargetY, hexNumber: i, ref x, ref y);
                                                                CombatMap.Current.MapContainer.MapLevels[0].GetField(x, y).SendMapFieldMessage(ref message);
                                                            }
                                                        }

                                                        break;
                                                    default:
                                                        break;
                                                }
                                            }
                                            else
                                            {
                                                if (effect.AbilityIndex == 52 && effect.SpellIndex == 127)
                                                {
                                                    switch (effect.Frame)
                                                    {
                                                        case 175:
                                                            message.MessageId = 200717;
                                                            MapFieldMessageParameters param9 = message.Param;
                                                            param9[0] = _unit;
                                                            message.Param = param9;
                                                            message.Result = -1;
                                                            {
                                                                i = 1;
                                                                for (; i <= 6; i += 1)
                                                                {
                                                                    HexGeometry.CenterHexNumberToHex(TargetX, TargetY, hexNumber: i, ref x, ref y);
                                                                    CombatMap.Current.MapContainer.MapLevels[0].GetField(x, y).SendMapFieldMessage(ref message);
                                                                }
                                                            }

                                                            break;
                                                        case 200:
                                                            message.MessageId = 200717;
                                                            MapFieldMessageParameters param10 = message.Param;
                                                            param10[0] = _unit;
                                                            message.Param = param10;
                                                            message.Result = -1;
                                                            {
                                                                i = 7;
                                                                for (; i <= 18; i += 1)
                                                                {
                                                                    HexGeometry.CenterHexNumberToHex(TargetX, TargetY, hexNumber: i, ref x, ref y);
                                                                    CombatMap.Current.MapContainer.MapLevels[0].GetField(x, y).SendMapFieldMessage(ref message);
                                                                }
                                                            }

                                                            break;
                                                        case 225:
                                                            message.MessageId = 200717;
                                                            MapFieldMessageParameters param11 = message.Param;
                                                            param11[0] = _unit;
                                                            message.Param = param11;
                                                            message.Result = -1;
                                                            {
                                                                i = 19;
                                                                for (; i <= 36; i += 1)
                                                                {
                                                                    HexGeometry.CenterHexNumberToHex(TargetX, TargetY, hexNumber: i, ref x, ref y);
                                                                    CombatMap.Current.MapContainer.MapLevels[0].GetField(x, y).SendMapFieldMessage(ref message);
                                                                }
                                                            }

                                                            break;
                                                        default:
                                                            break;
                                                    }
                                                }
                                                else
                                                {
                                                    if (effect.AbilityIndex == 52 && effect.SpellIndex == 128)
                                                    {
                                                        switch (effect.Frame)
                                                        {
                                                            case 100:
                                                                sbyte hexY = _unit.UnitHexSprite.HexY;
                                                                EffectPaths.CreateFire(50, _unit.UnitHexSprite.HexX, hexY, engine: GetEngine());
                                                                message.MessageId = 200718;
                                                                MapFieldMessageParameters param12 = message.Param;
                                                                param12[0] = _unit;
                                                                message.Param = param12;
                                                                message.Result = -1;
                                                                sbyte fUnitHexSpriteHexY = _unit.UnitHexSprite.HexY;
                                                                sbyte hexX = _unit.UnitHexSprite.HexX;
                                                                CombatMap.Current.MapContainer.MapLevels[0].GetField(hexX, fUnitHexSpriteHexY).SendMapFieldMessage(ref message);
                                                                break;
                                                            case 175:
                                                                message.MessageId = 200718;
                                                                MapFieldMessageParameters param13 = message.Param;
                                                                param13[0] = _unit;
                                                                message.Param = param13;
                                                                message.Result = -1;
                                                                {
                                                                    i = 0;
                                                                    for (; i <= 18; i += 1)
                                                                    {
                                                                        HexGeometry.CenterHexNumberToHex(TargetX, TargetY, hexNumber: i, ref x, ref y);
                                                                        CombatMap.Current.MapContainer.MapLevels[0].GetField(x, y).SendMapFieldMessage(ref message);
                                                                        EffectPaths.CreateFire(50, x, y, engine: GetEngine());
                                                                    }
                                                                }

                                                                break;
                                                            default:
                                                                break;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (effect.AbilityIndex == 52 && effect.SpellIndex == 105 && effect.Target is not null)
                                                        {
                                                            i = 0;
                                                            int last = CombatMap.Current.Combat.CombatData.GetCount() - 1;
                                                            for (; i <= last; i += 1)
                                                            {
                                                                if (CombatMap.Current.Combat.CombatData.GetObjects(i) is not null && CombatMap.Current.Combat.CombatData.GetObjects(i) is CombatUnit && CombatMap.Current.Combat.CombatData.GetObjects(i).Side != _unit.Side && CombatMap.Current.Combat.CombatData.GetObjects(i).GetEnabled() && CombatMap.Current.Combat.CombatData.GetObjects<TacticalBattleUnit>(i).UnitHexSprite is not null)
                                                                {
                                                                    RandSeed = StrategicMap.Current.Random(16777215);
                                                                    CombatObject combatObject = CombatMap.Current.Combat.CombatData.GetObjects(i);
                                                                    ExclusiveCombatSpellCombatAction exclusive = GamePack.Current.SpellControl[SpellId].CreateCombatAction<ExclusiveCombatSpellCombatAction>(_unit, target: combatObject, chargeMana: false);
                                                                    CombatAction action = exclusive;
                                                                    if (exclusive.Succeeded)
                                                                    {
                                                                        CombatMap.Current.Combat.CombatData.GetObjects<TacticalBattleUnit>(i).UnitHexSprite.SetAction(TacticalBattleUnitHexSprite.SpriteAction.Hit);
                                                                    }

                                                                    CombatMap.Current.Combat.ExecuteCombatAction(action);
                                                                    _ = action.Release();
                                                                }
                                                            }

                                                            effect.Target = null;
                                                        }
                                                        else
                                                        {
                                                            if (effect.AbilityIndex == 52 && effect.SpellIndex == 120)
                                                            {
                                                                switch (effect.Frame)
                                                                {
                                                                    case 2_999:
                                                                        message.MessageId = 200716;
                                                                        MapFieldMessageParameters param14 = message.Param;
                                                                        param14[0] = _unit;
                                                                        message.Param = param14;
                                                                        message.Result = -1;
                                                                        {
                                                                            i = 0;
                                                                            for (; i <= 6; i += 1)
                                                                            {
                                                                                HexGeometry.CenterHexNumberToHex(TargetX, TargetY, hexNumber: i, ref x, ref y);
                                                                                CombatMap.Current.MapContainer.MapLevels[0].GetField(x, y).SendMapFieldMessage(ref message);
                                                                            }
                                                                        }

                                                                        break;
                                                                    default:
                                                                        break;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                if (effect.EffectType == BattleEffectType.SacredWrath && effect.AbilityIndex == 52 && effect.SpellIndex == 125)
                                                                {
                                                                    switch (effect.Frame)
                                                                    {
                                                                        case 5:
                                                                            {
                                                                                i = 0;
                                                                                int last = CombatMap.Current.Combat.CombatData.GetCount() - 1;
                                                                                for (; i <= last; i += 1)
                                                                                {
                                                                                    if (CombatMap.Current.Combat.CombatData.GetObjects(i) is CombatUnit && CombatMap.Current.Combat.CombatData.GetObjects(i).GetEnabled())
                                                                                    {
                                                                                        hexSprite = CombatMap.Current.Combat.CombatData.GetObjects<TacticalBattleUnit>(i).UnitHexSprite;
                                                                                        if (hexSprite is not null)
                                                                                        {
                                                                                            BattleEffectSprite extraEffect = CombatMap.Current.AddEffect(BattleEffectType.SWComet);
                                                                                            extraEffect.AbilityIndex = AbilityId;
                                                                                            extraEffect.SpellIndex = SpellId;
                                                                                            extraEffect.Target = hexSprite.Unit;
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }

                                                                            break;
                                                                        default:
                                                                            break;
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (effect.Done && effect.Target is not null && effect.Target.GetEnabled())
                                                                    {
                                                                        i = 0;
                                                                        RandSeed = StrategicMap.Current.Random(16777215);
                                                                        effect.CombatAction = AbilityId == 52
                                                                            ? GamePack.Current.SpellControl[SpellId].CreateCombatAction(_unit, effect.Target, chargeMana: false)
                                                                            : GamePack.Current.AbilityControl[AbilityId].CreateRangedCombatAction(_unit, target: effect.Target);

                                                                        if (effect.CombatAction is DamageCombatAction damage)
                                                                        {
                                                                            i = damage.Damage;
                                                                            sbyte hexY = effect.Target.UnitHexSprite.HexY;
                                                                            HexGeometry.HexToPixel(effect.Target.UnitHexSprite.HexX, hexY, out x, out y);
                                                                            if ((damage.DamageTypes & ~damage.Effects) != DamageTypes.None)
                                                                            {
                                                                                CombatEvents.MakeDamageEffectHexSpriteList(x, y, damageTypes: damage.DamageTypes & ~damage.Effects, resisted: false);
                                                                            }

                                                                            if (damage.Effects != DamageTypes.None)
                                                                            {
                                                                                CombatEvents.MakeDamageEffectHexSpriteList(x, y, damageTypes: damage.Effects, resisted: true);
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            if (effect.CombatAction is EntangleSpellCombatAction { Succeeded: true })
                                                                            {
                                                                                effect.Target.UnitHexSprite.SetAction(TacticalBattleUnitHexSprite.SpriteAction.Hit);
                                                                            }
                                                                            else
                                                                            {
                                                                                if (effect.CombatAction is MindDecayCombatAction { Succeeded: true })
                                                                                {
                                                                                    effect.Target.UnitHexSprite.SetAction(TacticalBattleUnitHexSprite.SpriteAction.Hit);
                                                                                }
                                                                            }
                                                                        }

                                                                        if (i > 0)
                                                                        {
                                                                            effect.Target.UnitHexSprite.SetAction(TacticalBattleUnitHexSprite.SpriteAction.Hit);
                                                                            TacticalBattleUnit.MakeHitBlood(effect.Target, damage: i);
                                                                            effect.Cancelled = true;
                                                                        }
                                                                    }

                                                                    if (effect.Done)
                                                                    {
                                                                        int count = effect.Path.Count;
                                                                        Point3D path = effect.Path[count - 1];
                                                                        int fPathCount = effect.Path.Count;
                                                                        HexGeometry.PixelToHex(effect.Path[fPathCount - 1].X, vertical: path.Y, out endX, out endY);
                                                                        if (effect.CombatAction is not null && effect.Target is not null)
                                                                        {
                                                                            CombatMap.Current.Combat.ExecuteCombatAction(effect.CombatAction);
                                                                            _ = effect.CombatAction.Release();
                                                                            effect.CombatAction = null;
                                                                            if (!effect.Target.GetEnabled())
                                                                            {
                                                                                i = 0;
                                                                                int last = CombatMap.Current.Effects.Count - 1;
                                                                                for (; i <= last; i += 1)
                                                                                {
                                                                                    if (i != effectIndex)
                                                                                    {
                                                                                        if (CombatMap.Current.Effects[i].Target == effect.Target)
                                                                                        {
                                                                                            CombatMap.Current.Effects[i].Target = null;
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            if (endX > -1)
                                                                            {
                                                                                switch (effect.AbilityIndex)
                                                                                {
                                                                                    case 30:
                                                                                    case >= 86 and <= 90:
                                                                                        if (effect.TargetX > 0)
                                                                                        {
                                                                                            message.MessageId = 200722;
                                                                                            MapFieldMessageParameters param15 = message.Param;
                                                                                            param15[0] = _unit;
                                                                                            message.Param = param15;
                                                                                            MapFieldMessageParameters param16 = message.Param;
                                                                                            param16[1] = effect.AbilityIndex;
                                                                                            message.Param = param16;
                                                                                            message.Result = -1;
                                                                                            CombatMap.Current.MapContainer.MapLevels[0].GetField(effect.TargetX, effect.TargetY).SendMapFieldMessage(ref message);
                                                                                        }

                                                                                        break;
                                                                                    case 24:
                                                                                        message.MessageId = 200705;
                                                                                        MapFieldMessageParameters param17 = message.Param;
                                                                                        param17[0] = _unit;
                                                                                        message.Param = param17;
                                                                                        message.Result = -1;
                                                                                        CombatMap.Current.MapContainer.MapLevels[0].GetField(endX, endY).SendMapFieldMessage(ref message);
                                                                                        break;
                                                                                    case 44:
                                                                                        message.MessageId = 200706;
                                                                                        MapFieldMessageParameters param18 = message.Param;
                                                                                        param18[0] = _unit;
                                                                                        message.Param = param18;
                                                                                        message.Result = -1;
                                                                                        CombatMap.Current.MapContainer.MapLevels[0].GetField(endX, endY).SendMapFieldMessage(ref message);
                                                                                        break;
                                                                                    case 31:
                                                                                        message.MessageId = 200714;
                                                                                        MapFieldMessageParameters param19 = message.Param;
                                                                                        param19[0] = _unit;
                                                                                        message.Param = param19;
                                                                                        message.Result = -1;
                                                                                        CombatMap.Current.MapContainer.MapLevels[0].GetField(endX, endY).SendMapFieldMessage(ref message);
                                                                                        EffectPaths.CreateFire(100, endX, endY, engine: GetEngine());
                                                                                        break;
                                                                                    case 52:
                                                                                        switch (effect.EffectType)
                                                                                        {
                                                                                            case BattleEffectType.Breath:
                                                                                                if (effect.TargetX > 0)
                                                                                                {
                                                                                                    message.MessageId = 200723;
                                                                                                    MapFieldMessageParameters param20 = message.Param;
                                                                                                    param20[0] = _unit;
                                                                                                    message.Param = param20;
                                                                                                    MapFieldMessageParameters param21 = message.Param;
                                                                                                    param21[1] = effect.SpellIndex;
                                                                                                    message.Param = param21;
                                                                                                    message.Result = -1;
                                                                                                    CombatMap.Current.MapContainer.MapLevels[0].GetField(effect.TargetX, effect.TargetY).SendMapFieldMessage(ref message);
                                                                                                }

                                                                                                break;
                                                                                            case BattleEffectType.ScreenNS:
                                                                                                switch (effect.SpellIndex)
                                                                                                {
                                                                                                    case 116:
                                                                                                        message.MessageId = 200710;
                                                                                                        MapFieldMessageParameters param22 = message.Param;
                                                                                                        param22[0] = _unit;
                                                                                                        message.Param = param22;
                                                                                                        message.Result = -1;
                                                                                                        CombatMap.Current.MapContainer.MapLevels[0].GetField(endX, endY).SendMapFieldMessage(ref message);
                                                                                                        break;
                                                                                                    case 124:
                                                                                                        if (CombatMap.Current.MapContainer.MapLevels[0].GetField(endX, endY).TerrainType != 0 && CombatMap.Current.MapContainer.MapLevels[0].GetField(endX, endY).TerrainType != 10 && CombatMap.Current.MapContainer.MapLevels[0].GetField(endX, endY).OverlayType != 2 && CombatMap.Current.MapContainer.MapLevels[0].GetField(endX, endY).OverlayType != 11 && CombatMap.Current.MapContainer.MapLevels[0].GetField(endX, endY).OverlayType != 8 && CombatMap.Current.MapContainer.MapLevels[0].GetField(endX, endY).TerrainType != 9 && CombatMap.Current.MapContainer.MapLevels[0].GetField(endX, endY).TerrainType != 15 && CombatMap.Current.MapContainer.MapLevels[0].GetField(endX, endY).TerrainType != 7)
                                                                                                        {
                                                                                                            TacticalBattleOozeHexSprite ooze = new(null) { Holder = CreationHolder() };
                                                                                                            _ = CombatMap.Current.PlaceHex(ooze, endX, endY, L: 0);
                                                                                                        }

                                                                                                        break;
                                                                                                    default:
                                                                                                        break;
                                                                                                }

                                                                                                break;
                                                                                            case BattleEffectType.Effect:
                                                                                                switch (effect.SpellIndex)
                                                                                                {
                                                                                                    case 113:
                                                                                                        message.MessageId = 200715;
                                                                                                        MapFieldMessageParameters param23 = message.Param;
                                                                                                        param23[0] = _unit;
                                                                                                        message.Param = param23;
                                                                                                        message.Result = -1;
                                                                                                        CombatMap.Current.MapContainer.MapLevels[0].GetField(endX, endY).SendMapFieldMessage(ref message);
                                                                                                        EffectPaths.CreateFire(100, endX, endY, engine: GetEngine());
                                                                                                        break;
                                                                                                    default:
                                                                                                        break;
                                                                                                }

                                                                                                break;
                                                                                            case BattleEffectType.None:
                                                                                            case BattleEffectType.Arrow:
                                                                                            case BattleEffectType.Rock:
                                                                                            case BattleEffectType.Comet:
                                                                                            case BattleEffectType.Touch:
                                                                                            case BattleEffectType.CometBall:
                                                                                            case BattleEffectType.FBExplosion:
                                                                                            case BattleEffectType.ScreenNESW:
                                                                                            case BattleEffectType.Ray:
                                                                                            case BattleEffectType.Chain:
                                                                                            case BattleEffectType.Swarm:
                                                                                            case BattleEffectType.ScreenSwarm:
                                                                                            case BattleEffectType.SFOverlay:
                                                                                            case BattleEffectType.DamLightning:
                                                                                            case BattleEffectType.DamFire:
                                                                                            case BattleEffectType.DamPoison:
                                                                                            case BattleEffectType.DamMagic:
                                                                                            case BattleEffectType.DamHoly:
                                                                                            case BattleEffectType.DamDeath:
                                                                                            case BattleEffectType.DamCold:
                                                                                            case BattleEffectType.SacredWrath:
                                                                                            case BattleEffectType.SWComet:
                                                                                            case BattleEffectType.Dispel:
                                                                                            case BattleEffectType.WallSmoke:
                                                                                            case BattleEffectType.Blood:
                                                                                            case BattleEffectType.AltArrow:
                                                                                            case BattleEffectType.Tremors:
                                                                                            case BattleEffectType.Geyser:
                                                                                            case BattleEffectType.GeyserWater:
                                                                                            case BattleEffectType.HailImpact:
                                                                                            case BattleEffectType.Cloud:
                                                                                            case BattleEffectType.CloudPuff:
                                                                                            case BattleEffectType.HitGround:
                                                                                            case BattleEffectType.HitWater:
                                                                                            case BattleEffectType.HighPrayer:
                                                                                            case BattleEffectType.PrayerGlow:
                                                                                            case BattleEffectType.Resurrect:
                                                                                            case BattleEffectType.FrostBeam:
                                                                                            case BattleEffectType.Frost:
                                                                                            case BattleEffectType.MindDecay:
                                                                                            case BattleEffectType.DecayLeg:
                                                                                            case BattleEffectType.ShockWave:
                                                                                            case BattleEffectType.DecayComet:
                                                                                            case BattleEffectType.SacFlame:
                                                                                            case BattleEffectType.Winds:
                                                                                            case BattleEffectType.SacFire:
                                                                                            case BattleEffectType.BreathDam:
                                                                                            default:
                                                                                                break;
                                                                                        }

                                                                                        break;
                                                                                    default:
                                                                                        break;
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

                            effectIndex += 1;
                        }

                        if (CombatMap.Current.Effects.Count == 0 && (_strikeIndex >= _strikeCount || !_unit.GetEnabled()))
                        {
                            if (_unit.GetEnabled())
                            {
                                _unit.UnitHexSprite.SetAction(TacticalBattleUnitHexSprite.SpriteAction.Move);
                            }

                            result = ExecuteResult.Done;
                        }

                        if (_unit.GetEnabled())
                        {
                            if (_effectType == BattleEffectType.Breath)
                            {
                                NextStrike();
                            }
                            else
                            {
                                if (_unit.UnitHexSprite.ActionDone || (AbilityId == 52 && _delay <= 0))
                                {
                                    _delay = StrategicMap.Current.Random(3) + 4;
                                }

                                _delay -= 1;
                                if (_delay == 0)
                                {
                                    NextStrike();
                                }
                            }
                        }
                    }
                    catch
                    {
                        _ = MessageBox(0, text: "Error during Token Execute", caption: $"Error - TCRangedToken {AbilityId.ToString(CultureInfo.InvariantCulture)}/{SpellId.ToString(CultureInfo.InvariantCulture)}", flags: 0);
                    }

                    break;
                default:
                    break;
            }

            if (result == ExecuteResult.Done)
            {
                (_sound as IDisposable)?.Dispose();

                if (_unit is not null && _unit.GetEnabled())
                {
                    _unit.UnitHexSprite.SelectedAbility = -1;
                    _unit.UnitHexSprite.SelectedSpell = -1;
                    _unit.UnitHexSprite.Unselect();
                    if (_unit.CombatPlayer == CombatMap.Current.CurrentCombatPlayer)
                    {
                        sbyte hexX = _unit.UnitHexSprite.HexX;
                        CombatMap.Current.Players.GetPlayers(CombatMap.Current.CurrentCombatPlayer).LastX = hexX;
                        sbyte hexY = _unit.UnitHexSprite.HexY;
                        CombatMap.Current.Players.GetPlayers(CombatMap.Current.CurrentCombatPlayer).LastY = hexY;
                    }
                }
            }
        }
        catch
        {
            _ = MessageBox(0, text: "Error during Token", caption: $"Error - TCRangedToken {AbilityId.ToString(CultureInfo.InvariantCulture)}/{SpellId.ToString(CultureInfo.InvariantCulture)}", flags: 0);
        }

        return result;
    }

    // CombatTE.pas:1373
    public void NextStrike()
    {
        DelphiList<TacticalBattleUnit> used;
        int deltaX = default;
        int deltaY = default;
        int j = default;
        int k = default;
        TacticalBattleUnit FindTarget(int X, int Y)
        {
            TacticalBattleUnit result = null;
            int bestDistance = 6;
            for (int i2 = 0, last = CombatMap.Current.Combat.CombatData.GetCount() - 1; i2 <= last; i2++)
            {
                if (CombatMap.Current.Combat.CombatData.GetObjects(i2) is CombatUnit && CombatMap.Current.Combat.CombatData.GetObjects(i2).GetEnabled() && CombatMap.Current.Combat.CombatData.GetObjects(i2).Side != _unit.Side)
                {
                    bool available = true;
                    for (int j2 = 0, lastJ = used.Count - 1; j2 <= lastJ; j2++)
                    {
                        if (CombatMap.Current.Combat.CombatData.GetObjects(i2) == used[j2])
                        {
                            available = false;
                        }
                    }

                    if (available)
                    {
                        sbyte hexY = CombatMap.Current.Combat.CombatData.GetObjects<TacticalBattleUnit>(i2).UnitHexSprite.HexY;
                        int distance = HexGeometry.DeltaHexToRadius(X, Y, CombatMap.Current.Combat.CombatData.GetObjects<TacticalBattleUnit>(i2).UnitHexSprite.HexX, hexY);
                        if (distance < bestDistance)
                        {
                            result = CombatMap.Current.Combat.CombatData.GetObjects<TacticalBattleUnit>(i2);
                            bestDistance = distance;
                        }
                    }
                }
            }

            return result;
        }

        TacticalBattleUnit target;
        void ChooseTarget()
        {
            used = new DelphiList<TacticalBattleUnit>();
            List<int> weights = [];
            int total = 0;
            int i2 = 0;
            int last = CombatMap.Current.Combat.CombatData.GetCount() - 1;
            for (; i2 <= last; i2 += 1)
            {
                if (CombatMap.Current.Combat.CombatData.GetObjects(i2) is CombatUnit && CombatMap.Current.Combat.CombatData.GetObjects(i2).GetEnabled() && CombatMap.Current.Combat.CombatData.GetObjects(i2) != target)
                {
                    sbyte hexY = CombatMap.Current.Combat.CombatData.GetObjects<TacticalBattleUnit>(i2).UnitHexSprite.HexY;
                    sbyte hexX = CombatMap.Current.Combat.CombatData.GetObjects<TacticalBattleUnit>(i2).UnitHexSprite.HexX;
                    int distance = HexGeometry.DeltaHexToRadius(TargetX, TargetY, hexX, hexY);
                    if (distance < 13)
                    {
                        total += 13 - distance;
                        _ = used.Add(CombatMap.Current.Combat.CombatData.GetObjects<TacticalBattleUnit>(i2));
                        weights.Add(13 - distance);
                    }
                }
            }

            if (target is not null)
            {
                _ = used.Add(target);
                weights.Add(total);
                total += total;
            }

            if (used.Count > 0)
            {
                i2 = StrategicMap.Current.Random(total) - weights[0];
                int index = 0;
                while (index < used.Count - 1 && i2 > 0)
                {
                    index++;
                    i2 -= weights[index];
                }

                target = used[index];
            }

            used?.Dispose();
        }

        if (_strikeIndex >= _strikeCount)
        {
            return;
        }

        BattleEffectSprite effect = CombatMap.Current.AddEffect(BattleEffectType.None);
        DelphiList<RangedObstacle> obstacles = new();
        int x1;
        int y1;
        int x2;
        int y2;
        TacticalBattleUnitHexSprite hexSprite;
        int i;
        Point3D point;
        switch (_effectType)
        {
            case BattleEffectType.Breath:
                sbyte hexY = _unit.UnitHexSprite.HexY;
                int deltaHexToHexNumber2 = HexGeometry.DeltaHexToHexNumber(_unit.UnitHexSprite.HexX, hexY, TargetX, TargetY);
                sbyte fUnitHexSpriteHexY = _unit.UnitHexSprite.HexY;
                HexGeometry.CenterHexNumberToHex(_unit.UnitHexSprite.HexX, fUnitHexSpriteHexY, hexNumber: TacticalTables.BreathHexNumber[(TacticalTables.BreathDirection[deltaHexToHexNumber2 - 1] * 12) - 3], ref j, ref k);
                sbyte hexY2 = _unit.UnitHexSprite.HexY;
                HexGeometry.HexToPixel(_unit.UnitHexSprite.HexX, hexY2, out x1, out y1);
                HexGeometry.HexToPixel(j, k, out x2, out y2);
                float sweep = _strikeIndex;
                if ((double)sweep > 9)
                {
                    sweep = (float)((double)19 - sweep);
                }

                float a = y2 == y1 ? 0 : x2 == x1 ? (float)(Math.PI / 2) : (float)Math.Atan((double)Math.Abs(x2 - x1) / Math.Abs(y2 - y1));

                float angle;
                if (y2 == y1 || x2 == x1)
                {
                    double pi = Math.PI;
                    double mathPI = Math.PI;
                    angle = (float)((double)((double)((double)pi / 2) - a) + ((double)((double)((double)((double)sweep * 6) - 30) * mathPI) / 180));
                }
                else
                {
                    angle = (float)((double)a + ((double)((double)((double)((double)sweep * 6) - 30) * Math.PI) / 180));
                }

                float radius = (float)Math.Sqrt(((x2 - x1) * (x2 - x1)) + ((y2 - y1) * (y2 - y1)));
                deltaX = x2 >= x1 ? Round((double)Math.Sin(angle) * radius) - x2 + x1 : x1 - x2 - Round((double)Math.Sin(angle) * radius);

                deltaY = y2 >= y1 ? Round((double)Math.Cos(angle) * radius) - y2 + y1 : y1 - y2 - Round((double)Math.Cos(angle) * radius);

                sbyte hexX = _unit.UnitHexSprite.HexX;
                sbyte hexY3 = _unit.UnitHexSprite.HexY;
                Point3DList path19 = effect.Path;
                TacticalTables.MakeRangedPath(_unit.Side, speed: 50, arc: 20, unusedHeight: 50, hexX, hexY3, (short)j, (short)k, (short)deltaX, (short)deltaY, includeUnits: false, path: path19, obstacles);
                effect.Path = path19;
                if (_strikeIndex < 12)
                {
                    sbyte hexY4 = _unit.UnitHexSprite.HexY;
                    int deltaHexToHexNumber3 = HexGeometry.DeltaHexToHexNumber(_unit.UnitHexSprite.HexX, hexY4, TargetX, TargetY);
                    sbyte hexY5 = _unit.UnitHexSprite.HexY;
                    int targetX = effect.TargetX;
                    int targetY = effect.TargetY;
                    HexGeometry.CenterHexNumberToHex(_unit.UnitHexSprite.HexX, hexY5, hexNumber: TacticalTables.BreathHexNumber[(TacticalTables.BreathDirection[deltaHexToHexNumber3 - 1] * 12) + BreathHit[_strikeIndex] - 12], ref targetX, ref targetY);
                    effect.TargetX = targetX;
                    effect.TargetY = targetY;
                }

                break;
            case BattleEffectType.Touch:
            case BattleEffectType.Effect:
            case BattleEffectType.SacredWrath:
            case BattleEffectType.Geyser:
            case BattleEffectType.Cloud:
            case BattleEffectType.MindDecay:
            case BattleEffectType.ShockWave:
            case BattleEffectType.SacFlame:
                if (_effectType == BattleEffectType.SacredWrath)
                {
                    TargetX = 32;
                    TargetY = 29;
                }

                HexGeometry.HexToPixel(TargetX, TargetY, out x2, out y2);
                switch (_effectType)
                {
                    case BattleEffectType.Geyser:
                        CombatMap.Current.CenterViewTo(TargetX, TargetY - 5, 0);
                        break;
                    case BattleEffectType.MindDecay:
                        effect.TargetX = TargetX;
                        effect.TargetY = TargetY;
                        CombatMap.Current.CenterViewTo(TargetX + 1, TargetY - 4, 0);
                        break;
                    case BattleEffectType.ShockWave:
                        sbyte hexY4 = _unit.UnitHexSprite.HexY;
                        sbyte fUnitHexSpriteHexX = _unit.UnitHexSprite.HexX;
                        CombatMap.Current.CenterViewTo(fUnitHexSpriteHexX, hexY4, 0);
                        break;
                    case BattleEffectType.SacFlame:
                        sbyte hexY5 = _unit.UnitHexSprite.HexY;
                        sbyte hexX2 = _unit.UnitHexSprite.HexX;
                        CombatMap.Current.CenterViewTo(hexX2, hexY5 - 3, 0);
                        break;
                    case BattleEffectType.None:
                    case BattleEffectType.Arrow:
                    case BattleEffectType.Rock:
                    case BattleEffectType.Comet:
                    case BattleEffectType.Touch:
                    case BattleEffectType.Breath:
                    case BattleEffectType.CometBall:
                    case BattleEffectType.Effect:
                    case BattleEffectType.FBExplosion:
                    case BattleEffectType.ScreenNESW:
                    case BattleEffectType.ScreenNS:
                    case BattleEffectType.Ray:
                    case BattleEffectType.Chain:
                    case BattleEffectType.Swarm:
                    case BattleEffectType.ScreenSwarm:
                    case BattleEffectType.SFOverlay:
                    case BattleEffectType.DamLightning:
                    case BattleEffectType.DamFire:
                    case BattleEffectType.DamPoison:
                    case BattleEffectType.DamMagic:
                    case BattleEffectType.DamHoly:
                    case BattleEffectType.DamDeath:
                    case BattleEffectType.DamCold:
                    case BattleEffectType.SacredWrath:
                    case BattleEffectType.SWComet:
                    case BattleEffectType.Dispel:
                    case BattleEffectType.WallSmoke:
                    case BattleEffectType.Blood:
                    case BattleEffectType.AltArrow:
                    case BattleEffectType.Tremors:
                    case BattleEffectType.GeyserWater:
                    case BattleEffectType.HailImpact:
                    case BattleEffectType.Cloud:
                    case BattleEffectType.CloudPuff:
                    case BattleEffectType.HitGround:
                    case BattleEffectType.HitWater:
                    case BattleEffectType.HighPrayer:
                    case BattleEffectType.PrayerGlow:
                    case BattleEffectType.Resurrect:
                    case BattleEffectType.FrostBeam:
                    case BattleEffectType.Frost:
                    case BattleEffectType.DecayLeg:
                    case BattleEffectType.DecayComet:
                    case BattleEffectType.Winds:
                    case BattleEffectType.SacFire:
                    case BattleEffectType.BreathDam:
                    default:
                        CombatMap.Current.CenterViewTo(TargetX, TargetY, 0);
                        break;
                }

                point = new Point3D
                {
                    X = x2 + 16,
                    Y = y2 + 16
                };
                effect.Path.Add(point);
                hexSprite = CombatMap.Current.MapContainer.MapLevels[0].GetField(TargetX, TargetY).FindOwnedChild<TacticalBattleUnitHexSprite>(TacticalBattleUnitHexSprite.ClassId);
                if (hexSprite is not null)
                {
                    effect.Target = hexSprite.Unit;
                }

                break;
            case BattleEffectType.Winds:
                HexGeometry.HexToPixel(TargetX, TargetY, out x2, out y2);
                point = new Point3D
                {
                    X = x2 + 16,
                    Y = y2 + 16
                };
                effect.Path.Add(point);
                hexSprite = CombatMap.Current.MapContainer.MapLevels[0].GetField(TargetX, TargetY).FindOwnedChild<TacticalBattleUnitHexSprite>(TacticalBattleUnitHexSprite.ClassId);
                if (hexSprite is not null)
                {
                    effect.Target = hexSprite.Unit;
                }

                i = StrategicMap.Current.Random(37) + 1;
                bool found = false;
                while (!found)
                {
                    HexGeometry.CenterHexNumberToHex(TargetX, TargetY, hexNumber: i, ref j, ref k);
                    bool condition = j is > 5 and < 60 && k is > 5 and < 53 && CombatMap.Current.MapContainer.MapLevels[0].GetField(j, k).FindOwnedChild(TacticalBattleUnitHexSprite.ClassId) is null;
                    if (condition)
                    {
                        bool test = CombatMap.Current.MapContainer.MapLevels[0].GetField(j, k).OverlayType != 8;
                        if (!test)
                        {
                            MapField fieldValue = CombatMap.Current.MapContainer.MapLevels[0].GetField(j, k);
                            MapField fieldValue2 = CombatMap.Current.MapContainer.MapLevels[0].GetField(TargetX, TargetY);
                            test = fieldValue.OverlayType == fieldValue2.OverlayType;
                        }

                        condition = test;
                    }

                    bool passed = condition && CombatMap.Current.MapContainer.MapLevels[0].GetField(j, k).TerrainType != 15 && CombatMap.Current.MapContainer.MapLevels[0].GetField(j, k).TerrainType != 7;
                    if (passed)
                    {
                        bool test = CombatMap.Current.MapContainer.MapLevels[0].GetField(j, k).TerrainType is not 0 and not 10 and not 9;
                        if (!test)
                        {
                            MapField fieldValue = CombatMap.Current.MapContainer.MapLevels[0].GetField(j, k);
                            MapField fieldValue2 = CombatMap.Current.MapContainer.MapLevels[0].GetField(TargetX, TargetY);
                            test = fieldValue.TerrainType == fieldValue2.TerrainType;
                        }

                        passed = test;
                    }

                    if (passed)
                    {
                        found = true;
                    }
                    else
                    {
                        i += 1;
                    }
                }

                HexGeometry.HexToPixel(j, k, out deltaX, out deltaY);
                CombatMap.Current.CenterViewTo(TargetX, TargetY, 0);
                {
                    i = 0;
                    for (; i <= 29; i += 1)
                    {
                        Point3D xyz = new();
                        effect.Path.Add(xyz);
                        Point3D pathX = effect.Path[0];
                        int random = Random(30);
                        int count = effect.Path.Count;
                        Point3DList path20 = effect.Path;
                        path20[count - 1].X = pathX.X + random - 15;
                        effect.Path = path20;
                        Point3D pathY = effect.Path[0];
                        int random2 = Random(30);
                        int fPathCount = effect.Path.Count;
                        Point3DList path21 = effect.Path;
                        path21[fPathCount - 1].Y = pathY.Y + random2 - 15;
                        effect.Path = path21;
                    }
                }

                j = Random(300);
                k = Random(300);
                int count2 = effect.Path.Count;
                Point3D path = effect.Path[count2 - 1];
                int count3 = effect.Path.Count;
                Point3D pathZ = effect.Path[count3 - 1];
                int count4 = effect.Path.Count;
                Point3D path2 = effect.Path[count4 - 1];
                int count5 = effect.Path.Count;
                Point3D path3 = effect.Path[count5 - 1];
                int count6 = effect.Path.Count;
                Point3D path4 = effect.Path[count6 - 1];
                int count7 = effect.Path.Count;
                Point3DList path22 = effect.Path;
                EffectPaths.MakeArcPathXY((short)effect.Path[count7 - 1].X, (short)(path2.Y - path3.Z), (short)(path4.X + j - 150), (short)(path.Y - pathZ.Z + k - 150), stepSize: (short)(((Math.Abs(j - 150) + Math.Abs(k - 150)) / 50) + 2), height: 100, list: path22);
                effect.Path = path22;
                effect.TargetX = effect.Path.Count;
                j = Random(200);
                k = Random(200);
                int count8 = effect.Path.Count;
                Point3D path5 = effect.Path[count8 - 1];
                int count9 = effect.Path.Count;
                Point3D path6 = effect.Path[count9 - 1];
                int count10 = effect.Path.Count;
                Point3D path7 = effect.Path[count10 - 1];
                int count11 = effect.Path.Count;
                Point3D path8 = effect.Path[count11 - 1];
                int count12 = effect.Path.Count;
                Point3D path9 = effect.Path[count12 - 1];
                int count13 = effect.Path.Count;
                Point3DList path23 = effect.Path;
                EffectPaths.MakeArcPathXY((short)effect.Path[count13 - 1].X, (short)(path7.Y - path8.Z), (short)(path9.X + j - 100), (short)(path5.Y - path6.Z + k - 100), stepSize: (short)(((Math.Abs(j - 100) + Math.Abs(k - 100)) / 50) + 3), height: 100, list: path23);
                effect.Path = path23;
                effect.TargetY = effect.Path.Count;
                int count14 = effect.Path.Count;
                Point3D item = effect.Path[count14 - 1];
                int count15 = effect.Path.Count;
                Point3D fPathItem = effect.Path[count15 - 1];
                int count16 = effect.Path.Count;
                Point3D path10 = effect.Path[count16 - 1];
                int count17 = effect.Path.Count;
                Point3D path11 = effect.Path[count17 - 1];
                int count18 = effect.Path.Count;
                Point3D path12 = effect.Path[count18 - 1];
                int count19 = effect.Path.Count;
                Point3DList path24 = effect.Path;
                EffectPaths.MakeArcPathXY((short)effect.Path[count19 - 1].X, (short)(path11.Y - path12.Z), (short)(deltaX + 16), (short)(deltaY + 16), stepSize: (short)(((Math.Abs(item.Y - fPathItem.Z - deltaY) + Math.Abs(path10.X - deltaX)) / 50) + 4), height: 100, list: path24);
                effect.Path = path24;
                break;
            case BattleEffectType.ScreenNESW:
                point = new Point3D
                {
                    X = Random(100) < 50 ? 0 : 1
                };

                effect.Path.Add(point);
                if (_strikeIndex == 9)
                {
                    effect.Target = _unit;
                }

                break;
            case BattleEffectType.ScreenNS:
                i = SpellId switch
                {
                    124 => 19,
                    _ => 7,
                };
                HexGeometry.CenterHexNumberToHex(TargetX, TargetY, hexNumber: RandomHex[_strikeIndex % i], ref deltaX, ref deltaY);
                HexGeometry.HexToPixel(deltaX, deltaY, out x2, out y2);
                CombatMap.Current.MakeVisible(deltaX, deltaY, 0, radius: 5);
                x1 = Random(20) + 6;
                y1 = Random(20) + 6;
                Point3DList path25 = effect.Path;
                EffectPaths.MakeLinePath(x2 + x1 - 100, y2 + y1 - 300, x2 + x1, y2 + y1, steps: 30, list: path25);
                effect.Path = path25;
                switch (SpellId)
                {
                    case 124:
                        effect.Target = null;
                        break;
                    default:
                        {
                            hexSprite = CombatMap.Current.MapContainer.MapLevels[0].GetField(deltaX, deltaY).FindOwnedChild<TacticalBattleUnitHexSprite>(TacticalBattleUnitHexSprite.ClassId);
                            if (hexSprite is not null)
                            {
                                effect.Target = hexSprite.Unit;
                            }
                        }

                        break;
                }

                break;
            case BattleEffectType.Chain:
                sbyte hexY6 = _unit.UnitHexSprite.HexY;
                HexGeometry.HexToPixel(_unit.UnitHexSprite.HexX, hexY6, out x1, out y1);
                HexGeometry.HexToPixel(TargetX, TargetY, out x2, out y2);
                Point3DList path26 = effect.Path;
                EffectPaths.MakeLightningPath(x1 + 16, y1, x2 + 16, y2, list: path26);
                effect.Path = path26;
                int count20 = effect.Path.Count;
                Point3DList path27 = effect.Path;
                path27[count20 - 1].Z = 16;
                effect.Path = path27;
                i = 3;
                used = new DelphiList<TacticalBattleUnit>();
                _ = used.Add(_unit);
                hexSprite = CombatMap.Current.MapContainer.MapLevels[0].GetField(TargetX, TargetY).FindOwnedChild<TacticalBattleUnitHexSprite>(TacticalBattleUnitHexSprite.ClassId);
                if (hexSprite is not null)
                {
                    _ = used.Add(hexSprite.Unit);
                }

                target = FindTarget(TargetX, TargetY);
                while (target is not null && i > 0)
                {
                    _ = used.Add(target);
                    x1 = x2 + 16;
                    y1 = y2 + 16;
                    sbyte hexY4 = target.UnitHexSprite.HexY;
                    HexGeometry.HexToPixel(target.UnitHexSprite.HexX, hexY4, out x2, out y2);
                    Point3DList path28 = effect.Path;
                    EffectPaths.MakeLightningPath(x1, y1, x2 + 16, y2, list: path28);
                    effect.Path = path28;
                    i -= 1;
                    sbyte hexY5 = target.UnitHexSprite.HexY;
                    target = FindTarget(target.UnitHexSprite.HexX, hexY5);
                }

                used?.Dispose();
                break;
            case BattleEffectType.Swarm:
                sbyte hexY7 = _unit.UnitHexSprite.HexY;
                HexGeometry.HexToPixel(_unit.UnitHexSprite.HexX, hexY7, out x1, out y1);
                int random3 = Random(360);
                int random4 = Random(20);
                int random5 = Random(20);
                int random6 = Random(2);
                int random7 = Random(90);
                int random8 = Random(5);
                Point3DList path29 = effect.Path;
                EffectPaths.MakeSwarmPath(Random(10) + 10, radius: random8 + 20, phase: random7, angle: random3, random4 + x1 + 6, random5 + y1 - 10, direction: (SwarmDirection)random6, list: path29);
                effect.Path = path29;
                k = effect.Path.Count - 1;
                {
                    i = 0;
                    for (; i <= 2; i += 1)
                    {
                        j = 0;
                        int last = k;
                        for (; j <= last; j += 1)
                        {
                            point = new Point3D
                            {
                                X = effect.Path[j].X,
                                Y = effect.Path[j].Y
                            };
                            effect.Path.Add(point);
                        }
                    }
                }

                HexGeometry.HexToPixel(TargetX, TargetY, out x2, out y2);
                int count21 = effect.Path.Count;
                Point3D path13 = effect.Path[count21 - 1];
                int count22 = effect.Path.Count;
                Point3DList path30 = effect.Path;
                EffectPaths.MakeLinePath(effect.Path[count22 - 1].X, path13.Y, x2 + 16, y2, steps: 15, list: path30);
                effect.Path = path30;
                Point3DList point3DList = new();
                int random9 = Random(360);
                int count23 = effect.Path.Count;
                Point3D path14 = effect.Path[count23 - 1];
                int count24 = effect.Path.Count;
                Point3D path15 = effect.Path[count24 - 1];
                int random10 = Random(2);
                int random11 = Random(90);
                int random12 = Random(5);
                EffectPaths.MakeSwarmPath(Random(10) + 10, radius: random12 + 20, phase: random11, angle: random9, path14.X, path15.Y, direction: (SwarmDirection)random10, list: point3DList);
                Point3D pathItem = point3DList[0];
                int count25 = effect.Path.Count;
                Point3D path16 = effect.Path[count25 - 1];
                int count26 = effect.Path.Count;
                Point3D path17 = effect.Path[count26 - 1];
                Point3D pathX2 = point3DList[0];
                Point3DList path31 = effect.Path;
                EffectPaths.MakeLinePath(path17.X, path16.Y, pathX2.X, pathItem.Y, steps: 5, list: path31);
                effect.Path = path31;
                {
                    i = 0;
                    int last = point3DList.Count - 1;
                    for (; i <= last; i += 1)
                    {
                        // { Original shares the points, then frees Path. }
                        Point3D path18 = point3DList[i];
                        effect.Path.Add(path18);
                    }
                }

                k = point3DList.Count - 1;
                {
                    i = 0;
                    for (; i <= 2; i += 1)
                    {
                        j = 0;
                        int last = k;
                        for (; j <= last; j += 1)
                        {
                            point = new Point3D
                            {
                                X = point3DList[j].X,
                                Y = point3DList[j].Y
                            };
                            effect.Path.Add(point);
                        }
                    }
                }

                point3DList?.Dispose();
                break;
            case BattleEffectType.ScreenSwarm:
                hexSprite = CombatMap.Current.MapContainer.MapLevels[0].GetField(TargetX, TargetY).FindOwnedChild<TacticalBattleUnitHexSprite>(TacticalBattleUnitHexSprite.ClassId);
                target = hexSprite?.Unit;

                deltaX = TargetX;
                deltaY = TargetY;
                ChooseTarget();
                if (target is not null)
                {
                    deltaX = target.UnitHexSprite.HexX;
                    deltaY = target.UnitHexSprite.HexY;
                }

                effect.Target = target;
                HexGeometry.HexToPixel(deltaX, deltaY, out x2, out y2);
                if (_strikeIndex == 0)
                {
                    CombatMap.Current.CenterViewTo(TargetX, TargetY, 0);
                }

                point = new Point3D
                {
                    X = x2,
                    Y = y2
                };
                effect.Path.Add(point);
                int random13 = Random(360);
                int random14 = Random(20);
                int random15 = Random(20);
                int random16 = Random(2);
                int random17 = Random(90);
                int random18 = Random(10);
                Point3DList path32 = effect.Path;
                EffectPaths.MakeSwarmPath(Random(10) + 10, radius: random18 + 25, phase: random17, angle: random13, random14, random15 - 10, direction: (SwarmDirection)random16, list: path32);
                effect.Path = path32;
                k = effect.Path.Count - 1;
                {
                    i = 0;
                    for (; i <= 4; i += 1)
                    {
                        j = 1;
                        int last = k;
                        for (; j <= last; j += 1)
                        {
                            point = new Point3D
                            {
                                X = effect.Path[j].X,
                                Y = effect.Path[j].Y
                            };
                            effect.Path.Add(point);
                        }
                    }
                }

                {
                    i = 0;
                    for (; i <= 49; i += 1)
                    {
                        Point3DList path33 = effect.Path;
                        path33[i + 1].Z = 100 - (i * 2);
                        effect.Path = path33;
                    }
                }

                break;
            case BattleEffectType.FrostBeam:
                HexGeometry.HexToPixel(TargetX, TargetY, out x2, out y2);
                effect.TargetX = TargetX;
                effect.TargetY = TargetY;
                CombatMap.Current.CenterViewTo(TargetX - 3, TargetY - 3, 0);
                Point3DList path34 = effect.Path;
                EffectPaths.MakeArcPathXY((short)(x2 - 200), (short)(y2 - 200), (short)(x2 + 16), (short)(y2 + 16), stepSize: 5, height: 100, list: path34);
                effect.Path = path34;
                break;
            case BattleEffectType.Ray:
                CombatMap.Current.MakeVisible(TargetX - 3, TargetY - 3, 0, radius: 3);
                sbyte hexX3 = _unit.UnitHexSprite.HexX;
                sbyte hexY8 = _unit.UnitHexSprite.HexY;
                Point3DList path35 = effect.Path;
                TacticalTables.MakeRangedPath(_unit.Side, speed: 10, arc: 0, unusedHeight: 50, hexX3, hexY8, (short)TargetX, (short)TargetY, 0, 0, includeUnits: true, path: path35, obstacles);
                effect.Path = path35;
                break;
            case BattleEffectType.None:
            case BattleEffectType.Arrow:
            case BattleEffectType.Rock:
            case BattleEffectType.Comet:
            case BattleEffectType.CometBall:
            case BattleEffectType.FBExplosion:
            case BattleEffectType.SFOverlay:
            case BattleEffectType.DamLightning:
            case BattleEffectType.DamFire:
            case BattleEffectType.DamPoison:
            case BattleEffectType.DamMagic:
            case BattleEffectType.DamHoly:
            case BattleEffectType.DamDeath:
            case BattleEffectType.DamCold:
            case BattleEffectType.SWComet:
            case BattleEffectType.Dispel:
            case BattleEffectType.WallSmoke:
            case BattleEffectType.Blood:
            case BattleEffectType.AltArrow:
            case BattleEffectType.Tremors:
            case BattleEffectType.GeyserWater:
            case BattleEffectType.HailImpact:
            case BattleEffectType.CloudPuff:
            case BattleEffectType.HitGround:
            case BattleEffectType.HitWater:
            case BattleEffectType.HighPrayer:
            case BattleEffectType.PrayerGlow:
            case BattleEffectType.Resurrect:
            case BattleEffectType.Frost:
            case BattleEffectType.DecayLeg:
            case BattleEffectType.DecayComet:
            case BattleEffectType.SacFire:
            case BattleEffectType.BreathDam:
            default:
                sbyte hexX4 = _unit.UnitHexSprite.HexX;
                sbyte hexY9 = _unit.UnitHexSprite.HexY;
                Point3DList path36 = effect.Path;
                TacticalTables.MakeAbilityPath(_unit.Side, ability: (short)AbilityId, spell: (short)SpellId, hexX4, hexY9, (short)TargetX, (short)TargetY, path: path36, obstacles);
                effect.Path = path36;
                break;
        }

        effect.AbilityIndex = AbilityId;
        effect.SpellIndex = SpellId;
        effect.EffectType = _effectType;
        switch (_effectType)
        {
            case BattleEffectType.Arrow:
            case BattleEffectType.Rock:
            case BattleEffectType.Comet:
            case BattleEffectType.Breath:
            case BattleEffectType.CometBall:
            case BattleEffectType.Ray:
            case BattleEffectType.Swarm:
            case BattleEffectType.AltArrow:
                hexSprite = CombatMap.Current.MapContainer.MapLevels[0].GetField(TargetX, TargetY).FindOwnedChild<TacticalBattleUnitHexSprite>(TacticalBattleUnitHexSprite.ClassId);
                if (hexSprite is not null)
                {
                    effect.Target = hexSprite.Unit;
                }

                i = 0;
                while (i < obstacles.Count)
                {
                    RangedObstacle obstacle = obstacles[i];
                    if (StrategicMap.Current.Random(100) < obstacle.Chance)
                    {
                        Point3D path = effect.Path[obstacle.PathIndex];
                        HexGeometry.PixelToHex(effect.Path[obstacle.PathIndex].X, vertical: path.Y, out j, out k);
                        HexSprite hitObject = effect.EffectType == BattleEffectType.Breath
                            ? null
                            : CombatMap.Current.MapContainer.MapLevels[0].GetField(j, k).FindOwnedChild<HexSprite>(TacticalBattleUnitHexSprite.ClassId);

                        hitObject ??= CombatMap.Current.MapContainer.MapLevels[0].GetField(j, k).FindOwnedChild<HexSprite>(World.Hexes.CombatTerrain.ClassId);

                        hitObject ??= CombatMap.Current.MapContainer.MapLevels[0].GetField(j, k).FindOwnedChild<HexSprite>(World.Terrain.CityWall.ClassId);

                        hitObject ??= CombatMap.Current.MapContainer.MapLevels[0].GetField(j, k).FindOwnedChild<HexSprite>(2_229_510);

                        if (hitObject is not null || CombatMap.Current.MapContainer.MapLevels[0].GetField(j, k).TerrainType == 15 || CombatMap.Current.MapContainer.MapLevels[0].GetField(j, k).TerrainType == 7 || CombatMap.Current.MapContainer.MapLevels[0].GetField(j, k).OverlayType == 8)
                        {
                            j = obstacle.PathIndex + 1;
                            int last = effect.Path.Count - 1;
                            for (; j <= last; j += 1)
                            {
                                int count = effect.Path.Count;
                                effect.Path.Delete(count - 1);
                            }

                            if (hitObject is TacticalBattleUnitHexSprite unitHexSprite)
                            {
                                effect.Target = unitHexSprite.Unit;
                            }
                            else
                            {
                                effect.Target = null;
                                if (effect.EffectType == BattleEffectType.Breath)
                                {
                                    effect.TargetX = 0;
                                }
                            }

                            i = obstacles.Count;
                        }
                    }

                    i += 1;
                }

                break;
            case BattleEffectType.None:
            case BattleEffectType.Touch:
            case BattleEffectType.Effect:
            case BattleEffectType.FBExplosion:
            case BattleEffectType.ScreenNESW:
            case BattleEffectType.ScreenNS:
            case BattleEffectType.Chain:
            case BattleEffectType.ScreenSwarm:
            case BattleEffectType.SFOverlay:
            case BattleEffectType.DamLightning:
            case BattleEffectType.DamFire:
            case BattleEffectType.DamPoison:
            case BattleEffectType.DamMagic:
            case BattleEffectType.DamHoly:
            case BattleEffectType.DamDeath:
            case BattleEffectType.DamCold:
            case BattleEffectType.SacredWrath:
            case BattleEffectType.SWComet:
            case BattleEffectType.Dispel:
            case BattleEffectType.WallSmoke:
            case BattleEffectType.Blood:
            case BattleEffectType.Tremors:
            case BattleEffectType.Geyser:
            case BattleEffectType.GeyserWater:
            case BattleEffectType.HailImpact:
            case BattleEffectType.Cloud:
            case BattleEffectType.CloudPuff:
            case BattleEffectType.HitGround:
            case BattleEffectType.HitWater:
            case BattleEffectType.HighPrayer:
            case BattleEffectType.PrayerGlow:
            case BattleEffectType.Resurrect:
            case BattleEffectType.FrostBeam:
            case BattleEffectType.Frost:
            case BattleEffectType.MindDecay:
            case BattleEffectType.DecayLeg:
            case BattleEffectType.ShockWave:
            case BattleEffectType.DecayComet:
            case BattleEffectType.SacFlame:
            case BattleEffectType.Winds:
            case BattleEffectType.SacFire:
            case BattleEffectType.BreathDam:
            default:
                break;
        }

        if (_effectType == BattleEffectType.Breath)
        {
            effect.Target = null;
        }
        else
        {
            switch (AbilityId)
            {
                case 31:
                    effect.Target = null;
                    break;
                case 52:
                    switch (SpellId)
                    {
                        case 104:
                        case 107:
                        case 113:
                        case 116:
                        case 117:
                        case 127:
                        case 129:
                        case 130:
                            effect.Target = null;
                            break;
                        case 102:
                        case 106:
                        case 120:
                        case 128:
                            effect.Target = _unit;
                            break;
                        case 115:
                            if (StrategicMap.Current.Random(100) < 75)
                            {
                                effect.Target = null;
                            }

                            break;
                        case 100:
                            BattleEffectSprite extraEffect = CombatMap.Current.AddEffect(BattleEffectType.SFOverlay);
                            sbyte hexY = _unit.UnitHexSprite.HexY;
                            HexGeometry.HexToPixel(_unit.UnitHexSprite.HexX, hexY, out x1, out y1);
                            while (extraEffect.Path.Count < effect.Path.Count)
                            {
                                point = new Point3D
                                {
                                    X = x1 + 16,
                                    Y = y1
                                };
                                extraEffect.Path.Add(point);
                            }

                            extraEffect.AbilityIndex = AbilityId;
                            extraEffect.SpellIndex = SpellId;
                            break;
                        default:
                            break;
                    }

                    break;
                default:
                    break;
            }
        }

        sbyte hexY10 = _unit.UnitHexSprite.HexY;
        sbyte hexX5 = _unit.UnitHexSprite.HexX;
        effect.Variant = GetRangedDirectionIndex(TargetX - hexX5, TargetY - hexY10);
        if (_strikeIndex == 0)
        {
            switch (_effectType)
            {
                case BattleEffectType.Arrow:
                case BattleEffectType.Rock:
                case BattleEffectType.Comet:
                case BattleEffectType.Breath:
                case BattleEffectType.CometBall:
                case BattleEffectType.Effect:
                case BattleEffectType.Ray:
                case BattleEffectType.Chain:
                case BattleEffectType.Swarm:
                case BattleEffectType.ScreenSwarm:
                case BattleEffectType.SFOverlay:
                case BattleEffectType.Dispel:
                case BattleEffectType.AltArrow:
                case BattleEffectType.Geyser:
                case BattleEffectType.FrostBeam:
                    switch (effect.Variant)
                    {
                        case 0:
                        case 1:
                        case 23:
                            _unit.UnitHexSprite.SetDirection(1);
                            break;
                        case >= 2 and <= 6:
                            _unit.UnitHexSprite.SetDirection(2);
                            break;
                        case >= 7 and <= 10:
                            _unit.UnitHexSprite.SetDirection(3);
                            break;
                        case >= 11 and <= 13:
                            _unit.UnitHexSprite.SetDirection(4);
                            break;
                        case >= 14 and <= 17:
                            _unit.UnitHexSprite.SetDirection(5);
                            break;
                        case >= 18 and <= 22:
                            _unit.UnitHexSprite.SetDirection(6);
                            break;
                        default:
                            break;
                    }

                    break;
                case BattleEffectType.None:
                case BattleEffectType.Touch:
                case BattleEffectType.FBExplosion:
                case BattleEffectType.ScreenNESW:
                case BattleEffectType.ScreenNS:
                case BattleEffectType.DamLightning:
                case BattleEffectType.DamFire:
                case BattleEffectType.DamPoison:
                case BattleEffectType.DamMagic:
                case BattleEffectType.DamHoly:
                case BattleEffectType.DamDeath:
                case BattleEffectType.DamCold:
                case BattleEffectType.SacredWrath:
                case BattleEffectType.SWComet:
                case BattleEffectType.WallSmoke:
                case BattleEffectType.Blood:
                case BattleEffectType.Tremors:
                case BattleEffectType.GeyserWater:
                case BattleEffectType.HailImpact:
                case BattleEffectType.Cloud:
                case BattleEffectType.CloudPuff:
                case BattleEffectType.HitGround:
                case BattleEffectType.HitWater:
                case BattleEffectType.HighPrayer:
                case BattleEffectType.PrayerGlow:
                case BattleEffectType.Resurrect:
                case BattleEffectType.Frost:
                case BattleEffectType.MindDecay:
                case BattleEffectType.DecayLeg:
                case BattleEffectType.ShockWave:
                case BattleEffectType.DecayComet:
                case BattleEffectType.SacFlame:
                case BattleEffectType.Winds:
                case BattleEffectType.SacFire:
                case BattleEffectType.BreathDam:
                default:
                    break;
            }
        }

        if (_effectType is BattleEffectType.Rock or BattleEffectType.Comet or BattleEffectType.Breath or BattleEffectType.CometBall or BattleEffectType.ScreenNS)
        {
            effect.Variant = 0;
        }

        if (_effectType == BattleEffectType.Breath && _strikeIndex == 0)
        {
            _unit.UnitHexSprite.SetAction(TacticalBattleUnitHexSprite.SpriteAction.BreathAttack);
        }
        else
        {
            if ((AbilityId != 52 && TacticalTables.AbilityTypes[AbilityId] != BattleEffectType.Breath) || _strikeIndex == 0)
            {
                _unit.UnitHexSprite.SetAction(TacticalBattleUnitHexSprite.SpriteAction.Ability);
            }
        }

        i = 0;
        int limit = obstacles.Count - 1;
        for (; i <= limit; i += 1)
        {
            (obstacles[i] as IDisposable)?.Dispose();
        }

        obstacles?.Dispose();
        _strikeIndex += 1;
    }

    private static ReadOnlySpan<int> RandomHex => [6, 0, 1, 3, 4, 5, 2, 18, 12, 7, 11, 10, 17, 15, 9, 16, 13, 8, 14];
    private static ReadOnlySpan<int> BreathHit => [0, 1, 4, 7, 8, 2, 5, 9, 10, 3, 6, 11];

    private static ushort GetRangedDirectionIndex(int deltaX, int deltaY)
    {
        ushort result;
        if (deltaX == 0)
        {
            result = 0;
        }
        else
        {
            int ratio = Math.Abs(deltaY * 100 / deltaX);
            result = ratio switch
            {
                >= 0 and <= 12 => 6,
                >= 13 and <= 40 => 5,
                >= 41 and <= 76 => 4,
                >= 77 and <= 129 => 3,
                >= 130 and <= 240 => 2,
                >= 241 and <= 759 => 1,
                _ => 0,
            };
        }

        if (deltaY > 0)
        {
            result = (ushort)(12 - result);
        }

        return deltaX < 0 && result > 0 ? (ushort)(24 - result) : result;
    }
}
