using System.Security.Cryptography.X509Certificates;

namespace Dimmy.Engine.Services
{
    public interface ICertificateService
    {
        X509Certificate2 CreateCertificate(string certificateName, string dnsName);
    }
}