using System;
using HarmonyLib;
using RimWorld;
using Verse;

namespace ComAil;

[HarmonyPatch(typeof(Pawn_HealthTracker), nameof(Pawn_HealthTracker.AddHediff), typeof(HediffDef),
    typeof(BodyPartRecord),
    typeof(DamageInfo), typeof(DamageWorker.DamageResult))]
public class Pawn_HealthTracker_AddHediff
{
    [HarmonyPostfix]
    [HarmonyPriority(0)]
    public static void PostFix(ref Hediff __result, ref Pawn ___pawn, HediffDef def, BodyPartRecord part = null)
    {
        if (!Controller.Settings.DoAilments || GenDate.DaysPassed < Controller.Settings.CAStartDays ||
            __result == null || Controller.Settings.CAChance <= 0 || ___pawn == null || part == null ||
            !CommonAilments.PawnCanAcceptCa(___pawn) || def?.addedPartProps is not { solid: true })
        {
            return;
        }

        var offset = CommonAilmentsUtility.AugmentOffset(___pawn);
        if (!CommonAilments.CanAddCa(Math.Max(12, Controller.Settings.CAChance * 2), offset))
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
            CommonAilments.SendCaMsg(___pawn, hdef);
        }
    }
}