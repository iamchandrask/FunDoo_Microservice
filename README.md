# Fundoo Notes Microservices with Dapr

This master guide is a single training handout for building a production-style **Fundoo Notes Microservices Application** using **ASP.NET Core (.NET 8+)**, **Clean Architecture**, **CQRS**, **EF Core**, **JWT**, **Ocelot**, **Redis**, **Docker**, **xUnit**, and **Dapr**.[1][2][3]

## What you are building

The application is split into small focused services: **User Service**, **Notes Service**, **Label Service**, **Reminder Service**, and **Collaboration Service**, with **Ocelot** as the API Gateway.[4][2]

Dapr is integrated to provide **Service Invocation**, **Pub/Sub**, **State Management**, and **Workflow**, while EF Core and SQL Server continue to manage the main relational data model.[1][3][5]

## Why this project matters

This project teaches both application structure and distributed system thinking in one solution.[3][1]

A beginner who completes it should understand microservices, Clean Architecture, CQRS, LINQ, JWT authentication, Ocelot routing, Dapr sidecars, Docker Compose, and unit testing with xUnit and Moq.[2][3][6]

## Core concepts

### Microservices

A microservice is a small application focused on one business responsibility, such as users, notes, labels, reminders, or collaboration.[4]

Microservices help with separation of concerns, independent deployment, and better scalability compared with a single large application.[4]

### Clean Architecture

Clean Architecture separates code into four layers: **Domain**, **Application**, **Infrastructure**, and **API**.[4]

This keeps business rules independent from controllers, database tools, and infrastructure details.[4]

### CQRS

CQRS means separating writes from reads: **Commands** change data and **Queries** read data.[1]

This pattern keeps handlers focused and makes controller code much smaller and easier to understand.[1]

### LINQ

LINQ is C# query syntax used with collections and EF Core to filter, order, and project data in a readable way.[5]

### Ocelot

Ocelot is an API Gateway for ASP.NET Core that routes client requests from upstream URLs to downstream microservices.[2]

### Dapr

Dapr is a distributed application runtime that adds building blocks such as service invocation, pub/sub, state management, and workflow through a sidecar model.[1][3]

## Final architecture

The final system includes these services:

| Service | Responsibility |
|---|---|
| User Service | Register, login, JWT token generation |
| Notes Service | Notes CRUD, drafts, note events |
| Label Service | Labels and note-label mapping |
| Reminder Service | Reminder CRUD and workflows |
| Collaboration Service | Collaborators and service-to-service validation |
| API Gateway | One public entry point using Ocelot |

The gateway handles client-to-service routing, while Dapr sidecars handle service invocation, pub/sub, state access, and workflow support.[2][7]

## Recommended folder structure

```text
FundooNotesMicroservices/
├── components/
│   ├── pubsub.yaml
│   ├── statestore.yaml
│   └── workflowstatestore.yaml
├── src/
│   ├── ApiGateway/
│   │   └── Fundoo.ApiGateway/
│   ├── BuildingBlocks/
│   │   ├── Shared.Contracts/
│   │   └── Shared.Infrastructure/
│   └── Services/
│       ├── UserService/
│       ├── NotesService/
│       ├── LabelService/
│       ├── ReminderService/
│       └── CollaborationService/
├── tests/
├── docker-compose.yml
└── FundooNotesMicroservices.sln
```

This structure follows Clean Architecture and keeps each microservice consistent.[4]

## Step-by-step build plan

### Step 1: Create the solution

```bash
mkdir FundooNotesMicroservices
cd FundooNotesMicroservices
dotnet new sln -n FundooNotesMicroservices
```

### Step 2: Create projects

Create shared libraries, gateway project, five service groups, and test projects using `dotnet new webapi`, `dotnet new classlib`, and `dotnet new xunit`.[1]

### Step 3: Add project references

Use this rule everywhere:

- Application -> Domain
- Infrastructure -> Domain + Application
- API -> Application + Infrastructure
- Tests -> target project under test

This keeps dependencies pointing inward, which is one of the main Clean Architecture rules.[4]

### Step 4: Install packages

Use these package groups as needed:

- Core: MediatR, EF Core SQL Server, EF Core Design, JWT Bearer, Swagger, Serilog
- Dapr: Dapr.AspNetCore, Dapr.Client, Dapr.Workflow
- Notes caching: StackExchangeRedis
- Gateway: Ocelot
- User password hashing: BCrypt.Net-Next
- Tests: Moq, FluentAssertions

The Dapr SDK packages enable client APIs and ASP.NET Core integration, while Ocelot is configured separately in the gateway service.[2][1]

## Dapr setup

### Install Dapr CLI

Install the Dapr CLI and verify it with:

```bash
dapr --version
```

The Dapr CLI is the standard tool for initializing local Dapr environments and running services with sidecars.[8][9]

### Initialize Dapr

```bash
dapr init
```

`dapr init` prepares a self-hosted local environment for Dapr development.[8]

### Create Dapr component files

Add these files under `components/`:

#### `statestore.yaml`
Use a Redis-backed state store for lightweight application state like note drafts.[7]

#### `pubsub.yaml`
Use a Redis-backed pub/sub component for local events such as `user.registered` and `note.created`.[7]

#### `workflowstatestore.yaml`
Use a Redis-backed state store for workflow durability.[7]

## Where Dapr fits in the project

Use Dapr this way:

