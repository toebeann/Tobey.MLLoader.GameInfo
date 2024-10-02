﻿using HarmonyLib.Tools;
using System;

namespace Tobey.MLLoader.GameInfo.Utility;
internal static class TraverseHelper
{
    public static T SuppressHarmonyWarnings<T>(Func<T> fn)
    {
        var initialFilter = Logger.ChannelFilter;
        Logger.ChannelFilter &= ~Logger.LogChannel.Warn;
        try
        {
            return fn();
        }
        finally
        {
            Logger.ChannelFilter = initialFilter;
        }
    }
}
