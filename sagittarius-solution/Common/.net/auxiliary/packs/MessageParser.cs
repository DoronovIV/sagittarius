using Tools.Formatting;

namespace Common.Packages
{
    public static class MessageParser
    {
        public static bool IsMessageIdenticalToAnotherOne(IMessage messageOne, IMessage messageTwo)
        {
            bool bMessageMatch = false;
            bool bDateMatch = false;
            bool bTimeMatch = false;
            bool bSenderMatch = false;

            bool debugInfoTime = IsTimeAproximatelyEqual(messageOne.GetTime(), messageTwo.GetTime());
            bool debugInfoContent = messageOne.GetMessage().Equals(messageTwo.GetMessage());

            bMessageMatch = debugInfoContent;
            bDateMatch = messageOne.GetDate().Equals(messageTwo.GetDate());
            bTimeMatch = debugInfoTime;
            bSenderMatch = messageOne.GetSender().Equals(messageTwo.GetSender());

            bool result = bMessageMatch && bDateMatch && bTimeMatch && bSenderMatch;

            return result;
        }

        public static bool IsTimeAproximatelyEqual(string timeOne, string timeTwo)
        {
            int nTimeOne = Int32.Parse(StringDateTime.RemoveSeparation(timeOne));
            int nTimeTwo = Int32.Parse(StringDateTime.RemoveSeparation(timeTwo));

            int debugResOne = nTimeOne;
            int debugResTwo = nTimeTwo;

            return debugResOne == debugResTwo;
        }
    }
}
