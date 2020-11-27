using System.Configuration;

namespace ClearBank.DeveloperTest.Services
{
    public class DataStoreTypeProvider : IDataStoreTypeProvider
    {
        public string GetDataStoreType()
        {
            return ConfigurationManager.AppSettings["DataStoreType"];
        }
    }
}