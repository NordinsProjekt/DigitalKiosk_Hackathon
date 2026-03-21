# Copilot Code Review Instructions

## Test Coverage Requirements

- Every PR that adds or modifies business logic in `Services/` **must** include corresponding tests in `Application.ServicesTests/`.
- Every PR that adds or modifies factory methods in `DomainFactories/` **must** include corresponding tests in `Domain.FactoriesTests/`.
- Every PR that adds or modifies controller actions in `DigitalKiosk_Hackathon/Controllers/` **must** include tests that cover the new or changed behavior.
- Entity changes in `Entities/` that affect validation or computed logic must be covered by unit tests.
- Flag any PR that introduces new public methods without accompanying test methods.

## Onion Architecture Enforcement

This project follows **Onion Architecture**. The layers from innermost to outermost are:

1. **Domain Entities** (`Entities/`) — Core business objects and enums. No dependencies on other project layers.
2. **Domain Factories** (`DomainFactories/`) — Factory pattern for creating domain aggregates. May only depend on `Entities`.
3. **Application Services** (`Services/`) — Business logic and use-case orchestration. May only depend on `Entities`. Must not reference `EF_MSSQL` or any infrastructure project directly.
4. **Infrastructure** (`EF_MSSQL/`) — EF Core DbContext, configurations, and repository implementations. May depend on `Entities`, `DomainFactories`, and `Services` (to implement interfaces defined there).
5. **Presentation** (`DigitalKiosk_Hackathon/`, `BackOfficeConsole/`) — API controllers and console entry points. May depend on all inner layers; wires up dependency injection.

### Dependency Rules — Flag violations of these rules:

- `Entities` must **never** reference any other project.
- `DomainFactories` may only reference `Entities`.
- `Services` may only reference `Entities`. It must **never** reference `EF_MSSQL` or any infrastructure/persistence concern.
- `EF_MSSQL` may reference `Entities`, `DomainFactories`, and `Services` (to implement repository interfaces).
- Presentation projects (`DigitalKiosk_Hackathon`, `BackOfficeConsole`) may reference all inner layers and register DI.
- No inner layer may reference an outer layer. The dependency arrow must always point inward.

### Repository Pattern

- Data access must be abstracted behind repository interfaces defined in the `Services` layer (or a dedicated contracts project).
- Flag any code that uses `KioskDbContext` or EF Core `DbSet` directly outside of `EF_MSSQL/`. Controllers and services must not depend on the DbContext directly.
- Repository implementations belong in `EF_MSSQL/` and must implement interfaces from the inner layers.

### Factory Pattern

- Domain object creation logic (especially for aggregates and complex entities) should go through factory methods in `DomainFactories/`.
- Flag PRs that instantiate domain entities with complex setup directly in services or controllers instead of using a factory.
- Factories must only depend on `Entities` and must not contain infrastructure concerns.

## C# Code Quality

All the above rules (test coverage, onion architecture, repository/factory patterns) apply specifically to the **C# codebase**. Ensure C# code follows established conventions and patterns described in this document.

## HTML & CSS Guidelines

- All HTML must be **semantic** — use appropriate elements (`<header>`, `<nav>`, `<main>`, `<section>`, `<article>`, `<footer>`, etc.) instead of generic `<div>` soup.
- Each HTML file in `html/` **must** have its own dedicated CSS file in `html/css/`. Flag any PR where a single CSS file is shared across multiple HTML pages or where inline styles are used extensively.
- CSS class names should be descriptive and follow a consistent naming convention.

## JavaScript Guidelines

- JavaScript files in `html/js/` must follow **clean code** principles:
  - Use descriptive, intention-revealing variable and function names.
  - Keep functions small and focused on a single responsibility.
  - Avoid deep nesting — extract logic into well-named helper functions.
  - No magic numbers or strings — use named constants.
  - Minimize global state; prefer modular patterns.
  - Flag dead code, commented-out code blocks, and duplicated logic.

## General Review Guidance

- Ensure `DatabaseSchema.xsd` at the repository root is updated whenever entity or EF Core configuration changes occur.
- Ensure enum conversions are stored as strings in EF configurations (not integers).
- Verify that new EF entity configurations follow the existing Fluent API pattern in `EF_MSSQL/Configurations/`.
