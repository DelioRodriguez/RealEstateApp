using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Application.Interfaces.Services.SalesType;
using RealEstateApp.Application.ViewModels.SaleType;
using RealEstateApp.Domain.Entities;

namespace WebApplication1.Controllers
{
    public class SaleTypeController : Controller
    {
        private readonly ISalesTypesService _salesTypesService;
        private readonly IMapper _mapper;

        public SaleTypeController(ISalesTypesService salesTypesService, IMapper mapper)
        {
            _salesTypesService = salesTypesService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var saletype = await _salesTypesService.GetAllAsync();
            var saleTypeList = _mapper.Map < List<SaleTypeListViewModel>>(saletype);

            foreach (var item in saleTypeList)
            {
                item.PropertiesCount = await _salesTypesService.GetPropertiesCountAsync(item.Id);
            }
            return View (saleTypeList);
        }

        public IActionResult Create()
        {
            return View(new CreateSaleTypeViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateSaleTypeViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var saleType = _mapper.Map<SaleType>(viewModel);
                await _salesTypesService.AddAsync(saleType);
                return RedirectToAction(nameof(Index));
            }
            return View(viewModel);    
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var saleType = await _salesTypesService.GetByIdAsync(id);
            if(saleType == null) return NotFound();

            var editVm = _mapper.Map<UpdateSaleTypeViewModel>(saleType);
            return View(editVm);
        }

        [HttpPost]
        public async Task <IActionResult> Edit(int id, UpdateSaleTypeViewModel updateSaleTypeViewModel)
        {
            if(ModelState.IsValid)
            {
                var saletype = await _salesTypesService.GetByIdAsync(id);
                if (saletype == null) return NotFound();

                _mapper.Map(updateSaleTypeViewModel, saletype);
                await _salesTypesService.UpdateAsync(saletype);

                return RedirectToAction(nameof(Index));
            }

            return View(updateSaleTypeViewModel);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var saletype = await _salesTypesService.GetByIdAsync(id);
            if (saletype == null) return NotFound();

            var viewModel = _mapper.Map<DeleteSaleTypeViewModel>(saletype);
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfi(int id)
        {
            await _salesTypesService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
