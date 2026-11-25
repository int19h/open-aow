using Aow.Formats.Tree;

namespace Aow.Headless;

/// <summary>
/// The partial-load rows a run record stores. Each row is the class, the path and the message
/// of one child the loader dropped. The exception itself stays out of the record.
/// </summary>
internal static class DroppedObjects
{
    public static IReadOnlyList<object> Record(IReadOnlyList<DroppedObject> dropped)
    {
        object[] rows = new object[dropped.Count];
        for (int i = 0; i < dropped.Count; i++)
        {
            DroppedObject droppedObject = dropped[i];
            rows[i] = new
            {
                @class = droppedObject.ClassName,
                path = droppedObject.Path,
                message = droppedObject.Message,
            };
        }

        return Array.AsReadOnly(rows);
    }
}
