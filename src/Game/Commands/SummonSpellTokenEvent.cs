// Translated from SummonSpells.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
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
    public override void ReadWrite(StorageStream stream)
    {
        base.ReadWrite(stream);
        stream.Payload(30, UnitIndices.LoadFromStream, UnitIndices.SaveToStream);
    }

    // SummonSpells.pas:159
    public SummonSpellTokenEvent(ECustomNode owner) : base(owner)
    {
        UnitIndices = new UnitIndexList();
    }

    // SummonSpells.pas:182
    public override bool Process(Troop source, int value)
    {
        bool done;
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        Unit U;
        ArmyHexSprite army;
        UnitEnchantment enchantment;
        AIGroup group;
        ImageSequenceAnimation animation;
        base.Process(source, value);
        done = false;
        source.GetLocation(out X, out Y, out L);
        for (int I = 0, last = UnitIndices.Count - 1; I <= last; I++)
        {
            U = new Unit(null) { Holder = CreationHolder() };
            var unitIndex = UnitIndices[I];
            U.SetUnitResource(GamePack.Current.UnitResources.GetUnitResource(unitIndex));
            army = U.Place(X, Y, L, source.Player, Set<UnitPlaceModeElement>.Of(UnitPlaceModeElement.ScanArea));
            if (army != null)
            {
                var hexY = army.GetHexY();
                var hexX = army.GetHexX();
                if (HexGeometry.DeltaHexToRadius(X, Y, hexX, hexY) <= 2)
                {
                    if (U.ExpandAbility(155))
                    {
                        enchantment = ((UnitEnchantmentAbilityData)U.GetAbilityData(155)).Enchantment;
                        enchantment.SetUpkeep(GamePack.Current.SpellControl.GetSpell(EventSpellId).ManaUpkeep);
                        enchantment.SetSource(source.Player);
                        enchantment.DispelResistance = (sbyte)((SummonSpell)GamePack.Current.SpellControl.GetSpell(EventSpellId)).GetDispelResistance(U);
                        group = source.Group;
                        if (group != null)
                        {
                            group.AddUnit(U);
                        }
                    }

                    done = true;
                }
                else
                {
                    U.SetOwner(null);
                }
            }

            U.Release();
        }

        if (done)
        {
            if (source.Player == StrategicMap.Current.SeatedPlayer)
            {
                StrategicMap.Current.MakeVisible(X, Y, L, 3);
            }

            StrategicMap.Current.ValidateObservation(source.Player, X, Y, L, 0);
            if (StrategicMap.Current.Visible(X, Y, L, 0))
            {
                if (GamePack.Current.SpellControl.GetSpell(EventSpellId).CastImages.ImageSequences[0] != null)
                {
                    animation = new ImageSequenceAnimation(null) { Holder = CreationHolder() };
                    animation.SetImageSequence(GamePack.Current.SpellControl.GetSpell(EventSpellId).CastImages.ImageSequences[0]);
                    StrategicMap.Current.PlaceHex(animation, X, Y, L);
                }

                if (source.Player == StrategicMap.Current.SeatedPlayer || StrategicMap.Current.Watching(X, Y, L, 0))
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
