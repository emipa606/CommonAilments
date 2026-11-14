using HarmonyLib;
using RimWorld;
using Verse;

namespace ComAil;

[HarmonyPatch(typeof(IncidentQueue), "IncidentQueueTick")]
public static class IncidentQueue_IncidentQueueTick
{
    public static void Postfix()
    {
        if (Controller.Settings.DoAilments && GenDate.DaysPassed >= Controller.Settings.CAStartDays &&
            (Find.TickManager.TicksGame + 5) % 600 == 0)
        {
            CommonAilments.CommonAilmentsTick();
        }
    }
}