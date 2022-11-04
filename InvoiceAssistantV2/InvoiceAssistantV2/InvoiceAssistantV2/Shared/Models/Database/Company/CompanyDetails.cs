using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceAssistantV2.Shared.Models.Database.Company
{
    /// <summary>
    /// A company we have done business with. 
    /// </summary>
    public class CompanyDetails
    {
        /// <summary>
        /// Id in the Database
        /// </summary>
        [Key]
        public int Id { get; set; }
        
        /// <summary>
        /// A friendly name to give the company that will show up in the UI to the user
        /// </summary>
        [Required]
        public string FriendlyName { get; set; } = null!;

        /// <summary>
        /// The name of the company (could just be persons name if not a company)
        /// </summary>
        [Required]       
        public string CompanyName { get; set; } = null!;

        /// <summary>
        /// When user deletes the <see cref="CompanyDetails"/> but the company is referenced to past invoices,
        /// we still need a refernce to the company. Therefore to the user is lookes deleted (by setting to true)
        /// </summary>
        [Required]
        public bool HasBeenDeleted { get; set; } = false;

        public List<CompanyAddress> Venues = null!;

    }
}
