using System;
using System.Linq;
using RimWorld;
using Verse;

namespace ComAil;

public class PawnCAData : ThingComp
{
    public int PawnCATick;

    private Pawn pawn => (Pawn)parent;

    public override void PostExposeData()
    {
        base.PostExposeData();
        Scribe_Values.Look(ref PawnCATick, "PawnCATick");
    }

    public static void PawnCADataTickSet(Pawn pawn, bool symptom = false)
    {
        ThingComp PComp = pawn?.TryGetComp<PawnCAData>();

        var PData = (PawnCAData)PComp;
        if (PData == null)
        {
            return;
        }

        if (!symptom)
        {
            PData.PawnCATick = Find.TickManager.TicksGame + (Controller.Settings.CAFrequency * 2500) +
                               Rand.Range(-7500, 7500);
            return;
        }

        PData.PawnCATick = Find.TickManager.TicksGame +
                           (int)(Controller.Settings.CAFrequency / 2f * 2500f) + Rand.Range(-2500, 2500);
    }

    public class CompProperties_PawnCAData : CompProperties
    {
        public CompProperties_PawnCAData()
        {
            compClass = typeof(PawnCAData);
        }
    }

    [StaticConstructorOnStartup]
    private static class ComAil_Setup
    {
        static ComAil_Setup()
        {
            ComAil_Setup_Pawns();
        }

        private static void ComAil_Setup_Pawns()
        {
            var Organic = DefDatabase<HediffGiverSetDef>.GetNamed("OrganicStandard");
            ComAilSetup_Comp(typeof(CompProperties_PawnCAData), delegate(ThingDef def)
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
}