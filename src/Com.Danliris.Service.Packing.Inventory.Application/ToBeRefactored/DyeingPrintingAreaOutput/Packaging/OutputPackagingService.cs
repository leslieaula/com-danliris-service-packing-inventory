﻿using Com.Danliris.Service.Packing.Inventory.Application.Utilities;
using Com.Danliris.Service.Packing.Inventory.Infrastructure.Repositories.DyeingPrintingAreaMovement;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Com.Danliris.Service.Packing.Inventory.Data.Models.DyeingPrintingAreaMovement;
using System.Linq;
using Com.Danliris.Service.Packing.Inventory.Application.CommonViewModelObjectProperties;
using Newtonsoft.Json;
using Com.Danliris.Service.Packing.Inventory.Infrastructure.Utilities;
using System.IO;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System.Data;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using OfficeOpenXml;
using System.Globalization;
using Com.Danliris.Service.Packing.Inventory.Application.ToBeRefactored.DyeingPrintingAreaInput.Packaging;
using Newtonsoft.Json.Linq;

namespace Com.Danliris.Service.Packing.Inventory.Application.ToBeRefactored.DyeingPrintingAreaOutput.Packaging
{
    public class OutputPackagingService : IOutputPackagingService
    {
        private readonly IDyeingPrintingAreaOutputRepository _repository;
        private readonly IDyeingPrintingAreaMovementRepository _movementRepository;
        private readonly IDyeingPrintingAreaSummaryRepository _summaryRepository;
        private readonly IDyeingPrintingAreaInputProductionOrderRepository _inputProductionOrderRepository;
        private readonly IDyeingPrintingAreaInputRepository _inputRepository;
        private readonly IDyeingPrintingAreaOutputProductionOrderRepository _outputProductionOrderRepository;

        private const string TYPE = "OUT";

        private const string OUT = "OUT";
        private const string ADJ = "ADJ";

        private const string ADJ_IN = "ADJ IN";
        private const string ADJ_OUT = "ADJ OUT";

        private const string IM = "IM";
        private const string TR = "TR";
        private const string PC = "PC";
        private const string GJ = "GJ";
        private const string GA = "GA";
        private const string SP = "SP";

        private const string INSPECTIONMATERIAL = "INSPECTION MATERIAL";
        private const string TRANSIT = "TRANSIT";
        private const string PACKING = "PACKING";
        private const string GUDANGJADI = "GUDANG JADI";
        private const string GUDANGAVAL = "GUDANG AVAL";
        private const string SHIPPING = "SHIPPING";

        public enum AreaAbbr
        {
            [Description("IM")]
            INSPECTIONMATERIAL = 0,
            [Description("TR")]
            TRANSIT,
            [Description("PC")]
            PACKING,
            [Description("GJ")]
            GUDANGJADI,
            [Description("GA")]
            GUDANGAVAL,
            [Description("SP")]
            SHIPPING,
        }

        //public enum AreaName
        //{
        //    [Description("INSPECTION MATERIAL")]
        //    INSPECTIONMATERIAL = 0,
        //    [Description("TRANSIT")]
        //    TRANSIT,
        //    [Description("PACKING")]
        //    PACKING,
        //    [Description("GUDANG JADI")]
        //    GUDANGJADI,
        //    [Description("GUDANG AVAL")]
        //    GUDANGAVAL,
        //    [Description("SHIPPING")]
        //    SHIPPING,
        //}
        //public enum UnitPackaging
        //{
        //    DEFAULT = 0,

        //    [Description("ROLLS")]
        //    ROLLS ,

        //    [Description("PIECE")]
        //    PIECE,

        //    [Description("PACK")]
        //    PACK
        //}
        //public enum TypePackaging
        //{
        //    DEFAULT = 0,

        //    [Description("WHITE")]
        //    WHITE = 0,

        //    [Description("DYEING")]
        //    DYEING,

        //    [Description("BATIK")]
        //    BATIK,

        //    [Description("TEXTILE")]
        //    TEXTILE,

        //    [Description("DIGITAL PRINTING")]
        //    DIGITALPRINTING,

        //    [Description("TRANSFER PRINT")]
        //    TRANSFERPRINT
        //}

        public OutputPackagingService(IServiceProvider serviceProvider)
        {
            _repository = serviceProvider.GetService<IDyeingPrintingAreaOutputRepository>();
            _movementRepository = serviceProvider.GetService<IDyeingPrintingAreaMovementRepository>();
            _summaryRepository = serviceProvider.GetService<IDyeingPrintingAreaSummaryRepository>();
            _inputProductionOrderRepository = serviceProvider.GetService<IDyeingPrintingAreaInputProductionOrderRepository>();
            _inputRepository = serviceProvider.GetService<IDyeingPrintingAreaInputRepository>();
            _outputProductionOrderRepository = serviceProvider.GetService<IDyeingPrintingAreaOutputProductionOrderRepository>();
        }

        private OutputPackagingViewModel MapToViewModel(DyeingPrintingAreaOutputModel model)
        {
            if (model.Type == "ADJ IN" || model.Type == "ADJ OUT")
            {
                var vm = new OutputPackagingViewModel()
                {
                    Type = model.Type.Contains("ADJ") ? "ADJ" : "OUT",
                    Active = model.Active,
                    Id = model.Id,
                    Area = model.Area,
                    BonNo = model.BonNo,
                    CreatedAgent = model.CreatedAgent,
                    CreatedBy = model.CreatedBy,
                    CreatedUtc = model.CreatedUtc,
                    Date = model.Date,
                    DeletedAgent = model.DeletedAgent,
                    DeletedBy = model.DeletedBy,
                    DeletedUtc = model.DeletedUtc,
                    IsDeleted = model.IsDeleted,
                    LastModifiedAgent = model.LastModifiedAgent,
                    LastModifiedBy = model.LastModifiedBy,
                    LastModifiedUtc = model.LastModifiedUtc,
                    Shift = model.Shift,
                    DestinationArea = model.DestinationArea,
                    HasNextAreaDocument = model.HasNextAreaDocument,
                    Group = model.Group,
                    PackagingProductionOrders = model.DyeingPrintingAreaOutputProductionOrders.Select(s => new OutputPackagingProductionOrderViewModel()
                    {
                        Active = s.Active,
                        LastModifiedUtc = s.LastModifiedUtc,
                        Balance = s.Balance,
                        Buyer = s.Buyer,
                        BuyerId = s.BuyerId,
                        CartNo = s.CartNo,
                        Color = s.Color,
                        Construction = s.Construction,
                        CreatedAgent = s.CreatedAgent,
                        CreatedBy = s.CreatedBy,
                        CreatedUtc = s.CreatedUtc,
                        DeletedAgent = s.DeletedAgent,
                        DeletedBy = s.DeletedBy,
                        DeletedUtc = s.DeletedUtc,
                        Grade = s.Grade,
                        PackingInstruction = s.PackingInstruction,
                        Remark = s.Remark,
                        Status = s.Status,
                        Id = s.Id,
                        IsDeleted = s.IsDeleted,
                        LastModifiedAgent = s.LastModifiedAgent,
                        LastModifiedBy = s.LastModifiedBy,
                        Motif = s.Motif,
                        ProductionOrder = new ProductionOrder()
                        {
                            Id = s.ProductionOrderId,
                            No = s.ProductionOrderNo,
                            Type = s.ProductionOrderType
                        },
                        Unit = s.Unit,
                        UomUnit = s.UomUnit,
                        PackagingQTY = s.PackagingQty,
                        PackagingType = s.PackagingType,
                        PackagingUnit = s.PackagingUnit,
                        QtyOrder = s.ProductionOrderOrderQuantity,
                        QtyOut = s.Balance,
                        ProductionOrderNo = s.ProductionOrderNo,
                        Keterangan = s.Description
                    }).ToList(),
                    PackagingProductionOrdersAdj = model.DyeingPrintingAreaOutputProductionOrders.Select(s => new InputPlainAdjPackagingProductionOrder()
                    {
                        Balance = s.Balance,
                        Buyer = s.Buyer,
                        BuyerId = s.BuyerId,
                        CartNo = s.CartNo,
                        Color = s.Color,
                        Construction = s.Construction,
                        Grade = s.Grade,
                        PackingInstruction = s.PackingInstruction,
                        Remark = s.Remark,
                        Status = s.Status,
                        Id = s.Id,
                        Motif = s.Motif,
                        ProductionOrder = new ProductionOrder()
                        {
                            Id = s.ProductionOrderId,
                            No = s.ProductionOrderNo,
                            Type = s.ProductionOrderType
                        },
                        Unit = s.Unit,
                        UomUnit = s.UomUnit,
                        PackagingQty = s.PackagingQty,
                        PackagingType = s.PackagingType,
                        PackagingUnit = s.PackagingUnit,
                        NoDocument = s.AdjDocumentNo
                    }).ToList()
                };
                return vm;
            }
            else
            {
                var vm = new OutputPackagingViewModel()
                {
                    Type = "OUT",
                    Active = model.Active,
                    Id = model.Id,
                    Area = model.Area,
                    BonNo = model.BonNo,
                    CreatedAgent = model.CreatedAgent,
                    CreatedBy = model.CreatedBy,
                    CreatedUtc = model.CreatedUtc,
                    Date = model.Date,
                    DeletedAgent = model.DeletedAgent,
                    DeletedBy = model.DeletedBy,
                    DeletedUtc = model.DeletedUtc,
                    IsDeleted = model.IsDeleted,
                    LastModifiedAgent = model.LastModifiedAgent,
                    LastModifiedBy = model.LastModifiedBy,
                    LastModifiedUtc = model.LastModifiedUtc,
                    Shift = model.Shift,
                    DestinationArea = model.DestinationArea,
                    HasNextAreaDocument = model.HasNextAreaDocument,
                    Group = model.Group,
                    PackagingProductionOrders = model.DyeingPrintingAreaOutputProductionOrders.Select(s => new OutputPackagingProductionOrderViewModel()
                    {
                        Active = s.Active,
                        LastModifiedUtc = s.LastModifiedUtc,
                        Balance = s.Balance,
                        Buyer = s.Buyer,
                        BuyerId = s.BuyerId,
                        CartNo = s.CartNo,
                        Color = s.Color,
                        Construction = s.Construction,
                        CreatedAgent = s.CreatedAgent,
                        CreatedBy = s.CreatedBy,
                        CreatedUtc = s.CreatedUtc,
                        DeletedAgent = s.DeletedAgent,
                        DeletedBy = s.DeletedBy,
                        DeletedUtc = s.DeletedUtc,
                        Grade = s.Grade,
                        PackingInstruction = s.PackingInstruction,
                        Remark = s.Remark,
                        Status = s.Status,
                        Id = s.Id,
                        IsDeleted = s.IsDeleted,
                        LastModifiedAgent = s.LastModifiedAgent,
                        LastModifiedBy = s.LastModifiedBy,
                        Motif = s.Motif,
                        ProductionOrder = new ProductionOrder()
                        {
                            Id = s.ProductionOrderId,
                            No = s.ProductionOrderNo,
                            Type = s.ProductionOrderType
                        },
                        Unit = s.Unit,
                        UomUnit = s.UomUnit,
                        PackagingQTY = s.PackagingQty,
                        PackagingType = s.PackagingType,
                        PackagingUnit = s.PackagingUnit,
                        QtyOrder = s.ProductionOrderOrderQuantity,
                        QtyOut = s.Balance,
                        ProductionOrderNo = s.ProductionOrderNo,
                        Keterangan = s.Description
                    }).ToList()
                };
                return vm;
            }
        }

