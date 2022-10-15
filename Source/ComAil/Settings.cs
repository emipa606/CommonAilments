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
        var listing_Standard = new Listing_Standard { ColumnWidth = canvas.width };
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
            CAStartDays = (int)listing_Standard.Slider(CAStartDays, 0f, 150f);
            listing_Standard.Gap(gap);
            listing_Standard.Label("ComAil.CAFrequency".Translate() + "  " + CAFrequency);
            CAFrequency = (int)listing_Standard.Slider(CAFrequency, 6f, 72f);
            listing_Standard.Gap(gap);
            listing_Standard.Label("ComAil.CAChance".Translate() + "  " + CAChance);
            CAChance = (int)listing_Standard.Slider(CAChance, 1f, 25f);
            listing_Standard.Gap(gap);
            listing_Standard.CheckboxLabeled("ComAil.DoSTI".Translate(), ref DoSTI);
            listing_Standard.Gap(gap);
            listing_Standard.Label("ComAil.CASTIChance".Translate() + "  " + CASTIChance);
            CASTIChance = (int)listing_Standard.Slider(CASTIChance, 1f, 25f);
            listing_Standard.Gap(gap);
            listing_Standard.Label("ComAil.CASTIOffset".Translate() + "  " + CASTIOffset);
            CASTIOffset = (int)listing_Standard.Slider(CASTIOffset, -150f, 0f);
            listing_Standard.Gap(gap);
            listing_Standard.CheckboxLabeled("ComAil.CanAddScenario".Translate(), ref CACanAddScenario);
        }

        listing_Standard.Gap(gap - 2f);
        Text.Font = GameFont.Tiny;
        listing_Standard.Label("          " + "ComAil.ChangeValueTip".Translate());
        Text.Font = GameFont.Small;
        listing_Standard.Gap(gap);
        if (Controller.currentVersion != null)
        {
            listing_Standard.Gap();
            GUI.contentColor = Color.gray;
            listing_Standard.Label("ComAil.CurrentModVersion".Translate(Controller.currentVersion));
            GUI.contentColor = Color.white;
        }

        listing_Standard.End();
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