using System.Collections;
using System.Reflection;
using System.Text;
using System.Text.Json;
using Aow.Game.Engine;
using Aow.Game.Engine.Nodes;
using Aow.Game.Magic.Spells;
using Aow.Game.Ui.Controls;
using Aow.Game.World.Map;

namespace Aow.Units.Tests;

[Collection("Engine")]
[EngineState]
public sealed class UiConstructionTests
{
    private sealed class ComponentOwner : TComponent
    {
        internal readonly List<string> Observations = [];

        public override void Notification(TComponent child, TOperation operation)
        {
            if (operation == TOperation.opInsert) Observations.Add(Snapshot(child));
            base.Notification(child, operation);
        }
    }

    private sealed class Fixture : IDisposable
    {
        private readonly TAoWEngine previousEngine = Aow.Game.Magic.Abilities.AoWEUnit.AoWEngine;
        private readonly TAoWHSMap previousMap = Aow.Game.Magic.Abilities.AoWEUnit.AoWHSMap;
        private readonly Aow.Game.Combat.TAoWCombatMap previousCombat = Aow.Game.Combat.AoWTCUnit.AoWCombatMap;
        private readonly EventStore<THSMap> loaded;
        private readonly EventStore<THSMap> closed;

        internal Fixture()
        {
            if (EngineUnit.Engines is null) EngineUnit.Initialize();
            Aow.Game.Magic.Abilities.AoWEUnit.AoWEngine = EngineFixture.Engine;
            loaded = Aow.Game.Magic.Abilities.AoWEUnit.AoWEngine.FOnHSMLoadedStore;
            closed = Aow.Game.Magic.Abilities.AoWEUnit.AoWEngine.FOnHSMCloseStore;
            Aow.Game.Magic.Abilities.AoWEUnit.AoWEngine.FOnHSMLoadedStore = new EventStore<THSMap>();
            Aow.Game.Magic.Abilities.AoWEUnit.AoWEngine.FOnHSMCloseStore = new EventStore<THSMap>();
            Aow.Game.Magic.Abilities.AoWEUnit.AoWHSMap = null!;
            Aow.Game.Combat.AoWTCUnit.AoWCombatMap = null!;
        }

