using SolTechnology.Avro;

namespace AvroConvertTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            TestData testData = new()
            {
                ID = 0,
                Name = "강준",
                Age = 30,
                Birth = DateTime.Parse("1990-09-18"),
                Address = "경기도 용인시",
                CrateTime = DateTime.Now
            };

            // 직렬화
            byte[] arvoObject = AvroConvert.Serialize(testData);

            // 역직렬화
            TestData deserializedObject = AvroConvert.Deserialize<TestData>(arvoObject);

            Console.WriteLine("");
            Console.WriteLine("◆ Avro 개체에서 스키마 읽기");
            string schemaInJsonFormat = AvroConvert.GetSchema(arvoObject);
            Console.WriteLine(schemaInJsonFormat);

            Console.WriteLine("");
            Console.WriteLine("◆ 대량 Avro 객체 컬렉션을 하나씩 역직렬화");
            List<TestData> testDatas = new();

            for (int i = 0; i < 100; i++)
            {
                testDatas.Add(new TestData()
                {
                    ID = i,
                    Name = "강준" + i,
                    Age = 30,
                    Birth = DateTime.Parse("1990-09-18"),
                    Address = "경기도 용인시",
                    CrateTime = DateTime.Now
                });
            }

            byte[] arvoObjects = AvroConvert.Serialize(testDatas);

            using (var reader = AvroConvert.OpenDeserializer<TestData>(new MemoryStream(arvoObjects)))
            {
                while (reader.HasNext())
                {
                    var item = reader.ReadNext();
                    Console.WriteLine(item.Name);
                }
            }

            Console.WriteLine("");
            Console.WriteLine("◆ Avro 객체에서 C# 모델 생성");
            string resultModel = AvroConvert.GenerateModel(arvoObject);
            Console.WriteLine(resultModel);

            Console.WriteLine("");
            Console.WriteLine("◆ Avro 객체를 JSON으로 변환");
            var resultJson = AvroConvert.Avro2Json(arvoObject);
            Console.WriteLine(resultJson);

            Console.ReadKey();
        }
    }

    public class TestData
    {
        public int ID { get; set; }
        public string? Name { get; set; }
        public int Age { get; set; }
        public DateTime Birth { get; set; }
        public string? Address { get; set; }
        public DateTime CrateTime { get; set; }

    }
}