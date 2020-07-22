using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;


namespace Docker.TesterWebSite.Controllers
{
    [ApiController]
    [Route("api/diagnostics")]
    public class DiagnosticsController : ControllerBase
    {
        private readonly IOptionsSnapshot<AppSettings> _options;

        public DiagnosticsController(IOptionsSnapshot<AppSettings> options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));

            _options = options;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var result = new
            {
                Environment.HasShutdownStarted,
                Environment.MachineName,
                Environment.ExitCode,
                Environment.CurrentManagedThreadId,
                Environment.CurrentDirectory,
                Environment.CommandLine,
                Environment.NewLine,
                Environment.OSVersion,
                Environment.ProcessorCount,
                Environment.Is64BitOperatingSystem,
                Environment.StackTrace,
                Environment.SystemPageSize,
                Environment.TickCount,
                Environment.TickCount64,
                Environment.UserDomainName,
                Environment.UserInteractive,
                Environment.UserName,
                Environment.Version,
                Environment.WorkingSet,
                Environment.SystemDirectory,
                Environment.Is64BitProcess,
                GetCommandLineArgs = Environment.GetCommandLineArgs(),
                GetEnvironmentVariables = Environment.GetEnvironmentVariables(),
                GetLogicalDrives = Environment.GetLogicalDrives()
            };

            return Ok(result);
        }

        [HttpGet]
        [Route("getAppDataFileList")]
        public IActionResult GetAppDataFileList()
        {
            var path = _options.Value.DataPath;

            var result = new
            {
                DataPath = path,
                Files = Directory.Exists(path) ? Directory.GetFiles(path) : new[] { "Does not exits" }
            };

            return Ok(result);
        }


        [HttpGet]
        [Route("getNetworkInfo")]
        public IActionResult GetNetworkInfo()
        {
            var ipV4s = NetworkInterface.GetAllNetworkInterfaces()
                        .Select(i => i.GetIPProperties().UnicastAddresses)
                        .SelectMany(u => u)
                        .Select(i => i.Address.ToString());

                return Ok(ipV4s);
        }

        [HttpGet]
        [Route("writeTestFile")]
        public IActionResult WriteTestFile()
        {
            var path = Path.Combine(_options.Value.DataPath, $"{nameof(WriteTestFile)}.txt");

            System.IO.File.WriteAllText(path, DateTime.Now.ToString());

            return Ok(path);
        }

        [HttpGet]
        [Route("listCertificates")]
        public IActionResult ListCertificates()
        {
            var items = new List<dynamic>();
            var store = new X509Store(StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadOnly);
            var certificates = store.Certificates;
            foreach (var certificate in certificates)
            {
                items.Add(new
                {
                    certificate.Subject,
                    certificate.Issuer,
                    certificate.IssuerName,
                    certificate.FriendlyName,
                    certificate.HasPrivateKey
                });
            }

            return Ok(items);
        }

        [HttpGet]
        [Route("createPfx")]
        public IActionResult CreatePfx()
        {
            var ba = Utils.ReadResource("X509Sample.pfx.txt");
            var pwd = "Pass@word1";

            using (var certificate = new X509Certificate2(ba, pwd, X509KeyStorageFlags.Exportable | X509KeyStorageFlags.PersistKeySet))
            using (var store = new X509Store(StoreLocation.CurrentUser))
            {
                store.Open(OpenFlags.ReadWrite);
                store.Add(certificate);
                store.Close();
            }

            return Ok();
        }
    }
}