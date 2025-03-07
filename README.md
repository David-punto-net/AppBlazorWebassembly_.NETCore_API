# Sistema Web con ASP.NET Core & Blazor

**Aplicación** web desarrollada con **.NET 8, ASP.NET Core y Blazor**, diseñada para gestionar un sistema de ventas en línea. La aplicación permite la administración de productos, categorías, pedidos y usuarios, con funcionalidades avanzadas de seguridad y autenticación. 

## **Características Principales**
- **Autenticación y Seguridad:**
  - Registro, login y logout de usuarios.
  - Confirmación de cuenta mediante correo electrónico.
  - Recuperación y cambio de contraseña.
  - Administración de roles y permisos.
  - Integración de **tokens en Swagger** para autenticación.
  - Autenticación con JWT: Implementación de JSON Web Tokens (JWT) para autenticación segura en la API.

- **Gestión de Datos:**
  - CRUD de países, regiones y ciudades con integración de datos externos.
  - CRUD de categorías y productos con múltiples imágenes.
  - Implementación de paginación y filtros en las vistas.

- **Carrito de Compras y Pedidos:**
  - Agregado de productos al carrito de compras.
  - Modificación del carrito y confirmación de pedidos.
  - Administración de estados de pedidos (nuevo, en proceso, enviado, entregado).

- **Interfaz de Usuario Mejorada:**
  - Uso de **Blazor WebAssembly** para una experiencia fluida.
  - Integración con **Bootstrap Icons** para mejorar la navegación.
  - Implementación de **SweetAlert2** para notificaciones y confirmaciones.

- **Optimización y Reutilización de Código:**
  - Uso de **componentes Blazor reutilizables** para formularios y listas.
  - Implementación del **patrón Repository y Unit of Work** para acceso a datos.
  - CRUD genérico para reducir código duplicado.

- **Despliegue y Configuración:**
  - Configuración del entorno de desarrollo con **Entity Framework Core Code First**.
  - Uso de **Seeders** para poblar la base de datos con datos iniciales.
  - Despliegue en **Azure**.

## **Tecnologías Utilizadas**
**Backend:**
- **ASP.NET Core (.NET 8) →** Framework principal para el desarrollo del backend.
- **Entity Framework Core →** ORM para la gestión de base de datos.
- **SQL Server →** Motor de base de datos utilizado.
- **JWT (JSON Web Token) →** Implementación de autenticación basada en tokens.
- **Swagger →** Documentación y pruebas de la API.
- **Repository Pattern y Unit of Work →** Para la gestión eficiente de datos.
- **MSTest →** Framework para pruebas unitarias.
- **Azure →** Plataforma de despliegue en la nube.

**Frontend:**
- **Blazor WebAssembly →** Framework para el desarrollo del frontend en C#.
- **Bootstrap & Bootstrap Icons →** Framework de diseño y librería de iconos.
- **SweetAlert2 →** Alertas visuales y notificaciones.
- **HttpClient →** Para la comunicación con el backend.
- **Blazor Components →** Uso de componentes reutilizables para formularios y listas dinámicas.

**El proyecto combina Blazor en el frontend con ASP.NET Core en el backend, ofreciendo una solución moderna y escalable.**


