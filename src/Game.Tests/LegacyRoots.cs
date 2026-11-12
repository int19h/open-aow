using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Game.AI;
using Aow.Game.AI.Groups;
using Aow.Game.Audio;
using Aow.Game.Combat;
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.Events;
using Aow.Game.Magic.Spells;
using Aow.Game.Net;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Dialogs;
using Aow.Game.UI.Grids;
using Aow.Game.UI.Windows;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Game.World.Structures;
using Aow.Game.World.Terrain;

namespace Aow.Game.Tests;

// Former direct subclasses of the removed root. The startup recording walks these classes and their descendants.
internal static class LegacyRoots
{
    private static readonly HashSet<Type> s_roots = new()
    {
        typeof(AITargetQuery),
        typeof(AIGroupTargetSelector.PrioritySettings),
        typeof(BehaviorRegistrar),
        typeof(GroupDefenseSelector),
        typeof(IndependentRegistrar),
        typeof(MainRegistrar),
        typeof(Sample),
        typeof(SampleData),
        typeof(SongPlayer),
        typeof(SongStatus),
        typeof(SoundSystem),
        typeof(BattleFieldQuery),
        typeof(BattleAI),
        typeof(CombatAbilityRef),
        typeof(CombatPredictor),
        typeof(CombatPredictorUnit),
        typeof(CombatResultQuery),
        typeof(CombatViewer),
        typeof(FastBattleCommand),
        typeof(FastBattleCommandList),
        typeof(MeleeRound),
        typeof(RangedObstacle),
        typeof(RandomBattleMapGenerator),
        typeof(StormDamageQuery),
        typeof(TacticalBattlePlayerList),
        typeof(BattleAIMove),
        typeof(TargetSelectionControl),
        typeof(BlendTable),
        typeof(BlendTableList),
        typeof(DisplayModeItem),
        typeof(DisplayModeList),
        typeof(Palette),
        typeof(ImageFile),
        typeof(FastLoadFileStream),
        typeof(GrabInfo),
        typeof(ImageLibraryManagerObject),
        typeof(ImageSequence),
        typeof(PaletteManager),
        typeof(LookupBlendTableCollection100),
        typeof(Matrix),
        typeof(Rectangle),
        typeof(Surface),
        typeof(UpdateFrameThread),
        typeof(Point3D),
        typeof(Point3DList),
        typeof(DefenseRequirementsQuery),
        typeof(EngineDebugNotification),
        typeof(EngineObject),
        typeof(EngineObjectEditorList),
        typeof(Plugin),
        typeof(PropertyTable),
        typeof(ShowMessageNotification),
        typeof(CompressStream),
        typeof(EStorageStream),
        typeof(FastMemoryStream),
        typeof(ExecuteEventLogCallbackList),
        typeof(ExecuteEventLogNotification),
        typeof(SpellList),
        typeof(MailAddressList),
        typeof(MailAttachmentList),
        typeof(MailHeader),
        typeof(PackageDataList),
        typeof(PlayerCommunicationControl),
        typeof(TokenControl),
        typeof(TokenExecutor),
        typeof(TokenManager),
        typeof(TokenPackageDistributor),
        typeof(ColorChange),
        typeof(FileTime),
        typeof(DirtyRectangle),
        typeof(IMSSendThread),
        typeof(PageTurnControl),
        typeof(HtmlMemo.ChangeInfo),
        typeof(UnitIndexListCollection),
        typeof(UnitSelector),
        typeof(HexSpriteMapMouse),
        typeof(MapChangedNotification),
        typeof(NewHexSpriteMapSettings),
        typeof(TerrainChangedNotification),
        typeof(TerrainList),
        typeof(MoveControl),
        typeof(MoveSelector),
        typeof(CityRegistrar),
        typeof(FireControl),
        typeof(Canvas),
        typeof(DelphiList),
        typeof(Filer),
        typeof(GDConnection),
        typeof(GDFile),
        typeof(GDMessage),
        typeof(IToolServices),
        typeof(IUnknown),
        typeof(IniFile),
        typeof(Persistent),
        typeof(PopupMenu),
        typeof(PtrBox<>),
        typeof(Registry),
        typeof(Runtime.IntegerObject),
        typeof(Text),
    };

    internal static bool Contains(Type type)
    {
        for (Type? current = type; current is not null && current != typeof(object); current = current.BaseType)
        {
            Type defined = current.IsGenericType ? current.GetGenericTypeDefinition() : current;
            if (s_roots.Contains(defined)) return true;
        }
        return false;
    }
}
