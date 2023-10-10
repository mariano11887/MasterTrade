using _3.Repository;
using _3.Repository.Repository;
using _4.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;

namespace _2.Service.Service
{
    public class ServiceCryptoPair
    {
        private readonly RepositoryCandle repositoryCandle;
        private readonly RepositorySupplier repositorySupplier;
        private readonly RepositoryCryptoPair repositoryCryptoPair;

        private readonly HttpClient httpClient;

        public ServiceCryptoPair()
        {
            repositoryCandle = new RepositoryCandle();
            repositorySupplier = new RepositorySupplier();
            repositoryCryptoPair = new RepositoryCryptoPair();

            httpClient = new HttpClient();
        }

        public void ImportCandles()
        {
            string baseUri = repositorySupplier.GetQuery().Select(s => s.Url).FirstOrDefault();

            DateTime startDate = new DateTime(2023, 8, 12);
            DateTime endDate = new DateTime(2023, 10, 1);

            for (int i = 0; i < endDate.DayOfYear - startDate.DayOfYear; i++)
            {
                DateTime dayToImport = startDate.AddDays(i);
                string startTimestamp = dayToImport.Subtract(new DateTime(1970, 1, 1)).TotalSeconds.ToString();
                string endTimestamp = dayToImport.AddDays(1).AddMinutes(-30).Subtract(new DateTime(1970, 1, 1)).TotalSeconds.ToString();

                string uri = baseUri.Replace("{From}", startTimestamp).Replace("{To}", endTimestamp);
                string responseContent = "";

                using (HttpResponseMessage response = httpClient.GetAsync(uri).Result)
                {
                    responseContent = response.Content.ReadAsStringAsync().Result;
                }
                CandleImportResponse candleImportResponse = JsonConvert.DeserializeObject<CandleImportResponse>(responseContent);
                if (candleImportResponse.s == "ok")
                {
                    DateTime unixInitialDate = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

                    for (int j = 0; j < candleImportResponse.c.Length; j++)
                    {
                        Candle candle = new Candle
                        {
                            CryptoPairId = 1,
                            StartDate = unixInitialDate.AddSeconds(candleImportResponse.t[j]),
                            Open = candleImportResponse.o[j],
                            Close = candleImportResponse.c[j],
                            High = candleImportResponse.h[j],
                            Low = candleImportResponse.l[j]
                        };

                        repositoryCandle.Insert(candle);
                    }

                    repositoryCandle.SaveChanges();
                }
                else
                {
                    i -= 1;
                }

                Thread.Sleep(90000);
            }
        }

        private class CandleImportResponse
        {
            public decimal[] c { get; set; }
            public decimal[] h { get; set; }
            public decimal[] l { get; set; }
            public decimal[] o { get; set; }
            public long[] t { get; set; }
            public string s { get; set; }
        }

        public List<DTOCryptoPair> GetCryptoPairs()
        {
            List<DTOCryptoPair> result = repositoryCryptoPair.GetQuery().Where(cp => !cp.IsDeleted).Select(cp => new DTOCryptoPair
            {
                Id = cp.Id,
                Name = cp.Name
            }).ToList();

            return result;
        }

        public (DateTime, DateTime) GetDateRange(int cryptoPairId)
        {
            IQueryable<Candle> query = repositoryCandle.GetQuery().Where(c => c.CryptoPairId == cryptoPairId);

            (DateTime, DateTime) result;
            result.Item1 = query.Min(c => c.StartDate);
            result.Item2 = query.Max(c => c.StartDate);

            return result;
        }
    }
}
