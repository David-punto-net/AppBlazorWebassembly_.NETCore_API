# Sistema Web en ASP.NET Core & Blazor WebAssembly

**Aplicación** web desarrollada con **.NET 8, Blazor WebAssembly y una API RESTful en ASP.NET Core.**. La aplicación permite la administración de productos, categorías, pedidos y usuarios, con funcionalidades avanzadas de seguridad y autenticación. 

## 🛠️ Tecnologías Utilizadas  
- **Backend:** ASP.NET Core(.NET8) con API RESTful y Entity Framework Core (Code First).  
- **Frontend:** Blazor WebAssembly con componentes reutilizables.  
- **Base de Datos:** SQL Server.  
- **Seguridad:** Autenticación con JWT(JSON Web Token), recuperación de contraseña y confirmación de email.  
- **Almacenamiento de Imágenes:** Manejo de archivos para productos y usuarios.  
- **Optimización:** Paginación, filtros dinámicos y carga de datos desde fuentes externas.
- **Repository Pattern y Unit of Work** Para la gestión eficiente de datos.
- **MSTest** Framework para pruebas unitarias.
- **Despliegue:** Configurado para Azure y uso en equipo con GitHub.  

## 🔗 Funcionalidades Clave  
✅ **API RESTful** con endpoints para CRUD de productos, pedidos, usuarios y categorías.  
✅ **Carrito de compras** con gestión de productos y procesamiento de pedidos.  
✅ **Panel de administración** para gestionar categorías, países y estados.  
✅ **Sistema de autenticación** con login, registro y recuperación de contraseña.  
✅ **Soporte para múltiples imágenes** en productos y perfiles de usuario.  

## **Características Principales**
- **Autenticación y Seguridad:**
  - Registro, login y logout de usuarios.
  - Confirmación de cuenta mediante correo electrónico.
  - Recuperación y cambio de contraseña.
  - Administración de roles y permisos.
  - Integración de **tokens en Swagger** para autenticación.
  - Autenticación con JWT: Implementación de JSON Web Tokens (JWT) para autenticación segura en la API.

- **Gestión de Datos:**
  - CRUD de países, regiones y ciudades.
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



