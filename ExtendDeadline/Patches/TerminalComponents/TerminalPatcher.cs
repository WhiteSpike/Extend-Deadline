using ExtendDeadline.Misc;
using HarmonyLib;

namespace ExtendDeadline.Patches.TerminalComponents
{
    [HarmonyPatch(typeof(Terminal))]
    internal static class TerminalPatcher
    {
        [HarmonyPostfix]
        [HarmonyPatch(nameof(Terminal.ParsePlayerSentence))]
        static void CustomParser(ref Terminal __instance, ref TerminalNode __result)
        {
            string text = __instance.screenText.text.Substring(__instance.screenText.text.Length - __instance.textAdded);
            CommandParser.ParseCommands(text, ref __instance, ref __result);
        }
    }
}
