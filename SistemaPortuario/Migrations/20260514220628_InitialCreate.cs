using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SistemaPortuario.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cliente",
                columns: table => new
                {
                    IdCliente = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RazonSocial = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: false),
                    Rut = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    NombreContacto = table.Column<string>(type: "nvarchar(140)", maxLength: 140, nullable: true),
                    Correo = table.Column<string>(type: "nvarchar(180)", maxLength: 180, nullable: true),
                    Telefono = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Direccion = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Activo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cliente", x => x.IdCliente);
                });

            migrationBuilder.CreateTable(
                name: "Cuadrilla",
                columns: table => new
                {
                    IdCuadrilla = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Activa = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cuadrilla", x => x.IdCuadrilla);
                });

            migrationBuilder.CreateTable(
                name: "Empresa",
                columns: table => new
                {
                    IdEmpresa = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    RazonSocial = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: false),
                    Rut = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    TipoEmpresa = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    Activa = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empresa", x => x.IdEmpresa);
                });

            migrationBuilder.CreateTable(
                name: "EstadoCitacion",
                columns: table => new
                {
                    IdEstadoCitacion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstadoCitacion", x => x.IdEstadoCitacion);
                });

            migrationBuilder.CreateTable(
                name: "EstadoMantenimiento",
                columns: table => new
                {
                    IdEstadoMantenimiento = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstadoMantenimiento", x => x.IdEstadoMantenimiento);
                });

            migrationBuilder.CreateTable(
                name: "EstadoOrdenServicio",
                columns: table => new
                {
                    IdEstadoOrden = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstadoOrdenServicio", x => x.IdEstadoOrden);
                });

            migrationBuilder.CreateTable(
                name: "EstadoTarea",
                columns: table => new
                {
                    IdEstadoTarea = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstadoTarea", x => x.IdEstadoTarea);
                });

            migrationBuilder.CreateTable(
                name: "LiquidacionEstiba",
                columns: table => new
                {
                    IdLiquidacion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PeriodoDesde = table.Column<DateOnly>(type: "date", nullable: false),
                    PeriodoHasta = table.Column<DateOnly>(type: "date", nullable: false),
                    TotalHoras = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false, defaultValue: 0m),
                    Estado = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    FechaGeneracion = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    Observaciones = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LiquidacionEstiba", x => x.IdLiquidacion);
                    table.CheckConstraint("CK_LiquidacionEstiba_Periodo", "[PeriodoHasta] >= [PeriodoDesde]");
                    table.CheckConstraint("CK_LiquidacionEstiba_TotalHoras", "[TotalHoras] >= 0");
                });

            migrationBuilder.CreateTable(
                name: "PersonalEstiba",
                columns: table => new
                {
                    IdPersonalEstiba = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Cedula = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Apellido = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Activo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonalEstiba", x => x.IdPersonalEstiba);
                });

            migrationBuilder.CreateTable(
                name: "Rol",
                columns: table => new
                {
                    IdRol = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rol", x => x.IdRol);
                });

            migrationBuilder.CreateTable(
                name: "TipoMantenimiento",
                columns: table => new
                {
                    IdTipoMantenimiento = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    UmbralHoras = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoMantenimiento", x => x.IdTipoMantenimiento);
                    table.CheckConstraint("CK_TipoMantenimiento_UmbralHoras", "[UmbralHoras] IS NULL OR [UmbralHoras] > 0");
                });

            migrationBuilder.CreateTable(
                name: "TipoMaquinaria",
                columns: table => new
                {
                    IdTipoMaquinaria = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoMaquinaria", x => x.IdTipoMaquinaria);
                });

            migrationBuilder.CreateTable(
                name: "CitacionEstiba",
                columns: table => new
                {
                    IdCitacion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdCliente = table.Column<int>(type: "int", nullable: true),
                    IdEstadoCitacion = table.Column<int>(type: "int", nullable: false),
                    Fecha = table.Column<DateOnly>(type: "date", nullable: false),
                    Hora = table.Column<TimeOnly>(type: "time", nullable: false),
                    Zona = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    DetalleOperativo = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CitacionEstiba", x => x.IdCitacion);
                    table.ForeignKey(
                        name: "FK_CitacionEstiba_Cliente_IdCliente",
                        column: x => x.IdCliente,
                        principalTable: "Cliente",
                        principalColumn: "IdCliente",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CitacionEstiba_EstadoCitacion_IdEstadoCitacion",
                        column: x => x.IdEstadoCitacion,
                        principalTable: "EstadoCitacion",
                        principalColumn: "IdEstadoCitacion",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CuadrillaPersonal",
                columns: table => new
                {
                    IdCuadrillaPersonal = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdCuadrilla = table.Column<int>(type: "int", nullable: false),
                    IdPersonalEstiba = table.Column<int>(type: "int", nullable: false),
                    FechaDesde = table.Column<DateOnly>(type: "date", nullable: false),
                    FechaHasta = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CuadrillaPersonal", x => x.IdCuadrillaPersonal);
                    table.CheckConstraint("CK_CuadrillaPersonal_Fechas", "[FechaHasta] IS NULL OR [FechaHasta] >= [FechaDesde]");
                    table.ForeignKey(
                        name: "FK_CuadrillaPersonal_Cuadrilla_IdCuadrilla",
                        column: x => x.IdCuadrilla,
                        principalTable: "Cuadrilla",
                        principalColumn: "IdCuadrilla",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CuadrillaPersonal_PersonalEstiba_IdPersonalEstiba",
                        column: x => x.IdPersonalEstiba,
                        principalTable: "PersonalEstiba",
                        principalColumn: "IdPersonalEstiba",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    IdUsuario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdEmpresa = table.Column<int>(type: "int", nullable: false),
                    IdRol = table.Column<int>(type: "int", nullable: false),
                    Cedula = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Apellido = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Correo = table.Column<string>(type: "nvarchar(180)", maxLength: 180, nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PasswordHash = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.IdUsuario);
                    table.ForeignKey(
                        name: "FK_Usuario_Empresa_IdEmpresa",
                        column: x => x.IdEmpresa,
                        principalTable: "Empresa",
                        principalColumn: "IdEmpresa",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Usuario_Rol_IdRol",
                        column: x => x.IdRol,
                        principalTable: "Rol",
                        principalColumn: "IdRol",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Maquinaria",
                columns: table => new
                {
                    IdMaquinaria = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdEmpresa = table.Column<int>(type: "int", nullable: false),
                    IdTipoMaquinaria = table.Column<int>(type: "int", nullable: false),
                    Codigo = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    Marca = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Modelo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Matricula = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    HorasAcumuladas = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false, defaultValue: 0m),
                    Activa = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Maquinaria", x => x.IdMaquinaria);
                    table.CheckConstraint("CK_Maquinaria_HorasAcumuladas", "[HorasAcumuladas] >= 0");
                    table.ForeignKey(
                        name: "FK_Maquinaria_Empresa_IdEmpresa",
                        column: x => x.IdEmpresa,
                        principalTable: "Empresa",
                        principalColumn: "IdEmpresa",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Maquinaria_TipoMaquinaria_IdTipoMaquinaria",
                        column: x => x.IdTipoMaquinaria,
                        principalTable: "TipoMaquinaria",
                        principalColumn: "IdTipoMaquinaria",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DetalleCitacionEstiba",
                columns: table => new
                {
                    IdDetalleCitacion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdCitacion = table.Column<int>(type: "int", nullable: false),
                    IdPersonalEstiba = table.Column<int>(type: "int", nullable: false),
                    IdCuadrilla = table.Column<int>(type: "int", nullable: true),
                    IdLiquidacion = table.Column<int>(type: "int", nullable: true),
                    Asistencia = table.Column<bool>(type: "bit", nullable: true),
                    HoraInicioReal = table.Column<TimeOnly>(type: "time", nullable: true),
                    HoraFinReal = table.Column<TimeOnly>(type: "time", nullable: true),
                    HorasTrabajadas = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: true),
                    EstadoAltaBps = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    Observaciones = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetalleCitacionEstiba", x => x.IdDetalleCitacion);
                    table.CheckConstraint("CK_DetalleCitacionEstiba_Horas", "[HorasTrabajadas] IS NULL OR [HorasTrabajadas] >= 0");
                    table.ForeignKey(
                        name: "FK_DetalleCitacionEstiba_CitacionEstiba_IdCitacion",
                        column: x => x.IdCitacion,
                        principalTable: "CitacionEstiba",
                        principalColumn: "IdCitacion",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DetalleCitacionEstiba_Cuadrilla_IdCuadrilla",
                        column: x => x.IdCuadrilla,
                        principalTable: "Cuadrilla",
                        principalColumn: "IdCuadrilla",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DetalleCitacionEstiba_LiquidacionEstiba_IdLiquidacion",
                        column: x => x.IdLiquidacion,
                        principalTable: "LiquidacionEstiba",
                        principalColumn: "IdLiquidacion",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DetalleCitacionEstiba_PersonalEstiba_IdPersonalEstiba",
                        column: x => x.IdPersonalEstiba,
                        principalTable: "PersonalEstiba",
                        principalColumn: "IdPersonalEstiba",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TareaAdministrativa",
                columns: table => new
                {
                    IdTarea = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdCreador = table.Column<int>(type: "int", nullable: false),
                    IdAsignado = table.Column<int>(type: "int", nullable: false),
                    IdEstadoTarea = table.Column<int>(type: "int", nullable: false),
                    Titulo = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    FechaVencimiento = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Prioridad = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false, defaultValue: "Media")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TareaAdministrativa", x => x.IdTarea);
                    table.CheckConstraint("CK_TareaAdministrativa_Prioridad", "[Prioridad] IN ('Baja', 'Media', 'Alta', 'Urgente')");
                    table.ForeignKey(
                        name: "FK_TareaAdministrativa_EstadoTarea_IdEstadoTarea",
                        column: x => x.IdEstadoTarea,
                        principalTable: "EstadoTarea",
                        principalColumn: "IdEstadoTarea",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TareaAdministrativa_Usuario_IdAsignado",
                        column: x => x.IdAsignado,
                        principalTable: "Usuario",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TareaAdministrativa_Usuario_IdCreador",
                        column: x => x.IdCreador,
                        principalTable: "Usuario",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Trazabilidad",
                columns: table => new
                {
                    IdTrazabilidad = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUsuario = table.Column<int>(type: "int", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    Accion = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    Entidad = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    IdRegistroAfectado = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    DatosPrevios = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DatosNuevos = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trazabilidad", x => x.IdTrazabilidad);
                    table.ForeignKey(
                        name: "FK_Trazabilidad_Usuario_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuario",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrdenServicio",
                columns: table => new
                {
                    IdOrdenServicio = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdEmpresa = table.Column<int>(type: "int", nullable: false),
                    IdCliente = table.Column<int>(type: "int", nullable: false),
                    IdEncargado = table.Column<int>(type: "int", nullable: false),
                    IdOperario = table.Column<int>(type: "int", nullable: false),
                    IdMaquinariaAsignada = table.Column<int>(type: "int", nullable: false),
                    IdMaquinariaFacturada = table.Column<int>(type: "int", nullable: true),
                    IdEstadoOrden = table.Column<int>(type: "int", nullable: false),
                    FechaSolicitud = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    LugarServicio = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    TrabajoARealizar = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    HoraInicioEstimada = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HoraInicioReal = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HoraFinalizacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Observaciones = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    RequiereFirmaCliente = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    EnviadaCliente = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    PrecargadaGSoft = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrdenServicio", x => x.IdOrdenServicio);
                    table.CheckConstraint("CK_OrdenServicio_Horas", "[HoraFinalizacion] IS NULL OR [HoraInicioReal] IS NULL OR [HoraFinalizacion] >= [HoraInicioReal]");
                    table.ForeignKey(
                        name: "FK_OrdenServicio_Cliente_IdCliente",
                        column: x => x.IdCliente,
                        principalTable: "Cliente",
                        principalColumn: "IdCliente",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrdenServicio_Empresa_IdEmpresa",
                        column: x => x.IdEmpresa,
                        principalTable: "Empresa",
                        principalColumn: "IdEmpresa",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrdenServicio_EstadoOrdenServicio_IdEstadoOrden",
                        column: x => x.IdEstadoOrden,
                        principalTable: "EstadoOrdenServicio",
                        principalColumn: "IdEstadoOrden",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrdenServicio_Maquinaria_IdMaquinariaAsignada",
                        column: x => x.IdMaquinariaAsignada,
                        principalTable: "Maquinaria",
                        principalColumn: "IdMaquinaria",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrdenServicio_Maquinaria_IdMaquinariaFacturada",
                        column: x => x.IdMaquinariaFacturada,
                        principalTable: "Maquinaria",
                        principalColumn: "IdMaquinaria",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrdenServicio_Usuario_IdEncargado",
                        column: x => x.IdEncargado,
                        principalTable: "Usuario",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrdenServicio_Usuario_IdOperario",
                        column: x => x.IdOperario,
                        principalTable: "Usuario",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EventoCalendario",
                columns: table => new
                {
                    IdEvento = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdCreador = table.Column<int>(type: "int", nullable: false),
                    IdTarea = table.Column<int>(type: "int", nullable: true),
                    Titulo = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    FechaInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaFin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TipoEvento = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventoCalendario", x => x.IdEvento);
                    table.CheckConstraint("CK_EventoCalendario_Fechas", "[FechaFin] IS NULL OR [FechaFin] >= [FechaInicio]");
                    table.ForeignKey(
                        name: "FK_EventoCalendario_TareaAdministrativa_IdTarea",
                        column: x => x.IdTarea,
                        principalTable: "TareaAdministrativa",
                        principalColumn: "IdTarea",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EventoCalendario_Usuario_IdCreador",
                        column: x => x.IdCreador,
                        principalTable: "Usuario",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DocumentoPdf",
                columns: table => new
                {
                    IdDocumento = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdOrdenServicio = table.Column<int>(type: "int", nullable: false),
                    NombreArchivo = table.Column<string>(type: "nvarchar(260)", maxLength: 260, nullable: false),
                    RutaArchivo = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    FechaGeneracion = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentoPdf", x => x.IdDocumento);
                    table.ForeignKey(
                        name: "FK_DocumentoPdf_OrdenServicio_IdOrdenServicio",
                        column: x => x.IdOrdenServicio,
                        principalTable: "OrdenServicio",
                        principalColumn: "IdOrdenServicio",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Facturacion",
                columns: table => new
                {
                    IdFacturacion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdOrdenServicio = table.Column<int>(type: "int", nullable: false),
                    FechaEnvio = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Estado = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    ReferenciaGSoft = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: true),
                    Observaciones = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Facturacion", x => x.IdFacturacion);
                    table.ForeignKey(
                        name: "FK_Facturacion_OrdenServicio_IdOrdenServicio",
                        column: x => x.IdOrdenServicio,
                        principalTable: "OrdenServicio",
                        principalColumn: "IdOrdenServicio",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Notificacion",
                columns: table => new
                {
                    IdNotificacion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUsuario = table.Column<int>(type: "int", nullable: true),
                    IdOrdenServicio = table.Column<int>(type: "int", nullable: true),
                    IdCitacion = table.Column<int>(type: "int", nullable: true),
                    Tipo = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Destinatario = table.Column<string>(type: "nvarchar(180)", maxLength: 180, nullable: false),
                    Mensaje = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    FechaEnvio = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Estado = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notificacion", x => x.IdNotificacion);
                    table.CheckConstraint("CK_Notificacion_Origen", "[IdUsuario] IS NOT NULL OR [IdOrdenServicio] IS NOT NULL OR [IdCitacion] IS NOT NULL");
                    table.CheckConstraint("CK_Notificacion_Tipo", "[Tipo] IN ('Email', 'WhatsApp', 'SMS', 'Sistema')");
                    table.ForeignKey(
                        name: "FK_Notificacion_CitacionEstiba_IdCitacion",
                        column: x => x.IdCitacion,
                        principalTable: "CitacionEstiba",
                        principalColumn: "IdCitacion",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Notificacion_OrdenServicio_IdOrdenServicio",
                        column: x => x.IdOrdenServicio,
                        principalTable: "OrdenServicio",
                        principalColumn: "IdOrdenServicio",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Notificacion_Usuario_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuario",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RegistroHorasMaquinaria",
                columns: table => new
                {
                    IdRegistroHoras = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdMaquinaria = table.Column<int>(type: "int", nullable: false),
                    IdOrdenServicio = table.Column<int>(type: "int", nullable: true),
                    Fecha = table.Column<DateOnly>(type: "date", nullable: false),
                    HorasTrabajadas = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    Observacion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistroHorasMaquinaria", x => x.IdRegistroHoras);
                    table.CheckConstraint("CK_RegistroHorasMaquinaria_Horas", "[HorasTrabajadas] > 0");
                    table.ForeignKey(
                        name: "FK_RegistroHorasMaquinaria_Maquinaria_IdMaquinaria",
                        column: x => x.IdMaquinaria,
                        principalTable: "Maquinaria",
                        principalColumn: "IdMaquinaria",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RegistroHorasMaquinaria_OrdenServicio_IdOrdenServicio",
                        column: x => x.IdOrdenServicio,
                        principalTable: "OrdenServicio",
                        principalColumn: "IdOrdenServicio",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Mantenimiento",
                columns: table => new
                {
                    IdMantenimiento = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdMaquinaria = table.Column<int>(type: "int", nullable: false),
                    IdTipoMantenimiento = table.Column<int>(type: "int", nullable: false),
                    IdEstadoMantenimiento = table.Column<int>(type: "int", nullable: false),
                    IdResponsable = table.Column<int>(type: "int", nullable: true),
                    IdRegistroHorasOrigen = table.Column<int>(type: "int", nullable: true),
                    FechaProgramada = table.Column<DateOnly>(type: "date", nullable: true),
                    FechaRealizada = table.Column<DateOnly>(type: "date", nullable: true),
                    Descripcion = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    HorasMaquinaAlMomento = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: true),
                    Observaciones = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mantenimiento", x => x.IdMantenimiento);
                    table.CheckConstraint("CK_Mantenimiento_Horas", "[HorasMaquinaAlMomento] IS NULL OR [HorasMaquinaAlMomento] >= 0");
                    table.ForeignKey(
                        name: "FK_Mantenimiento_EstadoMantenimiento_IdEstadoMantenimiento",
                        column: x => x.IdEstadoMantenimiento,
                        principalTable: "EstadoMantenimiento",
                        principalColumn: "IdEstadoMantenimiento",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Mantenimiento_Maquinaria_IdMaquinaria",
                        column: x => x.IdMaquinaria,
                        principalTable: "Maquinaria",
                        principalColumn: "IdMaquinaria",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Mantenimiento_RegistroHorasMaquinaria_IdRegistroHorasOrigen",
                        column: x => x.IdRegistroHorasOrigen,
                        principalTable: "RegistroHorasMaquinaria",
                        principalColumn: "IdRegistroHoras",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Mantenimiento_TipoMantenimiento_IdTipoMantenimiento",
                        column: x => x.IdTipoMantenimiento,
                        principalTable: "TipoMantenimiento",
                        principalColumn: "IdTipoMantenimiento",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Mantenimiento_Usuario_IdResponsable",
                        column: x => x.IdResponsable,
                        principalTable: "Usuario",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "EstadoCitacion",
                columns: new[] { "IdEstadoCitacion", "Nombre" },
                values: new object[,]
                {
                    { 1, "Pendiente" },
                    { 2, "Enviada" },
                    { 3, "Confirmada" },
                    { 4, "Completada" },
                    { 5, "Cancelada" }
                });

            migrationBuilder.InsertData(
                table: "EstadoMantenimiento",
                columns: new[] { "IdEstadoMantenimiento", "Nombre" },
                values: new object[,]
                {
                    { 1, "Pendiente" },
                    { 2, "Programado" },
                    { 3, "EnProceso" },
                    { 4, "Realizado" },
                    { 5, "Cancelado" }
                });

            migrationBuilder.InsertData(
                table: "EstadoOrdenServicio",
                columns: new[] { "IdEstadoOrden", "Descripcion", "Nombre" },
                values: new object[,]
                {
                    { 1, "Orden creada y pendiente de asignación o inicio.", "Solicitada" },
                    { 2, "Orden asignada a operario y maquinaria.", "Asignada" },
                    { 3, "Servicio en ejecución.", "EnProceso" },
                    { 4, "Finalizada por operario y pendiente de validación.", "PendienteValidacion" },
                    { 5, "Validada por encargado.", "Validada" },
                    { 6, "PDF enviado al cliente.", "EnviadaCliente" },
                    { 7, "Orden enviada o registrada en facturación.", "Facturada" },
                    { 8, "Orden cancelada.", "Cancelada" }
                });

            migrationBuilder.InsertData(
                table: "EstadoTarea",
                columns: new[] { "IdEstadoTarea", "Nombre" },
                values: new object[,]
                {
                    { 1, "Sin comenzar" },
                    { 2, "En proceso" },
                    { 3, "Listo" },
                    { 4, "Cancelada" }
                });

            migrationBuilder.InsertData(
                table: "Rol",
                columns: new[] { "IdRol", "Nombre" },
                values: new object[,]
                {
                    { 1, "Administrador" },
                    { 2, "Encargado" },
                    { 3, "Operario" },
                    { 4, "Oficina" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CitacionEstiba_Fecha",
                table: "CitacionEstiba",
                column: "Fecha");

            migrationBuilder.CreateIndex(
                name: "IX_CitacionEstiba_IdCliente",
                table: "CitacionEstiba",
                column: "IdCliente");

            migrationBuilder.CreateIndex(
                name: "IX_CitacionEstiba_IdEstadoCitacion",
                table: "CitacionEstiba",
                column: "IdEstadoCitacion");

            migrationBuilder.CreateIndex(
                name: "IX_Cliente_Rut",
                table: "Cliente",
                column: "Rut",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cuadrilla_Nombre",
                table: "Cuadrilla",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CuadrillaPersonal_IdCuadrilla",
                table: "CuadrillaPersonal",
                column: "IdCuadrilla");

            migrationBuilder.CreateIndex(
                name: "IX_CuadrillaPersonal_IdPersonalEstiba",
                table: "CuadrillaPersonal",
                column: "IdPersonalEstiba");

            migrationBuilder.CreateIndex(
                name: "IX_DetalleCitacionEstiba_IdCitacion_IdPersonalEstiba",
                table: "DetalleCitacionEstiba",
                columns: new[] { "IdCitacion", "IdPersonalEstiba" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DetalleCitacionEstiba_IdCuadrilla",
                table: "DetalleCitacionEstiba",
                column: "IdCuadrilla");

            migrationBuilder.CreateIndex(
                name: "IX_DetalleCitacionEstiba_IdLiquidacion",
                table: "DetalleCitacionEstiba",
                column: "IdLiquidacion");

            migrationBuilder.CreateIndex(
                name: "IX_DetalleCitacionEstiba_IdPersonalEstiba",
                table: "DetalleCitacionEstiba",
                column: "IdPersonalEstiba");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentoPdf_IdOrdenServicio",
                table: "DocumentoPdf",
                column: "IdOrdenServicio",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Empresa_Rut",
                table: "Empresa",
                column: "Rut",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EstadoCitacion_Nombre",
                table: "EstadoCitacion",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EstadoMantenimiento_Nombre",
                table: "EstadoMantenimiento",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EstadoOrdenServicio_Nombre",
                table: "EstadoOrdenServicio",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EstadoTarea_Nombre",
                table: "EstadoTarea",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EventoCalendario_FechaInicio_FechaFin",
                table: "EventoCalendario",
                columns: new[] { "FechaInicio", "FechaFin" });

            migrationBuilder.CreateIndex(
                name: "IX_EventoCalendario_IdCreador",
                table: "EventoCalendario",
                column: "IdCreador");

            migrationBuilder.CreateIndex(
                name: "IX_EventoCalendario_IdTarea",
                table: "EventoCalendario",
                column: "IdTarea");

            migrationBuilder.CreateIndex(
                name: "IX_Facturacion_IdOrdenServicio",
                table: "Facturacion",
                column: "IdOrdenServicio",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Mantenimiento_IdEstadoMantenimiento",
                table: "Mantenimiento",
                column: "IdEstadoMantenimiento");

            migrationBuilder.CreateIndex(
                name: "IX_Mantenimiento_IdMaquinaria",
                table: "Mantenimiento",
                column: "IdMaquinaria");

            migrationBuilder.CreateIndex(
                name: "IX_Mantenimiento_IdRegistroHorasOrigen",
                table: "Mantenimiento",
                column: "IdRegistroHorasOrigen");

            migrationBuilder.CreateIndex(
                name: "IX_Mantenimiento_IdResponsable",
                table: "Mantenimiento",
                column: "IdResponsable");

            migrationBuilder.CreateIndex(
                name: "IX_Mantenimiento_IdTipoMantenimiento",
                table: "Mantenimiento",
                column: "IdTipoMantenimiento");

            migrationBuilder.CreateIndex(
                name: "IX_Maquinaria_Codigo",
                table: "Maquinaria",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Maquinaria_IdEmpresa",
                table: "Maquinaria",
                column: "IdEmpresa");

            migrationBuilder.CreateIndex(
                name: "IX_Maquinaria_IdTipoMaquinaria",
                table: "Maquinaria",
                column: "IdTipoMaquinaria");

            migrationBuilder.CreateIndex(
                name: "IX_Notificacion_IdCitacion",
                table: "Notificacion",
                column: "IdCitacion");

            migrationBuilder.CreateIndex(
                name: "IX_Notificacion_IdOrdenServicio",
                table: "Notificacion",
                column: "IdOrdenServicio");

            migrationBuilder.CreateIndex(
                name: "IX_Notificacion_IdUsuario",
                table: "Notificacion",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_OrdenServicio_FechaSolicitud",
                table: "OrdenServicio",
                column: "FechaSolicitud");

            migrationBuilder.CreateIndex(
                name: "IX_OrdenServicio_IdCliente",
                table: "OrdenServicio",
                column: "IdCliente");

            migrationBuilder.CreateIndex(
                name: "IX_OrdenServicio_IdEmpresa",
                table: "OrdenServicio",
                column: "IdEmpresa");

            migrationBuilder.CreateIndex(
                name: "IX_OrdenServicio_IdEncargado",
                table: "OrdenServicio",
                column: "IdEncargado");

            migrationBuilder.CreateIndex(
                name: "IX_OrdenServicio_IdEstadoOrden",
                table: "OrdenServicio",
                column: "IdEstadoOrden");

            migrationBuilder.CreateIndex(
                name: "IX_OrdenServicio_IdMaquinariaAsignada",
                table: "OrdenServicio",
                column: "IdMaquinariaAsignada");

            migrationBuilder.CreateIndex(
                name: "IX_OrdenServicio_IdMaquinariaFacturada",
                table: "OrdenServicio",
                column: "IdMaquinariaFacturada");

            migrationBuilder.CreateIndex(
                name: "IX_OrdenServicio_IdOperario",
                table: "OrdenServicio",
                column: "IdOperario");

            migrationBuilder.CreateIndex(
                name: "IX_PersonalEstiba_Cedula",
                table: "PersonalEstiba",
                column: "Cedula",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RegistroHorasMaquinaria_IdMaquinaria_Fecha",
                table: "RegistroHorasMaquinaria",
                columns: new[] { "IdMaquinaria", "Fecha" });

            migrationBuilder.CreateIndex(
                name: "IX_RegistroHorasMaquinaria_IdOrdenServicio",
                table: "RegistroHorasMaquinaria",
                column: "IdOrdenServicio");

            migrationBuilder.CreateIndex(
                name: "IX_Rol_Nombre",
                table: "Rol",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TareaAdministrativa_IdAsignado_IdEstadoTarea",
                table: "TareaAdministrativa",
                columns: new[] { "IdAsignado", "IdEstadoTarea" });

            migrationBuilder.CreateIndex(
                name: "IX_TareaAdministrativa_IdCreador",
                table: "TareaAdministrativa",
                column: "IdCreador");

            migrationBuilder.CreateIndex(
                name: "IX_TareaAdministrativa_IdEstadoTarea",
                table: "TareaAdministrativa",
                column: "IdEstadoTarea");

            migrationBuilder.CreateIndex(
                name: "IX_TipoMantenimiento_Nombre",
                table: "TipoMantenimiento",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TipoMaquinaria_Nombre",
                table: "TipoMaquinaria",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Trazabilidad_Entidad_IdRegistroAfectado",
                table: "Trazabilidad",
                columns: new[] { "Entidad", "IdRegistroAfectado" });

            migrationBuilder.CreateIndex(
                name: "IX_Trazabilidad_IdUsuario_Fecha",
                table: "Trazabilidad",
                columns: new[] { "IdUsuario", "Fecha" });

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_Cedula",
                table: "Usuario",
                column: "Cedula",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_Correo",
                table: "Usuario",
                column: "Correo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_IdEmpresa",
                table: "Usuario",
                column: "IdEmpresa");

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_IdRol",
                table: "Usuario",
                column: "IdRol");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CuadrillaPersonal");

            migrationBuilder.DropTable(
                name: "DetalleCitacionEstiba");

            migrationBuilder.DropTable(
                name: "DocumentoPdf");

            migrationBuilder.DropTable(
                name: "EventoCalendario");

            migrationBuilder.DropTable(
                name: "Facturacion");

            migrationBuilder.DropTable(
                name: "Mantenimiento");

            migrationBuilder.DropTable(
                name: "Notificacion");

            migrationBuilder.DropTable(
                name: "Trazabilidad");

            migrationBuilder.DropTable(
                name: "Cuadrilla");

            migrationBuilder.DropTable(
                name: "LiquidacionEstiba");

            migrationBuilder.DropTable(
                name: "PersonalEstiba");

            migrationBuilder.DropTable(
                name: "TareaAdministrativa");

            migrationBuilder.DropTable(
                name: "EstadoMantenimiento");

            migrationBuilder.DropTable(
                name: "RegistroHorasMaquinaria");

            migrationBuilder.DropTable(
                name: "TipoMantenimiento");

            migrationBuilder.DropTable(
                name: "CitacionEstiba");

            migrationBuilder.DropTable(
                name: "EstadoTarea");

            migrationBuilder.DropTable(
                name: "OrdenServicio");

            migrationBuilder.DropTable(
                name: "EstadoCitacion");

            migrationBuilder.DropTable(
                name: "Cliente");

            migrationBuilder.DropTable(
                name: "EstadoOrdenServicio");

            migrationBuilder.DropTable(
                name: "Maquinaria");

            migrationBuilder.DropTable(
                name: "Usuario");

            migrationBuilder.DropTable(
                name: "TipoMaquinaria");

            migrationBuilder.DropTable(
                name: "Empresa");

            migrationBuilder.DropTable(
                name: "Rol");
        }
    }
}

