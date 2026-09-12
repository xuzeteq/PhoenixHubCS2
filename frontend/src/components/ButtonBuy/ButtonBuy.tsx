interface ButtonProps {
    isOpen: () => void
}

export default function ButtonBuy({ isOpen }: ButtonProps) {

    return (
        <button onClick={isOpen}
            className="w-full relative rounded-xl h-12 bg-linear-to-r from-blue-600 to-cyan-500 text-white font-bold text-base tracking-wide
                cursor-pointer hover:shadow-[0_0_35px_rgba(59,130,246,0.5)] hover:scale-[1.02] active:scale-[0.98] transition-all duration-300
                overflow-hidden group/btn">
                <div className="absolute inset-0 w-full h-full bg-linear-to-r from-transparent via-white/20 to-transparent
                -translate-x-full group-hover/btn:translate-x-full transition-transform duration-700" />
                    
                <span className="relative z-10">Приобрести</span>
        </button>
    )
}