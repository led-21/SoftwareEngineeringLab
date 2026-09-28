import React from 'react';
import { Clock, HardDrive } from 'lucide-react';

interface ComplexityBadgeProps {
  timeComplexity: string;
  spaceComplexity: string;
}

export const ComplexityBadge: React.FC<ComplexityBadgeProps> = ({ timeComplexity, spaceComplexity }) => {
  return (
    <div style={{ display: 'flex', gap: '0.5rem', alignItems: 'center', flexWrap: 'wrap' }}>
      <span className="badge badge-blue">
        <Clock size={12} />
        Time: {timeComplexity}
      </span>
      <span className="badge badge-emerald">
        <HardDrive size={12} />
        Space: {spaceComplexity}
      </span>
    </div>
  );
};