| Dapr Feature | Use in Fundoo Notes |
|---|---|
| Service Invocation | Collaboration Service calls Notes Service |
| Pub/Sub | User registration and note creation events |
| State Management | Save and retrieve note drafts |
| Workflow | Long-running reminder processing |

Keep EF Core + SQL Server for main business tables and keep LINQ for relational querying.[5][1]

## Service implementation summary

### User Service

Responsibilities:
- Register user
- Login user
- Generate JWT
- Publish `user.registered`

When a user registers, the service stores the user in SQL Server and publishes a Dapr event for other services to react to.[3][1]

### Notes Service

Responsibilities:
- Create and query notes
- Save note drafts using Dapr State Management
- Subscribe to `user.registered`
- Publish `note.created`

The Notes Service is a good place to demonstrate both EF Core and Dapr together, because note records are relational data while drafts are lightweight state data.[10][1]

### Label Service

Responsibilities:
- Create labels
- Assign labels to notes
- Remove labels from notes

This service is mostly standard relational CRUD using EF Core and LINQ.[5]

### Reminder Service

Responsibilities:
- Create and update reminders
- Start a workflow when a reminder is created
- Execute reminder processing later through a workflow activity

This service shows how Dapr Workflow supports durable, time-based business processes.[11][12]

### Collaboration Service

Responsibilities:
- Add collaborator
- Remove collaborator
- Validate note existence through Dapr Service Invocation

This is the clearest example of one service directly calling another through Dapr’s app-id based invocation model.[7][1]

## Ocelot Gateway setup

Ocelot acts as the single public entry point and routes requests from paths like `/gateway/auth/...` or `/gateway/notes/...` to downstream services.[2]

Typical route groups include:
- auth
- notes
- drafts
- labels
- reminders
- reminder workflow
- collaborators

In ASP.NET Core, Ocelot is configured by adding Ocelot configuration, registering Ocelot services, and awaiting `UseOcelot()` in the application pipeline.[2]

## Database migrations

Each service that owns a DbContext should have its own migrations.[5]

Example pattern:

```bash
dotnet ef migrations add InitialCreate \
  --project src/Services/UserService/Fundoo.UserService.Infrastructure \
  --startup-project src/Services/UserService/Fundoo.UserService.API
```

```bash
dotnet ef database update \
  --project src/Services/UserService/Fundoo.UserService.Infrastructure \
  --startup-project src/Services/UserService/Fundoo.UserService.API
```

When the DbContext is in one project and the executable startup is in another, EF Core supports this through `--project` and `--startup-project`.[13][5]

## Local run commands with Dapr

Run each service with the Dapr CLI so the service and sidecar start together.[14]

Example:

```bash
dapr run \
  --app-id user-service \
  --app-port 5001 \
  --dapr-http-port 3501 \
  --resources-path ./components \
  -- dotnet run --project ./src/Services/UserService/Fundoo.UserService.API --urls http://localhost:5001
```

Repeat with different app IDs and ports for:
- notes-service
- label-service
- reminder-service
- collaboration-service

## Docker Compose model

A self-hosted Dapr Docker Compose setup uses:
- SQL Server
- Redis
- one container per microservice
- one Dapr sidecar per microservice
- Dapr placement
- Dapr scheduler
- API Gateway

Dapr’s self-hosted Docker Compose guidance uses the sidecar pattern with a separate `daprd` container for each application service.[7]

## Testing approach

Use xUnit + Moq to test:
- handlers
- controller responses
- repository interaction
- Dapr-enabled behavior through mocked dependencies

This keeps unit tests fast because they do not need real SQL Server, Redis, or Dapr runtime instances.[6][15]

## Recommended execution order

1. Create solution and projects
2. Add references
3. Install packages
4. Add source code
5. Create Dapr component files
6. Install tools
7. Run `dapr init`
8. Run migrations
9. Build solution
10. Run tests
11. Start infrastructure
12. Start services with Dapr
13. Start Ocelot gateway
14. Test APIs and workflows

This order reduces confusion because each dependency is ready before it is used.[8][2][5]

## End-to-end testing flow

### Register user
- User is saved in SQL Server
- `user.registered` is published
- Notes Service creates a welcome note

### Login
- JWT token is returned

### Create note
- Note is stored in SQL Server
- `note.created` is published

### Save draft
- Draft is stored through Dapr state management

### Add label
- Label and note-label mapping are stored relationally

### Add collaborator
- Collaboration Service validates the note by invoking Notes Service through Dapr
- Collaborator is stored in SQL Server

### Create reminder
- Reminder is stored in SQL Server
- Workflow is started and processed later

This flow proves that API Gateway routing, relational persistence, sidecar-based service invocation, pub/sub, state management, and workflow are all working together.[2][7][1]

## Final learning outcomes

By completing this project, a beginner should be able to explain:

- What a microservice is
- Why Clean Architecture matters
- How CQRS separates writes and reads
- How LINQ works with EF Core
- How JWT secures APIs
- How Ocelot routes external requests
- How Dapr sidecars add invocation, pub/sub, state, and workflow
- How Docker Compose runs a multi-service system
- How xUnit + Moq help test application logic

Dapr University and the Dapr learning site provide a useful follow-up path after this project if you want to deepen your understanding of distributed applications.[11][3]

## Final mentor advice

Do not try to perfect every service at once.

Build one service well, understand the pattern, and then repeat it with discipline across the rest of the system. That is the safest way to learn microservices and distributed application design without getting overwhelmed.[4][3]