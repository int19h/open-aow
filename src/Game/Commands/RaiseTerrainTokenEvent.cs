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
    public new const int ClassID = 135946;

    // Mountain.pas:317
    public override bool Process(Troop source, int value)
    {
        base.Process(source, value);
        RaiseTerrain(source);
        AoWHSMap.Current.ValidateTerrainObservation(source.FPlayer, FLocation.X, FLocation.Y, FLocation.L, 1);
        AoWHSSet.Current.FSpellControl.GetSpell(FEventSpellID).CastingDone(source, FEventMana);
        return false;
    }

    // Mountain.pas:261
    public void RaiseTerrain(Troop Caster)
    {
        DelphiList Resources = default;
        MapLevel Level = default;
        HexNumberWalkData Walk = default;
        AoWMapField Field = default;
        RaisedMountain Mountain = default;
        MapObjectResource Resource = default;
        int I = default;
        bool Found = default;
        ImageSequenceAnimation Anim = default;
        Resources = AoWHSSet.Current.MapObjectResourceList.FChildren;
        Level = AoWHSMap.Current.MapContainer.MapLevels[FLocation.L];
        HexNumberWalkData.InitHNWalk(ref Walk, FLocation.X, FLocation.Y, 0);
        while (Walk.Radius <= 1)
        {
            if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < Level.Width && Walk.Y < Level.Height)
            {
                Field = (AoWMapField)Level.Field[Walk.X, Walk.Y];
                Resource = null;
                if (((GlobalTargetSpell)AoWHSSet.Current.FSpellControl.GetSpell(19)).ValidTargetMapF(Field))
                {
                    Found = false;
                    I = Resources.Count;
                    while (!Found && I != 0)
                    {
                        I -= 1;
                        Resource = (MapObjectResource)Resources.List[I];
                        Found = Resource.EClass.ClassID == 131435 && Resource.TerrainList.UsedTerrain() == 1;
                    }

                    if (Found)
                    {
                        Field.RestoreTerrain();
                        Mountain = (RaisedMountain)Resource.CreateResourceUser();
                        Mountain.FDuration = (sbyte)((AoWHSMap.Current.Random(11) - 5) / 5 + 3);
                        Mountain.FPlayer = Caster.FPlayer;
                        Mountain.FOriginalOverlay = Field.OverlayType;
                        AoWHSMap.Current.ClearTerrain(Walk.X, Walk.Y, FLocation.L, 0, -1);
                        if (AoWHSMap.Current.PlaceHX(Mountain, Walk.X, Walk.Y, FLocation.L))
                        {
                            Mountain.FMode = Mountain.FMode + Set<RaisedMountainMode>.Of(RaisedMountainMode.Raising);
                            Anim = new ImageSequenceAnimation(null);
                            Anim.SetImageSequence(AoWHSSet.Current.FSpellControl.GetSpell(19).FCastImages.ImageSequences[0]);
                            Anim.FOnShow = Mountain.ShowRaiseTerrainAnimation;
                            Anim.FOnNewFrame = Mountain.NewFrameRaiseTerrainAnimation;
                            Anim.FFrameCounter = -AoWHSMap.Current.Random(15);
                            AoWHSMap.Current.PlaceHX(Anim, Walk.X, Walk.Y, FLocation.L);
                        }
                    }
                }
            }

            HexNumberWalkData.WalkToNextHN(ref Walk);
        }
    }

    // NetworkE.pas:230

}
