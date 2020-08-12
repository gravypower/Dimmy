using System.Security.Cryptography.X509Certificates;

namespace Dimmy.Engine.Services
{
    public interface ICertificateService
    {
        X509Certificate2 BuildCertificate(string certificateName, string dnsName);
        string CreateKey (X509Certificate2 cert);
        string CreateCertificate(X509Certificate2 cert);
    }
}