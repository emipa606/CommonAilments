using System;
using HarmonyLib;
using RimWorld;
using Verse;

namespace ComAil
{
    // Token: 0x02000002 RID: 2
    [HarmonyPatch(typeof(Pawn_HealthTracker), "AddHediff", typeof(HediffDef), typeof(BodyPartRecord),
        typeof(DamageInfo), typeof(DamageWorker.DamageResult))]
    public class AddHediff_PostPatch
    {
        // Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
        [HarmonyPostfix]
        [HarmonyPriority(0)]
        public static void PostFix(ref Pawn_HealthTracker __instance, ref Hediff __result, ref Pawn ___pawn,
            HediffDef def, BodyPartRecord part = null, DamageInfo? dinfo = null,
            DamageWorker.DamageResult result = null)
        {
            if (!Controller.Settings.DoAilments || GenDate.DaysPassed < Controller.Settings.CAStartDays ||
                __result == null || Controller.Settings.CAChance <= 0 || ___pawn == null || part == null ||
                !CommonAilments.PawnCanAcceptCA(___pawn) || def?.addedPartProps == null ||
                !def.addedPartProps.solid)
            {
                return;
            }

            var offset = CommonAilmentsUtility.AugmentOffset(___pawn);
            if (!CommonAilments.CanAddCA(Math.Max(12, Controller.Settings.CAChance * 2), offset))
            {
                return;
            }

            var hdef = DefDatabase<HediffDef>.GetNamed("CA_PhantomPain", false);
            if (hdef == null)
            {
                return;
            }

            var sev = CommonAilmentsUtility.RandNum(0.5f, 3f);
            if (CommonAilmentsUtility.HediffEffect(hdef, sev, ___pawn, part, out _))
            {
                CommonAilments.SendCAMsg(___pawn, hdef);
            }
        }
    }
}