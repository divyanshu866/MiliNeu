# 🛍️ Milineu — Custom Fashion E-Commerce Platform

Milineu is a full-stack e-commerce platform built using **ASP.NET Core MVC** and **Entity Framework Core (PostgreSQL)**.  
It powers a real clothing brand (MiliNeu Studio), providing customers with a fast, secure, and scalable online shopping experience.

---

## 🚀 Features

### 👩‍💻 Customer Experience
- Browse collections & products with optimized WebP images (using ImageSharp).
- Secure checkout flow with **Razorpay payment integration**.
- Order tracking by status (Placed → Dispatched → Delivered → Refunded).
- Customer account management (JWT-based authentication).

### 🧑‍💼 Admin Dashboard
- Add, edit, and delete products with image uploads (auto-compressed & optimized).
- Manage hero section banners, CTA links, and collection content.
- Manage orders and update statuses (Delivered, Refunded, etc.).
- Filter and search orders by status, date, or customer.
- Email notifications using **SendGrid** (order confirmations, refunds, etc.).

### ⚙️ Backend / Technical
- **ASP.NET Core MVC 8** with **Entity Framework Core (PostgreSQL)**.
- Database migrations, seeding, and entity relationships.
- **Middleware**, **Filters**, and **Dependency Injection**.
- **JWT Authentication** and **Role-based Authorization**.
- **ImageSharp** for image compression & WebP generation.
- Structured logging and error handling.
- Modular service architecture similar to repository pattern.
- Ready for containerization (Docker) and CI/CD pipelines.

---

## 🧱 Tech Stack

| Layer | Technology |
|-------|-------------|
| Frontend | ASP.NET Razor Views, CSS3 |
| Backend | ASP.NET Core MVC 8, C# |
| Database | PostgreSQL via Entity Framework Core |
| Authentication | JWT + ASP.NET Identity |
| Payment Gateway | Razorpay |
| Email Service | SendGrid |
| Image Processing | SixLabors.ImageSharp |
| Hosting | Azure App Service |

---




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
