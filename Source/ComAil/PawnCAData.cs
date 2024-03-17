using Verse;

namespace ComAil;

public class PawnCAData : ThingComp
{
    public int PawnCATick;

    private Pawn pawn => (Pawn)parent;

    public override void PostExposeData()
    {
        base.PostExposeData();
        Scribe_Values.Look(ref PawnCATick, "PawnCATick");
    }

    public static void PawnCADataTickSet(Pawn pawn, bool symptom = false)
    {
        ThingComp PComp = pawn?.TryGetComp<PawnCAData>();

        var PData = (PawnCAData)PComp;
        if (PData == null)
        {
            return;
        }

        if (!symptom)
        {
            PData.PawnCATick = Find.TickManager.TicksGame + (Controller.Settings.CAFrequency * 2500) +
                               Rand.Range(-7500, 7500);
            return;
        }

        PData.PawnCATick = Find.TickManager.TicksGame +
                           (int)(Controller.Settings.CAFrequency / 2f * 2500f) + Rand.Range(-2500, 2500);
    }

    public class CompProperties_PawnCAData : CompProperties
    {
        public CompProperties_PawnCAData()
        {
            compClass = typeof(PawnCAData);
        }
    }
}