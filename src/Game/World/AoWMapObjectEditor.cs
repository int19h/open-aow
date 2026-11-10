// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.World;

public partial class AoWMapObjectEditor : EObjectEditor, IEObjectEditorClass
{
    public AoWMapObjectEditor(ECustomNode owner) : base(owner)
    { }

    // Army.PAS:5753
    public static new int EditorClassID => 65878;

}
