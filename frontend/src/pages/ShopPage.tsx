import { useEffect } from "react";
import DonateCard from "../components/Card/DonateCard";
import Header from "../components/Header/Header";

export default function ShopPage() {

    useEffect(() => {
        document.title = 'Phoenix Hub - Магазин'
    }, [])

    return (
        <>
            <div className="ml-55 h-full">

                <Header />

                <section id="#donate-section" className="w-330 mx-auto mt-12 mb-4">

                    <div className="flex items-center justify-center gap-4">
                        <DonateCard title="PREMIUM" price={249} image="premium.png" oldPrice={499} />
                        <DonateCard title="ELITE" price={749} image="admin.png" oldPrice={999}/>
                        <DonateCard title="ULTRA" price={1499} image="cs2.png" oldPrice={1999}/>
                    </div>

                </section>

            </div>
        </>
    )
}