using Aow.Game;
using Aow.Game.Engine;
using Aow.Rtl;

namespace Aow.Engine.Nodes;

public partial class EObjectEditor
{
    // The popup hooks of the editor shell. No editor class overrides them yet: they join the
    // IEObjectEditorClass descriptor when the first one does.
    public static void UpdateMultiPopupMenu(EObject obj, GameEngine engine, ref Set<EditModeElement> handled, PopupMenu menu, EObjectList objects) { }

    public static void UpdatePopupMenu(EObject obj, GameEngine engine, ref Set<EditModeElement> handled, PopupMenu menu) { }
}
