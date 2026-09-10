import type { LogsQuery, LogsResponse } from "../types/Logs";
import { api } from "./api";

export const logsApi = {
    getLogs: async (query: LogsQuery = {}): Promise<LogsResponse> => {
        const res = await api.get('/Admin/logs', { params: query });
        return res.data;
    }
}