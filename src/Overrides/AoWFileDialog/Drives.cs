namespace Aow.Units.AoWFileDialog;

public partial class TFileDlgWindow
{
    public void FindDrives()
    {
        AoWFileDialog.Drives.Clear();
        AoWFileDialog.Drives.Add($"({Aow.Rtl.Rtl.Files.VirtualDrive}\\)");
        DriveCmb.Items.Clear();
        DriveCmb.Items.AddStrings(AoWFileDialog.Drives);
        DriveCmb.Selected = 0;
        FindFiles();
    }
}
