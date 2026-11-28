using Aow.Engine;
using Aow.Formats;
using Aow.Formats.Json;
using Aow.Formats.Schema;
using Aow.Formats.Tree;
using Aow.Game.Commands;
using Aow.Rtl;

namespace Aow.Game.Recording;

/// <summary>
/// The own-format document node of a token event (issue 343): the tree the event's persistence
/// writes, mapped by the document backend under the event's stored class. Writing it reads the
/// event and changes nothing, as the packet a network game sends of the same event.
/// </summary>
/// <remarks>
/// A recording takes the tree when the event first executes and maps it after the game. The
/// backend fills caches of its own the first time it maps a class, and the identity hashes it
/// takes then move the hashes the game takes later (<see cref="IsolatedThread"/>). The tree
/// writer does not.
/// </remarks>
public static class TokenEventDocument
{
    /// <summary>The stored class name of the event, the class its document node is read back as.</summary>
    public static string ClassName(TokenEvent tokenEvent) => NameTable.Stored(tokenEvent.GetType());

    /// <summary>The tree of the event as its persistence writes it now.</summary>
    public static ObjectNode Capture(TokenEvent tokenEvent, GameEngine engine) => TreeWriter.Write(tokenEvent, factory: new EnginePersistableFactory(engine));

    /// <summary>The document node of a captured tree of the stored class <paramref name="className"/>.</summary>
    public static JsonObject Write(ObjectNode tree, string className)
    {
        FormatSchema schema = FormatSchema.Current;
        SchemaClass classValue = schema.Get(className);
        return DocumentBackend.Write(tree, classValue: classValue, schema: schema, engine: GameBoundary.PacketClassTable(classValue));
    }

    public static JsonObject Write(TokenEvent tokenEvent, GameEngine engine) => Write(Capture(tokenEvent, engine: engine), className: ClassName(tokenEvent));

    /// <summary>
    /// The document of an event as it was added. A recording captures the event at its first
    /// execution, after the executor numbered it at the prefetch. An added event carries -1, and a
    /// replayed event with a number the executor has passed is dropped without a refusal
    /// (TokenExecutor.PrefetchTokenEvent), so the recorded document carries -1.
    /// </summary>
    public static JsonObject AsAdded(JsonObject document)
    {
        JsonMember? counter = document.Find("TokenCounter");
        if (counter is not null)
        {
            counter.Value = new JsonInteger(-1);
        }
        else
        {
            document.Members.Insert(0, item: new JsonMember("TokenCounter", value: new JsonInteger(-1)));
        }

        return document;
    }
}
