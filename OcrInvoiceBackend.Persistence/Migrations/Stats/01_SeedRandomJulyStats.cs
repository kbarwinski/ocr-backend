using FluentMigrator;
using OcrInvoiceBackend.Domain.Entities;
using OcrInvoiceBackend.Persistence.Context;

namespace OcrInvoiceBackend.Persistence.Migrations.Stats
{
    [Migration(202308091823)]
    public class _01_SeedRandomJulyStats : BaseMigration
    {
        private readonly DataContext _dataContext;
        private readonly Random _rng;

        public _01_SeedRandomJulyStats(DataContext dataContext)
        {
            _dataContext = dataContext;
            _rng = new Random();
        }

        public override void Up()
        {
            List<Statistics> randomStats = new List<Statistics>();

            var start = new DateTime(2023, 07, 01);
            var end = new DateTime(2023, 07, 31);

            for (var day = start.Date; day <= end.Date; day = day.AddDays(1))
            {
                var stat = GenerateRandomStatistics();
                stat.DateCreated = day.ToUniversalTime();

                randomStats.Add(stat);
            }

            _dataContext.Set<Statistics>().AddRange(randomStats);

            _dataContext.SaveChanges();
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }

        private Statistics GenerateRandomStatistics()
        {
            var res = new Statistics();

            res.InvoicesUploaded = _rng.Next(50, 500);
            res.InvoicesScanned = _rng.Next(res.InvoicesUploaded / 4, res.InvoicesUploaded);
            res.InvoicesParsed = _rng.Next(res.InvoicesScanned / 2, res.InvoicesScanned);
            res.InvoicesApproved = _rng.Next(res.InvoicesParsed / 2, res.InvoicesParsed);

            res.DetailsParsed = res.InvoicesParsed * 7;
            res.DetailsApproved = res.InvoicesApproved * 7;

            res.DetailsGuessed = res.DetailsApproved / _rng.Next(2, 5);
            res.DetailsCorrected = res.DetailsApproved - res.DetailsGuessed;

            res.TotalUploadTime = _rng.NextDouble() * _rng.Next(1, 300);
            res.TotalScanTime = _rng.NextDouble() * _rng.Next(1, 300);
            res.TotalParsingTime = _rng.NextDouble() * _rng.Next(1, 300);

            res.TotalScanCertainty = res.InvoicesScanned / _rng.Next(2, 5);
            res.TotalParsingCertainty = res.DetailsParsed / _rng.Next(2, 5);

            res.AverageUploadTime = res.TotalUploadTime / res.InvoicesUploaded;
            res.AverageScanTime = res.TotalScanTime / res.InvoicesScanned;
            res.AverageParsingTime = res.TotalParsingTime / res.InvoicesParsed;

            res.AverageScanCertainty = res.TotalScanCertainty / res.InvoicesScanned;
            res.AverageParsingCertainty = res.TotalParsingCertainty / res.InvoicesParsed;

            return res;
        }
    }
}
