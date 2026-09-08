import { useEffect, useState, useMemo } from "react"
import type { Server } from "../types/Server"
import { serverApi } from "../api/servers.api"

import Header from "../components/Header/Header"
import Information from "../components/Information/Information"
import News from "../components/News/News"
import StatisticCard from "../components/Card/StatisticCard"
import Card from "../components/Card/Card"
import { Link } from "react-router-dom"
export default function HomePage() {

    const [servers, setServers] = useState<Server[]>([]);
        
        const totalOnline = useMemo(() => {
            return servers.reduce((sum, server) => sum + server.online, 0);
        }, [servers]);

        useEffect(() => {
            document.title = 'Phoenix Hub - игровые сервера CS2'
            try {
                serverApi.getAllServers().then(data => setServers(data))
            }
            catch (ex) {
                console.error(ex)
            }
        }, [])

    return (
        <div className="flex flex-col min-h-screen ml-55 bg-[#141414]">
            <Header />

            <main className="flex-1 w-330 mx-auto px-8">
                
                <Information />

                <section id="news-section" className="mt-8">
                    <News />

                    
                </section>

                <section id="card-section">
                    <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-5 gap-6 justify-items-center mt-8">
                        <Link to={"/servers/public"} className="w-full h-full">
                            <Card title="PUBLIC" players={totalOnline} bgUrl="public.jpg"/>
                        </Link>

                        <Card title="DM" players={0} bgUrl='mirage.png' isSoon/>
                        <Card title="BHOP" players={0} bgUrl="bhop.jpg" isSoon/>
                        <Card title="MANIAC" players={0} bgUrl="maniac.jpg" isSoon/>
                        <Card title="SURF" players={0} bgUrl="none" isSoon/>
                    </div>
                </section>

                

                <section id="statistics">
                    <div className="flex justify-center gap-4 mt-8">
                        <StatisticCard />
                    </div>
                </section>

            </main>
        </div>
    )
}