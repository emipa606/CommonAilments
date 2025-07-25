using System;
using System.Collections.Generic;
using RimWorld;
using Verse;

namespace ComAil;

internal class CommonAilmentsUtility
{
    public static List<string> GetComAilDefNames()
    {
        var list = new List<string>();
        list.AddDistinct("CA_CommonCold");
        list.AddDistinct("CA_Conjunctivitis");
        list.AddDistinct("CA_Earache");
        list.AddDistinct("CA_Headache");
        list.AddDistinct("CA_Indigestion");
        list.AddDistinct("CA_Migraine");
        list.AddDistinct("CA_Restless");
        list.AddDistinct("CA_SoreThroat");
        list.AddDistinct("CA_Hayfever");
        list.AddDistinct("CA_SkinRash");
        list.AddDistinct("CA_Sinusitis");
        list.AddDistinct("CA_Minor_STD");
        list.AddDistinct("CA_Fatigue");
        return list;
    }

    internal static bool HediffEffect(HediffDef hediffdef, float SeverityToApply, Pawn pawn, BodyPartRecord part,
        out bool immune)
    {
        immune = false;
        if (pawn.RaceProps.IsMechanoid || hediffdef == null)
        {
            return false;
        }

        if (!ImmuneTo(pawn, hediffdef, out _))
        {
            if (pawn.health.WouldDieAfterAddingHediff(hediffdef, part, SeverityToApply))
            {
                return false;
            }

            var health = pawn.health;
            Hediff hediff;
            if (health == null)
            {
                hediff = null;
            }
            else
            {
                var hediffSet = health.hediffSet;
                hediff = hediffSet?.GetFirstHediffOfDef(hediffdef);
            }

            var hashediff = hediff;
            if (hashediff != null)
            {
                hashediff.Severity += SeverityToApply;
                return true;
            }

            var addhediff = HediffMaker.MakeHediff(hediffdef, pawn, part);
            addhediff.Severity = SeverityToApply;
            pawn.health.AddHediff(addhediff, part);
            return true;
        }

        immune = true;

        return false;
    }

    internal static bool ImmuneTo(Pawn pawn, HediffDef def, out List<string> Immunities)
    {
        Immunities = [];
        var immune = false;
        var hediffs = pawn.health.hediffSet.hediffs;
        foreach (var hediff in hediffs)
        {
            var curStage = hediff.CurStage;
            if (curStage?.makeImmuneTo == null)
            {
                continue;
            }

            foreach (var hediffDef in curStage.makeImmuneTo)
            {
                if (hediffDef != def)
                {
                    continue;
                }

                Immunities.AddDistinct(hediff.def.defName);
                immune = true;
            }
        }

        return immune;
    }

    internal static bool HasHediff(Pawn p, HediffDef h)
    {
        bool result;
        if (p == null)
        {
            result = false;
        }
        else
        {
            var health = p.health;
            result = health?.hediffSet.GetFirstHediffOfDef(h) != null;
        }

        return result;
    }

    internal static int AugmentOffset(Pawn patient)
    {
        var spread = 10;
        var offset = CommonAilments.GetImmunityOS(patient);
        if (patient.story.traits.HasTrait(DefDatabase<TraitDef>.GetNamed("Transhumanist", false)))
        {
            offset -= 5;
        }

        if (patient.story.traits.HasTrait(DefDatabase<TraitDef>.GetNamed("BodyPurist", false)))
        {
            offset += 5;
        }

        if (!patient.story.traits.HasTrait(DefDatabase<TraitDef>.GetNamed("NaturalMood", false)))
        {
            return Math.Max(0 - spread, Math.Min(spread, offset));
        }

        var traits = patient.story.traits;
        var NMTrait = traits?.GetTrait(DefDatabase<TraitDef>.GetNamed("NaturalMood", false));
        if (NMTrait != null)
        {
            offset -= NMTrait.Degree * 2;
        }

        return Math.Max(0 - spread, Math.Min(spread, offset));
    }

    internal static float RandNum(float min, float max)
    {
        return Rand.Range(min, max);
    }

    public static List<string> Symptomatic()
    {
        var list = new List<string>();
        list.AddDistinct("Malaria");
        list.AddDistinct("Flu");
        list.AddDistinct("Plague");
        list.AddDistinct("SleepingSickness");
        list.AddDistinct("WoundInfection");
        list.AddDistinct("Carcinoma");
        list.AddDistinct("Cirrhosis");
        list.AddDistinct("ResurrectionPsychosis");
        list.AddDistinct("Malnutrition");
        list.AddDistinct("FoodPoisoning");
        list.AddDistinct("ToxicBuildup");
        list.AddDistinct("ResurrectionSickness");
        if (ModLister.HasActiveModWithName("Diseases Overhauled"))
        {
            list.AddDistinct("HepatitisK");
            list.AddDistinct("Tuberculosis");
            list.AddDistinct("KindredDickVirus");
            list.AddDistinct("Sepsis");
            list.AddDistinct("VoightBernsteinDisease");
            list.AddDistinct("HansenKampffDisease");
            list.AddDistinct("Necrosis");
            list.AddDistinct("Psoriasis");
            list.AddDistinct("NewReschianFever");
            list.AddDistinct("BloodCancer");
        }

        if (!ModLister.HasActiveModWithName("Dubs Bad Hygiene"))
        {
            return list;
        }

        list.AddDistinct("Cholera");
        list.AddDistinct("Dysentery");
        list.AddDistinct("BadHygiene");

        return list;
    }

    private static List<string> Symptoms()
    {
        var list = new List<string>();
        list.AddDistinct("CA_CommonCold");
        list.AddDistinct("CA_SoreThroat");
        list.AddDistinct("CA_Headache");
        list.AddDistinct("CA_Restless");
        list.AddDistinct("CA_Fatigue");
        return list;
    }

    public static void ApplySymptom(Pawn p)
    {
        var def = DefDatabase<HediffDef>.GetNamed(Symptoms().RandomElement(), false);
        if (def == null)
        {
            return;
        }

        var maxSev = Math.Max(1f, def.maxSeverity);
        var sev = RandNum(0.33f, maxSev);
        if (CommonAilments.HasCa(p, def) || !HediffEffect(def, sev, p, null, out _))
        {
            return;
        }

        CommonAilments.SendCaMsg(p, def);
        PawnCAData.PawnCADataTickSet(p, true);
    }
}