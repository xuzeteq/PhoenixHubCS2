import PingCircle from "../PingCircle/PingCircle";

interface CardProps {
    title: string;
    players: number;
    bgUrl: string;
    isSoon?: boolean;
};

export default function Card({ title, players, bgUrl, isSoon }: CardProps) {
    const isOnline = players > 0;

    return (
        <div className={`relative w-full max-w-55 aspect-square rounded-2xl overflow-hidden group cursor-pointer select-none border-2 transition-colors duration-500 ${
            isSoon 
                ? 'border-white/5 grayscale hover:grayscale-0' 
                : 'border-white/10 hover:border-white/30'
        }`}>
            
            <div className="absolute inset-0 bg-neutral-800" />
            
            {bgUrl !== "none" && (
                <img 
                    src={bgUrl} 
                    alt={title} 
                    className="absolute inset-0 w-full h-full object-cover transition-transform duration-700 ease-out group-hover:scale-125"
                />
            )}

            <div className="absolute inset-0 bg-linear-to-t from-black/80 via-black/30 to-transparent transition-all duration-500 group-hover:from-black/90" />

            {isSoon && (
                <div className="absolute top-4 left-1/2 -translate-x-1/2 z-30 bg-white/10 text-white text-xs font-bold uppercase px-4 py-1.5 rounded-full tracking-widest">
                    Скоро
                </div>
            )}

            <div className="absolute bottom-0 left-0 right-0 p-5 z-20 transition-all duration-500 ease-in-out group-hover:translate-y-16 group-hover:opacity-0">
                <p className="text-white font-bold text-2xl">{title}</p>
                {!isSoon && (
                    <div className="flex items-center gap-2 mt-1">
                        <PingCircle />
                        <p className="text-white/60 text-sm">
                            {isOnline ? `${players} игроков` : 'Нет игроков'}
                        </p>
                    </div>
                )}
            </div>
        </div>
    );
}