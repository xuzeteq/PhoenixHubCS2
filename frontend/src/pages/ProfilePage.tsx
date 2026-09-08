import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import Header from "../components/Header/Header";
import { useAuth } from "../contexts/AuthProvider";
import { RoleBadge } from "../components/RoleStyle/RoleBadge";
import { IconCircleCheck, IconLoader } from "@tabler/icons-react";
import { usersApi } from "../api/users.api";
import type { User } from "../types/User";

export default function ProfilePage() {
    const { steamId } = useParams<{ steamId?: string }>();
    const { user: currentUser, refreshUser } = useAuth();
    const [activeTab, setActiveTab] = useState<"info" | "services" | "inventory">("info");
    const [profileUser, setProfileUser] = useState<User | null>(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

    const isOwnProfile = !steamId || (currentUser && currentUser.steamId === steamId);

    const formatDate = (date: string | undefined) => {
        if (!date) return "Не указана";
        return new Date(date).toLocaleDateString("ru-RU", {
            day: "2-digit",
            month: "2-digit",
            year: "numeric"
        });
    };

    useEffect(() => {
        document.title = 'Phoenix Hub - профиль пользователя';
        
        const fetchProfile = async () => {
            try {
                setLoading(true);
                setError(null);

                if (isOwnProfile) {
                    if (currentUser) {
                        setProfileUser(currentUser);
                    } else {
                        await refreshUser();
                    }
                } else if (steamId) {
                    const user = await usersApi.getProfile(steamId);
                    setProfileUser(user);
                }
            } catch (err) {
                setError("Не удалось загрузить профиль пользователя");
                console.error(err);
            } finally {
                setLoading(false);
            }
        };

        void fetchProfile();
    }, [steamId, currentUser, refreshUser, isOwnProfile]);

    const displayUser = profileUser || currentUser;

    if (loading) {
        return (
            <>
                <div className="ml-55">
                    <Header />
                </div>
                <div className="ml-55 flex items-center justify-center mt-20">
                    <IconLoader className="animate-spin text-white/60" size={40} />
                </div>
            </>
        );
    }

    if (error || !displayUser) {
        return (
            <>
                <div className="ml-55">
                    <Header />
                </div>
                <div className="ml-55 flex items-center justify-center mt-20">
                    <p className="text-white/60 text-xl">{error || "Пользователь не найден"}</p>
                </div>
            </>
        );
    }

    return (
        <>
            <div className="ml-55">
                <Header />
            </div>  

            <div className="ml-55">
                <div className="mt-4 w-330 mx-auto rounded-2xl overflow-hidden">

                    <div>
                        <div className="flex items-center gap-2">
                            <img src={displayUser.avatarUrl} alt="" className="w-27 h-27 rounded-full"/>

                            <div>
                                <p className="text-white/40">{displayUser.steamId}</p>
                                <div className="flex gap-2 items-center pb-2">
                                    <h2 className="text-white text-3xl font-bold">{displayUser.username}</h2>
                                    {displayUser.isVerify ? (
                                        <IconCircleCheck title="Верифицированный пользователь"
                                        size={20} className="text-blue-400"/>
                                    ) : (
                                        ''
                                    )}
                                </div>
                            </div>
                        </div>
                    </div>

                    <div className="mt-6 bg-neutral-900/50 backdrop-blur-sm rounded-2xl border border-neutral-800 overflow-hidden">
                        <div className="flex border-b border-neutral-800">
                            <button
                                onClick={() => setActiveTab("info")}
                                className={`px-6 py-3 text-lg font-bold transition-colors relative group ${
                                    activeTab === "info" 
                                        ? "text-white" 
                                        : "text-white/40 hover:text-white/70"
                                }`}
                            >
                                Информация
                            </button>
                            <button
                                onClick={() => setActiveTab("services")}
                                className={`px-6 py-3 text-lg font-bold transition-colors relative ${
                                    activeTab === "services" 
                                        ? "text-white" 
                                        : "text-white/40 hover:text-white/70"
                                }`}
                            >
                                Услуги
                            </button>
                            <button
                                onClick={() => setActiveTab("inventory")}
                                className={`px-6 py-3 text-lg font-bold transition-colors relative ${
                                    activeTab === "inventory" 
                                        ? "text-white" 
                                        : "text-white/40 hover:text-white/70"
                                }`}
                            >
                                Инвентарь
                            </button>
                        </div>

                        {activeTab === 'info' && (
                            <div className="my-6">
                                <h2 className="text-white text-xl font-bold italic px-4">Информация:</h2>

                                <div className="mt-4 px-2">
                                    <div className="flex justify-between my-2 bg-[#1c1c1c] rounded-2xl w-full py-4 px-4">
                                        <h2 className="text-white/70">ID:</h2>
                                        <p className="text-white/80 text-md">{displayUser.id}</p>
                                    </div>

                                    <div className="flex justify-between my-2 bg-[#1c1c1c] rounded-2xl w-full py-4 px-4">
                                        <h2 className="text-white/70">Steam ID:</h2>
                                        <p className="text-white/80 text-md">{displayUser.steamId}</p>
                                    </div>

                                    <div className="flex justify-between my-2 bg-[#1c1c1c] rounded-2xl w-full py-4 px-4">
                                        <h2 className="text-white/70">Роль:</h2>
                                        <p className="text-white/80 text-md"><RoleBadge role={displayUser.roleName || 'Пользователь'}/></p>
                                    </div>

                                    <div className="flex justify-between my-2 bg-[#1c1c1c] rounded-2xl w-full py-4 px-4">
                                        <h2 className="text-white/70">Дата регистрации:</h2>
                                        <p className="text-white/80 text-md">{formatDate(displayUser.createdAt)}</p>
                                    </div>
                                </div>
                            </div>
                        )}

                        {activeTab === 'services' && (
                            <div className="my-6">
                                <h2 className="text-white text-xl font-bold italic px-4">Услуги:</h2>

                                {displayUser.subscribtionExpireAt ? (
                                    <div className="mt-4 px-2">
                                        <div className="flex justify-between my-2 bg-[#1c1c1c] rounded-2xl w-full py-4 px-4">
                                            <h2 className="text-white/70">Подписка Phoenix:</h2>
                                            <p className="text-white/80 text-md">{formatDate(displayUser.subscribtionExpireAt)}</p>
                                        </div>
                                    </div>
                                ) : (
                                    <div className="mt-2 px-4">
                                        <p className="text-white/70 text-md">Нет активных услуг</p>
                                    </div>
                                )}
                            </div>
                        )}

                        {activeTab === 'inventory' && (
                            <div className="my-6">
                                <h2 className="text-white text-xl font-bold italic px-4">Инвентарь:</h2>

                                <div className="mt-2 mx-4 bg-[#1c1c1c] rounded-xl">
                                    <div className="px-12 rounded">
                                        {isOwnProfile ? (
                                            <p className="text-center text-white/80 font-bold py-8"> Ваш инвентарь пустой</p>
                                        ) : (
                                            <p className="text-center text-white/80 font-bold py-8"> Инвентарь пользователя пустой</p>
                                        )}
                                    </div>
                                </div>
                            </div>
                        )}
                    </div>

                </div>
            </div>
        </>
    )
}