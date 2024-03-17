using System;
using System.Linq;
using RimWorld;
using Verse;

namespace ComAil;

[StaticConstructorOnStartup]
public static class ComAil_Setup
{
    static ComAil_Setup()
    {
        ComAil_Setup_Pawns();
    }

    private static void ComAil_Setup_Pawns()
    {
        var Organic = DefDatabase<HediffGiverSetDef>.GetNamed("OrganicStandard");
        ComAilSetup_Comp(typeof(PawnCAData.CompProperties_PawnCAData), delegate(ThingDef def)
        {
            var race = def.race;
            if (race == null || !race.Humanlike)
            {
                return false;
            }

            var race2 = def.race;
            return race2?.hediffGiverSets != null &&
                   def.race.hediffGiverSets.Contains(Organic);
        });
    }

    private static void ComAilSetup_Comp(Type compType, Func<ThingDef, bool> qualifier)
    {
        var list = DefDatabase<ThingDef>.AllDefsListForReading.Where(qualifier).ToList();
        list.RemoveDuplicates();
        foreach (var def in list)
        {
            if (def.comps != null && !def.comps.Any(c => c.GetType() == compType))
            {
                def.comps.Add((CompProperties)Activator.CreateInstance(compType));
            }
        }
    }
}