﻿using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.AI;

namespace ComAil
{
	// Token: 0x02000004 RID: 4
	public class CommonAilments
	{
		// Token: 0x06000006 RID: 6 RVA: 0x0000223C File Offset: 0x0000043C
		public static void CommonAilmentsTick()
		{
			List<Pawn> PawnList = PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive.ToList<Pawn>();
			if (PawnList.Count > 0)
			{
				for (int i = 0; i < PawnList.Count; i++)
				{
					Pawn pawn = PawnList[i];
					ThingComp PComp = (pawn != null) ? pawn.TryGetComp<PawnCAData>() : null;
					if (PComp != null && (PComp as PawnCAData).PawnCATick < Find.TickManager.TicksGame)
					{
						if (CommonAilments.PawnCanAcceptCA(PawnList[i]))
						{
							int offset = 0;
							bool glut = false;
							bool rested = false;
							bool physical = false;
							bool planting = false;
							bool dirty = false;
							HediffDef hediffdef = null;
							float severity = 0f;
							BodyPartRecord part = null;
							CommonAilments.CurSituation(PawnList[i], out offset, out glut, out rested, out physical, out planting, out dirty);
							offset += CommonAilments.GetImmunityOS(PawnList[i]);
							int CABaseChance = Controller.Settings.CAChance;
							if (offset > 50)
							{
								offset = 50;
							}
							if (offset < -50)
							{
								offset = -50;
							}
							int rndtype = Rand.Range(1, 11);
							if (rndtype == 1)
							{
								if (glut)
								{
									if (CommonAilments.CanAddCA(CABaseChance, offset))
									{
										hediffdef = ComAilDefOf.HediffDefOf.CA_Indigestion;
										severity = 0.5f;
									}
								}
								else if (CommonAilments.CanAddCA(CABaseChance, offset))
								{
									CommonAilments.GetRndAilment(out hediffdef, out severity);
								}
							}
							else if (rndtype == 2)
							{
								if (rested)
								{
									if (CommonAilments.CanAddCA(CABaseChance, offset))
									{
										hediffdef = ComAilDefOf.HediffDefOf.CA_Restless;
										severity = Rand.Range(1f, 3f);
									}
								}
								else if (CommonAilments.CanAddCA(CABaseChance, offset))
								{
									CommonAilments.GetRndAilment(out hediffdef, out severity);
								}
							}
							else if (rndtype == 3)
							{
								if (physical)
								{
									if (CommonAilments.CanAddCA(CABaseChance, offset))
									{
										BodyPartDef partdef = null;
										if (CommonAilments.GetBPR(PawnList[i], out part, out partdef))
										{
											hediffdef = CommonAilments.GetBPHed(partdef);
											if (part != null)
											{
												severity = 1f;
											}
										}
									}
								}
								else if (CommonAilments.CanAddCA(CABaseChance, offset))
								{
									CommonAilments.GetRndAilment(out hediffdef, out severity);
								}
							}
							else if (rndtype == 4)
							{
								Pawn pawn2 = PawnList[i];
								if (((pawn2 != null) ? pawn2.Map : null) != null)
								{
									Pawn pawn3 = PawnList[i];
									bool flag;
									if (pawn3 == null)
									{
										flag = false;
									}
									else
									{
										IntVec3 position = pawn3.Position;
										Pawn pawn4 = PawnList[i];
										TerrainDef terrain = position.GetTerrain((pawn4 != null) ? pawn4.Map : null);
										float? num = (terrain != null) ? new float?(terrain.fertility) : null;
										float num2 = 0f;
										flag = (num.GetValueOrDefault() > num2 & num != null);
									}
									if (flag)
									{
										if (planting)
										{
											if (CommonAilments.CanAddCA(CABaseChance, offset))
											{
												hediffdef = ComAilDefOf.HediffDefOf.CA_Hayfever;
												severity = Rand.Range(0.5f, 2f);
												goto IL_351;
											}
											goto IL_351;
										}
										else
										{
											if (CommonAilments.CanAddCA(CABaseChance, offset))
											{
												CommonAilments.GetRndAilment(out hediffdef, out severity);
												goto IL_351;
											}
											goto IL_351;
										}
									}
								}
								if (CommonAilments.CanAddCA(CABaseChance, offset))
								{
									CommonAilments.GetRndAilment(out hediffdef, out severity);
								}
							}
							else if (rndtype == 5)
							{
								if (dirty)
								{
									if (CommonAilments.CanAddCA(CABaseChance, offset))
									{
										int rnd = Rand.Range(1, 100);
										if (rnd <= 50)
										{
											hediffdef = ComAilDefOf.HediffDefOf.CA_SkinRash;
											severity = Rand.Range(3f, 5f);
										}
										else if (rnd <= 75)
										{
											hediffdef = ComAilDefOf.HediffDefOf.CA_Earache;
											severity = Rand.Range(0.25f, 2f);
										}
										else
										{
											hediffdef = ComAilDefOf.HediffDefOf.CA_Conjunctivitis;
											severity = Rand.Range(2f, 5f);
										}
									}
								}
								else if (CommonAilments.CanAddCA(CABaseChance, offset))
								{
									CommonAilments.GetRndAilment(out hediffdef, out severity);
								}
							}
							else if (CommonAilments.CanAddCA(CABaseChance, offset))
							{
								CommonAilments.GetRndAilment(out hediffdef, out severity);
							}
							IL_351:
							bool imm;
							if (hediffdef != null && severity > 0f && !CommonAilments.TryApplyAilment(hediffdef, severity, PawnList[i], part, out imm) && !imm)
							{
								hediffdef = null;
								severity = 0f;
								part = null;
								if (CommonAilments.CanAddCA(CABaseChance * 2, offset))
								{
									CommonAilments.GetRndAilment(out hediffdef, out severity);
									if (hediffdef != null && severity > 0f)
									{
										bool imm2;
										CommonAilments.TryApplyAilment(hediffdef, severity, PawnList[i], part, out imm2);
									}
								}
							}
						}
						PawnCAData.PawnCADataTickSet(PawnList[i], false);
					}
				}
			}
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002624 File Offset: 0x00000824
		public static bool GetEitherProtected(Pawn partner, Pawn p)
		{
			HediffDef Condom = DefDatabase<HediffDef>.GetNamed("MSCondom_High", false);
			if (Condom != null)
			{
				if (partner != null && CommonAilmentsUtility.HasHediff(partner, Condom))
				{
					return true;
				}
				if (p != null && CommonAilmentsUtility.HasHediff(p, Condom))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002660 File Offset: 0x00000860
		public static bool TryApplyAilment(HediffDef h, float s, Pawn p, BodyPartRecord b, out bool i)
		{
			i = false;
			if (!CommonAilments.HasCA(p, h))
			{
				bool i2;
				if (CommonAilmentsUtility.HediffEffect(h, s, p, b, out i2))
				{
					CommonAilments.SendCAMsg(p, h);
					return true;
				}
				i = i2;
			}
			return false;
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002698 File Offset: 0x00000898
		public static void GetRndAilment(out HediffDef hediffdef, out float severity)
		{
			if (Rand.Range(1, 100) <= 65)
			{
				int chance = Rand.Range(1, 100);
				if (chance <= 25)
				{
					hediffdef = ComAilDefOf.HediffDefOf.CA_CommonCold;
					severity = Rand.Range(0.5f, 2f);
					return;
				}
				if (chance <= 30)
				{
					hediffdef = ComAilDefOf.HediffDefOf.CA_Migraine;
					severity = Rand.Range(0.25f, 0.5f);
					return;
				}
				if (chance <= 65)
				{
					hediffdef = ComAilDefOf.HediffDefOf.CA_Headache;
					severity = Rand.Range(0.5f, 1f);
					return;
				}
				hediffdef = ComAilDefOf.HediffDefOf.CA_Fatigue;
				severity = Rand.Range(0.5f, 3f);
				return;
			}
			else
			{
				switch (Rand.Range(1, 6))
				{
				case 1:
					hediffdef = ComAilDefOf.HediffDefOf.CA_CommonCold;
					severity = Rand.Range(0.5f, 2f);
					return;
				case 2:
					hediffdef = ComAilDefOf.HediffDefOf.CA_SoreThroat;
					severity = Rand.Range(0.25f, 0.5f);
					return;
				case 3:
					hediffdef = ComAilDefOf.HediffDefOf.CA_Headache;
					severity = Rand.Range(0.5f, 1f);
					return;
				case 4:
					hediffdef = ComAilDefOf.HediffDefOf.CA_Restless;
					severity = Rand.Range(1f, 3f);
					return;
				case 5:
					hediffdef = ComAilDefOf.HediffDefOf.CA_Sinusitis;
					severity = Rand.Range(1f, 2.5f);
					return;
				case 6:
					hediffdef = ComAilDefOf.HediffDefOf.CA_Fatigue;
					severity = Rand.Range(0.5f, 3f);
					return;
				default:
					hediffdef = ComAilDefOf.HediffDefOf.CA_Fatigue;
					severity = Rand.Range(0.5f, 1f);
					return;
				}
			}
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002808 File Offset: 0x00000A08
		public static HediffDef GetBPHed(BodyPartDef partdef)
		{
			HediffDef hediffdef = null;
			if (Rand.Range(1, 100) <= 50)
			{
				if (partdef == BodyPartDefOf.Hand)
				{
					hediffdef = ComAilDefOf.HediffDefOf.CA_Sprain_Hand;
				}
				else if (partdef == BodyPartDefOf.Arm)
				{
					hediffdef = ComAilDefOf.HediffDefOf.CA_Sprain_Arm;
				}
				else if (partdef == DefDatabase<BodyPartDef>.GetNamed("Foot", false))
				{
					hediffdef = ComAilDefOf.HediffDefOf.CA_Sprain_Foot;
				}
				else if (partdef == BodyPartDefOf.Leg)
				{
					hediffdef = ComAilDefOf.HediffDefOf.CA_Sprain_Leg;
				}
			}
			else if (partdef == BodyPartDefOf.Hand)
			{
				hediffdef = ComAilDefOf.HediffDefOf.CA_Knick_Hand;
			}
			else if (partdef == BodyPartDefOf.Arm)
			{
				hediffdef = ComAilDefOf.HediffDefOf.CA_Knick_Arm;
			}
			else if (partdef == DefDatabase<BodyPartDef>.GetNamed("Foot", false))
			{
				hediffdef = ComAilDefOf.HediffDefOf.CA_Knick_Foot;
			}
			else if (partdef == BodyPartDefOf.Leg)
			{
				hediffdef = ComAilDefOf.HediffDefOf.CA_Knick_Leg;
			}
			return hediffdef;
		}

		// Token: 0x0600000B RID: 11 RVA: 0x000028B0 File Offset: 0x00000AB0
		public static int GetImmunityOS(Pawn p)
		{
			int offset = 0;
			if (p.story.traits.HasTrait(DefDatabase<TraitDef>.GetNamed("Immunity", false)))
			{
				TraitSet traits = p.story.traits;
				Trait ImmunityTrait = (traits != null) ? traits.GetTrait(DefDatabase<TraitDef>.GetNamed("Immunity", false)) : null;
				if (ImmunityTrait != null)
				{
					offset -= ImmunityTrait.Degree * 5;
				}
			}
			if (offset < -5)
			{
				offset = -5;
			}
			if (offset > 5)
			{
				offset = 5;
			}
			return offset;
		}

		// Token: 0x0600000C RID: 12 RVA: 0x0000291C File Offset: 0x00000B1C
		public static bool PawnCanAcceptCA(Pawn p)
		{
			return p.RaceProps.Humanlike && !p.IsBurning() && !p.Dead && !p.RaceProps.Animal && !p.RaceProps.IsMechanoid && (!p.InContainerEnclosed || !CommonAilments.IsInCryptoCasket(p));
		}

		// Token: 0x0600000D RID: 13 RVA: 0x0000297C File Offset: 0x00000B7C
		public static bool IsInCryptoCasket(Pawn p)
		{
			if (((p != null) ? p.ParentHolder : null) != null)
			{
				Building holder = p.ParentHolder as Building;
				if (holder != null && (holder is Building_AncientCryptosleepCasket || holder is Building_CryptosleepCasket))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600000E RID: 14 RVA: 0x000029BC File Offset: 0x00000BBC
		public static bool GetBPR(Pawn p, out BodyPartRecord part, out BodyPartDef partdef)
		{
			part = null;
			partdef = null;
			int partsChance = Rand.Range(1, 100);
			if (partsChance <= 50)
			{
				if (partsChance <= 20)
				{
					partdef = BodyPartDefOf.Arm;
				}
				else
				{
					partdef = BodyPartDefOf.Hand;
				}
			}
			else if (partsChance >= 80)
			{
				partdef = BodyPartDefOf.Leg;
			}
			else
			{
				partdef = DefDatabase<BodyPartDef>.GetNamed("Foot", false);
			}
			BodyDef body = p.RaceProps.body;
			List<BodyPartRecord> parts = ((body != null) ? body.GetPartsWithDef(partdef) : null).ToList<BodyPartRecord>();
			if (parts != null)
			{
				part = parts.RandomElement<BodyPartRecord>();
			}
			if (part != null && partdef != null)
			{
				Pair<string, Color> condition = HealthUtility.GetPartConditionLabel(p, part);
				if (!(condition.First == "SeriouslyImpaired".Translate()) && !(condition.First == "ShatteredBodyPart".Translate()) && !(condition.First == "DestroyedBodyPart".Translate()) && !(condition.First == "MissingBodyPart".Translate()))
				{
					Pawn_HealthTracker health = p.health;
					return ((health != null) ? health.hediffSet : null) == null || !p.health.hediffSet.HasDirectlyAddedPartFor(part);
				}
			}
			return false;
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002AF8 File Offset: 0x00000CF8
		public static void SendCAMsg(Pawn p, HediffDef h)
		{
			if (p.IsColonist && Controller.Settings.DoMsgs)
			{
				string text = p.Label.CapitalizeFirst() + "ComAil.Confirmation".Translate() + h.label.CapitalizeFirst();
				MessageTypeDef MTD = MessageTypeDefOf.NegativeHealthEvent;
				if (!Controller.Settings.NegHealthSound)
				{
					MTD = MessageTypeDefOf.NeutralEvent;
				}
				Messages.Message(text, p, MTD, true);
			}
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00002B70 File Offset: 0x00000D70
		public static bool HasCA(Pawn p, HediffDef h)
		{
			Pawn_HealthTracker health = p.health;
			bool flag;
			if (health == null)
			{
				flag = (null != null);
			}
			else
			{
				HediffSet hediffSet = health.hediffSet;
				flag = (((hediffSet != null) ? hediffSet.GetFirstHediffOfDef(h, false) : null) != null);
			}
			if (flag)
			{
				return true;
			}
			if (h == ComAilDefOf.HediffDefOf.CA_Headache)
			{
				Pawn_HealthTracker health2 = p.health;
				bool flag2;
				if (health2 == null)
				{
					flag2 = (null != null);
				}
				else
				{
					HediffSet hediffSet2 = health2.hediffSet;
					flag2 = (((hediffSet2 != null) ? hediffSet2.GetFirstHediffOfDef(ComAilDefOf.HediffDefOf.CA_Migraine, false) : null) != null);
				}
				if (flag2)
				{
					return true;
				}
			}
			if (h == ComAilDefOf.HediffDefOf.CA_Migraine)
			{
				Pawn_HealthTracker health3 = p.health;
				bool flag3;
				if (health3 == null)
				{
					flag3 = (null != null);
				}
				else
				{
					HediffSet hediffSet3 = health3.hediffSet;
					flag3 = (((hediffSet3 != null) ? hediffSet3.GetFirstHediffOfDef(ComAilDefOf.HediffDefOf.CA_Headache, false) : null) != null);
				}
				if (flag3)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002C02 File Offset: 0x00000E02
		public static bool CanAddCA(int chance, int offset)
		{
			return Rand.Range(1, 100 - offset) <= chance;
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002C14 File Offset: 0x00000E14
		public static bool CanAddCASTI(int chance, int offset, bool infected)
		{
			int bias = 2000;
			if (infected)
			{
				bias = 200;
			}
			return Rand.Range(1, bias - offset) <= chance;
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002C40 File Offset: 0x00000E40
		internal static bool isCAValidBodyRace(Pawn pawn)
		{
			bool valid = false;
			if (CommonAilments.CAValidRaceBodies().Contains(pawn.kindDef.race.defName))
			{
				valid = true;
			}
			return valid;
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00002C6E File Offset: 0x00000E6E
		internal static List<string> CAValidRaceBodies()
		{
			List<string> list = new List<string>();
			list.AddDistinct("Human");
			return list;
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00002C80 File Offset: 0x00000E80
		internal static List<CommonAilments.SickListData> CASickList()
		{
			List<CommonAilments.SickListData> list = new List<CommonAilments.SickListData>();
			CommonAilments.SickListData SLD = new CommonAilments.SickListData(HediffDefOf.WoundInfection, 1);
			list.Add(SLD);
			SLD = new CommonAilments.SickListData(HediffDefOf.ResurrectionSickness, 1);
			list.Add(SLD);
			SLD = new CommonAilments.SickListData(HediffDefOf.Plague, 2);
			list.Add(SLD);
			SLD = new CommonAilments.SickListData(HediffDefOf.Malnutrition, 1);
			list.Add(SLD);
			SLD = new CommonAilments.SickListData(HediffDefOf.Malaria, 2);
			list.Add(SLD);
			SLD = new CommonAilments.SickListData(HediffDefOf.FoodPoisoning, 1);
			list.Add(SLD);
			SLD = new CommonAilments.SickListData(HediffDefOf.Flu, 1);
			list.Add(SLD);
			SLD = new CommonAilments.SickListData(HediffDefOf.CryptosleepSickness, 2);
			list.Add(SLD);
			SLD = new CommonAilments.SickListData(HediffDefOf.Carcinoma, 2);
			list.Add(SLD);
			SLD = new CommonAilments.SickListData(ComAilDefOf.HediffDefOf.CA_CommonCold, 1);
			list.Add(SLD);
			SLD = new CommonAilments.SickListData(ComAilDefOf.HediffDefOf.CA_Conjunctivitis, 1);
			list.Add(SLD);
			SLD = new CommonAilments.SickListData(ComAilDefOf.HediffDefOf.CA_Earache, 1);
			list.Add(SLD);
			SLD = new CommonAilments.SickListData(ComAilDefOf.HediffDefOf.CA_Hayfever, 1);
			list.Add(SLD);
			SLD = new CommonAilments.SickListData(ComAilDefOf.HediffDefOf.CA_SkinRash, 1);
			list.Add(SLD);
			SLD = new CommonAilments.SickListData(ComAilDefOf.HediffDefOf.CA_SoreThroat, 1);
			list.Add(SLD);
			SLD = new CommonAilments.SickListData(ComAilDefOf.HediffDefOf.CA_Minor_STD, 1);
			list.Add(SLD);
			List<string> ModList = new List<string>();
			ModList.AddDistinct("DBHDehydration");
			ModList.AddDistinct("Cholera");
			ModList.AddDistinct("Dysentery");
			ModList.AddDistinct("Diarrhea");
			ModList.AddDistinct("BadHygiene");
			if (ModList.Count > 0)
			{
				for (int i = 0; i < ModList.Count; i++)
				{
					int ModFactor = 1;
					HediffDef ModHediffDef = DefDatabase<HediffDef>.GetNamed(ModList[i], false);
					if (ModHediffDef != null)
					{
						if (ModList[i] == "BadHygiene")
						{
							ModFactor = 2;
						}
						SLD = new CommonAilments.SickListData(ModHediffDef, ModFactor);
						list.Add(SLD);
					}
				}
			}
			return list;
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002E6C File Offset: 0x0000106C
		public static void CurSituation(Pawn pawn, out int offset, out bool glut, out bool rested, out bool physical, out bool planting, out bool dirty)
		{
			offset = 0;
			HediffSet hediffSet;
			if (pawn == null)
			{
				hediffSet = null;
			}
			else
			{
				Pawn_HealthTracker health = pawn.health;
				hediffSet = ((health != null) ? health.hediffSet : null);
			}
			HediffSet phedset = hediffSet;
			if (phedset != null)
			{
				List<CommonAilments.SickListData> chkList = new List<CommonAilments.SickListData>();
				if (CommonAilments.CASickListCached != null && CommonAilments.CASickListCached.Count > 0)
				{
					chkList = CommonAilments.CASickListCached;
				}
				else
				{
					chkList = CommonAilments.CASickList();
					CommonAilments.CASickListCached = CommonAilments.CASickList();
				}
				if (chkList.Count > 0)
				{
					for (int i = 0; i < chkList.Count; i++)
					{
						HediffDef chkhediff = chkList[i].GetSLDHD(chkList[i]);
						int chkfactor = chkList[i].GetSLDF(chkList[i]);
						if (((phedset != null) ? phedset.GetFirstHediffOfDef(chkhediff, false) : null) != null)
						{
							offset += chkfactor;
						}
					}
				}
			}
			int bioAgeOS = (pawn.ageTracker.AgeBiologicalYears - 40) / 10;
			if (bioAgeOS > 3)
			{
				bioAgeOS = 3;
			}
			if (bioAgeOS < -3)
			{
				bioAgeOS = -3;
			}
			offset += bioAgeOS;
			bool flag;
			if (pawn == null)
			{
				flag = (null != null);
			}
			else
			{
				Pawn_HealthTracker health2 = pawn.health;
				if (health2 == null)
				{
					flag = (null != null);
				}
				else
				{
					HediffSet hediffSet2 = health2.hediffSet;
					flag = (((hediffSet2 != null) ? hediffSet2.GetFirstHediffOfDef(HediffDefOf.Frail, false) : null) != null);
				}
			}
			if (flag)
			{
				offset += 2;
			}
			BodyTypeDef bodyTypeDef;
			if (pawn == null)
			{
				bodyTypeDef = null;
			}
			else
			{
				Pawn_StoryTracker story = pawn.story;
				bodyTypeDef = ((story != null) ? story.bodyType : null);
			}
			if (bodyTypeDef == BodyTypeDefOf.Fat)
			{
				offset++;
			}
			if (((pawn != null) ? pawn.Map : null) != null)
			{
				float outtemp = pawn.Map.mapTemperature.OutdoorTemp;
				if (outtemp < 21f)
				{
					offset++;
					if (outtemp < 10f)
					{
						offset++;
						if (outtemp < 0f)
						{
							offset++;
						}
					}
				}
				else if (outtemp > 29f)
				{
					offset--;
					if (outtemp > 38f)
					{
						offset--;
						if (outtemp > 47f)
						{
							offset--;
						}
					}
				}
			}
			glut = false;
			Pawn_NeedsTracker needs = pawn.needs;
			if (((needs != null) ? needs.food : null) != null)
			{
				if (pawn.needs.food.Starving)
				{
					offset += 2;
				}
				if (pawn.needs.food.CurLevelPercentage > 0.75f)
				{
					glut = true;
					offset -= 2;
				}
			}
			rested = false;
			Pawn_NeedsTracker needs2 = pawn.needs;
			if (((needs2 != null) ? needs2.rest : null) != null)
			{
				if (pawn.needs.rest.CurLevelPercentage < 0.2f)
				{
					offset += 3;
				}
				if (pawn.needs.rest.CurLevelPercentage > 0.75f)
				{
					rested = true;
					offset -= 3;
				}
			}
			Pawn_NeedsTracker needs3 = pawn.needs;
			if (((needs3 != null) ? needs3.outdoors : null) != null)
			{
				if (pawn.needs.outdoors.CurLevelPercentage < 0.2f)
				{
					offset += 2;
				}
				if (pawn.needs.outdoors.CurLevelPercentage > 0.75f)
				{
					offset -= 2;
				}
			}
			dirty = false;
			if (pawn.Map != null && !pawn.IsInAnyStorage())
			{
				if (pawn.Position.GetRoom(pawn.Map, RegionType.Set_Passable).GetStat(RoomStatDefOf.Cleanliness) < 0f)
				{
					dirty = true;
				}
				int RoomOffset = (int)((0f - pawn.Position.GetRoom(pawn.Map, RegionType.Set_Passable).GetStat(RoomStatDefOf.Cleanliness)) * 10f);
				if (RoomOffset > 5)
				{
					RoomOffset = 5;
				}
				if (RoomOffset < -5)
				{
					RoomOffset = -5;
				}
				offset += RoomOffset;
			}
			physical = false;
			planting = false;
			if (((pawn != null) ? pawn.CurJob : null) != null)
			{
				physical = CommonAilments.CurJobPhysical(pawn.CurJob);
				planting = CommonAilments.CurJobPlants(pawn.CurJob);
			}
		}

		// Token: 0x06000017 RID: 23 RVA: 0x000031D2 File Offset: 0x000013D2
		internal static List<JobDef> CurJobPlantsListing()
		{
			List<JobDef> list = new List<JobDef>();
			list.AddDistinct(JobDefOf.CutPlant);
			list.AddDistinct(JobDefOf.CutPlantDesignated);
			list.AddDistinct(JobDefOf.Harvest);
			list.AddDistinct(JobDefOf.HarvestDesignated);
			list.AddDistinct(JobDefOf.Sow);
			return list;
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00003210 File Offset: 0x00001410
		internal static bool CurJobPlants(Job job)
		{
			bool workplants = false;
			JobDef jobdef = job.def;
			List<JobDef> chkList = CommonAilments.CurJobPlantsListing();
			if (chkList != null && chkList.Contains(jobdef))
			{
				workplants = true;
			}
			return workplants;
		}

		// Token: 0x06000019 RID: 25 RVA: 0x0000323C File Offset: 0x0000143C
		internal static List<JobDef> CurJobPhysicalListing()
		{
			List<JobDef> list = CommonAilments.CurJobPlantsListing();
			list.AddDistinct(JobDefOf.BuildRoof);
			list.AddDistinct(JobDefOf.Deconstruct);
			list.AddDistinct(JobDefOf.DoBill);
			list.AddDistinct(JobDefOf.FixBrokenDownBuilding);
			list.AddDistinct(JobDefOf.RemoveFloor);
			list.AddDistinct(JobDefOf.RemoveRoof);
			list.AddDistinct(JobDefOf.Repair);
			list.AddDistinct(JobDefOf.Slaughter);
			list.AddDistinct(JobDefOf.SmoothFloor);
			list.AddDistinct(JobDefOf.SmoothWall);
			return list;
		}

		// Token: 0x0600001A RID: 26 RVA: 0x000032BC File Offset: 0x000014BC
		internal static bool CurJobPhysical(Job job)
		{
			bool physical = false;
			JobDef jobdef = job.def;
			List<JobDef> chkList = CommonAilments.CurJobPhysicalListing();
			if (chkList != null && chkList.Contains(jobdef))
			{
				physical = true;
			}
			return physical;
		}

		// Token: 0x04000001 RID: 1
		private static List<CommonAilments.SickListData> CASickListCached;

		// Token: 0x02000012 RID: 18
		internal struct SickListData
		{
			// Token: 0x06000047 RID: 71 RVA: 0x00004336 File Offset: 0x00002536
			internal SickListData(HediffDef hd, int i)
			{
				this.SLDdef = hd;
				this.SLDfactor = i;
			}

			// Token: 0x06000048 RID: 72 RVA: 0x00004346 File Offset: 0x00002546
			internal HediffDef GetSLDHD(CommonAilments.SickListData s)
			{
				return s.SLDdef;
			}

			// Token: 0x06000049 RID: 73 RVA: 0x0000434E File Offset: 0x0000254E
			internal int GetSLDF(CommonAilments.SickListData s)
			{
				return s.SLDfactor;
			}

			// Token: 0x04000015 RID: 21
			private HediffDef SLDdef;

			// Token: 0x04000016 RID: 22
			private int SLDfactor;
		}
	}
}
