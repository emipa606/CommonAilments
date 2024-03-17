using Verse;

namespace ComAil;

public class HediffCompProperties_CASymptom : HediffCompProperties
{
    public readonly float symptomChance = 25f;

    public HediffCompProperties_CASymptom()
    {
        compClass = typeof(HediffComp_CASymptom);
    }
}