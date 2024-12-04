namespace POE.Models
{
    public class LecturerClaim
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string LecturerName { get; set; }
        public decimal HoursWorked { get; set; }
        public decimal HourlyRate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Notes { get; set; }
        public DateTime SubmissionDate { get; set; }
        public string Status { get; set; } = "Pending"; // Default status
        public string? DocumentPath { get; set; }  // For file upload
        }
}
