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

        [HttpGet("api/player/{playerId}/games")]
        public async Task<IActionResult> getPlayerGames(int playerId)
        {
            IEnumerable<GameModel> games;
            try
            {
                games = await _playerRepo.getPlayerGames(playerId, null);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
            return Ok(games);
        }

        [HttpGet("api/player/friend/{id}")]
        public async Task<IActionResult> getPlayerFriends(int id)
        {
            IEnumerable<int> players;
            try
            {
                players = await _playerRepo.getFriendsOfPlayer(id);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
            return Ok(players);
        }

        [HttpGet("api/player/id/{id}")]
        public async Task<IActionResult> getPlayerById(int id)
        {
            PlayerModel player;
            try
            {
                player = await _playerRepo.getPlayerById(id);
            }
            catch(Exception)
            {
                return StatusCode(500);
            }
            return Ok(player);
        }

        [HttpGet("api/player/username/{username}")]
        public async Task<IActionResult> getPlayerByUsername(string username)
        {
            PlayerModel player;
            try
            {
                player = await _playerRepo.getPlayerByUsername(username);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
            return Ok(player);
        }


        [HttpGet("api/players")]
        public async Task<IActionResult> getAllPlayers()
        {
            IEnumerable<PlayerModel> sortedPlayers;
            try
            {
                var players = await _playerRepo.getAllPlayers();
                sortedPlayers = players.OrderBy(p => p.Id);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
            return Ok(sortedPlayers);
        }

        [HttpPost("api/player")]
        public async Task<IActionResult> createPlayer(PlayerModel newPlayer)
        {
            int playerId;
            try
            {
                playerId = await _playerRepo.createPlayer(newPlayer);
            }
            catch (InvalidOperationException)
            {
                return StatusCode(400);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
            return Ok(playerId);
        }

        [HttpPost("api/player/{playerId}/friend/{friendId}")]
        public async Task<IActionResult> createFriend(int playerId, int friendId)
        {
            try
            {
                await _playerRepo.createFriend(playerId, friendId);
            }
            catch (InvalidOperationException)
            {
                return StatusCode(400);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
            return Ok();
        }

        [HttpDelete("api/player/{playerId}/friend/{friendId}")]
        public async Task<IActionResult> deleteFriend(int playerId, int friendId)
        {
            try
            {
                await _playerRepo.deleteFriend(playerId, friendId);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
            return Ok();
        }
    }
}
