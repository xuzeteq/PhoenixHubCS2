interface InfoCardProps {
    title: string,
    result: number,
    icon: React.ElementType
}

export default function AdminInfoCard({title, result, icon: Icon}: InfoCardProps) {
    return (
        <>
            <div className="bg-[#1c1c1c] w-92 h-26 rounded-lg">
                <div className="px-3 py-5">
                    <div className="flex justify-between items-center">
                        <div>
                            <h3 className="text-white/70 text-md">{title}:</h3>
                            <span className="text-4xl text-white font-bold mt-4">{result}</span>
                        </div>

                        <Icon size={48} className="text-white"/>
                    </div>
                    
                </div>
            </div>
        </>
    )
}