using UnityEngine;
using Verse;

namespace ComAil;

public class Settings : ModSettings
{
    public bool CACanAddScenario = true;

    public int CAChance = 10;

    public int CAFrequency = 24;

    public int CAStartDays = 60;

    public int CASTIChance = 10;

    public int CASTIOffset = -50;

    public bool DoAilments = true;

    public bool DoMsgs = true;

    public bool DoSTI = true;

    public bool DoSymptoms;

    public bool NegHealthSound = true;

    public void DoWindowContents(Rect canvas)
    {
        var gap = 8f;
        var listingStandard = new Listing_Standard { ColumnWidth = canvas.width };
        listingStandard.Begin(canvas);
        listingStandard.Gap(gap);
        listingStandard.CheckboxLabeled("ComAil.DoAilments".Translate(), ref DoAilments);
        listingStandard.Gap(gap);
        listingStandard.CheckboxLabeled("ComAil.DoSymptoms".Translate(), ref DoSymptoms);
        listingStandard.Gap(gap);
        listingStandard.CheckboxLabeled("ComAil.DoMsgs".Translate(), ref DoMsgs);
        listingStandard.Gap(gap);
        listingStandard.CheckboxLabeled("ComAil.NegHealthSound".Translate(), ref NegHealthSound);
        listingStandard.Gap(gap);
        listingStandard.Label("ComAil.CAStartDays".Translate() + "  " + CAStartDays);
        checked
        {
            CAStartDays = (int)listingStandard.Slider(CAStartDays, 0f, 150f);
            listingStandard.Gap(gap);
            listingStandard.Label("ComAil.CAFrequency".Translate() + "  " + CAFrequency);
            CAFrequency = (int)listingStandard.Slider(CAFrequency, 6f, 72f);
            listingStandard.Gap(gap);
            listingStandard.Label("ComAil.CAChance".Translate() + "  " + CAChance);
            CAChance = (int)listingStandard.Slider(CAChance, 1f, 25f);
            listingStandard.Gap(gap);
            listingStandard.CheckboxLabeled("ComAil.DoSTI".Translate(), ref DoSTI);
            listingStandard.Gap(gap);
            listingStandard.Label("ComAil.CASTIChance".Translate() + "  " + CASTIChance);
            CASTIChance = (int)listingStandard.Slider(CASTIChance, 1f, 25f);
            listingStandard.Gap(gap);
            listingStandard.Label("ComAil.CASTIOffset".Translate() + "  " + CASTIOffset);
            CASTIOffset = (int)listingStandard.Slider(CASTIOffset, -150f, 0f);
            listingStandard.Gap(gap);
            listingStandard.CheckboxLabeled("ComAil.CanAddScenario".Translate(), ref CACanAddScenario);
        }

        listingStandard.Gap(gap - 2f);
        Text.Font = GameFont.Tiny;
        listingStandard.Label("          " + "ComAil.ChangeValueTip".Translate());
        Text.Font = GameFont.Small;
        listingStandard.Gap(gap);
        if (Controller.currentVersion != null)
        {
            listingStandard.Gap();
            GUI.contentColor = Color.gray;
            listingStandard.Label("ComAil.CurrentModVersion".Translate(Controller.currentVersion));
            GUI.contentColor = Color.white;
        }

        listingStandard.End();
    }

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