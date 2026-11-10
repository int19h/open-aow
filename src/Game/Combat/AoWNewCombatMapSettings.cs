// Translated from AoWTC.pas.
using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

namespace Aow.Game.Combat;

// Inherited Width. Height. Levels. HSSFileName. HSMapClassID. MapLevelClassID
public partial class AoWNewCombatMapSettings : NewHSMapSettings
{
    public AoWNewCombatMapSettings() : base()
    {
        HSMapClassID = 2228480;
        MapLevelClassID = 2228482;
        Width = 65;
        Height = 59;
        Levels = 1;
        HSSFileName = GameTables.AoWEngine.HSSFileName;
    }

    // AoWTC.pas:2147

    // AoWTC.pas:70

}
