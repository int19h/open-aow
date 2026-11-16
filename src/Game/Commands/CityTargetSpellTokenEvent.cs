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
            result = targetCity != null && targetCity.Player == TargetPlayer;
        }

        return result;
    }

    // CitySpells.pas:182
    public void SetLocation(MapCoordinate location) => _targetLocation = location;

    public void SetLocation(sbyte x, sbyte y, sbyte level) =>
        SetLocation(new MapCoordinate(x, y, level));

    // CitySpells.pas:204
    public City GetSelectedCity() => (City)StrategicMap.Current.MapContainer.GetFieldMapCoordinate(_targetLocation).FindNoneTransparentHexSprite(typeof(City));

    public override bool Process(Troop source, int value) =>
        GamePack.Current.SpellControl.GetSpell(EventSpellId).ExecuteTokenEvent(this, source: source, executeCount: value);

    // NetworkE.pas:230

}
