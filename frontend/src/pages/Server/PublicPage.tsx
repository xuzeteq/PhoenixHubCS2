import { useEffect, useState } from "react";
import ServerCard from "../../components/Card/ServerCard";
import Header from "../../components/Header/Header";
import type { Server } from "../../types/Server";
import { serverApi } from "../../api/servers.api";

export default function PublicPage() {

    const [servers, setServers] = useState<Server[]>([]);

    useEffect(() => {
        try {
            serverApi.getAllServers().then(data => setServers(data))
        }
        catch (ex) {
            console.error(ex)
        }
    }, [])



    return (
        <>
            <div className="ml-55">
                <Header />



                <section id="#public-section" className="w-330 mx-auto bg-[#141414] mt-10">
                    
                    <h2 className="text-3xl italic font-bold text-white">PUBLIC:</h2>

                    <div className="flex flex-wrap gap-4">
                        {servers.map(serv => (
                            <ServerCard 
                                key={serv.id}
                                server={serv}
                            />
                        ))}
                    </div>

                </section>
            </div>
        </>
    )
}