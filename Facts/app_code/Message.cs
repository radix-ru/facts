using System;
using System.Collections.Generic;
using System.Data;

namespace Facts {
	public class Message {
		public static List<Message> messages = new List<Message>();

		public enum Vote: sbyte {
			Down = -1,
			Zero = 0,
			Up = +1,
		}

		public enum MessageType: sbyte {
			Disproving = -1,
			Confirming = +1,
		}

		public uint Id { get; set; }
		public Message Parent { get; set; }
		public User Author { get; set; }
		public DateTime Created { get; set; }
		public MessageType Type { get; set; }
		public String Text { get; set; }

		// cached
		public Thread Thread { get; set; }
		public List<User> UpVoters { get; set; }
		public List<User> DownVoters { get; set; }
		public uint Rating { get; set; }
		public List<Message> Children { get; set; }

		public int VotesScore {
			get {
				return UpVoters.Count - DownVoters.Count;
			}
		}

		public enum MessagesTableColumn {
			Id,
			ParentId,
			AuthorId,
			Created,
			Type,
			Text,
		};

		public Message(IDataReader rdr) {
			Id = (uint)rdr.GetInt32((int)MessagesTableColumn.Id);
			if (rdr.IsDBNull((int)MessagesTableColumn.ParentId)) {
				Parent = null;
			} else {
				var parentId = (uint)rdr.GetInt32((int)MessagesTableColumn.ParentId);
				Parent = messages[(int)parentId];
			}
			var authorId = (uint)rdr.GetInt32((int)MessagesTableColumn.AuthorId);
			Author = User.users[(int)authorId];
			Created = rdr.GetDateTime((int)MessagesTableColumn.Created);
			Type = (MessageType)rdr.GetInt16((int)MessagesTableColumn.Type);
			Text = rdr.GetString((int)MessagesTableColumn.Text);
			Children = new List<Message>();
			UpVoters = new List<User>();
			DownVoters = new List<User>();
		}

		public Message(Thread thread, User author, String text) {
			if (!author.ChangeActionPoints(-2)) {
				return;
			}
			lock (messages) {
				Id = (uint)messages.Count;
			}
			Parent = null;
			Author = author;
			Created = DateTime.Now;
			Type = 0;
			Text = text;
			Thread = thread;
			UpVoters = new List<User>();
			DownVoters = new List<User>();
			Rating = 0;
			Children = new List<Message>();

			using (var connection = Global.CreateConnection()) {
				using (var command = connection.CreateCommand()) {
					command.CommandText =
						"INSERT INTO Messages SET " +
							"id = @Id, " +
							"parent_id = NULL, " +
							"author_id = @AuthorId, " +
							"created = @Created, " +
							"type = 0, " +
							"text = @Text";
					Global.AddParameter(command, "@Id", Id);
					Global.AddParameter(command, "@AuthorId", Author.Id);
					Global.AddParameter(command, "@Created", Created);
					Global.AddParameter(command, "@Text", Text);
					command.ExecuteNonQuery();
				}
			}

			messages.Add(this);
		}

		public Message(Message parent, User author, MessageType type, String text) {
			if (!author.ChangeActionPoints(-2)) {
				return;
			}
			lock (messages) {
				Id = (uint)messages.Count;
			}
			Parent = parent;
			Author = author;
			Created = DateTime.Now;
			Type = type;
			Text = text;
			Thread = parent.Thread;
			UpVoters = new List<User>();
			DownVoters = new List<User>();
			Rating = 0;
			Children = new List<Message>();

			Parent.Children.Add(this);

			Thread.MessageCount++;
			Thread.LastActivity = Created;

			using (var connection = Global.CreateConnection()) {
				using (var command = connection.CreateCommand()) {
					command.CommandText =
						"INSERT INTO Messages SET " +
							"id := @Id, " +
							"parent_id := @ParentId, " +
							"author_id := @AuthorId, " +
							"created := @Created, " +
							"type := @Type, " +
							"text := @Text";
					Global.AddParameter(command, "@Id", Id);
					Global.AddParameter(command, "@ParentId", Parent.Id);
					Global.AddParameter(command, "@AuthorId", Author.Id);
					Global.AddParameter(command, "@Created", Created);
					Global.AddParameter(command, "@Type", Type);
					Global.AddParameter(command, "@Text", Text);
					command.ExecuteNonQuery();
				}
			}

			messages.Add(this);
		}

