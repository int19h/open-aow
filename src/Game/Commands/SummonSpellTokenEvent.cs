#nullable disable warnings
// Translated from SummonSpells.pas.
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
public class SummonSpellTokenEvent(CustomNode? owner) : SpellTokenEvent(owner)
{

    // SummonSpells.pas:171
    public new const int ClassId = 135_947;

    public UnitIndexList UnitIndices { get; } = [];

    // SummonSpells.pas:165
    public override void Dispose() => base.Dispose();

    // SummonSpells.pas:176
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        persister.UnitIndexList(name: "UnitIndices", list: UnitIndices);
    }

    // SummonSpells.pas:182
    public override bool Process(Troop source, int value)
    {
        _ = base.Process(source, value);
        bool done = false;
        MapCoordinate coordinate11 = source.GetLocation();
        int x = coordinate11.X;
        int y = coordinate11.Y;
        int l = coordinate11.MapLevel;
        for (int i = 0, last = UnitIndices.Count - 1; i <= last; i++)
        {
            Unit u = new(null) { Holder = CreationHolder() };
            int unitIndex = UnitIndices[i];
            u.SetUnitResource(GetEngine().GamePack.UnitResources.GetUnitResource(unitIndex));
            ArmyHexSprite? army = u.Place(x, y, l, source.Player, mode: UnitPlaceModes.ScanArea);
            if (army is not null)
            {
                int hexY = army.HexY;
                int hexX = army.HexX;
                if (HexGeometry.DeltaHexToRadius(x, y, hexX, hexY) <= 2)
                {
                    if (u.ExpandAbility(155))
                    {
                        UnitEnchantment? enchantment = u.GetAbilityData<UnitEnchantmentAbilityData>(155).Enchantment;
                        enchantment.SetUpkeep(GetEngine().GamePack.SpellControl[EventSpellId].ManaUpkeep);
                        enchantment.SetSource(source.Player);
                        enchantment.DispelResistance = GetEngine().GamePack.SpellControl.GetSpell<SummonSpell>(EventSpellId).GetDispelResistance(u).TruncateToSByte();
                        AIGroup? group = source.Group;
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
            StrategicMap strategicMap = GetEngine().StrategicMap;
            if (source.Player == strategicMap.SeatedPlayer)
            {
                strategicMap.MakeVisible(x, y, l, radius: 3);
            }

            _ = GetEngine().StrategicMap.ValidateObservation(source.Player, x, y, l, radius: 0);
            StrategicMap engineStrategicMap = GetEngine().StrategicMap;
            if (engineStrategicMap.Visible(x, y, l, radius: 0))
            {
                GamePack gamePack = GetEngine().GamePack;
                if (gamePack.SpellControl[EventSpellId].CastImages.ImageSequences[0] is not null)
                {
                    ImageSequenceAnimation animation = new(null) { Holder = CreationHolder() };
                    animation.SetImageSequence(gamePack.SpellControl[EventSpellId].CastImages.ImageSequences[0]);
                    _ = engineStrategicMap.PlaceHex(animation, x, y, l);
                }

                StrategicMap watchingStrategicMap = GetEngine().StrategicMap;
                if (source.Player == watchingStrategicMap.SeatedPlayer || watchingStrategicMap.Watching(x, y, l, radius: 0))
                {
                    _ = GetEngine().GamePack.SpellControl[EventSpellId].CastSounds.Play(0);
                }
            }

            GamePack engineGamePack = GetEngine().GamePack;
            Spell? spell = engineGamePack.SpellControl[EventSpellId];
            engineGamePack.SpellControl[EventSpellId].CastingDone(source, mana: spell.BaseManaCost);
        }
        else
        {
            if (source.Player == GetEngine().StrategicMap.SeatedPlayer)
            {
                string noSummonLocationFound = AbilityRules.TranslateResourceString(GetEngine(), S: s_noSummonLocationFoundText);
                GetEngine().StrategicMap.ShowMessage(noSummonLocationFound);
            }
        }

        return false;
    }

    private static readonly string s_noSummonLocationFoundText = "No nearby summon location found";
}