        public string GenerateBonNo(int totalPreviousData, DateTimeOffset date, string destinationAreaName)
        {
            string sourceArea = AreaAbbr.PACKING.ToDescription();
            if (destinationAreaName == TRANSIT)
            {
                return string.Format("{0}.{1}.{2}.{3}", sourceArea, TR, date.ToString("yy"), totalPreviousData.ToString().PadLeft(4, '0'));
            }
            else if (destinationAreaName == INSPECTIONMATERIAL)
            {
                return string.Format("{0}.{1}.{2}.{3}", sourceArea, IM, date.ToString("yy"), totalPreviousData.ToString().PadLeft(4, '0'));
            }
            else if (destinationAreaName == GUDANGJADI)
            {
                return string.Format("{0}.{1}.{2}.{3}", sourceArea, GJ, date.ToString("yy"), totalPreviousData.ToString().PadLeft(4, '0'));
            }
            else if (destinationAreaName == GUDANGAVAL)
            {
                return string.Format("{0}.{1}.{2}.{3}", sourceArea, GA, date.ToString("yy"), totalPreviousData.ToString().PadLeft(4, '0'));
            }
            else if (destinationAreaName == SHIPPING)
            {
                return string.Format("{0}.{1}.{2}.{3}", sourceArea, SP, date.ToString("yy"), totalPreviousData.ToString().PadLeft(4, '0'));
            }
            else
            {
                return string.Format("{0}.{1}.{2}.{3}", sourceArea, GA, date.ToString("yy"), totalPreviousData.ToString().PadLeft(4, '0'));
            }
        }
        public string GenerateBonNoAdj(int totalPreviousData, DateTimeOffset date, string area, IEnumerable<double> qtys)
        {
            if (qtys.All(s => s > 0))
            {
                return string.Format("{0}.{1}.{2}.{3}", ADJ_IN, TR, date.ToString("yy"), totalPreviousData.ToString().PadLeft(4, '0'));
            }
            else
            {
                return string.Format("{0}.{1}.{2}.{3}", ADJ_OUT, TR, date.ToString("yy"), totalPreviousData.ToString().PadLeft(4, '0'));
            }
        }
        public async Task<int> Create(OutputPackagingViewModel viewModel)
        {
            int result = 0;
            int totalCurrentYearData = _repository.ReadAllIgnoreQueryFilter().Count(s => s.Area == PACKING && s.DestinationArea == viewModel.DestinationArea
                && s.CreatedUtc.Year == viewModel.Date.Year);
            string bonNo = GenerateBonNo(totalCurrentYearData + 1, viewModel.Date, viewModel.DestinationArea);
            viewModel.PackagingProductionOrders = viewModel.PackagingProductionOrders.Where(s => s.Balance > 0).ToList();

            //get BonNo with shift
            var hasBonNoWithShift = _repository.ReadAll().Where(x => x.Shift == viewModel.Shift && x.Area == PACKING && x.Date == viewModel.Date).FirstOrDefault();
            DyeingPrintingAreaOutputModel model = new DyeingPrintingAreaOutputModel();
            if (hasBonNoWithShift == null)
            {

                model = new DyeingPrintingAreaOutputModel(viewModel.Date, viewModel.Area, viewModel.Shift, bonNo, false, viewModel.DestinationArea, viewModel.Group, viewModel.PackagingProductionOrders.Select(s =>
                     new DyeingPrintingAreaOutputProductionOrderModel(viewModel.Area, viewModel.DestinationArea, false, s.ProductionOrder.Id, s.ProductionOrder.No, s.CartNo, s.Buyer, s.Construction, s.Unit, s.Color,
                     s.Motif, s.UomUnit, s.Remark, s.Grade, s.Status, s.QtyOut, s.PackingInstruction, s.ProductionOrder.Type, s.ProductionOrder.OrderQuantity, s.PackagingType, s.PackagingQTY, s.PackagingUnit, s.QtyOrder, s.Keterangan, 0, s.Id, s.BuyerId)).ToList());
                result += await _repository.InsertAsync(model);
            }
            else
            {
                model = new DyeingPrintingAreaOutputModel(viewModel.Date, viewModel.Area, viewModel.Shift, hasBonNoWithShift.BonNo, false, viewModel.DestinationArea, viewModel.Group, viewModel.PackagingProductionOrders.Select(s =>
                     new DyeingPrintingAreaOutputProductionOrderModel(viewModel.Area, viewModel.DestinationArea, false, s.ProductionOrder.Id, s.ProductionOrder.No, s.CartNo, s.Buyer, s.Construction, s.Unit, s.Color,
                     s.Motif, s.UomUnit, s.Remark, s.Grade, s.Status, s.QtyOut, s.PackingInstruction, s.ProductionOrder.Type, s.ProductionOrder.OrderQuantity, s.PackagingType, s.PackagingQTY, s.PackagingUnit, s.QtyOrder, s.Keterangan, hasBonNoWithShift.Id, s.Id, s.BuyerId)).ToList());
                model.Id = hasBonNoWithShift.Id;
                bonNo = model.BonNo;
            }
            var modelInput = _inputRepository.ReadAll().Where(x => x.BonNo == viewModel.BonNoInput && x.Area == PACKING);

            var modelInputProductionOrder = _inputProductionOrderRepository.ReadAll().Join(modelInput,
                                                                                s => s.DyeingPrintingAreaInputId,
                                                                                s2 => s2.Id,
                                                                                (s, s2) => s);
            foreach (var item in model.DyeingPrintingAreaOutputProductionOrders)
            {
                var vmItem = viewModel.PackagingProductionOrders.FirstOrDefault(s => s.ProductionOrder.Id == item.ProductionOrderId);
                //update balance SPP Input
                var inputSPP = _inputProductionOrderRepository.ReadAll().FirstOrDefault(x => x.Id == item.DyeingPrintingAreaInputProductionOrderId);
                inputSPP.SetBalanceRemains(inputSPP.BalanceRemains - item.Balance, "REPOSITORY", "");
                inputSPP.SetHasOutputDocument(true, "REPOSITORY", "");
                result += await _inputProductionOrderRepository.UpdateAsync(inputSPP.Id, inputSPP);

                //var previousProductionOrder = modelInputProductionOrder.Where(x => x.ProductionOrderNo == vmItem.ProductionOrder.No).FirstOrDefault();
                //var lastBalance = previousProductionOrder.Balance - vmItem.QtyOut;
                //previousProductionOrder.SetBalance(lastBalance,"REPOSITORY","");

                //result += await _inputProductionOrderRepository.UpdateAsync(previousProductionOrder.Id,previousProductionOrder);
                result += await _inputProductionOrderRepository.UpdateFromOutputAsync(vmItem.Id, true);


                var movementModel = new DyeingPrintingAreaMovementModel(viewModel.Date, viewModel.Area, TYPE, model.Id, bonNo, item.ProductionOrderId, item.ProductionOrderNo,
                    item.CartNo, item.Buyer, item.Construction, item.Unit, item.Color, item.Motif, item.UomUnit, item.Balance);

                var previousSummary = _summaryRepository.ReadAll().FirstOrDefault(s => s.DyeingPrintingAreaDocumentId == viewModel.InputPackagingId && s.ProductionOrderId == item.ProductionOrderId);

                var summaryModel = new DyeingPrintingAreaSummaryModel(viewModel.Date, viewModel.Area, TYPE, model.Id, bonNo, item.ProductionOrderId, item.ProductionOrderNo,
                    item.CartNo, item.Buyer, item.Construction, item.Unit, item.Color, item.Motif, item.UomUnit, item.Balance);

                //var updateBalance = new DyeingPrintingAreaInputModel(viewModel.Date, viewModel.Area, viewModel.Shift, viewModel.BonNoInput, viewModel.Group, viewModel.PackagingProductionOrders.Select(s =>
                // new DyeingPrintingAreaInputProductionOrderModel(viewModel.Area, s.ProductionOrder.Id, s.ProductionOrder.No, s.ProductionOrder.Type, s.PackingInstruction, s.CartNo, s.Buyer, s.Construction,
                // s.Unit, s.Color, s.Motif, s.UomUnit, s.Balance-vmItem.QtyOut, false, s.QtyOrder, s.Grade)).ToList());

                if (hasBonNoWithShift != null)
                    result += await _outputProductionOrderRepository.InsertAsync(item);

                result += await _movementRepository.InsertAsync(movementModel);
                if (previousSummary != null)
                    result += await _summaryRepository.UpdateAsync(previousSummary.Id, summaryModel);

            }

            return result;
        }
        private async Task<int> InsertAdj(OutputPackagingViewModel viewModel)
        {
            int result = 0;
            string type = "";
            string bonNo = "";
            if (viewModel.PackagingProductionOrdersAdj.All(d => d.Balance > 0))
            {
                int totalCurrentYearData = _repository.ReadAllIgnoreQueryFilter().Count(s => s.Area == PACKING && s.Type == ADJ_IN && s.CreatedUtc.Year == viewModel.Date.Year);
                bonNo = GenerateBonNoAdj(totalCurrentYearData + 1, viewModel.Date, viewModel.Area, viewModel.PackagingProductionOrdersAdj.Select(d => d.Balance));
                type = ADJ_IN;
            }
            else
            {
                int totalCurrentYearData = _repository.ReadAllIgnoreQueryFilter().Count(s => s.Area == PACKING && s.Type == ADJ_OUT && s.CreatedUtc.Year == viewModel.Date.Year);
                bonNo = GenerateBonNoAdj(totalCurrentYearData + 1, viewModel.Date, viewModel.Area, viewModel.PackagingProductionOrdersAdj.Select(d => d.Balance));
                type = ADJ_OUT;
            }
            var model = new DyeingPrintingAreaOutputModel(viewModel.Date, viewModel.Area, viewModel.Shift, bonNo, true, "", viewModel.Group, type,
                    viewModel.PackagingProductionOrdersAdj.Select(item => new DyeingPrintingAreaOutputProductionOrderModel(viewModel.Area, "", true, item.ProductionOrder.Id, item.ProductionOrder.No,
                        item.ProductionOrder.Type, item.ProductionOrder.OrderQuantity, item.PackingInstruction, item.CartNo, item.Buyer, item.Construction,
                        item.Unit, item.Color, item.Motif, item.UomUnit, item.Remark, "", "", item.Balance, 0, item.BuyerId,
                        item.MaterialObj.Id, item.MaterialObj.Name, item.MaterialConstruction.Id, item.MaterialConstruction.Name, item.MaterialWidth, item.NoDocument,item.PackagingType,item.PackagingQty,item.PackagingUnit)).ToList());

            result = await _repository.InsertAsync(model);

            foreach (var item in model.DyeingPrintingAreaOutputProductionOrders)
            {
                var movementModel = new DyeingPrintingAreaMovementModel(viewModel.Date, viewModel.Area, type, model.Id, model.BonNo, item.ProductionOrderId, item.ProductionOrderNo,
                        item.CartNo, item.Buyer, item.Construction, item.Unit, item.Color, item.Motif, item.UomUnit, item.Balance, item.Id);

                result += await _movementRepository.InsertAsync(movementModel);
            }

            return result;
        }


