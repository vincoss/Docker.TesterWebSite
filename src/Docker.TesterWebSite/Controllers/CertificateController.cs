using Docker.TesterWebSite.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;


namespace Docker.TesterWebSite.Controllers
{
    [ApiController]
    [Route("api/certificate")]
    public class CertificateController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            var items = new List<dynamic>();
            var store = new X509Store(StoreName.Root, StoreLocation.LocalMachine);
            store.Open(OpenFlags.ReadOnly);
            var certificates = store.Certificates;
            foreach (var certificate in certificates)
            {
                items.Add(CertificateDto.Map(store, certificate));
            }

            if (items.Any())
            {
                return Ok(items);
            }

            return Ok(new
            {
                StoreName = store.Name,
                StoreLocation = store.Location,
            });
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

            return Ok("Success");
        }

        [HttpGet]
        [Route("findCertificate")]
        public IActionResult FindCertificate(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                id = "test";
            }

            var store = new X509Store(StoreName.Root, StoreLocation.LocalMachine);
            store.Open(OpenFlags.ReadOnly);
            X509Certificate2 cert = store.Certificates.OfType<X509Certificate2>().AsEnumerable().FirstOrDefault(c => c.Subject.Contains(id, StringComparison.OrdinalIgnoreCase));

            if (cert != null)
            {
                var dto = CertificateDto.Map(store, cert);
                return Ok(dto);
            }

            return Ok($"Not found: {id}");

        }
    }
}
