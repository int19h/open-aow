// Translated from UnitSpells.pas.
using Aow.Engine.Buffers;
using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.Magic.Abilities;

using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

public partial class CosmeticSurgery : UnitSpell
{
    // UnitSpells.pas:901
    public CosmeticSurgery(ECustomNode owner) : base(owner)
    {
        FSpellName = AbilityRules.TranslateRStr(DeformFaceRStr);
        FSpellID = 59;
        FSpellType = SpellType.Unit;
        FSpellLevel = 1;
        FMagicSphere = MagicSphere.Secret;
        FAllowMultipleTargets = false;
        FUnitDispelResistance = 10;
        FTargetRange = -1;
        FSpellControlTypes = Set<SpellControlTypeElement>.Of(SpellControlTypeElement.Global);
    }

    // UnitSpells.pas:968
    public override void GetCombatInfo(Troop Source, ref SpellCombatInfo Info)
    {
        // { The original saves Self and Source as well as Info; this compiler saves only Info. }
        base.GetCombatInfo(Source, ref Info);
        Info.TargetTypes = Set<CombatTargetTypeElement>.Of(CombatTargetTypeElement.Enemy);
    }

    // UnitSpells.pas:940
    public override bool ValidTargetUnit(Troop Source, Troop Target)
    {
        bool Result = base.ValidTargetUnit(Source, Target);
        return Result && Set<UnitType>.Of(UnitType.Humanoid, UnitType.Creature).Contains(Target.GetUnitType());
    }

    // UnitSpells.pas:946
    public override bool CastSpell(Troop Source, UnitList Targets)
    {
        bool Result;
        CosmeticSurgerySpellCaster Caster;
        if (Targets.GetCount() == 1)
        {
            Caster = new CosmeticSurgerySpellCaster(null);
            Caster.FCasterSourceID = Source.FUnitID;
            Caster.FCasterSpellID = FSpellID;
            Caster.FTargetUnitID = Targets.GetUnit(0).FUnitID;
            if (Targets.GetUnit(0) is Hero)
            {
                Caster.FFaceIndex = ((Hero)Targets.GetUnit(0)).FFaceIndex;
            }
            else
            {
                Caster.FFaceIndex = 0;
            }

            AoWHSMap.Current.FCosmeticSurgeryEventsStore.Raise(Caster);
            Caster.Release();
            Result = true;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // UnitSpells.pas:915
    public override bool ExecuteSpell(Troop Source, Troop Target, int Mana, ref Int32Array2 Data)
    {
        bool Result;
        CosmeticSurgeryEnchantment Enchantment;
        bool Expanded;
        if (ValidTargetUnit(Source, Target))
        {
            if (!Target.GetAbilityEnabled(126))
            {
                Expanded = Target.ExpandAbility(126);
            }
            else
            {
                Expanded = true;
            }

            if (Expanded)
            {
                Enchantment = (CosmeticSurgeryEnchantment)((UnitEnchantmentAbilityData)Target.GetAbilityData(126)).FEnchantment;
                Enchantment.SetUpkeep(FManaUpkeep);
                Enchantment.FFaceIndex = Data[0];
                Enchantment.SetSource(Source.FPlayer);
            }

            // { Original succeeds even when expansion fails. }
            Result = true;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    private static readonly string DeformFaceRStr = "Cosmagic Surgery";
}
