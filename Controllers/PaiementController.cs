using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ges_commande.Models.entity;
using GesCommande.core;
using ges_commande.Models.Enum;

namespace ges_commande.Controllers
{
    public class PaiementController : Controller
    {
        private readonly Context _context;

        public PaiementController(Context context)
        {
            _context = context;
        }

        // GET: Paiement
        public async Task<IActionResult> Index()
        {
            var context = _context.Paiements.Include(p => p.Commande);
            return View(await context.ToListAsync());
        }

        // GET: Paiement/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paiement = await _context.Paiements
                .Include(p => p.Commande)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (paiement == null)
            {
                return NotFound();
            }

            return View(paiement);
        }

        // GET: Paiement/Create
        public IActionResult Create()
        {
            ViewData["CommandeId"] = new SelectList(_context.Commandes, "Id", "Id");
            return View();
        }

        // POST: Paiement/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("typePaiement,CommandeId,Id,CreateAt,UpdateAt")] Paiement paiement)
        {
            if (ModelState.IsValid)
            {
                _context.Add(paiement);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CommandeId"] = new SelectList(_context.Commandes, "Id", "Id", paiement.CommandeId);
            return View(paiement);
        }

        // GET: Paiement/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paiement = await _context.Paiements.FindAsync(id);
            if (paiement == null)
            {
                return NotFound();
            }
            ViewData["CommandeId"] = new SelectList(_context.Commandes, "Id", "Id", paiement.CommandeId);
            return View(paiement);
        }

        // POST: Paiement/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("typePaiement,CommandeId,Id,CreateAt,UpdateAt")] Paiement paiement)
        {
            if (id != paiement.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(paiement);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PaiementExists(paiement.Id))
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
            ViewData["CommandeId"] = new SelectList(_context.Commandes, "Id", "Id", paiement.CommandeId);
            return View(paiement);
        }

        // GET: Paiement/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paiement = await _context.Paiements
                .Include(p => p.Commande)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (paiement == null)
            {
                return NotFound();
            }

            return View(paiement);
        }

        // POST: Paiement/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var paiement = await _context.Paiements.FindAsync(id);
            if (paiement != null)
            {
                _context.Paiements.Remove(paiement);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PaiementExists(int id)
        {
            return _context.Paiements.Any(e => e.Id == id);
        }

                [HttpGet]
public IActionResult Paiement(int id)
{
    // Vérifier si la commande existe
    var commande = _context.Commandes
                           .Include(c => c.Client)  // Charger le client associé
                           .FirstOrDefault(c => c.Id == id);

    if (commande == null)
    {
        return NotFound(); // Si la commande n'existe pas
    }

    // Passer la commande à la vue pour afficher les détails
    return View(commande);
}


[HttpPost]
[ValidateAntiForgeryToken]
public IActionResult PaiementConfirmed(int id,string moyenPaiement)
{
    // Récupérer la commande
    var commande = _context.Commandes.FirstOrDefault(c => c.Id == id);
    if (commande == null)
    {
        return NotFound();
    }
var typePaiement = TypePaiementHelper.GetValue(moyenPaiement);
    if (typePaiement == null)
    {
        return BadRequest("Moyen de paiement invalide");
    }

    // Vérifier si un paiement existe déjà, sinon en créer un
    if (commande.Paiement == null)
    {
        commande.Paiement = new Paiement();  // Créer une nouvelle instance de Paiement
    }
    // Simuler un paiement en mettant à jour l'état de la commande
    commande.Etat = Etat.Payer;
    commande.Paiement.typePaiement = typePaiement.Value;
    commande.Paiement.CreateAt = DateTime.Now;
    commande.UpdateAt = DateTime.Now;

    _context.SaveChanges();

    // Rediriger vers la liste des commandes après paiement
    return RedirectToAction("CommandeClient" ,"Commande");
}
    }
}
