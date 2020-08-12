using System;
using Verse;

namespace ComAil
{
	// Token: 0x0200000B RID: 11
	public class HediffCompProperties_CASymptom : HediffCompProperties
	{
		// Token: 0x06000030 RID: 48 RVA: 0x000039AE File Offset: 0x00001BAE
		public HediffCompProperties_CASymptom()
		{
			this.compClass = typeof(HediffComp_CASymptom);
		}

		// Token: 0x04000005 RID: 5
		public float symptomChance = 25f;
	}
}
