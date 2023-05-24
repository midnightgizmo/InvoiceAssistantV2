using InvoiceAssistantV2.Shared.Models.Database.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.DbInteractions
{
	public class UserAddressDb
	{
		private Data.InvoiceAssistantDbContext _DbContext;
		public UserAddressDb(Data.InvoiceAssistantDbContext dbContext)
		{
			_DbContext = dbContext;
		}

		public UserAddress? Select(int Id)
		{
			return this._DbContext.UserAddress.Where(i => i.Id == Id).FirstOrDefault();
		}
		public UserAddress? SelectByUserDetailsId(int UserDetailsId)
		{
			return this._DbContext.UserAddress.Where(i => i.UserDetailsId == UserDetailsId).FirstOrDefault();
		}

		/// <summary>
		/// Adds the passed in object to the database. On sucsefull insert into the database, the Id property will
		/// be populated in the passed in object
		/// </summary>
		/// <param name="userAddress"></param>
		/// <returns>true if sucsefull, else false</returns>
		public bool Insert(UserAddress userAddress) 
		{
			this._DbContext.UserAddress.Add(userAddress);
			
			if (this._DbContext.SaveChanges() > 0)
				return true;
			else
				return false;
		}

		public bool Update(UserAddress userAddress) 
		{
			this._DbContext.UserAddress.Update(userAddress);
			if (this._DbContext.SaveChanges() > 0)
				return true; 
			else
				return false;

		}

		public bool Delete(int Id) 
		{
			UserAddress? address = this.Select(Id);
			if (address != null) 
			{
				this._DbContext.UserAddress.Remove(address);
				if(this._DbContext.SaveChanges() > 0)
					return true;
				else
					return false;
			}
			else 
				return false;
		}

		public bool DeleteByUserDetailsId(int UserDetailsId) 
		{
			this._DbContext.UserAddress.RemoveRange(
					this._DbContext.UserAddress.Where(i => i.UserDetailsId == UserDetailsId)
					);

			if(this._DbContext.SaveChanges() > 0)
				return true;
			else 
				return false;
		}


	}
}
