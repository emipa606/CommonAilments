using System.Reflection;
using HarmonyLib;
using Verse;

namespace ComAil;

[StaticConstructorOnStartup]
public static class HarmonySetup
{
    static HarmonySetup()
    {
        new Harmony("CommonAilments.Pelador").PatchAll(Assembly.GetExecutingAssembly());
    }
}