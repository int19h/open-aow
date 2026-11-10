// Translated from ImageLib.pas.
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw.Images;

// a combo box with the library names of a manager. FILManager, FCategory, FSubDirectory (only
// the names in it are listed, without it), FSkipDirectories (separated by semicolons),
// FEmptyILName (the first item, for no library)
public partial class ILManagerComboBox : ComboBox
{
    internal ImageLibraryManager FILManager;
    internal Set<ILManagerCategoryElement> FCategory;
    internal string FSubDirectory = "";
    internal string FSkipDirectories = "";
    internal string FEmptyILName = "";

    // ImageLib.pas:8373
    internal void SetILManager(ImageLibraryManager Value)
    {
        FILManager = Value;
        GenerateItems();
    }

    // ImageLib.pas:8379
    internal void SetSubDirectory(string Value)
    {
        FSubDirectory = UpperCase(Value);
        if (!IsEmpty(Value) && FSubDirectory[FSubDirectory.Length - 1] != 92)
        {
            FSubDirectory = FSubDirectory + "\\";
        }
    }

    // ImageLib.pas:8393
    internal string GetImageLibraryName()
    {
        /* TODO: Implement StdCtrls.ItemIndex in the platform or editor service. */
        throw new NotImplementedException();
    }

    // ImageLib.pas:8401
    internal void SetImageLibraryName(string Value)
    {
        /* TODO: Implement StdCtrls.Items in the platform or editor service. */
        throw new NotImplementedException();
    }

    // ImageLib.pas:8386

    public ILManagerComboBox(Component owner) : base(owner)
    {
        /* TODO: Implement StdCtrls.Create in the platform or editor service. */
        throw new NotImplementedException();
    }

    // ImageLib.pas:8414
    public void GenerateItems()
    {
        /* TODO: Implement StdCtrls.Items in the platform or editor service. */
        throw new NotImplementedException();
    }

    public string ImageLibraryName { get => GetImageLibraryName(); set => SetImageLibraryName(value); }

    [Published]
    public ImageLibraryManager ILManager { get => FILManager; set => SetILManager(value); }

    [Published]
    public Set<ILManagerCategoryElement> Category { get => FCategory; set => FCategory = value; }

    [Published]
    public string SubDirectory { get => FSubDirectory; set => SetSubDirectory(value); }

    [Published]
    public string SkipDirectories { get => FSkipDirectories; set => FSkipDirectories = value; }

    [Published]
    public string EmptyILName { get => FEmptyILName; set => FEmptyILName = value; }
}
