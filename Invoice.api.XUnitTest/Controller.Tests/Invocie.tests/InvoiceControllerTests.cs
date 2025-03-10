using InvoiceApp.Api.Services.InvoiceService;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InvoiceApp.Api.Controllers;
using FluentAssertions;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Invocie.api.Tests.TestModels;
using InvoiceApp.Service.DTOS.Invoice;
[assembly:CollectionBehavior(collectionBehavior:CollectionBehavior.CollectionPerClass,MaxParallelThreads =6)]
namespace Invocie.api.Tests.Controller.Tests.Invocie.tests
{
    public class InvoiceControllerTests
    {
      
        private readonly Mock<IinvoiceService> _invoiceServiceMock;
        public InvoiceControllerTests()
        {
            _invoiceServiceMock = new();

        }

        [Theory]
        //[InlineData(1)]
        [MemberData(nameof(PassDatausingMemberData.PassDataToSuccess),MemberType = typeof(PassDatausingMemberData))]
        public void GetById_GetExistinvoice_returnstatuscode200(int id)
        {
            //Arrange
            var testInvoices = new List<InvoiceDto>
                    {
                        new InvoiceDto
                        {
                            invoiceid = 1,
                            InvoiceNO = 1001,
                            InvoiceDate = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                            nettotal = 500.00m,
                            taxes = 50.00m,
                            NetAftertaxes = 450.00m,
                            storeName = "Tech Store",
                            storeid = 10

                        },
                        new InvoiceDto
                        {
                            invoiceid = 2,
                            InvoiceNO = 1002,
                            InvoiceDate = DateTime.UtcNow.AddDays(-1).ToString("yyyy-MM-ddTHH:mm:ssZ"),
                            nettotal = 1200.00m,
                            taxes = 100.00m,
                            NetAftertaxes = 1100.00m,
                            storeName = "Gadget World",
                            storeid = 20
                        }
                    };

            _invoiceServiceMock.Setup(x=>x.GetById(id)).Returns(testInvoices.FirstOrDefault(i=>i.invoiceid==id));
            var invoice = new InvoiceController(_invoiceServiceMock.Object);

            //Act 
           // var query = invoice.GetById(id);
            var result = invoice.GetById(id) as ObjectResult;
            //Assert 
            result.StatusCode.Should().Be((int)HttpStatusCode.OK);
            result.Should().NotBeNull();
          //  result.Should().BeEquivalentTo(testInvoiceDto);


        
        }

        
        [Theory]
        [MemberData(nameof(PassDatausingMemberData.PassDatatofailed), MemberType = typeof(PassDatausingMemberData))]
        //[InlineData(3)]
        public void GetById_GetNotExistinvoice_returnstatuscode404NotFound(int id)
        {
            //Arrange
            var testInvoices = new List<InvoiceDto>
                    {
                        new InvoiceDto
                        {
                            invoiceid = 1,
                            InvoiceNO = 1001,
                            InvoiceDate = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                            nettotal = 500.00m,
                            taxes = 50.00m,
                            NetAftertaxes = 450.00m,
                            storeName = "Tech Store",
                            storeid = 10
       
                        },
                        new InvoiceDto
                        {
                            invoiceid = 2,
                            InvoiceNO = 1002,
                            InvoiceDate = DateTime.UtcNow.AddDays(-1).ToString("yyyy-MM-ddTHH:mm:ssZ"),
                            nettotal = 1200.00m,
                            taxes = 100.00m,
                            NetAftertaxes = 1100.00m,
                            storeName = "Gadget World",
                            storeid = 20
                        }
                    };


            _invoiceServiceMock.Setup(x => x.GetById(id)).Returns(testInvoices.FirstOrDefault(i=>i.invoiceid==id));
            var invoice = new InvoiceController(_invoiceServiceMock.Object);

            //Act 
            var result = invoice.GetById(id);
            //Assert 
            result.Should().BeOfType<NotFoundResult>();
            (result as NotFoundResult).StatusCode.Should().Be((int)HttpStatusCode.NotFound);



         }


