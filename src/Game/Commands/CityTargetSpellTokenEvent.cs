// Translated from CitySpells.pas.
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;
using Aow.Game.Magic.Abilities;
using Aow.Game.World.Map;
using Aow.Game.World.Structures;

namespace Aow.Game.Commands;

// { ot=1 }
// packed target coordinates. target owner
public partial class CityTargetSpellTokenEvent(CustomNode owner) : SpellTokenEvent(owner)
{

    // CitySpells.pas:170
    public new const int ClassId = 135949;

    private MapCoordinate _targetLocation;

    public sbyte TargetPlayer { get; set; }

    // CitySpells.pas:175
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        int location = persister.Int32("TargetLocation", value: StoredCoordinateImage.From(_targetLocation).ToInt32());
        _targetLocation = StoredCoordinateImage.FromInt32(location).Location;
        TargetPlayer = PersistSupport.UnsignedByte(persister, name: "TargetPlayer", value: TargetPlayer);
    }

    // CitySpells.pas:189
    public override bool Validate(Hero source)
    {
        City targetCity;
        bool result = base.Validate(source);
        if (result)
        {
            targetCity = GetSelectedCity();
            result = targetCity is not null && targetCity.Player == TargetPlayer;
        }

        return result;
    }

    // CitySpells.pas:182
    public void SetLocation(MapCoordinate location) => _targetLocation = location;

    public void SetLocation(sbyte x, sbyte y, sbyte level) =>
        SetLocation(new MapCoordinate(x, y, level));

    // CitySpells.pas:204
    public City GetSelectedCity() => StrategicMap.Current.MapContainer.GetFieldMapCoordinate(_targetLocation).FindNoneTransparentHexSprite<City>();

    // NetworkE.pas:230

}
