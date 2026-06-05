import React, { useState } from 'react';

export function SearchableSelect({ 
  value, 
  onChange, 
  options, 
  placeholder 
}: { 
  value: number; 
  onChange: (val: number) => void; 
  options: { id: number; label: string }[]; 
  placeholder: string; 
}) {
  const [isOpen, setIsOpen] = useState(false);
  const [searchTerm, setSearchTerm] = useState('');

  const selectedOption = options.find(o => o.id === value);

  return (
    <div className="searchable-select" style={{ position: 'relative' }}>
      <div 
        className="text-input" 
        onClick={() => setIsOpen(!isOpen)}
        style={{ cursor: 'pointer', display: 'flex', justifyContent: 'space-between', alignItems: 'center', background: 'rgba(255,255,255,0.04)' }}
      >
        <span style={{ overflow: 'hidden', textOverflow: 'ellipsis', whiteSpace: 'nowrap' }}>
          {selectedOption ? selectedOption.label : placeholder}
        </span>
        <span style={{ fontSize: '10px', color: 'var(--text-secondary)' }}>▼</span>
      </div>
      
      {isOpen && (
        <>
          <div 
            style={{ position: 'fixed', inset: 0, zIndex: 99 }} 
            onClick={() => { setIsOpen(false); setSearchTerm(''); }} 
          />
          <div 
            className="glass-card" 
            style={{ 
              position: 'absolute', 
              top: '100%', left: 0, right: 0, 
              zIndex: 100, 
              marginTop: '4px',
              maxHeight: '220px',
              overflowY: 'auto',
              padding: '8px',
              background: '#090f0d',
              border: '1px solid var(--baust-green-light)'
            }}
          >
            <input 
              autoFocus
              type="text" 
              className="text-input" 
              placeholder="Search..." 
              value={searchTerm}
              onChange={e => setSearchTerm(e.target.value)}
              style={{ marginBottom: '8px', padding: '6px 10px' }}
            />
            {options.filter(o => o.label.toLowerCase().includes(searchTerm.toLowerCase())).map(o => (
              <div 
                key={o.id}
                onClick={() => {
                  onChange(o.id);
                  setIsOpen(false);
                  setSearchTerm('');
                }}
                style={{
                  padding: '8px 10px',
                  cursor: 'pointer',
                  borderRadius: '4px',
                  background: value === o.id ? 'rgba(0,168,118,0.2)' : 'transparent',
                  color: value === o.id ? 'var(--baust-green-light)' : 'var(--text-primary)',
                  fontSize: '13px'
                }}
                onMouseEnter={e => e.currentTarget.style.background = 'rgba(255,255,255,0.05)'}
                onMouseLeave={e => e.currentTarget.style.background = value === o.id ? 'rgba(0,168,118,0.2)' : 'transparent'}
              >
                {o.label}
              </div>
            ))}
            {options.filter(o => o.label.toLowerCase().includes(searchTerm.toLowerCase())).length === 0 && (
              <div style={{ padding: '8px', fontSize: '12px', color: 'var(--text-muted)', textAlign: 'center' }}>No results found</div>
            )}
          </div>
        </>
      )}
    </div>
  );
}
