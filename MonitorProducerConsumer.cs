using System;
using System.Collections.Generic;
using System.Threading;

public class MonitorProducerConsumer
{
	private static readonly Queue<int> buffer = new Queue<int>();
	private static readonly int capacity = 5;
	private static readonly object lockObject = new object();

	public static void Run()
	{
		Thread producer = new Thread(() =>
		{
			for (int i = 0; i < 10; i++)
			{
				lock (lockObject)
				{
					while (buffer.Count == capacity) Monitor.Wait(lockObject);
					buffer.Enqueue(i);
					Console.WriteLine($"Produced: {i}");
					Monitor.Pulse(lockObject);
				}
				Thread.Sleep(100);
			}
		});

		Thread consumer = new Thread(() =>
		{
			for (int i = 0; i < 10; i++)
			{
				lock (lockObject)
				{
					while (buffer.Count == 0) Monitor.Wait(lockObject);
					int item = buffer.Dequeue();
					Console.WriteLine($"Consumed: {item}");
					Monitor.Pulse(lockObject);
				}
				Thread.Sleep(500);
			}
		});

		producer.Start();
		consumer.Start();
		producer.Join();
		consumer.Join();
	}
}