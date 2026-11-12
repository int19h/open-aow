// Translated from Sound.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Audio;

// FSongs, FSongNames (the titles, from the files). An engine object with a list of music files
public partial class SongList : EngineObject
{
    // Sound.pas:801
    public new const int ClassId = 2162692;

    public SongList(ECustomNode owner) : base(owner)
    {
        _songs = new StringList();
        _songs.OnChange = SongsChanged;
        _songNames = new StringList();
    }

    private StringList _songs;
    private StringList _songNames;

    // Sound.pas:813
    internal void SetSongs(StringList value)
    {
        _songs.Assign(value);
    }

    // Sound.pas:819
    // { Reads the song titles; a file that cannot be read is dropped from the list. }
    internal void SongsChanged(object sender)
    {
        _songNames.Clear();
        for (int I = _songs.Count - 1; I >= 0; I--)
        {
            try
            {
                var songName = AudioPlayback.GetSongName(_songs[I]);
                _songNames.Insert(0, songName);
            }
            catch
            {
                _songs.Delete(I);
            }
        }
    }

    // Sound.pas:806
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        if (!stream.StorageMode().Contains(EngineServices.Text))
        {
            stream.rwStrings(_songs, 10);
        }
    }

    // Sound.pas:786

    // Sound.pas:794
    public override void Dispose()
    {
        _songs?.Dispose();
        _songNames?.Dispose();
        base.Dispose();
    }

    // Sound.pas:832
    public void Search(string path, string ext)
    {
        SearchRec sr = default;
        string fullPath;
        bool found;
        if (path[path.Length - 1] != 92)
        {
            path = path + "\\";
        }

        fullPath = AudioHost.BaseSoundEnginePath + path;
        _songs.BeginUpdate();
        found = FindFirst(fullPath + "*." + ext, 63 & ~16, ref sr) == 0;
        while (found)
        {
            _songs.Add(UpperCase(path + sr.Name));
            found = FindNext(ref sr) == 0;
        }

        FindClose(ref sr);
        _songs.EndUpdate();
    }

    public StringList Songs { get => _songs; set => SetSongs(value); }
    public StringList SongNames { get => _songNames; }

}
