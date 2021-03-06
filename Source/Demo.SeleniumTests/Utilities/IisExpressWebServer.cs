﻿//code from http://net.tutsplus.com/tutorials/maintainable-automated-ui-tests/
//excellent article as well regarding UI Testing

using System;
using System.Diagnostics;



namespace Demo.SeleniumTests.Utilities
{
    internal class IisExpressWebServer
    {
        private static WebApplication _application;
        private static Process _webHostProcess;

        public IisExpressWebServer(WebApplication application)
        {
            _application = application;
        }

        public void Start()
        {
            var webHostStartInfo = ProcessStartInfo(_application.Location.FullPath, _application.PortNumber);
            _webHostProcess = Process.Start(webHostStartInfo);
        }

        public void Stop()
        {
            if (_webHostProcess == null)
                return;
            if (!_webHostProcess.HasExited)
                _webHostProcess.Kill();
            _webHostProcess.Dispose();
        }

        public string BaseUrl
        {
            get { return string.Format("http://localhost:{0}", _application.PortNumber); }
        }

        private static ProcessStartInfo ProcessStartInfo(string applicationPath, int port)
        {
            // todo: grab stdout and/or stderr for logging purposes?
            var key = Environment.Is64BitOperatingSystem ? "programfiles(x86)" : "programfiles";
            var programfiles = Environment.GetEnvironmentVariable(key);

            return new ProcessStartInfo
                {
                    WindowStyle = ProcessWindowStyle.Normal,
                    ErrorDialog = true,
                    LoadUserProfile = true,
                    CreateNoWindow = false,
                    UseShellExecute = false,
                    Arguments = String.Format("/path:\"{0}\" /port:{1}", applicationPath, port),
                    FileName = string.Format("{0}\\IIS Express\\iisexpress.exe", programfiles)
                };
        }
    }
}