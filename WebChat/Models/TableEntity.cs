using Azure;
using Azure.Data.Tables;

namespace WebChat.Models
{
	public abstract class TableEntity : ITableEntity
	{
		abstract protected string PartKey { get; }

		public string PartitionKey { get; set; }
		public string RowKey { get; set; }
		public DateTimeOffset? Timestamp { get; set; }
		public ETag ETag { get; set; }

		public TableEntity()
		{
			PartitionKey = this.PartKey;
			RowKey = Guid.NewGuid().ToString();
			Timestamp = DateTimeOffset.UtcNow;
			ETag = new ETag();
		}

        public TableEntity(string partitionKey, string rowKey, DateTimeOffset? timestamp, ETag eTag)
        {
            PartitionKey = partitionKey;
            RowKey = rowKey;
            Timestamp = timestamp;
            ETag = eTag;
        }
    }
}
