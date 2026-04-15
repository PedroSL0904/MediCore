# 🏥 MediCore SaaS API

> **Estatus del Proyecto:** 🚀 MVP Refactorizado (Arquitectura de Servicios)

MediCore es un ecosistema backend diseñado bajo un modelo SaaS (Software as a Service) para la gestión integral de clínicas médicas. Implementa patrones de diseño modernos para garantizar escalabilidad, mantenibilidad y seguridad de grado empresarial.

## 💻 Tecnologías Utilizadas
* **C# 12 / .NET 8** (Web API)
* **Entity Framework Core 8** (ORM)
* **SQL Server** (Base de Datos Relacional)
* **BCrypt.Net** (Hashing de contraseñas)
* **FluentValidation** (Validación estricta de DTOs)

## 🛡️ Arquitectura y Seguridad
* **Capa de Servicios (Service Layer):** Lógica de negocio totalmente desacoplada de los controladores, facilitando pruebas unitarias y mantenimiento.
* **Seguridad de Secretos:** Implementación de **.NET User Secrets** para proteger llaves JWT y cadenas de conexión, evitando fugas de información en repositorios.
* **Aislamiento SaaS (Multi-Tenant):** Datos protegidos por *Global Query Filters*. Cada clínica es un silo de información independiente.
* **Optimización EF Core:** Uso sistemático de `.AsNoTracking()` en consultas de lectura para maximizar el rendimiento del servidor.
* **Control de Acceso (RBAC):** Seguridad granular basada en roles (Medico, Admin, SuperAdmin).
* **Resiliencia:** Manejo global de excepciones con `IExceptionHandler` y respuestas estandarizadas `ProblemDetails`.

## 🛠️ Roadmap de Desarrollo

### ✅ Fase 1 y 2: Arquitectura y Core (Completadas)
- [x] Estructura inicial y conexión EF Core.
- [x] Autenticación JWT + BCrypt + User Secrets.
- [x] Módulos Core: Pacientes, Citas y Cobros.
- [x] **Refactorización a Capa de Servicios** (Interfaces + Services).
- [x] **Optimización de rendimiento** con `AsNoTracking` y paginación.
- [x] Blindaje de datos con FluentValidation y Soft Delete.

### 🚀 Siguiente Paso: Fase 3 (Documentación y DevOps)
- [ ] **Integración de Swagger / OpenAPI:** Documentación interactiva con soporte para seguridad JWT.
- [ ] **Logging Estructurado:** Implementación de Serilog para trazabilidad.
- [ ] **Unit Testing:** Pruebas unitarias para la capa de servicios.
- [ ] **Dockerización:** Preparación de contenedores para despliegue en la nube.

## ⚙️ Cómo ejecutar el proyecto
1. Clonar el repositorio.
2. Navegar a `MediCore.Api` e inicializar secretos: `dotnet user-secrets init`.
3. Configurar la llave JWT: `dotnet user-secrets set "Jwt:Key" "TU_LLAVE_SECRETA"`.
4. Actualizar la cadena de conexión en `appsettings.json`.
5. Ejecutar `dotnet run`.
6. Usar `Pruebas.http` con variables globales para testing rápido.