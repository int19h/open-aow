using Aow.Game.Draw;
using Aow.Game.Events;
using Aow.Game.Magic;
using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Game.Units;
using Aow.Game.World.Structures;
using Aow.Rtl;
using static Aow.Game.Events.EventLogUnit;
using static Aow.Game.Magic.Abilities.AoWEUnit;
using static Aow.Rtl.RtlUnit;

// What a production item orders: produce a unit, research one, trade, or one of the city actions.
// Each kind holds its own fields and does what the switches over the production id and its city
// action did, arm by arm (Structur.PAS, City.pas, PrdPlace.pas and the three screens). The original
// format stores an order as three integers (TProductionItem properties 20 to 22), and ToFields and
// FromFields convert between the two. A mod order registers an id of its own and overrides these.

namespace Aow.Game.World.Structures
{
    public abstract record ProductionOrder
    {
        private static readonly Catalog<int, Func<int, int, int, ProductionOrder>> Kinds = new("production order");

        // The production id, parameter and group id the original format stores.
        public abstract (int Id, int Parameter, int GroupId) ToFields();

        // The order the three stored fields name. An id no kind registered reads as UnknownOrder,
        // which writes the three fields back unchanged.
        public static ProductionOrder FromFields(int Id, int Parameter, int GroupId) =>
            Kinds.TryGet(Id, out var create) ? create(Id, Parameter, GroupId) : new UnknownOrder(Id, Parameter, GroupId);

        // Registers the kind of a production id. A second registration of an id throws.
        internal static void Register(int Id, Func<int, int, int, ProductionOrder> Create) => Kinds.Add(Id, Create);

        // The production ids with a registered kind.
        internal static bool IsRegistered(int Id) => Kinds.TryGet(Id, out _);

        // The city action the city screen and the city queries name this order by (City.pas:3220).
        public virtual TCityAction CityAction => TCityAction.caNone;

        // The unit production the screens draw for this order, or null.
        public virtual int? UnitProductionIndex => null;

        // The queue line (Structur.PAS:840).
        public virtual string Name(TProductionControl Control) => TranslateRStr(UnknownRStr);

        // The line of the running order (Structur.PAS:865).
        public virtual string NameProgressive(TProductionControl Control) => TranslateRStr(UnknownRStr);

        // Whether Control can start the order now, with the reason when it cannot (Structur.PAS:361).
        public virtual bool CanExecute(TProductionControl Control, ref string Reason)
        {
            Waiting(Control, ref Reason);
            return false;
        }

        // Starts the order inside the control's update bracket (Structur.PAS:392).
        public virtual void Execute(TProductionControl Control)
        {
        }

        // Starts the order once the control's update bracket closed (City.pas:434, PrdPlace.pas:238).
        public virtual void Executed(TProductionControl Control)
        {
        }

        // The order's turns ran out in the turn of Player (Structur.PAS:804).
        public virtual void Complete(TProductionControl Control, sbyte Player)
        {
        }

        // The order is still running with no turns left after Complete (City.pas:496, PrdPlace.pas:231).
        public virtual void Completed(TProductionControl Control)
        {
        }

        // The running order is cancelled, before the control refunds it (City.pas:573).
        public virtual void Cancel(TProductionControl Control)
        {
        }

        // Whether a looping queue puts the order back at its end (Structur.PAS:837, City.pas:604).
        public virtual bool Loopable(TProductionControl Control) => true;

        // Whether the order in the queue bars further entries (Structur.PAS:834, City.pas:595).
        public virtual bool LocksQueue(TProductionControl Control) => false;

        // The city screen's activity image (AoWCityScreen.pas:904).
        public virtual int ActivityImage(TCity City) => 40;

        // The production screen's activity image (AoWProdScreen.pas:719).
        public virtual int ActivityImage(TProductionPlace Place) => 4;

