using System;
using RimWorld;
using Verse;

namespace ComAil;

public class HediffComp_CASymptom : HediffComp
{
    public HediffCompProperties_CASymptom CAProps => (HediffCompProperties_CASymptom)props;

    public override void CompPostTick(ref float severityAdjustment)
    {
        if (!Controller.Settings.DoSymptoms || !CommonAilments.PawnCanAcceptCA(Pawn) ||
            !Controller.Settings.DoAilments || GenDate.DaysPassed < Controller.Settings.CAStartDays ||
            !Pawn.IsHashIntervalTick(2501))
        {
            return;
        }

        var chance = (int)(CAProps.symptomChance + Controller.Settings.CAChance);
        CommonAilments.CurSituation(Pawn, out var offset, out _, out _, out _, out _,
            out _);
        offset += Math.Max(-20, Math.Min(10, CommonAilments.GetImmunityOS(Pawn)));
        if (CommonAilments.CanAddCA(chance, offset))
        {
            CommonAilmentsUtility.ApplySymptom(Pawn);
        }
    }
}