using MiniMapper.Core;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Mapper.CreateMap<DataModel, DataContract>();

            var dataModel = new DataModel {StringField = "stringValue", IntegerField = 72};
            var dataContract = new DataContract();

            Mapper.Map(dataModel, dataContract);
        }
    }
}
