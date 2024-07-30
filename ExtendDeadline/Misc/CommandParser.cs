using ExtendDeadline;
using UnityEngine;

namespace ExtendDeadline.Misc
{
    internal static class CommandParser
    {
        const string EXTEND_HELP_COMMAND = ">EXTEND DEADLINE <DAYS>\n" +
            "Extends the deadline by specified amount. Consumes {0} for each day extended and the price is increased by {1} per every quota fullfilled.\n\n";
        private static TerminalNode DisplayTerminalMessage(string message, bool clearPreviousText = true)
        {
            TerminalNode node = ScriptableObject.CreateInstance<TerminalNode>();
            node.displayText = message;
            node.clearPreviousText = clearPreviousText;
            return node;
        }
        public static void ParseCommands(string fullText, ref Terminal terminal, ref TerminalNode outputNode)
        {
            string[] textArray = fullText.Split();
            string firstWord = textArray[0].ToLower();
            string secondWord = textArray.Length > 1 ? textArray[1].ToLower() : "";
            string thirdWord = textArray.Length > 2 ? textArray[2].ToLower() : "";
            switch (firstWord)
            {
                case "extend": outputNode = ExecuteExtendCommands(secondWord, thirdWord, ref terminal, ref outputNode); return;
                default: return;
            }
        }
        private static TerminalNode ExecuteExtendCommands(string secondWord, string thirdWord, ref Terminal terminal, ref TerminalNode outputNode)
        {
            return secondWord switch
            {
                "deadline" => ExecuteExtendDeadlineCommand(thirdWord, ref terminal, ref outputNode),
                _ => outputNode,
            };
        }
        private static TerminalNode ExecuteExtendDeadlineCommand(string thirdWord, ref Terminal terminal, ref TerminalNode outputNode)
        {
            if (!string.IsNullOrEmpty(thirdWord) && thirdWord == "help")
                return DisplayTerminalMessage(string.Format(EXTEND_HELP_COMMAND, Plugin.Config.EXTEND_DEADLINE_PRICE.Value, Plugin.Config.EXTEND_DEADLINE_ADDITIONAL_PRICE_PER_QUOTA.Value));

            return outputNode;
        }
    }
}
