using System;
using System.Threading.Channels;
using System.Threading.Tasks;

public class ChannelsExample
{
	public static async Task Run()
	{
		var channel = Channel.CreateBounded<int>(new BoundedChannelOptions(5)
		{ FullMode = BoundedChannelFullMode.Wait });

		Task producerTask = Task.Run(async () =>
		{
			for (int i = 0; i < 10; i++)
			{
				await channel.Writer.WriteAsync(i);
				Console.WriteLine($"Channel Producer: Wrote {i}");
				await Task.Delay(100);
			}
			channel.Writer.Complete();
		});

		Task consumerTask = Task.Run(async () =>
		{
			await foreach (var item in channel.Reader.ReadAllAsync())
			{
				Console.WriteLine($"Channel Consumer: Read {item}");
				await Task.Delay(500);
			}
			Console.WriteLine("Channel Consumer: Finished reading.");
		});

		await Task.WhenAll(producerTask, consumerTask);
	}
}