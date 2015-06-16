using System;

namespace Facts {
	public static class SignUpChecker {
		public static bool CheckEmail(String email) {
			return email.Contains("@"); // LONG_TODO: properly check email
		}

		public const String NameRequirement = "Name should not be empty";

		public static bool CheckName(String name) {
			return name.Length > 0; // LONG_TODO: probably elaborate?
		}
		
		public const String PasswordRequirement = "Password should be at least 5 characters long";

		public static bool CheckPassword(String name) {
			return name.Length > 5; // LONG_TODO: probably elaborate?
		}
	}
}
