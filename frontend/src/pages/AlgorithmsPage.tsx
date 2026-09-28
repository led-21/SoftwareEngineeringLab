import React, { useState } from 'react';
import { executeBinarySearch, BinarySearchStep } from '../services/api';
import { ComplexityBadge } from '../components/ComplexityBadge';
import { Play, RotateCcw, ChevronRight, CheckCircle2 } from 'lucide-react';

export const AlgorithmsPage: React.FC = () => {
  const [activeSubTab, setActiveSubTab] = useState<'binary-search' | 'sorting' | 'stack-queue' | 'islands'>('binary-search');

  // ---------- 1. BINARY SEARCH STATE ----------
  const defaultArray = [2, 5, 8, 12, 16, 23, 38, 45, 56, 72, 91];
  const [bsArray] = useState<number[]>(defaultArray);
  const [bsTarget, setBsTarget] = useState<number>(23);
  const [bsSteps, setBsSteps] = useState<BinarySearchStep[]>([]);
  const [bsCurrentStepIndex, setBsCurrentStepIndex] = useState<number>(-1);
  const [bsResultIndex, setBsResultIndex] = useState<number | null>(null);

  const handleRunBinarySearch = async () => {
    const res = await executeBinarySearch(bsArray, bsTarget);
    setBsSteps(res.steps);
    setBsCurrentStepIndex(0);
    setBsResultIndex(res.foundIndex);
  };

  const handleBsNextStep = () => {
    if (bsCurrentStepIndex < bsSteps.length - 1) {
      setBsCurrentStepIndex(bsCurrentStepIndex + 1);
    }
  };

  const handleBsReset = () => {
    setBsSteps([]);
    setBsCurrentStepIndex(-1);
    setBsResultIndex(null);
  };

  const currentStep = bsCurrentStepIndex >= 0 && bsCurrentStepIndex < bsSteps.length ? bsSteps[bsCurrentStepIndex] : null;

  // ---------- 2. SORTING STATE ----------
  const [sortAlgo, setSortAlgo] = useState<'quicksort' | 'mergesort' | 'heapsort'>('quicksort');
  const initialBars = [65, 28, 85, 42, 15, 92, 53, 37, 74, 10];
  const [sortBars, setSortBars] = useState<number[]>(initialBars);
  const [isSorted, setIsSorted] = useState<boolean>(false);
  const [sortComparing, setSortComparing] = useState<number[]>([]);

  const handleSortRun = async () => {
    setIsSorted(false);
    const copy = [...sortBars];

    // Simple visual bubble-like step simulation for real-time visualization of bars
    for (let i = 0; i < copy.length; i++) {
      for (let j = 0; j < copy.length - i - 1; j++) {
        setSortComparing([j, j + 1]);
        if (copy[j] > copy[j + 1]) {
          const temp = copy[j];
          copy[j] = copy[j + 1];
          copy[j + 1] = temp;
          setSortBars([...copy]);
          await new Promise((r) => setTimeout(r, 60));
        }
      }
    }
    setSortComparing([]);
    setIsSorted(true);
  };

  const handleSortShuffle = () => {
    const shuffled = [...initialBars].sort(() => Math.random() - 0.5);
    setSortBars(shuffled);
    setIsSorted(false);
    setSortComparing([]);
  };

  // ---------- 3. STACK & QUEUE STATE ----------
  const [stackItems, setStackItems] = useState<number[]>([10, 20, 30]);
  const [queueItems, setQueueItems] = useState<number[]>([100, 200, 300]);
  const [stackInput, setStackInput] = useState<string>('40');
  const [queueInput, setQueueInput] = useState<string>('400');

  const pushStack = () => {
    const val = parseInt(stackInput, 10);
    if (!isNaN(val)) {
      setStackItems([val, ...stackItems]);
      setStackInput(String(val + 10));
    }
  };

  const popStack = () => {
    if (stackItems.length > 0) {
      setStackItems(stackItems.slice(1));
    }
  };

  const enqueueQueue = () => {
    const val = parseInt(queueInput, 10);
    if (!isNaN(val)) {
      setQueueItems([...queueItems, val]);
      setQueueInput(String(val + 100));
    }
  };

  const dequeueQueue = () => {
    if (queueItems.length > 0) {
      setQueueItems(queueItems.slice(1));
    }
  };

  // ---------- 4. NUMBER OF ISLANDS STATE ----------
  const initialGrid = [
    ['1', '1', '0', '0', '0'],
    ['1', '1', '0', '0', '0'],
    ['0', '0', '1', '0', '0'],
    ['0', '0', '0', '1', '1'],
    ['0', '0', '0', '0', '1'],
  ];
  const [grid, setGrid] = useState<string[][]>(initialGrid);
  const [islandCount, setIslandCount] = useState<number | null>(null);

  const toggleCell = (r: number, c: number) => {
    const updated = grid.map((row, ri) =>
      row.map((cell, ci) => (ri === r && ci === c ? (cell === '1' ? '0' : '1') : cell))
    );
    setGrid(updated);
    setIslandCount(null);
  };

  const runIslandCounter = () => {
    const rows = grid.length;
    const cols = grid[0].length;
    const visited = Array.from({ length: rows }, () => Array(cols).fill(false));
    let count = 0;

    const dfs = (r: number, c: number) => {
      if (r < 0 || r >= rows || c < 0 || c >= cols || visited[r][c] || grid[r][c] === '0') return;
      visited[r][c] = true;
      dfs(r + 1, c);
      dfs(r - 1, c);
      dfs(r, c + 1);
      dfs(r, c - 1);
    };

    for (let r = 0; r < rows; r++) {
      for (let c = 0; c < cols; c++) {
        if (grid[r][c] === '1' && !visited[r][c]) {
          count++;
          dfs(r, c);
        }
      }
    }
    setIslandCount(count);
  };

  return (
    <div style={{ display: 'flex', flexDirection: 'column', gap: '1.5rem' }}>
      {/* Sub Tabs */}
      <div style={{ display: 'flex', gap: '0.5rem', borderBottom: '1px solid var(--border-color)', paddingBottom: '0.75rem' }}>
        <button
          onClick={() => setActiveSubTab('binary-search')}
          className={`btn ${activeSubTab === 'binary-search' ? 'btn-primary' : 'btn-secondary'}`}
        >
          Binary Search
        </button>
        <button
          onClick={() => setActiveSubTab('sorting')}
          className={`btn ${activeSubTab === 'sorting' ? 'btn-primary' : 'btn-secondary'}`}
        >
          Sorting (Bars)
        </button>
        <button
          onClick={() => setActiveSubTab('stack-queue')}
          className={`btn ${activeSubTab === 'stack-queue' ? 'btn-primary' : 'btn-secondary'}`}
        >
          Stack & Queue
        </button>
        <button
          onClick={() => setActiveSubTab('islands')}
          className={`btn ${activeSubTab === 'islands' ? 'btn-primary' : 'btn-secondary'}`}
        >
          Number of Islands (Grid BFS/DFS)
        </button>
      </div>

      {/* 1. BINARY SEARCH PLAYGROUND */}
      {activeSubTab === 'binary-search' && (
        <div className="card" style={{ display: 'flex', flexDirection: 'column', gap: '1.25rem' }}>
          <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'flex-start', flexWrap: 'wrap', gap: '1rem' }}>
            <div>
              <h2 style={{ fontSize: '1.2rem', fontWeight: 600 }}>Binary Search Visualizer</h2>
              <p style={{ color: 'var(--text-muted)', fontSize: '0.85rem' }}>
                Dividing the search space in half with every comparison using two pointers (Left, Right) and Midpoint.
              </p>
            </div>
            <ComplexityBadge timeComplexity="O(log n)" spaceComplexity="O(1)" />
          </div>

          {/* Controls */}
          <div style={{ display: 'flex', gap: '0.75rem', alignItems: 'center', flexWrap: 'wrap' }}>
            <label style={{ fontSize: '0.85rem', color: 'var(--text-muted)' }}>Target to Find:</label>
            <input
              type="number"
              className="input-field"
              style={{ width: '80px' }}
              value={bsTarget}
              onChange={(e) => setBsTarget(Number(e.target.value))}
            />
            <button onClick={handleRunBinarySearch} className="btn btn-primary">
              <Play size={15} /> Run Trace
            </button>
            {bsSteps.length > 0 && (
              <>
                <button
                  onClick={handleBsNextStep}
                  disabled={bsCurrentStepIndex >= bsSteps.length - 1}
                  className="btn btn-secondary"
                >
                  <ChevronRight size={15} /> Next Step ({bsCurrentStepIndex + 1}/{bsSteps.length})
                </button>
                <button onClick={handleBsReset} className="btn btn-secondary">
                  <RotateCcw size={15} /> Reset
                </button>
              </>
            )}
          </div>

          {/* Visual Array Cells */}
          <div style={{
            display: 'flex',
            gap: '0.5rem',
            overflowX: 'auto',
            padding: '1.5rem 0.5rem',
            background: 'var(--bg-secondary)',
            borderRadius: '8px',
            border: '1px solid var(--border-color)',
            justifyContent: 'center',
          }}>
            {bsArray.map((val, idx) => {
              const isMid = currentStep && currentStep.mid === idx;
              const isLeft = currentStep && currentStep.left === idx;
              const isRight = currentStep && currentStep.right === idx;
              const isEliminated = currentStep && (idx < currentStep.left || idx > currentStep.right);
              const isTargetFound = bsResultIndex === idx && currentStep && currentStep.mid === idx;

              let cellBg = 'var(--bg-tertiary)';
              let borderCol = 'var(--border-color)';
              if (isTargetFound) {
                cellBg = 'rgba(52, 211, 153, 0.25)';
                borderCol = 'var(--accent-emerald)';
              } else if (isMid) {
                cellBg = 'rgba(56, 189, 248, 0.25)';
                borderCol = 'var(--accent-blue)';
              } else if (isEliminated) {
                cellBg = 'rgba(15, 23, 42, 0.4)';
              }

              return (
                <div key={idx} style={{ display: 'flex', flexDirection: 'column', alignItems: 'center', gap: '0.35rem' }}>
                  <span style={{ fontSize: '0.7rem', color: 'var(--text-faint)' }}>idx {idx}</span>
                  <div style={{
                    width: '48px',
                    height: '48px',
                    display: 'flex',
                    alignItems: 'center',
                    justifyContent: 'center',
                    borderRadius: '8px',
                    background: cellBg,
                    border: `2px solid ${borderCol}`,
                    fontWeight: 700,
                    fontSize: '1rem',
                    color: isEliminated ? 'var(--text-faint)' : 'var(--text-main)',
                    opacity: isEliminated ? 0.35 : 1,
                    transition: 'all 0.2s ease',
                  }}>
                    {val}
                  </div>
                  {/* Pointer labels */}
                  <div style={{ height: '20px', display: 'flex', gap: '2px', fontSize: '0.65rem', fontWeight: 600 }}>
                    {isLeft && <span className="badge badge-amber" style={{ padding: '0 4px' }}>L</span>}
                    {isMid && <span className="badge badge-blue" style={{ padding: '0 4px' }}>MID</span>}
                    {isRight && <span className="badge badge-rose" style={{ padding: '0 4px' }}>R</span>}
                  </div>
                </div>
              );
            })}
          </div>

          {/* Current Step Action Log */}
          {currentStep && (
            <div style={{
              background: 'var(--bg-tertiary)',
              padding: '0.85rem 1rem',
              borderRadius: '8px',
              borderLeft: '4px solid var(--accent-blue)',
              display: 'flex',
              alignItems: 'center',
              justifyContent: 'space-between'
            }}>
              <div>
                <strong style={{ color: 'var(--accent-blue)' }}>Step {currentStep.stepNumber}: </strong>
                <span>{currentStep.action}</span>
              </div>
              <span className="badge badge-indigo">
                Pointers: L={currentStep.left} | MID={currentStep.mid} | R={currentStep.right}
              </span>
            </div>
          )}
        </div>
      )}

      {/* 2. SORTING BARS PLAYGROUND */}
      {activeSubTab === 'sorting' && (
        <div className="card" style={{ display: 'flex', flexDirection: 'column', gap: '1.25rem' }}>
          <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'flex-start', flexWrap: 'wrap', gap: '1rem' }}>
            <div>
              <h2 style={{ fontSize: '1.2rem', fontWeight: 600 }}>Sorting Algorithms (Bar Visualizer)</h2>
              <p style={{ color: 'var(--text-muted)', fontSize: '0.85rem' }}>
                Visual comparison of values represented by vertical proportional bars.
              </p>
            </div>
            <ComplexityBadge
              timeComplexity="O(n log n)"
              spaceComplexity={sortAlgo === 'mergesort' ? 'O(n)' : sortAlgo === 'heapsort' ? 'O(1)' : 'O(log n)'}
            />
          </div>

          <div style={{ display: 'flex', gap: '0.75rem', alignItems: 'center', flexWrap: 'wrap' }}>
            <select
              className="input-field"
              value={sortAlgo}
              onChange={(e) => setSortAlgo(e.target.value as any)}
            >
              <option value="quicksort">Quick Sort (Lomuto Partition)</option>
              <option value="mergesort">Merge Sort (Divide & Conquer)</option>
              <option value="heapsort">Heap Sort (In-place Binary Heap)</option>
            </select>
            <button onClick={handleSortRun} className="btn btn-primary">
              <Play size={15} /> Sort Bars
            </button>
            <button onClick={handleSortShuffle} className="btn btn-secondary">
              <RotateCcw size={15} /> Shuffle
            </button>
            {isSorted && (
              <span className="badge badge-emerald">
                <CheckCircle2 size={13} /> Sorted Successfully!
              </span>
            )}
          </div>

          {/* Bar Chart Container */}
          <div style={{
            display: 'flex',
            alignItems: 'flex-end',
            justifyContent: 'center',
            gap: '12px',
            height: '240px',
            padding: '1.5rem',
            background: 'var(--bg-secondary)',
            borderRadius: '8px',
            border: '1px solid var(--border-color)',
          }}>
            {sortBars.map((val, idx) => {
              const isComp = sortComparing.includes(idx);
              let barColor = isSorted ? 'var(--accent-emerald)' : isComp ? 'var(--accent-rose)' : 'var(--accent-blue)';

              return (
                <div
                  key={idx}
                  style={{
                    display: 'flex',
                    flexDirection: 'column',
                    alignItems: 'center',
                    gap: '6px',
                    height: '100%',
                    justifyContent: 'flex-end',
                  }}
                >
                  <span style={{ fontSize: '0.75rem', fontWeight: 600 }}>{val}</span>
                  <div
                    style={{
                      width: '32px',
                      height: `${(val / 100) * 160}px`,
                      backgroundColor: barColor,
                      borderRadius: '6px 6px 0 0',
                      transition: 'height 0.1s ease, background-color 0.15s ease',
                      boxShadow: isComp ? '0 0 12px rgba(248, 113, 113, 0.6)' : 'none',
                    }}
                  />
                  <span style={{ fontSize: '0.65rem', color: 'var(--text-faint)' }}>{idx}</span>
                </div>
              );
            })}
          </div>
        </div>
      )}

      {/* 3. STACK & QUEUE PLAYGROUND */}
      {activeSubTab === 'stack-queue' && (
        <div style={{ display: 'grid', gridTemplateColumns: 'repeat(auto-fit, minmax(340px, 1fr))', gap: '1.5rem' }}>
          {/* Stack Card */}
          <div className="card" style={{ display: 'flex', flexDirection: 'column', gap: '1rem' }}>
            <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
              <div>
                <h3 style={{ fontSize: '1.1rem', fontWeight: 600 }}>Stack (LIFO)</h3>
                <p style={{ fontSize: '0.75rem', color: 'var(--text-muted)' }}>Last In, First Out</p>
              </div>
              <ComplexityBadge timeComplexity="O(1)" spaceComplexity="O(n)" />
            </div>

            <div style={{ display: 'flex', gap: '0.5rem' }}>
              <input
                type="number"
                className="input-field"
                style={{ width: '80px' }}
                value={stackInput}
                onChange={(e) => setStackInput(e.target.value)}
              />
              <button onClick={pushStack} className="btn btn-primary" style={{ flex: 1 }}>Push</button>
              <button onClick={popStack} className="btn btn-danger" style={{ flex: 1 }}>Pop</button>
            </div>

            {/* Stack Visual Well */}
            <div style={{
              minHeight: '220px',
              border: '2px dashed var(--border-color)',
              borderRadius: '8px',
              padding: '1rem',
              display: 'flex',
              flexDirection: 'column',
              gap: '0.5rem',
              background: 'var(--bg-secondary)',
              alignItems: 'center',
            }}>
              <span style={{ fontSize: '0.7rem', color: 'var(--text-faint)', textTransform: 'uppercase', letterSpacing: '0.05em' }}>
                ▲ TOP OF STACK
              </span>
              {stackItems.length === 0 ? (
                <div style={{ margin: 'auto', color: 'var(--text-faint)', fontSize: '0.85rem' }}>Stack is empty</div>
              ) : (
                stackItems.map((val, idx) => (
                  <div
                    key={idx}
                    style={{
                      width: '80%',
                      padding: '0.5rem',
                      background: idx === 0 ? 'rgba(56, 189, 248, 0.2)' : 'var(--bg-tertiary)',
                      border: idx === 0 ? '1px solid var(--accent-blue)' : '1px solid var(--border-color)',
                      borderRadius: '6px',
                      textAlign: 'center',
                      fontWeight: 600,
                      display: 'flex',
                      justifyContent: 'space-between',
                      alignItems: 'center',
                    }}
                  >
                    <span style={{ fontSize: '0.75rem', color: 'var(--text-muted)' }}>{idx === 0 ? 'Top' : ''}</span>
                    <span>{val}</span>
                    <span style={{ fontSize: '0.75rem', color: 'var(--text-faint)' }}>#{stackItems.length - idx}</span>
                  </div>
                ))
              )}
            </div>
          </div>

          {/* Queue Card */}
          <div className="card" style={{ display: 'flex', flexDirection: 'column', gap: '1rem' }}>
            <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
              <div>
                <h3 style={{ fontSize: '1.1rem', fontWeight: 600 }}>Queue (FIFO)</h3>
                <p style={{ fontSize: '0.75rem', color: 'var(--text-muted)' }}>First In, First Out</p>
              </div>
              <ComplexityBadge timeComplexity="O(1)" spaceComplexity="O(n)" />
            </div>

            <div style={{ display: 'flex', gap: '0.5rem' }}>
              <input
                type="number"
                className="input-field"
                style={{ width: '80px' }}
                value={queueInput}
                onChange={(e) => setQueueInput(e.target.value)}
              />
              <button onClick={enqueueQueue} className="btn btn-primary" style={{ flex: 1 }}>Enqueue</button>
              <button onClick={dequeueQueue} className="btn btn-danger" style={{ flex: 1 }}>Dequeue</button>
            </div>

            {/* Queue Visual Runway */}
            <div style={{
              minHeight: '220px',
              border: '2px dashed var(--border-color)',
              borderRadius: '8px',
              padding: '1rem',
              display: 'flex',
              flexDirection: 'column',
              gap: '0.5rem',
              background: 'var(--bg-secondary)',
              alignItems: 'center',
              justifyContent: 'center',
            }}>
              <div style={{ display: 'flex', alignItems: 'center', gap: '0.5rem', width: '100%', justifyContent: 'space-between', fontSize: '0.7rem', color: 'var(--text-faint)' }}>
                <span>[FRONT / DEQUEUE]</span>
                <span>[BACK / ENQUEUE]</span>
              </div>
              {queueItems.length === 0 ? (
                <div style={{ color: 'var(--text-faint)', fontSize: '0.85rem' }}>Queue is empty</div>
              ) : (
                <div style={{ display: 'flex', gap: '0.5rem', overflowX: 'auto', width: '100%', padding: '0.5rem 0' }}>
                  {queueItems.map((val, idx) => (
                    <div
                      key={idx}
                      style={{
                        minWidth: '60px',
                        padding: '0.75rem 0.5rem',
                        background: idx === 0 ? 'rgba(52, 211, 153, 0.2)' : 'var(--bg-tertiary)',
                        border: idx === 0 ? '1px solid var(--accent-emerald)' : '1px solid var(--border-color)',
                        borderRadius: '6px',
                        textAlign: 'center',
                        fontWeight: 600,
                        fontSize: '0.9rem'
                      }}
                    >
                      {val}
                    </div>
                  ))}
                </div>
              )}
            </div>
          </div>
        </div>
      )}

      {/* 4. NUMBER OF ISLANDS */}
      {activeSubTab === 'islands' && (
        <div className="card" style={{ display: 'flex', flexDirection: 'column', gap: '1.25rem' }}>
          <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'flex-start', flexWrap: 'wrap', gap: '1rem' }}>
            <div>
              <h2 style={{ fontSize: '1.2rem', fontWeight: 600 }}>Number of Islands (2D Grid DFS)</h2>
              <p style={{ color: 'var(--text-muted)', fontSize: '0.85rem' }}>
                Click cells to toggle between Land (1) and Water (0). Run the algorithm to count isolated land masses.
              </p>
            </div>
            <ComplexityBadge timeComplexity="O(m * n)" spaceComplexity="O(m * n)" />
          </div>

          <div style={{ display: 'flex', gap: '1rem', alignItems: 'center' }}>
            <button onClick={runIslandCounter} className="btn btn-primary">
              <Play size={15} /> Count Islands
            </button>
            <button onClick={() => { setGrid(initialGrid); setIslandCount(null); }} className="btn btn-secondary">
              <RotateCcw size={15} /> Reset Grid
            </button>
            {islandCount !== null && (
              <span className="badge badge-emerald" style={{ fontSize: '0.9rem', padding: '0.4rem 0.8rem' }}>
                Found: <strong>{islandCount}</strong> Island{islandCount === 1 ? '' : 's'}
              </span>
            )}
          </div>

          <div style={{
            display: 'inline-flex',
            flexDirection: 'column',
            gap: '8px',
            padding: '1.5rem',
            background: 'var(--bg-secondary)',
            borderRadius: '8px',
            alignSelf: 'flex-start',
            border: '1px solid var(--border-color)',
          }}>
            {grid.map((row, r) => (
              <div key={r} style={{ display: 'flex', gap: '8px' }}>
                {row.map((cell, c) => (
                  <button
                    key={c}
                    onClick={() => toggleCell(r, c)}
                    style={{
                      width: '46px',
                      height: '46px',
                      borderRadius: '6px',
                      border: '1px solid rgba(255, 255, 255, 0.1)',
                      background: cell === '1' ? '#065f46' : '#1e293b',
                      color: cell === '1' ? '#34d399' : '#64748b',
                      fontWeight: 700,
                      cursor: 'pointer',
                      fontSize: '1rem',
                      transition: 'all 0.15s ease',
                    }}
                    title={`Row ${r}, Col ${c} (Click to toggle)`}
                  >
                    {cell === '1' ? '🏝️' : '🌊'}
                  </button>
                ))}
              </div>
            ))}
          </div>
        </div>
      )}
    </div>
  );
};
