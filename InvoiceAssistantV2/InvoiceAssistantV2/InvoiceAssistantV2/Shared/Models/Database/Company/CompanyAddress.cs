﻿using System.ComponentModel.DataAnnotations;

namespace InvoiceAssistantV2.Shared.Models.Database.Company
{
    /// <summary>
    /// An address that is linked to the <see cref="CompanyDetails"/>
    /// </summary>
    public class CompanyAddress
    {
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// Foreign key link to <see cref="CompanyDetails"/>
        /// </summary>
        public int CompanyAddressId { get; set; }
        /// <summary>
        /// FriendlyName given to the address we can use to display in the UI
        /// </summary>
        [Required]
        public string FriendlyName { get; set; } = null!;
        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? AddressLine3 { get; set; }
        public string? AddressLine4 { get; set; }
        public string? AddressLine5 { get; set; }
        public string? PostCode { get; set; }
        // The driving distance from where we start to the companies address (used for cauclating milage cost)
        public int DrivingDistanceToAddress { get; set; } = 0;
    }
}