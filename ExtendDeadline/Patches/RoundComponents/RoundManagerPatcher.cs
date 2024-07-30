using HarmonyLib;

namespace ExtendDeadline.Patches.RoundComponents
{
    [HarmonyPatch(typeof(RoundManager))]
    internal static class RoundManagerPatcher
    {
        static int previousDaysDeadline = TimeOfDay.Instance.daysUntilDeadline;
        static int DEFAULT_DAYS_DEADLINE = 4;
        static bool savedPrevious = false;

        /// <summary>
        /// Shoutout to ustaalon (https://github.com/ustaalon) for pointing out the issue when increasing the amount of days before deadline affecting
        /// the enemy spawning
        /// </summary>
        [HarmonyPatch(nameof(RoundManager.PlotOutEnemiesForNextHour))]
        [HarmonyPatch(nameof(RoundManager.AdvanceHourAndSpawnNewBatchOfEnemies))]
        [HarmonyPrefix]
        static void ChangeDaysForEnemySpawns()
        {
            if (TimeOfDay.Instance.daysUntilDeadline < DEFAULT_DAYS_DEADLINE) return; // Either it's already fine or some other mod already changed the value to be acceptable
            Plugin.mls.LogDebug("Changing deadline to allow spawning enemies.");
            previousDaysDeadline = TimeOfDay.Instance.daysUntilDeadline;
            TimeOfDay.Instance.daysUntilDeadline %= DEFAULT_DAYS_DEADLINE;
            savedPrevious = true;
        }

        [HarmonyPatch(nameof(RoundManager.PlotOutEnemiesForNextHour))]
        [HarmonyPatch(nameof(RoundManager.AdvanceHourAndSpawnNewBatchOfEnemies))]
        [HarmonyPostfix]
        static void UndoChangeDaysForEnemySpawns()
        {
            if (!savedPrevious) return;
            Plugin.mls.LogDebug("Changing back the deadline...");
            TimeOfDay.Instance.daysUntilDeadline = previousDaysDeadline;
            savedPrevious = false;
        }
    }
}
