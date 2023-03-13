using Database;
using Database.Data;
using Database.DbInteractions;
using InvoiceAssistantV2.Shared.Models.Database.Invoice;
using Microsoft.EntityFrameworkCore;

namespace Test
{
    [TestClass]
    public class DatabaseUnitTest
    {

        [AssemblyInitialize]
        public static void AssemblyInit(TestContext context)
        {
            // Executes once before the test run. (Optional)

            // set the evironment as Development.
            // we need to do this so we load the correct database location from the appsettings.Development.json file
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");
        }

        [TestCategory("Invoice"), TestMethod()]
        public void AddInvoice()
        {
            // tests to see if we can add a new invoice to the database
            
            DatabaseInteraction dbInteraction = new DatabaseInteraction();


            Invoice anInvoice = new Invoice()
            {
                DateOfInvoice = DateTime.Now,
                Description = "Anohter Invoice",
                PaymentType = dbInteraction.DbContext.PaymentTypes.ToList()[2],
                ReferenceNumber = "15102201",
                TotalInvoiceAmmount = 45.54M
            };

            dbInteraction.Invoices.Insert(anInvoice);
            dbInteraction.SaveChanges();

            // if the ID is greater than zero, the invoice has been added
            Assert.IsTrue(anInvoice.Id > 0);
            
            
        }

        [TestCategory("Invoice"), TestMethod()]
        public void SearchForInvoices()
        {
            DatabaseInteraction dbInteraction = new DatabaseInteraction();

			//string ReferenceNumber = "14102201";

			//List<Invoice> ListOfInvoices = dbInteraction.Invoices.Select_CustomQuery(null, null, null, null, null, null, ReferenceNumber, null, null, null);

			DateTime StartDate = DateTime.Now.Subtract(new TimeSpan(100,0,0,0,0));

			List<Invoice> ListOfInvoices = dbInteraction.Invoices.Select_CustomQuery(StartDate, null, null, null, null, null, null, null, null, null);


			Assert.IsTrue(ListOfInvoices.Count > 0);

        }

		[TestCategory("Invoice"), TestMethod()]
		public void FindPlacesVisitedForInvoice()
        {
            InvoiceAssistantDbContext dbContext= new InvoiceAssistantDbContext();
            InvoiceDb invoiceDb = new InvoiceDb(dbContext);
            Invoice anInvoice = invoiceDb.SelectAll()[0];

            PlacesVisitedForInvoiceDb placesVisited = new PlacesVisitedForInvoiceDb(dbContext);
            List<PlacesVisitedForInvoice> PlacesVisitedList;
			PlacesVisitedList = placesVisited.SelectAllPlacesVisitedForInvoice(anInvoice.Id);

            Assert.IsTrue(PlacesVisitedList != null);
        }
    }
}