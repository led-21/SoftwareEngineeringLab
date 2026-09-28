# System Design Building Blocks Study Notes

This guide provides conceptual analyses of core distributed systems components implemented in `SoftwareEngineeringLab.Core.SystemDesign`.

---

## 1. Token Bucket Rate Limiter

### Algorithm
- A bucket holds up to $C$ tokens and refills at rate $R$ tokens per second.
- Inbound requests consume 1 or more tokens.
- If sufficient tokens are available, the request is allowed; otherwise, it is rejected (HTTP 429).

### Precision Time Tracking & Starvation Prevention
In naive implementations:
```csharp
// ❌ Starvation Bug: If polled every 50ms, elapsed seconds < 1, tokensToAdd truncates to 0!
var elapsed = (int)(now - last).TotalSeconds;
tokens += elapsed * rate;
last = now; // Tokens never accumulate!
```

**Fixed Formulation**:
```csharp
//  Accurate fractional accumulation
var elapsedSeconds = (now - _lastRefillUtc).TotalSeconds;
_tokens = Math.Min(_capacity, _tokens + elapsedSeconds * _refillRatePerSecond);
_lastRefillUtc = now;
```

---

## 2. Consistent Hashing

### Problem
Standard hashing ($hash(key) \pmod N$) forces nearly 100% of keys to be remapped when the number of nodes $N$ changes.

### Ring Formulation
- Map hash space to a circular ring ($0$ to $2^{32} - 1$).
- Both server nodes and data keys are hashed using **FNV-1a 32-bit** or MD5 onto this ring.
- Keys are assigned to the first node encountered clockwise on the ring.
- **Virtual Nodes**: Each physical server registers $K$ virtual replicas (e.g., `NodeA#0`, `NodeA#1`) across the ring to prevent statistical clustering and hot spots.
- **Lookup Complexity**: $O(\log(N \times K))$ via Binary Search over sorted ring keys.

---

## 3. LRU Cache (Least Recently Used)

### Architecture
- **Hash Map**: Provides $O(1)$ key-to-node lookup.
- **Doubly Linked List**: Maintains chronological order of access.
  - Front (Head): Most Recently Used (MRU).
  - Back (Tail): Least Recently Used (LRU).
- **Operations**:
  - `Get(k)`: Lookup node in map ($O(1)$), unlink from current position, prepend to head ($O(1)$).
  - `Put(k, v)`: If present, update value and move to head. If absent and count exceeds capacity, remove tail node from list and map ($O(1)$), then prepend new item to head.
