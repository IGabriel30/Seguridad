using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Seguridad.Models;

namespace Seguridad.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UsuariosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Usuarios
        public async Task<IActionResult> Index(Usuario usuario)
        {
            var query = _context.Usuarios.AsQueryable();
            if(string.IsNullOrWhiteSpace(usuario.Nombre) == false)
            {
                query = query.Where(s => s.Nombre.Contains(usuario.Nombre));
            }
            if (string.IsNullOrWhiteSpace(usuario.Apellido) == false)
            {
                query = query.Where(s => s.Apellido.Contains(usuario.Apellido));
            }
            if (usuario.Status ==1 || usuario.Status ==2)
            {
                query = query.Where(s => s.Status == usuario.Status);
            }

            //cantidad de registros.
            if (usuario.Take == 0)
                usuario.Take = 10;
            query = query.Take(usuario.Take);

            return query != null ? View(await query.ToListAsync()):
                Problem("Entity set 'ApplicationDbContext.Usuarios'  is null.");
        }

        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Usuarios == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.IdUsuario == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }


        // GET: Usuarios/Login
        //Carga la vista
        public IActionResult Login(string ReturnUrl)
        {
            ViewBag.pReturnUrl = ReturnUrl;
            return View();
        }

        //se recibe, se comparann datos
        [HttpPost]
        public async Task<IActionResult> Login([Bind("Email,Password")] Usuario usuario, string ReturnUrl)
        {
            usuario.Password = CalcularHashMD5(usuario.Password);
            var usuarioAut = await _context.Usuarios.FirstOrDefaultAsync(s => s.Email == usuario.Email && s.Password == usuario.Password && s.Status == 1);
            if (usuarioAut?.IdUsuario > 0 && usuarioAut.Email == usuario.Email)
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, usuario.Email),
                    new Claim("IdUsario", usuarioAut.IdUsuario.ToString())
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), new AuthenticationProperties { IsPersistent = true }); ;
                var result = User.Identity.IsAuthenticated;
                if (!string.IsNullOrWhiteSpace(ReturnUrl))
                    return Redirect(ReturnUrl);
                else
                    return RedirectToAction("Index", "Home");
            }
            else ViewBag.Error = "Credenciales Incorrectas";
            ViewBag.ReturnUrl = ReturnUrl;
            return View(usuario);
        }


        // GET: Usuarios/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Usuarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdUsuario,Nombre,Apellido,Email,Password,Status")] Usuario usuario)
        {

            usuario.Password = CalcularHashMD5(usuario.Password);
                _context.Add(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
           
            //return View(usuario);
        }

        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Usuarios == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdUsuario,Nombre,Apellido,Email,Status")] Usuario usuario)
        {
            if (id != usuario.IdUsuario)
            {
                return NotFound();
            }

         
                try
                {
                var usuarioUpdate = await _context.Usuarios.FirstOrDefaultAsync(s => s.IdUsuario == id);
                usuarioUpdate.Nombre = usuario.Nombre;
                usuarioUpdate.Apellido = usuario.Apellido;
                usuarioUpdate.Email = usuario.Email;
                usuario.Status = usuario.Status;
                    _context.Update(usuarioUpdate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.IdUsuario))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            
            //return View(usuario);
        }

        // GET: Usuarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Usuarios == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.IdUsuario == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Usuarios == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Usuarios'  is null.");
            }
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioExists(int id)
        {
          return _context.Usuarios.Any(e => e.IdUsuario == id);
        }

        private string CalcularHashMD5(string texto)
        {
            using (MD5 md5 = MD5.Create()){
                //Convierte la cadena de texto a bytes
                byte[] inputbytes= Encoding.UTF8.GetBytes(texto);

                //Calcula el hash MD5 de los bytes
                byte[] HashBytes = md5.ComputeHash(inputbytes);

                //convierte el hash a una cadena hexadecimal
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < HashBytes.Length; i++)
                {
                    sb.Append(HashBytes[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }
    }
}
