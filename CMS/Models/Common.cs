using BALibrary.Admin;
using CMS.Data;
using Microsoft.EntityFrameworkCore;
using NuGet.Versioning;
using System;
using System.Diagnostics.CodeAnalysis;

namespace CMS.Models
{
    public class Common
    {
        private static readonly ApplicationDbContext db = new ApplicationDbContext();
        
        public static List<AllRoute> GetAllRoutes(int RoleId)
        {
            List<AllRoute> a = new List<AllRoute>();
            var RoleModules = db.RoleModules.Where(r => r.RoleId == RoleId);
            foreach(var Module in RoleModules)
            {
                AllRoute b = new AllRoute();
                var module = db.Modules.Find(Module.ModuleId);
                b.Name = module.Name;
                
                var moduleTable = db.ModuleTables.Where(m => m.ModuleId == Module.ModuleId);
                foreach(var mt in moduleTable)
                {
                    var moduleException = db.RoleModuleExceptions
                        .FirstOrDefault(r => r.RoleModuleId == Module.Id && r.ModuleTableId == mt.Id);
                    if (moduleException == null || moduleException.FullyGranted || moduleException.Browse)
                    {
                        b.Tables.Add(mt.TableName);                       
                    }
                }
                a.Add(b);
            }
            return a;
        }
        public static bool isAuthorized(int userRoleId, string areaName, string controllerName, string actionName)
        {
            bool pass = false;
            /*var roleModle = db.RoleModules.FirstOrDefault(r => r.RoleId == userRoleId && r.Module.Name == areaName);
            if (roleModle == null)
            {
                pass = false;
                return false;
            }
            var roleModuleExceptipon = db.RoleModuleExceptions.FirstOrDefault(r => r.RoleModuleId == roleModle.Id && r.ModuleTable.TableName == controllerName);
            if(roleModuleExceptipon == null || roleModuleExceptipon.FullyGranted||(roleModuleExceptipon.Browse && actionName.Equals("Index")))
            {
                pass = true;
                return pass;
            }
            if (roleModuleExceptipon.FullyDenied)
            {
                pass = false;
                return pass;
            }
            if ((roleModuleExceptipon.Add && actionName.Equals("Create"))||( roleModuleExceptipon.Read && actionName.Equals("Details")) || (roleModuleExceptipon.Edit && actionName.Equals("Edit")) || (roleModuleExceptipon.Delete && actionName.Equals("Delete")))
            {
                pass = true;
                return pass;
            }*/
                
            return pass;
        }
        public static int TotalAmount(int id)
        {
            int amount;
            var lpd = db.LabPaymentDetails
                     .Where(l => l.LabPaymentId == id);
            if(lpd == null)
            {
                amount = 0;
                return amount;
            }
            amount = lpd.Sum(l => l.Amount);
            return amount;

        }
        public static string? LabDocterFullName(int labDocterId)
        {
            string fullname;
            var labDocter = db.Users.Find(labDocterId);
            if(labDocter == null)
            {
                return null;
            }
            fullname = labDocter.FirstName+ " " + labDocter.LastName;
            return fullname;
        }
        public static int TotalPatientEntriesForLast(int months)
        {
            var startdate = DateTime.Now.Date;
            var enddate = startdate.AddDays(-7);
            if (months > 0)
            {
                startdate = new DateTime(startdate.Year, startdate.Month, 1);

                startdate = startdate.AddMonths(-(months - 1));

                enddate = startdate.AddMonths(-1);                
            }
            else if(months == 0)
            {
                enddate = new DateTime(startdate.Year, startdate.Month, 1);
            }
            var totalSessions = db.Sessions
                           .Where(s =>s.SartDateTime >= enddate && s.SartDateTime <= startdate)
                           .Count();
            return totalSessions;
        }
        public static int GetAllCases(int docterId)
        {
            int session = db.Sessions.Where(s => s.DocterId == docterId).Count();
            return session;
        }
        public static int GetThisMonth(int docterId)
        {
            var startdate = DateTime.Now.Date;
            var enddate = new DateTime(startdate.Year, startdate.Month, 1);
            int session = db.Sessions.Where(s => s.DocterId == docterId
               && s.SartDateTime >= enddate && s.SartDateTime <= startdate)
                .Count();
            return session;
        }
        public class AllRoute
        {
            public string Name;
            public List<string> Tables;
            public AllRoute()
            {
                Tables = new List<string>();
            }
        }
    }    
}
