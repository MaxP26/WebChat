using Azure;
using Azure.Data.Tables;
using Microsoft.AspNetCore.Identity;

namespace WebChat.Models
{
	public class TRole : TableEntity
	{
		public string Name { get; set; }

		protected override string PartKey => "role";

		public TRole()
			: base()
		{
		}
		public TRole(string name)
			: this()
		{
			Name = name;
		}
	}
}
