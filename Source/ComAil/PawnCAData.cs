using System;
using System.Linq;
using RimWorld;
using Verse;

namespace ComAil
{
    // Token: 0x02000010 RID: 16
    public class PawnCAData : ThingComp
    {
        // Token: 0x04000009 RID: 9
        public int PawnCATick;

        // Token: 0x17000003 RID: 3
        // (get) Token: 0x06000041 RID: 65 RVA: 0x00003E39 File Offset: 0x00002039
        private Pawn pawn => (Pawn) parent;

        // Token: 0x06000040 RID: 64 RVA: 0x00003E1F File Offset: 0x0000201F
        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look(ref PawnCATick, "PawnCATick");
        }

        // Token: 0x06000042 RID: 66 RVA: 0x00003E48 File Offset: 0x00002048
        public static void PawnCADataTickSet(Pawn pawn, bool symptom = false)
        {
            ThingComp PComp = pawn?.TryGetComp<PawnCAData>();

            var PData = (PawnCAData) PComp;
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
                               (int) (Controller.Settings.CAFrequency / 2f * 2500f) + Rand.Range(-2500, 2500);
        }

        // Token: 0x02000014 RID: 20
        public class CompProperties_PawnCAData : CompProperties
        {
            // Token: 0x0600004A RID: 74 RVA: 0x00004356 File Offset: 0x00002556
            public CompProperties_PawnCAData()
            {
                compClass = typeof(PawnCAData);
            }
        }

        // Token: 0x02000015 RID: 21
        [StaticConstructorOnStartup]
        private static class ComAil_Setup
        {
            // Token: 0x0600004B RID: 75 RVA: 0x0000436E File Offset: 0x0000256E
            static ComAil_Setup()
            {
                ComAil_Setup_Pawns();
            }

            // Token: 0x0600004C RID: 76 RVA: 0x00004378 File Offset: 0x00002578
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

            // Token: 0x0600004D RID: 77 RVA: 0x000043B8 File Offset: 0x000025B8
            private static void ComAilSetup_Comp(Type compType, Func<ThingDef, bool> qualifier)
            {
                var list = DefDatabase<ThingDef>.AllDefsListForReading.Where(qualifier).ToList();
                list.RemoveDuplicates();
                foreach (var def in list)
                {
                    if (def.comps != null && !def.comps.Any(c => c.GetType() == compType))
                    {
                        def.comps.Add((CompProperties) Activator.CreateInstance(compType));
                    }
                }
            }
        }
    }
}