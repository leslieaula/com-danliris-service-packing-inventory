﻿using Com.Danliris.Service.Packing.Inventory.Application.CommonViewModelObjectProperties;
using Com.Danliris.Service.Packing.Inventory.Application.ToBeRefactored.GarmentShipping.GarmentShippingInvoice;
using Com.Danliris.Service.Packing.Inventory.Application.Utilities;
using Com.Danliris.Service.Packing.Inventory.Data.Models.Garmentshipping.GarmentPackingList;
using Com.Danliris.Service.Packing.Inventory.Infrastructure.IdentityProvider;
using Com.Danliris.Service.Packing.Inventory.Infrastructure.Repositories.GarmentShipping.GarmentPackingList;
using Com.Danliris.Service.Packing.Inventory.Infrastructure.Repositories.GarmentShipping.GarmentShippingInvoice;
using Com.Danliris.Service.Packing.Inventory.Infrastructure.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Packing.Inventory.Application.ToBeRefactored.GarmentShipping.GarmentPackingList
{
    public class GarmentPackingListService : IGarmentPackingListService
    {
        private const string UserAgent = "GarmentPackingListService";

        private readonly IGarmentPackingListRepository _packingListRepository;
        private readonly IGarmentShippingInvoiceRepository _invoiceRepository;
        private readonly IIdentityProvider _identityProvider;

        public GarmentPackingListService(IServiceProvider serviceProvider)
        {
            _packingListRepository = serviceProvider.GetService<IGarmentPackingListRepository>();
            _invoiceRepository = serviceProvider.GetService<IGarmentShippingInvoiceRepository>();
            _identityProvider = serviceProvider.GetService<IIdentityProvider>();
        }

        private GarmentPackingListViewModel MapToViewModel(GarmentPackingListModel model)
        {
            var vm =  new GarmentPackingListViewModel()
            {
                Active = model.Active,
                Id = model.Id,
                CreatedAgent = model.CreatedAgent,
                CreatedBy = model.CreatedBy,
                CreatedUtc = model.CreatedUtc,
                DeletedAgent = model.DeletedAgent,
                DeletedBy = model.DeletedBy,
                DeletedUtc = model.DeletedUtc,
                IsDeleted = model.IsDeleted,
                LastModifiedAgent = model.LastModifiedAgent,
                LastModifiedBy = model.LastModifiedBy,
                LastModifiedUtc = model.LastModifiedUtc,

                InvoiceNo = model.InvoiceNo,
                PackingListType = model.PackingListType,
                InvoiceType = model.InvoiceType,
                Section = new Section
                {
                    Id = model.SectionId,
                    Code = model.SectionCode
                },
                Date = model.Date,
                PaymentTerm = model.PaymentTerm,
                LCNo = model.LCNo,
                LCDate = model.LCDate,
                IssuedBy = model.IssuedBy,
                BuyerAgent = new Buyer
                {
                    Id = model.BuyerAgentId,
                    Code = model.BuyerAgentCode,
                    Name = model.BuyerAgentName,
                },
                Destination = model.Destination,
                ShipmentMode = model.ShipmentMode,
                TruckingDate = model.TruckingDate,
                TruckingEstimationDate = model.TruckingEstimationDate,
                ExportEstimationDate = model.ExportEstimationDate,
                Omzet = model.Omzet,
                Accounting = model.Accounting,
                FabricCountryOrigin = model.FabricCountryOrigin,
                FabricComposition = model.FabricComposition,
                RemarkMd = model.RemarkMd,
                IsUsed = model.IsUsed,
                IsPosted = model.IsPosted,
                Status = model.Status.ToString(),
                Items = (model.Items ?? new List<GarmentPackingListItemModel>()).Select(i => new GarmentPackingListItemViewModel
                {
                    Active = i.Active,
                    Id = i.Id,
                    CreatedAgent = i.CreatedAgent,
                    CreatedBy = i.CreatedBy,
                    CreatedUtc = i.CreatedUtc,
                    DeletedAgent = i.DeletedAgent,
                    DeletedBy = i.DeletedBy,
                    DeletedUtc = i.DeletedUtc,
                    IsDeleted = i.IsDeleted,
                    LastModifiedAgent = i.LastModifiedAgent,
                    LastModifiedBy = i.LastModifiedBy,
                    LastModifiedUtc = i.LastModifiedUtc,

                    RONo = i.RONo,
                    SCNo = i.SCNo,
                    BuyerBrand = new Buyer
                    {
                        Id = i.BuyerBrandId,
                        Name = i.BuyerBrandName
                    },
                    Comodity = new Comodity
                    {
                        Id = i.ComodityId,
                        Code = i.ComodityCode,
                        Name = i.ComodityName
                    },
                    ComodityDescription = i.ComodityDescription,
                    Quantity = i.Quantity,
                    Uom = new UnitOfMeasurement
                    {
                        Id = i.UomId,
                        Unit = i.UomUnit
                    },
                    PriceRO = i.PriceRO,
                    Price = i.Price,
                    PriceFOB = i.PriceFOB,
                    PriceCMT = i.PriceCMT,
                    Amount = i.Amount,
                    Valas = i.Valas,
                    Unit = new Unit
                    {
                        Id = i.UnitId,
                        Code = i.UnitCode
                    },
                    Article = i.Article,
                    OrderNo = i.OrderNo,
                    Description = i.Description,
                    DescriptionMd = i.DescriptionMd,

                    Details = (i.Details ?? new List<GarmentPackingListDetailModel>()).Select(d => new GarmentPackingListDetailViewModel
                    {
                        Active = d.Active,
                        Id = d.Id,
                        CreatedAgent = d.CreatedAgent,
                        CreatedBy = d.CreatedBy,
                        CreatedUtc = d.CreatedUtc,
                        DeletedAgent = d.DeletedAgent,
                        DeletedBy = d.DeletedBy,
                        DeletedUtc = d.DeletedUtc,
                        IsDeleted = d.IsDeleted,
                        LastModifiedAgent = d.LastModifiedAgent,
                        LastModifiedBy = d.LastModifiedBy,
                        LastModifiedUtc = d.LastModifiedUtc,

                        Carton1 = d.Carton1,
                        Carton2 = d.Carton2,
                        Colour = d.Colour,
                        CartonQuantity = d.CartonQuantity,
                        QuantityPCS = d.QuantityPCS,
                        TotalQuantity = d.TotalQuantity,

                        Length = d.Length,
                        Width = d.Width,
                        Height = d.Height,
                        CartonsQuantity = d.CartonsQuantity,

                        Sizes = (d.Sizes ?? new List<GarmentPackingListDetailSizeModel>()).Select(s => new GarmentPackingListDetailSizeViewModel
                        {
                            Active = s.Active,
                            Id = s.Id,
                            CreatedAgent = s.CreatedAgent,
                            CreatedBy = s.CreatedBy,
                            CreatedUtc = s.CreatedUtc,
                            DeletedAgent = s.DeletedAgent,
                            DeletedBy = s.DeletedBy,
                            DeletedUtc = s.DeletedUtc,
                            IsDeleted = s.IsDeleted,
                            LastModifiedAgent = s.LastModifiedAgent,
                            LastModifiedBy = s.LastModifiedBy,
                            LastModifiedUtc = s.LastModifiedUtc,

                            Size = new SizeViewModel
                            {
                                Id = s.SizeId,
                                Size = s.Size
                            },
                            Quantity = s.Quantity
                        }).ToList()

                    }).ToList(),

                    AVG_GW = i.AVG_GW,
                    AVG_NW = i.AVG_NW,
                }).ToList(),

                GrossWeight = model.GrossWeight,
                NettWeight = model.NettWeight,
                TotalCartons = model.TotalCartons,
                Measurements = (model.Measurements ?? new List<GarmentPackingListMeasurementModel>()).Select(m => new GarmentPackingListMeasurementViewModel
                {
                    Active = m.Active,
                    Id = m.Id,
                    CreatedAgent = m.CreatedAgent,
                    CreatedBy = m.CreatedBy,
                    CreatedUtc = m.CreatedUtc,
                    DeletedAgent = m.DeletedAgent,
                    DeletedBy = m.DeletedBy,
                    DeletedUtc = m.DeletedUtc,
                    IsDeleted = m.IsDeleted,
                    LastModifiedAgent = m.LastModifiedAgent,
                    LastModifiedBy = m.LastModifiedBy,
                    LastModifiedUtc = m.LastModifiedUtc,

                    Length = m.Length,
                    Width = m.Width,
                    Height = m.Height,
                    CartonsQuantity = m.CartonsQuantity,

                }).ToList(),
                SayUnit = model.SayUnit,

                ShippingMark = model.ShippingMark,
                SideMark = model.SideMark,
                Remark = model.Remark,
            };
            return vm;
        }

        private GarmentPackingListModel MapToModel(GarmentPackingListViewModel viewModel)
        {
            var items = (viewModel.Items ?? new List<GarmentPackingListItemViewModel>()).Select(i =>
            {
                var details = (i.Details ?? new List<GarmentPackingListDetailViewModel>()).Select(d =>
                {
                    var sizes = (d.Sizes ?? new List<GarmentPackingListDetailSizeViewModel>()).Select(s =>
                    {
                        s.Size = s.Size ?? new SizeViewModel();
                        return new GarmentPackingListDetailSizeModel(s.Size.Id, s.Size.Size, s.Quantity) { Id = s.Id };
                    }).ToList();

                    return new GarmentPackingListDetailModel(d.Carton1, d.Carton2, d.Colour, d.CartonQuantity, d.QuantityPCS, d.TotalQuantity, d.Length, d.Width, d.Height, d.CartonsQuantity, sizes) { Id = d.Id };
                }).ToList();

                i.BuyerBrand = i.BuyerBrand ?? new Buyer();
                i.Uom = i.Uom ?? new UnitOfMeasurement();
                i.Unit = i.Unit ?? new Unit();
                i.Comodity = i.Comodity ?? new Comodity();
                return new GarmentPackingListItemModel(i.RONo, i.SCNo, i.BuyerBrand.Id, i.BuyerBrand.Name, i.Comodity.Id, i.Comodity.Code, i.Comodity.Name, i.ComodityDescription, i.Quantity, i.Uom.Id.GetValueOrDefault(), i.Uom.Unit, i.PriceRO, i.Price, i.PriceFOB, i.PriceCMT, i.Amount, i.Valas, i.Unit.Id, i.Unit.Code, i.Article, i.OrderNo, i.Description, i.DescriptionMd, details, i.AVG_GW, i.AVG_NW)
                {
                    Id = i.Id
                };
            }).ToList();

            var measurements = (viewModel.Measurements ?? new List<GarmentPackingListMeasurementViewModel>()).Select(m => new GarmentPackingListMeasurementModel(m.Length, m.Width, m.Height, m.CartonsQuantity)
            {
                Id = m.Id
            }).ToList();

            viewModel.Section = viewModel.Section ?? new Section();
            viewModel.BuyerAgent = viewModel.BuyerAgent ?? new Buyer();
            viewModel.InvoiceNo = GenerateInvoiceNo(viewModel);
            Enum.TryParse(viewModel.Status, true, out GarmentPackingListStatusEnum status);
            GarmentPackingListModel garmentPackingListModel = new GarmentPackingListModel(viewModel.InvoiceNo, viewModel.PackingListType, viewModel.InvoiceType, viewModel.Section.Id, viewModel.Section.Code, viewModel.Date.GetValueOrDefault(), viewModel.PaymentTerm, viewModel.LCNo, viewModel.LCDate.GetValueOrDefault(), viewModel.IssuedBy, viewModel.BuyerAgent.Id, viewModel.BuyerAgent.Code, viewModel.BuyerAgent.Name, viewModel.Destination, viewModel.ShipmentMode, viewModel.TruckingDate.GetValueOrDefault(), viewModel.TruckingEstimationDate.GetValueOrDefault(), viewModel.ExportEstimationDate.GetValueOrDefault(), viewModel.Omzet, viewModel.Accounting, viewModel.FabricCountryOrigin, viewModel.FabricComposition, viewModel.RemarkMd, items, viewModel.GrossWeight, viewModel.NettWeight, viewModel.TotalCartons, measurements, viewModel.SayUnit, viewModel.ShippingMark, viewModel.SideMark, viewModel.Remark, viewModel.IsUsed, viewModel.IsPosted, status);

            return garmentPackingListModel;
        }

        public async Task<string> Create(GarmentPackingListViewModel viewModel)
        {
            GarmentPackingListModel garmentPackingListModel = MapToModel(viewModel);

            await _packingListRepository.InsertAsync(garmentPackingListModel);

            return garmentPackingListModel.InvoiceNo;
        }

        private string GenerateInvoiceNo(GarmentPackingListViewModel viewModel)
        {
            var year = DateTime.Now.ToString("yy");

            var prefix = $"{(viewModel.InvoiceType ?? "").Trim().ToUpper()}/{year}";

            var lastInvoiceNo = _packingListRepository.ReadAll().Where(w => w.InvoiceNo.StartsWith(prefix))
                .OrderByDescending(o => o.InvoiceNo)
                .Select(s => int.Parse(s.InvoiceNo.Replace(prefix, "")))
                .FirstOrDefault();
            var invoiceNo = $"{prefix}{(lastInvoiceNo + 1).ToString("D4")}";

            return invoiceNo;
        }

        public ListResult<GarmentPackingListViewModel> Read(int page, int size, string filter, string order, string keyword)
        {
            var query = _packingListRepository.ReadAll();
            List<string> SearchAttributes = new List<string>()
            {
                "InvoiceNo", "PackingListType", "SectionCode", "Destination"
            };
            query = QueryHelper<GarmentPackingListModel>.Search(query, SearchAttributes, keyword);

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            query = QueryHelper<GarmentPackingListModel>.Filter(query, FilterDictionary);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            query = QueryHelper<GarmentPackingListModel>.Order(query, OrderDictionary);

            var data = query
                .Skip((page - 1) * size)
                .Take(size)
                .Select(model => MapToViewModel(model))
                .ToList();

            return new ListResult<GarmentPackingListViewModel>(data, page, size, query.Count());
        }

        public async Task<GarmentPackingListViewModel> ReadById(int id)
        {
            var data = await _packingListRepository.ReadByIdAsync(id);

            var viewModel = MapToViewModel(data);

            return viewModel;
        }

        public async Task<GarmentPackingListViewModel> ReadByInvoiceNo(string no)
        {
            var data = await _packingListRepository.ReadByInvoiceNoAsync(no);

            var viewModel = MapToViewModel(data);

            return viewModel;
        }

        public ListResult<GarmentPackingListViewModel> ReadNotUsed(int page, int size, string filter, string order, string keyword)
		{
			var query = _packingListRepository.ReadAll();
			List<string> SearchAttributes = new List<string>()
			{
				"InvoiceNo"
			};
			query = QueryHelper<GarmentPackingListModel>.Search(query, SearchAttributes, keyword);

			Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
			query = QueryHelper<GarmentPackingListModel>.Filter(query, FilterDictionary);

			Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
			query = QueryHelper<GarmentPackingListModel>.Order(query, OrderDictionary);

			var data = query
				.Skip((page - 1) * size)
				.Take(size)
				.Where(s=>s.IsUsed== false)
				.Select(model => MapToViewModel(model))
				.ToList();

			return new ListResult<GarmentPackingListViewModel>(data, page, size, query.Count());
		}



		public async Task<int> Update(int id, GarmentPackingListViewModel viewModel)
        {
            GarmentPackingListModel garmentPackingListModel = MapToModel(viewModel);

            return await _packingListRepository.UpdateAsync(id, garmentPackingListModel);
        }

        public async Task<int> Delete(int id)
        {
            return await _packingListRepository.DeleteAsync(id);
        }

        public async Task<ExcelResult> ReadPdfById(int id)
        {
            var data = await _packingListRepository.ReadByIdAsync(id);

            var PdfTemplate = new GarmentPackingListPdfTemplate(_identityProvider);
            var fob = _invoiceRepository.ReadAll().Where(w => w.PackingListId == data.Id).Select(s => s.From).FirstOrDefault();

            var stream = PdfTemplate.GeneratePdfTemplate(MapToViewModel(data), fob);

            return new ExcelResult(stream, "Packing List " + data.InvoiceNo + ".pdf");
        }

        public async Task SetPost(List<int> ids)
        {
            var models = _packingListRepository.Query.Where(m => ids.Contains(m.Id));
            foreach (var model in models)
            {
                model.SetIsPosted(true, _identityProvider.Username, UserAgent);
                model.SetStatus(GarmentPackingListStatusEnum.POSTED, _identityProvider.Username, UserAgent);
                model.StatusActivities.Add(new GarmentPackingListStatusActivityModel(_identityProvider.Username, UserAgent, GarmentPackingListStatusEnum.POSTED));
            }

            await _packingListRepository.SaveChanges();
        }

        public async Task SetUnpost(int id)
        {
            var model = _packingListRepository.Query.Single(m => m.Id == id);
            model.SetIsPosted(false, _identityProvider.Username, UserAgent);
            model.SetStatus(GarmentPackingListStatusEnum.ON_PROCESS, _identityProvider.Username, UserAgent);
            model.StatusActivities.Add(new GarmentPackingListStatusActivityModel(_identityProvider.Username, UserAgent, GarmentPackingListStatusEnum.ON_PROCESS));

            await _packingListRepository.SaveChanges();
        }

        public async Task SetCancel(int id)
        {
            var model = _packingListRepository.Query.Single(m => m.Id == id);
            model.SetStatus(GarmentPackingListStatusEnum.CANCELED, _identityProvider.Username, UserAgent);
            model.StatusActivities.Add(new GarmentPackingListStatusActivityModel(_identityProvider.Username, UserAgent, GarmentPackingListStatusEnum.CANCELED));

            await _packingListRepository.SaveChanges();
        }

        public async Task SetRejectMd(int id, string remark)
        {
            var model = _packingListRepository.Query.Single(m => m.Id == id);
            model.SetStatus(GarmentPackingListStatusEnum.REJECTED_MD, _identityProvider.Username, UserAgent);
            model.StatusActivities.Add(new GarmentPackingListStatusActivityModel(_identityProvider.Username, UserAgent, GarmentPackingListStatusEnum.REJECTED_MD, remark));

            await _packingListRepository.SaveChanges();
        }

        public async Task SetApproveMd(int id, GarmentPackingListViewModel viewModel)
        {
            GarmentPackingListModel garmentPackingListModel = MapToModel(viewModel);

            await _packingListRepository.UpdateAsync(id, garmentPackingListModel);

            var oldModel = _packingListRepository.Query.Single(m => m.Id == id);
            oldModel.SetStatus(GarmentPackingListStatusEnum.APPROVED_MD, _identityProvider.Username, UserAgent);
            oldModel.StatusActivities.Add(new GarmentPackingListStatusActivityModel(_identityProvider.Username, UserAgent, GarmentPackingListStatusEnum.APPROVED_MD));

            await _packingListRepository.SaveChanges();
        }
    }
}
