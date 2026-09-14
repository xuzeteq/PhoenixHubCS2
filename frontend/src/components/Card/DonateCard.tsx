import { IconCircleCheck } from "@tabler/icons-react";
import ButtonBuy from "../ButtonBuy/ButtonBuy";
import { useState } from "react";
import ConfirmPurchaseModal from "../Modals/ConfirmPurchaseModal";
import type { Feature } from "../../types/Privilege";

interface DonateCardProps {
    id: number;
    title: string;
    price: number;
    oldPrice?: number;
    image: string;
    features: Feature[]
}

export default function DonateCard({ id, title, price, oldPrice, image, features }: DonateCardProps) {

    const [isOpenModal, setIsOpenModal] = useState(false);


    return (
        <>
            <div className="w-full flex flex-col max-w-95 bg-[#161616]/80 backdrop-blur-sm rounded-2xl border border-white/10 overflow-hidden
            hover:shadow-blue-900/20 transition-all duration-500 group">
                
                <div className="relative h-55 overflow-hidden">
                    <img 
                        src={image} 
                        alt={title} 
                        className="w-full h-full object-cover transition-transform duration-700 group-hover:scale-110"
                    />

                    <div className="absolute bottom-0 left-0 right-0 h-24 bg-linear-to-t from-[#161616] to-transparent z-10" />
                </div>

                <div className="flex flex-col items-center pt-6 pb-4 relative z-10">
                    <h2 className="text-white text-2xl font-bold tracking-wide">{title}</h2>
                    <div className="mt-4 h-px w-2/3 bg-linear-to-r from-transparent via-white/10 to-transparent"></div>
                </div>

                <div className="space-y-2.5 px-6 relative z-10">
                    {features.map((item, index) => (
                        <div 
                            key={index} 
                            className="flex items-center gap-3 rounded-xl bg-white/5 border border-white/5 px-4 py-2.5 
                            hover:bg-blue-500/10 hover:border-blue-500/30 transition-all duration-300 group/item cursor-default"
                        >
                            <div className="shrink-0">
                                <IconCircleCheck 
                                    size={18} 
                                    className="text-blue-400 drop-shadow-[0_0_6px_rgba(59,130,246,0.6)]" 
                                />
                            </div>
                            <p className="text-gray-300 text-sm font-medium leading-snug group-hover/item:text-white transition-colors">
                                {item.title}
                            </p>
                        </div>
                    ))}
                </div>

                <div className="mt-auto px-6 pt-4 flex items-center justify-center gap-3 relative z-10">
                    {oldPrice && (
                        <span className="text-gray-500 text-base font-semibold line-through">
                            {oldPrice} ₽
                        </span>
                    )}
                    <p className="text-white text-lg font-bold px-5 py-1.5 bg-blue-500/20 border border-blue-500/30 rounded-full shadow-[0_0_20px_rgba(59,130,246,0.3)]">
                        {price} ₽ 
                        <span className="text-blue-300 font-normal text-sm ml-1">/ Навсегда</span>
                    </p>
                </div>

                <div className="mt-5 px-6 pb-6 relative z-10">
                    <ButtonBuy  isOpen={() => setIsOpenModal(true)}/>
                </div>
            </div>
            
            <div>
                {isOpenModal && (
                    <ConfirmPurchaseModal privilegeId={id} onClose={() => setIsOpenModal(false)} privilegePrice={price} privilegeTitle={title}/>
                )}
            </div>
        </>
    );
}