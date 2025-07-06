using HarmonyLib;
using RimWorld;
using Verse;
using Verse.AI;

namespace ComAil;

[HarmonyPatch(typeof(JobDriver_Lovin), "GenerateRandomMinTicksToNextLovin")]
public class JobDriver_Lovin_GenerateRandomMinTicksToNextLovin
{
    [HarmonyPostfix]
    public static void PostFix(ref JobDriver_Lovin __instance, ref TargetIndex ___PartnerInd, Pawn pawn)
    {
        if (!Controller.Settings.DoAilments || !Controller.Settings.DoSTI ||
            GenDate.DaysPassed < Controller.Settings.CAStartDays)
        {
            return;
        }

        HediffDef hediffdef = null;
        var severity = 0f;
        CommonAilments.CurSituation(pawn, out var offset, out _, out _, out _, out _,
            out _);
        offset += CommonAilments.GetImmunityOS(pawn);
        var CASTIBaseChance = Controller.Settings.CASTIChance;
        if (offset > 25)
        {
            offset = 25;
        }

        if (offset < -25)
        {
            offset = -25;
        }

        Hediff std = null;
        var sexwith = (Pawn)(Thing)__instance.job.GetTarget(___PartnerInd);
        if (sexwith == null)
        {
            return;
        }

        if (!CommonAilments.GetEitherProtected(sexwith, pawn))
        {
            var health = sexwith.health;
            var hediffSet = health?.hediffSet;

            var infected = false;
            if (hediffSet != null)
            {
                offset += Controller.Settings.CASTIOffset;
                std = hediffSet.GetFirstHediffOfDef(ComAilDefOf.HediffDefOf.CA_Minor_STD);
                if (std != null)
                {
                    infected = true;
                }
            }

            if (CommonAilments.CanAddCasti(CASTIBaseChance, offset, infected))
            {
                if (std != null)
                {
                    hediffdef = std.def;
                    severity = std.Severity + GetARnd(-0.5f, 0.5f);
                }
                else
                {
                    hediffdef = ComAilDefOf.HediffDefOf.CA_Minor_STD;
                    severity = GetARnd(1f, 5f);
                }
            }
        }

        if (hediffdef == null || !(severity > 0f))
        {
            return;
        }

        CommonAilments.TryApplyAilment(hediffdef, severity, pawn, null, out _);
        if (std != null || !(GetARnd(1f, 100f) < 50f))
        {
            return;
        }

        CommonAilments.TryApplyAilment(hediffdef, severity, sexwith, null, out _);
    }

    private static float GetARnd(float first, float second)
    {
        return Rand.Range(first, second);
    }
}