using System;
using System.Collections.Generic;
using Verse;

namespace ComAil
{
	// Token: 0x02000003 RID: 3
	[StaticConstructorOnStartup]
	internal static class ComAil_Options_Initializer
	{
		// Token: 0x06000003 RID: 3 RVA: 0x00002127 File Offset: 0x00000327
		static ComAil_Options_Initializer()
		{
			LongEventHandler.QueueLongEvent(new Action(ComAil_Options_Initializer.Setup), "LibraryStartup", false, null, true);
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00002144 File Offset: 0x00000344
		public static void Setup()
		{
			List<HediffDef> allDefsListForReading = DefDatabase<HediffDef>.AllDefsListForReading;
			List<string> ComAilDefNames = CommonAilmentsUtility.GetComAilDefNames();
			List<string> SymptomaticList = CommonAilmentsUtility.Symptomatic();
			int symptomatic = 0;
			checked
			{
				for (int i = 0; i < allDefsListForReading.Count; i++)
				{
					if (ComAilDefNames.Contains(allDefsListForReading[i].defName))
					{
						allDefsListForReading[i].scenarioCanAdd = Controller.Settings.CACanAddScenario;
					}
					if (SymptomaticList.Contains(allDefsListForReading[i].defName))
					{
						ComAil_Options_Initializer.ApplySymptoms(allDefsListForReading[i], typeof(HediffCompProperties_CASymptom), ref symptomatic);
					}
				}
				if (symptomatic > 0)
				{
					Log.Message("ComAil.Symptomatic".Translate(symptomatic.ToString()), false);
				}
			}
		}

		// Token: 0x06000005 RID: 5 RVA: 0x000021F9 File Offset: 0x000003F9
		public static void ApplySymptoms(HediffDef def, Type compType, ref int symptomatic)
		{
			if (!def.HasComp(compType))
			{
				if (((def != null) ? def.comps : null) == null)
				{
					def.comps = new List<HediffCompProperties>();
				}
				def.comps.Add((HediffCompProperties)Activator.CreateInstance(compType));
				symptomatic++;
			}
		}
	}
}
