# MiliNeu

<p>This is an e-commerce website in initial phase of developement for a clothing brand Milineu Studio.</p>

<p>Add your connection string in the "appsettings.json" file and Run Command "update-database" in the package manager console of Visual studio to create the database and run the website 
<br>
<br>
Admin Login Credentials:
<br>

  <h4> Email: admin@admin <h4>
  <h4> Password: Admin123$ <h4>
<br>
    <br>
Added product and collection image/info upload/update/delete functionality.

Ability to add and manage new collections and products when admin account holder logs in.

Implemented IEmailSender interface for email confirmation using SendGrid service when registering. Can be enable in program.cs and add the SendGrit credentials and 

Api keys in appsettings.json:
<br>
<br>
  "SendGridSettings": {
    "FromEmail": "email@email.com",
    "EmailName": "emailname",
    "ApiKey": "SendGridAPIKey"
  },

</p>
