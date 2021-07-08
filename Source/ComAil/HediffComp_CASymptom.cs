using System;
using RimWorld;
using Verse;

namespace ComAil
{
    // Token: 0x0200000D RID: 13
    public class HediffComp_CASymptom : HediffComp
    {
        // Token: 0x17000002 RID: 2
        // (get) Token: 0x06000037 RID: 55 RVA: 0x00003BB2 File Offset: 0x00001DB2
        public HediffCompProperties_CASymptom CAProps => (HediffCompProperties_CASymptom) props;

        // Token: 0x06000038 RID: 56 RVA: 0x00003BC0 File Offset: 0x00001DC0
        public override void CompPostTick(ref float severityAdjustment)
        {
            if (!Controller.Settings.DoSymptoms || !CommonAilments.PawnCanAcceptCA(Pawn) ||
                !Controller.Settings.DoAilments || GenDate.DaysPassed < Controller.Settings.CAStartDays ||
                !Pawn.IsHashIntervalTick(2501))
            {
                return;
            }

            var chance = (int) (CAProps.symptomChance + Controller.Settings.CAChance);
            CommonAilments.CurSituation(Pawn, out var offset, out _, out _, out _, out _,
                out _);
            offset += Math.Max(-20, Math.Min(10, CommonAilments.GetImmunityOS(Pawn)));
            if (CommonAilments.CanAddCA(chance, offset))
            {
                CommonAilmentsUtility.ApplySymptom(Pawn);
            }
        }
    }
}