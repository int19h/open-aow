// Translated from GlobalTargetSpells.pas.
using Aow.Engine.Collections;

using Aow.Game.Combat;

using Aow.Game.Draw.Images;

using Aow.Game.Events;

using Aow.Rtl;

namespace Aow.Game.Magic.Spells.GlobalTarget;

public partial class TargetSelectionView
{
    public CustomImageLibrary ImageLibrary;
    public byte Status;
    public Set<TargetSelectionModeElement> Mode;
    public ShowTargetSelectionEvent OnShow;
    public ValidateTargetEvent OnValidate;
    public int Distance;
    public int CenterX;
    public int CenterY;
    public int CenterLayer;
    public EventList Changes;
    public TargetSelectionChange Change;
    public sbyte X;
    public sbyte Y;
    public sbyte Layer;
    public StringCollection Text;
    public string Caption = "";
    public string CancelCaption = "";
    public LibraryImage Preview;
    public LibraryImage ValidPreview;
    public LibraryImage InvalidPreview;
}
