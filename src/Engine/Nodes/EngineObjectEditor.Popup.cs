using Aow.Game;

using Aow.Rtl;

namespace Aow.Engine.Nodes;

public partial class EngineObjectEditor
{
    // The popup hooks of the editor shell. No editor class overrides them yet: they join the
    // IEngineObjectEditorClass descriptor when the first one does.
    public static void UpdateMultiPopupMenu(EngineObject objectValue, GameEngine engine, ref Set<EditModeElement> handled, PopupMenu menu, EngineObjectList objects) { }

    public static void UpdatePopupMenu(EngineObject objectValue, GameEngine engine, ref Set<EditModeElement> handled, PopupMenu menu) { }
}
