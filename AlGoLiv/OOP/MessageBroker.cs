using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AlGoLiv.OOP
{
    //https://github.com/anomaly2104/low-level-design-messaging-queue-pub-sub
    //https://www.youtube.com/watch?v=4BEzgPlLKTo&t=249s
    public class MessageQueueTester
    {
        public static void Run()
        {
            MessageBroker messageQueue = new MessageBroker();
            Topic topic1 = messageQueue.CreateTopic("NewTopic1");
            Topic topic2 = messageQueue.CreateTopic("NewTopic2");
            
            SleepingSubscirber sleepingSubscirber1 = new SleepingSubscirber("sub1", 1000);
            SleepingSubscirber sleepingSubscirber2 = new SleepingSubscirber("sub2", 1000);
            messageQueue.Subscribe(sleepingSubscirber1, topic1);
            messageQueue.Subscribe(sleepingSubscirber2, topic1);

            SleepingSubscirber sleepingSubscirber3 = new SleepingSubscirber("sub2", 1000);
            messageQueue.Subscribe(sleepingSubscirber3, topic2);


            Message message = new Message("Message1 to NewTopic1");
            messageQueue.Publish(topic1, message);
            messageQueue.Publish(topic1, new Message("Message2 to NewTopic1"));

            messageQueue.Publish(topic2, new Message("Message1 to NewTopic2"));

            Thread.Sleep(15000);

            messageQueue.Publish(topic2, new Message("Message2 to NewTopic1"));
            messageQueue.Publish(topic1, new Message("Message3 to NewTopic1"));

            messageQueue.ResetOffset(topic1, sleepingSubscirber1, 0);
        }
    }
    public class MessageBroker
    {
        private Dictionary<string, TopicHandler> _topicHandler;

        public MessageBroker()
        {
            _topicHandler = new Dictionary<string, TopicHandler>();

        }
        public Topic CreateTopic(string topicName)
        {
            Topic topic = new Topic(topicName, Guid.NewGuid().ToString());
            TopicHandler topicHandler = new TopicHandler(topic);
            _topicHandler[topic.TopicId]= topicHandler;

            Console.WriteLine("CreateTopic {0}", topic.TopicName);

            return topic;
        }

        public void Subscribe (ISubscriber subscriber, Topic topic)
        {
            topic.AddSubscriber(new TopicSubscriber(subscriber));

            Console.WriteLine(subscriber.GetId() + " subscribed to topic: " + topic.TopicName);
        }
        public void Publish(Topic topic, Message msg)
        {
            topic.AddMessage(msg);

            Console.WriteLine(msg.Msg + " published to topic: " + topic.TopicName);

            new Thread(() => { _topicHandler[topic.TopicId].Publish(); }).Start();    
        }

        public void ResetOffset(Topic topic, ISubscriber subscriber, int newOffset)
        {
            foreach(TopicSubscriber ts in topic.Subscribers)
            {
                if (ts.Subscriber.Equals(subscriber))
                {
                    ts.Offset = newOffset;
                    Console.WriteLine(ts.Subscriber.GetId() + " offset reset to " + newOffset);

                    new Thread(() => _topicHandler[topic.TopicId].StartSubscriberWorker(ts)).Start();
                    break;
                }
            }
        }
    }
    public class SleepingSubscirber : ISubscriber
    {
        private string _id;
        private int _sleepTimeInMillis;

        public SleepingSubscirber(string id, int sleepTimeInMillis)
        {
            _id = id;
            _sleepTimeInMillis = sleepTimeInMillis;
        }

        public void Consume(Message msg)
        {
            Console.WriteLine("Subscriber: " + _id + " started consuming: " + msg.Msg);
            Thread.Sleep(_sleepTimeInMillis);
            Console.WriteLine("Subscriber: " + _id + " done consuming: " + msg.Msg);

        }

        public string GetId()
        {
            return _id;
        }
    }
    public class TopicHandler
    {
        private object monitor = new object();
        private Topic _topic;
        private Dictionary<string, SubscriberWorker> _subscriberWorkers;

        public TopicHandler(Topic topic)
        {
            _topic = topic;
            _subscriberWorkers = new Dictionary<string, SubscriberWorker>();
        }

        public void Publish()
        {
            foreach(TopicSubscriber ts in _topic.Subscribers)
            {
                StartSubscriberWorker(ts);
            }
        }

        public void StartSubscriberWorker(TopicSubscriber ts)
        {
            Monitor.Enter(monitor);
            var subid = ts.Subscriber.GetId();
            
            if (!_subscriberWorkers.ContainsKey(subid))
            {
                SubscriberWorker sw = new SubscriberWorker(_topic, ts);
                _subscriberWorkers[subid] = sw;
                sw = _subscriberWorkers[subid];
                new Thread(sw.Run).Start();

            }
            _subscriberWorkers[subid].wakeUpIfNeeded(); //TODO
            Monitor.Exit(monitor);


        }
    }
    public class SubscriberWorker
    {
        private object monitor = new object();
        public SubscriberWorker(Topic topic, TopicSubscriber topicSubscriber)
        {
            Topic = topic;
            TopicSubscriber = topicSubscriber;
        }

        public Topic Topic { get; }
        public TopicSubscriber TopicSubscriber { get; }

        public void Run()
        {
            
            do
            {
                Monitor.Enter(monitor);
                int curOffset = TopicSubscriber.Offset;
                while(curOffset >= Topic.Messages.Count){
                    Monitor.Wait(monitor);
                }
                Message msg = Topic.Messages.ElementAt(curOffset);
                TopicSubscriber.Subscriber.Consume(msg);

                // We cannot just increment here since subscriber offset can be reset while it is consuming. So, after
                // consuming we need to increase only if it was previous one.

                TopicSubscriber.CompareAndExchangeOffset();

                Monitor.Exit(monitor);
            } while (true);
            

        }

        internal void wakeUpIfNeeded()
        {
            lock (monitor)
            {
                Monitor.Pulse(monitor);
            }
            
        }
    }
    public class Topic
    {
        private object monitor = new object();
        private string _topicName;
        private string _topicId;
        private List<Message> _messages;
        private List<TopicSubscriber> _subscribers;

        public Topic(string topicName, string topicId)
        {
            _topicId = topicId;
            _topicName = topicName;
            _messages = new List<Message>();
            _subscribers = new List<TopicSubscriber>();
        }

        public string TopicName { get { return _topicName; } }
        public string TopicId { get { return _topicId; } }
        public IReadOnlyCollection<Message> Messages { get { return _messages.AsReadOnly(); } }
        public IReadOnlyCollection<TopicSubscriber> Subscribers { get { return _subscribers.AsReadOnly(); } }

        public void AddMessage(Message msg)
        {
            lock (monitor)
            {
                _messages.Add(msg);
            }
        }

        public void AddSubscriber(TopicSubscriber subscriber)
        {
            _subscribers.Add(subscriber);
        }
    }
    public interface ISubscriber
    {
        string GetId();
        void Consume(Message msg);
    }
    public class TopicSubscriber
    {
        private int _offset=0;
        private ISubscriber _subscriber;

        public TopicSubscriber(ISubscriber subscriber)
        {
            _subscriber = subscriber;
            Interlocked.Increment(ref _offset);
        }
        public int Offset
        {
            get
            {
                return _offset;
            }
            set
            {
                _offset = value;
            }
        }
        public ISubscriber Subscriber
        {
            get
            {
                return _subscriber;
            }
        }

        internal void CompareAndExchangeOffset()
        {
            Interlocked.CompareExchange(ref _offset, _offset+1, _offset);
        }
    }
    public class Message
    {
        private string _msg;

        public Message(string msg)
        {
            _msg = msg;
        }

        public string Msg
        {
            get
            {
                return _msg;
            }
        }
    }
}
