using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Movies.Data;
using Movies.Models;

namespace Movies.Controllers
{
    public class MovieClassesController : Controller
    {
        private readonly MoviesContext _context;

        public MovieClassesController(MoviesContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Ratings()
        {
          
            var ratings = from m in _context.MovieClass select m;
            var ratings1 = ratings.Where(z => z.Rating.Contains("1"));
            int ratings1Int = ratings1.Count();
            var ratings2 = ratings.Where(e => e.Rating.Contains("2"));
            int ratings2Int = ratings2.Count();
            var ratings3 = ratings.Where(c => c.Rating.Contains("3"));
            int ratings3Int = ratings3.Count();
            var ratings4 = ratings.Where(a => a.Rating.Contains("4"));
            int ratings4Int = ratings4.Count();
            var ratings5 = ratings.Where(r => r.Rating.Contains("5"));
            int ratings5Int = ratings5.Count();
            ViewBag.Rating1 = ratings1Int;
            ViewBag.Rating2 = ratings2Int;
            ViewBag.Rating3 = ratings3Int;
            ViewBag.Rating5 = ratings5Int;
            ViewBag.Rating4 = ratings4Int;
            
            return View();
        }

        // GET: MovieClasses
        public async Task<IActionResult> Index(string movieRating ,string movieGenre ,string searchString)
        {
            //string searchString = id;
            var genreList = new List<String>();
            var ratingList = new List<String>();
            var ratingqry = from z in _context.MovieClass orderby z.Rating select z.Rating;
            var genreQry = from g in _context.MovieClass orderby g.Category select g.Category;

            genreList.AddRange(genreQry.Distinct());
            ViewBag.movieGenre = new SelectList(genreList);

            ratingList.AddRange(ratingqry.Distinct());
            ViewBag.ratings = new SelectList(ratingList);

            var movies = from m in _context.MovieClass select m;
            //  return View(await _context.MovieClass.ToListAsync());
            if(!String.IsNullOrEmpty(searchString))
            {
                movies = movies.Where(s => s.Name.Contains(searchString));
            }
            if (!String.IsNullOrEmpty(movieGenre))
            {
                movies = movies.Where(x => x.Category == movieGenre);
            }
            if (!String.IsNullOrEmpty(movieRating))
            {
                movies = movies.Where(x => x.Rating == movieRating);
            }

            return View(movies);
        }

        // GET: MovieClasses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movieClass = await _context.MovieClass
                .FirstOrDefaultAsync(m => m.ID == id);
            if (movieClass == null)
            {
                return NotFound();
            }
            ViewBag.DetailName = movieClass.Name;
            return View(movieClass);
        }

        public IActionResult MovieExists()
        {
            return View();
        }

        // GET: MovieClasses/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MovieClasses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Category,Rating")] MovieClass movieClass)
        {

           
              if (ModelState.IsValid)
                  {
                var movies = await _context.MovieClass.FirstOrDefaultAsync(m => m.Name == movieClass.Name.ToString());
                if (movies == null)
                {
                    _context.Add(movieClass);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    //already exists
                    ModelState.AddModelError("Name", "Movie already exists");
                    return RedirectToAction(nameof(MovieExists));
                }
                 }
            //   if (ModelState.IsValid)
            //      {
            //           _context.Add(movieClass);
            //           await _context.SaveChangesAsync();
            //          return RedirectToAction(nameof(Index));
            //       }
            return View(movieClass);
        }

        // GET: MovieClasses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
          
            var movieClass = await _context.MovieClass.FindAsync(id);
            if (movieClass == null)
            {
                return NotFound();
            }
            ViewBag.EditName = movieClass.Name;
            return View(movieClass);
        }

        // POST: MovieClasses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Category,Rating")] MovieClass movieClass)
        {
            
            if (id != movieClass.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movieClass);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieClassExists(movieClass.ID))
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
            return View(movieClass);
        }

        // GET: MovieClasses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movieClass = await _context.MovieClass
                .FirstOrDefaultAsync(m => m.ID == id);
            if (movieClass == null)
            {
                return NotFound();
            }
            ViewBag.DeleteName = movieClass.Name;
            return View(movieClass);
        }

        // POST: MovieClasses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movieClass = await _context.MovieClass.FindAsync(id);
            _context.MovieClass.Remove(movieClass);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieClassExists(int id)
        {
            return _context.MovieClass.Any(e => e.ID == id);
        }
    }
}