        // The image sequence of City's map image while the order runs, Row 0 to 2 from the top
        // (City.pas:2503, 2539, 2575): null for the race image, -1 for no image.
        public virtual int? CitySequence(TCity City, int Row) => null;

        // The activity column of the realm screen (AoWRealm.pas).
        public virtual string RealmActivity(TCity City) => "-";

        // The info window's lines for the order at the head of Control's queue (AoWInfoWin.pas:439, 496).
        public virtual void ActivityLines(TProductionControl Control, TStrings Lines)
        {
            Lines.Text = NameProgressive(Control);
        }

        // The base control's waiting note, which every arm of the base control sets first. The
        // original continues checking costs after assigning the waiting text.
        protected static void Waiting(TProductionControl Control, ref string Reason)
        {
            if (Control.FStatus == TProductionControlStatus.pcsWaiting)
            {
                Reason = "(" + LowerCase(TranslateRStr(WaitingRStr)) + ")";
            }
        }

        // Changes the relation of City's race to the control's player by Change.
        protected static void ChangeRelation(TProductionControl Control, TCity City, int Change)
        {
            var playerRelationValue = AoWHSMap.FRaces.GetRace(City.FRace).GetPlayerRelationValue(Control.GetPlayer());
            AoWHSMap.FRaces.GetRace(City.FRace).SetPlayerRelationValue(Control.GetPlayer(), playerRelationValue + Change);
        }

        // Adds Text at City to the logbook of the control's player.
        protected static void Log(TProductionControl Control, TCity City, string Text)
        {
            var Event = new TCityProductionEventLog(null);
            Event.FPlayer = Control.GetPlayer();
            Event.FText = Text;
            var lhx = City.GetLhx();
            var yhx = City.GetYhx();
            Event.SetLocation(City.GetXhx(), yhx, lhx);
            AoWHSMap.FPlayers.GetPlayers(Event.FPlayer).FEventLogbook.AddEvent(Event);
            Event.Release();
        }

        // Charges the settings' costs to the settings' player.
        protected static void Pay(TProductionControl Control)
        {
            var player = AoWHSMap.FPlayers.GetPlayers(Control.FSettings.FPlayer);
            player.SetGems(player.FGems - Control.FSettings.FGold);
            player.FMagicControl.SetMana(player.FMagicControl.FMana - Control.FSettings.FMana);
        }
    }

    // An id no kind registered. It does what the default arms did and keeps the three fields.
    public sealed record UnknownOrder(int Id, int Parameter, int GroupId) : ProductionOrder
    {
        public override (int Id, int Parameter, int GroupId) ToFields() => (Id, Parameter, GroupId);
    }

    // Production id 1: produce unit production Index, for AI group GroupId when not 0.
    public sealed record ProduceUnit(int Index, int GroupId) : ProductionOrder
    {
        public const int Kind = 1;

        public override (int Id, int Parameter, int GroupId) ToFields() => (Kind, Index, GroupId);

        public override TCityAction CityAction => TCityAction.caProduce;

        public override int? UnitProductionIndex => Index;

        public override string Name(TProductionControl Control)
        {
            var production = Control.FUnitProductions.GetItem(Index);
            if (production == null)
            {
                return TranslateRStr(UnknownRStr);
            }

            var format = Control.UnitProductionGold(Index) != 0 ? ProduceXRStr : SummonXRStr;
            var unitName = AoWHSSet.FUnitResources.GetUnitResource(production.FUnitIndex).GetUnitName();
            return AoWEUnit.Format(TranslateRStr(format), [unitName]);
        }

        public override string NameProgressive(TProductionControl Control)
        {
            var production = Control.FUnitProductions.GetItem(Index);
            if (production == null)
            {
                return TranslateRStr(UnknownRStr);
            }

            var format = Control.UnitProductionGold(Index) != 0 ? ProducingXRStr : SummoningXRStr;
            var unitName = AoWHSSet.FUnitResources.GetUnitResource(production.FUnitIndex).GetUnitName();
            return AoWEUnit.Format(TranslateRStr(format), [unitName]);
        }

