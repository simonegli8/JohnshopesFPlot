using System;
using System.IO;
using System.Diagnostics;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Linq;

namespace Backup {
	class Program {

		static void Main(string[] args) {
			var version = "Latest";

			try {

				if (args.Length > 0) {
					// get current project version from main assembly
					Assembly a = Assembly.LoadFrom(args[0]);
					if (a != null) {
						var ver = a.GetCustomAttribute<AssemblyFileVersionAttribute>()?.Version ?? a.GetCustomAttribute<AssemblyVersionAttribute>()?.Version;
						Match m = Regex.Match(ver.ToString(), @"^(\d+.\d+)");
						if (m != null) version = m.Groups[1].Value;
					}
				}

			} catch (Exception ex) {
			}
			var variable = "Version";
			if (args.Length > 1) variable = args[1];
			Environment.SetEnvironmentVariable(variable, version, EnvironmentVariableTarget.Process);
			Console.WriteLine(version);
		}
	}
}
