using System.Collections;
using System.Reflection;
using System.Runtime.ExceptionServices;

using System.Text.Json;
using Aow.Engine;
using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.UI.Dialogs;
using Aow.Game.World.Map;

namespace Aow.Game.Tests;

[Collection("Engine")]
[EngineState]
public sealed class UIConstructionTests
{
    private sealed class ComponentOwner : Component
    {
        internal List<string> Observations { get; } = [];

        public override void Notification(Component child, Component.Operation operation)
        {
            if (operation == Component.Operation.Insert) Observations.Add(Snapshot(child));
            base.Notification(child, operation);
        }
    }

    private sealed class Fixture : IDisposable
    {
        private readonly GameEngine _previousEngine = GameTables.GameEngine;
        private readonly StrategicMap _previousMap = StrategicMap.Current;
        private readonly CombatMap _previousCombat = CombatMap.Current;
        private readonly EventStore<GameMap> _loaded;
        private readonly EventStore<GameMap> _closed;

        internal Fixture()
        {
            if (EngineHost.Engines is null) EngineHost.Initialize();
            GameTables.GameEngine = EngineFixture.Engine;
            _loaded = GameTables.GameEngine.OnHsmLoadedStore;
            _closed = GameTables.GameEngine.OnHsmCloseStore;
            GameTables.GameEngine.OnHsmLoadedStore = new EventStore<GameMap>();
            GameTables.GameEngine.OnHsmCloseStore = new EventStore<GameMap>();
            StrategicMap.Current = null!;
            CombatMap.Current = null!;
        }

        public void Dispose()
        {
            GameTables.GameEngine.OnHsmLoadedStore = _loaded;
            GameTables.GameEngine.OnHsmCloseStore = _closed;
            GameTables.GameEngine = _previousEngine;
            StrategicMap.Current = _previousMap;
            CombatMap.Current = _previousCombat;
        }
    }

