import { Terminal, Cpu, Layers, GitBranch, Network } from 'lucide-react';

export type TabType = 'algorithms' | 'solid' | 'patterns' | 'system-design';

interface HeaderProps {
  activeTab: TabType;
  onTabChange: (tab: TabType) => void;
  backendOnline: boolean;
}

export const Header: React.FC<HeaderProps> = ({ activeTab, onTabChange, backendOnline }) => {
  return (
    <header style={{ borderBottom: '1px solid var(--border-color)', background: 'var(--bg-secondary)', position: 'sticky', top: 0, zIndex: 50 }}>
      <div className="container" style={{ display: 'flex', alignItems: 'center', justifyContent: 'space-between', height: '4.25rem' }}>
        <div style={{ display: 'flex', alignItems: 'center', gap: '0.85rem' }}>
          <div style={{
            background: 'linear-gradient(135deg, #2563eb, #7c3aed)',
            padding: '0.55rem',
            borderRadius: '10px',
            display: 'flex',
            alignItems: 'center',
            justifyContent: 'center',
            color: 'white',
            boxShadow: '0 4px 12px rgba(37, 99, 235, 0.3)'
          }}>
            <Terminal size={22} />
          </div>
          <div>
            <h1 style={{ fontSize: '1.15rem', fontWeight: 700, letterSpacing: '-0.025em', display: 'flex', alignItems: 'center', gap: '0.5rem' }}>
              Software Engineering Lab
              <span className="badge badge-indigo" style={{ fontSize: '0.65rem' }}>PUBLIC LAB</span>
            </h1>
            <p style={{ fontSize: '0.75rem', color: 'var(--text-muted)' }}>
              Interactive Engineering Studies • C# .NET 10 • React • TypeScript
            </p>
          </div>
        </div>

        {/* Navigation Tabs */}
        <nav style={{ display: 'flex', gap: '0.35rem' }}>
          <button
            onClick={() => onTabChange('algorithms')}
            className={`btn ${activeTab === 'algorithms' ? 'btn-primary' : 'btn-secondary'}`}
          >
            <Cpu size={16} />
            Algorithms & DSA
          </button>
          <button
            onClick={() => onTabChange('solid')}
            className={`btn ${activeTab === 'solid' ? 'btn-primary' : 'btn-secondary'}`}
          >
            <Layers size={16} />
            SOLID
          </button>
          <button
            onClick={() => onTabChange('patterns')}
            className={`btn ${activeTab === 'patterns' ? 'btn-primary' : 'btn-secondary'}`}
          >
            <GitBranch size={16} />
            Design Patterns
          </button>
          <button
            onClick={() => onTabChange('system-design')}
            className={`btn ${activeTab === 'system-design' ? 'btn-primary' : 'btn-secondary'}`}
          >
            <Network size={16} />
            System Design
          </button>
        </nav>

        {/* Backend Connectivity Status */}
        <div style={{ display: 'flex', alignItems: 'center', gap: '0.5rem', fontSize: '0.75rem' }}>
          <span style={{
            display: 'inline-block',
            width: '8px',
            height: '8px',
            borderRadius: '50%',
            backgroundColor: backendOnline ? 'var(--accent-emerald)' : 'var(--accent-amber)',
          }} className={backendOnline ? '' : 'pulse-glow'} />
          <span style={{ color: 'var(--text-muted)' }}>
            API: <strong style={{ color: backendOnline ? 'var(--accent-emerald)' : 'var(--accent-amber)' }}>
              {backendOnline ? 'Online (C# .NET 10)' : 'Client Simulation'}
            </strong>
          </span>
        </div>
      </div>
    </header>
  );
};
