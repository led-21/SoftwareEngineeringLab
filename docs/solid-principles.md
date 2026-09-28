# SOLID Principles Study Notes

SOLID is an acronym for five design principles for maintainable, testable object-oriented software engineering.

---

## 1. Single Responsibility Principle (SRP)
> *"A class should have one, and only one, reason to change."*

- **The Antipattern**: God classes or services that manage both domain persistence and communication protocols (e.g., `UserManager` saving to DB and formatting SMTP packets).
- **The Remedy**: High cohesion. Extract disparate responsibilities into cohesive units (`IUserRepository`, `IEmailNotificationService`).

---

## 2. Open-Closed Principle (OCP)
> *"Software entities should be open for extension, but closed for modification."*

- **The Antipattern**: Long switch statements or `is` type checks (`AreaCalculator` checking `if (shape is Circle) ...`).
- **The Remedy**: Polymorphism. Code against an abstraction (`IShape.CalculateArea()`). Adding a new `Triangle` requires only authoring a new class implementing the interface, leaving tested calculations untouched.

---

## 3. Liskov Substitution Principle (LSP)
> *"Subtypes must be substitutable for their base types without altering correctness."*

- **The Antipattern**: The Rectangle-Square inheritance trap. Overriding `Width` setter in `Square` to mutate `Height` violates expectations of callers that treat the object as a regular `Rectangle`.
- **The Remedy**: Model behavior rather than taxonomic intuition. Both can implement `IGeometricShape`, but neither inherits mutating invariants from the other.

---

## 4. Interface Segregation Principle (ISP)
> *"Clients should not be forced to depend upon interfaces that they do not use."*

- **The Antipattern**: Fat interface `IWorker` containing `Work()`, `Eat()`, `Sleep()`. A `RobotWorker` is forced to throw `NotImplementedException`.
- **The Remedy**: Segregate fat interfaces into fine-grained role contracts: `IWorkable`, `IEatable`, `ISleepable`.

---

## 5. Dependency Inversion Principle (DIP)
> *"High-level modules should not depend on low-level modules. Both should depend on abstractions."*

- **The Antipattern**: High-level `NotificationService` creating a `new SmtpClient()` internally.
- **The Remedy**: Invert control. Depend on `IMessageSender` and inject concrete senders (`EmailSender`, `SmsSender`) via constructor injection.
