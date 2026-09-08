interface Props {
  role: string;
}

const ROLE_COLORS: Record<string, string> = {
  Владелец: 'bg-purple-500 text-white',
  Администратор: 'bg-red-500 text-white',
  Модератор: 'bg-blue-500 text-white',
  Phoenix: 'bg-yellow-500/30 text-yellow-500',
  Пользователь: 'bg-neutral-500 text-white'
};

export const RoleBadge = ({ role }: Props) => {
  return (
    <span className={`px-3 py-1 rounded-full text-sm font-bold ${ROLE_COLORS[role]}`}>
      {role}
    </span>
  );
};