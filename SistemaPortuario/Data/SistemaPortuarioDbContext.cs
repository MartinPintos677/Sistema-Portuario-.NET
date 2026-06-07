using System.Security.Claims;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SistemaPortuario.Data.Configurations;
using SistemaPortuario.Models;

namespace SistemaPortuario.Data;

/// <summary>
/// DbContext principal del sistema portuario.
/// Expone las tablas del dominio y centraliza la auditoría de cambios.
/// </summary>
public class SistemaPortuarioDbContext(
    DbContextOptions<SistemaPortuarioDbContext> options,
    IHttpContextAccessor httpContextAccessor) : DbContext(options)
{
    private static readonly JsonSerializerOptions AuditJsonOptions = new(JsonSerializerDefaults.Web);
    private static readonly HashSet<string> ExcludedAuditEntities = new(StringComparer.OrdinalIgnoreCase)
    {
        nameof(RefreshToken)
    };
    private static readonly HashSet<string> SensitiveProperties = new(StringComparer.OrdinalIgnoreCase)
    {
        "PasswordHash"
    };

    private bool _savingAudit;

    // DbSets del dominio. Se organizan por módulo funcional del sistema.
    public DbSet<Empresa> Empresas => Set<Empresa>();
    public DbSet<Rol> Roles => Set<Rol>();
    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Cliente> Clientes => Set<Cliente>();
    public DbSet<TipoMaquinaria> TiposMaquinaria => Set<TipoMaquinaria>();
    public DbSet<Maquinaria> Maquinarias => Set<Maquinaria>();
    public DbSet<RegistroHorasMaquinaria> RegistrosHorasMaquinaria => Set<RegistroHorasMaquinaria>();
    public DbSet<EstadoOrdenServicio> EstadosOrdenServicio => Set<EstadoOrdenServicio>();
    public DbSet<OrdenServicio> OrdenesServicio => Set<OrdenServicio>();
    public DbSet<DocumentoPdf> DocumentosPdf => Set<DocumentoPdf>();
    public DbSet<Facturacion> Facturaciones => Set<Facturacion>();
    public DbSet<TipoMantenimiento> TiposMantenimiento => Set<TipoMantenimiento>();
    public DbSet<EstadoMantenimiento> EstadosMantenimiento => Set<EstadoMantenimiento>();
    public DbSet<Mantenimiento> Mantenimientos => Set<Mantenimiento>();
    public DbSet<EstadoTarea> EstadosTarea => Set<EstadoTarea>();
    public DbSet<TareaAdministrativa> TareasAdministrativas => Set<TareaAdministrativa>();
    public DbSet<EventoCalendario> EventosCalendario => Set<EventoCalendario>();
    public DbSet<PersonalEstiba> PersonalEstiba => Set<PersonalEstiba>();
    public DbSet<Cuadrilla> Cuadrillas => Set<Cuadrilla>();
    public DbSet<CuadrillaPersonal> CuadrillasPersonal => Set<CuadrillaPersonal>();
    public DbSet<EstadoCitacion> EstadosCitacion => Set<EstadoCitacion>();
    public DbSet<CitacionEstiba> CitacionesEstiba => Set<CitacionEstiba>();
    public DbSet<LiquidacionEstiba> LiquidacionesEstiba => Set<LiquidacionEstiba>();
    public DbSet<DetalleCitacionEstiba> DetallesCitacionEstiba => Set<DetalleCitacionEstiba>();
    public DbSet<Notificacion> Notificaciones => Set<Notificacion>();
    public DbSet<Trazabilidad> Trazabilidades => Set<Trazabilidad>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ConfigureSistemaPortuarioModel();
    }

    /// <summary>
    /// Guarda cambios y, si existe usuario autenticado, registra trazabilidad.
    /// </summary>
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        if (_savingAudit)
        {
            return await base.SaveChangesAsync(cancellationToken);
        }

        var auditEntries = CreateAuditEntries();
        var result = await base.SaveChangesAsync(cancellationToken);

        if (auditEntries.Count > 0)
        {
            _savingAudit = true;
            try
            {
                Trazabilidades.AddRange(auditEntries.Select(entry => entry.ToTrazabilidad()));
                await base.SaveChangesAsync(cancellationToken);
            }
            finally
            {
                _savingAudit = false;
            }
        }

        return result;
    }

    private List<AuditEntry> CreateAuditEntries()
    {
        var idUsuario = GetCurrentUserId();
        if (idUsuario is null)
        {
            return [];
        }

        ChangeTracker.DetectChanges();

        return ChangeTracker.Entries()
            .Where(entry =>
                entry.Entity is not Trazabilidad &&
                !ExcludedAuditEntities.Contains(entry.Metadata.ClrType.Name) &&
                entry.State is EntityState.Added or EntityState.Modified or EntityState.Deleted &&
                HasAuditableChanges(entry))
            .Select(entry => AuditEntry.From(entry, idUsuario.Value))
            .ToList();
    }

    // La auditoría toma el usuario desde el claim del JWT emitido por AuthService.
    private int? GetCurrentUserId()
    {
        var value = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        return int.TryParse(value, out var idUsuario) ? idUsuario : null;
    }

    private static bool HasAuditableChanges(EntityEntry entry) =>
        entry.State switch
        {
            EntityState.Added or EntityState.Deleted => true,
            EntityState.Modified => entry.Properties.Any(property => property.IsModified && !property.Metadata.IsPrimaryKey()),
            _ => false
        };

    /// <summary>
    /// Representa un cambio pendiente antes de persistirlo como Trazabilidad.
    /// </summary>
    private sealed class AuditEntry
    {
        private readonly EntityEntry _entry;

        private AuditEntry(EntityEntry entry, int idUsuario)
        {
            _entry = entry;
            IdUsuario = idUsuario;
            Accion = entry.State switch
            {
                EntityState.Added => "Crear",
                EntityState.Modified => "Actualizar",
                EntityState.Deleted => "Eliminar",
                _ => entry.State.ToString()
            };
            Entidad = entry.Metadata.ClrType.Name;
            DatosPrevios = entry.State is EntityState.Modified or EntityState.Deleted
                ? SerializeValues(entry, useOriginalValues: true)
                : null;
        }

        private int IdUsuario { get; }
        private string Accion { get; }
        private string Entidad { get; }
        private string? DatosPrevios { get; }

        public static AuditEntry From(EntityEntry entry, int idUsuario) => new(entry, idUsuario);

        public Trazabilidad ToTrazabilidad()
        {
            return new Trazabilidad
            {
                IdUsuario = IdUsuario,
                Fecha = DateTime.UtcNow,
                Accion = Accion,
                Entidad = Entidad,
                IdRegistroAfectado = GetPrimaryKeyValue(_entry),
                DatosPrevios = DatosPrevios,
                DatosNuevos = _entry.State is EntityState.Deleted
                    ? null
                    : SerializeValues(_entry, useOriginalValues: false)
            };
        }

        private static string GetPrimaryKeyValue(EntityEntry entry)
        {
            var key = entry.Metadata.FindPrimaryKey();
            if (key is null)
            {
                return string.Empty;
            }

            var values = key.Properties
                .Select(property => entry.Property(property.Name).CurrentValue?.ToString() ?? string.Empty);

            return string.Join("|", values);
        }

        private static string SerializeValues(EntityEntry entry, bool useOriginalValues)
        {
            var values = new Dictionary<string, object?>();

            foreach (var property in entry.Properties)
            {
                var propertyName = property.Metadata.Name;

                if (SensitiveProperties.Contains(propertyName))
                {
                    if (entry.State == EntityState.Modified && property.IsModified)
                    {
                        values["Password"] = useOriginalValues ? "Contraseña" : "Nueva contraseña";
                    }

                    continue;
                }

                if (!useOriginalValues && property.Metadata.IsPrimaryKey() && property.IsTemporary)
                {
                    continue;
                }

                values[propertyName] = useOriginalValues ? property.OriginalValue : property.CurrentValue;
            }

            return JsonSerializer.Serialize(values, AuditJsonOptions);
        }
    }
}

