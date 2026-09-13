import { IconWheel } from "@tabler/icons-react";
import { useState } from "react";
import ModalWheel from "../Modals/ModalWheel";

export default function WheelSpin() {
    const [modal, setModal] = useState(false);

    return (
        <>
            <button
                onClick={() => setModal(true)}
                className="text-white/70 hover:text-white p-2 bg-blue-500/30 border hover:bg-blue-500/50 border-blue-500/40 rounded-full transition cursor-pointer"
            >
                <IconWheel />
            </button>

            {modal && <ModalWheel onClose={() => setModal(false)} />}
        </>
    );
}