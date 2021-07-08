using RimWorld;
using Verse;

namespace ComAil
{
    // Token: 0x0200000C RID: 12
    public class HediffComp_CACure : HediffComp
    {
        // Token: 0x04000007 RID: 7
        private bool curing;

        // Token: 0x04000006 RID: 6
        private int ticksToCure;

        // Token: 0x17000001 RID: 1
        // (get) Token: 0x06000031 RID: 49 RVA: 0x000039D1 File Offset: 0x00001BD1
        public HediffCompProperties_CACure MSProps => (HediffCompProperties_CACure) props;

        // Token: 0x06000032 RID: 50 RVA: 0x000039E0 File Offset: 0x00001BE0
        public void SetTicksToCure()
        {
            var period = 2500;
            int basehours;
            if (MSProps.CureHoursMin > 0f && MSProps.CureHoursMax > 0f && MSProps.CureHoursMax >= MSProps.CureHoursMin)
            {
                basehours = (int) (Rand.Range(MSProps.CureHoursMin, MSProps.CureHoursMax) * period);
            }
            else
            {
                basehours = Rand.Range(2, 5) * period;
            }

            if (basehours < period)
            {
                basehours = period;
            }

            if (basehours > 36 * period)
            {
                basehours = 36 * period;
            }

            ticksToCure = basehours;
        }

        // Token: 0x06000033 RID: 51 RVA: 0x00003A74 File Offset: 0x00001C74
        public override void CompPostTick(ref float severityAdjustment)
        {
            if (curing && ticksToCure > 0)
            {
                ticksToCure--;
                return;
            }

            if (curing)
            {
                parent.Severity = 0f;
                Messages.Message(
                    "ComAil.CureMsg".Translate(Pawn.LabelShort.CapitalizeFirst(), Def.label.CapitalizeFirst()), Pawn,
                    MessageTypeDefOf.PositiveEvent);
                return;
            }

            if (!CommonAilmentsUtility.ImmuneTo(Pawn, Def, out var Immunities))
            {
                return;
            }

            var ImmunitiesAsCure = 0;
            foreach (var s in Immunities)
            {
                if (s != "MSCondom_High")
                {
                    ImmunitiesAsCure++;
                }
            }

            if (ImmunitiesAsCure <= 0)
            {
                return;
            }

            SetTicksToCure();
            curing = true;
        }

        // Token: 0x06000034 RID: 52 RVA: 0x00003B5F File Offset: 0x00001D5F
        public override void CompExposeData()
        {
            Scribe_Values.Look(ref ticksToCure, "ticksToCure");
            Scribe_Values.Look(ref curing, "curing");
        }

        // Token: 0x06000035 RID: 53 RVA: 0x00003B85 File Offset: 0x00001D85
        public override string CompDebugString()
        {
            if (curing)
            {
                return "ticksToCure: " + ticksToCure;
            }

            return "No active cure.";
        }
    }
}