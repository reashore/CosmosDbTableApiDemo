using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using Microsoft.Azure.CosmosDB.Table;
using Microsoft.Azure.Storage;

namespace CosmosDbTableApiDemo
{
	public static class TableDemo
	{
		public static void Run()
		{
			Debugger.Break();

			string connectionString = ConfigurationManager.AppSettings["StorageConnectionString"];
			CloudStorageAccount account = CloudStorageAccount.Parse(connectionString);
			CloudTableClient client = account.CreateCloudTableClient();
			CloudTable table = client.GetTableReference("Movies");

			ListSciFiMovies(table);

			AddSciFiMovie(table);

			ListSciFiMovies(table);

			DeleteSciFiMovie(table);

			Console.ReadKey();
		}

		private static void ListSciFiMovies(CloudTable table)
		{
			string sciFiFilter = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "sci-fi");
			TableQuery<MovieEntity> query = new TableQuery<MovieEntity>().Where(sciFiFilter);
			IEnumerable<MovieEntity> movies = table.ExecuteQuery(query);

			Console.WriteLine($"Found {movies.Count()} Sci-Fi movies");

			foreach (MovieEntity movie in movies)
			{
			    PrintMovie(movie);
			}
		}

	    private static void PrintMovie(MovieEntity movie)
	    {
	        Console.WriteLine($" Title: {movie.Title}");
	        Console.WriteLine($" Genre: {movie.Genre}");
	        Console.WriteLine($" Year: {movie.Year}");
	        Console.WriteLine($" Length: {movie.Length}");
	        Console.WriteLine($" Description: {movie.Description}");
	    }

	    private static void AddSciFiMovie(CloudTable table)
		{
			MovieEntity movie = new MovieEntity("sci-fi", "Star Wars VI - Return of the Jedi")
			{
				Year = 1983,
				Length = "2hr, 11min",
				Description = "After a daring mission to rescue Han Solo from Jabba the Hutt, the rebels dispatch to Endor to destroy a more powerful Death Star. Meanwhile, Luke struggles to help Vader back from the dark side without falling into the Emperor's trap."
			};
			TableOperation insertOperation = TableOperation.Insert(movie);
			TableResult result = table.Execute(insertOperation);
		}

		private static void DeleteSciFiMovie(CloudTable table)
		{
			TableOperation queryOperation = TableOperation.Retrieve<MovieEntity>("sci-fi", "Star Wars VI - Return of the Jedi");
			TableResult result = table.Execute(queryOperation);
			MovieEntity entity = (MovieEntity)result.Result;

			TableOperation deleteOperation = TableOperation.Delete(entity);
			table.Execute(deleteOperation);
		}
	}
}