        public async Task<int> CreateV2(OutputPackagingViewModel viewModel)
        {
            int result = 0;
            int totalCurrentYearData = _repository.ReadAllIgnoreQueryFilter().Count(s => s.Area == PACKING && s.DestinationArea == viewModel.DestinationArea
                && s.CreatedUtc.Year == viewModel.Date.Year);
            string bonNo = GenerateBonNo(totalCurrentYearData + 1, viewModel.Date, viewModel.DestinationArea);
            viewModel.PackagingProductionOrders = viewModel.PackagingProductionOrders.Where(s => s.Balance > 0).ToList();

            foreach (var item in viewModel.PackagingProductionOrders)
            {
                //get BonNo with shift
                var hasBonNoWithShift = _repository.ReadAll().Where(x => x.Shift == viewModel.Shift && x.Area == PACKING && x.Date.Date == viewModel.Date.Date).FirstOrDefault();
                DyeingPrintingAreaOutputModel model = new DyeingPrintingAreaOutputModel();

                //get spp in that will be decrease balance
                var sppInDecrease = _inputProductionOrderRepository.ReadAll().Where(x => x.ProductionOrderId == item.ProductionOrder.Id && x.Area == PACKING && !x.HasOutputDocument && x.BalanceRemains >0 ).OrderBy(x=> x.Id).ToList();
                List<DyeingPrintingAreaInputProductionOrderModel> listSppHasDescrease = new List<DyeingPrintingAreaInputProductionOrderModel>();
                var qtyOut = item.QtyOut;
                foreach(var spp in sppInDecrease)
                {
                    if (qtyOut <= 0)
                        break;
                    else
                    {
                        double qtyDecrase = 0;
                        if(qtyOut >= spp.BalanceRemains)
                            //balance remains empty
                        {
                            qtyDecrase = spp.BalanceRemains;
                            qtyOut -= qtyDecrase;
                            listSppHasDescrease.Add(spp);
                            result += await _inputProductionOrderRepository.UpdateFromOutputAsync(spp.Id, true);

                        }
                        else
                        //balance remains has residu
                        {
                            qtyDecrase = qtyOut;
                            qtyOut -= qtyDecrase;
                            spp.SetBalanceRemains(qtyDecrase,"OUTPUTPACKAGING","SERVICE");
                            listSppHasDescrease.Add(spp);
                            result += await _inputProductionOrderRepository.UpdateAsync(spp.Id, spp);
                        }
                    }
                }
                var jsonSetting = new JsonSerializerSettings();
                jsonSetting.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                jsonSetting.NullValueHandling = NullValueHandling.Ignore;
                jsonSetting.MissingMemberHandling = MissingMemberHandling.Ignore;
                jsonSetting.Formatting = Formatting.None;
                jsonSetting.DateFormatHandling = DateFormatHandling.IsoDateFormat;
                jsonSetting.FloatParseHandling = FloatParseHandling.Double;

                var jsonLIstSppHasDecrease = JsonConvert.SerializeObject(listSppHasDescrease,Formatting.Indented,jsonSetting);
                if (hasBonNoWithShift == null)
                {

                    model = new DyeingPrintingAreaOutputModel(viewModel.Date, viewModel.Area, viewModel.Shift, bonNo, false, viewModel.DestinationArea, viewModel.Group, viewModel.PackagingProductionOrders.Select(s =>
                         new DyeingPrintingAreaOutputProductionOrderModel(viewModel.Area, viewModel.DestinationArea, false, s.ProductionOrder.Id, s.ProductionOrder.No, s.CartNo, s.Buyer, s.Construction, s.Unit, s.Color,
                         s.Motif, s.UomUnit, s.Remark, s.Grade, s.Status, s.QtyOut, s.PackingInstruction, s.ProductionOrder.Type, s.ProductionOrder.OrderQuantity, s.PackagingType, s.PackagingQTY, s.PackagingUnit, s.QtyOrder, s.Keterangan, 0, s.Id, s.BuyerId,jsonLIstSppHasDecrease)).ToList());
                    result += await _repository.InsertAsync(model);
                }
                else
                {
                    model = new DyeingPrintingAreaOutputModel(viewModel.Date, viewModel.Area, viewModel.Shift, hasBonNoWithShift.BonNo, false, viewModel.DestinationArea, viewModel.Group, viewModel.PackagingProductionOrders.Select(s =>
                         new DyeingPrintingAreaOutputProductionOrderModel(viewModel.Area, viewModel.DestinationArea, false, s.ProductionOrder.Id, s.ProductionOrder.No, s.CartNo, s.Buyer, s.Construction, s.Unit, s.Color,
                         s.Motif, s.UomUnit, s.Remark, s.Grade, s.Status, s.QtyOut, s.PackingInstruction, s.ProductionOrder.Type, s.ProductionOrder.OrderQuantity, s.PackagingType, s.PackagingQTY, s.PackagingUnit, s.QtyOrder, s.Keterangan, hasBonNoWithShift.Id, s.Id, s.BuyerId,jsonLIstSppHasDecrease)).ToList());
                    model.Id = hasBonNoWithShift.Id;
                    bonNo = model.BonNo;
                }

                foreach (var items in model.DyeingPrintingAreaOutputProductionOrders)
                {

                    var movementModel = new DyeingPrintingAreaMovementModel(viewModel.Date, viewModel.Area, TYPE, model.Id, bonNo, items.ProductionOrderId, items.ProductionOrderNo,
                        items.CartNo, items.Buyer, items.Construction, items.Unit, items.Color, items.Motif, items.UomUnit, items.Balance);

                    if (hasBonNoWithShift != null)
                        result += await _outputProductionOrderRepository.InsertAsync(items);

                    result += await _movementRepository.InsertAsync(movementModel);


                }
            }
            return result;
        }