        public override bool CanExecute(TProductionControl Control, ref string Reason)
        {
            bool Result;
            Waiting(Control, ref Reason);
            var players = AoWHSMap.FPlayers.GetPlayers(Control.GetPlayer());
            if (Control.UnitProductionGold(Index) <= players.FGems)
            {
                var fPlayersPlayers = AoWHSMap.FPlayers.GetPlayers(Control.GetPlayer());
                if (Control.UnitProductionMana(Index) <= fPlayersPlayers.FMagicControl.FMana)
                {
                    Result = true;
                }
                else
                {
                    Result = false;
                    Reason = TranslateRStr(NotEnoughManaRStr);
                }
            }
            else
            {
                Result = false;
                Reason = TranslateRStr(NotEnoughGoldRStr);
            }

            return Result;
        }

        public override void Execute(TProductionControl Control)
        {
            var settings = Control.FSettings;
            settings.Clear();
            settings.FUnitProductionIndex = Index;
            settings.FUnit = Control.CreateUnitProduction(Index);
            settings.FGold = Control.UnitProductionGold(Index);
            settings.FMana = Control.UnitProductionMana(Index);
            settings.FGoldUpkeep = Control.UnitProductionGoldUpkeep(Index);
            settings.FManaUpkeep = Control.UnitProductionManaUpkeep(Index);
            settings.FGroupID = GroupId;
            settings.FTurnsTotal = (sbyte)Control.UnitProductionTime(Index);
            settings.FPlayer = Control.GetPlayer();
            settings.FTurnsLeft = settings.FTurnsTotal;
            Pay(Control);
            Control.FStatus = TProductionControlStatus.pcsProducing;
            if (settings.FGroupID != 0)
            {
                Control.RegisterGroupProduction();
            }
        }

        public override void Complete(TProductionControl Control, sbyte Player)
        {
            if (Control.UnitProduced())
            {
                if (AoWHSMap.FPlayers.GetPlayers(Player).FPlayerType != TPlayerType.ptHuman && Control.FSettings.FGroupID != 0)
                {
                    Control.GroupProductionProduced(Control.FSettings.FUnit, Control.FSettings.FGroupID);
                }

                Control.FSettings.Clear();
                Control.ProductionDone();
            }
        }

        public override int ActivityImage(TCity City) => 4;

        public override string RealmActivity(TCity City) => "5";

        public override void ActivityLines(TProductionControl Control, TStrings Lines)
        {
            var unit = Control.CreateUnitProduction(Index);
            Lines.Clear();
            Lines.Add(TranslateRStr(ProducingRStr));
            Lines.Add(unit.GetName());
            unit.Release();
        }
    }

    // Production id 2: research unit production Index.
    public sealed record ResearchSpell(int Index) : ProductionOrder
    {
        public const int Kind = 2;

        public override (int Id, int Parameter, int GroupId) ToFields() => (Kind, Index, 0);

        public override TCityAction CityAction => TCityAction.caResearch;

        public override int? UnitProductionIndex => Index;

        public override string Name(TProductionControl Control)
        {
            var production = Control.FUnitProductions.GetItem(Index);
            if (production == null)
            {
                return TranslateRStr(UnknownRStr);
            }

            var unitName = AoWHSSet.FUnitResources.GetUnitResource(production.FUnitIndex).GetUnitName();
            return AoWEUnit.Format(TranslateRStr(InstallXRStr), [unitName]);
        }

        public override string NameProgressive(TProductionControl Control)
        {
            var production = Control.FUnitProductions.GetItem(Index);
            if (production == null)
            {
                return TranslateRStr(UnknownRStr);
            }

            var unitName = AoWHSSet.FUnitResources.GetUnitResource(production.FUnitIndex).GetUnitName();
            return AoWEUnit.Format(TranslateRStr(InstallingXRStr), [unitName]);
        }

