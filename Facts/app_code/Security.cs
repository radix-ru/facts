using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace Facts {
	public static class Security {
		private const uint _SaltSize = 16;

		public static byte[] GenerateSalt() {
			var salt = new byte[_SaltSize];
			using (var rng = new RNGCryptoServiceProvider()) {
				rng.GetBytes(salt);
			}
			return salt;
		}

		// The point of this method is to predictably convert string to byte array.
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static byte[] Binarize(String password) {
			var result = new byte[password.Length*2];
			for (int i = 0; i < password.Length; ++i) {
				result[2*i] = (byte)(password[i] & 0xFF00U);
				result[2*i + 1] = (byte)(password[i] & 0x00FFU);
			}
			return result;
		}

		public static byte[] SaltAndHash(String password, byte[] salt) {
			var binarized = Binarize(password);
			var salted = new byte[binarized.Length + salt.Length];
			binarized.CopyTo(salted, 0);
			salt.CopyTo(salted, binarized.Length);
			return new SHA256Managed().ComputeHash(salted);
		}
	}
}