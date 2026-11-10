using Aow.Game.Draw;
using Aow.Game.Events;
using Aow.Game.Magic;
using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Game.Units;
using Aow.Game.World.Structures;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

// What a production item orders: produce a unit, research one, trade, or one of the city actions.
// Each kind holds its own fields and does what the switches over the production id and its city
// action did, arm by arm (Structur.PAS, City.pas, PrdPlace.pas and the three screens). The original
// format stores an order as three integers (TProductionItem properties 20 to 22), and ToFields and
// FromFields convert between the two. A mod order registers an id of its own and overrides these.

namespace Aow.Game.World.Structures
{
    public abstract partial record ProductionOrder
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
        public virtual CityAction CityAction => Structures.CityAction.None;

        // The unit production the screens draw for this order, or null.
        public virtual int? UnitProductionIndex => null;

        // The queue line (Structur.PAS:840).
        public virtual string Name(ProductionControl Control) => Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameTables.UnknownRStr);

        // The line of the running order (Structur.PAS:865).
        public virtual string NameProgressive(ProductionControl Control) => Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameTables.UnknownRStr);

        // Whether Control can start the order now, with the reason when it cannot (Structur.PAS:361).
        public virtual bool CanExecute(ProductionControl Control, ref string Reason)
        {
            Waiting(Control, ref Reason);
            return false;
        }

        // Starts the order inside the control's update bracket (Structur.PAS:392).
        public virtual void Execute(ProductionControl Control)
        {
        }

        // Starts the order once the control's update bracket closed (City.pas:434, PrdPlace.pas:238).
        public virtual void Executed(ProductionControl Control)
        {
        }

        // The order's turns ran out in the turn of Player (Structur.PAS:804).
        public virtual void Complete(ProductionControl Control, sbyte Player)
        {
        }

        // The order is still running with no turns left after Complete (City.pas:496, PrdPlace.pas:231).
        public virtual void Completed(ProductionControl Control)
        {
        }

        // The running order is cancelled, before the control refunds it (City.pas:573).
        public virtual void Cancel(ProductionControl Control)
        {
        }

        // Whether a looping queue puts the order back at its end (Structur.PAS:837, City.pas:604).
        public virtual bool Loopable(ProductionControl Control) => true;

        // Whether the order in the queue bars further entries (Structur.PAS:834, City.pas:595).
        public virtual bool LocksQueue(ProductionControl Control) => false;

        // The city screen's activity image (AoWCityScreen.pas:904).
        public virtual int ActivityImage(City City) => 40;

        // The production screen's activity image (AoWProdScreen.pas:719).
        public virtual int ActivityImage(ProductionPlace Place) => 4;

        // The image sequence of City's map image while the order runs, Row 0 to 2 from the top
        // (City.pas:2503, 2539, 2575): null for the race image, -1 for no image.
        public virtual int? CitySequence(City City, int Row) => null;

        // The activity column of the realm screen (AoWRealm.pas).
        public virtual string RealmActivity(City City) => "-";

        // The info window's lines for the order at the head of Control's queue (AoWInfoWin.pas:439, 496).
        public virtual void ActivityLines(ProductionControl Control, StringCollection Lines)
        {
            Lines.Text = NameProgressive(Control);
        }

        // The base control's waiting note, which every arm of the base control sets first. The
        // original continues checking costs after assigning the waiting text.
        protected static void Waiting(ProductionControl Control, ref string Reason)
        {
            if (Control.FStatus == ProductionControl.Status.Waiting)
            {
                Reason = "(" + LowerCase(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(WaitingRStr)) + ")";
            }
        }

        // Changes the relation of City's race to the control's player by Change.
        protected static void ChangeRelation(ProductionControl Control, City City, int Change)
        {
            var playerRelationValue = Aow.Game.World.Map.AoWHSMap.Current.FRaces.GetRace(City.FRace).GetPlayerRelationValue(Control.GetPlayer());
            Aow.Game.World.Map.AoWHSMap.Current.FRaces.GetRace(City.FRace).SetPlayerRelationValue(Control.GetPlayer(), playerRelationValue + Change);
        }

        // Adds Text at City to the logbook of the control's player.
        protected static void Log(ProductionControl Control, City City, string Text)
        {
            var Event = new CityProductionEventLog(null);
            Event.FPlayer = Control.GetPlayer();
            Event.FText = Text;
            var lhx = City.GetLhx();
            var yhx = City.GetYhx();
            Event.SetLocation(City.GetXhx(), yhx, lhx);
            Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(Event.FPlayer).FEventLogbook.AddEvent(Event);
            Event.Release();
        }

        // Charges the settings' costs to the settings' player.
        protected static void Pay(ProductionControl Control)
        {
            var player = Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(Control.FSettings.FPlayer);
            player.SetGems(player.FGems - Control.FSettings.FGold);
            player.FMagicControl.SetMana(player.FMagicControl.FMana - Control.FSettings.FMana);
        }

        private static readonly string WaitingRStr = "Waiting";
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

        public override CityAction CityAction => Structures.CityAction.Produce;

        public override int? UnitProductionIndex => Index;

        public override string Name(ProductionControl Control)
        {
            var production = Control.FUnitProductions.GetItem(Index);
            if (production == null)
            {
                return Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameTables.UnknownRStr);
            }

            var format = Control.UnitProductionGold(Index) != 0 ? GameText.ProduceXRStr : GameText.SummonXRStr;
            var unitName = Aow.Game.Engine.AoWHSSet.Current.FUnitResources.GetUnitResource(production.FUnitIndex).GetUnitName();
            return Aow.Game.Magic.Abilities.AbilityRules.Format(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(format), [unitName]);
        }

        public override string NameProgressive(ProductionControl Control)
        {
            var production = Control.FUnitProductions.GetItem(Index);
            if (production == null)
            {
                return Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameTables.UnknownRStr);
            }

            var format = Control.UnitProductionGold(Index) != 0 ? GameText.ProducingXRStr : GameText.SummoningXRStr;
            var unitName = Aow.Game.Engine.AoWHSSet.Current.FUnitResources.GetUnitResource(production.FUnitIndex).GetUnitName();
            return Aow.Game.Magic.Abilities.AbilityRules.Format(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(format), [unitName]);
        }

        public override bool CanExecute(ProductionControl Control, ref string Reason)
        {
            bool Result;
            Waiting(Control, ref Reason);
            var players = Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(Control.GetPlayer());
            if (Control.UnitProductionGold(Index) <= players.FGems)
            {
                var fPlayersPlayers = Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(Control.GetPlayer());
                if (Control.UnitProductionMana(Index) <= fPlayersPlayers.FMagicControl.FMana)
                {
                    Result = true;
                }
                else
                {
                    Result = false;
                    Reason = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameText.NotEnoughManaRStr);
                }
            }
            else
            {
                Result = false;
                Reason = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameText.NotEnoughGoldRStr);
            }

            return Result;
        }

        public override void Execute(ProductionControl Control)
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
            Control.FStatus = ProductionControl.Status.Producing;
            if (settings.FGroupID != 0)
            {
                Control.RegisterGroupProduction();
            }
        }

        public override void Complete(ProductionControl Control, sbyte Player)
        {
            if (Control.UnitProduced())
            {
                if (Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(Player).FPlayerType != PlayerType.Human && Control.FSettings.FGroupID != 0)
                {
                    Control.GroupProductionProduced(Control.FSettings.FUnit, Control.FSettings.FGroupID);
                }

                Control.FSettings.Clear();
                Control.ProductionDone();
            }
        }

        public override int ActivityImage(City City) => 4;

        public override string RealmActivity(City City) => "5";

        public override void ActivityLines(ProductionControl Control, StringCollection Lines)
        {
            var unit = Control.CreateUnitProduction(Index);
            Lines.Clear();
            Lines.Add(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameText.ProducingRStr));
            Lines.Add(unit.GetName());
            unit.Release();
        }
    }

    // Production id 2: research unit production Index.
    public sealed partial record ResearchSpell(int Index) : ProductionOrder
    {
        public const int Kind = 2;

        public override (int Id, int Parameter, int GroupId) ToFields() => (Kind, Index, 0);

        public override CityAction CityAction => Structures.CityAction.Research;

        public override int? UnitProductionIndex => Index;

        public override string Name(ProductionControl Control)
        {
            var production = Control.FUnitProductions.GetItem(Index);
            if (production == null)
            {
                return Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameTables.UnknownRStr);
            }

            var unitName = Aow.Game.Engine.AoWHSSet.Current.FUnitResources.GetUnitResource(production.FUnitIndex).GetUnitName();
            return Aow.Game.Magic.Abilities.AbilityRules.Format(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameText.InstallXRStr), [unitName]);
        }

        public override string NameProgressive(ProductionControl Control)
        {
            var production = Control.FUnitProductions.GetItem(Index);
            if (production == null)
            {
                return Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameTables.UnknownRStr);
            }

            var unitName = Aow.Game.Engine.AoWHSSet.Current.FUnitResources.GetUnitResource(production.FUnitIndex).GetUnitName();
            return Aow.Game.Magic.Abilities.AbilityRules.Format(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(InstallingXRStr), [unitName]);
        }

        public override bool CanExecute(ProductionControl Control, ref string Reason)
        {
            Waiting(Control, ref Reason);
            var players = Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(Control.GetPlayer());
            if (Control.UnitResearchGold(Index) <= players.FGems)
            {
                return true;
            }

            Reason = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameText.NotEnoughGoldRStr);
            return false;
        }

        public override void Execute(ProductionControl Control)
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
            Control.FStatus = ProductionControl.Status.Producing;
        }

        public override void Complete(ProductionControl Control, sbyte Player)
        {
            Control.UnitResearched();
            Control.FSettings.Clear();
            Control.ProductionDone();
        }

        public override bool Loopable(ProductionControl Control) => false;

        public override int ActivityImage(City City) => 5;

        public override int? CitySequence(City City, int Row) => -1;

        public override string RealmActivity(City City) => "5";

        public override void ActivityLines(ProductionControl Control, StringCollection Lines)
        {
            var unit = Control.CreateUnitProduction(Index);
            Lines.Add(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(InstallingRStr));
            Lines.Add(unit.GetName());
            unit.Release();
        }

        private static readonly string InstallingRStr = "Installing";
        private static readonly string InstallingXRStr = "Installing %s";
    }

    // Production id 10: trade for a turn, in a city or a production place. TurnsTotal is 1 for a
    // queued trade and -1 for the default one (City.pas:1475, PrdPlace.pas:511). Both controls
    // trade alike, a power node names it after the power it channels.
    public sealed partial record Trade(int TurnsTotal) : ProductionOrder
    {
        public const int Kind = 10;

        public override (int Id, int Parameter, int GroupId) ToFields() => (Kind, TurnsTotal, 0);

        public override CityAction CityAction => Structures.CityAction.Trade;

        public override string Name(ProductionControl Control) => Control.MagicSphere() != MagicSphere.None
            ? Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GenerateExtraPowerRStr)
            : Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameText.ProduceMerchandiseRStr);

        public override string NameProgressive(ProductionControl Control) => Control.MagicSphere() != MagicSphere.None
            ? Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameText.GeneratingExtraPowerRStr)
            : Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameText.ProducingMerchandiseRStr);

        public override bool CanExecute(ProductionControl Control, ref string Reason) => true;

        public override void Executed(ProductionControl Control)
        {
            Control.FSettings.FTurnsTotal = (sbyte)TurnsTotal;
            Control.FSettings.FTurnsLeft = 1;
            Control.FSettings.FPlayer = Control.GetPlayer();
            Control.FStatus = ProductionControl.Status.Producing;
        }

        public override void Completed(ProductionControl Control)
        {
            Control.ProductionDone();
        }

        public override int ActivityImage(ProductionPlace Place) => Place.GetMagicSphere() != MagicSphere.None ? 48 : 47;

        internal static readonly string GenerateExtraPowerRStr = "Generate Extra Power";
    }

    // Production id 11: raise the city's upgrade level.
    public sealed partial record Upgrade : ProductionOrder
    {
        public const int Kind = 11;

        public override (int Id, int Parameter, int GroupId) ToFields() => (Kind, 0, 0);

        public override CityAction CityAction => Structures.CityAction.Upgrade;

        public override string Name(ProductionControl Control) =>
            Control.ProductionCity() != null ? Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameText.UpgradeRStr) : base.Name(Control);

        public override string NameProgressive(ProductionControl Control) =>
            Control.ProductionCity() != null ? Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(UpgradingRStr) : base.NameProgressive(Control);

        public override bool CanExecute(ProductionControl Control, ref string Reason)
        {
            var city = Control.ProductionCity();
            if (city == null)
            {
                return base.CanExecute(Control, ref Reason);
            }

            if (Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(Control.GetPlayer()).FGems >= city.UpgradeCost())
            {
                return true;
            }

            Reason = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameText.NotEnoughGoldRStr);
            return false;
        }

        public override void Executed(ProductionControl Control)
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
            var player = Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(Control.GetPlayer());
            Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(Control.GetPlayer()).SetGems(player.FGems - Control.FSettings.FGold);
            Control.FStatus = ProductionControl.Status.Producing;
        }

        public override void Completed(ProductionControl Control)
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
            Log(Control, city, RtlUnit.Format(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(XUpgradedRStr), [city.GetName()]));
            Control.EndUpdate();
        }

        public override bool Loopable(ProductionControl Control) => Control.ProductionCity() == null;

        public override int ActivityImage(City City) => 43;

        public override int? CitySequence(City City, int Row) => 33 + Row;

        public override string RealmActivity(City City) => "2";

        private static readonly string UpgradingRStr = "Upgrading";
        private static readonly string XUpgradedRStr = "%s upgraded";
    }

    // Production id 12: build the city's next wall.
    public sealed partial record Fortify : ProductionOrder
    {
        public const int Kind = 12;

        public override (int Id, int Parameter, int GroupId) ToFields() => (Kind, 0, 0);

        public override CityAction CityAction => Structures.CityAction.Fortify;

        public override string Name(ProductionControl Control) =>
            Control.ProductionCity() != null ? Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameText.FortifyRStr) : base.Name(Control);

        public override string NameProgressive(ProductionControl Control) =>
            Control.ProductionCity() != null ? Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(FortifyingRStr) : base.NameProgressive(Control);

        public override bool CanExecute(ProductionControl Control, ref string Reason)
        {
            var city = Control.ProductionCity();
            if (city == null)
            {
                return base.CanExecute(Control, ref Reason);
            }

            if (Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(Control.GetPlayer()).FGems >= city.FortifyCost())
            {
                return true;
            }

            Reason = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameText.NotEnoughGoldRStr);
            return false;
        }

        public override void Executed(ProductionControl Control)
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
            var playerGems = Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(Control.GetPlayer());
            Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(Control.GetPlayer()).SetGems(playerGems.FGems - Control.FSettings.FGold);
            Control.FStatus = ProductionControl.Status.Producing;
        }

        public override void Completed(ProductionControl Control)
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

            Log(Control, city, RtlUnit.Format(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(CityFortifiedRStr), [city.GetName()]));
            city.UpdateImages();
            ChangeRelation(Control, city, 5);
            Control.ProductionDone();
            Control.EndUpdate();
        }

        public override bool Loopable(ProductionControl Control) => Control.ProductionCity() == null;

        public override int ActivityImage(City City) => City.FWallType == 0 ? 41 : 42;

        public override int? CitySequence(City City, int Row) => City.FWallType switch
        {
            0 => 30 + Row,
            1 => 48 + Row,
            _ => -1,
        };

        public override string RealmActivity(City City) => City.FWallType == 1 ? "1" : "0";

        private static readonly string CityFortifiedRStr = "%s fortified";
        private static readonly string FortifyingRStr = "Fortifying";
    }

    // Production id 13: the city's people move out and Race moves in.
    public sealed partial record Migrate(int Race) : ProductionOrder
    {
        public const int Kind = 13;

        public override (int Id, int Parameter, int GroupId) ToFields() => (Kind, Race, 0);

        public override CityAction CityAction => Structures.CityAction.Migrate;

        public override string Name(ProductionControl Control) => Control.ProductionCity() != null
            ? RtlUnit.Format(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(MigrateToXRStr), [Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameTables.RaceNamesPluralRStr[Race])])
            : base.Name(Control);

        public override string NameProgressive(ProductionControl Control) => Control.ProductionCity() != null
            ? RtlUnit.Format(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(MigratingToXRStr), [Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameTables.RaceNamesPluralRStr[Race])])
            : base.NameProgressive(Control);

        public override bool CanExecute(ProductionControl Control, ref string Reason)
        {
            var city = Control.ProductionCity();
            return city != null ? city.CanMigrate((sbyte)Race, ref Reason) : base.CanExecute(Control, ref Reason);
        }

        public override void Executed(ProductionControl Control)
        {
            var city = Control.ProductionCity();
            if (city == null)
            {
                return;
            }

            Control.FSettings.FTurnsTotal = (sbyte)city.MigrationTime((sbyte)Race);
            Control.FSettings.FTurnsLeft = Control.FSettings.FTurnsTotal;
            Control.FSettings.FPlayer = Control.GetPlayer();
            Control.FStatus = ProductionControl.Status.Producing;
            ChangeRelation(Control, city, -15);
        }

        public override void Completed(ProductionControl Control)
        {
            var city = Control.ProductionCity();
            if (city == null)
            {
                return;
            }

            Control.BeginUpdate();
            var raceNamesPlural = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameTables.RaceNamesPluralRStr[Race]);
            Log(Control, city, RtlUnit.Format(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(CityMigratedToRStr), [city.GetName(), raceNamesPlural]));
            city.SetRace((sbyte)Race);
            ChangeRelation(Control, city, 10);
            Control.FUnitProductions.Clear();
            city.SetUpgradeLevel(city.FUpgradeLevel);
            Control.ProductionDone();
            Control.EndUpdate();
        }

        public override void Cancel(ProductionControl Control)
        {
            var city = Control.ProductionCity();
            if (city != null)
            {
                ChangeRelation(Control, city, Control.FSettings.FTurnsLeft == Control.FSettings.FTurnsTotal ? 15 : 10);
            }
        }

        public override bool Loopable(ProductionControl Control) => Control.ProductionCity() == null;

        public override bool LocksQueue(ProductionControl Control) => Control.ProductionCity() != null;

        public override int ActivityImage(City City) => 44;

        public override int? CitySequence(City City, int Row) => 39 + Row;

        public override string RealmActivity(City City) => "4";

        private static readonly string CityMigratedToRStr = "%0:s migrated to %1:s";
        internal static readonly string MigrateToXRStr = "Migrate to %s";
        private static readonly string MigratingToXRStr = "Migrating to %s";
    }

    // Production id 14: loot the city and raze it.
    public sealed partial record Loot : ProductionOrder
    {
        public const int Kind = 14;

        public override (int Id, int Parameter, int GroupId) ToFields() => (Kind, 0, 0);

        public override CityAction CityAction => Structures.CityAction.Loot;

        public override string Name(ProductionControl Control) =>
            Control.ProductionCity() != null ? Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameText.LootRStr) : base.Name(Control);

        public override string NameProgressive(ProductionControl Control) =>
            Control.ProductionCity() != null ? Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(LootingRStr) : base.NameProgressive(Control);

        public override bool CanExecute(ProductionControl Control, ref string Reason)
        {
            var city = Control.ProductionCity();
            return city != null ? city.CanLoot(ref Reason) : base.CanExecute(Control, ref Reason);
        }

        public override void Executed(ProductionControl Control)
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
            var Log = new LocationTextEventLog(null);
            Log.FPlayer = Control.GetPlayer();
            Log.FText = RtlUnit.Format(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(CityLootingRStr), [city.GetName()]);
            var lhx = city.GetLhx();
            var yhx = city.GetYhx();
            Log.SetLocation(city.GetXhx(), yhx, lhx);
            var cityYhx = city.GetYhx();
            var cityLhx = city.GetLhx();
            var xhx = city.GetXhx();
            Aow.Game.Events.EventLog.DistributeLocationEventLog(Log, Control.GetPlayer(), xhx, cityYhx, cityLhx, false);
            Log.Release();
            city.FOverlay.SetMode(CityOverlayHS.CityOverlayMode.Looting);
            Control.FStatus = ProductionControl.Status.Producing;
        }

        public override void Completed(ProductionControl Control)
        {
            var city = Control.ProductionCity();
            if (city == null)
            {
                return;
            }

            Control.BeginUpdate();
            Log(Control, city, RtlUnit.Format(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(CityLootedRStr), [city.GetName(), IntToStr(city.FIncome * 3)]));
            var playerGems = Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(Control.GetPlayer());
            Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(Control.GetPlayer()).SetGems(playerGems.FGems + city.FIncome * 3);
            city.FOverlay.SetMode(CityOverlayHS.CityOverlayMode.Idle);
            city.SetRazed(true);
            Control.EndUpdate();
        }

        public override void Cancel(ProductionControl Control)
        {
            var city = Control.ProductionCity();
            if (city != null)
            {
                ChangeRelation(Control, city, Control.FSettings.FTurnsLeft == Control.FSettings.FTurnsTotal ? 30 : 20);
            }
        }

        public override bool Loopable(ProductionControl Control) => Control.ProductionCity() == null;

        public override bool LocksQueue(ProductionControl Control) => Control.ProductionCity() != null;

        public override int ActivityImage(City City) => 45;

        public override int? CitySequence(City City, int Row) => 45 + Row;

        public override string RealmActivity(City City) => "3";

        private static readonly string CityLootedRStr = "%0:s looted (%1:s gold gained)";
        private static readonly string CityLootingRStr = "%s is being looted";
        private static readonly string LootingRStr = "Looting";
    }

    public partial class ProductionControl
    {
        // The city this control produces for, null for a production place.
        public virtual City ProductionCity() => null;

        // The magic sphere of the production place, msNone for a city.
        public virtual MagicSphere MagicSphere() => Magic.MagicSphere.None;
    }
}

namespace Aow.Game.World.Structures
{
    public partial class CityProductionControl
    {
        public override City ProductionCity() => FCity;
    }
}

namespace Aow.Game.World.Structures
{
    public partial class ProductionPlaceControl
    {
        public override MagicSphere MagicSphere() => FProductionPlace.GetMagicSphere();
    }
}
