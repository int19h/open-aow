// Translated from Sound.pas.
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Audio;

// FSFXManager, FSubDirectory, FSkipDirectories ('.'-separated), FEmptySFXName
public partial class SFXManagerComboBox : ComboBox
{
    internal SFXManager FSFXManager;
    internal string FSubDirectory = "";
    internal string FSkipDirectories = "";
    internal string FEmptySFXName = "";

    // Sound.pas:472
    internal void SetSFXManager(SFXManager Value)
    {
        FSFXManager = Value;
        GenerateItems();
    }

    // Sound.pas:478
    internal void SetSubDirectory(string Value)
    {
        FSubDirectory = UpperCase(Value);
        if (!IsEmpty(Value) && FSubDirectory[FSubDirectory.Length - 1] != 92)
        {
            FSubDirectory = FSubDirectory + "\\";
        }
    }

    // Sound.pas:491
    internal string GetSFXName()
    {
        /* TODO: Implement StdCtrls.ItemIndex in the platform or editor service. */
        throw new NotImplementedException();
    }

    // Sound.pas:499
    internal void SetSFXName(string Value)
    {
        /* TODO: Implement StdCtrls.Items in the platform or editor service. */
        throw new NotImplementedException();
    }

    // Sound.pas:485

    public SFXManagerComboBox(Component owner) : base(owner)
    {
        /* TODO: Implement StdCtrls.Create in the platform or editor service. */
        throw new NotImplementedException();
    }

    // Sound.pas:518
    // { Fills the items with the manager's files under SubDirectory (without the subdirectory), less
    //   those in SkipDirectories, after EmptySFXName. }
    public void GenerateItems()
    {
        /* TODO: Implement StdCtrls.Items in the platform or editor service. */
        throw new NotImplementedException();
    }

    public string SFXName { get => GetSFXName(); set => SetSFXName(value); }

    [Published]
    public SFXManager SFXManager { get => FSFXManager; set => SetSFXManager(value); }

    [Published]
    public string SubDirectory { get => FSubDirectory; set => SetSubDirectory(value); }

    [Published]
    public string SkipDirectories { get => FSkipDirectories; set => FSkipDirectories = value; }

    [Published]
    public string EmptySFXName { get => FEmptySFXName; set => FEmptySFXName = value; }
}
