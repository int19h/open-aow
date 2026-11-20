#nullable disable
// Translated from SummonSpells.pas.
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;
using Aow.Game.AI.Groups;
using Aow.Game.Draw;
using Aow.Game.Draw.Animations;
using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.Commands;

// UnitIndices
public partial class SummonSpellTokenEvent(CustomNode owner) : SpellTokenEvent(owner)
{

    // SummonSpells.pas:171
    public new const int ClassId = 135947;

    public UnitIndexList UnitIndices { get; } = new UnitIndexList();

    // SummonSpells.pas:165
    public override void Dispose()
    {
        UnitIndices?.Dispose();
        base.Dispose();
    }

    // SummonSpells.pas:176
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        PersistSupport.UnitIndexList(persister, name: "UnitIndices", list: UnitIndices);
    }

    // SummonSpells.pas:182
    public override bool Process(Troop source, int value)
    {
        _ = base.Process(source, value);
        bool done = false;
        MapCoordinate coordinate11 = source.GetLocation();
        sbyte x = coordinate11.X;
        sbyte y = coordinate11.Y;
        sbyte l = coordinate11.MapLevel;
        for (int i = 0, last = UnitIndices.Count - 1; i <= last; i++)
        {
            Unit u = new(null) { Holder = CreationHolder() };
            int unitIndex = UnitIndices[i];
            u.SetUnitResource(GamePack.Current.UnitResources.GetUnitResource(unitIndex));
            ArmyHexSprite army = u.Place(x, y, l, source.Player, Set<UnitPlaceModeElement>.Of(UnitPlaceModeElement.ScanArea));
            if (army is not null)
            {
                sbyte hexY = army.GetHexY();
                sbyte hexX = army.GetHexX();
                if (HexGeometry.DeltaHexToRadius(x, y, hexX, hexY) <= 2)
                {
                    if (u.ExpandAbility(155))
                    {
                        UnitEnchantment enchantment = u.GetAbilityData<UnitEnchantmentAbilityData>(155).Enchantment;
                        enchantment.SetUpkeep(GamePack.Current.SpellControl.GetSpell(EventSpellId).ManaUpkeep);
                        enchantment.SetSource(source.Player);
                        enchantment.DispelResistance = (sbyte)GamePack.Current.SpellControl.GetSpell<SummonSpell>(EventSpellId).GetDispelResistance(u);
                        AIGroup group = source.Group;
                        _ = (group?.AddUnit(u));
                    }

                    done = true;
                }
                else
                {
                    u.SetOwner(null);
                }
            }

            _ = u.Release();
        }

        if (done)
        {
            if (source.Player == StrategicMap.Current.SeatedPlayer)
            {
                StrategicMap.Current.MakeVisible(x, y, l, 3);
            }

            _ = StrategicMap.Current.ValidateObservation(source.Player, x, y, l, 0);
            if (StrategicMap.Current.Visible(x, y, l, 0))
            {
                if (GamePack.Current.SpellControl.GetSpell(EventSpellId).CastImages.ImageSequences[0] is not null)
                {
                    ImageSequenceAnimation animation = new(null) { Holder = CreationHolder() };
                    animation.SetImageSequence(GamePack.Current.SpellControl.GetSpell(EventSpellId).CastImages.ImageSequences[0]);
                    _ = StrategicMap.Current.PlaceHex(animation, x, y, l);
                }

                if (source.Player == StrategicMap.Current.SeatedPlayer || StrategicMap.Current.Watching(x, y, l, 0))
                {
                    _ = GamePack.Current.SpellControl.GetSpell(EventSpellId).CastSounds.Play(0);
                }
            }

            Spell spell = GamePack.Current.SpellControl.GetSpell(EventSpellId);
            GamePack.Current.SpellControl.GetSpell(EventSpellId).CastingDone(source, spell.BaseManaCost);
        }
        else
        {
            if (source.Player == StrategicMap.Current.SeatedPlayer)
            {
                string noSummonLocationFound = AbilityRules.TranslateResourceString(GetEngine(), s_noSummonLocationFoundText);
                StrategicMap.Current.ShowMessage(noSummonLocationFound);
            }
        }

        return false;
    }

    private static readonly string s_noSummonLocationFoundText = "No nearby summon location found";

    // SummonSpells.pas:34

    // SummonSpells.pas:34

}
