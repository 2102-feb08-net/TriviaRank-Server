using Microsoft.AspNetCore.Http;
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
    public class PlayerController : ControllerBase
    {
        private readonly IPlayerRepository _playerRepo;
        public PlayerController(IPlayerRepository playerRepo)
        {
            _playerRepo = playerRepo;
        }

        [HttpGet("api/player/friend/{id}")]
        public async Task<IActionResult> getPlayerFriends(int id)
        {
            IEnumerable<int> players;
            try
            {
                players = await _playerRepo.getFriendsOfPlayer(id);
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }
            return Ok(players);
        }

        [HttpGet("api/player/{id}")]
        public async Task<IActionResult> getPlayerById(int id)
        {
            TriviaPlayer player;
            try
            {
                player = await _playerRepo.getPlayerById(id);
            }
            catch(Exception e)
            {
                return StatusCode(500);
            }
            return Ok(player);
        }


        [HttpGet("api/players")]
        public async Task<IActionResult> getAllPlayers()
        {
            IEnumerable<TriviaPlayer> sortedPlayers;
            try
            {
                var players = await _playerRepo.getAllPlayers();
                sortedPlayers = players.OrderBy(p => p.Id);
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }
            return Ok(sortedPlayers);
        }

        [HttpPost("api/player")]
        public async Task<IActionResult> createPlayer(TriviaPlayer newPlayer)
        {
            int playerId;
            try
            {
                playerId = await _playerRepo.createPlayer(newPlayer);
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }
            return Ok(playerId);
        }

        //[HttpPost("api/test/friend/{playerId}/{friendId}")]
        //public async Task<IActionResult> createFriend(int playerId, int friendId)
        //{
        //    try
        //    {
        //        await _playerRepo.createFriend(playerId, friendId);
        //    }
        //    catch (Exception e)
        //    {
        //        return StatusCode(500);
        //    }
        //    return Ok();
        //}
    }
}
