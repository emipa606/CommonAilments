using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace ComAil
{
	// Token: 0x02000010 RID: 16
	public class PawnCAData : ThingComp
	{
		// Token: 0x06000040 RID: 64 RVA: 0x00003E1F File Offset: 0x0000201F
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.PawnCATick, "PawnCATick", 0, false);
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000041 RID: 65 RVA: 0x00003E39 File Offset: 0x00002039
		private Pawn pawn
		{
			get
			{
				return (Pawn)this.parent;
			}
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00003E48 File Offset: 0x00002048
		public static void PawnCADataTickSet(Pawn pawn, bool symptom = false)
		{
			ThingComp PComp = (pawn != null) ? pawn.TryGetComp<PawnCAData>() : null;
			if (PComp != null)
			{
				PawnCAData PData = PComp as PawnCAData;
				if (PData != null)
				{
					if (!symptom)
					{
						PData.PawnCATick = Find.TickManager.TicksGame + Controller.Settings.CAFrequency * 2500 + Rand.Range(-7500, 7500);
						return;
					}
					PData.PawnCATick = Find.TickManager.TicksGame + (int)((float)Controller.Settings.CAFrequency / 2f * 2500f) + Rand.Range(-2500, 2500);
				}
			}
		}

		// Token: 0x04000009 RID: 9
		public int PawnCATick;

		// Token: 0x02000014 RID: 20
		public class CompProperties_PawnCAData : CompProperties
		{
			// Token: 0x0600004A RID: 74 RVA: 0x00004356 File Offset: 0x00002556
			public CompProperties_PawnCAData()
			{
				this.compClass = typeof(PawnCAData);
			}
		}

		// Token: 0x02000015 RID: 21
		[StaticConstructorOnStartup]
		private static class ComAil_Setup
		{
			// Token: 0x0600004B RID: 75 RVA: 0x0000436E File Offset: 0x0000256E
			static ComAil_Setup()
			{
				PawnCAData.ComAil_Setup.ComAil_Setup_Pawns();
			}

			// Token: 0x0600004C RID: 76 RVA: 0x00004378 File Offset: 0x00002578
			private static void ComAil_Setup_Pawns()
			{
				HediffGiverSetDef Organic = DefDatabase<HediffGiverSetDef>.GetNamed("OrganicStandard", true);
				PawnCAData.ComAil_Setup.ComAilSetup_Comp(typeof(PawnCAData.CompProperties_PawnCAData), delegate(ThingDef def)
				{
					RaceProperties race = def.race;
					if (race != null && race.Humanlike)
					{
						RaceProperties race2 = def.race;
						return ((race2 != null) ? race2.hediffGiverSets : null) != null && def.race.hediffGiverSets.Contains(Organic);
					}
					return false;
				});
			}

			// Token: 0x0600004D RID: 77 RVA: 0x000043B8 File Offset: 0x000025B8
			private static void ComAilSetup_Comp(Type compType, Func<ThingDef, bool> qualifier)
			{
				List<ThingDef> list = DefDatabase<ThingDef>.AllDefsListForReading.Where(qualifier).ToList();
				GenList.RemoveDuplicates<ThingDef>(list);
				foreach (ThingDef def in list)
				{
					if (def.comps != null && !GenCollection.Any<CompProperties>(def.comps, (Predicate<CompProperties>)((CompProperties c) => ((object)c).GetType() == compType)))
					{
						def.comps.Add((CompProperties)(object)(CompProperties)Activator.CreateInstance(compType));
					}
				}
			}
		}
	}
}
