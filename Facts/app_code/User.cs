using System;
using System.Data;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Facts {
	public class User {
		public uint Id { get; set; }
		public DateTime Created { get; set; }
		public String Name { get; set; }
		public String Email { get; set; }
		public byte[] Salt { get; set; }
		public byte[] Hash { get; set; }
		public uint ActionPoints { get; set; }

		// cached
		public uint MessageCount { get; set; }
		public int Reputation { get; set; }

		private enum UsersTableField {
			Id,
			Created,
			Name,
			Email,
			Salt,
			Hash,
			ActionPoints,
		}

		public User(IDataReader reader) {
			Id = (uint)reader.GetInt32((int)UsersTableField.Id);
			Created = reader.GetDateTime((int)UsersTableField.Created);
			Name = reader.GetString((int)UsersTableField.Name);
			Email = reader.GetString((int)UsersTableField.Email);
			Salt = new byte[16];
			reader.GetBytes((int)UsersTableField.Salt, 0, Salt, 0, 16);
			Hash = new byte[32];
			reader.GetBytes((int)UsersTableField.Hash, 0, Hash, 0, 32);
			ActionPoints = (uint)reader.GetInt32((int)UsersTableField.ActionPoints);
			usersByEmail[Email] = this;
		}
		
		public static void RecacheAll() {
			foreach (var user in User.users) {
				if (user == null) {
					continue;
				}
				user.MessageCount = 0;
				user.Reputation = 0;
			}
			foreach (var message in Message.messages) {
				if (message == null) {
					continue;
				}
				message.Author.MessageCount++;
				message.Author.Reputation += message.UpVoters.Count;
				message.Author.Reputation -= message.DownVoters.Count;
			}
		}

		public class EmailUsedException: Exception {
		}

		public User(String name, String email, byte[] salt, byte[] hash) {
			lock (User.users) {
				if (usersByEmail.ContainsKey(email)) {
					throw new EmailUsedException();
				}
				Id = (uint)User.users.Count;
			}

			Created = DateTime.Now;
			Name = name;
			Email = email;
			Salt = salt;
			Hash = hash;
			MessageCount = 0;
			Reputation = 0;
			ActionPoints = 10;

			using (var connection = Global.CreateConnection()) {
				using (var command = connection.CreateCommand()) {
					command.CommandText = 
						"INSERT INTO Users SET " +
							"id := @Id, " +
							"created := @Created, " +
							"email := @Email, " +
							"name := @Name, " +
							"salt := @Salt, " +
							"hash := @Hash, " + 
							"action_points := @ActionPoints";
					Global.AddParameter(command, "@Id", Id);
					Global.AddParameter(command, "@Created", Created);
					Global.AddParameter(command, "@Name", Name);
					Global.AddParameter(command, "@Email", Email);
					Global.AddParameter(command, "@Salt", Salt);
					Global.AddParameter(command, "@Hash", Hash);
					Global.AddParameter(command, "@ActionPoints", ActionPoints);
					command.ExecuteNonQuery();
				}
			}

			User.users.Add(this);
			usersByEmail[Email] = this;
		}

		public static User Create(String email, String name, String password) {
			byte[] salt = Security.GenerateSalt();
			byte[] hash = Security.SaltAndHash(password, salt);
			try {
				return new Facts.User(name, email, salt, hash);
			} catch (User.EmailUsedException) {
				return null;
			}
		}

		public static List<User> users = new List<User>();
		private static readonly Dictionary<String, User> usersByEmail = new Dictionary<String, User>();

		public static User FindByEmail(String email) {
			User user;
			return usersByEmail.TryGetValue(email, out user) ? user : null;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static bool Equals(byte[] a, byte[] b) {
			if (a == b) {
				return true;
			}
			if (a == null || b == null) {
				return false;
			}
			if (a.Length != b.Length) {
				return false;
			}
			for (int i = 0; i < a.Length; ++i) {
				if (a[i] != b[i]) {
					return false;
				}
			}
			return true;
		}

		public static User LogIn(String email, String password) {
			var user = Facts.User.FindByEmail(email);
			if (user == null) {
				return null;
			}
			byte[] hash = Security.SaltAndHash(password, user.Salt);
			return Equals(hash, user.Hash) ? user : null;
		}

		public bool ChangeActionPoints(int d) {
			if (ActionPoints + d < 0) {
				return false;
			}
			ActionPoints = (uint)(ActionPoints + d);
			using (var connection = Global.CreateConnection()) {
				using (var command = connection.CreateCommand()) {
					command.CommandText = 
						"UPDATE Users SET action_points := @ActionPoints WHERE id = @Id";
					Global.AddParameter(command, "@Id", Id);
					Global.AddParameter(command, "@ActionPoints", ActionPoints);
					command.ExecuteNonQuery();
				}
			}
			return true;
		}
	}
}
