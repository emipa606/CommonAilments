using System;
using RimWorld;
using Verse;

namespace ComAil
{
	// Token: 0x0200000D RID: 13
	public class HediffComp_CASymptom : HediffComp
	{
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000037 RID: 55 RVA: 0x00003BB2 File Offset: 0x00001DB2
		public HediffCompProperties_CASymptom CAProps
		{
			get
			{
				return (HediffCompProperties_CASymptom)this.props;
			}
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00003BC0 File Offset: 0x00001DC0
		public override void CompPostTick(ref float severityAdjustment)
		{
			if (Controller.Settings.DoSymptoms && CommonAilments.PawnCanAcceptCA(base.Pawn) && Controller.Settings.DoAilments && GenDate.DaysPassed >= Controller.Settings.CAStartDays && base.Pawn.IsHashIntervalTick(2501))
			{
				int chance = (int)(this.CAProps.symptomChance + (float)Controller.Settings.CAChance);
				int offset;
				bool glut;
				bool rested;
				bool physical;
				bool planting;
				bool dirty;
				CommonAilments.CurSituation(base.Pawn, out offset, out glut, out rested, out physical, out planting, out dirty);
				offset += Math.Max(-20, Math.Min(10, CommonAilments.GetImmunityOS(base.Pawn)));
				if (CommonAilments.CanAddCA(chance, offset))
				{
					CommonAilmentsUtility.ApplySymptom(base.Pawn);
				}
			}
		}
	}
}
