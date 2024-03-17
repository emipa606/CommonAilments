using System;
using Verse;

namespace ComAil;

[StaticConstructorOnStartup]
internal static class ComAil_Options_Initializer
{
    static ComAil_Options_Initializer()
    {
        LongEventHandler.QueueLongEvent(Setup, "LibraryStartup", false, null);
    }

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

    public static void ApplySymptoms(HediffDef def, Type compType, ref int symptomatic)
    {
        if (def.HasComp(compType))
        {
            return;
        }

        if (def.comps == null)
        {
            def.comps = [];
        }

        def.comps.Add((HediffCompProperties)Activator.CreateInstance(compType));
        symptomatic++;
    }
}