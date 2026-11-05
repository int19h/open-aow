namespace Aow.Units.AoWFileDialog;

public partial class TFileDlgWindow
{
    public void FindDrives()
    {
        AoWFileDialog.Drives.Clear();
        string drive = Rtl.Rtl.Files.VirtualDrive + "\\";
        string label = OperatingSystem.IsWindows() ? new DriveInfo(drive).VolumeLabel + " " : "";
        AoWFileDialog.Drives.Add($"{label}({drive})");
        DriveCmb.Items.Clear();
        DriveCmb.Items.AddStrings(AoWFileDialog.Drives);
        DriveCmb.Selected = 0;
        FindFiles();
    }
}
