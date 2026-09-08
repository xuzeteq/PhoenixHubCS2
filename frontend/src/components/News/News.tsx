import { Link } from "react-router-dom";

export default function News() {
  return (
    <div className="relative overflow-hidden rounded-xl">
      <div className="absolute inset-0 bg-linear-to-br from-blue-500  to-cyan-500" />
      
      <div className="absolute top-0 left-0 w-96 h-96 bg-white/10 rounded-full blur-3xl -ml-48 -mt-48" />
      <div className="absolute bottom-0 right-0 w-64 h-64 bg-purple-300/20 rounded-full blur-2xl -mr-32 -mb-32" />
      <div className="absolute top-1/2 left-1/2 w-32 h-32 bg-cyan-300/20 rounded-full blur-xl -translate-x-1/2 -translate-y-1/2" />
      
      <div 
        className="absolute inset-0 opacity-20"
        style={{
          backgroundImage: `radial-gradient(circle at 1px 1px, white 1px, transparent 0)`,
          backgroundSize: '40px 40px'
        }}
      />
      
      <div className="relative z-10 p-8 text-white">
        <div>
            <div>
                <h2 className="text-3xl font-bold mb-2">Подписка Phoenix</h2>
                <p className="text-blue-100">Получи множество бонусов играя на наших серверах!</p>
                <div className="mt-2 gap-2 flex items-center">
                    <Link to={'/subscribtion'}>
                        <button className="text-white px-32 py-4 font-bold cursor-pointer rounded bg-blue-500 hover:bg-blue-500/70 transition">
                            Приобрести
                        </button>
                    </Link>
                </div>
            </div>
        </div>
      </div>
    </div>
  );
}