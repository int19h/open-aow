#nullable disable
// Translated from Storms.pas.
using System.Runtime.CompilerServices;
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Formats.Tree;
using Aow.Game.Draw.Animations;

using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;

using Aow.Game.World.Map;

namespace Aow.Game.Commands;

// storm, X, Y, layer
public partial class StormSpellTokenEvent : SpellTokenEvent
{
    public StormSpellTokenEvent(CustomNode owner) : base(owner)
    { }

    // Storms.pas:730
    public new const int ClassId = 135937;

    private Storm _storm;

    public sbyte X { get; set; }
    public sbyte Y { get; set; }
    public sbyte MapLevel { get; set; }

    // Storms.pas:735
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        X = PersistSupport.UnsignedByte(persister, name: "X", value: X);
        Y = PersistSupport.UnsignedByte(persister, name: "Y", value: Y);
        MapLevel = PersistSupport.UnsignedByte(persister, name: "MapLevel", value: MapLevel);
    }

    // Storms.pas:743
    public override bool Process(Troop source, int value)
    {
        bool result;
        base.Process(source, value);
        if (value == 0)
        {
            _storm = ((StormSpell)GamePack.Current.SpellControl.GetSpell(EventSpellId)).CreateStorm();
            _storm.InitializeStorm(source.Player, X, Y, MapLevel);
            GamePack.Current.SpellControl.GetSpell(EventSpellId).CastingDone(source, EventMana);
        }

        if (!_storm.UpdateStorm())
        {
            _storm.FinalizeStorm();
            _storm?.Dispose();
            _storm = null;
            result = false;
        }
        else
        {
            result = true;
        }

        return result;
    }

    // NetworkE.pas:230

}
