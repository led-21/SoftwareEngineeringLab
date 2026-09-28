import React, { useEffect, useState } from 'react';
import { fetchSolidPrinciples, SolidPrinciple } from '../services/api';
import { CodeBlock } from '../components/CodeBlock';
import { AlertTriangle, Lightbulb } from 'lucide-react';

export const SolidPage: React.FC = () => {
  const [principles, setPrinciples] = useState<SolidPrinciple[]>([]);
  const [selectedLetter, setSelectedLetter] = useState<string>('S');

  useEffect(() => {
    fetchSolidPrinciples().then(setPrinciples);
  }, []);

  const currentPrinciple = principles.find((p) => p.letter === selectedLetter) || principles[0];

  return (
    <div style={{ display: 'flex', flexDirection: 'column', gap: '1.5rem' }}>
      {/* Principle Tabs */}
      <div style={{
        display: 'flex',
        gap: '0.5rem',
        borderBottom: '1px solid var(--border-color)',
        paddingBottom: '0.75rem',
        flexWrap: 'wrap',
      }}>
        {principles.map((p) => (
          <button
            key={p.letter}
            onClick={() => setSelectedLetter(p.letter)}
            className={`btn ${selectedLetter === p.letter ? 'btn-primary' : 'btn-secondary'}`}
            style={{ fontWeight: 600 }}
          >
            <span style={{
              background: selectedLetter === p.letter ? 'rgba(255, 255, 255, 0.2)' : 'var(--bg-card)',
              width: '22px',
              height: '22px',
              borderRadius: '4px',
              display: 'inline-flex',
              alignItems: 'center',
              justifyContent: 'center',
              fontSize: '0.8rem',
            }}>
              {p.letter}
            </span>
            {p.name.split(' ')[0]}
          </button>
        ))}
      </div>

      {currentPrinciple && (
        <div style={{ display: 'flex', flexDirection: 'column', gap: '1.25rem' }}>
          {/* Header Card */}
          <div className="card" style={{ display: 'flex', flexDirection: 'column', gap: '0.5rem' }}>
            <div style={{ display: 'flex', alignItems: 'center', gap: '0.75rem' }}>
              <span className="badge badge-indigo" style={{ fontSize: '1.1rem', padding: '0.3rem 0.7rem' }}>
                {currentPrinciple.letter}
              </span>
              <div>
                <h2 style={{ fontSize: '1.35rem', fontWeight: 700 }}>{currentPrinciple.name}</h2>
                <p style={{ color: 'var(--accent-blue)', fontSize: '0.95rem', fontWeight: 500 }}>
                  "{currentPrinciple.summary}"
                </p>
              </div>
            </div>

            {/* Problem explanation banner */}
            <div style={{
              marginTop: '0.75rem',
              padding: '0.85rem 1rem',
              borderRadius: '8px',
              background: 'rgba(248, 113, 113, 0.08)',
              border: '1px solid rgba(248, 113, 113, 0.25)',
              display: 'flex',
              gap: '0.75rem',
              alignItems: 'flex-start',
            }}>
              <AlertTriangle size={20} color="var(--accent-rose)" style={{ flexShrink: 0, marginTop: '2px' }} />
              <div>
                <strong style={{ color: 'var(--accent-rose)', fontSize: '0.85rem', textTransform: 'uppercase', letterSpacing: '0.05em' }}>
                  The Flaw / Design Antipattern:
                </strong>
                <p style={{ fontSize: '0.875rem', color: '#cbd5e1', marginTop: '0.2rem' }}>
                  {currentPrinciple.problemDescription}
                </p>
              </div>
            </div>
          </div>

          {/* Split Code Comparison: Bad vs Refactored */}
          <div style={{ display: 'grid', gridTemplateColumns: 'repeat(auto-fit, minmax(420px, 1fr))', gap: '1.25rem' }}>
            {/* Bad Example */}
            <div style={{ display: 'flex', flexDirection: 'column', gap: '0.5rem' }}>
              <CodeBlock
                code={currentPrinciple.badCodeSnippet}
                title="❌ Violation (Poor Design)"
                badge="Antipattern"
                badgeType="danger"
              />
            </div>

            {/* Refactored Example */}
            <div style={{ display: 'flex', flexDirection: 'column', gap: '0.5rem' }}>
              <CodeBlock
                code={currentPrinciple.goodCodeSnippet}
                title=" Refactored (SOLID Compliant)"
                badge="Best Practice"
                badgeType="success"
              />
            </div>
          </div>

          {/* Key Takeaway Card */}
          <div className="card" style={{
            background: 'linear-gradient(135deg, rgba(16, 21, 34, 0.9), rgba(24, 32, 50, 0.9))',
            display: 'flex',
            alignItems: 'center',
            gap: '1rem',
          }}>
            <div style={{
              background: 'rgba(251, 191, 36, 0.15)',
              padding: '0.75rem',
              borderRadius: '10px',
              color: 'var(--accent-amber)',
            }}>
              <Lightbulb size={24} />
            </div>
            <div>
              <strong style={{ color: 'var(--accent-amber)', fontSize: '0.8rem', textTransform: 'uppercase', letterSpacing: '0.05em' }}>
                Key Engineering Takeaway:
              </strong>
              <p style={{ color: 'var(--text-main)', fontSize: '0.9rem', marginTop: '0.25rem' }}>
                {currentPrinciple.keyTakeaway}
              </p>
            </div>
          </div>
        </div>
      )}
    </div>
  );
};
