// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.Audio;

// inherited Songs, SongNames
public partial class StartupSongList : Songlist
{
    // AoWHSMap.PAS:1215
    public StartupSongList(ECustomNode owner) : base(owner)
    {
        Songs.OnChange = OriginalSongsChanged;
    }

    // AoWHSMap.PAS:1221
    public void OriginalSongsChanged(object Sender)
    {
        string Name;
        SongNames.Clear();
        for (int I = Songs.Count - 1; I >= 0; I--)
        {
            try
            {
                Name = AudioPlayback.GetSongName(Songs[I]);
            }
            catch
            {
                Name = AbilityRules.TranslateRStr(GameText.FileNotFoundRStr);
            }

            SongNames.Insert(0, Name);
        }
    }

    // intf_P1_05.inc:194

}
