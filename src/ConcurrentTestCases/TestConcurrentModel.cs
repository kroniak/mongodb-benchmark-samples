using MongoDB.Bson;

namespace MongodbTransactions.ConcurrentTestCases
{
    public class TestConcurrentModel
    {
        public ObjectId Id { get; set; }

        public int Counter { get; set; } = 0;

        public string LastUpdater { get; set; }

        public int Spread { get; set; }

        public TestStruct Additional { get; set; }
    }
}