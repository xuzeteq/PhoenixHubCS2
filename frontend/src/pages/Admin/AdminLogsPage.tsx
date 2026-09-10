import { useEffect, useState } from "react"
import type { LogsResponse } from "../../types/Logs"
import { logsApi } from "../../api/logs.api";
import Header from "../../components/Header/Header";

export default function AdminLogsPage() {

    const [logs, setLogs] = useState<LogsResponse>();

    const handleLogs = async () => {
        logsApi.getLogs().then(res => setLogs(res))
    }

    useEffect(() => {
        handleLogs()
    }, [])

    const levelStyle: Record<string, string> = {
        Information: "text-white uppercase",
        Error: "uppercase text-red-400",
        Warning: "uppercase text-yellow-400",
    }

    return (
        <>
            <div className="ml-55">
                <Header />
            </div>


            <div className="ml-55 mt-6">
                <div className="w-330 mx-auto">
                    <h1 className="text-white text-2xl font-bold">Системные логи</h1>

                    <div className="w-330 h-150 bg-[#111111] rounded-lg mt-4 p-4 overflow-y-scroll">
                        {logs?.logs.map((log, index) => (
                            <p key={index} className="text-white">
                                <span className={`${levelStyle[log.level]}`}>[{log.level}]</span> [{new Date(log.timestamp).toLocaleTimeString()}] {log.message}
                            </p>
                        ))}
                    </div>
                </div>
            </div>
        </>
    )
}