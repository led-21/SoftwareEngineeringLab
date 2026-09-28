import React, { useState, useEffect } from 'react';
import { consumeRateLimitToken, shortenUrl } from '../services/api';
import { MermaidViewer } from '../components/MermaidViewer';
import { ShieldAlert, Link, Database, Server, RefreshCw, Zap, AlertTriangle } from 'lucide-react';

export const SystemDesignPage: React.FC = () => {
  const [activeStudy, setActiveStudy] = useState<'rate-limiter' | 'url-shortener' | 'lru-cache' | 'consistent-hashing' | 'load-balancer'>('rate-limiter');

  // ---------- 1. RATE LIMITER STATE ----------
  const [rlTokens, setRlTokens] = useState<number>(10);
  const [rlCapacity] = useState<number>(10);
  const [rlRequests, setRlRequests] = useState<{ id: number; time: string; allowed: boolean }[]>([]);

  // Simulation refill timer
  useEffect(() => {
    const interval = setInterval(() => {
      setRlTokens((prev) => Math.min(rlCapacity, Number((prev + 0.5).toFixed(1))));
    }, 500);
    return () => clearInterval(interval);
  }, [rlCapacity]);

  const handleFireRequest = async () => {
    const res = await consumeRateLimitToken();
    setRlTokens(res.remainingTokens);
    setRlRequests((prev) => [
      { id: Date.now(), time: new Date().toLocaleTimeString(), allowed: res.allowed },
      ...prev.slice(0, 7),
    ]);
  };

  const handleBurstRequests = async () => {
    for (let i = 0; i < 5; i++) {
      await handleFireRequest();
      await new Promise((r) => setTimeout(r, 60));
    }
  };

  // ---------- 2. URL SHORTENER STATE ----------
  const [longUrlInput, setLongUrlInput] = useState<string>('https://github.com/microsoft/dotnet');
  const [shortUrlResult, setShortUrlResult] = useState<{ shortCode: string; shortUrl: string } | null>(null);

  const handleShorten = async () => {
    if (!longUrlInput.trim()) return;
    const res = await shortenUrl(longUrlInput);
    setShortUrlResult(res);
  };

  // ---------- 3. LRU CACHE STATE ----------
  const [lruCapacity] = useState<number>(4);
  const [lruItems, setLruItems] = useState<{ key: string; val: string }[]>([
    { key: 'user:101', val: 'Alice' },
    { key: 'user:102', val: 'Bob' },
    { key: 'user:103', val: 'Charlie' },
  ]);
  const [lruKeyInput, setLruKeyInput] = useState('user:104');
  const [lruValInput, setLruValInput] = useState('David');

  const handleLruPut = () => {
    if (!lruKeyInput.trim()) return;
    const existingIndex = lruItems.findIndex((item) => item.key === lruKeyInput);
    let updated = [...lruItems];

    if (existingIndex !== -1) {
      updated.splice(existingIndex, 1);
    } else if (updated.length >= lruCapacity) {
      updated.pop(); // Evict LRU (tail)
    }

    updated = [{ key: lruKeyInput, val: lruValInput }, ...updated];
    setLruItems(updated);
  };

  const handleLruGet = (key: string) => {
    const existingIndex = lruItems.findIndex((item) => item.key === key);
    if (existingIndex !== -1) {
      const item = lruItems[existingIndex];
      const updated = [item, ...lruItems.filter((_, idx) => idx !== existingIndex)];
      setLruItems(updated);
    }
  };

  // ---------- 4. CONSISTENT HASHING RING ----------
  const [ringNodes, setRingNodes] = useState<string[]>(['Server-A', 'Server-B', 'Server-C']);
  const [keyToRoute, setKeyToRoute] = useState<string>('order#8921');
  const [routedNode, setRoutedNode] = useState<string>('Server-B');

  const handleRouteKey = () => {
    if (ringNodes.length === 0) return;
    // Simple deterministic ring hash simulation
    let hash = 0;
    for (let i = 0; i < keyToRoute.length; i++) hash = (hash * 31 + keyToRoute.charCodeAt(i)) >>> 0;
    const assigned = ringNodes[hash % ringNodes.length];
    setRoutedNode(assigned);
  };

  const handleAddRingNode = () => {
    const nextName = `Server-${String.fromCharCode(65 + ringNodes.length)}`;
    setRingNodes([...ringNodes, nextName]);
  };

  const handleRemoveRingNode = () => {
    if (ringNodes.length > 1) {
      setRingNodes(ringNodes.slice(0, ringNodes.length - 1));
    }
  };

  // ---------- 5. LOAD BALANCER ----------
  const [lbServers] = useState<string[]>(['10.0.0.1 (Web-01)', '10.0.0.2 (Web-02)', '10.0.0.3 (Web-03)']);
  const [lbCurrentIndex, setLbCurrentIndex] = useState<number>(0);
  const [lbLog, setLbLog] = useState<string[]>([]);

  const handleLbNext = () => {
    const chosen = lbServers[lbCurrentIndex];
    setLbCurrentIndex((lbCurrentIndex + 1) % lbServers.length);
    setLbLog((prev) => [`Request dispatched to ${chosen}`, ...prev.slice(0, 5)]);
  };

  return (
    <div style={{ display: 'flex', flexDirection: 'column', gap: '1.5rem' }}>
      {/* Notice Banner */}
      <div style={{
        padding: '0.75rem 1rem',
        borderRadius: '8px',
        background: 'rgba(251, 191, 36, 0.08)',
        border: '1px solid rgba(251, 191, 36, 0.25)',
        display: 'flex',
        alignItems: 'center',
        gap: '0.75rem',
      }}>
        <AlertTriangle size={18} color="var(--accent-amber)" />
        <span style={{ fontSize: '0.85rem', color: '#fcd34d' }}>
          <strong>Educational Architecture Lab:</strong> These models demonstrate core algorithmic principles (Token Bucket, Consistent Hash Rings, Cache Eviction). They are pedagogical studies, not production-ready distributed microservices.
        </span>
      </div>

      {/* Sub Tabs */}
      <div style={{ display: 'flex', gap: '0.5rem', borderBottom: '1px solid var(--border-color)', paddingBottom: '0.75rem', flexWrap: 'wrap' }}>
        <button
          onClick={() => setActiveStudy('rate-limiter')}
          className={`btn ${activeStudy === 'rate-limiter' ? 'btn-primary' : 'btn-secondary'}`}
        >
          <ShieldAlert size={15} /> Rate Limiter (Token Bucket)
        </button>
        <button
          onClick={() => setActiveStudy('url-shortener')}
          className={`btn ${activeStudy === 'url-shortener' ? 'btn-primary' : 'btn-secondary'}`}
        >
          <Link size={15} /> URL Shortener
        </button>
        <button
          onClick={() => setActiveStudy('lru-cache')}
          className={`btn ${activeStudy === 'lru-cache' ? 'btn-primary' : 'btn-secondary'}`}
        >
          <Database size={15} /> LRU Cache
        </button>
        <button
          onClick={() => setActiveStudy('consistent-hashing')}
          className={`btn ${activeStudy === 'consistent-hashing' ? 'btn-primary' : 'btn-secondary'}`}
        >
          <RefreshCw size={15} /> Consistent Hashing
        </button>
        <button
          onClick={() => setActiveStudy('load-balancer')}
          className={`btn ${activeStudy === 'load-balancer' ? 'btn-primary' : 'btn-secondary'}`}
        >
          <Server size={15} /> Load Balancer
        </button>
      </div>

      {/* 1. RATE LIMITER SIMULATION */}
      {activeStudy === 'rate-limiter' && (
        <div style={{ display: 'grid', gridTemplateColumns: 'repeat(auto-fit, minmax(380px, 1fr))', gap: '1.5rem' }}>
          <div className="card" style={{ display: 'flex', flexDirection: 'column', gap: '1.25rem' }}>
            <div>
              <h3 style={{ fontSize: '1.2rem', fontWeight: 600 }}>Token Bucket Simulator</h3>
              <p style={{ color: 'var(--text-muted)', fontSize: '0.85rem' }}>
                Tokens replenish at 2/sec up to capacity 10. When a request arrives, 1 token is consumed. If bucket is empty, request receives HTTP 429.
              </p>
            </div>

            {/* Token Bucket Visual Gauge */}
            <div style={{
              background: 'var(--bg-secondary)',
              padding: '1.5rem',
              borderRadius: '8px',
              border: '1px solid var(--border-color)',
              display: 'flex',
              flexDirection: 'column',
              gap: '0.75rem',
            }}>
              <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
                <span style={{ fontWeight: 600, fontSize: '0.9rem' }}>Bucket Level:</span>
                <span className="badge badge-blue">{rlTokens} / {rlCapacity} Tokens</span>
              </div>
              <div style={{
                height: '18px',
                background: 'var(--bg-tertiary)',
                borderRadius: '9999px',
                overflow: 'hidden',
                border: '1px solid var(--border-color)',
              }}>
                <div style={{
                  height: '100%',
                  width: `${(rlTokens / rlCapacity) * 100}%`,
                  background: rlTokens > 3 ? 'var(--accent-blue)' : rlTokens > 0 ? 'var(--accent-amber)' : 'var(--accent-rose)',
                  transition: 'width 0.2s ease, background-color 0.2s ease',
                }} />
              </div>
            </div>

            {/* Fire Controls */}
            <div style={{ display: 'flex', gap: '0.75rem' }}>
              <button onClick={handleFireRequest} className="btn btn-primary" style={{ flex: 1 }}>
                <Zap size={15} /> Send 1 Request
              </button>
              <button onClick={handleBurstRequests} className="btn btn-secondary" style={{ flex: 1 }}>
                🔥 Burst (5 Rapid)
              </button>
            </div>

            {/* Request Log */}
            <div>
              <h4 style={{ fontSize: '0.8rem', color: 'var(--text-faint)', textTransform: 'uppercase', marginBottom: '0.5rem' }}>
                Live Ingress Log
              </h4>
              <div style={{ display: 'flex', flexDirection: 'column', gap: '0.35rem', minHeight: '120px' }}>
                {rlRequests.map((req) => (
                  <div
                    key={req.id}
                    style={{
                      display: 'flex',
                      justifyContent: 'space-between',
                      alignItems: 'center',
                      padding: '0.4rem 0.75rem',
                      borderRadius: '6px',
                      background: req.allowed ? 'rgba(52, 211, 153, 0.08)' : 'rgba(248, 113, 113, 0.08)',
                      border: `1px solid ${req.allowed ? 'rgba(52, 211, 153, 0.2)' : 'rgba(248, 113, 113, 0.2)'}`,
                      fontSize: '0.8rem',
                    }}
                  >
                    <span>{req.time} — Inbound API Request</span>
                    <span className={`badge ${req.allowed ? 'badge-emerald' : 'badge-rose'}`}>
                      {req.allowed ? '200 OK (Allowed)' : '429 Throttled'}
                    </span>
                  </div>
                ))}
              </div>
            </div>
          </div>

          {/* Architecture Diagram */}
          <div className="card" style={{ display: 'flex', flexDirection: 'column', gap: '1rem' }}>
            <h3 style={{ fontSize: '1.1rem', fontWeight: 600 }}>Architecture Flow (Mermaid)</h3>
            <MermaidViewer chart={`flowchart TD\n    Client([Client HTTP Request]) --> Limiter{Has Tokens?}\n    Limiter -- Yes --> Deduct[Consume 1 Token] --> Gateway[200 OK -> Upstream API]\n    Limiter -- No --> Drop[429 Too Many Requests]\n    RefillClock((Refill Timer +2/s)) -. Adds Tokens .-> Limiter`} />
          </div>
        </div>
      )}

      {/* 2. URL SHORTENER */}
      {activeStudy === 'url-shortener' && (
        <div style={{ display: 'grid', gridTemplateColumns: 'repeat(auto-fit, minmax(380px, 1fr))', gap: '1.5rem' }}>
          <div className="card" style={{ display: 'flex', flexDirection: 'column', gap: '1.25rem' }}>
            <div>
              <h3 style={{ fontSize: '1.2rem', fontWeight: 600 }}>Base62 URL Shortener</h3>
              <p style={{ color: 'var(--text-muted)', fontSize: '0.85rem' }}>
                Converts long web addresses into high-density 7-character Base62 keys with collision detection.
              </p>
            </div>

            <div style={{ display: 'flex', flexDirection: 'column', gap: '0.75rem' }}>
              <label style={{ fontSize: '0.85rem', color: 'var(--text-muted)' }}>Long URL to Encode:</label>
              <input
                type="text"
                className="input-field"
                value={longUrlInput}
                onChange={(e) => setLongUrlInput(e.target.value)}
              />
              <button onClick={handleShorten} className="btn btn-primary" style={{ alignSelf: 'flex-start' }}>
                Generate Short Link
              </button>
            </div>

            {shortUrlResult && (
              <div style={{
                background: 'var(--bg-secondary)',
                padding: '1.25rem',
                borderRadius: '8px',
                border: '1px solid var(--accent-blue)',
                display: 'flex',
                flexDirection: 'column',
                gap: '0.5rem',
              }}>
                <span style={{ fontSize: '0.75rem', color: 'var(--text-faint)', textTransform: 'uppercase' }}>
                  Generated Short Link:
                </span>
                <div style={{ fontSize: '1.1rem', fontWeight: 700, color: 'var(--accent-blue)' }}>
                  {shortUrlResult.shortUrl}
                </div>
                <div style={{ fontSize: '0.8rem', color: 'var(--text-muted)' }}>
                  Code: <code className="badge badge-indigo">{shortUrlResult.shortCode}</code> (7 chars base62)
                </div>
              </div>
            )}
          </div>

          <div className="card" style={{ display: 'flex', flexDirection: 'column', gap: '1rem' }}>
            <h3 style={{ fontSize: '1.1rem', fontWeight: 600 }}>System Diagram</h3>
            <MermaidViewer chart={`flowchart LR\n    URL[Long URL] --> Hash[SHA256 Hash]\n    Hash --> Base62[Base62 Encode 7-chars]\n    Base62 --> Check{Collision?}\n    Check -- Yes --> Salt[Append Salt & Retry]\n    Check -- No --> Store[(Key-Value Store)]\n    Store --> Link[https://sel.dev/code]`} />
          </div>
        </div>
      )}

      {/* 3. LRU CACHE */}
      {activeStudy === 'lru-cache' && (
        <div style={{ display: 'grid', gridTemplateColumns: 'repeat(auto-fit, minmax(380px, 1fr))', gap: '1.5rem' }}>
          <div className="card" style={{ display: 'flex', flexDirection: 'column', gap: '1.25rem' }}>
            <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
              <div>
                <h3 style={{ fontSize: '1.2rem', fontWeight: 600 }}>LRU Cache Visualizer</h3>
                <p style={{ color: 'var(--text-muted)', fontSize: '0.85rem' }}>Capacity: {lruCapacity} items. Click an item to simulate GET (promotes to MRU).</p>
              </div>
              <span className="badge badge-emerald">O(1) Get / Put</span>
            </div>

            {/* Put Item Input */}
            <div style={{ display: 'flex', gap: '0.5rem', alignItems: 'center' }}>
              <input
                type="text"
                placeholder="Key"
                className="input-field"
                style={{ width: '110px' }}
                value={lruKeyInput}
                onChange={(e) => setLruKeyInput(e.target.value)}
              />
              <input
                type="text"
                placeholder="Value"
                className="input-field"
                style={{ width: '110px' }}
                value={lruValInput}
                onChange={(e) => setLruValInput(e.target.value)}
              />
              <button onClick={handleLruPut} className="btn btn-primary">Put Item</button>
            </div>

            {/* Cache Slots Container */}
            <div style={{
              display: 'flex',
              flexDirection: 'column',
              gap: '0.65rem',
              padding: '1.25rem',
              background: 'var(--bg-secondary)',
              borderRadius: '8px',
              border: '1px solid var(--border-color)',
            }}>
              <div style={{ display: 'flex', justifyContent: 'space-between', fontSize: '0.7rem', color: 'var(--text-faint)' }}>
                <span>▲ MOST RECENTLY USED (MRU - Front)</span>
                <span>LEAST RECENTLY USED (LRU - Tail) ▼</span>
              </div>

              {lruItems.map((item, idx) => (
                <div
                  key={item.key}
                  onClick={() => handleLruGet(item.key)}
                  style={{
                    display: 'flex',
                    justifyContent: 'space-between',
                    alignItems: 'center',
                    padding: '0.65rem 1rem',
                    borderRadius: '6px',
                    background: idx === 0 ? 'rgba(56, 189, 248, 0.15)' : 'var(--bg-tertiary)',
                    border: idx === 0 ? '1px solid var(--accent-blue)' : '1px solid var(--border-color)',
                    cursor: 'pointer',
                    transition: 'all 0.15s ease',
                  }}
                  title="Click to access (Promote to MRU)"
                >
                  <div style={{ display: 'flex', gap: '0.75rem', alignItems: 'center' }}>
                    <span className="badge badge-indigo">#{idx + 1}</span>
                    <strong style={{ color: 'var(--text-main)' }}>{item.key}</strong>
                  </div>
                  <div style={{ display: 'flex', gap: '0.5rem', alignItems: 'center' }}>
                    <span style={{ color: 'var(--text-muted)', fontSize: '0.85rem' }}>{item.val}</span>
                    {idx === 0 && <span className="badge badge-blue">MRU</span>}
                    {idx === lruItems.length - 1 && <span className="badge badge-rose">Next Evict</span>}
                  </div>
                </div>
              ))}
            </div>
          </div>

          <div className="card" style={{ display: 'flex', flexDirection: 'column', gap: '1rem' }}>
            <h3 style={{ fontSize: '1.1rem', fontWeight: 600 }}>Data Structure Layout</h3>
            <MermaidViewer chart={`classDiagram\n    class LRUCache {\n        -Dictionary~Key, Node~ _map\n        -LinkedList~Item~ _lruList\n        +TryGet(key) bool\n        +Put(key, value)\n    }\n    class CacheNode {\n        +Key key\n        +Value val\n        +Node next\n        +Node prev\n    }\n    LRUCache o--> CacheNode`} />
          </div>
        </div>
      )}

      {/* 4. CONSISTENT HASHING RING */}
      {activeStudy === 'consistent-hashing' && (
        <div style={{ display: 'grid', gridTemplateColumns: 'repeat(auto-fit, minmax(380px, 1fr))', gap: '1.5rem' }}>
          <div className="card" style={{ display: 'flex', flexDirection: 'column', gap: '1.25rem' }}>
            <div>
              <h3 style={{ fontSize: '1.2rem', fontWeight: 600 }}>Consistent Hash Ring</h3>
              <p style={{ color: 'var(--text-muted)', fontSize: '0.85rem' }}>
                Servers map to circular ring slots (0 to 2³² - 1). Keys walk clockwise until they encounter the nearest server.
              </p>
            </div>

            <div style={{ display: 'flex', gap: '0.5rem', alignItems: 'center' }}>
              <input
                type="text"
                className="input-field"
                value={keyToRoute}
                onChange={(e) => setKeyToRoute(e.target.value)}
                placeholder="Key to route"
              />
              <button onClick={handleRouteKey} className="btn btn-primary">Route Key</button>
              <button onClick={handleAddRingNode} className="btn btn-secondary">+ Node</button>
              <button onClick={handleRemoveRingNode} className="btn btn-danger">- Node</button>
            </div>

            {/* Circular Ring SVG Presentation */}
            <div style={{
              display: 'flex',
              flexDirection: 'column',
              alignItems: 'center',
              padding: '1.5rem',
              background: 'var(--bg-secondary)',
              borderRadius: '8px',
              border: '1px solid var(--border-color)',
            }}>
              <svg width="240" height="240" viewBox="0 0 240 240">
                <circle cx="120" cy="120" r="90" fill="none" stroke="var(--border-color)" strokeWidth="4" strokeDasharray="6 4" />
                {ringNodes.map((node, i) => {
                  const angle = (i / ringNodes.length) * 2 * Math.PI - Math.PI / 2;
                  const cx = 120 + 90 * Math.cos(angle);
                  const cy = 120 + 90 * Math.sin(angle);
                  const isAssigned = routedNode === node;

                  return (
                    <g key={node}>
                      <circle
                        cx={cx}
                        cy={cy}
                        r={isAssigned ? 14 : 10}
                        fill={isAssigned ? 'var(--accent-emerald)' : 'var(--accent-blue)'}
                        stroke="#fff"
                        strokeWidth="2"
                      />
                      <text
                        x={cx}
                        y={cy + 24}
                        fill="var(--text-main)"
                        fontSize="11"
                        fontWeight="600"
                        textAnchor="middle"
                      >
                        {node}
                      </text>
                    </g>
                  );
                })}
              </svg>

              <div style={{ marginTop: '0.5rem', textAlign: 'center' }}>
                <span className="badge badge-emerald" style={{ fontSize: '0.9rem', padding: '0.3rem 0.8rem' }}>
                  Key "{keyToRoute}" ➜ Handled by: <strong>{routedNode}</strong>
                </span>
              </div>
            </div>
          </div>

          <div className="card" style={{ display: 'flex', flexDirection: 'column', gap: '1rem' }}>
            <h3 style={{ fontSize: '1.1rem', fontWeight: 600 }}>Topology</h3>
            <MermaidViewer chart={`flowchart LR\n    Key[Key: order#8921] --> Hash[FNV-1a 32-bit Hash]\n    Hash --> BinarySearch[Binary Search Ring]\n    BinarySearch --> Node[Clockwise Node: Server-B]\n    Node --> DB[(Local Partition)]`} />
          </div>
        </div>
      )}

      {/* 5. LOAD BALANCER */}
      {activeStudy === 'load-balancer' && (
        <div style={{ display: 'grid', gridTemplateColumns: 'repeat(auto-fit, minmax(380px, 1fr))', gap: '1.5rem' }}>
          <div className="card" style={{ display: 'flex', flexDirection: 'column', gap: '1.25rem' }}>
            <div>
              <h3 style={{ fontSize: '1.2rem', fontWeight: 600 }}>Round Robin Load Balancer</h3>
              <p style={{ color: 'var(--text-muted)', fontSize: '0.85rem' }}>
                Sequential cycle distribution of inbound client traffic across active application backends.
              </p>
            </div>

            <button onClick={handleLbNext} className="btn btn-primary" style={{ alignSelf: 'flex-start' }}>
              <Zap size={15} /> Send Client Request
            </button>

            {/* Server Nodes */}
            <div style={{ display: 'flex', flexDirection: 'column', gap: '0.5rem' }}>
              {lbServers.map((srv, idx) => {
                const isActive = (lbCurrentIndex + lbServers.length - 1) % lbServers.length === idx && lbLog.length > 0;
                return (
                  <div
                    key={srv}
                    style={{
                      padding: '0.75rem 1rem',
                      borderRadius: '8px',
                      background: isActive ? 'rgba(52, 211, 153, 0.15)' : 'var(--bg-secondary)',
                      border: isActive ? '1px solid var(--accent-emerald)' : '1px solid var(--border-color)',
                      display: 'flex',
                      justifyContent: 'space-between',
                      alignItems: 'center',
                    }}
                  >
                    <span>{srv}</span>
                    {isActive && <span className="badge badge-emerald">Last Selected</span>}
                  </div>
                );
              })}
            </div>

            {/* Traffic Dispatch Log */}
            <div>
              <h4 style={{ fontSize: '0.8rem', color: 'var(--text-faint)', textTransform: 'uppercase', marginBottom: '0.5rem' }}>
                Dispatch Log
              </h4>
              <div style={{ display: 'flex', flexDirection: 'column', gap: '0.35rem' }}>
                {lbLog.map((entry, idx) => (
                  <div key={idx} style={{ fontSize: '0.8rem', color: 'var(--text-muted)' }}>
                    • {entry}
                  </div>
                ))}
              </div>
            </div>
          </div>

          <div className="card" style={{ display: 'flex', flexDirection: 'column', gap: '1rem' }}>
            <h3 style={{ fontSize: '1.1rem', fontWeight: 600 }}>Round Robin Flow</h3>
            <MermaidViewer chart={`sequenceDiagram\n    autonumber\n    actor Client\n    participant LB as RoundRobin LoadBalancer\n    participant S1 as Server 1\n    participant S2 as Server 2\n    participant S3 as Server 3\n    Client->>LB: Request #1\n    LB->>S1: Forward\n    Client->>LB: Request #2\n    LB->>S2: Forward\n    Client->>LB: Request #3\n    LB->>S3: Forward\n    Client->>LB: Request #4\n    LB->>S1: Wrap around`} />
          </div>
        </div>
      )}
    </div>
  );
};
