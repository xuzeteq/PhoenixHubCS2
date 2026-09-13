import { IconExclamationCircle, IconX } from "@tabler/icons-react";
import { motion, AnimatePresence } from "framer-motion";
import { useState, useRef, useEffect } from "react";
import axios from "axios";
import { wheelApi } from "../../api/wheelSpin.api";

type ModalWheelProps = {
    onClose: () => void;
};

type ErrorType = "ALREADY_SPUN" | "GENERAL";

interface ErrorInfo {
    type: ErrorType;
    message: string;
}

const PRIZES = [
    { amount: 5,   color: "#404040", label: "5₽" },
    { amount: 10,  color: "#404040", label: "10₽" },
    { amount: 15,  color: "#404040", label: "15₽" },
    { amount: 25,  color: "#0b5b2b", label: "25₽" },
    { amount: 30,  color: "#0b5b2b", label: "30₽" },
    { amount: 35,  color: "#0b5b2b", label: "35₽" },
    { amount: 50,  color: "#1c3e71", label: "50₽" },
    { amount: 60,  color: "#1c3e71", label: "60₽" },
    { amount: 75,  color: "#502771", label: "75₽" },
    { amount: 125, color: "#ff6900", label: "125₽" },
];

const SIZE = 320;
const RADIUS = SIZE / 2;
const SEGMENT_ANGLE = 360 / PRIZES.length;

function polarToCartesian(cx: number, cy: number, r: number, angleDeg: number) {
    const rad = ((angleDeg - 90) * Math.PI) / 180;
    return { x: cx + r * Math.cos(rad), y: cy + r * Math.sin(rad) };
}

function describeArc(startAngle: number, endAngle: number) {
    const start = polarToCartesian(RADIUS, RADIUS, RADIUS, endAngle);
    const end = polarToCartesian(RADIUS, RADIUS, RADIUS, startAngle);
    const largeArc = endAngle - startAngle <= 180 ? 0 : 1;
    return [
        `M ${RADIUS} ${RADIUS}`,
        `L ${start.x} ${start.y}`,
        `A ${RADIUS} ${RADIUS} 0 ${largeArc} 0 ${end.x} ${end.y}`,
        "Z",
    ].join(" ");
}

