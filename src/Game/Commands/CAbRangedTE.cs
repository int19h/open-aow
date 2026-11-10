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

public partial class CAbRangedTE : TokenEvent
{
    public CAbRangedTE(ECustomNode owner) : base(owner)
    { }

    // CombatTE.pas:917
    public new const int ClassID = 2236419;

    public TacticalCombatUnit FUnit;
    public int FAbilityID;
    public int FSpellID;
    public ushort FSourceX;
    public ushort FSourceY;
    public ushort FTargetX;
    public ushort FTargetY;
    public TfxType FEffectType;
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
    public override ExecuteResult Execute(TokenExecuter TE)
    {
        int I = default;
        int X = default;
        int Y = default;
        int EffectIndex = default;
        TfxHS Effect = default;
        TfxHS ExtraEffect = default;
        int EndX = default;
        int EndY = default;
        SpellCombatInfo Info = default;
        TacticalCombatUnitHS HS;
        COozeHS Ooze = default;
        CombatAction Action = default;
        MapFieldMsg Msg = default;
        base.Execute(TE);
        ExecuteResult Result = ExecuteResult.Retry;
        try
        {
            switch (FState)
            {
                case 0:
                    try
                    {
                        HS = (TacticalCombatUnitHS)AoWCombatMap.Current.MapContainer.MapLevels[0].GetField(FSourceX, FSourceY).FindOwnedChild(2228484);
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

                            bool notExplorationEnabled = !AoWCombatMap.Current.FExplorationEnabled;
                            if (!notExplorationEnabled)
                            {
                                var yhx = FUnit.FUnitHS.Yhx;
                                var xhx = FUnit.FUnitHS.Xhx;
                                notExplorationEnabled = ((AoWMapField)AoWCombatMap.Current.MapContainer.GetField(xhx, yhx, 0)).GetExplored(AoWCombatMap.Current.FSeatedPlayer);
                            }

                            if (notExplorationEnabled)
                            {
                                var yhx = FUnit.FUnitHS.Yhx;
                                var xhx = FUnit.FUnitHS.Xhx;
                                AoWCombatMap.Current.MakeVisible(xhx, yhx, 0, 2);
                            }

                            switch (FEffectType)
                            {
                                case TfxType.Breath:
                                    FStrikeCount = 20;
                                    break;
                                case TfxType.ScreenNESW:
                                    FStrikeCount = 10;
                                    break;
                                case TfxType.ScreenNS:
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
                                case TfxType.ShockWave:
                                    FTargetX = FSourceX;
                                    FTargetY = FSourceY;
                                    FStrikeCount = 1;
                                    break;
                                case TfxType.Touch:
                                    FStrikeCount = 1;
                                    break;
                                case TfxType.Swarm:
                                case TfxType.ScreenSwarm:
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
                                case TfxType.Tremors:
                                    FSound = AoWHSSet.Current.FSpellControl.GetSpell(FSpellID).FCastSounds.PlayEx(0, 64, true, 0, SamplePanning.Center, null);
                                    NextStrike();
                                    break;
                                case TfxType.ScreenSwarm:
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
                        while (EffectIndex < AoWCombatMap.Current.FEffects.Count)
                        {
                            EndX = -1;
                            EndY = -1;
                            Effect = (TfxHS)AoWCombatMap.Current.FEffects[EffectIndex];
                            if (Effect.FAbilityIndex == 52 && Effect.FSpellIndex == 117)
                            {
                                if (Effect.FEffectType == TfxType.Rock)
                                {
                                    if (Effect.FDone)
                                    {
                                        Effect.FEffectType = TfxType.FBExplosion;
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
                                                    AoWCombatMap.Current.MapContainer.MapLevels[0].GetField(X, Y).SendMapFieldMsg(ref Msg);
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
                                        HS = (TacticalCombatUnitHS)AoWCombatMap.Current.MapContainer.MapLevels[0].GetField(X, Y).FindOwnedChild(2228484);
                                        if (HS != null)
                                        {
                                            RandSeed = AoWHSMap.Current.Random(16777215);
                                            Effect.FCombatAction = AoWHSSet.Current.FSpellControl.GetSpell(Effect.FSpellIndex).CreateCA(FUnit, HS.FUnit, false);
                                            var yhx = HS.Yhx;
                                            HexGeometry.HXtoHP(HS.Xhx, yhx, ref I, ref X);
                                            if (((DamageCA)Effect.FCombatAction).FDamageTypes.Value - ((DamageCA)Effect.FCombatAction).FEffects.Value != Set<DamageType>.Of())
                                            {
                                                CombatEvents.MakeDEHSList(I, X, ((DamageCA)Effect.FCombatAction).FDamageTypes.Value - ((DamageCA)Effect.FCombatAction).FEffects.Value, false);
                                            }

                                            if (((DamageCA)Effect.FCombatAction).FEffects.Value != Set<DamageType>.Of())
                                            {
                                                CombatEvents.MakeDEHSList(I, X, ((DamageCA)Effect.FCombatAction).FEffects.Value, true);
                                            }

                                            AoWCombatMap.Current.FCombat.ExecuteCombatAction(Effect.FCombatAction);
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
                                                            AoWCombatMap.Current.MapContainer.MapLevels[0].GetField(I, X).SendMapFieldMsg(ref Msg);
                                                        }
                                                    }
                                                }

                                                break;
                                        }
                                    }
                                    else
                                    {
                                        if (Effect.FAbilityIndex == 52 && Effect.FEffectType == TfxType.Resurrect && (Effect.FSpellIndex == 130 || Effect.FSpellIndex == 129))
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
                                                            AoWCombatMap.Current.MapContainer.MapLevels[0].GetField(X, Y).SendMapFieldMsg(ref Msg);
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
                                                            AoWCombatMap.Current.MapContainer.MapLevels[0].GetField(X, Y).SendMapFieldMsg(ref Msg);
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
                                                                AoWCombatMap.Current.MapContainer.MapLevels[0].GetField(X, Y).SendMapFieldMsg(ref Msg);
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
                                                                    AoWCombatMap.Current.MapContainer.MapLevels[0].GetField(X, Y).SendMapFieldMsg(ref Msg);
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
                                                                    AoWCombatMap.Current.MapContainer.MapLevels[0].GetField(X, Y).SendMapFieldMsg(ref Msg);
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
                                                                    AoWCombatMap.Current.MapContainer.MapLevels[0].GetField(X, Y).SendMapFieldMsg(ref Msg);
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
                                                                AoWCombatMap.Current.MapContainer.MapLevels[0].GetField(xhx, fUnitHSYhx).SendMapFieldMsg(ref Msg);
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
                                                                        AoWCombatMap.Current.MapContainer.MapLevels[0].GetField(X, Y).SendMapFieldMsg(ref Msg);
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
                                                            int last = AoWCombatMap.Current.FCombat.FCombatData.GetCount() - 1;
                                                            for (; I <= last; I += 1)
                                                            {
                                                                if (AoWCombatMap.Current.FCombat.FCombatData.GetObjects(I) != null && AoWCombatMap.Current.FCombat.FCombatData.GetObjects(I) is CombatUnit && AoWCombatMap.Current.FCombat.FCombatData.GetObjects(I).GetSide() != FUnit.GetSide() && AoWCombatMap.Current.FCombat.FCombatData.GetObjects(I).GetEnabled() && ((TacticalCombatUnit)AoWCombatMap.Current.FCombat.FCombatData.GetObjects(I)).FUnitHS != null)
                                                                {
                                                                    RandSeed = AoWHSMap.Current.Random(16777215);
                                                                    var combatObject = AoWCombatMap.Current.FCombat.FCombatData.GetObjects(I);
                                                                    Action = AoWHSSet.Current.FSpellControl.GetSpell(FSpellID).CreateCA(FUnit, combatObject, false);
                                                                    if (((ExclusiveCombatSpellCA)Action).FSucceeded)
                                                                    {
                                                                        ((TacticalCombatUnit)AoWCombatMap.Current.FCombat.FCombatData.GetObjects(I)).FUnitHS.SetAction(TacticalCombatUnitHS.SpriteAction.Hit);
                                                                    }

                                                                    AoWCombatMap.Current.FCombat.ExecuteCombatAction(Action);
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
                                                                                AoWCombatMap.Current.MapContainer.MapLevels[0].GetField(X, Y).SendMapFieldMsg(ref Msg);
                                                                            }
                                                                        }

                                                                        break;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                if (Effect.FEffectType == TfxType.SacredWrath && Effect.FAbilityIndex == 52 && Effect.FSpellIndex == 125)
                                                                {
                                                                    switch (Effect.FFrame)
                                                                    {
                                                                        case 5:
                                                                            {
                                                                                I = 0;
                                                                                int last = AoWCombatMap.Current.FCombat.FCombatData.GetCount() - 1;
                                                                                for (; I <= last; I += 1)
                                                                                {
                                                                                    if (AoWCombatMap.Current.FCombat.FCombatData.GetObjects(I) is CombatUnit && AoWCombatMap.Current.FCombat.FCombatData.GetObjects(I).GetEnabled())
                                                                                    {
                                                                                        HS = ((TacticalCombatUnit)AoWCombatMap.Current.FCombat.FCombatData.GetObjects(I)).FUnitHS;
                                                                                        if (HS != null)
                                                                                        {
                                                                                            ExtraEffect = AoWCombatMap.Current.AddEffect(TfxType.SWComet);
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

                                                                        if (Effect.FCombatAction is DamageCA)
                                                                        {
                                                                            I = ((DamageCA)Effect.FCombatAction).FDamage;
                                                                            var yhx = ((TacticalCombatUnit)Effect.FTarget).FUnitHS.Yhx;
                                                                            HexGeometry.HXtoHP(((TacticalCombatUnit)Effect.FTarget).FUnitHS.Xhx, yhx, ref X, ref Y);
                                                                            if (((DamageCA)Effect.FCombatAction).FDamageTypes.Value - ((DamageCA)Effect.FCombatAction).FEffects.Value != Set<DamageType>.Of())
                                                                            {
                                                                                CombatEvents.MakeDEHSList(X, Y, ((DamageCA)Effect.FCombatAction).FDamageTypes.Value - ((DamageCA)Effect.FCombatAction).FEffects.Value, false);
                                                                            }

                                                                            if (((DamageCA)Effect.FCombatAction).FEffects.Value != Set<DamageType>.Of())
                                                                            {
                                                                                CombatEvents.MakeDEHSList(X, Y, ((DamageCA)Effect.FCombatAction).FEffects.Value, true);
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            if (Effect.FCombatAction is EntangleSpellCA)
                                                                            {
                                                                                if (((ExclusiveCombatSpellCA)Effect.FCombatAction).FSucceeded)
                                                                                {
                                                                                    ((TacticalCombatUnit)Effect.FTarget).FUnitHS.SetAction(TacticalCombatUnitHS.SpriteAction.Hit);
                                                                                }
                                                                            }
                                                                            else
                                                                            {
                                                                                if (Effect.FCombatAction is MindDecayCA && ((ExclusiveCombatSpellCA)Effect.FCombatAction).FSucceeded)
                                                                                {
                                                                                    ((TacticalCombatUnit)Effect.FTarget).FUnitHS.SetAction(TacticalCombatUnitHS.SpriteAction.Hit);
                                                                                }
                                                                            }
                                                                        }

                                                                        if (I > 0)
                                                                        {
                                                                            ((TacticalCombatUnit)Effect.FTarget).FUnitHS.SetAction(TacticalCombatUnitHS.SpriteAction.Hit);
                                                                            TacticalCombatUnit.MakeHitBlood((TacticalCombatUnit)Effect.FTarget, I);
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
                                                                            AoWCombatMap.Current.FCombat.ExecuteCombatAction(Effect.FCombatAction);
                                                                            Effect.FCombatAction.Release();
                                                                            Effect.FCombatAction = null;
                                                                            if (!Effect.FTarget.GetEnabled())
                                                                            {
                                                                                I = 0;
                                                                                int last = AoWCombatMap.Current.FEffects.Count - 1;
                                                                                for (; I <= last; I += 1)
                                                                                {
                                                                                    if (I != EffectIndex)
                                                                                    {
                                                                                        if (((TfxHS)AoWCombatMap.Current.FEffects[I]).FTarget == Effect.FTarget)
                                                                                        {
                                                                                            ((TfxHS)AoWCombatMap.Current.FEffects[I]).FTarget = null;
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
                                                                                            AoWCombatMap.Current.MapContainer.MapLevels[0].GetField(Effect.FTargetX, Effect.FTargetY).SendMapFieldMsg(ref Msg);
                                                                                        }

                                                                                        break;
                                                                                    case 24:
                                                                                        Msg.MsgID = 200705;
                                                                                        Msg.Param[0] = FUnit;
                                                                                        Msg.Result = -1;
                                                                                        AoWCombatMap.Current.MapContainer.MapLevels[0].GetField(EndX, EndY).SendMapFieldMsg(ref Msg);
                                                                                        break;
                                                                                    case 44:
                                                                                        Msg.MsgID = 200706;
                                                                                        Msg.Param[0] = FUnit;
                                                                                        Msg.Result = -1;
                                                                                        AoWCombatMap.Current.MapContainer.MapLevels[0].GetField(EndX, EndY).SendMapFieldMsg(ref Msg);
                                                                                        break;
                                                                                    case 31:
                                                                                        Msg.MsgID = 200714;
                                                                                        Msg.Param[0] = FUnit;
                                                                                        Msg.Result = -1;
                                                                                        AoWCombatMap.Current.MapContainer.MapLevels[0].GetField(EndX, EndY).SendMapFieldMsg(ref Msg);
                                                                                        EffectPaths.CreateFire(false, 100, EndX, EndY);
                                                                                        break;
                                                                                    case 52:
                                                                                        switch (Effect.FEffectType)
                                                                                        {
                                                                                            case TfxType.Breath:
                                                                                                if (Effect.FTargetX > 0)
                                                                                                {
                                                                                                    Msg.MsgID = 200723;
                                                                                                    Msg.Param[0] = FUnit;
                                                                                                    Msg.Param[1] = Effect.FSpellIndex;
                                                                                                    Msg.Result = -1;
                                                                                                    AoWCombatMap.Current.MapContainer.MapLevels[0].GetField(Effect.FTargetX, Effect.FTargetY).SendMapFieldMsg(ref Msg);
                                                                                                }

                                                                                                break;
                                                                                            case TfxType.ScreenNS:
                                                                                                switch (Effect.FSpellIndex)
                                                                                                {
                                                                                                    case 116:
                                                                                                        Msg.MsgID = 200710;
                                                                                                        Msg.Param[0] = FUnit;
                                                                                                        Msg.Result = -1;
                                                                                                        AoWCombatMap.Current.MapContainer.MapLevels[0].GetField(EndX, EndY).SendMapFieldMsg(ref Msg);
                                                                                                        break;
                                                                                                    case 124:
                                                                                                        if (AoWCombatMap.Current.MapContainer.MapLevels[0].GetField(EndX, EndY).TerrainType != 0 && AoWCombatMap.Current.MapContainer.MapLevels[0].GetField(EndX, EndY).TerrainType != 10 && AoWCombatMap.Current.MapContainer.MapLevels[0].GetField(EndX, EndY).OverlayType != 2 && AoWCombatMap.Current.MapContainer.MapLevels[0].GetField(EndX, EndY).OverlayType != 11 && AoWCombatMap.Current.MapContainer.MapLevels[0].GetField(EndX, EndY).OverlayType != 8 && AoWCombatMap.Current.MapContainer.MapLevels[0].GetField(EndX, EndY).TerrainType != 9 && AoWCombatMap.Current.MapContainer.MapLevels[0].GetField(EndX, EndY).TerrainType != 15 && AoWCombatMap.Current.MapContainer.MapLevels[0].GetField(EndX, EndY).TerrainType != 7)
                                                                                                        {
                                                                                                            Ooze = new COozeHS(null);
                                                                                                            AoWCombatMap.Current.PlaceHX(Ooze, EndX, EndY, 0);
                                                                                                        }

                                                                                                        break;
                                                                                                }

                                                                                                break;
                                                                                            case TfxType.Effect:
                                                                                                switch (Effect.FSpellIndex)
                                                                                                {
                                                                                                    case 113:
                                                                                                        Msg.MsgID = 200715;
                                                                                                        Msg.Param[0] = FUnit;
                                                                                                        Msg.Result = -1;
                                                                                                        AoWCombatMap.Current.MapContainer.MapLevels[0].GetField(EndX, EndY).SendMapFieldMsg(ref Msg);
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

                        if (AoWCombatMap.Current.FEffects.Count == 0 && (FStrikeIndex >= FStrikeCount || !FUnit.GetEnabled()))
                        {
                            if (FUnit.GetEnabled())
                            {
                                FUnit.FUnitHS.SetAction(TacticalCombatUnitHS.SpriteAction.Move);
                            }

                            Result = ExecuteResult.Done;
                        }

                        if (FUnit.GetEnabled())
                        {
                            if (FEffectType == TfxType.Breath)
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
                    if (FUnit.FCombatPlayer == AoWCombatMap.Current.FCurrentCombatPlayer)
                    {
                        var xhx = FUnit.FUnitHS.Xhx;
                        AoWCombatMap.Current.FPlayers.GetPlayers(AoWCombatMap.Current.FCurrentCombatPlayer).FLastX = xhx;
                        var yhx = FUnit.FUnitHS.Yhx;
                        AoWCombatMap.Current.FPlayers.GetPlayers(AoWCombatMap.Current.FCurrentCombatPlayer).FLastY = yhx;
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
        TacticalCombatUnit Target;
        TfxHS Effect;
        TfxHS ExtraEffect;
        TacticalCombatUnitHS HS;
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
        XYZ Point;
        HexagonSprite HitObject;
        XYZList Path;
        DelphiList Obstacles;
        ROb Obstacle;
        bool Found;
        TacticalCombatUnit FindTarget(int X, int Y)
        {
            int Distance;
            int BestDistance;
            bool Available;
            TacticalCombatUnit Result = null;
            BestDistance = 6;
            for (int I = 0, last = AoWCombatMap.Current.FCombat.FCombatData.GetCount() - 1; I <= last; I++)
            {
                if (AoWCombatMap.Current.FCombat.FCombatData.GetObjects(I) is CombatUnit && AoWCombatMap.Current.FCombat.FCombatData.GetObjects(I).GetEnabled() && AoWCombatMap.Current.FCombat.FCombatData.GetObjects(I).GetSide() != FUnit.GetSide())
                {
                    Available = true;
                    for (int J = 0, lastJ = Used.Count - 1; J <= lastJ; J++)
                    {
                        if (AoWCombatMap.Current.FCombat.FCombatData.GetObjects(I) == Used[J])
                        {
                            Available = false;
                        }
                    }

                    if (Available)
                    {
                        var yhx = ((TacticalCombatUnit)AoWCombatMap.Current.FCombat.FCombatData.GetObjects(I)).FUnitHS.Yhx;
                        Distance = HexGeometry.dHXtoRad(X, Y, ((TacticalCombatUnit)AoWCombatMap.Current.FCombat.FCombatData.GetObjects(I)).FUnitHS.Xhx, yhx);
                        if (Distance < BestDistance)
                        {
                            Result = (TacticalCombatUnit)AoWCombatMap.Current.FCombat.FCombatData.GetObjects(I);
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
            int last = AoWCombatMap.Current.FCombat.FCombatData.GetCount() - 1;
            for (; I <= last; I += 1)
            {
                if (AoWCombatMap.Current.FCombat.FCombatData.GetObjects(I) is CombatUnit && AoWCombatMap.Current.FCombat.FCombatData.GetObjects(I).GetEnabled() && AoWCombatMap.Current.FCombat.FCombatData.GetObjects(I) != Target)
                {
                    var yhx = ((TacticalCombatUnit)AoWCombatMap.Current.FCombat.FCombatData.GetObjects(I)).FUnitHS.Yhx;
                    var xhx = ((TacticalCombatUnit)AoWCombatMap.Current.FCombat.FCombatData.GetObjects(I)).FUnitHS.Xhx;
                    Distance = HexGeometry.dHXtoRad(FTargetX, FTargetY, xhx, yhx);
                    if (Distance < 13)
                    {
                        Total = Total + (13 - Distance);
                        var combatObject = AoWCombatMap.Current.FCombat.FCombatData.GetObjects(I);
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

                Target = (TacticalCombatUnit)Used[Index];
            }

            Used?.Dispose();
            Weights?.Dispose();
        }

        if (FStrikeIndex >= FStrikeCount)
        {
            return;
        }

        Effect = AoWCombatMap.Current.AddEffect(TfxType.None);
        Obstacles = new DelphiList();
        switch (FEffectType)
        {
            case TfxType.Breath:
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
            case TfxType.Touch:
            case TfxType.Effect:
            case TfxType.SacredWrath:
            case TfxType.Geyser:
            case TfxType.Cloud:
            case TfxType.MindDecay:
            case TfxType.ShockWave:
            case TfxType.SacFlame:
                if (FEffectType == TfxType.SacredWrath)
                {
                    FTargetX = 32;
                    FTargetY = 29;
                }

                HexGeometry.HXtoHP(FTargetX, FTargetY, ref X2, ref Y2);
                switch (FEffectType)
                {
                    case TfxType.Geyser:
                        AoWCombatMap.Current.CenterViewTo(FTargetX, FTargetY - 5, 0);
                        break;
                    case TfxType.MindDecay:
                        Effect.FTargetX = FTargetX;
                        Effect.FTargetY = FTargetY;
                        AoWCombatMap.Current.CenterViewTo(FTargetX + 1, FTargetY - 4, 0);
                        break;
                    case TfxType.ShockWave:
                        var yhx4 = FUnit.FUnitHS.Yhx;
                        var fUnitHSXhx = FUnit.FUnitHS.Xhx;
                        AoWCombatMap.Current.CenterViewTo(fUnitHSXhx, yhx4, 0);
                        break;
                    case TfxType.SacFlame:
                        var yhx5 = FUnit.FUnitHS.Yhx;
                        var xhx2 = FUnit.FUnitHS.Xhx;
                        AoWCombatMap.Current.CenterViewTo(xhx2, yhx5 - 3, 0);
                        break;
                    default:
                        AoWCombatMap.Current.CenterViewTo(FTargetX, FTargetY, 0);
                        break;
                }

                Point = new XYZ();
                Point.X = X2 + 16;
                Point.Y = Y2 + 16;
                Effect.FPath.Add(Point);
                HS = (TacticalCombatUnitHS)AoWCombatMap.Current.MapContainer.MapLevels[0].GetField(FTargetX, FTargetY).FindOwnedChild(2228484);
                if (HS != null)
                {
                    Effect.FTarget = HS.FUnit;
                }

                break;
            case TfxType.Winds:
                HexGeometry.HXtoHP(FTargetX, FTargetY, ref X2, ref Y2);
                Point = new XYZ();
                Point.X = X2 + 16;
                Point.Y = Y2 + 16;
                Effect.FPath.Add(Point);
                HS = (TacticalCombatUnitHS)AoWCombatMap.Current.MapContainer.MapLevels[0].GetField(FTargetX, FTargetY).FindOwnedChild(2228484);
                if (HS != null)
                {
                    Effect.FTarget = HS.FUnit;
                }

                I = AoWHSMap.Current.Random(37) + 1;
                Found = false;
                while (!Found)
                {
                    HexGeometry.CenterHNtoHX(FTargetX, FTargetY, I, ref J, ref K);
                    bool condition = J > 5 && J < 60 && K > 5 && K < 53 && AoWCombatMap.Current.MapContainer.MapLevels[0].GetField(J, K).FindOwnedChild(2228484) == null;
                    if (condition)
                    {
                        bool test = AoWCombatMap.Current.MapContainer.MapLevels[0].GetField(J, K).OverlayType != 8;
                        if (!test)
                        {
                            var fieldValue = AoWCombatMap.Current.MapContainer.MapLevels[0].GetField(J, K);
                            var fieldValue2 = AoWCombatMap.Current.MapContainer.MapLevels[0].GetField(FTargetX, FTargetY);
                            test = fieldValue.OverlayType == fieldValue2.OverlayType;
                        }

                        condition = test;
                    }

                    bool passed = condition && AoWCombatMap.Current.MapContainer.MapLevels[0].GetField(J, K).TerrainType != 15 && AoWCombatMap.Current.MapContainer.MapLevels[0].GetField(J, K).TerrainType != 7;
                    if (passed)
                    {
                        bool test = AoWCombatMap.Current.MapContainer.MapLevels[0].GetField(J, K).TerrainType != 0 && AoWCombatMap.Current.MapContainer.MapLevels[0].GetField(J, K).TerrainType != 10 && AoWCombatMap.Current.MapContainer.MapLevels[0].GetField(J, K).TerrainType != 9;
                        if (!test)
                        {
                            var fieldValue = AoWCombatMap.Current.MapContainer.MapLevels[0].GetField(J, K);
                            var fieldValue2 = AoWCombatMap.Current.MapContainer.MapLevels[0].GetField(FTargetX, FTargetY);
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
                AoWCombatMap.Current.CenterViewTo(FTargetX, FTargetY, 0);
                {
                    I = 0;
                    for (; I <= 29; I += 1)
                    {
                        var xyz = new XYZ();
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
            case TfxType.ScreenNESW:
                Point = new XYZ();
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
            case TfxType.ScreenNS:
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
                AoWCombatMap.Current.MakeVisible(DX, DY, 0, 5);
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
                            HS = (TacticalCombatUnitHS)AoWCombatMap.Current.MapContainer.MapLevels[0].GetField(DX, DY).FindOwnedChild(2228484);
                            if (HS != null)
                            {
                                Effect.FTarget = HS.FUnit;
                            }
                        }

                        break;
                }

                break;
            case TfxType.Chain:
                var yhx6 = FUnit.FUnitHS.Yhx;
                HexGeometry.HXtoHP(FUnit.FUnitHS.Xhx, yhx6, ref X1, ref Y1);
                HexGeometry.HXtoHP(FTargetX, FTargetY, ref X2, ref Y2);
                EffectPaths.MakeLightningPath(X1 + 16, Y1, X2 + 16, Y2, ref Effect.FPath);
                var count20 = Effect.FPath.Count;
                Effect.FPath[count20 - 1].Z = 16;
                I = 3;
                Used = new DelphiList();
                Used.Add(FUnit);
                HS = (TacticalCombatUnitHS)AoWCombatMap.Current.MapContainer.MapLevels[0].GetField(FTargetX, FTargetY).FindOwnedChild(2228484);
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
            case TfxType.Swarm:
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
                            Point = new XYZ();
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
                Path = new XYZList();
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
                            Point = new XYZ();
                            Point.X = Path[J].X;
                            Point.Y = Path[J].Y;
                            Effect.FPath.Add(Point);
                        }
                    }
                }

                Path?.Dispose();
                break;
            case TfxType.ScreenSwarm:
                HS = (TacticalCombatUnitHS)AoWCombatMap.Current.MapContainer.MapLevels[0].GetField(FTargetX, FTargetY).FindOwnedChild(2228484);
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
                    AoWCombatMap.Current.CenterViewTo(FTargetX, FTargetY, 0);
                }

                Point = new XYZ();
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
                            Point = new XYZ();
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
            case TfxType.FrostBeam:
                HexGeometry.HXtoHP(FTargetX, FTargetY, ref X2, ref Y2);
                Effect.FTargetX = FTargetX;
                Effect.FTargetY = FTargetY;
                AoWCombatMap.Current.CenterViewTo(FTargetX - 3, FTargetY - 3, 0);
                EffectPaths.MakeArcPathXY((short)(X2 - 200), (short)(Y2 - 200), (short)(X2 + 16), (short)(Y2 + 16), 5, 100, ref Effect.FPath);
                break;
            case TfxType.Ray:
                AoWCombatMap.Current.MakeVisible(FTargetX - 3, FTargetY - 3, 0, 3);
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
            case TfxType.Arrow:
            case TfxType.Rock:
            case TfxType.Comet:
            case TfxType.Breath:
            case TfxType.CometBall:
            case TfxType.Ray:
            case TfxType.Swarm:
            case TfxType.AltArrow:
                HS = (TacticalCombatUnitHS)AoWCombatMap.Current.MapContainer.MapLevels[0].GetField(FTargetX, FTargetY).FindOwnedChild(2228484);
                if (HS != null)
                {
                    Effect.FTarget = HS.FUnit;
                }

                I = 0;
                while (I < Obstacles.Count)
                {
                    Obstacle = (ROb)Obstacles[I];
                    if (AoWHSMap.Current.Random(100) < Obstacle.Chance)
                    {
                        var path = Effect.FPath[Obstacle.PathIndex];
                        HexGeometry.HPtoHX(Effect.FPath[Obstacle.PathIndex].X, path.Y, ref J, ref K);
                        if (Effect.FEffectType == TfxType.Breath)
                        {
                            HitObject = null;
                        }
                        else
                        {
                            HitObject = (HexagonSprite)AoWCombatMap.Current.MapContainer.MapLevels[0].GetField(J, K).FindOwnedChild(2228484);
                        }

                        if (HitObject == null)
                        {
                            HitObject = (HexagonSprite)AoWCombatMap.Current.MapContainer.MapLevels[0].GetField(J, K).FindOwnedChild(2228582);
                        }

                        if (HitObject == null)
                        {
                            HitObject = (HexagonSprite)AoWCombatMap.Current.MapContainer.MapLevels[0].GetField(J, K).FindOwnedChild(2229508);
                        }

                        if (HitObject == null)
                        {
                            HitObject = (HexagonSprite)AoWCombatMap.Current.MapContainer.MapLevels[0].GetField(J, K).FindOwnedChild(2229510);
                        }

                        if (HitObject != null || AoWCombatMap.Current.MapContainer.MapLevels[0].GetField(J, K).TerrainType == 15 || AoWCombatMap.Current.MapContainer.MapLevels[0].GetField(J, K).TerrainType == 7 || AoWCombatMap.Current.MapContainer.MapLevels[0].GetField(J, K).OverlayType == 8)
                        {
                            J = Obstacle.PathIndex + 1;
                            int last = Effect.FPath.Count - 1;
                            for (; J <= last; J += 1)
                            {
                                var count = Effect.FPath.Count;
                                Effect.FPath.Delete(count - 1);
                            }

                            if (!(HitObject is TacticalCombatUnitHS))
                            {
                                Effect.FTarget = null;
                                if (Effect.FEffectType == TfxType.Breath)
                                {
                                    Effect.FTargetX = 0;
                                }
                            }
                            else
                            {
                                Effect.FTarget = ((TacticalCombatUnitHS)HitObject).FUnit;
                            }

                            I = Obstacles.Count;
                        }
                    }

                    I += 1;
                }

                break;
        }

        if (FEffectType == TfxType.Breath)
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
                            ExtraEffect = AoWCombatMap.Current.AddEffect(TfxType.SFOverlay);
                            var yhx = FUnit.FUnitHS.Yhx;
                            HexGeometry.HXtoHP(FUnit.FUnitHS.Xhx, yhx, ref X1, ref Y1);
                            while (ExtraEffect.FPath.Count < Effect.FPath.Count)
                            {
                                Point = new XYZ();
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
                case TfxType.Arrow:
                case TfxType.Rock:
                case TfxType.Comet:
                case TfxType.Breath:
                case TfxType.CometBall:
                case TfxType.Effect:
                case TfxType.Ray:
                case TfxType.Chain:
                case TfxType.Swarm:
                case TfxType.ScreenSwarm:
                case TfxType.SFOverlay:
                case TfxType.Dispel:
                case TfxType.AltArrow:
                case TfxType.Geyser:
                case TfxType.FrostBeam:
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

        if (Set<TfxType>.Of(TfxType.Rock, TfxType.Comet, TfxType.Breath, TfxType.CometBall, TfxType.ScreenNS).Contains(FEffectType))
        {
            Effect.FVariant = 0;
        }

        if (FEffectType == TfxType.Breath && FStrikeIndex == 0)
        {
            FUnit.FUnitHS.SetAction(TacticalCombatUnitHS.SpriteAction.BreathAttack);
        }
        else
        {
            if ((FAbilityID != 52 && TacticalTables.AbilTypes[FAbilityID] != TfxType.Breath) || FStrikeIndex == 0)
            {
                FUnit.FUnitHS.SetAction(TacticalCombatUnitHS.SpriteAction.Ability);
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

    private static FixedArray_int_19 rndHex = FixedArray_int_19.From([6, 0, 1, 3, 4, 5, 2, 18, 12, 7, 11, 10, 17, 15, 9, 16, 13, 8, 14]);
    private static FixedArray_int_12 BreathHit = FixedArray_int_12.From([0, 1, 4, 7, 8, 2, 5, 9, 10, 3, 6, 11]);

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