        public void Dispose()
        {
            Aow.Game.Magic.Abilities.AoWEUnit.AoWEngine.FOnHSMLoadedStore = loaded;
            Aow.Game.Magic.Abilities.AoWEUnit.AoWEngine.FOnHSMCloseStore = closed;
            Aow.Game.Magic.Abilities.AoWEUnit.AoWEngine = previousEngine;
            Aow.Game.Magic.Abilities.AoWEUnit.AoWHSMap = previousMap;
            Aow.Game.Combat.AoWTCUnit.AoWCombatMap = previousCombat;
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
                    if (field.Name == "captureOrdinal") continue;
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
        "Aow.Game.Ui.Dialogs.TChatWindow",
        "Aow.Game.Ui.Dialogs.TControlWin",
        "Aow.Game.Ui.Dialogs.TDWin",
        "Aow.Game.Ui.Controls.TAoWGauge",
        "Aow.Game.Draw.Images.TInterfaceIL",
        "Aow.Game.Ui.Dialogs.TMWindow",
        "Aow.Game.Ui.Windows.TAOWMenu",
        "Aow.Game.Ui.Controls.TAOWMenuBar",
        "Aow.Game.Ui.Dialogs.TMultiWindow",
        "Aow.Game.Ui.Dialogs.TPyreScreen",
        "Aow.Game.Ui.Dialogs.TTitleWindow",
        "Aow.Game.Ui.Windows.TAOWToolTip",
        "Aow.Game.Ui.Controls.TAOWWinManager",
        "Aow.Game.Ui.Dialogs.TArenaDlg",
        "Aow.Game.Ui.Dialogs.TArmyEventDlg",
        "Aow.Game.Ui.Dialogs.TArmyInfoDlg",
        "Aow.Game.Ui.Controls.TAoWArnoLine",
        "Aow.Game.Ui.Windows.TAOWBaseWindow",
        "Aow.Game.Ui.Windows.TAoWBookWindow",
        "Aow.Game.Ui.Controls.TAOWBaseButton",
        "Aow.Game.Ui.Controls.TAOWButton",
        "Aow.Game.Ui.Dialogs.TCampaignJoinDlg",
        "Aow.Game.Ui.Dialogs.TCampaignSettingsDlg",
        "Aow.Game.Ui.Dialogs.TCampaignTransferDlg",
        "Aow.Game.Ui.Dialogs.TCastUnitSpellDlg",
        "Aow.Game.Ui.Controls.TAOWCheckBox",
        "Aow.Game.Ui.Dialogs.TCityJoinDlg",
        "Aow.Game.Ui.Dialogs.TCityRebelDlg",
        "Aow.Game.Ui.Dialogs.TCityScreen",
        "Aow.Game.Ui.Controls.TAOWComboBox",
        "Aow.Game.Ui.Controls.TAoWComponent",
        "Aow.Game.Ui.Dialogs.TConstructDlg",
        "Aow.Game.Ui.Controls.TAOWControl",
        "Aow.Game.Ui.Dialogs.TDeformFaceDlg",
        "Aow.Game.Ui.Dialogs.TDisjunctionDlg",
        "Aow.Game.Engine.TAgeOfWondersEngine",
        "Aow.Game.Ui.Controls.TAOWEdit",
        "Aow.Game.Ui.Dialogs.TEmailGameSettingsDlg",
        "Aow.Game.Ui.Dialogs.TEmailSettingsDlg",
        "Aow.Game.Ui.Dialogs.TFileDlgWindow",
        "Aow.Game.Ui.Controls.TAoWFrame",
        "Aow.Game.Ui.Dialogs.TGamePlayDlg",
        "Aow.Game.Ui.Controls.TAOWHScrollBar",
        "Aow.Game.Ui.Windows.TaowHTMLMemo",
        "Aow.Game.Ui.Dialogs.THeroInfoDlg",
        "Aow.Game.Ui.Dialogs.THeroJoinDlg",
        "Aow.Game.Ui.Dialogs.THeroUpgradeDlg",
        "Aow.Game.Ui.Controls.TAOWImage",
        "Aow.Game.Ui.Dialogs.TInfoBanner",
        "Aow.Game.Ui.Dialogs.TIWindow",
        "Aow.Game.Ui.Dialogs.TIntroScene",
        "Aow.Game.Ui.Dialogs.TItemBanner",
        "Aow.Game.Ui.Dialogs.TItemsFoundDlg",
        "Aow.Game.Ui.Controls.TAOWLabel",
        "Aow.Game.Ui.Dialogs.TLeaderSetupWin",
        "Aow.Game.Ui.Controls.TAOWListBox",
        "Aow.Game.Ui.Dialogs.TMagicWin",
        "Aow.Game.Ui.Controls.TAOWMemo",
        "Aow.Game.Ui.Dialogs.TMessageDlg",
        "Aow.Game.Ui.Dialogs.TMsgReceiveDlg",
        "Aow.Game.Ui.Dialogs.TNewTurnDlg",
        "Aow.Game.Ui.Dialogs.TPInfoW",
        "Aow.Game.Ui.Dialogs.TPOP3Dlg",
        "Aow.Game.Ui.Dialogs.TPopupWindows",
        "Aow.Game.Ui.Windows.TAOWPanel",
        "Aow.Game.Ui.Dialogs.TPartyJoinDlg",
        "Aow.Game.Ui.Dialogs.TPasswordDlg",
        "Aow.Game.Ui.Dialogs.TPowerDlg",
        "Aow.Game.Ui.Dialogs.TProductionScreen",
        "Aow.Game.Ui.Dialogs.TProgressDialog",
        "Aow.Game.Ui.Dialogs.TRealmWin",
        "Aow.Game.Ui.Dialogs.TRelWindow",
        "Aow.Game.Ui.Dialogs.TResultsWin",
        "Aow.Game.Ui.Dialogs.TSMTPDlg",
        "Aow.Game.Net.TmsMessage",
        "Aow.Game.Net.TmsSMTP",
        "Aow.Game.Ui.Dialogs.TSWindow",
        "Aow.Game.Ui.Dialogs.TScenSetup",
        "Aow.Game.Ui.Dialogs.TSelFactionWin",
        "Aow.Game.Ui.Dialogs.TSendEmailGameDlg",
        "Aow.Game.Session.TSetupControl",
        "Aow.Game.Ui.Dialogs.TSoundDlg",
        "Aow.Game.Ui.Dialogs.TSpellBook",
        "Aow.Game.Ui.Dialogs.TSynchronizeDlg",
        "Aow.Game.Ui.Dialogs.TTBWindow",
        "Aow.Game.Ui.Dialogs.TTCScanner",
        "Aow.Game.Ui.Dialogs.TTCUnit",
        "Aow.Game.Ui.Controls.TAOWTabPanel",
        "Aow.Game.Ui.Dialogs.TTargetSelectionDlg",
        "Aow.Game.Ui.Dialogs.TTitleScene",
        "Aow.Game.Ui.Dialogs.TTutorialWindow",
        "Aow.Game.Ui.Dialogs.TUnitBanner",
        "Aow.Game.Ui.Dialogs.TUnitSelectionDlg",
        "Aow.Game.Ui.Dialogs.TUnitWindow",
        "Aow.Game.Ui.Controls.TAOWScrollBar",
        "Aow.Game.Ui.Controls.TAOWVScrollBar",
        "Aow.Game.Ui.Dialogs.TVideoDialog",
        "Aow.Game.Ui.Controls.TAOWWindowControl",
        "Aow.Game.Ui.Windows.TAOWGenericWindow",
        "Aow.Game.Ui.Dialogs.TGeneral",
        "Aow.Game.Ui.Windows.TAOWWindow",
        "Aow.Game.Ui.Dialogs.TWizardsTowerDlg",
        "Aow.Game.Ui.Dialogs.TWorldMap",
        "Aow.Game.Ui.Windows.TAoWUserWindow",
        "Aow.Game.Ui.Grids.TAoWCombatMapControl",
        "Aow.Game.Engine.Nodes.TEComponent",
        "Aow.Game.Ui.Dialogs.TFastCombatWindow",
        "Aow.Game.Ui.Dialogs.TFontModule",
        "Aow.Game.World.Map.THSMapViewer",
        "Aow.Game.World.Map.THexagonSpriteEngine",
        "Aow.Game.Draw.THSLibrary",
        "Aow.Game.Ui.Grids.TAoWMapControl",
        "Aow.Game.Engine.TPlugInManager",
        "Aow.Game.Ui.Grids.TScanner",
        "Aow.Game.Ui.Windows.TStartMenu",
        "Aow.Game.Ui.Dialogs.TTCFCDialog",
        "Aow.Game.Ui.Grids.TAOWTurnButton",
        "Aow.Game.Ui.Dialogs.THelpWindow",
        "Aow.Game.Ui.Dialogs.TIntGfxMod",
        "Aow.Game.Ui.Dialogs.TMapEvents",
        "Aow.Game.Ui.Dialogs.TSetupEvents",
        "Aow.Game.Ui.Dialogs.TTCMapEvents",
        "Aow.Game.Ui.Controls.TaowUserDrawBox",
    ];

