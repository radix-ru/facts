using System;
using System.Data;
using System.Web;
using MySql.Data.MySqlClient;
using System.Threading;

namespace Facts {
	public partial class Global: HttpApplication {
		private static readonly String _ConnectionString;

		static Global() {
			var builder = new MySqlConnectionStringBuilder();
			builder.Server = "localhost";
			builder.UserID = "facts_user";
			builder.Password = "qwerty";
			builder.Database = "Facts";
			//builder.CheckParameters = false;
			_ConnectionString = builder.ConnectionString;
		}

		public static IDbConnection CreateConnection() {
			var connection = new MySqlConnection(_ConnectionString);
			connection.Open();
			return connection;
		}

		public static void AddParameter(IDbCommand command, String name, Object value) {
			IDbDataParameter parameter = command.CreateParameter();
			parameter.ParameterName = name;
			parameter.Value = value;
			command.Parameters.Add(parameter);
		}

		/* 1. Load users
		 * 2. Load messages (in order):
		 *  - Link parent (it's already loaded)
		 *  - Link author
		 * 3. Load votes
		 * 4. Load threads: 
		 *  - Link root
		 * 5. Recache messages
		 * 6. Recache users
		 * 7. Recache threads
		 */
		protected void Application_Start(Object sender, EventArgs e) {
			using (var connection = CreateConnection()) {
				using (var command = connection.CreateCommand()) {
					command.CommandText = "SELECT * FROM Users";
					using (var reader = command.ExecuteReader()) {
						while (reader.Read()) {
							var user = new User(reader);
							while (Facts.User.users.Count <= user.Id) {
								Facts.User.users.Add(null);
							}
							Facts.User.users[(int)user.Id] = user;
						}
					}
				}
				using (var command = connection.CreateCommand()) {
					command.CommandText = "SELECT * FROM Messages ORDER BY id";
					using (var reader = command.ExecuteReader()) {
						while (reader.Read()) {
							var message = new Message(reader);
							while (Message.messages.Count <= message.Id) {
								Message.messages.Add(null);
							}
							Message.messages[(int)message.Id] = message;
						}
					}
				}
				using (var command = connection.CreateCommand()) {
					command.CommandText = "SELECT * FROM Votes";
					using (var reader = command.ExecuteReader()) {
						while (reader.Read()) {
							Message.RegisterVote(reader);
						}
					}
				}
				using (var command = connection.CreateCommand()) {
					command.CommandText = "SELECT * FROM Threads";
					using (var reader = command.ExecuteReader()) {
						while (reader.Read()) {
							var thread = new Thread(reader);
							while (Thread.threads.Count <= thread.Id) {
								Thread.threads.Add(null);
							}
							Thread.threads[(int)thread.Id] = thread;
						}
					}
				}
			}

			Message.RecacheAll();
			Facts.User.RecacheAll();
			Thread.RecacheAll();

			var now = DateTime.Now;
			var nextMidnight = now.AddDays(1).Date;

			_Timer = new Timer(UpdateActionPoints, null, (nextMidnight - now).Milliseconds, 24*60*60*1000);
		}

		private static Timer _Timer;

		private void UpdateActionPoints(Object dummy) {
			lock (Facts.User.users) {
				foreach (var user in Facts.User.users) {
					uint ap = user.ActionPoints;
					int rep = user.Reputation;
					uint newAp = rep < 2 ? 2U : rep > 50 ? 50U : (uint)rep + 10;
					user.ChangeActionPoints((int)newAp - (int)ap);
				}
			}
		}

		private const String _UserVar = "User";

		public static new User User {
			get {
				return (User)HttpContext.Current.Session[_UserVar];
			}
			set {
				HttpContext.Current.Session[_UserVar] = value;
			}
		}
	}
}
