#nullable disable
// Translated from GlobalTargetSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;
using Aow.Game.Commands;

using Aow.Game.Magic.Abilities;

using Aow.Game.World;
using Aow.Game.World.Map;

namespace Aow.Game.Magic.Spells;

public class SmokeCloudSpell : GlobalTargetSpell
{
    // GlobalTargetSpells.pas:2325
    public SmokeCloudSpell(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), GameText.SmokeCloudText);
        SpellId = 74;
        BaseManaCost = 5;
        SpellLevel = 3;
        MagicSphere = MagicSphere.Fire;
    }

    // GlobalTargetSpells.pas:2373
    public override bool ExecuteTokenEvent(SpellTokenEvent eventValue, Troop source, int executeCount)
    {
        // SmokeCloudSpell.TargetSelected constructs the GlobalTargetSpellTokenEvent this spell executes.
        GlobalTargetSpellTokenEvent targetEvent = (GlobalTargetSpellTokenEvent)eventValue;
        Troop sourceLocal = targetEvent.Source;
        _ = StrategicMap.Current.ValidateObservation(sourceLocal.Player, targetEvent.Location.X, targetEvent.Location.Y, targetEvent.Location.MapLevel, 2);
        if (targetEvent.Source.Player == StrategicMap.Current.SeatedPlayer || StrategicMap.Current.Watching(targetEvent.Location.X, targetEvent.Location.Y, targetEvent.Location.MapLevel, 2))
        {
            _ = GamePack.Current.SpellControl[targetEvent.EventSpellId].CastSounds.Play(0);
        }

        EngineResource resource = ((CustomNode)(object)GamePack.Current.MapObjectResourceList).FindChild<EngineResource>(135747);
        if (resource is not null)
        {
            SmokeCloud cloud = resource.CreateResourceUser<SmokeCloud>();
            cloud.Player = source.Player;
            cloud.TurnsRemaining = -1;
            cloud.Enchantment.PlayerIndex = source.Player;
            cloud.Enchantment.EnchantmentManaUpkeep = ManaUpkeep;
            cloud.Drifting = false;
            if (!StrategicMap.Current.PlaceHex(cloud, targetEvent.Location.X, targetEvent.Location.Y, targetEvent.Location.MapLevel))
            {
                cloud?.Dispose();
            }

            GamePack.Current.SpellControl[SpellId].CastingDone(source, BaseManaCost);
        }

        return false;
    }

    // GlobalTargetSpells.pas:2343
    public override void SetupTargetSelection(Troop source, TargetSelectionControl targetSelection)
    {
        TargetSelectionControl target = targetSelection;
        base.SetupTargetSelection(source, target);
        target.HexNumbers.Clear();
        for (int i = 0, last = HexGeometry.RadiusToHexNumber(4) - 1; i <= last; i++)
        {
            _ = target.HexNumbers.Add(0);
        }
    }

    // GlobalTargetSpells.pas:2356
    public override bool TargetSelected(Troop source, int X, int Y, int L)
    {
        GlobalTargetSpellTokenEvent eventValue = new(null) { Holder = CreationHolder() };
        eventValue.Initialize(SpellId, (byte)source.Player, source.UnitId, BaseManaCost);
        eventValue.SetLocation((sbyte)X, (sbyte)Y, (sbyte)L);
        StrategicMap.Current.TokenManager.GetTokenControl(source.Player).AddEvent(eventValue);
        _ = eventValue.Release();
        return true;
    }

    // GlobalTargetSpells.pas:2339
    public override bool ValidTargetSelectionMapField(MapField field) => base.ValidTargetSelectionMapField(field);

    // GlobalTargetSpells.pas:2335
    public override bool ValidTargetMapField(MapField field) => base.ValidTargetMapField(field);
}
