// src/api/audit.api.ts
import type { AuditQuery, AuditResponse } from '../types/Audit';
import { api } from './api';

export const auditApi = {
    getLogs: async (query: AuditQuery = {}): Promise<AuditResponse> => {
        const res = await api.get('Admin/audit', { params: query });
        return res.data;
    },

    getUserLogs: async (userId: number, query: AuditQuery = {}): Promise<AuditResponse> => {
        const res = await api.get(`Admin/audit/user/${userId}`, { params: query });
        return res.data;
    }
};