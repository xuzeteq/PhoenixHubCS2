import { useEffect, useState } from "react";
import Header from "../../components/Header/Header";
import type { User } from "../../types/User";
import { adminUserApi } from "../../api/admin.users.api";
import { RoleBadge } from "../../components/RoleStyle/RoleBadge";

export default function AdminUsersPage() {
    const [users, setUsers] = useState<User[]>([]);

    useEffect(() => {
        adminUserApi.getAllUsers().then(data => setUsers(data));
    }, []);

    return (
        <>
            <div className="ml-55">
                <Header />
            </div>

            <div className="ml-55">
                <div className="w-330 mx-auto">
                    <div className="overflow-hidden rounded-xl">
                        <table className="w-full border-separate border-spacing-0 bg-[#1c1c1c] text-[#e0e0e0]">
                            <thead>
                                <tr className="bg-[#252525]">
                                    <th className="px-5 py-4 text-left text-xs font-semibold uppercase tracking-wide text-white border-b border-[#333]">
                                        ID
                                    </th>
                                    <th className="px-5 py-4 text-left text-xs font-semibold uppercase tracking-wide text-white border-b border-[#333]">
                                        Аватар
                                    </th>
                                    <th className="px-5 py-4 text-left text-xs font-semibold uppercase tracking-wide text-white border-b border-[#333]">
                                        Steam ID
                                    </th>
                                    <th className="px-5 py-4 text-left text-xs font-semibold uppercase tracking-wide text-white border-b border-[#333]">
                                        Никнейм
                                    </th>
                                    <th className="px-5 py-4 text-left text-xs font-semibold uppercase tracking-wide text-white border-b border-[#333]">
                                        Роль
                                    </th>
                                    <th className="px-5 py-4 text-left text-xs font-semibold uppercase tracking-wide text-white border-b border-[#333]">
                                        Баланс
                                    </th>
                                    <th className="px-5 py-4 text-left text-xs font-semibold uppercase tracking-wide text-white border-b border-[#333]">
                                        Дата регистрации
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                                {users.map((user, index) => (
                                    <tr
                                        key={user.id}
                                        className={`${index % 2 === 0 ? 'bg-[#1c1c1c]' : 'bg-[#212121]'} hover:bg-[#2a2a2a] transition-colors`}
                                    >
                                        <td className="px-5 py-3.5 border-b border-[#2a2a2a]">
                                            <span className="text-[#888] font-mono">#{user.id}</span>
                                        </td>
                                        <td className="px-5 py-3.5 border-b border-[#2a2a2a]">
                                            <img
                                                src={user.avatarUrl}
                                                alt={user.username}
                                                className="w-10 h-10 rounded-full object-cover border-2 border-[#333]"
                                            />
                                        </td>
                                        <td className="px-5 py-3.5 border-b border-[#2a2a2a]">
                                            <span className="text-[#66c0f4] font-mono text-sm">
                                                {user.steamId}
                                            </span>
                                        </td>
                                        <td className="px-5 py-3.5 border-b border-[#2a2a2a] font-medium text-white">
                                            {user.username}
                                        </td>
                                        <td className="px-5 py-3.5 border-b border-[#2a2a2a]">
                                            <RoleBadge role={user.roleName}/>
                                        </td>
                                        <td className="px-5 py-3.5 border-b border-[#2a2a2a] font-medium text-white">
                                            {user.balance}
                                        </td>
                                        <td className="px-5 py-3.5 border-b border-[#2a2a2a] text-[#a0a0a0]">
                                            {new Date(user.createdAt).toLocaleDateString('ru-RU', {
                                                day: '2-digit',
                                                month: 'long',
                                                year: 'numeric',
                                            })}
                                        </td>
                                    </tr>
                                ))}
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </>
    );
}