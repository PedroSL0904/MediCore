# 🏥 MediCore SaaS API

> **Estatus del Proyecto:** 🚀 MVP Funcional 

MediCore es un ecosistema backend diseñado bajo un modelo SaaS (Software as a Service) para la gestión integral de clínicas médicas. Está construido con arquitectura robusta orientada a la seguridad de la información, el aislamiento de datos y el rendimiento.

## 💻 Tecnologías Utilizadas
* **C# 12 / .NET 8** (Web API)
* **Entity Framework Core 8** (ORM)
* **SQL Server** (Base de Datos Relacional)
* **BCrypt.Net** (Hashing de contraseñas)
* **FluentValidation** (Validación estricta de DTOs)

## 🛡️ Arquitectura y Seguridad
* **Aislamiento de Datos (SaaS Multi-Tenant):** Implementado a nivel de base de datos usando *Global Query Filters* en EF Core. Matemáticamente imposible cruzar información entre diferentes clínicas.
* **Seguridad de Acceso:** Autenticación mediante Tokens JWT y Encriptación de contraseñas con BCrypt.
* **Control de Acceso Basado en Roles (RBAC):** Endpoints protegidos por nivel de usuario (Ej. `[Authorize(Roles = "SuperAdmin")]`).
* **Integridad Transaccional:** Uso de transacciones implícitas en SQL Server para evitar pérdidas de datos en operaciones financieras (Cobros y Citas).
* **Resiliencia (Global Exception Handler):** Middleware nativo de .NET 8 (`IExceptionHandler`) para capturar errores, evitar exposición de código fuente y devolver `ProblemDetails` estandarizados.
* **Borrado Lógico (Soft Delete):** Historial médico financiero inmutable. Los registros nunca se borran físicamente mediante `DELETE`, se desactivan.
* **Rendimiento:** Paginación de datos a nivel de base de datos (`Skip` / `Take`) para el manejo eficiente de grandes volúmenes de pacientes.

## 🛠️ Roadmap de Desarrollo (Fase 1: Completada)
- [x] Estructura inicial de Base de Datos (SQL Server).
- [x] Conexión mediante Entity Framework Core.
- [x] Implementación de Autenticación con JWT, BCrypt y Aislamiento SaaS.
- [x] Módulo de Expediente Clínico Electrónico (Pacientes).
- [x] Módulo de Gestión de Citas Médicas.
- [x] Motor de Cobranza y Facturación.
- [x] Reportes avanzados con inyección de SQL Crudo (Window Functions).
- [x] Blindaje Enterprise (Validaciones, Paginación, Manejo de Excepciones, Soft Delete, RBAC).

## 🚀 Siguientes Pasos (Fase 2: Refactorización y Buenas Prácticas)
- [ ] Separación de Responsabilidades (Extraer lógica a Capa de Servicios).
- [ ] Optimizaciones de lectura en EF Core (`AsNoTracking`).
- [ ] Integración de Swagger / OpenAPI para documentación.
- [ ] Implementación de User Secrets y manejo seguro de configuraciones.

## ⚙️ Cómo ejecutar el proyecto
1. Clonar el repositorio.
2. Asegurarse de tener una instancia de **SQL Server** activa.
3. Actualizar la cadena de conexión en `appsettings.json`.
4. Ejecutar las migraciones de base de datos o el script SQL (según corresponda).
5. Ejecutar `dotnet run` o iniciar desde Visual Studio.
6. Utilizar la carpeta `.http` incluida en el proyecto para probar los flujos directamente desde el IDE.
