using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Aot.Net
{
	/// <summary>
	/// Словарь, который в <c>this[key]</c> выдаёт значение по умолчанию вместо эксепшена
	/// </summary>
	/// <typeparam name="K"></typeparam>
	/// <typeparam name="V"></typeparam>
	public class DefaultDictionary<K, V> : IDictionary<K, V>, IReadOnlyDictionary<K, V>
		where K : notnull
	{
		private readonly SortedDictionary<K, V> _dictionary = new();
		private readonly V _defaultValue;

		public DefaultDictionary(V defaultValue)
		{
			_defaultValue = defaultValue;
		}

		public V this[K key]
		{
			get => _dictionary.TryGetValue(key, out var value) ? value : _defaultValue;
			set => _dictionary[key] = value;
		}

		public ICollection<K> Keys => _dictionary.Keys;

		public ICollection<V> Values => _dictionary.Values;

		public int Count => _dictionary.Count;

		bool ICollection<KeyValuePair<K, V>>.IsReadOnly => false;

		IEnumerable<K> IReadOnlyDictionary<K, V>.Keys => Keys;
		IEnumerable<V> IReadOnlyDictionary<K, V>.Values => Values;

		public bool ContainsKey(K key) => _dictionary.ContainsKey(key);

		public IEnumerator<KeyValuePair<K, V>> GetEnumerator() => _dictionary.GetEnumerator();

		public bool TryGetValue(K key, [MaybeNullWhen(false)] out V value) =>
			_dictionary.TryGetValue(key, out value);

		public void Add(K key, V value) => _dictionary.Add(key, value);

		public void Add(KeyValuePair<K, V> item) => _dictionary.Add(item.Key, item.Value);

		public void Clear() => _dictionary.Clear();

		bool ICollection<KeyValuePair<K, V>>.Contains(KeyValuePair<K, V> item) =>
			((ICollection<KeyValuePair<K, V>>)_dictionary).Contains(item);

		void ICollection<KeyValuePair<K, V>>.CopyTo(KeyValuePair<K, V>[] array, int arrayIndex) =>
			((ICollection<KeyValuePair<K, V>>)_dictionary).CopyTo(array, arrayIndex);

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		bool IDictionary<K, V>.Remove(K key) => _dictionary.Remove(key);

		bool ICollection<KeyValuePair<K, V>>.Remove(KeyValuePair<K, V> item) =>
			((ICollection<KeyValuePair<K, V>>)_dictionary).Remove(item);
	}
}
