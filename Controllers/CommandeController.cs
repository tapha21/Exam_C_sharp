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
    public class CommandeController : Controller
    {
        private readonly Context _context;

        public CommandeController(Context context)
        {
            _context = context;
        }

public IActionResult CommandeClient(string etat)
{
    // Vérifier si l'utilisateur est authentifié
    if (!User.Identity.IsAuthenticated)
    {
        return RedirectToAction("Login", "Securite"); // Rediriger vers la page de connexion si l'utilisateur n'est pas authentifié
    }

    var userName = User.Identity.Name;

    var utilisateur = _context.Users
                          .FirstOrDefault(u => u.login == userName); // Assurez-vous que "Nom" est le champ que vous utilisez pour stocker le nom d'utilisateur

    if (utilisateur == null)
    {
        return RedirectToAction("Login", "Securite"); // Si l'utilisateur n'est pas trouvé, rediriger vers la page de connexion
    }

    var clientId = utilisateur.Id;

    // Filtrer les commandes associées à cet utilisateur (client)
    var commandes = _context.Commandes
                             .Where(c => c.ClientId == clientId) // Assurez-vous que clientID fait référence à l'ID du client
                             .ToList();

     if (!string.IsNullOrEmpty(etat))
    {
commandes = commandes.Where(c => c.Etat == EtatHelper.GetValue(etat)).ToList();
    }

    // Passer la liste des commandes à la vue
    return View(commandes);
}

        // GET: Commande
        public async Task<IActionResult> MesCommande()
        {
            var context = _context.Commandes.Include(c => c.Client).Include(c => c.Livreur);
            return View(await context.ToListAsync());
        }

        // GET: Commande/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var commande = await _context.Commandes
                .Include(c => c.Client)
                .Include(c => c.Livreur)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (commande == null)
            {
                return NotFound();
            }

            return View(commande);
        }

        // GET: Commande/Create
        public IActionResult Create()
        {
            ViewData["clientID"] = new SelectList(_context.Clients, "Id", "Id");
            ViewData["livreurID"] = new SelectList(_context.Livreurs, "Id", "Id");
            return View();
        }

        // POST: Commande/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,dateTime,montant,clientID,livreurID,CreateAt,UpdateAt")] Commande commande)
        {
            if (ModelState.IsValid)
            {
                commande.CreateAt = DateTime.Now;
                commande.UpdateAt = DateTime.Now;
                commande.Client = _context.Clients.Find(commande.ClientId);
                commande.Livreur = _context.Livreurs.Find(commande.livreurID);
                _context.Add(commande);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["clientID"] = new SelectList(_context.Clients, "Id", "Text", commande.ClientId);
            ViewData["livreurID"] = new SelectList(_context.Livreurs, "Id", "Text", commande.livreurID);
            return View(commande);
        }

        // GET: Commande/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var commande = await _context.Commandes.FindAsync(id);
            if (commande == null)
            {
                return NotFound();
            }
            ViewData["clientID"] = new SelectList(_context.Clients, "Id", "Id", commande.ClientId);
            ViewData["livreurID"] = new SelectList(_context.Livreurs, "Id", "Id", commande.livreurID);
            return View(commande);
        }

        // POST: Commande/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,dateTime,montant,clientID,livreurID,CreateAt,UpdateAt")] Commande commande)
        {
            if (id != commande.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(commande);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommandeExists(commande.Id))
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
            ViewData["clientID"] = new SelectList(_context.Clients, "Id", "Id", commande.ClientId);
            ViewData["livreurID"] = new SelectList(_context.Livreurs, "Id", "Id", commande.livreurID);
            return View(commande);
        }

        // GET: Commande/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var commande = await _context.Commandes
                .Include(c => c.Client)
                .Include(c => c.Livreur)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (commande == null)
            {
                return NotFound();
            }

            return View(commande);
        }

        // POST: Commande/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var commande = await _context.Commandes.FindAsync(id);
            if (commande != null)
            {
                _context.Commandes.Remove(commande);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CommandeExists(int id)
        {
            return _context.Commandes.Any(e => e.Id == id);
        }

        public IActionResult CommandeRS()
        {
            // Vérifier si l'utilisateur est authentifié
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Securite"); // Rediriger vers la page de connexion si l'utilisateur n'est pas authentifié
            }

            // Récupérer les commandes avec l'état "En cours"
           var commandesEnCours = _context.Commandes
                                         .Include(c => c.Client)
                                         .Where(c =>c.Etat == Etat.Pret_A_LIVRER)
                                         .ToList();

            // Passer la liste des commandes à la vue
            return View(commandesEnCours);
        }

         public IActionResult CommandeComp()
        {
            // Vérifier si l'utilisateur est authentifié
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Securite"); // Rediriger vers la page de connexion si l'utilisateur n'est pas authentifié
            }

            // Récupérer les commandes avec l'état "En cours"
         var commandesEnCours = _context.Commandes
                                .Include(c => c.Client)
                               .Where(c => c.Etat == Etat.Encours)
                               .ToList();

            // Passer la liste des commandes à la vue
            return View(commandesEnCours);
        }
        public IActionResult Transferer(int id)
{
    if (!User.Identity.IsAuthenticated)
    {
        return RedirectToAction("Login", "Securite"); // Redirection si l'utilisateur n'est pas connecté
    }

    // Rechercher la commande dans la base de données
    var commande = _context.Commandes.FirstOrDefault(c => c.Id == id);
    if (commande == null)
    {
        return NotFound(); // Retourne une erreur 404 si la commande n'existe pas
    }

    // Changer l'état de la commande à "Prêt à être livré"
    commande.Etat = Etat.Pret_A_LIVRER; // Assurez-vous que "Etat.Pret_A_LIVRER" correspond à votre énumération
    _context.SaveChanges(); // Sauvegarder les modifications dans la base de données

    return RedirectToAction("CommandeRS");
}
public IActionResult Livrer(int id)
{
    // Récupération de la commande par son Id
    var commande = _context.Commandes
        .Include(c => c.Client)
        .FirstOrDefault(c => c.Id == id);

    if (commande == null)
    {
        return NotFound();
    }

    // Charger les livreurs disponibles
    var livreursDisponibles = _context.Livreurs
        .Where(l => l.Disponible)
        .Select(l => new SelectListItem
        {
            Value = l.Id.ToString(),
            Text = l.Nom + " " + l.Prenom
        }).ToList();

    ViewBag.Livreurs = livreursDisponibles;

    return View(commande);
}

[HttpPost]
public IActionResult ConfirmerLivraison(int id, DateTime dateTime, string AdresseLivraison, int livreurID)
{
    var commande = _context.Commandes.Find(id);
    if (commande == null)
    {
        return NotFound();
    }

    // Assigner les informations de livraison
    commande.dateTime = dateTime;
    commande.livreurID = livreurID;

    // Changer l'état de la commande
    commande.Etat = Etat.Pret_A_LIVRER; // Assurez-vous que cette valeur existe dans votre enum `Etat`.

    var livreur = _context.Livreurs.Find(livreurID);
    if (livreur != null)
    {
        livreur.Disponible = false; // Le livreur n'est plus disponible
    }

    // Persister les modifications
    _context.SaveChanges();

    return RedirectToAction("Index");
}



    }
}