        public override bool CanExecute(TProductionControl Control, ref string Reason)
        {
            Waiting(Control, ref Reason);
            var players = AoWHSMap.FPlayers.GetPlayers(Control.GetPlayer());
            if (Control.UnitResearchGold(Index) <= players.FGems)
            {
                return true;
            }

            Reason = TranslateRStr(NotEnoughGoldRStr);
            return false;
        }

        public override void Execute(TProductionControl Control)
        {
            var settings = Control.FSettings;
            settings.Clear();
            settings.FUnitProductionIndex = Index;
            settings.FGold = Control.UnitResearchGold(Index);
            settings.FMana = 0;
            settings.FTurnsTotal = (sbyte)Control.UnitResearchTime(Index);
            settings.FPlayer = Control.GetPlayer();
            settings.FTurnsLeft = settings.FTurnsTotal;
            Pay(Control);
            Control.FStatus = TProductionControlStatus.pcsProducing;
        }

        public override void Complete(TProductionControl Control, sbyte Player)
        {
            Control.UnitResearched();
            Control.FSettings.Clear();
            Control.ProductionDone();
        }

        public override bool Loopable(TProductionControl Control) => false;

        public override int ActivityImage(TCity City) => 5;

        public override int? CitySequence(TCity City, int Row) => -1;

        public override string RealmActivity(TCity City) => "5";

        public override void ActivityLines(TProductionControl Control, TStrings Lines)
        {
            var unit = Control.CreateUnitProduction(Index);
            Lines.Add(TranslateRStr(InstallingRStr));
            Lines.Add(unit.GetName());
            unit.Release();
        }
    }

    // Production id 10: trade for a turn, in a city or a production place. TurnsTotal is 1 for a
    // queued trade and -1 for the default one (City.pas:1475, PrdPlace.pas:511). Both controls
    // trade alike, a power node names it after the power it channels.
    public sealed record Trade(int TurnsTotal) : ProductionOrder
    {
        public const int Kind = 10;

        public override (int Id, int Parameter, int GroupId) ToFields() => (Kind, TurnsTotal, 0);

        public override TCityAction CityAction => TCityAction.caTrade;

        public override string Name(TProductionControl Control) => Control.MagicSphere() != TMagicSphere.msNone
            ? TranslateRStr(GenerateExtraPowerRStr)
            : TranslateRStr(ProduceMerchandiseRStr);

        public override string NameProgressive(TProductionControl Control) => Control.MagicSphere() != TMagicSphere.msNone
            ? TranslateRStr(GeneratingExtraPowerRStr)
            : TranslateRStr(ProducingMerchandiseRStr);

        public override bool CanExecute(TProductionControl Control, ref string Reason) => true;

        public override void Executed(TProductionControl Control)
        {
            Control.FSettings.FTurnsTotal = (sbyte)TurnsTotal;
            Control.FSettings.FTurnsLeft = 1;
            Control.FSettings.FPlayer = Control.GetPlayer();
            Control.FStatus = TProductionControlStatus.pcsProducing;
        }

        public override void Completed(TProductionControl Control)
        {
            Control.ProductionDone();
        }

        public override int ActivityImage(TProductionPlace Place) => Place.GetMagicSphere() != TMagicSphere.msNone ? 48 : 47;
    }

    // Production id 11: raise the city's upgrade level.
    public sealed record Upgrade : ProductionOrder
    {
        public const int Kind = 11;

        public override (int Id, int Parameter, int GroupId) ToFields() => (Kind, 0, 0);

        public override TCityAction CityAction => TCityAction.caUpgrade;

        public override string Name(TProductionControl Control) =>
            Control.ProductionCity() != null ? TranslateRStr(UpgradeRStr) : base.Name(Control);

        public override string NameProgressive(TProductionControl Control) =>
            Control.ProductionCity() != null ? TranslateRStr(UpgradingRStr) : base.NameProgressive(Control);

