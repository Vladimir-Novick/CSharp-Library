
using System;
using System.Diagnostics;
using System.Text;
using System.Security;
using System.Collections.Generic;

namespace SGcombo.SysUtils
{

    ////////////////////////////////////////////////////////////////////////////
    //	Copyright 2013 - 2017 : Vladimir Novick    https://www.linkedin.com/in/vladimirnovick/  
    //        
    //             https://github.com/Vladimir-Novick/CSharp-Library
    //
    //    NO WARRANTIES ARE EXTENDED. USE AT YOUR OWN RISK. 
    //
    // To contact the author with suggestions or comments, use  :vlad.novick@gmail.com
    //
    ////////////////////////////////////////////////////////////////////////////

    /// <summary>
    /// Run Console application in the background
    /// </summary>

    public class TaskAdapter
    {

        public List<EnvironmentVariable> EnvironmentVariables = new List<EnvironmentVariable>();

        public string WorkingDirectory { get; set; }

        private static SecureString MakeSecureString(string text)
        {
            SecureString secure = new SecureString();
            foreach (char c in text)
            {
                secure.AppendChar(c);
            }

            return secure;
        }

        public String username = null;
        public String password = null;
        public String domain = null;

        private StringBuilder strSummaryOutputString = new StringBuilder();


        public string RunProcess(string EXEFileName, string CommandLineParams, int Timeout)
        {

            try
            {

                Process RunningProcess = new Process();

                RunningProcess.StartInfo.UseShellExecute = false;

                ProcessStartInfo StartInfo = new ProcessStartInfo();

                StartInfo.UseShellExecute = false;

                StartInfo.UserName = username;
                StartInfo.Password = MakeSecureString(password);
                StartInfo.Domain = domain;

                foreach (EnvironmentVariable variable in EnvironmentVariables)
                {
                    StartInfo.EnvironmentVariables[variable.Name] = variable.Value;
                }

                StartInfo.WorkingDirectory = WorkingDirectory;

                StartInfo.FileName = EXEFileName;
                StartInfo.Arguments = CommandLineParams;
                StartInfo.RedirectStandardOutput = true;
                StartInfo.RedirectStandardError = true;
                StartInfo.CreateNoWindow = true;
                StartInfo.ErrorDialog = false;

                StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

                RunningProcess.EnableRaisingEvents = true;
                RunningProcess.Exited += new EventHandler(myProcess_Exited);

                RunningProcess.ErrorDataReceived += proc_DataReceived;
                RunningProcess.OutputDataReceived += proc_DataReceived;

                RunningProcess.StartInfo = StartInfo;

                RunningProcess.Start();

                RunningProcess.BeginErrorReadLine();
                RunningProcess.BeginOutputReadLine();

                if (!RunningProcess.WaitForExit(Timeout))
                {

                    RunningProcess.Kill();
                    strSummaryOutputString.AppendLine(string.Format(@"Timeout of {0} ms while is executing ""{1} {2}""",
                     Timeout, RunningProcess.StartInfo.FileName, RunningProcess.StartInfo.Arguments));

                }

                int exitCode = RunningProcess.ExitCode;
            }
            catch (Exception ex)
            {

                strSummaryOutputString.AppendLine(ex.Message);

            }

            return strSummaryOutputString.ToString();

        }

        private void myProcess_Exited(object sender, System.EventArgs e)
        {

        }

        void proc_DataReceived(object sender, DataReceivedEventArgs e)
        {
            String strLine = e.Data as string;
            if (strLine != null)
            {

                Console.WriteLine(strLine);
                strSummaryOutputString.AppendLine(strLine);

            }
        }

    }

    public class EnvironmentVariable
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public EnvironmentVariable(String _Name, String _Value)
        {
            Name = _Name;
            Value = _Value;
        }

    }

}
