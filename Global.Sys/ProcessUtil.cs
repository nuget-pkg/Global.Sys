using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Global {
    public static class ProcessUtil {
        public static bool StartHiddenWindowProcess(
            string exePath,
            string[] args,
            Dictionary<string, string>? vars = null
            ) {
            string processArgumentsString = "";
            for (int i = 0; i < args.Length; i++) {
                if (i > 0) {
                    processArgumentsString += " ";
                }
                processArgumentsString = (!args[i].Contains(" ")) ? (processArgumentsString + args[i]) : (processArgumentsString + "\"" + args[i] + "\"");
            }
            Process process = new Process();
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.FileName = exePath;
            process.StartInfo.Arguments = processArgumentsString;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            if (vars != null) {
                foreach (string key in vars.Keys) {
                    process.StartInfo.EnvironmentVariables[key] = vars[key];
                }
            }
            return process.Start();
        }

        public static void AssociateProgramToFileExtension(
            string programPath,
            string extension, /* with dot, e.g. ".txt" */
            string productName,
            string description) {
#if NETFRAMEWORK
            if (Environment.OSVersion.Platform != PlatformID.Win32NT)
#else
            if (!OperatingSystem.IsWindows())
#endif
            {
                return;
            }
            string commandline = "\"" + programPath + "\" \"%1\"";
            string fileType = productName + ".0";
            string verb = "open";
            string verbDescription = "Open with " + productName + "(&O)";
            string iconPath = programPath;
            int iconIndex = 0;
            Microsoft.Win32.RegistryKey currentUserKey = Microsoft.Win32.Registry.CurrentUser;
            Microsoft.Win32.RegistryKey regkey = currentUserKey.CreateSubKey("Software\\Classes\\" + extension);
            regkey.SetValue("", fileType);
            regkey.Close();
            Microsoft.Win32.RegistryKey typekey = currentUserKey.CreateSubKey("Software\\Classes\\" + fileType);
            typekey.SetValue("", description);
            typekey.Close();
            Microsoft.Win32.RegistryKey verblkey = currentUserKey.CreateSubKey("Software\\Classes\\" + fileType + "\\shell\\" + verb);
            verblkey.SetValue("", verbDescription);
            verblkey.Close();
            Microsoft.Win32.RegistryKey cmdkey = currentUserKey.CreateSubKey("Software\\Classes\\" + fileType + "\\shell\\" + verb + "\\command");
            cmdkey.SetValue("", commandline);
            cmdkey.Close();
            Microsoft.Win32.RegistryKey iconkey = currentUserKey.CreateSubKey("Software\\Classes\\" + fileType + "\\DefaultIcon");
            iconkey.SetValue("", iconPath + "," + iconIndex.ToString());
            iconkey.Close();
        }
    }
}
