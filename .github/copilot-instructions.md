# GitHub Copilot Instructions for RimWorld Mod: Common Ailments (Continued)

## Mod Overview and Purpose
The **Common Ailments (Continued)** mod for RimWorld is an update to pelador's original mod, which adds various common illnesses and conditions to the game. This mod enhances the medical and social aspects of RimWorld by introducing new ailments that can affect colonists, including common colds, conjunctivitis, migraines, minor STDs, and phantom pain.

## Key Features and Systems
### Ailments
- **Common Cold**: A mild illness that reduces a colonist's work efficiency and increases their likelihood of catching more severe ailments.
- **Conjunctivitis**: An eye infection that causes temporary blindness and reduces social interactions.
- **Migraine**: A severe headache that disables a colonist for a period and affects their mental capacity.
- **Minor STDs**: A range of minor sexually transmitted diseases that can lead to social isolation and reduced fertility.
- **Phantom Pain**: A condition where a colonist experiences pain in an amputated limb, affecting their movement and interaction.

### Systems
- **Hediff Comp Systems**: Custom hediff components that manage ailment symptoms and cures.
- **Compatibility Patches**: Integrations with other popular mods to ensure optimal functionality.

## Coding Patterns and Conventions
### C# Coding Patterns
- **Mod Initialization**: Use `ComAil_Setup` to initialize mod settings and pawn ailments.
- **Hediff Components**: Implement custom hediff components like `HediffComp_CASymptom` for managing ailment symptoms.
- **Property Classes**: Use property classes like `HediffCompProperties_CACure` to define hediff component properties.

### XML Conventions
- **Defs Files**: Define ailments and their properties in XML files located in the `Defs` directory.
- **XPath Patching**: Use XPath patching to integrate with other mods and ensure compatibility.

## XML Integration
To integrate new ailments and modify existing ones, follow the XML conventions outlined below:
1. **Create New Defs**: Create new XML files for each ailment (e.g., `Defs_Common_Cold.xml`).
2. **Define HediffDefs**: Define `HediffDef` elements within each XML file to specify ailment properties.
3. **XPath Patches**: Use XPath patches to modify existing mod definitions and ensure compatibility with other mods.

### Example XML Definition
xml
<HediffDef>
    <defName>ComAil_CommonCold</defName>
    <label>Common Cold</label>
    <description>A mild illness that reduces work efficiency and increases susceptibility to other ailments.</description>
    <stages>
        <li>
            <label>Mild Cold</label>
            <severity>0.1</severity>
            <ticksToHeal>15000</ticksToHeal>
        </li>
    </stages>
    <comp class="HediffComp_CASymptom">
        <CAProps>
            <severityMultiplier>0.5</severityMultiplier>
        </CAProps>
    </comp>
</HediffDef>


## Harmony Patching
The mod uses Harmony patching to extend and modify existing RimWorld functionality. Key patches include:
- **Patching Drug Effects**: Modify drug effects to enhance compatibility with new ailments.
- **Integration with Other Mods**: Use Harmony patches to integrate with popular mods like Apothecary and Diseases Overhauled.

### Example Harmony Patch
csharp
[HarmonyPatch(typeof(DrugPolicyEntry), "PostApply")]
public static class DrugPolicyEntry_PostApply_Patch
{
    [HarmonyPostfix]
    public static void PostApply(ref DrugPolicyEntry __instance, ref Pawn ___pawn)
    {
        if (___pawn.HasAilment(ComAilDefOf.ComAil_CommonCold))
        {
            // Modify drug effect based on common cold
        }
    }
}


## Suggestions for Copilot
1. **Provide XML Templates**: Include XML templates for defining new ailments to streamline the process of adding new conditions.
2. **Highlight Harmony Patches**: Emphasize the importance of Harmony patches for integrating with other mods.
3. **Offer Code Snippets**: Provide code snippets for common tasks such as initializing mod settings and managing hediff components.
4. **Document Compatibility**: Document the compatibility of the mod with other popular RimWorld mods to facilitate integration.

By following these guidelines, you can efficiently contribute to the **Common Ailments (Continued)** mod and enhance the medical experience in RimWorld.

## Project Solution Guidelines
- Relevant mod XML files are included as Solution Items under the solution folder named XML, these can be read and modified from within the solution.
- Use these in-solution XML files as the primary files for reference and modification.
- The `.github/copilot-instructions.md` file is included in the solution under the `.github` solution folder, so it should be read/modified from within the solution instead of using paths outside the solution. Update this file once only, as it and the parent-path solution reference point to the same file in this workspace.
- When making functional changes in this mod, ensure the documented features stay in sync with implementation; use the in-solution `.github` copy as the primary file.
- In the solution is also a project called Assembly-CSharp, containing a read-only version of the decompiled game source, for reference and debugging purposes.
- For any new documentation, update this copilot-instructions.md file rather than creating separate documentation files.


## Hard rules (must follow)
- Do NOT run commands that modify the repo (no git commit, git apply, dotnet format) unless explicitly asked.
- Prefer minimal reads: read only the smallest code region needed (around the suspicious lines).
- When mentioning SonarQube issues, automatically use the SonarQube MCP service to fetch and address issues instead of making inferred fixes without querying SonarQube first.
- When mentioning the rimworld log, automatically use the Rimworld MCP service to fetch the log.

