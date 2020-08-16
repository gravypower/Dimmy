using System.Security.Cryptography.X509Certificates;

namespace Dimmy.Engine.Services
{
    public interface ICertificateService
    {
        X509Certificate2 CreateSelfSignedCertificate(string certificateName, string dnsName);
        X509Certificate2 CreateSignedCertificate(string certificateName, string dnsName, X509Certificate2 issuerCertificate);
        string CreateKey (X509Certificate2 cert);
        string CreateCertificate(X509Certificate2 cert);
    }
}