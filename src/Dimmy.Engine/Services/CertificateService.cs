using System;
using System.Net;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace Dimmy.Engine.Services
{
    public class CertificateService : ICertificateService
    {
        public X509Certificate2 CreateCertificate(string certificateName, string dnsName)
        {
            var ecdsa = ECDsa.Create(); // generate asymmetric key pair
            var req = new CertificateRequest($"cn={dnsName}", ecdsa, HashAlgorithmName.SHA256);
            var cert = req.CreateSelfSigned(DateTimeOffset.Now, DateTimeOffset.Now.AddYears(10));
        
            return cert;
        }
    }
}