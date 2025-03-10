using InvoiceApp.Api.Controllers;
using InvoiceApp.Service.DTOS.Store;
using FluentAssertions;
using Invocie.api.Tests.TestModels;
using InvoiceApp.Api.Services.StoreService;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Invocie.api.Tests.Controller.Tests.Store.tests
{
    public class StoreControllerTests
    {

        private readonly Mock<IStoreService> _storeMock;
        private readonly List<StoreDto> Teststores;
        private readonly StoreController storeController;
        public StoreControllerTests()
        {
            _storeMock = new();
            storeController = new StoreController(_storeMock.Object);
            Teststores = new List<StoreDto>
        {
            new StoreDto
            {
                Id = 1,
                Name = "Store A",
                storeitems = new List<StoreitemsDto>
                {
                    new StoreitemsDto { itemid = 101, itemname = "Item A1" },
                    new StoreitemsDto { itemid = 102, itemname = "Item A2" }
                },
                storeunits = new List<StoreUnitsDto>
                {
                    new StoreUnitsDto { unitid = 201, unitname = "Unit A1" },
                    new StoreUnitsDto { unitid = 202, unitname = "Unit A2" }
                }
            },
            new StoreDto
            {
                Id = 2,
                Name = "Store B",
                storeitems = new List<StoreitemsDto>
                {
                    new StoreitemsDto { itemid = 103, itemname = "Item B1" },
                    new StoreitemsDto { itemid = 104, itemname = "Item B2" }
                },
                storeunits = new List<StoreUnitsDto>
                {
                    new StoreUnitsDto { unitid = 203, unitname = "Unit B1" },
                    new StoreUnitsDto { unitid = 204, unitname = "Unit B2" }
                }
            }
        };
        }

        [Theory]
        [MemberData(nameof(PassDatausingMemberData.PassDataToSuccess),MemberType =typeof(PassDatausingMemberData))]
        public void Get_FoundStoreWithThisId_ReturnStatuscode200(int id)
        {
            //Arrange
           
            var expectedstore = Teststores.FirstOrDefault(s => s.Id == id);
           
            //Act
            _storeMock.Setup(x => x.GetById(id)).Returns(Teststores.FirstOrDefault(s => s.Id == id));
            var result = storeController.Get(id) as ObjectResult;
            //Assert
            
            result.StatusCode.Should().Be((int)HttpStatusCode.OK);
            result.Value.Should().Equals (expectedstore);


        }

        [Theory]
        [MemberData(nameof(PassDatausingMemberData.PassDatatofailed), MemberType = typeof(PassDatausingMemberData))]
        public void Get_NotFoundStoreWithThisId_ReturnStatuscode404(int id)
        {
            //Arrange
            var expectedstore = Teststores.FirstOrDefault(s => s.Id == id);
            
           
            //Act
            _storeMock.Setup(x => x.GetById(id)).Returns((StoreDto)null);
            var result = storeController.Get(id) as ObjectResult;
            //Assert

            result.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
            result.Value.Should().Equals(expectedstore);


        }
      



        [Fact]
        public void GetAll_NoStoresFound_ReturnStatuscode404()
        {
            //Arrange
            
            
            //Act
            _storeMock.Setup(x => x.GetAllStores()).Returns((List<StoreDto>)null);
            var result = storeController.GetAll() as ObjectResult;
            //Assert
            result.ContentTypes.Count.Should().Equals(0);
            result.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
            _storeMock.Verify(x=>x.GetAllStores(), Times.Once(),"Get all stores More than one time ");
           


        }

        [Fact]
        public void GetAll_StoresFound_ReturnStatuscode200andstores()
        {
            //Arrange
            var ExpectedResult = Teststores.ToList();

            //Act
            _storeMock.Setup(x => x.GetAllStores()).Returns(Teststores);
            var result = storeController.GetAll() as ObjectResult;
            //Assert
            result.ContentTypes.Count.Should().Equals(2);
            result.StatusCode.Should().Be((int)HttpStatusCode.OK);
            _storeMock.Verify(x => x.GetAllStores(), Times.Once(), "Get all stores More than one time ");
            result.Value.Should().Equals(ExpectedResult);


        }

    }
}
