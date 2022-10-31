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


        public int HideAllAddressesLinkedToCompany(int CompanyId)
		{
            
            int EffectedRows = _DbContext.Database.ExecuteSqlRaw($"UPDATE {nameof(CompanyAddress)} SET {nameof(CompanyAddress.HasBeenDeleted)} = 1 WHERE id = {CompanyId}");

            return EffectedRows;
        }
	}
}
