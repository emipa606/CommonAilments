using System;
using RimWorld;
using Verse;

namespace ComAil
{
	// Token: 0x02000005 RID: 5
	public class ComAilDefOf
	{
		// Token: 0x0600001C RID: 28 RVA: 0x000032EF File Offset: 0x000014EF
		static ComAilDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ComAilDefOf.HediffDefOf));
		}

		// Token: 0x02000013 RID: 19
		[DefOf]
		public static class HediffDefOf
		{
			// Token: 0x04000017 RID: 23
			public static HediffDef CA_CommonCold;

			// Token: 0x04000018 RID: 24
			public static HediffDef CA_Conjunctivitis;

			// Token: 0x04000019 RID: 25
			public static HediffDef CA_Earache;

			// Token: 0x0400001A RID: 26
			public static HediffDef CA_Headache;

			// Token: 0x0400001B RID: 27
			public static HediffDef CA_Indigestion;

			// Token: 0x0400001C RID: 28
			public static HediffDef CA_Migraine;

			// Token: 0x0400001D RID: 29
			public static HediffDef CA_Sprain_Hand;

			// Token: 0x0400001E RID: 30
			public static HediffDef CA_Sprain_Arm;

			// Token: 0x0400001F RID: 31
			public static HediffDef CA_Sprain_Foot;

			// Token: 0x04000020 RID: 32
			public static HediffDef CA_Sprain_Leg;

			// Token: 0x04000021 RID: 33
			public static HediffDef CA_Knick_Hand;

			// Token: 0x04000022 RID: 34
			public static HediffDef CA_Knick_Arm;

			// Token: 0x04000023 RID: 35
			public static HediffDef CA_Knick_Foot;

			// Token: 0x04000024 RID: 36
			public static HediffDef CA_Knick_Leg;

			// Token: 0x04000025 RID: 37
			public static HediffDef CA_Restless;

			// Token: 0x04000026 RID: 38
			public static HediffDef CA_SoreThroat;

			// Token: 0x04000027 RID: 39
			public static HediffDef CA_Hayfever;

			// Token: 0x04000028 RID: 40
			public static HediffDef CA_SkinRash;

			// Token: 0x04000029 RID: 41
			public static HediffDef CA_Sinusitis;

			// Token: 0x0400002A RID: 42
			public static HediffDef CA_Minor_STD;

			// Token: 0x0400002B RID: 43
			public static HediffDef CA_PhantomPain;

			// Token: 0x0400002C RID: 44
			public static HediffDef CA_Fatigue;
		}
	}
}
