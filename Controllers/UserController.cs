using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MyMvcApp.Models;

namespace MyMvcApp.Controllers;

/// <summary>
/// Controlador para gestionar las operaciones relacionadas con los usuarios.
/// </summary>
public class UserController : Controller
{
    /// <summary>
    /// Lista estática que almacena los usuarios en memoria.
    /// </summary>
    public static System.Collections.Generic.List<User> userlist = new System.Collections.Generic.List<User>();

    /// <summary>
    /// Muestra la lista de todos los usuarios.
    /// </summary>
    /// <returns>Vista con la lista de usuarios.</returns>
    public ActionResult Index()
    {
        return View(userlist);
    }

    /// <summary>
    /// Muestra los detalles de un usuario específico.
    /// </summary>
    /// <param name="id">ID del usuario.</param>
    /// <returns>Vista con los detalles del usuario o un resultado NotFound si no se encuentra.</returns>
    public ActionResult Details(int id)
    {
        var user = userlist.FirstOrDefault(u => u.Id == id);
        if (user == null)
        {
            return NotFound();
        }
        return View(user);
    }

    /// <summary>
    /// Muestra el formulario para crear un nuevo usuario.
    /// </summary>
    /// <returns>Vista del formulario de creación.</returns>
    public ActionResult Create()
    {
        return View();
    }

    /// <summary>
    /// Procesa la creación de un nuevo usuario.
    /// </summary>
    /// <param name="user">Objeto usuario con los datos ingresados.</param>
    /// <returns>Redirige a la lista de usuarios si es válido, de lo contrario, muestra el formulario con errores.</returns>
    [HttpPost]
    public ActionResult Create(User user)
    {
        if (ModelState.IsValid)
        {
            user.Id = userlist.Count > 0 ? userlist.Max(u => u.Id) + 1 : 1; // Asigna un nuevo ID
            userlist.Add(user);
            return RedirectToAction(nameof(Index));
        }
        return View(user);
    }

    /// <summary>
    /// Muestra el formulario para editar un usuario existente.
    /// </summary>
    /// <param name="id">ID del usuario a editar.</param>
    /// <returns>Vista del formulario de edición o un resultado NotFound si no se encuentra.</returns>
    public ActionResult Edit(int id)
    {
        var user = userlist.FirstOrDefault(u => u.Id == id);
        if (user == null)
        {
            return NotFound();
        }
        return View(user);
    }

    /// <summary>
    /// Procesa la edición de un usuario existente.
    /// </summary>
    /// <param name="id">ID del usuario a editar.</param>
    /// <param name="user">Objeto usuario con los datos actualizados.</param>
    /// <returns>Redirige a la lista de usuarios si es válido, de lo contrario, muestra el formulario con errores.</returns>
    [HttpPost]
    public ActionResult Edit(int id, User user)
    {
        var existingUser = userlist.FirstOrDefault(u => u.Id == id);
        if (existingUser == null)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            existingUser.Name = user.Name;
            existingUser.Email = user.Email;
            return RedirectToAction(nameof(Index));
        }
        return View(user);
    }

    /// <summary>
    /// Muestra el formulario para eliminar un usuario.
    /// </summary>
    /// <param name="id">ID del usuario a eliminar.</param>
    /// <returns>Vista del formulario de eliminación o un resultado NotFound si no se encuentra.</returns>
    public ActionResult Delete(int id)
    {
        var user = userlist.FirstOrDefault(u => u.Id == id);
        if (user == null)
        {
            return NotFound();
        }
        return View(user);
    }

    /// <summary>
    /// Procesa la eliminación de un usuario.
    /// </summary>
    /// <param name="id">ID del usuario a eliminar.</param>
    /// <param name="collection">Colección de datos del formulario (no utilizado).</param>
    /// <returns>Redirige a la lista de usuarios.</returns>
    [HttpPost]
    public ActionResult Delete(int id, IFormCollection collection)
    {
        var user = userlist.FirstOrDefault(u => u.Id == id);
        if (user != null)
        {
            userlist.Remove(user);
        }
        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// Busca usuarios por nombre.
    /// </summary>
    /// <param name="query">Término de búsqueda.</param>
    /// <returns>Vista con los resultados de la búsqueda.</returns>
    public ActionResult Search(string query)
    {
        var results = string.IsNullOrEmpty(query)
            ? userlist
            : userlist.Where(u => u.Name.Contains(query, StringComparison.OrdinalIgnoreCase)).ToList();
        ViewBag.Query = query;
        return View(results);
    }
}
