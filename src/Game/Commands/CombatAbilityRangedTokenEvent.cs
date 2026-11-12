// Translated from CombatTE.pas.
using Aow.Engine.Buffers;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Game.Audio;
using Aow.Game.Combat;
using Aow.Game.Combat.Actions;

using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.Net;

using Aow.Game.World.Map;

using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Commands;

public partial class CombatAbilityRangedTokenEvent : TokenEvent
{
    public CombatAbilityRangedTokenEvent(ECustomNode owner) : base(owner)
    { }

    // CombatTE.pas:917
    public new const int ClassId = 2236419;

    private TacticalBattleUnit _unit;
    public int AbilityId;
    public int SpellId;
    public ushort SourceX;
    public ushort SourceY;
    public ushort TargetX;
    public ushort TargetY;
    private BattleEffectType _effectType;
    private int _strikeCount;
    private object _sound;
    private int _delay;
    private int _strikeIndex;
    private int _state;

    // CombatTE.pas:920
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwInteger(ref AbilityId, 30);
        stream.rwInteger(ref SpellId, 31);
        stream.rwWord(ref TargetX, 32);
        stream.rwWord(ref TargetY, 33);
        stream.rwWord(ref SourceX, 34);
        stream.rwWord(ref SourceY, 35);
    }

    // CombatTE.pas:931
    public override ExecuteResult Execute(TokenExecutor tokenEvent)
    {
        int I = default;
        int X = default;
        int Y = default;
        int effectIndex = default;
        BattleEffectSprite effect = default;
        BattleEffectSprite extraEffect = default;
        int endX = default;
        int endY = default;
        SpellCombatInfo info = default;
        TacticalBattleUnitHexSprite hexSprite;
        TacticalBattleOozeHexSprite ooze = default;
        CombatAction action = default;
        MapFieldMessage message = default;
        base.Execute(tokenEvent);
        ExecuteResult result = ExecuteResult.Retry;
        try
        {
            switch (_state)
            {
                case 0:
                    try
                    {
                        hexSprite = (TacticalBattleUnitHexSprite)CombatMap.Current.MapContainer.MapLevels[0].GetField(SourceX, SourceY).FindOwnedChild(2228484);
                        if (hexSprite == null)
                        {
                            result = ExecuteResult.Done;
                        }
                        else
                        {
                            _unit = hexSprite.Unit;
                            _unit.UnitHexSprite.Unselect();
                            _unit.UnitHexSprite.SelectedAbility = AbilityId;
                            _unit.UnitHexSprite.SelectedSpell = SpellId;
                            if (AbilityId == 52)
                            {
                                _effectType = TacticalTables.SpellTypes[SpellId];
                            }
                            else
                            {
                                _effectType = TacticalTables.AbilityTypes[AbilityId];
                            }

                            if (AbilityId == 52)
                            {
                                AoWHSSet.Current.SpellControl.GetSpell(SpellId).CombatSpellCast(_unit);
                            }

                            bool notExplorationEnabled = !CombatMap.Current.ExplorationEnabled;
                            if (!notExplorationEnabled)
                            {
                                var hexY = _unit.UnitHexSprite.HexY;
                                var hexX = _unit.UnitHexSprite.HexX;
                                notExplorationEnabled = ((AoWMapField)CombatMap.Current.MapContainer.GetField(hexX, hexY, 0)).GetExplored(CombatMap.Current.SeatedPlayer);
                            }

                            if (notExplorationEnabled)
                            {
                                var hexY = _unit.UnitHexSprite.HexY;
                                var hexX = _unit.UnitHexSprite.HexX;
                                CombatMap.Current.MakeVisible(hexX, hexY, 0, 2);
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
                                    switch (SpellId)
                                    {
                                        case 116:
                                            _strikeCount = 21;
                                            break;
                                        case 124:
                                            _strikeCount = 19;
                                            break;
                                        default:
                                            _strikeCount = 35;
                                            break;
                                    }

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
                                default:
                                    if (AbilityId == 52)
                                    {
                                        AoWHSSet.Current.SpellControl.GetSpell(SpellId).GetCombatInfo(_unit.Troop, ref info);
                                        _strikeCount = info.RepeatCount;
                                    }
                                    else
                                    {
                                        _strikeCount = ((RangedAttackAbility)AoWHSSet.Current.AbilityControl.GetAbility(AbilityId)).GetAttackRepeatRangedAttack(_unit.Troop);
                                    }

                                    break;
                            }

                            switch (_effectType)
                            {
                                case BattleEffectType.Tremors:
                                    _sound = AoWHSSet.Current.SpellControl.GetSpell(SpellId).CastSounds.Play(0, 64, true, 0, SamplePanning.Center, null);
                                    NextStrike();
                                    break;
                                case BattleEffectType.ScreenSwarm:
                                    _sound = AoWHSSet.Current.SpellControl.GetSpell(SpellId).CastSounds.Play(2, 64, true, 0, SamplePanning.Center, null);
                                    {
                                        I = 0;
                                        int last = _strikeCount - 1;
                                        for (; I <= last; I += 1)
                                        {
                                            NextStrike();
                                        }
                                    }

                                    break;
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
                        MessageBox(0, "Error during Token Start", "Error - TCRangedToken " + IntToStr(AbilityId) + "/" + IntToStr(SpellId), 0);
                    }

                    break;
                case 1:
                    try
                    {
                        effectIndex = 0;
                        while (effectIndex < CombatMap.Current.Effects.Count)
                        {
                            endX = -1;
                            endY = -1;
                            effect = CombatMap.Current.Effects[effectIndex];
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
                                            message.Param[0] = _unit;
                                            message.Result = -1;
                                            var path = effect.Path[0];
                                            HexGeometry.PixelToHex(effect.Path[0].X, path.Y, ref endX, ref endY);
                                            {
                                                I = 0;
                                                for (; I <= 6; I += 1)
                                                {
                                                    HexGeometry.CenterHexNumberToHex(endX, endY, I, ref X, ref Y);
                                                    CombatMap.Current.MapContainer.MapLevels[0].GetField(X, Y).SendMapFieldMessage(ref message);
                                                    EffectPaths.CreateFire(false, 50, X, Y);
                                                }
                                            }

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
                                        var path = effect.Path[effect.Frame];
                                        HexGeometry.PixelToHex(effect.Path[effect.Frame].X, path.Y, ref X, ref Y);
                                        hexSprite = (TacticalBattleUnitHexSprite)CombatMap.Current.MapContainer.MapLevels[0].GetField(X, Y).FindOwnedChild(2228484);
                                        if (hexSprite != null)
                                        {
                                            RandSeed = AoWHSMap.Current.Random(16777215);
                                            effect.CombatAction = AoWHSSet.Current.SpellControl.GetSpell(effect.SpellIndex).CreateCombatAction(_unit, hexSprite.Unit, false);
                                            var hexY = hexSprite.HexY;
                                            HexGeometry.HexToPixel(hexSprite.HexX, hexY, ref I, ref X);
                                            if (((DamageCombatAction)effect.CombatAction).DamageTypes.Value - ((DamageCombatAction)effect.CombatAction).Effects.Value != Set<DamageType>.Of())
                                            {
                                                CombatEvents.MakeDamageEffectHexSpriteList(I, X, ((DamageCombatAction)effect.CombatAction).DamageTypes.Value - ((DamageCombatAction)effect.CombatAction).Effects.Value, false);
                                            }

                                            if (((DamageCombatAction)effect.CombatAction).Effects.Value != Set<DamageType>.Of())
                                            {
                                                CombatEvents.MakeDamageEffectHexSpriteList(I, X, ((DamageCombatAction)effect.CombatAction).Effects.Value, true);
                                            }

                                            CombatMap.Current.Combat.ExecuteCombatAction(effect.CombatAction);
                                            effect.CombatAction.Release();
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
                                                message.Param[0] = _unit;
                                                message.Param[1] = 0;
                                                message.Result = -1;
                                                {
                                                    I = 6;
                                                    for (; I <= 59; I += 1)
                                                    {
                                                        X = 6;
                                                        for (; X <= 52; X += 1)
                                                        {
                                                            CombatMap.Current.MapContainer.MapLevels[0].GetField(I, X).SendMapFieldMessage(ref message);
                                                        }
                                                    }
                                                }

                                                break;
                                        }
                                    }
                                    else
                                    {
                                        if (effect.AbilityIndex == 52 && effect.EffectType == BattleEffectType.Resurrect && (effect.SpellIndex == 130 || effect.SpellIndex == 129))
                                        {
                                            switch (effect.Frame)
                                            {
                                                case 0:
                                                    message.MessageId = 200712;
                                                    message.Param[0] = _unit;
                                                    message.Param[1] = SpellId;
                                                    message.Result = -1;
                                                    {
                                                        I = 0;
                                                        for (; I <= 6; I += 1)
                                                        {
                                                            HexGeometry.CenterHexNumberToHex(TargetX, TargetY, I, ref X, ref Y);
                                                            CombatMap.Current.MapContainer.MapLevels[0].GetField(X, Y).SendMapFieldMessage(ref message);
                                                        }
                                                    }

                                                    break;
                                                case 99:
                                                    message.MessageId = 200713;
                                                    message.Param[0] = _unit;
                                                    message.Param[1] = SpellId;
                                                    message.Result = -1;
                                                    {
                                                        I = 0;
                                                        for (; I <= 6; I += 1)
                                                        {
                                                            HexGeometry.CenterHexNumberToHex(TargetX, TargetY, I, ref X, ref Y);
                                                            CombatMap.Current.MapContainer.MapLevels[0].GetField(X, Y).SendMapFieldMessage(ref message);
                                                        }
                                                    }

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
                                                        message.Param[0] = _unit;
                                                        message.Result = -1;
                                                        {
                                                            I = 0;
                                                            for (; I <= 6; I += 1)
                                                            {
                                                                HexGeometry.CenterHexNumberToHex(TargetX, TargetY, I, ref X, ref Y);
                                                                CombatMap.Current.MapContainer.MapLevels[0].GetField(X, Y).SendMapFieldMessage(ref message);
                                                            }
                                                        }

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
                                                            message.Param[0] = _unit;
                                                            message.Result = -1;
                                                            {
                                                                I = 1;
                                                                for (; I <= 6; I += 1)
                                                                {
                                                                    HexGeometry.CenterHexNumberToHex(TargetX, TargetY, I, ref X, ref Y);
                                                                    CombatMap.Current.MapContainer.MapLevels[0].GetField(X, Y).SendMapFieldMessage(ref message);
                                                                }
                                                            }

                                                            break;
                                                        case 200:
                                                            message.MessageId = 200717;
                                                            message.Param[0] = _unit;
                                                            message.Result = -1;
                                                            {
                                                                I = 7;
                                                                for (; I <= 18; I += 1)
                                                                {
                                                                    HexGeometry.CenterHexNumberToHex(TargetX, TargetY, I, ref X, ref Y);
                                                                    CombatMap.Current.MapContainer.MapLevels[0].GetField(X, Y).SendMapFieldMessage(ref message);
                                                                }
                                                            }

                                                            break;
                                                        case 225:
                                                            message.MessageId = 200717;
                                                            message.Param[0] = _unit;
                                                            message.Result = -1;
                                                            {
                                                                I = 19;
                                                                for (; I <= 36; I += 1)
                                                                {
                                                                    HexGeometry.CenterHexNumberToHex(TargetX, TargetY, I, ref X, ref Y);
                                                                    CombatMap.Current.MapContainer.MapLevels[0].GetField(X, Y).SendMapFieldMessage(ref message);
                                                                }
                                                            }

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
                                                                var hexY = _unit.UnitHexSprite.HexY;
                                                                EffectPaths.CreateFire(false, 50, _unit.UnitHexSprite.HexX, hexY);
                                                                message.MessageId = 200718;
                                                                message.Param[0] = _unit;
                                                                message.Result = -1;
                                                                var fUnitHexSpriteHexY = _unit.UnitHexSprite.HexY;
                                                                var hexX = _unit.UnitHexSprite.HexX;
                                                                CombatMap.Current.MapContainer.MapLevels[0].GetField(hexX, fUnitHexSpriteHexY).SendMapFieldMessage(ref message);
                                                                break;
                                                            case 175:
                                                                message.MessageId = 200718;
                                                                message.Param[0] = _unit;
                                                                message.Result = -1;
                                                                {
                                                                    I = 0;
                                                                    for (; I <= 18; I += 1)
                                                                    {
                                                                        HexGeometry.CenterHexNumberToHex(TargetX, TargetY, I, ref X, ref Y);
                                                                        CombatMap.Current.MapContainer.MapLevels[0].GetField(X, Y).SendMapFieldMessage(ref message);
                                                                        EffectPaths.CreateFire(false, 50, X, Y);
                                                                    }
                                                                }

                                                                break;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (effect.AbilityIndex == 52 && effect.SpellIndex == 105 && effect.Target != null)
                                                        {
                                                            I = 0;
                                                            int last = CombatMap.Current.Combat.CombatData.GetCount() - 1;
                                                            for (; I <= last; I += 1)
                                                            {
                                                                if (CombatMap.Current.Combat.CombatData.GetObjects(I) != null && CombatMap.Current.Combat.CombatData.GetObjects(I) is CombatUnit && CombatMap.Current.Combat.CombatData.GetObjects(I).GetSide() != _unit.GetSide() && CombatMap.Current.Combat.CombatData.GetObjects(I).GetEnabled() && ((TacticalBattleUnit)CombatMap.Current.Combat.CombatData.GetObjects(I)).UnitHexSprite != null)
                                                                {
                                                                    RandSeed = AoWHSMap.Current.Random(16777215);
                                                                    var combatObject = CombatMap.Current.Combat.CombatData.GetObjects(I);
                                                                    action = AoWHSSet.Current.SpellControl.GetSpell(SpellId).CreateCombatAction(_unit, combatObject, false);
                                                                    if (((ExclusiveCombatSpellCombatAction)action).Succeeded)
                                                                    {
                                                                        ((TacticalBattleUnit)CombatMap.Current.Combat.CombatData.GetObjects(I)).UnitHexSprite.SetAction(TacticalBattleUnitHexSprite.SpriteAction.Hit);
                                                                    }

                                                                    CombatMap.Current.Combat.ExecuteCombatAction(action);
                                                                    action.Release();
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
                                                                    case 2999:
                                                                        message.MessageId = 200716;
                                                                        message.Param[0] = _unit;
                                                                        message.Result = -1;
                                                                        {
                                                                            I = 0;
                                                                            for (; I <= 6; I += 1)
                                                                            {
                                                                                HexGeometry.CenterHexNumberToHex(TargetX, TargetY, I, ref X, ref Y);
                                                                                CombatMap.Current.MapContainer.MapLevels[0].GetField(X, Y).SendMapFieldMessage(ref message);
                                                                            }
                                                                        }

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
                                                                                I = 0;
                                                                                int last = CombatMap.Current.Combat.CombatData.GetCount() - 1;
                                                                                for (; I <= last; I += 1)
                                                                                {
                                                                                    if (CombatMap.Current.Combat.CombatData.GetObjects(I) is CombatUnit && CombatMap.Current.Combat.CombatData.GetObjects(I).GetEnabled())
                                                                                    {
                                                                                        hexSprite = ((TacticalBattleUnit)CombatMap.Current.Combat.CombatData.GetObjects(I)).UnitHexSprite;
                                                                                        if (hexSprite != null)
                                                                                        {
                                                                                            extraEffect = CombatMap.Current.AddEffect(BattleEffectType.SWComet);
                                                                                            extraEffect.AbilityIndex = AbilityId;
                                                                                            extraEffect.SpellIndex = SpellId;
                                                                                            extraEffect.Target = hexSprite.Unit;
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }

                                                                            break;
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (effect.Done && effect.Target != null && effect.Target.GetEnabled())
                                                                    {
                                                                        I = 0;
                                                                        RandSeed = AoWHSMap.Current.Random(16777215);
                                                                        if (AbilityId == 52)
                                                                        {
                                                                            effect.CombatAction = AoWHSSet.Current.SpellControl.GetSpell(SpellId).CreateCombatAction(_unit, effect.Target, false);
                                                                        }
                                                                        else
                                                                        {
                                                                            if (AbilityId == 47)
                                                                            {
                                                                                effect.CombatAction = ((HealingAbility)AoWHSSet.Current.AbilityControl.GetAbility(AbilityId)).CreateHealingCombatAction(_unit, effect.Target);
                                                                            }
                                                                            else
                                                                            {
                                                                                if (AbilityId == 60)
                                                                                {
                                                                                    effect.CombatAction = ((DispelMagicAbility)AoWHSSet.Current.AbilityControl.GetAbility(AbilityId)).CreateCombatAction(_unit, effect.Target);
                                                                                }
                                                                                else
                                                                                {
                                                                                    effect.CombatAction = ((RangedAttackAbility)AoWHSSet.Current.AbilityControl.GetAbility(AbilityId)).CreateRangedAttackCombatAction(_unit, effect.Target, 0);
                                                                                }
                                                                            }
                                                                        }

                                                                        if (effect.CombatAction is DamageCombatAction)
                                                                        {
                                                                            I = ((DamageCombatAction)effect.CombatAction).Damage;
                                                                            var hexY = ((TacticalBattleUnit)effect.Target).UnitHexSprite.HexY;
                                                                            HexGeometry.HexToPixel(((TacticalBattleUnit)effect.Target).UnitHexSprite.HexX, hexY, ref X, ref Y);
                                                                            if (((DamageCombatAction)effect.CombatAction).DamageTypes.Value - ((DamageCombatAction)effect.CombatAction).Effects.Value != Set<DamageType>.Of())
                                                                            {
                                                                                CombatEvents.MakeDamageEffectHexSpriteList(X, Y, ((DamageCombatAction)effect.CombatAction).DamageTypes.Value - ((DamageCombatAction)effect.CombatAction).Effects.Value, false);
                                                                            }

                                                                            if (((DamageCombatAction)effect.CombatAction).Effects.Value != Set<DamageType>.Of())
                                                                            {
                                                                                CombatEvents.MakeDamageEffectHexSpriteList(X, Y, ((DamageCombatAction)effect.CombatAction).Effects.Value, true);
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            if (effect.CombatAction is EntangleSpellCombatAction)
                                                                            {
                                                                                if (((ExclusiveCombatSpellCombatAction)effect.CombatAction).Succeeded)
                                                                                {
                                                                                    ((TacticalBattleUnit)effect.Target).UnitHexSprite.SetAction(TacticalBattleUnitHexSprite.SpriteAction.Hit);
                                                                                }
                                                                            }
                                                                            else
                                                                            {
                                                                                if (effect.CombatAction is MindDecayCombatAction && ((ExclusiveCombatSpellCombatAction)effect.CombatAction).Succeeded)
                                                                                {
                                                                                    ((TacticalBattleUnit)effect.Target).UnitHexSprite.SetAction(TacticalBattleUnitHexSprite.SpriteAction.Hit);
                                                                                }
                                                                            }
                                                                        }

                                                                        if (I > 0)
                                                                        {
                                                                            ((TacticalBattleUnit)effect.Target).UnitHexSprite.SetAction(TacticalBattleUnitHexSprite.SpriteAction.Hit);
                                                                            TacticalBattleUnit.MakeHitBlood((TacticalBattleUnit)effect.Target, I);
                                                                            effect.Cancelled = true;
                                                                        }
                                                                    }

                                                                    if (effect.Done)
                                                                    {
                                                                        var count = effect.Path.Count;
                                                                        var path = effect.Path[count - 1];
                                                                        var fPathCount = effect.Path.Count;
                                                                        HexGeometry.PixelToHex(effect.Path[fPathCount - 1].X, path.Y, ref endX, ref endY);
                                                                        if (effect.CombatAction != null && effect.Target != null)
                                                                        {
                                                                            CombatMap.Current.Combat.ExecuteCombatAction(effect.CombatAction);
                                                                            effect.CombatAction.Release();
                                                                            effect.CombatAction = null;
                                                                            if (!effect.Target.GetEnabled())
                                                                            {
                                                                                I = 0;
                                                                                int last = CombatMap.Current.Effects.Count - 1;
                                                                                for (; I <= last; I += 1)
                                                                                {
                                                                                    if (I != effectIndex)
                                                                                    {
                                                                                        if ((CombatMap.Current.Effects[I]).Target == effect.Target)
                                                                                        {
                                                                                            (CombatMap.Current.Effects[I]).Target = null;
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
                                                                                            message.Param[0] = _unit;
                                                                                            message.Param[1] = effect.AbilityIndex;
                                                                                            message.Result = -1;
                                                                                            CombatMap.Current.MapContainer.MapLevels[0].GetField(effect.TargetX, effect.TargetY).SendMapFieldMessage(ref message);
                                                                                        }

                                                                                        break;
                                                                                    case 24:
                                                                                        message.MessageId = 200705;
                                                                                        message.Param[0] = _unit;
                                                                                        message.Result = -1;
                                                                                        CombatMap.Current.MapContainer.MapLevels[0].GetField(endX, endY).SendMapFieldMessage(ref message);
                                                                                        break;
                                                                                    case 44:
                                                                                        message.MessageId = 200706;
                                                                                        message.Param[0] = _unit;
                                                                                        message.Result = -1;
                                                                                        CombatMap.Current.MapContainer.MapLevels[0].GetField(endX, endY).SendMapFieldMessage(ref message);
                                                                                        break;
                                                                                    case 31:
                                                                                        message.MessageId = 200714;
                                                                                        message.Param[0] = _unit;
                                                                                        message.Result = -1;
                                                                                        CombatMap.Current.MapContainer.MapLevels[0].GetField(endX, endY).SendMapFieldMessage(ref message);
                                                                                        EffectPaths.CreateFire(false, 100, endX, endY);
                                                                                        break;
                                                                                    case 52:
                                                                                        switch (effect.EffectType)
                                                                                        {
                                                                                            case BattleEffectType.Breath:
                                                                                                if (effect.TargetX > 0)
                                                                                                {
                                                                                                    message.MessageId = 200723;
                                                                                                    message.Param[0] = _unit;
                                                                                                    message.Param[1] = effect.SpellIndex;
                                                                                                    message.Result = -1;
                                                                                                    CombatMap.Current.MapContainer.MapLevels[0].GetField(effect.TargetX, effect.TargetY).SendMapFieldMessage(ref message);
                                                                                                }

                                                                                                break;
                                                                                            case BattleEffectType.ScreenNS:
                                                                                                switch (effect.SpellIndex)
                                                                                                {
                                                                                                    case 116:
                                                                                                        message.MessageId = 200710;
                                                                                                        message.Param[0] = _unit;
                                                                                                        message.Result = -1;
                                                                                                        CombatMap.Current.MapContainer.MapLevels[0].GetField(endX, endY).SendMapFieldMessage(ref message);
                                                                                                        break;
                                                                                                    case 124:
                                                                                                        if (CombatMap.Current.MapContainer.MapLevels[0].GetField(endX, endY).TerrainType != 0 && CombatMap.Current.MapContainer.MapLevels[0].GetField(endX, endY).TerrainType != 10 && CombatMap.Current.MapContainer.MapLevels[0].GetField(endX, endY).OverlayType != 2 && CombatMap.Current.MapContainer.MapLevels[0].GetField(endX, endY).OverlayType != 11 && CombatMap.Current.MapContainer.MapLevels[0].GetField(endX, endY).OverlayType != 8 && CombatMap.Current.MapContainer.MapLevels[0].GetField(endX, endY).TerrainType != 9 && CombatMap.Current.MapContainer.MapLevels[0].GetField(endX, endY).TerrainType != 15 && CombatMap.Current.MapContainer.MapLevels[0].GetField(endX, endY).TerrainType != 7)
                                                                                                        {
                                                                                                            ooze = new TacticalBattleOozeHexSprite(null);
                                                                                                            CombatMap.Current.PlaceHex(ooze, endX, endY, 0);
                                                                                                        }

                                                                                                        break;
                                                                                                }

                                                                                                break;
                                                                                            case BattleEffectType.Effect:
                                                                                                switch (effect.SpellIndex)
                                                                                                {
                                                                                                    case 113:
                                                                                                        message.MessageId = 200715;
                                                                                                        message.Param[0] = _unit;
                                                                                                        message.Result = -1;
                                                                                                        CombatMap.Current.MapContainer.MapLevels[0].GetField(endX, endY).SendMapFieldMessage(ref message);
                                                                                                        EffectPaths.CreateFire(false, 100, endX, endY);
                                                                                                        break;
                                                                                                }

                                                                                                break;
                                                                                        }

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
                                    _delay = AoWHSMap.Current.Random(3) + 4;
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
                        MessageBox(0, "Error during Token Execute", "Error - TCRangedToken " + IntToStr(AbilityId) + "/" + IntToStr(SpellId), 0);
                    }

                    break;
            }

            if (result == ExecuteResult.Done)
            {
                if (_sound != null)
                {
                    (_sound as IDisposable)?.Dispose();
                }

                if (_unit != null && _unit.GetEnabled())
                {
                    _unit.UnitHexSprite.SelectedAbility = -1;
                    _unit.UnitHexSprite.SelectedSpell = -1;
                    _unit.UnitHexSprite.Unselect();
                    if (_unit.CombatPlayer == CombatMap.Current.CurrentCombatPlayer)
                    {
                        var hexX = _unit.UnitHexSprite.HexX;
                        CombatMap.Current.Players.GetPlayers(CombatMap.Current.CurrentCombatPlayer).LastX = hexX;
                        var hexY = _unit.UnitHexSprite.HexY;
                        CombatMap.Current.Players.GetPlayers(CombatMap.Current.CurrentCombatPlayer).LastY = hexY;
                    }
                }
            }
        }
        catch
        {
            MessageBox(0, "Error during Token", "Error - TCRangedToken " + IntToStr(AbilityId) + "/" + IntToStr(SpellId), 0);
        }

        return result;
    }

    // CombatTE.pas:1373
    public void NextStrike()
    {
        DelphiList<CombatObject> used;
        TacticalBattleUnit target;
        BattleEffectSprite effect;
        BattleEffectSprite extraEffect;
        TacticalBattleUnitHexSprite hexSprite;
        float A;
        float sweep;
        float angle;
        float radius;
        int x1 = default;
        int y1 = default;
        int x2 = default;
        int y2 = default;
        int deltaX = default;
        int deltaY = default;
        int I;
        int J = default;
        int K = default;
        Point3D point;
        HexSprite hitObject;
        Point3DList Path;
        DelphiList<RangedObstacle> obstacles;
        RangedObstacle obstacle;
        bool found;
        TacticalBattleUnit FindTarget(int X, int Y)
        {
            int distance;
            int bestDistance;
            bool available;
            TacticalBattleUnit result = null;
            bestDistance = 6;
            for (int I = 0, last = CombatMap.Current.Combat.CombatData.GetCount() - 1; I <= last; I++)
            {
                if (CombatMap.Current.Combat.CombatData.GetObjects(I) is CombatUnit && CombatMap.Current.Combat.CombatData.GetObjects(I).GetEnabled() && CombatMap.Current.Combat.CombatData.GetObjects(I).GetSide() != _unit.GetSide())
                {
                    available = true;
                    for (int J = 0, lastJ = used.Count - 1; J <= lastJ; J++)
                    {
                        if (CombatMap.Current.Combat.CombatData.GetObjects(I) == used[J])
                        {
                            available = false;
                        }
                    }

                    if (available)
                    {
                        var hexY = ((TacticalBattleUnit)CombatMap.Current.Combat.CombatData.GetObjects(I)).UnitHexSprite.HexY;
                        distance = HexGeometry.DeltaHexToRadius(X, Y, ((TacticalBattleUnit)CombatMap.Current.Combat.CombatData.GetObjects(I)).UnitHexSprite.HexX, hexY);
                        if (distance < bestDistance)
                        {
                            result = (TacticalBattleUnit)CombatMap.Current.Combat.CombatData.GetObjects(I);
                            bestDistance = distance;
                        }
                    }
                }
            }

            return result;
        }

        void ChooseTarget()
        {
            int I;
            int index;
            int total;
            int distance;
            List<int> weights;
            used = new DelphiList<CombatObject>();
            weights = new List<int>();
            total = 0;
            I = 0;
            int last = CombatMap.Current.Combat.CombatData.GetCount() - 1;
            for (; I <= last; I += 1)
            {
                if (CombatMap.Current.Combat.CombatData.GetObjects(I) is CombatUnit && CombatMap.Current.Combat.CombatData.GetObjects(I).GetEnabled() && CombatMap.Current.Combat.CombatData.GetObjects(I) != target)
                {
                    var hexY = ((TacticalBattleUnit)CombatMap.Current.Combat.CombatData.GetObjects(I)).UnitHexSprite.HexY;
                    var hexX = ((TacticalBattleUnit)CombatMap.Current.Combat.CombatData.GetObjects(I)).UnitHexSprite.HexX;
                    distance = HexGeometry.DeltaHexToRadius(TargetX, TargetY, hexX, hexY);
                    if (distance < 13)
                    {
                        total = total + (13 - distance);
                        var combatObject = CombatMap.Current.Combat.CombatData.GetObjects(I);
                        used.Add(combatObject);
                        weights.Add(13 - distance);
                    }
                }
            }

            if (target != null)
            {
                used.Add(target);
                weights.Add(total);
                total = total + total;
            }

            if (used.Count > 0)
            {
                I = AoWHSMap.Current.Random(total) - weights[0];
                index = 0;
                while (index < used.Count - 1 && I > 0)
                {
                    index = index + 1;
                    I = I - weights[index];
                }

                target = (TacticalBattleUnit)used[index];
            }

            used?.Dispose();
        }

        if (_strikeIndex >= _strikeCount)
        {
            return;
        }

        effect = CombatMap.Current.AddEffect(BattleEffectType.None);
        obstacles = new DelphiList<RangedObstacle>();
        switch (_effectType)
        {
            case BattleEffectType.Breath:
                var hexY = _unit.UnitHexSprite.HexY;
                var deltaHexToHexNumber2 = HexGeometry.DeltaHexToHexNumber(_unit.UnitHexSprite.HexX, hexY, TargetX, TargetY);
                var fUnitHexSpriteHexY = _unit.UnitHexSprite.HexY;
                HexGeometry.CenterHexNumberToHex(_unit.UnitHexSprite.HexX, fUnitHexSpriteHexY, TacticalTables.BreathHexNumber[TacticalTables.BreathDirection[deltaHexToHexNumber2 - 1] * 12 - 3], ref J, ref K);
                var hexY2 = _unit.UnitHexSprite.HexY;
                HexGeometry.HexToPixel(_unit.UnitHexSprite.HexX, hexY2, ref x1, ref y1);
                HexGeometry.HexToPixel(J, K, ref x2, ref y2);
                sweep = _strikeIndex;
                if ((double)sweep > 9)
                {
                    sweep = (float)((double)19 - sweep);
                }

                if (y2 == y1)
                {
                    A = 0;
                }
                else
                {
                    if (x2 == x1)
                    {
                        A = (float)((double)Math.PI / 2);
                    }
                    else
                    {
                        A = (float)Math.Atan((double)Math.Abs(x2 - x1) / Math.Abs(y2 - y1));
                    }
                }

                if (y2 == y1 || x2 == x1)
                {
                    var pi = Math.PI;
                    var mathPI = Math.PI;
                    angle = (float)((double)((double)((double)pi / 2) - A) + (double)((double)((double)((double)sweep * 6) - 30) * mathPI) / 180);
                }
                else
                {
                    angle = (float)((double)A + (double)((double)((double)((double)sweep * 6) - 30) * Math.PI) / 180);
                }

                radius = (float)Math.Sqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1));
                if (x2 >= x1)
                {
                    deltaX = Round((double)Math.Sin(angle) * radius) - x2 + x1;
                }
                else
                {
                    deltaX = x1 - x2 - Round((double)Math.Sin(angle) * radius);
                }

                if (y2 >= y1)
                {
                    deltaY = Round((double)Math.Cos(angle) * radius) - y2 + y1;
                }
                else
                {
                    deltaY = y1 - y2 - Round((double)Math.Cos(angle) * radius);
                }

                var hexX = _unit.UnitHexSprite.HexX;
                var hexY3 = _unit.UnitHexSprite.HexY;
                TacticalTables.MakeRangedPath(_unit.GetSide(), 50, 20, 50, hexX, hexY3, (short)J, (short)K, (short)deltaX, (short)deltaY, false, ref effect.Path, ref obstacles);
                if (_strikeIndex < 12)
                {
                    var hexY4 = _unit.UnitHexSprite.HexY;
                    var deltaHexToHexNumber3 = HexGeometry.DeltaHexToHexNumber(_unit.UnitHexSprite.HexX, hexY4, TargetX, TargetY);
                    var hexY5 = _unit.UnitHexSprite.HexY;
                    HexGeometry.CenterHexNumberToHex(_unit.UnitHexSprite.HexX, hexY5, TacticalTables.BreathHexNumber[TacticalTables.BreathDirection[deltaHexToHexNumber3 - 1] * 12 + BreathHit[_strikeIndex] - 12], ref effect.TargetX, ref effect.TargetY);
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

                HexGeometry.HexToPixel(TargetX, TargetY, ref x2, ref y2);
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
                        var hexY4 = _unit.UnitHexSprite.HexY;
                        var fUnitHexSpriteHexX = _unit.UnitHexSprite.HexX;
                        CombatMap.Current.CenterViewTo(fUnitHexSpriteHexX, hexY4, 0);
                        break;
                    case BattleEffectType.SacFlame:
                        var hexY5 = _unit.UnitHexSprite.HexY;
                        var hexX2 = _unit.UnitHexSprite.HexX;
                        CombatMap.Current.CenterViewTo(hexX2, hexY5 - 3, 0);
                        break;
                    default:
                        CombatMap.Current.CenterViewTo(TargetX, TargetY, 0);
                        break;
                }

                point = new Point3D();
                point.X = x2 + 16;
                point.Y = y2 + 16;
                effect.Path.Add(point);
                hexSprite = (TacticalBattleUnitHexSprite)CombatMap.Current.MapContainer.MapLevels[0].GetField(TargetX, TargetY).FindOwnedChild(2228484);
                if (hexSprite != null)
                {
                    effect.Target = hexSprite.Unit;
                }

                break;
            case BattleEffectType.Winds:
                HexGeometry.HexToPixel(TargetX, TargetY, ref x2, ref y2);
                point = new Point3D();
                point.X = x2 + 16;
                point.Y = y2 + 16;
                effect.Path.Add(point);
                hexSprite = (TacticalBattleUnitHexSprite)CombatMap.Current.MapContainer.MapLevels[0].GetField(TargetX, TargetY).FindOwnedChild(2228484);
                if (hexSprite != null)
                {
                    effect.Target = hexSprite.Unit;
                }

                I = AoWHSMap.Current.Random(37) + 1;
                found = false;
                while (!found)
                {
                    HexGeometry.CenterHexNumberToHex(TargetX, TargetY, I, ref J, ref K);
                    bool condition = J > 5 && J < 60 && K > 5 && K < 53 && CombatMap.Current.MapContainer.MapLevels[0].GetField(J, K).FindOwnedChild(2228484) == null;
                    if (condition)
                    {
                        bool test = CombatMap.Current.MapContainer.MapLevels[0].GetField(J, K).OverlayType != 8;
                        if (!test)
                        {
                            var fieldValue = CombatMap.Current.MapContainer.MapLevels[0].GetField(J, K);
                            var fieldValue2 = CombatMap.Current.MapContainer.MapLevels[0].GetField(TargetX, TargetY);
                            test = fieldValue.OverlayType == fieldValue2.OverlayType;
                        }

                        condition = test;
                    }

                    bool passed = condition && CombatMap.Current.MapContainer.MapLevels[0].GetField(J, K).TerrainType != 15 && CombatMap.Current.MapContainer.MapLevels[0].GetField(J, K).TerrainType != 7;
                    if (passed)
                    {
                        bool test = CombatMap.Current.MapContainer.MapLevels[0].GetField(J, K).TerrainType != 0 && CombatMap.Current.MapContainer.MapLevels[0].GetField(J, K).TerrainType != 10 && CombatMap.Current.MapContainer.MapLevels[0].GetField(J, K).TerrainType != 9;
                        if (!test)
                        {
                            var fieldValue = CombatMap.Current.MapContainer.MapLevels[0].GetField(J, K);
                            var fieldValue2 = CombatMap.Current.MapContainer.MapLevels[0].GetField(TargetX, TargetY);
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
                        I += 1;
                    }
                }

                HexGeometry.HexToPixel(J, K, ref deltaX, ref deltaY);
                CombatMap.Current.CenterViewTo(TargetX, TargetY, 0);
                {
                    I = 0;
                    for (; I <= 29; I += 1)
                    {
                        var xyz = new Point3D();
                        effect.Path.Add(xyz);
                        var pathX = effect.Path[0];
                        var random = Random(30);
                        var count = effect.Path.Count;
                        effect.Path[count - 1].X = pathX.X + random - 15;
                        var pathY = effect.Path[0];
                        var random2 = Random(30);
                        var fPathCount = effect.Path.Count;
                        effect.Path[fPathCount - 1].Y = pathY.Y + random2 - 15;
                    }
                }

                J = Random(300);
                K = Random(300);
                var count2 = effect.Path.Count;
                var path = effect.Path[count2 - 1];
                var count3 = effect.Path.Count;
                var pathZ = effect.Path[count3 - 1];
                var count4 = effect.Path.Count;
                var path2 = effect.Path[count4 - 1];
                var count5 = effect.Path.Count;
                var path3 = effect.Path[count5 - 1];
                var count6 = effect.Path.Count;
                var path4 = effect.Path[count6 - 1];
                var count7 = effect.Path.Count;
                EffectPaths.MakeArcPathXY((short)effect.Path[count7 - 1].X, (short)(path2.Y - path3.Z), (short)(path4.X + J - 150), (short)(path.Y - pathZ.Z + K - 150), (short)((Math.Abs(J - 150) + Math.Abs(K - 150)) / 50 + 2), 100, ref effect.Path);
                effect.TargetX = effect.Path.Count;
                J = Random(200);
                K = Random(200);
                var count8 = effect.Path.Count;
                var path5 = effect.Path[count8 - 1];
                var count9 = effect.Path.Count;
                var path6 = effect.Path[count9 - 1];
                var count10 = effect.Path.Count;
                var path7 = effect.Path[count10 - 1];
                var count11 = effect.Path.Count;
                var path8 = effect.Path[count11 - 1];
                var count12 = effect.Path.Count;
                var path9 = effect.Path[count12 - 1];
                var count13 = effect.Path.Count;
                EffectPaths.MakeArcPathXY((short)effect.Path[count13 - 1].X, (short)(path7.Y - path8.Z), (short)(path9.X + J - 100), (short)(path5.Y - path6.Z + K - 100), (short)((Math.Abs(J - 100) + Math.Abs(K - 100)) / 50 + 3), 100, ref effect.Path);
                effect.TargetY = effect.Path.Count;
                var count14 = effect.Path.Count;
                var item = effect.Path[count14 - 1];
                var count15 = effect.Path.Count;
                var fPathItem = effect.Path[count15 - 1];
                var count16 = effect.Path.Count;
                var path10 = effect.Path[count16 - 1];
                var count17 = effect.Path.Count;
                var path11 = effect.Path[count17 - 1];
                var count18 = effect.Path.Count;
                var path12 = effect.Path[count18 - 1];
                var count19 = effect.Path.Count;
                EffectPaths.MakeArcPathXY((short)effect.Path[count19 - 1].X, (short)(path11.Y - path12.Z), (short)(deltaX + 16), (short)(deltaY + 16), (short)((Math.Abs(item.Y - fPathItem.Z - deltaY) + Math.Abs(path10.X - deltaX)) / 50 + 4), 100, ref effect.Path);
                break;
            case BattleEffectType.ScreenNESW:
                point = new Point3D();
                if (Random(100) < 50)
                {
                    point.X = 0;
                }
                else
                {
                    point.X = 1;
                }

                effect.Path.Add(point);
                if (_strikeIndex == 9)
                {
                    effect.Target = _unit;
                }

                break;
            case BattleEffectType.ScreenNS:
                switch (SpellId)
                {
                    case 124:
                        I = 19;
                        break;
                    default:
                        I = 7;
                        break;
                }

                HexGeometry.CenterHexNumberToHex(TargetX, TargetY, RandomHex[_strikeIndex % I], ref deltaX, ref deltaY);
                HexGeometry.HexToPixel(deltaX, deltaY, ref x2, ref y2);
                CombatMap.Current.MakeVisible(deltaX, deltaY, 0, 5);
                x1 = Random(20) + 6;
                y1 = Random(20) + 6;
                EffectPaths.MakeLinePath(x2 + x1 - 100, y2 + y1 - 300, x2 + x1, y2 + y1, 30, ref effect.Path);
                switch (SpellId)
                {
                    case 124:
                        effect.Target = null;
                        break;
                    default:
                        {
                            hexSprite = (TacticalBattleUnitHexSprite)CombatMap.Current.MapContainer.MapLevels[0].GetField(deltaX, deltaY).FindOwnedChild(2228484);
                            if (hexSprite != null)
                            {
                                effect.Target = hexSprite.Unit;
                            }
                        }

                        break;
                }

                break;
            case BattleEffectType.Chain:
                var hexY6 = _unit.UnitHexSprite.HexY;
                HexGeometry.HexToPixel(_unit.UnitHexSprite.HexX, hexY6, ref x1, ref y1);
                HexGeometry.HexToPixel(TargetX, TargetY, ref x2, ref y2);
                EffectPaths.MakeLightningPath(x1 + 16, y1, x2 + 16, y2, ref effect.Path);
                var count20 = effect.Path.Count;
                effect.Path[count20 - 1].Z = 16;
                I = 3;
                used = new DelphiList<CombatObject>();
                used.Add(_unit);
                hexSprite = (TacticalBattleUnitHexSprite)CombatMap.Current.MapContainer.MapLevels[0].GetField(TargetX, TargetY).FindOwnedChild(2228484);
                if (hexSprite != null)
                {
                    used.Add(hexSprite.Unit);
                }

                target = FindTarget(TargetX, TargetY);
                while (target != null && I > 0)
                {
                    used.Add(target);
                    x1 = x2 + 16;
                    y1 = y2 + 16;
                    var hexY4 = target.UnitHexSprite.HexY;
                    HexGeometry.HexToPixel(target.UnitHexSprite.HexX, hexY4, ref x2, ref y2);
                    EffectPaths.MakeLightningPath(x1, y1, x2 + 16, y2, ref effect.Path);
                    I -= 1;
                    var hexY5 = target.UnitHexSprite.HexY;
                    target = FindTarget(target.UnitHexSprite.HexX, hexY5);
                }

                used?.Dispose();
                break;
            case BattleEffectType.Swarm:
                var hexY7 = _unit.UnitHexSprite.HexY;
                HexGeometry.HexToPixel(_unit.UnitHexSprite.HexX, hexY7, ref x1, ref y1);
                var random3 = Random(360);
                var random4 = Random(20);
                var random5 = Random(20);
                var random6 = Random(2);
                var random7 = Random(90);
                var random8 = Random(5);
                EffectPaths.MakeSwarmPath(Random(10) + 10, random8 + 20, random7, random3, random4 + x1 + 6, random5 + y1 - 10, (SwarmDirection)random6, ref effect.Path);
                K = effect.Path.Count - 1;
                {
                    I = 0;
                    for (; I <= 2; I += 1)
                    {
                        J = 0;
                        int last = K;
                        for (; J <= last; J += 1)
                        {
                            point = new Point3D();
                            point.X = effect.Path[J].X;
                            point.Y = effect.Path[J].Y;
                            effect.Path.Add(point);
                        }
                    }
                }

                HexGeometry.HexToPixel(TargetX, TargetY, ref x2, ref y2);
                var count21 = effect.Path.Count;
                var path13 = effect.Path[count21 - 1];
                var count22 = effect.Path.Count;
                EffectPaths.MakeLinePath(effect.Path[count22 - 1].X, path13.Y, x2 + 16, y2, 15, ref effect.Path);
                Path = new Point3DList();
                var random9 = Random(360);
                var count23 = effect.Path.Count;
                var path14 = effect.Path[count23 - 1];
                var count24 = effect.Path.Count;
                var path15 = effect.Path[count24 - 1];
                var random10 = Random(2);
                var random11 = Random(90);
                var random12 = Random(5);
                EffectPaths.MakeSwarmPath(Random(10) + 10, random12 + 20, random11, random9, path14.X, path15.Y, (SwarmDirection)random10, ref Path);
                var pathItem = Path[0];
                var count25 = effect.Path.Count;
                var path16 = effect.Path[count25 - 1];
                var count26 = effect.Path.Count;
                var path17 = effect.Path[count26 - 1];
                var pathX2 = Path[0];
                EffectPaths.MakeLinePath(path17.X, path16.Y, pathX2.X, pathItem.Y, 5, ref effect.Path);
                {
                    I = 0;
                    int last = Path.Count - 1;
                    for (; I <= last; I += 1)
                    {
                        // { Original shares the points, then frees Path. }
                        var path18 = Path[I];
                        effect.Path.Add(path18);
                    }
                }

                K = Path.Count - 1;
                {
                    I = 0;
                    for (; I <= 2; I += 1)
                    {
                        J = 0;
                        int last = K;
                        for (; J <= last; J += 1)
                        {
                            point = new Point3D();
                            point.X = Path[J].X;
                            point.Y = Path[J].Y;
                            effect.Path.Add(point);
                        }
                    }
                }

                Path?.Dispose();
                break;
            case BattleEffectType.ScreenSwarm:
                hexSprite = (TacticalBattleUnitHexSprite)CombatMap.Current.MapContainer.MapLevels[0].GetField(TargetX, TargetY).FindOwnedChild(2228484);
                if (hexSprite == null)
                {
                    target = null;
                }
                else
                {
                    target = hexSprite.Unit;
                }

                deltaX = TargetX;
                deltaY = TargetY;
                ChooseTarget();
                if (target != null)
                {
                    deltaX = target.UnitHexSprite.HexX;
                    deltaY = target.UnitHexSprite.HexY;
                }

                effect.Target = target;
                HexGeometry.HexToPixel(deltaX, deltaY, ref x2, ref y2);
                if (_strikeIndex == 0)
                {
                    CombatMap.Current.CenterViewTo(TargetX, TargetY, 0);
                }

                point = new Point3D();
                point.X = x2;
                point.Y = y2;
                effect.Path.Add(point);
                var random13 = Random(360);
                var random14 = Random(20);
                var random15 = Random(20);
                var random16 = Random(2);
                var random17 = Random(90);
                var random18 = Random(10);
                EffectPaths.MakeSwarmPath(Random(10) + 10, random18 + 25, random17, random13, random14, random15 - 10, (SwarmDirection)random16, ref effect.Path);
                K = effect.Path.Count - 1;
                {
                    I = 0;
                    for (; I <= 4; I += 1)
                    {
                        J = 1;
                        int last = K;
                        for (; J <= last; J += 1)
                        {
                            point = new Point3D();
                            point.X = effect.Path[J].X;
                            point.Y = effect.Path[J].Y;
                            effect.Path.Add(point);
                        }
                    }
                }

                {
                    I = 0;
                    for (; I <= 49; I += 1)
                    {
                        effect.Path[I + 1].Z = 100 - I * 2;
                    }
                }

                break;
            case BattleEffectType.FrostBeam:
                HexGeometry.HexToPixel(TargetX, TargetY, ref x2, ref y2);
                effect.TargetX = TargetX;
                effect.TargetY = TargetY;
                CombatMap.Current.CenterViewTo(TargetX - 3, TargetY - 3, 0);
                EffectPaths.MakeArcPathXY((short)(x2 - 200), (short)(y2 - 200), (short)(x2 + 16), (short)(y2 + 16), 5, 100, ref effect.Path);
                break;
            case BattleEffectType.Ray:
                CombatMap.Current.MakeVisible(TargetX - 3, TargetY - 3, 0, 3);
                var hexX3 = _unit.UnitHexSprite.HexX;
                var hexY8 = _unit.UnitHexSprite.HexY;
                TacticalTables.MakeRangedPath(_unit.GetSide(), 10, 0, 50, hexX3, hexY8, (short)TargetX, (short)TargetY, 0, 0, true, ref effect.Path, ref obstacles);
                break;
            default:
                var hexX4 = _unit.UnitHexSprite.HexX;
                var hexY9 = _unit.UnitHexSprite.HexY;
                TacticalTables.MakeAbilityPath(_unit.GetSide(), (short)AbilityId, (short)SpellId, hexX4, hexY9, (short)TargetX, (short)TargetY, ref effect.Path, ref obstacles);
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
                hexSprite = (TacticalBattleUnitHexSprite)CombatMap.Current.MapContainer.MapLevels[0].GetField(TargetX, TargetY).FindOwnedChild(2228484);
                if (hexSprite != null)
                {
                    effect.Target = hexSprite.Unit;
                }

                I = 0;
                while (I < obstacles.Count)
                {
                    obstacle = obstacles[I];
                    if (AoWHSMap.Current.Random(100) < obstacle.Chance)
                    {
                        var path = effect.Path[obstacle.PathIndex];
                        HexGeometry.PixelToHex(effect.Path[obstacle.PathIndex].X, path.Y, ref J, ref K);
                        if (effect.EffectType == BattleEffectType.Breath)
                        {
                            hitObject = null;
                        }
                        else
                        {
                            hitObject = (HexSprite)CombatMap.Current.MapContainer.MapLevels[0].GetField(J, K).FindOwnedChild(2228484);
                        }

                        if (hitObject == null)
                        {
                            hitObject = (HexSprite)CombatMap.Current.MapContainer.MapLevels[0].GetField(J, K).FindOwnedChild(2228582);
                        }

                        if (hitObject == null)
                        {
                            hitObject = (HexSprite)CombatMap.Current.MapContainer.MapLevels[0].GetField(J, K).FindOwnedChild(2229508);
                        }

                        if (hitObject == null)
                        {
                            hitObject = (HexSprite)CombatMap.Current.MapContainer.MapLevels[0].GetField(J, K).FindOwnedChild(2229510);
                        }

                        if (hitObject != null || CombatMap.Current.MapContainer.MapLevels[0].GetField(J, K).TerrainType == 15 || CombatMap.Current.MapContainer.MapLevels[0].GetField(J, K).TerrainType == 7 || CombatMap.Current.MapContainer.MapLevels[0].GetField(J, K).OverlayType == 8)
                        {
                            J = obstacle.PathIndex + 1;
                            int last = effect.Path.Count - 1;
                            for (; J <= last; J += 1)
                            {
                                var count = effect.Path.Count;
                                effect.Path.Delete(count - 1);
                            }

                            if (!(hitObject is TacticalBattleUnitHexSprite))
                            {
                                effect.Target = null;
                                if (effect.EffectType == BattleEffectType.Breath)
                                {
                                    effect.TargetX = 0;
                                }
                            }
                            else
                            {
                                effect.Target = ((TacticalBattleUnitHexSprite)hitObject).Unit;
                            }

                            I = obstacles.Count;
                        }
                    }

                    I += 1;
                }

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
                            if (AoWHSMap.Current.Random(100) < 75)
                            {
                                effect.Target = null;
                            }

                            break;
                        case 100:
                            extraEffect = CombatMap.Current.AddEffect(BattleEffectType.SFOverlay);
                            var hexY = _unit.UnitHexSprite.HexY;
                            HexGeometry.HexToPixel(_unit.UnitHexSprite.HexX, hexY, ref x1, ref y1);
                            while (extraEffect.Path.Count < effect.Path.Count)
                            {
                                point = new Point3D();
                                point.X = x1 + 16;
                                point.Y = y1;
                                extraEffect.Path.Add(point);
                            }

                            extraEffect.AbilityIndex = AbilityId;
                            extraEffect.SpellIndex = SpellId;
                            break;
                    }

                    break;
            }
        }

        var hexY10 = _unit.UnitHexSprite.HexY;
        var hexX5 = _unit.UnitHexSprite.HexX;
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
                    }

                    break;
            }
        }

        if (Set<BattleEffectType>.Of(BattleEffectType.Rock, BattleEffectType.Comet, BattleEffectType.Breath, BattleEffectType.CometBall, BattleEffectType.ScreenNS).Contains(_effectType))
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

        I = 0;
        int limit = obstacles.Count - 1;
        for (; I <= limit; I += 1)
        {
            (obstacles[I] as IDisposable)?.Dispose();
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
            switch (ratio)
            {
                case >= 0 and <= 12:
                    result = 6;
                    break;
                case >= 13 and <= 40:
                    result = 5;
                    break;
                case >= 41 and <= 76:
                    result = 4;
                    break;
                case >= 77 and <= 129:
                    result = 3;
                    break;
                case >= 130 and <= 240:
                    result = 2;
                    break;
                case >= 241 and <= 759:
                    result = 1;
                    break;
                default:
                    result = 0;
                    break;
            }
        }

        if (deltaY > 0)
        {
            result = (ushort)(12 - result);
        }

        if (deltaX < 0 && result > 0)
        {
            result = (ushort)(24 - result);
        }

        return result;
    }

    // NetworkE.pas:230

    // NetworkE.pas:230

}
