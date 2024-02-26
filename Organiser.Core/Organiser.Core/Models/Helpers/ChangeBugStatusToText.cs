using Organiser.Cores.Models.Enums;

namespace Organiser.Cores.Models.Helpers
{
    public class ChangeBugStatusToText
    {
        public ChangeBugStatusToText() { }

        public static string BugStatusText(BugStatusEnum status)
        {
            var statuses = new Dictionary<BugStatusEnum, string>()
            {
                { BugStatusEnum.New, "Nowy"},
                { BugStatusEnum.InVerification, "W weryfikacji"},
                { BugStatusEnum.Rejected, "Odrzucony"},
                { BugStatusEnum.Accepted, "Zaakceptowany"},
                { BugStatusEnum.InDevelopment, "W naprawie"},
                { BugStatusEnum.Fixed, "Naprawiony"},
            };

            var statusMessage = statuses.ContainsKey(status) ? statuses[status] : "";

            return statusMessage;
        }
    }
}