        public override bool CanExecute(TProductionControl Control, ref string Reason)
        {
            var city = Control.ProductionCity();
            if (city == null)
            {
                return base.CanExecute(Control, ref Reason);
            }

            if (AoWHSMap.FPlayers.GetPlayers(Control.GetPlayer()).FGems >= city.UpgradeCost())
            {
                return true;
            }

            Reason = TranslateRStr(NotEnoughGoldRStr);
            return false;
        }

        public override void Executed(TProductionControl Control)
        {
            var city = Control.ProductionCity();
            if (city == null)
            {
                return;
            }

            Control.FSettings.FTurnsTotal = (sbyte)city.UpgradeTime();
            Control.FSettings.FTurnsLeft = Control.FSettings.FTurnsTotal;
            Control.FSettings.FPlayer = Control.GetPlayer();
            Control.FSettings.FGold = city.UpgradeCost();
            var player = AoWHSMap.FPlayers.GetPlayers(Control.GetPlayer());
            AoWHSMap.FPlayers.GetPlayers(Control.GetPlayer()).SetGems(player.FGems - Control.FSettings.FGold);
            Control.FStatus = TProductionControlStatus.pcsProducing;
        }

        public override void Completed(TProductionControl Control)
        {
            var city = Control.ProductionCity();
            if (city == null)
            {
                return;
            }

            Control.BeginUpdate();
            city.SetUpgradeLevel((byte)(city.FUpgradeLevel + 1));
            ChangeRelation(Control, city, 5);
            Control.ProductionDone();
            Log(Control, city, RtlUnit.Format(TranslateRStr(XUpgradedRStr), [city.GetName()]));
            Control.EndUpdate();
        }

        public override bool Loopable(TProductionControl Control) => Control.ProductionCity() == null;

        public override int ActivityImage(TCity City) => 43;

        public override int? CitySequence(TCity City, int Row) => 33 + Row;

        public override string RealmActivity(TCity City) => "2";
    }

    // Production id 12: build the city's next wall.
    public sealed record Fortify : ProductionOrder
    {
        public const int Kind = 12;

        public override (int Id, int Parameter, int GroupId) ToFields() => (Kind, 0, 0);

        public override TCityAction CityAction => TCityAction.caFortify;

        public override string Name(TProductionControl Control) =>
            Control.ProductionCity() != null ? TranslateRStr(FortifyRStr) : base.Name(Control);

        public override string NameProgressive(TProductionControl Control) =>
            Control.ProductionCity() != null ? TranslateRStr(FortifyingRStr) : base.NameProgressive(Control);

        public override bool CanExecute(TProductionControl Control, ref string Reason)
        {
            var city = Control.ProductionCity();
            if (city == null)
            {
                return base.CanExecute(Control, ref Reason);
            }

            if (AoWHSMap.FPlayers.GetPlayers(Control.GetPlayer()).FGems >= city.FortifyCost())
            {
                return true;
            }

            Reason = TranslateRStr(NotEnoughGoldRStr);
            return false;
        }

        public override void Executed(TProductionControl Control)
        {
            var city = Control.ProductionCity();
            if (city == null)
            {
                return;
            }

            Control.FSettings.FTurnsTotal = (sbyte)city.FortifyTime();
            Control.FSettings.FTurnsLeft = Control.FSettings.FTurnsTotal;
            Control.FSettings.FPlayer = Control.GetPlayer();
            Control.FSettings.FGold = city.FortifyCost();
            var playerGems = AoWHSMap.FPlayers.GetPlayers(Control.GetPlayer());
            AoWHSMap.FPlayers.GetPlayers(Control.GetPlayer()).SetGems(playerGems.FGems - Control.FSettings.FGold);
            Control.FStatus = TProductionControlStatus.pcsProducing;
        }

