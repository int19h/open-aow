namespace Aow.Rtl.Tests;

internal sealed class ProbeDirectory : IDisposable
{
    private readonly string previous = Directory.GetCurrentDirectory();

    internal string Root { get; } = Path.Combine(AppContext.BaseDirectory, "probe-temp", Guid.NewGuid().ToString("N"));

    internal ProbeDirectory() { Directory.CreateDirectory(Root); Directory.SetCurrentDirectory(Root); }

    public void Dispose()
    {
        Directory.SetCurrentDirectory(previous);
        foreach (string entry in Directory.EnumerateFileSystemEntries(Root, "*", SearchOption.AllDirectories).OrderByDescending(p => p.Length))
        {
            bool directory = Directory.Exists(entry);
            File.SetAttributes(entry, directory ? FileAttributes.Directory : FileAttributes.Normal);
            if (directory) Directory.Delete(entry); else File.Delete(entry);
        }
        Directory.Delete(Root);
    }
}
