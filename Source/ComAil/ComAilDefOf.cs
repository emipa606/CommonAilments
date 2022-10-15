using RimWorld;
using Verse;

namespace ComAil;

public class ComAilDefOf
{
    static ComAilDefOf()
    {
        DefOfHelper.EnsureInitializedInCtor(typeof(HediffDefOf));
    }

    [DefOf]
    public static class HediffDefOf
    {
        public static HediffDef CA_CommonCold;

        public static HediffDef CA_Conjunctivitis;

        public static HediffDef CA_Earache;

        public static HediffDef CA_Headache;

        public static HediffDef CA_Indigestion;

        public static HediffDef CA_Migraine;

        public static HediffDef CA_Sprain_Hand;

        public static HediffDef CA_Sprain_Arm;

        public static HediffDef CA_Sprain_Foot;

        public static HediffDef CA_Sprain_Leg;

        public static HediffDef CA_Knick_Hand;

        public static HediffDef CA_Knick_Arm;

        public static HediffDef CA_Knick_Foot;

        public static HediffDef CA_Knick_Leg;

        public static HediffDef CA_Restless;

        public static HediffDef CA_SoreThroat;

        public static HediffDef CA_Hayfever;

        public static HediffDef CA_SkinRash;

        public static HediffDef CA_Sinusitis;

        public static HediffDef CA_Minor_STD;

        public static HediffDef CA_PhantomPain;

        public static HediffDef CA_Fatigue;
    }
}