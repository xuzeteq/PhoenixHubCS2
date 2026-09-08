import { useState, useEffect, useMemo } from 'react';
import { Line } from 'react-chartjs-2';
import {
  Chart as ChartJS,
  CategoryScale,
  LinearScale,
  PointElement,
  LineElement,
  Title,
  Tooltip,
  Filler,
} from 'chart.js';

ChartJS.register(
  CategoryScale,
  LinearScale,
  PointElement,
  LineElement,
  Title,
  Tooltip,
  Filler
);

interface DataPoint {
  time: string;
  online: number;
}

interface OnlineChartProps {
  servers: { online: number }[];
}

export default function OnlineChart({ servers }: OnlineChartProps) {
  const [history, setHistory] = useState<DataPoint[]>([]);

  const totalOnline = useMemo(() => {
    return servers.reduce((sum, server) => sum + server.online, 0);
  }, [servers]);

  useEffect(() => {
    const addPoint = () => {
      const now = new Date();
      const time = now.toLocaleTimeString('ru-RU', {
        hour: '2-digit',
        minute: '2-digit',
      });

      setHistory((prev) => {
        const next = [...prev, { time, online: totalOnline }];
        return next.length > 60 ? next.slice(-60) : next;
      });
    };

    addPoint();

    const interval = setInterval(addPoint, 60_000);

    return () => clearInterval(interval);
  }, [totalOnline]);

  const data = {
    labels: history.map((p) => p.time),
    datasets: [
      {
        label: 'Онлайн',
        data: history.map((p) => p.online),
        borderColor: 'rgb(81, 162, 255)',       // green-500
        backgroundColor: 'rgba(81, 162, 255, 0.1)', // зелёная заливка под линией
        fill: true,       // закрашивает область под линией
        tension: 0,     // сглаживание линии (0 = углы, 1 = максимум)
        pointRadius: 0,   // убираем точки на линии (чище выглядит)
        borderWidth: 2,
      },
    ],
  };

  const options = {
    responsive: true,
    maintainAspectRatio: false, // чтобы высота задавалась через Tailwind
    plugins: {
      legend: { display: false },
      title: {
        display: true,
        text: `Онлайн сейчас`,
        color: '#e5e7eb',
        font: { size: 24 },
      },
      tooltip: {
        callbacks: {
          // eslint-disable-next-line @typescript-eslint/no-explicit-any
          label: (ctx: any) => `${ctx.parsed.y} игроков`,
        },
      },
    },
    scales: {
      x: {
        grid: { display: false },
        ticks: {
          color: '#9ca3af',
          maxTicksLimit: 12, // не лепить все 60 меток, показывать ~8
        },
      },
      y: {
        beginAtZero: true,
        grid: { color: 'rgba(255, 255, 255, 0.05)' }, // еле заметная сетка
        ticks: { color: '#9ca3af' },
      },
    },
  };

  return (
    <div className="w-full h-64 bg-[#1c1c1c] rounded-xl p-4">
      <Line data={data} options={options} />
    </div>
  );
}