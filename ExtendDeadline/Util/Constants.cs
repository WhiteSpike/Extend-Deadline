namespace ExtendDeadline.Misc.Util
{
    internal static class Constants
    {
        internal const string EXTEND_DEADLINE_PRICE_KEY = $"{ExtendDeadlineBehaviour.COMMAND_NAME} Price";
        internal const int EXTEND_DEADLINE_PRICE_DEFAULT = 800;
        internal const string EXTEND_DEADLINE_PRICE_DESCRIPTION = "Price of each day extension requested in the terminal.";

        internal const string EXTEND_DEADLINE_ADDITIONAL_PRICE_PER_QUOTA_KEY = $"{ExtendDeadlineBehaviour.COMMAND_NAME} Additional Cost per Quota";
        internal const int EXTEND_DEADLINE_ADDITIONAL_PRICE_PER_QUOTA_DEFAULT = 0;
        internal const string EXTEND_DEADLINE_ADDITIONAL_PRICE_PER_QUOTA_DESCRIPTION = "Additional cost added to the Extend Deadline command per every quota completed";

        internal const string EXTEND_DEADLINE_ADDITIONAL_PRICE_PER_DAY_KEY = $"{ExtendDeadlineBehaviour.COMMAND_NAME} Additional Cost per Day";
        internal const int EXTEND_DEADLINE_ADDITIONAL_PRICE_PER_DAY_DEFAULT = 0;
        internal const string EXTEND_DEADLINE_ADDITIONAL_PRICE_PER_DAY_DESCRIPTION = "Additional cost added to the Extend Deadline command per every day extended";

        internal const string NOT_ENOUGH_CREDITS_EXTEND = "Not enough credits to purchase the selected amount of days to extend.";
        internal const string PURCHASE_EXTEND_DEADLINE_FORMAT = "Do you wish to purchase {0} days to extend the deadline for the cost of {1} credits?";
    }
}
