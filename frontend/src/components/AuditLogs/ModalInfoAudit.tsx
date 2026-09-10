import type { AuditLog } from "../../types/Audit"

export function ModalInfoAudit({
    log,
    onClose,
}: {
    log: AuditLog
    onClose: () => void
}) {
    

    const formatDateTime = (iso: string) =>
        new Date(iso).toLocaleString('ru-RU', {
            day: '2-digit',
            month: '2-digit',
            year: 'numeric',
            hour: '2-digit',
            minute: '2-digit',
            second: '2-digit',
        })

    const formatMoney = (amount?: number, currency?: string) => {
        if (amount == null) return '—'
        return `${amount.toLocaleString('ru-RU')} ${currency ?? ''}`.trim()
    }

    return (
        <div
            className="fixed inset-0 bg-black/70 flex items-center justify-center z-50 p-6"
            onClick={onClose}
        >
            <div
                className="bg-[#111111] rounded-lg max-w-3xl w-full max-h-[85vh] overflow-y-auto p-6"
                onClick={(e) => e.stopPropagation()}
            >
                <div className="flex justify-between items-center mb-4">
                    <h2 className="text-white text-xl font-bold">
                        Лог #{log.id}
                    </h2>
                    <button
                        onClick={onClose}
                        className="text-gray-400 hover:text-white text-2xl leading-none"
                    >
                        ×
                    </button>
                </div>

                <div className="grid grid-cols-2 gap-x-6 gap-y-3 text-sm">
                    <Field label="Время" value={formatDateTime(log.timestamp)} />
                    <Field
                        label="Пользователь"
                        value={
                            log.username
                                ? `${log.username} (#${log.userId})`
                                : `#${log.userId ?? '—'}`
                        }
                    />
                    <Field label="Действие" value={log.action} />
                    <Field label="Тип сущности" value={log.entityType} />
                    <Field label="Сущность" value={log.entityName ?? '—'} />
                    <Field label="ID сущности" value={log.entityId ?? '—'} />
                    <Field
                        label="Статус"
                        value={`${log.statusCode} ${log.isSuccess ? '(OK)' : '(FAIL)'}`}
                        valueClass={log.isSuccess ? 'text-green-400' : 'text-red-400'}
                    />
                    {log.amount != null && (
                        <Field
                            label="Сумма"
                            value={formatMoney(log.amount, log.currency)}
                        />
                    )}
                    {log.durationDays != null && (
                        <Field label="Длительность (дней)" value={String(log.durationDays)} />
                    )}
                    {log.validUntil && (
                        <Field label="Действует до" value={formatDateTime(log.validUntil)} />
                    )}
                </div>

                {log.errorMessage && (
                    <div className="mt-4">
                        <div className="text-gray-500 text-xs mb-1">Ошибка</div>
                        <div className="text-red-400 text-sm bg-red-500/10 rounded p-3 font-mono">
                            {log.errorMessage}
                        </div>
                    </div>
                )}

                {(log.oldValue || log.newValue) && (
                    <div className="mt-4 grid grid-cols-2 gap-4">
                        <div>
                            <div className="text-gray-500 text-xs mb-1">Было</div>
                            <pre className="text-gray-300 text-xs bg-[#181818] rounded p-3 overflow-x-auto whitespace-pre-wrap break-all">
                                {log.oldValue ?? '—'}
                            </pre>
                        </div>
                        <div>
                            <div className="text-gray-500 text-xs mb-1">Стало</div>
                            <pre className="text-gray-300 text-xs bg-[#181818] rounded p-3 overflow-x-auto whitespace-pre-wrap break-all">
                                {log.newValue ?? '—'}
                            </pre>
                        </div>
                    </div>
                )}

                {log.metadata && (
                    <div className="mt-4">
                        <div className="text-gray-500 text-xs mb-1">Metadata</div>
                        <pre className="text-gray-300 text-xs bg-[#181818] rounded p-3 overflow-x-auto whitespace-pre-wrap break-all">
                            {log.metadata}
                        </pre>
                    </div>
                )}
            </div>
        </div>
    )
}

export function Field({
    label,
    value,
    valueClass = 'text-white',
}: {
    label: string
    value: string
    valueClass?: string
}) {
    return (
        <div>
            <div className="text-gray-500 text-xs">{label}</div>
            <div className={`${valueClass} break-all`}>{value}</div>
        </div>
    )
}