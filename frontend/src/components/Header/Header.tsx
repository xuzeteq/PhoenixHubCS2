import PingCircle from "../PingCircle/PingCircle";
import { IconBrandSteam } from "@tabler/icons-react";
import { useEffect, useState, useMemo } from "react"
import type { Server } from "../../types/Server";
import { serverApi } from "../../api/servers.api";
import { authApi } from "../../api/auth.api";
import { useAuth } from "../../contexts/AuthProvider";
import DropdownMenu from "../Dropdown/DropdownMenu";


export default function Header() {

    const { isAuthenticated, isLoading } = useAuth();

    const [servers, setServers] = useState<Server[]>([]);
        

        const totalOnline = useMemo(() => {
            return servers.reduce((sum, server) => sum + server.online, 0);
        }, [servers]);

        const handleSteamLogin = () => {
            window.location.href = authApi.getLoginUrl();
        };

        useEffect(() => {
            try {
                serverApi.getAllServers().then(data => setServers(data))
            }
            catch (ex) {
                console.error(ex)
            }
        }, [])

    return (
        <header className="w-full max-w-330 mx-auto px-8 pt-6 pb-2 flex items-center justify-between">
            
            <div className="flex items-center gap-3">
                <PingCircle />
                
                <div className="flex flex-col">
                    <span className="text-gray-500 text-xs uppercase tracking-wider font-medium">Онлайн</span>
                    <span className="text-white text-2xl font-bold leading-tight">{totalOnline}</span>
                </div>
            </div>

            {isLoading ? (
                <div className="w-36 h-11 rounded-xl bg-white/5 animate-pulse" />
            ) : isAuthenticated ? (
                <DropdownMenu />
            ) : (
                <button className="flex items-center gap-2.5 px-5 py-2.5 rounded-xl border border-white/10 bg-blue-500/70 text-gray-300 text-sm font-semibold cursor-pointer
                    hover:bg-blue-500/80 hover:border-white/20 hover:text-white transition-all duration-300 group"
                    onClick={handleSteamLogin}>
                    <IconBrandSteam className="w-5 h-5 text-white/60 group-hover:text-white transition-colors" />
                    <span>Войти через Steam</span>
                </button>
            )}
            
        </header>
    );
}