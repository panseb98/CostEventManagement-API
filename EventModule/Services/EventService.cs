using CostEventManegement.DatabaseModule;
using CostEventManegement.DatabaseModule.Models;
using CostEventManegement.EventModule.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CostEventManegement.EventModule.Services
{
    public class EventService : IEventService
    {
        private readonly ApiDbContext _context;

        public EventService(ApiDbContext context)
        {
            _context = context;
        }

        public async Task AddCost(CostDTO costDTO)
        {
            var eventModel = await _context.Events.FirstAsync(x => x.Id == costDTO.EventId);

            if (eventModel.DefaultCurrencyId != costDTO.CurrencyId)
            {
                var exchange = await GetCurrentCurrenciesExchange(costDTO.CurrencyId, eventModel.DefaultCurrencyId);
                costDTO.Value = Math.Round(costDTO.Value * exchange, 2);
                costDTO.CurrencyId = eventModel.DefaultCurrencyId;
            }

            var individualCost = costDTO.Value / costDTO.Users.Count + 1;
            var dbCost = new Cost()
            {
                EventId = costDTO.EventId,
                CurrencyId = costDTO.CurrencyId,
                Name = costDTO.Name,
                PayerId = costDTO.PayerId,
                Value = costDTO.Value,
                UserCosts = costDTO.Users.Select(x => new UserCost() { DebtorId = x, Value = individualCost, PayerId = costDTO.PayerId }).ToList()
            };

            await _context.AddAsync(dbCost);

            await _context.SaveChangesAsync();
        }

        public async Task<int> AddEvent(EventDTO eventModel)
        {
            var newEvent = new Event()
            {
                Name = eventModel.Name,
                DefaultCurrencyId = eventModel.DefaultCurrencyId,
                Code = Guid.NewGuid()
                    .ToString()
                    .Substring(0, 7)
                    .Replace("-", "")
                    .ToUpper()
            };

            await _context.Events.AddAsync(newEvent);
            await _context.SaveChangesAsync();
            return newEvent.Id;
        }

        public async Task<int> JoinToEvent(string eventCode, int userId)
        {
            var eventModel = await _context.Events.FirstOrDefaultAsync(x => x.Code == eventCode);


            if (eventModel == null)
            {
                throw new Exception("Nie istnieje takie wydarzenie");
            }
            else
            {
                var isUserExists = _context.EventUsers.Any(x => x.UserId == userId && x.EventId == eventModel.Id);

                if (isUserExists)
                {
                    throw new Exception("Jesteś już przypisany do tego wydarzenia");
                }

                await _context.EventUsers.AddAsync(new EventUser() { UserId = userId, EventId = eventModel.Id });
                await _context.SaveChangesAsync();

                return eventModel.Id;
            }
        }

        public async Task<List<CurrencyDTO>> GetCurrencies()
        {
            return await _context.Currencies.Select(x => new CurrencyDTO() { Code = x.Code, Id = x.Id, ImageUrl = x.ImageUrl, Name = x.Name }).ToListAsync();
        }

        public double GetCurrencyExchange(int currencyId)
        {
            throw new NotImplementedException();
        }

        public async Task<EventVM> GetEvent(int id, int userId)
        {
            var userEvent = await _context.Events.Include(x => x.EventUsers).ThenInclude(x => x.User).FirstOrDefaultAsync(x => x.Id == id);

            var userCosts = await _context.Costs.Include(x => x.UserCosts).Include(x => x.Currency).Where(x => x.EventId == id).ToListAsync();

            var costsOnPlus = userCosts.SelectMany(x => x.UserCosts).Where(x => x.PayerId == id);
            var costsOnMinus = userCosts.SelectMany(x => x.UserCosts).Where(x => x.DebtorId == id);

            var dictionaryCost = new Dictionary<int, double>();

            var eventVM = new EventVM()
            {
                Name = userEvent.Name,
                Code = userEvent.Code,
                DefaultCurrencyId = userEvent.DefaultCurrencyId,
                Users = userEvent.EventUsers.Select(x => new UserBalanceVM()
                { Id = x.UserId,
                    Name = x.User.Name + " " + x.User.Surname
                }).ToList(),
                Costs = userCosts.Where(x => x.PayerId == id).Select(x => new CostDTO() { Name = x.Name, Value = x.Value }).ToList()
            };

            foreach (var cost in costsOnPlus)
            {
                if (dictionaryCost.ContainsKey(cost.DebtorId))
                {
                    var valueToPay = dictionaryCost[cost.DebtorId];
                    valueToPay =+ cost.Value;
                    dictionaryCost[cost.DebtorId] = valueToPay;
                }
                else
                {
                    dictionaryCost.Add(cost.DebtorId, cost.Value);
                }
            }

            foreach (var cost in costsOnMinus)
            {
                if (dictionaryCost.ContainsKey(cost.PayerId))
                {
                    var valueToPay = dictionaryCost[cost.PayerId];
                    valueToPay = +cost.Value;
                    dictionaryCost[cost.PayerId] = valueToPay;
                }
                else
                {
                    dictionaryCost.Add(cost.PayerId, cost.Value);
                }
            }

            eventVM.Users.ForEach(x => { x.Costs = dictionaryCost.ContainsKey(x.Id) ? dictionaryCost[x.Id] : 0; });

            return eventVM;
        }

        public async Task<List<EventDTO>> GetUserEvents(int userId)
        {
            return await _context.Events.Include(x => x.DefaultCurrency).Include(x => x.EventUsers).ThenInclude(x => x.User).Where(x => x.EventUsers.Any(x => x.UserId == userId)).Select(x => new EventDTO() { Name = x.Name, Code = x.Code, Id = x.Id, DefaultCurrencyId = x.DefaultCurrencyId, DefaultCurrencyCode = x.DefaultCurrency.Name, Users = x.EventUsers.Where(x => x.UserId != userId).Select(x => new SimpleUserVM { Name = x.User.Name + " " + x.User.Surname, Id = x.User.Id }).ToList() }).ToListAsync();
        }

        public void SettleUser(SettleUserEvent settleUserModel)
        {
            throw new NotImplementedException();
        }

        public async Task<double> GetCurrentCurrenciesExchange(int fromCurrencyId, int toCurrencyId)
        {
            using HttpClient client = new();
            var fromCurrency = await _context.Currencies.FirstOrDefaultAsync(x => x.Id == fromCurrencyId);
            var toCurrency = await _context.Currencies.FirstOrDefaultAsync(x => x.Id == toCurrencyId);

            client.DefaultRequestHeaders.Add("Cookie", "privacy=1671485285");
            var currenciesCode = $"{fromCurrency.Code.ToLower()}{toCurrency.Code.ToLower()}";

            var json = await client.GetStringAsync(
                $"https://stooq.pl/q/?s={currenciesCode}"
                );

            var encoded = HttpUtility.HtmlEncode(json);

            HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();

            htmlDoc.OptionFixNestedTags = true;

            htmlDoc.LoadHtml(json);

            if (htmlDoc.ParseErrors.Count() == 0)
            {
                if (htmlDoc.DocumentNode != null)
                {
                    var nodeCollections = htmlDoc.DocumentNode.SelectNodes($"//span[contains(@id, 'aq_{currenciesCode.ToLower()}_c')]");
                    var result = nodeCollections.First().InnerHtml.Replace(".", ",");
                    return Convert.ToDouble(result);
                }
            }

            return 0;
        }
    }
}
