using System;
using System.Data;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Facts {
	public class Thread {
		public static List<Thread> threads = new List<Thread>();

		public uint Id { get; set; }
		public String Title { get; set; }
		public Message Root { get; set; }

		// cached
		public uint MessageCount { get; set; }
		public DateTime LastActivity { get; set; }

		private enum ThreadsTableField {
			Id,
			Title,
			RootId,
		}

		public Thread(IDataReader rdr) {
			Id = (uint)rdr.GetInt32((int)ThreadsTableField.Id);
			Title = rdr.GetString((int)ThreadsTableField.Title);
			var rootId = (uint)rdr.GetInt32((int)ThreadsTableField.RootId);
			Root = Message.messages[(int)rootId];
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void Recache() {
			uint messageCount;
			DateTime lastActivity;
			Root.MessageCountAndLastActivity(out messageCount, out lastActivity);
			MessageCount = messageCount;
			LastActivity = lastActivity;
		}

		public static void RecacheAll() {
			foreach (var thread in Thread.threads) {
				if (thread == null) {
					continue;
				}
				thread.Recache();
			}
		}

		public Thread(String title) {
			lock (Thread.threads) {
				Id = (uint)Thread.threads.Count;
			}
			Title = title;
			Root = null;
			MessageCount = 0;

			String commandText = 
				"INSERT INTO Threads SET " +
					"id := @Id, " +
					"title := @Title, " +
					"root_id := NULL";
			using (var connection = Global.CreateConnection()) {
				using (var command = connection.CreateCommand()) {
					command.CommandText = commandText;
					Global.AddParameter(command, "@Id", Id);
					Global.AddParameter(command, "@Title", Title);
					command.ExecuteNonQuery();
				}
			}

			Thread.threads.Add(this);
		}

		public void SetRoot(Message message) {
			Root = message;
			MessageCount = 1;
			LastActivity = message.Created;

			using (var connection = Global.CreateConnection()) {
				using (var command = connection.CreateCommand()) {
					command.CommandText = "UPDATE Threads SET root_id := @RootId WHERE id = @Id";
					Global.AddParameter(command, "@RootId", Root.Id);
					Global.AddParameter(command, "@Id", Id);
					command.ExecuteNonQuery();
				}
			}
		}

		public static Thread CreateThread(User author, String title, String text) {
			var thread = new Thread(title);
			var message = new Message(thread, author, text);
			thread.SetRoot(message);
			return thread;
		}
	}
}
