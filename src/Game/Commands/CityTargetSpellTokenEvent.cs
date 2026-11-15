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
public partial class CityTargetSpellTokenEvent : SpellTokenEvent
{
    public CityTargetSpellTokenEvent(CustomNode owner) : base(owner)
    { }

    // CitySpells.pas:170
    public new const int ClassId = 135949;

    private MapCoordinate _targetLocation;

    // The fourth byte of the packed location stored at id 20, 0 from every writer and in every
    // corpus file, kept so that a loaded value saves back unchanged.
    public byte UnknownBytes { get; set; }

    public sbyte TargetPlayer { get; set; }

    // CitySpells.pas:175
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        int location = StoredCoordinateImage.From(_targetLocation, unknownBytes: UnknownBytes).ToInt32();
        location = persister.Int32("TargetLocation", value: location);
        (_targetLocation, UnknownBytes) = StoredCoordinateImage.FromInt32(location);
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
            if (targetCity == null || targetCity.Player != TargetPlayer)
            {
                result = false;
            }
            else
            {
                result = true;
            }
        }

        return result;
    }

    // CitySpells.pas:182
    public void SetLocation(sbyte X, sbyte Y, sbyte L)
    {
        _targetLocation = new MapCoordinate(X: X, Y: Y, MapLevel: L);
    }

    // CitySpells.pas:204
    public City GetSelectedCity() => (City)StrategicMap.Current.MapContainer.GetFieldMapCoordinate(_targetLocation).FindNoneTransparentHexSprite(typeof(City));

    // NetworkE.pas:230

}
