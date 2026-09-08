import type { Server } from "../types/Server";
import { api } from "./api";

export const serverApi = {
    getAllServers: async (): Promise<Server[]> =>
        api.get('/Server').then(res => res.data),
}