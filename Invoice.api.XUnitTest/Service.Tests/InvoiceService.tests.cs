using InvoiceAppData.Models;
using FluentAssertions;

using InvoiceApp.Infrastructure.Models;
using InvoiceApp.Api.Services.InvoiceService;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InvoiceApp.Service.DTOS.Invoice;
using InvoiceApp.Infrastructure.Abstracts;




namespace Invocie.api.Tests.Service.Tests
{
    public class InvoiceServiceTests
    {
        private readonly Mock<IGenericRepository<Invoice>> _mockRepository;
        private readonly InvoiceService _invoiceService;
        public InvoiceServiceTests()
        {
            _mockRepository = new();
            _invoiceService = new InvoiceService(_mockRepository.Object);
        }
        [Fact]
        public void addInvoice_successAdded_ReturnSuccess()
        {
            //Arrange
            var Testinvoice = new InvoiceDto
            {
                invoiceid = 1,
                InvoiceNO = 1001,
                InvoiceDate = "2025-02-24",
                nettotal = 500.00m,
                taxes = 50.00m,
                NetAftertaxes = 550.00m,
                storeName = "Main Warehouse",
                storeid = 10,
                invoiceitems = new List<InvoiceitemDto>
                {
                    new InvoiceitemDto
                    {
                        itemid = 101,
                        unitid = 1,
                        price = 100.00m,
                        qty = 2,
                        total = 200.00m,
                        discount = 10.00m,
                        net = 190.00m
                    },
                    new InvoiceitemDto
                    {
                        itemid = 102,
                        unitid = 2,
                        price = 150.00m,
                        qty = 2,
                        total = 300.00m,
                        discount = 15.00m,
                        net = 285.00m
                    }
                }
            };
            //Act   
            _mockRepository.Setup(x => x.Add(It.IsAny<Invoice>()));
           var result = _invoiceService.addInvoice(Testinvoice);
            //Assert
            result.Should().Equals("Success");
        }


        [Fact]
        public void addInvoice_FailedAdded_ReturnFailed()
        {
            //Arrange
            var Testinvoice = new InvoiceDto
            {
                invoiceid = 1,
                InvoiceNO = 1001,
                InvoiceDate = "2025-02-24",
                nettotal = 500.00m,
                taxes = 50.00m,
                NetAftertaxes = 550.00m,
                storeName = "Main Warehouse",
                storeid = 10,
                invoiceitems = new List<InvoiceitemDto>
                {
                    new InvoiceitemDto
                    {
                        itemid = 101,
                        unitid = 1,
                        price = 100.00m,
                        qty = 2,
                        total = 200.00m,
                        discount = 10.00m,
                        net = 190.00m
                    },
                    new InvoiceitemDto
                    {
                        itemid = 102,
                        unitid = 2,
                        price = 150.00m,
                        qty = 2,
                        total = 300.00m,
                        discount = 15.00m,
                        net = 285.00m
                    }
                }
            };
            //Act
            _mockRepository.Setup(x => x.Add(It.IsAny<Invoice>())).Throws(new Exception());
            var result = _invoiceService.addInvoice(Testinvoice);
            //Assert
            result.Should().Equals("Failed");
        }


        [Fact]
        public void GetAll_FetchallInvoices_ReturnInvoices()
        {
            //Arrange
            var TestInvoices =  new List<Invoice>
            {
               new Invoice
               {

                 Id = 1,
                Invoicenumber = 1001,
                Date = new DateTime(2025, 2, 24),
                Total = 500.00m,
                Taxes = 50.00m,
                Net = 550.00m,
                storid = 10,
             
                Invoiceitems = new List<InvoiceItem>
                {
                new InvoiceItem
                {
                    Id = 1,
                    ItemId = 101,
                    UnitId = 1,
                    Price = 100.00m,
                    Quantity = 2,
                    Totalprice = 200.00m,
                    Discount = 10.00m,
                    Net = 190.00m,
                    InvoiceId = 1
                },
                new InvoiceItem
                {
                    Id = 2,
                    ItemId = 102,
                    UnitId = 1,
                    Price = 150.00m,
                    Quantity = 2,
                    Totalprice = 300.00m,
                    Discount = 20.00m,
                    Net = 280.00m,
                    InvoiceId = 1
                }
                }

               },
               new Invoice
               {

                Id = 2,
                Invoicenumber = 15,
                Date = new DateTime(2025, 2, 24),
                Total = 500.00m,
                Taxes = 50.00m,
                Net = 550.00m,
                storid = 10,
                Invoiceitems = new List<InvoiceItem>
                {
                new InvoiceItem
                {
                    Id = 500,
                    ItemId = 101,
                    UnitId = 1,
                    Price = 100.00m,
                    Quantity = 2,
                    Totalprice = 200.00m,
                    Discount = 10.00m,
                    Net = 190.00m,
                    InvoiceId = 1
                },
                new InvoiceItem
                {
                    Id = 2,
                    ItemId = 102,
                    UnitId = 1,
                    Price = 150.00m,
                    Quantity = 2,
                    Totalprice = 300.00m,
                    Discount = 20.00m,
                    Net = 280.00m,
                    InvoiceId = 1
                }
                }

               }    
            };
            //Act
            _mockRepository.Setup(x => x.GetAll()).Returns(TestInvoices.AsQueryable);
            var result = _invoiceService.GetAll();
            //Assert
            result.Should().NotBeNullOrEmpty();
            result.Should().BeOfType<List<InvoiceDto>>();
        }

    }
}
