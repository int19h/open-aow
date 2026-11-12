// Translated from GlobalTargetSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;
using Aow.Game.Commands;

using Aow.Game.Magic.Abilities;

using Aow.Game.World;
using Aow.Game.World.Map;

namespace Aow.Game.Magic.Spells;

public partial class SmokeCloudSpell : GlobalTargetSpell
{
    // GlobalTargetSpells.pas:2325
    public SmokeCloudSpell(ECustomNode owner) : base(owner)
    {
        this.SpellName = AbilityRules.TranslateResourceString(GameText.SmokeCloudText);
        SpellId = 74;
        BaseManaCost = 5;
        SpellLevel = 3;
        MagicSphere = MagicSphere.Fire;
    }

    // GlobalTargetSpells.pas:2373
    public override bool ExecuteTokenEvent(SpellTokenEvent eventValue, Troop source, int executeCount)
    {
        EngineResource resource;
        SmokeCloud cloud;
        var sourceLocal = eventValue.GetSource();
        AoWHSMap.Current.ValidateObservation(sourceLocal.Player, ((GlobalTargetSpellTokenEvent)eventValue).Location.X, ((GlobalTargetSpellTokenEvent)eventValue).Location.Y, ((GlobalTargetSpellTokenEvent)eventValue).Location.MapLevel, 2);
        if (eventValue.GetSource().Player == AoWHSMap.Current.SeatedPlayer || AoWHSMap.Current.Watching(((GlobalTargetSpellTokenEvent)eventValue).Location.X, ((GlobalTargetSpellTokenEvent)eventValue).Location.Y, ((GlobalTargetSpellTokenEvent)eventValue).Location.MapLevel, 2))
        {
            AoWHSSet.Current.SpellControl.GetSpell(eventValue.EventSpellId).CastSounds.Play(0);
        }

        resource = (EngineResource)((ECustomNode)(object)AoWHSSet.Current.MapObjectResourceList).FindChild(135747);
        if (resource != null)
        {
            cloud = (SmokeCloud)resource.CreateResourceUser();
            cloud.Player = source.Player;
            cloud.TurnsRemaining = -1;
            cloud.Enchantment.PlayerIndex = source.Player;
            cloud.Enchantment.EnchantmentManaUpkeep = this.ManaUpkeep;
            cloud.Drifting = false;
            if (!AoWHSMap.Current.PlaceHex(cloud, ((GlobalTargetSpellTokenEvent)eventValue).Location.X, ((GlobalTargetSpellTokenEvent)eventValue).Location.Y, ((GlobalTargetSpellTokenEvent)eventValue).Location.MapLevel))
            {
                cloud?.Dispose();
            }

            AoWHSSet.Current.SpellControl.GetSpell(this.SpellId).CastingDone(source, this.BaseManaCost);
        }

        return false;
    }

    // GlobalTargetSpells.pas:2343
    public override void SetupTargetSelection(Troop source, TargetSelectionControl targetSelection)
    {
        TargetSelectionControl target;
        target = targetSelection;
        base.SetupTargetSelection(source, target);
        target.HexNumbers.Clear();
        for (int I = 0, last = HexGeometry.RadiusToHexNumber(4) - 1; I <= last; I++)
        {
            target.HexNumbers.Add(0);
        }
    }

    // GlobalTargetSpells.pas:2356
    public override bool TargetSelected(Troop source, int X, int Y, int L)
    {
        GlobalTargetSpellTokenEvent eventValue;
        eventValue = new GlobalTargetSpellTokenEvent(null);
        eventValue.Initialize(SpellId, (byte)source.Player, source.UnitId, BaseManaCost);
        eventValue.SetLocation((sbyte)X, (sbyte)Y, (sbyte)L);
        AoWHSMap.Current.TokenManager.GetTokenControl(source.Player).AddEvent(eventValue);
        eventValue.Release();
        return true;
    }

    // GlobalTargetSpells.pas:2339
    public override bool ValidTargetSelectionMapField(MapField field) => base.ValidTargetSelectionMapField(field);

    // GlobalTargetSpells.pas:2335
    public override bool ValidTargetMapField(MapField field) => base.ValidTargetMapField(field);
}
