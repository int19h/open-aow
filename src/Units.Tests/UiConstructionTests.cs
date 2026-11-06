using System.Collections;
using System.Reflection;
using System.Text;
using System.Text.Json;
using Aow.Units.AoWE;
using Aow.Units.Engine;
using Aow.Units.HSEngine;
using Game = Aow.Units.AoWE.AoWEUnit;

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
        private readonly TAoWEngine previousEngine = Game.AoWEngine;
        private readonly TAoWHSMap previousMap = Game.AoWHSMap;
        private readonly Aow.Units.AoWTC.TAoWCombatMap previousCombat = AoWTC.AoWTCUnit.AoWCombatMap;
        internal Fixture()
        {
            if (EngineUnit.Engines is null) EngineUnit.Initialize();
            Game.AoWEngine = EngineFixture.Engine;
            Game.AoWEngine.FOnHSMLoaded = new TEventList();
            Game.AoWEngine.FOnHSMClose = new TEventList();
            Game.AoWHSMap = null!;
            AoWTC.AoWTCUnit.AoWCombatMap = null!;
        }
        public void Dispose()
        {
            Game.AoWEngine.FOnHSMLoaded.Free();
            Game.AoWEngine.FOnHSMClose.Free();
            Game.AoWEngine = previousEngine;
            Game.AoWHSMap = previousMap;
            AoWTC.AoWTCUnit.AoWCombatMap = previousCombat;
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
            for (Type? current = type; current is not null && current != typeof(TObject) && current != typeof(object); current = current.BaseType)
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
        "Aow.Units.AOWChatWin.TChatWindow",
        "Aow.Units.AOWControlWin.TControlWin",
        "Aow.Units.AOWDialogWin.TDWin",
        "Aow.Units.AOWGauge.TAoWGauge",
        "Aow.Units.AOWInterfaceImgLib.TInterfaceIL",
        "Aow.Units.AOWMWindow.TMWindow",
        "Aow.Units.AOWMenu.TAOWMenu",
        "Aow.Units.AOWMenuBar.TAOWMenuBar",
        "Aow.Units.AOWMultiWin.TMultiWindow",
        "Aow.Units.AOWPyreScrn.TPyreScreen",
        "Aow.Units.AOWTitleWindow.TTitleWindow",
        "Aow.Units.AOWToolTip.TAOWToolTip",
        "Aow.Units.AOWWinManager.TAOWWinManager",
        "Aow.Units.AoWArenaDlg.TArenaDlg",
        "Aow.Units.AoWArmyEventDlg.TArmyEventDlg",
        "Aow.Units.AoWArmyInfoDlg.TArmyInfoDlg",
        "Aow.Units.AoWArnoLine.TAoWArnoLine",
        "Aow.Units.AoWBaseWin.TAOWBaseWindow",
        "Aow.Units.AoWBookWindow.TAoWBookWindow",
        "Aow.Units.AoWButton.TAOWBaseButton",
        "Aow.Units.AoWButton.TAOWButton",
        "Aow.Units.AoWCampaignJoin.TCampaignJoinDlg",
        "Aow.Units.AoWCampaignSettings.TCampaignSettingsDlg",
        "Aow.Units.AoWCampaignTransferDlg.TCampaignTransferDlg",
        "Aow.Units.AoWCastUnitSpellDlg.TCastUnitSpellDlg",
        "Aow.Units.AoWCheckBox.TAOWCheckBox",
        "Aow.Units.AoWCityJoinDlg.TCityJoinDlg",
        "Aow.Units.AoWCityRebelDlg.TCityRebelDlg",
        "Aow.Units.AoWCityScreen.TCityScreen",
        "Aow.Units.AoWComboBox.TAOWComboBox",
        "Aow.Units.AoWComp.TAoWComponent",
        "Aow.Units.AoWConstructDlg.TConstructDlg",
        "Aow.Units.AoWControl.TAOWControl",
        "Aow.Units.AoWDeformFaceDlg.TDeformFaceDlg",
        "Aow.Units.AoWDisjunctionDlg.TDisjunctionDlg",
        "Aow.Units.AoWE.TAgeOfWondersEngine",
        "Aow.Units.AoWEdit.TAOWEdit",
        "Aow.Units.AoWEmailGameSettingsDlg.TEmailGameSettingsDlg",
        "Aow.Units.AoWEmailSettingsDlg.TEmailSettingsDlg",
        "Aow.Units.AoWFileDialog.TFileDlgWindow",
        "Aow.Units.AoWFrame.TAoWFrame",
        "Aow.Units.AoWGamePlayDlg.TGamePlayDlg",
        "Aow.Units.AoWHScroll.TAOWHScrollBar",
        "Aow.Units.AoWHTMLMemo.TaowHTMLMemo",
        "Aow.Units.AoWHeroInfoDlg.THeroInfoDlg",
        "Aow.Units.AoWHeroJoinDlg.THeroJoinDlg",
        "Aow.Units.AoWHeroUpgradeDlg.THeroUpgradeDlg",
        "Aow.Units.AoWImage.TAOWImage",
        "Aow.Units.AoWInfoBanner.TInfoBanner",
        "Aow.Units.AoWInfoWin.TIWindow",
        "Aow.Units.AoWIntroScene.TIntroScene",
        "Aow.Units.AoWItemBanner.TItemBanner",
        "Aow.Units.AoWItemsFoundDlg.TItemsFoundDlg",
        "Aow.Units.AoWLabel.TAOWLabel",
        "Aow.Units.AoWLeaderSetup.TLeaderSetupWin",
        "Aow.Units.AoWListBox.TAOWListBox",
        "Aow.Units.AoWMagic.TMagicWin",
        "Aow.Units.AoWMemo.TAOWMemo",
        "Aow.Units.AoWMessageDlg.TMessageDlg",
        "Aow.Units.AoWMsgReceiveDlg.TMsgReceiveDlg",
        "Aow.Units.AoWNewTurnDialog.TNewTurnDlg",
        "Aow.Units.AoWPInfoWin.TPInfoW",
        "Aow.Units.AoWPOP3Dlg.TPOP3Dlg",
        "Aow.Units.AoWPWindows.TPopupWindows",
        "Aow.Units.AoWPanel.TAOWPanel",
        "Aow.Units.AoWPartyJoin.TPartyJoinDlg",
        "Aow.Units.AoWPasswordDlg.TPasswordDlg",
        "Aow.Units.AoWPowerDlg.TPowerDlg",
        "Aow.Units.AoWProdScreen.TProductionScreen",
        "Aow.Units.AoWProgressDialog.TProgressDialog",
        "Aow.Units.AoWRealm.TRealmWin",
        "Aow.Units.AoWRelations.TRelWindow",
        "Aow.Units.AoWResultsWin.TResultsWin",
        "Aow.Units.AoWSMTPDlg.TSMTPDlg",
        "Aow.Units.AoWSMTPDlg.TmsMessage",
        "Aow.Units.AoWSMTPDlg.TmsSMTP",
        "Aow.Units.AoWSWindow.TSWindow",
        "Aow.Units.AoWScenSetup.TScenSetup",
        "Aow.Units.AoWSelFactionWin.TSelFactionWin",
        "Aow.Units.AoWSendEmailGameDlg.TSendEmailGameDlg",
        "Aow.Units.AoWSetup.TSetupControl",
        "Aow.Units.AoWSoundDlg.TSoundDlg",
        "Aow.Units.AoWSpellBook.TSpellBook",
        "Aow.Units.AoWSyncDlg.TSynchronizeDlg",
        "Aow.Units.AoWTBWindow.TTBWindow",
        "Aow.Units.AoWTCScanner.TTCScanner",
        "Aow.Units.AoWTCUnit.TTCUnit",
        "Aow.Units.AoWTabPanel.TAOWTabPanel",
        "Aow.Units.AoWTargetSelection.TTargetSelectionDlg",
        "Aow.Units.AoWTitleScene.TTitleScene",
        "Aow.Units.AoWTutorialWin.TTutorialWindow",
        "Aow.Units.AoWUnitBanner.TUnitBanner",
        "Aow.Units.AoWUnitSelDlg.TUnitSelectionDlg",
        "Aow.Units.AoWUnitWindow.TUnitWindow",
        "Aow.Units.AoWVScroll.TAOWScrollBar",
        "Aow.Units.AoWVScroll.TAOWVScrollBar",
        "Aow.Units.AoWVideoDlg.TVideoDialog",
        "Aow.Units.AoWWinControl.TAOWWindowControl",
        "Aow.Units.AoWWinGeneric.TAOWGenericWindow",
        "Aow.Units.AoWWinMan.TGeneral",
        "Aow.Units.AoWWindow.TAOWWindow",
        "Aow.Units.AoWWizardsTowerDlg.TWizardsTowerDlg",
        "Aow.Units.AoWWorldMap.TWorldMap",
        "Aow.Units.AowUserWindow.TAoWUserWindow",
        "Aow.Units.CMapCtrl.TAoWCombatMapControl",
        "Aow.Units.Engine.TEComponent",
        "Aow.Units.FastCombatWin.TFastCombatWindow",
        "Aow.Units.FontMod.TFontModule",
        "Aow.Units.HSEngine.THSMapViewer",
        "Aow.Units.HSEngine.THexagonSpriteEngine",
        "Aow.Units.HSLib.THSLibrary",
        "Aow.Units.MapCtrl.TAoWMapControl",
        "Aow.Units.PlugInM.TPlugInManager",
        "Aow.Units.Scanner.TScanner",
        "Aow.Units.StartMenu.TStartMenu",
        "Aow.Units.TCFCDlg.TTCFCDialog",
        "Aow.Units.TurnButton.TAOWTurnButton",
        "Aow.Units.aowHelpWin.THelpWindow",
        "Aow.Units.aowIntGfxMod.TIntGfxMod",
        "Aow.Units.aowMapEvents.TMapEvents",
        "Aow.Units.aowSetupEvents.TSetupEvents",
        "Aow.Units.aowTCMapEvents.TTCMapEvents",
        "Aow.Units.aowUserDrawBox.TaowUserDrawBox",
    ];
    public static IEnumerable<object[]> Classes => Names.Select(name => new object[] { name });

    [Fact]
    public void InheritOnlyLineHasItsOwnCompletedOwnerConstructor()
    {
        var owner = new TComponent(null!);
        var line = new Aow.Units.AoWLenLine.TAoWLenLine(owner);
        Assert.Same(owner, line.Owner);
        Assert.Same(line, owner.Components[0]);
        Assert.True(line.FVisible);
        Assert.True(line.FFirstDraw);
        Assert.True(line.FCaptureInput);
        Assert.NotNull(line.FAlignment);
        Assert.NotNull(line.FChildren);
        Assert.NotNull(line.FToolTipText);
        line.Destroy();
        Assert.Equal(0, owner.ComponentCount);
        owner.Destroy();
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
                try { value?.Destroy(); }
                catch (System.Exception error) { states.Add("destroy:" + error.GetType().FullName + ":" + error.Message); }
            }
            states.Add("owner:" + (owner?.ComponentCount ?? 0));
            if (owner is not null) states.Add(JsonSerializer.Serialize(owner.Observations));
            states.Add("subscriptions:" + Game.AoWEngine.FOnHSMLoaded.Count + ":" + Game.AoWEngine.FOnHSMClose.Count);
            string actual = string.Join("\n", states);
            string key = name + ".owner" + owned;
            ConstructionSnapshot.Check("UiConstruction", key, actual);
        }
    }
}
