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
            LongEventHandler.QueueLongEvent(Setup, "LibraryStartup", false, null);
        }

        // Token: 0x06000004 RID: 4 RVA: 0x00002144 File Offset: 0x00000344
        public static void Setup()
        {
            var allDefsListForReading = DefDatabase<HediffDef>.AllDefsListForReading;
            var ComAilDefNames = CommonAilmentsUtility.GetComAilDefNames();
            var SymptomaticList = CommonAilmentsUtility.Symptomatic();
            var symptomatic = 0;
            checked
            {
                foreach (var hediffDef in allDefsListForReading)
                {
                    if (ComAilDefNames.Contains(hediffDef.defName))
                    {
                        hediffDef.scenarioCanAdd = Controller.Settings.CACanAddScenario;
                    }

                    if (SymptomaticList.Contains(hediffDef.defName))
                    {
                        ApplySymptoms(hediffDef, typeof(HediffCompProperties_CASymptom),
                            ref symptomatic);
                    }
                }

                if (symptomatic > 0)
                {
                    Log.Message("ComAil.Symptomatic".Translate(symptomatic.ToString()));
                }
            }
        }

        // Token: 0x06000005 RID: 5 RVA: 0x000021F9 File Offset: 0x000003F9
        public static void ApplySymptoms(HediffDef def, Type compType, ref int symptomatic)
        {
            if (def.HasComp(compType))
            {
                return;
            }

            if (def.comps == null)
            {
                def.comps = new List<HediffCompProperties>();
            }

            def.comps.Add((HediffCompProperties) Activator.CreateInstance(compType));
            symptomatic++;
        }
    }
}