using InvoiceAssistantV2.Shared.Models.Database.Company;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.DbInteractions
{
	public class CompanyAddressDb
	{

        private Data.InvoiceAssistantDbContext _DbContext;
        public CompanyAddressDb(Data.InvoiceAssistantDbContext dbContext)
        {
            _DbContext = dbContext;
        }

        /// <summary>
        /// Selects all address that belong to the company that have not been hidden
        /// </summary>
        /// <param name="CompanyId"></param>
        /// <returns></returns>
        public List<CompanyAddress> SelectAllAddressForCompany(int CompanyId)
        {
            return this._DbContext.CompanyAddress.Where(a => a.CompanyDetailsID == CompanyId && a.HasBeenDeleted == false)
                                                 .OrderBy(o => o.FriendlyName)
                                                 .ToList();
        }

        /// <summary>
        /// Find the Compandy Address Details for the specifed ID.
        /// </summary>
        /// <param name="companyAddressId"></param>
        /// <returns>null if could not be found</returns>
        public CompanyAddress? Select(int companyAddressId)
        {
            return this._DbContext.CompanyAddress.Where(a => a.Id == companyAddressId).FirstOrDefault();
        }

        /// <summary>
        /// Adds a new Company address to the database
        /// </summary>
        /// <param name="AddressToAdd"></param>
        /// <returns>null if adding fails</returns>
        public CompanyAddress? AddNewCompanyAddress(CompanyAddress AddressToAdd)
        {
            // copy of the address details into a new object (so we don't effect the passed in object
            CompanyAddress? newCompanyAddress = new CompanyAddress()
            {
                CompanyDetailsID = AddressToAdd.CompanyDetailsID,
                HasBeenDeleted = AddressToAdd.HasBeenDeleted,
                FriendlyName = AddressToAdd.FriendlyName,
                AddressLine1 = AddressToAdd.AddressLine1,
                AddressLine2 = AddressToAdd.AddressLine2,
                AddressLine3 = AddressToAdd.AddressLine3,
                AddressLine4 = AddressToAdd.AddressLine4,
                AddressLine5 = AddressToAdd.AddressLine5,
                PostCode = AddressToAdd.PostCode,
                DrivingDistanceToAddress = AddressToAdd.DrivingDistanceToAddress,
            };

            this._DbContext.CompanyAddress.Add(newCompanyAddress);
            this._DbContext.SaveChanges();
            // if the add was sucsefull, return the new address details, else return null
            return newCompanyAddress.Id > 0? newCompanyAddress : null;


        }

        public CompanyAddress? EditCompanyAddressDetails(CompanyAddress companyAddressDetails)
        {
            CompanyAddress? address = this.Select(companyAddressDetails.Id);
            if (address == null)
                return null;

            address.FriendlyName = companyAddressDetails.FriendlyName;
            address.DrivingDistanceToAddress = companyAddressDetails.DrivingDistanceToAddress;
            address.AddressLine1 = companyAddressDetails.AddressLine1;
            address.AddressLine2 = companyAddressDetails.AddressLine2;
            address.AddressLine3 = companyAddressDetails.AddressLine3;
            address.AddressLine4 = companyAddressDetails.AddressLine4;
            address.AddressLine5 = companyAddressDetails.AddressLine5;
            address.PostCode = companyAddressDetails.PostCode;

            this._DbContext.SaveChanges();
            
            return address;
        }

        public int HideAllAddressesLinkedToCompany(int CompanyId)
		{
            
            int EffectedRows = _DbContext.Database.ExecuteSqlRaw($"UPDATE {nameof(CompanyAddress)} SET {nameof(CompanyAddress.HasBeenDeleted)} = 1 WHERE id = {CompanyId}");

            return EffectedRows;
        }

        /// <summary>
        /// Checks if the CompanyAddressId passed in is linked to an invoices.
        /// </summary>
        /// <param name="CompanyAddressId"></param>
        /// <returns>true if Company Address linked to one or more invoices, else false</returns>
        public bool IsCompanyAddressLinkedToAnyInvoices(int CompanyAddressId)
        {
            return this._DbContext.Invoices.Where(i => i.AddressToMakeInvoiceOutToId == CompanyAddressId).Any();
        }

        /// <summary>
        /// Hide the Company Address so the user can't see it anymore
        /// </summary>
        /// <param name="CompanyAddressId"></param>
        /// <returns>true if sucsefull, else false</returns>
        public bool HideCompanyAddress(int CompanyAddressId)
        {

            int EffectedRows = _DbContext.Database.ExecuteSqlRaw($"UPDATE {nameof(CompanyAddress)} SET {nameof(CompanyAddress.HasBeenDeleted)} = 1 WHERE id = {CompanyAddressId}");

            // return true if effected rows is greater than zero, else false
            return EffectedRows > 0 ? true : false;
        }

        /// <summary>
        /// Deletes the Company Address, from the database.
        /// </summary>
        /// <param name="CompanyId"></param>
        /// <returns>true if delete sucsefull, else false</returns>
        public bool DeleteCompanyAddress(int CompanyAddressId)
        {
            // find the company address we want to delete
            var companyAddressToDelete = _DbContext.CompanyAddress.Where(a => a.Id == CompanyAddressId).FirstOrDefault();
            // make sure we found it
            if (companyAddressToDelete != null)
            {
                // delete it from the database
                _DbContext.CompanyAddress.Remove(companyAddressToDelete);
                _DbContext.SaveChanges();
                return true;
            }
            // we could not find the company so could not remove it
            else
                return false;

        }

        
    }
}
