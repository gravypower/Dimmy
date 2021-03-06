using System;
using System.Net;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Dimmy.Engine.Services
{
    public class CertificateService : ICertificateService
    {
        public X509Certificate2 CreateCaCertificate(string certificateName, string dnsName)
        {
            var distinguishedName = new X500DistinguishedName($"CN={certificateName}");

            using var rsa = RSA.Create(2048);

            var request = new CertificateRequest(
                distinguishedName,
                rsa,
                HashAlgorithmName.SHA256,
                RSASignaturePadding.Pkcs1);

            request.CertificateExtensions.Add(
                new X509BasicConstraintsExtension(true, true, 12, true));

            request.CertificateExtensions.Add(
                new X509KeyUsageExtension(X509KeyUsageFlags.KeyCertSign, true));


            var sanBuilder = new SubjectAlternativeNameBuilder();
            sanBuilder.AddDnsName(dnsName);
            var sanExtension = sanBuilder.Build();
            request.CertificateExtensions.Add(sanExtension);

            request.CertificateExtensions.Add(
                new X509EnhancedKeyUsageExtension(
                    new OidCollection
                    {
                        new Oid("1.3.6.1.5.5.7.3.2"), // TLS Client auth
                        new Oid("1.3.6.1.5.5.7.3.1") // TLS Server auth
                    },
                    false));

            request.CertificateExtensions.Add(new X509SubjectKeyIdentifierExtension(request.PublicKey, false));

            var notBefore = new DateTimeOffset(DateTime.UtcNow.AddDays(-1));
            var notAfter = new DateTimeOffset(DateTime.UtcNow.AddYears(10));

            var certificate = request.CreateSelfSigned(notBefore, notAfter);
            return certificate;
        }

        public X509Certificate2 CreateSignedCertificate(string certificateName, string dnsName, X509Certificate2 issuerCertificate)
        {
            var sanBuilder = new SubjectAlternativeNameBuilder();
            sanBuilder.AddIpAddress(IPAddress.Loopback);
            sanBuilder.AddIpAddress(IPAddress.IPv6Loopback);
            sanBuilder.AddDnsName(dnsName);
            sanBuilder.AddDnsName(Environment.MachineName);

            var distinguishedName = new X500DistinguishedName($"CN={certificateName}");

            using var rsa =  RSA.Create(2048);
            var request = new CertificateRequest(
                distinguishedName,
                rsa,
                HashAlgorithmName.SHA256,
                RSASignaturePadding.Pkcs1);

            request.CertificateExtensions.Add(
                new X509KeyUsageExtension(
                    X509KeyUsageFlags.DataEncipherment | X509KeyUsageFlags.KeyEncipherment |
                    X509KeyUsageFlags.DigitalSignature, false));

            request.CertificateExtensions.Add(
                new X509EnhancedKeyUsageExtension(
                    new OidCollection {new Oid("1.3.6.1.5.5.7.3.1")}, false));

            request.CertificateExtensions.Add(sanBuilder.Build());

            var notBefore = issuerCertificate.NotBefore;
            var notAfter = issuerCertificate.NotAfter.AddDays(-1);

            // cert serial is the epoch/unix timestamp
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var unixTime = Convert.ToInt64((DateTime.UtcNow - epoch).TotalSeconds);
            var serial = BitConverter.GetBytes(unixTime);
            
            var certificate = request.Create(issuerCertificate, notBefore, notAfter, serial);

            return certificate.CopyWithPrivateKey(rsa);
        }

        public X509Certificate2 CreateSelfSignedCertificate(string certificateName, string dnsName)
        {
            var sanBuilder = new SubjectAlternativeNameBuilder();
            sanBuilder.AddIpAddress(IPAddress.Loopback);
            sanBuilder.AddIpAddress(IPAddress.IPv6Loopback);
            sanBuilder.AddDnsName(dnsName);
            sanBuilder.AddDnsName(Environment.MachineName);

            var distinguishedName = new X500DistinguishedName($"CN={certificateName}");

            using var rsa = RSA.Create(2048);
            
            var request = new CertificateRequest(
                distinguishedName, 
                rsa, 
                HashAlgorithmName.SHA256,
                RSASignaturePadding.Pkcs1);

            request.CertificateExtensions.Add(
                new X509KeyUsageExtension(
                    X509KeyUsageFlags.DataEncipherment | X509KeyUsageFlags.KeyEncipherment |
                    X509KeyUsageFlags.DigitalSignature, false));
            
            request.CertificateExtensions.Add(
                new X509EnhancedKeyUsageExtension(
                    new OidCollection {new Oid("1.3.6.1.5.5.7.3.1")}, false));

            request.CertificateExtensions.Add(sanBuilder.Build());

            var certificate = request.CreateSelfSigned(
                new DateTimeOffset(DateTime.UtcNow.AddDays(-1)),
                new DateTimeOffset(DateTime.UtcNow.AddYears(10)));
            certificate.FriendlyName = certificateName;

            return certificate;
        }

        public string CreateCertificateString(X509Certificate2 cert)
        {
            var certBytes = cert.Export(X509ContentType.Cert);
            var builder = new StringBuilder();
            builder.AppendLine("-----BEGIN CERTIFICATE-----");
            builder.AppendLine(Convert.ToBase64String(certBytes));
            builder.AppendLine("-----END CERTIFICATE-----");

            return builder.ToString();
        }

        public string CreateKeyString(X509Certificate2 cert)
        {
            var privateKeyBytes = cert.PrivateKey.ExportPkcs8PrivateKey();
            var builder = new StringBuilder();
            builder.AppendLine("-----BEGIN PRIVATE KEY-----");
            builder.AppendLine(Convert.ToBase64String(privateKeyBytes));
            builder.AppendLine("-----END PRIVATE KEY-----");

            return builder.ToString();
        }
    }
}