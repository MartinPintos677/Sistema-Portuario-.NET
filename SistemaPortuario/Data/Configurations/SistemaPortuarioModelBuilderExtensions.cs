using Microsoft.EntityFrameworkCore;
using SistemaPortuario.Models;

namespace SistemaPortuario.Data.Configurations;

/// <summary>
/// Configuracion central de Entity Framework Core para el modelo relacional.
/// Mantiene claves, indices, relaciones, restricciones y catálogos fuera del DbContext.
/// </summary>
public static class SistemaPortuarioModelBuilderExtensions
{
    /// <summary>
    /// Aplica toda la configuracion del dominio en un unico punto de entrada.
    /// </summary>
    public static void ConfigureSistemaPortuarioModel(this ModelBuilder modelBuilder)
    {
        ConfigureEmpresa(modelBuilder);
        ConfigureRol(modelBuilder);
        ConfigureUsuario(modelBuilder);
        ConfigureCliente(modelBuilder);
        ConfigureMaquinaria(modelBuilder);
        ConfigureOrdenesServicio(modelBuilder);
        ConfigureMantenimiento(modelBuilder);
        ConfigureAdministracion(modelBuilder);
        ConfigureEstiba(modelBuilder);
        ConfigureNotificacion(modelBuilder);
        ConfigureTrazabilidad(modelBuilder);
        ConfigureRefreshToken(modelBuilder);
        SeedCatalogos(modelBuilder);
    }

    private static void ConfigureEmpresa(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Empresa>(entity =>
        {
            entity.ToTable("Empresa");
            entity.HasKey(e => e.IdEmpresa);
            entity.Property(e => e.Nombre).HasMaxLength(120).IsRequired();
            entity.Property(e => e.RazonSocial).HasMaxLength(160).IsRequired();
            entity.Property(e => e.Rut).HasMaxLength(30).IsRequired();
            entity.Property(e => e.TipoEmpresa).HasMaxLength(80).IsRequired();
            entity.Property(e => e.Activa).HasDefaultValue(true);
            entity.Property(e => e.FechaCreacion).HasDefaultValueSql("SYSUTCDATETIME()");
            entity.HasIndex(e => e.Rut).IsUnique();
        });
    }

    private static void ConfigureRol(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Rol>(entity =>
        {
            entity.ToTable("Rol");
            entity.HasKey(e => e.IdRol);
            entity.Property(e => e.Nombre).HasMaxLength(60).IsRequired();
            entity.HasIndex(e => e.Nombre).IsUnique();
        });
    }

