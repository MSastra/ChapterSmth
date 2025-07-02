using System;
using System.Threading.Tasks;
using SemaphoreSlimExampleNamespace;

internal class Program
{
	private static async Task Main(string[] args)
	{
		//1: Monitor Producer/ Consumer(synchronous)
		//MonitorProducerConsumer.Run();

		//2: SemaphoreSlim (async)
		await SemaphoreSlimExample.Run();

		// 3: Channels (async)
		//await ChannelsExample.Run();

		// 4: Async SQL Isolation Setup (async)
		//await AsyncSqlIsolationExamples.SetupDatabase();
		//await AsyncSqlIsolationExamples.ReadAllProducts();
	}
}