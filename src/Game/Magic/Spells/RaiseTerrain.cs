// Translated from Mountain.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;
using Aow.Game.Commands;
using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using Aow.Game.World.Structures;

using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

public partial class RaiseTerrain : GlobalTargetSpell
{
    public RaiseTerrain(ECustomNode owner) : base(owner)
    {
        FSpellName = AbilityRules.TranslateRStr(RaiseTerrainRStr);
        FSpellID = 19;
        FBaseManaCost = 5;
        FSpellLevel = 4;
        FMagicSphere = MagicSphere.Earth;
        FAboveGroundOnly = true;
    }

    // Mountain.pas:327

    // Mountain.pas:364
    public override void SetupTargetSelection(Troop Source, TargetSelectionControl TargetSelection)
    {
        base.SetupTargetSelection(Source, TargetSelection);
        TargetSelection.FHNs.Clear();
        for (int I = 0, last = HexGeometry.RadToHN(2) - 1; I <= last; I++)
        {
            TargetSelection.FHNs.Add(0);
        }
    }

    // Mountain.pas:373
    public override bool TargetSelected(Troop Source, int X, int Y, int L)
    {
        RaiseTerrainTokenEvent Event;
        Event = new RaiseTerrainTokenEvent(null);
        Event.Initialize(FSpellID, (byte)Source.FPlayer, Source.FUnitID, FBaseManaCost);
        Event.SetLocation((sbyte)X, (sbyte)Y, (sbyte)L);
        AoWHSMap.Current.FTokenManager.GetTokenControl(Source.FPlayer).AddEvent(Event);
        Event.Release();
        return true;
    }

    // Mountain.pas:347
    public override bool ValidTargetSelectionMapF(MapField Field)
    {
        bool Valid;
        ArmyHexSprite Army;
        Valid = base.ValidTargetSelectionMapF(Field) && !Set<byte>.WithBounds(0, 255).With(0).With(6).With(10).With(13).Contains((byte)Field.TerrainType) && !Set<byte>.WithBounds(0, 255).With(0).Contains((byte)Field.OverlayType) && Field.FindNoneTransparentHS(typeof(Structure)) == null;
        if (Valid)
        {
            Army = ((AoWMapField)Field).GetArmyHS();
            if (Army != null)
            {
                Valid = !Army.VisibleForSeatedPlayer();
            }
        }

        return Valid;
    }

    // Mountain.pas:338
    public override bool ValidTargetMapF(MapField Field) => base.ValidTargetMapF(Field) && !Set<byte>.WithBounds(0, 255).With(0).With(6).With(10).With(13).Contains((byte)Field.TerrainType) && !Set<byte>.WithBounds(0, 255).With(0).Contains((byte)Field.OverlayType) && Field.FindNoneTransparentHS(typeof(Structure)) == null && ((AoWMapField)Field).GetArmyHS() == null;

    private static readonly string RaiseTerrainRStr = "Raise Terrain";

    // Mountain.pas:77

    // Mountain.pas:77

}
