using System.Collections.Generic;

namespace DTO
{
    public class ConfigOption
    {
    }

    public class ConfigAuthen
    {
        public const string Authen = "Authen";
        public string TokenIssuer { get; set; }
        public string AudienceClientId { get; set; }
        public string AudienceBase64Secret { get; set; }

        public string Url { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ClientId { get; set; }
        public int TokenInterval { get; set; }

    }
    public class ConfigRabbitMQ
    {
        public const string RabbitMQ = "RabbitMQ";
        public string ConnectionString { get; set; }
        public string DefaultQueue { get; set; }
    }
    public class ConfigRedis
    {
        public const string Redis = "Redis";
        public string ConnectionMNP { get; set; }
        public string ConnectionDuplicateContent { get; set; }
        public string ConnectionDuplicateTrackingId { get; set; }
        public string ConnectionConfig { get; set; }
    }

    public class ConfigAuditLog
    {
        public const string AuditLog = "AuditLog";
        public bool Enable { get; set; }
        public string Collection { get; set; }
        public string ConnectionString { get; set; }
        public string Database { get; set; }
        public string[] SensitiveDataJson { get; set; }
        public string[] AllowHeader { get; set; }
        public List<string> ServiceUnAudit { get; set; }
    }
    public class IOMediaConfig
    {
        public const string Name = "IOMedia";
        public string Username { get; set; }
        public string PublicKey { get; set; }
        public string PrivateKey { get; set; }
        public string Url { get; set; }
    }

    public class FinvietConfig
    {
        public const string Name = "Finviet";
        public string Username { get; set; }
        public string Password { get; set; }
        public string SecretKey { get; set; }
        public string HashKey { get; set; }
        public string Url { get; set; }
    }
    public class IMediaConfig
    {
        public const string Name = "IMedia";
        public string Username { get; set; }
        public string Password { get; set; }
        public int RefreshIntervalInMinute { get; set; }
        public string Url { get; set; }
    }

    public class MobifoneConfig
    {
        public const string Name = "Mobifone";
        public string Username { get; set; }
        public string Password { get; set; }
        public int RefreshIntervalInMinute { get; set; }
        public string Url { get; set; }
    }

    public class MobifoneGlobalConfig
    {
        public const string Name = "MobifoneGlobal";
        public string Username { get; set; }
        public string Password { get; set; }
        public string SecretKey { get; set; }
        public string Url { get; set; }
    }

    public class VinaphoneConfig
    {
        public const string Name = "Vinaphone";
        public string AccountId { get; set; }
        public string PrivateKey { get; set; }
        public string Url { get; set; }
    }

    public class ReddiConfig
    {
        public const string Name = "Reddi";
        public string Username { get; set; }
        public string Password { get; set; }
        public int RefreshIntervalInMinute { get; set; }
        public string Version { get; set; }
        public string ShopId { get; set; }
        public string Checksum { get; set; }
        public string SecretKey { get; set; }
        public byte[] SecretKeyMD5 { get; set; }
        public long MerchantId { get; set; }
        public string Url { get; set; }
    }
    public class VietnamobileConfig
    {
        public const string Name = "Vietnamobile";
        public string Host { get; set; }
        public int Port { get; set; }
        public string GatewayUsername { get; set; }
        public string GatewayPassword { get; set; }
        public string GatewayToken { get; set; }
        public string EVoucherUsername { get; set; }
        public string EVoucherPassword { get; set; }
        public string EVoucherMsisdn { get; set; }

        public string EVoucherServiceCode { get; set; }
        public string EVoucherMPIN { get; set; }
        public int GatewayTokenData { get; set; }
        public int SocketAlive { get; set; }
        public int SocketMaxConnection { get; set; }
        public int SocketTimeout { get; set; }
    }
}
