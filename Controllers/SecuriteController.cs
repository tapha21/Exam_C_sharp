using ges_commande.Models.Securite;
using GesCommande.core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using ges_commande.Models.Enum;

namespace ges_commande.Controllers
{
    [Authorize]
    public class SecuriteController : Controller
    {
        private readonly Context _context;

        public SecuriteController(Context context)
        {
            _context = context;
        }
        [AllowAnonymous]
    public IActionResult Login()
    {
        return View();
    }

        // Vue pour les utilisateurs ayant le rôle RS
        [Authorize(Roles = "RS")]
        public IActionResult RS()
        {
            return View();
        }

        // Vue pour les clients
        [Authorize(Roles = "Client")]
        public IActionResult Client()
        {
            return View();
        }

        // Vue pour les comptables
        [Authorize(Roles = "Comptable")]
        public IActionResult Comptable()
        {
            return View();
        }

        // Vue de connexion
    [HttpPost]
[AllowAnonymous]
public async Task<IActionResult> Login(LoginModel model)
{
    if (ModelState.IsValid)
    {
        // Rechercher l'utilisateur dans la base de données
        var utilisateur = _context.Users.FirstOrDefault(u => u.login == model.Username && u.password == model.Password);

        if (utilisateur != null)
        {
            // Créer les Claims pour l'utilisateur
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, utilisateur.login),
                new Claim(ClaimTypes.Role, utilisateur.role.ToString()) // Ajouter le rôle
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            // Se connecter avec les cookies d'authentification
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);
            ViewBag.UserRole = utilisateur.role;

            // Redirection en fonction du rôle
            if (utilisateur.role == Role.RS)
                return RedirectToAction("CommandeRS", "Commande");
            if (utilisateur.role == Role.Client)
                return RedirectToAction("Clientviews", "Produit");
            if (utilisateur.role == Role.Comptable)
                return RedirectToAction("CommandeComp", "Commande");
        }
        else
        {
            ModelState.AddModelError("", "Nom d'utilisateur ou mot de passe incorrect.");
        }
    }
    return View(model);
}
[Authorize]
public async Task<IActionResult> Logout()
{
    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    return RedirectToAction("Login");
}
}}