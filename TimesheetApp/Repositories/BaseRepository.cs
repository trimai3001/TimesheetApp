using MongoDB.Driver;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimesheetApp.Helper;

namespace TimesheetApp.Repositories
{
    public abstract class BaseRepository
    {
        protected IMongoDatabase database;
        protected JObject jObject;
        public BaseRepository(IMongoClient client)
        {
            jObject = Utilities.ParseJsonFileToObject(Configure.DB_CONFIG_LOCATION);
            database = client.GetDatabase(jObject.SelectToken("dbName").ToString());
        }
    }
}
