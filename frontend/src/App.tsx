import { useState, useEffect } from 'react';
import { Header, TabType } from './components/Header';
import { AlgorithmsPage } from './pages/AlgorithmsPage';
import { SolidPage } from './pages/SolidPage';
import { DesignPatternsPage } from './pages/DesignPatternsPage';
import { SystemDesignPage } from './pages/SystemDesignPage';
import { checkBackendHealth } from './services/api';

export function App() {
  const [activeTab, setActiveTab] = useState<TabType>('algorithms');
  const [backendOnline, setBackendOnline] = useState<boolean>(false);

  useEffect(() => {
    // Initial health check and periodic ping
    checkBackendHealth().then(setBackendOnline);
    const interval = setInterval(() => {
      checkBackendHealth().then(setBackendOnline);
    }, 4000);
    return () => clearInterval(interval);
  }, []);

  return (
    <div style={{ minHeight: '100vh', display: 'flex', flexDirection: 'column' }}>
      <Header
        activeTab={activeTab}
        onTabChange={setActiveTab}
        backendOnline={backendOnline}
      />

      <main className="container" style={{ padding: '2rem 1.5rem', flex: 1 }}>
        {activeTab === 'algorithms' && <AlgorithmsPage />}
        {activeTab === 'solid' && <SolidPage />}
        {activeTab === 'patterns' && <DesignPatternsPage />}
        {activeTab === 'system-design' && <SystemDesignPage />}
      </main>

      <footer style={{
        borderTop: '1px solid var(--border-color)',
        padding: '1.5rem 0',
        background: 'var(--bg-secondary)',
        marginTop: 'auto',
      }}>
        <div className="container" style={{
          display: 'flex',
          justifyContent: 'space-between',
          alignItems: 'center',
          flexWrap: 'wrap',
          gap: '1rem',
          fontSize: '0.8rem',
          color: 'var(--text-faint)',
        }}>
          <div>
            <strong>SoftwareEngineeringLab</strong> — Non-commercial Public Technical Studies & Architecture Demonstrations.
          </div>
          <div style={{ display: 'flex', gap: '0.75rem', alignItems: 'center' }}>
            <span className="badge badge-blue">.NET 10</span>
            <span className="badge badge-indigo">React 19</span>
            <span className="badge badge-emerald">TypeScript 5</span>
            <span className="badge badge-amber">Vite</span>
          </div>
        </div>
      </footer>
    </div>
  );
}

export default App;
