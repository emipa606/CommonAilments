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

    private static void Setup()
    {
        var allDefsListForReading = DefDatabase<HediffDef>.AllDefsListForReading;
        var comAilDefNames = CommonAilmentsUtility.GetComAilDefNames();
        var symptomaticList = CommonAilmentsUtility.Symptomatic();
        var symptomatic = 0;
        foreach (var hediffDef in allDefsListForReading)
        {
            if (comAilDefNames.Contains(hediffDef.defName))
            {
                hediffDef.scenarioCanAdd = Controller.Settings.CACanAddScenario;
            }

            if (symptomaticList.Contains(hediffDef.defName))
            {
                applySymptoms(hediffDef, typeof(HediffCompProperties_CASymptom),
                    ref symptomatic);
            }
        }

        if (symptomatic > 0)
        {
            Log.Message("ComAil.Symptomatic".Translate(symptomatic.ToString()));
        }
    }

    private static void applySymptoms(HediffDef def, Type compType, ref int symptomatic)
    {
        if (def.HasComp(compType))
        {
            return;
        }

        def.comps ??= [];

        def.comps.Add((HediffCompProperties)Activator.CreateInstance(compType));
        symptomatic++;
    }
}