using Verse;

namespace ComAil
{
    // Token: 0x0200000A RID: 10
    public class HediffCompProperties_CACure : HediffCompProperties
    {
        // Token: 0x04000004 RID: 4
        public float CureHoursMax;

        // Token: 0x04000003 RID: 3
        public float CureHoursMin;

        // Token: 0x0600002F RID: 47 RVA: 0x00003996 File Offset: 0x00001B96
        public HediffCompProperties_CACure()
        {
            compClass = typeof(HediffComp_CACure);
        }
    }
}