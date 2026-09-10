export interface AuditLog {
    id: number;
    timestamp: string;
    userId?: number;
    username?: string;
    action: string;
    entityType: string;
    entityName?: string;
    entityId?: string;
    oldValue?: string;
    newValue?: string;
    amount?: number;
    currency?: string;
    durationDays?: number;
    validUntil?: string;
    statusCode: number;
    isSuccess: boolean;
    errorMessage?: string;
    metadata?: string;
}

export interface AuditQuery {
    page?: number;
    pageSize?: number;
    userId?: number;
    action?: string;
    entityType?: string;
    isSuccess?: boolean;
    from?: string;
    to?: string;
    search?: string;
}

export interface AuditResponse {
    logs: AuditLog[];
    totalLogs: number;
    page: number;
    pageSize: number;
    totalPages: number;
}