using System.Collections.Generic;
using Newtonsoft.Json;

namespace Logic.Generic.Twitter {
	public class Tweet {
		[JsonProperty(PropertyName = "Id")]
		public long Id { get; set; }

		[JsonProperty(PropertyName = "Text")]
		public string Text { get; set; }

		[JsonProperty(PropertyName = "IsRetweet")]
		public bool IsRetweet { get; set; }

		[JsonProperty(PropertyName = "CreatedById")]
		public long CreatedById { get; set; }
	}
}