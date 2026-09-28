import React, { useEffect, useState } from 'react';
import { fetchDesignPatterns, DesignPattern } from '../services/api';
import { CodeBlock } from '../components/CodeBlock';
import { MermaidViewer } from '../components/MermaidViewer';
import { CheckCircle2, XCircle } from 'lucide-react';

export const DesignPatternsPage: React.FC = () => {
  const [patterns, setPatterns] = useState<DesignPattern[]>([]);
  const [categoryFilter, setCategoryFilter] = useState<'All' | 'Creational' | 'Structural' | 'Behavioral'>('All');
  const [selectedPattern, setSelectedPattern] = useState<DesignPattern | null>(null);

  useEffect(() => {
    fetchDesignPatterns().then((data) => {
      setPatterns(data);
      if (data.length > 0) setSelectedPattern(data[0]);
    });
  }, []);

  const filteredPatterns = categoryFilter === 'All'
    ? patterns
    : patterns.filter((p) => p.category === categoryFilter);

  return (
    <div style={{ display: 'flex', flexDirection: 'column', gap: '1.5rem' }}>
      {/* Category Filter */}
      <div style={{
        display: 'flex',
        gap: '0.5rem',
        borderBottom: '1px solid var(--border-color)',
        paddingBottom: '0.75rem',
      }}>
        {(['All', 'Creational', 'Structural', 'Behavioral'] as const).map((cat) => (
          <button
            key={cat}
            onClick={() => setCategoryFilter(cat)}
            className={`btn ${categoryFilter === cat ? 'btn-primary' : 'btn-secondary'}`}
          >
            {cat} Patterns
          </button>
        ))}
      </div>

      <div style={{ display: 'grid', gridTemplateColumns: '280px 1fr', gap: '1.5rem', alignItems: 'start' }}>
        {/* Pattern List Sidebar */}
        <div style={{ display: 'flex', flexDirection: 'column', gap: '0.5rem' }}>
          {filteredPatterns.map((pat) => {
            const isSelected = selectedPattern?.name === pat.name;
            return (
              <button
                key={pat.name}
                onClick={() => setSelectedPattern(pat)}
                className="card"
                style={{
                  padding: '0.85rem 1rem',
                  display: 'flex',
                  justifyContent: 'space-between',
                  alignItems: 'center',
                  background: isSelected ? 'var(--bg-tertiary)' : 'var(--bg-card)',
                  borderColor: isSelected ? 'var(--accent-blue)' : 'var(--border-color)',
                  cursor: 'pointer',
                  textAlign: 'left',
                  width: '100%',
                }}
              >
                <div>
                  <strong style={{ fontSize: '0.95rem', color: isSelected ? 'var(--accent-blue)' : 'var(--text-main)' }}>
                    {pat.name}
                  </strong>
                  <div style={{ fontSize: '0.75rem', color: 'var(--text-faint)' }}>{pat.category}</div>
                </div>
                <span className={`badge ${
                  pat.category === 'Creational' ? 'badge-blue' :
                  pat.category === 'Structural' ? 'badge-emerald' : 'badge-amber'
                }`} style={{ fontSize: '0.65rem' }}>
                  {pat.category[0]}
                </span>
              </button>
            );
          })}
        </div>

        {/* Selected Pattern Showcase */}
        {selectedPattern && (
          <div className="card" style={{ display: 'flex', flexDirection: 'column', gap: '1.25rem' }}>
            <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'flex-start' }}>
              <div>
                <div style={{ display: 'flex', alignItems: 'center', gap: '0.5rem' }}>
                  <h2 style={{ fontSize: '1.35rem', fontWeight: 700 }}>{selectedPattern.name} Pattern</h2>
                  <span className="badge badge-indigo">{selectedPattern.category}</span>
                </div>
                <p style={{ color: 'var(--text-muted)', fontSize: '0.9rem', marginTop: '0.25rem' }}>
                  {selectedPattern.problemSolved}
                </p>
              </div>
            </div>

            {/* Mermaid Class Diagram */}
            <div>
              <h4 style={{ fontSize: '0.8rem', textTransform: 'uppercase', letterSpacing: '0.05em', color: 'var(--text-faint)', marginBottom: '0.5rem' }}>
                Structure & Class Diagram (Mermaid)
              </h4>
              <MermaidViewer chart={selectedPattern.mermaidDiagram} />
            </div>

            {/* C# Implementation */}
            <div>
              <h4 style={{ fontSize: '0.8rem', textTransform: 'uppercase', letterSpacing: '0.05em', color: 'var(--text-faint)', marginBottom: '0.5rem' }}>
                C# .NET 10 Implementation
              </h4>
              <CodeBlock code={selectedPattern.csharpExample} title={`${selectedPattern.name}.cs`} />
            </div>

            {/* Trade-offs: When to Use & When to Avoid */}
            <div style={{ display: 'grid', gridTemplateColumns: 'repeat(auto-fit, minmax(280px, 1fr))', gap: '1rem' }}>
              <div style={{
                padding: '0.85rem 1rem',
                borderRadius: '8px',
                background: 'rgba(52, 211, 153, 0.06)',
                border: '1px solid rgba(52, 211, 153, 0.25)',
                display: 'flex',
                gap: '0.65rem',
              }}>
                <CheckCircle2 size={18} color="var(--accent-emerald)" style={{ flexShrink: 0, marginTop: '2px' }} />
                <div>
                  <strong style={{ fontSize: '0.8rem', color: 'var(--accent-emerald)', textTransform: 'uppercase' }}>When to Use</strong>
                  <p style={{ fontSize: '0.825rem', color: '#cbd5e1', marginTop: '0.2rem' }}>{selectedPattern.whenToUse}</p>
                </div>
              </div>

              <div style={{
                padding: '0.85rem 1rem',
                borderRadius: '8px',
                background: 'rgba(248, 113, 113, 0.06)',
                border: '1px solid rgba(248, 113, 113, 0.25)',
                display: 'flex',
                gap: '0.65rem',
              }}>
                <XCircle size={18} color="var(--accent-rose)" style={{ flexShrink: 0, marginTop: '2px' }} />
                <div>
                  <strong style={{ fontSize: '0.8rem', color: 'var(--accent-rose)', textTransform: 'uppercase' }}>When to Avoid</strong>
                  <p style={{ fontSize: '0.825rem', color: '#cbd5e1', marginTop: '0.2rem' }}>{selectedPattern.whenToAvoid}</p>
                </div>
              </div>
            </div>
          </div>
        )}
      </div>
    </div>
  );
};
