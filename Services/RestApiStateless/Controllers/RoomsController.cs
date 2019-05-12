using AffittaCamere.RoomsService.Interfaces;
using AffittaCamere.WebStateless.DTO;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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
        public async Task<ActionResult<IEnumerable<string>>> Get()
        {

            IRoomsService roomsServiceClient = ServiceProxy.Create<IRoomsService>(new Uri("fabric:/AffittaCamere/RoomsService"));
            var result = await roomsServiceClient.GetAllRoomsAsync(default(CancellationToken));

            var dtos = result.AsQueryable()
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
