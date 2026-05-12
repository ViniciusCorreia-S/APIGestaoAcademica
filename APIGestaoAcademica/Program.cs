using System.Text;
using GestaoAcademica.Data;
using GestaoAcademica.Repositories;
using GestaoAcademica.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Configuracao da string de conexao para MySQL, obtida do appsettings.json
var connectionString = builder.Configuration.GetConnectionString("ConexaoPadrao")
    ?? throw new InvalidOperationException("ConnectionString ConexaoPadrao nao configurada.");

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configuracao do Swagger para documentacao da API
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "API Sistema de Gestao Academica",
        Version = "v1",
        Description = "API REST academica com CRUD, EF Core, JWT, autorizacao e interface Bootstrap."
    });

    // Configuracao de seguranca para JWT no Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Informe apenas o token JWT gerado no login."
    });

    // Requer autenticacao para acessar os endpoints no Swagger
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[]{}
        }
    });
});

// Configuracao do DbContext para MySQL usando Pomelo.EntityFrameworkCore.MySql
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 36))));

// Registrando os repositórios e serviços para injeção de dependência
builder.Services.AddScoped<IAlunoRepository, AlunoRepository>();
builder.Services.AddScoped<ICursoRepository, CursoRepository>();
builder.Services.AddScoped<IDisciplinaRepository, DisciplinaRepository>();
builder.Services.AddScoped<IMatriculaRepository, MatriculaRepository>();
builder.Services.AddScoped<IAlunoService, AlunoService>();
builder.Services.AddScoped<ICursoService, CursoService>();
builder.Services.AddScoped<IDisciplinaService, DisciplinaService>();
builder.Services.AddScoped<IMatriculaService, MatriculaService>();

var jwtKey = builder.Configuration["Jwt:Key"]
    ?? throw new InvalidOperationException("Jwt:Key nao configurada.");
var key = Encoding.ASCII.GetBytes(jwtKey);

// Configuracao da autenticacao JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidateLifetime = true,
            //ClockSkew = TimeSpan.Zero
        };
    });

// Configuracao de autorizacao com base em roles
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("SomenteAdministrador", policy => policy.RequireRole("Administrador"));
    options.AddPolicy("EquipeAcademica", policy => policy.RequireRole("Administrador", "Secretaria"));
});

// Configuracao de CORS para permitir acesso da interface web
builder.Services.AddCors(options =>
{
    options.AddPolicy("InterfaceWeb", policy =>
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Habilitando a interface web em wwwroot (index.html, app.js, styles.css)
app.UseDefaultFiles();
app.UseStaticFiles();

// Habilitando CORS para a interface web
app.UseCors("InterfaceWeb");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
