using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.ComponentModel;
using System.Xml;
using System.Xml.XPath;
using System.Net;
using System.IO;
using System.Globalization;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace JohnsHope.Update {

	[Serializable]
	public class VersionInfo {

		public VersionInfo() {
			Assembly a = Assembly.GetAssembly(typeof(Updater));
			Version = a.GetName().Version;
			Date = DateTime.UtcNow.AddYears(-5);
			homeUrl = "http://www.johnshope.com/fplot";
			updateUrl = homeUrl + "/update.aspx";
			downloadUrl = homeUrl;
			sourceUrl = homeUrl;
			directDownloadUrl = homeUrl + "/download/JohnsHope.FPlot.{version}.exe";
			installerArgs = "";
			netFramework = System.Environment.Version;
			netFrameworkDownloadUrl = "";
			netFrameworkDirectDownloadUrl = "";
			netFrameworkInstallerArgs = "";
			netFrameworkUpdateUrl = homeUrl + "/update.aspx?netFramework=true";
			AutoUpdate = true;
		}

		public Version Version;
		public string DisplayVersion {
			get { return Version.Major.ToString() + "." + Version.Minor.ToString(); }
		}
		Version downloadableVersion = null;
		public Version DownloadableVersion { get { return downloadableVersion ?? Version; } set { downloadableVersion = value; } }

		public DateTime Date;
		public string homeUrl {get; set; }
		public string updateUrl { get; set; }
		public string downloadUrl { get; set; }
		public string sourceUrl { get; set; }
		public string directDownloadUrl { get; set; }
		public string installerArgs { get; set; }
		public Version netFramework { get; set; }
		public string netFrameworkDownloadUrl { get; set; }
		public string netFrameworkDirectDownloadUrl { get; set; }
		public string netFrameworkInstallerArgs { get; set; }
		public string netFrameworkUpdateUrl { get; set; }
		public bool AutoUpdate { get; set; }
		public string directDownloadVersionedUrl {
			get { return directDownloadUrl.Replace("{version}", DownloadableVersion.Major.ToString() + "." + DownloadableVersion.Minor.ToString()); }
		}

		public void Update(VersionInfo newer) {
			if (newer != null) {
				Date = newer.Date;
				Version dv;
				if (newer.Version > newer.DownloadableVersion) dv = newer.Version;
				else dv = newer.DownloadableVersion;
				if (dv > DownloadableVersion) DownloadableVersion = dv;
				if (newer.Version > Version) {
					homeUrl = newer.homeUrl;
					sourceUrl = newer.sourceUrl;
					updateUrl = newer.updateUrl;
					downloadUrl = newer.downloadUrl;
					directDownloadUrl = newer.directDownloadUrl;
					installerArgs = newer.installerArgs;
					netFramework = newer.netFramework;
					netFrameworkDownloadUrl = newer.netFrameworkDownloadUrl;
					netFrameworkDirectDownloadUrl = newer.netFrameworkDirectDownloadUrl;
					netFrameworkInstallerArgs = newer.netFrameworkInstallerArgs;
					netFrameworkUpdateUrl = newer.netFrameworkUpdateUrl;
				}
			}
		}

		public void CopyFrom(VersionInfo source) {
			Version = new Version("0.0.0.0");
			Update(source);
			//DownloadableVersion = source.DownloadableVersion;
		}

		public void Save(string filename) {
			try {
				using (FileStream file = new FileStream(filename, FileMode.Create)) {
					BinaryFormatter bf = new BinaryFormatter();
					bf.Serialize(file, this);
				}
			} catch { }
		}

		public void Load(string filename) {
			if (File.Exists(filename)) {
				try {
					using (FileStream file = new FileStream(filename, FileMode.Open)) {
						BinaryFormatter bf = new BinaryFormatter();
						var infofile = bf.Deserialize(file) as VersionInfo;
						CopyFrom(infofile);
					}
				} catch { }
			}
		}

		public void Load(XmlDocument xml) {
			try {
				XPathNavigator n = xml.CreateNavigator();
				// go to root element
				while (n.NodeType != XPathNodeType.Root && n.MoveToNext()) ;

				if (!n.MoveToFirstChild() && n.Name != "programm") throw new ArgumentException();
				
				if (n.HasAttributes) {
					n.MoveToFirstAttribute();
					do {
						if (n.Name == "version") {
							Version = new Version(n.Value);
							DownloadableVersion = new Version(n.Value);
						} else if (n.Name == "date") {
							DateTime date = DateTime.UtcNow;
							DateTime.TryParseExact(n.Value.Replace("UTC", ""), "s",
								CultureInfo.InvariantCulture.DateTimeFormat, DateTimeStyles.AssumeUniversal, out date);
							Date = date;
						}
					} while (n.MoveToNextAttribute());
					n.MoveToParent();
				}

				if (n.HasChildren) {
					n.MoveToFirstChild();
					do {
						switch (n.Name) {
							case"homeUrl": homeUrl = n.Value; break;
							case "sourceUrl": sourceUrl = n.Value; break;
							case "downloadUrl": downloadUrl = n.Value; break;
							case "directDownloadUrl": directDownloadUrl = n.Value; break;
							case "installerArgs": installerArgs = n.Value; break;
							case "updateUrl": updateUrl = n.Value; break;
							case "netFramework": netFramework = new Version(n.Value); break;
							case "netFrameworkDownloadUrl": netFrameworkDownloadUrl = n.Value; break;
							case "netFrameworkInstallerArgs": netFrameworkInstallerArgs = n.Value; break;
							case "netFrameworkDirectDownloadUrl": netFrameworkDirectDownloadUrl = n.Value; break;
							case "netFrameworkUpdateUrl": netFrameworkUpdateUrl = n.Value; break;
							default: break;
						}
					} while (n.MoveToNext());
					n.MoveToParent();
				}
			} catch { }
		}

	}
	
	public class UpdaterAttribute: Attribute {
		string appName = null;
		public string AppName {
			get {
				if (appName == null) {
					AssemblyProductAttribute product = Updater.AssemblyAttribute(typeof(AssemblyProductAttribute)) as AssemblyProductAttribute;
					appName = product.Product;
				}
				return appName;
			}
			set { appName = value; }
		}
			
		public string VersionUrl { get; set; }

		public Type UpdateForm { get; set; }

		public UpdaterAttribute() { AppName = null; VersionUrl = null; UpdateForm = null; }
	}

	public class Updater {

		public static int DelayDays = 14;

		public static Assembly EntryAssembly { get { return Assembly.GetEntryAssembly(); } }
		
		public static Attribute AssemblyAttribute(Type type) {
			object[] attributes = EntryAssembly.GetCustomAttributes(true);
			foreach (var a in attributes) {
				if (type.IsInstanceOfType(a)) return a as Attribute;
			}
			return null;
		}
	
		static UpdaterAttribute updaterAttribute = null;
		static UpdaterAttribute UpdaterAttribute {
			get {
				if (updaterAttribute == null) updaterAttribute =  AssemblyAttribute(typeof(UpdaterAttribute)) as UpdaterAttribute;
				if (updaterAttribute == null) updaterAttribute = new UpdaterAttribute();
				return updaterAttribute;
			}
		}

		public static string AppName { get { return UpdaterAttribute.AppName; } set { UpdaterAttribute.AppName = value; } }
		public static string VersionUrl { get { return UpdaterAttribute.VersionUrl; } set { UpdaterAttribute.VersionUrl = value; } }
		public static Version AssemblyVersion { get { return EntryAssembly.GetName().Version; } }

		private static string AppFolder(string path, bool create) {
			path = Path.Combine(path, AppName);
			var info = new DirectoryInfo(path);
			if (create && !info.Exists) {
				try {
					Directory.CreateDirectory(path);
				} catch {
				}
			}
			return path;
		}

		private static string AppFolder(string path) { return AppFolder(path, true); }

		private static string AppDataFolder { get { return AppFolder(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)); } }
		private static string AppTempFolder { get { return AppFolder(Path.GetTempPath()); } }
		private static string AppTempPath { get { return AppFolder(Path.GetTempPath(), false); } }

		private static string ConfigFile { get { return Path.Combine(AppDataFolder, AppName + ".Version.bin"); } }

		private static VersionInfo installedVersion = null;

		public static VersionInfo InstalledVersion {
			get {
				if (installedVersion != null) return installedVersion;

				VersionInfo info = new VersionInfo();
				info.Load(ConfigFile);
				info.netFramework = System.Environment.Version;
				if (Assembly.GetEntryAssembly() != Assembly.GetExecutingAssembly()) info.Version = AssemblyVersion;
				info.Save(ConfigFile);

				return installedVersion = info;
			}
			set {
				installedVersion = value;
				installedVersion.Date = DateTime.UtcNow;
				installedVersion.Save(ConfigFile);
			}
		}

		public static void Save() {
			InstalledVersion = InstalledVersion;
		}

		public static bool Enabled {
			get { return InstalledVersion.AutoUpdate; }
			set {
				InstalledVersion.AutoUpdate = value;
				Save();
			}
		}

		static void TraceMsg(string msg, params object[] p) {
#if DEBUG
			Trace.TraceInformation(msg, p);
#endif
		}

		static void TraceMsg(string msg) {
#if DEBUG
			Trace.TraceInformation(msg);
#endif
		}

		static void GetDownloadableVersion() {
			VersionInfo info = new VersionInfo();
			// Create a request for the URL.
			using (var web = new WebClient()) {
				try {
					web.Encoding = Encoding.UTF8;
					TraceMsg("Start Version Download: {0}", VersionUrl);
					var t0 = DateTime.Now;
					string xml = web.DownloadString(VersionUrl);
					TraceMsg("End Version Download: {0} seconds", (DateTime.Now - t0).TotalSeconds);
					var doc = new XmlDocument();
					doc.LoadXml(xml);
					info.Load(doc);
				} catch { }
			}
			downloadableVersion = info;

			InstalledVersion.Update(info);
			Save();
		}

		static VersionInfo downloadableVersion = null;

		public static VersionInfo DownloadableVersion {
			get {
				if (downloadableVersion != null) return downloadableVersion;

				
				if (Enabled && (DateTime.UtcNow - InstalledVersion.Date).TotalDays > DelayDays) {
					GetDownloadableVersion();
				} else {
					VersionInfo info = new VersionInfo();
					info.Version = InstalledVersion.Version; 
					downloadableVersion = info;
				}
				return downloadableVersion;
			}
		}

		public static bool Check() {
			/* bool res = InstalledVersion.Version < DownloadableVersion.Version;
			TraceMsg("Check: {0}", res);
			return res; */
			return false;
		}

		public static bool ForceCheck() {
			GetDownloadableVersion();
			return Check();
		}

		public static void StartUpdate() {
			if (Check()) {
				TraceMsg("StartUpdate");
				string assemblyName = Assembly.GetExecutingAssembly().ManifestModule.FullyQualifiedName;
				string updater = Path.Combine(AppTempFolder, Path.GetFileName(assemblyName));
				try {
					File.Copy(assemblyName, updater);
					TraceMsg("StartUpdater");
					Process.Start(updater, "\"" + AppName + "\"");
				} catch { }
				Application.Exit();
			}
		}

		static WebClient web;
		static string SetupExe;
		static DownloadForm downloadForm;

		public static void Update() {
			string url = InstalledVersion.directDownloadVersionedUrl;
			string exe = AppName + " Setup.exe";
			string name = AppName + " Setup";
			SetupExe = Path.Combine(AppTempFolder, exe);
			web = new WebClient();
			downloadForm = new DownloadForm(name, web);
			downloadForm.cancelButton.Enabled = false;
			//Application.DoEvents();
			ThreadPool.QueueUserWorkItem(delegate(object state) {
				web.DownloadProgressChanged += downloadForm.Progress;
				web.DownloadFileCompleted += Install;
				web.DownloadFileAsync(new Uri(url), SetupExe);
				downloadForm.Invoke(new MethodInvoker(delegate() { downloadForm.cancelButton.Enabled = true; }));
			});
			downloadForm.Show();
			downloadForm.BringToFront();
		}

		public static void Update(string appName) {
			AppName = appName;
			Update();
		}

		private static void Install(object sender, AsyncCompletedEventArgs e) {
			downloadForm.Invoke(new MethodInvoker(delegate() { downloadForm.Hide(); }));
			if (e.Error != null) {
				MessageBox.Show(
					string.Format("There was an error downloading the update.\nPlease update the software manually.\nError: {0}", e.Error.Message),
					"Error", MessageBoxButtons.OK);
			} else if (!e.Cancelled) Process.Start(SetupExe, DownloadableVersion.installerArgs);
			Application.Exit();
		}

		public static void Cleanup() {
			try {
				var info = new DirectoryInfo(AppTempPath);
				if (info.Exists) info.Delete(true);
			} catch (Exception ex) {
				TraceMsg("Error deleting temp dir: {0}", ex.Message);			
			}
		}

		static Form updateForm = null;
		public static Form UpdateForm {
			get {
				if (updateForm == null && UpdaterAttribute.UpdateForm != null) {
					updateForm = Activator.CreateInstance(UpdaterAttribute.UpdateForm) as Form;
				}
				return updateForm;	
			}
			set { updateForm = value; }
		}

		private static void ShowForm(object sender, EventArgs e) {
			TraceMsg("ShowForm");
			if (UpdateForm == null) UpdateForm = new UpdateForm();
			UpdateForm.Show();
			UpdateForm.BringToFront();
			TraceMsg("Form showed");
		}

		public static void ShowForm() { ShowForm(null, EventArgs.Empty); }

		private static void Dialog() {
			Cleanup();
			var form = Form.ActiveForm;
			if (Check() && form != null) {
				TraceMsg("Invoke ShowForm");
				form.Invoke(new EventHandler(ShowForm), form, EventArgs.Empty);
			}
		}

		private static void Dialog(object state) {
			try {
				Dialog();
			} catch (Exception ex){
				TraceMsg("Exception: {0}\n{1}", ex.Message, ex.StackTrace);
			}
		}

		public static void Start() {
			ThreadPool.QueueUserWorkItem(new WaitCallback(Dialog), null);
		}

	}
}
