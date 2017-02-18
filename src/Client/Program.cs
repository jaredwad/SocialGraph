using System;
using System.Collections.Generic;
using System.Linq;
using Logic.DataAccess.Twitter;
using Logic.DataStorage.Neo4J;
using Logic.Generic;
using Logic.Generic.Twitter;
using Logic.Twitter;
using Neo4jClient;
using Tweetinvi;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Models;
using Tweet = Logic.Generic.Twitter.Tweet;

namespace Client {
	internal class Program {
		private const long InitialUserId = 1259825000;

		public static void Main(string[] args) {
			Console.WriteLine("Starting Client");
			TwitterToNeo4J();
			Console.WriteLine("Complete");
		}

		public static void RunNeo4J() {
			var client = new GraphClient(new Uri("http://localhost:7474/db/data"), "SocialGraph", "SocialGraph");

			client.Connect();

			var movies = client.Cypher
			                   .Match("(m:Movie)")
			                   .Return(m => m.As <Movie>())
			                   .Limit(10)
			                   .Results;

			foreach (Movie movie in movies) {
				Console.WriteLine("{0} ({1}) - {2}", movie.Title, movie.Released, movie.TagLine);
			}
		}

		public static void RunTwitter() {
			var auth = new TwitterAuth(
			                           "nYy5AhN7oiGMHYtSZwuovXsud",
			                           "40U6Vn9KbACfwKhZUCyNEBuP7XiOoDmrbTTkV0Bah6YdJHtbWF",
			                           "1259825000-AzHEC4d37thxV5YyNxo7QGEllzDmJHbADPVNbsn",
			                           "aYFtTTtBdjnLAjWciD8wJBjomDKlaSqwAnWTscm9cPiMr"
			                          );

			try {
				var api = new TwitterApi(auth);

				foreach (ITweet tweet in api.GetTimelineForUser(InitialUserId)) {
					Console.WriteLine(tweet.Text);

				}
			} catch (Exception e) {
				Console.WriteLine(e.Message);
			}
		}

		public static void TwitterToNeo4J() {
			var auth = new TwitterAuth(
			                           "nYy5AhN7oiGMHYtSZwuovXsud",
			                           "40U6Vn9KbACfwKhZUCyNEBuP7XiOoDmrbTTkV0Bah6YdJHtbWF",
			                           "1259825000-AzHEC4d37thxV5YyNxo7QGEllzDmJHbADPVNbsn",
			                           "aYFtTTtBdjnLAjWciD8wJBjomDKlaSqwAnWTscm9cPiMr"
			                          );

			var t = new TwitterNeo4J();

			try {
				var api = new TwitterApi(auth);

				var tweets = api.GetTimelineForUser(InitialUserId);
				var tweetArray = tweets as ITweet[] ?? tweets.ToArray();

				TwitterUser user = IUserToUser(tweetArray[0].CreatedBy);

				t.InsertUser(user);

				foreach (ITweet iTweet in tweetArray) {

					Tweet tweet = ITweetToTweet(iTweet);

					t.InsertTweet(tweet);
					t.AddTweeted(user, tweet);
				}

				foreach (long userId in api.GetAllFollowingForUser(InitialUserId)) {
					TwitterUser user2 = IUserToUser(api.getUserFromId(userId));

					t.InsertUser(user2);

					t.AddFollowing(user, user2);
				}

				foreach (long userId in api.GetAllFollowersForUser(InitialUserId)) {
					TwitterUser user2 = IUserToUser(api.getUserFromId(userId));

					t.InsertUser(user2);

					t.AddFollowing(user2, user);
				}
			} catch (Exception e) {
				Console.WriteLine(e.Message);
			}
		}

		private static TwitterUser IUserToUser(IUser user) {
			return new TwitterUser {
				                       Id = user.Id,
				                       Name = user.Name,
				                       ScreenName = user.ScreenName,
				                       Description = user.Description,
				                       Language = user.Language.ToString(),
				                       Location = user.Location
			                       };
		}

		private static Tweet ITweetToTweet(ITweet tweet) {
			return new Tweet {
				                 Id = tweet.Id,
				                 CreatedById = tweet.CreatedBy.Id,
				                 Text = tweet.Text,
				                 IsRetweet = tweet.IsRetweet
			                 };
		}
	}
}
