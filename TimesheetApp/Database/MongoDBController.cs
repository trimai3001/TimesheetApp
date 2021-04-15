using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimesheetApp.Helper;

namespace TimesheetApp.Database
{
    public class MongoDBController
    {
        private MongoClient _dbClient;
        private IMongoDatabase _database;
        private IMongoCollection<BsonDocument> _collection;

        private string _connectionString;
        private string _dbName;

        private void ConfigureDatabase()
        {
            var jObject = Utilities.ParseJsonFileToObject(Configure.DB_CONFIG_LOCATION);
            _connectionString = jObject.SelectToken("connectionString").ToString();
            _dbName = jObject.SelectToken("dbName").ToString();
        }


        public void ConnectMongoClient()
        {
            ConfigureDatabase();
            _dbClient = new MongoClient(_connectionString);
            _database = _dbClient.GetDatabase(_dbName);
        }

        public void GetCollection(string collection)
        {
            try
            {
                _collection = _database.GetCollection<BsonDocument>(collection);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public List<BsonDocument> GetAllDocuments()
        {
            var documents = new List<BsonDocument>();
            try
            {
                documents = _collection.Find(new BsonDocument()).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return documents;
        }

        public void InsertDocument(BsonDocument document)
        {
            try
            {
                _collection.InsertOne(document);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void InsertDocuments(List<BsonDocument> documents)
        {
            try
            {
                _collection.InsertMany(documents);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void DeleteDocument(BsonDocument document)
        {
            try
            {
                _collection.DeleteOne(document);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void DeleteDocuments(List<BsonDocument> documents)
        {
            try
            {
                documents.ForEach(document => DeleteDocument(document));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        //public void UpdateDocument(BsonDocument document)
        //{
        //    try
        //    {
        //        var filter = Builders<BsonDocument>.Filter.Eq("id", document.);
        //        _collection.UpdateOne(document);
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.Message);
        //    }
        //}
    }
}