        public ListResult<IndexViewModel> Read(int page, int size, string filter, string order, string keyword)
        {
            var query = _repository.ReadAll().Where(s => s.Area == PACKING && (!s.HasNextAreaDocument||s.Type == "ADJ IN"||s.Type == "ADJ OUT"));
            List<string> SearchAttributes = new List<string>()
            {
                "BonNo"
            };

            query = QueryHelper<DyeingPrintingAreaOutputModel>.Search(query, SearchAttributes, keyword);

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            query = QueryHelper<DyeingPrintingAreaOutputModel>.Filter(query, FilterDictionary);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            query = QueryHelper<DyeingPrintingAreaOutputModel>.Order(query, OrderDictionary);
            var data = query.Skip((page - 1) * size).Take(size).Select(s => new IndexViewModel()
            {
                Area = s.Area,
                BonNo = s.BonNo,
                Date = s.Date,
                Id = s.Id,
                Shift = s.Shift,
                Group = s.Group,
                DestinationArea = s.DestinationArea,
                HasNextAreaDocument = s.HasNextAreaDocument,
                PackagingProductionOrders = s.DyeingPrintingAreaOutputProductionOrders.Select(d => new OutputPackagingProductionOrderViewModel()
                {
                    Balance = d.Balance,
                    Buyer = d.Buyer,
                    BuyerId = d.BuyerId,
                    CartNo = d.CartNo,
                    Color = d.Color,
                    Construction = d.Construction,
                    Motif = d.Motif,
                    ProductionOrder = new ProductionOrder()
                    {
                        Id = d.ProductionOrderId,
                        No = d.ProductionOrderNo,
                        Type = d.ProductionOrderType
                    },
                    Id = d.Id,
                    Unit = d.Unit,
                    Grade = d.Grade,
                    Remark = d.Remark,
                    Status = d.Status,
                    PackingInstruction = d.PackingInstruction,
                    UomUnit = d.UomUnit,
                    PackagingQTY = d.PackagingQty,
                    PackagingType = d.PackagingType,
                    PackagingUnit = d.PackagingUnit,
                    Material = d.Construction,
                    QtyOrder = d.ProductionOrderOrderQuantity,
                    ProductionOrderNo = d.ProductionOrderNo,
                    Keterangan = d.Description
                }).ToList()
            });

            return new ListResult<IndexViewModel>(data.ToList(), page, size, query.Count());
        }

        public async Task<OutputPackagingViewModel> ReadById(int id)
        {
            var model = await _repository.ReadByIdAsync(id);
            if (model == null)
                return null;

            OutputPackagingViewModel vm = MapToViewModel(model);

            return vm;
        }

        public async Task<MemoryStream> GenerateExcel(int id)
        {
            var model = await _repository.ReadByIdAsync(id);
            var query = model.DyeingPrintingAreaOutputProductionOrders;
            //var query = GetQuery(date, group, zona, timeOffSet);
            DataTable dt = new DataTable();

            #region Mapping Properties Class to Head excel
            Dictionary<string, string> mappedClass = new Dictionary<string, string>
            {
                {"ProductionOrderNo","No SPP" },
                {"ProductionOrderOrderQuantity","Qty Order" },
                {"Buyer","Buyer" },
                {"Unit","Unit"},
                {"Construction","Material "},
                {"Color","Warna"},
                {"Motif","Motif"},
                {"PackagingType","Jenis"},
                {"Grade","Grade"},
                {"PackagingQty","Qty Packaging"},
                {"PackagingUnit","Packaging"},
                {"UomUnit","Satuan"},
                {"Balance","Saldo"},
                //{"Balance","Qty Keluar" },
                {"Description","Keterangan" },
                {"Menyerahkan","Menyerahkan" },
                {"Menerima","Menerima" },
            };
            var listClass = query.ToList().FirstOrDefault().GetType().GetProperties();
            #endregion
            #region Assign Column
            foreach (var prop in mappedClass.Select((item, index) => new { Index = index, Items = item }))
            {
                string fieldName = prop.Items.Value;
                dt.Columns.Add(new DataColumn() { ColumnName = fieldName, DataType = typeof(string) });
            }
            #endregion
            #region Assign Data
            foreach (var item in query)
            {
                List<string> data = new List<string>();
                foreach (DataColumn column in dt.Columns)
                {
                    var searchMappedClass = mappedClass.Where(x => x.Value == column.ColumnName && column.ColumnName != "Menyerahkan" && column.ColumnName != "Menerima");
                    string valueClass = "";
                    if (searchMappedClass != null && searchMappedClass != null && searchMappedClass.FirstOrDefault().Key != null)
                    {
                        var searchProperty = item.GetType().GetProperty(searchMappedClass.FirstOrDefault().Key);
                        var searchValue = searchProperty.GetValue(item, null);
                        valueClass = searchValue == null ? "" : searchValue.ToString();
                    }
                    else
                    {
                        valueClass = "";
                    }
                    data.Add(valueClass);
                }
                dt.Rows.Add(data.ToArray());
            }
            #endregion

            #region Render Excel Header
            ExcelPackage package = new ExcelPackage();
            var sheet = package.Workbook.Worksheets.Add("BON PACKAGING");
            sheet.Cells[1, 1].Value = "TANGGAL";
            sheet.Cells[1, 2].Value = model.Date.ToString("dd MMM yyyy", new CultureInfo("id-ID"));

            sheet.Cells[2, 1].Value = "SHIFT";
            sheet.Cells[2, 2].Value = model.Shift;

            sheet.Cells[3, 1].Value = "ZONA";
            sheet.Cells[3, 2].Value = model.DestinationArea;

            sheet.Cells[4, 1].Value = "NOMOR BON";
            sheet.Cells[4, 2].Value = model.BonNo;

            int startHeaderColumn = 1;
            int endHeaderColumn = mappedClass.Count;

            sheet.Cells[1, 1, 6, endHeaderColumn].Style.Font.Bold = true;


            sheet.Cells[6, startHeaderColumn, 7, endHeaderColumn].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            sheet.Cells[6, startHeaderColumn, 7, endHeaderColumn].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            sheet.Cells[6, startHeaderColumn, 7, endHeaderColumn].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            sheet.Cells[6, startHeaderColumn, 7, endHeaderColumn].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            sheet.Cells[6, startHeaderColumn, 7, endHeaderColumn].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            sheet.Cells[6, startHeaderColumn, 7, endHeaderColumn].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Aqua);

