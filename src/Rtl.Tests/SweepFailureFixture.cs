using System.Reflection;
using System.Text.Json;
using Aow.Units.AoWE;
using Aow.Units.Engine;
using Game = Aow.Units.AoWE.AoWE;

namespace Aow.Rtl.Tests;

internal static class SweepFailureFixture
{
    internal static readonly string[] Names =
    [
        "Aow.Units.City.TCity",
        "Aow.Units.HolyG.THolyGroundSpell",
        "Aow.Units.UnHolyG.TUnholyGroundSpell",
        "Aow.Units.Mountain.TRaiseTerrain",
        "Aow.Units.PoisonP.TPoisonPlantSpell",
        "Aow.Units.AoWE.TAoWMapLevel",
        "Aow.Units.AoWTC.TAoWCombatMapLevel",
    ];

    private sealed class Owner(bool reject) : TECustomNode(null!)
    {
        internal readonly List<TEObject> Members = [];
        internal readonly List<string> Events = [];
        internal TEObject? Last;
        public override int AddChild(TEObject child)
        {
            Last = child;
            Events.Add("insert:" + ReferenceEquals(this, child.Owner));
            Members.Add(child);
            if (reject) throw new InvalidDataException("owner rejects after insertion");
            return Members.Count - 1;
        }
        public override int RemoveChild(TEObject child)
        {
            Events.Add("remove:" + ReferenceEquals(Last, child));
            int index = Members.IndexOf(child);
            Members.Remove(child);
            return index;
        }
    }

    internal static string Observe(string name, bool reject, int route)
    {
        var previousEngine = Game.AoWEngine;
        var dictionary = EngineFixture.Engine.FDictionary;
        Game.AoWEngine = EngineFixture.Engine;
        Game.AoWEngine.FDictionary = null!;
        var owner = new Owner(reject);
        string error = "no failure";
        Type type = typeof(TSpell).Assembly.GetType(name)!;
        try
        {
            if (route == 0)
            {
                MethodInfo factory = type.GetMethod("Create", BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly, [typeof(TECustomNode)])!;
                factory.Invoke(null, [owner]);
            }
            else if (route == 1) EClass.Of(type).Create(owner);
            else if (route == 2) ((TEObject.Meta)MetaRegistry.Of(type)).Create(owner);
            else
            {
                if (Aow.Units.Engine.Engine.Engines is null) Aow.Units.Engine.Engine.Initialize();
                var engine = new TEngine(null!);
                try
                {
                    EClass descriptor = EClass.Of(type);
                    engine.Options -= TSet<TEngineModeElement>.Of(Aow.Units.Engine.Engine.eoVerifyClasses);
                    try { engine.UnregisterEClasses([engine.GetEClass(descriptor.ClassID)]); }
                    catch (Aow.Rtl.Exception) { }
                    engine.RegisterEClasses([descriptor]);
                    engine.CreateEObject(descriptor.ClassID, owner);
                }
                finally { engine.Destroy(); }
            }
        }
        catch (System.Exception exception)
        {
            if (exception is TargetInvocationException { InnerException: { } inner }) exception = inner;
            error = exception.GetType().FullName + ":" + exception.Message;
        }
        finally { EngineFixture.Engine.FDictionary = dictionary; Game.AoWEngine = previousEngine; }
        return JsonSerializer.Serialize(new { Error = error, owner.Events, Count = owner.Members.Count, HasOwner = owner.Last?.Owner is not null });
    }
}
