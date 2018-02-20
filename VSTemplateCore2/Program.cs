using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using VSTemplateCore2.Data;

namespace VSTemplateCore2 {
	public class Program {
		public static void Main(string[] args) {
			var host = BuildWebHost(args);

			using (var scope = host.Services.CreateScope()) {
				// Get the ServiceProvider service so that all other services can be injected as required
				var services = scope.ServiceProvider;

				// Make sure the database has been seeded with the default users and model entities
				try {
					SeedData.Initialize(services).Wait();
				} catch (Exception ex) {
					var logger = services.GetRequiredService<ILogger<Program>>();
					logger.LogError(ex, $"An error occured while creating the default database entities:\n{ex.Message}");
					throw ex;
				}
			}

			host.Run();
		}

		public static IWebHost BuildWebHost(string[] args) =>
				WebHost.CreateDefaultBuilder(args)
						.UseStartup<Startup>()
						.Build();
	}
}
