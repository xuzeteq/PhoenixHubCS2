export default function PingCircle() {
    return (
        <>
            <span className="relative flex h-2 w-2">
                <span className="animate-ping absolute inline-flex bg-blue-400 h-full w-full rounded-full opacity-70"></span>
                <span className="relative h-2 w-2 inline-flex rounded-full bg-blue-400"></span>
            </span>
        </>
    )
}