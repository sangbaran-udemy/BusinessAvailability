using BusinessAvailability.API.Models.Domain;
using CsvHelper;
using System.Globalization;
using System.Xml.Linq;

namespace BusinessAvailability.API.Repositories
{
    public class BusinessRepository : IBusinessRepository
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<BusinessRepository> logger;

        public BusinessRepository(IConfiguration configuration, ILogger<BusinessRepository> logger)
        {
            this.configuration = configuration;
            this.logger = logger;
        }
        public IEnumerable<BusinessService> GetAll()
        {
            var busiessServices = GetBusinessDataFromCSV();
            return busiessServices;
        }

        public IEnumerable<BusinessService> GetAvailableBusinesses(string openTime, string closeTime)
        {
            var startTime = DateTime.Parse(openTime, CultureInfo.InvariantCulture);
            var endTime = DateTime.Parse(closeTime, CultureInfo.InvariantCulture);

            var busiessServices = GetBusinessDataFromCSV();
            var availableBusinesses = busiessServices.Where(timings => timings.OpenTime >= startTime 
            && timings.CloseTime <= endTime).ToList();

            return availableBusinesses;
        }

        public IEnumerable<BusinessService> GetBusinessDataFromCSV()
        {
            var businessServices = new List<BusinessService>();
            try
            {
                var filePath = configuration.GetValue<string>("CSVFilePath");
                if (!string.IsNullOrEmpty(filePath))
                {
                    var csvFilePaths = Directory.GetFiles(filePath);
                    foreach (var csvFilePath in csvFilePaths)
                    {
                        using (var fileReader = File.OpenText(csvFilePath))
                        {
                            using (var csvParser = new CsvParser(fileReader, CultureInfo.InvariantCulture))
                            {
                                var record = csvParser.Read();

                                while (record = csvParser.Read())
                                {
                                    businessServices.Add(
                                        new BusinessService
                                        {
                                            Id = Convert.ToInt16(csvParser.Record[Fields.BusinessId]),
                                            Name = csvParser.Record[Fields.Name],
                                            Category = csvParser.Record[Fields.Category],
                                            Location = csvParser.Record[Fields.Location],
                                            OpenTime = DateTime.Parse(csvParser.Record[Fields.OpenTime], CultureInfo.InvariantCulture),
                                            CloseTime = DateTime.Parse(csvParser.Record[Fields.CloseTime], CultureInfo.InvariantCulture)
                                        });
                                }
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                logger.LogError("Error whilereading the data from the CSV file: Exception: " + ex.InnerException);
            }
            
            return businessServices;
        }

        public static class Fields
        {
            public const int BusinessId = 0;
            public const int Name = 1;
            public const int Category = 2;
            public const int Location = 3;
            public const int OpenTime = 4;
            public const int CloseTime = 5;
        }
    }
}