        public override void Completed(TProductionControl Control)
        {
            var city = Control.ProductionCity();
            if (city == null)
            {
                return;
            }

            Control.BeginUpdate();
            switch (city.FWallType)
            {
                case 0:
                    city.FWallType = 1;
                    break;
                case 1:
                    city.FWallType = 2;
                    break;
            }

            Log(Control, city, RtlUnit.Format(TranslateRStr(CityFortifiedRStr), [city.GetName()]));
            city.UpdateImages();
            ChangeRelation(Control, city, 5);
            Control.ProductionDone();
            Control.EndUpdate();
        }

        public override bool Loopable(TProductionControl Control) => Control.ProductionCity() == null;

        public override int ActivityImage(TCity City) => City.FWallType == 0 ? 41 : 42;

        public override int? CitySequence(TCity City, int Row) => City.FWallType switch
        {
            0 => 30 + Row,
            1 => 48 + Row,
            _ => -1,
        };

        public override string RealmActivity(TCity City) => City.FWallType == 1 ? "1" : "0";
    }

    // Production id 13: the city's people move out and Race moves in.
    public sealed record Migrate(int Race) : ProductionOrder
    {
        public const int Kind = 13;

        public override (int Id, int Parameter, int GroupId) ToFields() => (Kind, Race, 0);

        public override TCityAction CityAction => TCityAction.caMigrate;

        public override string Name(TProductionControl Control) => Control.ProductionCity() != null
            ? RtlUnit.Format(TranslateRStr(MigrateToXRStr), [TranslateRStr(RaceNamesPluralRStr[Race])])
            : base.Name(Control);

        public override string NameProgressive(TProductionControl Control) => Control.ProductionCity() != null
            ? RtlUnit.Format(TranslateRStr(MigratingToXRStr), [TranslateRStr(RaceNamesPluralRStr[Race])])
            : base.NameProgressive(Control);

        public override bool CanExecute(TProductionControl Control, ref string Reason)
        {
            var city = Control.ProductionCity();
            return city != null ? city.CanMigrate((sbyte)Race, ref Reason) : base.CanExecute(Control, ref Reason);
        }

        public override void Executed(TProductionControl Control)
        {
            var city = Control.ProductionCity();
            if (city == null)
            {
                return;
            }

            Control.FSettings.FTurnsTotal = (sbyte)city.MigrationTime((sbyte)Race);
            Control.FSettings.FTurnsLeft = Control.FSettings.FTurnsTotal;
            Control.FSettings.FPlayer = Control.GetPlayer();
            Control.FStatus = TProductionControlStatus.pcsProducing;
            ChangeRelation(Control, city, -15);
        }

        public override void Completed(TProductionControl Control)
        {
            var city = Control.ProductionCity();
            if (city == null)
            {
                return;
            }

            Control.BeginUpdate();
            var raceNamesPlural = TranslateRStr(RaceNamesPluralRStr[Race]);
            Log(Control, city, RtlUnit.Format(TranslateRStr(CityMigratedToRStr), [city.GetName(), raceNamesPlural]));
            city.SetRace((sbyte)Race);
            ChangeRelation(Control, city, 10);
            Control.FUnitProductions.Clear();
            city.SetUpgradeLevel(city.FUpgradeLevel);
            Control.ProductionDone();
            Control.EndUpdate();
        }

        public override void Cancel(TProductionControl Control)
        {
            var city = Control.ProductionCity();
            if (city != null)
            {
                ChangeRelation(Control, city, Control.FSettings.FTurnsLeft == Control.FSettings.FTurnsTotal ? 15 : 10);
            }
        }

        public override bool Loopable(TProductionControl Control) => Control.ProductionCity() == null;

        public override bool LocksQueue(TProductionControl Control) => Control.ProductionCity() != null;

        public override int ActivityImage(TCity City) => 44;

        public override int? CitySequence(TCity City, int Row) => 39 + Row;

        public override string RealmActivity(TCity City) => "4";
    }

    // Production id 14: loot the city and raze it.
    public sealed record Loot : ProductionOrder
    {
        public const int Kind = 14;

