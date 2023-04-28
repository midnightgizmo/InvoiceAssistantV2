using InvoiceAssistantV2.Shared.Models.Database.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.DbInteractions
{
	public class UserDetailsDb
	{
		private Data.InvoiceAssistantDbContext _DbContext;
		public UserDetailsDb(Data.InvoiceAssistantDbContext dbContext)
		{
			_DbContext = dbContext;
		}

		/// <summary>
		/// Gets the first row from the database of UserDetails
		/// </summary>
		/// <returns>null if no row exists</returns>
		public UserDetails? SelectUsersDetails()
		{
			return this._DbContext.UserDetails.FirstOrDefault();
		}

		/// <summary>
		/// Updates the databse with the details from the passed in <see cref="UserDetails"/> object.
		/// </summary>
		/// <param name="userDetails">the user details to use to updated the database</param>
		public void UpdateUsersDetails(UserDetails userDetails)
		{
			this._DbContext.UserDetails.Update(userDetails);
			this._DbContext.SaveChanges();
		}

		/// <summary>
		/// Updates a database row with the passed in user details
		/// </summary>
		/// <param name="userDetails">the row to search for and update</param>
		public void InsertUserDetails(UserDetails userDetails) 
		{
			this._DbContext.Add(userDetails);
			this._DbContext.SaveChanges();
		}

	}
}
