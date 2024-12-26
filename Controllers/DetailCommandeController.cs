using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ges_commande.Models.entity;
using GesCommande.core;

namespace ges_commande.Controllers
{
    public class DetailCommandeController : Controller
    {
        private readonly Context _context;

        public DetailCommandeController(Context context)
        {
            _context = context;
        }

        // GET: DetailCommande
        public async Task<IActionResult> Index()
        {
            var context = _context.DetailCommandes.Include(d => d.Commande).Include(d => d.Produit);
            return View(await context.ToListAsync());
        }

        // GET: DetailCommande/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detailCommande = await _context.DetailCommandes
                .Include(d => d.Commande)
                .Include(d => d.Produit)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (detailCommande == null)
            {
                return NotFound();
            }

            return View(detailCommande);
        }

        // GET: DetailCommande/Create
        public IActionResult Create()
        {
            ViewData["CommandeId"] = new SelectList(_context.Commandes, "Id", "Id");
            ViewData["ProduitId"] = new SelectList(_context.Produits, "Id", "Id");
            return View();
        }

        // POST: DetailCommande/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CommandeId,ProduitId,QuantiteComamnde,prixTotal,Id,CreateAt,UpdateAt")] DetailCommande detailCommande)
        {
            if (ModelState.IsValid)
            {
                _context.Add(detailCommande);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CommandeId"] = new SelectList(_context.Commandes, "Id", "Id", detailCommande.CommandeId);
            ViewData["ProduitId"] = new SelectList(_context.Produits, "Id", "Id", detailCommande.ProduitId);
            return View(detailCommande);
        }

        // GET: DetailCommande/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detailCommande = await _context.DetailCommandes.FindAsync(id);
            if (detailCommande == null)
            {
                return NotFound();
            }
            ViewData["CommandeId"] = new SelectList(_context.Commandes, "Id", "Id", detailCommande.CommandeId);
            ViewData["ProduitId"] = new SelectList(_context.Produits, "Id", "Id", detailCommande.ProduitId);
            return View(detailCommande);
        }

        // POST: DetailCommande/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CommandeId,ProduitId,QuantiteComamnde,prixTotal,Id,CreateAt,UpdateAt")] DetailCommande detailCommande)
        {
            if (id != detailCommande.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(detailCommande);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DetailCommandeExists(detailCommande.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CommandeId"] = new SelectList(_context.Commandes, "Id", "Id", detailCommande.CommandeId);
            ViewData["ProduitId"] = new SelectList(_context.Produits, "Id", "Id", detailCommande.ProduitId);
            return View(detailCommande);
        }

        // GET: DetailCommande/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detailCommande = await _context.DetailCommandes
                .Include(d => d.Commande)
                .Include(d => d.Produit)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (detailCommande == null)
            {
                return NotFound();
            }

            return View(detailCommande);
        }

        // POST: DetailCommande/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var detailCommande = await _context.DetailCommandes.FindAsync(id);
            if (detailCommande != null)
            {
                _context.DetailCommandes.Remove(detailCommande);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DetailCommandeExists(int id)
        {
            return _context.DetailCommandes.Any(e => e.Id == id);
        }
    }
}
