// Translated from Storms.pas.
using Aow.Engine.Nodes;
using Aow.Game.AI;
using Aow.Game.Combat;
using Aow.Game.Commands;

using Aow.Game.Draw.Animations;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

// targeting storm, caster
public partial class StormSpell : Spell
{
    public Storm FTargetStorm;
    public Troop FSource;

    // Storms.pas:762
    public StormSpell(ECustomNode owner) : base(owner)
    {
        FSpellType = SpellType.Global;
    }

    // Storms.pas:888
    public override void AIPrefetchCastSpellActions(Troop Source, AICastSpellActionList Actions)
    {
        int Priority;
        AICastSpellAction Action;
        if (AIRequestCastSpellExpense(Source, FBaseManaCost, 0, 0))
        {
            Priority = AICastSpellPriority(Source);
            if (Priority != -1)
            {
                Action = CreateCastSpellAction();
                SetupCastSpellAction(Source, Action);
                Action.FPriority = Priority;
                Action.SetOwner(Actions);
            }
        }
    }

    // Storms.pas:906
    public override bool AIExecuteCastSpellAction(Troop Source)
    {
        bool Result;
        string Reason = "";
        MapCoordinate Target = default;
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        Storm Storm;
        StormDamageInfo Info = default;
        if (AICastSpellPriority(Source) != -1)
        {
            AICastSpellExpenseMade(Source, FBaseManaCost, FManaUpkeep, 0);
            if (((Hero)Source).CanCastSpellInstantly(FSpellID, FBaseManaCost, ref Reason))
            {
                Source.GetLocation(ref X, ref Y, ref L);
                Storm = CreateStorm();
                if (Storm.GetBestTarget(Source.FPlayer, X, Y, L, 12, ref Target, ref Info))
                {
                    ExecuteStormSpell(Source, Target.X, Target.Y, Target.L);
                    Result = true;
                }
                else
                {
                    Result = false;
                }

                Storm?.Dispose();
            }
            else
            {
                if (((Hero)Source).FCastingSpellID != FSpellID)
                {
                    ((Hero)Source).ExecuteStartCasting(FSpellID);
                    Result = true;
                }
                else
                {
                    Result = true;
                }
            }
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Storms.pas:809
    public override bool CanActivate(Troop Source, ref string MessageText)
    {
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        bool Result = base.CanActivate(Source, ref MessageText);
        if (Result)
        {
            Source.GetLocation(ref X, ref Y, ref L);
            if (L != 0)
            {
                Result = false;
                MessageText = AbilityRules.TranslateRStr(GameText.CannotCastSpellInUndergoundRStr);
            }
        }

        return Result;
    }

    // Storms.pas:825
    public override bool Activate(Troop Source)
    {
        bool Result = default;
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        string Reason = "";
        TargetSelectionControl Selection = default;
        base.Activate(Source);
        if (CanActivate(Source, ref Reason))
        {
            FSource = Source;
            Source.GetLocation(ref X, ref Y, ref L);
            Selection = AoWHSMap.Current.FTargetSelection;
            FTargetStorm = CreateStorm();
            Selection.Initialize(this);
            Selection.FTargetMode = Selection.FTargetMode + Set<TargetSelectionModeElement>.Of(TargetSelectionModeElement.ValidateDistance, TargetSelectionModeElement.ExternalFloater);
            Selection.FSourceX = X;
            Selection.FSourceY = Y;
            Selection.FSourceL = L;
            AoWHSMap.Current.MakeVisible(X, Y, L, 5);
            var name = FTargetStorm.Name();
            var format = RtlUnit.Format(AbilityRules.TranslateRStr(GameText.SelectTargetLocationForXOnMapRStr), [name]);
            Selection.FText.Text = format;
            Selection.FTitle = FSpellName;
            Selection.FCancelText = AbilityRules.TranslateRStr(GameText.CancelRStr);
            Selection.FRange = 12;
            Selection.FOnChange += OnTargetSelectionChange;
            Selection.FSourceImage = AoWHSSet.Current.FSpellControl.GetSpell(FSpellID).FCastImages.ImageSequences[20].GetPreviewImage();
            Selection.FValidTargetImage = AoWHSSet.Current.FSpellControl.GetSpell(FSpellID).FCastImages.ImageSequences[21].GetPreviewImage();
            Selection.FInvalidTargetImage = AoWHSSet.Current.FSpellControl.GetSpell(FSpellID).FCastImages.ImageSequences[22].GetPreviewImage();
            Selection.Activate();
            FTargetStorm.UpdateTargetSelection(AoWHSMap.Current.FTargetSelection);
            Result = true;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Storms.pas:156
    public virtual Storm CreateStorm()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Storms.pas:768
    public virtual bool ExecuteStormSpell(Troop Source, sbyte X, sbyte Y, sbyte L)
    {
        bool Result;
        StormSpellTokenEvent TE;
        if (!AoWHSMap.Current.FPlayers.GetPlayers(Source.FPlayer).GetBusy())
        {
            TE = new StormSpellTokenEvent(null);
            TE.Initialize(FSpellID, (byte)Source.FPlayer, Source.FUnitID, FBaseManaCost);
            TE.FX = X;
            TE.FY = Y;
            TE.FL = L;
            AoWHSMap.Current.FTokenManager.GetTokenControl(Source.FPlayer).AddEvent(TE);
            TE.Release();
            Result = true;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Storms.pas:860
    public virtual int AICastSpellPriority(Troop Source)
    {
        int Result;
        MapCoordinate Target = default;
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        string Reason = "";
        Storm Storm;
        StormDamageInfo Info = default;
        int Damage;
        if (CanActivate(Source, ref Reason))
        {
            Result = -1;
            Source.GetLocation(ref X, ref Y, ref L);
            if (L == 0)
            {
                Storm = CreateStorm();
                if (Storm.GetBestTarget(Source.FPlayer, X, Y, L, 12, ref Target, ref Info))
                {
                    Damage = Info.EnemyDamage - Info.FriendlyDamage;
                    if ((double)((double)FBaseManaCost * 0.75) < Damage)
                    {
                        Result = Damage * 5 + 750;
                    }
                }

                Storm?.Dispose();
            }
        }
        else
        {
            Result = -1;
        }

        return Result;
    }

    // Storms.pas:786
    public void OnTargetSelectionChange(TargetSelectionControl Selection)
    {
        switch (Selection.FChange)
        {
            case TargetSelectionChange.Deactivated:
                FTargetStorm?.Dispose();
                FTargetStorm = null;
                Selection.FOnChange -= OnTargetSelectionChange;
                break;
            case TargetSelectionChange.Selected:
                Selection.FOnChange -= OnTargetSelectionChange;
                if (ExecuteStormSpell(FSource, Selection.FTargetX, Selection.FTargetY, Selection.FTargetL))
                {
                    Selection.Deactivate();
                }

                FTargetStorm?.Dispose();
                FTargetStorm = null;
                break;
        }
    }
}
