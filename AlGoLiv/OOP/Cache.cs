using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using AlGoLiv.ProbSol.DoublyLinkedList;
namespace AlGoLiv.OOP
{

    //https://github.com/anomaly2104/cache-low-level-system-design   

public class CacheFactory<Key, Value>
    {

        public Cache<Key, Value> defaultCache( int capacity)
        {
            return new Cache<Key, Value>(new LRUEvictionPolicy<Key>(),
                    new HashMapBasedStorage<Key, Value>(capacity));
        }
    }

    public interface Storage<Key, Value>
    {
        void Add(Key key, Value value);

        void remove(Key key);

        Value get(Key key);
    }

    public class Cache<Key, Value>
    {
        private EvictionPolicy<Key> evictionPolicy;
        private  Storage<Key, Value> storage;

        public Cache(EvictionPolicy<Key> evictionPolicy, Storage<Key, Value> storage)
        {
            this.evictionPolicy = evictionPolicy;
            this.storage = storage;
        }

        public void put(Key key, Value value)
        {
            try
            {
                this.storage.Add(key, value);
                this.evictionPolicy.keyAccessed(key);
            }
            catch (StorageFullException exception)
            {
                Console.WriteLine("Got storage full. Will try to evict.");
                Key keyToRemove = evictionPolicy.evictKey();
                if (keyToRemove == null)
                {
                    throw new RuntimeException("Unexpected State. Storage full and no key to evict.");
                }
                this.storage.remove(keyToRemove);
                Console.WriteLine("Creating space by evicting item..." + keyToRemove);
                put(key, value);
            }
        }

        public Value get(Key key)
        {
            try
            {
                Value value = this.storage.get(key);
                this.evictionPolicy.keyAccessed(key);
                return value;
            }
            catch (NotFoundException notFoundException)
            {
                Console.WriteLine("Tried to access non-existing key.");
                return default(Value);
            }
        }


    }

    [Serializable]
    internal class RuntimeException : Exception
    {
        public RuntimeException()
        {
        }

        public RuntimeException(string message) : base(message)
        {
        }

        public RuntimeException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected RuntimeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    public class HashMapBasedStorage<Key, Value> : Storage<Key, Value> {

        Dictionary<Key, Value> storage;
        private int capacity;

    public HashMapBasedStorage(int capacity)
    {
        this.capacity = capacity;
        storage = new Dictionary<Key, Value>();
    }

    
    public void Add(Key key, Value value) 
    {
        if (isStorageFull()) throw new StorageFullException("Capacity Full.....");
        storage.Add(key, value);
    }


    public void remove(Key key) 
{
        if (!storage.ContainsKey(key)) throw new NotFoundException(key + "doesn't exist in cache.");
        storage.Remove(key);
    }

    
    public Value get(Key key) 
{
        if (!storage.ContainsKey(key)) throw new NotFoundException(key + "doesn't exist in cache.");
        return storage[key];
    }

    private bool isStorageFull()
{
    return storage.Count == capacity;
}
}

    public interface EvictionPolicy<Key>
    {

        void keyAccessed(Key key);

        /**
         * Evict key from eviction policy and return it.
         */
        Key evictKey();
    }
    /**
 * Eviction policy based on LRU algorithm.
 *
 * @param <Key> Key type.
 */
    public class LRUEvictionPolicy<Key> : EvictionPolicy<Key> {

    private DoublyLinkedList<Key> dll;
    private Dictionary<Key, DoublyLinkedListNode<Key>> mapper;

    public LRUEvictionPolicy()
    {
            this.dll = new DoublyLinkedList<Key>();
            this.mapper = new Dictionary<Key, DoublyLinkedListNode<Key>>();
    }

    
    public void keyAccessed(Key key)
    {
        if (mapper.ContainsKey(key))
        {
            dll.detachNode(mapper[key]);
            dll.addNodeAtLast(mapper[key]);
        }
        else
        {
            DoublyLinkedListNode<Key> newNode = dll.addElementAtLast(key);
            mapper.Add(key, newNode);
        }
    }

    
    public Key evictKey()
    {
        DoublyLinkedListNode<Key> first = dll.getFirstNode();
        if (first == null)
        {
            return default(Key);
        }
        dll.detachNode(first);
        return first.Element;
    }
}

[Serializable]
    internal class StorageFullException : Exception
    {
        public StorageFullException()
        {
        }

        public StorageFullException(string message) : base(message)
        {
        }

        public StorageFullException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected StorageFullException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
