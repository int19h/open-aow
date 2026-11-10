// Translated from Sound.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Audio;

// FSongs, FSongNames (the titles, from the files). An engine object with a list of music files
public partial class Songlist : EObject
{
    // Sound.pas:801
    public new const int ClassID = 2162692;

    public Songlist(ECustomNode owner) : base(owner)
    {
        FSongs = new StringList();
        FSongs.OnChange = SongsChanged;
        FSongNames = new StringList();
    }

    internal StringList FSongs;
    internal StringList FSongNames;

    // Sound.pas:813
    internal void SetSongs(StringList Value)
    {
        FSongs.Assign(Value);
    }

    // Sound.pas:819
    // { Reads the song titles; a file that cannot be read is dropped from the list. }
    internal void SongsChanged(object Sender)
    {
        FSongNames.Clear();
        for (int I = FSongs.Count - 1; I >= 0; I--)
        {
            try
            {
                var songName = AudioPlayback.GetSongName(FSongs[I]);
                FSongNames.Insert(0, songName);
            }
            catch
            {
                FSongs.Delete(I);
            }
        }
    }

    // Sound.pas:806
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        if (!Stream.StorageMode().Contains(EngineServices.smText))
        {
            Stream.rwStrings(FSongs, 10);
        }
    }

    // Sound.pas:786

    // Sound.pas:794
    public override void Dispose()
    {
        FSongs?.Dispose();
        FSongNames?.Dispose();
        base.Dispose();
    }

    // Sound.pas:832
    public void Search(string Path, string Ext)
    {
        SearchRec SR = default;
        string FullPath;
        bool Found;
        if (Path[Path.Length - 1] != 92)
        {
            Path = Path + "\\";
        }

        FullPath = AudioHost.BaseSoundEnginePath + Path;
        FSongs.BeginUpdate();
        Found = FindFirst(FullPath + "*." + Ext, 63 & ~16, ref SR) == 0;
        while (Found)
        {
            FSongs.Add(UpperCase(Path + SR.Name));
            Found = FindNext(ref SR) == 0;
        }

        FindClose(ref SR);
        FSongs.EndUpdate();
    }

    public StringList Songs { get => FSongs; set => SetSongs(value); }
    public StringList SongNames { get => FSongNames; }

}