    private static string Snapshot(object value)
    {
        var visited = new Dictionary<object, string>(ReferenceEqualityComparer.Instance);
        object? State(object? item, string path)
        {
            if (item is null) return null;
            Type type = item.GetType();
            if (item is string text) return text;
            if (type.IsEnum) return Convert.ToInt64(item);
            if (type.IsPrimitive || item is string || item is decimal) return item;
            if (item is Type represented) return represented.FullName;
            if (item is Delegate handler) return handler.GetInvocationList().Select(x => x.Method.DeclaringType!.FullName + "." + x.Method.Name).ToArray();
            if (!type.IsValueType)
            {
                if (visited.TryGetValue(item, out string? found)) return "$ref:" + found;
                visited.Add(item, path);
            }
            if (item is Array array) return array.Cast<object?>().Select((x, i) => State(x, path + "[" + i + "]")).ToArray();
            if (InlineArrays.Elements(item) is { } elements) return elements.Select((x, i) => State(x, path + "[" + i + "]")).ToArray();
            if (item is IList list) return list.Cast<object?>().Select((x, i) => State(x, path + "[" + i + "]")).ToArray();
            var fields = new SortedDictionary<string, object?>(StringComparer.Ordinal) { ["$type"] = type.FullName };
            for (Type? current = type; current is not null && current != typeof(object) && current != typeof(object); current = current.BaseType)
                foreach (FieldInfo field in current.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly).OrderBy(MemberNames.Of, StringComparer.Ordinal))
                {
                    if (field.Name == "_engineClass") continue; // the descriptor cache is not constructor state
                    string key = current.Name + "." + MemberNames.Of(field);
                    object? child = field.GetValue(item);
                    if (field.Name is "_owner" or "<Owner>k__BackingField") { fields[key] = child is null ? null : "$owner"; continue; }
                    if (child is UIContext) { fields[key] = child.GetType().FullName; continue; }
                    fields[key] = field.Name is "_engine" or "_hexSpriteEngine" or "FGameEngine" ? child?.GetType().FullName : State(child, path + "." + key);
                }
            return fields;
        }
        return JsonSerializer.Serialize(State(value, "$"));
    }

    private static readonly string[] s_names =
    [
        "Aow.Game.UI.Dialogs.ChatWindow",
        "Aow.Game.UI.Dialogs.ControlWindow",
        "Aow.Game.UI.Dialogs.ConfirmationDialog",
        "Aow.Game.UI.Controls.Gauge",
        "Aow.Game.Draw.Images.InterfaceImageLibrary",
        "Aow.Game.UI.Dialogs.MapWindow",
        "Aow.Game.UI.Windows.Menu",
        "Aow.Game.UI.Controls.MenuBar",
        "Aow.Game.UI.Dialogs.MultiWindow",
        "Aow.Game.UI.Dialogs.PyreScreen",
        "Aow.Game.UI.Dialogs.TitleWindow",
        "Aow.Game.UI.Windows.ToolTip",
        "Aow.Game.UI.Controls.WindowManager",
        "Aow.Game.UI.Dialogs.ArenaDialog",
        "Aow.Game.UI.Dialogs.ArmyEventDialog",
        "Aow.Game.UI.Dialogs.ArmyInfoDialog",
        "Aow.Game.UI.Controls.ArnoLine",
        "Aow.Game.UI.Windows.BaseWindow",
        "Aow.Game.UI.Windows.BookWindow",
        "Aow.Game.UI.Controls.BaseButton",
        "Aow.Game.UI.Controls.Button",
        "Aow.Game.UI.Dialogs.CampaignJoinDialog",
        "Aow.Game.UI.Dialogs.CampaignSettingsDialog",
        "Aow.Game.UI.Dialogs.CampaignTransferDialog",
        "Aow.Game.UI.Dialogs.CastUnitSpellDialog",
        "Aow.Game.UI.Controls.CheckBox",
        "Aow.Game.UI.Dialogs.CityJoinDialog",
        "Aow.Game.UI.Dialogs.CityRebelDialog",
        "Aow.Game.UI.Dialogs.CityScreen",
        "Aow.Game.UI.Controls.ComboBox",
        "Aow.Game.UI.Controls.UIComponent",
        "Aow.Game.UI.Dialogs.ConstructDialog",
        "Aow.Game.UI.Controls.Control",
        "Aow.Game.UI.Dialogs.DeformFaceDialog",
        "Aow.Game.UI.Dialogs.DisjunctionDialog",
        "Aow.Game.UI.Controls.Edit",
        "Aow.Game.UI.Dialogs.EmailGameSettingsDialog",
        "Aow.Game.UI.Dialogs.EmailSettingsDialog",
        "Aow.Game.UI.Dialogs.FileDialogWindow",
        "Aow.Game.UI.Controls.Frame",
        "Aow.Game.UI.Dialogs.GamePlayDialog",
        "Aow.Game.UI.Controls.HorizontalScrollBar",
        "Aow.Game.UI.Windows.HtmlMemo",
        "Aow.Game.UI.Dialogs.HeroInfoDialog",
        "Aow.Game.UI.Dialogs.HeroJoinDialog",
        "Aow.Game.UI.Dialogs.HeroUpgradeDialog",
        "Aow.Game.UI.Controls.Image",
        "Aow.Game.UI.Dialogs.InfoBanner",
        "Aow.Game.UI.Dialogs.InfoWindow",
        "Aow.Game.UI.Dialogs.IntroScene",
        "Aow.Game.UI.Dialogs.ItemBanner",
        "Aow.Game.UI.Dialogs.ItemsFoundDialog",
        "Aow.Game.UI.Controls.Label",
        "Aow.Game.UI.Dialogs.LeaderSetupWindow",
        "Aow.Game.UI.Controls.ListBox",
        "Aow.Game.UI.Dialogs.MagicWindow",
        "Aow.Game.UI.Controls.Memo",
        "Aow.Game.UI.Dialogs.MessageDialog",
        "Aow.Game.UI.Dialogs.MessageReceiveDialog",
        "Aow.Game.UI.Dialogs.NewTurnDialog",
        "Aow.Game.UI.Dialogs.PlayerInfoWindow",
        "Aow.Game.UI.Dialogs.Pop3Dialog",
        "Aow.Game.UI.Dialogs.PopupWindows",
        "Aow.Game.UI.Windows.Panel",
        "Aow.Game.UI.Dialogs.PartyJoinDialog",
        "Aow.Game.UI.Dialogs.PasswordDialog",
        "Aow.Game.UI.Dialogs.PowerDialog",
        "Aow.Game.UI.Dialogs.ProductionScreen",
        "Aow.Game.UI.Dialogs.ProgressDialog",
        "Aow.Game.UI.Dialogs.RealmWindow",
        "Aow.Game.UI.Dialogs.RelationWindow",
        "Aow.Game.UI.Dialogs.ResultsWindow",
        "Aow.Game.UI.Dialogs.SmtpDialog",
        "Aow.Game.Net.MailMessage",
        "Aow.Game.Net.SmtpSender",
        "Aow.Game.UI.Dialogs.ScannerWindow",
        "Aow.Game.UI.Dialogs.ScenarioSetup",
        "Aow.Game.UI.Dialogs.FactionSelectionWindow",
        "Aow.Game.UI.Dialogs.SendEmailGameDialog",
        "Aow.Game.Session.SetupControl",
        "Aow.Game.UI.Dialogs.SoundDialog",
        "Aow.Game.UI.Dialogs.SpellBook",
        "Aow.Game.UI.Dialogs.SynchronizeDialog",
        "Aow.Game.UI.Dialogs.TaskBar",
        "Aow.Game.UI.Dialogs.CombatScanner",
        "Aow.Game.UI.Dialogs.CombatUnitInfo",
        "Aow.Game.UI.Controls.TabPanel",
        "Aow.Game.UI.Dialogs.TargetSelectionDialog",
        "Aow.Game.UI.Dialogs.TitleScene",
        "Aow.Game.UI.Dialogs.TutorialWindow",
        "Aow.Game.UI.Dialogs.UnitBanner",
        "Aow.Game.UI.Dialogs.UnitSelectionDialog",
        "Aow.Game.UI.Dialogs.UnitWindow",
        "Aow.Game.UI.Controls.ScrollBar",
        "Aow.Game.UI.Controls.VerticalScrollBar",
        "Aow.Game.UI.Dialogs.VideoDialog",
        "Aow.Game.UI.Controls.WindowControl",
        "Aow.Game.UI.Windows.GenericWindow",
        "Aow.Game.UI.Dialogs.General",
        "Aow.Game.UI.Windows.Window",
        "Aow.Game.UI.Dialogs.WizardsTowerDialog",
        "Aow.Game.UI.Dialogs.WorldMap",
        "Aow.Game.UI.Windows.UserWindow",
        "Aow.Game.UI.Grids.CombatMapControl",
        "Aow.Game.UI.Dialogs.FastBattleWindow",
        "Aow.Game.UI.Dialogs.UIFonts",
        "Aow.Game.World.Map.HexSpriteMapViewer",
        "Aow.Game.UI.Grids.MapControl",
        "Aow.Game.UI.Grids.Scanner",
        "Aow.Game.UI.Windows.StartMenu",
        "Aow.Game.UI.Dialogs.CombatChoice",
        "Aow.Game.UI.Grids.TurnButton",
        "Aow.Game.UI.Dialogs.HelpWindow",
        "Aow.Game.UI.Dialogs.UIGraphics",
        "Aow.Game.UI.Dialogs.MapEvents",
        "Aow.Game.UI.Dialogs.SetupEvents",
        "Aow.Game.UI.Dialogs.CombatMapEvents",
        "Aow.Game.UI.Controls.UserDrawBox",
    ];

    public static IEnumerable<object[]> Classes => s_names.Select(name => new object[] { name });

    // A dialog module takes the UI context and the two resource modules take nothing (issue 196).
    // A data module has no owner.
    private static Component Direct(Type type, Component? owner)
    {
        try
        {
            if (typeof(DataModule).IsAssignableFrom(type))
            {
                if (owner is not null) throw new InvalidOperationException(type.FullName + " takes no owner.");
                if (type.GetConstructor([typeof(UIContext)]) is { } module) return (Component)module.Invoke([new UIContext()]);
                if (type.GetConstructor(Type.EmptyTypes) is { } resource) return (Component)resource.Invoke([]);
            }
            var constructor = type.GetConstructor([typeof(Component)]);
            if (constructor is not null) return (Component)constructor.Invoke([owner]);
            var factory = type.GetMethod("Create", BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly, [typeof(Component)]);
            if (factory is not null) return (Component)factory.Invoke(null, [owner])!;
            throw new InvalidOperationException(type.FullName + " has no completed owner entry.");
        }
        catch (TargetInvocationException error) when (error.InnerException is not null)
        {
            ExceptionDispatchInfo.Capture(error.InnerException).Throw();
            throw;
        }
    }

    [Theory]
    [MemberData(nameof(Classes))]
    public void ConstructorStateOwnerObservationAndTeardownMatchBaseCommit(string name)
    {
        Type type = typeof(Spell).Assembly.GetType(name)!;
        for (int owned = 0; owned < 2; owned++)
        {
            using var fixture = new Fixture();
            var owner = owned == 0 ? null : new ComponentOwner();
            var states = new List<string>();
            Component? value = null;
            try
            {
                value = Direct(type, owner);
                Assert.IsType(type, value);
                Assert.Same(owner, value.Owner);
                states.Add("success:" + Snapshot(value));
            }
            catch (Exception error) { states.Add("failure:" + error.GetType().FullName + ":" + error.Message); }
            finally
            {
                try { value?.Dispose(); }
                catch (Exception error) { states.Add("destroy:" + error.GetType().FullName + ":" + error.Message); }
            }
            states.Add("owner:" + (owner?.ComponentCount ?? 0));
            if (owner is not null) states.Add(JsonSerializer.Serialize(owner.Observations));
            states.Add("subscriptions:" + GameTables.GameEngine.OnHsmLoadedStore.Count + ":" + GameTables.GameEngine.OnHsmCloseStore.Count);
            string actual = string.Join("\n", states);
            string key = name + ".owner" + owned;
            ConstructionSnapshot.Check("UiConstruction", key, actual);
        }
    }
}
