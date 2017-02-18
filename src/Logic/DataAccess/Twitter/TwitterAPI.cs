using System.Collections.Generic;
using Logic.Twitter;
using Tweetinvi;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Models;

namespace Logic.DataAccess.Twitter {
	public class TwitterApi {
		public TwitterApi(TwitterAuth auth) {
			Auth.SetUserCredentials(auth.ConsumerKey, auth.ConsumerSecret, auth.AccessToken, auth.AccessTokenSecret);
		}

		public IEnumerable <long> GetAllFollowersForUser(long userId) {
			IEnumerable <long> followerIds = null;

			try {
				followerIds = User.GetFollowerIds(userId);
			} catch { }

			return followerIds;
		}

		public IEnumerable <long> GetAllFollowingForUser(long userId) {
			IEnumerable <long> followingIds = null;

			try {
				followingIds = User.GetFriendIds(userId);
			} catch { }

			return followingIds;
		}

		public IEnumerable <ITweet> GetTimelineForUser(long userId) {
			IEnumerable <ITweet> tweets = null;
			try {
				tweets = Timeline.GetUserTimeline(userId);
			} catch { }

			return tweets;
		}

		public IUser getUserFromId(long id) { return User.GetUserFromId(id); }
	}
}
