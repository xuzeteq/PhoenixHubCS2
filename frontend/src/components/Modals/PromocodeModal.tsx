import { useRef, useState } from "react";
import { IconX } from "@tabler/icons-react";
import { promocodeApi } from "../../api/promocde.api";
import axios from "axios";

interface PromocodeModalProps {
  isOpen: boolean;
  onClose: () => void;
}

type ErrorType = 'NOT_FOUND' | 'EXPIRED' | 'USAGE_LIMIT' | 'ALREADY_USED' | 'GENERAL';

interface ErrorInfo {
  type: ErrorType;
  message: string;
}

export default function PromocodeModal({ isOpen, onClose }: PromocodeModalProps) {
  const modalRef = useRef<HTMLDivElement>(null);
  const [code, setCode] = useState('');
  const [error, setError] = useState<ErrorInfo | null>(null);
  const [loading, setLoading] = useState(false);

  const getErrorInfo = (errorCode?: string, errorMessage?: string): ErrorInfo => {
    switch (errorCode) {
      case 'NOT_FOUND':
        return {
          type: 'NOT_FOUND',
          message: 'Промокод не найден',
        };
      case 'EXPIRED':
        return {
          type: 'EXPIRED',
          message: 'Срок действия промокода истёк',
        };
      case 'USAGE_LIMIT':
        return {
          type: 'USAGE_LIMIT',
          message: 'Промокод больше недоступен',
        };
      case 'ALREADY_USED':
        return {
          type: 'ALREADY_USED',
          message: 'Вы уже использовали этот промокод',
        };
      default:
        return {
          type: 'GENERAL',
          message: errorMessage || 'Ошибка активации промокода',
        };
    }
  };

  const handleActivatePromocode = async () => {
    if (!code.trim()) return;

    setLoading(true);
    setError(null);

    try {
      await promocodeApi.activatePromocode(code)
      setCode('');
      onClose();
    } catch (err) {
      if (axios.isAxiosError(err) && err.response) {
        const errorData = err.response.data;
        setError(getErrorInfo(errorData.code, errorData.error));
      } else {
        setError(getErrorInfo(undefined, 'Нет соединения с сервером'));
      }
    } finally {
      setLoading(false);
    }
  };

  const handleBackdropClick = (e: React.MouseEvent) => {
    if (modalRef.current && !modalRef.current.contains(e.target as Node)) {
      onClose();
    }
  };

  if (!isOpen) return null;

  return (
    <div
      className="fixed inset-0 z-50 flex items-center justify-center bg-black/60 backdrop-blur-sm
                 animate-in fade-in duration-200"
      onClick={handleBackdropClick}
    >
      <div
        ref={modalRef}
        className="w-96 bg-[#1c1c1c] rounded-xl p-6 shadow-2xl border border-white/5
                   animate-in zoom-in-95 duration-200 relative"
        onClick={(e) => e.stopPropagation()}
      >
        <button
          onClick={onClose}
          className="absolute top-3 right-3 p-1 rounded-full hover:bg-white/10 
                     transition-colors duration-200 text-white/50 hover:text-white"
        >
          <IconX className="w-5 h-5" />
        </button>

        <div className="flex flex-col">
          <h1 className="text-white font-bold text-2xl italic tracking-wide">
            ПРОМОКОД:
          </h1>

          {error && (
            <div className={`mt-3 p-3 rounded-lg border flex items-start gap-3
                            animate-in slide-in-from-top-2 duration-300
                            ${error.type === 'NOT_FOUND' ? 'bg-red-500/10 border-red-500/30 text-red-400' : ''}
                            ${error.type === 'EXPIRED' ? 'bg-red-500/10 border-red-500/30 text-red-400' : ''}
                            ${error.type === 'USAGE_LIMIT' ? 'bg-red-500/10 border-red-500/30 text-red-400' : ''}
                            ${error.type === 'ALREADY_USED' ? 'bg-red-500/10 border-red-500/30 text-red-400' : ''}
                            ${error.type === 'GENERAL' ? 'bg-red-500/10 border-red-500/30 text-red-400' : ''}`}>
              <p className="text-sm font-medium">{error.message}</p>
            </div>
          )}

          <input
            type="text"
            value={code}
            onChange={(e) => {
              setCode(e.target.value.toUpperCase());
              if (error) setError(null);
            }}
            placeholder="Промокод"
            disabled={loading}
            className={`mt-3 w-full px-4 py-2.5 text-white bg-[#141414] border rounded-lg
                       focus:outline-none transition-all duration-200 placeholder:text-white/30
                       disabled:opacity-50 disabled:cursor-not-allowed
                       ${error 
                         ? 'border-red-500/50 focus:border-red-500 focus:ring-1 focus:ring-red-500/30' 
                         : 'border-neutral-800 focus:border-blue-500/50 focus:ring-1 focus:ring-blue-500/30'}`}
          />

          <button 
            onClick={handleActivatePromocode}
            disabled={loading || !code.trim()}
            className="mt-6 w-full relative rounded-xl h-11 bg-linear-to-r from-blue-600 to-cyan-500 
                             text-white font-bold text-sm tracking-wide
                             cursor-pointer hover:scale-[1.02] active:scale-[0.98] transition-all duration-300
                             overflow-hidden group/btn shadow-lg shadow-blue-500/20
                             disabled:opacity-50 disabled:cursor-not-allowed disabled:hover:scale-100">
            <div className="absolute inset-0 w-full h-full bg-linear-to-r from-transparent via-white/20 to-transparent
                            -translate-x-full group-hover/btn:translate-x-full transition-transform duration-700" />
            <span className="relative z-10 flex items-center justify-center gap-2">
              {loading ? (
                <>
                  <svg className="animate-spin h-4 w-4" viewBox="0 0 24 24">
                    <circle className="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" strokeWidth="4" fill="none" />
                    <path className="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z" />
                  </svg>
                  Активация...
                </>
              ) : (
                'Активировать промокод'
              )}
            </span>
          </button>
        </div>
      </div>
    </div>
  );
}