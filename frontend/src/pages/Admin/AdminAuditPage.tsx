import { useEffect, useMemo, useState } from "react";
import type { AuditQuery, AuditResponse } from "../../types/Audit";
import { auditApi } from "../../api/audit.api";
import Header from "../../components/Header/Header";
import Pagination from "../../components/Pagination/Pagination";

export default function AdminAuditPage() {

    const [logs, setLogs] = useState<AuditResponse>();
    const [page, setPage] = useState(1);
    const [username, setUsername] = useState('');
    const [entity, setEntity] = useState('');
    const [action, setAction] = useState('')

    const query: AuditQuery = useMemo(() => ({
        page,
        pageSize: 50,
        username: username || undefined,
        entityType: entity || undefined,
        action: action || undefined,
    }), [page, username, action, entity])

    const formatDateTime = (iso: string) => {
        const d = new Date(iso)
        const pad = (n: number) => String(n).padStart(2, '0')
        return (
            `${d.getFullYear()}-${pad(d.getMonth() + 1)}-${pad(d.getDate())} ` +
            `${pad(d.getHours())}:${pad(d.getMinutes())}:${pad(d.getSeconds())}`
        )
    }

    const handleCleanFilters = () => {
        setAction('')
        setUsername('')
        setEntity('')
    }

    useEffect(() => {
        auditApi.getLogs(query)
            .then(res => setLogs(res))
    }, [query])

    return (
        <>
            <div className="ml-55">
                <Header />
            </div>

            <div className="ml-55 mt-6">
                <div className="w-330 mx-auto">
                    <h1 className="text-white text-2xl font-bold">Аудит логи</h1>

                    <div className="mt-5 flex items-center justify-between">
                        <div className="flex gap-3">
                            <input type="text" className="w-50 bg-transparent placeholder:text-slate-300 text-white text-sm border border-slate-400
                                rounded-md px-3 py-2 transition duration-300 ease focus:outline-none focus:border-slate-50 hover:border-slate-300
                                shadow-sm focus:shadow"
                                value={username}
                                placeholder="Поиск по нику"
                                onChange={(e) => { setUsername(e.target.value); setPage(1) }}/>

                        <input type="text" className="w-50 bg-transparent placeholder:text-slate-300 text-white text-sm border border-slate-400
                            rounded-md px-3 py-2 transition duration-300 ease focus:outline-none focus:border-slate-50 hover:border-slate-300
                            shadow-sm focus:shadow"
                            value={action}
                            placeholder="Поиск по действию"
                            onChange={(e) => { setAction(e.target.value); setPage(1) }}/>

                        <input type="text" className="w-50 bg-transparent placeholder:text-slate-300 text-white text-sm border border-slate-400
                            rounded-md px-3 py-2 transition duration-300 ease focus:outline-none focus:border-slate-50 hover:border-slate-300
                            shadow-sm focus:shadow"
                            value={entity}
                            placeholder="Поиск по типу"
                            onChange={(e) => { setEntity(e.target.value); setPage(1) }}/>
                        </div>

                        <div>
                            <button className="text-white px-4 py-2 bg-blue-400/40 cursor-pointer rounded-md
                                hover:bg-blue-400/70 transition"
                            onClick={handleCleanFilters}>Сбросить фильтры</button>
                        </div>
                    </div>

                    <div className="border-b border-neutral-800 bg-[#121212] rounded-t-lg mt-5">
                        <div className="grid grid-cols-[180px_1fr_120px] text-white font-semibold uppercase mx-4 py-2 text-sm">
                            <span>Дата</span>
                            <span>Действие</span>
                            <span>Сущность</span>
                        </div>
                    </div>

                    <div className="w-330 h-150 rounded-b-lg bg-[#121212] overflow-y-scroll">
                        <div className="text-white text-sm">
                            {logs?.audit.map(log => (
                                <div key={log.id} className="grid grid-cols-[180px_1fr_120px] py-2 border-b border-neutral-800 px-4
                                    hover:bg-neutral-800 transition">
                                    <span>{formatDateTime(log.timestamp)}</span>
                                    <span><span className="text-blue-400/70">{log.username}</span> {log.action}</span>
                                    <span>{log.entityType}</span>
                                </div>
                            ))}
                        </div>
                    </div>
                        {logs && (
                            <Pagination page={page} totalPages={logs.totalPages} onChange={setPage}/>
                        )}
                </div>
            </div>
        </>
    )
}