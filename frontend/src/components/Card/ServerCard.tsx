import { IconCopy, IconPlayerPlay, IconCheck } from "@tabler/icons-react";
import { useState } from "react";
import type { Server } from "../../types/Server";

interface ServerCardProps {
    server: Server
}

export default function ServerCard({ server }: ServerCardProps) {
    const [isCopied, setIsCopied] = useState(false);
    const ipString = `${server.ipAddress}:${server.port}`;
    const connectCommand = `connect ${ipString}`;
    const steamConnectUrl = `steam://connect/${ipString}`;

    const handleCopy = async () => {
        try {
            if (navigator.clipboard && window.isSecureContext) {
                await navigator.clipboard.writeText(connectCommand);
            }
            
            setIsCopied(true);
            setTimeout(() => setIsCopied(false), 3000);
        } catch (err) {
            console.error("Не удалось скопировать:", err);
        }
    }

    const handleConnect = () => window.location.href = steamConnectUrl;

    const progress = Math.min((server.online / server.maxOnline) * 100, 100);

    return (
        <div className="relative h-32 w-78 rounded-2xl mt-4 group shrink-0 overflow-hidden">
            <div className="absolute inset-0 bg-neutral-800 overflow-hidden rounded-2xl">
                <img
                    src="/mirage.png"
                    alt="map"
                    className="absolute inset-0 rounded-2xl w-full h-full object-cover transition-transform duration-700 ease-out group-hover:scale-125"
                />
                <div className="absolute inset-0 bg-black/70 group-hover:bg-black/40 transition-all rounded" />
            </div>

            <div className="relative flex justify-between h-full p-2">
                <div className="flex flex-col justify-between flex-1 min-w-0">
                    <div>
                        <h2 className="text-white text-xl font-bold truncate w-full max-w-65">
                            {server.title}
                        </h2>
                        <p className="text-white/70 text-sm font-semibold truncate w-full max-w-45">
                            {server.map}
                        </p>
                    </div>

                    <div className="pr-2">
                        <div className="w-full h-1 rounded-full bg-gray-600 relative overflow-hidden">
                            <div
                                className="absolute inset-0 bg-blue-500 transition-all duration-300"
                                style={{ width: `${progress}%` }}
                            />
                        </div>
                        <p className="text-white/70 text-sm font-semibold mt-1">
                            {server.online}/{server.maxOnline}
                        </p>
                    </div>
                </div>

                <div className="flex flex-col justify-between py-1 shrink-0 ml-2">
                    <button 
                        onClick={handleConnect}
                        className="p-2 rounded-full bg-white/10 text-white/80 hover:text-white hover:bg-white/20 transition cursor-pointer"
                        title="Подключиться через Steam"
                    >
                        <IconPlayerPlay className="w-5 h-5" />
                    </button>
                    
                    <button 
                        onClick={handleCopy}
                        className="p-2 rounded-full bg-white/10 text-white/80 hover:text-white hover:bg-white/20 transition cursor-pointer"
                        title={isCopied ? "Скопировано!" : `Скопировать: ${connectCommand}`}
                    >
                        {isCopied ? (
                            <IconCheck className="w-5 h-5 text-green-400" />
                        ) : (
                            <IconCopy className="w-5 h-5" />
                        )}
                    </button>
                </div>
            </div>
        </div>
    );
}