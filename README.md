# Software Engineering Lab

> A collection of interactive software engineering studies covering algorithms, design patterns, SOLID principles and system design using C#, .NET, React and TypeScript.

[![.NET 10](https://img.shields.io/badge/.NET-10.0-512BD4?logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![React 19](https://img.shields.io/badge/React-19.3-61DAFB?logo=react&logoColor=black)](https://react.dev/)
[![TypeScript 6](https://img.shields.io/badge/TypeScript-6.0-3178C6?logo=typescript&logoColor=white)](https://www.typescriptlang.org/)
[![Vite 8](https://img.shields.io/badge/Vite-8.3-646CFF?logo=vite&logoColor=white)](https://vitejs.dev/)
[![CI](https://github.com/led-21/SoftwareEngineeringLab/actions/workflows/ci.yml/badge.svg)](https://github.com/led-21/SoftwareEngineeringLab/actions/workflows/ci.yml)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

---

## Overview

**SoftwareEngineeringLab** is an educational, non-commercial public technical laboratory designed to explore, benchmark, and visualize foundational software engineering concepts. 

Rather than disguising trivial code behind complex enterprise layers, this laboratory prioritizes **code clarity**, **pedagogical trade-off analysis**, **mathematical algorithmic rigor**, and **interactive visual learning**.

---

## Topics

### 1. Data Structures & Algorithms (DSA)
- **Arrays**: Two Sum, Kadane's Algorithm ($O(n)$ Max Subarray), Stock Profit, In-place Array Rotation, Contains Duplicate, Product Except Self.
- **Searching**: Binary Search with real-time pointer tracing ($L, \text{MID}, R$), Rotated Array Search, Range Searching.
- **Sorting**: QuickSort (Lomuto partitioning), MergeSort (Divide & Conquer), HeapSort (In-place binary heap).
- **Linked Lists**: Single linked list reversal, Two sorted lists merge, Cycle detection (Floyd's Tortoise & Hare), $N$-th node removal from end.
- **Trees**: Binary Tree Maximum Depth, IsSameTree structural identity, Invert Tree, Inorder Traversal, BST Invariant Validation, Lowest Common Ancestor.
- **Dynamic Programming**: Space-optimized Fibonacci, Climbing Stairs combinations, House Robber ($O(1)$ space), Coin Change, Longest Increasing Subsequence (LIS).
- **Graphs**: Depth-First Search (DFS), Breadth-First Search (BFS), Number of Islands (2D Grid connected components), Course Schedule (Kahn's Topological Sort).
- **Strings**: Palindrome validation, Anagram frequency checking, Sliding Window Longest Substring, Group Anagrams.
- **Custom Data Structures**: `CustomStack<T>` (LIFO) and `CustomQueue<T>` (FIFO).

### 2. SOLID Principles
Detailed side-by-side analysis of anti-patterns vs refactored implementations:
- **S — Single Responsibility Principle (SRP)**: Segregating user domain persistence from notification dispatching.
- **O — Open-Closed Principle (OCP)**: Polymorphic shape computation vs modifying legacy conditional calculators.
- **L — Liskov Substitution Principle (LSP)**: Solving the classic Rectangle-Square invariant violation.
- **I — Interface Segregation Principle (ISP)**: Lean role interfaces (`IWorkable`, `IEatable`) vs bloated fat interfaces.
- **D — Dependency Inversion Principle (DIP)**: Abstract messaging contracts (`IMessageSender`) injected via constructor vs tight coupling to concrete SMTP clients.

### 3. Design Patterns
Comprehensive patterns documented with problem statements, Mermaid diagrams, clean C# implementations, and practical trade-offs:
- **Creational**: Singleton (canonical thread-safe `Lazy<T>`), Factory Method (Payment Provider routing), Builder (Fluent multi-step configuration).
- **Structural**: Adapter (Bridging legacy XML to modern JSON), Decorator (Transparent in-memory caching), Facade (Subsystem order fulfillment coordinator).
- **Behavioral**: Observer (Reactive pub/sub stock price ticker), Strategy (Interchangeable pricing algorithms), Command (Transactional command execution with Undo history).

### 4. System Design Studies
Independent architectural building block studies:
- **Rate Limiter**: Token Bucket algorithm with sub-millisecond fractional time tracking (preventing token starvation).
- **URL Shortener**: Base62 encoding of cryptographic hashes with automated collision resolution.
- **LRU Cache**: $O(1)$ access and eviction using a Doubly Linked List combined with a Hash Map.
- **Consistent Hashing**: Uniform ring partitioning using FNV-1a 32-bit hashing and $O(\log n)$ binary search clockwise routing.
- **Round Robin Load Balancer**: Dynamic pool management and sequential round-robin traffic dispatch.
- **Chat Queue Broker**: Concurrent user inboxes with asynchronous long-polling awaiting (`TaskCompletionSource`).

### 5. Frontend Experiments
- Interactive engineering playground built with React, TypeScript, and Vite.
- Real-time step-by-step Binary Search visualization with active pointer badges.
- Sorting bar graph showing comparisons and swaps in real time.
- Interactive Stack & Queue container with push/pop/enqueue/dequeue controls.
- Dynamic 2D grid toggle for the Number of Islands DFS algorithm.
- Interactive Token Bucket rate-limiter simulator with live token refill gauge and burst testing.
- Visual SVG circular Consistent Hash Ring with dynamic key mapping.

---

## Architecture

```text
SoftwareEngineeringLab/
├── backend/
│   ├── SoftwareEngineeringLab.slnx          # Solution file
│   ├── SoftwareEngineeringLab.Core/         # Algorithmic engine & design patterns
│   │   ├── Algorithms/                      # Arrays, Sorting, Searching, Graphs, DP, Trees, Strings
│   │   ├── DataStructures/                  # Stack, Queue, LRU Cache
│   │   ├── Solid/                           # SRP, OCP, LSP, ISP, DIP
│   │   ├── DesignPatterns/                  # Creational, Structural, Behavioral
│   │   └── SystemDesign/                    # RateLimiter, UrlShortener, LoadBalancer, ConsistentHash
│   ├── SoftwareEngineeringLab.Api/          # Minimal API endpoints with CORS
│   └── SoftwareEngineeringLab.Tests/        # 83 xUnit automated tests
│
├── frontend/                                # Engineering Playground (React + TypeScript + Vite)
│   ├── src/
│   │   ├── components/                      # Header, CodeBlock, MermaidViewer, ComplexityBadge
│   │   ├── pages/                           # AlgorithmsPage, SolidPage, PatternsPage, SystemDesignPage
│   │   └── services/                        # api.ts (backend client + local fallback simulator)
│   ├── package.json
│   └── vite.config.ts
│
├── docs/                                    # Technical deep dives and study guides
└── README.md
```

---

## Interactive Demo

The frontend functions both as a **connected client** communicating with the .NET 10 API and as an **offline simulator** (using native TypeScript fallback algorithms):

1. **Input**: Provide parameters or interact with intuitive controls (buttons, grids, sliders).
2. **Execution**: Step through or trigger algorithmic passes.
3. **Output**: Observe instant state changes, logs, or graphical updates.
4. **Complexity**: Review verified asymptotic time and space guarantees (e.g. $\text{Time: } O(\log n), \text{Space: } O(1)$).

---

## Running Locally

### Prerequisites
- [.NET SDK 10.0+](https://dotnet.microsoft.com/download)
- [Node.js 22+ & npm](https://nodejs.org/)

### 1. Run the Backend API
```bash
cd backend/SoftwareEngineeringLab.Api
dotnet run
```
The API starts at `http://localhost:5000`.

### 2. Run the Frontend Playground
In another terminal:
```bash
cd frontend
npm install
npm run dev
```
Open your browser at `http://localhost:5173`.

---

## Tests

Execute the automated test suite across all algorithms and components:

```bash
cd backend
dotnet test SoftwareEngineeringLab.slnx
```

Output:
```text
Aprovado!  – Com falha: 0, Aprovado: 83, Ignorado: 0, Total: 83 - SoftwareEngineeringLab.Tests.dll (net10.0)
```

---

## Roadmap

- [x] Complete refactoring of legacy code into clean .NET 10 modular libraries.
- [x] Comprehensive xUnit test suite (83 unit tests covering DSA and System Design).
- [x] Minimal API with CORS support.
- [x] Interactive React + TypeScript + Vite playground with dark mode and Mermaid diagrams.
- [ ] Add Trie (Prefix Tree) interactive autocomplete visualizer.
- [ ] Add Dijkstra / A* Pathfinding interactive grid.
- [ ] Add Distributed Lock (Redlock concept) educational simulation.

---

## License

This project is licensed under the [MIT License](LICENSE).
