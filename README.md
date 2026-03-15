# Todo API — ASP.NET Core + MongoDB + Identity + Docker

**Université Cheikh Anta Diop de Dakar — École Supérieure Polytechnique (ESP)**
**Département Génie Informatique — Année 2025/2026**
**TP .NET — Chargé de Cours : E. H. Ousmane Diallo**

---

## Description

API REST de gestion de tâches (Todo) construite avec **ASP.NET Core 10** basée sur des contrôleurs.
Les todos sont stockés dans **MongoDB**. L'authentification et la gestion des rôles sont assurées par **ASP.NET Core Identity** avec **JWT Bearer tokens**. La base des utilisateurs est persistée dans **SQLite**.

---

## Architecture

```
TodoApiMoussaNdoye/
├── Controllers/
│   ├── AuthController.cs        # Inscription et connexion (JWT)
│   └── TodoItemsController.cs   # CRUD des todos
├── Models/
│   ├── TodoItem.cs              # Document MongoDB (avec champ Secret)
│   ├── TodoItemDTO.cs           # DTO public (sans Secret)
│   ├── TodoDatabaseSettings.cs  # Configuration MongoDB
│   ├── RegisterRequest.cs       # Requête d'inscription
│   └── LoginRequest.cs          # Requête de connexion
├── Services/
│   └── TodosService.cs          # Accès MongoDB
├── Data/
│   └── AppDbContext.cs          # IdentityDbContext (SQLite)
├── Migrations/                  # Migrations EF Core
├── Program.cs                   # Configuration et démarrage
├── appsettings.json
├── Dockerfile
└── docker-compose.yml           # (à la racine du repo)
```

---

## Endpoints

### Authentification

| Méthode | Route | Description | Auth requise |
|---|---|---|---|
| POST | `/api/auth/register` | Créer un compte | Non |
| POST | `/api/auth/login` | Se connecter et obtenir un JWT | Non |

### Todos

| Méthode | Route | Description | Auth requise |
|---|---|---|---|
| GET | `/api/todoitems` | Lister tous les todos | Tout utilisateur connecté |
| GET | `/api/todoitems/{id}` | Obtenir un todo par ID | Tout utilisateur connecté |
| POST | `/api/todoitems` | Créer un todo | Rôle `admin` |
| PUT | `/api/todoitems/{id}` | Modifier un todo | Rôle `admin` |
| DELETE | `/api/todoitems/{id}` | Supprimer un todo | Rôle `admin` |

---

## Prérequis

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [MongoDB](https://www.mongodb.com/try/download/community) (en local sur le port 27017)
- Docker + Docker Compose *(pour la version conteneurisée)*

---

## Lancer en local

### 1. Restaurer les dépendances

```bash
dotnet restore TodoApiMoussaNdoye/TodoApiMoussaNdoye.csproj
```

### 2. Lancer l'application

```bash
# HTTP sur http://localhost:5175
dotnet run --project TodoApiMoussaNdoye --launch-profile http

# HTTPS sur https://localhost:7255
dotnet run --project TodoApiMoussaNdoye --launch-profile https
```

> La migration SQLite est appliquée automatiquement au démarrage.
> Les rôles `admin` et `user` sont créés automatiquement s'ils n'existent pas.

### 3. Accéder à Swagger

```
https://localhost:7255/swagger
```

---

## Lancer avec Docker

```bash
docker-compose up --build
```

L'API sera disponible sur `http://localhost:8080/swagger`.

Le fichier `docker-compose.yml` démarre deux services :
- `mongodb` — image officielle `mongo:7`, port 27017
- `api` — l'application .NET, port 8080, connectée à MongoDB via le réseau interne

---

## Guide de test rapide

### 1. Créer un compte admin

```http
POST /api/auth/register
Content-Type: application/json

{
  "username": "admin1",
  "password": "Admin@123",
  "role": "admin"
}
```

Réponse : `{ "token": "<jwt>" }`

### 2. Créer un todo (admin requis)

```http
POST /api/todoitems
Authorization: Bearer <jwt>
Content-Type: application/json

{
  "name": "Apprendre ASP.NET Core",
  "isComplete": false
}
```

Réponse : `201 Created`

### 3. Lister les todos

```http
GET /api/todoitems
Authorization: Bearer <jwt>
```

Réponse : `200 OK` — tableau de todos **sans le champ `Secret`**

### 4. Créer un compte utilisateur simple

```http
POST /api/auth/register
Content-Type: application/json

{
  "username": "user1",
  "password": "User@123",
  "role": "user"
}
```

### 5. Tenter de créer un todo avec le token user

```http
POST /api/todoitems
Authorization: Bearer <jwt_user>
```

Réponse attendue : `403 Forbidden`

---

## Sécurité

- Les mots de passe sont hashés par ASP.NET Core Identity.
- Les tokens JWT expirent après **60 minutes** (configurable via `Jwt:ExpiryMinutes`).
- Le champ `Secret` du modèle `TodoItem` n'est **jamais exposé** dans les réponses API (pattern DTO).
- En production, remplacer la clé JWT dans `appsettings.json` par une valeur sécurisée (variable d'environnement).

---

## Configuration

`appsettings.json` :

```json
{
  "TodoDatabaseSettings": {
    "ConnectionString": "mongodb://localhost:27017",
    "DatabaseName": "TodoDb",
    "CollectionName": "Todos"
  },
  "Jwt": {
    "Key": "SuperSecretKey_ChangeInProduction_AtLeast32Chars!",
    "Issuer": "TodoApi",
    "Audience": "TodoApiUsers",
    "ExpiryMinutes": 60
  },
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=identity.db"
  }
}
```

En mode Docker, la variable d'environnement `MONGODB_URI` prend la priorité sur `TodoDatabaseSettings:ConnectionString`.

---

## Technologies utilisées

| Technologie | Usage |
|---|---|
| ASP.NET Core 10 | Framework API |
| MongoDB + MongoDB.Driver 3.x | Stockage des todos |
| ASP.NET Core Identity | Gestion des utilisateurs et rôles |
| Entity Framework Core + SQLite | Persistance des utilisateurs |
| JWT Bearer | Authentification stateless |
| Swashbuckle / Swagger UI | Documentation interactive |
| Docker + Docker Compose | Conteneurisation |

---

## Auteur

**Moussa Ndoye** — Étudiant en DI3 Génie Informatique, ESP/UCAD
