using System.Collections;
using System.Reflection;
using System.Runtime.ExceptionServices;

using System.Text.Json;
using Aow.Engine;
using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.Ui.Controls;
using Aow.Game.World.Map;

namespace Aow.Game.Tests;

[Collection("Engine")]
[EngineState]
public sealed class UiConstructionTests
{
    private sealed class ComponentOwner : Component
    {
        internal readonly List<string> Observations = [];

        public override void Notification(Component child, Component.Operation operation)
        {
            if (operation == Component.Operation.Insert) Observations.Add(Snapshot(child));
            base.Notification(child, operation);
        }
    }

    private sealed class Fixture : IDisposable
    {
        private readonly GameEngine previousEngine = GameTables.AoWEngine;
        private readonly AoWHSMap previousMap = AoWHSMap.Current;
        private readonly AoWCombatMap previousCombat = AoWCombatMap.Current;
        private readonly EventStore<HSMap> loaded;
        private readonly EventStore<HSMap> closed;

        internal Fixture()
        {
            if (EngineHost.Engines is null) EngineHost.Initialize();
            GameTables.AoWEngine = EngineFixture.Engine;
            loaded = GameTables.AoWEngine.FOnHSMLoadedStore;
            closed = GameTables.AoWEngine.FOnHSMCloseStore;
            GameTables.AoWEngine.FOnHSMLoadedStore = new EventStore<HSMap>();
            GameTables.AoWEngine.FOnHSMCloseStore = new EventStore<HSMap>();
            AoWHSMap.Current = null!;
            AoWCombatMap.Current = null!;
        }

