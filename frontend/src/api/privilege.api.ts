import type { Privilege } from "../types/Privilege";
import { api } from "./api";

export const privilegeApi = {
    getAllPrivileges: async (): Promise<Privilege[]> =>
        api.get('/Privilege').then(res => res.data),

    purchasePrivilege: async(privilegeId: number) =>
        api.post('/Privilege/purchase', { privilegeId }).then(res => res.data)
}