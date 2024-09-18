using ExtendDeadline.Misc.Util;
using InteractiveTerminalAPI.UI;
using InteractiveTerminalAPI.UI.Application;
using InteractiveTerminalAPI.UI.Cursor;
using InteractiveTerminalAPI.UI.Screen;
using System;

namespace ExtendDeadline.Misc.UI.Application
{
    internal class ExtendDeadlineApplication : InteractiveCounterApplication<CursorCounterMenu, CursorCounterElement>
    {
        public override void Initialization()
        {
            CursorOutputElement<string>[] cursorCounterElements = new CursorOutputElement<string>[1];
            Func<int, string>[] array = new Func<int, string>[1];
            CursorCounterMenu cursorCounterMenu = CursorCounterMenu.Create(0, '>', cursorCounterElements);
            IScreen screen = BoxedOutputScreen<string, string>.Create(ExtendDeadlineBehaviour.COMMAND_NAME, [cursorCounterMenu], () => cursorCounterElements[0].ApplyFunction(), (string x) => x);
            for (int i = 0; i < cursorCounterElements.Length; i++)
            {
                int counter = i;
                array[i] = (int x) => $"${ExtendDeadlineBehaviour.Instance.GetTotalCostPerDay(x)}";
                cursorCounterElements[i] = CursorOutputElement<string>.Create(name: "Days to extend",
                                                                            description: "",
                                                                            action: () => TryPurchaseExtendedDays(cursorCounterElements[counter], backAction: () => SwitchScreen(screen, cursorCounterMenu, previous: true)),
                                                                            counter: 0,
                                                                            func: array[counter]);
            }

            currentCursorMenu = cursorCounterMenu;
            currentScreen = screen;
        }

        void TryPurchaseExtendedDays<T>(CursorOutputElement<T> element, Action backAction)
        {
            int days = element.Counter;
            int totalCost = ExtendDeadlineBehaviour.Instance.GetTotalCostPerDay(days);
            if (terminal.groupCredits < totalCost)
            {
                ErrorMessage(ExtendDeadlineBehaviour.COMMAND_NAME, Constants.NOT_ENOUGH_CREDITS_EXTEND, backAction, "");
                return;
            }
            Confirm(ExtendDeadlineBehaviour.COMMAND_NAME, string.Format(Constants.PURCHASE_EXTEND_DEADLINE_FORMAT, days, totalCost), () => PurchaseExtendedDays(days, totalCost, backAction), backAction);
        }
        void PurchaseExtendedDays(int days, int totalCost, Action backAction)
        {
            if (terminal.IsServer)
            {
                ExtendDeadlineBehaviour.Instance.ExtendDeadlineClientRpc(days);
                terminal.SyncGroupCreditsClientRpc(terminal.groupCredits - totalCost, terminal.numberOfItemsInDropship);
            }
            else
            {
                ExtendDeadlineBehaviour.Instance.ExtendDeadlineServerRpc(days);
                terminal.BuyItemsServerRpc([], terminal.groupCredits - totalCost, terminal.numberOfItemsInDropship);
            }
            backAction();
        }
        protected void Confirm(string title, string description, Action confirmAction, Action declineAction, string additionalMessage = "")
        {
            CursorCounterElement[] elements =
            [
            CursorCounterElement.Create("Confirm", "", confirmAction, showCounter: false),
            CursorCounterElement.Create("Abort", "", declineAction, showCounter: false)
            ];
            CursorCounterMenu cursorMenu = CursorCounterMenu.Create(0, '>', elements);
            ITextElement[] elements2 =
            [
            TextElement.Create(description),
            TextElement.Create(" "),
            TextElement.Create(additionalMessage),
            cursorMenu
            ];
            IScreen screen = BoxedScreen.Create(title, elements2);
            SwitchScreen(screen, cursorMenu, previous: false);
        }
        protected void ErrorMessage(string title, string description, Action backAction, string error)
        {
            CursorCounterElement[] elements = [CursorCounterElement.Create("Back", "", backAction, showCounter: false)];
            CursorCounterMenu cursorMenu = CursorCounterMenu.Create(0, '>', elements);
            ITextElement[] elements2 =
            [
            TextElement.Create(description),
            TextElement.Create(" "),
            TextElement.Create(error),
            TextElement.Create(" "),
            cursorMenu
            ];
            IScreen screen = BoxedScreen.Create(title, elements2);
            SwitchScreen(screen, cursorMenu, previous: false);
        }
    }
}
