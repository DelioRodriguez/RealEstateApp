using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Application.Interfaces.Services.PropertiesType;
using RealEstateApp.Application.ViewModels.PropertiesType;
using RealEstateApp.Domain.Entities;

namespace WebApplication1.Controllers
{
    public class PropertiesTypeController : Controller
    {
        private readonly IPropertiesTypeServices _propertyTypeService;
        private readonly IMapper _mapper;

        public PropertiesTypeController(IPropertiesTypeServices propertyTypeService, IMapper mapper)
        {
            _propertyTypeService = propertyTypeService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var propertyTypes = await _propertyTypeService.GetAllAsync();
            var propertyTypeList = _mapper.Map<List<PropertyTypeListViewModel>>(propertyTypes);

            foreach (var item in propertyTypeList)
            {
                item.PropertiesCount = await _propertyTypeService.GetPropertiesCountAsync(item.Id);
            }

            return View(propertyTypeList);
        }

        public IActionResult Create()
        {
            return View(new CreatePropertyTypeViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatePropertyTypeViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var propertyType = _mapper.Map<PropertyType>(viewModel);
                await _propertyTypeService.AddAsync(propertyType);
                return RedirectToAction(nameof(Index));
            }

            return View(viewModel);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var propertyType = await _propertyTypeService.GetByIdAsync(id);
            if (propertyType == null) return NotFound();

            var editViewModel = _mapper.Map<EditPropertyTypeViewModel>(propertyType);
            return View(editViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditPropertyTypeViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var propertyType = await _propertyTypeService.GetByIdAsync(id);
                if (propertyType == null) return NotFound();

                _mapper.Map(viewModel, propertyType);
                await _propertyTypeService.UpdateAsync(propertyType);

                return RedirectToAction(nameof(Index));
            }

            return View(viewModel);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var propertyType = await _propertyTypeService.GetByIdAsync(id);
            if (propertyType == null) return NotFound();

            var viewModel = _mapper.Map<DeletePropertyTypeViewModel>(propertyType);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _propertyTypeService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
