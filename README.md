# SQLite Entity Framework Database Generator

Description:

This project aims to fill a gap that exists between Entity Framework (EF) and SQLite. Currently SQLite can be used with Entity Framework in C# however the functionality is limited and does not allow you to adopt a Code First or Database First approach as you cannot generate one from the other. This project aims to accomodate the Code First approach and use Reflection to initially generate a database from an assembly file. Beyond this, the project will take on the ability to migrate a sqlite database from an existing state to the desired state without loss of data.

How it will work:

The project includes custom attributes which will accompany the existing EF attributes to identify the classes that should be generated, properties to be included and foreign keys. These attributes should be used in the desired project. Once the project is built, the assembly file path will be passed to the program and a script generated to create the database and/or to carry out a migration on a database.

Current state:

The project can currently generate a blank database script but no foreign key constraints. Foreign key constraints are nearly completed but not integrated in the script creation. Following this, functionality will be added to ensure table dependencies do not cause issues in database generation.

Project Breakdown:
*	SqliteEFScriptGenerator.Core
	*	This will be the core package that can be used to generate the scripts
*	SqliteEFScriptGenerator
	*	A console project for testing the Core library
*	SQLiteTestAssembly
	*	A basic assembly to test the Core library
