# AGENTS.md - Sistema Portuario de Gruas

## Descripción del proyecto

Sistema de gestión integral para operador portuario, logística, estiba y maquinaria.

El sistema contempla:
- Gestión de usuarios y roles
- Gestión de órdenes de servicio
- Gestión de maquinaria
- Gestión de mantenimiento
- Gestión de estiba
- Gestión de tareas administrativas
- Notificaciones
- Facturación
- Auditoría y trazabilidad

El proyecto debe desarrollarse con estándares profesionales, priorizando:
- código limpio
- mantenibilidad
- escalabilidad
- separación de responsabilidades
- buenas prácticas
- seguridad
- rendimiento

---

---

# Principios de diseño y buenas prácticas

El código debe seguir principios SOLID de forma pragmática, sin sobreingeniería.

## SOLID aplicado al proyecto

- Single Responsibility Principle:
  Cada clase debe tener una responsabilidad clara.
  Los Controllers no deben contener lógica de negocio.
  Los Services deben concentrar reglas de negocio.
  Los DTOs solo deben transportar datos.

- Open/Closed Principle:
  Evitar modificar código existente innecesariamente cuando se pueda extender de forma limpia.

- Liskov Substitution Principle:
  Las interfaces y abstracciones deben poder reemplazarse sin romper el comportamiento esperado.

- Interface Segregation Principle:
  Evitar interfaces enormes.
  Crear interfaces pequeñas y específicas por módulo cuando sea necesario.

- Dependency Inversion Principle:
  Los Controllers deben depender de interfaces de Services, no de implementaciones concretas.
  Los Services pueden usar DbContext directamente, pero no deben crear dependencias manualmente.

## Importante

- No aplicar patrones de diseño innecesarios.
- No usar Repository Pattern sobre Entity Framework Core.
- No crear capas extra si no aportan valor real.
- Priorizar claridad, mantenibilidad y simplicidad.
- No se debe agregar lógica de negocio en Controllers. Si una acción requiere validaciones, reglas, auditoría o cálculos, debe implementarse en el Service correspondiente.
- Utilizar Dependency Injection nativa de ASP.NET Core.

- Services: Scoped
- DbContext: Scoped
- Helpers sin estado: Singleton solo si corresponde
- No registrar DbContext como Singleton

# Stack tecnológico

## Backend
- ASP.NET Core Web API (.NET 8)
- Entity Framework Core
- SQL Server
- JWT Authentication
- BCrypt
- Swagger / OpenAPI

## Frontend
- React
- Axios
- React Router

---

# Arquitectura

La arquitectura debe seguir el siguiente flujo:

```text
Controller
?
Service
?
DbContext
?
SQL Server
```