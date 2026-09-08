import { IconBan, IconUser, IconHeadphonesOff, IconPlayerPlay } from "@tabler/icons-react"

 const statisticCards = [
     {title: 'Блокировок', count: 39, icon: IconBan },
     {title: 'Заглушено', count: 143, icon: IconHeadphonesOff},
     {title: 'Пользователей', count: 942, icon: IconUser},
     {title: 'Сейчас играет', count: 21, icon: IconPlayerPlay},
 ]

export default function StatisticCard() {
    return (
        <>
            {statisticCards.map(item => (
                <>
                    <div className="relative w-64 h-full bg-[#181818] flex items-center gap-2 rounded-xl p-4 cursor-pointer group">
                        <item.icon size={64} className="group-hover:scale-115 transition duration-250 stroke-blue-400/80"/>

                        <div>
                            <h2 className='text-white font-white/60 text-sm'>{item.title}:</h2>
                            <p className='text-white font-semibold text-3xl italic'>{item.count}</p>
                        </div>
                    </div>
                </>
            ))}
        </>
    )
}