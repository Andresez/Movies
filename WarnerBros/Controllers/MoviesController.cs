using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WarnerBros.Areas.Identity.Data;
using WarnerBros.Models;

namespace WarnerBros.Controllers
{
    public class MoviesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MoviesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Movies
        public async Task<IActionResult> Index()
        {
            //Creamos lista
            IEnumerable<Movies> ListMovies = _context.movies;


            return View(ListMovies);//Retorna la lista
        }

        // GET: Movies/Create
        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Graficas()
        {
            List<Movies> listMovies = 
                (from movies in _context.movies
                group movies by movies.nombre into grupal
                orderby grupal.Max(Movies => Movies.puntuacion) descending
                select new Movies
                                {
                                   nombre = grupal.Key,
                                   puntuacion = grupal.Max(p => p.puntuacion),
                                }
                                   ).Take(100).ToList();


                return StatusCode(StatusCodes.Status200OK, listMovies);

            //return View();
        }

        /// POST Create
        [HttpPost]
        // para que no cargen la bd con basura - protección del formulario
        [ValidateAntiForgeryToken]
        public IActionResult Create(Movies movies)
        {
            if (ModelState.IsValid)
            {
                _context.movies.Add(movies);
                _context.SaveChanges();
                return RedirectToAction("Index"); // Redirige a la acción "Index"
            }
            return View();
        }
        private IActionResult RedirecToAction(string v)
        {
            throw new NotImplementedException();
        }

        // GET: Movies/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var cinta = _context.movies.Find(id);
            if (cinta == null)
            {
                return NotFound("Movie Not Found");
            }
            return View(cinta);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        // para que no cargen la bd con basura - protección del formulario
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Movies movie)
        {
            if (ModelState.IsValid)
            {
                // Obtén la película existente desde la base de datos, por ejemplo, usando su ID.
                var existingMovie = _context.movies.Find(movie.id);

                if (existingMovie != null)
                {
                    // Actualiza solo la propiedad de puntuación.
                    existingMovie.puntuacion = movie.puntuacion;

                    // Guarda los cambios en la base de datos.
                    _context.SaveChanges();
                }
                return RedirectToAction("Index");
            }

            return View(movie);
        }

        // GET: Movies/Delete/5

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var cinta = _context.movies.Find(id);
            if (cinta == null)
            {
                return NotFound();
            }

            return View(cinta); // Mostrar la vista de eliminación con los detalles de la película
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var cinta = _context.movies.Find(id);
            if (cinta == null)
            {
                return NotFound();
            }

            _context.movies.Remove(cinta); // Eliminar película de la base de datos
            _context.SaveChanges(); // Guardar eliminación

            return RedirectToAction("Index"); // Redireccionar al Index
        }

        //public IActionResult Delete(int? id)
        //{
        //    if (id == null || id == 0)
        //    {
        //        return NotFound();
        //    }

        //    var cinta = _context.movies.Find(id);
        //    if (cinta == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.movies.Remove(cinta); //Eliminar película de la base de datos
        //    _context.SaveChanges(); //Guardar eliminación

        //    return RedirectToAction("Index"); //Redireccionar al Index
        //}
    }
}