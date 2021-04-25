using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeSheetApp.Models;

namespace TimesheetApp.Models
{
    public class WorkingDay
    {
        [BsonElement("_id")]
        public ObjectId Id { get; set; }
        [BsonElement("workDate")]
        public DateTime WorkDate { get; set; }
        [BsonElement("workHour")]
        public int WorkHour { get; set; }
        [BsonElement("workingWeekId")]
        public ObjectId WorkingWeekId { get; set; }

        public WorkingDay(ObjectId workingWeekId)
        {
            Id = ObjectId.GenerateNewId();
            WorkDate = new DateTime();
            WorkHour = 0;
            WorkingWeekId = workingWeekId;
        }
    }
}
