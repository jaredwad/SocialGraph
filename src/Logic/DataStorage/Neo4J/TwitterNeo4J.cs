using System;
using System.Collections.Generic;
using System.Linq;
using Logic.Generic.Twitter;
using Neo4jClient;

namespace Logic.DataStorage.Neo4J {
	public class TwitterNeo4J {
		private GraphClient client;

		public TwitterNeo4J() {
			client = new GraphClient(new Uri("http://localhost:7474/db/data"), "SocialGraph", "SocialGraph");

			client.Connect();
		}

		public void InsertTweet(Tweet tweet) {
			if (!TweetInDb(tweet.Id)) {
				client.Cypher
				      .Create("(tweet:Tweet {tweet})")
				      .WithParam("tweet", tweet)
				      .ExecuteWithoutResults();
			}
		}

		public void InsertUser(TwitterUser user) {
			if (!UserInDb(user.Id)) {
				client.Cypher
				      .Create("(user:TwitterUser {user})")
				      .WithParam("user", user)
				      .ExecuteWithoutResults();
			}
		}

		public void AddTweeted(TwitterUser user, Tweet tweet) {
			client.Cypher
			      .Match("(u:TwitterUser)", "(t:Tweet)")
			      .Where((TwitterUser u) => u.Id == user.Id)
			      .AndWhere((Tweet t) => t.Id == tweet.Id)
			      .CreateUnique("(u)-[:TWEETED]->(t)")
			      .ExecuteWithoutResults();
		}

		public void AddFollowing(TwitterUser user1, TwitterUser user2) {
			client.Cypher
			      .Match("(u1:TwitterUser)", "(u2:TwitterUser)")
			      .Where((TwitterUser u1) => u1.Id == user1.Id)
			      .AndWhere((TwitterUser u2) => u2.Id == user2.Id)
			      .CreateUnique("(u1)-[:FOLLOWS]->(u2)")
			      .ExecuteWithoutResults();
		}

		public IEnumerable <Tweet> GetAllTweetsForUser(long userId) {
			return client.Cypher
			             .Match($"(t:Tweet)")
			             .Return(t => t.As <Tweet>())
			             .Results;
		}

		public TwitterUser GetUser(long id) {
			return client.Cypher
			             .Match("(u: TwitterUser)")
			             .Where((TwitterUser u) => u.Id == id)
			             .Return(u => u.As <TwitterUser>())
			             .Results
			             .FirstOrDefault();
		}

		public Tweet GetTweet(long id) {
			return client.Cypher
			             .Match("(t: Tweet)")
			             .Where((Tweet t) => t.Id == id)
			             .Return(t => t.As <Tweet>())
			             .Results
			             .FirstOrDefault();
		}

		public bool UserInDb(long id) { return GetUser(id) != null; }
		public bool TweetInDb(long id) { return GetTweet(id) != null; }

		private string UserToCypher(TwitterUser user) {
			return "(" + user.Id + ": TwitterUser {"
			       + "Name:" + user.Name
			       + ",ScreenName:" + user.ScreenName
			       + ",Description:" + user.Description
			       + ",Language:" + user.Language
			       + ",Location:" + user.Location
			       + "})";
		}

		private string TweetToCypher(Tweet tweet) {
			return "(" + tweet.Id + ": User {"
			       + "Text:" + tweet.Text
			       + "})";
		}
	}
}
