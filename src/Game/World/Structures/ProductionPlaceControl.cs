#nullable disable
// Translated from PrdPlace.pas.
using Aow.Engine.Nodes;

using Aow.Game.Events;
using Aow.Game.Magic;
using Aow.Game.Magic.Abilities;

using Aow.Game.Units;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.World.Structures;

// production place
public partial class ProductionPlaceControl(CustomNode owner) : ProductionControl(owner)
{
    public ProductionPlace ProductionPlace { get; set; }

    // PrdPlace.pas:169
    public override MapCoordinate GetLocation() => new(
        ProductionPlace.HexX,
        ProductionPlace.HexY,
        ProductionPlace.HexLevel);

    // PrdPlace.pas:166
    public override sbyte GetPlayer() => ProductionPlace.Player;

    // PrdPlace.pas:179
    public override bool UnitProduced()
    {
        ProductionPlace productionPlace = ProductionPlace;
        sbyte hexY = productionPlace.HexY;
        sbyte hexX = productionPlace.HexX;
        bool result = Settings.Unit.Place(hexX, hexY, productionPlace.HexLevel, productionPlace.Player, Set<UnitPlaceModeElement>.Of(UnitPlaceModeElement.ScanArea)) is not null;
        if (result && ProductionPlace.GetMagicSphere() != Magic.MagicSphere.None && Settings.Unit.ExpandAbility(155))
        {
            UnitEnchantment enchantment = Settings.Unit.GetAbilityData<UnitEnchantmentAbilityData>(155).Enchantment;
            enchantment.SetUpkeep((Settings.Unit.GetUnitLevel() + 1) * 2);
            enchantment.SetSource(ProductionPlace.Player);
            enchantment.DispelResistance = (sbyte)(Settings.Unit.GetUnitLevel() + 3);
        }

        ProductionPlaceEventLog log = new(null)
        {
            Holder = CreationHolder(),
            Player = productionPlace.Player
        };
        string name = Settings.Unit.GetName();
        string fProductionPlaceName = ProductionPlace.GetName();
        log.Text = DelphiFormat.Format(AbilityRules.TranslateResourceString(GetEngine(), GameText.ProducedFormat), [name, fProductionPlaceName]);
        log.SetLocation(y: productionPlace.HexY, x: productionPlace.HexX, level: productionPlace.HexLevel);
        StrategicMap.Current.Players.GetPlayers(log.Player).EventLogbook.AddEvent(log);
        _ = log.Release();

        return result;
    }

    // PrdPlace.pas:203
    public override void UnitResearched()
    {
        base.UnitResearched();
        ProductionPlace productionPlace = ProductionPlace;
        ProductionPlaceEventLog log = new(null)
        {
            Holder = CreationHolder(),
            Player = productionPlace.Player
        };
        UnitProduction unitProduction = UnitProductions.GetItem(Settings.UnitProductionIndex);
        string unitName = GamePack.Current.UnitResources.GetUnitResource(unitProduction.UnitIndex).GetUnitName();
        string name = ProductionPlace.GetName();
        log.Text = DelphiFormat.Format(AbilityRules.TranslateResourceString(GetEngine(), GameText.InstalledFormat), [unitName, name]);
        log.SetLocation(y: productionPlace.HexY, x: productionPlace.HexX, level: productionPlace.HexLevel);
        StrategicMap.Current.Players.GetPlayers(log.Player).EventLogbook.AddEvent(log);
        _ = log.Release();
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
    public override int UnitProductionGold(int index) => ProductionPlace.GetMagicSphere() != Magic.MagicSphere.None ? 0 : base.UnitProductionGold(index);

    // PrdPlace.pas:145
    public override int UnitProductionMana(int index) => ProductionPlace.GetMagicSphere() != Magic.MagicSphere.None ? base.UnitProductionGold(index) : 0;// { The original uses the gold production cost as the node's mana cost. }

    // PrdPlace.pas:152
    public override int UnitProductionGoldUpkeep(int index) => ProductionPlace.GetMagicSphere() != Magic.MagicSphere.None ? 0 : base.UnitProductionMana(index);// { The original uses the inherited mana production cost for gold upkeep. }

    // PrdPlace.pas:159
    public override int UnitProductionManaUpkeep(int index)
    {
        if (ProductionPlace.GetMagicSphere() != Magic.MagicSphere.None)
        {
            UnitProduction unitProduction = UnitProductions.GetItem(index);
            return GamePack.Current.UnitResources.GetUnitResource(unitProduction.UnitIndex).Level * 5;
        }
        else
        {
            return 0;
        }
    }

    // PrdPlace.pas:121
    public override int UnitProductionTime(int index)
    {
        index = UnitProductions.GetItem(index).UnitIndex;
        int result = GamePack.Current.UnitResources.GetUnitResource(index).Level * 2;
        if (ProductionPlace.GetMagicSphere() != Magic.MagicSphere.None)
        {
            sbyte player = ProductionPlace.Player;
            int picks;
            if (player > 0)
            {
                MagicSphere magicSphere = ProductionPlace.GetMagicSphere();
                picks = StrategicMap.Current.Players.GetPlayers(player).Leader.GetSpherePicks(magicSphere);
            }
            else
            {
                picks = 0;
            }

            result -= picks / 2;
            if (result < 1)
            {
                return 1;
            }
        }

        return result;
    }

    // intf_P1_02.inc:120

}
