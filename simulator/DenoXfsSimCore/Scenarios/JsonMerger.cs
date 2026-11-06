using System.Text.Json.Nodes;

namespace DenoXfs.Sim.Core.Scenarios;

/// <summary>Deep merge used for profile fallback and for applying profile sections to command outputs.</summary>
public interface IJsonMerger
{
    /// <summary>
    /// Merges <paramref name="overlay"/> into <paramref name="target"/> (changed in place) and returns it: objects merge
    /// recursively, arrays and scalars replace, an explicit null sets null. The overlay is not changed.
    /// </summary>
    JsonObject Merge(JsonObject target, JsonObject overlay);
}

public sealed class JsonMerger : IJsonMerger
{
    public JsonObject Merge(JsonObject target, JsonObject overlay)
    {
        foreach (var (name, value) in overlay)
        {
            if (value is JsonObject overlayObject && target[name] is JsonObject targetObject)
                Merge(targetObject, overlayObject);
            else
                target[name] = value?.DeepClone();
        }
        return target;
    }
}
