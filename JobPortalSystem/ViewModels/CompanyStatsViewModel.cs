namespace JobPortalSystem.ViewModels
{
    public class CompanyStatsViewModel
    {
        public string CompanyName { get; set; }
        public int TotalJobs { get; set; }
        public int ActiveJobs { get; set; }
        public int ClosedJobs { get; set; }
        public int TotalApplicants { get; set; }
        public int AcceptedApplicants { get; set; }
        public int PendingApplicants { get; set; }
        public int RejectedApplicants { get; set; }
    }
}
