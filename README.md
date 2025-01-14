# MiliNeu.in

## Instructions to Launch:

### 1. Configure `AppSettings.json`:

#### Add your connection string:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=Milineu;Trusted_Connection=True;MultipleActiveResultSets=true"
}
```
#### Add your RazorPay Api key and secret to allow payments and WebhookSecret to allow Order paymet status updates:
#### Webhook URL: YourUrl//webhook/razorpay
```json
   "RazorPay": {
   "Key": "Enter Your RazorPay Api Key",
   "Secret": "Enter Your RazorPay Secret",
   "WebhookSecret": "Enter Your RazorPay Webhook Secret"
```
#### Add your SendGrid Api details and Key to allow email alerts:
```json
  "SendGridSettings": {
    "FromEmail": "{Enter Your Business Email}",
    "SenderName": "MiliNeu Studio",
    "ApiKey": "{Enter Your Sendgrid Api Key}"
  },
```
### 2. open Package Mangement Console, and run command "Update-Database" to create and seed database.

### 3. Launch MiliNeu.
