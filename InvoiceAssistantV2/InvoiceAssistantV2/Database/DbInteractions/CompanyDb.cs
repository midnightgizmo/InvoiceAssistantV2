using InvoiceAssistantV2.Shared.Models.Database.Company;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.DbInteractions
{
    public class CompanyDb
    {
        private Data.InvoiceAssistantDbContext _DbContext;
        public CompanyDb(Data.InvoiceAssistantDbContext dbContext)
        {
            _DbContext = dbContext;
        }

        /// <summary>
        /// Get a list of all the companys in the database
        /// </summary>
        /// <param name="IncludeHiddenOnes">true if also want hidden ones, else false</param>
        /// <returns></returns>
        public List<CompanyDetails> SelectAllCompanies(bool IncludeHiddenOnes)
        {
            if (IncludeHiddenOnes)
                return this._DbContext.CompanyDetails.OrderBy(c => c.FriendlyName.ToLower()).ToList();
            else
                return this._DbContext.CompanyDetails.Where(c => c.HasBeenDeleted == false).OrderBy(c => c.FriendlyName.ToLower()).ToList();
        }


        /// <summary>
        /// Returns a single <see cref="CompanyDetails"/> or null if not found
        /// </summary>
        /// <param name="CompanyId">The Id used to find the record in the database</param>
        /// <returns><see cref="CompanyDetails"/> or null</returns>
        public CompanyDetails? Select(int CompanyId)
        {
            return this._DbContext.CompanyDetails.Where(c => c.Id == CompanyId).FirstOrDefault();
        }


        /// <summary>
        /// Checks if the CompanyId passed in is linked to an invoices.
        /// </summary>
        /// <param name="CompanyId"></param>
        /// <returns>true if Compnay linked to one or more invoices, else false</returns>
        public bool IsCompanyAddressLinkedToAnyInvoices(int CompanyId)
        {
            var result = (from i in _DbContext.Invoices
                          join a in _DbContext.CompanyAddress
                          on i.AddressToMakeInvoiceOutToId equals a.Id
                          join c in _DbContext.CompanyDetails
                          on a.CompanyDetailsID equals c.Id
                          where c.Id == CompanyId
                          select i).FirstOrDefault();

            // return falise if null, else true
            return result == null ? false : true;
        }

        /// <summary>
        /// Hide the Company so the user can't see it anymore
        /// </summary>
        /// <param name="CompanyId"></param>
        /// <returns>true if sucsefull, else false</returns>
        public bool HideCompany(int CompanyId)
        {
            
            int EffectedRows = _DbContext.Database.ExecuteSqlRaw($"UPDATE {nameof(CompanyDetails)} SET {nameof(CompanyDetails.HasBeenDeleted)} = 1 WHERE id = {CompanyId}");

            // return true if effected rows is greater than zero, else false
            return EffectedRows > 0 ? true : false;
        }

        public CompanyDetails? AddNewCompany(string FriendlyName, string CompanyName)
        {
            CompanyDetails companyDetails = new CompanyDetails();
            companyDetails.FriendlyName = FriendlyName;
            companyDetails.CompanyName = CompanyName;

            this._DbContext.CompanyDetails.Add(companyDetails);
            this._DbContext.SaveChanges();

            return companyDetails.Id > 0 ? companyDetails : null;
        }


        public CompanyDetails? EditCompanyDetails(CompanyDetails companyDetails)
        {
            CompanyDetails? company = this.Select(companyDetails.Id);
            if (company == null)
                return null;

            company.FriendlyName = companyDetails.FriendlyName;
            company.CompanyName = companyDetails.CompanyName;
            company.HasBeenDeleted = companyDetails.HasBeenDeleted;

            this._DbContext.SaveChanges();

            return company;
        }

        /// <summary>
        /// Deletes the Company Details, and any assoshiated Company addrsses from the database.
        /// </summary>
        /// <param name="CompanyId"></param>
        /// <returns>true if delete sucsefull, else false</returns>
        public bool DeleteCompany(int CompanyId)
        {
            // find the company we want to delete
            var companyToDelete = _DbContext.CompanyDetails.Where(c => c.Id == CompanyId).FirstOrDefault();
            // make sure we found it
            if (companyToDelete != null)
            {
                // delete it from the database
                _DbContext.CompanyDetails.Remove(companyToDelete);
                _DbContext.SaveChanges();
                return true;
            }
            // we could not find the company so could not remove it
            else
                return false;
            
        }

    }
}
