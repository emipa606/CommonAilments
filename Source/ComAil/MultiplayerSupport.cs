using System.Reflection;
using HarmonyLib;
using Multiplayer.API;
using Verse;

namespace ComAil;

[StaticConstructorOnStartup]
internal static class MultiplayerSupport
{
    private static readonly Harmony harmony = new Harmony("rimworld.pelador.commonailments.multiplayersupport");

    static MultiplayerSupport()
    {
        if (!MP.enabled)
        {
            return;
        }

        MethodInfo[] array =
        {
            AccessTools.Method(typeof(CommonAilments), "CommonAilmentsTick"),
            AccessTools.Method(typeof(CommonAilments), "GetRndAilment"),
            AccessTools.Method(typeof(CommonAilments), "GetBPHed"),
            AccessTools.Method(typeof(CommonAilments), "GetBPR"),
            AccessTools.Method(typeof(CommonAilments), "CanAddCA"),
            AccessTools.Method(typeof(HediffComp_CACure), "SetTicksToCure"),
            AccessTools.Method(typeof(PawnCAData), "PawnCADataTickSet"),
            AccessTools.Method(typeof(GenerateRandomMinTicksToNextLovin_PostPatch), "GetARnd")
        };
        foreach (var methodInfo in array)
        {
            FixRNG(methodInfo);
        }
    }

    private static void FixRNG(MethodInfo method)
    {
        harmony.Patch(method, new HarmonyMethod(typeof(MultiplayerSupport), "FixRNGPre"),
            new HarmonyMethod(typeof(MultiplayerSupport), "FixRNGPos"));
    }

    private static void FixRNGPre()
    {
        Rand.PushState(Find.TickManager.TicksAbs);
    }

    private static void FixRNGPos()
    {
        Rand.PopState();
    }
}