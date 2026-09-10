// src/pages/Admin/AdminLogsPage.tsx
import { useState, useEffect, useCallback } from 'react';
import { IconSearch, IconFilter, IconRefresh, IconChevronLeft, IconChevronRight } from '@tabler/icons-react';

import Header from '../../components/Header/Header'; 
import { logsApi } from '../../api/logs.api';
import type { LogsQuery } from '../../types/Logs';
import type { Log } from '../../types/Logs';

const getPaginationRange = (current: number, total: number) => {
    const delta = 1;
    const range: number[] = []; 
    const rangeWithDots: (number | string)[] = [];

    let l: number | undefined;

    for (let i = 1; i <= total; i++) {
        if (i === 1 || i === total || (i >= current - delta && i <= current + delta)) {
            range.push(i);
        }
    }

    for (const i of range) {
        if (l !== undefined) {
            if (i - l === 2) {
                rangeWithDots.push(l + 1);
            } else if (i - l !== 1) {
                rangeWithDots.push('...');
            }
        }
        rangeWithDots.push(i);
        l = i;
    }
    
    return rangeWithDots;
};

const getLevelBadge = (level: string) => {
    const map: Record<string, { short: string; color: string }> = {
        'Information': { short: 'INF', color: 'text-blue-400 bg-blue-500/10 border-blue-500/30' },
        'Warning': { short: 'WRN', color: 'text-orange-400 bg-orange-500/10 border-orange-500/30' },
        'Error': { short: 'ERR', color: 'text-red-400 bg-red-500/10 border-red-500/30' },
        'Fatal': { short: 'FTL', color: 'text-red-600 bg-red-600/10 border-red-600/30 font-bold' },
        'Debug': { short: 'DBG', color: 'text-gray-400 bg-gray-500/10 border-gray-500/30' },
    };
    const config = map[level] || { short: '[UNK]', color: 'text-gray-500 bg-gray-500/10 border-gray-500/30' };
    
    return (
        <span className={`inline-flex items-center justify-center font-mono text-xs px-2 py-0.5 rounded border ${config.color}`}>
            {config.short}
        </span>
    );
};

