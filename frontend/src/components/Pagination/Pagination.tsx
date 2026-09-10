type PaginationProps = {
    page: number
    totalPages: number
    onChange: (page: number) => void
    siblings?: number
}

export default function Pagination({
    page,
    totalPages,
    onChange,
    siblings = 1,
}: PaginationProps) {
    if (totalPages <= 1) return null

    const pages = buildPages(page, totalPages, siblings)

    return (
        <div className="flex justify-center items-center gap-2 mt-4 select-none">
            <button
                disabled={page <= 1}
                onClick={() => onChange(page - 1)}
                className="px-3 py-1 rounded bg-[#222] text-white text-sm disabled:opacity-40 hover:bg-[#2a2a2a]"
            >
                ←
            </button>

            {pages.map((p, i) =>
                p === '...' ? (
                    <span key={`dots-${i}`} className="text-gray-500 text-sm px-1">
                        …
                    </span>
                ) : (
                    <button
                        key={p}
                        onClick={() => onChange(p)}
                        className={`min-w-9 px-3 py-1 rounded text-sm transition-colors ${
                            p === page
                                ? 'bg-blue-600 text-white font-semibold'
                                : 'bg-[#222] text-gray-300 hover:bg-[#2a2a2a]'
                        }`}
                    >
                        {p}
                    </button>
                ),
            )}

            <button
                disabled={page >= totalPages}
                onClick={() => onChange(page + 1)}
                className="px-3 py-1 rounded bg-[#222] text-white text-sm disabled:opacity-40 hover:bg-[#2a2a2a]"
            >
                →
            </button>
        </div>
    )
}

function buildPages(
    current: number,
    total: number,
    siblings: number,
): (number | '...')[] {
    const totalNumbers = siblings * 2 + 5

    if (total <= totalNumbers) {
        return Array.from({ length: total }, (_, i) => i + 1)
    }

    const left = Math.max(current - siblings, 1)
    const right = Math.min(current + siblings, total)

    const showLeftDots = left > 2
    const showRightDots = right < total - 1

    const result: (number | '...')[] = [1]

    if (showLeftDots) result.push('...')
    for (let i = left; i <= right; i++) {
        if (i !== 1 && i !== total) result.push(i)
    }
    if (showRightDots) result.push('...')

    result.push(total)

    return result
}