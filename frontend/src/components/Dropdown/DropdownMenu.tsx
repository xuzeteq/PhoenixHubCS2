import { useState } from "react";
import { useAuth } from "../../contexts/AuthProvider";
import { Link } from "react-router-dom";
import { IconChevronDown, IconUser, IconLogout, IconUsers, IconBriefcase2, IconTicket, IconStarFilled, IconBrandTabler } from "@tabler/icons-react";
import PromocodeModal from "../Modals/PromocodeModal";

export default function DropdownMenu() {
  const { user, logout } = useAuth();
  const [isOpen, setIsOpen] = useState(false);
  const [isPromocodeModal, setIsPromocodeModal] = useState(false);

  const handleOpenPromocode = () => {
    setIsOpen(false);
    setTimeout(() => {
      setIsPromocodeModal(true);
    }, 100); 
  };

  return (
    <>
      <div className="relative">
        <div className="flex gap-3 items-center">
          <img
            src={user?.avatarUrl}
            alt={user?.username}
            className="w-11 h-11 rounded-full object-cover"
          />
          <div>
            <p className="text-white/40 text-sm font-semibold">{user?.username}</p>
            <p className="text-white text-lg font-bold flex items-center gap-1">
              {user?.balance}₽
            </p>
          </div>

          <button
            className="p-1 rounded-full bg-white/20 cursor-pointer hover:bg-white/30 transition group"
            onClick={() => setIsOpen(!isOpen)}
          >
            <IconChevronDown
              className={`text-white/80 w-5 h-5 transition-transform duration-300 ${
                isOpen ? "rotate-180" : "rotate-0"
              }`}
            />
          </button>
        </div>

        {isOpen && (
          <div
            className="fixed inset-0 z-10"
            onClick={() => setIsOpen(false)}
          />
        )}

        {isOpen && (
          <div className="absolute right-0 mt-2 w-56 bg-[#1c1c1c] rounded-xl shadow-2xl border border-white/15 overflow-hidden z-20">
            <div className="py-2">
              <Link to={`/profile/${user?.steamId}`}>
                <button
                  className="w-full flex items-center gap-3 px-4 py-2.5 text-white/70 hover:text-white
                    hover:bg-white/10 transition-colors duration-200"
                  onClick={() => setIsOpen(false)}
                >
                  <IconUser className="w-4 h-4" />
                  <span className="text-sm">Профиль</span>
                </button>
              </Link>

              {/* 👇 Промокоды */}
              <button
                className="w-full flex items-center gap-3 px-4 py-2.5 text-white/70 hover:text-white
                  hover:bg-white/10 transition-colors duration-200"
                onClick={handleOpenPromocode}
              >
                <IconTicket className="w-4 h-4" />
                <span className="text-sm">Промокоды</span>
              </button>

              <Link to="/subscribtion">
                <button
                  className="w-full flex items-center gap-3 px-4 py-2.5 text-white/70 hover:text-white
                    hover:bg-white/10 transition-colors duration-200"
                  onClick={() => setIsOpen(false)}
                >
                  <IconStarFilled className="w-4 h-4 text-blue-400" />
                  <span className="text-sm font-semibold text-blue-400/80">Подписка</span>
                </button>
              </Link>

              <Link to="/profile">
                <button
                  className="w-full flex items-center gap-3 px-4 py-2.5 text-white/70 hover:text-white
                    hover:bg-white/10 transition-colors duration-200"
                  onClick={() => setIsOpen(false)}
                >
                  <IconBriefcase2 className="w-4 h-4" />
                  <span className="text-sm">Вакансии</span>
                </button>
              </Link>

              <Link to="/referals">
                <button
                  className="w-full flex items-center gap-3 px-4 py-2.5 text-white/70 hover:text-white
                    hover:bg-white/10 transition-colors duration-200"
                  onClick={() => setIsOpen(false)}
                >
                  <IconUsers className="w-4 h-4" />
                  <span className="text-sm">Рефералы</span>
                </button>
              </Link>

              {user?.roleName === 'Владелец' && (
                <Link to="/admin">
                  <button
                    className="w-full flex items-center gap-3 px-4 py-2.5 text-white font-bold hover:text-white
                      hover:bg-white/10 transition-colors duration-200"
                    onClick={() => setIsOpen(false)}
                  >
                    <IconBrandTabler className="w-4 h-4" />
                    <span className="text-sm">Админ-панель</span>
                  </button>
                </Link>
              )}

              <div className="border-t border-white/10" />

              <div className="pt-2">
                <button
                  className="w-full flex items-center gap-3 px-4 pt-2 pb-2 text-red-400 hover:text-red-300
                    hover:bg-red-500/10 transition-colors duration-200"
                  onClick={() => {
                    setIsOpen(false);
                    logout();
                  }}
                >
                  <IconLogout className="w-4 h-4" />
                  <span className="text-sm">Выйти</span>
                </button>
              </div>
            </div>
          </div>
        )}
      </div>

      <PromocodeModal 
        isOpen={isPromocodeModal} 
        onClose={() => setIsPromocodeModal(false)} 
      />
    </>
  );
}