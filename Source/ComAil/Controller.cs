using UnityEngine;
using Verse;

namespace ComAil
{
    // Token: 0x02000007 RID: 7
    public class Controller : Mod
    {
        // Token: 0x04000002 RID: 2
        public static Settings Settings;

        // Token: 0x0600002A RID: 42 RVA: 0x000037DE File Offset: 0x000019DE
        public Controller(ModContentPack content) : base(content)
        {
            Settings = GetSettings<Settings>();
        }

        // Token: 0x06000028 RID: 40 RVA: 0x000037C0 File Offset: 0x000019C0
        public override string SettingsCategory()
        {
            return "ComAil.Name".Translate();
        }

        // Token: 0x06000029 RID: 41 RVA: 0x000037D1 File Offset: 0x000019D1
        public override void DoSettingsWindowContents(Rect canvas)
        {
            Settings.DoWindowContents(canvas);
        }
    }
}