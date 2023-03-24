namespace HereticalSolutions.Persistence.Serializers
{
    public class XMLSerializer : ISerializer
    {
        #region ISerializer
        
        public bool Serialize<TValue>(ISerializationArgument argument, TValue DTO)
        {
            throw new System.NotImplementedException();
        }

        public bool Deserialize<TValue>(ISerializationArgument argument, out TValue DTO)
        {
            throw new System.NotImplementedException();
        }

        public void Erase(ISerializationArgument argument)
        {
            throw new System.NotImplementedException();
        }
        
        #endregion
    }
}