        [Fact]
        public void AddInvoice_SuccessAdded_returnstatuscode200()
        {
            //Arrange
            var testInvoices = new InvoiceDto


            {
                invoiceid = 1,
                InvoiceNO = 1001,
                InvoiceDate = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                nettotal = 500.00m,
                taxes = 50.00m,
                NetAftertaxes = 450.00m,
                storeName = "Tech Store",
                storeid = 10

            };
                    


            _invoiceServiceMock.Setup(x => x.addInvoice(It.IsAny<InvoiceDto>())).Returns("Success");
            var invoice = new InvoiceController(_invoiceServiceMock.Object);

            //Act 
            var result =invoice.Add(testInvoices) as OkResult;
            //Assert 

            result.StatusCode.Should().Be((int)HttpStatusCode.OK);
            _invoiceServiceMock.Verify(x=>x.addInvoice(It.IsAny<InvoiceDto>()), Times.Once(),"called More than One");



        }

        [Fact]
        public void AddInvoice_FailedAdded_returnstatuscode400()
        {
            //Arrange
            var testInvoices = new InvoiceDto


            {
                invoiceid = 1,
                InvoiceNO = 1001,
                InvoiceDate = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                nettotal = 500.00m,
                taxes = 50.00m,
                NetAftertaxes = 450.00m,
                storeName = "Tech Store",
                storeid = 10

            };



            _invoiceServiceMock.Setup(x => x.addInvoice(It.IsAny<InvoiceDto>())).Returns("Failed");
            var invoice = new InvoiceController(_invoiceServiceMock.Object);

            //Act 
            var result = invoice.Add(testInvoices) as BadRequestResult;
            //Assert 

            result.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
            _invoiceServiceMock.Verify(x => x.addInvoice(It.IsAny<InvoiceDto>()), Times.Once(), "called More than One");



        }


        [Fact]
        public void UpdateInvoice_SuccessUpdated_returnstatuscode204()
        {
            //Arrange
            var testInvoices = new InvoiceDto


            {
                invoiceid = 1,
                InvoiceNO = 1001,
                InvoiceDate = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                nettotal = 500.00m,
                taxes = 50.00m,
                NetAftertaxes = 450.00m,
                storeName = "Tech Store",
                storeid = 10

            };



            _invoiceServiceMock.Setup(x => x.Update(It.IsAny<InvoiceDto>())).Returns(true);
            var invoice = new InvoiceController(_invoiceServiceMock.Object);

            //Act 
            var result = invoice.UpdateInvoice(testInvoices) as NoContentResult ;
            //Assert 

            result.StatusCode.Should().Be((int)HttpStatusCode.NoContent);
            _invoiceServiceMock.Verify(x => x.Update(It.IsAny<InvoiceDto>()), Times.Once(), "called More than One");



        }
        [Fact]
        public void UpdateInvoice_SendModelNull_returnstatuscode204()
        {
            //Arrange
            InvoiceDto? testInvoices = null;



           // _invoiceServiceMock.Setup(x => x.Update(It.IsAny<InvoiceDto>())).Returns(true);
            var invoice = new InvoiceController(_invoiceServiceMock.Object);

            //Act 
            var result = invoice.UpdateInvoice(testInvoices) as BadRequestResult;
            //Assert 

            result.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
            _invoiceServiceMock.Verify(x => x.Update(It.IsAny<InvoiceDto>()), Times.Never, "called update");



        }

        [Fact]
        public void UpdateInvoice_NotfoundInvoiceoranException_returnstatuscode404()
        {
            //Arrange
            var testInvoices = new InvoiceDto


            {
                invoiceid = 1,
                InvoiceNO = 1001,
                InvoiceDate = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                nettotal = 500.00m,
                taxes = 50.00m,
                NetAftertaxes = 450.00m,
                storeName = "Tech Store",
                storeid = 10

            };



            _invoiceServiceMock.Setup(x => x.Update(It.IsAny<InvoiceDto>())).Returns(false);
            var invoice = new InvoiceController(_invoiceServiceMock.Object);

            //Act 
            var result = invoice.UpdateInvoice(testInvoices) as NotFoundResult;
            //Assert 

            result.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
            _invoiceServiceMock.Verify(x => x.Update(It.IsAny<InvoiceDto>()), Times.Once(), "called More than One");



        }