export default function AdminLogsPage() {
    const [logs, setLogs] = useState<Log[]>([]);
    const [loading, setLoading] = useState(false);
    const [page, setPage] = useState(1);
    const [pageSize] = useState(50);
    const [totalPages, setTotalPages] = useState(1);
    const [totalLogs, setTotalLogs] = useState(0);

    const [level, setLevel] = useState('');
    const [search, setSearch] = useState('');
    const [fromDate, setFromDate] = useState('');
    const [toDate, setToDate] = useState('');

    const fetchLogs = useCallback(async () => {
        setLoading(true);
        try {
            const query: LogsQuery = { page, pageSize };
            if (level) query.level = level;
            if (search) query.search = search;
            if (fromDate) query.from = new Date(fromDate).toISOString();
            if (toDate) query.to = new Date(toDate).toISOString();

            const data = await logsApi.getLogs(query);
            setLogs(data.logs);
            setTotalPages(data.totalPages);
            setTotalLogs(data.totalLogs);
        } catch (error) {
            console.error('Ошибка загрузки логов:', error);
        } finally {
            setLoading(false);
        }
    }, [page, pageSize, level, search, fromDate, toDate]);

    useEffect(() => {
        // eslint-disable-next-line react-hooks/set-state-in-effect
        fetchLogs();
    }, [fetchLogs]);

    const handleSearch = () => {
        setPage(1);
    };

    const handleReset = () => {
        setLevel('');
        setSearch('');
        setFromDate('');
        setToDate('');
        setPage(1);
    };

    return (
        <div className="ml-55 min-h-screen bg-[#0a0a0a]">
            <Header />

            <div className="max-w-7xl mx-auto p-6">
                <div className="flex items-center justify-between mb-6">
                    <div>
                        <h1 className="text-2xl font-bold text-white">Системные логи</h1>
                        <p className="text-gray-400 text-sm mt-1">Всего записей: {totalLogs}</p>
                    </div>
                    <button
                        onClick={() => fetchLogs()}
                        className="flex items-center gap-2 px-4 py-2 bg-blue-600 hover:bg-blue-700 
                                   text-white text-sm font-medium rounded-lg transition-colors"
                    >
                        <IconRefresh className="w-4 h-4" />
                        Обновить
                    </button>
                </div>

                <div className="bg-[#1c1c1c] rounded-xl p-4 mb-6 border border-white/5">
                    <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-4">
                        <div>
                            <label className="block text-xs font-medium text-gray-400 mb-1.5 uppercase tracking-wider">Уровень</label>
                            <select
                                value={level}
                                onChange={(e) => setLevel(e.target.value)}
                                className="w-full px-3 py-2 bg-[#141414] border border-neutral-800 rounded-lg
                                           text-white text-sm focus:outline-none focus:border-blue-500/50 transition-colors"
                            >
                                <option value="">Все уровни</option>
                                <option value="Information">Information</option>
                                <option value="Warning">Warning</option>
                                <option value="Error">Error</option>
                                <option value="Fatal">Fatal</option>
                                <option value="Debug">Debug</option>
                            </select>
                        </div>

                        <div>
                            <label className="block text-xs font-medium text-gray-400 mb-1.5 uppercase tracking-wider">Поиск</label>
                            <div className="relative">
                                <input
                                    type="text"
                                    value={search}
                                    onChange={(e) => setSearch(e.target.value)}
                                    placeholder="Текст сообщения..."
                                    className="w-full px-3 py-2 pl-9 bg-[#141414] border border-neutral-800 
                                               rounded-lg text-white text-sm placeholder:text-gray-600
                                               focus:outline-none focus:border-blue-500/50 transition-colors"
                                />
                                <IconSearch className="absolute left-3 top-1/2 -translate-y-1/2 w-4 h-4 text-gray-600" />
                            </div>
                        </div>

                        <div>
                            <label className="block text-xs font-medium text-gray-400 mb-1.5 uppercase tracking-wider">Дата от</label>
                            <input
                                type="datetime-local"
                                value={fromDate}
                                onChange={(e) => setFromDate(e.target.value)}
                                className="w-full px-3 py-2 bg-[#141414] border border-neutral-800 rounded-lg
                                           text-white text-sm focus:outline-none focus:border-blue-500/50 transition-colors
                                           [&::-webkit-calendar-picker-indicator]:invert"
                            />
                        </div>

                        <div>
                            <label className="block text-xs font-medium text-gray-400 mb-1.5 uppercase tracking-wider">Дата до</label>
                            <input
                                type="datetime-local"
                                value={toDate}
                                onChange={(e) => setToDate(e.target.value)}
                                className="w-full px-3 py-2 bg-[#141414] border border-neutral-800 rounded-lg
                                           text-white text-sm focus:outline-none focus:border-blue-500/50 transition-colors
                                           [&::-webkit-calendar-picker-indicator]:invert"
                            />
                        </div>
                    </div>

                    <div className="flex gap-3 mt-4 pt-4 border-t border-white/5">
                        <button
                            onClick={handleSearch}
                            className="flex items-center gap-2 px-5 py-2 bg-blue-600 hover:bg-blue-700 
                                       text-white text-sm font-medium rounded-lg transition-colors"
                        >
                            <IconFilter className="w-4 h-4" />
                            Применить
                        </button>
                        <button
                            onClick={handleReset}
                            className="px-5 py-2 bg-transparent hover:bg-white/5 text-gray-400 hover:text-white
                                       text-sm font-medium rounded-lg border border-neutral-800 transition-colors"
                        >
                            Сбросить
                        </button>
                    </div>
                </div>

                <div className="bg-[#1c1c1c] rounded-xl border border-white/5 overflow-hidden">
                    {loading ? (
                        <div className="flex items-center justify-center py-20">
                            <div className="animate-spin rounded-full h-8 w-8 border-2 border-blue-500 border-t-transparent"></div>
                        </div>
                    ) : logs.length === 0 ? (
                        <div className="text-center py-20 text-gray-500">
                            <p>Логи не найдены</p>
                            <p className="text-sm mt-1">Попробуйте изменить параметры фильтрации</p>
                        </div>
                    ) : (
                        <div className="overflow-x-auto">
                            <table className="w-full">
                                <thead className="bg-[#141414] border-b border-white/5">
                                    <tr>
                                        <th className="px-4 py-3 text-left text-xs font-medium text-gray-400 uppercase tracking-wider w-44">Время</th>
                                        <th className="px-4 py-3 text-left text-xs font-medium text-gray-400 uppercase tracking-wider w-24">Уровень</th>
                                        <th className="px-4 py-3 text-left text-xs font-medium text-gray-400 uppercase tracking-wider">Сообщение</th>
                                    </tr>
                                </thead>
                                <tbody className="divide-y divide-white/5">
                                    {logs.map((log, index) => (
                                        <tr key={index} className="hover:bg-white/2 transition-colors">
                                            <td className="px-4 py-3 text-sm text-gray-400 font-mono whitespace-nowrap">
                                                {new Date(log.timestamp).toLocaleString('ru-RU', {
                                                    day: '2-digit', month: '2-digit', year: 'numeric',
                                                    hour: '2-digit', minute: '2-digit', second: '2-digit'
                                                })}
                                            </td>
                                            <td className="px-4 py-3">
                                                {getLevelBadge(log.level)}
                                            </td>
                                            <td className="px-4 py-3 text-sm text-gray-200">
                                                <div className="wrap-break-word">{log.message}</div>
                                                {log.exception && (
                                                    <details className="mt-2 group">
                                                        <summary className="cursor-pointer text-red-400 text-xs font-medium hover:text-red-300 flex items-center gap-1">
                                                            <span>Показать stack trace</span>
                                                        </summary>
                                                        <pre className="mt-2 p-3 bg-black/40 rounded-lg text-xs text-red-300/90 overflow-x-auto border border-red-500/20">
                                                            {log.exception}
                                                        </pre>
                                                    </details>
                                                )}
                                            </td>
                                        </tr>
                                    ))}
                                </tbody>
                            </table>
                        </div>
                    )}
                </div>

                {totalPages > 1 && (
                    <div className="flex items-center justify-between mt-6">
                        <div className="text-sm text-gray-500">
                            Страница <span className="font-medium text-gray-300">{page}</span> из <span className="font-medium text-gray-300">{totalPages}</span>
                        </div>
                        
                        <div className="flex items-center gap-1">
                            <button
                                onClick={() => setPage(p => Math.max(1, p - 1))}
                                disabled={page === 1}
                                className="p-2 rounded-lg border border-neutral-800 bg-[#141414] text-gray-400 
                                           hover:bg-white/5 hover:text-white disabled:opacity-30 disabled:cursor-not-allowed transition-colors"
                                aria-label="Предыдущая страница"
                            >
                                <IconChevronLeft className="w-4 h-4" />
                            </button>

                            {getPaginationRange(page, totalPages).map((item, idx) => {
                                if (item === '...') {
                                    return (
                                        <span key={`dots-${idx}`} className="px-3 py-2 text-gray-600 text-sm">
                                            ...
                                        </span>
                                    );
                                }
                                
                                const pageNum = item as number;
                                const isActive = pageNum === page;
                                
                                return (
                                    <button
                                        key={pageNum}
                                        onClick={() => setPage(pageNum)}
                                        className={`min-w-9 h-9 rounded-lg text-sm font-medium transition-colors
                                            ${isActive 
                                                ? 'bg-blue-600 text-white border border-blue-600' 
                                                : 'bg-[#141414] text-gray-400 border border-neutral-800 hover:bg-white/5 hover:text-white'
                                            }`}
                                    >
                                        {pageNum}
                                    </button>
                                );
                            })}

                            <button
                                onClick={() => setPage(p => Math.min(totalPages, p + 1))}
                                disabled={page === totalPages}
                                className="p-2 rounded-lg border border-neutral-800 bg-[#141414] text-gray-400 
                                           hover:bg-white/5 hover:text-white disabled:opacity-30 disabled:cursor-not-allowed transition-colors"
                                aria-label="Следующая страница"
                            >
                                <IconChevronRight className="w-4 h-4" />
                            </button>
                        </div>
                    </div>
                )}
            </div>
        </div>
    );
}