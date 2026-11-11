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
    public new const int ClassID = 2236419;

    public TacticalBattleUnit FUnit;
    public int FAbilityID;
    public int FSpellID;
    public ushort FSourceX;
    public ushort FSourceY;
    public ushort FTargetX;
    public ushort FTargetY;
    public BattleEffectType FEffectType;
    public int FStrikeCount;
    public object FSound;
    public int FDelay;
    public int FStrikeIndex;
    public int FState;

    // CombatTE.pas:920
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwInteger(ref FAbilityID, 30);
        Stream.rwInteger(ref FSpellID, 31);
        Stream.rwWord(ref FTargetX, 32);
        Stream.rwWord(ref FTargetY, 33);
        Stream.rwWord(ref FSourceX, 34);
        Stream.rwWord(ref FSourceY, 35);
    }

    // CombatTE.pas:931
    public override ExecuteResult Execute(TokenExecutor TE)
    {
        int I = default;
        int X = default;
        int Y = default;
        int EffectIndex = default;
        BattleEffectSprite Effect = default;
        BattleEffectSprite ExtraEffect = default;
        int EndX = default;
        int EndY = default;
        SpellCombatInfo Info = default;
        TacticalBattleUnitHexSprite HS;
        TacticalBattleOozeHexSprite Ooze = default;
        CombatAction Action = default;
        MapFieldMessage Msg = default;
        base.Execute(TE);
        ExecuteResult Result = ExecuteResult.Retry;
        try
        {
            switch (FState)
            {
                case 0:
                    try
                    {
                        HS = (TacticalBattleUnitHexSprite)CombatMap.Current.MapContainer.MapLevels[0].GetField(FSourceX, FSourceY).FindOwnedChild(2228484);
                        if (HS == null)
                        {
                            Result = ExecuteResult.Done;
                        }
                        else
                        {
                            FUnit = HS.FUnit;
                            FUnit.FUnitHS.Unselect();
                            FUnit.FUnitHS.FSelectedAbility = FAbilityID;
                            FUnit.FUnitHS.FSelectedSpell = FSpellID;
                            if (FAbilityID == 52)
                            {
                                FEffectType = TacticalTables.SpellTypes[FSpellID];
                            }
                            else
                            {
                                FEffectType = TacticalTables.AbilTypes[FAbilityID];
                            }

                            if (FAbilityID == 52)
                            {
                                AoWHSSet.Current.FSpellControl.GetSpell(FSpellID).CombatSpellCast(FUnit);
                            }

                            bool notExplorationEnabled = !CombatMap.Current.FExplorationEnabled;
                            if (!notExplorationEnabled)
                            {
                                var yhx = FUnit.FUnitHS.Yhx;
                                var xhx = FUnit.FUnitHS.Xhx;
                                notExplorationEnabled = ((AoWMapField)CombatMap.Current.MapContainer.GetField(xhx, yhx, 0)).GetExplored(CombatMap.Current.FSeatedPlayer);
                            }

                            if (notExplorationEnabled)
                            {
                                var yhx = FUnit.FUnitHS.Yhx;
                                var xhx = FUnit.FUnitHS.Xhx;
                                CombatMap.Current.MakeVisible(xhx, yhx, 0, 2);
                            }

                            switch (FEffectType)
                            {
                                case BattleEffectType.Breath:
                                    FStrikeCount = 20;
                                    break;
                                case BattleEffectType.ScreenNESW:
                                    FStrikeCount = 10;
                                    break;
                                case BattleEffectType.ScreenNS:
                                    switch (FSpellID)
                                    {
                                        case 116:
                                            FStrikeCount = 21;
                                            break;
                                        case 124:
                                            FStrikeCount = 19;
                                            break;
                                        default:
                                            FStrikeCount = 35;
                                            break;
                                    }

                                    break;
                                case BattleEffectType.ShockWave:
                                    FTargetX = FSourceX;
                                    FTargetY = FSourceY;
                                    FStrikeCount = 1;
                                    break;
                                case BattleEffectType.Touch:
                                    FStrikeCount = 1;
                                    break;
                                case BattleEffectType.Swarm:
                                case BattleEffectType.ScreenSwarm:
                                    FStrikeCount = 30;
                                    break;
                                default:
                                    if (FAbilityID == 52)
                                    {
                                        AoWHSSet.Current.FSpellControl.GetSpell(FSpellID).GetCombatInfo(FUnit.FUnit, ref Info);
                                        FStrikeCount = Info.RepeatCount;
                                    }
                                    else
                                    {
                                        FStrikeCount = ((RangedAttackAbility)AoWHSSet.Current.FAbilityControl.GetAbility(FAbilityID)).GetAttackRepeatRA(FUnit.FUnit);
                                    }

                                    break;
                            }

                            switch (FEffectType)
                            {
                                case BattleEffectType.Tremors:
                                    FSound = AoWHSSet.Current.FSpellControl.GetSpell(FSpellID).FCastSounds.PlayEx(0, 64, true, 0, SamplePanning.Center, null);
                                    NextStrike();
                                    break;
                                case BattleEffectType.ScreenSwarm:
                                    FSound = AoWHSSet.Current.FSpellControl.GetSpell(FSpellID).FCastSounds.PlayEx(2, 64, true, 0, SamplePanning.Center, null);
                                    {
                                        I = 0;
                                        int last = FStrikeCount - 1;
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

                            FUnit.FMovePoints = 0;
                            FState += 1;
                        }
                    }
                    catch
                    {
                        MessageBox(0, "Error during Token Start", "Error - TCRangedToken " + IntToStr(FAbilityID) + "/" + IntToStr(FSpellID), 0);
                    }

                    break;
                case 1:
                    try
                    {
                        EffectIndex = 0;
                        while (EffectIndex < CombatMap.Current.FEffects.Count)
                        {
                            EndX = -1;
                            EndY = -1;
                            Effect = CombatMap.Current.FEffects[EffectIndex];
                            if (Effect.FAbilityIndex == 52 && Effect.FSpellIndex == 117)
                            {
                                if (Effect.FEffectType == BattleEffectType.Rock)
                                {
                                    if (Effect.FDone)
                                    {
                                        Effect.FEffectType = BattleEffectType.FBExplosion;
                                        Effect.FDone = false;
                                        while (Effect.FPath.Count > 1)
                                        {
                                            Effect.FPath.Delete(0);
                                        }
                                    }
                                }
                                else
                                {
                                    switch (Effect.FFrame)
                                    {
                                        case 5:
                                            Msg.MsgID = 200707;
                                            Msg.Param[0] = FUnit;
                                            Msg.Result = -1;
                                            var path = Effect.FPath[0];
                                            HexGeometry.HPtoHX(Effect.FPath[0].X, path.Y, ref EndX, ref EndY);
                                            {
                                                I = 0;
                                                for (; I <= 6; I += 1)
                                                {
                                                    HexGeometry.CenterHNtoHX(EndX, EndY, I, ref X, ref Y);
                                                    CombatMap.Current.MapContainer.MapLevels[0].GetField(X, Y).SendMapFieldMsg(ref Msg);
                                                    EffectPaths.CreateFire(false, 50, X, Y);
                                                }
                                            }

                                            break;
                                    }
                                }
                            }
                            else
                            {
                                if (Effect.FAbilityIndex == 52 && Effect.FSpellIndex == 107)
                                {
                                    if (Effect.FFrame != -1 && Effect.FPath[Effect.FFrame].Z == 15)
                                    {
                                        var path = Effect.FPath[Effect.FFrame];
                                        HexGeometry.HPtoHX(Effect.FPath[Effect.FFrame].X, path.Y, ref X, ref Y);
                                        HS = (TacticalBattleUnitHexSprite)CombatMap.Current.MapContainer.MapLevels[0].GetField(X, Y).FindOwnedChild(2228484);
                                        if (HS != null)
                                        {
                                            RandSeed = AoWHSMap.Current.Random(16777215);
                                            Effect.FCombatAction = AoWHSSet.Current.FSpellControl.GetSpell(Effect.FSpellIndex).CreateCA(FUnit, HS.FUnit, false);
                                            var yhx = HS.Yhx;
                                            HexGeometry.HXtoHP(HS.Xhx, yhx, ref I, ref X);
                                            if (((DamageCombatAction)Effect.FCombatAction).FDamageTypes.Value - ((DamageCombatAction)Effect.FCombatAction).FEffects.Value != Set<DamageType>.Of())
                                            {
                                                CombatEvents.MakeDEHSList(I, X, ((DamageCombatAction)Effect.FCombatAction).FDamageTypes.Value - ((DamageCombatAction)Effect.FCombatAction).FEffects.Value, false);
                                            }

                                            if (((DamageCombatAction)Effect.FCombatAction).FEffects.Value != Set<DamageType>.Of())
                                            {
                                                CombatEvents.MakeDEHSList(I, X, ((DamageCombatAction)Effect.FCombatAction).FEffects.Value, true);
                                            }

                                            CombatMap.Current.FCombat.ExecuteCombatAction(Effect.FCombatAction);
                                            Effect.FCombatAction.Release();
                                            Effect.FCombatAction = null;
                                        }
                                    }
                                }
                                else
                                {
                                    if (Effect.FAbilityIndex == 52 && Effect.FSpellIndex == 112)
                                    {
                                        switch (Effect.FFrame)
                                        {
                                            case 89:
                                                Msg.MsgID = 200708;
                                                Msg.Param[0] = FUnit;
                                                Msg.Param[1] = 0;
                                                Msg.Result = -1;
                                                {
                                                    I = 6;
                                                    for (; I <= 59; I += 1)
                                                    {
                                                        X = 6;
                                                        for (; X <= 52; X += 1)
                                                        {
                                                            CombatMap.Current.MapContainer.MapLevels[0].GetField(I, X).SendMapFieldMsg(ref Msg);
                                                        }
                                                    }
                                                }

                                                break;
                                        }
                                    }
                                    else
                                    {
                                        if (Effect.FAbilityIndex == 52 && Effect.FEffectType == BattleEffectType.Resurrect && (Effect.FSpellIndex == 130 || Effect.FSpellIndex == 129))
                                        {
                                            switch (Effect.FFrame)
                                            {
                                                case 0:
                                                    Msg.MsgID = 200712;
                                                    Msg.Param[0] = FUnit;
                                                    Msg.Param[1] = FSpellID;
                                                    Msg.Result = -1;
                                                    {
                                                        I = 0;
                                                        for (; I <= 6; I += 1)
                                                        {
                                                            HexGeometry.CenterHNtoHX(FTargetX, FTargetY, I, ref X, ref Y);
                                                            CombatMap.Current.MapContainer.MapLevels[0].GetField(X, Y).SendMapFieldMsg(ref Msg);
                                                        }
                                                    }

                                                    break;
                                                case 99:
                                                    Msg.MsgID = 200713;
                                                    Msg.Param[0] = FUnit;
                                                    Msg.Param[1] = FSpellID;
                                                    Msg.Result = -1;
                                                    {
                                                        I = 0;
                                                        for (; I <= 6; I += 1)
                                                        {
                                                            HexGeometry.CenterHNtoHX(FTargetX, FTargetY, I, ref X, ref Y);
                                                            CombatMap.Current.MapContainer.MapLevels[0].GetField(X, Y).SendMapFieldMsg(ref Msg);
                                                        }
                                                    }

                                                    break;
                                            }
                                        }
                                        else
                                        {
                                            if (Effect.FAbilityIndex == 52 && Effect.FSpellIndex == 104)
                                            {
                                                switch (Effect.FFrame)
                                                {
                                                    case 199:
                                                        Msg.MsgID = 200709;
                                                        Msg.Param[0] = FUnit;
                                                        Msg.Result = -1;
                                                        {
                                                            I = 0;
                                                            for (; I <= 6; I += 1)
                                                            {
                                                                HexGeometry.CenterHNtoHX(FTargetX, FTargetY, I, ref X, ref Y);
                                                                CombatMap.Current.MapContainer.MapLevels[0].GetField(X, Y).SendMapFieldMsg(ref Msg);
                                                            }
                                                        }

                                                        break;
                                                }
                                            }
                                            else
                                            {
                                                if (Effect.FAbilityIndex == 52 && Effect.FSpellIndex == 127)
                                                {
                                                    switch (Effect.FFrame)
                                                    {
                                                        case 175:
                                                            Msg.MsgID = 200717;
                                                            Msg.Param[0] = FUnit;
                                                            Msg.Result = -1;
                                                            {
                                                                I = 1;
                                                                for (; I <= 6; I += 1)
                                                                {
                                                                    HexGeometry.CenterHNtoHX(FTargetX, FTargetY, I, ref X, ref Y);
                                                                    CombatMap.Current.MapContainer.MapLevels[0].GetField(X, Y).SendMapFieldMsg(ref Msg);
                                                                }
                                                            }

                                                            break;
                                                        case 200:
                                                            Msg.MsgID = 200717;
                                                            Msg.Param[0] = FUnit;
                                                            Msg.Result = -1;
                                                            {
                                                                I = 7;
                                                                for (; I <= 18; I += 1)
                                                                {
                                                                    HexGeometry.CenterHNtoHX(FTargetX, FTargetY, I, ref X, ref Y);
                                                                    CombatMap.Current.MapContainer.MapLevels[0].GetField(X, Y).SendMapFieldMsg(ref Msg);
                                                                }
                                                            }

                                                            break;
                                                        case 225:
                                                            Msg.MsgID = 200717;
                                                            Msg.Param[0] = FUnit;
                                                            Msg.Result = -1;
                                                            {
                                                                I = 19;
                                                                for (; I <= 36; I += 1)
                                                                {
                                                                    HexGeometry.CenterHNtoHX(FTargetX, FTargetY, I, ref X, ref Y);
                                                                    CombatMap.Current.MapContainer.MapLevels[0].GetField(X, Y).SendMapFieldMsg(ref Msg);
                                                                }
                                                            }

                                                            break;
                                                    }
                                                }
                                                else
                                                {
                                                    if (Effect.FAbilityIndex == 52 && Effect.FSpellIndex == 128)
                                                    {
                                                        switch (Effect.FFrame)
                                                        {
                                                            case 100:
                                                                var yhx = FUnit.FUnitHS.Yhx;
                                                                EffectPaths.CreateFire(false, 50, FUnit.FUnitHS.Xhx, yhx);
                                                                Msg.MsgID = 200718;
                                                                Msg.Param[0] = FUnit;
                                                                Msg.Result = -1;
                                                                var fUnitHSYhx = FUnit.FUnitHS.Yhx;
                                                                var xhx = FUnit.FUnitHS.Xhx;
                                                                CombatMap.Current.MapContainer.MapLevels[0].GetField(xhx, fUnitHSYhx).SendMapFieldMsg(ref Msg);
                                                                break;
                                                            case 175:
                                                                Msg.MsgID = 200718;
                                                                Msg.Param[0] = FUnit;
                                                                Msg.Result = -1;
                                                                {
                                                                    I = 0;
                                                                    for (; I <= 18; I += 1)
                                                                    {
                                                                        HexGeometry.CenterHNtoHX(FTargetX, FTargetY, I, ref X, ref Y);
                                                                        CombatMap.Current.MapContainer.MapLevels[0].GetField(X, Y).SendMapFieldMsg(ref Msg);
                                                                        EffectPaths.CreateFire(false, 50, X, Y);
                                                                    }
                                                                }

                                                                break;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (Effect.FAbilityIndex == 52 && Effect.FSpellIndex == 105 && Effect.FTarget != null)
                                                        {
                                                            I = 0;
                                                            int last = CombatMap.Current.FCombat.FCombatData.GetCount() - 1;
                                                            for (; I <= last; I += 1)
                                                            {
                                                                if (CombatMap.Current.FCombat.FCombatData.GetObjects(I) != null && CombatMap.Current.FCombat.FCombatData.GetObjects(I) is CombatUnit && CombatMap.Current.FCombat.FCombatData.GetObjects(I).GetSide() != FUnit.GetSide() && CombatMap.Current.FCombat.FCombatData.GetObjects(I).GetEnabled() && ((TacticalBattleUnit)CombatMap.Current.FCombat.FCombatData.GetObjects(I)).FUnitHS != null)
                                                                {
                                                                    RandSeed = AoWHSMap.Current.Random(16777215);
                                                                    var combatObject = CombatMap.Current.FCombat.FCombatData.GetObjects(I);
                                                                    Action = AoWHSSet.Current.FSpellControl.GetSpell(FSpellID).CreateCA(FUnit, combatObject, false);
                                                                    if (((ExclusiveCombatSpellCombatAction)Action).FSucceeded)
                                                                    {
                                                                        ((TacticalBattleUnit)CombatMap.Current.FCombat.FCombatData.GetObjects(I)).FUnitHS.SetAction(TacticalBattleUnitHexSprite.SpriteAction.Hit);
                                                                    }

                                                                    CombatMap.Current.FCombat.ExecuteCombatAction(Action);
                                                                    Action.Release();
                                                                }
                                                            }

                                                            Effect.FTarget = null;
                                                        }
                                                        else
                                                        {
                                                            if (Effect.FAbilityIndex == 52 && Effect.FSpellIndex == 120)
                                                            {
                                                                switch (Effect.FFrame)
                                                                {
                                                                    case 2999:
                                                                        Msg.MsgID = 200716;
                                                                        Msg.Param[0] = FUnit;
                                                                        Msg.Result = -1;
                                                                        {
                                                                            I = 0;
                                                                            for (; I <= 6; I += 1)
                                                                            {
                                                                                HexGeometry.CenterHNtoHX(FTargetX, FTargetY, I, ref X, ref Y);
                                                                                CombatMap.Current.MapContainer.MapLevels[0].GetField(X, Y).SendMapFieldMsg(ref Msg);
                                                                            }
                                                                        }

                                                                        break;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                if (Effect.FEffectType == BattleEffectType.SacredWrath && Effect.FAbilityIndex == 52 && Effect.FSpellIndex == 125)
                                                                {
                                                                    switch (Effect.FFrame)
                                                                    {
                                                                        case 5:
                                                                            {
                                                                                I = 0;
                                                                                int last = CombatMap.Current.FCombat.FCombatData.GetCount() - 1;
                                                                                for (; I <= last; I += 1)
                                                                                {
                                                                                    if (CombatMap.Current.FCombat.FCombatData.GetObjects(I) is CombatUnit && CombatMap.Current.FCombat.FCombatData.GetObjects(I).GetEnabled())
                                                                                    {
                                                                                        HS = ((TacticalBattleUnit)CombatMap.Current.FCombat.FCombatData.GetObjects(I)).FUnitHS;
                                                                                        if (HS != null)
                                                                                        {
                                                                                            ExtraEffect = CombatMap.Current.AddEffect(BattleEffectType.SWComet);
                                                                                            ExtraEffect.FAbilityIndex = FAbilityID;
                                                                                            ExtraEffect.FSpellIndex = FSpellID;
                                                                                            ExtraEffect.FTarget = HS.FUnit;
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }

                                                                            break;
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (Effect.FDone && Effect.FTarget != null && Effect.FTarget.GetEnabled())
                                                                    {
                                                                        I = 0;
                                                                        RandSeed = AoWHSMap.Current.Random(16777215);
                                                                        if (FAbilityID == 52)
                                                                        {
                                                                            Effect.FCombatAction = AoWHSSet.Current.FSpellControl.GetSpell(FSpellID).CreateCA(FUnit, Effect.FTarget, false);
                                                                        }
                                                                        else
                                                                        {
                                                                            if (FAbilityID == 47)
                                                                            {
                                                                                Effect.FCombatAction = ((HealingAbility)AoWHSSet.Current.FAbilityControl.GetAbility(FAbilityID)).CreateHealingCA(FUnit, Effect.FTarget);
                                                                            }
                                                                            else
                                                                            {
                                                                                if (FAbilityID == 60)
                                                                                {
                                                                                    Effect.FCombatAction = ((DispelMagicAbility)AoWHSSet.Current.FAbilityControl.GetAbility(FAbilityID)).CreateCA(FUnit, Effect.FTarget);
                                                                                }
                                                                                else
                                                                                {
                                                                                    Effect.FCombatAction = ((RangedAttackAbility)AoWHSSet.Current.FAbilityControl.GetAbility(FAbilityID)).CreateRangedAttackCA(FUnit, Effect.FTarget, 0);
                                                                                }
                                                                            }
                                                                        }

                                                                        if (Effect.FCombatAction is DamageCombatAction)
                                                                        {
                                                                            I = ((DamageCombatAction)Effect.FCombatAction).FDamage;
                                                                            var yhx = ((TacticalBattleUnit)Effect.FTarget).FUnitHS.Yhx;
                                                                            HexGeometry.HXtoHP(((TacticalBattleUnit)Effect.FTarget).FUnitHS.Xhx, yhx, ref X, ref Y);
                                                                            if (((DamageCombatAction)Effect.FCombatAction).FDamageTypes.Value - ((DamageCombatAction)Effect.FCombatAction).FEffects.Value != Set<DamageType>.Of())
                                                                            {
                                                                                CombatEvents.MakeDEHSList(X, Y, ((DamageCombatAction)Effect.FCombatAction).FDamageTypes.Value - ((DamageCombatAction)Effect.FCombatAction).FEffects.Value, false);
                                                                            }

                                                                            if (((DamageCombatAction)Effect.FCombatAction).FEffects.Value != Set<DamageType>.Of())
                                                                            {
                                                                                CombatEvents.MakeDEHSList(X, Y, ((DamageCombatAction)Effect.FCombatAction).FEffects.Value, true);
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            if (Effect.FCombatAction is EntangleSpellCombatAction)
                                                                            {
                                                                                if (((ExclusiveCombatSpellCombatAction)Effect.FCombatAction).FSucceeded)
                                                                                {
                                                                                    ((TacticalBattleUnit)Effect.FTarget).FUnitHS.SetAction(TacticalBattleUnitHexSprite.SpriteAction.Hit);
                                                                                }
                                                                            }
                                                                            else
                                                                            {
                                                                                if (Effect.FCombatAction is MindDecayCombatAction && ((ExclusiveCombatSpellCombatAction)Effect.FCombatAction).FSucceeded)
                                                                                {
                                                                                    ((TacticalBattleUnit)Effect.FTarget).FUnitHS.SetAction(TacticalBattleUnitHexSprite.SpriteAction.Hit);
                                                                                }
                                                                            }
                                                                        }

                                                                        if (I > 0)
                                                                        {
                                                                            ((TacticalBattleUnit)Effect.FTarget).FUnitHS.SetAction(TacticalBattleUnitHexSprite.SpriteAction.Hit);
                                                                            TacticalBattleUnit.MakeHitBlood((TacticalBattleUnit)Effect.FTarget, I);
                                                                            Effect.FCancelled = true;
                                                                        }
                                                                    }

                                                                    if (Effect.FDone)
                                                                    {
                                                                        var count = Effect.FPath.Count;
                                                                        var path = Effect.FPath[count - 1];
                                                                        var fPathCount = Effect.FPath.Count;
                                                                        HexGeometry.HPtoHX(Effect.FPath[fPathCount - 1].X, path.Y, ref EndX, ref EndY);
                                                                        if (Effect.FCombatAction != null && Effect.FTarget != null)
                                                                        {
                                                                            CombatMap.Current.FCombat.ExecuteCombatAction(Effect.FCombatAction);
                                                                            Effect.FCombatAction.Release();
                                                                            Effect.FCombatAction = null;
                                                                            if (!Effect.FTarget.GetEnabled())
                                                                            {
                                                                                I = 0;
                                                                                int last = CombatMap.Current.FEffects.Count - 1;
                                                                                for (; I <= last; I += 1)
                                                                                {
                                                                                    if (I != EffectIndex)
                                                                                    {
                                                                                        if ((CombatMap.Current.FEffects[I]).FTarget == Effect.FTarget)
                                                                                        {
                                                                                            (CombatMap.Current.FEffects[I]).FTarget = null;
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            if (EndX > -1)
                                                                            {
                                                                                switch (Effect.FAbilityIndex)
                                                                                {
                                                                                    case 30:
                                                                                    case >= 86 and <= 90:
                                                                                        if (Effect.FTargetX > 0)
                                                                                        {
                                                                                            Msg.MsgID = 200722;
                                                                                            Msg.Param[0] = FUnit;
                                                                                            Msg.Param[1] = Effect.FAbilityIndex;
                                                                                            Msg.Result = -1;
                                                                                            CombatMap.Current.MapContainer.MapLevels[0].GetField(Effect.FTargetX, Effect.FTargetY).SendMapFieldMsg(ref Msg);
                                                                                        }

                                                                                        break;
                                                                                    case 24:
                                                                                        Msg.MsgID = 200705;
                                                                                        Msg.Param[0] = FUnit;
                                                                                        Msg.Result = -1;
                                                                                        CombatMap.Current.MapContainer.MapLevels[0].GetField(EndX, EndY).SendMapFieldMsg(ref Msg);
                                                                                        break;
                                                                                    case 44:
                                                                                        Msg.MsgID = 200706;
                                                                                        Msg.Param[0] = FUnit;
                                                                                        Msg.Result = -1;
                                                                                        CombatMap.Current.MapContainer.MapLevels[0].GetField(EndX, EndY).SendMapFieldMsg(ref Msg);
                                                                                        break;
                                                                                    case 31:
                                                                                        Msg.MsgID = 200714;
                                                                                        Msg.Param[0] = FUnit;
                                                                                        Msg.Result = -1;
                                                                                        CombatMap.Current.MapContainer.MapLevels[0].GetField(EndX, EndY).SendMapFieldMsg(ref Msg);
                                                                                        EffectPaths.CreateFire(false, 100, EndX, EndY);
                                                                                        break;
                                                                                    case 52:
                                                                                        switch (Effect.FEffectType)
                                                                                        {
                                                                                            case BattleEffectType.Breath:
                                                                                                if (Effect.FTargetX > 0)
                                                                                                {
                                                                                                    Msg.MsgID = 200723;
                                                                                                    Msg.Param[0] = FUnit;
                                                                                                    Msg.Param[1] = Effect.FSpellIndex;
                                                                                                    Msg.Result = -1;
                                                                                                    CombatMap.Current.MapContainer.MapLevels[0].GetField(Effect.FTargetX, Effect.FTargetY).SendMapFieldMsg(ref Msg);
                                                                                                }

                                                                                                break;
                                                                                            case BattleEffectType.ScreenNS:
                                                                                                switch (Effect.FSpellIndex)
                                                                                                {
                                                                                                    case 116:
                                                                                                        Msg.MsgID = 200710;
                                                                                                        Msg.Param[0] = FUnit;
                                                                                                        Msg.Result = -1;
                                                                                                        CombatMap.Current.MapContainer.MapLevels[0].GetField(EndX, EndY).SendMapFieldMsg(ref Msg);
                                                                                                        break;
                                                                                                    case 124:
                                                                                                        if (CombatMap.Current.MapContainer.MapLevels[0].GetField(EndX, EndY).TerrainType != 0 && CombatMap.Current.MapContainer.MapLevels[0].GetField(EndX, EndY).TerrainType != 10 && CombatMap.Current.MapContainer.MapLevels[0].GetField(EndX, EndY).OverlayType != 2 && CombatMap.Current.MapContainer.MapLevels[0].GetField(EndX, EndY).OverlayType != 11 && CombatMap.Current.MapContainer.MapLevels[0].GetField(EndX, EndY).OverlayType != 8 && CombatMap.Current.MapContainer.MapLevels[0].GetField(EndX, EndY).TerrainType != 9 && CombatMap.Current.MapContainer.MapLevels[0].GetField(EndX, EndY).TerrainType != 15 && CombatMap.Current.MapContainer.MapLevels[0].GetField(EndX, EndY).TerrainType != 7)
                                                                                                        {
                                                                                                            Ooze = new TacticalBattleOozeHexSprite(null);
                                                                                                            CombatMap.Current.PlaceHX(Ooze, EndX, EndY, 0);
                                                                                                        }

                                                                                                        break;
                                                                                                }

                                                                                                break;
                                                                                            case BattleEffectType.Effect:
                                                                                                switch (Effect.FSpellIndex)
                                                                                                {
                                                                                                    case 113:
                                                                                                        Msg.MsgID = 200715;
                                                                                                        Msg.Param[0] = FUnit;
                                                                                                        Msg.Result = -1;
                                                                                                        CombatMap.Current.MapContainer.MapLevels[0].GetField(EndX, EndY).SendMapFieldMsg(ref Msg);
                                                                                                        EffectPaths.CreateFire(false, 100, EndX, EndY);
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

                            EffectIndex += 1;
                        }

                        if (CombatMap.Current.FEffects.Count == 0 && (FStrikeIndex >= FStrikeCount || !FUnit.GetEnabled()))
                        {
                            if (FUnit.GetEnabled())
                            {
                                FUnit.FUnitHS.SetAction(TacticalBattleUnitHexSprite.SpriteAction.Move);
                            }

                            Result = ExecuteResult.Done;
                        }

                        if (FUnit.GetEnabled())
                        {
                            if (FEffectType == BattleEffectType.Breath)
                            {
                                NextStrike();
                            }
                            else
                            {
                                if (FUnit.FUnitHS.FActionDone || (FAbilityID == 52 && FDelay <= 0))
                                {
                                    FDelay = AoWHSMap.Current.Random(3) + 4;
                                }

                                FDelay -= 1;
                                if (FDelay == 0)
                                {
                                    NextStrike();
                                }
                            }
                        }
                    }
                    catch
                    {
                        MessageBox(0, "Error during Token Execute", "Error - TCRangedToken " + IntToStr(FAbilityID) + "/" + IntToStr(FSpellID), 0);
                    }

                    break;
            }

            if (Result == ExecuteResult.Done)
            {
                if (FSound != null)
                {
                    (FSound as IDisposable)?.Dispose();
                }

                if (FUnit != null && FUnit.GetEnabled())
                {
                    FUnit.FUnitHS.FSelectedAbility = -1;
                    FUnit.FUnitHS.FSelectedSpell = -1;
                    FUnit.FUnitHS.Unselect();
                    if (FUnit.FCombatPlayer == CombatMap.Current.FCurrentCombatPlayer)
                    {
                        var xhx = FUnit.FUnitHS.Xhx;
                        CombatMap.Current.FPlayers.GetPlayers(CombatMap.Current.FCurrentCombatPlayer).FLastX = xhx;
                        var yhx = FUnit.FUnitHS.Yhx;
                        CombatMap.Current.FPlayers.GetPlayers(CombatMap.Current.FCurrentCombatPlayer).FLastY = yhx;
                    }
                }
            }
        }
        catch
        {
            MessageBox(0, "Error during Token", "Error - TCRangedToken " + IntToStr(FAbilityID) + "/" + IntToStr(FSpellID), 0);
        }

        return Result;
    }

    // CombatTE.pas:1373
    public void NextStrike()
    {
        DelphiList Used;
        TacticalBattleUnit Target;
        BattleEffectSprite Effect;
        BattleEffectSprite ExtraEffect;
        TacticalBattleUnitHexSprite HS;
        float A;
        float Sweep;
        float Angle;
        float Radius;
        int X1 = default;
        int Y1 = default;
        int X2 = default;
        int Y2 = default;
        int DX = default;
        int DY = default;
        int I;
        int J = default;
        int K = default;
        Point3D Point;
        HexSprite HitObject;
        Point3DList Path;
        DelphiList Obstacles;
        RangedObstacle Obstacle;
        bool Found;
        TacticalBattleUnit FindTarget(int X, int Y)
        {
            int Distance;
            int BestDistance;
            bool Available;
            TacticalBattleUnit Result = null;
            BestDistance = 6;
            for (int I = 0, last = CombatMap.Current.FCombat.FCombatData.GetCount() - 1; I <= last; I++)
            {
                if (CombatMap.Current.FCombat.FCombatData.GetObjects(I) is CombatUnit && CombatMap.Current.FCombat.FCombatData.GetObjects(I).GetEnabled() && CombatMap.Current.FCombat.FCombatData.GetObjects(I).GetSide() != FUnit.GetSide())
                {
                    Available = true;
                    for (int J = 0, lastJ = Used.Count - 1; J <= lastJ; J++)
                    {
                        if (CombatMap.Current.FCombat.FCombatData.GetObjects(I) == Used[J])
                        {
                            Available = false;
                        }
                    }

                    if (Available)
                    {
                        var yhx = ((TacticalBattleUnit)CombatMap.Current.FCombat.FCombatData.GetObjects(I)).FUnitHS.Yhx;
                        Distance = HexGeometry.dHXtoRad(X, Y, ((TacticalBattleUnit)CombatMap.Current.FCombat.FCombatData.GetObjects(I)).FUnitHS.Xhx, yhx);
                        if (Distance < BestDistance)
                        {
                            Result = (TacticalBattleUnit)CombatMap.Current.FCombat.FCombatData.GetObjects(I);
                            BestDistance = Distance;
                        }
                    }
                }
            }

            return Result;
        }

        void ChooseTarget()
        {
            int I;
            int Index;
            int Total;
            int Distance;
            DelphiList Weights;
            Used = new DelphiList();
            Weights = new DelphiList();
            Total = 0;
            I = 0;
            int last = CombatMap.Current.FCombat.FCombatData.GetCount() - 1;
            for (; I <= last; I += 1)
            {
                if (CombatMap.Current.FCombat.FCombatData.GetObjects(I) is CombatUnit && CombatMap.Current.FCombat.FCombatData.GetObjects(I).GetEnabled() && CombatMap.Current.FCombat.FCombatData.GetObjects(I) != Target)
                {
                    var yhx = ((TacticalBattleUnit)CombatMap.Current.FCombat.FCombatData.GetObjects(I)).FUnitHS.Yhx;
                    var xhx = ((TacticalBattleUnit)CombatMap.Current.FCombat.FCombatData.GetObjects(I)).FUnitHS.Xhx;
                    Distance = HexGeometry.dHXtoRad(FTargetX, FTargetY, xhx, yhx);
                    if (Distance < 13)
                    {
                        Total = Total + (13 - Distance);
                        var combatObject = CombatMap.Current.FCombat.FCombatData.GetObjects(I);
                        Used.Add(combatObject);
                        Weights.Add(IntToObject(13 - Distance));
                    }
                }
            }

            if (Target != null)
            {
                Used.Add(Target);
                Weights.Add(IntToObject(Total));
                Total = Total + Total;
            }

            if (Used.Count > 0)
            {
                I = AoWHSMap.Current.Random(Total) - ObjectToInt((object)Weights[0]);
                Index = 0;
                while (Index < Used.Count - 1 && I > 0)
                {
                    Index = Index + 1;
                    I = I - ObjectToInt((object)Weights[Index]);
                }

                Target = (TacticalBattleUnit)Used[Index];
            }

            Used?.Dispose();
            Weights?.Dispose();
        }

        if (FStrikeIndex >= FStrikeCount)
        {
            return;
        }

        Effect = CombatMap.Current.AddEffect(BattleEffectType.None);
        Obstacles = new DelphiList();
        switch (FEffectType)
        {
            case BattleEffectType.Breath:
                var yhx = FUnit.FUnitHS.Yhx;
                var dHXtoHN2 = HexGeometry.dHXtoHN(FUnit.FUnitHS.Xhx, yhx, FTargetX, FTargetY);
                var fUnitHSYhx = FUnit.FUnitHS.Yhx;
                HexGeometry.CenterHNtoHX(FUnit.FUnitHS.Xhx, fUnitHSYhx, TacticalTables.BreathHN[TacticalTables.BreathDirection[dHXtoHN2 - 1] * 12 - 3], ref J, ref K);
                var yhx2 = FUnit.FUnitHS.Yhx;
                HexGeometry.HXtoHP(FUnit.FUnitHS.Xhx, yhx2, ref X1, ref Y1);
                HexGeometry.HXtoHP(J, K, ref X2, ref Y2);
                Sweep = FStrikeIndex;
                if ((double)Sweep > 9)
                {
                    Sweep = (float)((double)19 - Sweep);
                }

                if (Y2 == Y1)
                {
                    A = 0;
                }
                else
                {
                    if (X2 == X1)
                    {
                        A = (float)((double)Math.PI / 2);
                    }
                    else
                    {
                        A = (float)Math.Atan((double)Math.Abs(X2 - X1) / Math.Abs(Y2 - Y1));
                    }
                }

                if (Y2 == Y1 || X2 == X1)
                {
                    var pi = Math.PI;
                    var mathPI = Math.PI;
                    Angle = (float)((double)((double)((double)pi / 2) - A) + (double)((double)((double)((double)Sweep * 6) - 30) * mathPI) / 180);
                }
                else
                {
                    Angle = (float)((double)A + (double)((double)((double)((double)Sweep * 6) - 30) * Math.PI) / 180);
                }

                Radius = (float)Math.Sqrt((X2 - X1) * (X2 - X1) + (Y2 - Y1) * (Y2 - Y1));
                if (X2 >= X1)
                {
                    DX = Round((double)Math.Sin(Angle) * Radius) - X2 + X1;
                }
                else
                {
                    DX = X1 - X2 - Round((double)Math.Sin(Angle) * Radius);
                }

                if (Y2 >= Y1)
                {
                    DY = Round((double)Math.Cos(Angle) * Radius) - Y2 + Y1;
                }
                else
                {
                    DY = Y1 - Y2 - Round((double)Math.Cos(Angle) * Radius);
                }

                var xhx = FUnit.FUnitHS.Xhx;
                var yhx3 = FUnit.FUnitHS.Yhx;
                TacticalTables.MakeRangedPath(FUnit.GetSide(), 50, 20, 50, xhx, yhx3, (short)J, (short)K, (short)DX, (short)DY, false, ref Effect.FPath, ref Obstacles);
                if (FStrikeIndex < 12)
                {
                    var yhx4 = FUnit.FUnitHS.Yhx;
                    var dHXtoHN3 = HexGeometry.dHXtoHN(FUnit.FUnitHS.Xhx, yhx4, FTargetX, FTargetY);
                    var yhx5 = FUnit.FUnitHS.Yhx;
                    HexGeometry.CenterHNtoHX(FUnit.FUnitHS.Xhx, yhx5, TacticalTables.BreathHN[TacticalTables.BreathDirection[dHXtoHN3 - 1] * 12 + BreathHit[FStrikeIndex] - 12], ref Effect.FTargetX, ref Effect.FTargetY);
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
                if (FEffectType == BattleEffectType.SacredWrath)
                {
                    FTargetX = 32;
                    FTargetY = 29;
                }

                HexGeometry.HXtoHP(FTargetX, FTargetY, ref X2, ref Y2);
                switch (FEffectType)
                {
                    case BattleEffectType.Geyser:
                        CombatMap.Current.CenterViewTo(FTargetX, FTargetY - 5, 0);
                        break;
                    case BattleEffectType.MindDecay:
                        Effect.FTargetX = FTargetX;
                        Effect.FTargetY = FTargetY;
                        CombatMap.Current.CenterViewTo(FTargetX + 1, FTargetY - 4, 0);
                        break;
                    case BattleEffectType.ShockWave:
                        var yhx4 = FUnit.FUnitHS.Yhx;
                        var fUnitHSXhx = FUnit.FUnitHS.Xhx;
                        CombatMap.Current.CenterViewTo(fUnitHSXhx, yhx4, 0);
                        break;
                    case BattleEffectType.SacFlame:
                        var yhx5 = FUnit.FUnitHS.Yhx;
                        var xhx2 = FUnit.FUnitHS.Xhx;
                        CombatMap.Current.CenterViewTo(xhx2, yhx5 - 3, 0);
                        break;
                    default:
                        CombatMap.Current.CenterViewTo(FTargetX, FTargetY, 0);
                        break;
                }

                Point = new Point3D();
                Point.X = X2 + 16;
                Point.Y = Y2 + 16;
                Effect.FPath.Add(Point);
                HS = (TacticalBattleUnitHexSprite)CombatMap.Current.MapContainer.MapLevels[0].GetField(FTargetX, FTargetY).FindOwnedChild(2228484);
                if (HS != null)
                {
                    Effect.FTarget = HS.FUnit;
                }

                break;
            case BattleEffectType.Winds:
                HexGeometry.HXtoHP(FTargetX, FTargetY, ref X2, ref Y2);
                Point = new Point3D();
                Point.X = X2 + 16;
                Point.Y = Y2 + 16;
                Effect.FPath.Add(Point);
                HS = (TacticalBattleUnitHexSprite)CombatMap.Current.MapContainer.MapLevels[0].GetField(FTargetX, FTargetY).FindOwnedChild(2228484);
                if (HS != null)
                {
                    Effect.FTarget = HS.FUnit;
                }

                I = AoWHSMap.Current.Random(37) + 1;
                Found = false;
                while (!Found)
                {
                    HexGeometry.CenterHNtoHX(FTargetX, FTargetY, I, ref J, ref K);
                    bool condition = J > 5 && J < 60 && K > 5 && K < 53 && CombatMap.Current.MapContainer.MapLevels[0].GetField(J, K).FindOwnedChild(2228484) == null;
                    if (condition)
                    {
                        bool test = CombatMap.Current.MapContainer.MapLevels[0].GetField(J, K).OverlayType != 8;
                        if (!test)
                        {
                            var fieldValue = CombatMap.Current.MapContainer.MapLevels[0].GetField(J, K);
                            var fieldValue2 = CombatMap.Current.MapContainer.MapLevels[0].GetField(FTargetX, FTargetY);
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
                            var fieldValue2 = CombatMap.Current.MapContainer.MapLevels[0].GetField(FTargetX, FTargetY);
                            test = fieldValue.TerrainType == fieldValue2.TerrainType;
                        }

                        passed = test;
                    }

                    if (passed)
                    {
                        Found = true;
                    }
                    else
                    {
                        I += 1;
                    }
                }

                HexGeometry.HXtoHP(J, K, ref DX, ref DY);
                CombatMap.Current.CenterViewTo(FTargetX, FTargetY, 0);
                {
                    I = 0;
                    for (; I <= 29; I += 1)
                    {
                        var xyz = new Point3D();
                        Effect.FPath.Add(xyz);
                        var pathX = Effect.FPath[0];
                        var random = Random(30);
                        var count = Effect.FPath.Count;
                        Effect.FPath[count - 1].X = pathX.X + random - 15;
                        var pathY = Effect.FPath[0];
                        var random2 = Random(30);
                        var fPathCount = Effect.FPath.Count;
                        Effect.FPath[fPathCount - 1].Y = pathY.Y + random2 - 15;
                    }
                }

                J = Random(300);
                K = Random(300);
                var count2 = Effect.FPath.Count;
                var path = Effect.FPath[count2 - 1];
                var count3 = Effect.FPath.Count;
                var pathZ = Effect.FPath[count3 - 1];
                var count4 = Effect.FPath.Count;
                var path2 = Effect.FPath[count4 - 1];
                var count5 = Effect.FPath.Count;
                var path3 = Effect.FPath[count5 - 1];
                var count6 = Effect.FPath.Count;
                var path4 = Effect.FPath[count6 - 1];
                var count7 = Effect.FPath.Count;
                EffectPaths.MakeArcPathXY((short)Effect.FPath[count7 - 1].X, (short)(path2.Y - path3.Z), (short)(path4.X + J - 150), (short)(path.Y - pathZ.Z + K - 150), (short)((Math.Abs(J - 150) + Math.Abs(K - 150)) / 50 + 2), 100, ref Effect.FPath);
                Effect.FTargetX = Effect.FPath.Count;
                J = Random(200);
                K = Random(200);
                var count8 = Effect.FPath.Count;
                var path5 = Effect.FPath[count8 - 1];
                var count9 = Effect.FPath.Count;
                var path6 = Effect.FPath[count9 - 1];
                var count10 = Effect.FPath.Count;
                var path7 = Effect.FPath[count10 - 1];
                var count11 = Effect.FPath.Count;
                var path8 = Effect.FPath[count11 - 1];
                var count12 = Effect.FPath.Count;
                var path9 = Effect.FPath[count12 - 1];
                var count13 = Effect.FPath.Count;
                EffectPaths.MakeArcPathXY((short)Effect.FPath[count13 - 1].X, (short)(path7.Y - path8.Z), (short)(path9.X + J - 100), (short)(path5.Y - path6.Z + K - 100), (short)((Math.Abs(J - 100) + Math.Abs(K - 100)) / 50 + 3), 100, ref Effect.FPath);
                Effect.FTargetY = Effect.FPath.Count;
                var count14 = Effect.FPath.Count;
                var item = Effect.FPath[count14 - 1];
                var count15 = Effect.FPath.Count;
                var fPathItem = Effect.FPath[count15 - 1];
                var count16 = Effect.FPath.Count;
                var path10 = Effect.FPath[count16 - 1];
                var count17 = Effect.FPath.Count;
                var path11 = Effect.FPath[count17 - 1];
                var count18 = Effect.FPath.Count;
                var path12 = Effect.FPath[count18 - 1];
                var count19 = Effect.FPath.Count;
                EffectPaths.MakeArcPathXY((short)Effect.FPath[count19 - 1].X, (short)(path11.Y - path12.Z), (short)(DX + 16), (short)(DY + 16), (short)((Math.Abs(item.Y - fPathItem.Z - DY) + Math.Abs(path10.X - DX)) / 50 + 4), 100, ref Effect.FPath);
                break;
            case BattleEffectType.ScreenNESW:
                Point = new Point3D();
                if (Random(100) < 50)
                {
                    Point.X = 0;
                }
                else
                {
                    Point.X = 1;
                }

                Effect.FPath.Add(Point);
                if (FStrikeIndex == 9)
                {
                    Effect.FTarget = FUnit;
                }

                break;
            case BattleEffectType.ScreenNS:
                switch (FSpellID)
                {
                    case 124:
                        I = 19;
                        break;
                    default:
                        I = 7;
                        break;
                }

                HexGeometry.CenterHNtoHX(FTargetX, FTargetY, rndHex[FStrikeIndex % I], ref DX, ref DY);
                HexGeometry.HXtoHP(DX, DY, ref X2, ref Y2);
                CombatMap.Current.MakeVisible(DX, DY, 0, 5);
                X1 = Random(20) + 6;
                Y1 = Random(20) + 6;
                EffectPaths.MakeLinePath(X2 + X1 - 100, Y2 + Y1 - 300, X2 + X1, Y2 + Y1, 30, ref Effect.FPath);
                switch (FSpellID)
                {
                    case 124:
                        Effect.FTarget = null;
                        break;
                    default:
                        {
                            HS = (TacticalBattleUnitHexSprite)CombatMap.Current.MapContainer.MapLevels[0].GetField(DX, DY).FindOwnedChild(2228484);
                            if (HS != null)
                            {
                                Effect.FTarget = HS.FUnit;
                            }
                        }

                        break;
                }

                break;
            case BattleEffectType.Chain:
                var yhx6 = FUnit.FUnitHS.Yhx;
                HexGeometry.HXtoHP(FUnit.FUnitHS.Xhx, yhx6, ref X1, ref Y1);
                HexGeometry.HXtoHP(FTargetX, FTargetY, ref X2, ref Y2);
                EffectPaths.MakeLightningPath(X1 + 16, Y1, X2 + 16, Y2, ref Effect.FPath);
                var count20 = Effect.FPath.Count;
                Effect.FPath[count20 - 1].Z = 16;
                I = 3;
                Used = new DelphiList();
                Used.Add(FUnit);
                HS = (TacticalBattleUnitHexSprite)CombatMap.Current.MapContainer.MapLevels[0].GetField(FTargetX, FTargetY).FindOwnedChild(2228484);
                if (HS != null)
                {
                    Used.Add(HS.FUnit);
                }

                Target = FindTarget(FTargetX, FTargetY);
                while (Target != null && I > 0)
                {
                    Used.Add(Target);
                    X1 = X2 + 16;
                    Y1 = Y2 + 16;
                    var yhx4 = Target.FUnitHS.Yhx;
                    HexGeometry.HXtoHP(Target.FUnitHS.Xhx, yhx4, ref X2, ref Y2);
                    EffectPaths.MakeLightningPath(X1, Y1, X2 + 16, Y2, ref Effect.FPath);
                    I -= 1;
                    var yhx5 = Target.FUnitHS.Yhx;
                    Target = FindTarget(Target.FUnitHS.Xhx, yhx5);
                }

                Used?.Dispose();
                break;
            case BattleEffectType.Swarm:
                var yhx7 = FUnit.FUnitHS.Yhx;
                HexGeometry.HXtoHP(FUnit.FUnitHS.Xhx, yhx7, ref X1, ref Y1);
                var random3 = Random(360);
                var random4 = Random(20);
                var random5 = Random(20);
                var random6 = Random(2);
                var random7 = Random(90);
                var random8 = Random(5);
                EffectPaths.MakeSwarmPath(Random(10) + 10, random8 + 20, random7, random3, random4 + X1 + 6, random5 + Y1 - 10, (SwarmDirection)random6, ref Effect.FPath);
                K = Effect.FPath.Count - 1;
                {
                    I = 0;
                    for (; I <= 2; I += 1)
                    {
                        J = 0;
                        int last = K;
                        for (; J <= last; J += 1)
                        {
                            Point = new Point3D();
                            Point.X = Effect.FPath[J].X;
                            Point.Y = Effect.FPath[J].Y;
                            Effect.FPath.Add(Point);
                        }
                    }
                }

                HexGeometry.HXtoHP(FTargetX, FTargetY, ref X2, ref Y2);
                var count21 = Effect.FPath.Count;
                var path13 = Effect.FPath[count21 - 1];
                var count22 = Effect.FPath.Count;
                EffectPaths.MakeLinePath(Effect.FPath[count22 - 1].X, path13.Y, X2 + 16, Y2, 15, ref Effect.FPath);
                Path = new Point3DList();
                var random9 = Random(360);
                var count23 = Effect.FPath.Count;
                var path14 = Effect.FPath[count23 - 1];
                var count24 = Effect.FPath.Count;
                var path15 = Effect.FPath[count24 - 1];
                var random10 = Random(2);
                var random11 = Random(90);
                var random12 = Random(5);
                EffectPaths.MakeSwarmPath(Random(10) + 10, random12 + 20, random11, random9, path14.X, path15.Y, (SwarmDirection)random10, ref Path);
                var pathItem = Path[0];
                var count25 = Effect.FPath.Count;
                var path16 = Effect.FPath[count25 - 1];
                var count26 = Effect.FPath.Count;
                var path17 = Effect.FPath[count26 - 1];
                var pathX2 = Path[0];
                EffectPaths.MakeLinePath(path17.X, path16.Y, pathX2.X, pathItem.Y, 5, ref Effect.FPath);
                {
                    I = 0;
                    int last = Path.Count - 1;
                    for (; I <= last; I += 1)
                    {
                        // { Original shares the points, then frees Path. }
                        var path18 = Path[I];
                        Effect.FPath.Add(path18);
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
                            Point = new Point3D();
                            Point.X = Path[J].X;
                            Point.Y = Path[J].Y;
                            Effect.FPath.Add(Point);
                        }
                    }
                }

                Path?.Dispose();
                break;
            case BattleEffectType.ScreenSwarm:
                HS = (TacticalBattleUnitHexSprite)CombatMap.Current.MapContainer.MapLevels[0].GetField(FTargetX, FTargetY).FindOwnedChild(2228484);
                if (HS == null)
                {
                    Target = null;
                }
                else
                {
                    Target = HS.FUnit;
                }

                DX = FTargetX;
                DY = FTargetY;
                ChooseTarget();
                if (Target != null)
                {
                    DX = Target.FUnitHS.Xhx;
                    DY = Target.FUnitHS.Yhx;
                }

                Effect.FTarget = Target;
                HexGeometry.HXtoHP(DX, DY, ref X2, ref Y2);
                if (FStrikeIndex == 0)
                {
                    CombatMap.Current.CenterViewTo(FTargetX, FTargetY, 0);
                }

                Point = new Point3D();
                Point.X = X2;
                Point.Y = Y2;
                Effect.FPath.Add(Point);
                var random13 = Random(360);
                var random14 = Random(20);
                var random15 = Random(20);
                var random16 = Random(2);
                var random17 = Random(90);
                var random18 = Random(10);
                EffectPaths.MakeSwarmPath(Random(10) + 10, random18 + 25, random17, random13, random14, random15 - 10, (SwarmDirection)random16, ref Effect.FPath);
                K = Effect.FPath.Count - 1;
                {
                    I = 0;
                    for (; I <= 4; I += 1)
                    {
                        J = 1;
                        int last = K;
                        for (; J <= last; J += 1)
                        {
                            Point = new Point3D();
                            Point.X = Effect.FPath[J].X;
                            Point.Y = Effect.FPath[J].Y;
                            Effect.FPath.Add(Point);
                        }
                    }
                }

                {
                    I = 0;
                    for (; I <= 49; I += 1)
                    {
                        Effect.FPath[I + 1].Z = 100 - I * 2;
                    }
                }

                break;
            case BattleEffectType.FrostBeam:
                HexGeometry.HXtoHP(FTargetX, FTargetY, ref X2, ref Y2);
                Effect.FTargetX = FTargetX;
                Effect.FTargetY = FTargetY;
                CombatMap.Current.CenterViewTo(FTargetX - 3, FTargetY - 3, 0);
                EffectPaths.MakeArcPathXY((short)(X2 - 200), (short)(Y2 - 200), (short)(X2 + 16), (short)(Y2 + 16), 5, 100, ref Effect.FPath);
                break;
            case BattleEffectType.Ray:
                CombatMap.Current.MakeVisible(FTargetX - 3, FTargetY - 3, 0, 3);
                var xhx3 = FUnit.FUnitHS.Xhx;
                var yhx8 = FUnit.FUnitHS.Yhx;
                TacticalTables.MakeRangedPath(FUnit.GetSide(), 10, 0, 50, xhx3, yhx8, (short)FTargetX, (short)FTargetY, 0, 0, true, ref Effect.FPath, ref Obstacles);
                break;
            default:
                var xhx4 = FUnit.FUnitHS.Xhx;
                var yhx9 = FUnit.FUnitHS.Yhx;
                TacticalTables.MakeAbilPath(FUnit.GetSide(), (short)FAbilityID, (short)FSpellID, xhx4, yhx9, (short)FTargetX, (short)FTargetY, ref Effect.FPath, ref Obstacles);
                break;
        }

        Effect.FAbilityIndex = FAbilityID;
        Effect.FSpellIndex = FSpellID;
        Effect.FEffectType = FEffectType;
        switch (FEffectType)
        {
            case BattleEffectType.Arrow:
            case BattleEffectType.Rock:
            case BattleEffectType.Comet:
            case BattleEffectType.Breath:
            case BattleEffectType.CometBall:
            case BattleEffectType.Ray:
            case BattleEffectType.Swarm:
            case BattleEffectType.AltArrow:
                HS = (TacticalBattleUnitHexSprite)CombatMap.Current.MapContainer.MapLevels[0].GetField(FTargetX, FTargetY).FindOwnedChild(2228484);
                if (HS != null)
                {
                    Effect.FTarget = HS.FUnit;
                }

                I = 0;
                while (I < Obstacles.Count)
                {
                    Obstacle = (RangedObstacle)Obstacles[I];
                    if (AoWHSMap.Current.Random(100) < Obstacle.Chance)
                    {
                        var path = Effect.FPath[Obstacle.PathIndex];
                        HexGeometry.HPtoHX(Effect.FPath[Obstacle.PathIndex].X, path.Y, ref J, ref K);
                        if (Effect.FEffectType == BattleEffectType.Breath)
                        {
                            HitObject = null;
                        }
                        else
                        {
                            HitObject = (HexSprite)CombatMap.Current.MapContainer.MapLevels[0].GetField(J, K).FindOwnedChild(2228484);
                        }

                        if (HitObject == null)
                        {
                            HitObject = (HexSprite)CombatMap.Current.MapContainer.MapLevels[0].GetField(J, K).FindOwnedChild(2228582);
                        }

                        if (HitObject == null)
                        {
                            HitObject = (HexSprite)CombatMap.Current.MapContainer.MapLevels[0].GetField(J, K).FindOwnedChild(2229508);
                        }

                        if (HitObject == null)
                        {
                            HitObject = (HexSprite)CombatMap.Current.MapContainer.MapLevels[0].GetField(J, K).FindOwnedChild(2229510);
                        }

                        if (HitObject != null || CombatMap.Current.MapContainer.MapLevels[0].GetField(J, K).TerrainType == 15 || CombatMap.Current.MapContainer.MapLevels[0].GetField(J, K).TerrainType == 7 || CombatMap.Current.MapContainer.MapLevels[0].GetField(J, K).OverlayType == 8)
                        {
                            J = Obstacle.PathIndex + 1;
                            int last = Effect.FPath.Count - 1;
                            for (; J <= last; J += 1)
                            {
                                var count = Effect.FPath.Count;
                                Effect.FPath.Delete(count - 1);
                            }

                            if (!(HitObject is TacticalBattleUnitHexSprite))
                            {
                                Effect.FTarget = null;
                                if (Effect.FEffectType == BattleEffectType.Breath)
                                {
                                    Effect.FTargetX = 0;
                                }
                            }
                            else
                            {
                                Effect.FTarget = ((TacticalBattleUnitHexSprite)HitObject).FUnit;
                            }

                            I = Obstacles.Count;
                        }
                    }

                    I += 1;
                }

                break;
        }

        if (FEffectType == BattleEffectType.Breath)
        {
            Effect.FTarget = null;
        }
        else
        {
            switch (FAbilityID)
            {
                case 31:
                    Effect.FTarget = null;
                    break;
                case 52:
                    switch (FSpellID)
                    {
                        case 104:
                        case 107:
                        case 113:
                        case 116:
                        case 117:
                        case 127:
                        case 129:
                        case 130:
                            Effect.FTarget = null;
                            break;
                        case 102:
                        case 106:
                        case 120:
                        case 128:
                            Effect.FTarget = FUnit;
                            break;
                        case 115:
                            if (AoWHSMap.Current.Random(100) < 75)
                            {
                                Effect.FTarget = null;
                            }

                            break;
                        case 100:
                            ExtraEffect = CombatMap.Current.AddEffect(BattleEffectType.SFOverlay);
                            var yhx = FUnit.FUnitHS.Yhx;
                            HexGeometry.HXtoHP(FUnit.FUnitHS.Xhx, yhx, ref X1, ref Y1);
                            while (ExtraEffect.FPath.Count < Effect.FPath.Count)
                            {
                                Point = new Point3D();
                                Point.X = X1 + 16;
                                Point.Y = Y1;
                                ExtraEffect.FPath.Add(Point);
                            }

                            ExtraEffect.FAbilityIndex = FAbilityID;
                            ExtraEffect.FSpellIndex = FSpellID;
                            break;
                    }

                    break;
            }
        }

        var yhx10 = FUnit.FUnitHS.Yhx;
        var xhx5 = FUnit.FUnitHS.Xhx;
        Effect.FVariant = GetRangedDirIndex(FTargetX - xhx5, FTargetY - yhx10);
        if (FStrikeIndex == 0)
        {
            switch (FEffectType)
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
                    switch (Effect.FVariant)
                    {
                        case 0:
                        case 1:
                        case 23:
                            FUnit.FUnitHS.SetDirection(1);
                            break;
                        case >= 2 and <= 6:
                            FUnit.FUnitHS.SetDirection(2);
                            break;
                        case >= 7 and <= 10:
                            FUnit.FUnitHS.SetDirection(3);
                            break;
                        case >= 11 and <= 13:
                            FUnit.FUnitHS.SetDirection(4);
                            break;
                        case >= 14 and <= 17:
                            FUnit.FUnitHS.SetDirection(5);
                            break;
                        case >= 18 and <= 22:
                            FUnit.FUnitHS.SetDirection(6);
                            break;
                    }

                    break;
            }
        }

        if (Set<BattleEffectType>.Of(BattleEffectType.Rock, BattleEffectType.Comet, BattleEffectType.Breath, BattleEffectType.CometBall, BattleEffectType.ScreenNS).Contains(FEffectType))
        {
            Effect.FVariant = 0;
        }

        if (FEffectType == BattleEffectType.Breath && FStrikeIndex == 0)
        {
            FUnit.FUnitHS.SetAction(TacticalBattleUnitHexSprite.SpriteAction.BreathAttack);
        }
        else
        {
            if ((FAbilityID != 52 && TacticalTables.AbilTypes[FAbilityID] != BattleEffectType.Breath) || FStrikeIndex == 0)
            {
                FUnit.FUnitHS.SetAction(TacticalBattleUnitHexSprite.SpriteAction.Ability);
            }
        }

        I = 0;
        int limit = Obstacles.Count - 1;
        for (; I <= limit; I += 1)
        {
            (Obstacles[I] as IDisposable)?.Dispose();
        }

        Obstacles?.Dispose();
        FStrikeIndex += 1;
    }

    private static Int32Array19 rndHex = Int32Array19.From([6, 0, 1, 3, 4, 5, 2, 18, 12, 7, 11, 10, 17, 15, 9, 16, 13, 8, 14]);
    private static Int32Array12 BreathHit = Int32Array12.From([0, 1, 4, 7, 8, 2, 5, 9, 10, 3, 6, 11]);

    private static ushort GetRangedDirIndex(int DX, int DY)
    {
        ushort result;
        if (DX == 0)
        {
            result = 0;
        }
        else
        {
            int ratio = Math.Abs(DY * 100 / DX);
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

        if (DY > 0)
        {
            result = (ushort)(12 - result);
        }

        if (DX < 0 && result > 0)
        {
            result = (ushort)(24 - result);
        }

        return result;
    }

    // NetworkE.pas:230

    // NetworkE.pas:230

}
