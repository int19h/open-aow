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
public partial class SummonSpellTokenEvent : SpellTokenEvent
{
    // SummonSpells.pas:171
    public new const int ClassId = 135947;

    public UnitIndexList UnitIndices { get; }

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

    // SummonSpells.pas:159
    public SummonSpellTokenEvent(CustomNode owner) : base(owner) => UnitIndices = new UnitIndexList();

    // SummonSpells.pas:182
    public override bool Process(Troop source, int value)
    {
        bool done;
        sbyte x = default;
        sbyte y = default;
        sbyte l = default;
        Unit u;
        ArmyHexSprite army;
        UnitEnchantment enchantment;
        AIGroup group;
        ImageSequenceAnimation animation;
        base.Process(source, value);
        done = false;
        var coordinate11 = source.GetLocation();
        x = coordinate11.X;
        y = coordinate11.Y;
        l = coordinate11.MapLevel;
        for (int i = 0, last = UnitIndices.Count - 1; i <= last; i++)
        {
            u = new Unit(null) { Holder = CreationHolder() };
            var unitIndex = UnitIndices[i];
            u.SetUnitResource(GamePack.Current.UnitResources.GetUnitResource(unitIndex));
            army = u.Place(x, y, l, source.Player, Set<UnitPlaceModeElement>.Of(UnitPlaceModeElement.ScanArea));
            if (army != null)
            {
                var hexY = army.GetHexY();
                var hexX = army.GetHexX();
                if (HexGeometry.DeltaHexToRadius(x, y, hexX, hexY) <= 2)
                {
                    if (u.ExpandAbility(155))
                    {
                        enchantment = ((UnitEnchantmentAbilityData)u.GetAbilityData(155)).Enchantment;
                        enchantment.SetUpkeep(GamePack.Current.SpellControl.GetSpell(EventSpellId).ManaUpkeep);
                        enchantment.SetSource(source.Player);
                        enchantment.DispelResistance = (sbyte)((SummonSpell)GamePack.Current.SpellControl.GetSpell(EventSpellId)).GetDispelResistance(u);
                        group = source.Group;
                        if (group != null)
                        {
                            group.AddUnit(u);
                        }
                    }

                    done = true;
                }
                else
                {
                    u.SetOwner(null);
                }
            }

            u.Release();
        }

        if (done)
        {
            if (source.Player == StrategicMap.Current.SeatedPlayer)
            {
                StrategicMap.Current.MakeVisible(x, y, l, 3);
            }

            StrategicMap.Current.ValidateObservation(source.Player, x, y, l, 0);
            if (StrategicMap.Current.Visible(x, y, l, 0))
            {
                if (GamePack.Current.SpellControl.GetSpell(EventSpellId).CastImages.ImageSequences[0] != null)
                {
                    animation = new ImageSequenceAnimation(null) { Holder = CreationHolder() };
                    animation.SetImageSequence(GamePack.Current.SpellControl.GetSpell(EventSpellId).CastImages.ImageSequences[0]);
                    StrategicMap.Current.PlaceHex(animation, x, y, l);
                }

                if (source.Player == StrategicMap.Current.SeatedPlayer || StrategicMap.Current.Watching(x, y, l, 0))
                {
                    GamePack.Current.SpellControl.GetSpell(EventSpellId).CastSounds.Play(0);
                }
            }

            var spell = GamePack.Current.SpellControl.GetSpell(EventSpellId);
            GamePack.Current.SpellControl.GetSpell(EventSpellId).CastingDone(source, spell.BaseManaCost);
        }
        else
        {
            if (source.Player == StrategicMap.Current.SeatedPlayer)
            {
                var noSummonLocationFound = AbilityRules.TranslateResourceString(GetEngine(), s_noSummonLocationFoundText);
                StrategicMap.Current.ShowMessage(noSummonLocationFound);
            }
        }

        return false;
    }

    private static readonly string s_noSummonLocationFoundText = "No nearby summon location found";

    // SummonSpells.pas:34

    // SummonSpells.pas:34

}
