using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;
using Verse.AI;

namespace ComAil
{
    // Token: 0x02000004 RID: 4
    public class CommonAilments
    {
        // Token: 0x04000001 RID: 1
        private static List<SickListData> CASickListCached;

        // Token: 0x06000006 RID: 6 RVA: 0x0000223C File Offset: 0x0000043C
        public static void CommonAilmentsTick()
        {
            var PawnList = PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive.ToList();
            if (PawnList.Count <= 0)
            {
                return;
            }

            foreach (var pawn5 in PawnList)
            {
                var pawn = pawn5;
                ThingComp PComp = pawn?.TryGetComp<PawnCAData>();
                if (PComp == null || ((PawnCAData) PComp).PawnCATick >= Find.TickManager.TicksGame)
                {
                    continue;
                }

                if (PawnCanAcceptCA(pawn5))
                {
                    HediffDef hediffdef = null;
                    var severity = 0f;
                    BodyPartRecord part = null;
                    CurSituation(pawn5, out var offset, out var glut, out var rested, out var physical,
                        out var planting,
                        out var dirty);
                    offset += GetImmunityOS(pawn5);
                    var CABaseChance = Controller.Settings.CAChance;
                    if (offset > 50)
                    {
                        offset = 50;
                    }

                    if (offset < -50)
                    {
                        offset = -50;
                    }

                    var rndtype = Rand.Range(1, 11);
                    if (rndtype == 1)
                    {
                        if (glut)
                        {
                            if (CanAddCA(CABaseChance, offset))
                            {
                                hediffdef = ComAilDefOf.HediffDefOf.CA_Indigestion;
                                severity = 0.5f;
                            }
                        }
                        else if (CanAddCA(CABaseChance, offset))
                        {
                            GetRndAilment(out hediffdef, out severity);
                        }
                    }
                    else if (rndtype == 2)
                    {
                        if (rested)
                        {
                            if (CanAddCA(CABaseChance, offset))
                            {
                                hediffdef = ComAilDefOf.HediffDefOf.CA_Restless;
                                severity = Rand.Range(1f, 3f);
                            }
                        }
                        else if (CanAddCA(CABaseChance, offset))
                        {
                            GetRndAilment(out hediffdef, out severity);
                        }
                    }
                    else if (rndtype == 3)
                    {
                        if (physical)
                        {
                            if (CanAddCA(CABaseChance, offset))
                            {
                                if (GetBPR(pawn5, out part, out var partdef))
                                {
                                    hediffdef = GetBPHed(partdef);
                                    if (part != null)
                                    {
                                        severity = 1f;
                                    }
                                }
                            }
                        }
                        else if (CanAddCA(CABaseChance, offset))
                        {
                            GetRndAilment(out hediffdef, out severity);
                        }
                    }
                    else if (rndtype == 4)
                    {
                        var pawn2 = pawn5;
                        if (pawn2?.Map != null)
                        {
                            var pawn3 = pawn5;
                            bool b;
                            if (pawn3 == null)
                            {
                                b = false;
                            }
                            else
                            {
                                var position = pawn3.Position;
                                var pawn4 = pawn5;
                                var terrain = position.GetTerrain(pawn4?.Map);
                                var num = terrain != null ? new float?(terrain.fertility) : null;
                                var num2 = 0f;
                                b = (num.GetValueOrDefault() > num2) & (num != null);
                            }

                            if (b)
                            {
                                if (planting)
                                {
                                    if (CanAddCA(CABaseChance, offset))
                                    {
                                        hediffdef = ComAilDefOf.HediffDefOf.CA_Hayfever;
                                        severity = Rand.Range(0.5f, 2f);
                                    }

                                    goto IL_351;
                                }

                                if (CanAddCA(CABaseChance, offset))
                                {
                                    GetRndAilment(out hediffdef, out severity);
                                }

                                goto IL_351;
                            }
                        }

                        if (CanAddCA(CABaseChance, offset))
                        {
                            GetRndAilment(out hediffdef, out severity);
                        }
                    }
                    else if (rndtype == 5)
                    {
                        if (dirty)
                        {
                            if (CanAddCA(CABaseChance, offset))
                            {
                                var rnd = Rand.Range(1, 100);
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
                        else if (CanAddCA(CABaseChance, offset))
                        {
                            GetRndAilment(out hediffdef, out severity);
                        }
                    }
                    else if (CanAddCA(CABaseChance, offset))
                    {
                        GetRndAilment(out hediffdef, out severity);
                    }

                    IL_351:
                    if (hediffdef != null && severity > 0f &&
                        !TryApplyAilment(hediffdef, severity, pawn5, part, out var imm) && !imm)
                    {
                        if (CanAddCA(CABaseChance * 2, offset))
                        {
                            GetRndAilment(out hediffdef, out severity);
                            if (hediffdef != null && severity > 0f)
                            {
                                TryApplyAilment(hediffdef, severity, pawn5, null, out _);
                            }
                        }
                    }
                }

                PawnCAData.PawnCADataTickSet(pawn5);
            }
        }

        // Token: 0x06000007 RID: 7 RVA: 0x00002624 File Offset: 0x00000824
        public static bool GetEitherProtected(Pawn partner, Pawn p)
        {
            var Condom = DefDatabase<HediffDef>.GetNamed("MSCondom_High", false);
            if (Condom == null)
            {
                return false;
            }

            if (partner != null && CommonAilmentsUtility.HasHediff(partner, Condom))
            {
                return true;
            }

            if (p != null && CommonAilmentsUtility.HasHediff(p, Condom))
            {
                return true;
            }

            return false;
        }

        // Token: 0x06000008 RID: 8 RVA: 0x00002660 File Offset: 0x00000860
        public static bool TryApplyAilment(HediffDef h, float s, Pawn p, BodyPartRecord b, out bool i)
        {
            i = false;
            if (HasCA(p, h))
            {
                return false;
            }

            if (CommonAilmentsUtility.HediffEffect(h, s, p, b, out var i2))
            {
                SendCAMsg(p, h);
                return true;
            }

            i = i2;

            return false;
        }

        // Token: 0x06000009 RID: 9 RVA: 0x00002698 File Offset: 0x00000898
        public static void GetRndAilment(out HediffDef hediffdef, out float severity)
        {
            if (Rand.Range(1, 100) <= 65)
            {
                var chance = Rand.Range(1, 100);
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
            var offset = 0;
            if (p.story.traits.HasTrait(DefDatabase<TraitDef>.GetNamed("Immunity", false)))
            {
                var traits = p.story.traits;
                var ImmunityTrait = traits?.GetTrait(DefDatabase<TraitDef>.GetNamed("Immunity", false));
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
            return p.RaceProps.Humanlike && !p.IsBurning() && !p.Dead && !p.RaceProps.Animal &&
                   !p.RaceProps.IsMechanoid && (!p.InContainerEnclosed || !IsInCryptoCasket(p));
        }

        // Token: 0x0600000D RID: 13 RVA: 0x0000297C File Offset: 0x00000B7C
        public static bool IsInCryptoCasket(Pawn p)
        {
            if (p?.ParentHolder is Building holder &&
                (holder is Building_AncientCryptosleepCasket || holder is Building_CryptosleepCasket))
            {
                return true;
            }

            return false;
        }

        // Token: 0x0600000E RID: 14 RVA: 0x000029BC File Offset: 0x00000BBC
        public static bool GetBPR(Pawn p, out BodyPartRecord part, out BodyPartDef partdef)
        {
            part = null;
            partdef = null;
            var partsChance = Rand.Range(1, 100);
            if (partsChance <= 50)
            {
                partdef = partsChance <= 20 ? BodyPartDefOf.Arm : BodyPartDefOf.Hand;
            }
            else if (partsChance >= 80)
            {
                partdef = BodyPartDefOf.Leg;
            }
            else
            {
                partdef = DefDatabase<BodyPartDef>.GetNamed("Foot", false);
            }

            var body = p.RaceProps.body;
            var parts = body?.GetPartsWithDef(partdef)!.ToList();
            part = parts.RandomElement();

            if (part == null || partdef == null)
            {
                return false;
            }

            var condition = HealthUtility.GetPartConditionLabel(p, part);
            if (condition.First == "SeriouslyImpaired".Translate() ||
                condition.First == "ShatteredBodyPart".Translate() ||
                condition.First == "DestroyedBodyPart".Translate() || condition.First == "MissingBodyPart".Translate())
            {
                return false;
            }

            var health = p.health;
            return health?.hediffSet == null ||
                   !p.health.hediffSet.HasDirectlyAddedPartFor(part);
        }

        // Token: 0x0600000F RID: 15 RVA: 0x00002AF8 File Offset: 0x00000CF8
        public static void SendCAMsg(Pawn p, HediffDef h)
        {
            if (!p.IsColonist || !Controller.Settings.DoMsgs)
            {
                return;
            }

            string text = p.Label.CapitalizeFirst() + "ComAil.Confirmation".Translate() + h.label.CapitalizeFirst();
            var MTD = MessageTypeDefOf.NegativeHealthEvent;
            if (!Controller.Settings.NegHealthSound)
            {
                MTD = MessageTypeDefOf.NeutralEvent;
            }

            Messages.Message(text, p, MTD);
        }

        // Token: 0x06000010 RID: 16 RVA: 0x00002B70 File Offset: 0x00000D70
        public static bool HasCA(Pawn p, HediffDef h)
        {
            var health = p.health;
            bool b;
            if (health == null)
            {
                b = false;
            }
            else
            {
                var hediffSet = health.hediffSet;
                b = hediffSet?.GetFirstHediffOfDef(h) != null;
            }

            if (b)
            {
                return true;
            }

            if (h == ComAilDefOf.HediffDefOf.CA_Headache)
            {
                var health2 = p.health;
                bool b1;
                if (health2 == null)
                {
                    b1 = false;
                }
                else
                {
                    var hediffSet2 = health2.hediffSet;
                    b1 = hediffSet2?.GetFirstHediffOfDef(ComAilDefOf.HediffDefOf.CA_Migraine) != null;
                }

                if (b1)
                {
                    return true;
                }
            }

            if (h != ComAilDefOf.HediffDefOf.CA_Migraine)
            {
                return false;
            }

            var health3 = p.health;
            bool hasCa;
            if (health3 == null)
            {
                hasCa = false;
            }
            else
            {
                var hediffSet3 = health3.hediffSet;
                hasCa = hediffSet3?.GetFirstHediffOfDef(ComAilDefOf.HediffDefOf.CA_Headache) != null;
            }

            return hasCa;
        }

        // Token: 0x06000011 RID: 17 RVA: 0x00002C02 File Offset: 0x00000E02
        public static bool CanAddCA(int chance, int offset)
        {
            return Rand.Range(1, 100 - offset) <= chance;
        }

        // Token: 0x06000012 RID: 18 RVA: 0x00002C14 File Offset: 0x00000E14
        public static bool CanAddCASTI(int chance, int offset, bool infected)
        {
            var bias = 2000;
            if (infected)
            {
                bias = 200;
            }

            return Rand.Range(1, bias - offset) <= chance;
        }

        // Token: 0x06000013 RID: 19 RVA: 0x00002C40 File Offset: 0x00000E40
        internal static bool isCAValidBodyRace(Pawn pawn)
        {
            var valid = CAValidRaceBodies().Contains(pawn.kindDef.race.defName);

            return valid;
        }

        // Token: 0x06000014 RID: 20 RVA: 0x00002C6E File Offset: 0x00000E6E
        internal static List<string> CAValidRaceBodies()
        {
            var list = new List<string>();
            list.AddDistinct("Human");
            return list;
        }

        // Token: 0x06000015 RID: 21 RVA: 0x00002C80 File Offset: 0x00000E80
        internal static List<SickListData> CASickList()
        {
            var list = new List<SickListData>();
            var SLD = new SickListData(HediffDefOf.WoundInfection, 1);
            list.Add(SLD);
            SLD = new SickListData(HediffDefOf.ResurrectionSickness, 1);
            list.Add(SLD);
            SLD = new SickListData(HediffDefOf.Plague, 2);
            list.Add(SLD);
            SLD = new SickListData(HediffDefOf.Malnutrition, 1);
            list.Add(SLD);
            SLD = new SickListData(HediffDefOf.Malaria, 2);
            list.Add(SLD);
            SLD = new SickListData(HediffDefOf.FoodPoisoning, 1);
            list.Add(SLD);
            SLD = new SickListData(HediffDefOf.Flu, 1);
            list.Add(SLD);
            SLD = new SickListData(HediffDefOf.CryptosleepSickness, 2);
            list.Add(SLD);
            SLD = new SickListData(HediffDefOf.Carcinoma, 2);
            list.Add(SLD);
            SLD = new SickListData(ComAilDefOf.HediffDefOf.CA_CommonCold, 1);
            list.Add(SLD);
            SLD = new SickListData(ComAilDefOf.HediffDefOf.CA_Conjunctivitis, 1);
            list.Add(SLD);
            SLD = new SickListData(ComAilDefOf.HediffDefOf.CA_Earache, 1);
            list.Add(SLD);
            SLD = new SickListData(ComAilDefOf.HediffDefOf.CA_Hayfever, 1);
            list.Add(SLD);
            SLD = new SickListData(ComAilDefOf.HediffDefOf.CA_SkinRash, 1);
            list.Add(SLD);
            SLD = new SickListData(ComAilDefOf.HediffDefOf.CA_SoreThroat, 1);
            list.Add(SLD);
            SLD = new SickListData(ComAilDefOf.HediffDefOf.CA_Minor_STD, 1);
            list.Add(SLD);
            var ModList = new List<string>();
            ModList.AddDistinct("DBHDehydration");
            ModList.AddDistinct("Cholera");
            ModList.AddDistinct("Dysentery");
            ModList.AddDistinct("Diarrhea");
            ModList.AddDistinct("BadHygiene");
            if (ModList.Count <= 0)
            {
                return list;
            }

            foreach (var defName in ModList)
            {
                var ModFactor = 1;
                var ModHediffDef = DefDatabase<HediffDef>.GetNamed(defName, false);
                if (ModHediffDef == null)
                {
                    continue;
                }

                if (defName == "BadHygiene")
                {
                    ModFactor = 2;
                }

                SLD = new SickListData(ModHediffDef, ModFactor);
                list.Add(SLD);
            }

            return list;
        }

        // Token: 0x06000016 RID: 22 RVA: 0x00002E6C File Offset: 0x0000106C
        public static void CurSituation(Pawn pawn, out int offset, out bool glut, out bool rested, out bool physical,
            out bool planting, out bool dirty)
        {
            offset = 0;
            HediffSet hediffSet;
            if (pawn == null)
            {
                hediffSet = null;
            }
            else
            {
                var health = pawn.health;
                hediffSet = health?.hediffSet;
            }

            var phedset = hediffSet;
            if (phedset != null)
            {
                List<SickListData> chkList;
                if (CASickListCached != null && CASickListCached.Count > 0)
                {
                    chkList = CASickListCached;
                }
                else
                {
                    chkList = CASickList();
                    CASickListCached = CASickList();
                }

                if (chkList.Count > 0)
                {
                    for (var i = 0; i < chkList.Count; i++)
                    {
                        var chkhediff = chkList[i].GetSLDHD(chkList[i]);
                        var chkfactor = chkList[i].GetSLDF(chkList[i]);
                        if (phedset.GetFirstHediffOfDef(chkhediff) != null)
                        {
                            offset += chkfactor;
                        }
                    }
                }
            }

            if (pawn != null)
            {
                var bioAgeOS = (pawn.ageTracker.AgeBiologicalYears - 40) / 10;
                if (bioAgeOS > 3)
                {
                    bioAgeOS = 3;
                }

                if (bioAgeOS < -3)
                {
                    bioAgeOS = -3;
                }

                offset += bioAgeOS;
            }

            bool b;
            if (pawn == null)
            {
                b = false;
            }
            else
            {
                var health2 = pawn.health;
                if (health2 == null)
                {
                    b = false;
                }
                else
                {
                    var hediffSet2 = health2.hediffSet;
                    b = hediffSet2?.GetFirstHediffOfDef(HediffDefOf.Frail) != null;
                }
            }

            if (b)
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
                var story = pawn.story;
                bodyTypeDef = story?.bodyType;
            }

            if (bodyTypeDef == BodyTypeDefOf.Fat)
            {
                offset++;
            }

            if (pawn?.Map != null)
            {
                var outtemp = pawn.Map.mapTemperature.OutdoorTemp;
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
            var needs = pawn?.needs;
            if (needs?.food != null)
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
            var needs2 = pawn.needs;
            if (needs2?.rest != null)
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

            var needs3 = pawn.needs;
            if (needs3?.outdoors != null)
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
                if (pawn.Position.GetRoom(pawn.Map).GetStat(RoomStatDefOf.Cleanliness) < 0f)
                {
                    dirty = true;
                }

                var RoomOffset =
                    (int) ((0f - pawn.Position.GetRoom(pawn.Map).GetStat(RoomStatDefOf.Cleanliness)) * 10f);
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
            if (pawn?.CurJob == null)
            {
                return;
            }

            physical = CurJobPhysical(pawn.CurJob);
            planting = CurJobPlants(pawn.CurJob);
        }

        // Token: 0x06000017 RID: 23 RVA: 0x000031D2 File Offset: 0x000013D2
        internal static List<JobDef> CurJobPlantsListing()
        {
            var list = new List<JobDef>();
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
            var workplants = false;
            var jobdef = job.def;
            var chkList = CurJobPlantsListing();
            if (chkList != null && chkList.Contains(jobdef))
            {
                workplants = true;
            }

            return workplants;
        }

        // Token: 0x06000019 RID: 25 RVA: 0x0000323C File Offset: 0x0000143C
        internal static List<JobDef> CurJobPhysicalListing()
        {
            var list = CurJobPlantsListing();
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
            var physical = false;
            var jobdef = job.def;
            var chkList = CurJobPhysicalListing();
            if (chkList != null && chkList.Contains(jobdef))
            {
                physical = true;
            }

            return physical;
        }

        // Token: 0x02000012 RID: 18
        internal struct SickListData
        {
            // Token: 0x06000047 RID: 71 RVA: 0x00004336 File Offset: 0x00002536
            internal SickListData(HediffDef hd, int i)
            {
                SLDdef = hd;
                SLDfactor = i;
            }

            // Token: 0x06000048 RID: 72 RVA: 0x00004346 File Offset: 0x00002546
            internal HediffDef GetSLDHD(SickListData s)
            {
                return s.SLDdef;
            }

            // Token: 0x06000049 RID: 73 RVA: 0x0000434E File Offset: 0x0000254E
            internal int GetSLDF(SickListData s)
            {
                return s.SLDfactor;
            }

            // Token: 0x04000015 RID: 21
            private readonly HediffDef SLDdef;

            // Token: 0x04000016 RID: 22
            private readonly int SLDfactor;
        }
    }
}