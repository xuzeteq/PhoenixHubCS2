import { BarElement, CategoryScale, Chart, Legend, LinearScale, Title, Tooltip } from "chart.js"
import { Bar } from "react-chartjs-2"

Chart.register(
    Title,
    Legend,
    Tooltip,
    CategoryScale,
    LinearScale,
    BarElement,
)

export default function UsersBarChart() {

    const data = {
        labels: ['Январь', 'Февраль', 'Март', 'Апрель', 'Май', 'Июнь', 'Июль'],
        datasets: [
            {
                label: 'Продажи',
                data: [12, 19, 8, 3, 2],
                backgroundColor: 'rgba(54, 162, 235, 0.6)',
                borderColor: 'rgba(54, 162, 225, 0.6)',
                borderWidth: 1
            },
            {
                label: 'Покупки',
                data: [3, 5, 12, 43, 234],
                backgroundColor: 'rgba(129, 12, 125, 0.6)',
                borderColor: 'rgba(54, 162, 235, 0.6)',
                borderWidth: 1
            }
        ],
    }

    const options = {
        responsive: true,
        plugins: {
            legend: {
                position: 'top' as const,
                display: false
            },
        },
        scales: {
            y: {
                beginAtZero: true,
                display: false,
                grid: { display: false }
            },
            x: {
                grid: { display: false },
                display: false
            }
        }
    }

    return (
        <>
            <Bar data={data} options={options}/>
        </>
    )
}