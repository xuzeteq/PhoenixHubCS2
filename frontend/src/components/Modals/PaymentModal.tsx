import { useState } from 'react'
import { AgreePayment } from '../Checkbox/AgreePayment'
import { IconX } from '@tabler/icons-react'
import { motion } from 'framer-motion'

type PaymentMethod = 'sbp' | 'international'

type PaymetModalProps = {
    onClose: () => void
}

const METHODS: { id: PaymentMethod; label: string; hint: string; icon: string }[] = [
    {
        id: 'sbp',
        label: 'СБП / Мир / Mastercard',
        hint: 'Оплата через СБП, картой Мир или Mastercard',
        icon: '🇷🇺',
    },
    {
        id: 'international',
        label: 'Международные карты',
        hint: 'Visa, Mastercard, AmEx и другие',
        icon: '🌍',
    },
]

const BALANCE = ["100", "200", "500", "1000", "2000"];

export default function PaymentModal({ onClose }: PaymetModalProps) {

    const [method, setMethod] = useState('sbp')
    const [balance, setBalance] = useState<string>('100');
    const [agree, setAgree] = useState<boolean>(false)
    const selected = METHODS.find(m => m.id === method);
    const showBalance = Number(balance);

    return (
        <>
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
                className="w-225 h-auto bg-[#111] rounded-2xl p-6"
                onClick={(e) => e.stopPropagation()}>
                        <div className='flex items-center justify-between'>
                            <h1 className='text-white font-bold text-xl'>Пополнение баланса</h1>
                            <button onClick={onClose}
                                className='text-white/40 font-bold text-xl hover:text-white transition'>
                                <IconX />
                            </button>
                        </div>

                        <div className='grid grid-cols-2 gap-4'>
                            <div className='flex items-center gap-2 flex-col mt-4'>
                                {METHODS.map(m => (
                                    <button
                                        key={m.id}
                                        onClick={() => setMethod(m.id)}
                                        className={`w-full flex items-center gap-1 p-3 rounded-lg
                                            border text-left transition-all
                                            ${method === m.id
                                                ? 'border-blue-500 bg-blue-500/30 shadow-[0_0_0_3px_rgba(59,130,246,0.15)]'
                                                : 'border-[#222] bg-[#181818] hover:border-[#333]'
                                            }`}
                                    >
                                        <span className='flex items-center leading-none px-2'>
                                            {m.icon}
                                        </span>
                                        <span className='font-semibold text-white text-lg'>
                                            {m.label}
                                        </span>
                                    </button>
                                ))}
                            </div>

                            <div className='mt-4 p-2 border border-neutral-800 rounded-xl'>
                                <h2 className='text-white/40 px-3 flex items-center justify-between font-semibold p-2 bg-[#161616] rounded-lg'>
                                    <span>Способ оплаты:</span>
                                    <span className='text-white'>{selected?.icon}</span>
                                </h2>
                                
                                <p className='mt-4 text-white/70 font-semibold text-sm'>Сумма пополнения, ₽</p>
                                <input type="text"  placeholder='Сумма оплаты'
                                    value={balance} inputMode='numeric'
                                    onChange={(e) => {
                                        const value = e.target.value
                                        if (value === '' || /^\d+$/.test(value)) {
                                            setBalance(value);
                                        }
                                    }}
                                    
                                    className='bg-[#161616] text-white/40 font-semibold p-2 w-full h-10 rounded-lg mt-2 
                                        focus:ring-0 focus:outline-none focus:text-white
                                        transition-all duration-300'/>
                                
                                <div className='flex items-center justify-between mt-2'>
                                    {BALANCE.map(b => (
                                            <button onClick={() => setBalance(b)}
                                            key={b} className={`py-2 px-4 rounded-full text-sm font-bold bg-[#161616] text-white transition
                                            ${balance === b ? 'border-blue-500 border bg-blue-500/30 shadow-[0_0_0_3px_rgba(59,130,246,0.15)]'
                                            : ''}`}>
                                                {b} ₽
                                            </button>
                                    ))}
                                </div>

                                <h2 className='text-white/40 px-3 flex items-center gap-2 font-semibold py-2 bg-[#161616] mt-4 rounded-lg'>
                                    <span>Получите на баланс:</span>
                                    <span className='text-white'>{showBalance}</span>
                                </h2>

                                <div>
                                    <AgreePayment checked={agree} onChange={() => setAgree(!agree)}/>
                                </div>
                            </div>
                        </div>

                        <button
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