// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.Audio;

// inherited Songs, SongNames
public partial class StartupSongList : SongList
{
    // AoWHSMap.PAS:1215
    public StartupSongList(ECustomNode owner) : base(owner)
    {
        Songs.OnChange = OriginalSongsChanged;
    }

    // AoWHSMap.PAS:1221
    public void OriginalSongsChanged(object sender)
    {
        string name;
        SongNames.Clear();
        for (int I = Songs.Count - 1; I >= 0; I--)
        {
            try
            {
                name = AudioPlayback.GetSongName(Songs[I]);
            }
            catch
            {
                name = AbilityRules.TranslateResourceString(GetEngine(), GameText.FileNotFoundText);
            }

            SongNames.Insert(0, name);
        }
    }

    // intf_P1_05.inc:194

}
