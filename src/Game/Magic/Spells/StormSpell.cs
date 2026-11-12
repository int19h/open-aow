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
    private Storm _targetStorm;
    private Troop _source;

    // Storms.pas:762
    public StormSpell(ECustomNode owner) : base(owner)
    {
        SpellType = SpellType.Global;
    }

    // Storms.pas:888
    public override void AIPrefetchCastSpellActions(Troop source, AICastSpellActionList actions)
    {
        int priority;
        AICastSpellAction action;
        if (AIRequestCastSpellExpense(source, BaseManaCost, 0, 0))
        {
            priority = AICastSpellPriority(source);
            if (priority != -1)
            {
                action = CreateCastSpellAction();
                SetupCastSpellAction(source, action);
                action.Priority = priority;
                action.SetOwner(actions);
            }
        }
    }

    // Storms.pas:906
    public override bool AIExecuteCastSpellAction(Troop source)
    {
        bool result;
        string reason = "";
        MapCoordinate target = default;
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        Storm storm;
        StormDamageInfo info = default;
        if (AICastSpellPriority(source) != -1)
        {
            AICastSpellExpenseMade(source, BaseManaCost, ManaUpkeep, 0);
            if (((Hero)source).CanCastSpellInstantly(SpellId, BaseManaCost, ref reason))
            {
                source.GetLocation(out X, out Y, out L);
                storm = CreateStorm();
                if (storm.GetBestTarget(source.Player, X, Y, L, 12, ref target, out info))
                {
                    ExecuteStormSpell(source, target.X, target.Y, target.MapLevel);
                    result = true;
                }
                else
                {
                    result = false;
                }

                storm?.Dispose();
            }
            else
            {
                if (((Hero)source).CastingSpellId != SpellId)
                {
                    ((Hero)source).ExecuteStartCasting(SpellId);
                    result = true;
                }
                else
                {
                    result = true;
                }
            }
        }
        else
        {
            result = false;
        }

        return result;
    }

    // Storms.pas:809
    public override bool CanActivate(Troop source, ref string messageText)
    {
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        bool result = base.CanActivate(source, ref messageText);
        if (result)
        {
            source.GetLocation(out X, out Y, out L);
            if (L != 0)
            {
                result = false;
                messageText = AbilityRules.TranslateResourceString(GameText.CannotCastSpellInUndergroundText);
            }
        }

        return result;
    }

    // Storms.pas:825
    public override bool Activate(Troop source)
    {
        bool result = default;
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        string reason = "";
        TargetSelectionControl selection = default;
        base.Activate(source);
        if (CanActivate(source, ref reason))
        {
            _source = source;
            source.GetLocation(out X, out Y, out L);
            selection = StrategicMap.Current.TargetSelection;
            _targetStorm = CreateStorm();
            selection.Initialize(this);
            selection.TargetMode = selection.TargetMode + Set<TargetSelectionModeElement>.Of(TargetSelectionModeElement.ValidateDistance, TargetSelectionModeElement.ExternalFloater);
            selection.SourceX = X;
            selection.SourceY = Y;
            selection.SourceL = L;
            StrategicMap.Current.MakeVisible(X, Y, L, 5);
            var name = _targetStorm.Name();
            var format = RtlUnit.Format(AbilityRules.TranslateResourceString(GameText.SelectTargetLocationForOnMapFormat), [name]);
            selection.Text.Text = format;
            selection.Title = SpellName;
            selection.CancelText = AbilityRules.TranslateResourceString(GameText.CancelText);
            selection.Range = 12;
            selection.Changed += OnTargetSelectionChange;
            selection.SourceImage = GamePack.Current.SpellControl.GetSpell(SpellId).CastImages.ImageSequences[20].GetPreviewImage();
            selection.ValidTargetImage = GamePack.Current.SpellControl.GetSpell(SpellId).CastImages.ImageSequences[21].GetPreviewImage();
            selection.InvalidTargetImage = GamePack.Current.SpellControl.GetSpell(SpellId).CastImages.ImageSequences[22].GetPreviewImage();
            selection.Activate();
            _targetStorm.UpdateTargetSelection(StrategicMap.Current.TargetSelection);
            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // Storms.pas:156
    public virtual Storm CreateStorm()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Storms.pas:768
    public virtual bool ExecuteStormSpell(Troop source, sbyte X, sbyte Y, sbyte L)
    {
        bool result;
        StormSpellTokenEvent tokenEvent;
        if (!StrategicMap.Current.Players.GetPlayers(source.Player).GetBusy())
        {
            tokenEvent = new StormSpellTokenEvent(null);
            tokenEvent.Initialize(SpellId, (byte)source.Player, source.UnitId, BaseManaCost);
            tokenEvent.X = X;
            tokenEvent.Y = Y;
            tokenEvent.MapLevel = L;
            StrategicMap.Current.TokenManager.GetTokenControl(source.Player).AddEvent(tokenEvent);
            tokenEvent.Release();
            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // Storms.pas:860
    public virtual int AICastSpellPriority(Troop source)
    {
        int result;
        MapCoordinate target = default;
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        string reason = "";
        Storm storm;
        StormDamageInfo info = default;
        int damage;
        if (CanActivate(source, ref reason))
        {
            result = -1;
            source.GetLocation(out X, out Y, out L);
            if (L == 0)
            {
                storm = CreateStorm();
                if (storm.GetBestTarget(source.Player, X, Y, L, 12, ref target, out info))
                {
                    damage = info.EnemyDamage - info.FriendlyDamage;
                    if ((double)((double)BaseManaCost * 0.75) < damage)
                    {
                        result = damage * 5 + 750;
                    }
                }

                storm?.Dispose();
            }
        }
        else
        {
            result = -1;
        }

        return result;
    }

    // Storms.pas:786
    public void OnTargetSelectionChange(TargetSelectionControl selection)
    {
        switch (selection.Change)
        {
            case TargetSelectionChange.Deactivated:
                _targetStorm?.Dispose();
                _targetStorm = null;
                selection.Changed -= OnTargetSelectionChange;
                break;
            case TargetSelectionChange.Selected:
                selection.Changed -= OnTargetSelectionChange;
                if (ExecuteStormSpell(_source, selection.TargetX, selection.TargetY, selection.TargetL))
                {
                    selection.Deactivate();
                }

                _targetStorm?.Dispose();
                _targetStorm = null;
                break;
        }
    }
}
