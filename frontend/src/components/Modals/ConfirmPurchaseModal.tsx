import { IconExclamationCircle, IconX } from "@tabler/icons-react";
import { AnimatePresence, motion } from "framer-motion";
import { useEffect, useState } from "react";
import { AgreePayment } from "../Checkbox/AgreePayment";
import { privilegeApi } from "../../api/privilege.api";
import axios from "axios";

type ConfirmPurchaseModalProps = {
    onClose: () => void
    privilegeTitle: string
    privilegeId: number
    privilegePrice: number
}

type ErrorType = 'HAVENT_MONEY' | 'ALREADY_PURCHASED' | 'GENERAL';

interface ErrorInfo {
  type: ErrorType;
  message: string;
}

export default function ConfirmPurchaseModal({ onClose, privilegeTitle, privilegePrice, privilegeId }: ConfirmPurchaseModalProps) {

    const [agree, setAgree] = useState(false);

        const [error, setError] = useState<ErrorInfo | null>(null);

        const getErrorInfo = (errorCode?: string, errorMessage?: string): ErrorInfo => {
        switch (errorCode) {
        case 'HAVENT_MONEY':
            return {
            type: 'HAVENT_MONEY',
            message: 'Недостаточно средств для покупки привилегии.',
            };
        case 'ALREADY_PURCHASED':
            return {
            type: 'ALREADY_PURCHASED',
            message: 'Вы уже имеете эту привилегию.',
            };
        default:
            return {
            type: 'GENERAL',
            message: errorMessage || 'Ошибка покупки привилегии',
            };
        }
    };

    useEffect(() => {
        if (!error) return;
        const t = setTimeout(() => setError(null), 2000);
        return () => clearTimeout(t);
    }, [error]);

    const handlePurchase = async () => {
        setError(null);

        try {
            await privilegeApi.purchasePrivilege(privilegeId);
            onClose();

        } catch (err) {
            if (axios.isAxiosError(err) && err.response) {
                const errorData = err.response.data;
                setError(getErrorInfo(errorData.code, errorData.error));
            } else {
                setError(getErrorInfo(undefined, 'Нет соединения с сервером'));
            }
        }
    }

    return (
        <>
            <AnimatePresence>
                {error && (
                    <div className="fixed bottom-0 left-1/2 -translate-x-1/2 z-90 pointer-events-none">
                    <motion.div
                        initial={{ y: 120, opacity: 0 }}
                        animate={{ y: 0, opacity: 1 }}
                        exit={{ y: 120, opacity: 0 }}
                        transition={{ type: 'spring', stiffness: 260, damping: 26 }}
                        className="pointer-events-auto mb-4
                                flex items-center gap-3 p-3 pr-4 rounded-lg border shadow-lg
                                bg-red-500/10 border-red-500/30 text-red-400"
                    >
                        <div className="w-6 h-6 flex items-center justify-center shrink-0 mt-0.5">
                        <IconExclamationCircle />
                        </div>
                        <p className="text-sm font-medium">{error.message}</p>
                    </motion.div>
                    </div>
                )}
            </AnimatePresence>

            <motion.div
            initial={{ opacity: 0 }}
            animate={{ opacity: 1 }}
            exit={{ opacity: 0 }}
            className='fixed inset-0 bg-black/70 flex items-center justify-center z-50 p-4'
            onClick={onClose}>

            <motion.div
                initial={{ opacity: 0, scale: 0.5, y: 30 }}
                animate={{ 
                    opacity: 1, 
                    scale: 1, 
                    y: 0,
                    transition: {
                        type: "spring",
                        stiffness: 300,
                        damping: 20
                    }
                }}
                exit={{ opacity: 0, scale: 0.8, y: 20 }}
                className="w-120 h-auto bg-[#111] rounded-2xl p-6"
                onClick={(e) => e.stopPropagation()}>
                        <div className='flex items-center justify-between'>
                            <h1 className='text-white font-bold text-xl'>Оформление покупки</h1>
                            <button onClick={onClose}
                                className='text-white/40 font-bold text-xl hover:text-white transition'>
                                <IconX />
                            </button>
                        </div>

                        <div className="p-4 bg-[#181818] rounded-lg border border-neutral-800 mt-2 flex items-center justify-between">
                            <h1 className="text-white font-semibold">{privilegeTitle}</h1>
                            <p className="text-white text-sm">{privilegePrice} ₽</p>
                        </div>

                        <div>
                            <AgreePayment checked={agree} onChange={() => setAgree(!agree)}/>
                        </div>

                        <button
                            onClick={handlePurchase}
                            disabled={!agree}
                            className="w-full mt-4 relative rounded-xl h-12
                                bg-linear-to-r from-blue-600 to-cyan-500
                                disabled:from-blue-600/50 disabled:to-cyan-500/50
                                disabled:cursor-not-allowed
                                text-white font-bold text-base tracking-wide
                                cursor-pointer transition-all duration-300
                                enabled:hover:scale-[1.02]
                                enabled:active:scale-[0.98]
                                overflow-hidden group/btn"
                        >
                            <div className="absolute inset-0 w-full h-full
                                bg-linear-to-r from-transparent via-white/20 to-transparent
                                -translate-x-full transition-transform duration-700
                                group-enabled/btn:group-hover/btn:translate-x-full" />

                            <span className="relative z-10">Приобрести</span>
                        </button>
            </motion.div>
        </motion.div>
        </>
    )
}