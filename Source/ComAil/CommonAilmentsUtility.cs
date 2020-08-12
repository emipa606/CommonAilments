using System;
using System.Collections.Generic;
using RimWorld;
using Verse;

namespace ComAil
{
	// Token: 0x02000006 RID: 6
	internal class CommonAilmentsUtility
	{
		// Token: 0x0600001E RID: 30 RVA: 0x00003308 File Offset: 0x00001508
		public static List<string> GetComAilDefNames()
		{
			List<string> list = new List<string>();
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

		// Token: 0x0600001F RID: 31 RVA: 0x000033AC File Offset: 0x000015AC
		internal static bool HediffEffect(HediffDef hediffdef, float SeverityToApply, Pawn pawn, BodyPartRecord part, out bool immune)
		{
			immune = false;
			if (!pawn.RaceProps.IsMechanoid && hediffdef != null)
			{
				List<string> Immunities = new List<string>();
				if (!CommonAilmentsUtility.ImmuneTo(pawn, hediffdef, out Immunities))
				{
					if (!pawn.health.WouldDieAfterAddingHediff(hediffdef, part, SeverityToApply))
					{
						Pawn_HealthTracker health = pawn.health;
						Hediff hediff;
						if (health == null)
						{
							hediff = null;
						}
						else
						{
							HediffSet hediffSet = health.hediffSet;
							hediff = ((hediffSet != null) ? hediffSet.GetFirstHediffOfDef(hediffdef, false) : null);
						}
						Hediff hashediff = hediff;
						if (hashediff != null)
						{
							hashediff.Severity += SeverityToApply;
							return true;
						}
						Hediff addhediff = HediffMaker.MakeHediff(hediffdef, pawn, part);
						addhediff.Severity = SeverityToApply;
						pawn.health.AddHediff(addhediff, part, null, null);
						return true;
					}
				}
				else
				{
					immune = true;
				}
			}
			return false;
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00003458 File Offset: 0x00001658
		internal static bool ImmuneTo(Pawn pawn, HediffDef def, out List<string> Immunities)
		{
			Immunities = new List<string>();
			bool immune = false;
			List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				HediffStage curStage = hediffs[i].CurStage;
				if (curStage != null && curStage.makeImmuneTo != null)
				{
					for (int j = 0; j < curStage.makeImmuneTo.Count; j++)
					{
						if (curStage.makeImmuneTo[j] == def)
						{
							Immunities.AddDistinct(hediffs[i].def.defName);
							immune = true;
						}
					}
				}
			}
			return immune;
		}

		// Token: 0x06000021 RID: 33 RVA: 0x000034ED File Offset: 0x000016ED
		internal static bool HasHediff(Pawn p, HediffDef h)
		{
			bool result;
			if (p == null)
			{
				result = (null != null);
			}
			else
			{
				Pawn_HealthTracker health = p.health;
				result = (((health != null) ? health.hediffSet.GetFirstHediffOfDef(h, false) : null) != null);
			}
			return result;
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00003514 File Offset: 0x00001714
		internal static int AugmentOffset(Pawn patient)
		{
			int spread = 10;
			int offset = CommonAilments.GetImmunityOS(patient);
			if (patient.story.traits.HasTrait(DefDatabase<TraitDef>.GetNamed("Transhumanist", false)))
			{
				offset -= 5;
			}
			if (patient.story.traits.HasTrait(DefDatabase<TraitDef>.GetNamed("BodyPurist", false)))
			{
				offset += 5;
			}
			if (patient.story.traits.HasTrait(DefDatabase<TraitDef>.GetNamed("NaturalMood", false)))
			{
				TraitSet traits = patient.story.traits;
				Trait NMTrait = (traits != null) ? traits.GetTrait(DefDatabase<TraitDef>.GetNamed("NaturalMood", false)) : null;
				if (NMTrait != null)
				{
					offset -= NMTrait.Degree * 2;
				}
			}
			return Math.Max(0 - spread, Math.Min(spread, offset));
		}

		// Token: 0x06000023 RID: 35 RVA: 0x000035CA File Offset: 0x000017CA
		internal static float RandNum(float min, float max)
		{
			return Rand.Range(min, max);
		}

		// Token: 0x06000024 RID: 36 RVA: 0x000035D4 File Offset: 0x000017D4
		public static List<string> Symptomatic()
		{
			List<string> list = new List<string>();
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
			if (ModLister.HasActiveModWithName("Dubs Bad Hygiene"))
			{
				list.AddDistinct("Cholera");
				list.AddDistinct("Dysentery");
				list.AddDistinct("BadHygiene");
			}
			return list;
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00003713 File Offset: 0x00001913
		public static List<string> Symptoms()
		{
			List<string> list = new List<string>();
			list.AddDistinct("CA_CommonCold");
			list.AddDistinct("CA_SoreThroat");
			list.AddDistinct("CA_Headache");
			list.AddDistinct("CA_Restless");
			list.AddDistinct("CA_Fatigue");
			return list;
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00003754 File Offset: 0x00001954
		public static void ApplySymptom(Pawn p)
		{
			HediffDef def = DefDatabase<HediffDef>.GetNamed(CommonAilmentsUtility.Symptoms().RandomElement<string>(), false);
			if (def != null)
			{
				float maxSev = Math.Max(1f, def.maxSeverity);
				float sev = CommonAilmentsUtility.RandNum(0.33f, maxSev);
				bool immune;
				if (!CommonAilments.HasCA(p, def) && CommonAilmentsUtility.HediffEffect(def, sev, p, null, out immune))
				{
					CommonAilments.SendCAMsg(p, def);
					PawnCAData.PawnCADataTickSet(p, true);
				}
			}
		}
	}
}
