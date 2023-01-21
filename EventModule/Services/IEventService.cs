using CostEventManegement.EventModule.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostEventManegement.EventModule.Services
{
    public interface IEventService
    {
        public Task<List<CurrencyDTO>> GetCurrencies();
        public Task<int> AddEvent(EventDTO eventModel);
        public Task<EventVM> GetEvent(int id, int userId);
        public Task<List<EventDTO>> GetUserEvents(int userId);
        public Task AddCost(CostDTO costDTO);
        public void SettleUser(SettleUserEvent settleUserModel);
        public Task<int> JoinToEvent(string eventCode, int userId);
        public double GetCurrencyExchange(int currencyId);
        public Task<double> GetCurrentCurrenciesExchange(int fromCurrencyId, int toCurrencyId);
    }
}
