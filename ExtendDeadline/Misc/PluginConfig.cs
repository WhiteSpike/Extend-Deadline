using BepInEx.Configuration;
using CSync.Extensions;
using CSync.Lib;
using ExtendDeadline.Misc.Util;
using System.Runtime.Serialization;

namespace ExtendDeadline.Misc
{
    [DataContract]
    public class PluginConfig : SyncedConfig2<PluginConfig>
    {
        [field: SyncedEntryField] public SyncedEntry<int> EXTEND_DEADLINE_PRICE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> EXTEND_DEADLINE_ADDITIONAL_PRICE_PER_DAY { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> EXTEND_DEADLINE_ADDITIONAL_PRICE_PER_QUOTA { get; set; }
        public PluginConfig(ConfigFile cfg) : base(Metadata.GUID)
        {
            string topSection = "General";
            EXTEND_DEADLINE_PRICE = cfg.BindSyncedEntry(topSection, Constants.EXTEND_DEADLINE_PRICE_KEY, Constants.EXTEND_DEADLINE_PRICE_DEFAULT, Constants.EXTEND_DEADLINE_PRICE_DESCRIPTION);
            EXTEND_DEADLINE_ADDITIONAL_PRICE_PER_QUOTA = cfg.BindSyncedEntry(topSection, Constants.EXTEND_DEADLINE_ADDITIONAL_PRICE_PER_QUOTA_KEY, Constants.EXTEND_DEADLINE_ADDITIONAL_PRICE_PER_QUOTA_DEFAULT, Constants.EXTEND_DEADLINE_ADDITIONAL_PRICE_PER_QUOTA_DESCRIPTION);
            EXTEND_DEADLINE_ADDITIONAL_PRICE_PER_DAY = cfg.BindSyncedEntry(topSection, Constants.EXTEND_DEADLINE_ADDITIONAL_PRICE_PER_DAY_KEY, Constants.EXTEND_DEADLINE_ADDITIONAL_PRICE_PER_DAY_DEFAULT, Constants.EXTEND_DEADLINE_ADDITIONAL_PRICE_PER_DAY_DESCRIPTION);
        }
    }
}
