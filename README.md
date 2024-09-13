# Tobey's Game Info Fix for BepInEx.MelonLoader.Loader

A simple patch to ensure the game info logs of [BepInEx.MelonLoader.Loader](https://github.com/BepInEx/BepInEx.MelonLoader.Loader) are correct.

## Why?

It's nice to have the correct game info in the logs, it's useful for troubleshooting.

Unfortunately, the version of MelonLoader shipped with BepInEx.MelonLoader.Loader uses [AssetsTools.NET](https://github.com/nesrak1/AssetsTools.NET) to parse the info from the `globalgamemanagers` file manually, which is only going to work correctly assuming the version of AssetsTools.NET installed has up-to-date information on how to parse that file, which it often does not. This leads to incorrect information sometimes being parsed from the file.

To address this, Tobey.MLLoader.GameInfo relies on Unity's own API for gathering the same information, which is available since at least Unity 5.6. Of course, this relies on Unity to have been initialised far enough to have parsed this information itself. This only works as long as MelonLoader is gathering the info after Unity has been initialised enough to have parsed it, which it usually is by the time BepInEx.MelonLoader.Loader is loaded.

## Usage

Just plop the contents of the downloaded .zip from [the releases page](https://github.com/toebeann/Tobey.MLLoader.GameInfo/releases) into your game folder (after installing [BepInEx](https://github.com/BepInEx/BepInEx) and [BepInEx.MelonLoader.Loader](https://github.com/BepInEx/BepInEx.MelonLoader.Loader), of course).
