using System;
using System.Reflection;
using HarmonyLib;
using Multiplayer.API;
using Verse;

namespace ComAil
{
	// Token: 0x0200000F RID: 15
	[StaticConstructorOnStartup]
	internal static class MultiplayerSupport
	{
		// Token: 0x0600003C RID: 60 RVA: 0x00003CC4 File Offset: 0x00001EC4
		static MultiplayerSupport()
		{
			if (!MP.enabled)
			{
				return;
			}
			MethodInfo[] array = new MethodInfo[]
			{
				AccessTools.Method(typeof(CommonAilments), "CommonAilmentsTick", null, null),
				AccessTools.Method(typeof(CommonAilments), "GetRndAilment", null, null),
				AccessTools.Method(typeof(CommonAilments), "GetBPHed", null, null),
				AccessTools.Method(typeof(CommonAilments), "GetBPR", null, null),
				AccessTools.Method(typeof(CommonAilments), "CanAddCA", null, null),
				AccessTools.Method(typeof(HediffComp_CACure), "SetTicksToCure", null, null),
				AccessTools.Method(typeof(PawnCAData), "PawnCADataTickSet", null, null),
				AccessTools.Method(typeof(GenerateRandomMinTicksToNextLovin_PostPatch), "GetARnd", null, null)
			};
			for (int i = 0; i < array.Length; i++)
			{
				MultiplayerSupport.FixRNG(array[i]);
			}
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00003DCD File Offset: 0x00001FCD
		private static void FixRNG(MethodInfo method)
		{
			MultiplayerSupport.harmony.Patch(method, new HarmonyMethod(typeof(MultiplayerSupport), "FixRNGPre", null), new HarmonyMethod(typeof(MultiplayerSupport), "FixRNGPos", null), null, null);
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00003E07 File Offset: 0x00002007
		private static void FixRNGPre()
		{
			Rand.PushState(Find.TickManager.TicksAbs);
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00003E18 File Offset: 0x00002018
		private static void FixRNGPos()
		{
			Rand.PopState();
		}

		// Token: 0x04000008 RID: 8
		private static Harmony harmony = new Harmony("rimworld.pelador.commonailments.multiplayersupport");
	}
}
