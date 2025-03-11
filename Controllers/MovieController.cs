using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesAPI.Data;
using MoviesAPI.Models;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;

namespace MoviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MovieController : ControllerBase
    {
        private readonly AppDbContext dbContext;
        public MovieController(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        [Route("GetAllMovies")]
        public IActionResult GetAllMovies()
        {
            var allMovies = dbContext.Movies.ToList();
            return Ok(allMovies);
        }

        [HttpGet]
        [Route("GetAllRentedMovies")]
        public IActionResult GetAllRentedMovies()
        {
            var allRentedMovies = dbContext.Movies.Where(m => m.isRented == true).ToList();
            return Ok(allRentedMovies);
        }


        [HttpGet]
        [Route("GetAllNotRentedMovies")]
        public IActionResult GetAllNotRentedMovies()
        {
            var allNotRentedMovies = dbContext.Movies.Where(m => m.isRented == false).ToList();
            return Ok(allNotRentedMovies);
        }
        
        [HttpGet]
        [Route("GetAllNotDeletedMovies")]
        public IActionResult GetAllNotDeletedMovies()
        {
            var allNotDeletedMovies = dbContext.Movies.Where(m => m.isDeleted == false).ToList();
            return Ok(allNotDeletedMovies);
        }

        [HttpGet]
        [Route("GetMovieById/{id}")]
        public IActionResult GetMovieById(int id)
        {
            var movie = dbContext.Movies.Find(id);
            return Ok(movie);
        }

        [HttpPost]
        [Route("AddMovie")]
        public IActionResult AddMovie([FromBody] Movie movie)
        {
            if (movie == null)
            {
                return BadRequest("Invalid movie data.");
            }
            if (movie.isRented)
            {
                movie.rentalDate = DateTime.Now;
            }
            else
            {
                movie.rentalDate = null;
            }
            dbContext.Movies.Add(movie);
            dbContext.SaveChanges();

            return Ok(new { message = "Movie added successfully!", movie });
        }

        [HttpPut]
        [Route("UpdateMovie/{id}")]
        public IActionResult UpdateMovie(int id, [FromBody] Movie updatedMovie)
        {
            if (updatedMovie == null)
            {
                return BadRequest("Invalid movie data.");
            }

            var existingMovie = dbContext.Movies.Find(id); 
            if (existingMovie == null)
            {
                return NotFound($"Movie with ID {id} not found.");
            }
            existingMovie.title = updatedMovie.title;
            existingMovie.description = updatedMovie.description;
            existingMovie.isRented = updatedMovie.isRented;
            if (updatedMovie.isRented)
            {
                existingMovie.rentalDate = DateTime.Now;
            }
            else
            {
                existingMovie.rentalDate = null;
            }
            
            existingMovie.isDeleted = updatedMovie.isDeleted;
            dbContext.Movies.Update(existingMovie);
            dbContext.SaveChanges();
            return Ok(new { message = "Movie updated successfully!", updatedMovie });
        }

        [HttpPut]
        [Route("DeleteMovieLabel/{id}")]
        public IActionResult DeleteMovieLabel(int id, [FromBody] Movie updatedMovie)
        {
            if (updatedMovie == null)
            {
                return BadRequest("Invalid movie data.");
            }

            var existingMovie = dbContext.Movies.Find(id);
            if (existingMovie == null)
            {
                return NotFound($"Movie with ID {id} not found.");
            }
            else { 
            }
          
            existingMovie.isDeleted = updatedMovie.isDeleted;
            dbContext.Movies.Update(existingMovie);
            dbContext.SaveChanges();
            return Ok(new { message = "Movie deleted successfully!", updatedMovie });
        }

        [HttpDelete]
        [Route("DeleteMovie/{id}")]
        public IActionResult DeleteMovie(int id)
        {
            var movie = dbContext.Movies.Find(id);

            if (movie == null)
            {
                return NotFound(new { message = "Movie not found!" });
            }

            dbContext.Movies.Remove(movie);
            dbContext.SaveChanges();

            return Ok(new { message = "Movie deleted successfully!" });
        }
    }
}
