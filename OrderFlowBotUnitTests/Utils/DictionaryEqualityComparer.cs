namespace OrderFlowBotUnitTests.Utils
{
    public class DictionaryEqualityComparer<TKey, TValue> : IEqualityComparer<IDictionary<TKey, TValue>>
    {
        public bool Equals(IDictionary<TKey, TValue> dict1, IDictionary<TKey, TValue> dict2)
        {
            if (dict1 == dict2) return true;
            if (dict1 == null || dict2 == null) return false;
            if (dict1.Count != dict2.Count) return false;

            var valueComparer = EqualityComparer<TValue>.Default;

            foreach (var kvp in dict1)
            {
                if (!dict2.TryGetValue(kvp.Key, out var value)) return false;
                if (!valueComparer.Equals(value, kvp.Value)) return false;
            }
            return true;
        }

        public int GetHashCode(IDictionary<TKey, TValue> obj)
        {
            throw new NotImplementedException();
        }
    }
}
