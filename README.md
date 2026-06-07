# Sistema Portuario - Backend

Backend REST desarrollado con ASP.NET Core Web API, Entity Framework Core y Azure SQL para una plataforma demo de operaciones portuarias, logística, estiba y maquinaria.

El proyecto fue construido como portfolio técnico profesional, simulando una arquitectura empresarial real con autenticación JWT, roles, trazabilidad, despliegue cloud y CI/CD.

---

# Link para probar el sistema

https://sistema-portuario.martinpintos677.workers.dev/

---

# Desarrollo asistido por IA

El proyecto fue desarrollado utilizando un enfoque moderno de AI-assisted development.

El frontend inicial fue generado parcialmente con Lovable como acelerador visual. Posteriormente, tanto frontend como backend fueron integrados, revisados y refinados mediante trabajo iterativo asistido con Codex y herramientas de IA.

El foco estuvo en:

* arquitectura backend modular
* integración React + .NET
* autenticación JWT y refresh tokens
* trazabilidad y auditoría
* Entity Framework Core
* SQL Server / Azure SQL
* manejo global de errores
* despliegue cloud
* CI/CD
* seguridad y hardening
* documentación y organización del proyecto

La IA fue utilizada como acelerador técnico, mientras las decisiones de arquitectura, validación, integración y evolución del sistema fueron dirigidas conscientemente por el desarrollador.

---

# Tecnologías

## Backend

* ASP.NET Core Web API (.NET 8)
* Entity Framework Core
* SQL Server / Azure SQL
* JWT Authentication
* Refresh Tokens
* BCrypt
* Swagger / OpenAPI

## Infraestructura

* Azure App Service
* Azure SQL
* GitHub Actions
* GitHub

---

# Funcionalidades principales

* Login JWT.
* Refresh token.
* Roles y autorización.
* Gestión de usuarios.
* Gestión de empresas.
* Gestión de clientes.
* Gestión de maquinaria.
* Gestión de mantenimiento.
* Gestión de órdenes de servicio.
* Gestión de estiba.
* Gestión de notificaciones.
* Trazabilidad y auditoría.
* Paginación reutilizable.
* Manejo global de errores con ProblemDetails.
* Endpoint Health/Warmup.
* Deploy productivo en Azure.

---

# Arquitectura backend

La API sigue una arquitectura basada en:

```text id="vq5wd1"
Controllers
Services
DTOs
Models
Data
Security
Configurations
Middleware
```

## Flujo principal

```text id="bh0mns"
Controller
↓
Service
↓
DbContext
↓
SQL Server
```

---

# Buenas prácticas aplicadas

* Controllers delgados.
* Lógica centralizada en Services.
* DTOs separados de entidades.
* Dependency Injection nativa.
* Async/Await.
* Validaciones con DataAnnotations.
* Paginación reutilizable.
* Manejo consistente de errores.
* Configuración sensible fuera del repositorio.
* CORS configurable.
* EF Core con Fluent API.
* BCrypt para contraseñas.
* JWT Bearer Authentication.
* Refresh tokens persistidos hasheados.

---

# Seguridad

El backend implementa medidas reales de seguridad para despliegue público:

* JWT firmado con HMAC SHA-256.
* Refresh token con rotación.
* Password hashing con BCrypt.
* Roles diferenciados.
* Scope por empresa mediante claims.
* ProblemDetails para evitar exposición de errores internos.
* Reintentos ante errores transitorios de Azure SQL.

---

# Base de datos

## Azure SQL

El sistema utiliza Azure SQL con:

* migraciones EF Core
* seed idempotente
* scripts de validación
* scripts de refresco demo
* datos anonimizados preparados para exposición pública

---

# CI/CD

El proyecto utiliza GitHub Actions para automatizar:

```text id="l8f4ja"
Restore
Build
Publish
Deploy Azure
```

Cada push a `master` dispara el flujo de despliegue automático hacia Azure App Service.

---

# Objetivo del proyecto

El objetivo del proyecto es demostrar:

* diseño de APIs REST modernas
* arquitectura backend en .NET
* autenticación y autorización
* integración frontend/backend
* despliegue cloud real
* CI/CD
* mantenibilidad
* escalabilidad
* criterio profesional de entrega
