import React, { useEffect, useRef, useState } from 'react';
import mermaid from 'mermaid';

interface MermaidViewerProps {
  chart: string;
}

mermaid.initialize({
  startOnLoad: false,
  theme: 'dark',
  securityLevel: 'loose',
  themeVariables: {
    darkMode: true,
    background: '#131929',
    primaryColor: '#2563eb',
    primaryTextColor: '#f1f5f9',
    primaryBorderColor: '#3b82f6',
    lineColor: '#60a5fa',
    secondaryColor: '#1e293b',
    tertiaryColor: '#0f172a',
  },
});

export const MermaidViewer: React.FC<MermaidViewerProps> = ({ chart }) => {
  const containerRef = useRef<HTMLDivElement>(null);
  const [svgContent, setSvgContent] = useState<string>('');
  const [hasError, setHasError] = useState(false);

  useEffect(() => {
    let isMounted = true;
    const renderChart = async () => {
      if (!chart.trim()) return;
      try {
        const id = `mermaid-${Math.random().toString(36).substring(2, 9)}`;
        const { svg } = await mermaid.render(id, chart);
        if (isMounted) {
          setSvgContent(svg);
          setHasError(false);
        }
      } catch (err) {
        console.error('Mermaid rendering error:', err);
        if (isMounted) {
          setHasError(true);
        }
      }
    };

    renderChart();
    return () => {
      isMounted = false;
    };
  }, [chart]);

  if (hasError) {
    return (
      <div style={{
        padding: '1rem',
        borderRadius: '8px',
        background: '#1e1b2e',
        border: '1px solid #4c1d95',
        fontSize: '0.8rem',
        color: '#c4b5fd'
      }}>
        <code>{chart}</code>
      </div>
    );
  }

  return (
    <div
      ref={containerRef}
      style={{
        display: 'flex',
        justifyContent: 'center',
        padding: '1rem',
        background: '#0d111a',
        borderRadius: '8px',
        border: '1px solid var(--border-color)',
        overflowX: 'auto',
      }}
      dangerouslySetInnerHTML={{ __html: svgContent }}
    />
  );
};
