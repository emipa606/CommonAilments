using System;
using System.Collections.Generic;
using RimWorld;
using Verse;

namespace ComAil
{
	// Token: 0x0200000C RID: 12
	public class HediffComp_CACure : HediffComp
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000031 RID: 49 RVA: 0x000039D1 File Offset: 0x00001BD1
		public HediffCompProperties_CACure MSProps
		{
			get
			{
				return (HediffCompProperties_CACure)this.props;
			}
		}

		// Token: 0x06000032 RID: 50 RVA: 0x000039E0 File Offset: 0x00001BE0
		public void SetTicksToCure()
		{
			int period = 2500;
			int basehours;
			if (this.MSProps.CureHoursMin > 0f && this.MSProps.CureHoursMax > 0f && this.MSProps.CureHoursMax >= this.MSProps.CureHoursMin)
			{
				basehours = (int)(Rand.Range(this.MSProps.CureHoursMin, this.MSProps.CureHoursMax) * (float)period);
			}
			else
			{
				basehours = Rand.Range(2, 5) * period;
			}
			if (basehours < period)
			{
				basehours = period;
			}
			if (basehours > 36 * period)
			{
				basehours = 36 * period;
			}
			this.ticksToCure = basehours;
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00003A74 File Offset: 0x00001C74
		public override void CompPostTick(ref float severityAdjustment)
		{
			if (this.curing && this.ticksToCure > 0)
			{
				this.ticksToCure--;
				return;
			}
			if (this.curing)
			{
				this.parent.Severity = 0f;
				Messages.Message("ComAil.CureMsg".Translate(base.Pawn.LabelShort.CapitalizeFirst(), base.Def.label.CapitalizeFirst()), base.Pawn, MessageTypeDefOf.PositiveEvent, true);
				return;
			}
			List<string> Immunities = new List<string>();
			if (CommonAilmentsUtility.ImmuneTo(base.Pawn, base.Def, out Immunities))
			{
				int ImmunitiesAsCure = 0;
				for (int i = 0; i < Immunities.Count; i++)
				{
					if (Immunities[i] != "MSCondom_High")
					{
						ImmunitiesAsCure++;
					}
				}
				if (ImmunitiesAsCure > 0)
				{
					this.SetTicksToCure();
					this.curing = true;
				}
			}
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00003B5F File Offset: 0x00001D5F
		public override void CompExposeData()
		{
			Scribe_Values.Look<int>(ref this.ticksToCure, "ticksToCure", 0, false);
			Scribe_Values.Look<bool>(ref this.curing, "curing", false, false);
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00003B85 File Offset: 0x00001D85
		public override string CompDebugString()
		{
			if (this.curing)
			{
				return "ticksToCure: " + this.ticksToCure;
			}
			return "No active cure.";
		}

		// Token: 0x04000006 RID: 6
		private int ticksToCure;

		// Token: 0x04000007 RID: 7
		private bool curing;
	}
}
