#nullable disable
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
    public StormSpell(CustomNode owner) : base(owner) => SpellType = SpellType.Global;

    // Storms.pas:888
    public override void AIPrefetchCastSpellActions(Troop source, AICastSpellActionList actions)
    {
        if (AIRequestCastSpellExpense(source, BaseManaCost, 0, 0))
        {
            int priority = AICastSpellPriority(source);
            if (priority != -1)
            {
                AICastSpellAction action = CreateCastSpellAction();
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
        if (AICastSpellPriority(source) != -1)
        {
            _ = AICastSpellExpenseMade(source, BaseManaCost, ManaUpkeep, 0);
            string reason = "";
            if (source.CanCastSpellInstantly(SpellId, BaseManaCost, ref reason))
            {
                MapCoordinate coordinate80 = source.GetLocation();
                sbyte x = coordinate80.X;
                sbyte y = coordinate80.Y;
                sbyte l = coordinate80.MapLevel;
                Storm storm = CreateStorm();
                if (storm.GetBestTarget(source.Player, x, y, l, 12, out MapCoordinate target, out _))
                {
                    _ = ExecuteStormSpell(source, target.X, target.Y, target.MapLevel);
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
                if (source.GetCastingSpellId() != SpellId)
                {
                    _ = source.ExecuteStartCasting(SpellId);
                    return true;
                }
                else
                {
                    return true;
                }
            }
        }
        else
        {
            return false;
        }

        return result;
    }

    // Storms.pas:809
    public override bool CanActivate(Troop source, ref string messageText)
    {
        bool result = base.CanActivate(source, ref messageText);
        if (result)
        {
            MapCoordinate coordinate81 = source.GetLocation();
            _ = coordinate81.X;
            _ = coordinate81.Y;
            sbyte l = coordinate81.MapLevel;
            if (l != 0)
            {
                result = false;
                messageText = AbilityRules.TranslateResourceString(GetEngine(), GameText.CannotCastSpellInUndergroundText);
            }
        }

        return result;
    }

    // Storms.pas:825
    public override bool Activate(Troop source)
    {
        string reason = "";
        _ = base.Activate(source);
        if (CanActivate(source, ref reason))
        {
            _source = source;
            MapCoordinate coordinate82 = source.GetLocation();
            sbyte x = coordinate82.X;
            sbyte y = coordinate82.Y;
            sbyte l = coordinate82.MapLevel;
            TargetSelectionControl selection = StrategicMap.Current.TargetSelection;
            _targetStorm = CreateStorm();
            selection.Initialize(this);
            selection.TargetMode += Set<TargetSelectionModeElement>.Of(TargetSelectionModeElement.ValidateDistance, TargetSelectionModeElement.ExternalFloater);
            selection.SourceLocation = new MapCoordinate(x, y, l);
            StrategicMap.Current.MakeVisible(x, y, l, 5);
            string name = _targetStorm.Name();
            string format = DelphiFormat.Format(AbilityRules.TranslateResourceString(GetEngine(), GameText.SelectTargetLocationForOnMapFormat), [name]);
            selection.Text.Text = format;
            selection.Title = SpellName;
            selection.CancelText = AbilityRules.TranslateResourceString(GetEngine(), GameText.CancelText);
            selection.Range = 12;
            selection.Changed += OnTargetSelectionChange;
            selection.SourceImage = GamePack.Current.SpellControl.GetSpell(SpellId).CastImages.ImageSequences[20].GetPreviewImage();
            selection.ValidTargetImage = GamePack.Current.SpellControl.GetSpell(SpellId).CastImages.ImageSequences[21].GetPreviewImage();
            selection.InvalidTargetImage = GamePack.Current.SpellControl.GetSpell(SpellId).CastImages.ImageSequences[22].GetPreviewImage();
            selection.Activate();
            _targetStorm.UpdateTargetSelection(StrategicMap.Current.TargetSelection);
            return true;
        }
        else
        {
            return false;
        }
    }

    // Storms.pas:156
    public virtual Storm CreateStorm() => throw AbstractErrorException.Create("Abstract Error");

    // Storms.pas:768
    public virtual bool ExecuteStormSpell(Troop source, sbyte X, sbyte Y, sbyte L)
    {
        if (!StrategicMap.Current.Players.GetPlayers(source.Player).GetBusy())
        {
            StormSpellTokenEvent tokenEvent = new(null) { Holder = CreationHolder() };
            tokenEvent.Initialize(SpellId, (byte)source.Player, source.UnitId, BaseManaCost);
            tokenEvent.Location = new MapCoordinate(X, Y, L);
            StrategicMap.Current.TokenManager.GetTokenControl(source.Player).AddEvent(tokenEvent);
            _ = tokenEvent.Release();
            return true;
        }
        else
        {
            return false;
        }
    }

    // Storms.pas:860
    public virtual int AICastSpellPriority(Troop source)
    {
        int result;
        string reason = "";
        if (CanActivate(source, ref reason))
        {
            result = -1;
            MapCoordinate coordinate83 = source.GetLocation();
            sbyte x = coordinate83.X;
            sbyte y = coordinate83.Y;
            sbyte l = coordinate83.MapLevel;
            if (l == 0)
            {
                Storm storm = CreateStorm();
                if (storm.GetBestTarget(source.Player, x, y, l, 12, out _, out StormDamageInfo info))
                {
                    int damage = info.EnemyDamage - info.FriendlyDamage;
                    if ((double)(BaseManaCost * 0.75) < damage)
                    {
                        result = (damage * 5) + 750;
                    }
                }

                storm?.Dispose();
            }
        }
        else
        {
            return -1;
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
                if (ExecuteStormSpell(_source, selection.Target.X, selection.Target.Y, selection.Target.MapLevel))
                {
                    selection.Deactivate();
                }

                _targetStorm?.Dispose();
                _targetStorm = null;
                break;
            case TargetSelectionChange.None:
            case TargetSelectionChange.Activated:
            default:
                break;
        }
    }
}
