using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AffittaCamere.DataAccess.Entities;
using AffittaCamere.RestApiStateless.Helpers;
using AffittaCamere.WebStateless.DTO;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace AffittaCamere.RestApiStateless.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly IMapper mapper;

        public RoomsController(IMapper mapper)
        {
            this.mapper = mapper;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            var rooms = new List<Room> ()
            {
                new Room()
                {
                    Name = "Room A"
                },
                new Room()
                {
                    Name = "Room B"
                }
            };

            var queryRooms = rooms.AsQueryable();

            var dtos = queryRooms
                .ProjectTo<RoomDTO>(mapper.ConfigurationProvider)
                .ToList()
                ;

            var dtosToStrings = dtos.Select(r => r.Name);

            return dtosToStrings.ToArray();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