        public void Dispose()
        {
            GameTables.AoWEngine.FOnHSMLoadedStore = loaded;
            GameTables.AoWEngine.FOnHSMCloseStore = closed;
            GameTables.AoWEngine = previousEngine;
            AoWHSMap.Current = previousMap;
            AoWCombatMap.Current = previousCombat;
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
            if (item is IList list) return list.Cast<object?>().Select((x, i) => State(x, path + "[" + i + "]")).ToArray();
            var fields = new SortedDictionary<string, object?>(StringComparer.Ordinal) { ["$type"] = type.FullName };
            for (Type? current = type; current is not null && current != typeof(object) && current != typeof(object); current = current.BaseType)
                foreach (FieldInfo field in current.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly).OrderBy(x => x.Name, StringComparer.Ordinal))
                {
                    if (field.Name == "eclass") continue; // the descriptor cache is not constructor state
                    string key = current.Name + "." + field.Name;
                    object? child = field.GetValue(item);
                    if (field.Name is "FOwner" or "<Owner>k__BackingField") { fields[key] = child is null ? null : "$owner"; continue; }
                    fields[key] = field.Name is "FEngine" or "FHSEngine" or "FGameEngine" ? child?.GetType().FullName : State(child, path + "." + key);
                }
            return fields;
        }
        return JsonSerializer.Serialize(State(value, "$"));
    }

    private static readonly string[] Names =
    [
        "Aow.Game.Ui.Dialogs.ChatWindow",
        "Aow.Game.Ui.Dialogs.ControlWin",
        "Aow.Game.Ui.Dialogs.DWin",
        "Aow.Game.Ui.Controls.AoWGauge",
        "Aow.Game.Draw.Images.InterfaceIL",
        "Aow.Game.Ui.Dialogs.MWindow",
        "Aow.Game.Ui.Windows.AOWMenu",
        "Aow.Game.Ui.Controls.AOWMenuBar",
        "Aow.Game.Ui.Dialogs.MultiWindow",
        "Aow.Game.Ui.Dialogs.PyreScreen",
        "Aow.Game.Ui.Dialogs.TitleWindow",
        "Aow.Game.Ui.Windows.AOWToolTip",
        "Aow.Game.Ui.Controls.AOWWinManager",
        "Aow.Game.Ui.Dialogs.ArenaDlg",
        "Aow.Game.Ui.Dialogs.ArmyEventDlg",
        "Aow.Game.Ui.Dialogs.ArmyInfoDlg",
        "Aow.Game.Ui.Controls.AoWArnoLine",
        "Aow.Game.Ui.Windows.AOWBaseWindow",
        "Aow.Game.Ui.Windows.AoWBookWindow",
        "Aow.Game.Ui.Controls.AOWBaseButton",
        "Aow.Game.Ui.Controls.AOWButton",
        "Aow.Game.Ui.Dialogs.CampaignJoinDlg",
        "Aow.Game.Ui.Dialogs.CampaignSettingsDlg",
        "Aow.Game.Ui.Dialogs.CampaignTransferDlg",
        "Aow.Game.Ui.Dialogs.CastUnitSpellDlg",
        "Aow.Game.Ui.Controls.AOWCheckBox",
        "Aow.Game.Ui.Dialogs.CityJoinDlg",
        "Aow.Game.Ui.Dialogs.CityRebelDlg",
        "Aow.Game.Ui.Dialogs.CityScreen",
        "Aow.Game.Ui.Controls.AOWComboBox",
        "Aow.Game.Ui.Controls.AoWComponent",
        "Aow.Game.Ui.Dialogs.ConstructDlg",
        "Aow.Game.Ui.Controls.AOWControl",
        "Aow.Game.Ui.Dialogs.DeformFaceDlg",
        "Aow.Game.Ui.Dialogs.DisjunctionDlg",
        "Aow.Game.Engine.AgeOfWondersEngine",
        "Aow.Game.Ui.Controls.AOWEdit",
        "Aow.Game.Ui.Dialogs.EmailGameSettingsDlg",
        "Aow.Game.Ui.Dialogs.EmailSettingsDlg",
        "Aow.Game.Ui.Dialogs.FileDlgWindow",
        "Aow.Game.Ui.Controls.AoWFrame",
        "Aow.Game.Ui.Dialogs.GamePlayDlg",
        "Aow.Game.Ui.Controls.AOWHScrollBar",
        "Aow.Game.Ui.Windows.TaowHTMLMemo",
        "Aow.Game.Ui.Dialogs.HeroInfoDlg",
        "Aow.Game.Ui.Dialogs.HeroJoinDlg",
        "Aow.Game.Ui.Dialogs.HeroUpgradeDlg",
        "Aow.Game.Ui.Controls.AOWImage",
        "Aow.Game.Ui.Dialogs.InfoBanner",
        "Aow.Game.Ui.Dialogs.IWindow",
        "Aow.Game.Ui.Dialogs.IntroScene",
        "Aow.Game.Ui.Dialogs.ItemBanner",
        "Aow.Game.Ui.Dialogs.ItemsFoundDlg",
        "Aow.Game.Ui.Controls.AOWLabel",
        "Aow.Game.Ui.Dialogs.LeaderSetupWin",
        "Aow.Game.Ui.Controls.AOWListBox",
        "Aow.Game.Ui.Dialogs.MagicWin",
        "Aow.Game.Ui.Controls.AOWMemo",
        "Aow.Game.Ui.Dialogs.MessageDlg",
        "Aow.Game.Ui.Dialogs.MsgReceiveDlg",
        "Aow.Game.Ui.Dialogs.NewTurnDlg",
        "Aow.Game.Ui.Dialogs.PInfoW",
        "Aow.Game.Ui.Dialogs.POP3Dlg",
        "Aow.Game.Ui.Dialogs.PopupWindows",
        "Aow.Game.Ui.Windows.AOWPanel",
        "Aow.Game.Ui.Dialogs.PartyJoinDlg",
        "Aow.Game.Ui.Dialogs.PasswordDlg",
        "Aow.Game.Ui.Dialogs.PowerDlg",
        "Aow.Game.Ui.Dialogs.ProductionScreen",
        "Aow.Game.Ui.Dialogs.ProgressDialog",
        "Aow.Game.Ui.Dialogs.RealmWin",
        "Aow.Game.Ui.Dialogs.RelWindow",
        "Aow.Game.Ui.Dialogs.ResultsWin",
        "Aow.Game.Ui.Dialogs.SMTPDlg",
        "Aow.Game.Net.MailMessage",
        "Aow.Game.Net.SmtpSender",
        "Aow.Game.Ui.Dialogs.SWindow",
        "Aow.Game.Ui.Dialogs.ScenSetup",
        "Aow.Game.Ui.Dialogs.SelFactionWin",
        "Aow.Game.Ui.Dialogs.SendEmailGameDlg",
        "Aow.Game.Session.SetupControl",
        "Aow.Game.Ui.Dialogs.SoundDlg",
        "Aow.Game.Ui.Dialogs.SpellBook",
        "Aow.Game.Ui.Dialogs.SynchronizeDlg",
        "Aow.Game.Ui.Dialogs.TaskBar",
        "Aow.Game.Ui.Dialogs.CombatScanner",
        "Aow.Game.Ui.Dialogs.CombatUnitInfo",
        "Aow.Game.Ui.Controls.AOWTabPanel",
        "Aow.Game.Ui.Dialogs.TargetSelectionDlg",
        "Aow.Game.Ui.Dialogs.TitleScene",
        "Aow.Game.Ui.Dialogs.TutorialWindow",
        "Aow.Game.Ui.Dialogs.UnitBanner",
        "Aow.Game.Ui.Dialogs.UnitSelectionDlg",
        "Aow.Game.Ui.Dialogs.UnitWindow",
        "Aow.Game.Ui.Controls.AOWScrollBar",
        "Aow.Game.Ui.Controls.AOWVScrollBar",
        "Aow.Game.Ui.Dialogs.VideoDialog",
        "Aow.Game.Ui.Controls.AOWWindowControl",
        "Aow.Game.Ui.Windows.AOWGenericWindow",
        "Aow.Game.Ui.Dialogs.General",
        "Aow.Game.Ui.Windows.AOWWindow",
        "Aow.Game.Ui.Dialogs.WizardsTowerDlg",
        "Aow.Game.Ui.Dialogs.WorldMap",
        "Aow.Game.Ui.Windows.AoWUserWindow",
        "Aow.Game.Ui.Grids.AoWCombatMapControl",
        "Aow.Game.Engine.Nodes.EComponent",
        "Aow.Game.Ui.Dialogs.FastCombatWindow",
        "Aow.Game.Ui.Dialogs.FontModule",
        "Aow.Game.World.Map.HSMapViewer",
        "Aow.Game.World.Map.HexagonSpriteEngine",
        "Aow.Game.Draw.HSLibrary",
        "Aow.Game.Ui.Grids.AoWMapControl",
        "Aow.Engine.PlugInManager",
        "Aow.Game.Ui.Grids.Scanner",
        "Aow.Game.Ui.Windows.StartMenu",
        "Aow.Game.Ui.Dialogs.CombatChoice",
        "Aow.Game.Ui.Grids.AOWTurnButton",
        "Aow.Game.Ui.Dialogs.HelpWindow",
        "Aow.Game.Ui.Dialogs.IntGfxMod",
        "Aow.Game.Ui.Dialogs.MapEvents",
        "Aow.Game.Ui.Dialogs.SetupEvents",
        "Aow.Game.Ui.Dialogs.CombatMapEvents",
        "Aow.Game.Ui.Controls.TaowUserDrawBox",
    ];

    public static IEnumerable<object[]> Classes => Names.Select(name => new object[] { name });

    [Fact]
    public void InheritOnlyLineHasItsOwnCompletedOwnerConstructor()
    {
        using var owner = new Component(null!);
        var line = new AoWLenLine(owner);
        Assert.Same(owner, line.Owner);
        Assert.Same(line, owner.Components[0]);
        Assert.True(line.FVisible);
        Assert.True(line.FFirstDraw);
        Assert.True(line.FCaptureInput);
        Assert.NotNull(line.FAlignment);
        Assert.NotNull(line.FChildren);
        Assert.NotNull(line.FToolTipText);
        line.Dispose();
        Assert.Equal(0, owner.ComponentCount);
    }

    private static Component Direct(Type type, Component? owner)
    {
        try
        {
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
            states.Add("subscriptions:" + GameTables.AoWEngine.FOnHSMLoadedStore.Count + ":" + GameTables.AoWEngine.FOnHSMCloseStore.Count);
            string actual = string.Join("\n", states);
            string key = name + ".owner" + owned;
            ConstructionSnapshot.Check("UiConstruction", key, actual);
        }
    }
}
