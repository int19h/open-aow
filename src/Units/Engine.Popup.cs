using Aow.Game.Engine;
using Aow.Rtl;

namespace Aow.Game.Engine.Nodes;

public partial class EObjectEditor
{
    // The popup hooks of the editor shell. No editor class overrides them yet: they join the
    // IEObjectEditorClass descriptor when the first one does.
    public static void UpdateMultiPopupMenu(EObject obj, Engine engine, ref Set<EditModeElement> handled, PopupMenu menu, EObjectList objects) { }

    public static void UpdatePopupMenu(EObject obj, Engine engine, ref Set<EditModeElement> handled, PopupMenu menu) { }
}
