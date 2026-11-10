// Translated from PrdPlace.pas.
using Aow.Engine.Nodes;

using Aow.Game.Events;

using Aow.Game.Magic.Abilities;

using Aow.Game.Units;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.World.Structures;

// production place
public partial class ProductionPlaceControl : ProductionControl
{
    public ProductionPlaceControl(ECustomNode owner) : base(owner)
    { }

    public ProductionPlace FProductionPlace;

    // PrdPlace.pas:169
    public override void GetLocation(ref sbyte X, ref sbyte Y, ref sbyte L)
    {
        X = this.FProductionPlace.Xhx;
        Y = this.FProductionPlace.Yhx;
        L = FProductionPlace.Lhx;
    }

    // PrdPlace.pas:166
    public override sbyte GetPlayer() => FProductionPlace.FPlayer;

    // PrdPlace.pas:179
    public override bool UnitProduced()
    {
        UnitEnchantment Enchantment;
        ProductionPlaceEventLog Log;
        var productionPlace = FProductionPlace;
        var yhx = productionPlace.Yhx;
        var xhx = productionPlace.Xhx;
        bool Result = FSettings.FUnit.Place(xhx, yhx, productionPlace.Lhx, productionPlace.FPlayer, Set<UnitPlaceModeElement>.Of(UnitPlaceModeElement.ScanArea)) != null;
        if (Result && FProductionPlace.GetMagicSphere() != Magic.MagicSphere.None && FSettings.FUnit.ExpandAbility(155))
        {
            Enchantment = ((UnitEnchantmentAbilityData)FSettings.FUnit.GetAbilityData(155)).FEnchantment;
            Enchantment.SetUpkeep((FSettings.FUnit.GetUnitLevel() + 1) * 2);
            Enchantment.SetSource(FProductionPlace.FPlayer);
            Enchantment.FDispelResistance = (sbyte)(FSettings.FUnit.GetUnitLevel() + 3);
        }

        Log = new ProductionPlaceEventLog(null);
        Log.FPlayer = productionPlace.FPlayer;
        var name = FSettings.FUnit.GetName();
        var fProductionPlaceName = FProductionPlace.GetName();
        Log.FText = RtlUnit.Format(AbilityRules.TranslateRStr(GameText.XProducedAtYRStr), [name, fProductionPlaceName]);
        var productionPlaceYhx = productionPlace.Yhx;
        Log.SetLocation(productionPlace.Xhx, productionPlaceYhx, productionPlace.Lhx);
        AoWHSMap.Current.FPlayers.GetPlayers(Log.FPlayer).FEventLogbook.AddEvent(Log);
        Log.Release();

        return Result;
    }

    // PrdPlace.pas:203
    public override void UnitResearched()
    {
        ProductionPlaceEventLog Log;
        base.UnitResearched();
        var productionPlace = FProductionPlace;
        Log = new ProductionPlaceEventLog(null);
        Log.FPlayer = productionPlace.FPlayer;
        var unitProduction = FUnitProductions.GetItem(FSettings.FUnitProductionIndex);
        var unitName = AoWHSSet.Current.FUnitResources.GetUnitResource(unitProduction.FUnitIndex).GetUnitName();
        var name = FProductionPlace.GetName();
        Log.FText = RtlUnit.Format(AbilityRules.TranslateRStr(GameText.XInstalledAtYRStr), [unitName, name]);
        var yhx = productionPlace.Yhx;
        Log.SetLocation(productionPlace.Xhx, yhx, productionPlace.Lhx);
        AoWHSMap.Current.FPlayers.GetPlayers(Log.FPlayer).FEventLogbook.AddEvent(Log);
        Log.Release();
    }

    // PrdPlace.pas:219
    public override void Update()
    {
        base.Update();
        if (FUpdateCount == 0)
        {
            FProductionPlace.Update();
        }
    }

    // PrdPlace.pas:139
    public override int UnitProductionGold(int Index)
    {
        int Result;
        if (FProductionPlace.GetMagicSphere() != Magic.MagicSphere.None)
        {
            Result = 0;
        }
        else
        {
            Result = base.UnitProductionGold(Index);
        }

        return Result;
    }

    // PrdPlace.pas:145
    public override int UnitProductionMana(int Index)
    {
        int Result;
        // { The original uses the gold production cost as the node's mana cost. }
        if (FProductionPlace.GetMagicSphere() != Magic.MagicSphere.None)
        {
            Result = base.UnitProductionGold(Index);
        }
        else
        {
            Result = 0;
        }

        return Result;
    }

    // PrdPlace.pas:152
    public override int UnitProductionGoldUpkeep(int Index)
    {
        int Result;
        // { The original uses the inherited mana production cost for gold upkeep. }
        if (FProductionPlace.GetMagicSphere() != Magic.MagicSphere.None)
        {
            Result = 0;
        }
        else
        {
            Result = base.UnitProductionMana(Index);
        }

        return Result;
    }

    // PrdPlace.pas:159
    public override int UnitProductionManaUpkeep(int Index)
    {
        int Result;
        if (FProductionPlace.GetMagicSphere() != Magic.MagicSphere.None)
        {
            var unitProduction = FUnitProductions.GetItem(Index);
            Result = AoWHSSet.Current.FUnitResources.GetUnitResource(unitProduction.FUnitIndex).FLevel * 5;
        }
        else
        {
            Result = 0;
        }

        return Result;
    }

    // PrdPlace.pas:121
    public override int UnitProductionTime(int Index)
    {
        sbyte Player;
        int Picks;
        Index = FUnitProductions.GetItem(Index).FUnitIndex;
        int Result = AoWHSSet.Current.FUnitResources.GetUnitResource(Index).FLevel * 2;
        if (FProductionPlace.GetMagicSphere() != Magic.MagicSphere.None)
        {
            Player = FProductionPlace.FPlayer;
            if (Player > 0)
            {
                var magicSphere = FProductionPlace.GetMagicSphere();
                Picks = AoWHSMap.Current.FPlayers.GetPlayers(Player).FLeader.GetSpherePicks(magicSphere);
            }
            else
            {
                Picks = 0;
            }

            Result -= Picks / 2;
            if (Result < 1)
            {
                Result = 1;
            }
        }

        return Result;
    }

    // intf_P1_02.inc:120

}
