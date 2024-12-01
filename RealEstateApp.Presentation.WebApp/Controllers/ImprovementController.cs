using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Application.Interfaces.Services.Improvements;
using RealEstateApp.Application.ViewModels.Improvements;
using RealEstateApp.Domain.Entities;

namespace WebApplication1.Controllers
{
    public class ImprovementController : Controller
    {
        private readonly IImprovementService _service;
        private readonly IMapper _mapper;

        public ImprovementController(IImprovementService improvement, IMapper mapper)
        {
            _mapper = mapper;
            _service = improvement;
        }

        public async Task<IActionResult> Index()
        {
            var improvement = await _service.GetAllAsync();
            var map = _mapper.Map<List<ImprovementsListViewModel>>(improvement);

            return View(map);
        }

        [HttpGet]
        public IActionResult create()
        {
            return View(new CreateImprovementViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> create(CreateImprovementViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var map = _mapper.Map<Improvement>(vm);
                await _service.AddAsync(map);

                return RedirectToAction(nameof(Index));
            }

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> edit(int id)
        {
            var improvement = await _service.GetByIdAsync(id);
            if(improvement == null) NotFound();

            var editvm = _mapper.Map<UpdateImprovementViewModel>(improvement);

            return View(editvm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateImprovementViewModel vm)
        {
            if(ModelState.IsValid)
            {
                var improvement = await _service.GetByIdAsync(id);
                if (improvement == null) NotFound();

                _mapper.Map(vm, improvement);
                await _service.UpdateAsync(improvement);

                return RedirectToAction(nameof(Index));
            }
            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var improvement = await _service.GetByIdAsync(id);
            if (improvement == null) return NotFound();

            var vm = _mapper.Map<DeleteImprovementViewModel>(improvement);

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfi(int id)
        {
            await _service.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
