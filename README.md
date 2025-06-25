{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "DefaultConnection": "Server=yourServerSQLName;Database=CarManagement;User Id=sa;Password=12345;Encrypt=False;TrustServerCertificate=True;"
  },
  "Jwt": {
    "Key": "ThisIsASecureKeyWithExactly32Characters",
    "Issuer": "YourIssuer",
    "Audience": "YourAudience"
  }
}
