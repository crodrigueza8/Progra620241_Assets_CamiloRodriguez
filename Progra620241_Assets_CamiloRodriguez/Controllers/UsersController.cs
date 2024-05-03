using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Progra620241_Assets_CamiloRodriguez.Models;
using Progra620241_Assets_CamiloRodriguez.ModelsDTOs;

namespace Progra620241_Assets_CamiloRodriguez.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly Progra620241Context _context;

        public UsersController(Progra620241Context context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        //GET: api/Users/GetUserData?pUserName=algo
        //Este get me permite obetener los datos puntuales de un usuario 
        //usando el correo como parametro .
        [HttpGet("GetUserData")]
        public ActionResult<IEnumerable<UserDTO>> GetUserData(string pUserName)
        {
            //El proposito de usar el DTO aca es combinar los datos de latabla user y userRole 
            //Y devolver un solo objeto Json con dicha informacion, ademas no se sabra como se llaman
            //los atributos originales.

            //Para hacer esta consulta no usaremos procesos alamacenados como en progra 5
            //En cambio usaremos LINQ, que permita hecer consultas sobre conexcionces
            //Directamente en la progra.

            var query = (from us in _context.Users
                         join ur in _context.UserRoles on us.UserRoleId equals ur.UserRoleId
                         where us.UserName == pUserName && us.Active == true
                         select new
                         {
                             idUsuario = us.UserId,
                             cedula = us.CardId,
                             nombre = us.FirstName,
                             apellidos = us.LastName,
                             telefono = us.PhoneNumber,
                             direccion = us.Address,
                             correo = us.UserName,
                             activo = us.Active,
                             idRol = ur.UserRoleId,
                             rol = ur.UserRoleDescription
                         }
                        ).ToList();
            //Ahora que tenemos el resultado de la consulta en la variable 
            //query, procedemos a crear el resultado de la funcion.

            //Crear el objeto de respuesta 
            List<UserDTO> ListaUsuarios = new List<UserDTO>();

            //Ahora hacemso un recorrido de la sposibles iteraciones de la variable query 
            //y rellenamos en cada una de ellas un nuevo objeto DTO.
            
            foreach (var item in query) 
            {
                UserDTO newUser = new UserDTO()
                {
                    CodigoUsuario = item.idUsuario,
                    Cedula = item.cedula,
                    Nombre = item.nombre,
                    Apellido = item.apellidos,
                    Telefono = item.telefono,
                    Direccion = item.direccion,
                    Correo = item.correo,
                    CodigoDeRol = item.idRol,
                    RolDeUsuario = item.rol,
                    NotasDelUsuario = "No hay comentarios"
                };
                ListaUsuarios.Add(newUser);
            }
            if(ListaUsuarios == null || ListaUsuarios.Count() ==0)
            {
                return NotFound();
            }

            return ListaUsuarios;

        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.UserId)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.UserId }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }
    }
}
