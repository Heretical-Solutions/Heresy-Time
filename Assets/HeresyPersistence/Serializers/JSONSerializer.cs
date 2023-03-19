namespace HereticalSolutions.Persistence.Serializers
{
    public class JSONSerializer : ISerializer
    {
        public bool Serialize<TValue>(object medium, TValue DTO)
        {
            throw new System.NotImplementedException();
        }

        public bool Deserialize<TValue>(object medium, out TValue DTO)
        {
            throw new System.NotImplementedException();
        }
    }
}