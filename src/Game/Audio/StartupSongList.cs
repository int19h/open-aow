// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.Audio;

// inherited Songs, SongNames
public class StartupSongList : SongList
{
    // AoWHSMap.PAS:1215
    public StartupSongList(CustomNode owner) : base(owner) => Songs.OnChange = OriginalSongsChanged;

    // AoWHSMap.PAS:1221
    public void OriginalSongsChanged(object sender)
    {
        SongNames.Clear();
        for (int i = Songs.Count - 1; i >= 0; i--)
        {
            string name;
            try
            {
                name = AudioPlayback.GetSongName(Songs[i]);
            }
            catch
            {
                name = AbilityRules.TranslateResourceString(GetEngine(), S: GameText.FileNotFoundText);
            }

            SongNames.Insert(0, value: name);
        }
    }
}
