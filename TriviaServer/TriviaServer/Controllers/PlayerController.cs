﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TriviaServer.Models;
using TriviaServer.Models.Repositories;

namespace TriviaServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        private readonly IPlayerRepository _playerRepo;
        public PlayerController(IPlayerRepository playerRepo)
        {
            _playerRepo = playerRepo;
        }

        [HttpGet("api/customers")]
        public async Task<IActionResult> getAllCustomers()
        {
            var players = await _playerRepo.getAllPlayers();
            var sortedPlayers = players.OrderBy(p => p.Id);
            return Ok(sortedPlayers);
        }
    }
}