        public override (int Id, int Parameter, int GroupId) ToFields() => (Kind, 0, 0);

        public override TCityAction CityAction => TCityAction.caLoot;

        public override string Name(TProductionControl Control) =>
            Control.ProductionCity() != null ? TranslateRStr(LootRStr) : base.Name(Control);

        public override string NameProgressive(TProductionControl Control) =>
            Control.ProductionCity() != null ? TranslateRStr(LootingRStr) : base.NameProgressive(Control);

        public override bool CanExecute(TProductionControl Control, ref string Reason)
        {
            var city = Control.ProductionCity();
            return city != null ? city.CanLoot(ref Reason) : base.CanExecute(Control, ref Reason);
        }

        public override void Executed(TProductionControl Control)
        {
            var city = Control.ProductionCity();
            if (city == null)
            {
                return;
            }

            Control.FSettings.FTurnsTotal = (sbyte)city.LootTime();
            Control.FSettings.FTurnsLeft = Control.FSettings.FTurnsTotal;
            Control.FSettings.FPlayer = Control.GetPlayer();
            Control.FSettings.FGold = 0;
            ChangeRelation(Control, city, -30);
            var Log = new TLocationTextEventLog(null);
            Log.FPlayer = Control.GetPlayer();
            Log.FText = RtlUnit.Format(TranslateRStr(CityLootingRStr), [city.GetName()]);
            var lhx = city.GetLhx();
            var yhx = city.GetYhx();
            Log.SetLocation(city.GetXhx(), yhx, lhx);
            var cityYhx = city.GetYhx();
            var cityLhx = city.GetLhx();
            var xhx = city.GetXhx();
            DistributeLocationEventLog(Log, Control.GetPlayer(), xhx, cityYhx, cityLhx, false);
            Log.Release();
            city.FOverlay.SetMode(TCityOverlayMode.omLooting);
            Control.FStatus = TProductionControlStatus.pcsProducing;
        }

        public override void Completed(TProductionControl Control)
        {
            var city = Control.ProductionCity();
            if (city == null)
            {
                return;
            }

            Control.BeginUpdate();
            Log(Control, city, RtlUnit.Format(TranslateRStr(CityLootedRStr), [city.GetName(), IntToStr(city.FIncome * 3)]));
            var playerGems = AoWHSMap.FPlayers.GetPlayers(Control.GetPlayer());
            AoWHSMap.FPlayers.GetPlayers(Control.GetPlayer()).SetGems(playerGems.FGems + city.FIncome * 3);
            city.FOverlay.SetMode(TCityOverlayMode.omIdle);
            city.SetRazed(true);
            Control.EndUpdate();
        }

        public override void Cancel(TProductionControl Control)
        {
            var city = Control.ProductionCity();
            if (city != null)
            {
                ChangeRelation(Control, city, Control.FSettings.FTurnsLeft == Control.FSettings.FTurnsTotal ? 30 : 20);
            }
        }

        public override bool Loopable(TProductionControl Control) => Control.ProductionCity() == null;

        public override bool LocksQueue(TProductionControl Control) => Control.ProductionCity() != null;

        public override int ActivityImage(TCity City) => 45;

        public override int? CitySequence(TCity City, int Row) => 45 + Row;

        public override string RealmActivity(TCity City) => "3";
    }

    public partial class TProductionControl
    {
        // The city this control produces for, null for a production place.
        public virtual TCity ProductionCity() => null;

        // The magic sphere of the production place, msNone for a city.
        public virtual TMagicSphere MagicSphere() => TMagicSphere.msNone;
    }
}

namespace Aow.Game.World.Structures
{
    public partial class TCityProductionControl
    {
        public override TCity ProductionCity() => FCity;
    }
}

namespace Aow.Game.World.Structures
{
    public partial class TProductionPlaceControl
    {
        public override TMagicSphere MagicSphere() => FProductionPlace.GetMagicSphere();
    }
}
