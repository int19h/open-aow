#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
// Translated from CitySpells.pas.
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;
using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Game.World.Map;
using Aow.Game.World.Structures;

namespace Aow.Game.Commands;

// { ot=1 }
// packed target coordinates. target owner
public class CityTargetSpellTokenEvent(CustomNode owner) : SpellTokenEvent(owner)
{

    // CitySpells.pas:170
    public new const int ClassId = 135_949;

    private MapCoordinate _targetLocation;

    public PlayerId? TargetPlayer { get; set; } = PlayerId.Independent;

    // CitySpells.pas:175
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        int location = persister.Int32("TargetLocation", value: StoredCoordinateImage.From(_targetLocation).ToInt32());
        _targetLocation = StoredCoordinateImage.FromInt32(location).Location;
        TargetPlayer = persister.OptionalPlayer(name: "TargetPlayer", value: TargetPlayer, width: IdWidth.UnsignedByte, fallback: 0);
    }

    // CitySpells.pas:189
    public override bool Validate(Hero source)
    {
        bool result = base.Validate(source);
        if (result)
        {
            City targetCity = GetSelectedCity();
            return targetCity is not null && targetCity.Player == TargetPlayer;
        }

        return result;
    }

    // CitySpells.pas:182
    public void SetLocation(MapCoordinate location) => _targetLocation = location;

    public void SetLocation(int x, int y, int level)
        => SetLocation(new MapCoordinate((sbyte)x, (sbyte)y, (sbyte)level));

    // CitySpells.pas:204
    public City GetSelectedCity() => GetEngine().StrategicMap.MapContainer.GetFieldMapCoordinate(_targetLocation).FindNoneTransparentHexSprite<City>();
}
