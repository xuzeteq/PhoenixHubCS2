export interface Server {
    id: number,
    title: string,
    map: string,
    ipAddress: string,
    port: number,
    online: number,
    maxOnline: number,
    isActive: boolean,
    createdAt: Date,
    updatedAt: Date
}