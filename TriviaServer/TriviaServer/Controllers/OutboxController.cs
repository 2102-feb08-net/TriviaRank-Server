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
    [ApiController]
    public class OutboxController : ControllerBase
    {
        private readonly IOutboxRepository _outboxRepo;
        public OutboxController(IOutboxRepository outboxRepo)
        {
            _outboxRepo = outboxRepo;
        }

        [HttpGet("api/outbox/friend/{playerId}")]
        public async Task<IActionResult> getFriendInvites(int playerId)
        {
            List<PlayerModel> players;
            try
            {
                players = await _outboxRepo.getFriendInvites(playerId);
            }
            catch (Exception e)
            {
                return StatusCode(500); 
            }
            return Ok(players);
        }

        [HttpGet("api/outbox/game/{playerId}")]
        public async Task<IActionResult> getGameInvites(int playerId)
        {
            List<GameModel> games;
            try
            {
                games = await _outboxRepo.getGameInvites(playerId);
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }
            return Ok(games);
        }

        [HttpPost("api/outbox/friend/{playerId}/{friendId}")]
        public async Task<IActionResult> createFriendInvite(int playerId, int friendId)
        {
            int obxId;
            try
            {
                obxId = await _outboxRepo.createFriendInvite(playerId, friendId);
            }
            catch (InvalidOperationException ioe)
            {
                return StatusCode(400);
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }
            return Ok(obxId);
        }

        [HttpPost("api/outbox/game/{gameId}/{playerId}")]
        public async Task<IActionResult> createGameInvite(int gameId, int playerId)
        {
            int obxId;
            try
            {
                obxId = await _outboxRepo.createGameInvite(gameId, playerId);
            }
            catch (InvalidOperationException ioe)
            {
                return StatusCode(400);
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }
            return Ok(obxId);
        }
    }
}
