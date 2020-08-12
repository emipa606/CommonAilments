using System;
using System.Reflection;
using HarmonyLib;
using Verse;

namespace ComAil
{
	// Token: 0x02000009 RID: 9
	[StaticConstructorOnStartup]
	public static class HarmonySetup
	{
		// Token: 0x0600002E RID: 46 RVA: 0x00003980 File Offset: 0x00001B80
		static HarmonySetup()
		{
			new Harmony("CommonAilments.Pelador").PatchAll(Assembly.GetExecutingAssembly());
		}
	}
}
