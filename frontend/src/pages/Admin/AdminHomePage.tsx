import { IconBlocks, IconUserPlus, IconUsers, IconWifi } from "@tabler/icons-react";
import AdminInfoCard from "../../components/AdminComponents/InfoCardAadmin";
import Header from "../../components/Header/Header";
import { useAuth } from "../../contexts/AuthProvider"
import OnlineChart from "../../components/Bars/OnlineChart";
import type { Server } from "../../types/Server";
import { useEffect, useState } from "react";
import { serverApi } from "../../api/servers.api";

export default function AdminHomePage() {

    const { user } = useAuth();
    const [servers, setServers] = useState<Server[]>([]);

    useEffect(() => {
                try {
                    serverApi.getAllServers().then(data => setServers(data))
                }
                catch (ex) {
                    console.error(ex)
                }
            }, [])

    const cardProps = [
        {title: 'Пользователей', result: 12, icon: IconUsers},
        {title: 'Блокировок', result: 12, icon: IconBlocks},
        {title: 'Средний онлайн', result: 32, icon: IconWifi},
        {title: 'Подписчиков', result: 3, icon: IconUserPlus},
    ]

    return (
        <>
            <div className="ml-55">
                <Header />
            </div>

            <div className="ml-55 mt-4">
                <div className="w-330 mx-auto">
                    <div>
                        <h1 className="text-white text-xl font-bold">Административная панель</h1>
                        <p className="text-white/50 text-sm">Добро пожаловать {user?.username}</p>
                    </div>

                    <div className="mt-8 flex gap-4 justify-center">
                        {cardProps.map(item => (
                            <AdminInfoCard key={item.title} title={item.title} result={item.result} icon={item.icon}/>
                        ))}
                    </div>

                    <div className="mt-4">
                        <OnlineChart servers={servers}/>
                    </div>
                    
                    <div className="flex items-center gap-4 mt-4">
                            <button className="w-full relative rounded-xl h-12 bg-linear-to-r from-blue-600 to-cyan-500 text-white font-bold text-base tracking-wide
                                cursor-pointer hover:scale-[1.02] active:scale-[0.98] transition-all duration-300
                                overflow-hidden group/btn">
                                <div className="absolute inset-0 w-full h-full bg-linear-to-r from-transparent via-white/20 to-transparent
                                -translate-x-full group-hover/btn:translate-x-full transition-transform duration-700" />
                                    
                                <span className="relative z-10" onClick={() => location.href = '/admin/users'}>Просмотр пользователей</span>
                            </button>

                        <button className="w-full relative rounded-xl h-12 bg-linear-to-r from-[#f2994a] to-[#f2c94c] text-white font-bold text-base tracking-wide
                            cursor-pointer hover:scale-[1.02] active:scale-[0.98] transition-all duration-300
                            overflow-hidden group/btn">
                            <div className="absolute inset-0 w-full h-full bg-linear-to-r from-transparent via-white/20 to-transparent
                            -translate-x-full group-hover/btn:translate-x-full transition-transform duration-700" />
                                
                            <span className="relative z-10">Просмотр логов</span>
                        </button>

                        <button className="w-full relative rounded-xl h-12 bg-linear-to-r from-[#660f24] to-[#e5203a] text-white font-bold text-base tracking-wide
                            cursor-pointer hover:scale-[1.02] active:scale-[0.98] transition-all duration-300
                            overflow-hidden group/btn">
                            <div className="absolute inset-0 w-full h-full bg-linear-to-r from-transparent via-white/20 to-transparent
                            -translate-x-full group-hover/btn:translate-x-full transition-transform duration-700" />
                                
                            <span className="relative z-10">Просмотр статистики</span>
                        </button>
                    </div>
                </div>
            </div>
        </>
    )
}