    public static IEnumerable<object[]> Classes => Names.Select(name => new object[] { name });

    [Fact]
    public void InheritOnlyLineHasItsOwnCompletedOwnerConstructor()
    {
        using var owner = new TComponent(null!);
        var line = new Aow.Game.Ui.Controls.TAoWLenLine(owner);
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

    private static TComponent Direct(Type type, TComponent? owner)
    {
        try
        {
            var constructor = type.GetConstructor([typeof(TComponent)]);
            if (constructor is not null) return (TComponent)constructor.Invoke([owner]);
            var factory = type.GetMethod("Create", BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly, [typeof(TComponent)]);
            if (factory is not null) return (TComponent)factory.Invoke(null, [owner])!;
            throw new InvalidOperationException(type.FullName + " has no completed owner entry.");
        }
        catch (TargetInvocationException error) when (error.InnerException is not null)
        {
            System.Runtime.ExceptionServices.ExceptionDispatchInfo.Capture(error.InnerException).Throw();
            throw;
        }
    }

    [Theory]
    [MemberData(nameof(Classes))]
    public void ConstructorStateOwnerObservationAndTeardownMatchBaseCommit(string name)
    {
        Type type = typeof(TSpell).Assembly.GetType(name)!;
        for (int owned = 0; owned < 2; owned++)
        {
            using var fixture = new Fixture();
            var owner = owned == 0 ? null : new ComponentOwner();
            var states = new List<string>();
            TComponent? value = null;
            try
            {
                value = Direct(type, owner);
                Assert.IsType(type, value);
                Assert.Same(owner, value.Owner);
                states.Add("success:" + Snapshot(value));
            }
            catch (System.Exception error) { states.Add("failure:" + error.GetType().FullName + ":" + error.Message); }
            finally
            {
                try { value?.Dispose(); }
                catch (System.Exception error) { states.Add("destroy:" + error.GetType().FullName + ":" + error.Message); }
            }
            states.Add("owner:" + (owner?.ComponentCount ?? 0));
            if (owner is not null) states.Add(JsonSerializer.Serialize(owner.Observations));
            states.Add("subscriptions:" + Aow.Game.Magic.Abilities.AoWEUnit.AoWEngine.FOnHSMLoadedStore.Count + ":" + Aow.Game.Magic.Abilities.AoWEUnit.AoWEngine.FOnHSMCloseStore.Count);
            string actual = string.Join("\n", states);
            string key = name + ".owner" + owned;
            ConstructionSnapshot.Check("UiConstruction", key, actual);
        }
    }
}
