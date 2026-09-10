export interface Log {
    message: string;
    level: string;
    timestamp: Date;
    exception?: string;
}

export interface LogsQuery {
    page?: number;
    pageSize?: number;
    level?: string;
    search?: string;
    from?: string;
    to?: string;
}

export interface LogsResponse {
    logs: Log[];
    totalLogs: number;
    page: number;
    pageSize: number;
    totalPages: number;
}