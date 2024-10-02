using BepInEx.Logging;
using HarmonyLib;
using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Tobey.MLLoader.GameInfo.ExtensionMethods;

namespace Tobey.MLLoader.GameInfo;

[HarmonyWrapSafe]
public static class Patcher
{
    // Without the contents of this region, the patcher will not be loaded by BepInEx - do not remove!
    #region BepInEx Patcher Contract
    public static IEnumerable<string> TargetDLLs { get; } = [];
    public static void Patch(AssemblyDefinition _) { }
    #endregion

    private static Harmony? harmony;

    private static ManualLogSource? logger = Logger.CreateLogSource("Tobey.MLLoader.GameInfo");

    // entry point - do not delete or rename!
    public static void Finish()
    {
        try
        {
            Assembly.Load("MelonLoader");
            harmony = Harmony.CreateAndPatchAll(typeof(Patcher));
        }
        catch (FileNotFoundException)
        {
            logger!.LogWarning("MelonLoader was not found, patch skipped.");
            Logger.Sources.Remove(logger);
            logger!.Dispose();
            logger = null;
        }
    }

    public static MethodBase TargetMethod() => AccessTools.Method("MelonLoader.InternalUtils.UnityInformationHandler:ReadGameInfo");

    public static void Postfix()
    {
        try
        {
            var unityInformationHandler = Traverse.CreateWithType("MelonLoader.InternalUtils.UnityInformationHandler");
            var application = Traverse.CreateWithType("UnityEngine.Application");

            var getValue = (string name) => application.OptionalProperty(name) switch
            {
                Traverse t when t.PropertyExists() => t.GetValue<string>(),
                _ => application.Field<string>(name).Value,
            };

            unityInformationHandler.Property<string>("GameVersion").Value = getValue("version");
            unityInformationHandler.Property<string>("GameName").Value = getValue("productName");
            unityInformationHandler.Property<string>("GameDeveloper").Value = getValue("companyName");
            unityInformationHandler.Property("EngineVersion").SetValue(
                Traverse.CreateWithType("AssetRipper.VersionUtilities.UnityVersion")
                .Method("Parse", [getValue("unityVersion")]).GetValue());

            logger!.LogMessage("Successfully patched MelonLoader game info.");
        }
        catch (Exception ex)
        {
            logger!.LogError(ex);
        }
        finally
        {
            harmony!.UnpatchSelf();
            harmony = null;
            Logger.Sources.Remove(logger);
            logger!.Dispose();
            logger = null;
        }
    }
}
