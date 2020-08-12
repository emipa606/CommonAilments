using System;
using HarmonyLib;
using RimWorld;
using Verse;

namespace ComAil
{
	// Token: 0x0200000E RID: 14
	[HarmonyPatch(typeof(IncidentQueue), "IncidentQueueTick")]
	public class IncidentQueueTick_PostPatch
	{
		// Token: 0x0600003A RID: 58 RVA: 0x00003C84 File Offset: 0x00001E84
		[HarmonyPostfix]
		public static void PostFix()
		{
			if (Controller.Settings.DoAilments && GenDate.DaysPassed >= Controller.Settings.CAStartDays && (Find.TickManager.TicksGame + 5) % 600 == 0)
			{
				CommonAilments.CommonAilmentsTick();
			}
		}
	}
}
