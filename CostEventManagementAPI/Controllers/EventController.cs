using CostEventManegement.AuthModule.Models;
using CostEventManegement.EventModule.Models;
using CostEventManegement.EventModule.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CostEventManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class EventController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly IEventService _eventSerice;
        public EventController(IEventService eventSerice, IHttpContextAccessor httpContextAccessor)
        {
            _eventSerice = eventSerice;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost("AddEvent")]
        public async Task<int> AddEvent(EventDTO eventModel)
        {
            return await _eventSerice.AddEvent(eventModel);
        }

        [HttpPost("AddCost")]
        public async Task AddCost(CostDTO costModel)
        {
            costModel.PayerId = GetUserId();
            await _eventSerice.AddCost(costModel);
        }

        [HttpPost("EditEvent")]
        public async Task<int> EditEvent(EventDTO eventModel)
        {
            return 1; 
        }

        [HttpGet("JoinToEvent")]
        public async Task<int> JoinToEvent(string eventCode)
        {
            return await _eventSerice.JoinToEvent(eventCode, GetUserId());
        }

        [HttpGet("GetCurrencies")]
        public async Task<List<CurrencyDTO>> GetCurrencies()
        {
            var username = GetUserId();
            return await _eventSerice.GetCurrencies();
        }

        [HttpGet("GetEvent")]
        public async Task<EventVM> GetEvent(int eventId)
        {
            return await _eventSerice.GetEvent(eventId, GetUserId());
        }

        [HttpGet("GetEvents")]
        public async Task<List<EventDTO>> GetEvents()
        {
            return await _eventSerice.GetUserEvents(GetUserId());
        }

        [HttpPost("SettleUser")]
        public async Task<EventDTO> SettleUser(int userId)
        {
            return null;
        }

        [HttpGet("GetCurrenciesExchange")]
        public async Task<double> GetCurrenciesExchange(int from, int to)
        {
            return await _eventSerice.GetCurrentCurrenciesExchange(from, to);
        }

        [HttpPost("AddReceipt")]
        public async Task<EventDTO> AddReceipt(int userId)
        {
            return null;
        }

        private int GetUserId() 
        {
            var identity = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;

            if (identity is not null)
            {
                return Convert.ToInt32(identity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);
            }

            return 0;
        }


    }
}
