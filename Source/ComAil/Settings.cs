using System;
using UnityEngine;
using Verse;

namespace ComAil
{
	// Token: 0x02000011 RID: 17
	public class Settings : ModSettings
	{
		// Token: 0x06000044 RID: 68 RVA: 0x00003EE8 File Offset: 0x000020E8
		public void DoWindowContents(Rect canvas)
		{
			float gap = 8f;
			Listing_Standard listing_Standard = new Listing_Standard();
			listing_Standard.ColumnWidth = canvas.width;
			listing_Standard.Begin(canvas);
			listing_Standard.Gap(gap);
			listing_Standard.CheckboxLabeled("ComAil.DoAilments".Translate(), ref this.DoAilments, null);
			listing_Standard.Gap(gap);
			listing_Standard.CheckboxLabeled("ComAil.DoSymptoms".Translate(), ref this.DoSymptoms, null);
			listing_Standard.Gap(gap);
			listing_Standard.CheckboxLabeled("ComAil.DoMsgs".Translate(), ref this.DoMsgs, null);
			listing_Standard.Gap(gap);
			listing_Standard.CheckboxLabeled("ComAil.NegHealthSound".Translate(), ref this.NegHealthSound, null);
			listing_Standard.Gap(gap);
			listing_Standard.Label("ComAil.CAStartDays".Translate() + "  " + this.CAStartDays, -1f, null);
			checked
			{
				this.CAStartDays = (int)listing_Standard.Slider((float)this.CAStartDays, 0f, 150f);
				listing_Standard.Gap(gap);
				listing_Standard.Label("ComAil.CAFrequency".Translate() + "  " + this.CAFrequency, -1f, null);
				this.CAFrequency = (int)listing_Standard.Slider((float)this.CAFrequency, 6f, 72f);
				listing_Standard.Gap(gap);
				listing_Standard.Label("ComAil.CAChance".Translate() + "  " + this.CAChance, -1f, null);
				this.CAChance = (int)listing_Standard.Slider((float)this.CAChance, 1f, 25f);
				listing_Standard.Gap(gap);
				listing_Standard.CheckboxLabeled("ComAil.DoSTI".Translate(), ref this.DoSTI, null);
				listing_Standard.Gap(gap);
				listing_Standard.Label("ComAil.CASTIChance".Translate() + "  " + this.CASTIChance, -1f, null);
				this.CASTIChance = (int)listing_Standard.Slider((float)this.CASTIChance, 1f, 25f);
				listing_Standard.Gap(gap);
				listing_Standard.Label("ComAil.CASTIOffset".Translate() + "  " + this.CASTIOffset, -1f, null);
				this.CASTIOffset = (int)listing_Standard.Slider((float)this.CASTIOffset, -150f, 0f);
				listing_Standard.Gap(gap);
				listing_Standard.CheckboxLabeled("ComAil.CanAddScenario".Translate(), ref this.CACanAddScenario, null);
			}
			listing_Standard.Gap(gap - 2f);
			Text.Font = GameFont.Tiny;
			listing_Standard.Label("          " + "ComAil.ChangeValueTip".Translate(), -1f, null);
			Text.Font = GameFont.Small;
			listing_Standard.Gap(gap);
			listing_Standard.End();
		}

		// Token: 0x06000045 RID: 69 RVA: 0x000041F8 File Offset: 0x000023F8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.DoAilments, "DoAilments", true, false);
			Scribe_Values.Look<bool>(ref this.DoSymptoms, "DoSymptoms", false, false);
			Scribe_Values.Look<int>(ref this.CAStartDays, "CAStartDays", 60, false);
			Scribe_Values.Look<int>(ref this.CAFrequency, "CAFrequency", 24, false);
			Scribe_Values.Look<int>(ref this.CAChance, "CAChance", 10, false);
			Scribe_Values.Look<bool>(ref this.DoSTI, "DoSTI", true, true);
			Scribe_Values.Look<int>(ref this.CASTIChance, "CASTIChance", 10, false);
			Scribe_Values.Look<int>(ref this.CASTIOffset, "CASTIOffset", -50, false);
			Scribe_Values.Look<bool>(ref this.CACanAddScenario, "CACanAddScenario", true, false);
			Scribe_Values.Look<bool>(ref this.DoMsgs, "DoMsgs", true, false);
			Scribe_Values.Look<bool>(ref this.NegHealthSound, "NegHealthSound", true, false);
		}

		// Token: 0x0400000A RID: 10
		public bool DoAilments = true;

		// Token: 0x0400000B RID: 11
		public bool DoSymptoms;

		// Token: 0x0400000C RID: 12
		public int CAStartDays = 60;

		// Token: 0x0400000D RID: 13
		public int CAFrequency = 24;

		// Token: 0x0400000E RID: 14
		public int CAChance = 10;

		// Token: 0x0400000F RID: 15
		public bool DoSTI = true;

		// Token: 0x04000010 RID: 16
		public int CASTIChance = 10;

		// Token: 0x04000011 RID: 17
		public int CASTIOffset = -50;

		// Token: 0x04000012 RID: 18
		public bool CACanAddScenario = true;

		// Token: 0x04000013 RID: 19
		public bool DoMsgs = true;

		// Token: 0x04000014 RID: 20
		public bool NegHealthSound = true;
	}
}
