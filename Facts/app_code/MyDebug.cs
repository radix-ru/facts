using System;
using System.IO;
using System.Diagnostics;

namespace Facts {
	public static class MyDebug {
		public class AssertException: Exception {
			public AssertException(String message): base(message) {
			}
		}

		[Conditional("DEBUG")]
		public static void Assert(bool condition) {
			if (!condition) {
				throw new AssertException("Assertion failed");
			}
		}

		[Conditional("DEBUG")]
		public static void Assert(bool condition, String conditionStr) {
			if (!condition) {
				throw new AssertException("Assertion " + conditionStr + " failed");
			}
		}

		[Conditional("DEBUG")]
		public static void Log(String message) {
			using (StreamWriter sw = File.AppendText("log.log")) {
				sw.WriteLine(DateTime.Now + ": " + message);
			}
		}
	}
}