export default function ModalWheel({ onClose }: ModalWheelProps) {
    const [spinning, setSpinning] = useState(false);
    const [rotation, setRotation] = useState(0);
    const [result, setResult] = useState<{ amount: number; label: string } | null>(null);
    const [error, setError] = useState<ErrorInfo | null>(null);
    const rotationRef = useRef(0);

    const getErrorInfo = (code?: string, message?: string): ErrorInfo => {
        switch (code) {
            case "ALREADY_SPUN":
                return { type: "ALREADY_SPUN", message: "Вы уже крутили колесо сегодня." };
            default:
                return { type: "GENERAL", message: message || "Ошибка прокрутки колеса." };
        }
    };

    useEffect(() => {
          if (!error) return;
          const t = setTimeout(() => setError(null), 2000);
          return () => clearTimeout(t);
    }, [error]);

    const handleSpin = async () => {
        if (spinning) return;

        setSpinning(true);
        setError(null);
        setResult(null);

        try {
            const res = await wheelApi.spinWheel();

            const prizeIndex = PRIZES.findIndex((p) => p.amount === res.amount);
            if (prizeIndex === -1) {
                setError(getErrorInfo(undefined, `Неизвестный приз: ${res.amount}`));
                setSpinning(false);
                return;
            }

            const segmentCenter = prizeIndex * SEGMENT_ANGLE + SEGMENT_ANGLE / 2;
            const fullSpins = 5 + Math.floor(Math.random() * 4);
            const baseTarget = rotationRef.current + fullSpins * 360 + (360 - segmentCenter);
            const jitter = (Math.random() - 0.5) * (SEGMENT_ANGLE * 0.7);
            const finalRotation = baseTarget + jitter;

            rotationRef.current = finalRotation;
            setRotation(finalRotation);

            setTimeout(() => {
                const prize = PRIZES[prizeIndex];
                setResult({ amount: prize.amount, label: prize.label });
                setSpinning(false);
            }, 4000);
        } catch (err) {
            if (axios.isAxiosError(err) && err.response) {
                const data = err.response.data;
                setError(getErrorInfo(data.code, data.message));
            } else {
                setError(getErrorInfo(undefined, "Нет соединения с сервером"));
            }
            setSpinning(false);
        }
    };

    return (
        <>
            <AnimatePresence>
                {error && (
                    <div className="fixed bottom-0 left-1/2 -translate-x-1/2 z-60 pointer-events-none">
                        <motion.div
                            initial={{ y: 120, opacity: 0 }}
                            animate={{ y: 0, opacity: 1 }}
                            exit={{ y: 120, opacity: 0 }}
                            transition={{ type: "spring", stiffness: 260, damping: 26 }}
                            className="pointer-events-auto mb-4 flex items-center gap-3 p-3 pr-4 rounded-lg border shadow-lg
                                       bg-red-500/10 border-red-500/30 text-red-400"
                        >
                            <IconExclamationCircle className="w-5 h-5 shrink-0" />
                            <p className="text-sm font-medium">{error.message}</p>
                        </motion.div>
                    </div>
                )}
            </AnimatePresence>

            <motion.div
                initial={{ opacity: 0 }}
                animate={{ opacity: 1 }}
                exit={{ opacity: 0 }}
                className="fixed inset-0 bg-black/70 flex items-center justify-center z-50 p-4"
                onClick={onClose}
            >
                <motion.div
                    initial={{ opacity: 0, scale: 0.5, y: 30 }}
                    animate={{ opacity: 1, scale: 1, y: 0 }}
                    exit={{ opacity: 0, scale: 0.8, y: 20 }}
                    className="w-full max-w-md bg-[#111] rounded-2xl p-6"
                    onClick={(e) => e.stopPropagation()}
                >
                    <div className="flex items-center justify-between mb-6">
                        <h1 className="text-white font-bold text-xl">Колесо фортуны</h1>
                        <button onClick={onClose} className="text-white/40 hover:text-white transition">
                            <IconX />
                        </button>
                    </div>

                    <AnimatePresence mode="wait">
                        {result ? (
                            <motion.div
                                key="result"
                                initial={{ opacity: 0, scale: 0.5 }}
                                animate={{ opacity: 1, scale: 1 }}
                                className="flex flex-col items-center gap-4 py-8 text-white"
                            >
                                <div className="text-5xl font-bold text-blue-500">
                                    {result.amount}₽
                                </div>
                                <p className="text-white/60 text-sm">Выигрыш начислен на ваш баланс</p>

                                <button
                                    onClick={onClose}
                                    className="w-full relative rounded-xl h-12 bg-linear-to-r from-blue-600 to-cyan-500 text-white font-bold text-base
                                               tracking-wide cursor-pointer hover:scale-[1.02] active:scale-[0.98] transition-all duration-300
                                               overflow-hidden group/btn"
                                >
                                    <div className="absolute inset-0 w-full h-full bg-linear-to-r from-transparent via-white/20 to-transparent
                                                    -translate-x-full group-hover/btn:translate-x-full transition-transform duration-700" />
                                    <span className="relative z-10">Закрыть</span>
                                </button>
                            </motion.div>
                        ) : (
                            <motion.div
                                key="wheel"
                                initial={{ opacity: 0 }}
                                animate={{ opacity: 1 }}
                                exit={{ opacity: 0 }}
                                className="flex flex-col items-center gap-6"
                            >
                                <div className="relative" style={{ width: SIZE, height: SIZE }}>
                                    <div className="absolute left-1/2 -translate-x-1/2 -top-2 z-20">
                                        <div className="w-0 h-0 border-l-14 border-r-14 border-t-24
                                                        border-l-transparent border-r-transparent border-t-yellow-400 drop-shadow-lg" />
                                    </div>

                                    <motion.div
                                        className="w-full h-full"
                                        animate={{ rotate: rotation }}
                                        transition={{ duration: 4, ease: [0.15, 0.85, 0.25, 1] }}
                                    >
                                        <svg width={SIZE} height={SIZE} viewBox={`0 0 ${SIZE} ${SIZE}`}>
                                            {PRIZES.map((prize, i) => {
                                                const start = i * SEGMENT_ANGLE;
                                                const end = start + SEGMENT_ANGLE;
                                                const centerAngle = start + SEGMENT_ANGLE / 2;
                                                const textPos = polarToCartesian(RADIUS, RADIUS, RADIUS * 0.65, centerAngle);

                                                return (
                                                    <g key={i}>
                                                        <path
                                                            d={describeArc(start, end)}
                                                            fill={prize.color}
                                                            stroke="#111"
                                                            strokeWidth={2}
                                                        />
                                                        <text
                                                            x={textPos.x}
                                                            y={textPos.y}
                                                            textAnchor="middle"
                                                            dominantBaseline="middle"
                                                            fontSize="18"
                                                            fontWeight="700"
                                                            fill="#c8c8c8"
                                                        >
                                                            {prize.label}
                                                        </text>
                                                    </g>
                                                );
                                            })}

                                            <circle cx={RADIUS} cy={RADIUS} r={28} fill="#111" stroke="#333" strokeWidth={2} />
                                        </svg>
                                    </motion.div>
                                </div>

                                <button
                                    onClick={handleSpin}
                                    disabled={spinning}
                                    className="w-full relative rounded-xl h-12 bg-linear-to-r from-blue-600 to-cyan-500 text-white font-bold text-base
                                               tracking-wide cursor-pointer hover:scale-[1.02] active:scale-[0.98] transition-all duration-300
                                               overflow-hidden group/btn disabled:opacity-60 disabled:cursor-not-allowed disabled:hover:scale-100"
                                >
                                    <div className="absolute inset-0 w-full h-full bg-linear-to-r from-transparent via-white/20 to-transparent
                                                    -translate-x-full group-hover/btn:translate-x-full transition-transform duration-700" />
                                    <span className="relative z-10">{spinning ? "Крутится..." : "Прокрутить колесо"}</span>
                                </button>

                                <div className="bg-[#131313] text-lg font-bold text-white w-full rounded-xl p-4">
                                    Шансы по редкости:
                                    <ul className="flex flex-col">
                                        <li className="flex items-center gap-2 mt-2">
                                            <div className="w-2 h-2 rounded-full bg-neutral-700" />
                                            <p className="text-white/60 text-sm font-medium">Обычный — 60%</p>
                                        </li>
                                        <li className="flex items-center gap-2 mt-2">
                                            <div className="w-2 h-2 rounded-full bg-green-500/40" />
                                            <p className="text-white/60 text-sm font-medium">Необычный — 24%</p>
                                        </li>
                                        <li className="flex items-center gap-2 mt-2">
                                            <div className="w-2 h-2 rounded-full bg-blue-500/40" />
                                            <p className="text-white/60 text-sm font-medium">Редкий — 10%</p>
                                        </li>
                                        <li className="flex items-center gap-2 mt-2">
                                            <div className="w-2 h-2 rounded-full bg-purple-500/40" />
                                            <p className="text-white/60 text-sm font-medium">Эпический — 5%</p>
                                        </li>
                                        <li className="flex items-center gap-2 mt-2">
                                            <div className="w-2 h-2 rounded-full bg-orange-500" />
                                            <p className="text-white/60 text-sm font-medium">Легендарный — 1%</p>
                                        </li>
                                    </ul>
                                </div>
                            </motion.div>
                        )}
                    </AnimatePresence>
                </motion.div>
            </motion.div>
        </>
    );
}