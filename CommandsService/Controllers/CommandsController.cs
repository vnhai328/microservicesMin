using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using CommandsService.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers
{
    [ApiController]
    [Route("api/c/platforms/{platformId}/[controller]")]
    public class CommandsController : ControllerBase
    {
        private readonly ICommandRepo _commandRepo;
        private readonly IMapper _mapper;

        public CommandsController(ICommandRepo commandRepo, IMapper mapper)
        {
            _commandRepo = commandRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CommandReadDto>> GetCommandsForPlatform(int platformId)
        {
            Console.WriteLine($"---> Hit get commands for platform: {platformId}");

            if(!_commandRepo.PlatFormExits(platformId))
            {
                return NotFound();
            }

            var commands = _commandRepo.GetCommandsForPlatform(platformId);

            return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commands));
        }

        [HttpGet("{commandId}", Name = "GetCommandForPlatform")]
        public ActionResult<CommandReadDto> GetCommandForPlatform(int platformId, int commandId)
        {
            Console.WriteLine($"---> Hit GetCommandForPlatform: {platformId} / {commandId}");

            if(!_commandRepo.PlatFormExits(platformId))
            {
                return NotFound();
            }

            var commands = _commandRepo.GetCommand(platformId, commandId);

            if(commandId == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<CommandReadDto>(commands));
        }

        [HttpPost]
        public ActionResult<CommandReadDto> CreateCommandForPlatform(int platformId, CommandCreateDto commandCreate)
        {
            Console.WriteLine($"---> Hit GetCommandForPlatform: {platformId}");

            if(!_commandRepo.PlatFormExits(platformId))
            {
                return NotFound();
            }

            var command = _mapper.Map<Command>(commandCreate);

            _commandRepo.CreateCommand(platformId, command);
            _commandRepo.SaveChanges();

            var commandReadDto = _mapper.Map<CommandReadDto>(command);

            return CreatedAtRoute(nameof(GetCommandForPlatform), new {platformId = platformId, commandId = commandReadDto.Id}, commandReadDto);
        }
    }
}