            foreach (DataColumn column in dt.Columns)
            {
                if (column.ColumnName != "Menyerahkan" && column.ColumnName != "Menerima")
                {
                    sheet.Cells[6, startHeaderColumn].Value = column.ColumnName;
                    sheet.Cells[6, startHeaderColumn, 7, startHeaderColumn].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    sheet.Cells[6, startHeaderColumn, 7, startHeaderColumn].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    sheet.Cells[6, startHeaderColumn, 7, startHeaderColumn].Merge = true;
                    startHeaderColumn++;
                }
            }

            sheet.Cells[6, startHeaderColumn].Value = "Paraf";
            sheet.Cells[6, startHeaderColumn, 6, startHeaderColumn + 1].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            sheet.Cells[6, startHeaderColumn, 6, startHeaderColumn + 1].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            sheet.Cells[6, startHeaderColumn, 6, startHeaderColumn + 1].Merge = true;

            sheet.Cells[7, startHeaderColumn].Value = "Menyerahkan";
            sheet.Cells[7, startHeaderColumn].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            sheet.Cells[7, startHeaderColumn].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            startHeaderColumn++;

            sheet.Cells[7, startHeaderColumn].Value = "Menerima";
            sheet.Cells[7, startHeaderColumn].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            sheet.Cells[7, startHeaderColumn].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            #endregion

            #region Insert Data To Excel
            int tableRowStart = 8;
            int tableColStart = 1;

            sheet.Cells[tableRowStart, tableColStart].LoadFromDataTable(dt, false, OfficeOpenXml.Table.TableStyles.Light8);
            //sheet.Cells[sheet.Dimension.Address].AutoFitColumns();
            #endregion

            MemoryStream stream = new MemoryStream();
            package.SaveAs(stream);

            return stream;
        }

        public MemoryStream GenerateExcelAll(DateTimeOffset? dateFrom, DateTimeOffset? dateTo, int offSet)
        {
            //var model = await _repository.ReadByIdAsync(id);
            //var modelAll = _repository.ReadAll().Where(s => s.Area == PACKING && !s.HasNextAreaDocument).ToList().Select(s =>
            //    new
            //    {
            //        SppList = s.DyeingPrintingAreaOutputProductionOrders.Select(d => new
            //        {
            //            BonNo = s.BonNo,
            //            NoSPP = d.ProductionOrderNo,
            //            QtyOrder = d.ProductionOrderOrderQuantity,
            //            Material = d.Construction,
            //            Unit = d.Unit,
            //            Buyer = d.Buyer,
            //            Warna = d.Color,
            //            Motif = d.Motif,
            //            Jenis = d.PackagingType,
            //            Grade = d.Grade,
            //            Ket = d.Description,
            //            QtyPack = d.PackagingQty,
            //            Pack = d.PackagingUnit,
            //            Qty = d.Balance,
            //            SAT = d.UomUnit
            //        })
            //    });

            var packingData = _repository.ReadAll().Where(s => s.Area == PACKING && !s.HasNextAreaDocument);

            if (dateFrom.HasValue && dateTo.HasValue)
            {
                packingData = packingData.Where(s => dateFrom.Value.Date <= s.Date.ToOffset(new TimeSpan(offSet, 0, 0)).Date &&
                            s.Date.ToOffset(new TimeSpan(offSet, 0, 0)).Date <= dateTo.Value.Date);
            }
            else if (!dateFrom.HasValue && dateTo.HasValue)
            {
                packingData = packingData.Where(s => s.Date.ToOffset(new TimeSpan(offSet, 0, 0)).Date <= dateTo.Value.Date);
            }
            else if (dateFrom.HasValue && !dateTo.HasValue)
            {
                packingData = packingData.Where(s => dateFrom.Value.Date <= s.Date.ToOffset(new TimeSpan(offSet, 0, 0)).Date);
            }

            packingData = packingData.OrderBy(s => s.BonNo);

            var modelAll = packingData.ToList().Select(s =>
                new
                {
                    SppList = s.DyeingPrintingAreaOutputProductionOrders.Select(d => new
                    {
                        BonNo = s.BonNo,
                        NoSPP = d.ProductionOrderNo,
                        QtyOrder = d.ProductionOrderOrderQuantity,
                        Material = d.Construction,
                        Unit = d.Unit,
                        Buyer = d.Buyer,
                        Warna = d.Color,
                        Motif = d.Motif,
                        Jenis = d.PackagingType,
                        Grade = d.Grade,
                        Ket = d.Description,
                        QtyPack = d.PackagingQty,
                        Pack = d.PackagingUnit,
                        Qty = d.Balance,
                        SAT = d.UomUnit
                    })
                });

            //var modelAll = _repository.ReadAll().Where(s => s.Area == PACKING && !s.HasNextAreaDocument).ToList().SelectMany(s =>
            //    new
            //    {
            //        SppList = s.DyeingPrintingAreaOutputProductionOrders.Select(d => new
            //        {
            //            BonNo = s.BonNo,
            //            NoSPP = d.ProductionOrderNo,
            //            QtyOrder = d.ProductionOrderOrderQuantity,
            //            Material = d.Construction,
            //            Unit = d.Unit,
            //            Buyer = d.Buyer,
            //            Warna = d.Color,
            //            Motif = d.Motif,
            //            Jenis = d.PackagingType,
            //            Grade = d.Grade,
            //            Ket = d.Description,
            //            QtyPack = d.PackagingQty,
            //            Pack = d.PackagingUnit,
            //            Qty = d.Balance,
            //            SAT = d.UomUnit
            //        })
            //    });

            var model = modelAll.First(); 
            //var query = model.DyeingPrintingAreaOutputProductionOrders;
            var query = modelAll.SelectMany(s=>s.SppList);

            //var query = GetQuery(date, group, zona, timeOffSet);
            DataTable dt = new DataTable();

            #region Mapping Properties Class to Head excel
            Dictionary<string, string> mappedClass = new Dictionary<string, string>
            {
                {"BonNo","NO BON" },
                {"NoSPP","NO SP" },
                {"QtyOrder","QTY ORDER" },
                {"Material","MATERIAL"},
                {"Unit","UNIT"},
                {"Buyer","BUYER"},
                {"Warna","WARNA"},
                {"Motif","MOTIF"},
                {"Jenis","JENIS"},
                {"Grade","GRADE"},
                {"Ket","KET"},
                {"QtyPack","QTY Pack"},
                {"Pack","PACK"},
                {"Qty","QTY" },
                {"SAT","SAT" },
            };
            var listClass = query.ToList().FirstOrDefault().GetType().GetProperties();
            #endregion
            #region Assign Column
            foreach (var prop in mappedClass.Select((item, index) => new { Index = index, Items = item }))
            {
                string fieldName = prop.Items.Value;
                dt.Columns.Add(new DataColumn() { ColumnName = fieldName, DataType = typeof(string) });
            }
            #endregion
            #region Assign Data
            foreach (var item in query)
            {
                List<string> data = new List<string>();
                foreach (DataColumn column in dt.Columns)
                {
                    var searchMappedClass = mappedClass.Where(x => x.Value == column.ColumnName && column.ColumnName != "Menyerahkan" && column.ColumnName != "Menerima");
                    string valueClass = "";
                    if (searchMappedClass != null && searchMappedClass != null && searchMappedClass.FirstOrDefault().Key != null)
                    {
                        var searchProperty = item.GetType().GetProperty(searchMappedClass.FirstOrDefault().Key);
                        var searchValue = searchProperty.GetValue(item, null);
                        valueClass = searchValue == null ? "" : searchValue.ToString();
                    }
                    //else
                    //{
                    //    valueClass = "";
                    //}
                    data.Add(valueClass);
                }
                dt.Rows.Add(data.ToArray());
            }
            #endregion

            #region Render Excel Header
            ExcelPackage package = new ExcelPackage();
            var sheet = package.Workbook.Worksheets.Add("BON PACKAGING");

            int startHeaderColumn = 1;
            int endHeaderColumn = mappedClass.Count;

            sheet.Cells[1, 1, 1, endHeaderColumn].Style.Font.Bold = true;


            sheet.Cells[1, startHeaderColumn].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            sheet.Cells[1, startHeaderColumn].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            sheet.Cells[1, startHeaderColumn].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            sheet.Cells[1, startHeaderColumn].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            sheet.Cells[1, startHeaderColumn,1,endHeaderColumn].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            sheet.Cells[1, startHeaderColumn,1,endHeaderColumn].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Aqua);

