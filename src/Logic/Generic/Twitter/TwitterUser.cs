using Newtonsoft.Json;

namespace Logic.Generic.Twitter {
	public class TwitterUser {
		[JsonProperty(PropertyName = "Id")]
		public long Id { get; set; }

		[JsonProperty(PropertyName = "Name")]
		public string Name { get; set; }

		[JsonProperty(PropertyName = "ScreenName")]
		public string ScreenName { get; set; }

		[JsonProperty(PropertyName = "Description")]
		public string Description { get; set; }

		[JsonProperty(PropertyName = "Language")]
		public string Language { get; set; }

		[JsonProperty(PropertyName = "Location")]
		public string Location { get; set; }
	}
}