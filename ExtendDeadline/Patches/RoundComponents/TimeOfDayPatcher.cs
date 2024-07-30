using ExtendDeadline.Misc;
using HarmonyLib;

namespace ExtendDeadline.Patches.RoundComponents
{
    [HarmonyPatch(typeof(TimeOfDay))]
    internal static class TimeOfDayPatcher
    {
        [HarmonyPostfix]
        [HarmonyPatch(nameof(TimeOfDay.SyncNewProfitQuotaClientRpc))]
        static void SyncNewProfitQuotaClientRpcPostfix()
        {
            ExtendDeadlineBehaviour.SetDaysExtended(daysExtended: 0);
        }
    }
}
