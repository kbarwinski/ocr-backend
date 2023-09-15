using Microsoft.EntityFrameworkCore;
using OcrInvoiceBackend.Application.Repositories;
using OcrInvoiceBackend.Domain.Entities;
using OcrInvoiceBackend.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcrInvoiceBackend.Persistence.Repositories
{
    public class StatisticsRepository : BaseRepository<Statistics>, IStatisticsRepository
    {
        public StatisticsRepository(DataContext context) : base(context)
        {
        }

        public async Task<Statistics> GetTodayStatistics()
        {
            var today = DateTime.Now.ToUniversalTime().Date;
            var tomorrow = today.AddDays(1);

            var statistics = await Context.Statistics
                .FirstOrDefaultAsync(s => s.DateCreated >= today && s.DateCreated < tomorrow);

            if (statistics == null)
            {
                statistics = new Statistics();
                base.Create(statistics);
                await Context.SaveChangesAsync();
            }

            return statistics;
        }
    }
}
