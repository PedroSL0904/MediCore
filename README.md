# 🏥 MediCore SaaS API
> **Estatus del Proyecto:** 🚧 En Desarrollo Activo (WIP)

MediCore es un ecosistema backend diseñado bajo un modelo SaaS para la gestión integral de clínicas médicas...

## Tecnologías Utilizadas
* C# 
* ASP.NET Core (Web API)
* Entity Framework Core
* SQL Server

## Arquitectura y Seguridad
* Diseño de base de datos relacional con integridad referencial estricta.
* Lógica preparada para aislamiento de datos (Esquema Multi-Cliente).

## 🛠️ Roadmap de Desarrollo
- [x] Estructura inicial de Base de Datos (SQL Server).
- [x] Conexión mediante Entity Framework Core.
- [ ] Implementación de Autenticación con JWT (Seguridad).
- [ ] Módulo de Expediente Clínico Electrónico.
- [ ] Motor de Cobranza y Facturación.
- [ ] Reportes avanzados con SQL Window Functions.

## 🚀 Cómo ejecutar el proyecto
1. Clonar el repositorio.
2. Asegurarse de tener una instancia de **SQL Server** activa.
3. Ejecutar el script SQL incluido en la carpeta `/Docs` (opcional, si guardas ahí tus scripts).
4. Actualizar la cadena de conexión en `appsettings.json`.
5. Ejecutar `dotnet run` o iniciar desde Visual Studio.