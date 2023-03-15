using System.Collections.Generic;

namespace HereticalSolutions.Repositories.Factories
{
    public static partial class RepositoriesFactory
    {
        //TODO: toss into proper repository
        #region Dictionary repository with equality comparer
		
        public static DictionaryRepository<TKey, TValue> BuildDictionaryRepository<TKey, TValue>(
            IEqualityComparer<TKey> comparer)
        {
            return new DictionaryRepository<TKey, TValue>(
                new Dictionary<TKey, TValue>(comparer));
        }
		
        #endregion
    }
}