    private static void ConfigureUsuario(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.ToTable("Usuario");
            entity.HasKey(e => e.IdUsuario);
            entity.Property(e => e.Cedula).HasMaxLength(30).IsRequired();
            entity.Property(e => e.Nombre).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Apellido).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Correo).HasMaxLength(180).IsRequired();
            entity.Property(e => e.Telefono).HasMaxLength(50);
            entity.Property(e => e.PasswordHash).HasMaxLength(500).IsRequired();
            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.FechaCreacion).HasDefaultValueSql("SYSUTCDATETIME()");
            entity.HasIndex(e => e.Cedula).IsUnique();
            entity.HasIndex(e => e.Correo).IsUnique();
            entity.HasIndex(e => e.IdEmpresa);

            entity.HasOne(e => e.Empresa)
                .WithMany(e => e.Usuarios)
                .HasForeignKey(e => e.IdEmpresa)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Rol)
                .WithMany(e => e.Usuarios)
                .HasForeignKey(e => e.IdRol)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private static void ConfigureCliente(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.ToTable("Cliente");
            entity.HasKey(e => e.IdCliente);
            entity.Property(e => e.RazonSocial).HasMaxLength(160).IsRequired();
            entity.Property(e => e.Rut).HasMaxLength(30).IsRequired();
            entity.Property(e => e.NombreContacto).HasMaxLength(140);
            entity.Property(e => e.Correo).HasMaxLength(180);
            entity.Property(e => e.Telefono).HasMaxLength(50);
            entity.Property(e => e.Direccion).HasMaxLength(250);
            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.FechaCreacion).HasDefaultValueSql("SYSUTCDATETIME()");
            entity.HasIndex(e => e.Rut).IsUnique();

            entity.HasOne(e => e.Empresa)
                .WithMany(e => e.Clientes)
                .HasForeignKey(e => e.IdEmpresa)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private static void ConfigureMaquinaria(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TipoMaquinaria>(entity =>
        {
            entity.ToTable("TipoMaquinaria");
            entity.HasKey(e => e.IdTipoMaquinaria);
            entity.Property(e => e.Nombre).HasMaxLength(80).IsRequired();
            entity.HasIndex(e => e.Nombre).IsUnique();
        });

        modelBuilder.Entity<Maquinaria>(entity =>
        {
            entity.ToTable("Maquinaria", table =>
                table.HasCheckConstraint("CK_Maquinaria_HorasAcumuladas", "[HorasAcumuladas] >= 0"));
            entity.HasKey(e => e.IdMaquinaria);
            entity.Property(e => e.Codigo).HasMaxLength(60).IsRequired();
            entity.Property(e => e.Nombre).HasMaxLength(120).IsRequired();
            entity.Property(e => e.Marca).HasMaxLength(100);
            entity.Property(e => e.Modelo).HasMaxLength(100);
            entity.Property(e => e.Matricula).HasMaxLength(80);
            entity.Property(e => e.HorasAcumuladas).HasPrecision(12, 2).HasDefaultValue(0);
            entity.Property(e => e.Activa).HasDefaultValue(true);
            entity.Property(e => e.FechaCreacion).HasDefaultValueSql("SYSUTCDATETIME()");
            entity.HasIndex(e => e.Codigo).IsUnique();

            entity.HasOne(e => e.Empresa)
                .WithMany(e => e.Maquinarias)
                .HasForeignKey(e => e.IdEmpresa)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.TipoMaquinaria)
                .WithMany(e => e.Maquinarias)
                .HasForeignKey(e => e.IdTipoMaquinaria)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<RegistroHorasMaquinaria>(entity =>
        {
            entity.ToTable("RegistroHorasMaquinaria", table =>
                table.HasCheckConstraint("CK_RegistroHorasMaquinaria_Horas", "[HorasTrabajadas] > 0"));
            entity.HasKey(e => e.IdRegistroHoras);
            entity.Property(e => e.HorasTrabajadas).HasPrecision(10, 2);
            entity.Property(e => e.Observacion).HasMaxLength(500);
            entity.Property(e => e.FechaCreacion).HasDefaultValueSql("SYSUTCDATETIME()");
            entity.HasIndex(e => new { e.IdMaquinaria, e.Fecha });
            entity.HasOne(e => e.Maquinaria)
                .WithMany(e => e.RegistrosHoras)
                .HasForeignKey(e => e.IdMaquinaria)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.OrdenServicio)
                .WithMany(e => e.RegistrosHorasMaquinaria)
                .HasForeignKey(e => e.IdOrdenServicio)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private static void ConfigureOrdenesServicio(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EstadoOrdenServicio>(entity =>
        {
            entity.ToTable("EstadoOrdenServicio");
            entity.HasKey(e => e.IdEstadoOrden);
            entity.Property(e => e.Nombre).HasMaxLength(80).IsRequired();
            entity.Property(e => e.Descripcion).HasMaxLength(250);
            entity.HasIndex(e => e.Nombre).IsUnique();
        });

        modelBuilder.Entity<OrdenServicio>(entity =>
        {
            entity.ToTable("OrdenServicio", table =>
                table.HasCheckConstraint(
                    "CK_OrdenServicio_Horas",
                    "[HoraFinalizacion] IS NULL OR [HoraInicioReal] IS NULL OR [HoraFinalizacion] >= [HoraInicioReal]"));
            entity.HasKey(e => e.IdOrdenServicio);
            entity.Property(e => e.FechaSolicitud).HasDefaultValueSql("SYSUTCDATETIME()");
            entity.Property(e => e.LugarServicio).HasMaxLength(250).IsRequired();
            entity.Property(e => e.TrabajoARealizar).HasMaxLength(1000).IsRequired();
            entity.Property(e => e.Observaciones).HasMaxLength(1000);
            entity.Property(e => e.RequiereFirmaCliente).HasDefaultValue(false);
            entity.Property(e => e.EnviadaCliente).HasDefaultValue(false);
            entity.Property(e => e.PrecargadaGSoft).HasDefaultValue(false);
            entity.Property(e => e.FechaCreacion).HasDefaultValueSql("SYSUTCDATETIME()");
            entity.HasIndex(e => e.FechaSolicitud);
            entity.HasIndex(e => e.IdCliente);
            entity.HasIndex(e => e.IdEncargado);
            entity.HasIndex(e => e.IdOperario);
            entity.HasIndex(e => e.IdEstadoOrden);

            entity.HasOne(e => e.Empresa)
                .WithMany(e => e.OrdenesServicio)
                .HasForeignKey(e => e.IdEmpresa)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Cliente)
                .WithMany(e => e.OrdenesServicio)
                .HasForeignKey(e => e.IdCliente)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Encargado)
                .WithMany(e => e.OrdenesComoEncargado)
                .HasForeignKey(e => e.IdEncargado)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Operario)
                .WithMany(e => e.OrdenesComoOperario)
                .HasForeignKey(e => e.IdOperario)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.MaquinariaAsignada)
                .WithMany(e => e.OrdenesAsignadas)
                .HasForeignKey(e => e.IdMaquinariaAsignada)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.MaquinariaFacturada)
                .WithMany(e => e.OrdenesFacturadas)
                .HasForeignKey(e => e.IdMaquinariaFacturada)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.EstadoOrden)
                .WithMany(e => e.OrdenesServicio)
                .HasForeignKey(e => e.IdEstadoOrden)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<DocumentoPdf>(entity =>
        {
            entity.ToTable("DocumentoPdf");
            entity.HasKey(e => e.IdDocumento);
            entity.Property(e => e.NombreArchivo).HasMaxLength(260).IsRequired();
            entity.Property(e => e.RutaArchivo).HasMaxLength(500).IsRequired();
            entity.Property(e => e.FechaGeneracion).HasDefaultValueSql("SYSUTCDATETIME()");
            entity.HasIndex(e => e.IdOrdenServicio).IsUnique();

            entity.HasOne(e => e.OrdenServicio)
                .WithOne(e => e.DocumentoPdf)
                .HasForeignKey<DocumentoPdf>(e => e.IdOrdenServicio)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Facturacion>(entity =>
        {
            entity.ToTable("Facturacion");
            entity.HasKey(e => e.IdFacturacion);
            entity.Property(e => e.Estado).HasMaxLength(80).IsRequired();
            entity.Property(e => e.ReferenciaGSoft).HasMaxLength(120);
            entity.Property(e => e.Observaciones).HasMaxLength(1000);
            entity.HasIndex(e => e.IdOrdenServicio).IsUnique();

            entity.HasOne(e => e.OrdenServicio)
                .WithOne(e => e.Facturacion)
                .HasForeignKey<Facturacion>(e => e.IdOrdenServicio)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private static void ConfigureMantenimiento(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TipoMantenimiento>(entity =>
        {
            entity.ToTable("TipoMantenimiento", table =>
                table.HasCheckConstraint("CK_TipoMantenimiento_UmbralHoras", "[UmbralHoras] IS NULL OR [UmbralHoras] > 0"));
            entity.HasKey(e => e.IdTipoMantenimiento);
            entity.Property(e => e.Nombre).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Descripcion).HasMaxLength(500);
            entity.Property(e => e.UmbralHoras).HasPrecision(10, 2);
            entity.HasIndex(e => e.Nombre).IsUnique();
        });

        modelBuilder.Entity<EstadoMantenimiento>(entity =>
        {
            entity.ToTable("EstadoMantenimiento");
            entity.HasKey(e => e.IdEstadoMantenimiento);
            entity.Property(e => e.Nombre).HasMaxLength(80).IsRequired();
            entity.HasIndex(e => e.Nombre).IsUnique();
        });

        modelBuilder.Entity<Mantenimiento>(entity =>
        {
            entity.ToTable("Mantenimiento", table =>
                table.HasCheckConstraint("CK_Mantenimiento_Horas", "[HorasMaquinaAlMomento] IS NULL OR [HorasMaquinaAlMomento] >= 0"));
            entity.HasKey(e => e.IdMantenimiento);
            entity.Property(e => e.Descripcion).HasMaxLength(1000).IsRequired();
            entity.Property(e => e.HorasMaquinaAlMomento).HasPrecision(12, 2);
            entity.Property(e => e.Observaciones).HasMaxLength(1000);
            entity.Property(e => e.FechaCreacion).HasDefaultValueSql("SYSUTCDATETIME()");
            entity.HasIndex(e => e.IdMaquinaria);
            entity.HasOne(e => e.Maquinaria)
                .WithMany(e => e.Mantenimientos)
                .HasForeignKey(e => e.IdMaquinaria)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.TipoMantenimiento)
                .WithMany(e => e.Mantenimientos)
                .HasForeignKey(e => e.IdTipoMantenimiento)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.EstadoMantenimiento)
                .WithMany(e => e.Mantenimientos)
                .HasForeignKey(e => e.IdEstadoMantenimiento)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Responsable)
                .WithMany(e => e.MantenimientosResponsable)
                .HasForeignKey(e => e.IdResponsable)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.RegistroHorasOrigen)
                .WithMany(e => e.MantenimientosOriginados)
                .HasForeignKey(e => e.IdRegistroHorasOrigen)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private static void ConfigureAdministracion(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EstadoTarea>(entity =>
        {
            entity.ToTable("EstadoTarea");
            entity.HasKey(e => e.IdEstadoTarea);
            entity.Property(e => e.Nombre).HasMaxLength(80).IsRequired();
            entity.HasIndex(e => e.Nombre).IsUnique();
        });

        modelBuilder.Entity<TareaAdministrativa>(entity =>
        {
            entity.ToTable("TareaAdministrativa", table =>
                table.HasCheckConstraint("CK_TareaAdministrativa_Prioridad", "[Prioridad] IN ('Baja', 'Media', 'Alta', 'Urgente')"));
            entity.HasKey(e => e.IdTarea);
            entity.Property(e => e.Titulo).HasMaxLength(160).IsRequired();
            entity.Property(e => e.Descripcion).HasMaxLength(1000);
            entity.Property(e => e.FechaCreacion).HasDefaultValueSql("SYSUTCDATETIME()");
            entity.Property(e => e.Prioridad).HasMaxLength(30).HasDefaultValue("Media");
            entity.HasIndex(e => new { e.IdAsignado, e.IdEstadoTarea });
            entity.HasOne(e => e.Creador)
                .WithMany(e => e.TareasCreadas)
                .HasForeignKey(e => e.IdCreador)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Asignado)
                .WithMany(e => e.TareasAsignadas)
                .HasForeignKey(e => e.IdAsignado)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.EstadoTarea)
                .WithMany(e => e.Tareas)
                .HasForeignKey(e => e.IdEstadoTarea)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<EventoCalendario>(entity =>
        {
            entity.ToTable("EventoCalendario", table =>
                table.HasCheckConstraint("CK_EventoCalendario_Fechas", "[FechaFin] IS NULL OR [FechaFin] >= [FechaInicio]"));
            entity.HasKey(e => e.IdEvento);
            entity.Property(e => e.Titulo).HasMaxLength(160).IsRequired();
            entity.Property(e => e.Descripcion).HasMaxLength(1000);
            entity.Property(e => e.TipoEvento).HasMaxLength(80).IsRequired();
            entity.HasIndex(e => new { e.FechaInicio, e.FechaFin });
            entity.HasOne(e => e.Creador)
                .WithMany(e => e.EventosCreados)
                .HasForeignKey(e => e.IdCreador)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Tarea)
                .WithMany(e => e.Eventos)
                .HasForeignKey(e => e.IdTarea)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private static void ConfigureEstiba(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PersonalEstiba>(entity =>
        {
            entity.ToTable("PersonalEstiba");
            entity.HasKey(e => e.IdPersonalEstiba);
            entity.Property(e => e.Cedula).HasMaxLength(30).IsRequired();
            entity.Property(e => e.Nombre).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Apellido).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Telefono).HasMaxLength(50);
            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.FechaCreacion).HasDefaultValueSql("SYSUTCDATETIME()");
            entity.HasIndex(e => e.IdEmpresa);
            entity.HasIndex(e => e.Cedula).IsUnique();

            entity.HasOne(e => e.Empresa)
                .WithMany(e => e.PersonalEstiba)
                .HasForeignKey(e => e.IdEmpresa)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Cuadrilla>(entity =>
        {
            entity.ToTable("Cuadrilla");
            entity.HasKey(e => e.IdCuadrilla);
            entity.Property(e => e.Nombre).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Descripcion).HasMaxLength(500);
            entity.Property(e => e.Activa).HasDefaultValue(true);
            entity.HasIndex(e => e.IdEmpresa);
            entity.HasIndex(e => e.Nombre).IsUnique();

            entity.HasOne(e => e.Empresa)
                .WithMany(e => e.Cuadrillas)
                .HasForeignKey(e => e.IdEmpresa)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<CuadrillaPersonal>(entity =>
        {
            entity.ToTable("CuadrillaPersonal", table =>
                table.HasCheckConstraint("CK_CuadrillaPersonal_Fechas", "[FechaHasta] IS NULL OR [FechaHasta] >= [FechaDesde]"));
            entity.HasKey(e => e.IdCuadrillaPersonal);

            entity.HasOne(e => e.Cuadrilla)
                .WithMany(e => e.Personal)
                .HasForeignKey(e => e.IdCuadrilla)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.PersonalEstiba)
                .WithMany(e => e.CuadrillasPersonal)
                .HasForeignKey(e => e.IdPersonalEstiba)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<EstadoCitacion>(entity =>
        {
            entity.ToTable("EstadoCitacion");
            entity.HasKey(e => e.IdEstadoCitacion);
            entity.Property(e => e.Nombre).HasMaxLength(80).IsRequired();
            entity.HasIndex(e => e.Nombre).IsUnique();
        });

        modelBuilder.Entity<CitacionEstiba>(entity =>
        {
            entity.ToTable("CitacionEstiba");
            entity.HasKey(e => e.IdCitacion);
            entity.Property(e => e.Zona).HasMaxLength(120).IsRequired();
            entity.Property(e => e.DetalleOperativo).HasMaxLength(1000);
            entity.Property(e => e.FechaCreacion).HasDefaultValueSql("SYSUTCDATETIME()");
            entity.HasIndex(e => e.IdEmpresa);
            entity.HasIndex(e => e.Fecha);

            entity.HasOne(e => e.Empresa)
                .WithMany(e => e.CitacionesEstiba)
                .HasForeignKey(e => e.IdEmpresa)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Cliente)
                .WithMany(e => e.CitacionesEstiba)
                .HasForeignKey(e => e.IdCliente)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.EstadoCitacion)
                .WithMany(e => e.Citaciones)
                .HasForeignKey(e => e.IdEstadoCitacion)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<LiquidacionEstiba>(entity =>
        {
            entity.ToTable("LiquidacionEstiba", table =>
            {
                table.HasCheckConstraint("CK_LiquidacionEstiba_Periodo", "[PeriodoHasta] >= [PeriodoDesde]");
                table.HasCheckConstraint("CK_LiquidacionEstiba_TotalHoras", "[TotalHoras] >= 0");
            });
            entity.HasKey(e => e.IdLiquidacion);
            entity.Property(e => e.TotalHoras).HasPrecision(12, 2).HasDefaultValue(0);
            entity.Property(e => e.Estado).HasMaxLength(80).IsRequired();
            entity.Property(e => e.FechaGeneracion).HasDefaultValueSql("SYSUTCDATETIME()");
            entity.Property(e => e.Observaciones).HasMaxLength(1000);
            entity.HasIndex(e => e.IdEmpresa);

            entity.HasOne(e => e.Empresa)
                .WithMany(e => e.LiquidacionesEstiba)
                .HasForeignKey(e => e.IdEmpresa)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<DetalleCitacionEstiba>(entity =>
        {
            entity.ToTable("DetalleCitacionEstiba", table =>
                table.HasCheckConstraint("CK_DetalleCitacionEstiba_Horas", "[HorasTrabajadas] IS NULL OR [HorasTrabajadas] >= 0"));
            entity.HasKey(e => e.IdDetalleCitacion);
            entity.Property(e => e.HorasTrabajadas).HasPrecision(10, 2);
            entity.Property(e => e.EstadoAltaBps).HasMaxLength(80);
            entity.Property(e => e.Observaciones).HasMaxLength(1000);
            entity.HasIndex(e => e.IdPersonalEstiba);
            entity.HasIndex(e => new { e.IdCitacion, e.IdPersonalEstiba }).IsUnique();
            entity.HasOne(e => e.Citacion)
                .WithMany(e => e.Detalles)
                .HasForeignKey(e => e.IdCitacion)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.PersonalEstiba)
                .WithMany(e => e.DetallesCitacion)
                .HasForeignKey(e => e.IdPersonalEstiba)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Cuadrilla)
                .WithMany(e => e.DetallesCitacion)
                .HasForeignKey(e => e.IdCuadrilla)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Liquidacion)
                .WithMany(e => e.DetallesCitacion)
                .HasForeignKey(e => e.IdLiquidacion)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private static void ConfigureNotificacion(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Notificacion>(entity =>
        {
            entity.ToTable("Notificacion", table =>
            {
                table.HasCheckConstraint("CK_Notificacion_Tipo", "[Tipo] IN ('Email', 'WhatsApp', 'SMS', 'Sistema')");
                table.HasCheckConstraint("CK_Notificacion_Origen", "[IdUsuario] IS NOT NULL OR [IdOrdenServicio] IS NOT NULL OR [IdCitacion] IS NOT NULL");
            });
            entity.HasKey(e => e.IdNotificacion);
            entity.Property(e => e.Tipo).HasMaxLength(40).IsRequired();
            entity.Property(e => e.Destinatario).HasMaxLength(180).IsRequired();
            entity.Property(e => e.Mensaje).HasMaxLength(1000).IsRequired();
            entity.Property(e => e.Estado).HasMaxLength(80).IsRequired();
            entity.Property(e => e.FechaCreacion).HasDefaultValueSql("SYSUTCDATETIME()");
            entity.HasOne(e => e.Usuario)
                .WithMany(e => e.Notificaciones)
                .HasForeignKey(e => e.IdUsuario)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.OrdenServicio)
                .WithMany(e => e.Notificaciones)
                .HasForeignKey(e => e.IdOrdenServicio)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Citacion)
                .WithMany(e => e.Notificaciones)
                .HasForeignKey(e => e.IdCitacion)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private static void ConfigureTrazabilidad(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Trazabilidad>(entity =>
        {
            entity.ToTable("Trazabilidad");
            entity.HasKey(e => e.IdTrazabilidad);
            entity.Property(e => e.Fecha).HasDefaultValueSql("SYSUTCDATETIME()");
            entity.Property(e => e.Accion).HasMaxLength(80).IsRequired();
            entity.Property(e => e.Entidad).HasMaxLength(120).IsRequired();
            entity.Property(e => e.IdRegistroAfectado).HasMaxLength(80).IsRequired();
            entity.HasIndex(e => new { e.Entidad, e.IdRegistroAfectado });
            entity.HasIndex(e => new { e.IdUsuario, e.Fecha });

            entity.HasOne(e => e.Usuario)
                .WithMany(e => e.Trazabilidades)
                .HasForeignKey(e => e.IdUsuario)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private static void ConfigureRefreshToken(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.ToTable("RefreshToken");
            entity.HasKey(e => e.IdRefreshToken);
            entity.Property(e => e.TokenHash).HasMaxLength(128).IsRequired();
            entity.Property(e => e.FechaCreacion).HasDefaultValueSql("SYSUTCDATETIME()");
            entity.HasIndex(e => e.TokenHash).IsUnique();
            entity.HasIndex(e => new { e.IdUsuario, e.Expira });

            entity.HasOne(e => e.Usuario)
                .WithMany(e => e.RefreshTokens)
                .HasForeignKey(e => e.IdUsuario)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private static void SeedCatalogos(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Rol>().HasData(
            new Rol { IdRol = 1, Nombre = "Administrador" },
            new Rol { IdRol = 2, Nombre = "Encargado" },
            new Rol { IdRol = 3, Nombre = "Operario" },
            new Rol { IdRol = 4, Nombre = "Oficina" });

        modelBuilder.Entity<EstadoOrdenServicio>().HasData(
            new EstadoOrdenServicio { IdEstadoOrden = 1, Nombre = "Solicitada", Descripcion = "Orden creada y pendiente de asignaciÃ³n o inicio." },
            new EstadoOrdenServicio { IdEstadoOrden = 2, Nombre = "Asignada", Descripcion = "Orden asignada a operario y maquinaria." },
            new EstadoOrdenServicio { IdEstadoOrden = 3, Nombre = "EnProceso", Descripcion = "Servicio en ejecuciÃ³n." },
            new EstadoOrdenServicio { IdEstadoOrden = 4, Nombre = "PendienteValidacion", Descripcion = "Finalizada por operario y pendiente de validaciÃ³n." },
            new EstadoOrdenServicio { IdEstadoOrden = 5, Nombre = "Validada", Descripcion = "Validada por encargado." },
            new EstadoOrdenServicio { IdEstadoOrden = 6, Nombre = "EnviadaCliente", Descripcion = "PDF enviado al cliente." },
            new EstadoOrdenServicio { IdEstadoOrden = 7, Nombre = "Facturada", Descripcion = "Orden enviada o registrada en facturaciÃ³n." },
            new EstadoOrdenServicio { IdEstadoOrden = 8, Nombre = "Cancelada", Descripcion = "Orden cancelada." });

        modelBuilder.Entity<EstadoMantenimiento>().HasData(
            new EstadoMantenimiento { IdEstadoMantenimiento = 1, Nombre = "Pendiente" },
            new EstadoMantenimiento { IdEstadoMantenimiento = 2, Nombre = "Programado" },
            new EstadoMantenimiento { IdEstadoMantenimiento = 3, Nombre = "EnProceso" },
            new EstadoMantenimiento { IdEstadoMantenimiento = 4, Nombre = "Realizado" },
            new EstadoMantenimiento { IdEstadoMantenimiento = 5, Nombre = "Cancelado" });

        modelBuilder.Entity<EstadoTarea>().HasData(
            new EstadoTarea { IdEstadoTarea = 1, Nombre = "Sin comenzar" },
            new EstadoTarea { IdEstadoTarea = 2, Nombre = "En proceso" },
            new EstadoTarea { IdEstadoTarea = 3, Nombre = "Listo" },
            new EstadoTarea { IdEstadoTarea = 4, Nombre = "Cancelada" });

        modelBuilder.Entity<EstadoCitacion>().HasData(
            new EstadoCitacion { IdEstadoCitacion = 1, Nombre = "Pendiente" },
            new EstadoCitacion { IdEstadoCitacion = 2, Nombre = "Enviada" },
            new EstadoCitacion { IdEstadoCitacion = 3, Nombre = "Confirmada" },
            new EstadoCitacion { IdEstadoCitacion = 4, Nombre = "Completada" },
            new EstadoCitacion { IdEstadoCitacion = 5, Nombre = "Cancelada" });
    }
}


