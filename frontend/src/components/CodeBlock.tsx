import React, { useState } from 'react';
import { Copy, Check } from 'lucide-react';

interface CodeBlockProps {
  code: string;
  language?: string;
  title?: string;
  badge?: string;
  badgeType?: 'danger' | 'success' | 'neutral';
}

export const CodeBlock: React.FC<CodeBlockProps> = ({
  code,
  title,
  badge,
  badgeType = 'neutral',
}) => {
  const [copied, setCopied] = useState(false);

  const handleCopy = () => {
    navigator.clipboard.writeText(code);
    setCopied(true);
    setTimeout(() => setCopied(false), 2000);
  };

  const getBadgeClass = () => {
    if (badgeType === 'danger') return 'badge-rose';
    if (badgeType === 'success') return 'badge-emerald';
    return 'badge-indigo';
  };

  return (
    <div style={{
      borderRadius: '8px',
      overflow: 'hidden',
      background: '#0d111a',
      border: '1px solid var(--border-color)',
      fontSize: '0.85rem'
    }}>
      {(title || badge) && (
        <div style={{
          display: 'flex',
          justifyContent: 'space-between',
          alignItems: 'center',
          padding: '0.45rem 0.85rem',
          background: 'rgba(255, 255, 255, 0.03)',
          borderBottom: '1px solid var(--border-color)',
        }}>
          <div style={{ display: 'flex', alignItems: 'center', gap: '0.5rem' }}>
            {title && <span style={{ fontWeight: 600, color: 'var(--text-main)' }}>{title}</span>}
            {badge && <span className={`badge ${getBadgeClass()}`}>{badge}</span>}
          </div>
          <button
            onClick={handleCopy}
            className="btn btn-secondary"
            style={{ padding: '0.2rem 0.5rem', fontSize: '0.75rem' }}
            title="Copy Code"
          >
            {copied ? <Check size={12} color="var(--accent-emerald)" /> : <Copy size={12} />}
            {copied ? 'Copied' : 'Copy'}
          </button>
        </div>
      )}

      <pre style={{
        padding: '0.85rem',
        overflowX: 'auto',
        color: '#e2e8f0',
        lineHeight: 1.45,
        margin: 0
      }}>
        <code>{code}</code>
      </pre>
    </div>
  );
};
