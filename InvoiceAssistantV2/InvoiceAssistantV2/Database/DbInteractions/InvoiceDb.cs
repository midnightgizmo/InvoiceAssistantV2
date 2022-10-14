using InvoiceAssistantV2.Shared.Models.Database.Invoice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.DbInteractions
{
    public class InvoiceDb
    {
        private Data.InvoiceAssistantDbContext _DbContext;
        public InvoiceDb(Data.InvoiceAssistantDbContext dbContext)
        {
            _DbContext = dbContext;
        }

        public List<Invoice> SelectAll()
        {
            return this._DbContext.Invoices.ToList();
        }

        public Invoice? SelectInvoice(int InvoiceID)
        {
            Invoice invoice = new Invoice() { Id=InvoiceID };

            return this._DbContext.Invoices.FirstOrDefault(I => I.Id == InvoiceID);
        }

        public List<Invoice> Select_CustomQuery(DateTime? StartDate, DateTime? EndDate,
                                                decimal? StartAmmount, decimal? EndAmmount,
                                                DateTime? DateRecievedMoneyStart, DateTime? DateRecievedMoneyEnd,
                                                string? ReferenceNumber, int? TypeOfPaymentId, int? AddressToMakePaymentOutToId,
                                                string? Description)
        {
            IQueryable<Invoice> query = this._DbContext.Invoices;
            
            if (StartDate != null)
                query = query.Where(I => I.DateOfInvoice >= StartDate);
            if (EndDate != null)
                query = query.Where(I => I.DateOfInvoice <= EndDate);

            if(StartAmmount != null)
                query = query.Where(I => I.TotalInvoiceAmmount >= StartAmmount);
            if(EndAmmount != null)
                query = query.Where(I => I.TotalInvoiceAmmount <= EndAmmount);

            if (DateRecievedMoneyStart != null)
                query = query.Where(I => I.DateRecievedMoney >= DateRecievedMoneyStart);
            if (DateRecievedMoneyEnd != null)
                query = query.Where(I => I.DateRecievedMoney <= DateRecievedMoneyEnd);

            if(ReferenceNumber != null)
                query = query.Where(I => I.ReferenceNumber.Contains(ReferenceNumber, StringComparison.OrdinalIgnoreCase));

            if (TypeOfPaymentId != null)
                query = query.Where(I => I.PaymentTypeID == TypeOfPaymentId);

            if(AddressToMakePaymentOutToId != null)
                query = query.Where(I => I.AddressToMakeInvoiceOutToId == AddressToMakePaymentOutToId);

            if(Description != null)
                query = query.Where(I => I.Description.Contains(Description,StringComparison.OrdinalIgnoreCase));

            return query.ToList();

        }
        public void Update(Invoice InvoiceToUpdate)
        {
            this._DbContext.Update(InvoiceToUpdate);
        }

        public void Insert(Invoice NewInvoice)
        {
            this._DbContext.Add(NewInvoice);
        }

        public void Delete(Invoice InvoiceToDelete)
        {
            this._DbContext.Remove(InvoiceToDelete);
        }
        
    }
}
