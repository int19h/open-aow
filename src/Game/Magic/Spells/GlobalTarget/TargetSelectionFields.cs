// Translated from GlobalTargetSpells.pas.
using Aow.Engine.Collections;

using Aow.Game.Combat;

using Aow.Game.Draw.Images;

using Aow.Game.Events;

using Aow.Rtl;

namespace Aow.Game.Magic.Spells.GlobalTarget;

public struct TargetSelectionFields
{
    public object VMT;
    public CustomImageLibrary ImageLibrary;
    public int Reserved08;
    public byte Status;
    public Set<TargetSelectionModeElement> Mode;
    public ushort Padding0E;
    public ShowTargetSelectionEvent OnShow;
    public ValidateTargetEvent OnValidate;
    public int Distance;
    public int CenterX;
    public int CenterY;
    public int CenterLayer;
    public int Reserved30;
    public EventList Changes;
    public TargetSelectionChange Change;
    public sbyte X;
    public sbyte Y;
    public sbyte Layer;
    public StringCollection Text;
    private string caption;

    public string Caption { readonly get => caption ?? ""; set => caption = value; }

    private string cancelCaption;

    public string CancelCaption { readonly get => cancelCaption ?? ""; set => cancelCaption = value; }

    public int Reserved48;
    public int Reserved4C;
    public LibraryImage Preview;
    public LibraryImage ValidPreview;
    public LibraryImage InvalidPreview;
}
