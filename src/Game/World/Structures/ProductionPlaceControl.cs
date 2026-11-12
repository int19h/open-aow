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

    public ProductionPlace ProductionPlace;

    // PrdPlace.pas:169
    public override void GetLocation(ref sbyte X, ref sbyte Y, ref sbyte L)
    {
        X = this.ProductionPlace.HexX;
        Y = this.ProductionPlace.HexY;
        L = ProductionPlace.HexLevel;
    }

    // PrdPlace.pas:166
    public override sbyte GetPlayer() => ProductionPlace.Player;

    // PrdPlace.pas:179
    public override bool UnitProduced()
    {
        UnitEnchantment enchantment;
        ProductionPlaceEventLog log;
        var productionPlace = ProductionPlace;
        var hexY = productionPlace.HexY;
        var hexX = productionPlace.HexX;
        bool result = Settings.Unit.Place(hexX, hexY, productionPlace.HexLevel, productionPlace.Player, Set<UnitPlaceModeElement>.Of(UnitPlaceModeElement.ScanArea)) != null;
        if (result && ProductionPlace.GetMagicSphere() != Magic.MagicSphere.None && Settings.Unit.ExpandAbility(155))
        {
            enchantment = ((UnitEnchantmentAbilityData)Settings.Unit.GetAbilityData(155)).Enchantment;
            enchantment.SetUpkeep((Settings.Unit.GetUnitLevel() + 1) * 2);
            enchantment.SetSource(ProductionPlace.Player);
            enchantment.DispelResistance = (sbyte)(Settings.Unit.GetUnitLevel() + 3);
        }

        log = new ProductionPlaceEventLog(null);
        log._player = productionPlace.Player;
        var name = Settings.Unit.GetName();
        var fProductionPlaceName = ProductionPlace.GetName();
        log.Text = RtlUnit.Format(AbilityRules.TranslateResourceString(GameText.s_producedFormat), [name, fProductionPlaceName]);
        var productionPlaceHexY = productionPlace.HexY;
        log.SetLocation(productionPlace.HexX, productionPlaceHexY, productionPlace.HexLevel);
        AoWHSMap.Current.Players.GetPlayers(log._player).EventLogbook.AddEvent(log);
        log.Release();

        return result;
    }

    // PrdPlace.pas:203
    public override void UnitResearched()
    {
        ProductionPlaceEventLog log;
        base.UnitResearched();
        var productionPlace = ProductionPlace;
        log = new ProductionPlaceEventLog(null);
        log._player = productionPlace.Player;
        var unitProduction = UnitProductions.GetItem(Settings.UnitProductionIndex);
        var unitName = AoWHSSet.Current.UnitResources.GetUnitResource(unitProduction.UnitIndex).GetUnitName();
        var name = ProductionPlace.GetName();
        log.Text = RtlUnit.Format(AbilityRules.TranslateResourceString(GameText.s_installedFormat), [unitName, name]);
        var hexY = productionPlace.HexY;
        log.SetLocation(productionPlace.HexX, hexY, productionPlace.HexLevel);
        AoWHSMap.Current.Players.GetPlayers(log._player).EventLogbook.AddEvent(log);
        log.Release();
    }

    // PrdPlace.pas:219
    public override void Update()
    {
        base.Update();
        if (UpdateCount == 0)
        {
            ProductionPlace.Update();
        }
    }

    // PrdPlace.pas:139
    public override int UnitProductionGold(int index)
    {
        int result;
        if (ProductionPlace.GetMagicSphere() != Magic.MagicSphere.None)
        {
            result = 0;
        }
        else
        {
            result = base.UnitProductionGold(index);
        }

        return result;
    }

    // PrdPlace.pas:145
    public override int UnitProductionMana(int index)
    {
        int result;
        // { The original uses the gold production cost as the node's mana cost. }
        if (ProductionPlace.GetMagicSphere() != Magic.MagicSphere.None)
        {
            result = base.UnitProductionGold(index);
        }
        else
        {
            result = 0;
        }

        return result;
    }

    // PrdPlace.pas:152
    public override int UnitProductionGoldUpkeep(int index)
    {
        int result;
        // { The original uses the inherited mana production cost for gold upkeep. }
        if (ProductionPlace.GetMagicSphere() != Magic.MagicSphere.None)
        {
            result = 0;
        }
        else
        {
            result = base.UnitProductionMana(index);
        }

        return result;
    }

    // PrdPlace.pas:159
    public override int UnitProductionManaUpkeep(int index)
    {
        int result;
        if (ProductionPlace.GetMagicSphere() != Magic.MagicSphere.None)
        {
            var unitProduction = UnitProductions.GetItem(index);
            result = AoWHSSet.Current.UnitResources.GetUnitResource(unitProduction.UnitIndex).Level * 5;
        }
        else
        {
            result = 0;
        }

        return result;
    }

    // PrdPlace.pas:121
    public override int UnitProductionTime(int index)
    {
        sbyte player;
        int picks;
        index = UnitProductions.GetItem(index).UnitIndex;
        int result = AoWHSSet.Current.UnitResources.GetUnitResource(index).Level * 2;
        if (ProductionPlace.GetMagicSphere() != Magic.MagicSphere.None)
        {
            player = ProductionPlace.Player;
            if (player > 0)
            {
                var magicSphere = ProductionPlace.GetMagicSphere();
                picks = AoWHSMap.Current.Players.GetPlayers(player).Leader.GetSpherePicks(magicSphere);
            }
            else
            {
                picks = 0;
            }

            result -= picks / 2;
            if (result < 1)
            {
                result = 1;
            }
        }

        return result;
    }

    // intf_P1_02.inc:120

}
