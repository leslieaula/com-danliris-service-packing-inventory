﻿using Com.Danliris.Service.Packing.Inventory.Application.CommonViewModelObjectProperties;
using Com.Danliris.Service.Packing.Inventory.Application.ToBeRefactored.CommonViewModelObjectProperties;
using Com.Danliris.Service.Packing.Inventory.Application.ToBeRefactored.DyeingPrintingAreaInput.Shipping;
using Com.Danliris.Service.Packing.Inventory.Data.Models.DyeingPrintingAreaMovement;
using Com.Danliris.Service.Packing.Inventory.Infrastructure.Repositories.DyeingPrintingAreaMovement;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Com.Danliris.Service.Packing.Inventory.Test.Services
{
    public class InputShippingServiceTest
    {
        public InputShippingService GetService(IServiceProvider serviceProvider)
        {
            return new InputShippingService(serviceProvider);
        }

        public Mock<IServiceProvider> GetServiceProvider(IDyeingPrintingAreaInputRepository repository, IDyeingPrintingAreaMovementRepository movementRepo,
           IDyeingPrintingAreaSummaryRepository summaryRepo, IDyeingPrintingAreaOutputRepository outputRepo, IDyeingPrintingAreaInputProductionOrderRepository sppRepo,
           IDyeingPrintingAreaOutputProductionOrderRepository outputSPPRepo)
        {
            var spMock = new Mock<IServiceProvider>();
            spMock.Setup(s => s.GetService(typeof(IDyeingPrintingAreaInputRepository)))
                .Returns(repository);

            spMock.Setup(s => s.GetService(typeof(IDyeingPrintingAreaMovementRepository)))
                .Returns(movementRepo);
            spMock.Setup(s => s.GetService(typeof(IDyeingPrintingAreaSummaryRepository)))
                .Returns(summaryRepo);
            spMock.Setup(s => s.GetService(typeof(IDyeingPrintingAreaOutputRepository)))
                .Returns(outputRepo);
            spMock.Setup(s => s.GetService(typeof(IDyeingPrintingAreaInputProductionOrderRepository)))
                .Returns(sppRepo);

            spMock.Setup(s => s.GetService(typeof(IDyeingPrintingAreaOutputProductionOrderRepository)))
                .Returns(outputSPPRepo);


            return spMock;
        }

        private InputShippingViewModel ViewModel
        {
            get
            {
                return new InputShippingViewModel()
                {
                    Area = "SHIPPING",
                    BonNo = "s",
                    Date = DateTimeOffset.UtcNow,
                    Shift = "pas",
                    Group = "A",
                    OutputId = 1,
                    ShippingProductionOrders = new List<InputShippingProductionOrderViewModel>()
                    {
                        new InputShippingProductionOrderViewModel()
                        {
                            Buyer = "s",
                            CartNo = "1",
                            Color = "red",
                            Construction = "sd",
                            Grade = "s",
                            HasOutputDocument = false,
                            Motif = "sd",
                            DeliveryOrder = new DeliveryOrderSales()
                            {
                                Id = 1,
                                No = "s"
                            },
                            ProductionOrder = new ProductionOrder()
                            {
                                Code = "sd",
                                Id = 1,
                                Type = "sd",
                                No = "sd"
                            },
                            Packing = "s",
                            PackingType = "sd",
                            Qty = 1,
                            QtyPacking = 1,
                            Unit = "s",
                            UomUnit = "d",
                            InputId = 1,
                            OutputId = 1,
                            DyeingPrintingAreaInputProductionOrderId = 1
                        }
                    }
                };
            }
        }

        private DyeingPrintingAreaInputModel Model
        {
            get
            {
                return new DyeingPrintingAreaInputModel(ViewModel.Date, ViewModel.Area, ViewModel.Shift, ViewModel.BonNo, ViewModel.Group, ViewModel.ShippingProductionOrders.Select(s =>
                    new DyeingPrintingAreaInputProductionOrderModel(ViewModel.Area, s.DeliveryOrder.Id, s.DeliveryOrder.No, s.ProductionOrder.Id, s.ProductionOrder.No, s.ProductionOrder.Type, s.ProductionOrder.OrderQuantity, s.Buyer, s.Construction,
                    s.PackingType, s.Color, s.Motif, s.Grade, s.QtyPacking, s.Packing, s.Qty, s.UomUnit, s.HasOutputDocument, s.Qty)).ToList());
            }
        }
        private OutputPreShippingProductionOrderViewModel OutputModel
        {
            get
            {
                return new OutputPreShippingProductionOrderViewModel()
                {
                    DeliveryOrder = new DeliveryOrderSales
                    {
                        Id = 1,
                        No = "12"
                    },
                    ProductionOrder = new ProductionOrder
                    {
                        Code = "Code",
                        Id = 1,
                        No = "daf",
                        OrderQuantity = 12,
                        Type = "type",
                    },
                    CartNo = "adsf",
                    Construction = "df",
                    Unit = "df",
                    Buyer = "df",
                    Color = "df",
                    Motif = "df",
                    UomUnit = "df",
                    Grade = "fd",
                    Packing ="dsaf",
                    QtyPacking = 1,
                    Qty = 1,
                    PackingType = "df",
                    OutputId = 1,
                    DyeingPrintingAreaInputProductionOrderId = 1
                };
            }
        }
        [Fact]
        public async Task Should_Success_Create()
        {
            var repoMock = new Mock<IDyeingPrintingAreaInputRepository>();
            var movementRepoMock = new Mock<IDyeingPrintingAreaMovementRepository>();
            var summaryRepoMock = new Mock<IDyeingPrintingAreaSummaryRepository>();
            var outputRepoMock = new Mock<IDyeingPrintingAreaOutputRepository>();
            var productionOrderRepoMock = new Mock<IDyeingPrintingAreaInputProductionOrderRepository>();
            var outputSPPRepoMock = new Mock<IDyeingPrintingAreaOutputProductionOrderRepository>();

            repoMock.Setup(s => s.InsertAsync(It.IsAny<DyeingPrintingAreaInputModel>()))
                .ReturnsAsync(1);

            repoMock.Setup(s => s.GetDbSet())
                .Returns(new List<DyeingPrintingAreaInputModel>() {  }.AsQueryable());

            repoMock.Setup(s => s.ReadAllIgnoreQueryFilter())
                .Returns(new List<DyeingPrintingAreaInputModel>() { Model }.AsQueryable());

            movementRepoMock.Setup(s => s.InsertAsync(It.IsAny<DyeingPrintingAreaMovementModel>()))
                 .ReturnsAsync(1);

            summaryRepoMock.Setup(s => s.UpdateAsync(It.IsAny<int>(), It.IsAny<DyeingPrintingAreaSummaryModel>()))
                 .ReturnsAsync(1);

            outputSPPRepoMock.Setup(s => s.UpdateFromInputAsync(It.IsAny<IEnumerable<int>>(), It.IsAny<bool>()))
                .ReturnsAsync(1);

            productionOrderRepoMock.Setup(s => s.UpdateFromNextAreaInputAsync(It.IsAny<int>(), It.IsAny<double>()))
                .ReturnsAsync(1);

            var item = ViewModel.ShippingProductionOrders.FirstOrDefault();

            summaryRepoMock.Setup(s => s.ReadAll())
                 .Returns(new List<DyeingPrintingAreaSummaryModel>() {
                     new DyeingPrintingAreaSummaryModel(ViewModel.Date, ViewModel.Area, "IN", ViewModel.OutputId, ViewModel.BonNo, item.ProductionOrder.Id,
                     item.ProductionOrder.No, item.CartNo, item.Buyer, item.Construction,item.Unit, item.Color,item.Motif,item.UomUnit, item.Qty)
                 }.AsQueryable());

            outputRepoMock.Setup(s => s.UpdateFromInputAsync(It.IsAny<int>(), It.IsAny<bool>()))
                .ReturnsAsync(1);

            var service = GetService(GetServiceProvider(repoMock.Object, movementRepoMock.Object, summaryRepoMock.Object, outputRepoMock.Object, productionOrderRepoMock.Object, outputSPPRepoMock.Object).Object);

            var result = await service.Create(ViewModel);

            Assert.NotEqual(0, result);
        }

        [Fact]
        public async Task Should_Success_Create_If_Summary_Null()
        {
            var repoMock = new Mock<IDyeingPrintingAreaInputRepository>();
            var movementRepoMock = new Mock<IDyeingPrintingAreaMovementRepository>();
            var summaryRepoMock = new Mock<IDyeingPrintingAreaSummaryRepository>();
            var outputRepoMock = new Mock<IDyeingPrintingAreaOutputRepository>();
            var productionOrderRepoMock = new Mock<IDyeingPrintingAreaInputProductionOrderRepository>();
            var outputSPPRepoMock = new Mock<IDyeingPrintingAreaOutputProductionOrderRepository>();

            repoMock.Setup(s => s.InsertAsync(It.IsAny<DyeingPrintingAreaInputModel>()))
                .ReturnsAsync(1);

            repoMock.Setup(s => s.GetDbSet())
                .Returns(new List<DyeingPrintingAreaInputModel>() { }.AsQueryable());

            repoMock.Setup(s => s.ReadAllIgnoreQueryFilter())
                .Returns(new List<DyeingPrintingAreaInputModel>() { Model }.AsQueryable());

            movementRepoMock.Setup(s => s.InsertAsync(It.IsAny<DyeingPrintingAreaMovementModel>()))
                 .ReturnsAsync(1);

            summaryRepoMock.Setup(s => s.UpdateAsync(It.IsAny<int>(), It.IsAny<DyeingPrintingAreaSummaryModel>()))
                 .ReturnsAsync(1);

            outputSPPRepoMock.Setup(s => s.UpdateFromInputAsync(It.IsAny<IEnumerable<int>>(), It.IsAny<bool>()))
                .ReturnsAsync(1);

            productionOrderRepoMock.Setup(s => s.UpdateFromNextAreaInputAsync(It.IsAny<int>(), It.IsAny<double>()))
                .ReturnsAsync(1);

            var item = ViewModel.ShippingProductionOrders.FirstOrDefault();

            //summaryRepoMock.Setup(s => s.ReadAll())
            //     .Returns(new List<DyeingPrintingAreaSummaryModel>() {
            //         new DyeingPrintingAreaSummaryModel()
            //     }.AsQueryable());
            //summaryRepoMock.Setup(s => s.ReadAll())
            //     .Returns<IQueryable<DyeingPrintingAreaSummaryModel>>(null);

            //summaryRepoMock.Setup(s => s.ReadAll().FirstOrDefault())
            //    .Returns<DyeingPrintingAreaSummaryModel>(null);
            summaryRepoMock.Setup(s => s.ReadAll())
                 .Returns(new List<DyeingPrintingAreaSummaryModel>() {
                                 new DyeingPrintingAreaSummaryModel(ViewModel.Date, ViewModel.Area, "IN", 0, ViewModel.BonNo, item.ProductionOrder.Id,
                                 item.ProductionOrder.No, item.CartNo, item.Buyer, item.Construction,item.Unit, item.Color,item.Motif,item.UomUnit, item.Qty)
                 }.AsQueryable());

            outputRepoMock.Setup(s => s.UpdateFromInputAsync(It.IsAny<int>(), It.IsAny<bool>()))
                .ReturnsAsync(1);

            var service = GetService(GetServiceProvider(repoMock.Object, movementRepoMock.Object, summaryRepoMock.Object, outputRepoMock.Object, productionOrderRepoMock.Object, outputSPPRepoMock.Object).Object);

            var result = await service.Create(ViewModel);

            Assert.NotEqual(0, result);
        }

        [Fact]
        public async Task Should_Success_Create_If_Model_Null()
        {
            var repoMock = new Mock<IDyeingPrintingAreaInputRepository>();
            var movementRepoMock = new Mock<IDyeingPrintingAreaMovementRepository>();
            var summaryRepoMock = new Mock<IDyeingPrintingAreaSummaryRepository>();
            var outputRepoMock = new Mock<IDyeingPrintingAreaOutputRepository>();
            var productionOrderRepoMock = new Mock<IDyeingPrintingAreaInputProductionOrderRepository>();
            var outputSPPRepoMock = new Mock<IDyeingPrintingAreaOutputProductionOrderRepository>();

            repoMock.Setup(s => s.InsertAsync(It.IsAny<DyeingPrintingAreaInputModel>()))
                .ReturnsAsync(1);

            repoMock.Setup(s => s.GetDbSet())
                .Returns(new List<DyeingPrintingAreaInputModel>() { }.AsQueryable());
            var modelModif = Model;
            modelModif.SetArea( "error","unittest","");
            repoMock.Setup(s => s.ReadAllIgnoreQueryFilter())
                .Returns(new List<DyeingPrintingAreaInputModel>() { modelModif }.AsQueryable());

            movementRepoMock.Setup(s => s.InsertAsync(It.IsAny<DyeingPrintingAreaMovementModel>()))
                 .ReturnsAsync(1);

            summaryRepoMock.Setup(s => s.UpdateAsync(It.IsAny<int>(), It.IsAny<DyeingPrintingAreaSummaryModel>()))
                 .ReturnsAsync(1);

            outputSPPRepoMock.Setup(s => s.UpdateFromInputAsync(It.IsAny<IEnumerable<int>>(), It.IsAny<bool>()))
                .ReturnsAsync(1);

            productionOrderRepoMock.Setup(s => s.UpdateFromNextAreaInputAsync(It.IsAny<int>(), It.IsAny<double>()))
                .ReturnsAsync(1);

            var item = ViewModel.ShippingProductionOrders.FirstOrDefault();

            //summaryRepoMock.Setup(s => s.ReadAll())
            //     .Returns(new List<DyeingPrintingAreaSummaryModel>() {
            //         new DyeingPrintingAreaSummaryModel()
            //     }.AsQueryable());
            //summaryRepoMock.Setup(s => s.ReadAll())
            //     .Returns<IQueryable<DyeingPrintingAreaSummaryModel>>(null);

            //summaryRepoMock.Setup(s => s.ReadAll().FirstOrDefault())
            //    .Returns<DyeingPrintingAreaSummaryModel>(null);
            summaryRepoMock.Setup(s => s.ReadAll())
                 .Returns(new List<DyeingPrintingAreaSummaryModel>() {
                                 new DyeingPrintingAreaSummaryModel(ViewModel.Date, ViewModel.Area, "IN", 0, ViewModel.BonNo, item.ProductionOrder.Id,
                                 item.ProductionOrder.No, item.CartNo, item.Buyer, item.Construction,item.Unit, item.Color,item.Motif,item.UomUnit, item.Qty)
                 }.AsQueryable());

            outputRepoMock.Setup(s => s.UpdateFromInputAsync(It.IsAny<int>(), It.IsAny<bool>()))
                .ReturnsAsync(1);

            var service = GetService(GetServiceProvider(repoMock.Object, movementRepoMock.Object, summaryRepoMock.Object, outputRepoMock.Object, productionOrderRepoMock.Object, outputSPPRepoMock.Object).Object);

            var result = await service.Create(ViewModel);

            Assert.NotEqual(0, result);
        }

        [Fact]
        public async Task Should_Success_Create_DuplicateShift()
        {
            var repoMock = new Mock<IDyeingPrintingAreaInputRepository>();
            var movementRepoMock = new Mock<IDyeingPrintingAreaMovementRepository>();
            var summaryRepoMock = new Mock<IDyeingPrintingAreaSummaryRepository>();
            var outputRepoMock = new Mock<IDyeingPrintingAreaOutputRepository>();
            var productionOrderRepoMock = new Mock<IDyeingPrintingAreaInputProductionOrderRepository>();
            var outputSPPRepoMock = new Mock<IDyeingPrintingAreaOutputProductionOrderRepository>();

            repoMock.Setup(s => s.InsertAsync(It.IsAny<DyeingPrintingAreaInputModel>()))
                .ReturnsAsync(1);

            repoMock.Setup(s => s.GetDbSet())
                .Returns(new List<DyeingPrintingAreaInputModel>() { Model }.AsQueryable());

            repoMock.Setup(s => s.ReadAllIgnoreQueryFilter())
                .Returns(new List<DyeingPrintingAreaInputModel>() { Model }.AsQueryable());

            movementRepoMock.Setup(s => s.InsertAsync(It.IsAny<DyeingPrintingAreaMovementModel>()))
                 .ReturnsAsync(1);

            summaryRepoMock.Setup(s => s.UpdateAsync(It.IsAny<int>(), It.IsAny<DyeingPrintingAreaSummaryModel>()))
                 .ReturnsAsync(1);

            outputSPPRepoMock.Setup(s => s.UpdateFromInputAsync(It.IsAny<IEnumerable<int>>(), It.IsAny<bool>()))
                .ReturnsAsync(1);

            productionOrderRepoMock.Setup(s => s.UpdateFromNextAreaInputAsync(It.IsAny<int>(), It.IsAny<double>()))
                .ReturnsAsync(1);

            var item = ViewModel.ShippingProductionOrders.FirstOrDefault();

            summaryRepoMock.Setup(s => s.ReadAll())
                 .Returns(new List<DyeingPrintingAreaSummaryModel>() {
                     new DyeingPrintingAreaSummaryModel(ViewModel.Date, ViewModel.Area, "IN", ViewModel.OutputId, ViewModel.BonNo, item.ProductionOrder.Id,
                     item.ProductionOrder.No, item.CartNo, item.Buyer, item.Construction,item.Unit, item.Color,item.Motif,item.UomUnit, item.Qty)
                 }.AsQueryable());

            outputRepoMock.Setup(s => s.UpdateFromInputAsync(It.IsAny<int>(), It.IsAny<bool>()))
                .ReturnsAsync(1);

            var service = GetService(GetServiceProvider(repoMock.Object, movementRepoMock.Object, summaryRepoMock.Object, outputRepoMock.Object, productionOrderRepoMock.Object, outputSPPRepoMock.Object).Object);

            var result = await service.Create(ViewModel);

            Assert.NotEqual(0, result);
        }

        [Fact]
        public void Should_Success_Read()
        {
            var repoMock = new Mock<IDyeingPrintingAreaInputRepository>();
            var movementRepoMock = new Mock<IDyeingPrintingAreaMovementRepository>();
            var summaryRepoMock = new Mock<IDyeingPrintingAreaSummaryRepository>();
            var outputRepoMock = new Mock<IDyeingPrintingAreaOutputRepository>();
            var productionOrderRepoMock = new Mock<IDyeingPrintingAreaInputProductionOrderRepository>();
            var outputSPPRepoMock = new Mock<IDyeingPrintingAreaOutputProductionOrderRepository>();

            repoMock.Setup(s => s.ReadAll())
                 .Returns(new List<DyeingPrintingAreaInputModel>() { Model }.AsQueryable());

            var service = GetService(GetServiceProvider(repoMock.Object, movementRepoMock.Object, summaryRepoMock.Object, outputRepoMock.Object, productionOrderRepoMock.Object, outputSPPRepoMock.Object).Object);

            var result = service.Read(1, 25, "{}", "{}", null);

            Assert.NotEmpty(result.Data);
        }

        [Fact]
        public async Task Should_Success_ReadById()
        {
            var repoMock = new Mock<IDyeingPrintingAreaInputRepository>();
            var movementRepoMock = new Mock<IDyeingPrintingAreaMovementRepository>();
            var summaryRepoMock = new Mock<IDyeingPrintingAreaSummaryRepository>();
            var outputRepoMock = new Mock<IDyeingPrintingAreaOutputRepository>();
            var productionOrderRepoMock = new Mock<IDyeingPrintingAreaInputProductionOrderRepository>();
            var outputSPPRepoMock = new Mock<IDyeingPrintingAreaOutputProductionOrderRepository>();

            repoMock.Setup(s => s.ReadByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(Model);

            var service = GetService(GetServiceProvider(repoMock.Object, movementRepoMock.Object, summaryRepoMock.Object, outputRepoMock.Object, productionOrderRepoMock.Object, outputSPPRepoMock.Object).Object);

            var result = await service.ReadById(1);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task Should_Null_ReadById()
        {
            var repoMock = new Mock<IDyeingPrintingAreaInputRepository>();
            var movementRepoMock = new Mock<IDyeingPrintingAreaMovementRepository>();
            var summaryRepoMock = new Mock<IDyeingPrintingAreaSummaryRepository>();
            var outputRepoMock = new Mock<IDyeingPrintingAreaOutputRepository>();
            var productionOrderRepoMock = new Mock<IDyeingPrintingAreaInputProductionOrderRepository>();
            var outputSPPRepoMock = new Mock<IDyeingPrintingAreaOutputProductionOrderRepository>();

            repoMock.Setup(s => s.ReadByIdAsync(It.IsAny<int>()))
               .ReturnsAsync(default(DyeingPrintingAreaInputModel));

            var service = GetService(GetServiceProvider(repoMock.Object, movementRepoMock.Object, summaryRepoMock.Object, outputRepoMock.Object, productionOrderRepoMock.Object, outputSPPRepoMock.Object).Object);

            var result = await service.ReadById(1);

            Assert.Null(result);
        }

        [Fact]
        public void Should_Success_ReadPreShipping()
        {
            var repoMock = new Mock<IDyeingPrintingAreaInputRepository>();
            var movementRepoMock = new Mock<IDyeingPrintingAreaMovementRepository>();
            var summaryRepoMock = new Mock<IDyeingPrintingAreaSummaryRepository>();
            var outputRepoMock = new Mock<IDyeingPrintingAreaOutputRepository>();
            var productionOrderRepoMock = new Mock<IDyeingPrintingAreaInputProductionOrderRepository>();
            var outputSPPRepoMock = new Mock<IDyeingPrintingAreaOutputProductionOrderRepository>();

            outputRepoMock.Setup(s => s.ReadAll())
                 .Returns(new List<DyeingPrintingAreaOutputModel>() { new DyeingPrintingAreaOutputModel(DateTimeOffset.UtcNow, "GUDANG JADI","pagi","no",false,
                    "SHIPPING","A", new List<DyeingPrintingAreaOutputProductionOrderModel>(){
                        new DyeingPrintingAreaOutputProductionOrderModel("PACKING","SHIPPING", false, 1,"no","t",1,"1","1","sd","cs","sd","as","sd","asd","asd","sd","sd",1, 1)
                    }) }.AsQueryable());

            var service = GetService(GetServiceProvider(repoMock.Object, movementRepoMock.Object, summaryRepoMock.Object, outputRepoMock.Object, productionOrderRepoMock.Object, outputSPPRepoMock.Object).Object);

            var result = service.ReadOutputPreShipping(1, 25, "{}", "{}", null);

            Assert.NotEmpty(result.Data);
        }

        [Fact]
        public void Should_Success_ReadPreShipping_SPP()
        {
            var repoMock = new Mock<IDyeingPrintingAreaInputRepository>();
            var movementRepoMock = new Mock<IDyeingPrintingAreaMovementRepository>();
            var summaryRepoMock = new Mock<IDyeingPrintingAreaSummaryRepository>();
            var outputRepoMock = new Mock<IDyeingPrintingAreaOutputRepository>();
            var productionOrderRepoMock = new Mock<IDyeingPrintingAreaInputProductionOrderRepository>();
            var outputSPPRepoMock = new Mock<IDyeingPrintingAreaOutputProductionOrderRepository>();

            productionOrderRepoMock.Setup(s => s.ReadAll())
                 .Returns(Model.DyeingPrintingAreaInputProductionOrders.AsQueryable());

            var service = GetService(GetServiceProvider(repoMock.Object, movementRepoMock.Object, summaryRepoMock.Object, outputRepoMock.Object, productionOrderRepoMock.Object, outputSPPRepoMock.Object).Object);

            var result = service.ReadProductionOrders(1, 25, "{}", "{}", null);

            Assert.NotEmpty(result.Data);
        }

        [Fact]
        public void Should_Success_GetOutputPreShippingProductionOrders()
        {
            var repoMock = new Mock<IDyeingPrintingAreaInputRepository>();
            var movementRepoMock = new Mock<IDyeingPrintingAreaMovementRepository>();
            var summaryRepoMock = new Mock<IDyeingPrintingAreaSummaryRepository>();
            var outputRepoMock = new Mock<IDyeingPrintingAreaOutputRepository>();
            var productionOrderRepoMock = new Mock<IDyeingPrintingAreaInputProductionOrderRepository>();
            var outputSPPRepoMock = new Mock<IDyeingPrintingAreaOutputProductionOrderRepository>();


            outputSPPRepoMock.Setup(s => s.ReadAll()).Returns(new List<DyeingPrintingAreaOutputProductionOrderModel>()
            {
                new DyeingPrintingAreaOutputProductionOrderModel("GUDANG JADI", "SHIPPING", false, 1, "a", "e", 1,"rr", "1", "as", "test", "unit", "color", "motif", "mtr", "rem", "a", "a", 1, 1)
            }.AsQueryable());
            var service = GetService(GetServiceProvider(repoMock.Object, movementRepoMock.Object, summaryRepoMock.Object, outputRepoMock.Object, productionOrderRepoMock.Object, outputSPPRepoMock.Object).Object);

            var result = service.GetOutputPreShippingProductionOrders();

            Assert.NotEmpty(result);
        }

        [Fact]
        public void Get_Set_OutputPreShiping()
        {
            var outPre = OutputModel;
            var test = outPre.DeliveryOrder;
            var test2 = outPre.ProductionOrder;
            var test3 = outPre.CartNo;
            var test4 = outPre.Construction;
            var Unit = outPre.Unit;
            var buyre = outPre.Buyer;
            var color = outPre.Color;
            var motif = outPre.Motif;
            var uomUnit = outPre.UomUnit;
            var grade = outPre.Grade;
            var packing = outPre.Packing;
            var qtyPacking = outPre.QtyPacking;
            var qty = outPre.Qty;
            var packingTYpe = outPre.PackingType;
            var OuptuId = outPre.OutputId;
            var dyeingid = outPre.DyeingPrintingAreaInputProductionOrderId;
            //var test = outPre.Grade;
            //var test2 = outPre.DeliveryOrder
            Assert.NotNull(test);
        }

        [Fact]
        public void Get_Set_InputSPPShiping()
        {
            var outPre = ViewModel;
            var inputId = outPre.ShippingProductionOrders.FirstOrDefault().InputId;
            //var test = outPre.Grade;
            //var test2 = outPre.DeliveryOrder
            Assert.NotNull(inputId);
        }

    }
}
