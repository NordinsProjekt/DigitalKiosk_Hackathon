# Copilot Instructions

## Project Guidelines
- When any database changes happen (entity changes, EF Core configuration changes, migrations, etc.), the file DatabaseSchema.xsd at the repository root must also be updated to reflect those changes. The XSD schema is the source of truth for the database structure and must stay in sync.
- The project follows SOLID principles. All code must adhere to: Single Responsibility, Open/Closed, Liskov Substitution, Interface Segregation, and Dependency Inversion.