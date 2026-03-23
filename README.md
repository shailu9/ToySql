# ToySql

A minimal, extensible SQL query engine built from scratch to explore database internals, query planning, and execution models.

---

## 🚀 Overview

ToySql is a learning-driven project that evolves into a modular query engine with components inspired by real-world databases:

* SQL parsing → AST
* Semantic validation
* Logical query planning
* Rule-based optimization
* Execution engine (Volcano model)
* Pluggable storage layer

---

## 🧠 Architecture

```
SQL
 ↓
Lexer → Tokens
 ↓
Parser → AST
 ↓
Binder → Semantic Model
 ↓
Logical Plan Builder
 ↓
Optimizer (Visitor-based)
 ↓
Execution Engine
 ↓
Storage Engine
```

---

## 🧩 Core Components

* **Lexer & Parser**
  Converts SQL into an Abstract Syntax Tree (AST)

* **AST + Visitor Pattern**
  Enables extensible operations like:

  * Printing
  * Validation
  * Plan generation

* **Logical Plan**
  Query represented as operators:

  ```
  Scan → Filter → Projection
  ```

* **Execution Engine**
  Iterator-based (Volcano model):

  ```csharp
  interface IOperator
  {
      Row Next();
  }
  ```

* **Storage Layer**
  Pluggable interface (file-based / in-memory / custom engine)

---

## 🔁 Example Flow

```sql
SELECT id, name FROM users WHERE id = 1;
```

Execution pipeline:

```
Parse → AST → Logical Plan → Optimize → Execute → Result
```

---

## 🧪 Goals

* Understand how databases work internally
* Implement clean architecture for query engines
* Explore Visitor pattern in real systems
* Build a foundation for future extensions:

  * Indexing
  * Query optimization
  * Distributed execution

---

## 📌 Current Status

* [x] Basic Lexer
* [x] SQL Parser (SELECT)
* [ ] AST Visitors
* [ ] Logical Plan
* [ ] Execution Engine
* [ ] Storage Integration
* [ ] CLI (REPL)

---

## 🛣️ Roadmap

* AST → Logical Plan transformation
* Rule-based optimizer (filter pushdown)
* Execution engine with operators
* Integration with custom storage engine
* `EXPLAIN` query support

---

## 🧱 Tech Stack

* C# (.NET)
* Clean architecture principles
* Visitor pattern for extensibility

---

## 📖 Why This Project?

Most developers use databases. Few understand how they work.

ToySql is an attempt to bridge that gap by building a query engine from first principles.

---
