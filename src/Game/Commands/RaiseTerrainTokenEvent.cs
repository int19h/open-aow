// Translated from Mountain.pas.
using Aow.Engine.Nodes;

using Aow.Game.Draw.Animations;

using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;

using Aow.Game.World;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

using Aow.Game.World.Terrain;
using Aow.Rtl;

namespace Aow.Game.Commands;

public partial class RaiseTerrainTokenEvent : GlobalTargetSpellTokenEvent
{
    public RaiseTerrainTokenEvent(ECustomNode owner) : base(owner)
    { }

    // Mountain.pas:258
    public new const int ClassId = 135946;

    // Mountain.pas:317
    public override bool Process(Troop source, int value)
    {
        base.Process(source, value);
        RaiseTerrain(source);
        AoWHSMap.Current.ValidateTerrainObservation(source.Player, Location.X, Location.Y, Location.MapLevel, 1);
        AoWHSSet.Current.SpellControl.GetSpell(EventSpellId).CastingDone(source, EventMana);
        return false;
    }

    // Mountain.pas:261
    public void RaiseTerrain(Troop caster)
    {
        DelphiList resources = default;
        MapLevel level = default;
        HexNumberWalkData walk = default;
        AoWMapField field = default;
        RaisedMountain mountain = default;
        MapObjectResource resource = default;
        int I = default;
        bool found = default;
        ImageSequenceAnimation animation = default;
        resources = AoWHSSet.Current.MapObjectResourceList._children;
        level = AoWHSMap.Current.MapContainer.MapLevels[Location.MapLevel];
        HexNumberWalkData.InitializeHexNumberWalk(ref walk, Location.X, Location.Y, 0);
        while (walk.Radius <= 1)
        {
            if (walk.X >= 0 && walk.Y >= 0 && walk.X < level.Width && walk.Y < level.Height)
            {
                field = (AoWMapField)level.Field[walk.X, walk.Y];
                resource = null;
                if (((GlobalTargetSpell)AoWHSSet.Current.SpellControl.GetSpell(19)).ValidTargetMapField(field))
                {
                    found = false;
                    I = resources.Count;
                    while (!found && I != 0)
                    {
                        I -= 1;
                        resource = (MapObjectResource)resources.List[I];
                        found = resource.EngineClass.ClassId == 131435 && resource.TerrainList.UsedTerrain() == 1;
                    }

                    if (found)
                    {
                        field.RestoreTerrain();
                        mountain = (RaisedMountain)resource.CreateResourceUser();
                        mountain.Duration = (sbyte)((AoWHSMap.Current.Random(11) - 5) / 5 + 3);
                        mountain.Player = caster.Player;
                        mountain.OriginalOverlay = field.OverlayType;
                        AoWHSMap.Current.ClearTerrain(walk.X, walk.Y, Location.MapLevel, 0, -1);
                        if (AoWHSMap.Current.PlaceHex(mountain, walk.X, walk.Y, Location.MapLevel))
                        {
                            mountain.Mode = mountain.Mode + Set<RaisedMountainMode>.Of(RaisedMountainMode.Raising);
                            animation = new ImageSequenceAnimation(null);
                            animation.SetImageSequence(AoWHSSet.Current.SpellControl.GetSpell(19).CastImages.ImageSequences[0]);
                            animation.OnShow = mountain.ShowRaiseTerrainAnimation;
                            animation.OnNewFrame = mountain.NewFrameRaiseTerrainAnimation;
                            animation.FrameCounter = -AoWHSMap.Current.Random(15);
                            AoWHSMap.Current.PlaceHex(animation, walk.X, walk.Y, Location.MapLevel);
                        }
                    }
                }
            }

            HexNumberWalkData.WalkToNextHexNumber(ref walk);
        }
    }

    // NetworkE.pas:230

}
