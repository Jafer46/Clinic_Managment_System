using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CMS.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            var configuration = builder.Build();
            optionsBuilder.UseSqlServer(configuration["ConnectionStrings:DefaultConnection"]);
        }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public ApplicationDbContext()
        {
        }

        public DbSet<BALibrary.Admin.Role> Roles { get; set; }
        public DbSet<BALibrary.Admin.Module> Modules { get; set; }
        public DbSet<BALibrary.Admin.ModuleTable> ModuleTables{ get; set; }
        public DbSet<BALibrary.Admin.RoleModule> RoleModules { get; set; }
        public DbSet<BALibrary.Admin.RoleModuleException> RoleModuleExceptions{ get; set; }

        public DbSet<BALibrary.Patient.PatientList> Patients { get; set; }
        public DbSet<BALibrary.Patient.PatientRecord> PatientRecords { get; set; }
        public DbSet<BALibrary.Patient.RecordSummary> RecordSummaries { get; set; }

        public DbSet<BALibrary.User.User> Users { get; set; }

        public DbSet<BALibrary.Payment.CardPayment> CardPayment { get; set; }
        public DbSet<BALibrary.Payment.LabPayment> LabPayments { get; set; }
        public DbSet<BALibrary.Payment.LabPaymentDetail> LabPaymentDetails { get; set; }

        public DbSet<BALibrary.Prescription.Prescription> Prescription { get; set; }
        public DbSet<BALibrary.Prescription.PrescriptionDetail> PrescriptionDetails { get; set; }
        public DbSet<BALibrary.Triage.PatientTriage> PatientTriages { get; set; }
        public DbSet<BALibrary.Triage.TriageDetail> TriageDetails { get; set; }

        public DbSet<BALibrary.Other.SessionStatus> SessionStatuses { get; set; }

        public DbSet<BALibrary.Medical.Session> Sessions { get; set; }
        public DbSet<BALibrary.Medical.LabRequest> LabRequests { get; set; }
        public DbSet<BALibrary.Medical.LabRequestDetail> LabRequestDetails { get; set; }
        public DbSet<BALibrary.Medical.LabReport> LabReports { get; set; }
        public DbSet<BALibrary.Medical.LabReportDetail> LabReportDetails { get; set; }



    }
}