		private enum VotesTableColumn {
			Message,
			User,
			Type,
		}

		public static void RegisterVote(IDataReader reader) {
			var messageId = (uint)reader.GetInt32((int)VotesTableColumn.Message);
			var userId = (uint)reader.GetInt32((int)VotesTableColumn.User);
			var type = (sbyte)reader.GetInt16((int)VotesTableColumn.Type);
			if (type == +1) {
				messages[(int)messageId].UpVoters.Add(User.users[(int)userId]);
			} else {
				messages[(int)messageId].DownVoters.Add(User.users[(int)userId]);
			}
		}

		public void ToggleVote(User user, Vote type) {
			Vote curVote = UpVoters.Contains(user) ? Vote.Up : DownVoters.Contains(user) ? Vote.Down : Vote.Zero;
			if (curVote == Vote.Zero && !user.ChangeActionPoints(-1)) {
				return;
			}
			if (type == curVote) {
				user.ChangeActionPoints(+1);
			}
			if (curVote == Vote.Up) {
				UpVoters.Remove(user);
			} else if (curVote == Vote.Down) {
				DownVoters.Remove(user);
			}
			if (type != curVote) {
				if (type == Vote.Up) {
					UpVoters.Add(user);
				} else {
					DownVoters.Add(user);
				}
			}
			String commandText;
			if (curVote == Vote.Zero) {
				commandText = "INSERT INTO Votes SET message_id := @MessageId, user_id := @UserId, type := @Type";
			} else if (type != curVote) {
				commandText = "UPDATE Votes SET type := @Type WHERE message_id = @MessageId AND user_id = @UserId";
			} else { // type == curVote
				commandText = "DELETE FROM Votes WHERE message_id = @MessageId AND user_id = @UserId";
			}
			using (var connection = Global.CreateConnection()) {
				using (var command = connection.CreateCommand()) {
					command.CommandText = commandText;
					Global.AddParameter(command, "@Type", type);
					Global.AddParameter(command, "@MessageId", Id);
					Global.AddParameter(command, "@UserId", user.Id);
					command.ExecuteNonQuery();
				}
			}
			Message.RecacheUpwards(this);
		}

		public void Reply(User author, MessageType type, String text) {
			var message = new Message(this, author, type, text);
		}

		public Vote GetVoteOf(User user) {
			return UpVoters.Contains(user) ? Vote.Up : DownVoters.Contains(user) ? Vote.Down : Vote.Zero;
		}

		public static uint VotesTreshold = 1000;

		private void RecacheThreadAndRating(Thread thread) {
			Thread = thread;
			int totalVotesScore = VotesScore;
			foreach (var child in Children) {
				child.RecacheThreadAndRating(thread);
				totalVotesScore += (int)child.Type*(int)child.Rating;
			}
			if (totalVotesScore < 0) {
				Rating = 0;
			} else if (totalVotesScore > VotesTreshold) {
				Rating = VotesTreshold;
			} else {
				Rating = (uint)totalVotesScore;
			}
		}

		public static void RecacheAll() {
			foreach (var message in messages) {
				if (message != null && message.Parent != null) {
					message.Parent.Children.Add(message);
				}
			}
			foreach (var message in messages) {
				if (message == null) {
					continue;
				}
				message.Children.Sort(delegate(Message a, Message b) {
					return a.Id < b.Id ? -1 : a.Id > b.Id ? +1 : 0;
				});
			}
			foreach (var thread in Thread.threads) {
				if (thread == null) {
					continue;
				}
				thread.Root.RecacheThreadAndRating(thread);
			}
		}

		public static void RecacheUpwards(Message message) {
			while (message != null) {
				int totalVotesScore = message.VotesScore;
				foreach (var child in message.Children) {
					totalVotesScore += (int)child.Type*(int)child.Rating;
				}
				if (totalVotesScore < 0) {
					message.Rating = 0;
				} else if (totalVotesScore > VotesTreshold) {
					message.Rating = VotesTreshold;
				} else {
					message.Rating = (uint)totalVotesScore;
				}
				message = message.Parent;
			}
		}

		public void MessageCountAndLastActivity(out uint mc, out DateTime la) {
			mc = 1;
			la = Created;
			foreach (var child in Children) {
				uint childMc;
				DateTime childLa;
				child.MessageCountAndLastActivity(out childMc, out childLa);
				mc += childMc;
				if (childLa > la) {
					la = childLa;
				}
			}
		}
	}
}