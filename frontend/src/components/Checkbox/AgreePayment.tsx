export function AgreePayment({
    checked,
    onChange,
}: {
    checked: boolean
    onChange: (checked: boolean) => void
}) {
    return (
        <div className="flex items-start gap-3 mt-4">
            <button
                type="button"
                onClick={() => onChange(!checked)}
                className={`mt-0.5 w-5 h-5 shrink-0 rounded border-2 flex items-center justify-center transition-colors
                    ${checked
                        ? 'border-blue-500 bg-blue-500'
                        : 'border-[#333] bg-[#161616] hover:border-[#444]'
                    }`}
            >
                {checked && (
                    <svg
                        viewBox="0 0 24 24"
                        className="w-3.5 h-3.5 text-white"
                        fill="none"
                        stroke="currentColor"
                        strokeWidth="3"
                        strokeLinecap="round"
                        strokeLinejoin="round"
                    >
                        <polyline points="20 6 9 17 4 12" />
                    </svg>
                )}
            </button>

            <label
                onClick={() => onChange(!checked)}
                className="text-xs text-gray-500 leading-snug cursor-pointer select-none"
            >
                Я согласен с{' '}
                <a
                    href="/terms"
                    target="_blank"
                    rel="noopener noreferrer"
                    onClick={(e) => e.stopPropagation()}
                    className="text-blue-400 hover:text-blue-300 underline"
                >
                    условиями оферты
                </a>{' '}
                и{' '}
                <a
                    href="/privacy"
                    target="_blank"
                    rel="noopener noreferrer"
                    onClick={(e) => e.stopPropagation()}
                    className="text-blue-400 hover:text-blue-300 underline"
                >
                    политикой конфиденциальности
                </a>
            </label>
        </div>
    )
}