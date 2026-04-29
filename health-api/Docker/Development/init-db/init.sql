-- Banco para o Keycloak
CREATE DATABASE keycloak_db;

-- Banco para a API .NET
CREATE DATABASE health_api_db;

-- Garante que o usuário admin tenha acesso total a ambos
GRANT ALL PRIVILEGES ON DATABASE keycloak_db TO admin;
GRANT ALL PRIVILEGES ON DATABASE health_api_db TO admin;