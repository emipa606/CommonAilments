using HarmonyLib;
using RimWorld;
using Verse;

namespace ComAil;

[HarmonyPatch(typeof(IncidentQueue), "IncidentQueueTick")]
public class IncidentQueueTick_PostPatch
{
    [HarmonyPostfix]
    public static void PostFix()
    {
        if (Controller.Settings.DoAilments && GenDate.DaysPassed >= Controller.Settings.CAStartDays &&
            (Find.TickManager.TicksGame + 5) % 600 == 0)
        {
            CommonAilments.CommonAilmentsTick();
        }
    }
}