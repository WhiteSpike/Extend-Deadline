using ExtendDeadline;
using Unity.Netcode;

namespace ExtendDeadline.Misc
{
    public class ExtendDeadlineBehaviour : NetworkBehaviour
    {
        internal const string COMMAND_NAME = "Extend Deadline";
        internal static ExtendDeadlineBehaviour Instance { get; set; }
        int daysExtended;
        static void SetInstance(ExtendDeadlineBehaviour instance )
        {
            Instance = instance;
        }
        void Start()
        {
            SetInstance(this);
            DontDestroyOnLoad(gameObject);
            if (ES3.KeyExists("daysExtended")) daysExtended = ES3.Load<int>("daysExtended");
            else daysExtended = 0;
        }

        [ClientRpc]
        public void ExtendDeadlineClientRpc(int days)
        {
            float before = TimeOfDay.Instance.timeUntilDeadline;
            TimeOfDay.Instance.timeUntilDeadline += TimeOfDay.Instance.totalTime * days;
            TimeOfDay.Instance.UpdateProfitQuotaCurrentTime();
            TimeOfDay.Instance.SyncTimeClientRpc(TimeOfDay.Instance.globalTime, (int)TimeOfDay.Instance.timeUntilDeadline);
            SetDaysExtended(GetDaysExtended() + days);
            if (IsHost || IsServer)
            {
                ES3.Save("daysExtended", daysExtended);
            }
            Plugin.mls.LogDebug($"Previous time: {before}, new time: {TimeOfDay.Instance.timeUntilDeadline}");
        }

        [ServerRpc(RequireOwnership = false)]
        public void ExtendDeadlineServerRpc(int days)
        {
            ExtendDeadlineClientRpc(days);
        }

        internal static int GetTotalCost()
        {
            return Plugin.Config.EXTEND_DEADLINE_PRICE + (Plugin.Config.EXTEND_DEADLINE_ADDITIONAL_PRICE_PER_QUOTA * TimeOfDay.Instance.timesFulfilledQuota);
        }

        internal int GetTotalCostPerDay(int days)
        {
            int daysExtended = GetDaysExtended();
            int totalCost = 0;
            for(int i = 0; i < days; i++)
            {
                totalCost += GetTotalCost() + (daysExtended * Plugin.Config.EXTEND_DEADLINE_ADDITIONAL_PRICE_PER_DAY);
                daysExtended++;
            }
            return totalCost;
        }

        public static int GetDaysExtended()
        {
            return Instance.daysExtended;
        }

        public static void SetDaysExtended(int daysExtended)
        {
            Instance.daysExtended = daysExtended;
        }
    }
}
