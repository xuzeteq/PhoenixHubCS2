import { useEffect, useState } from 'react';
import {
  IconShield,
  IconMessage,
  IconSwords,
  IconGavel,
  IconAlertCircle,
  IconCheck,
} from '@tabler/icons-react';
import Header from '../components/Header/Header';

export default function RulesPage() {

  useEffect(() => {
    document.title = 'Phoenix Hub - правила серверов'
  })

  const [activeTab, setActiveTab] = useState('general');

  const tabs = [
    { id: 'general', label: 'Общие правила', icon: IconShield },
    { id: 'gameplay', label: 'Игровой процесс', icon: IconSwords },
    { id: 'chat', label: 'Чат и общение', icon: IconMessage },
    { id: 'admin', label: 'Администрация', icon: IconGavel },
  ];

  const rulesData = {
    general: [
      { text: 'Запрещено использование читов и стороннего ПО' },
      { text: 'Запрещено оскорбление игроков и администрации' },
      { text: 'Запрещено использование багов и ошибок карт' },
      { text: 'Запрещено намеренное создание задержек (лаг) игры' },
      { text: 'Запрещена реклама сторонних ресурсов без разрешения' },
      { text: 'Запрещено использование нецензурной лексики в любом виде' },
    ],
    gameplay: [
      { text: 'Запрещено покидание игры во время раунда' },
      { text: 'Запрещено блокирование союзников (team blocking)' },
      { text: 'Запрещено намеренное убийство союзников (team killing)' },
      { text: 'Запрещено использование нестандартных конфигов' },
      { text: 'Запрещено подглядывание за противниками (ghosting)' },
      { text: 'Запрещено использование недокументированных возможностей' },
    ],
    chat: [
      { text: 'Запрещено оскорбление других игроков в чате' },
      { text: 'Запрещено использование капса (заглавные буквы)' },
      { text: 'Запрещен спам и флуд в текстовом/голосовом чате' },
      { text: 'Запрещено обсуждение политических и религиозных тем' },
      { text: 'Запрещено использование голосового чата без необходимости' },
      { text: 'Запрещено распространение личной информации игроков' },
    ],
    admin: [
      { text: 'Администраторы имеют полное право наказания' },
      { text: 'Решение администратора окончательно и обжалованию не подлежит' },
      { text: 'Запрещено оспаривание действий администрации' },
      { text: 'Администраторы имеют право изменять правила в любое время' },
      { text: 'Запрещено обсуждение действий администрации в общем чате' },
      { text: 'Администраторы имеют право наказывать за нарушения без предупреждения' },
    ],
  };

  return (
    <>
        <div className='ml-55'>
            <Header />
        </div>

        <div className="ml-55 mt-6 bg-[#141414] flex items-center justify-center p-6">        
        <div className="w-330 max-w-4xl mx-auto bg-[#181818] rounded-2xl border border-[#2a2a2a] overflow-hidden">
            <div className="bg-linear-to-r from-blue-500 to-blue-600 p-6 border-b border-blue-400/20">
            <div className="flex items-center gap-3">
                <IconShield className="text-white h-8 w-8" />
                <div>
                <h1 className="text-2xl font-bold text-white">Правила сервера</h1>
                <p className="text-blue-100 text-sm opacity-90">Для комфортной игры соблюдайте правила</p>
                </div>
            </div>
            </div>

            <div className="flex border-b border-[#2a2a2a] bg-[#1a1a1a]">
            {tabs.map((tab) => {
                const isActive = activeTab === tab.id;
                const Icon = tab.icon;
                return (
                <button
                    key={tab.id}
                    onClick={() => setActiveTab(tab.id)}
                    className={`
                    flex items-center justify-center gap-2 px-5 py-3 text-sm font-medium transition-all duration-200 relative
                    ${isActive 
                        ? 'text-white bg-blue-500/10' 
                        : 'text-gray-400 hover:text-gray-200 hover:bg-[#222222]'
                    }
                    ${isActive ? 'border-b-2 border-blue-500' : 'border-b-2 border-transparent'}
                    `}
                >
                    <Icon className={`h-4 w-4 ${isActive ? 'text-blue-400' : 'text-gray-500'}`} />
                    <span>{tab.label}</span>
                </button>
                );
            })}
            </div>

            <div className="p-6 bg-[#181818]">
            <div className="space-y-3">
                {rulesData[activeTab as keyof typeof rulesData].map((rule, index) => (
                <div
                    key={index}
                    className="flex items-center gap-3 p-4 rounded-lg bg-[#1a1a1a] border border-[#2a2a2a] hover:border-[#333333] transition-all duration-200"
                >
                    <div className="shrink-0 flex items-center justify-center">
                    <IconAlertCircle className="h-5 w-5 text-blue-400" />
                    </div>
                    <div className="flex-1 flex items-center">
                    <p className="text-sm text-gray-300">
                        {rule.text}
                    </p>
                    </div>
                    <div className="shrink-0 flex items-center justify-center">
                    <IconCheck className="h-4 w-4 text-blue-400" />
                    </div>
                </div>
                ))}
            </div>

            <div className="mt-6 pt-4 border-t border-[#2a2a2a] flex items-center justify-between text-xs text-gray-400">
                <div className="flex items-center gap-2">
                <IconGavel className="h-4 w-4 text-blue-400" />
                <span>Нарушение правил влечет за собой наказание</span>
                </div>
                <div className="flex items-center gap-2">
                <span className="bg-[#1f1f1f] px-3 py-1 rounded-full border border-[#2a2a2a] text-gray-400">
                    Последнее обновление: 27.08.2026
                </span>
                </div>
            </div>
            </div>
        </div>
        </div>
    </>
  );
};