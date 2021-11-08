using Crud.Data;
using Crud.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Crud.Controllers
{
    public class MediosDePagosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MediosDePagosController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            IEnumerable<MediosDePagos> listaMedios = _context.MediosDePagos;
            return View(listaMedios);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(string searchString)
        {
            var medios = from m in _context.MediosDePagos
                         select m;

            if (!string.IsNullOrEmpty(searchString))
            {
                medios = medios.Where(s => s.Detalle.Contains(searchString));
            }

            return View(medios);
        }


        public IActionResult Create()
        {
            return View();
        }

        //Http Get Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(MediosDePagos medio)
        {
            if(ModelState.IsValid)
            {
                medio.FechaCreacion = DateTime.Now;
                _context.MediosDePagos.Add(medio);
                _context.SaveChanges();
                TempData["mensaje"] = "el medio de pago se ha creado correctamente";
                return RedirectToAction("Index");
            }
            return View();
        }

        //Http Get Edit
        public IActionResult Edit(int? id)
        {
            if(id == null || id == 0) 
            {
                return NotFound();
            }

            //obtener el medio
            var medio = _context.MediosDePagos.Find(id);

            if (medio == null)
            {
                return NotFound();
            }

            return View(medio);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(MediosDePagos medio)
        {
            if (ModelState.IsValid)
            {
                _context.MediosDePagos.Update(medio);
                _context.SaveChanges();
                TempData["mensaje"] = "el medio de pago se ha actualizado correctamente";
                return RedirectToAction("Index");
            }
            return View();
        }

        //Http Get Delete
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            //obtener el medio
            var medio = _context.MediosDePagos.Find(id);

            if (medio == null)
            {
                return NotFound();
            }

            return View(medio);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteMedio(int? id)
        {
            //obtener medio por Id
            var medio = _context.MediosDePagos.Find(id);

            if (medio == null)
            {
                return NotFound();
            }

            _context.MediosDePagos.Remove(medio);
            _context.SaveChanges();
            TempData["mensaje"] = "el medio de pago se ha eliminado correctamente";
            return RedirectToAction("Index");

        }


    }
}