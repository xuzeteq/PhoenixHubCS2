import { IconCircleCheck, IconExclamationCircle } from "@tabler/icons-react";
import Header from "../components/Header/Header";
import { useEffect, useState } from "react";
import { subscribtionApi } from "../api/subscribtion.api";
import { useNavigate } from "react-router-dom";
import axios from "axios";
import { AnimatePresence, motion } from "framer-motion";

type ErrorType = 'HAVENT_MONEY' | 'GENERAL'

interface ErrorInfo {
    type: ErrorType,
    message: string
}

export default function SubscribePage() {

    useEffect(() => {
        document.title = 'Phoenix Hub - подписка'
    })

    const navigate = useNavigate();

    const [error, setError] = useState<ErrorInfo | null>(null)

    const getError = (errorCode?: string, errorMessage?: string): ErrorInfo => {
        switch (errorCode) {
            case "HAVENT_MONEY":
                return {
                    type: 'HAVENT_MONEY',
                    message: 'Недостаточно средств для покупки или продления подписки.'
                }
            default:
                return {
                    type: 'GENERAL',
                    message: errorMessage || 'Ошибка активации промокода',
                };
            }
        }

    useEffect(() => {
        if (!error) return;
        const t = setTimeout(() => setError(null), 2000);
        return () => clearTimeout(t);
    }, [error]);

    const handlePurchase = async () => {
        try {
            setError(null);

            await subscribtionApi.purchaseSubscribtion();
            navigate('/');
        } catch (err) {
            if (axios.isAxiosError(err) && err.response) {
                const errorData = err.response.data;
                setError(getError(errorData.code, errorData.error))
            } else {
                setError(getError(undefined, "Нет соединения с сервером."))
            }
        }
    }

    return (
        <>
            <div className="ml-55 min-h-screen bg-[#141414]">

                <Header />

                <AnimatePresence>
                    {error && (
                        <div className="fixed bottom-0 left-1/2 -translate-x-1/2 z-50 pointer-events-none">
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

                <section className="w-full flex justify-center pt-16 pb-12 px-4">
                    <div className="relative w-full max-w-105 bg-[#161616]/80 backdrop-blur-xl rounded-2xl border border-white/10 overflow-hidden">
                        
                        <div className="absolute -top-12.5 -right-12.5 w-40 h-40 bg-blue-500/20 rounded-full blur-3xl animate-pulse pointer-events-none"></div>
                        <div className="absolute -bottom-7.5 -left-7.5 w-32 h-32 bg-cyan-500/15 rounded-full blur-3xl animate-pulse pointer-events-none" style={{ animationDelay: '1s' }}></div>
                        <div className="absolute top-1/2 left-1/2 -translate-x-1/2 -translate-y-1/2 w-20 h-20 bg-cyan-400/10 rounded-full blur-2xl animate-ping pointer-events-none"></div>

                        <div className="absolute top-0 left-0 w-full h-0.5 bg-linear-to-r from-transparent via-blue-500 to-transparent"></div>

                        <div className="relative z-10 p-8">
                            <div className="flex flex-col items-center gap-4 mb-8">
                                <div className="text-center">
                                    <h2 className="text-white text-2xl uppercase font-bold tracking-wide">Подписка</h2>
                                    <div className="mt-2 flex justify-center">
                                        <p className="bg-linear-to-r from-blue-500 to-cyan-400 px-6 py-1.5 text-white text-lg font-bold uppercase rounded-full shadow-[0_0_25px_rgba(59,130,246,0.5)] animate-pulse">
                                            Phoenix
                                        </p>
                                    </div>
                                </div>
                            </div>

                            <div className="space-y-3 mb-10">
                                {[
                                    "Множитель коинов 50% при игре на серверах",
                                    "Увеличенный шанс на дроп скинов",
                                    "Значок верификации и медаль в профиль",
                                    "Множитель X3 при игре на режиме AFK",
                                    "Изменение ника, тега и чата на сайте"
                                ].map((text, index) => (
                                    <div 
                                        key={index}
                                        className="flex items-center w-full rounded-xl gap-3 bg-white/5 border border-white/5 px-4 py-3.5 
                                        hover:bg-blue-500/10 hover:border-blue-500/30 transition-all duration-300 group cursor-default"
                                    >
                                        <div className="shrink-0 p-1 bg-blue-500/20 rounded-lg group-hover:bg-blue-500/30 transition-colors">
                                            <IconCircleCheck size={20} className="text-blue-400 drop-shadow-[0_0_8px_rgba(59,130,246,0.8)]"/>
                                        </div>
                                        <p className="text-gray-300 text-sm font-medium leading-snug group-hover:text-white transition-colors">
                                            {text}
                                        </p>
                                    </div>
                                ))}
                            </div>

                            <button onClick={handlePurchase}
                                className="w-full relative rounded-xl h-12 bg-linear-to-r from-blue-600 to-cyan-500 text-white font-bold text-base tracking-wide
                                    cursor-pointer hover:shadow-[0_0_35px_rgba(59,130,246,0.5)] hover:scale-[1.02] active:scale-[0.98] transition-all duration-300
                                    overflow-hidden group/btn">
                                    <div className="absolute inset-0 w-full h-full bg-linear-to-r from-transparent via-white/20 to-transparent
                                    -translate-x-full group-hover/btn:translate-x-full transition-transform duration-700" />
                                        
                                    <span className="relative z-10">Приобрести</span>
                            </button>
                            
                            <p className="text-center text-xs text-gray-600 mt-4">
                                Нажимая кнопку, вы соглашаетесь с офертой
                            </p>
                        </div> 
                    </div>
                </section>
            </div>
        </>
    )
}