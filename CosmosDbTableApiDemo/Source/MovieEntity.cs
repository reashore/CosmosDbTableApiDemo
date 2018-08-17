using Microsoft.Azure.CosmosDB.Table;

namespace CosmosDbTableApiDemo.Source
{
	public class MovieEntity : TableEntity
	{
	    public MovieEntity()
	    {

	    }

		public MovieEntity(string genre, string movieTitle)
		{
			PartitionKey = genre;
			RowKey = movieTitle;
		}

		public string Genre => PartitionKey;

		public string Title => RowKey;

		public int Year { get; set; }

		public string Length { get; set; }

		public string Description { get; set; }
	}
}
