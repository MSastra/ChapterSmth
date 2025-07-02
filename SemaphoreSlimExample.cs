using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SemaphoreSlimExampleNamespace;

public class SemaphoreSlimExample
{
	private static SemaphoreSlim semaphore = new SemaphoreSlim(3);

	public static async Task Run()
	{
		List<Task> tasks = new List<Task>();
		for (int i = 0; i < 10; i++)
		{
			int taskId = i;
			tasks.Add(Task.Run(async () => await AccessResource(taskId)));
		}
		await Task.WhenAll(tasks);
	}

	private static async Task AccessResource(int taskId)
	{
		Console.WriteLine($"Task {taskId}: Waiting for semaphore...");
		await semaphore.WaitAsync();

		try
		{
			Console.WriteLine($"Task {taskId}: Acquired. Accessing resource.");
			await Task.Delay(new Random().Next(500, 2000));
			Console.WriteLine($"Task {taskId}: Finished resource access.");
		}
		finally
		{
			semaphore.Release();
			Console.WriteLine($"Task {taskId}: Released semaphore.");
		}
	}
}