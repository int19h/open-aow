// Translated from Sound.pas.
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

using Aow.Rtl;
using Aow.Rtl.Host;
using static Aow.Rtl.DelphiTextCompare;
using static Aow.Rtl.RtlHost;

namespace Aow.Game.Audio;

// FSongs, FSongNames (the titles, from the files). An engine object with a list of music files
public class SongList : EngineObject
{
    // Sound.pas:801
    public new const int ClassId = 2162692;

    public SongList(CustomNode owner) : base(owner)
    {
        Songs = new DelphiStringList
        {
            OnChange = SongsChanged
        };
        SongNames = new DelphiStringList();
    }

    // Sound.pas:813
    internal void SetSongs(DelphiStringList value) => Songs.Assign(value);

    // Sound.pas:819
    // { Reads the song titles; a file that cannot be read is dropped from the list. }
    internal void SongsChanged(object sender)
    {
        SongNames.Clear();
        for (int i = Songs.Count - 1; i >= 0; i--)
        {
            try
            {
                string songName = AudioPlayback.GetSongName(Songs[i]);
                SongNames.Insert(0, value: songName);
            }
            catch
            {
                Songs.Delete(i);
            }
        }
    }

    // Sound.pas:806
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        // The original ReadWrite leaves out the songs under text only (the translate stream).
        if (!PersistSupport.Translating(persister))
        {
            PersistSupport.Lines(persister, name: "Songs", lines: Songs);
        }
    }

    // Sound.pas:794
    public override void Dispose()
    {
        Songs?.Dispose();
        SongNames?.Dispose();
        base.Dispose();
    }

    // Sound.pas:832
    public void Search(string path, string ext)
    {
        if (path[^1] != 92)
        {
            path += "\\";
        }

        string fullPath = AudioHost.BaseSoundEnginePath + path;
        Songs.BeginUpdate();
        foreach (RtlFileEntry entry in Files.FindEntries($"{fullPath}*.{ext}", attributes: 63 & ~16))
        {
            _ = Songs.Add(UpperCase(path + entry.Name));
        }

        Songs.EndUpdate();
    }

    public DelphiStringList Songs { get; }
    public DelphiStringList SongNames { get; }
}
