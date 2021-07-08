using UnityEngine;
using Verse;

namespace ComAil
{
    // Token: 0x02000011 RID: 17
    public class Settings : ModSettings
    {
        // Token: 0x04000012 RID: 18
        public bool CACanAddScenario = true;

        // Token: 0x0400000E RID: 14
        public int CAChance = 10;

        // Token: 0x0400000D RID: 13
        public int CAFrequency = 24;

        // Token: 0x0400000C RID: 12
        public int CAStartDays = 60;

        // Token: 0x04000010 RID: 16
        public int CASTIChance = 10;

        // Token: 0x04000011 RID: 17
        public int CASTIOffset = -50;

        // Token: 0x0400000A RID: 10
        public bool DoAilments = true;

        // Token: 0x04000013 RID: 19
        public bool DoMsgs = true;

        // Token: 0x0400000F RID: 15
        public bool DoSTI = true;

        // Token: 0x0400000B RID: 11
        public bool DoSymptoms;

        // Token: 0x04000014 RID: 20
        public bool NegHealthSound = true;

        // Token: 0x06000044 RID: 68 RVA: 0x00003EE8 File Offset: 0x000020E8
        public void DoWindowContents(Rect canvas)
        {
            var gap = 8f;
            var listing_Standard = new Listing_Standard {ColumnWidth = canvas.width};
            listing_Standard.Begin(canvas);
            listing_Standard.Gap(gap);
            listing_Standard.CheckboxLabeled("ComAil.DoAilments".Translate(), ref DoAilments);
            listing_Standard.Gap(gap);
            listing_Standard.CheckboxLabeled("ComAil.DoSymptoms".Translate(), ref DoSymptoms);
            listing_Standard.Gap(gap);
            listing_Standard.CheckboxLabeled("ComAil.DoMsgs".Translate(), ref DoMsgs);
            listing_Standard.Gap(gap);
            listing_Standard.CheckboxLabeled("ComAil.NegHealthSound".Translate(), ref NegHealthSound);
            listing_Standard.Gap(gap);
            listing_Standard.Label("ComAil.CAStartDays".Translate() + "  " + CAStartDays);
            checked
            {
                CAStartDays = (int) listing_Standard.Slider(CAStartDays, 0f, 150f);
                listing_Standard.Gap(gap);
                listing_Standard.Label("ComAil.CAFrequency".Translate() + "  " + CAFrequency);
                CAFrequency = (int) listing_Standard.Slider(CAFrequency, 6f, 72f);
                listing_Standard.Gap(gap);
                listing_Standard.Label("ComAil.CAChance".Translate() + "  " + CAChance);
                CAChance = (int) listing_Standard.Slider(CAChance, 1f, 25f);
                listing_Standard.Gap(gap);
                listing_Standard.CheckboxLabeled("ComAil.DoSTI".Translate(), ref DoSTI);
                listing_Standard.Gap(gap);
                listing_Standard.Label("ComAil.CASTIChance".Translate() + "  " + CASTIChance);
                CASTIChance = (int) listing_Standard.Slider(CASTIChance, 1f, 25f);
                listing_Standard.Gap(gap);
                listing_Standard.Label("ComAil.CASTIOffset".Translate() + "  " + CASTIOffset);
                CASTIOffset = (int) listing_Standard.Slider(CASTIOffset, -150f, 0f);
                listing_Standard.Gap(gap);
                listing_Standard.CheckboxLabeled("ComAil.CanAddScenario".Translate(), ref CACanAddScenario);
            }

            listing_Standard.Gap(gap - 2f);
            Text.Font = GameFont.Tiny;
            listing_Standard.Label("          " + "ComAil.ChangeValueTip".Translate());
            Text.Font = GameFont.Small;
            listing_Standard.Gap(gap);
            listing_Standard.End();
        }

        // Token: 0x06000045 RID: 69 RVA: 0x000041F8 File Offset: 0x000023F8
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref DoAilments, "DoAilments", true);
            Scribe_Values.Look(ref DoSymptoms, "DoSymptoms");
            Scribe_Values.Look(ref CAStartDays, "CAStartDays", 60);
            Scribe_Values.Look(ref CAFrequency, "CAFrequency", 24);
            Scribe_Values.Look(ref CAChance, "CAChance", 10);
            Scribe_Values.Look(ref DoSTI, "DoSTI", true, true);
            Scribe_Values.Look(ref CASTIChance, "CASTIChance", 10);
            Scribe_Values.Look(ref CASTIOffset, "CASTIOffset", -50);
            Scribe_Values.Look(ref CACanAddScenario, "CACanAddScenario", true);
            Scribe_Values.Look(ref DoMsgs, "DoMsgs", true);
            Scribe_Values.Look(ref NegHealthSound, "NegHealthSound", true);
        }
    }
}