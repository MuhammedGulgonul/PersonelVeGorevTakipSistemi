using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using PersonelVeGorevTakipSistemi.DataAccess;
using PersonelVeGorevTakipSistemi.Business.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Veritabanı bağlantı ayarlarını yapılandırıyoruz
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Çerez tabanlı kimlik doğrulama ayarlarını yapılandırıyoruz
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/AccessDenied";
    });

// İş mantığı servislerimizi sisteme kaydediyoruz
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<DepartmentService>();
builder.Services.AddScoped<EmployeeService>();
builder.Services.AddScoped<TaskService>();
builder.Services.AddScoped<FileService>();
builder.Services.AddScoped<ReportService>();
builder.Services.AddScoped<AnnouncementService>();

var app = builder.Build();

// Veritabanındaki yeni tabloların (Announcements, TaskComments vb.) otomatik açılmasını sağlıyoruz
using (var scope = app.Services.CreateScope())
{
    try
    {
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        db.Database.ExecuteSqlRaw(@"
            IF OBJECT_ID(N'[Announcements]') IS NULL
            BEGIN
                CREATE TABLE [Announcements] (
                    [Id] int NOT NULL IDENTITY,
                    [Title] nvarchar(max) NOT NULL,
                    [Content] nvarchar(max) NOT NULL,
                    [CreatedDate] datetime2 NOT NULL,
                    [IsActive] bit NOT NULL,
                    [EmployeeId] int NOT NULL,
                    CONSTRAINT [PK_Announcements] PRIMARY KEY ([Id]),
                    CONSTRAINT [FK_Announcements_Employees_EmployeeId] FOREIGN KEY ([EmployeeId]) REFERENCES [Employees] ([Id]) ON DELETE CASCADE
                );
            END;

            IF OBJECT_ID(N'[TaskComments]') IS NULL
            BEGIN
                CREATE TABLE [TaskComments] (
                    [Id] int NOT NULL IDENTITY,
                    [TaskId] int NOT NULL,
                    [EmployeeId] int NOT NULL,
                    [CommentText] nvarchar(max) NOT NULL,
                    [CreatedDate] datetime2 NOT NULL,
                    CONSTRAINT [PK_TaskComments] PRIMARY KEY ([Id]),
                    CONSTRAINT [FK_TaskComments_Tasks_TaskId] FOREIGN KEY ([TaskId]) REFERENCES [Tasks] ([Id]) ON DELETE CASCADE,
                    CONSTRAINT [FK_TaskComments_Employees_EmployeeId] FOREIGN KEY ([EmployeeId]) REFERENCES [Employees] ([Id]) ON DELETE NO ACTION
                );
            END;

            IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[Tasks]') AND name = N'Visibility')
            BEGIN
                ALTER TABLE [Tasks] ADD [Visibility] int NOT NULL DEFAULT 0;
            END;
        ");
    }
    catch (System.Exception)
    {
        // Otomatik tablo oluşturma hatası olursa yoksay
    }
}

app.UseMiddleware<PersonelVeGorevTakipSistemi.WebUI.Middleware.ExceptionHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Kullanıcının kim olduğunu doğrular
app.UseAuthorization();  // Kullanıcının yetkisini kontrol eder

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
