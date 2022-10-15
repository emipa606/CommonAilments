using Verse;

namespace ComAil;

public class HediffCompProperties_CACure : HediffCompProperties
{
    public float CureHoursMax;

    public float CureHoursMin;

    public HediffCompProperties_CACure()
    {
        compClass = typeof(HediffComp_CACure);
    }
}