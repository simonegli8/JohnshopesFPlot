using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using JohnsHope.FPlot.Library;

namespace JohnsHope.FPlot.Tests {
	/// <summary>
	/// Summary description for SourceHeaderSortTest
	/// </summary>
	[TestClass]
	public class SourceHeaderSortTest {
		public SourceHeaderSortTest() {
		}

		private TestContext testContextInstance;

		/// <summary>
		///Gets or sets the test context which provides
		///information about and functionality for the current test run.
		///</summary>
		public TestContext TestContext {
			get {
				return testContextInstance;
			}
			set {
				testContextInstance = value;
			}
		}

		#region Additional test attributes
		//
		// You can use the following additional attributes as you write your tests:
		//
		// Use ClassInitialize to run code before running the first test in the class
		// [ClassInitialize()]
		// public static void MyClassInitialize(TestContext testContext) { }
		//
		// Use ClassCleanup to run code after all tests in a class have run
		// [ClassCleanup()]
		// public static void MyClassCleanup() { }
		//
		// Use TestInitialize to run code before running each test 
		// [TestInitialize()]
		// public void MyTestInitialize() { }
		//
		// Use TestCleanup to run code after each test has run
		// [TestCleanup()]
		// public void MyTestCleanup() { }
		//
		#endregion

		[TestMethod]
		public void TestMethod1() {
			string[] dlls = new string[6]{ @"C:\Temp\Test.dll", @"c:\temp\test.dll", @"c:\temp\a.dll", @"Microsoft.Text.dll", 
				@"JohnsHope.Excel.dll", @"c:\temp\b.dll" };

			CompilerOptions.PackagedAssembly a = new CompilerOptions.PackagedAssembly(@"C:\Temp\test.dll");

			CompilerOptions o = new CompilerOptions(), n = new CompilerOptions();
			n.PackagedImports.AddRange(dlls);
			n.Imports.AddRange(dlls);

			o.Combine(n);
 
		}

	}
}
