//using InvoiceAPI.Repositories.InvocieItemRepository;
//using WepApp.Models;

//namespace InvoiceAPI.Services.InvocieitemService
//{
//    public class invoiceitemService : IinvoiceitemService
//    {
//        private readonly IinvoiceitemRepository invoiceitemRepo;

//        public invoiceitemService(IinvoiceitemRepository invoiceitemRepo)
//        {
//            this.invoiceitemRepo = invoiceitemRepo;
//        }

//        public string update(InvoiceItem invoiceItem)
//        {
//            try
//            {

//                invoiceitemRepo.Update(invoiceItem);
//                invoiceitemRepo.Save();
//                return "Yes";
//            }
//            catch (Exception ex)
//            {
//                return "Not updated";
//            }
//        }
//    }
//}
