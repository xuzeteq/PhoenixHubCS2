import { useEffect, useState } from "react";
import DonateCard from "../components/Card/DonateCard";
import Header from "../components/Header/Header";
import type { Privilege } from "../types/Privilege";
import { privilegeApi } from "../api/privilege.api";

export default function ShopPage() {

    useEffect(() => {
        document.title = 'Phoenix Hub - Магазин'
    }, [])

    const [privilege, setPrivilege] = useState<Privilege[]>();

    useEffect(() => {
        privilegeApi.getAllPrivileges().then(res => setPrivilege(res))
    }, [])

    return (
        <>
            <div className="ml-55 h-full">

                <Header />

                <section id="#donate-section" className="w-330 mx-auto mt-12 mb-4">

                    <div className="flex justify-center gap-4">
                        {privilege?.map(p => (
                            <DonateCard id={p.id} key={p.id} price={p.price} title={p.title} image={p.imageUrl}
                                features={p.features}/>
                        ))}
                    </div>

                </section>

            </div>
        </>
    )
}