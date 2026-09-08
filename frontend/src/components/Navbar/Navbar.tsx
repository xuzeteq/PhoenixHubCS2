import { IconHome, IconShoppingCart, IconCrown, IconBook2, IconLayoutDashboard, IconCalendarTime } from '@tabler/icons-react';
import { NavLink } from 'react-router-dom';

export default function Navbar() {

    const navLinks = [
        { to: '/', label: 'Главная', icon: IconHome},
        { to: '/shop', label: 'Магазин', icon: IconShoppingCart},
        { to: '/subscribtion', label: 'Подписка', icon: IconCrown},
        { to: '/rules', label: 'Правила', icon: IconBook2},
        { to: '/skinchanger', label: 'Скинченджер', icon: IconLayoutDashboard},
        { to: '/battle-pass', label: 'Боевой-пропуск', icon: IconCalendarTime},
    ]

    return (
        <>
            <div className="fixed w-55 h-full overflow-y-auto min-h-screen border-r border-neutral-800 bg-[#131313] flex flex-col">

                <div className='flex items-center gap-2 justify-center py-4'>
                    <img src="logo.png" alt="" className='w-8'/>
                    <h1 className='font-bold text-white text-lg'>PHOENIX HUB</h1>

                </div>

                <div className="flex justify-center">
                    <div className="border border-neutral-800 w-48"/>
                </div>

                <nav className="flex flex-1 flex-col px-2">

                    {navLinks.map(link => (
                        <NavLink
                            key={link.to}
                            to={link.to}
                            end
                            className={({ isActive }) => 
                                `flex items-center text-sm font-medium gap-3 text-white/80 my-0.5 py-3 px-4 rounded-2xl
                    hover:bg-white/10 group transition
                                ${isActive 
                                    ? 'bg-white/5 text-white font-medium' 
                                    : 'text-white/50 hover:bg-white/5 hover:text-white/80'
                                }`
                            }
                            
                        >
                        <link.icon size={24} stroke={2} className='stroke-white/80 group-hover:stroke-white transition-all group-hover:scale-115' />
                        <span>{link.label}</span>
                        </NavLink>
                    ))}
                </nav>

                <div className="flex justify-center">
                    <div className="border border-neutral-800 w-48"/>
                </div>

                <div className='flex gap-4 items-center justify-center mt-auto py-4'>
                    <a href="" className='hover:bg-white/10 p-2 rounded-full transition group'>
                        <svg className='stroke-white/80 group-hover:stroke-blue-400 transition' xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                            <path stroke="none" d="M0 0h24v24H0z" fill="none" />
                            <path d="M14 19h-4a8 8 0 0 1 -8 -8v-5h4v5a4 4 0 0 0 4 4v-9h4v4.5l.03 0a4.531 4.531 0 0 0 3.97 -4.496h4l-.342 1.711a6.858 6.858 0 0 1 -3.658 4.789a5.34 5.34 0 0 1 3.566 4.111l.434 2.389h-4a4.531 4.531 0 0 0 -3.97 -4.496v4.5l-.03 -.008" />
                        </svg>
                    </a>

                    <a href="" className='hover:bg-white/10 p-2 rounded-full transition group'>
                        <svg className='stroke-white/80 group-hover:stroke-blue-400 transition' xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                            <path stroke="none" d="M0 0h24v24H0z" fill="none" />
                            <path d="M15 10l-4 4l6 6l4 -16l-18 7l4 2l2 6l3 -4" />
                        </svg>
                    </a>

                    <a href="" className='hover:bg-white/10 p-2 rounded-full transition group'>
                        <svg className='stroke-white/80 group-hover:stroke-blue-400 transition' xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                            <path stroke="none" d="M0 0h24v24H0z" fill="none" />
                            <path d="M8 12a1 1 0 1 0 2 0a1 1 0 0 0 -2 0" />
                            <path d="M14 12a1 1 0 1 0 2 0a1 1 0 0 0 -2 0" />
                            <path d="M15.5 17c0 1 1.5 3 2 3c1.5 0 2.833 -1.667 3.5 -3c.667 -1.667 .5 -5.833 -1.5 -11.5c-1.457 -1.015 -3 -1.34 -4.5 -1.5l-.972 1.923a11.913 11.913 0 0 0 -4.053 0l-.975 -1.923c-1.5 .16 -3.043 .485 -4.5 1.5c-2 5.667 -2.167 9.833 -1.5 11.5c.667 1.333 2 3 3.5 3c.5 0 2 -2 2 -3" />
                            <path d="M7 16.5c3.5 1 6.5 1 10 0" />
                        </svg>
                    </a>
                </div>
            </div>
        </>
    )
}