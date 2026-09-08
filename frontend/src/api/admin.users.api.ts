import type { User } from "../types/User";
import { api } from "./api";

export const adminUserApi = {
    getAllUsers: async (): Promise<User[]> =>
        api.get('/Admin/users').then(res => res.data),
}