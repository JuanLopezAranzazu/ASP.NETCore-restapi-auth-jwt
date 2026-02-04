# Sistema de Reservas con ASP.NET Core Web API

Este proyecto es una API web desarrollada con ASP.NET Core que permite gestionar un sistema de reservas. Permite reservar recursos evitando conflictos de horarios y asegurando la disponibilidad.

## Ejecución del Proyecto

Para ejecutar el proyecto, asegúrate de tener instalado .NET SDK en tu máquina. Luego, navega al directorio del proyecto y ejecuta el siguiente comando:

```bash
dotnet run
```

## Migraciones de la Base de Datos

Para aplicar las migraciones y actualizar la base de datos, utiliza el siguiente comando:

```bash
dotnet ef database update
```

## Endpoints Disponibles

- **Autenticación**
  - `POST /api/auth/login`: Iniciar sesión y obtener un token JWT.
  - `POST /api/auth/register`: Registrar un nuevo usuario.
  - `POST /api/auth/refresh`: Refrescar el token JWT.

- **Recursos**
  - `GET /api/resources`: Obtener la lista de recursos disponibles.
  - `GET /api/resources/{id}`: Obtener detalles de un recurso específico.
  - `POST /api/resources`: Crear un nuevo recurso.
  - `PUT /api/resources/{id}`: Actualizar un recurso existente.
  - `DELETE /api/resources/{id}`: Eliminar un recurso.

- **Reservas**
  - `GET /api/bookings`: Obtener la lista de reservas del usuario autenticado.
  - `GET /api/bookings/all`: Obtener la lista de todas las reservas.
  - `GET /api/bookings/{id}`: Obtener detalles de una reserva específica.
  - `POST /api/bookings`: Crear una nueva reserva.
  - `DELETE /api/bookings/{id}`: Cancelar una reserva.