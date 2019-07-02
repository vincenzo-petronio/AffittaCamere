using AffittaCamere.RestApiStateless.Models;
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
        private readonly IRoomsService roomsService;

        public RoomsController(IMapper mapper)
        {
            this.mapper = mapper;
            roomsService = ServiceProxy.Create<IRoomsService>(
                new Uri("fabric:/AffittaCamere/RoomsStateful")
                ,new Microsoft.ServiceFabric.Services.Client.ServicePartitionKey(0));
        }

        // GET api/rooms
        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> Get()
        {
            //IRoomsService roomsServiceClient = ServiceProxy.Create<IRoomsService>(
            //    new Uri("fabric:/AffittaCamere/RoomsStateful")
            //    ,new Microsoft.ServiceFabric.Services.Client.ServicePartitionKey(0));
            var result = await roomsService.GetAllRoomsAsync(default(CancellationToken));

            var dtos = result.AsQueryable()
            .ProjectTo<RoomDTO>(mapper.ConfigurationProvider)
            .ToList()
            ;
            var dtosToStrings = dtos.Select(r => r.Name);

            return dtosToStrings.ToArray();
        }

        // GET api/rooms/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/rooms
        [HttpPost]
        public async Task Post([FromBody] RoomRequest roomRequest)
        {
            RoomData room = new RoomData()
            {
                Id = Guid.NewGuid(),
                Name = roomRequest.Name,
                IsAvailable = true,
                Number = roomRequest.Number
            };

            await roomsService.AddOrUpdateRoomAsync(room, default(CancellationToken));
        }

        // PUT api/rooms/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/rooms/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