            foreach (DataColumn column in dt.Columns)
            {

                    sheet.Cells[1, startHeaderColumn].Value = column.ColumnName;
                    sheet.Cells[1, startHeaderColumn].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    sheet.Cells[1, startHeaderColumn].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    startHeaderColumn++;
            }
            #endregion

            #region Insert Data To Excel
            int tableRowStart = 2;
            int tableColStart = 1;

            sheet.Cells[tableRowStart, tableColStart].LoadFromDataTable(dt, false, OfficeOpenXml.Table.TableStyles.Light8);
            //sheet.Cells[sheet.Dimension.Address].AutoFitColumns();
            #endregion

            MemoryStream stream = new MemoryStream();
            package.SaveAs(stream);

            return stream;
        }

        public ListResult<IndexViewModel> ReadBonOutFromPack(int page, int size, string filter, string order, string keyword)
        {
            var query = _inputRepository.ReadAll().Where(s => s.Area == PACKING && s.DyeingPrintingAreaInputProductionOrders.Any(d => Convert.ToInt32(d.BalanceRemains) > 0 && d.DyeingPrintingAreaInputId == s.Id && d.HasOutputDocument == false));
            List<string> SearchAttributes = new List<string>()
            {
                "BonNo"
            };

            query = QueryHelper<DyeingPrintingAreaInputModel>.Search(query, SearchAttributes, keyword);

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            query = QueryHelper<DyeingPrintingAreaInputModel>.Filter(query, FilterDictionary);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            query = QueryHelper<DyeingPrintingAreaInputModel>.Order(query, OrderDictionary);
            var data = query.Skip((page - 1) * size).Take(size).Select(s => new IndexViewModel()
            {
                Area = s.Area,
                BonNo = s.BonNo,
                Date = s.Date,
                Id = s.Id,
                Shift = s.Shift,
                PackagingProductionOrders = MapModeltoModelView(s.DyeingPrintingAreaInputProductionOrders.ToList())
            });

            return new ListResult<IndexViewModel>(data.ToList(), page, size, query.Count());
        }
        public ListResult<InputPackagingProductionOrdersViewModel> ReadSppInFromPack(int page, int size, string filter, string order, string keyword)
        {
            var query2 = _inputRepository.ReadAll().Where(s => s.Area == PACKING && s.DyeingPrintingAreaInputProductionOrders.Any(d => d.DyeingPrintingAreaInputId == s.Id)); ;
            var query = _inputProductionOrderRepository.ReadAll().Join(query2,
                                                                        s => s.DyeingPrintingAreaInputId,
                                                                        s2 => s2.Id,
                                                                        (s, s2) => s);


            List<string> SearchAttributes = new List<string>()
            {
                "ProductionOrderNo"
            };

            query = QueryHelper<DyeingPrintingAreaInputProductionOrderModel>.Search(query, SearchAttributes, keyword);

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            query = QueryHelper<DyeingPrintingAreaInputProductionOrderModel>.Filter(query, FilterDictionary);

            var data = query.ToList().Select(s => new InputPackagingProductionOrdersViewModel()
            {
                Id = s.Id,
                Balance = s.Balance,
                Buyer = s.Buyer,
                CartNo = s.CartNo,
                Color = s.Color,
                Construction = s.Construction,
                //HasOutputDocument = s.HasOutputDocument,
                //IsChecked = s.IsChecked,
                Motif = s.Motif,
                PackingInstruction = s.PackingInstruction,
                ProductionOrder = new ProductionOrder()
                {
                    Id = s.ProductionOrderId,
                    No = s.ProductionOrderNo,
                    Type = s.ProductionOrderType
                },
                Unit = s.Unit,
                UomUnit = s.UomUnit,
                ProductionOrderNo = s.ProductionOrderNo,
                Area = s.Area,
                BuyerId = s.BuyerId,
                Grade = s.Grade,
                Status = s.Status,
                HasOutputDocument= s.HasOutputDocument,
                QtyOrder = s.ProductionOrderOrderQuantity,
                Remark = s.Remark
            });

            return new ListResult<InputPackagingProductionOrdersViewModel>(data.ToList(), page, size, query.Count());
        }
        public ListResult<PlainAdjPackagingProductionOrder> GetDistinctProductionOrder(int page, int size, string filter, string order, string keyword)
        {
            var query = _inputProductionOrderRepository.ReadAll().OrderByDescending(s => s.LastModifiedUtc)
                .Where(s => s.Area == PACKING && !s.HasOutputDocument);
            List<string> SearchAttributes = new List<string>()
            {
                "ProductionOrderNo"
            };

            query = QueryHelper<DyeingPrintingAreaInputProductionOrderModel>.Search(query, SearchAttributes, keyword);

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            query = QueryHelper<DyeingPrintingAreaInputProductionOrderModel>.Filter(query, FilterDictionary);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            query = QueryHelper<DyeingPrintingAreaInputProductionOrderModel>.Order(query, OrderDictionary);
            var data = query
                .GroupBy(d => d.ProductionOrderId)
                .Select(s => s.First())
                .Skip((page - 1) * size).Take(size)
                .OrderBy(s => s.ProductionOrderNo)
                .Select(s => new PlainAdjPackagingProductionOrder()
                {
                    ProductionOrder = new ProductionOrder()
                    {
                        Id = s.ProductionOrderId,
                        No = s.ProductionOrderNo,
                        OrderQuantity = s.ProductionOrderOrderQuantity,
                        Type = s.ProductionOrderType

                    },
                    PackagingQTY = s.PackagingQty,
                    PackagingType = s.PackagingType,
                    PackagingUnit = s.PackagingUnit,
                    Active = s.Active,
                    Area = s.Area,
                    Balance = s.Balance,
                    BalanceRemains = s.BalanceRemains,
                    Buyer = s.Buyer,
                    CartNo = s.CartNo,
                    BuyerId = s.BuyerId,
                    Color = s.Color,
                    Construction = s.Construction,
                    Status = s.Status,
                    DyeingPrintingAreaInputProductionOrderId = s.DyeingPrintingAreaInputId,
                    DyeingPrintingAreaOutputProductionOrderId = s.DyeingPrintingAreaOutputProductionOrderId,
                    Grade = s.Grade,
                    HasOutputDocument = s.HasOutputDocument,
                    Id = s.Id,
                    Material = new Material {
                        Name = s.MaterialName,
                        Id = s.MaterialId
                    },
                    MaterialConstruction = new MaterialConstruction
                    {
                        Name = s.MaterialConstructionName,
                        Id = s.MaterialConstructionId
                    },
                    MaterialWidth = s.MaterialWidth,
                    UomUnit = s.UomUnit,
                    Unit = s.Unit,
                    Motif = s.Motif,
                    PackingInstruction = s.PackingInstruction,
                    Remark = s.Remark,
                    AtQty = s.Balance / Convert.ToDouble(s.PackagingQty)
                });

            return new ListResult<PlainAdjPackagingProductionOrder>(data.ToList(), page, size, query.Count());
        }
        public ListResult<InputPackagingProductionOrdersViewModel> ReadSppInFromPackSumBySPPNo(int page, int size, string filter, string order, string keyword)
        {
            var query2 = _inputRepository.ReadAll().Where(s => s.Area == PACKING && s.DyeingPrintingAreaInputProductionOrders.Any(d => d.DyeingPrintingAreaInputId == s.Id)); ;
            var query = _inputProductionOrderRepository.ReadAll().Join(query2,
                                                                        s => s.DyeingPrintingAreaInputId,
                                                                        s2 => s2.Id,
                                                                        (s, s2) => s);


            List<string> SearchAttributes = new List<string>()
            {
                "ProductionOrderNo"
            };

            query = QueryHelper<DyeingPrintingAreaInputProductionOrderModel>.Search(query, SearchAttributes, keyword);

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            query = QueryHelper<DyeingPrintingAreaInputProductionOrderModel>.Filter(query, FilterDictionary);

            //var queryGroup = query.GroupBy(
            //                        s => s.ProductionOrderId,
            //                        s => s,
            //                        (key, item) => new { Key = key, Items = item })
            //                        .Select(s=> new InputPackagingProductionOrdersViewModel()
            //                        {
            //                            Id = s.Items.FirstOrDefault().Id,
            //                            Balance = s.Items.Sum(d=> d.Balance),
            //                            Buyer = s.Items.First().Buyer,
            //                            CartNo = s.Items.First().CartNo,
            //                            Color = s.Items.First().Color,
            //                            Construction = s.Items.First().Construction,
            //                            //HasOutputDocument = s.HasOutputDocument,
            //                            //IsChecked = s.IsChecked,
            //                            Motif = s.Items.First().Motif,
            //                            PackingInstruction = s.Items.First().PackingInstruction,
            //                            ProductionOrder = new ProductionOrder()
            //                            {
            //                                Id = s.Items.First().ProductionOrderId,
            //                                No = s.Items.First().ProductionOrderNo,
            //                                Type = s.Items.First().ProductionOrderType
            //                            },
            //                            Unit = s.Items.First().Unit,
            //                            UomUnit = s.Items.First().UomUnit,
            //                            ProductionOrderNo = s.Items.First().ProductionOrderNo,
            //                            Area = s.Items.First().Area,
            //                            BuyerId = s.Items.First().BuyerId,
            //                            Grade = s.Items.First().Grade,
            //                            Status = s.Items.First().Status,
            //                            HasOutputDocument = s.Items.First().HasOutputDocument,
            //                            QtyOrder = s.Items.First().ProductionOrderOrderQuantity,
            //                            Remark = s.Items.First().Remark,
            //                        });
            var queryGroup = query.GroupBy(
                                   s => s.ProductionOrderId,
                                   s => s,
                                   (key, item) => new { Key = key, Items = item });

            var data = queryGroup.ToList().Select(s => new InputPackagingProductionOrdersViewModel()
            {
                Id = 0,
                Balance = s.Items.Sum(d=>d.Balance),
                Buyer = s.Items.First().Buyer,
                CartNo = s.Items.First().CartNo,
                Color = s.Items.First().Color,
                Construction = s.Items.First().Construction,
                //HasOutputDocument = s.HasOutputDocument,
                //IsChecked = s.IsChecked,
                Motif = s.Items.First().Motif,
                PackingInstruction = s.Items.First().PackingInstruction,
                ProductionOrder = new ProductionOrder()
                {
                    Id = s.Items.First().ProductionOrderId,
                    No = s.Items.First().ProductionOrderNo,
                    Type = s.Items.First().ProductionOrderType
                },
                Unit = s.Items.First().Unit,
                UomUnit = s.Items.First().UomUnit,
                ProductionOrderNo = s.Items.First().ProductionOrderNo,
                Area = s.Items.First().Area,
                BuyerId = s.Items.First().BuyerId,
                Grade = s.Items.First().Grade,
                Status = s.Items.First().Status,
                HasOutputDocument = s.Items.First().HasOutputDocument,
                QtyOrder = s.Items.First().ProductionOrderOrderQuantity,
                Remark = s.Items.First().Remark
            });

            return new ListResult<InputPackagingProductionOrdersViewModel>(data.ToList(), page, size, query.Count());
        }
        public ListResult<OutputPackagingProductionOrderGroupedViewModel> ReadSppInFromPackGroup(int page, int size, string filter, string order, string keyword)
        {
            var query2 = _inputRepository.ReadAll().Where(s => s.Area == PACKING && s.DyeingPrintingAreaInputProductionOrders.Any(d => d.DyeingPrintingAreaInputId == s.Id)); ;
            var query = _inputProductionOrderRepository.ReadAll().Join(query2,
                                                                        s => s.DyeingPrintingAreaInputId,
                                                                        s2 => s2.Id,
                                                                        (s, s2) => s);


            List<string> SearchAttributes = new List<string>()
            {
                "ProductionOrderNo"
            };

            query = QueryHelper<DyeingPrintingAreaInputProductionOrderModel>.Search(query, SearchAttributes, keyword);

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            query = QueryHelper<DyeingPrintingAreaInputProductionOrderModel>.Filter(query, FilterDictionary);

            var datas = query.ToList().Select(s => new InputPackagingProductionOrdersViewModel()
            {
                Id = s.Id,
                Balance = s.Balance,
                Buyer = s.Buyer,
                CartNo = s.CartNo,
                Color = s.Color,
                Construction = s.Construction,
                //HasOutputDocument = s.HasOutputDocument,
                //IsChecked = s.IsChecked,
                Motif = s.Motif,
                PackingInstruction = s.PackingInstruction,
                ProductionOrder = new ProductionOrder()
                {
                    Id = s.ProductionOrderId,
                    No = s.ProductionOrderNo,
                    Type = s.ProductionOrderType
                },
                Unit = s.Unit,
                UomUnit = s.UomUnit,
                ProductionOrderNo = s.ProductionOrderNo,
                Area = s.Area,
                BuyerId = s.BuyerId,
                Grade = s.Grade,
                Status = s.Status,
                HasOutputDocument = s.HasOutputDocument,
                QtyOrder = s.ProductionOrderOrderQuantity,
                Remark = s.Remark
            });

            var data = datas.GroupBy(s => s.ProductionOrderNo).Select(s => new OutputPackagingProductionOrderGroupedViewModel
            {
                ProductionOrder = s.Key,
                ProductionOrderList = s.ToList()
            });

            return new ListResult<OutputPackagingProductionOrderGroupedViewModel>(data.ToList(),page,size,data.Count());
        }


        public ICollection<OutputPackagingProductionOrderViewModel> MapModeltoModelView(List<DyeingPrintingAreaInputProductionOrderModel> source)
        {
            List<OutputPackagingProductionOrderViewModel> result = new List<OutputPackagingProductionOrderViewModel>();
            foreach (var d in source)
            {
                result.Add(new OutputPackagingProductionOrderViewModel
                {
                    Balance = d.Balance,
                    Buyer = d.Buyer,
                    BuyerId = d.BuyerId,
                    CartNo = d.CartNo,
                    Color = d.Color,
                    Construction = d.Construction,
                    Remark = d.Remark,
                    Motif = d.Motif,
                    ProductionOrder = new ProductionOrder()
                    {
                        Id = d.ProductionOrderId,
                        No = d.ProductionOrderNo,
                        Type = d.ProductionOrderType
                    },
                    Grade = d.Grade,
                    Id = d.Id,
                    Unit = d.Unit,
                    Material = d.Construction,
                    PackingInstruction = d.PackingInstruction,
                    UomUnit = d.UomUnit,
                    ProductionOrderNo = d.ProductionOrderNo,
                    QtyOrder = d.ProductionOrderOrderQuantity
                });
            }
            return result;
        }

        public async Task<int> Delete(int bonId)
        {
            var result = 0;
            var bonOutput = _repository.ReadAll().FirstOrDefault(x => x.Id == bonId && x.DyeingPrintingAreaOutputProductionOrders.Any(s=>!s.HasNextAreaDocument));
            if (bonOutput != null)
            {
                var listBonInputByBonOutput = _inputProductionOrderRepository.ReadAll().Join(bonOutput.DyeingPrintingAreaOutputProductionOrders,
                                                                              sppInput => sppInput.Id,
                                                                              sppOutput => sppOutput.DyeingPrintingAreaInputProductionOrderId,
                                                                              (sppInput, sppOutput) => new { Input = sppInput, Output = sppOutput});
                foreach (var spp in listBonInputByBonOutput)
                {
                    spp.Input.SetHasOutputDocument(false, "OUTPACKINGSERVICE", "SERVICE");

                    //update balance remains
                    var newBalance = spp.Input.BalanceRemains + spp.Output.Balance;

                    spp.Input.SetBalanceRemains(newBalance, "OUTPACKINGSERVICE", "SERVICE");
                    result += await _inputProductionOrderRepository.UpdateAsync(spp.Input.Id, spp.Input);
                }
            }
            result += await _repository.DeleteAsync(bonId);

            return result;
        }

        public async Task<int> DeleteV2(int bonId)
        {
            var result = 0;
            var bonOutput = _repository.ReadAll().FirstOrDefault(x => x.Id == bonId && x.DyeingPrintingAreaOutputProductionOrders.Any(s => !s.HasNextAreaDocument));
            // get all SPP backup balance remains
            List<DyeingPrintingAreaInputProductionOrderModel> listBackUpSpp = new List<DyeingPrintingAreaInputProductionOrderModel>();
            if (bonOutput != null)
            {
                var listBonInputByBonOutput = bonOutput.DyeingPrintingAreaOutputProductionOrders.Select(s => s.PrevSppInJson).ToList();
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore,
                    FloatFormatHandling = FloatFormatHandling.DefaultValue,
                    FloatParseHandling = FloatParseHandling.Double
                };
                //var str = "[{'ProductionOrderId':63,'ProductionOrderNo':'F/2020/0010','MaterialId':0,'MaterialName':null,'MaterialConstructionId':0,'MaterialConstructionName':null,'MaterialWidth':null,'CartNo':'KER','BuyerId':551,'Buyer':'ERWAN KURNIADI','Construction':'Greige Test Dyeing Printing / TWILL 3/1. 104 x 52 / 100','Unit':'DYEING','Color':'Brown','Motif':null,'UomUnit':'MTR','Balance':5000.0,'HasOutputDocument':false,'IsChecked':false,'PackingInstruction':'a','ProductionOrderType':'SOLID','ProductionOrderOrderQuantity':5000.0,'Remark':'A','Grade':'A','Status':null,'InitLength':0.0,'AvalALength':0.0,'AvalBLength':0.0,'AvalConnectionLength':0.0,'AvalType':null,'AvalCartNo':null,'AvalMachine':null,'DeliveryOrderSalesId':0,'DeliveryOrderSalesNo':null,'PackagingUnit':null,'PackagingType':null,'PackagingQty':0.00,'Area':'PACKING','BalanceRemains':2500.0,'InputAvalBonNo':null,'AvalQuantityKg':0.0,'AvalQuantity':0.0,'DyeingPrintingAreaInputId':6,'DyeingPrintingAreaOutputProductionOrderId':3,'DyeingPrintingAreaInput':{'Date':'2020-06-26T17:00:00+00:00','Area':'PACKING','Shift':'PAGI','BonNo':'PC.20.0003','Group':'A','AvalType':null,'TotalAvalQuantity':0.0,'TotalAvalWeight':0.0,'IsTransformedAval':false,'DyeingPrintingAreaInputProductionOrders':[{'ProductionOrderId':63,'ProductionOrderNo':'F/2020/0010','MaterialId':0,'MaterialName':null,'MaterialConstructionId':0,'MaterialConstructionName':null,'MaterialWidth':null,'CartNo':'KAR','BuyerId':551,'Buyer':'ERWAN KURNIADI','Construction':'Greige Test Dyeing Printing / TWILL 3/1. 104 x 52 / 100','Unit':'DYEING','Color':'Brown','Motif':null,'UomUnit':'MTR','Balance':2500.0,'HasOutputDocument':false,'IsChecked':false,'PackingInstruction':'a','ProductionOrderType':'SOLID','ProductionOrderOrderQuantity':5000.0,'Remark':'A','Grade':'A','Status':null,'InitLength':0.0,'AvalALength':0.0,'AvalBLength':0.0,'AvalConnectionLength':0.0,'AvalType':null,'AvalCartNo':null,'AvalMachine':null,'DeliveryOrderSalesId':0,'DeliveryOrderSalesNo':null,'PackagingUnit':null,'PackagingType':null,'PackagingQty':0.00,'Area':'PACKING','BalanceRemains':2500.0,'InputAvalBonNo':null,'AvalQuantityKg':0.0,'AvalQuantity':0.0,'DyeingPrintingAreaInputId':6,'DyeingPrintingAreaOutputProductionOrderId':1,'Active':false,'CreatedUtc':'2020-06-24T20:36:35.9994134','CreatedBy':'dev2','CreatedAgent':'Repository','LastModifiedUtc':'2020-06-24T20:36:35.9994138','LastModifiedBy':'dev2','LastModifiedAgent':'Repository','IsDeleted':false,'DeletedUtc':'0001-01-01T00:00:00','DeletedBy':'','DeletedAgent':'','Id':6}],'Active':false,'CreatedUtc':'2020-06-24T20:36:35.9994044','CreatedBy':'dev2','CreatedAgent':'Repository','LastModifiedUtc':'2020-06-24T20:36:35.9994055','LastModifiedBy':'dev2','LastModifiedAgent':'Repository','IsDeleted':false,'DeletedUtc':'0001-01-01T00:00:00','DeletedBy':'','DeletedAgent':'','Id':6},'Active':false,'CreatedUtc':'2020-06-24T20:36:35.9994089','CreatedBy':'dev2','CreatedAgent':'Repository','LastModifiedUtc':'2020-06-24T21:08:44.9945567Z','LastModifiedBy':'OUTPUTPACKAGING','LastModifiedAgent':'SERVICE','IsDeleted':false,'DeletedUtc':'0001-01-01T00:00:00','DeletedBy':'','DeletedAgent':'','Id':4}]";
                //var test = JArray.Parse(str);
                //var test2 = test[0][]
                //foreach(var i in test)
                //{
                //    var a = i.ToObject<DyeingPrintingAreaInputProductionOrderModel>();
                //}
                //var listtest = test.Select(s => new DyeingPrintingAreaInputProductionOrderModel((string)s["Area"], (int)s["ProductionOrderId"], (string)s["ProductionOrderNo"], (string)s["ProductionOrder"], (string)s["PackingInstruction"], (string)s["CartNo"], (string)s["Buyer"], (string)s["Construction"],
                //     (string)s["Unit"], (string)s["Color"], (string)s["Motif"], (string)s["UomUnit"], (double)s["ProductionOrderOrderQuantity"], false, (double)s["QtyOrder"], (string)s["Grade"], (double)s["Balance"], (int)s["BuyerId"], (int)s["Id"], (string)s["Remark"]));

                //var listtest = test.First.ToObject<DyeingPrintingAreaInputProductionOrderModel>();
                foreach (var spp in listBonInputByBonOutput)
                {
                    var listSPP = JsonConvert.DeserializeObject<DyeingPrintingAreaInputProductionOrderModel[]>(spp,settings);
                    //var sppObject = 

                    foreach (var sppBck in listSPP)
                    {
                        //update balance remains
                        var modelToUpdate = _inputProductionOrderRepository.ReadAll().Where(x => x.Id == sppBck.Id).ToList();
                        foreach(var model in modelToUpdate)
                        {
                            var newBalance = model.BalanceRemains + sppBck.BalanceRemains;
                            model.SetBalanceRemains(newBalance, "OUTPUTPACKING", "SERVICE");
                            model.SetHasOutputDocument(false, "OUTPUTPACKING", "SERVICE");

                            result += await _inputProductionOrderRepository.UpdateAsync(sppBck.Id, model);
                        }
                    }
                }
            }
            result += await _repository.DeleteAsync(bonId);

            return result;
        }

        public async Task<int> CreateAdj(OutputPackagingViewModel viewModel)
        {
            return await InsertAdj(viewModel);
        }
    }
    internal static class Extensions
    {
        public static string ToDescription(this Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());
            DescriptionAttribute attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
            return attribute == null ? value.ToString() : attribute.Description;
        }
        //public static Dictionary<int, string> ToDictionaries(Enum myEnum)
        //{
        //    var myEnumType = myEnum.GetType();
        //    var names = myEnumType.GetFields()
        //        .Where(m => m.GetCustomAttribute<DisplayAttribute>() != null)
        //        .Select(e => e.GetCustomAttribute<DisplayAttribute>().Name);
        //    var values = Enum.GetValues(myEnumType).Cast<int>();
        //    return names.Zip(values, (n, v) => new KeyValuePair<int, string>(v, n))
        //        .ToDictionary(kv => kv.Key, kv => kv.Value);
        //}
        //public static Enum ToObject(this Enum value)
        //{
        //    FieldInfo field = value.GetType().GetField(value.ToString());
        //    DescriptionAttribute attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
        //    return attribute == null ? value.ToString() : attribute.Description;
        //}
    }
}
