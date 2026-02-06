using lab4.Models;
using Lab4.Data;
using Lab4.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;

    // ========== 2FA ==========
    // ❌ TẮT AUTHENTICATOR APP
    options.Tokens.AuthenticatorTokenProvider = null;

    // ✅ CHỈ DÙNG EMAIL OTP
    options.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;

    options.Lockout.AllowedForNewUsers = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders(); // ⚠️ BẮT BUỘC cho Email OTP


    // ================= GOOGLE LOGIN =================
builder.Services.AddAuthentication()
    .AddGoogle(options =>
    {
        options.ClientId = "960632346424-lanmhm44ij4ioce1bt51jcnokmd75ov4.apps.googleusercontent.com";
        options.ClientSecret = "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";
    });

// ================= EMAIL =================
builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings"));

builder.Services.AddTransient<IEmailSender, EmailSender>();

// ================= VNPAY =================
builder.Services.Configure<Lab4.Models.VnPayConfig>(
    builder.Configuration.GetSection("VnPay"));
builder.Services.AddScoped<Lab4.Services.IVnPayService, Lab4.Services.VnPayService>();

// Cấu hình Claims-Based Authorization [1], [4]
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly",
        p => p.RequireClaim("Permission", "Admin.Access"));

    options.AddPolicy("ManageProduct",
        p => p.RequireClaim("Permission", "Product.Create"));

    options.AddPolicy("ManageInventory",
        p => p.RequireClaim("Permission", "Inventory.Manage"));

    options.AddPolicy("ManageOrder",
        p => p.RequireClaim("Permission", "Order.Manage"));
});
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(2);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    // 🔥 BẮT BUỘC để Vue (5173) nói chuyện với MVC (7045)
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowVue", policy =>
    {
        policy
            .WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
// Khởi tạo Scope để lấy UserManager và chạy Seeding Data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    
    // Gọi hàm Initialize đã tạo ở Bước 1
    await DbInitializer.Initialize(userManager);
}
app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("AllowVue");
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();
app.UseSwagger();
app.UseSwaggerUI();
app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Welcome}/{id?}")
    .WithStaticAssets();

app.MapRazorPages()
   .WithStaticAssets();

app.Run();
