using API.Controllers;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API
{
    
    public class VIPController : BaseApiController
    {
        private readonly IUnitOfWork _uow;

        public VIPController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpPost("add-visit/{username}")]
        public async Task<ActionResult> AddVisit(string username)
        {
            var sourceUserId = User.GetUserId();
            var visitedUser = await _uow.UserRepository.GetUserByUsernameAsync(username);
            var sourceUser = await _uow.VisitsRepository.GetUserWithVisits(sourceUserId);

            if (visitedUser == null) return NotFound();

            if (sourceUser.UserName == username) return BadRequest("You cannot visit yourself");

            var userVisit = await _uow.VisitsRepository.GetUserVisit(sourceUserId, visitedUser.Id);

            if (userVisit != null) return BadRequest("Visit already tracked");

            userVisit = new UserVisit
            {
                SourceUserId = sourceUserId,
                TargetUserId = visitedUser.Id
            };

            sourceUser.VisitedUsers.Add(userVisit);

            if (await _uow.Complete()) return Ok();

            return BadRequest("Failed to track visit");
        }

        [Authorize(Policy = "RequireVIPRole")]
        [HttpGet]
        public async Task<ActionResult<PagedList<VisitDto>>> GetUserVisits([FromQuery] VisitsParams visitsParams)
        {
            visitsParams.UserId = User.GetUserId();

            var users = await _uow.VisitsRepository.GetUserVisits(visitsParams);

            Response.AddPaginationHeader(new PaginationHeader(users.CurrentPage, users.PageSize, 
                users.TotalCount, users.TotalPages));

            return Ok(users);
        }
    }
}
