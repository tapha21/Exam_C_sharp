using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ges_commande.Models.entity;
using GesCommande.core;

namespace ges_commande.Controllers
{
    public class ProduitController : Controller
    {
        private readonly Context _context;
        private readonly string _baseImageUrl = "http://localhost:5204/images";

        public ProduitController(Context context)
        {
            _context = context;
        }

        [Route("Produit/MesProduit")]
        public async Task<IActionResult> MesProduit(string disponibilite = "")
        {
            var query = _context.Produits.AsQueryable();

            if (disponibilite == "true")
            {
                query = query.Where(p => p.Quantite > 0);
            }
            else if (disponibilite == "false")
            {
                query = query.Where(p => p.Quantite <= 0);
            }

            ViewBag.Disponibilite = disponibilite;
            return View(await query.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produit = await _context.Produits.FirstOrDefaultAsync(m => m.Id == id);
            if (produit == null)
            {
                return NotFound();
            }

            return View(produit);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Libelle,Prix,Quantite,Id,CreateAt,UpdateAt")] Produit produit, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                if (image != null && image.Length > 0)
                {
                    var imageFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                    if (!Directory.Exists(imageFolder))
                    {
                        Directory.CreateDirectory(imageFolder);
                    }

                    var fileName = Guid.NewGuid() + Path.GetExtension(image.FileName);
                    var filePath = Path.Combine(imageFolder, fileName);

                    try
                    {
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await image.CopyToAsync(stream);
                        }

                        produit.image = $"{_baseImageUrl}/{fileName}";
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", "Erreur lors de l'upload de l'image : " + ex.Message);
                        return View(produit);
                    }
                }

                try
                {
                    _context.Add(produit);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(MesProduit));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Erreur lors de l'enregistrement du produit : " + ex.Message);
                }
            }

            return View(produit);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produit = await _context.Produits.FindAsync(id);
            if (produit == null)
            {
                return NotFound();
            }

            return View(produit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Libelle,Prix,Quantite,Id,CreateAt,UpdateAt")] Produit produit)
        {
            if (id != produit.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(produit);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProduitExists(produit.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(MesProduit));
            }

            return View(produit);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produit = await _context.Produits.FirstOrDefaultAsync(m => m.Id == id);
            if (produit == null)
            {
                return NotFound();
            }

            return View(produit);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var produit = await _context.Produits.FindAsync(id);
            if (produit != null)
            {
                _context.Produits.Remove(produit);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(MesProduit));
        }

        private bool ProduitExists(int id)
        {
            return _context.Produits.Any(e => e.Id == id);
        }

        public async Task<IActionResult> Clientviews()
        {
            var produits = await _context.Produits.ToListAsync();

            if (produits == null || !produits.Any())
            {
                return NotFound();
            }

            return View(produits);
        }

        public async Task<IActionResult> Panier()
        {
            var produits = await _context.Produits.ToListAsync();
            return View(produits);
        }

        public async Task<IActionResult> Supprimer(int id)
        {
            var produit = await _context.Produits.FindAsync(id);
            if (produit != null)
            {
                _context.Produits.Remove(produit);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Panier");
        }

        public IActionResult Commander()
        {
            return RedirectToAction("Panier");
        }
    }
}
