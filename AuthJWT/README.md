# Microservicio de Autenticación

Este microservicio se encarga exclusivamente de la gestión de autenticación y autorización mediante JWT.

## Características

- **Registro de usuarios**: Endpoint para crear nuevos usuarios
- **Login**: Autenticación con username/password
- **JWT Tokens**: Generación de access tokens y refresh tokens
- **Refresh Token**: Renovación de tokens expirados
- **Validación de tokens**: Endpoint para validar tokens
- **Revocación de tokens**: Capacidad de revocar refresh tokens

## Endpoints

### POST `/api/auth/register`
Registra un nuevo usuario
```json
{
  "usuarioNombre": "string",
  "contraseña": "string",
  "rol": "string"
}
```

### POST `/api/auth/login`
Autentica un usuario
```json
{
  "usuarioNombre": "string",
  "contraseña": "string"
}
```

### POST `/api/auth/refresh`
Renueva un access token usando el refresh token
```json
{
  "refreshToken": "string"
}
```

### POST `/api/auth/validate`
Valida si un token es válido
```json
"token_string"
```

### POST `/api/auth/revoke`
Revoca un refresh token (requiere autenticación)
```json
{
  "refreshToken": "string"
}
```

### GET `/api/auth/protected`
Endpoint protegido para probar la autenticación

## Configuración

### Base de datos
Configura la cadena de conexión en `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "tu_cadena_de_conexion"
  }
}
```

### JWT
Configura los parámetros JWT en `appsettings.json`:
```json
{
  "Jwt": {
    "Key": "tu_clave_secreta_muy_larga",
    "Issuer": "AuthMicroservice",
    "Audience": "AuthMicroservice"
  }
}
```

## Uso con Docker

```bash
docker-compose up -d
```

## Migración de base de datos

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

## Integración con otros microservicios

Los otros microservicios pueden validar los tokens JWT usando la configuración JWT apropiada y el endpoint `/api/auth/validate` para validación remota.
