#nullable disable warnings
// Translated from Storms.pas.
using Aow.Engine.Nodes;
using Aow.Game.AI;
using Aow.Game.Combat;
using Aow.Game.Commands;

using Aow.Game.Draw.Animations;

using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

// targeting storm, caster
public class StormSpell : Spell
{
    private Storm? _targetStorm;
    private Troop _source;

    // Storms.pas:762
    public StormSpell(CustomNode? owner) : base(owner) => SpellType = SpellType.Global;

    // Storms.pas:888
    public override void AIPrefetchCastSpellActions(Troop source, AICastSpellActionList actions)
    {
        if (AIRequestCastSpellExpense(source, budget: BaseManaCost, mana: 0, upkeep: 0))
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
            _ = AICastSpellExpenseMade(source, budget: BaseManaCost, mana: ManaUpkeep, upkeep: 0);
            string reason = "";
            if (source.CanCastSpellInstantly(SpellId, mana: BaseManaCost, ref reason))
            {
                MapCoordinate coordinate80 = source.GetLocation();
                int x = coordinate80.X;
                int y = coordinate80.Y;
                int l = coordinate80.MapLevel;
                Storm storm = CreateStorm();
                if (storm.GetBestTarget(source.Player, x, y, l, range: 12, out MapCoordinate target, info: out _))
                {
                    _ = ExecuteStormSpell(source, target.X, target.Y, L: target.MapLevel);
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
            int l = coordinate81.MapLevel;
            if (l != 0)
            {
                result = false;
                messageText = AbilityRules.TranslateResourceString(GetEngine(), S: GameText.CannotCastSpellInUndergroundText);
            }
        }

        return result;
    }

    // Storms.pas:825
    public override bool Activate(Troop source)
    {
        string reason = "";
        _ = base.Activate(source);
        if (CanActivate(source, messageText: ref reason))
        {
            _source = source;
            MapCoordinate coordinate82 = source.GetLocation();
            int x = coordinate82.X;
            int y = coordinate82.Y;
            int l = coordinate82.MapLevel;
            TargetSelectionControl selection = StrategicMap.Current.TargetSelection;
            _targetStorm = CreateStorm();
            selection.Initialize(this);
            selection.TargetMode |= TargetSelectionModes.ValidateDistance | TargetSelectionModes.ExternalFloater;
            selection.SourceLocation = new MapCoordinate((sbyte)x, (sbyte)y, (sbyte)l);
            StrategicMap.Current.MakeVisible(x, y, l, radius: 5);
            string name = _targetStorm.Name();
            string format = DelphiFormat.Format(AbilityRules.TranslateResourceString(GetEngine(), S: GameText.SelectTargetLocationForOnMapFormat), args: [name]);
            selection.Text.Text = format;
            selection.Title = SpellName;
            selection.CancelText = AbilityRules.TranslateResourceString(GetEngine(), S: GameText.CancelText);
            selection.Range = 12;
            selection.Changed += TargetSelection_Changed;
            selection.SourceImage = GamePack.Current.SpellControl[SpellId].CastImages.ImageSequences[20].GetPreviewImage();
            selection.ValidTargetImage = GamePack.Current.SpellControl[SpellId].CastImages.ImageSequences[21].GetPreviewImage();
            selection.InvalidTargetImage = GamePack.Current.SpellControl[SpellId].CastImages.ImageSequences[22].GetPreviewImage();
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
    public virtual bool ExecuteStormSpell(Troop source, int X, int Y, int L)
    {
        if (!StrategicMap.Current.Players.GetPlayers(source.Player).GetBusy())
        {
            StormSpellTokenEvent tokenEvent = new(null) { Holder = CreationHolder() };
            tokenEvent.Initialize(SpellId, playerIndex: PlayerId.FromOriginal((byte)PlayerIdExtensions.ToOriginal(source.Player)), sourceId: source.UnitId, mana: BaseManaCost);
            tokenEvent.Location = new MapCoordinate((sbyte)X, (sbyte)Y, (sbyte)L);
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
        if (CanActivate(source, messageText: ref reason))
        {
            result = -1;
            MapCoordinate coordinate83 = source.GetLocation();
            int x = coordinate83.X;
            int y = coordinate83.Y;
            int l = coordinate83.MapLevel;
            if (l == 0)
            {
                Storm storm = CreateStorm();
                if (storm.GetBestTarget(source.Player, x, y, l, range: 12, target: out _, out StormDamageInfo info))
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
    public void TargetSelection_Changed(object? sender, TargetSelectionChangedEventArgs e)
    {
        TargetSelectionControl selection = (TargetSelectionControl)sender!;
        switch (e.Change)
        {
            case TargetSelectionChange.Deactivated:
                _targetStorm?.Dispose();
                _targetStorm = null;
                selection.Changed -= TargetSelection_Changed;
                break;
            case TargetSelectionChange.Selected:
                selection.Changed -= TargetSelection_Changed;
                if (ExecuteStormSpell(_source, selection.Target.X, selection.Target.Y, L: selection.Target.MapLevel))
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
