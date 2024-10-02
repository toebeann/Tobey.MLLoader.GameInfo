using HarmonyLib;
using Tobey.MLLoader.GameInfo.Utility;

namespace Tobey.MLLoader.GameInfo.ExtensionMethods;
internal static class TraverseExtensions
{
    public static Traverse OptionalProperty(this Traverse traverse, string name, object[]? index = null) =>
        TraverseHelper.SuppressHarmonyWarnings(() => traverse.Property(name, index));

    public static Traverse<T> OptionalProperty<T>(this Traverse traverse, string name, object[]? index = null) =>
        TraverseHelper.SuppressHarmonyWarnings(() => traverse.Property<T>(name, index));
}
