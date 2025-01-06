# MiliNeu.in


Instructions to launch:

Inside AppSettings.json:
1: add your connection string:

  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=Milineu;Trusted_Connection=True;MultipleActiveResultSets=true",
    "ApplicationDbContextConnection": "Server=(localdb)\\mssqllocaldb;Database=Milineu;Trusted_Connection=True;MultipleActiveResultSets=true"
  },

2: Add your RazorPay Api key and secret to allow payments:

  "RazorPay": {
    "Key": "Enter Your RazorPay Api Key",
    "Secret": "Enter Your RazorPay Secret"
  },

3: Add your SendGrid Api details and Key to allow email alerts:

  "SendGridSettings": {
    "FromEmail": "{Enter Your Business Email}",
    "SenderName": "MiliNeu Studio",
    "ApiKey": "{Enter Your Sendgrid Api Key}"
  },

open Package Mangement Console, and run command "Update-Database" to create and seed database.

Launch MiliNeu.
