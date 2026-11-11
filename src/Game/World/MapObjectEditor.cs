// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.World;

public partial class MapObjectEditor : EngineObjectEditor, IEngineObjectEditorClass
{
    public MapObjectEditor(ECustomNode owner) : base(owner)
    { }

    // Army.PAS:5753
    public static new int EditorClassID => 65878;

}
