using HarmonyLib;
using RimWorld;
using Verse;
using Verse.AI;

namespace ComAil
{
    // Token: 0x02000008 RID: 8
    [HarmonyPatch(typeof(JobDriver_Lovin), "GenerateRandomMinTicksToNextLovin")]
    public class GenerateRandomMinTicksToNextLovin_PostPatch
    {
        // Token: 0x0600002B RID: 43 RVA: 0x000037F4 File Offset: 0x000019F4
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
            var sexwith = (Pawn) (Thing) __instance.job.GetTarget(___PartnerInd);
            if (sexwith == null)
            {
                return;
            }

            if (!CommonAilments.GetEitherProtected(sexwith, pawn))
            {
                var health = sexwith.health;
                var hediffSet = health?.hediffSet;

                var swset = hediffSet;
                var infected = false;
                if (swset != null)
                {
                    offset += Controller.Settings.CASTIOffset;
                    std = swset.GetFirstHediffOfDef(ComAilDefOf.HediffDefOf.CA_Minor_STD);
                    if (std != null)
                    {
                        infected = true;
                    }
                }

                if (CommonAilments.CanAddCASTI(CASTIBaseChance, offset, infected))
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

        // Token: 0x0600002C RID: 44 RVA: 0x0000396F File Offset: 0x00001B6F
        public static float GetARnd(float first, float second)
        {
            return Rand.Range(first, second);
        }
    }
}