# ToySql

A minimal SQL query engine built from scratch in C#, focused on understanding how databases parse, plan, and execute queries.

---

## 🚀 What This Project Is

ToySql is not just a parser.

It is an evolving **query engine prototype** that models how real databases work internally:

* SQL → Tokens → AST
* AST → Logical Plan
* Logical Plan → Execution
* Execution → Results

---

## 🧠 Architecture

```text
SQL Query
   ↓
Lexer (Tokenization)
   ↓
Parser (AST निर्माण)
   ↓
AST Visitors
   ↓
Logical Plan
   ↓
Execution Engine
   ↓
Storage
```

---

## 🧩 Current Capabilities

* SQL Lexer
* SQL Parser (SELECT support)
* AST generation

Example:

```sql
SELECT id, name FROM users WHERE id = 1;
```

Produces an AST representing:

* Projection → `id, name`
* Source → `users`
* Filter → `id = 1`

---

## 🔁 Next Steps (Active Development)

### 1. Visitor Pattern (Core Focus)

* AST traversal without modifying node classes
* Planned visitors:

  * `PrintVisitor`
  * `ValidationVisitor`
  * `LogicalPlanVisitor`

---

### 2. Logical Plan

Transform AST into execution-friendly operators:

```text
Scan(users)
   ↓
Filter(id = 1)
   ↓
Projection(id, name)
```

---

### 3. Execution Engine

Iterator-based model:

```csharp
interface IOperator
{
    Row Next();
}
```

Planned operators:

* TableScan
* Filter
* Projection

---

### 4. Storage Layer

Pluggable storage abstraction:

* File-based (JSON/Binary)
* In-memory
* Future: custom engine

---

## 🧪 Long-Term Vision

* Query optimization (rule-based)
* Filter pushdown
* Index support
* `EXPLAIN` query plans
* CLI (REPL interface)

---

## 📌 Why This Project

Most developers use SQL.

Few understand:

* How queries are parsed
* How plans are generated
* How execution pipelines work

ToySql is an attempt to bridge that gap by building a query engine from first principles.

---

## 🛠 Tech Stack

* C#
* .NET
* Clean Architecture (modular design)
* Visitor Pattern (for extensibility)

---

## 📖 Inspiration

Inspired by:

* Database internals
* Compiler design patterns
* Query engines like PostgreSQL / SQLite

---

## 🤝 Contributions

This is a learning-focused project, but suggestions and improvements are welcome.
