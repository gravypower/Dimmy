using System.Security.Cryptography.X509Certificates;

namespace Dimmy.Engine.Services
{
    public interface ICertificateService
    {
        X509Certificate2 CreateCaCertificate(string certificateName, string dnsName);
        X509Certificate2 CreateSignedCertificate(string certificateName, string dnsName, X509Certificate2 issuerCertificate);
        X509Certificate2 CreateSelfSignedCertificate(string certificateName, string dnsName);
        string CreateKeyString (X509Certificate2 cert);
        string CreateCertificateString(X509Certificate2 cert);
    }
}