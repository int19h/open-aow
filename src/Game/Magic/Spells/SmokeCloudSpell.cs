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
        this.FSpellName = AbilityRules.TranslateRStr(GameText.SmokeCloudRStr);
        FSpellID = 74;
        FBaseManaCost = 5;
        FSpellLevel = 3;
        FMagicSphere = MagicSphere.Fire;
    }

    // GlobalTargetSpells.pas:2373
    public override bool ExecuteTE(SpellTE Event, AbstractUnit Source, int ExecuteCount)
    {
        EResource Resource;
        SmokeCloud Cloud;
        var source = Event.GetSource();
        AoWHSMap.Current.ValidateObservation(source.FPlayer, ((GlobalTargetSpellTE)Event).FLocation.X, ((GlobalTargetSpellTE)Event).FLocation.Y, ((GlobalTargetSpellTE)Event).FLocation.L, 2);
        if (Event.GetSource().FPlayer == AoWHSMap.Current.FSeatedPlayer || AoWHSMap.Current.Watching(((GlobalTargetSpellTE)Event).FLocation.X, ((GlobalTargetSpellTE)Event).FLocation.Y, ((GlobalTargetSpellTE)Event).FLocation.L, 2))
        {
            AoWHSSet.Current.FSpellControl.GetSpell(Event.FEventSpellID).FCastSounds.Play(0);
        }

        Resource = (EResource)((ECustomNode)(object)AoWHSSet.Current.MapObjectResourceList).FindChild(135747);
        if (Resource != null)
        {
            Cloud = (SmokeCloud)Resource.CreateResourceUser();
            Cloud.FPlayer = Source.FPlayer;
            Cloud.FTurnsRemaining = -1;
            Cloud.FEnchantment.FPlayerIndex = Source.FPlayer;
            Cloud.FEnchantment.FEnchantmentManaUpkeep = this.FManaUpkeep;
            Cloud.FDrifting = false;
            if (!AoWHSMap.Current.PlaceHX(Cloud, ((GlobalTargetSpellTE)Event).FLocation.X, ((GlobalTargetSpellTE)Event).FLocation.Y, ((GlobalTargetSpellTE)Event).FLocation.L))
            {
                Cloud?.Dispose();
            }

            AoWHSSet.Current.FSpellControl.GetSpell(this.FSpellID).CastingDone(Source, this.FBaseManaCost);
        }

        return false;
    }

    // GlobalTargetSpells.pas:2343
    public override void SetupTargetSelection(AbstractUnit Source, TargetSelectionControl TargetSelection)
    {
        TargetSelectionControl Target;
        Target = TargetSelection;
        base.SetupTargetSelection(Source, Target);
        Target.FHNs.Clear();
        for (int I = 0, last = HexGeometry.RadToHN(4) - 1; I <= last; I++)
        {
            Target.FHNs.Add(0);
        }
    }

    // GlobalTargetSpells.pas:2356
    public override bool TargetSelected(AbstractUnit Source, int X, int Y, int L)
    {
        GlobalTargetSpellTE Event;
        Event = new GlobalTargetSpellTE(null);
        Event.Initialize(FSpellID, (byte)Source.FPlayer, Source.FUnitID, FBaseManaCost);
        Event.SetLocation((sbyte)X, (sbyte)Y, (sbyte)L);
        AoWHSMap.Current.FTokenManager.GetTokenControl(Source.FPlayer).AddEvent(Event);
        Event.Release();
        return true;
    }

    // GlobalTargetSpells.pas:2339
    public override bool ValidTargetSelectionMapF(MapField Field) => base.ValidTargetSelectionMapF(Field);

    // GlobalTargetSpells.pas:2335
    public override bool ValidTargetMapF(MapField Field) => base.ValidTargetMapF(Field);
}
