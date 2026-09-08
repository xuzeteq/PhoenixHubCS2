import { useNavigate } from "react-router-dom"
import Header from "../components/Header/Header"
import { useEffect } from "react";

export default function NotFoundPage() {

    const navigate = useNavigate();

    useEffect(() => {
        document.title = 'Phoenix Hub - страница не найдена'
    })

    return (
        <>
            <div className="ml-55">
                
                <section id="#not-found-section" className="w-330 mx-auto">

                    <Header />

                    <div className="bg-[#181818] rounded-2xl p-4 mt-8 cursor-pointer" onClick={() => navigate('/') }>
                        <h2 className="text-white text-2xl font-bold text-center">Упс, а здесь ничего нет!</h2>
                        <p className="text-white/30 text-lg font-semibold text-center">Нажмите на баннер для перехода на главную страницу.</p>
                    </div>

                </section>

            </div>
        </>
    )
}