         [Theory]
        [MemberData(nameof(PassDatausingMemberData.PassDataToSuccess), MemberType = typeof(PassDatausingMemberData))]
        public void DeleteInvoice_SuccessDeleted_returnstatuscode204(int id)
        {
            //Arrange

            var testInvoices = new List<InvoiceDto>
         {
             new InvoiceDto
             {
                 invoiceid = 1,
                 InvoiceNO = 1001,
                 InvoiceDate = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                 nettotal = 500.00m,
                 taxes = 50.00m,
                 NetAftertaxes = 450.00m,
                 storeName = "Tech Store",
                 storeid = 10

             },
             new InvoiceDto
             {
                 invoiceid = 2,
                 InvoiceNO = 1002,
                 InvoiceDate = DateTime.UtcNow.AddDays(-1).ToString("yyyy-MM-ddTHH:mm:ssZ"),
                 nettotal = 1200.00m,
                 taxes = 100.00m,
                 NetAftertaxes = 1100.00m,
                 storeName = "Gadget World",
                 storeid = 20
             }
         };


            _invoiceServiceMock.Setup(x => x.Delete(id)).Returns(true);
            var invoice = new InvoiceController(_invoiceServiceMock.Object);

            //Act 
            var result = invoice.DeleteInvoice(id) as NoContentResult;
            //Assert 

            result.StatusCode.Should().Be((int)HttpStatusCode.NoContent);
            _invoiceServiceMock.Verify(x => x.Delete(id), Times.Once(), "called More than One");



        }




        [Theory]
        [MemberData(nameof(PassDatausingMemberData.PassDatatofailed), MemberType = typeof(PassDatausingMemberData))]
        public void DeleteInvoice_FailedDeleted_returnstatuscode404(int id)
        {
            //Arrange

            var testInvoices = new List<InvoiceDto>
         {
             new InvoiceDto
             {
                 invoiceid = 1,
                 InvoiceNO = 1001,
                 InvoiceDate = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                 nettotal = 500.00m,
                 taxes = 50.00m,
                 NetAftertaxes = 450.00m,
                 storeName = "Tech Store",
                 storeid = 10

             },
             new InvoiceDto
             {
                 invoiceid = 2,
                 InvoiceNO = 1002,
                 InvoiceDate = DateTime.UtcNow.AddDays(-1).ToString("yyyy-MM-ddTHH:mm:ssZ"),
                 nettotal = 1200.00m,
                 taxes = 100.00m,
                 NetAftertaxes = 1100.00m,
                 storeName = "Gadget World",
                 storeid = 20
             }
         };


            _invoiceServiceMock.Setup(x => x.Delete(id)).Returns(false);
            var invoice = new InvoiceController(_invoiceServiceMock.Object);

            //Act 
            var result = invoice.DeleteInvoice(id) as NotFoundResult;

            //Assert 

            result.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
            _invoiceServiceMock.Verify(x => x.Delete(id), Times.Once(), "called More than One");



        }


        [Fact]
        public void GetAll_Fetchalllsuccess_returnstatuscode200()
        {
            //Arrange

            var testInvoices = new List<InvoiceDto>
         {
             new InvoiceDto
             {
                 invoiceid = 1,
                 InvoiceNO = 1001,
                 InvoiceDate = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                 nettotal = 500.00m,
                 taxes = 50.00m,
                 NetAftertaxes = 450.00m,
                 storeName = "Tech Store",
                 storeid = 10

             },
             new InvoiceDto
             {
                 invoiceid = 2,
                 InvoiceNO = 1002,
                 InvoiceDate = DateTime.UtcNow.AddDays(-1).ToString("yyyy-MM-ddTHH:mm:ssZ"),
                 nettotal = 1200.00m,
                 taxes = 100.00m,
                 NetAftertaxes = 1100.00m,
                 storeName = "Gadget World",
                 storeid = 20
             }
         };


            _invoiceServiceMock.Setup(x => x.GetAll()).Returns(testInvoices);
            var invoice = new InvoiceController(_invoiceServiceMock.Object);

            //Act 
            var result = invoice.GetAll() as ObjectResult;

            //Assert 

            result.StatusCode.Should().Be((int)HttpStatusCode.OK);
            _invoiceServiceMock.Verify(x => x.GetAll(), Times.Once(), "called More than One");



        }

        [Fact]
        public void GetAll_FetchalllFailed_returnstatuscode404()
        {
            //Arrange

            var testInvoices = new List<InvoiceDto>();
       


            _invoiceServiceMock.Setup(x => x.GetAll()).Returns(testInvoices);
            var invoice = new InvoiceController(_invoiceServiceMock.Object);

            //Act 
            var result = invoice.GetAll() as ObjectResult;

            //Assert 

            result.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
            _invoiceServiceMock.Verify(x => x.GetAll(), Times.Once(), "called More than One");



        }

      

    }
}
