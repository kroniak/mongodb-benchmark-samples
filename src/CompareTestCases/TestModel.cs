namespace MongodbTransactions.CompareTestCases
{
    public class TestModel
    {
        public int Counter { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public int Count { get; set; }

        public TestStruct Info { get; set; }
    }

    public struct TestStruct
    {
        public int X;
        public int Y;
    }
}